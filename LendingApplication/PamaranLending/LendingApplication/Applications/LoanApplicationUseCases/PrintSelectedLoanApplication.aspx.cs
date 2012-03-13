using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using System.IO;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class PrintSelectedLoanApplication : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            var imageFilename = "";
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int customerId = int.Parse(Request.QueryString["customerPartyRoleId"]);
                int loanApplicationId = int.Parse(Request.QueryString["loanApplicationId"]);

                var loanApplication = LoanApplication.GetById(loanApplicationId);
                hdnLoanApplicationId.Value = loanApplicationId;
                hdnCustomerId.Value = customerId;
                var uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Images");
                imgPersonPicture.ImageUrl = imageFilename + "../../../Resources/images/noimage.jpg";
                //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderInformation(party);

                FillBorrowersBasicInformation(customerId);
                FillLoanApplicationDetails(loanApplication.ApplicationId);
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

        public void FillBorrowersBasicInformation(int partyRoleId)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.PartyRoleId = partyRoleId;
            CustomerDetailsModel model = new CustomerDetailsModel(partyRoleId);
            this.lblCustomerName.Text = model.Name;
            this.lblDistricts.Text = model.District;
            this.lblStation.Text = model.StationNumber;
            this.lblGender.Text = model.Gender;
            this.lblAge.Text = model.Age.ToString();
            this.lblCreditLimit.Text = String.Format("{0:#,##0.00;(#,##0.00);Zero}", model.CreditLimit);
            this.lblPrimaryHomeAddress.Text = model.PrimaryHomeAddress;
            this.lblCellphoneNumber.Text = model.CountryCode + model.CellphoneAreaCode + model.CellphoneNumber;
            this.lblTelephoneNumber.Text = model.TelephoneNumberAreaCode + model.TelephoneNumber;
            this.lblPrimaryEmailAddress.Text = model.PrimaryEmailAddress;

            var borrowerParty = PartyRole.GetById(form.PartyRoleId);
            if (borrowerParty.Party.Person.ImageFilename != null)
                imgPersonPicture.ImageUrl = borrowerParty.Party.Person.ImageFilename;
        }

        public void FillLoanApplicationDetails(int loanApplicationId)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.Retrieve(loanApplicationId);
            this.lblApplicationId.Text = form.LoanApplicationId.ToString();
            this.lblApplicationDate.Text = String.Format("{0:MMMM d, yyyy}", form.LoanApplicationDate);
            List<string> x = new List<string>();
            x.Add("");

            var loanApplication = LoanApplication.GetById(form.LoanApplicationId);
            var stat = LoanApplicationStatu.GetActive(loanApplication);

            this.lblApplicationStatus.Text = form.LoanApplicationStatus + " on " + String.Format("{0:MMMM d, yyyy}", stat.TransitionDateTime);
            this.lblStatusComment.Text = form.StatusComment;

            var product = FinancialProduct.GetById(form.FinancialProductId);
            this.lblProductName.Text = product.Name;
            this.lblLoanAmount.Text = String.Format("{0:#,##0.00;(#,##0.00);Zero}", form.LoanAmount);
            var loanTerm = UnitOfMeasure.GetByID(form.LoanTermUomId);
            this.lblLoanTerm.Text = form.LoanTerm.ToString() + " " + loanTerm.Name;
            //this.lblLoanPurpose.Text = form.LoanPurpose;

            var collateral = ProductFeatureApplicability.GetById(form.CollateralRequirementId);
            this.lblCollateralRequirement.Text = collateral.ProductFeature.Name;

            var interestComputation = ProductFeatureApplicability.GetById(form.InterestComputationModeId);
            this.lblInterestComputation.Text = interestComputation.ProductFeature.Name;

            var paymentMode = UnitOfMeasure.GetByID(form.PaymentModeUomId);
            this.lblPaymentMode.Text = paymentMode.Name;

            var methodOfChargingInterest = ProductFeatureApplicability.GetById(form.MethodOfChargingInterestId);
            this.lblMethodOfChargingInterest.Text = methodOfChargingInterest.ProductFeature.Name;

            //form.SetSelectedOutstandingLoansToPayoff(loanApplicationId, form.RetrieveOutstandingLoans());
            //var selectedLoans = form.SelectedLoansToPayOff;
            //if (selectedLoans != null)
            //{
            //    storePayOutstandingLoan.DataSource = selectedLoans;
            //    storePayOutstandingLoan.DataBind();
            //}

            this.lblInterestRateDesc.Text = form.InterestRateDescription;
            this.lblInterestRate.Text = form.InterestRate.ToString() + " " + "%";

            //this.lblPastDueDesc.Text = form.PastDueInterestRateDescription;
            //this.lblPastDueRate.Text = form.PastDueInterestRate.ToString() + " " + "%";

            decimal? total = 0; 
            foreach (var item in form.AvailableFees)
            {
                total += item.TotalChargePerFee;
            }
            this.lblTotalFees.Text = "Total (Php): " + " " + total.ToString();
            StoreFee.DataSource = form.AvailableFees.ToList();
            StoreFee.DataBind();
            StoreCoBorrower.DataSource = form.AvailableCoBorrowers;
            StoreCoBorrower.DataBind();
            StoreGuarantor.DataSource = form.AvailableGuarantors;
            StoreGuarantor.DataBind();
            StoreCollaterals.DataSource = form.AvailableCollaterals;
            StoreCollaterals.DataBind();
            StoreSubmittedDocuments.DataSource = form.AvailableSubmittedDocuments;
            StoreSubmittedDocuments.DataBind();
            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();

            var status = LoanApplicationStatu.GetStatus(loanApplication, LoanApplicationStatusType.PendingApprovalType);
            //LoanApplicationRole role = ObjectContext.LoanApplicationRoles.FirstOrDefault(entity => entity.PartyRole.EffectiveDate == status.TransitionDateTime
            //        && entity.PartyRole.RoleTypeId == RoleType.ProcessedByApplicationType.Id);
            //this.lblProcessedBy.Text = Person.GetPersonFullName(role.PartyRole.Party).ToUpper();
            if (status == null)
            {
                LoanApplicationRole role = ObjectContext.LoanApplicationRoles.FirstOrDefault(entity => entity.PartyRole.RoleTypeId == RoleType.ProcessedByApplicationType.Id);
                this.lblProcessedBy.Text = Person.GetPersonFullName(role.PartyRole.Party).ToUpper();
            }
            else
            {
                LoanApplicationRole role = ObjectContext.LoanApplicationRoles.FirstOrDefault(entity => entity.PartyRole.EffectiveDate == status.TransitionDateTime
                    && entity.PartyRole.RoleTypeId == RoleType.ProcessedByApplicationType.Id);
                this.lblProcessedBy.Text = Person.GetPersonFullName(role.PartyRole.Party).ToUpper();
            }

            if (form.LoanApplicationStatus == LoanApplicationStatusType.PendingInFundingType.Name || 
                form.LoanApplicationStatus == LoanApplicationStatusType.ClosedType.Name)
            {
                PartyRole approvedByPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, RoleType.ApprovedByAgreementType);
                if (approvedByPartyRole != null && approvedByPartyRole.Party != null && approvedByPartyRole.Party.PartyTypeId == PartyType.PersonType.Id)
                {
                    this.lblApprovedBy.Text = Person.GetPersonFullName(approvedByPartyRole.Party).ToUpper();
                }
            }
            else if (form.LoanApplicationStatus == LoanApplicationStatusType.RejectedType.Name)
            {
                PartyRole rejectedByPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, RoleType.ProcessedByApplicationType);
                this.lblApprovedBy.Text = Person.GetPersonFullName(rejectedByPartyRole.Party).ToUpper();
            }
        }
    }
}