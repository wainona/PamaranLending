using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class ForExReport : ActivityPageBase
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
                var today = DateTime.Today;
                RetreiveHeaderDetails();
                dtEndDate.SelectedDate = today;
                dtEndDate.MaxDate = today;
                dtStartDate.MaxDate = today;
            }
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var startDate = dtStartDate.SelectedDate;
            var endDate = dtEndDate.SelectedDate;
            if (startDate == endDate)
                lblDateRange.Text = startDate.ToString("MMMM dd, yyyy");
            else
                lblDateRange.Text = startDate.ToString("MMMM dd, yyyy") + " - " + endDate.ToString("MMMM dd, yyyy");

            var list = CreateQuery(startDate, endDate);

            StoreForExReport.DataSource = list;
            StoreForExReport.DataBind();

            btnPrint.Disabled = false;
        }

        protected void onStartDateSelect(object sender, EventArgs e)
        {
            dtEndDate.MinDate = dtStartDate.SelectedDate.Date;
        }

        protected void onEndDateSelect(object sender, EventArgs e)
        {
            dtStartDate.MaxDate = dtEndDate.SelectedDate.Date;
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
            lblLenderFaxNumber.Text = "Fax " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }

        public IEnumerable<ForExTransactionModel> CreateQuery(DateTime startDate, DateTime endDate)
        {
            var endDatePlusOne = endDate.AddDays(1);
            var query = from fe in ObjectContext.ForeignExchanges
                        where fe.TransactionDate >= startDate && fe.TransactionDate <= endDatePlusOne
                        select new ForExTransactionModel()
                        {
                            ForeignExchange = fe,
                            Id = fe.Id,
                            TransactionDate = fe.TransactionDate,
                            OriginalAmount = fe.AmountReceived,
                            ConvertedAmount = fe.AmountReleased,
                            Rate = fe.Rate
                        };
            IEnumerable<ForExTransactionModel> result = query.ToList();
            return result;
        }

        public class ForExTransactionModel 
        {
            public ForeignExchange ForeignExchange { get; set; }

            public int Id { get; set; }
            public string CustomerName
            {
                get
                {
                    return this.ForeignExchange.PartyRole1.Party.Name;
                }
            }
            public DateTime TransactionDate { get; set; }
            public string _TransactionDate { 
                get
                {
                    return this.TransactionDate.ToString("MMM dd, yyyy");
                }
            }
            public decimal OriginalAmount { get; set; }
            public string OriginalCurrency
            {
                get
                {
                    return this.ForeignExchange.Currency.Symbol;
                }
            }
            public decimal ConvertedAmount { get; set; }
            public string ConvertedCurrency
            {
                get
                {
                    return this.ForeignExchange.Currency1.Symbol;
                }
            }
            public decimal Rate { get; set; }
        }
     }
}