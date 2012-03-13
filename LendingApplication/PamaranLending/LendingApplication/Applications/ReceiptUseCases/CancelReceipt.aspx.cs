using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ReceiptUseCases
{
    public partial class CancelReceipt : System.Web.UI.Page
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
                int selectedPaymentID= int.Parse(Request.QueryString["id"]);
                hdnReceiptID.Text = selectedPaymentID.ToString();
            }
        }

        protected void btnSave_onDirectEventClick(object sender, DirectEventArgs e)
        {
            int receiptId = int.Parse(hdnReceiptID.Text);
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var cancelledReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
            if(!EndCurrentReceiptStatus(receiptId, cancelledReceiptStatusTypeId)) return;
            
            ReceiptStatu newReceiptStatus = new ReceiptStatu();
            DateTime now = DateTime.Now;

            newReceiptStatus.TransitionDateTime = now;
            newReceiptStatus.IsActive = true;
            newReceiptStatus.ReceiptStatusTypeId = cancelledReceiptStatusTypeId;
            newReceiptStatus.ReceiptId = receiptId;
            newReceiptStatus.Remarks = txtComment.Text;

            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            if (check != null)
            {
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
                checkStatus.IsActive = false;

                ChequeStatu newCheckStatus = new ChequeStatu();

                newCheckStatus.CheckId = check.Id;
                newCheckStatus.ChequeStatusType = ChequeStatusType.CancelledType;
                newCheckStatus.TransitionDateTime = now;
                newCheckStatus.Remarks = txtComment.Text;
                newCheckStatus.IsActive = true;

                ObjectContext.ChequeStatus.AddObject(newCheckStatus);
            }

            ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
            ObjectContext.SaveChanges();
        }

        protected bool EndCurrentReceiptStatus(int selectedReceiptId, int receiptStatusTypeId)
        {
            var currentReceiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.IsActive == true && entity.ReceiptId == selectedReceiptId);
            var receiptStatusTypeAssoc = ObjectContext.ReceiptStatusTypeAssocs.Where(entity => entity.EndDate == null
                                            && entity.FromStatusTypeId == currentReceiptStatus.ReceiptStatusTypeId &&
                                            entity.ToStatusTypeId == receiptStatusTypeId);
            if (receiptStatusTypeAssoc == null)
            {
                return false;
            }

            currentReceiptStatus.IsActive = false;
            ObjectContext.SaveChanges();
            return true;
        }
    }
}