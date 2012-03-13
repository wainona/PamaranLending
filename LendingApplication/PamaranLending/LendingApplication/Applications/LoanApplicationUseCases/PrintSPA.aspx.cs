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
using BusinessLogic.Facade;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class PrintSPA : ActivityPageBase
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

        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

        private string uploadDirectory;
        protected string imageFilename;

        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Agreement");

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int customerId = int.Parse(Request.QueryString["customerPartyRoleId"]);
                int loanApplicationId = int.Parse(Request.QueryString["loanApplicationId"]);
                string mode = Request.QueryString["mode"];
                var loanApplication = LoanApplication.GetById(loanApplicationId);
                hdnLoanApplicationId.Value = loanApplicationId;
                hdnCustomerId.Value = customerId;
                hdnMode.Value = mode;
                if (mode == "approve")
                {
                    this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                    string status = Request.QueryString["status"];
                    hdnIsSigned.Value = status;
                    LoanApprovalForm form = this.Retrieve<LoanApprovalForm>(ParentResourceGuid);
                    this.hdnResourceGuid.Value = this.ParentResourceGuid;
                    FillSignaturesForApproval();
                }

                var customerPartyRole = PartyRole.GetById(int.Parse(hdnCustomerId.Value.ToString()));
                var borrowerHasLoans = LoanApprovalForm.BorrowerLoans(customerPartyRole.Party);
                if (borrowerHasLoans.Count() > 0)
                {
                    loanApplication = borrowerHasLoans.FirstOrDefault();
                }


                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderInformation(party);
                FillDetails(loanApplication);
                FillCustomerDetails(customerId, loanApplication);
                EnableDisableControls(mode, loanApplication);
                FillSignatures(loanApplication);
            }
            uploadDirectory = CreateFolders(uploadDirectory, hdnLoanApplicationId.Value.ToString());
        }

        private string CreateFolders(string path, string loanAccountId)
        {
            string newPath = Path.Combine(path, loanAccountId, FormType.SPAType.Name);
            return newPath = CreateDirectory(newPath);
        }

        private string CreateDirectory(string path){
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        private void EnableDisableControls(string mode, LoanApplication loanApplication)
        {
            if (mode == "approve")
            {
                txtWitness.Show();
                txtWitness2.Show();
                txtWitness3.Show();
                txtWitness4.Show();
                txtLender.Show();

                imgWitness.Show();
                imgWitness2.Show();
                imgWitness3.Show();
                imgWitness4.Show();
                imgLender.Show();
                imgBorrower.Show();

                flUpWitness.Show();
                flUpWitness2.Show();
                flUpWitness3.Show();
                flUpWitness4.Show();
                flUpLender.Show();
                flUpBorrower.Show();

                btnPrint.Disabled = true;
                
            }
            else if (mode == "modify")
            {
                txtWitness.Hide();
                txtWitness2.Hide();
                txtWitness3.Hide();
                txtWitness4.Hide();
                txtLender.Hide();

                imgWitness.Show();
                imgWitness2.Show();
                imgWitness3.Show();
                imgWitness4.Show();
                imgLender.Show();
                imgBorrower.Show();

                flUpWitness.Hide();
                flUpWitness2.Hide();
                flUpWitness3.Hide();
                flUpWitness4.Hide();
                flUpLender.Hide();
                flUpBorrower.Hide();

                btnSaveSignatures.Disabled = true;
            }
        }

        protected void onUploadLender(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpLender, imgLender, hdnLender, "Lender");
        }

        protected void onUploadBorrower(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpBorrower, imgBorrower, hdnBorrower, "Borrower");
        }

        protected void onUploadWitness1(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpWitness, imgWitness, hdnWitness1, "Witness1");
        }

        protected void onUploadWitness2(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpWitness2, imgWitness2, hdnWitness2, "Witness2");
        }

        protected void onUploadWitness3(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpWitness3, imgWitness3, hdnWitness3, "Witness3");
        }

        protected void onUploadWitness4(object sender, DirectEventArgs e)
        {
            onUploadBorrower(flUpWitness4, imgWitness4, hdnWitness4, "Witness4");
        }

        [DirectMethod]
        public void SaveSignatures()
        {
            LoanApprovalForm form = this.Retrieve<LoanApprovalForm>(ParentResourceGuid);
            form.SPADocumentDetails.Borrower.FilePath = hdnBorrower.Value.ToString();
            form.SPADocumentDetails.Borrower.RoleName = "Borrower";

            form.SPADocumentDetails.Lender.FilePath = hdnLender.Value.ToString();
            form.SPADocumentDetails.Lender.PersonName = txtLender.Text;
            form.SPADocumentDetails.Lender.RoleName = "Lender";

            form.SPADocumentDetails.Witness1.FilePath = hdnWitness1.Value.ToString();
            form.SPADocumentDetails.Witness1.PersonName = txtWitness.Text;
            form.SPADocumentDetails.Lender.RoleName = "Witness1";

            form.SPADocumentDetails.Witness2.FilePath = hdnWitness2.Value.ToString();
            form.SPADocumentDetails.Witness2.PersonName = txtWitness2.Text;
            form.SPADocumentDetails.Lender.RoleName = "Witness2";

            form.SPADocumentDetails.Witness3.FilePath = hdnWitness3.Value.ToString();
            form.SPADocumentDetails.Witness3.PersonName = txtWitness3.Text;
            form.SPADocumentDetails.Lender.RoleName = "Witness3";

            form.SPADocumentDetails.Witness4.FilePath = hdnWitness4.Text;
            form.SPADocumentDetails.Witness4.PersonName = txtWitness4.Text;
            form.SPADocumentDetails.Lender.RoleName = "Witness4";
        }

        public void DeleteImages(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void onUploadBorrower(FileUploadField fileUpload, Ext.Net.Image image, Hidden hidden, string name)
        {
            string recentDirectory = CreateDirectory(Path.Combine(uploadDirectory, name));
            if (Directory.Exists(recentDirectory))
            {
                string[] files = Directory.GetFiles(recentDirectory);
                foreach (var f in files)
                {
                    File.Delete(f);
                }
            }
            uploadDirectory = recentDirectory;
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

                //X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Agreement/" + hdnLoanApplicationId.Value.ToString() + "/" + FormType.SPAType.Name + "/" +
                                name + "/" + fileUpload.PostedFile.FileName;
                image.ImageUrl = imageFilename;
                hidden.Value = imageFilename;
            }
        }

        public void FillCustomerDetails(int partyRoleId, LoanApplication loan)
        {
            List<int> partyRoleCoborrower = new List<int>();
            List<int> partyRoleCoBorrower = new List<int>();
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.Retrieve(loan.ApplicationId);
            form.PartyRoleId = partyRoleId;
            CustomerDetailsModel model = new CustomerDetailsModel(partyRoleId);
            this.lblCustomerName.Text = model.Name;
            this.lblBorrowerName.Text = model.Name.ToUpper();

            var taxpayer = ObjectContext.Taxpayers.SingleOrDefault(x => x.PartyRoleId == partyRoleId);
            if (taxpayer != null && (!string.IsNullOrEmpty(taxpayer.Tin)))
            {
                var ctc = taxpayer.CurrentCtc;
                lblCtcNo.Text = ctc.CtcNumber;
                lblCtcIssuedOn.Text = String.Format("{0:MMMM dd, yyyy}", ctc.DateIssued);
                lblCtcIssuedAt.Text = ctc.IssuedWhere;
            }

            //District && Address
            var customerClassif = Customer.GetById(partyRoleId);
            var district = customerClassif.CurrentClassification;
            lblDistrict.Text = district.ClassificationType.District;
            lblHomeAddress.Text = model.PrimaryHomeAddress;

            //Contact No.
            string cellphoneNumber = "";
            string telephoneNumber = "";
            if (model.CellphoneNumber != null && model.TelephoneNumber != null)
            {
                if (!string.IsNullOrWhiteSpace(model.CellphoneNumber))
                    cellphoneNumber = model.CountryCode + model.CellphoneAreaCode + model.CellphoneNumber;
                if (!string.IsNullOrWhiteSpace(model.TelephoneNumber))
                    telephoneNumber = "/" + model.TelephoneNumberAreaCode + "-" + model.TelephoneNumber;
                lblContactNo.Text = cellphoneNumber + telephoneNumber;
            }

            //CoBorrower
            if (form.AvailableCoBorrowers.Count() > 0)
            {
                foreach (var item in form.AvailableCoBorrowers)
                {
                    lblCoBorrowerName.Text = StringConcatUtility.Build("/ ", lblCoBorrowerName.Text, item.Name);
                    partyRoleCoborrower.Add(item.PartyRoleId);
                }
                lblContactNo1.Text = "";
                FillCoBorrowerDetails(partyRoleCoborrower, loan);
            }
            else
            {
                lblCoBorrowerName.Text = "_____________________";
                lblContactNo1.Text = "________________";
                lblHomeAddress1.Text = "________________";
            }
        }

        private void FillCoBorrowerDetails(List<int> partyRoleCoborrower, LoanApplication loan)
        {
            if (partyRoleCoborrower.Count > 0)
            {
                lblHomeAddress1.Text = "";
                foreach (var item in partyRoleCoborrower)
                {
                    RoleType coBorrowerRoleType = RoleType.CoBorrowerApplicationType;
                    PartyRole coBorrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loan, coBorrowerRoleType);
                    PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(coBorrowerPartyRole.PartyId, RoleType.CustomerType);
                    PartyRole employeePartyRole = PartyRole.GetByPartyIdAndRole(coBorrowerPartyRole.PartyId, RoleType.EmployeeType);
                    PartyRole contactPartyRole = PartyRole.GetByPartyIdAndRole(coBorrowerPartyRole.PartyId, RoleType.ContactType);
                    var model = new CustomerDetailsModel();
                    int coBorrowerPartyRoleId = 0;
                    if (customerPartyRole != null)
                    {
                        coBorrowerPartyRoleId = customerPartyRole.Id;
                        model = new CustomerDetailsModel(coBorrowerPartyRoleId);   
                    }
                    else if (employeePartyRole != null)
                    {
                        coBorrowerPartyRoleId = employeePartyRole.Id;
                        model.InitializeAddresses(employeePartyRole.Party);
                    }
                    else if (contactPartyRole != null)
                    {
                        model.InitializeAddresses(contactPartyRole.Party);
                        coBorrowerPartyRoleId = contactPartyRole.Id;
                    }
                    else
                    {
                        return;
                    }

                    lblHomeAddress1.Text = StringConcatUtility.Build(" / ", lblHomeAddress1.Text, model.PrimaryHomeAddress);
                    string cellphoneNumber = "";
                    string telephoneNumber = "";
                    if (model.CellphoneNumber != null && model.TelephoneNumber != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.CellphoneNumber))
                            cellphoneNumber = model.CountryCode + model.CellphoneAreaCode + model.CellphoneNumber;
                        if (!string.IsNullOrWhiteSpace(model.TelephoneNumber))
                            telephoneNumber = " / " + model.TelephoneNumberAreaCode + "-" + model.TelephoneNumber;
                        lblContactNo1.Text = cellphoneNumber + telephoneNumber;
                    }
                    else
                    {
                        lblContactNo1.Text = "___________";
                    }

                    var taxpayerCoBorrower = ObjectContext.Taxpayers.SingleOrDefault(x => x.PartyRoleId == coBorrowerPartyRoleId);
                    if (taxpayerCoBorrower != null && (!string.IsNullOrWhiteSpace(taxpayerCoBorrower.Tin)))
                    {
                        var ctc = taxpayerCoBorrower.CurrentCtc;
                        lblCtcNo1.Text = StringConcatUtility.Build("/ ", lblCtcNo1.Text, ctc.CtcNumber);
                        var date = String.Format("{0:MMMM dd, yyyy}", ctc.DateIssued);
                        lblCtcIssuedOn1.Text = StringConcatUtility.Build("/ ", lblCtcIssuedOn1.Text, date);
                        lblCtcIssuedAt1.Text = StringConcatUtility.Build("/ ", lblCtcIssuedAt1.Text, ctc.IssuedWhere);
                    }

                    //District && Address
                    var customerClassif = Customer.GetById(coBorrowerPartyRoleId);
                    if (customerClassif != null)
                    {
                        var district = customerClassif.CurrentClassification;
                        lblDistrict1.Text = district.ClassificationType.District;
                    }

                }
            }
        }

        private class EmployeeDetailsModel
        {
            public string CountryCode { get; set; }
            public string TelephoneNumberAreaCode { get; set; }
            public string CellphoneAreaCode { get; set; }
            public string PrimaryHomeAddress { get; set; }
            public string CellphoneNumber { get; set; }
            public string TelephoneNumber { get; set; }
            public string PrimaryEmailAddress { get; set; }

            public EmployeeDetailsModel(Party party)
            {
                //Postal Address
                CustomerDetailsModel model = new CustomerDetailsModel();
                model.InitializeAddresses(party);
            }
        }

        public void FillDetails(LoanApplication loanApplication)
        {
            lblLoanAmount.Text = loanApplication.LoanAmount.ToString("N");
            var agreement = Agreement.GetByApplicationId(loanApplication.ApplicationId);
            var amort = ObjectContext.AmortizationSchedules.SingleOrDefault(x => x.AgreementId == agreement.Id);
            lblReleaseDate.Text = String.Format("{0:MMMM dd, yyyy}", amort.LoanReleaseDate);
            if (amort.AmortizationScheduleItems.Count() == 0)
            {
                lblMaturityDate.Text = "________________";
            }
            else
            {
                var date = amort.AmortizationScheduleItems.OrderByDescending(x=>x.ScheduledPaymentDate).First().ScheduledPaymentDate;
                lblMaturityDate.Text = String.Format("{0:MMMM dd, yyyy}", date);
            }

            lblPrincipalLoan.Text = ConvertNumbers.EnglishFromNumber((double)loanApplication.LoanAmount);
            lblPrincipal.Text = loanApplication.LoanAmount.ToString("N");
            lblPercentInWords.Text = ConvertNumbers.EnglishFromNumber((double)loanApplication.InterestRate);
            lblInterestRate.Text = loanApplication.InterestRate.ToString();

            var today = DateTime.Today;
            var numSuffix = NumberFacade.DaySuffix(today);

            //SPA
            lblDayToday.Text = today.Day.ToString() + numSuffix;
            lblMonthToday.Text = today.ToString("MMMM");
            lblYearToday.Text = today.ToString("yyyy");

            //Memorandum of Agreement
            lblDayToday1.Text = today.Day.ToString() + numSuffix;
            lblMonthToday1.Text = today.ToString("MMMM");
            lblYearToday1.Text = today.ToString("yyyy");

            lblDayToday2.Text = today.Day.ToString() + numSuffix;
            lblMonthToday2.Text = today.ToString("MMMM");
            lblYearToday2.Text = today.ToString("yyyy");

        }

        public void FillSignaturesForApproval()
        {
            LoanApprovalForm form = this.Retrieve<LoanApprovalForm>(ParentResourceGuid);
            string status = hdnIsSigned.Value.ToString();
            if (status == "Signed" || status == "Incomplete")
            {
                var detail = form.SPADocumentDetails;
                if (detail != null)
                {
                    imgLender.ImageUrl = detail.Lender.FilePath;
                    txtLender.Text = detail.Lender.PersonName;
                    hdnLender.Value = detail.Lender.FilePath;

                    imgBorrower.ImageUrl = detail.Borrower.FilePath;
                    hdnBorrower.Value = detail.Borrower.FilePath;
                    
                    imgWitness.ImageUrl = detail.Witness1.FilePath;
                    txtWitness.Text = detail.Witness1.PersonName;
                    hdnWitness1.Value = detail.Witness1.FilePath;

                    imgWitness2.ImageUrl = detail.Witness2.FilePath;
                    txtWitness2.Text = detail.Witness2.PersonName;
                    hdnWitness2.Value = detail.Witness2.FilePath;

                    imgWitness3.ImageUrl = detail.Witness3.FilePath;
                    txtWitness3.Text = detail.Witness3.PersonName;
                    hdnWitness3.Value = detail.Witness3.FilePath;

                    imgWitness4.ImageUrl = detail.Witness4.FilePath;
                    txtWitness4.Text = detail.Witness4.PersonName;
                    hdnWitness4.Value = detail.Witness4.FilePath;
                }
            }
        }

        public void FillSignatures(LoanApplication loanApplication)
        {
            btnPrint.Disabled = false;
            var formDetails = FormDetail.GetByLoanAppIdAndType(loanApplication.ApplicationId, FormType.SPAType);

            if (formDetails.Count() == 0 &&
                    loanApplication.CurrentStatus.LoanApplicationStatusType.Name !=
                    LoanApplicationStatusType.PendingApprovalType.Name)
            {
                imgWitness.Hide();
                imgWitness2.Hide();
                imgWitness3.Hide();
                imgWitness4.Hide();
                imgLender.Hide();
                imgBorrower.Hide();
            }
            foreach (var item in formDetails)
            {
                switch (item.RoleString)
                {
                    case "Lender":
                        lblLender.Text = item.PersonString;
                        imgLender.ImageUrl = item.Signature;
                        break;
                    case "Borrower":
                        imgBorrower.ImageUrl = item.Signature;
                        break;
                    case "Witness1":
                        lblWitness.Text = item.PersonString;
                        imgWitness.ImageUrl = item.Signature;
                        break;
                    case "Witness2":
                        lblWitness2.Text = item.PersonString;
                        imgWitness2.ImageUrl = item.Signature;
                        break;
                    case "Witness3":
                        lblWitness3.Text = item.PersonString;
                        imgWitness3.ImageUrl = item.Signature;
                        break;
                    case "Witness4":
                        lblWitness4.Text = item.PersonString;
                        imgWitness4.ImageUrl = item.Signature;
                        break;
                    default:
                        break;
                }
            }
        }

        public void FillLenderInformation(Party party)
        {
            //Fill Lender Name
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;

            //Fill Lender Address
            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);

            //Fill Lender Numbers
            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        public void FillPostalAddress(PostalAddress postalAddress)
        {
            lblStreetAddress.Text = postalAddress.StreetAddress;
            lblBarangay.Text = postalAddress.Barangay;
            lblCity.Text = postalAddress.City;
            lblMunicipality.Text = postalAddress.Municipality;
            lblProvince.Text = postalAddress.Province;
            lblCountry.Text = postalAddress.Country.Name;
            lblPostalCode.Text = postalAddress.PostalCode;
        }
    }
}