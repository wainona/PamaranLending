using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class ListCollection : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;
 
                DataSource dataSource = new DataSource();
                dataSource.SearchString = txtSearchKey.Text;
                dataSource.SearchBy = searchBy;
                dataSource.FromDate = datFromDate.SelectedDate;
                dataSource.ToDate = datToDate.SelectedDate;
                dataSource.Currency = cmbCurrency.SelectedItem.Text;

                e.Total = dataSource.Count;
                this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
                this.PageGridPanelStore.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var today = DateTime.Now;
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                List<CurrencySymbolModel> currencies = new List<CurrencySymbolModel>();
                currencies.AddRange(Currency.GetCurrencySymbols());

                CurrencySymbolModel allModel = new CurrencySymbolModel();
                allModel.Id = -1;
                allModel.Symbol = "All";
              
                currencies.Add(allModel);
                strCurrency.DataSource = currencies.ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = -1;
            }
        }

        private class CollectionListModel
        {
            public int? CollectionID { get; private set; }
            public DateTime Date { get; private set; }
            public string DateStr { get; private set; }
            public string Customer { get; private set; }
            public Decimal Amount { get; private set; }
            public string Collector { get; private set; }
            public string CollectionType { get; private set; }
            public string CurrencySymbol { get; private set; }

            public CollectionListModel(Payment payment, string type)
            {
                this.CollectionType = type;
                this.CollectionID = payment.Id;
                this.Date = payment.TransactionDate;
                this.DateStr = this.Date.ToString("yyyy-MM-dd");
                this.Amount = payment.TotalAmount;
                var collectorRole = PartyRole.GetById(payment.ProcessedByPartyRoleId);
                this.Collector = collectorRole.Party.Name;
                this.CurrencySymbol = Currency.CurrencySymbolByPaymentId(payment.Id);
                if (payment.ProcessedToPartyRoleId.HasValue)
                {
                    var customerRole = PartyRole.GetById(payment.ProcessedToPartyRoleId.Value);
                    this.Customer = customerRole.Party.Name;
                }
            }
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
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
            All = 0,
            FeePayment = 1,
            LoanPayment = 2,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<CollectionListModel>
        {
            public string Name { get; set; }
            public string FilterString { get; set; }
            public int ID { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public string Currency { get; set; }

            public DataSource()
            {
                this.Currency = string.Empty;
                this.Name = string.Empty;
                this.FilterBy = ListCollection.FilterBy.None;
                this.SearchBy = ListCollection.SearchBy.None;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IEnumerable<CollectionListModel> CreateQuery(FinancialEntities context)
            {
                var loanPayments = from lp in context.LoanPayments
                                   join p in context.Payments on lp.PaymentId equals p.Id
                                   where p.TotalAmount > 0
                                   select p;

                var feePayments = from fp in context.FeePayments
                                  join p in context.Payments on fp.PaymentId equals p.Id
                                  where p.TotalAmount > 0
                                  select p;



                var collection = new List<CollectionListModel>();
                foreach (var payment in loanPayments)
                {
                    CollectionListModel model = new CollectionListModel(payment,"Loan Payment");
                    collection.Add(model);
                }
                foreach (var payment in feePayments)
                {
                    CollectionListModel model = new CollectionListModel(payment, "Fee Payment");
                    collection.Add(model);
                }
                 var models = collection.AsEnumerable();

                if (string.IsNullOrEmpty(Currency) == false)
                {
                    if (Currency == "All")
                        models = models.Where(entity => entity.CurrencySymbol.Contains(""));
                    else models = models.Where(entity => entity.CurrencySymbol.Contains(Currency));
                }
               
                switch (SearchBy)
                {
                    case SearchBy.CustomerName:
                        models = models.Where(entity => entity.Customer.Contains(SearchString));
                        break;

                    case SearchBy.CollectorName:
                        models = models.Where(entity => entity.Collector.Contains(SearchString));
                        break;

                    case SearchBy.None:
                        break;

                    default:
                        break;
                }
                

                if (!FromDate.Equals(DateTime.MinValue) && !ToDate.Equals(DateTime.MinValue))
                {
                    if (ToDate.Equals(DateTime.MinValue))
                        ToDate = DateTime.MaxValue;
                     models = models.Where(entity => entity.Date >= FromDate && entity.Date <= ToDate);
                }

                //query = query.OrderByDescending(entity => entity.CollectionID);
                return models;
            }

            public override List<CollectionListModel> SelectAll(int start, int limit, Func<CollectionListModel, string> orderBy)
            {
                List<CollectionListModel> collection;
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