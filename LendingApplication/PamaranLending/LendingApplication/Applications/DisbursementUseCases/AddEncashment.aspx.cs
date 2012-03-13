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
    public partial class AddEncashment : ActivityPageBase
    {

        private string uploadDirectory;
        protected string imageFilename;

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
                EncashmentForm form = this.CreateOrRetrieve<EncashmentForm>();
                this.hiddenResourceGUID.Value = this.ResourceGuid;
                var today = DateTime.Now;
                txtTransactionDate.MaxDate = today;
                txtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                txtTransactionDate.SelectedDate = today;
                txtCashAmount.SetValue(0);
                txtCheckAmount.SetValue(0);
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
                cmbPaymentMethod.SelectedIndex = 1;

            }
        }

        [DirectMethod]
        public void AddChequesManually(string bankName, string bankBranch, string checkNumber, string checkType, DateTime checkDate, decimal checkAmount, int BankId)
        {
            EncashmentForm form = CreateOrRetrieve<EncashmentForm>();
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

        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            EncashmentForm form = CreateOrRetrieve<EncashmentForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var chequePaymentMethod = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                EncashmentForm form = this.CreateOrRetrieve<EncashmentForm>();
                form.BankId = int.Parse(txtBankId.Text);
                form.BankName = txtBank.Text;
                form.DisbursedToName = txtDisbursedToName.Text;
                form.PaymentMethodTypeId = chequePaymentMethod.Id;
                form.ProcessedToCustomerId = int.Parse(txtProcessedToCustomerId.Text);
                form.CheckNumber = txtChkNumber.Text;
                form.CheckDate = txtChkDate.SelectedDate;
                form.CheckAmount = decimal.Parse(txtChkAmount.Text);
                form.CashAmountToDisburse = decimal.Parse(txtCashAmount.Text);
                form.CheckAmountToDisburse = decimal.Parse(txtCheckAmount.Text);
                form.TransactionDate = txtTransactionDate.SelectedDate;
                form.AmountToDisburse = decimal.Parse(txtAmountToDisburse.Text);
                form.EmployeePartyRoleID = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.SignatureString = hdnSignatureImage1.Value.ToString();

                form.PrepareForSave();
            }
 
            var payment =  from  p in ObjectContext.Payments
                           join d in ObjectContext.Disbursements on p.Id equals d.PaymentId
                           where p.PaymentReferenceNumber == txtChkNumber.Text
                           && p.PaymentTypeId == PaymentType.Disbursement.Id
                               && p.SpecificPaymentTypeId == SpecificPaymentType.EncashmentType.Id
                               select p;
            var disbursepayment = payment.FirstOrDefault();
            hdnDisbursementId.Value = disbursepayment.Id;
        }

        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            EncashmentForm form = this.CreateOrRetrieve<EncashmentForm>();
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
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        protected void onUploadImage1_Click(object sender, DirectEventArgs e)
        {
            string msg = "";

            // Check that a file is actually being submitted.
            if (flupSpecimenSignature1.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
            }
            else //else if file exists
            {
                // Check the extension.
                string extension = Path.GetExtension(flupSpecimenSignature1.PostedFile.FileName);
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
                string serverFileName = Path.GetFileName(flupSpecimenSignature1.PostedFile.FileName);
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
                    flupSpecimenSignature1.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Images/" + flupSpecimenSignature1.PostedFile.FileName;
                imgSpecimenSignature1.ImageUrl = imageFilename;
                this.hdnSignatureImage1.Value = imageFilename;
            }
        }
     
    }
}