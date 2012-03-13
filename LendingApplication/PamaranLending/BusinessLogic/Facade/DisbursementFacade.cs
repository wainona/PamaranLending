using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementFacade
    {
        public static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static void SaveToDisburseCash(DateTime transactdate, DateTime entrydate, Payment parent, string CheckNumber, int processedTo, int processedby, decimal AmountTodisburse, SpecificPaymentType specifictype, DisbursementType distype, string DisbursedToName, int CurrencyId)
        {
            Payment payment = SaveToDisburseCash(transactdate, entrydate, parent, processedTo, processedby, AmountTodisburse, specifictype, distype, DisbursedToName, CurrencyId);
            if (string.IsNullOrEmpty(CheckNumber) == false)
                payment.PaymentReferenceNumber = CheckNumber;

        }
        //with associated cheque but no currency
        public static void SaveToDisburseCash(DateTime transactdate, DateTime entrydate, Payment parent, string CheckNumber, int processedTo, int processedby, decimal AmountTodisburse, SpecificPaymentType specifictype, DisbursementType distype, string DisbursedToName)
        {
            Payment payment = SaveToDisburseCash(transactdate, entrydate, parent, processedTo, processedby, AmountTodisburse, specifictype, distype, DisbursedToName);
            if (string.IsNullOrEmpty(CheckNumber) == false)
                payment.PaymentReferenceNumber = CheckNumber;

        }
        // without currency and associated cheque
        public static Payment SaveToDisburseCash(DateTime transactdate, DateTime entrydate, Payment parent, int processedTo, int processedby, decimal AmountTodisburse, SpecificPaymentType specifictype, DisbursementType distype, string DisbursedToName)
        {
            Payment cashpayment = new Payment();
            cashpayment.PaymentMethodType = PaymentMethodType.CashType;
            cashpayment.Payment2 = parent;
            cashpayment.ProcessedToPartyRoleId = processedTo;
            cashpayment.ProcessedByPartyRoleId = processedby;

            cashpayment.TransactionDate = transactdate;
            cashpayment.EntryDate = entrydate;
            cashpayment.TotalAmount = AmountTodisburse;
            cashpayment.PaymentType = PaymentType.Disbursement;
            cashpayment.SpecificPaymentType = specifictype;
            ObjectContext.Payments.AddObject(cashpayment);

            Disbursement disbursement = new Disbursement();
            disbursement.Payment = cashpayment;
            disbursement.DisbursementType = distype;
            if (string.IsNullOrEmpty(DisbursedToName) == false)
                disbursement.DisbursedToName = DisbursedToName;
            ObjectContext.Disbursements.AddObject(disbursement);
            return cashpayment;
        }

        // With currency
        public static Payment  SaveToDisburseCash(DateTime transactdate, DateTime entrydate, Payment parent, int processedTo, int processedby, decimal AmountTodisburse, SpecificPaymentType specifictype,DisbursementType distype, string DisbursedToName, int CurrencyId)
        {
            Payment cashpayment = SaveToDisburseCash(transactdate, entrydate, parent, processedTo, processedby, AmountTodisburse, specifictype, distype, DisbursedToName);

            Currency.CreatePaymentCurrencyAssoc(cashpayment, CurrencyId);
            return cashpayment;

        }
      
        public static void SaveToDisbursementCheques(DateTime transactdate, DateTime entrydate, Payment parent, List<AddChequesModel> Cheques, int processedTo, int processedby, SpecificPaymentType specifictype, DisbursementType distype, string DisbursedToName, int CurrencyId)
        {
            foreach (var cheque in Cheques)
            {
                PaymentMethodType type = PaymentMethodType.PayCheckType;
                if (cheque.CheckType == PaymentMethodType.PersonalCheckType.Name)
                    type = PaymentMethodType.PersonalCheckType;
                Payment chequepayment = new Payment();
         
                chequepayment.PaymentMethodType = type;
                chequepayment.Payment2 = parent;
                chequepayment.ProcessedToPartyRoleId = processedTo;
                chequepayment.ProcessedByPartyRoleId = processedby;
                chequepayment.PaymentReferenceNumber = cheque.CheckNumber;
                chequepayment.TransactionDate = transactdate;
                chequepayment.EntryDate = entrydate;
                chequepayment.TotalAmount = cheque.Amount;
                chequepayment.PaymentType = PaymentType.Disbursement;
                chequepayment.SpecificPaymentType = specifictype;
                ObjectContext.Payments.AddObject(chequepayment);

                Disbursement disbursement = new Disbursement();
                disbursement.Payment = chequepayment;
                disbursement.DisbursementType = distype;
                if (string.IsNullOrEmpty(DisbursedToName) == false)
                    disbursement.DisbursedToName = DisbursedToName;
                ObjectContext.Disbursements.AddObject(disbursement);

                Cheque newCheque = new Cheque();
                newCheque.BankPartyRoleId = cheque.BankPartyRoleId;
                newCheque.Payment = chequepayment;
                newCheque.CheckDate = cheque.CheckDate;
                ObjectContext.Cheques.AddObject(newCheque);

                ChequeStatu newChequeStatus = new ChequeStatu();
                newChequeStatus.Cheque = newCheque;
                newChequeStatus.CheckStatusTypeId = ChequeStatusType.ClearedType.Id;
                newChequeStatus.TransitionDateTime = entrydate;
                newChequeStatus.IsActive = true;
                ObjectContext.ChequeStatus.AddObject(newChequeStatus);

                Currency.CreatePaymentCurrencyAssoc(chequepayment, CurrencyId);

            }
        }

        public static IEnumerable<AmountBreakdown> GetBreakdown(int parentPaymentId)
        {
              var cashBreakdown = from p in ObjectContext.Payments
                                where p.ParentPaymentId ==parentPaymentId
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
                                 where p.ParentPaymentId == parentPaymentId
                                &&( p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                                || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                 && p.PaymentTypeId == PaymentType.Disbursement.Id
                                 select new AmountBreakdown
                                 {
                                     PaymentMethod = p.PaymentMethodType.Name,
                                     TotalAmount = p.TotalAmount,
                                     CheckNumber = p.PaymentReferenceNumber,
                                    Payment = p
                                 };
            return cashBreakdown.Concat(checkBreakDown);
            
        }
    }

    public class AmountBreakdown
    {
        public string PaymentMethod { get; set; }
        public string BankBranch
        {
            get
            {
                if (this.Payment.Cheques.FirstOrDefault() != null)
                {
                    var bank = Bank.GetById(this.Payment.Cheques.FirstOrDefault().BankPartyRoleId);
                    if (bank != null && (string.IsNullOrEmpty(bank.Branch) ==false))
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
                if (this.Payment.Cheques.FirstOrDefault() != null)
                    return Organization.GetOrganizationName(this.Payment.Cheques.FirstOrDefault().BankPartyRoleId);
                else return "N/A";
            }
        }
        public string CheckNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public Payment Payment { get; set; }
       
    }
    public class AddChequesModel : BusinessObjectModel
    {
        public string BankName { get; set; }
        public int BankPartyRoleId { get; set; }
        public string Branch { get; set; }
        public string CheckType { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string _CheckDate { 
            get { 
               return CheckDate.ToString("MM/dd/yyyy");
            }
        }
        public decimal Amount { get; set; }
        public PaymentMethodType PaymentType
        {
            get
            {
                if (string.IsNullOrEmpty(CheckType) == false)
                {
                    if (CheckType == PaymentMethodType.PayCheckType.Name)
                        return PaymentMethodType.PayCheckType;
                    else return PaymentMethodType.PersonalCheckType;

                }
                else return PaymentMethodType.PayCheckType;

            }
        }

        protected static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public AddChequesModel()
        {
            this.IsNew = true;
        }

        public AddChequesModel(Cheque cheque)
        {
            this.IsNew = false;

            PartyRole partyRole = cheque.PartyRole;
            Party party = partyRole.Party;
            Organization bankName = party.Organization;
            this.BankName = bankName.OrganizationName;

            Bank bankBranch = partyRole.Bank;
            this.Branch = bankBranch.Branch;
            this.BankPartyRoleId = partyRole.Id;

            Payment checkPayment = cheque.Payment;
            this.CheckType = checkPayment.PaymentMethodType.Name;
            this.CheckNumber = checkPayment.PaymentReferenceNumber;
            this.Amount = checkPayment.TotalAmount;

            this.CheckDate = cheque.CheckDate;
        }

    }
}
