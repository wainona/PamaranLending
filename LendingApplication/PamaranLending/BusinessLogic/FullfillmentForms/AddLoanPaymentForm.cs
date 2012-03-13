 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class ReceivableInfo
    {
        public int ReceivableId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string _DueDate
        {
            get
            {
                return DueDate.ToString("MMM yyyy");
            }
        }
    }

    public class LoanPaymentModel : BusinessObjectModel
    {
        public int LoanID { get; set; }
        public string LoanProductName { get; set; }
        public int LoanProductID { get; set; }
        public decimal PrincipalDue { get; set; }
        public decimal InterestDue { get; set; }
        public decimal PastDue { get; set; }
        public decimal TotalAmountDue { get; set; }
        public decimal PaymentAmount { get; set; }
        public int PaymentID { get; set; }
        public string InterestComputationMode { get; set; }
        public string MethodOfCharging { get; set; }
        public decimal ComputedInterest { get; set; }
        public decimal WaiveInterest { get; set; }
        public decimal WaivePastDue { get; set; }
        public decimal WaivePrincipalDue { get; set; }
        public decimal RebateInterest { get; set; }
        public decimal RebatePrincipalDue { get; set; }

        public LoanPaymentModel()
        {
            PastDueReceivables = new List<ReceivableInfo>();
            InterestReceivables = new List<ReceivableInfo>();
        }

        public List<ReceivableInfo> PastDueReceivables { get; private set; }
        public List<ReceivableInfo> InterestReceivables { get; private set; }

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

        public void ComputeTotals()
        {
            this.PastDue = 0;
            this.InterestDue = 0;
            this.PrincipalDue = 0;

            var loanAccount = Context.LoanAccounts.FirstOrDefault(entity =>
                entity.FinancialAccountId == LoanID);

            if (loanAccount != null)
                this.PrincipalDue = loanAccount.LoanBalance;

            if (PastDueReceivables != null && PastDueReceivables.Count() > 0)
            {
                decimal pastDue = PastDueReceivables.Sum(entity => entity.Amount);
                this.PastDue = pastDue;
            }
            if (InterestReceivables != null && InterestReceivables.Count() > 0)
            {
                decimal interestdue = InterestReceivables.Sum(entity => entity.Amount);
                this.InterestDue = interestdue;
            }

            this.TotalAmountDue = PrincipalDue + InterestDue + PastDue;
        }

        public void UpdateAll()
        {
            ComputeTotals();
            this.PrincipalDue -= this.RebatePrincipalDue;
            this.PrincipalDue -= this.WaivePrincipalDue;
            this.InterestDue -= this.WaiveInterest;
            this.InterestDue -= this.RebateInterest;
            this.PastDue -= this.WaivePastDue;
            this.TotalAmountDue = this.InterestDue + this.PastDue + this.PrincipalDue;
        }

    }

    public class ItemsModel : BusinessObjectModel
    {
        public decimal PrincipalDue { get; set; }
        public decimal ReceivableInterestDue { get; set; }
        public decimal ReceivablePastDue { get; set; }
        public decimal ReceivableTotalLoadBalance { get; set; }
        public decimal WaivedAmountInterestDue { get; set; }
        public decimal WaivedPastDue { get; set; }
        public decimal WaivedTotalLoadBalance { get; set; }

        public ItemsModel(decimal principalDue, decimal receivableInterestDue, decimal receivablePastDue,
            decimal receivableTotalLoadBalance, decimal waivedAmountInterestDue, decimal waivedPastDue,
            decimal waivedTotalLoadBalance)
        {
            this.PrincipalDue = principalDue;
            this.ReceivableInterestDue = receivableInterestDue;
            this.ReceivablePastDue = receivablePastDue;
            this.ReceivableTotalLoadBalance = receivableTotalLoadBalance;
            this.WaivedAmountInterestDue = waivedAmountInterestDue;
            this.WaivedPastDue = waivedPastDue;
            this.WaivedTotalLoadBalance = waivedTotalLoadBalance;
        }
    }

    public class ManualBillingModel : BusinessObjectModel
    {
        public DateTime ScheduledPaymentDate { get; set; }
        public decimal PrincipalPayment { get; set; }
        public decimal InterestPayment { get; set; }
        public decimal TotalLoanBalance { get; set; }

        public ManualBillingModel(DateTime scheduledPaymentDate, decimal principalPayment, decimal interestPayment,
            decimal totalLoanBalance)
        {
            this.ScheduledPaymentDate = scheduledPaymentDate;
            this.PrincipalPayment = principalPayment;
            this.InterestPayment = interestPayment;
            this.TotalLoanBalance = totalLoanBalance;
        }
    }

    public class ManualBillerModel : BusinessObjectModel
    {
        public int manualBillerBorrower { get; set; }
        public int manualBillerCoBorrower { get; set; }
    }

    public class ChequesModel : BusinessObjectModel
    {
        public string BankName { get; set; }
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

        public ChequesModel()
        {
            this.IsNew = true;
        }

    }

    public partial class AddLoanPaymentForm : FullfillmentForm<FinancialEntities>
    {
        private List<ManualBillingModel> ManualBilling;
        private List<ItemsModel> WaiveItemsModel;
        private List<ItemsModel> RebateItemsModel;
        private List<LoanPaymentModel> CoBorrowerGuarantor;
        private List<LoanPaymentModel> Borrower;
        private List<ChequesModel> Cheques;
        private List<ATMPaymentModel> ATMPayments;
        public AddLoanPaymentForm()
        {
            ManualBilling = new List<ManualBillingModel>();
            WaiveItemsModel = new List<ItemsModel>();
            RebateItemsModel = new List<ItemsModel>();
            CoBorrowerGuarantor = new List<LoanPaymentModel>();
            Borrower = new List<LoanPaymentModel>();
            Cheques = new List<ChequesModel>();
            ATMPayments = new List<ATMPaymentModel>();
        }

        /// <summary>
        /// ATM RECORD
        /// </summary>
        /// <param name="model"></param>
        public void AddATM(ATMPaymentModel model)
        {
            if (this.ATMPayments.Contains(model))
                return;
            this.ATMPayments.Add(model);

        }

        public void RemoveATMs(ATMPaymentModel model)
        {
            if (this.ATMPayments.Contains(model) == true)
            {
                if (model.IsNew)
                    ATMPayments.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void RemoveATMs(string randomKey)
        {
            ATMPaymentModel model = this.ATMPayments.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveATMs(model);
        }
        public ATMPaymentModel GetATMs(string randomKey)
        {
            return this.ATMPayments.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }
        public IEnumerable<ATMPaymentModel> AvailableATMs
        {
            get
            {
                return this.ATMPayments.Where(model => model.ToBeDeleted == false);
            }
        }

        public void AddGuaCoBorror(LoanPaymentModel model)
        {
            if (this.CoBorrowerGuarantor.Contains(model) == false)
                this.CoBorrowerGuarantor.Add(model);
        }
        public void AddBorrower(LoanPaymentModel model)
        {
            if (this.Borrower.Contains(model) == false)
                this.Borrower.Add(model);
        }
        public IEnumerable<LoanPaymentModel> AvailableBorrower
        {
            get
            {
                return this.Borrower.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<LoanPaymentModel> AvailableAddGuaBorror
        {
            get
            {
                return this.CoBorrowerGuarantor.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<ChequesModel> AvailableCheques
        {
            get
            {
                return this.Cheques.Where(model => model.ToBeDeleted == false);
            }
        }
        public LoanPaymentModel RetrieveBorrower(string randomKey)
        {
            return this.Borrower.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }
        public LoanPaymentModel RetrieveBorrower(int loanId)
        {
            return this.Borrower.FirstOrDefault(entity => entity.LoanID == loanId);
        }
        public LoanPaymentModel RetrieveGuaCoBorower(string randomKey)
        {
            return this.CoBorrowerGuarantor.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }
        public LoanPaymentModel RetrieveGuaCoBorower(int loanId)
        {
            return this.CoBorrowerGuarantor.FirstOrDefault(entity => entity.LoanID == loanId);
        }
        public void ClearBorrowerList()
        {
            this.Borrower.Clear();
        }
        public void ClearCoBorrowerList()
        {
            this.CoBorrowerGuarantor.Clear();
        }
        public int CustomerId { get; set; }
        public Payment ParentPayment { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal AmountTendered { get; set; }
        public decimal CashPayment { get; set; }
        public decimal BalanceOfCashReceipts { get; set; }
        public string PaymentReferenceNumber { get; set; }
        public decimal TotalAmountApplied { get; set; }
        public decimal totalPrincipalDue { get; set; }
        public decimal totalInterestDue { get; set; }
        public decimal totalPastDue { get; set; }
        public decimal totalTotalAmountDue { get; set; }
        public int ProcessedByPartyRoleId { get; set; }

        public List<int> ReceiptsOfCashPaymentMadeByCustomer { get; set; }

        public override void Retrieve(int id)
        {
        }

        public override void PrepareForSave()
        {
            if (TotalAmountApplied == 0)
                return;

            var today = DateTime.Now;
            var borrower = this.AvailableBorrower;
            var coBorrowerGuarantor = this.AvailableAddGuaBorror;
            var cheques = this.AvailableCheques;
            //Save New Cheques Added. Browsed Checks are already saved.
            var customer = PartyRole.GetById(this.CustomerId);

            SaveNewCheques(today, customer);
            SaveNewATM(today, customer);

            ParentPayment = new Payment();
            ParentPayment.TotalAmount = TotalAmountApplied;
            ParentPayment.ProcessedByPartyRoleId = ProcessedByPartyRoleId;
            ParentPayment.ProcessedToPartyRoleId = CustomerId;
            ParentPayment.PaymentType = PaymentType.LoanPayment;
            ParentPayment.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;
            ParentPayment.PaymentMethodType = PaymentMethodType.CashType;
            ParentPayment.TransactionDate = TransactionDate;
            ParentPayment.EntryDate = today;
            Context.Payments.AddObject(ParentPayment);

            foreach (var owner in borrower)
            {
                SaveBorrowerInfo(today, ParentPayment, owner);
            }
            foreach (var coOwner in coBorrowerGuarantor)
            {
                SaveCoBorrowerInfo(today, ParentPayment, coOwner);
            }

            Context.SaveChanges();

            //After Payment is applied .. Create LoanPayment Record
            LoanPayment loanpayment = Payment.CreateLoanPayment(ParentPayment, customer);
            Context.LoanPayments.AddObject(loanpayment);


            if (this.BalanceOfCashReceipts > 0)
            {
                var totalCashPayment = this.BalanceOfCashReceipts;
                if (this.BalanceOfCashReceipts > TotalAmountApplied)
                    totalCashPayment = TotalAmountApplied;
                foreach (var receiptId in ReceiptsOfCashPaymentMadeByCustomer)
                {
                    if (totalCashPayment == 0) break;

                    var receipt = Receipt.GetById(receiptId);
                    var customerPayment = ReceiptPaymentFacade.RetrieveAssociatedPayment(receiptId, PaymentType.Receipt);

                    totalCashPayment = ApplyPayment(today, ParentPayment, totalCashPayment, receipt, customerPayment);
                }

                if (this.BalanceOfCashReceipts < TotalAmountApplied)
                    TotalAmountApplied = TotalAmountApplied - BalanceOfCashReceipts;
                else
                    TotalAmountApplied = 0;

            }

            //Close
            foreach (var usedCheques in Cheques)
            {
                if (TotalAmountApplied == 0) break;

                //TODO:: Affected by change in payment table.
                var chequePayment = Context.Payments.FirstOrDefault(entity =>
                    entity.PaymentReferenceNumber == usedCheques.CheckNumber);

                var receipt = ReceiptPaymentFacade.RetrieveAssociatedReceipt(chequePayment, PaymentType.Receipt);

                TotalAmountApplied = ApplyPayment(today, ParentPayment, TotalAmountApplied, receipt, chequePayment);
            }

            //Close used ATM
            foreach (var usedAtms in AvailableATMs)
            {
                if (TotalAmountApplied == 0) break;

                //TODO:: Affected by change in payment table.
                var atmPayment = Context.Payments.FirstOrDefault(entity =>
                    entity.PaymentReferenceNumber == usedAtms.ATMReferenceNumber);

                var receipt = ReceiptPaymentFacade.RetrieveAssociatedReceipt(atmPayment, PaymentType.Receipt);
                TotalAmountApplied = ApplyPayment(today, ParentPayment, TotalAmountApplied, receipt, atmPayment);
            }

            //TODO:: But if their is an extra cash amount, record it.
            if (CashPayment > 0)
            {
                ReceiptPaymentAssoc assoc = CreateNewPayment(today, customer, CashPayment, PaymentMethodType.CashType);
                Payment newPayment = assoc.Payment;
                Receipt receipt = assoc.Receipt;

                if (TotalAmountApplied > 0)
                {
                    TotalAmountApplied = ApplyPayment(today, ParentPayment, TotalAmountApplied, receipt, newPayment);
                    //if TotalAmountApplied > 0 means that their is change. It could be from cash or cheques.
                    //do nothing here. the receipt already had balance > 0 for those that have change.
                }
            }
            ControlNumberFacade.Create(FormType.PaymentFormType, ParentPayment);
        }

        public void AddCheques(ChequesModel model)
        {
             if (this.Cheques.Contains(model))
                return;
            this.Cheques.Add(model);
        
        }

        public void RemoveCheques(ChequesModel model)
        {
            if (this.Cheques.Contains(model) == true)
            {
                if (model.IsNew)
                    Cheques.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void RemoveCheques(string randomKey)
        {
            ChequesModel model = this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCheques(model);
        }

        private decimal ApplyPayment(DateTime today, Payment parentPayment, decimal total, Receipt receipt, Payment customerPayment)
        {
            decimal amount = 0;
            if (total >= receipt.ReceiptBalance)
                amount = (decimal)receipt.ReceiptBalance;
            else if (total < receipt.ReceiptBalance)
                amount = total;
            receipt.ReceiptBalance -= amount;
            total -= amount;
            InsertReceiptStatus(receipt, today, receipt.ReceiptBalance.Value, receipt.CurrentStatus.ReceiptStatusType);

            Payment payment = new Payment();
            payment.Payment2 = parentPayment;
            payment.ProcessedByPartyRoleId = ProcessedByPartyRoleId;
            payment.ProcessedToPartyRoleId = CustomerId;
            payment.PaymentType = PaymentType.LoanPayment;
            payment.PaymentMethodTypeId = customerPayment.PaymentMethodTypeId;
            payment.TransactionDate = TransactionDate;
            payment.TotalAmount = amount;
            payment.EntryDate = today;
            payment.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;
            Context.Payments.AddObject(payment);

            ReceiptPaymentAssoc assoc = new ReceiptPaymentAssoc();
            assoc.Receipt = receipt;
            assoc.Payment = payment;
            assoc.Amount = receipt.ReceiptBalance.Value + amount;
            Context.ReceiptPaymentAssocs.AddObject(assoc);

            return total;
        }

        private ReceiptPaymentAssoc CreateNewPayment(DateTime today, PartyRole customer, decimal amount, PaymentMethodType methodType)
        {
            Payment newPayment = new Payment();
            newPayment.ProcessedByPartyRoleId = ProcessedByPartyRoleId;
            newPayment.ProcessedToPartyRoleId = CustomerId;
            newPayment.PaymentType = PaymentType.Receipt;
            newPayment.PaymentMethodType = methodType;
            newPayment.TransactionDate = TransactionDate;
            newPayment.EntryDate = today;
            newPayment.TotalAmount = amount;
            newPayment.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;
            Context.Payments.AddObject(newPayment);

            //TODO:: Affected by change in payment table.
            Receipt newReceipt = new Receipt();
            newReceipt.ReceivedFromName = customer.Party.Name;
            newReceipt.ReceiptBalance = amount;
            Context.Receipts.AddObject(newReceipt);

            ReceiptPaymentAssoc assoc = new ReceiptPaymentAssoc();
            assoc.Receipt = newReceipt;
            assoc.Payment = newPayment;
            assoc.Amount = newReceipt.ReceiptBalance.Value;
            Context.ReceiptPaymentAssocs.AddObject(assoc);

            ReceiptStatu newReceiptStatus = new ReceiptStatu();
            newReceiptStatus.Receipt = newReceipt;
            newReceiptStatus.ReceiptStatusTypeId = ReceiptStatusType.OpenReceiptStatusType.Id;
            newReceiptStatus.TransitionDateTime = today;
            newReceiptStatus.IsActive = true;
            Context.ReceiptStatus.AddObject(newReceiptStatus);

            return assoc;
        }

        public void SaveNewCheques(DateTime today, PartyRole customer)
        {
            foreach (var cheque in Cheques)
            {
                PaymentMethodType methodType = null;
                var newChequePayments = Context.Payments.FirstOrDefault(entity =>
                        entity.PaymentReferenceNumber == cheque.CheckNumber);

                if (cheque.CheckType == "Pay Check")
                    methodType = PaymentMethodType.PayCheckType;
                else
                    methodType = PaymentMethodType.PersonalCheckType;

                if (newChequePayments == null)
                {
                    var bank = Context.Banks.SingleOrDefault(entity => entity.PartyRole.Party.Organization.OrganizationName == cheque.BankName);

                    ReceiptPaymentAssoc assoc = CreateNewPayment(today, customer, cheque.Amount, methodType);
                    Payment newPayment = assoc.Payment;
                    newPayment.PaymentReferenceNumber = cheque.CheckNumber;

                    Cheque newCheque = new Cheque();
                    newCheque.BankPartyRoleId = bank.PartyRoleId;
                    newCheque.Payment = newPayment;
                    newCheque.CheckDate = cheque.CheckDate;
                    Context.Cheques.AddObject(newCheque);

                    ChequeStatu newChequeStatus = new ChequeStatu();
                    newChequeStatus.Cheque = newCheque;
                    newChequeStatus.CheckStatusTypeId = ChequeStatusType.ClearedType.Id;
                    newChequeStatus.TransitionDateTime = today;
                    newChequeStatus.IsActive = true;
                    Context.ChequeStatus.AddObject(newChequeStatus);
                }
            }
            Context.SaveChanges();
        }
        public void SaveNewATM(DateTime today, PartyRole customer)
        {
            foreach (var atm in AvailableATMs)
            {
                ReceiptPaymentAssoc assoc = CreateNewPayment(today, customer, atm.Amount, PaymentMethodType.ATMType);
                Payment newPayment = assoc.Payment;
                newPayment.PaymentReferenceNumber = atm.ATMReferenceNumber;
             
            }
            Context.SaveChanges();
        }

        public void ClearCheques()
        {
            this.Cheques.Clear();
        }

        private void SaveBorrowerInfo(DateTime today, Payment payment, LoanPaymentModel borrower)
        {
            if (borrower.PaymentAmount > 0)
            {
                PartyRole customerPartyRole = null;
                LoanAccount loanAccount = Context.LoanAccounts.FirstOrDefault(entity =>
                    entity.FinancialAccountId == borrower.LoanID);
                var otherPartyRole = PartyRole.GetById(CustomerId);
                FinancialAccountRole financialAccountRole = Context.FinancialAccountRoles.FirstOrDefault(entity => entity.FinancialAccountId == borrower.LoanID
                && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id);
                if (otherPartyRole.PartyId == financialAccountRole.PartyRole.PartyId)
                    customerPartyRole = PartyRole.GetByPartyIdAndRole(financialAccountRole.PartyRole.PartyId, RoleType.CustomerType);

                //Create Financial Account Transaction
                FinAcctTran financialAccountTransaction = new FinAcctTran();
                financialAccountTransaction.FinlAcctTransType = FinlAcctTransType.AccountPaymentType;
                financialAccountTransaction.Payment = payment;
                financialAccountTransaction.TransactionDate = TransactionDate;
                financialAccountTransaction.EntryDate = today;
                financialAccountTransaction.FinancialAccountId = borrower.LoanID;
                financialAccountTransaction.Amount = borrower.PaymentAmount;

                borrower.WaivePastDue = AdjustAndUpdateReceivables(borrower, borrower.PastDueReceivables,
                    borrower.WaivePastDue, AdjustmentType.WaivedType, today, "Waived Receivable");
                borrower.WaiveInterest = AdjustAndUpdateReceivables(borrower, borrower.InterestReceivables,
                    borrower.WaiveInterest, AdjustmentType.WaivedType, today, "Waived Receivable");
                borrower.WaivePrincipalDue = AdjustAndUpdateLoanBalance(borrower, borrower.WaivePrincipalDue,
                    today, AdjustmentType.WaivedType, "Waived Loan Balance");

                borrower.RebateInterest = AdjustAndUpdateReceivables(borrower, borrower.InterestReceivables,
                    borrower.RebateInterest, AdjustmentType.RebateType, today, "Rebated Receivable");
                borrower.RebatePrincipalDue = AdjustAndUpdateLoanBalance(borrower, borrower.RebatePrincipalDue,
                    today, AdjustmentType.RebateType, "Rebated Loan Balance");

                if (borrower.InterestReceivables.Count > 0)
                    SavePaymentReceivable(borrower, borrower.InterestReceivables, payment, today);

                SavePaymentPrincipal(borrower, loanAccount, payment, today);

                //if (customerPartyRole != null)
                //{
                //    var customer = customerPartyRole.Customer;
                //    if (customer.CreditLimit != null)
                //        customer.CreditLimit += loanAccount.LoanBalance;
                //}

                UpdateLoanStatus(today, loanAccount, loanAccount.LoanBalance);
            }
        }

        private void SaveCoBorrowerInfo(DateTime today, Payment payment, LoanPaymentModel coborrower)
        {
            if (coborrower.PaymentAmount > 0)
            {
                FinAcctTran financialAccounttransaction = new FinAcctTran();
                financialAccounttransaction.FinlAcctTransType = FinlAcctTransType.AccountPaymentType;
                financialAccounttransaction.Payment = payment;
                financialAccounttransaction.TransactionDate = TransactionDate;
                financialAccounttransaction.EntryDate = today;
                financialAccounttransaction.FinancialAccountId = coborrower.LoanID;
                financialAccounttransaction.Amount = coborrower.PaymentAmount;
                LoanAccount loanAccount = Context.LoanAccounts.FirstOrDefault(entity =>
                    entity.FinancialAccountId == coborrower.LoanID);

                coborrower.WaivePastDue = AdjustAndUpdateReceivables(coborrower, coborrower.PastDueReceivables,
                    coborrower.WaivePastDue, AdjustmentType.WaivedType, today, "Waived Interest");
                coborrower.WaiveInterest = AdjustAndUpdateReceivables(coborrower, coborrower.InterestReceivables,
                    coborrower.WaiveInterest, AdjustmentType.WaivedType, today, "Waived Receivable");
                coborrower.WaivePrincipalDue = AdjustAndUpdateLoanBalance(coborrower, coborrower.WaivePrincipalDue,
                    today, AdjustmentType.WaivedType, "Waived Loan Balance");

                if(coborrower.InterestReceivables.Count > 0)
                    SavePaymentReceivable(coborrower, coborrower.InterestReceivables, payment, today);
                SavePaymentPrincipal(coborrower, loanAccount, payment, today);
                UpdateLoanStatus(today, loanAccount, loanAccount.LoanBalance);
            }
        }

        private decimal AdjustAndUpdateReceivables(LoanPaymentModel model, List<ReceivableInfo> infos, decimal adjustmentPayment,
            AdjustmentType adjustmentType, DateTime today, string remarks)
        {
            if (adjustmentPayment > 0)
            {
                foreach (var info in infos)
                {
                    if (adjustmentPayment > 0)
                    {
                        var receivable = Context.Receivables.SingleOrDefault(entity =>
                            entity.Id == info.ReceivableId);
                        adjustmentPayment = UpdateBorrowerBalance(adjustmentPayment, receivable);
                        info.Amount = receivable.Balance;
                        CreateReceivableAdjustment(model, today, adjustmentType, receivable, remarks);
                    }
                    else break;
                }
            }

            return adjustmentPayment;
        }

        private decimal UpdateBorrowerBalance(decimal adjustmentPayment, Receivable receivable)
        {
            decimal amountApplied = 0;
            if (adjustmentPayment <= receivable.Balance)
                amountApplied = adjustmentPayment;
            //to avoid negative receivable balance
            else if (adjustmentPayment > receivable.Balance)
                amountApplied = receivable.Balance;

            receivable.Balance -= amountApplied;
            adjustmentPayment -= amountApplied;
            Context.SaveChanges();
            return adjustmentPayment;
        }

        private ReceivableAdjustment CreateReceivableAdjustment(LoanPaymentModel model, DateTime today, AdjustmentType adjustmentType, Receivable receivable, string remarks)
        {
            ReceivableAdjustment receivableAdjusment = new ReceivableAdjustment();
            receivableAdjusment.ReceivableId = receivable.Id;
            receivableAdjusment.AdjustmentType = adjustmentType;
            receivableAdjusment.PartyRoleId = CustomerId;
            receivableAdjusment.Date = TransactionDate;
            if (adjustmentType == AdjustmentType.WaivedType)
                receivableAdjusment.Amount = model.WaivePastDue + model.WaiveInterest;
            if (adjustmentType == AdjustmentType.RebateType)
                receivableAdjusment.Amount = model.RebateInterest;
            receivableAdjusment.Remarks = remarks;
            return receivableAdjusment;
        }

        private LoanAdjustment CreateLoanAdjustment(LoanPaymentModel model, DateTime today, AdjustmentType adjustmentType, LoanAccount loanAccount, string remarks)
        {
            LoanAdjustment loanAdjustment = new LoanAdjustment();
            loanAdjustment.FinancialAccountId = loanAccount.FinancialAccountId;
            loanAdjustment.AdjustmentType = adjustmentType;
            loanAdjustment.Date = TransactionDate;
            if (adjustmentType == AdjustmentType.RebateType)
                loanAdjustment.Amount = model.RebatePrincipalDue;
            if (adjustmentType == AdjustmentType.WaivedType)
                loanAdjustment.Amount = model.WaivePrincipalDue;
            loanAdjustment.Remarks = remarks;
            return loanAdjustment;
        }

        private decimal AdjustAndUpdateLoanBalance(LoanPaymentModel model, decimal adjustmentPayment, DateTime today,
            AdjustmentType adjustmentType, string remarks)
        {
            var loanAccount = Context.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == model.LoanID);
            if (adjustmentPayment != 0)
            {
                decimal adjustedPrincipal = 0;
                if (adjustmentType == AdjustmentType.WaivedType)
                    adjustedPrincipal = model.WaivePrincipalDue;
                if (adjustmentType == AdjustmentType.RebateType)
                    adjustedPrincipal = model.RebatePrincipalDue;
                loanAccount.LoanBalance -= adjustedPrincipal;
                CreateLoanAdjustment(model, today, adjustmentType, loanAccount, remarks);
            }
            return adjustmentPayment;
        }

        private PaymentApplication CreatePaymentApplicationReceivables(DateTime today, Payment payment, LoanPaymentModel model, ReceivableInfo receivableInfo, decimal amountApplied)
        {
            PaymentApplication paymentApplication = new PaymentApplication();
            paymentApplication.Payment = payment;
            paymentApplication.ReceivableId = receivableInfo.ReceivableId;
            paymentApplication.AmountApplied = amountApplied;
            var receivable = Context.Receivables.SingleOrDefault(entity =>
                entity.Id == receivableInfo.ReceivableId);
            receivable.Balance -= paymentApplication.AmountApplied;
            model.PaymentAmount -= paymentApplication.AmountApplied;
            //TotalAmountApplied += paymentApplication.AmountApplied;
            //LoanBalanceApplied += paymentApplication.AmountApplied;
            InsertReceivableStatus(receivable.Id, receivable.Balance, today);
            return paymentApplication;
        }

        private PaymentApplication CreatePaymentApplicationPrincipal(DateTime today, Payment payment, LoanPaymentModel model, LoanAccount loanAccount, decimal amountApplied)
        {
            PaymentApplication paymentApplication = new PaymentApplication();
            paymentApplication.Payment = payment;
            paymentApplication.FinancialAccountId = loanAccount.FinancialAccountId;
            paymentApplication.AmountApplied = amountApplied;
            var loan = Context.LoanAccounts.SingleOrDefault(entity =>
                entity.FinancialAccountId == model.LoanID);
            loan.LoanBalance -= paymentApplication.AmountApplied;
            model.PaymentAmount -= paymentApplication.AmountApplied;
            //TotalAmountApplied += paymentApplication.AmountApplied;
            //LoanBalanceApplied += paymentApplication.AmountApplied;
 
            return paymentApplication;
        }

        public void SavePaymentReceivable(LoanPaymentModel model, List<ReceivableInfo> receivableInfos, Payment payment, DateTime today)
        {
            foreach (var info in receivableInfos)
            {
                if (model.PaymentAmount <= info.Amount && model.PaymentAmount > 0)
                {
                    PaymentApplication paymentApplication = 
                        CreatePaymentApplicationReceivables(today, payment, model, info, model.PaymentAmount);
                }
                else if (model.PaymentAmount > info.Amount && model.PaymentAmount > 0)
                {
                    PaymentApplication paymentApplication = 
                        CreatePaymentApplicationReceivables(today, payment, model, info, info.Amount);
                }
            }
        }

        public void SavePaymentPrincipal(LoanPaymentModel model, LoanAccount loanAccount, Payment payment, DateTime today)
        {
            if (model.PaymentAmount <= loanAccount.LoanAmount && model.PaymentAmount > 0)
            {
                PaymentApplication paymentApplication = 
                    CreatePaymentApplicationPrincipal(today, payment, model, loanAccount, model.PaymentAmount);
            }
            else if (model.PaymentAmount > loanAccount.LoanAmount && model.PaymentAmount > 0)
            {
                PaymentApplication paymentApplication = 
                    CreatePaymentApplicationPrincipal(today, payment, model, loanAccount, loanAccount.LoanAmount);
            }
        }

        private void UpdateLoanStatus(DateTime today, LoanAccount loanAccount, decimal balance)
        {
            var currentstatus = loanAccount.CurrentStatus;

            if ((currentstatus.StatusTypeId == LoanAccountStatusType.CurrentType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.DelinquentType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.RestructuredType.Id)
                     && balance == 0)
            {
                var isValidIndicator = Context.LoanAccountStatusTypeAssocs.Where(entity => entity.FromStatusTypeId == currentstatus.StatusTypeId
                    && entity.ToStatusTypeId == LoanAccountStatusType.PaidOffType.Id && entity.EndDate == null).Count();

                if (isValidIndicator > 0)
                {
                    currentstatus.IsActive = false;
                    LoanAccountStatu loanAccountStatus = new LoanAccountStatu();
                    loanAccountStatus.FinancialAccountId = currentstatus.FinancialAccountId;
                    loanAccountStatus.StatusTypeId = LoanAccountStatusType.PaidOffType.Id;
                    loanAccountStatus.TransitionDateTime = today;
                    loanAccountStatus.IsActive = true;
                    Context.LoanAccountStatus.AddObject(loanAccountStatus);
                }
            }
        }

        public void InsertReceivableStatus(int receivableID, decimal balance, DateTime today)
        {
            var receivable = Context.Receivables.FirstOrDefault(entity => entity.Id == receivableID);
            if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.OpenType.Id && balance != 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.PartiallyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.OpenType.Id && balance == 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.FullyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id && balance != 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.PartiallyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id && balance == 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.FullyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
        }

        public void InsertReceiptStatus(Receipt receipt, DateTime today, decimal balance, ReceiptStatusType statustype)
        {
            //var receiptstatus = receipt.CurrentStatus;
            ReceiptStatu newreceiptstatus = new ReceiptStatu();

            if (statustype.Id == ReceiptStatusType.OpenReceiptStatusType.Id && balance == 0)
            {
                //create receipt status
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                  entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id &&
                  entity.ToStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                  entity.EndDate == null);
                if (validIndicator != null)
                {
                    if (receipt.CurrentStatus != null) receipt.CurrentStatus.IsActive = false;
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
                //create next receipt status
                var validIndicator1 = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                  entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                  entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id &&
                  entity.EndDate == null);
                if (validIndicator1 != null)
                {
                    newreceiptstatus.IsActive = false;
                    ReceiptStatu nextreceiptstatus = new ReceiptStatu();
                    nextreceiptstatus.Receipt = receipt;
                    nextreceiptstatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                    nextreceiptstatus.TransitionDateTime = today;
                    nextreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(nextreceiptstatus);
                }
            }
            else if (statustype.Id == ReceiptStatusType.OpenReceiptStatusType.Id && balance != 0)
            {
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                     entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id &&
                     entity.ToStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                     entity.EndDate == null);
                if (validIndicator != null)
                {
                    if (receipt.CurrentStatus != null) receipt.CurrentStatus.IsActive = false;
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
            }
            else if (statustype.Id == ReceiptStatusType.AppliedReceiptStatusType.Id && balance == 0)
            {
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                      entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                      entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id &&
                      entity.EndDate == null);
                if (validIndicator != null)
                {
                    if (receipt.CurrentStatus != null) receipt.CurrentStatus.IsActive = false;
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
            }
        }
    }
}
