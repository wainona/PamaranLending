using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class OpenFeePayment : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
                return allowed;
            }
        }
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
            int id = int.Parse(Request.QueryString["id"]);
            hiddenPaymentId.Text = id.ToString();
            FillBreakDown(id);
            FillItems();
        }
        protected void FillBreakDown(int id)
        {
            var parentFeePayment = ObjectContext.Payments.FirstOrDefault(entity => entity.Id == id);
            txtTransactionDate.Text = parentFeePayment.TransactionDate.ToString();
            txtTotalFeeAmount.Text = parentFeePayment.TotalAmount.ToString(); ;
            txtReceivedFrom.Text = Person.GetPersonFullName((int)parentFeePayment.ProcessedToPartyRoleId);
            txtReceivedBy.Text = Person.GetPersonFullName(parentFeePayment.ProcessedByPartyRoleId);
   
            var breakdown = Payment.GetBreakdown(id, PaymentType.FeePayment);
            strBreakdown.DataSource = breakdown.ToList();
            strBreakdown.DataBind();
        }
        protected void RefreshItems(object sender, StoreRefreshDataEventArgs e)
        {

            FillItems();

        }
        private void FillItems()
        {
            int id = int.Parse(hiddenPaymentId.Text);
            var items = FeePayment.GetFeesById(id);
            FeeItems.DataSource = items;
            FeeItems.DataBind();
        }
    }
}