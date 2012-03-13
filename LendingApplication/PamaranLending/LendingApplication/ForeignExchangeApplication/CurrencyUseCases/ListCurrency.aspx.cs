using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.ForeignExchangeApplication.CurrencyUseCases
{
    public partial class ListCurrency : ActivityPageBase
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
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, (entity => entity.Id.ToString()));
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
            var rsm = this.PageGridPanelSelectionModel.SelectedRows;
            
            var id = int.Parse(rsm[0].RecordID);

            if (CheckCurrencyIfActive(id))
            {
                X.Msg.Alert("Alert", "The selected currency is currently used by the system. The currency cannot be deleted.").Show();
                return;
            }

            //check if the currency is used by the system
            var record = ObjectContext.Currencies.SingleOrDefault(entity => entity.Id == id);
            ObjectContext.Currencies.DeleteObject(record);
            
            ObjectContext.SaveChanges();
            X.Msg.Alert("Delete Success", "The selected currency conversion record/s have been successfully deleted").Show();
        }

        public bool CheckCurrencyIfActive(int currencyId)
        {
            //var rsm = this.PageGridPanelSelectionModel.SelectedRows;

            //var currencyId = int.Parse(rsm[0].RecordID);

            //return 1 if active else 0
            var paymentCurrencyAssoc = ObjectContext.PaymentCurrencyAssocs.Where(entity => entity.CurrencyId == currencyId);
            var covTransaction = ObjectContext.COVTransactions.Where(entity => entity.CurrencyId == currencyId);
            var exchangeRateFrom = ObjectContext.ExchangeRates.Where(entity => entity.CurrencyFromId == currencyId);
            var exchangeRateTo = ObjectContext.ExchangeRates.Where(entity => entity.CurrencyToId == currencyId);
            var originalForEx = ObjectContext.ForeignExchanges.Where(entity => entity.ReceivedCurrencyId == currencyId);
            var convertedForEx = ObjectContext.ForeignExchanges.Where(entity => entity.ReleasedCurrencyId == currencyId);

            if (paymentCurrencyAssoc.Count() == 0 && covTransaction.Count() == 0 && exchangeRateFrom.Count() == 0 && exchangeRateTo.Count() == 0
                && originalForEx.Count() == 0 && convertedForEx.Count() == 0)
            {
                return false;
            }

            return true;
        }

        protected void EditClick(object sender, DirectEventArgs e)
        {
            var rsm = this.PageGridPanelSelectionModel.SelectedRows;
            var id = int.Parse(rsm[0].RecordID);
            hdnRecordId.Text = id.ToString();
            var currency = Currency.GetCurrencyById(id);
            txtSymbol.Text = currency.Symbol;
            txtDescription.Text = currency.Description;
            wndAddEditCurrency.Show();
        }

        protected void AddClick(object sender, DirectEventArgs e)
        {
            hdnRecordId.Text = "Add";
            txtSymbol.Clear();
            txtDescription.Clear();
            wndAddEditCurrency.Show();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            if (hdnRecordId.Text == "Add")
            {
                if (Currency.GetCurrencyBySymbol(txtSymbol.Text) == null)
                {
                    Currency currency = Currency.CreateCurrency(txtSymbol.Text, txtDescription.Text);
                    ObjectContext.Currencies.AddObject(currency);
                    ObjectContext.SaveChanges();

                    X.Msg.Alert("Success", "Successfully added the currency record.", new JFunction("wndAddEditCurrency.hide(); PageGridPanel.reload();")).Show();
                }
                else
                {
                    X.Msg.Alert("Error", "Currency already exists. Please select another currency symbol.").Show();
                }
            }
            else
            {
                var currency = Currency.GetCurrencyById(int.Parse(hdnRecordId.Text));

                if (Currency.GetCurrencyBySymbol(txtSymbol.Text) == null || txtSymbol.Text == currency.Symbol)
                {
                    if (currency.Symbol != txtSymbol.Text) currency.Symbol = txtSymbol.Text;
                    if (currency.Description != txtDescription.Text) currency.Description = txtDescription.Text;
                    ObjectContext.SaveChanges();

                    X.Msg.Alert("Success", "Successfully updated the currency record.", new JFunction("wndAddEditCurrency.hide(); PageGridPanel.reload();")).Show();
                }
                else
                {
                    X.Msg.Alert("Error", "Currency already exists. Please select another currency symbol.").Show();
                }
            }
        }

        private class DataSource : IPageAbleDataSource<CurrencyViewModel>
        {
            private IEnumerable<CurrencyViewModel> CreateQuery()
            {
                var query = from c in ObjectContext.Currencies
                            select new CurrencyViewModel()
                            {
                                Id = c.Id,
                                Name = c.Symbol,
                                Description = c.Description
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

            public override List<CurrencyViewModel> SelectAll(int start, int limit, Func<CurrencyViewModel, string> orderBy)
            {
                var query = CreateQuery().ToList();
                query.OrderBy(orderBy).Skip(start).Take(limit);
                return query;
            }
        }
    }

    public class CurrencyViewModel
    {
        //public CurrencyConversion CurrencyCoversion { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}