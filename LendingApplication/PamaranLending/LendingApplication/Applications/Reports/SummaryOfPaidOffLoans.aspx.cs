using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using Ext.Net;

namespace LendingApplication.Applications.Reports
{
    public partial class SummaryOfPaidOffLoans : ActivityPageBase
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            var startDate = dtStartDate.SelectedDate;
            var endDate = dtEndDate.SelectedDate;
            if (startDate.Date == endDate.Date)
            {
                lblStartDate.Text = startDate.Date.ToString("MMMM dd, yyyy");
                lblEndDate.Text = string.Empty;
            }
            else
            {
                lblStartDate.Text = startDate.Date.ToString("MMMM dd, yyyy") + " -";
                lblEndDate.Text = endDate.Date.ToString("MMMM dd, yyyy");
            }

            var queryList = CreateQuery(startDate, endDate);

            PaidOffLoansStore.DataSource = queryList;
            PaidOffLoansStore.DataBind();

        }

        protected void onStartDateSelect(object sender, EventArgs e)
        {
            dtEndDate.MinDate = dtStartDate.SelectedDate.Date;
        }

        protected void onEndDateSelect(object sender, EventArgs e)
        {
            dtStartDate.MaxDate = dtEndDate.SelectedDate.Date;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            RetreiveHeaderDetails();
        }

        private void RetreiveHeaderDetails()
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            var lenderName = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == lenderPartyRole.Party.Id).OrganizationName;
            lblLenderName.Text = lenderName;

            var lenderAddress = PostalAddress.GetCurrentPostalAddress(lenderPartyRole.Party, PostalAddressType.BusinessAddressType, true);
            lblLenderAddress.Text = StringConcatUtility.Build(", ", lenderAddress.StreetAddress, lenderAddress.Barangay,
                                                                    lenderAddress.City, lenderAddress.Municipality);

            lblLenderTelephoneNumber.Text = "Tel. No. " + PrintFacade.GetPrimaryPhoneNumber(lenderPartyRole.Party, lenderAddress);
            lblLenderFaxNumber.Text = "Fax No. " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }

        private List<PaidOffLoansModel> CreateQuery(DateTime startDate, DateTime endDate)
        {
            var date1 = endDate.AddDays(1);
            var query = from la in ObjectContext.LoanAccounts
                        join las in ObjectContext.LoanAccountStatus.Where(entity => entity.IsActive == true 
                                        && entity.LoanAccountStatusType.Id == LoanAccountStatusType.PaidOffType.Id) 
                        on la.FinancialAccountId equals las.FinancialAccountId
                        join fa in ObjectContext.FinancialAccounts on la.FinancialAccountId equals fa.Id
                        join fap in ObjectContext.FinancialAccountProducts on fa.Id equals fap.FinancialAccountId
                        join fp in ObjectContext.FinancialProducts on fap.FinancialProductId equals fp.Id
                        join ag in ObjectContext.Agreements on fa.AgreementId equals ag.Id
                        join ai in ObjectContext.AgreementItems.Where(entity => entity.IsActive == true) on ag.Id equals ai.AgreementId
                        join lar in ObjectContext.FinancialAccountRoles on fa.Id equals lar.FinancialAccountId
                        join pr in ObjectContext.PartyRoles.Where(entity => entity.RoleTypeId == RoleType.OwnerFinancialType.Id)
                        on lar.PartyRoleId equals pr.Id
                        where las.TransitionDateTime >= startDate && las.TransitionDateTime <= date1
                        select lar;

            List<PaidOffLoansModel> paidOffLoansItems = new List<PaidOffLoansModel>();

            foreach (var item in query)
            {
                paidOffLoansItems.Add(new PaidOffLoansModel(item));
            }

            return paidOffLoansItems;

        }


        private class PaidOffLoansModel
        {
            public string Name { get; set; }
            public string LoanType { get; set; }
            public decimal LoanAmount { get; set; }
            public string InterestRate { get; set; }
            public string LoanTerm { get; set; }
            public DateTime DatePaidOff { get; set; }
            public string _DatePaidOff { get; set; }

            public PaidOffLoansModel(FinancialAccountRole far)
            {
                var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.Id == far.PartyRoleId && entity.EndDate == null);
                var agreementItem = far.FinancialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.AgreementId == far.FinancialAccount.AgreementId && entity.IsActive);
                this.Name = Person.GetPersonFullName(partyRole.Party);
                this.LoanType = far.FinancialAccount.FinancialAccountProducts.SingleOrDefault(entity => entity.EndDate == null).FinancialProduct.Name;
                this.LoanAmount = far.FinancialAccount.LoanAccount.LoanAmount;
                this.InterestRate = agreementItem.InterestRate.ToString() + "%";
                this.LoanTerm = agreementItem.LoanTermLength + " " + agreementItem.LoanTermUom;
                this.DatePaidOff = far.FinancialAccount.LoanAccount.CurrentStatus.TransitionDateTime;
                this._DatePaidOff = this.DatePaidOff.ToString("MMMM dd, yyyy");
            }
        }
    }
}