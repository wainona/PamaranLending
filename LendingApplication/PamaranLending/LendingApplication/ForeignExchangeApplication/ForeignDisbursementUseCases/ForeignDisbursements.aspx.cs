using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.ForeignExchangeApplication.ForeignDisbursementUseCases
{
    public partial class ForeignDisbursements : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                allowed.Add("Cashier");
                return allowed;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.LoginInfo.UserType == UserAccountType.Accountant.Name)
                {
                    btnOpen.Hidden = true;
                    btnAdd.Hidden = true;
                    btnOpenSeparator.Hidden = true;
                }
            }
        }
        [DirectMethod]
        public int checkType(int id)
        {
            int result = 0;
            using (var context = new FinancialEntities())
            {
                var disbursementType = context.Disbursements.SingleOrDefault(entity => entity.PaymentId == id);
                if (disbursementType.DisbursementTypeId == DisbursementType.EncashmentType.Id) result = 1;
                else if (disbursementType.DisbursementTypeId == DisbursementType.RediscountingType.Id) result = 2;
            }

            return result;
        }
        public void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            using (var context = new FinancialEntities())
            {

                SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;
                FilterBy filterBy = (FilterBy)cmbFilter.SelectedIndex;

                DataSource dataSource = new DataSource();
                dataSource.SearchString = txtSearch.Text;
                if (cmbFilter.SelectedItem.Text == "All") dataSource.FilterString = "";
                else dataSource.FilterString = cmbFilter.SelectedItem.Text;

                dataSource.SearchBy = searchBy;
                dataSource.FilterBy = filterBy;
                if (dtFrom.SelectedValue != null)
                    dataSource.FromDate = dtFrom.SelectedDate;
                if (dtTo.SelectedValue != null)
                    dataSource.ToDate = dtTo.SelectedDate;

                e.Total = dataSource.Count;
                this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.DisbursementId.ToString());
                this.PageGridPanelStore.DataBind();
            }
        }
        public void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        public enum SearchBy
        {
            CustomerName = 0,
            CollectorName = 1,
            None = -1
        }
        public enum FilterBy
        {
            Encashment = 0,
            Rediscounting = 1,
            All = -1

        }
        private class DisbursementModel
        {
            public int DisbursementId { get; set; }
            public DateTime Date { get; set; }
            public string DateStr 
            {
                get
                {
                    return this.Date.ToString("yyyy-MM-dd");
                }
            }
            public string DisbursedTo { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
            public string DisbursedBy { get; set; }
            public int DisbursementTypeId { get; set; }
            public string DisbursementType { get; set; }
            public int LoanAccountId { get; set; }
            public string Currency { get; set; }
            public string strLoanAccountId
            {
                get
                {
                    if (this.LoanAccountId != -1) return this.LoanAccountId.ToString();
                    else return "N/A";
                }
            }
        }
        private class DataSource : IPageAbleDataSource<DisbursementModel>
        {
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public string FilterString { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.FilterString = string.Empty;
 
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        if (query != null) count = query.Count();
                    }

                    return count;
                }
            }
            private IQueryable<DisbursementModel> CreateQuery(FinancialEntities context)
            {
                var disbursements = from d in context.DisbursementViewLists
                                    select d;

                var foreignDisbursements = from p in context.PaymentCurrencyAssocs
                                           join d in context.DisbursementViewLists on p.PaymentId equals d.DisbursementId
                                           where p.Currency.Symbol == "PHP"
                                           select d;

                var notforeignDisbursements = disbursements.Except(foreignDisbursements);

                var query = from p in notforeignDisbursements
                                   join pd in context.Payments on p.DisbursementId equals pd.Id
                                   join pc in context.PaymentCurrencyAssocs on p.DisbursementId equals pc.PaymentId
                                   where p.DisbursementTypeId != DisbursementType.LoanDisbursementType.Id
                                   && pd.ParentPaymentId == null
                                   select new DisbursementModel()
                                   {
                                       DisbursementId = p.DisbursementId,
                                       Date = p.Date,
                                       DisbursedTo = p.DisbursedTo,
                                       Amount = p.Amount,
                                       Type = p.Type,
                                       DisbursedBy = p.DisbursedBy,
                                       DisbursementTypeId = p.DisbursementTypeId,
                                       DisbursementType = p.DisbursementType,
                                       LoanAccountId = -1,
                                       Currency = pc.Currency.Description
                                   };
                
                switch (SearchBy)
                {
                    case SearchBy.CustomerName:
                        query = query.Where(entity => entity.DisbursedTo.Contains(SearchString));
                        break;
                    case SearchBy.CollectorName:
                        query = query.Where(entity => entity.DisbursedBy.Contains(SearchString));
                        break;
                    case SearchBy.None:
                        break;
                    default:
                        break;
                }
                switch (FilterBy)
                {
                    case FilterBy.Encashment:
                        query = query.Where(entity => entity.DisbursementType.Equals(FilterString));
                        break;
                    case FilterBy.All:
                        query = query.Where(entity => entity.DisbursementType.Contains(FilterString));
                        break;
                    case FilterBy.Rediscounting:
                        query = query.Where(entity => entity.DisbursementType.Contains(FilterString));
                        break;
                    default:
                        break;
                }

                if (FromDate.HasValue && ToDate.HasValue)
                {
                    query = query.Where(entity => entity.Date >= FromDate && entity.Date <= ToDate);
                }

                return query;
            }

            public override List<DisbursementModel> SelectAll(int start, int limit, Func<DisbursementModel, string> orderBy)
            {
                List<DisbursementModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }
                return collection;
            }

        }
    }
}