using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class OtherDisbursementView : ActivityPageBase
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
            hiddenDisbursementID.Text = id.ToString();

            var details = ObjectContext.OtherDisbursementDetails.FirstOrDefault(entity => entity.PaymentId == id);
            var totalAmountDisbursed = ObjectContext.DisbursementViewLists.FirstOrDefault(entity => entity.DisbursementId == id && entity.DisbursementTypeId == DisbursementType.OtherLoanDisbursementType.Id);
                txtTotalAmount.Text = totalAmountDisbursed.Amount.ToString("N");
                txtDateDisbursed.Text = details.Date_Disbursed.ToString();
                txtDisbursedTo.Text = details.DisbursedTo;
                txtDisbursedby.Text = details.DisbursedBy;
                txtTotalAmountDisbursed.Text = totalAmountDisbursed.Amount.ToString("N");
                PaymentId.Text = id.ToString();

                var breakdown = DisbursementFacade.GetBreakdown(id);
                strBreakdown.DataSource = breakdown.ToList();
                strBreakdown.DataBind();
               
          
        }
        protected void RefreshItems(object sender, StoreRefreshDataEventArgs e)
        {
          
                int id  = int.Parse(PaymentId.Text);
                var items = ObjectContext.DisbursementItems.Where(entity => entity.PaymentId == id).ToList();
                OtherDisbursementItems.DataSource = items;
                OtherDisbursementItems.DataBind();
           
        }
    }
}