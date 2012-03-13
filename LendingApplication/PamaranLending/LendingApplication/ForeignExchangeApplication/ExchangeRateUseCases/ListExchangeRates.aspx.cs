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
    public partial class ListExchangeRates : ActivityPageBase
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
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, string.Empty).OrderBy(entity => entity.Id);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {

            }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e) 
        {
            var rsm = PageGridPanelSelectionModel.SelectedRows;
            int selectedId = int.Parse(rsm[0].RecordID);
            var exrate = ExchangeRate.GetById(selectedId);
            ObjectContext.ExchangeRates.DeleteObject(exrate);
            ObjectContext.SaveChanges();
        }

        private class DataSource : IPageAbleDataSource<ExchangeRateViewModel>
        {
            private IEnumerable<ExchangeRateViewModel> CreateQuery()
            {
                var query = from er in ObjectContext.ExchangeRates
                            where er.IsActive == true
                            select new ExchangeRateViewModel()
                            {
                                _ExchangeRate = er,
                                Id = er.Id,
                                Rate = er.Rate,
                                ExchangeRateType = er.ExchangeRateType.Name,
                                Date = er.Date
                            };

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
            public string _Date
            {
                get
                {
                    return this.Date.ToString("MMMM dd, yyyy");
                }
            }
            public string ExchangeRateType { get; set; }
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
        }
    
    }

  
}