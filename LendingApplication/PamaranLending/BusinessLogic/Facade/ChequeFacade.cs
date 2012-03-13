using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public class ChequesTransactionModel
    {
        public string ProcessedTo { get; set; }
        public string ProcessedBy { get; set; }
        public string CheckNumber { get; set; }
        public decimal Amount { get; set; }
        public string CurrencySymbol { get; set; }
        public string TransactionDate { get; set; }

        public ChequesTransactionModel(Payment payment)
        {
            this.ProcessedTo = Person.GetPersonFullName((int)payment.ProcessedToPartyRoleId);
            this.ProcessedBy = Person.GetPersonFullName(payment.ProcessedByPartyRoleId);
            this.CheckNumber = payment.PaymentReferenceNumber;
            this.Amount = payment.TotalAmount;
            this.CurrencySymbol = Currency.CurrencySymbolByPaymentId(payment.Id);
            this.TransactionDate = payment.TransactionDate.ToString("MMM dd,yyyy");
        }
    }
    public partial class Cheque
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

        public static Cheque GetById(int id)
        {
            return Context.Cheques.SingleOrDefault(entity => entity.Id == id);
        }

        public static Cheque GetByPaymentId(Payment payment)
        {
            return Context.Cheques.SingleOrDefault(entity => entity.PaymentId == payment.Id);
        }

        public static Cheque CreateCheque(Payment payment, int bankPartyRoleId, DateTime checkDate)
        {
            Cheque newCheck = new Cheque();
            newCheck.BankPartyRoleId = bankPartyRoleId;
            newCheck.CheckDate = checkDate;
            newCheck.PaymentId = payment.Id;
            return newCheck;
        }

        public ChequeStatu CurrentStatus
        {
            get
            {
                return this.ChequeStatus.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public static int ValidateCheckNumber(string inputCheckNumber)
        {
            int value = 0;
            var checkNumbers = Context.Payments;
            foreach (var item in checkNumbers)
            {
                if (inputCheckNumber == item.PaymentReferenceNumber)
                {
                    value = 1;
                    break;
                }
            }
            return value;
        }
        public static IEnumerable<ChequesTransactionModel> GetForeignChequesByCurrency(int currencyId, PaymentType paymentType, DateTime today)
        {
            var tomm = today.AddDays(1);

            var query = from chq in Context.Cheques
                        join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                            on chq.PaymentId equals pymnt.Id
                        join assoc in Context.PaymentCurrencyAssocs on pymnt.Id equals assoc.PaymentId
                        where pymnt.EntryDate >= today && pymnt.EntryDate < tomm && assoc.CurrencyId == currencyId
                        select pymnt;

            List<ChequesTransactionModel> chequesDisbursedList = new List<ChequesTransactionModel>();

            foreach (var item in query)
            {
                chequesDisbursedList.Add(new ChequesTransactionModel(item));
            }


            return chequesDisbursedList;
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
        public static IEnumerable<ChequesTransactionModel> GetAllCheques(PaymentType paymentType, DateTime today)
        {
            var tomm = today.AddDays(1);
            var query = from chq in Context.Cheques
                        join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                            on chq.PaymentId equals pymnt.Id
                        where pymnt.EntryDate >= today && pymnt.EntryDate < tomm
                        select pymnt;

            List<ChequesTransactionModel> chequesDisbursedList = new List<ChequesTransactionModel>();

            foreach (var item in query)
            {
                chequesDisbursedList.Add(new ChequesTransactionModel(item));
            }
            return chequesDisbursedList;
        }
        public static IEnumerable<ChequesTransactionModel> GetPhilippineCheques(PaymentType paymentType, DateTime today)
        {
            var philCurrency = Currency.GetCurrencyBySymbol("PHP").Id;
            var tomm = today.AddDays(1);
            var query = from chq in Context.Cheques
                        join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                            on chq.PaymentId equals pymnt.Id
                        where pymnt.EntryDate >= today && pymnt.EntryDate < tomm
                        select pymnt;

            var foreign = from chq in Context.Cheques
                          join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                              on chq.PaymentId equals pymnt.Id
                          join assoc in Context.PaymentCurrencyAssocs on pymnt.Id equals assoc.PaymentId
                          where pymnt.EntryDate >= today && pymnt.EntryDate < tomm && assoc.CurrencyId != philCurrency
                          select pymnt;

            var cancelledpostdatedcheques = from chq in Context.Cheques
                                            join pymnt in Context.Payments.Where(entity => entity.PaymentTypeId == paymentType.Id)
                                            on chq.PaymentId equals pymnt.Id
                                            join chqassoc in Context.ChequeApplicationAssocs on chq.Id equals chqassoc.ChequeId
                                            join las in Context.LoanApplicationStatus on chqassoc.ApplicationId equals las.ApplicationId
                                            where las.IsActive == true && (las.LoanApplicationStatusType.Id
                                            == LoanApplicationStatusType.RejectedType.Id
                                            || las.LoanApplicationStatusType.Id == LoanApplicationStatusType.CancelledType.Id)
                                            select pymnt;
            List<ChequesTransactionModel> chequesDisbursedList = new List<ChequesTransactionModel>();

            var result = query.Except(foreign);
            result = result.Except(cancelledpostdatedcheques);
            foreach (var item in result)
            {
                chequesDisbursedList.Add(new ChequesTransactionModel(item));
            }
            return chequesDisbursedList;

        }

        public static void ApplyPostdatedCheckAsPayment(DateTime entryDate, DateTime transactionDate,int receiptId, decimal InterestPayment, decimal LoanPayment, decimal TotalPayment)
        {

            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = Context.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
            var checkLoanAssoc = Context.ChequeLoanAssocs.SingleOrDefault(entity => entity.ChequeId == check.Id);
            var receipt = Receipt.GetById(receiptId);
            var tempReceiptBalance = receipt.ReceiptBalance;
          
            if (checkLoanAssoc != null && TotalPayment > 0)
            {
                var loanAccount = Context.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == checkLoanAssoc.FinancialAccountId);
                var agreement = loanAccount.FinancialAccount.Agreement;
                var amortizationSchedule = Context.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id && entity.EndDate == null);
                var amortizationScheduleItem = amortizationSchedule.AmortizationScheduleItems.SingleOrDefault(entity => entity.ScheduledPaymentDate == check.CheckDate);
                var agreementItem = agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
                var principalDue = amortizationScheduleItem.PrincipalPayment;

                Payment ParentPayment = Payment.CreatePayment(entryDate,
                                                       transactionDate,
                                                        check.Payment.ProcessedToPartyRoleId.Value,
                                                        check.Payment.ProcessedByPartyRoleId,
                                                        TotalPayment,
                                                        PaymentType.LoanPayment.Id,
                                                        PaymentMethodType.CashType.Id,
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        null);

                Context.Payments.AddObject(ParentPayment);
                Context.SaveChanges();
                var customerPartyRole = PartyRole.GetById(check.Payment.ProcessedToPartyRoleId.Value);
                LoanPayment newLoanPayment = Payment.CreateLoanPayment(ParentPayment, customerPartyRole);
                Context.LoanPayments.AddObject(newLoanPayment);

                //For Payment Breakdown
                Payment paymentBreakdown = new Payment();
                paymentBreakdown.Payment2 = ParentPayment;
                paymentBreakdown.ProcessedByPartyRoleId = check.Payment.ProcessedByPartyRoleId;
                paymentBreakdown.ProcessedToPartyRoleId = check.Payment.ProcessedToPartyRoleId;
                paymentBreakdown.PaymentType = PaymentType.LoanPayment;
                paymentBreakdown.PaymentMethodTypeId = check.Payment.PaymentMethodTypeId;
                paymentBreakdown.TransactionDate = transactionDate;
                paymentBreakdown.TotalAmount = TotalPayment;
                paymentBreakdown.EntryDate = entryDate;
                paymentBreakdown.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;
                Context.Payments.AddObject(paymentBreakdown);
                Context.SaveChanges();

                ReceiptPaymentAssoc assoc = new ReceiptPaymentAssoc();
                assoc.Receipt = receipt;
                assoc.Payment = paymentBreakdown;
                Context.ReceiptPaymentAssocs.AddObject(assoc);
                Context.SaveChanges();


                var finAcctTran = FinAcctTran.CreateFinAcctTran(loanAccount.FinancialAccountId, ParentPayment, transactionDate, entryDate, FinlAcctTransType.AccountPaymentType);
                Context.FinAcctTrans.AddObject(finAcctTran);
                Context.SaveChanges();

                //SaveInterestPayment
                SaveInterestPayment(entryDate, InterestPayment, loanAccount, ParentPayment);
                Context.SaveChanges();


                //SavePrincipalPayment
                LoanPayment = SaveLoanPayment(entryDate, LoanPayment, loanAccount, ParentPayment);

                //UpdateReceiptBalance and Status
                receipt.ReceiptBalance -= TotalPayment;
                var receiptStatus = ReceiptStatu.GetActive(receipt);
                if (receipt.ReceiptBalance == 0 && receiptStatus.ReceiptStatusType.Id != ReceiptStatusType.ClosedReceiptStatusType.Id)
                {
                    ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.ClosedReceiptStatusType, string.Empty);
                }
                else if (receipt.ReceiptBalance > 0 && receiptStatus.ReceiptStatusType.Id != ReceiptStatusType.AppliedReceiptStatusType.Id)
                {
                    ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.AppliedReceiptStatusType, string.Empty);
                }

                ControlNumberFacade.Create(FormType.PaymentFormType, ParentPayment);
                Context.SaveChanges();
            }
        }

        private static decimal SaveLoanPayment(DateTime entryDate, decimal LoanPayment, LoanAccount loanAccount, Payment ParentPayment)
        {
            if (LoanPayment > 0)
            {
                decimal amountapplied = 0;
                if (loanAccount.LoanBalance <= LoanPayment)
                    amountapplied = loanAccount.LoanBalance;
                else amountapplied = LoanPayment;
                PaymentApplication loanpaymentApplication = new PaymentApplication();
                loanpaymentApplication.Payment = ParentPayment;
                loanpaymentApplication.FinancialAccountId = loanAccount.FinancialAccountId;
                loanpaymentApplication.AmountApplied = amountapplied;
                Context.PaymentApplications.AddObject(loanpaymentApplication);


                loanAccount.LoanBalance -= amountapplied;
                LoanPayment -= amountapplied;
                LoanAccount.UpdateLoanStatus(entryDate, loanAccount, loanAccount.LoanBalance);


            }
            return LoanPayment;
        }

        private static void SaveInterestPayment(DateTime entryDate, decimal InterestPayment, LoanAccount loanAccount, Payment ParentPayment)
        {
            if (InterestPayment > 0)
            {
                var receivables = from r in Context.Receivables
                                  join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                                  where r.FinancialAccountId == loanAccount.FinancialAccountId
                                  && rs.IsActive == true && (rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
                                  || rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id)
                                  orderby r.ValidityPeriod
                                  select r;
                foreach (var receivable in receivables)
                {
                    if (InterestPayment > 0)
                    {
                        decimal amountapplied = 0;
                        if (receivable.Balance <= InterestPayment)
                            amountapplied = (decimal)receivable.Balance;
                        else if (receivable.Balance > InterestPayment)
                            amountapplied = InterestPayment;

                        PaymentApplication paymentApplication = new PaymentApplication();
                        paymentApplication.Payment = ParentPayment;
                        paymentApplication.ReceivableId = receivable.Id;
                        paymentApplication.AmountApplied = amountapplied;
                        Context.PaymentApplications.AddObject(paymentApplication);
                        receivable.Balance -= amountapplied;


                        Receivable.InsertReceivableStatus(receivable.Id, receivable.Balance, entryDate);

                        InterestPayment -= amountapplied;
                     

                    }
                    //Context.SaveChanges();

                }
            }
        }
    }
}


