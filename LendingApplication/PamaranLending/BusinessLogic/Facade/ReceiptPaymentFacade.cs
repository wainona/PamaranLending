using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public class ReceiptPaymentFacade
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        public static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static List<Payment> RetrieveAssociatedPayments(int receiptId, PaymentType type)
        {
            var payments = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.ReceiptId == receiptId
                           select p;

            return payments.ToList();
        }
        public static Payment RetrieveAssociatedPayment(int receiptId, PaymentType type)
        {
            var payments = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.ReceiptId == receiptId
                           select p;

            return payments.First();
        }
        public static Payment RetrieveAssociatedPayment(Receipt receipt, PaymentType type)
        {
            var payments = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.ReceiptId == receipt.Id
                           select p;

            return payments.First();
        }

        public static List<Receipt> RetrieveAssociatedReceipts(int paymentId, PaymentType type)
        {
            var receipts = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.PaymentId == paymentId
                           select rpa.Receipt;

            return receipts.ToList();
        }

        public static List<Receipt> RetrieveAssociatedReceipts(Payment payment, PaymentType type)
        {
            var receipts = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.PaymentId == payment.Id
                           select rpa.Receipt;

            return receipts.ToList();
        }

        public static Receipt RetrieveAssociatedReceipt(int paymentId, PaymentType type)
        {
            var receipts = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.PaymentId == paymentId
                           select rpa.Receipt;

            return receipts.First();
        }

        public static Receipt RetrieveAssociatedReceipt(Payment payment, PaymentType type)
        {
            var receipts = from rpa in Context.ReceiptPaymentAssocs
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where p.PaymentTypeId == type.Id && rpa.PaymentId == payment.Id
                           select rpa.Receipt;

            return receipts.First();
        }

        public static IEnumerable<Receipt> RetrieveReceiptsOfCashPaymentMadeByCustomer(PartyRole customer)
        {
            var payments = from p in Context.Payments
                           where p.ProcessedToPartyRoleId == customer.Id
                           && p.PaymentTypeId == PaymentType.Receipt.Id
                           && (p.PaymentMethodTypeId == PaymentMethodType.CashType.Id ||
                            p.PaymentMethodTypeId == PaymentMethodType.ATMType.Id)
                           select p;

            var receipts = from p in payments
                           join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                           join r in Context.Receipts on rpa.ReceiptId equals r.Id
                           join rs in Context.ReceiptStatus on r.Id equals rs.ReceiptId
                           where rs.IsActive && r.ReceiptBalance > 0
                           && (rs.ReceiptStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id ||
                            rs.ReceiptStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id)
                           select r;

            return receipts;
        }

        public static IEnumerable<Cheque> RetrieveChequesOfCustomerWithBalance(PartyRole customer)
        {
            var clearedCheques = from c in Context.Cheques
                                 join cs in Context.ChequeStatus on c.Id equals cs.CheckId
                                 where cs.CheckStatusTypeId == ChequeStatusType.ClearedType.Id
                                 && cs.IsActive
                                 select c;

            var cheques = from c in clearedCheques
                          join p in Context.Payments on c.PaymentId equals p.Id
                          join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                          join r in Context.Receipts on rpa.ReceiptId equals r.Id
                          join rs in Context.ReceiptStatus on r.Id equals rs.ReceiptId
                          where p.ProcessedToPartyRoleId == customer.Id
                          && p.PaymentTypeId == PaymentType.Receipt.Id
                          && r.ReceiptBalance > 0
                          && ((rs.ReceiptStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id ||
                          rs.ReceiptStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id)
                          && rs.IsActive)
                          select c;

            return cheques;
        }

        public static List<ChequeViewModel> RetrieveChequeModelOfCustomerWithBalance(PartyRole customer)
        {
            var clearedCheques = from c in Context.Cheques
                                 join cs in Context.ChequeStatus on c.Id equals cs.CheckId
                                 where cs.CheckStatusTypeId == ChequeStatusType.ClearedType.Id
                                 && cs.IsActive
                                 select c;

            var results = from c in clearedCheques
                          join p in Context.Payments on c.PaymentId equals p.Id
                          join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                          join r in Context.Receipts on rpa.ReceiptId equals r.Id
                          join rs in Context.ReceiptStatus on r.Id equals rs.ReceiptId
                          where p.ProcessedToPartyRoleId == customer.Id
                          && p.PaymentTypeId == PaymentType.Receipt.Id
                          && r.ReceiptBalance > 0
                          && ((rs.ReceiptStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id ||
                          rs.ReceiptStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id)
                          && rs.IsActive)
                          select new { Cheque = c, Receipt = r };

            var collection = new List<ChequeViewModel>();
            foreach (var result in results)
            {
                var model = new ChequeViewModel(result.Cheque, result.Receipt);
                collection.Add(model);
            }

            return collection;
        }
    }

    public class ChequeViewModel
    {
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string CheckType { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string _CheckDate
        {
            get
            {
                return CheckDate.ToString("MM/dd/yyyy");
            }
        }
        public decimal? TotalAmount { get; set; }

        public ChequeViewModel(Cheque cheque, Receipt receipt)
        {
            this.BankName = cheque.PartyRole.Party.Name;
            this.Branch = cheque.PartyRole.Bank.Branch;
            this.CheckType = cheque.Payment.PaymentMethodType.Name;
            this.CheckNumber = cheque.Payment.PaymentReferenceNumber;
            this.CheckDate = cheque.CheckDate;
            this.TotalAmount = receipt.ReceiptBalance;
            //TODO:: Affected by change in payment table.
            //this.TotalAmount = c.Payment.Receipt.ReceiptBalance;
        }
    }
}
