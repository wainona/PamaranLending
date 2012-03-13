using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.ForeignExchangeApplication.ExchangeRateUseCases
{
    public partial class OpenExchangeRate : ActivityPageBase
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
                int selectedId = int.Parse(Request.QueryString["id"]);
                hdnExchangeRateId.Text = selectedId.ToString();
                StoreCurrenciesOnStore();
                RetrieveAndFill(selectedId);
                storeExchangeRateType.DataSource = ObjectContext.ExchangeRateTypes.ToList();
                storeExchangeRateType.DataBind();
                var exRate = ExchangeRate.GetById(selectedId);
                cmbExchangeRateType.SelectedItem.Value = exRate.ExchangeRateTypeId.ToString();
            }
        }

        protected void RetrieveAndFill(int recordId)
        {
            var er = ExchangeRate.GetById(recordId);
            cmbCurrencyFrom.SelectedItem.Value = er.CurrencyFromId.ToString();
            cmbCurrencyTo.SelectedItem.Value = er.CurrencyToId.ToString();
            txtRate.Text = er.Rate.ToString("N");
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            DateTime today = DateTime.Today;
            var currencyFromId = int.Parse(cmbCurrencyFrom.SelectedItem.Value);
            var currencyToId = int.Parse(cmbCurrencyTo.SelectedItem.Value);
            var exRateTypeId = int.Parse(cmbExchangeRateType.SelectedItem.Value);
            var recordId = int.Parse(hdnExchangeRateId.Text);
            var er = ExchangeRate.GetById(recordId);

            if (ExchangeRate.CompareExRateIfEqual(er, currencyFromId, currencyToId, exRateTypeId) || ExchangeRate.ExchangeRateExists(currencyFromId, currencyToId, exRateTypeId) == false)
            {
                //
                if (er.Date.Date != today)
                {
                    ExchangeRate.CreateExchangeRate(currencyFromId, currencyToId, decimal.Parse(txtRate.Text), today, exRateTypeId);
                    er.IsActive = false;
                }
                else
                {
                    if (er.CurrencyFromId != int.Parse(cmbCurrencyFrom.SelectedItem.Value))
                        er.CurrencyFromId = int.Parse(cmbCurrencyFrom.SelectedItem.Value);
                    if (er.CurrencyToId != int.Parse(cmbCurrencyTo.SelectedItem.Value))
                        er.CurrencyToId = int.Parse(cmbCurrencyTo.SelectedItem.Value);
                    if (er.Rate != decimal.Parse(txtRate.Text))
                        er.Rate = decimal.Parse(txtRate.Text);
                    if (er.ExchangeRateTypeId != int.Parse(cmbExchangeRateType.SelectedItem.Value))
                        er.ExchangeRateTypeId = int.Parse(cmbExchangeRateType.SelectedItem.Value);
                }

                ObjectContext.SaveChanges();
                X.Msg.Alert("Success", "Successfully updated the exchange rate record.", new JFunction("window.proxy.sendToAll('editexchangerate', 'editexchangerate'); cancelClick();")).Show();
            }
            else
            {
                X.Msg.Alert("Error", "Exchange rate already exists. Please create a new exchange rate.").Show();
            }
        }

        protected void GetAllCurrencies(object sender, EventArgs e)
        {
            StoreCurrenciesOnStore();
        }

        protected void StoreCurrenciesOnStore()
        {
            var currencyModel = from cur in ObjectContext.Currencies
                                select new CurrencyModel()
                                {
                                    Name = cur.Symbol,
                                    Desciption = cur.Description,
                                    Id = cur.Id
                                };
            storeCurrencyFrom.DataSource = currencyModel;
            storeCurrencyFrom.DataBind();
            storeCurrencyTo.DataSource = currencyModel;
            storeCurrencyTo.DataBind();
        }
    }
}