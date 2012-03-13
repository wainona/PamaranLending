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
    public partial class AddExchangeRate : ActivityPageBase
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
                storeExchangeRateType.DataSource = ObjectContext.ExchangeRateTypes.ToList();
                storeExchangeRateType.DataBind();
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

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            int currencyFromId = int.Parse(cmbCurrencyFrom.SelectedItem.Value);
            int currencyToId = int.Parse(cmbCurrencyTo.SelectedItem.Value);
            int exchangeRateTypeId = int.Parse(cmbExchangeRateType.SelectedItem.Value);
            if (ExchangeRate.ExchangeRateExists(currencyFromId, currencyToId, exchangeRateTypeId))
            {
                X.Msg.Alert("Error", "Exchange rate already exists. Please create a new exchange rate.").Show();
            }
            else
            {
                ExchangeRate.CreateExchangeRate(int.Parse(cmbCurrencyFrom.SelectedItem.Value), int.Parse(cmbCurrencyTo.SelectedItem.Value), decimal.Parse(txtRate.Text), DateTime.Today, int.Parse(cmbExchangeRateType.SelectedItem.Value));
                ObjectContext.SaveChanges();
                X.Msg.Alert("Success", "Successfully added an exchange rate record.", new JFunction("window.proxy.sendToAll('addexchangerate', 'addexchangerate'); window.proxy.requestClose();")).Show();
            }
        }
    }
}