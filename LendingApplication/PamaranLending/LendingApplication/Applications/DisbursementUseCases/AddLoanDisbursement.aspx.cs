using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;
using System.IO;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class AddLoanDisbursement : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
                return allowed;
            }
        }
        private string uploadDirectory;
        protected string imageFilename;
        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Images");
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
             
                LoanDisbursementForm form = this.CreateOrRetrieve<LoanDisbursementForm>();
                var today = DateTime.Today;
                txtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                txtTransactionDate.SelectedDate = today;
                hiddenResourceGUID.Value = this.ResourceGuid;
                txtCashAmount.SetValue(0);
                txtCheckAmount.SetValue(0);
                txtDeductions.SetValue(0);
                txtAmountDisbursed.SetValue(0);
                txtNetAmountReceived.SetValue(0);
                Employee user = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
                var id = PartyRole.GetByPartyIdAndRole(user.PartyRole.PartyId, RoleType.BorrowerAgreementType);
                if (id != null) txtUserID.Value = id.Id; else txtUserID.Value = -1;
                strLoanDisType.DataSource = ObjectContext.LoanDisbursementTypes.Where(entity => entity.Name != LoanDisbursementType.FirstAvailment.Name);
                strLoanDisType.DataBind();
                cmbLoanDisType.SelectedIndex = 0;
             
            }

        }

        protected void onUpload_Click(object sender, DirectEventArgs e)
        {
            string msg = "";

            // Check that a file is actually being submitted.
            if (fileUpDisburseTo.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
            }
            else //else if file exists
            {
                // Check the extension.
                string extension = Path.GetExtension(fileUpDisburseTo.PostedFile.FileName);
                switch (extension.ToLower())
                {
                    case ".bmp":
                    case ".gif":
                    case ".jpg":
                    case ".png":
                    case ".tiff":
                    case ".jpeg":
                    case ".tif":
                        break;
                    default:
                        X.MessageBox.Alert("Alert", "This file type is not allowed.").Show();
                        return;
                }

                // Using this code, the saved file will retain its original
                // file name when it's placed on the server.
                string serverFileName = Path.GetFileName(fileUpDisburseTo.PostedFile.FileName);
                string fullUploadPath = Path.Combine(uploadDirectory, serverFileName);
                string file = "";
                string fileName = "";
                try
                {
                    //check if file already exists
                    if (File.Exists(fullUploadPath))
                    {
                        file = Path.GetFileNameWithoutExtension(serverFileName);
                        file += DateTime.Now.ToString("M-dd-yyyy hhmmss.ff") + Path.GetExtension(serverFileName);
                        fileName = file;
                        file = Path.Combine(uploadDirectory, file);
                    }
                    else
                    {
                        file = fullUploadPath;
                        fileName = serverFileName;
                    }

                    //save file
                    fileUpDisburseTo.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Images/" + fileUpDisburseTo.PostedFile.FileName;
                PersonImageFile.ImageUrl = imageFilename;
                this.hiddenImageUrl.Value = imageFilename;
            }
        }
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }
        [DirectMethod]
        public int CheckIfAdditional(int voucherId)
        {
            return IsAdditional(voucherId);   
        }
        public int IsAdditional(int voucherId)
        {
            int flag = 1;
            var disbursements = ObjectContext.PaymentApplications.Where(entity => entity.LoanDisbursementVoucherId == voucherId);
            var agreementId = LoanDisbursementVcr.GetById(voucherId).AgreementId;
            var financialaccount = ObjectContext.FinancialAccounts.FirstOrDefault(e => e.AgreementId == agreementId);
            if (disbursements.Count() == 0 && financialaccount == null)
            {
                flag = 0;
            }
            else if (disbursements.Count() == 0 && financialaccount != null)
            {
                if (financialaccount.ParentFinancialAccountId == null)
                    flag = 0;
            }
            return flag;
        }
        public void btnSave_Click(object sender, DirectEventArgs e)
        {
            LoanDisbursementForm form = this.CreateOrRetrieve<LoanDisbursementForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                form.LoanDisbursementVcrId = int.Parse(txtLoanDisbursementVoucherId.Text);
                form.CustomerId = int.Parse(txtCustID.Text);
                form.DisbursedToName = txtDisbursedToName.Text;
                form.LoanAgreementId = int.Parse(txtLoanAgreementId.Text);
                form.LoanProduct = txtLoanProduct.Text;
                form.LoanProductId = int.Parse(txtLoanProductID.Text);
                form.LoanAmount = decimal.Parse(txtLoanAmount.Text);
                form.TransactionDate = txtTransactionDate.SelectedDate;
                form.ReceivedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.CashAmountToDisburse = decimal.Parse(txtCashAmount.Text);
                form.CheckAmountToDisburse = decimal.Parse(txtCheckAmount.Text);
                form.TotalAmountToDisburse = decimal.Parse(txtAmountDisbursed.Text);
                form.Deductions = decimal.Parse(txtDeductions.Text);
                form.Signature = hiddenImageUrl.Text;
                if (string.IsNullOrWhiteSpace(hiddenInterest.Text) == false)
                    form.InterestPayment = decimal.Parse(hiddenInterest.Text);
                else form.InterestPayment = 0;

                var flag = IsAdditional(form.LoanDisbursementVcrId);
                if (flag == 0)
                    form.LoanDisbursementType = LoanDisbursementType.FirstAvailment;
                else if (cmbLoanDisType.SelectedItem.Text == LoanDisbursementType.ACCheque.Name && flag == 1)
                    form.LoanDisbursementType = LoanDisbursementType.ACCheque;
                else if (cmbLoanDisType.SelectedItem.Text == LoanDisbursementType.ACATM.Name && flag == 1)
                    form.LoanDisbursementType = LoanDisbursementType.ACATM;
                else if (flag == 1 && cmbLoanDisType.SelectedItem.Text == LoanDisbursementType.Additional.Name)
                    form.LoanDisbursementType = LoanDisbursementType.Additional;
                else if (flag == 1 && cmbLoanDisType.SelectedItem.Text == LoanDisbursementType.Change.Name)
                    form.LoanDisbursementType = LoanDisbursementType.Change;
                form.PrepareForSave();
            }
            hdnDisbursemntId.Value = form.DisbursementId;
           
        }
        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            LoanDisbursementForm form = CreateOrRetrieve<LoanDisbursementForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            LoanDisbursementForm form = this.CreateOrRetrieve<LoanDisbursementForm>();
            SelectedRowCollection rows = this.ChequesSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCheques(row.RecordID);
            }

            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
            btnDeleteCheck.Disabled = true;
        }


        [DirectMethod]
        public void AddChequesManually(string bankName, string bankBranch, string checkNumber, string checkType, DateTime checkDate, decimal checkAmount, int BankId)
        {
            LoanDisbursementForm form = CreateOrRetrieve<LoanDisbursementForm>();
            AddChequesModel model = new AddChequesModel();
            model.BankName = bankName;
            model.Branch = bankBranch;
            model.BankPartyRoleId = BankId;
            model.CheckType = checkType;
            model.CheckNumber = checkNumber;
            model.CheckDate = checkDate;
            model.Amount = checkAmount;

            form.AddCheques(model);
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        [DirectMethod]
        public void SetDate(int voucherId, int agreementId)
        {
            var isAdd = IsAdditional(voucherId);
            DateTime mindate =  ObjectContext.AmortizationSchedules.FirstOrDefault( e => e.AgreementId == agreementId).LoanReleaseDate;

            if (isAdd == 1)
            {
                var disbursements = from pa in ObjectContext.PaymentApplications
                                    join p in ObjectContext.Payments on pa.PaymentId equals p.Id
                                    where pa.LoanDisbursementVoucherId == voucherId
                                    orderby p.TransactionDate
                                    select p;
                if (disbursements.Count() != 0)
                    mindate = disbursements.FirstOrDefault().TransactionDate;
            }
            txtTransactionDate.MinDate = mindate;
        }

        [DirectMethod]
        public decimal AmountDisbursedChanged()
        {
            decimal varDisbursed = decimal.Parse(txtAmountDisbursed.Text);
            decimal varAmount = decimal.Parse(txtLoanAmount.Text);
            var agreementId = int.Parse(txtLoanAgreementId.Text);
            var voucherId = int.Parse(txtLoanDisbursementVoucherId.Text);
            decimal sum = 0;
            decimal interestpayment = 0;

            var agreement = ObjectContext.Agreements.FirstOrDefault(entity => entity.Id == agreementId);
            var restructured = ObjectContext.AmortizationSchedules.FirstOrDefault(entity => entity.AgreementId == agreementId).ParentAmortizationScheduleId;
            var voucher = LoanDisbursementVcr.GetById(voucherId);
            if (agreement != null && restructured == null)
            {
                decimal computedValue = 0;
                decimal loanBalance = 0;
              
                var applicationFees = ObjectContext.ApplicationItems.Where
                    (entity => entity.ApplicationId == agreement.ApplicationId && entity.FeeComputedValue.HasValue && entity.EndDate == null);
                if (applicationFees.Count() > 0)
                    computedValue = applicationFees.Sum(entity => entity.FeeComputedValue.Value);
            
                var rootPayments = from pa in voucher.PaymentApplications
                                   join p in ObjectContext.Payments on pa.PaymentId equals p.Id
                                   where p.Disbursement != null && p.PaymentTypeId == PaymentType.Disbursement.Id
                                   orderby p.EntryDate ascending
                                   select p;
                if (rootPayments.Count() > 0)
                {
                    var rootPayment = rootPayments.First();
                    var ParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == rootPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);
                    if (ParentPayment != null)
                    {
                      
                        var feeParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);
                        if (feeParentPayment != null)
                        {
                            var feePayments = ObjectContext.FeePayments.Where(entity => entity.PaymentId == feeParentPayment.Id);
                            decimal paidFees = feePayments.Sum(entity => entity.FeeAmount);
                            computedValue -= paidFees;
                        }
                    }
                }

                hiddenInterest.Text = interestpayment.ToString();
                sum = loanBalance + computedValue + interestpayment;
            }
            txtDeductions.Text = sum.ToString();
            hiddenInterest.Text = interestpayment.ToString();
            return sum;
        }
        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

    }
}