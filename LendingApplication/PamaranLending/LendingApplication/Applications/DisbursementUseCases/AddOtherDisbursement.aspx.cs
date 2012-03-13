using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;
using System.IO;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class AddOtherDisbursement : ActivityPageBase
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
                txtAmountDisbursed.SetValue(0);
                txtCashAmount.SetValue(0);
                txtCheckAmount.SetValue(0);
                txtAmount.SetValue(0);
                OtherDisbursementForm form = this.CreateOrRetrieve<OtherDisbursementForm>();
                var today = DateTime.Now;
                hiddenResourceGUID.Value = this.ResourceGuid;
                txtTransactionDate.SelectedDate = today;
                txtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
                var disbursementParticulars = OtherDisbursementParticular.All();
                storeParticulars.DataSource = disbursementParticulars;
                storeParticulars.DataBind();
            }
        }

        [DirectMethod(ShowMask=true, Msg="Adding particular..")]
        public string saveOtherDisbursementParticular()
        {

            string name = txtParticularA.Text;
            if (!string.IsNullOrEmpty(name))
            {
                OtherDisbursementParticular.CreateParticular(name);
                ObjectContext.SaveChanges();
            }

            txtParticularA.AllowBlank = true;
            cmbParticulars.AllowBlank = false;

            return name;
        }

        [DirectMethod]
        public void LoadDropDown(string particular)
        {

            var disbursementParticulars = OtherDisbursementParticular.All();

            storeParticulars.DataSource = disbursementParticulars;
            storeParticulars.DataBind();

            var particularT = OtherDisbursementParticular.GetByName(particular);

            cmbParticulars.SelectedItem.Value = particularT.Id.ToString();
        }

        public void btnSave_Click(object sender, DirectEventArgs e)
        {
            OtherDisbursementForm form = this.CreateOrRetrieve<OtherDisbursementForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {

                form.TransactionDate = txtTransactionDate.SelectedDate;
                form.CustomerID = int.Parse(txtCustId.Text);
                form.DisbursedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.CashAmountToDisburse = decimal.Parse(txtCashAmount.Text);
                form.CheckAmountToDisburse = decimal.Parse(txtCheckAmount.Text);
                form.AmountToDisburse = decimal.Parse(txtTotalAmountToDisburse.Text);
                form.now = DateTime.Now;
                form.SignatureString = hdnSignatureImage1.Value.ToString();

                form.PrepareForSave();
            }
                 //OtherDisbursementForm form2 = CreateOrRetrieve<OtherDisbursementForm>();

               // var amountTodisburse = decimal.Parse(txtTotalAmountToDisburse.Text);
            hdnDisbursmentId.Value = form.DisId;
        }

        private class OtherDisbursementModel
        {
            public decimal Amount { get; set; }
            public string Particular { get; set; }
        }

        private void ShowOtherDisbursementItems(FinancialEntities context, int customerId)
        {
            var query = from p in context.DisbursementItems
                        select new OtherDisbursementModel()
                        {
                            Particular = p.Particular,
                            Amount = p.PerItemAmount
                        };

            PageGridPanelStore.DataSource = query.ToList();
            PageGridPanelStore.DataBind();
        }


        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            OtherDisbursementForm form = CreateOrRetrieve<OtherDisbursementForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            OtherDisbursementForm form = this.CreateOrRetrieve<OtherDisbursementForm>();
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
            OtherDisbursementForm form = CreateOrRetrieve<OtherDisbursementForm>();
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
        public void btnSaveOtherItems_Click(object sender, DirectEventArgs e)
        {
            string particular = cmbParticulars.SelectedItem.Text;
            decimal amount = Decimal.Parse(txtAmount.Text);
            OtherDisbursementForm form = this.CreateOrRetrieve<OtherDisbursementForm>();
            if (string.IsNullOrWhiteSpace(hiddenRandomKey.Text))
            {
                form.AddOtherItems(new OtherItemsModel(particular, amount));
            }
            else
            {
                OtherItemsModel model = form.GetOtherItems(hiddenRandomKey.Text);
                model.Particular = particular;
                model.Amount = amount;
                model.MarkEdited();
            }

            PageGridPanelStore.DataSource = form.AvailableOtherItems;
            PageGridPanelStore.DataBind();
        }

        public void otherItemsDelete_Click(object sender, DirectEventArgs e)
        {
            OtherDisbursementForm form = this.CreateOrRetrieve<OtherDisbursementForm>();
            SelectedRowCollection rows = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveOtherItems(row.RecordID);
            }
            PageGridPanelStore.DataSource = form.AvailableOtherItems;
            PageGridPanelStore.DataBind();
        }
        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        [DirectMethod]
        public bool CheckIfValid()
        {
            bool result = true;
            string particular = txtParticularA.Text;

            var particulars = OtherDisbursementParticular.All();

            var count = particulars.Where(entity => entity.Name == particular);

            result = result && (count.Count() == 0);

            return result;
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