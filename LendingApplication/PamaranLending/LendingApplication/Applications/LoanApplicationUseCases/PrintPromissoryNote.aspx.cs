using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;
using BusinessLogic.FullfillmentForms;
using System.IO;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class PrintPromisoryNote : ActivityPageBase
    {
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

        private string uploadDirectory;
        protected string imageFilename;

        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Agreement");

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"];
                hdnMode.Value = mode;

                int loanApplicationId = int.Parse(Request.QueryString["loanApplicationId"]);
                var loanApplication = LoanApplication.GetById(loanApplicationId);
                hdnLoanApplicationId.Value = loanApplicationId;

                FillHeaderInformation();
                FillNoteParts(loanApplication);
                FillControlNumber(loanApplication);
                EnableDisableControls(mode, loanApplication);
            }
            uploadDirectory = CreateFolders(uploadDirectory, hdnLoanApplicationId.Value.ToString());
        }

        private void EnableDisableControls(string mode, LoanApplication loanApplication)
        {
            switch (mode)
            {
                case "approve":
                    btnPrint.Disabled = true;
                    break;
                case "modify":
                    if (loanApplication.CurrentStatus.LoanApplicationStatusType.Name !=
                    LoanApplicationStatusType.PendingApprovalType.Name)
                    {
                        btnPrint.Disabled = false;
                        btnSaveSignatures.Hidden = true;
                    }
                    btnSaveSignatures.Disabled = true;
                    flUpBorrower.Hide();

                    var formDetails = FormDetail.GetByLoanAppIdAndType(loanApplication.ApplicationId, FormType.PromissoryNoteType);
                    foreach (var item in formDetails)
                    {
                        imgBorrower.ImageUrl = item.Signature;
                    }
                    break;
                default:
                    break;
            }
        }

        private string CreateFolders(string path, string loanAccountId)
        {
            string newPath = Path.Combine(path, loanAccountId, FormType.PromissoryNoteType.Name);
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            return newPath;
        }

        [DirectMethod]
        public void SaveSignatures(int loanAppId)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.Retrieve(loanAppId);

            var loanApp = LoanApplication.GetById(loanAppId);
            CustomerDetailsModel model = new CustomerDetailsModel(form.PartyRoleId);

            var formDetail = FormDetail.Create(FormType.PromissoryNoteType.Id, loanApp, "Borrower", model.Name.ToUpper(), hdnBorrower.Text);

            ObjectContext.FormDetails.AddObject(formDetail);
            ObjectContext.SaveChanges();
        }

        private void FillHeaderInformation()
        {
            //FILL HEADER
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblHeader.Text = organization.OrganizationName;
        }

        private void FillControlNumber(LoanApplication loanApplication)
        {
            //var controlNumber = ObjectContext.ControlNumbers.SingleOrDefault(entity =>
            //    entity.FormType == FormType.PromissoryNoteType);
            var controlNumber = ControlNumberFacade.GetByLoanAppId(loanApplication.ApplicationId);
            this.lblControlNum.Text = string.Format("{0:00000}", controlNumber.LastControlNumber);
        }

        private void FillNoteParts(LoanApplication loanApplication)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.Retrieve(loanApplication.ApplicationId);

            this.lblLoanAmountWords.Text = ConvertNumbers.EnglishFromNumber((double)form.LoanAmount) + " Pesos Only";
            
            this.lblLoanAmount.Text = "(P " + String.Format("{0:#,##0.00;(#,##0.00);Zero}", form.LoanAmount) + " )";
            
            var agreement = ObjectContext.Agreements.SingleOrDefault(entity =>
                entity.ApplicationId == form.LoanApplicationId);

            var schedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity =>
                entity.AgreementId == agreement.Id);

            this.lblmonthTerm.Text = String.Format("{0:MMMM}", schedule.PaymentStartDate);
            
            this.lblRate.Text = form.InterestRate.ToString() + " % ";
            
            CustomerDetailsModel model = new CustomerDetailsModel(form.PartyRoleId);

            foreach (var item in form.AvailableCoBorrowers)
            {
                this.lblCoborrower.Text = StringConcatUtility.Build(", ", lblCoborrower.Text, item.Name);
            }

            foreach (var item in form.AvailableGuarantors)
            {
                this.lblGuarantor.Text = StringConcatUtility.Build(", ", lblGuarantor.Text, item.Name);
            }

            this.lblName.Text = model.Name.ToUpper();

            this.lblDistrict.Text = model.District;

            this.lblAddress.Text = model.PrimaryHomeAddress;

            string cellphoneNumber = "";
            string telephoneNumber = "";

            if (!string.IsNullOrWhiteSpace(model.CellphoneNumber))
                cellphoneNumber = model.CountryCode + model.CellphoneAreaCode + model.CellphoneNumber;

            if (!string.IsNullOrWhiteSpace(model.TelephoneNumber))
                telephoneNumber = "/" + model.TelephoneNumberAreaCode + "-" + model.TelephoneNumber;

            this.lblContact.Text = cellphoneNumber + telephoneNumber;

            this.lblDate.Text = String.Format("{0:MM/dd/yy}", DateTime.Now);
        }

        protected void onUploadBorrower(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpBorrower, imgBorrower, hdnBorrower, "Borrower");
        }

        public void onUploadBorrower(FileUploadField fileUpload, Ext.Net.Image image, Hidden hidden, string name)
        {
            string msg = "";

            // Check that a file is actually being submitted.
            if (fileUpload.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
                btnSaveSignatures.Disabled = true;
                btnSaveSignatures.Disabled = true;
            }
            else //else if file exists
            {
                // Check the extension.
                string extension = Path.GetExtension(fileUpload.PostedFile.FileName);
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
                string serverFileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                string fullUploadPath = Path.Combine(uploadDirectory, name + serverFileName);
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
                        file = Path.Combine(uploadDirectory, name + file);
                    }
                    else
                    {
                        file = fullUploadPath;
                        fileName = serverFileName;
                    }

                    //save file
                    fileUpload.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                    btnSaveSignatures.Disabled = false;
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Agreement/" + hdnLoanApplicationId.Value.ToString() + "/" + FormType.PromissoryNoteType.Name + "/" +
                                name + fileUpload.PostedFile.FileName;
                image.ImageUrl = imageFilename;
                hidden.Value = imageFilename;
            }
        }
    } 
}