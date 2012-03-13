using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class ViewChange : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                allowed.Add("Cashier");
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
            
                var details = from d in ObjectContext.Disbursements
                              join p in ObjectContext.Payments on d.PaymentId equals p.Id
                              where d.PaymentId == id
                              select p;
                if(details.Count() != 0){
                    var disbursement = details.FirstOrDefault();
                    txtDateDisbursed.Text = disbursement.TransactionDate.ToString();

                    txtAmountDisbursed.Text = disbursement.TotalAmount.ToString("N");

                    var receipt = (from d in details
                                  join r in ObjectContext.ReceiptPaymentAssocs on d.Id equals r.PaymentId
                                  select r.Receipt).FirstOrDefault();
                    txtReceiptID.Text = receipt.Id.ToString();
                    var payment = (from r in ObjectContext.ReceiptPaymentAssocs
                                  join p in ObjectContext.Payments on r.PaymentId equals p.Id
                                  where p.PaymentTypeId == PaymentType.Receipt.Id && r.ReceiptId == receipt.Id
                                  select p).FirstOrDefault();

                    txtReceiptAmount.Text = payment.TotalAmount.ToString("N");
                    txtDisbursedBy.Text = Person.GetPersonFullName((int)disbursement.ProcessedByPartyRoleId);
                    txtDisbursedTo.Text = Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId);


                    var cashBreakdown = from p in ObjectContext.Payments
                                        where p.ParentPaymentId == id
                                        && p.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                        && p.PaymentTypeId == PaymentType.Disbursement.Id
                                        select new AmountBreakdown
                                        {
                                            PaymentMethod = p.PaymentMethodType.Name,
                                            TotalAmount = p.TotalAmount,
                                            CheckNumber = "N/A",
                                            Payment = p
                                        };
                    var checkBreakDown = from p in ObjectContext.Payments
                                         join c in ObjectContext.Cheques on p.Id equals c.PaymentId
                                         where p.ParentPaymentId == id
                                        && (p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                                        || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                         && p.PaymentTypeId == PaymentType.Disbursement.Id
                                         select new AmountBreakdown
                                         {
                                             PaymentMethod = p.PaymentMethodType.Name,
                                             TotalAmount = p.TotalAmount,
                                             CheckNumber = p.PaymentReferenceNumber,
                                             Payment = p
                                         };
                    checkBreakDown = checkBreakDown.Concat(cashBreakdown);
                    strBreakdown.DataSource = checkBreakDown.ToList();
                    strBreakdown.DataBind();
                                             
                }
        }
    }
}