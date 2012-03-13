using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.CashOnVaultUseCases
{
    public partial class CashOnVaultHistory : ActivityPageBase
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

        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                return allowed;
            }
        }

      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
             
                strCurrency.DataSource = Currency.GetCurrencies().ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = 0;
            }
        }

        [DirectMethod]
        public void FillCOVHistory()
        {
            var selectedCurId = int.Parse(cmbCurrency.SelectedItem.Value);
            Store.DataSource = CashOnVault.GetCOVHistory(selectedCurId).ToList();
            Store.DataBind();
        }

   
    }
}