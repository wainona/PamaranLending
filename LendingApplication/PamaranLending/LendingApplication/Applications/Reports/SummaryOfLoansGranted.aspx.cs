using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.Reports
{
    public partial class SummaryOfLoansGranted : ActivityPageBase
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
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //HEADER LENDER INFORMATION
                //dtMonthEnd.SelectedDate = DateTime.Now;
                dtMonthStart.SelectedDate = DateTime.Now;
                int endOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                dtMonthStart.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, endOfMonth);
                //SystemSetting.YearsofLoanstobeDeleted

                
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderInformation(party);
            }
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var ownerRoleType = RoleType.OwnerFinancialType;
            var FromDate = DateTime.Parse(hdnDate.Value.ToString());
            int endOfMonth = DateTime.DaysInMonth(FromDate.Year, FromDate.Month);
            DateTime maxFromDate = new DateTime(FromDate.Year, FromDate.Month, endOfMonth);

            var filter = from la in ObjectContext.LoanAccounts
                         join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                         where las.StatusTypeId == LoanAccountStatusType.RestructuredType.Id &&
                         las.TransitionDateTime >= FromDate && las.TransitionDateTime <= maxFromDate
                         select la;

            var query = from la in ObjectContext.LoanAccounts.Except(filter)
                        join fa in ObjectContext.FinancialAccounts.Where(entity => entity.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id)
                             on la.FinancialAccountId equals fa.Id
                        join far in ObjectContext.FinancialAccountRoles.Where(entity => entity.PartyRole.RoleTypeId == ownerRoleType.Id)
                              on fa.Id equals far.FinancialAccountId
                        join a in ObjectContext.Agreements on fa.AgreementId equals a.Id
                        join ldv in ObjectContext.LoanDisbursementVcrs on a.Id equals ldv.AgreementId
                        join las in ObjectContext.LoanAccountStatus on fa.Id equals las.FinancialAccountId
                        join dvs in ObjectContext.DisbursementVcrStatus on ldv.Id equals dvs.LoanDisbursementVoucherId
                        join lapp in ObjectContext.LoanApplications on a.ApplicationId equals lapp.ApplicationId
                        where a.AgreementType.Id == AgreementType.LoanAgreementType.Id
                        && la.LoanReleaseDate >= FromDate && la.LoanReleaseDate <= maxFromDate
                        && (dvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id ||
                        dvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.FullyDisbursedType.Id)
                        && (las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id)
                        //&& fa.ParentFinancialAccountId == null
                        select new {
                            LoanApplication = lapp,
                            Party = far.PartyRole.Party,
                            LoanAccount = la
                        };

            List<LoansGrantedModel> models = new List<LoansGrantedModel>();
            foreach (var item in query.ToList())
            {
                models.Add(new LoansGrantedModel(item.LoanApplication, item.Party, item.LoanAccount));
            }

            lblFormName.Text = String.Format("{0:MMMM dd}", new DateTime(FromDate.Year, FromDate.Month, 1)) + " - " +
                String.Format("{0:MMMM dd yyyy}", maxFromDate);
            strLoansGranted.DataSource = models;
            strLoansGranted.DataBind();
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

        public enum Months
        { 
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        private class LoansGrantedModel
        {
            public int LoanApplicationId { get; set; }
            public string BorrowersName { get; set; }
            public string LoanProduct { get; set; }
            public decimal LoanAmount { get; set; }
            public decimal? InterestRate { get; set; }
            public int LoanTerm { get; set; }
            public string PaymentMode { get; set; }
            public string CollateralRequirement { get; set; }
            public decimal LoanBalance { get; set; }
            public string LoanAccountStatus { get; set; }

            public LoansGrantedModel(LoanApplication loanApplication, Party party, LoanAccount loanAccount)
            {
                this.LoanApplicationId = loanApplication.ApplicationId;
                this.LoanAmount = loanApplication.LoanAmount;
                this.InterestRate = loanApplication.InterestRate;
                this.LoanTerm = loanApplication.LoanTermLength;
                var aiCollateralRequirement = ApplicationItem.GetFirstActive(loanApplication.Application, ProductFeatureCategory.CollateralRequirementType);
                this.LoanProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct.Name;
                this.LoanAccountStatus = loanAccount.CurrentStatus.LoanAccountStatusType.Name;
                var collateral = aiCollateralRequirement.ProductFeatureApplicability.ProductFeature.Name;
                if (!string.IsNullOrWhiteSpace(collateral))
                    this.CollateralRequirement = collateral;
                else
                    this.CollateralRequirement = "N/A";
                this.PaymentMode = UnitOfMeasure.GetByID(loanApplication.PaymentModeUomId).Name;
                this.LoanBalance = loanAccount.LoanBalance;
                if (party.PartyTypeId == PartyType.PersonType.Id)
                {
                    Person personAsCustomer = party.Person;

                    this.BorrowersName = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                        , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                        personAsCustomer.NameSuffixString);
                }
            }
        }
    }
}