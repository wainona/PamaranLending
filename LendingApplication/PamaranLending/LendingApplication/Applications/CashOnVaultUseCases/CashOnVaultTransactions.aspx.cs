using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.CashOnVaultUseCases
{
    public partial class CashOnVaultTransactions : ActivityPageBase
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
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                strCurrency.DataSource = Currency.GetCurrencies().ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = 0;
            }
        }

        [DirectMethod]
        public void FillCOVHistory()
        {
            var selectedCurId = int.Parse(cmbCurrency.SelectedItem.Value);
            Store.DataSource = CashOnVault.GetCOVTransactions(selectedCurId);
            Store.DataBind();
        }


    }
}