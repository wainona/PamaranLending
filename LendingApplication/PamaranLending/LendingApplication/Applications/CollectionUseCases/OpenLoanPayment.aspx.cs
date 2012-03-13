using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.CollectionUseCases
{

    public partial class OpenLoanPayment : System.Web.UI.Page
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            var querylist = CreateQuery();
            PageGridPanelStore.DataSource = querylist;
            PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var paymentId = int.Parse(Request.QueryString["id"]);
                hiddenPaymentID.Text = paymentId.ToString();
            }
        }

        private IEnumerable<PaymentModel> CreateQuery()
        {
            int paymentId = int.Parse(hiddenPaymentID.Text);
            Payment loanPayment = Payment.GetById(paymentId);
            var model = OpenLoanPaymentModel.Create(loanPayment);
            IEnumerable<PaymentModel> payments = model.CashReceiptsFromPreviousTransaction.Concat(
                model.Cheques).Concat(model.CashPayments).Concat(model.ATMPayments);
            return payments;
        }

        public class OpenLoanPaymentModel
        {
            public List<PaymentModel> CashReceiptsFromPreviousTransaction { get; set; }
            public List<PaymentModel> Cheques { get; set; }
            public List<PaymentModel> CashPayments { get; set; }
            public List<PaymentModel> ATMPayments { get; set; }

            private static FinancialEntities Context
            {
                get
                {
                    if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                        return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                    else
                        return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
                }
            }

            public static OpenLoanPaymentModel Create(Payment loanPayment)
            {
                if (loanPayment.LoanPayment == null)
                    throw new ArgumentException("Please supply the parent of the payments.");

                var customer = PartyRole.GetById(loanPayment.ProcessedToPartyRoleId.Value);
                var teller = PartyRole.GetById(loanPayment.ProcessedByPartyRoleId);
                var district = Context.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == customer.Id);

                OpenLoanPaymentModel model = new OpenLoanPaymentModel();
                model.Cheques = GetListOfCheques(loanPayment);
                model.ATMPayments = GetATMPayments(loanPayment);
                FillListOfCashReceiptsUsed(loanPayment, model);
                return model;
            }

            private static List<PaymentModel> GetListOfCheques(Payment loanPayment)
            {
                var chequeReceiptPayments = from p in Context.Payments
                                            join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                                            where p.ParentPaymentId == loanPayment.Id
                                            && (p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                                            || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                            && p.PaymentTypeId == PaymentType.LoanPayment.Id
                                            select rpa;

                var customerChequePayments = from crp in chequeReceiptPayments
                                             join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                             join p in Context.Payments on rpa.PaymentId equals p.Id
                                             join c in Context.Cheques on p.Id equals c.PaymentId
                                             where p.PaymentTypeId == PaymentType.Receipt.Id
                                             select new { ActualPayment = crp.Payment, ReceivePayment = p, Receipt = crp.Receipt, Cheque = c };

                var models = new List<PaymentModel>();
                foreach (var chequePayment in customerChequePayments)
                {
                    var bankPartyRole = PartyRole.GetById(chequePayment.Cheque.BankPartyRoleId);
                    Bank bank = Bank.GetById(bankPartyRole.Id);
                    PaymentModel model = new PaymentModel();
                    model.PaymentMethod = "Cheque";
                    model.BankName = bankPartyRole.Party.Name;
                    model.BankBranch = bank.Branch;
                    model.CheckNumber = chequePayment.ReceivePayment.PaymentReferenceNumber;
                    model.TransactionDate = chequePayment.Cheque.CheckDate;
                    model.AmountTendered = chequePayment.ReceivePayment.TotalAmount;
                    model.AmountApplied = chequePayment.ActualPayment.TotalAmount;
                    models.Add(model);
                }

                return models;
            }
            private static List<PaymentModel> GetATMPayments(Payment loanPayment)
            {
                var atmReceiptPayments = from p in Context.Payments
                                            join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                                            where p.ParentPaymentId == loanPayment.Id
                                            && (p.PaymentMethodTypeId == PaymentMethodType.ATMType.Id)
                                            && p.PaymentTypeId == PaymentType.LoanPayment.Id
                                            select rpa;

                var customerAtmPayments = from crp in atmReceiptPayments
                                             join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                             join p in Context.Payments on rpa.PaymentId equals p.Id
                                             where p.PaymentTypeId == PaymentType.Receipt.Id
                                  select new { ActualPayment = crp.Payment, ReceivePayment = p, Receipt = crp.Receipt };

                var models = new List<PaymentModel>();
                foreach (var atmPayment in customerAtmPayments)
                {
             
                    PaymentModel model = new PaymentModel();
                    model.PaymentMethod = atmPayment.ActualPayment.PaymentMethodType.Name;
                    model.BankName = "NA";
                    model.BankBranch = "NA";
                    model.CheckNumber = atmPayment.ReceivePayment.PaymentReferenceNumber;
                    model.TransactionDate = atmPayment.ReceivePayment.TransactionDate;
                    model.AmountTendered = atmPayment.ReceivePayment.TotalAmount;
                    model.AmountApplied = atmPayment.ActualPayment.TotalAmount;
                    models.Add(model);
                }

                return models;
            }

            private static void FillListOfCashReceiptsUsed(Payment loanPayment, OpenLoanPaymentModel loanPaymentModel)
            {
                var cashPayments = from p in Context.Payments
                                   join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                                   where p.ParentPaymentId == loanPayment.Id
                                   && p.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                   && p.PaymentTypeId == PaymentType.LoanPayment.Id
                                   select rpa;

                var customerCashPayments = from crp in cashPayments
                                           join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                           join p in Context.Payments on rpa.PaymentId equals p.Id
                                           where p.PaymentTypeId == PaymentType.Receipt.Id
                                           && p.TransactionDate < loanPayment.TransactionDate
                                           select new { ActualPayment = crp.Payment, ReceivePayment = p, Receipt = crp.Receipt };

                var customerCashPaymentsToday = from crp in cashPayments
                                                join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                                join p in Context.Payments on rpa.PaymentId equals p.Id
                                                where p.PaymentTypeId == PaymentType.Receipt.Id
                                                && p.TransactionDate == loanPayment.TransactionDate
                                                select new { ActualPayment = crp.Payment, ReceivePayment = p, Receipt = crp.Receipt };

                var modelsFromPreviousReceipts = new List<PaymentModel>();
                foreach (var cashPayment in customerCashPayments)
                {
                    PaymentModel paymentCash = new PaymentModel();
                    paymentCash.PaymentMethod = "Cash Previous Receipt";
                    paymentCash.BankName = "NA";
                    paymentCash.BankBranch = "NA";
                    paymentCash.CheckNumber = "NA";
                    paymentCash.TransactionDate = cashPayment.ReceivePayment.TransactionDate;
                    paymentCash.AmountTendered = cashPayment.Receipt.ReceiptBalance.Value + cashPayment.ActualPayment.TotalAmount;
                    paymentCash.AmountApplied = cashPayment.ActualPayment.TotalAmount;
                    modelsFromPreviousReceipts.Add(paymentCash);
                }

                loanPaymentModel.CashReceiptsFromPreviousTransaction = modelsFromPreviousReceipts;

                var models = new List<PaymentModel>();
                foreach (var cashPayment in customerCashPaymentsToday)
                {
                    PaymentModel paymentCash = new PaymentModel();
                    paymentCash.PaymentMethod = "Cash";
                    paymentCash.BankName = "NA";
                    paymentCash.BankBranch = "NA";
                    paymentCash.CheckNumber = "NA";
                    paymentCash.TransactionDate = cashPayment.ReceivePayment.TransactionDate;
                    paymentCash.AmountTendered = cashPayment.Receipt.ReceiptBalance.Value + cashPayment.ActualPayment.TotalAmount;
                    paymentCash.AmountApplied = cashPayment.ActualPayment.TotalAmount;
                    models.Add(paymentCash);
                }

                loanPaymentModel.CashPayments = models;
            }
        }
    }
}