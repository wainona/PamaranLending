using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class ListForExTransactions : ActivityPageBase
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
            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchString.Text;
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, (entity => entity.Id.ToString()));
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {

            }
        }

        private class DataSource : IPageAbleDataSource<ForExTransactionViewModel>
        {
            public string SearchString { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
            }

            private IEnumerable<ForExTransactionViewModel> CreateQuery()
            {
                var query = from fet in ObjectContext.ForeignExchanges
                            select new ForExTransactionViewModel()
                            {
                                Id = fet.Id,
                                ProcessedById = fet.ProcessedByPartyRoleId,
                                ProcessedToId = fet.ProcessedToPartyRoleId,
                                Date = fet.TransactionDate,
                                AmountReceived = fet.AmountReceived,
                                AmountReleased = fet.AmountReleased,
                                ReceivedCurrency = fet.Currency.Symbol,
                                ReleasedCurrency = fet.Currency1.Symbol,
                                Rate = fet.Rate,
                                Type = fet.ExchangeRate.ExchangeRateType.Name
                            };
                //query.ToList();
                IEnumerable<ForExTransactionViewModel> result = query.ToList();
                result = result.Where(entity => entity.CustomerName.ToLower().Contains(this.SearchString.ToLower()));

                return result;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    var query = CreateQuery();
                    count = query.Count();

                    return count;
                }
            }

            public override List<ForExTransactionViewModel> SelectAll(int start, int limit, Func<ForExTransactionViewModel, string> orderBy)
            {
                var query = CreateQuery().ToList();
                query.OrderBy(orderBy).Skip(start).Take(limit);
                return query;
            }
        }

        public class ForExTransactionViewModel
        {
            public int Id { get; set; }
            public int ProcessedById { get; set; }
            public int ProcessedToId { get; set; }
            public DateTime Date { get; set; }
            public bool IsSpot { get; set; }
            public string _Date
            {
                get
                {
                    return this.Date.ToString("yyyy-MM-dd");
                }
            }
            public decimal Rate { get; set; }
            public decimal AmountReceived { get; set; }
            public decimal AmountReleased { get; set; }
            public string CustomerName
            {
                get
                {
                    return PartyRole.GetById(this.ProcessedToId).Party.Name;
                }
            }
            public string ProcessedBy
            {
                get
                {
                    return PartyRole.GetById(this.ProcessedById).Party.Name;
                }
            }
            public string ReceivedCurrency { get; set; }
            public string ReleasedCurrency { get; set; }
            public string Type { get; set; }
            public string ForExType
            {
                get
                {

                    if (this.IsSpot == true)
                        return Type + "-Spot";
                    else return Type;
                    
                }
            }
        }
    }
}