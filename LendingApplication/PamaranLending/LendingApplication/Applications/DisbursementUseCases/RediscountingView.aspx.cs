using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class RediscountingView : ActivityPageBase
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
            Fill(id);
        }

        protected void Fill(int id)
        {
            var disburseRecord = (from d in ObjectContext.Disbursements
                                  join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                  join sr in ObjectContext.Encashments on d.PaymentId equals sr.PaymentId
                                  where d.PaymentId == id && d.DisbursementTypeId == DisbursementType.RediscountingType.Id
                                  select new { p, sr }).FirstOrDefault();

            txtAmountDisbursed.Text = disburseRecord.p.TotalAmount.ToString("N");
            txtDateDisbursed.Text = disburseRecord.p.TransactionDate.ToString();
            txtDisbursedTo.Text = Person.GetPersonFullName((int)disburseRecord.p.ProcessedToPartyRoleId);
            txtDisbursedBy.Text = Person.GetPersonFullName(disburseRecord.p.ProcessedByPartyRoleId);
            txtSuchargeFee.Text = disburseRecord.sr.Surcharge.ToString("N");

            var disbursement = disburseRecord.p.Disbursement;
            if (disbursement.DisbursedToName != null)
                txtReceivedBy.Text = disbursement.DisbursedToName;
            else txtReceivedBy.Text = Person.GetPersonFullName((int)disburseRecord.p.ProcessedToPartyRoleId);
            var currency = Currency.CurrencySymbolByPaymentId(disburseRecord.sr.PaymentId);
                txtCurrency1.Text = currency;
                txtCurrency2.Text = currency;
                txtCurrency3.Text = currency;
         
            var receiptPaymentRecord = (from rpa in ObjectContext.ReceiptPaymentAssocs
                                        join r in ObjectContext.Receipts on rpa.ReceiptId equals r.Id
                                        where rpa.PaymentId == disburseRecord.p.Id
                                        select new { r, rpa }).FirstOrDefault();

            var cheque = (from rpa in ObjectContext.ReceiptPaymentAssocs
                          join p in ObjectContext.Payments on rpa.PaymentId equals p.Id
                          join c in ObjectContext.Cheques on p.Id equals c.PaymentId
                          where p.PaymentTypeId == PaymentType.Receipt.Id && p.SpecificPaymentTypeId == SpecificPaymentType.CheckForRediscountingType.Id
                          && rpa.ReceiptId == receiptPaymentRecord.r.Id
                          select c).FirstOrDefault();
            txtBank.Text = Organization.GetOrganizationName(cheque.BankPartyRoleId);
            txtCheckNumber.Text = cheque.Payment.PaymentReferenceNumber.ToString();
            txtChkDate.Text = cheque.CheckDate.ToString();
            txtChkAmount.Text = cheque.Payment.TotalAmount.ToString("N");

            var breakdown = DisbursementFacade.GetBreakdown(receiptPaymentRecord.rpa.PaymentId);
            strBreakdown.DataSource = breakdown.ToList();
            strBreakdown.DataBind();
        }
    }
}