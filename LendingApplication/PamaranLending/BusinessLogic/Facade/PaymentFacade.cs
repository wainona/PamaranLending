using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Payment
    {
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
        public static IEnumerable<PaymentBreakdown> GetBreakdown(int parentPaymentId, PaymentType paymenttype)
        {
            var Payments = from p in Context.Payments
                           where p.ParentPaymentId == parentPaymentId
                           && p.PaymentTypeId == paymenttype.Id
                           select p;
            if (Payments.Count() == 0)
            {
                var parentPayment = from p in Context.Payments
                                    where p.Id == parentPaymentId
                                    select new PaymentBreakdown
                                    {
                                        PaymentMethod = p.PaymentMethodType.Name,
                                        TotalAmount = p.TotalAmount,
                                        CheckNumber = "N/A",
                                        Payment = p
                                    };
                return parentPayment;
            }
            else
            {

                var cashBreakdown = from p in Payments
                                    where p.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                    select new PaymentBreakdown
                                    {
                                        PaymentMethod = p.PaymentMethodType.Name,
                                        TotalAmount = p.TotalAmount,
                                        CheckNumber = "N/A",
                                        Payment = p
                                    };
                var atmBreakdown = from p in Payments
                                   where p.PaymentMethodTypeId == PaymentMethodType.ATMType.Id
                                   select new PaymentBreakdown
                                   {
                                       PaymentMethod = p.PaymentMethodType.Name,
                                       TotalAmount = p.TotalAmount,
                                       CheckNumber = p.PaymentReferenceNumber,
                                       Payment = p
                                   };

                var checkBreakDown = from p in Payments
                                     where (p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                                    || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                     select new PaymentBreakdown
                                     {
                                         PaymentMethod = p.PaymentMethodType.Name,
                                         TotalAmount = p.TotalAmount,
                                         CheckNumber = p.PaymentReferenceNumber,
                                         Payment = p
                                     };
                var breakdown = cashBreakdown.Concat(atmBreakdown).Concat(checkBreakDown);
                return breakdown;
            }

        }
        public class PaymentBreakdown
        {
            public string PaymentMethod { get; set; }
            public string CurrencySymbol
            {
                get
                {
                    if (this.Payment != null)
                    {
                        return Currency.CurrencySymbolByPaymentId(PaymentId);
                    }
                    else return "PHP";
                }
            }
            public int PaymentId
            {
                get
                {
                    return Payment.Id;
                }
            }
            public Cheque Cheque
            {
                get
                {
                    if (this.Payment != null)
                    {
                        var receipt = Context.ReceiptPaymentAssocs.FirstOrDefault(e => e.PaymentId == Payment.Id).Receipt;
                        if (receipt != null)
                        {
                            var cheque = from rp in Context.ReceiptPaymentAssocs.Where(e => e.ReceiptId == receipt.Id)
                                         join p in Context.Payments on rp.PaymentId equals p.Id
                                         join c in Context.Cheques on p.Id equals c.PaymentId
                                         where p.PaymentTypeId == PaymentType.Receipt.Id
                                         select c;
                            if (cheque.Count() != 0)
                            {
                                return cheque.FirstOrDefault();
                            }
                            else return null;
                        }
                        else return null;

                    }
                    else return null;
                }
            }
            public string BankBranch
            {
                get
                {
                    if (this.Cheque != null)
                    {
                        var bank = Bank.GetById(Cheque.BankPartyRoleId);
                        if (bank != null && (string.IsNullOrEmpty(bank.Branch) == false))
                            return bank.Branch;
                        else return "N/A";
                    }
                    else return "N/A";
                }
            }
            public string BankName
            {
                get
                {
                    if (this.Cheque != null)
                        return Organization.GetOrganizationName(Cheque.BankPartyRoleId);
                    else return "N/A";
                }
            }
            public string CheckNumber { get; set; }
            public decimal TotalAmount { get; set; }
            public Payment Payment { get; set; }

        }
        public static Payment GetById(int id)
        {
            return Context.Payments.SingleOrDefault(entity => entity.Id == id);
        }
        public static IEnumerable<Payment> GetChildrenByParentId(int id)
        {
            return Context.Payments.Where(e => e.ParentPaymentId == id);
        }

        public static Payment GetReceiptPayment(Receipt receipt)
        {
            var payments = from r in Context.Receipts
                           join rpa in Context.ReceiptPaymentAssocs on r.Id equals rpa.ReceiptId
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where r.Id == receipt.Id && p.PaymentTypeId == PaymentType.Receipt.Id && p.ParentPaymentId == null
                           select p;
            return payments.First();
        }

        public static IEnumerable<ChequesTransactionModel> GetAllForeignCheques(PaymentType paymentType, DateTime today)
        {
            var tomm = today.AddDays(1);
            var philCurrency = Currency.GetCurrencyBySymbol("PHP").Id;
            var query = from chq in Context.Cheques
                        join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                            on chq.PaymentId equals pymnt.Id
                        join assoc in Context.PaymentCurrencyAssocs on pymnt.Id equals assoc.PaymentId
                        where pymnt.EntryDate >= today && pymnt.EntryDate < tomm && assoc.CurrencyId != philCurrency
                        select pymnt;

            List<ChequesTransactionModel> chequesDisbursedList = new List<ChequesTransactionModel>();

            foreach (var item in query)
            {
                chequesDisbursedList.Add(new ChequesTransactionModel(item));
            }
            return chequesDisbursedList;
        }

        public static Payment GetReceiptPayment(int receiptId)
        {
            var payments = from r in Context.Receipts
                           join rpa in Context.ReceiptPaymentAssocs on r.Id equals rpa.ReceiptId
                           join p in Context.Payments on rpa.PaymentId equals p.Id
                           where r.Id == receiptId && p.PaymentTypeId == PaymentType.Receipt.Id && p.ParentPaymentId == null
                           select p;
            return payments.First();
        }
        public static LoanPayment CreateLoanPayment(Payment payment, PartyRole customerPartyRole)
        {
            decimal OwnerLoanBalance = 0;
            decimal OwnerInterest = 0;
            decimal CoOwnerBalance = 0;
            decimal CoOwnerInterest = 0;

            var ownerLoans = from farc in Context.FinancialAccountRoles
                               join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                               join ls in Context.LoanAccountStatus on farc.FinancialAccountId equals ls.FinancialAccountId
                               where pr.PartyId == customerPartyRole.PartyId
                               && pr.EndDate == null
                               && (pr.RoleTypeId == RoleType.OwnerFinancialType.Id)
                               && ls.IsActive == true && (ls.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                               && ls.StatusTypeId != LoanAccountStatusType.PaidOffType.Id)
                               select ls.LoanAccount;
            if (ownerLoans.Count() != 0)
                OwnerLoanBalance = ownerLoans.Sum(entity => entity.LoanBalance);

            var coOwnerLoans = from farc in Context.FinancialAccountRoles
                               join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                               join ls in Context.LoanAccountStatus on farc.FinancialAccountId equals ls.FinancialAccountId
                               where pr.PartyId == customerPartyRole.PartyId
                               && pr.EndDate == null
                               && (pr.RoleTypeId == RoleType.CoOwnerFinancialType.Id
                               || pr.RoleTypeId == RoleType.GuarantorFinancialType.Id)
                               && ls.IsActive == true && (ls.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                               && ls.StatusTypeId != LoanAccountStatusType.PaidOffType.Id)
                               select ls.LoanAccount;

            if (coOwnerLoans.Count() != 0)
                CoOwnerBalance = coOwnerLoans.Sum(entity => entity.LoanBalance);
           

            var ownerReceivables = from l in ownerLoans
                              join r in Context.Receivables on l.FinancialAccountId equals r.FinancialAccountId
                              join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                              where rs.IsActive == true && r.Balance > 0 && (rs.StatusTypeId == ReceivableStatusType.OpenType.Id
                              || rs.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id)
                              select r;

            if (ownerReceivables.Count() != 0)
                OwnerInterest = ownerReceivables.Sum(entity => entity.Balance);

            var coOwnerReceivables = from l in coOwnerLoans
                                   join r in Context.Receivables on l.FinancialAccountId equals r.FinancialAccountId
                                   join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                                   where rs.IsActive == true && r.Balance > 0 && (rs.StatusTypeId == ReceivableStatusType.OpenType.Id
                                   || rs.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id)
                                   select r;

            if (coOwnerReceivables.Count() != 0)
                CoOwnerInterest = coOwnerReceivables.Sum(entity => entity.Balance);
            
            LoanPayment loanpayment = new LoanPayment();
            loanpayment.Payment = payment;
            loanpayment.OwnerOutstandingLoan = OwnerLoanBalance;
            loanpayment.OwnerOutstandingInterest = OwnerInterest;
            loanpayment.CoOwnerOutstandingLoan = CoOwnerBalance;
            loanpayment.CoOwnerOutstandingInterest = CoOwnerInterest;
            return loanpayment;
        }

        public static Payment CreatePayment(DateTime entryDate,
                                     DateTime transactionDate,
                                     int processedToPartyRoleId, 
                                     int processedByPartyRoleId,
                                     decimal totalAmount,
                                     PaymentType paymentType,
                                     PaymentMethodType paymentMethodType,
                                     SpecificPaymentType specificPaymentType,
                                     string paymentReferenceNumber)
        {
            Payment newPayment = new Payment();
            newPayment.EntryDate = entryDate;
            newPayment.TransactionDate = transactionDate;
            newPayment.ProcessedToPartyRoleId = processedToPartyRoleId;
            newPayment.ProcessedByPartyRoleId = processedByPartyRoleId;//partytroleID of teller
            newPayment.TotalAmount = totalAmount;
            newPayment.PaymentTypeId = paymentType.Id;
            newPayment.PaymentMethodTypeId = paymentMethodType.Id;
            newPayment.SpecificPaymentTypeId = specificPaymentType.Id;
            newPayment.PaymentReferenceNumber = paymentReferenceNumber;

            return newPayment;
        }

        public static Payment CreatePayment(DateTime entryDate,
                                     DateTime transactionDate,
                                     int processedToPartyRoleId,
                                     int processedByPartyRoleId,
                                     decimal totalAmount,
                                     int paymentTypeId,
                                     int paymentMethodTypeId,
                                     int specificPaymentTypeId,
                                     string paymentReferenceNumber)
        {
            Payment newPayment = new Payment();
            newPayment.EntryDate = entryDate;
            newPayment.TransactionDate = transactionDate;
            newPayment.ProcessedToPartyRoleId = processedToPartyRoleId;
            newPayment.ProcessedByPartyRoleId = processedByPartyRoleId;//partytroleID of teller
            newPayment.TotalAmount = totalAmount;
            newPayment.PaymentTypeId = paymentTypeId;
            newPayment.PaymentMethodTypeId = paymentMethodTypeId;
            newPayment.SpecificPaymentTypeId = specificPaymentTypeId;
            newPayment.PaymentReferenceNumber = paymentReferenceNumber;

            return newPayment;
        }

        public static Payment CreatePayment(DateTime entryDate,
                                     DateTime transactionDate,
                                     int processedToPartyRoleId,
                                     int processedByPartyRoleId,
                                     decimal totalAmount,
                                     int paymentTypeId,
                                     int paymentMethodTypeId,
                                     int specificPaymentTypeId,
                                     int parentPaymentId,
                                     string paymentReferenceNumber)
        {
            Payment newPayment = new Payment();
            newPayment.EntryDate = entryDate;
            newPayment.TransactionDate = transactionDate;
            newPayment.ProcessedToPartyRoleId = processedToPartyRoleId;
            newPayment.ProcessedByPartyRoleId = processedByPartyRoleId;//partytroleID of teller
            newPayment.TotalAmount = totalAmount;
            newPayment.PaymentTypeId = paymentTypeId;
            newPayment.PaymentMethodTypeId = paymentMethodTypeId;
            newPayment.SpecificPaymentTypeId = specificPaymentTypeId;
            newPayment.ParentPaymentId = parentPaymentId;
            newPayment.PaymentReferenceNumber = paymentReferenceNumber;

            return newPayment;
        }
    }
}