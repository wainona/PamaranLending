using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using System.IO;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class AddRediscounting : ActivityPageBase
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
        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Images");

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                RediscountingForm form = this.CreateOrRetrieve<RediscountingForm>();
                hiddenResourceGUID.Value = this.ResourceGuid;

                var today = DateTime.Now;
                txtTransactionDate.MaxDate = today;
                txtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                txtTransactionDate.SelectedDate = today;
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
                cmbPaymentMethod.SelectedIndex = 1;
                txtCashAmount.SetValue(0);
                txtCheckAmount.SetValue(0);
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

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var chequePaymentMethod = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                RediscountingForm form = this.CreateOrRetrieve<RediscountingForm>();
                form.BankId = int.Parse(txtBankId.Text);
                form.BankName = txtBank.Text;
                form.DisbursedToName = txtReceivedBy.Text;
                form.PaymentMethodTypeId = chequePaymentMethod.Id;
                form.ProcessedToCustomerId = int.Parse(txtProcessedToCustomerId.Text);
                form.CheckNumber = txtChkNumber.Text;
                form.CheckDate = txtChkDate.SelectedDate;
                form.CheckAmount = decimal.Parse(txtChkAmount.Text);
                form.CashAmountToDisburse = decimal.Parse(txtCashAmount.Text);
                form.CheckAmountToDisburse = decimal.Parse(txtCheckAmount.Text);
                form.TransactionDate = txtTransactionDate.SelectedDate;
                form.SurchargeRate = int.Parse(txtSurchargeRate.Text);
                form.SurchargeFee = decimal.Parse(txtSurchargeFee.Text);
                form.AmountToDisburse = decimal.Parse(txtAmountToDisburse.Text);
                form.EmployeePartyRoleID = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.Signature = hiddenImageUrl.Text;
                form.PrepareForSave();
            }
            var payment = from p in ObjectContext.Payments
                          join d in ObjectContext.Disbursements on p.Id equals d.PaymentId
                          where p.PaymentReferenceNumber == txtChkNumber.Text
                          && p.PaymentTypeId == PaymentType.Disbursement.Id
                              && p.SpecificPaymentTypeId == SpecificPaymentType.RediscountingType.Id
                          select p;
            var disbursepayment = payment.FirstOrDefault();
            hdnDisbursementId.Value = disbursepayment.Id;
        }
        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            RediscountingForm form = CreateOrRetrieve<RediscountingForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }

        [DirectMethod]
        public void CheckAmountBlur()
        {
            FillSurchargeFee();
        }
        protected void FillSurchargeFee()
        {
            if ((string.IsNullOrWhiteSpace(txtChkAmount.Text) == false) && (string.IsNullOrWhiteSpace(txtSurchargeRate.Text) == false
                && !txtTransactionDate.SelectedDate.Equals(DateTime.MinValue) && !txtChkDate.SelectedDate.Equals(DateTime.MaxValue)))
            {
               
                int dayDiff = 0;
                decimal surchargeFee = 0;
                
                decimal checkAmount = decimal.Parse(txtChkAmount.Text);
                decimal surchargeRate = decimal.Parse(txtSurchargeRate.Text);
                decimal surchargeRatePercent = surchargeRate / 100;

             
               if (txtChkDate.SelectedDate.Day != txtTransactionDate.SelectedDate.Day)
               {
                   dayDiff = txtChkDate.SelectedDate.Subtract(txtTransactionDate.SelectedDate).Days + 1;
                   if(dayDiff <= 7)
                   dayDiff = 7;
                    surchargeFee = ((checkAmount * surchargeRatePercent) / 30) * (dayDiff);
               }
               else
               {
                   dayDiff = (txtChkDate.SelectedDate.Year * 12 + txtChkDate.SelectedDate.Month) - (txtTransactionDate.SelectedDate.Year * 12 + txtTransactionDate.SelectedDate.Month);
                   surchargeFee = ((checkAmount * surchargeRatePercent)) * dayDiff;
               }
                decimal amountToDisburse = checkAmount - surchargeFee;
                txtSurchargeFee.Text = surchargeFee.ToString("N");
                txtAmountToDisburse.Text = amountToDisburse.ToString("N");
            }
        }
        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            RediscountingForm form = this.CreateOrRetrieve<RediscountingForm>();
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
            RediscountingForm form = CreateOrRetrieve<RediscountingForm>();
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
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }
    }
}