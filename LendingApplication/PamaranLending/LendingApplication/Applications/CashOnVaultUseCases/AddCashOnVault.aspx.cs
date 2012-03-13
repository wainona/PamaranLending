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
    public partial class AddCashOnVault : ActivityPageBase
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
                txtTransactionDate.SelectedDate = DateTime.Today;
                txtTransactionDate.MinDate = DateTime.Today;

                strCOVTransType.DataSource = CashOnVault.GetCOVTransTypes().ToList();
                strCOVTransType.DataBind();
                cmbCOVTransType.SelectedIndex = 0;
             
            }
        }

        protected void btnAdd_Click(object sender, DirectEventArgs e)
        {
            int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            decimal amount = decimal.Parse(txtAmount.Text);
            int currId = int.Parse(cmbCurrency.SelectedItem.Value);
            int covTransType = int.Parse(cmbCOVTransType.SelectedItem.Value);
            string remarks = txtRemarks.Text;
            DateTime date = txtTransactionDate.SelectedDate;
            CashOnVault.CreateCOVTrans(partyroleid, amount, currId, covTransType, remarks, date);
            ObjectContext.SaveChanges();
           
        }
    }
}