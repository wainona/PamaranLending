using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.ForeignExchangeApplication.ExchangeRateUseCases
{
    public partial class PickListExchangeRate : ActivityPageBase
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
            dataSource.ExchangeRateType = hdnExRateType.Text;
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, (entity => entity.Id.ToString()));
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                hdnExRateType.Text = Request.QueryString["ratetype"];
            }
        }

        private class DataSource : IPageAbleDataSource<ExchangeRateViewModel>
        {
            public string ExchangeRateType { get; set; }
            private IEnumerable<ExchangeRateViewModel> CreateQuery()
            {
                var query = from er in ObjectContext.ExchangeRates
                            where er.IsActive == true
                            select new ExchangeRateViewModel()
                            { 
                                _ExchangeRate = er,
                                Id = er.Id,
                                Rate = er.Rate,
                                Date = er.Date,
                                Type = er.ExchangeRateType.Name
                            };

                if (this.ExchangeRateType != "None")
                    query = query.Where(x => x.Type == this.ExchangeRateType);

                return query.ToList();
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery();
                        count = query.Count();
                    }

                    return count;
                }
            }

            public override List<ExchangeRateViewModel> SelectAll(int start, int limit, Func<ExchangeRateViewModel, string> orderBy)
            {
                var query = CreateQuery().ToList();
                query.OrderBy(orderBy).Skip(start).Take(limit);
                return query;
            }
        }

        public class ExchangeRateViewModel
        {
            public int Id { get; set; }
            public ExchangeRate _ExchangeRate { get; set; }
            public DateTime Date { get; set; }
            public string CurrencyFrom
            {
                get
                {
                    return this._ExchangeRate.Currency.Symbol + " - " + this._ExchangeRate.Currency.Description;
                }
            }
            public string CurrencyTo
            {
                get
                {
                    return this._ExchangeRate.Currency1.Symbol + " - " + this._ExchangeRate.Currency1.Description;
                }
            }
            public decimal Rate { get; set; }
            public string Type { get; set; }
        }
        
    }

  
}