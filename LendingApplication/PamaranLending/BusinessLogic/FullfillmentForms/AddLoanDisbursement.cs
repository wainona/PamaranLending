using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class LoanDisbursementForm : FullfillmentForm<FinancialEntities>
    {
        public int CurrencyId { get; set; }
        public LoanDisbursementType LoanDisbursementType { get; set; }
        public int LoanDisbursementVcrId { get; set; }
        public int CustomerId { get; set; }
        public string DisbursedToName { get; set; }
        public int LoanAgreementId { get; set; }
        public string LoanProduct { get; set; }
        public int LoanProductId { get; set; }
        public decimal LoanAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ReceivedBy { get; set; }
        public decimal InterestPayment { get; set; }
        public decimal CashAmountToDisburse { get; set; }
        public decimal CheckAmountToDisburse { get; set; }
        public decimal TotalAmountToDisburse { get; set; }
        public decimal Deductions { get; set; }
        public int DisbursementId { get; set; }
        public string Signature { get; set; }
        private List<AddChequesModel> Cheques;

        public LoanDisbursementForm()
        {
            Cheques = new List<AddChequesModel>();
            CurrencyId = -1;
        }

        public void AddCheques(AddChequesModel model)
        {
            if (this.Cheques.Contains(model))
                return;
            this.Cheques.Add(model);

        }

        public void RemoveCheques(AddChequesModel model)
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
            AddChequesModel model = this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCheques(model);
        }
        public IEnumerable<AddChequesModel> AvailableCheques
        {
            get
            {
                return this.Cheques.Where(model => model.ToBeDeleted == false);
            }
        }
        public override void Retrieve(int id)
        {
        }
 
        public override void PrepareForSave()
        {
            var now = DateTime.Now;
            var customerOtherRole = PartyRole.GetById(CustomerId);
            var customer = PartyRole.GetByPartyIdAndRole(customerOtherRole.PartyId, RoleType.CustomerType);
            CustomerId = customer.Id;
            var agreement = Context.Agreements.FirstOrDefault(entity => entity.Id == LoanAgreementId && entity.EndDate == null);
            
            //First payment record
            Payment payment = new Payment();
            payment.PaymentMethodType = PaymentMethodType.CashType;
            payment.ProcessedByPartyRoleId = ReceivedBy;
            payment.ProcessedToPartyRoleId = CustomerId;
            payment.PaymentType = PaymentType.Disbursement;
            payment.TransactionDate = TransactionDate;
            payment.EntryDate = now;
            payment.TotalAmount = TotalAmountToDisburse;
            payment.SpecificPaymentType = SpecificPaymentType.LoanDisbursementType;


            var formDetail = FormDetail.Create(FormType.TransactionSlipType.Id, payment, "Disbursed To", DisbursedToName, Signature);
            Context.FormDetails.AddObject(formDetail);

            PaymentApplication paymentApplications = new PaymentApplication();
            paymentApplications.Payment = payment;
            paymentApplications.LoanDisbursementVoucherId = LoanDisbursementVcrId;
            paymentApplications.AmountApplied = payment.TotalAmount;
            
            decimal pAmount = 0;
            var loanDisbursementVcr = Context.LoanDisbursementVcrs.FirstOrDefault(entity => entity.Id == LoanDisbursementVcrId);
            if (loanDisbursementVcr.Balance > paymentApplications.AmountApplied)
                pAmount = paymentApplications.AmountApplied;
            else if(loanDisbursementVcr.Balance <= paymentApplications.AmountApplied)
                pAmount = (decimal)loanDisbursementVcr.Balance;
            loanDisbursementVcr.Balance = loanDisbursementVcr.Balance - pAmount;


            //Update ammortization Shedule
            var disbursementVcrStat = Context.DisbursementVcrStatus.SingleOrDefault(entity => entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.PendingType.Id && entity.IsActive == true && entity.LoanDisbursementVoucherId == LoanDisbursementVcrId);//must be changed
            if (disbursementVcrStat != null)
            {
                var ammortization = disbursementVcrStat.LoanDisbursementVcr.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == LoanAgreementId &&
                    ((entity.LoanReleaseDate.Date != now.Date) || (entity.LoanReleaseDate.Date != TransactionDate))
                    && entity.EndDate == null);
                if (ammortization != null)
                    ammortization.EndDate = now.Date;
            }

            bool disbursementvcrstatusflag = SetDisbursementVcrStatus(now, loanDisbursementVcr);

            Disbursement disbursement = new Disbursement();
            disbursement.Payment = payment;
            disbursement.DisbursementType = DisbursementType.LoanDisbursementType;
            if (string.IsNullOrEmpty(DisbursedToName) == false)
                disbursement.DisbursedToName = DisbursedToName;

        

            //////////////////////////////////////MUST FINISH
            if (CashAmountToDisburse > 0)
                DisbursementFacade.SaveToDisburseCash(TransactionDate, now, payment,CustomerId, ReceivedBy, CashAmountToDisburse, SpecificPaymentType.LoanDisbursementType,DisbursementType.LoanDisbursementType,DisbursedToName,CurrencyId);
            if (CheckAmountToDisburse > 0)
                DisbursementFacade.SaveToDisbursementCheques(TransactionDate, now, payment, Cheques, CustomerId, ReceivedBy, SpecificPaymentType.LoanDisbursementType, DisbursementType.LoanDisbursementType, DisbursedToName,CurrencyId);
            LoanDisbursement loanDisbursement = new LoanDisbursement();

            var financialAccount1 = Context.FinancialAccounts.FirstOrDefault(entity => entity.AgreementId == LoanAgreementId);
            if (financialAccount1 == null)
            {
                loanDisbursement.Disbursement = disbursement;
                loanDisbursement.LoanDisbursementType = LoanDisbursementType;
                loanDisbursement.LoanAmount = TotalAmountToDisburse;
                loanDisbursement.LoanBalance = TotalAmountToDisburse;
                loanDisbursement.InterestBalance = 0;

                FinancialAccount newFinancialAccount = new FinancialAccount();
                newFinancialAccount.FinancialAccountType = FinancialAccountType.LoanAccountType;
                newFinancialAccount.AgreementId = LoanAgreementId;

                LoanAccount loanAccount = new LoanAccount();
                loanAccount.FinancialAccount = newFinancialAccount;

                loanAccount.LoanAmount = TotalAmountToDisburse;
                loanAccount.LoanBalance = TotalAmountToDisburse;
                loanAccount.LoanReleaseDate = payment.EntryDate;

                var ammortizationSchedItems = from a in Context.AmortizationSchedules
                                              join asi in Context.AmortizationScheduleItems on a.Id equals asi.AmortizationScheduleId
                                              where a.AgreementId == LoanAgreementId && a.EndDate == null
                                              orderby asi.ScheduledPaymentDate ascending
                                              select asi;

                //if (ammortizationSchedItems.Count() > 0) loanAccount.MaturityDate = ammortizationSchedItems.ToList().Last().ScheduledPaymentDate;
                if (ammortizationSchedItems.Count() > 0) loanAccount.MaturityDate = ammortizationSchedItems.OrderByDescending(x => x.ScheduledPaymentDate).First().ScheduledPaymentDate;

  

                //Insert Cheque Loan Account
                InsertChequeAssoc(agreement, loanAccount);


                LoanAccountStatu loanStatus = new LoanAccountStatu();
                loanStatus.LoanAccount = loanAccount;
                loanStatus.LoanAccountStatusType = LoanAccountStatusType.CurrentType;
                loanStatus.TransitionDateTime = now;
                loanStatus.IsActive = true;

                FinancialAccountProduct financialAccProduct = new FinancialAccountProduct();
                financialAccProduct.FinancialAccount = newFinancialAccount;
                financialAccProduct.FinancialProductId = LoanProductId;
                financialAccProduct.EffectiveDate = now;

                var agreementrole = from ar in Context.AgreementRoles.Where(entity => entity.AgreementId == LoanAgreementId) select ar;

                var coBorrowerRole = from r in Context.PartyRoles.Where(entity => entity.RoleTypeId == RoleType.CoBorrowerAgreementType.Id)
                                     join a in agreementrole on r.Id equals a.PartyRoleId
                                     select r;
                var guarantorRole = from r in Context.PartyRoles.Where(entity => entity.RoleTypeId == RoleType.GuarantorAgreementType.Id)
                                    join a in agreementrole on r.Id equals a.PartyRoleId
                                    select r;

                foreach (var cb in coBorrowerRole)
                {
                    PartyRole partyRole = new PartyRole();
                    partyRole.Party = cb.Party;
                    partyRole.RoleTypeId = RoleType.CoOwnerFinancialType.Id;
                    partyRole.EffectiveDate = now;

                    FinancialAccountRole finanAccRole = new FinancialAccountRole();
                    finanAccRole.FinancialAccount = newFinancialAccount;
                    finanAccRole.PartyRole = partyRole;
                }
                foreach (var cb in guarantorRole)
                {
                    PartyRole partyRole = new PartyRole();
                    partyRole.Party = cb.Party;
                    partyRole.RoleTypeId = RoleType.GuarantorFinancialType.Id;
                    partyRole.EffectiveDate = now;

                    FinancialAccountRole finanAccRole = new FinancialAccountRole();
                    finanAccRole.FinancialAccount = newFinancialAccount;
                    finanAccRole.PartyRole = partyRole;
                }
                //As Borrower
                PartyRole borrowerpartyRole = new PartyRole();
                borrowerpartyRole.Party = customer.Party;
                borrowerpartyRole.RoleTypeId = RoleType.OwnerFinancialType.Id;
                borrowerpartyRole.EffectiveDate = now;

                FinancialAccountRole borrowerfinanAccRole = new FinancialAccountRole();
                borrowerfinanAccRole.FinancialAccount = newFinancialAccount;
                borrowerfinanAccRole.PartyRole = borrowerpartyRole;
            }
            else
            {
                var la = Context.LoanAccounts.FirstOrDefault(entity => entity.FinancialAccountId == financialAccount1.Id);
                la.LoanBalance += TotalAmountToDisburse;
                la.LoanAmount += TotalAmountToDisburse;

                decimal interest = 0;
                var receivables = Receivable.GetInterestOutstandingReceivables(la.FinancialAccountId);
                if (receivables.Count() != 0) interest = receivables.Sum(e => e.Balance);

                loanDisbursement.Disbursement = disbursement;
                loanDisbursement.LoanDisbursementType = LoanDisbursementType;
                loanDisbursement.LoanAmount = la.LoanAmount;
                loanDisbursement.LoanBalance = la.LoanBalance;
                loanDisbursement.InterestBalance = interest;

            }

            //Inserting Loan Application Status
         
            var loanAppStatus = Context.LoanApplicationStatus.FirstOrDefault(entity => entity.ApplicationId == agreement.ApplicationId && entity.LoanApplicationStatusType.Id== LoanApplicationStatusType.PendingInFundingType.Id
                && entity.IsActive == true);
            if (loanAppStatus != null && disbursementvcrstatusflag == true && agreement != null)
            {
                LoanApplicationStatu loanAppStatus2 = new LoanApplicationStatu();
                loanAppStatus2.ApplicationId = agreement.ApplicationId;
                loanAppStatus2.LoanApplicationStatusType = LoanApplicationStatusType.ClosedType;
                loanAppStatus2.TransitionDateTime = now;
                loanAppStatus2.IsActive = true;
                loanAppStatus.IsActive = false;
            }

     
            //Create Receipt Payment Record
            //Second payment record
            

            decimal computedValue = 0;
            decimal loanBalance = 0;
            var applicationItems = Context.ApplicationItems.Where
                          (entity => entity.ApplicationId == agreement.ApplicationId && entity.FeeComputedValue.HasValue && entity.EndDate == null);
            if (applicationItems.Count() > 0)
                computedValue = applicationItems.Sum(entity => entity.FeeComputedValue.Value);
            var applicationitem2 = from lr in Context.LoanReAvailments
                                   join las in Context.LoanAccountStatus on lr.FinancialAccountId equals las.FinancialAccountId
                                   where lr.ApplicationId == agreement.ApplicationId && (las.StatusTypeId != LoanAccountStatusType.PaidOffType.Id || las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id || las.StatusTypeId != LoanAccountStatusType.WrittenOffType.Id)
                                   && las.IsActive == true
                                   select lr;
            if (applicationitem2.Count() > 0)
                loanBalance = applicationitem2.Sum(entity => entity.LoanBalance);
            
            //Create Receipt Payment, Loan Payment, or Fee Payment if either of the 3 has balance
            if ((computedValue != 0 || loanBalance != 0 || InterestPayment != 0) && Deductions != 0)
            {
            
                //Receipt record for the fee payment
                Payment receiptPayment = Payment.CreatePayment(now, TransactionDate, CustomerId, ReceivedBy,
                Deductions, PaymentType.Receipt, PaymentMethodType.CashType,
               SpecificPaymentType.FeePaymentType, string.Empty);
                Receipt receipt = Receipt.CreateReceipt(string.Empty, 0);
                ReceiptStatu.Create(receipt, now, ReceiptStatusType.OpenReceiptStatusType, false);
                Receipt.ChangeReceiptStatusFrom(receipt, now, 0, ReceiptStatusType.OpenReceiptStatusType);
                Receipt.CreateReceiptPaymentAssoc(receipt, receiptPayment,Deductions);

                //Actual Payment for the Fee
                Payment payment2 = Payment.CreatePayment(now, TransactionDate, CustomerId, ReceivedBy,
             Deductions, PaymentType.FeePayment, PaymentMethodType.CashType,
            SpecificPaymentType.FeePaymentType, string.Empty);
                payment2.Payment2 = payment;

            //Create Fee Payment Record if there is value for computedValue
            //Third Payment
                if (computedValue != 0)
                {
                    Payment payment3 = new Payment();
                    payment3.Payment2 = payment2; //parent payment
                    payment3.PaymentMethodType = payment2.PaymentMethodType;
                    payment3.ProcessedByPartyRoleId = ReceivedBy;
                    payment3.ProcessedToPartyRoleId = CustomerId;
                    payment3.PaymentType = PaymentType.FeePayment;
                    payment3.TransactionDate = payment2.TransactionDate;
                    payment3.EntryDate = now;
                    payment3.TotalAmount = computedValue;
                    payment3.SpecificPaymentType = SpecificPaymentType.FeePaymentType;

                    ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
                    receiptpaymentassoc2.Payment = payment3;
                    receiptpaymentassoc2.Receipt = receipt;
                    receiptpaymentassoc2.Amount = computedValue;

                    var fee = Context.ApplicationItems.Where(entity => entity.ApplicationId == agreement.ApplicationId && entity.FeeComputedValue.HasValue && entity.EndDate == null);
                    foreach (var f in fee)
                    {
                        FeePayment feePayment = new FeePayment();
                        feePayment.Payment = payment3; //passing payment ID through receipt
                        var feeName = Context.ProductFeatures.SingleOrDefault(entity => entity.Id == f.ProductFeatureApplicability.ProductFeatureId && entity.ProductFeatureCategory.Name == "Fee");
                        feePayment.Particular = feeName.Name;
                        feePayment.FeeAmount = (decimal)f.FeeComputedValue;
                    }
                }

                //Interest Payment
                //PayDiscountedInterest(today, receipt, receiptpaymentassoc, payment2);
                //FourthPaymentRecord, Create loanpayment when there is existing reavailments;
                //PayReloan(today, customer, agreement, loanBalance, receipt, receiptpaymentassoc, payment2);
            }



            //Update Revailment,  Update LoanAccountStatus
            UpdateLoanReavailment(now, loanDisbursement, agreement);

            //Set  CustomerStatus 
            SetCustomerStatus(now);
            
            //If first time to disburse, create release statement
            if(financialAccount1 ==  null)
            InsertReleaseStatement(loanDisbursement,customer.PartyId, -1);
            else InsertReleaseStatement(loanDisbursement, customer.PartyId, financialAccount1.Id);

            ControlNumberFacade.Create(FormType.TransactionSlipType, payment);
            Context.SaveChanges();
            DisbursementId = payment.Id;
        }

        private void PayDiscountedInterest(DateTime today, Receipt receipt, ReceiptPaymentAssoc receiptpaymentassoc, Payment payment2)
        {
            //Interest Payment
            if (InterestPayment != 0)
            {
                Payment payment3 = new Payment();
                payment3.Payment2 = payment2; //parent payment
                payment3.PaymentMethodType = payment2.PaymentMethodType;
                payment3.ProcessedByPartyRoleId = ReceivedBy;
                payment3.ProcessedToPartyRoleId = CustomerId;
                payment3.PaymentType = PaymentType.Receipt;
                payment3.TransactionDate = payment2.TransactionDate;
                payment3.EntryDate = today;
                payment3.TotalAmount = InterestPayment;
                payment3.SpecificPaymentType = SpecificPaymentType.InterestPaymentType;

                Receipt receipt3 = new Receipt();
                receipt3.ReceiptBalance = 0;

                ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
                receiptpaymentassoc.Payment = payment3;
                receiptpaymentassoc.Receipt = receipt;
                receiptpaymentassoc2.Amount = InterestPayment;

            }
        }

        private void PayReloan(DateTime today, PartyRole customer, Agreement agreement, decimal loanBalance, Receipt receipt, ReceiptPaymentAssoc receiptpaymentassoc, Payment payment2)
        {
            if (loanBalance != 0)
            {
                Payment payment4 = new Payment();
                payment4.Payment2 = payment2; //parent paymnet ID
                payment4.PaymentMethodType = payment2.PaymentMethodType;
                payment4.ProcessedByPartyRoleId = ReceivedBy;
                payment4.ProcessedToPartyRoleId = CustomerId;
                payment4.PaymentType = PaymentType.Receipt;
                payment4.TransactionDate = payment2.TransactionDate;
                payment4.EntryDate = today;
                payment4.TotalAmount = loanBalance;
                payment4.PaymentType = PaymentType.LoanPayment;
                payment4.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;

                Receipt receipt4 = new Receipt();
                receipt4.ReceiptBalance = 0;

                ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
                receiptpaymentassoc.Payment = payment4;
                receiptpaymentassoc.Receipt = receipt;
                receiptpaymentassoc.Amount = loanBalance;
                
                var customerPartyRole = PartyRole.GetById(CustomerId);
                LoanPayment loanPayment = Payment.CreateLoanPayment(payment4, customerPartyRole);

                var financialAccounts = from lr in Context.LoanReAvailments
                                        join fa in Context.FinancialAccounts.Where(entity =>
                                         entity.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id) on lr.FinancialAccountId equals fa.Id
                                        join las in Context.LoanAccountStatus on fa.Id equals las.FinancialAccountId
                                        where las.IsActive == true && (las.LoanAccountStatusType.Id == LoanAccountStatusType.CurrentType.Id
                                        || las.LoanAccountStatusType.Id == LoanAccountStatusType.DelinquentType.Id
                                        || las.LoanAccountStatusType.Id == LoanAccountStatusType.UnderLitigationType.Id) && lr.ApplicationId == agreement.ApplicationId
                                        select fa;

                var loanAccounts = from fa in financialAccounts
                                   join far in Context.FinancialAccountRoles on fa.Id equals far.FinancialAccountId
                                   join pr in Context.PartyRoles on far.PartyRoleId equals pr.Id
                                   where far.PartyRole.PartyId == customer.PartyId &&
                                   (pr.RoleTypeId == RoleType.OwnerFinancialType.Id ||
                                   pr.RoleTypeId == RoleType.CoOwnerFinancialType.Id ||
                                   pr.RoleTypeId == RoleType.GuarantorFinancialType.Id) &&
                                   pr.EndDate == null
                                   select fa.LoanAccount;
                var amount = loanBalance;

                amount = CreateLoanPayment(today, payment4, loanAccounts, amount);
            }
        }

        private decimal CreateLoanPayment(DateTime today, Payment payment4, IQueryable<LoanAccount> loanAccounts, decimal amount)
        {
            foreach (var loanAccount in loanAccounts)
            {
                decimal TotalAmountApplied = 0;
                var interestReceivables = from r in Context.Receivables
                                          join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                                          where rs.IsActive == true && (rs.ReceivableStatusType.Id != ReceivableStatusType.CancelledType.Id
                                          || rs.ReceivableStatusType.Id != ReceivableStatusType.FullyPaidType.Id)
                                          && r.FinancialAccountId == loanAccount.FinancialAccountId
                                          && r.ReceivableTypeId == ReceivableType.InterestType.Id
                                          select r;
                if (amount != 0)
                {
                    foreach (var interestDue in interestReceivables)
                    {
                        if (amount != 0)
                        {
                            PaymentApplication paymentApplication1 = new PaymentApplication();
                            if (amount <= interestDue.Balance)
                            {
                                paymentApplication1.Payment = payment4;
                                paymentApplication1.ReceivableId = interestDue.Id;
                                paymentApplication1.AmountApplied = amount;
                                interestDue.Balance -= paymentApplication1.AmountApplied;
                                amount -= paymentApplication1.AmountApplied;
                                TotalAmountApplied += paymentApplication1.AmountApplied;
                                Receivable.InsertReceivableStatus(interestDue.Id, interestDue.Balance, today);
                            }
                            else if (amount > interestDue.Balance)
                            {
                                paymentApplication1.Payment = payment4;
                                paymentApplication1.ReceivableId = interestDue.Id;
                                paymentApplication1.AmountApplied = interestDue.Balance;
                                interestDue.Balance -= paymentApplication1.AmountApplied;
                                amount -= paymentApplication1.AmountApplied;
                                TotalAmountApplied += paymentApplication1.AmountApplied;
                                Receivable.InsertReceivableStatus(interestDue.Id, interestDue.Balance, today);
                            }
                        }

                    }

                    if (amount != 0)
                    {
                        decimal amountApplied = 0;

                        if (amount <= loanAccount.LoanBalance)
                            amountApplied = amount;
                        else if (amount > loanAccount.LoanBalance)
                            amountApplied = loanAccount.LoanBalance;
                        loanAccount.LoanBalance -= amountApplied;

                        PaymentApplication paymentApplication1 = new PaymentApplication();
                        paymentApplication1.Payment = payment4;
                        paymentApplication1.FinancialAccountId = loanAccount.FinancialAccountId;
                        paymentApplication1.AmountApplied = amountApplied;
                        amount -= amountApplied;
                        TotalAmountApplied += amountApplied;

                    }

                }

                FinAcctTran finAccTrans = new FinAcctTran();
                finAccTrans.Amount = TotalAmountApplied;
                finAccTrans.FinancialAcctTransTypeId = FinlAcctTransType.AccountPaymentType.Id;
                finAccTrans.FinancialAccountId = loanAccount.FinancialAccountId;
                finAccTrans.Payment = payment4;
                finAccTrans.TransactionDate = TransactionDate;
                finAccTrans.EntryDate = today;

            }
            return amount;
        }

        private static void InsertChequeAssoc(Agreement agreement, LoanAccount loanAccount)
        {
            if (agreement != null)
            {
                var applicationWithCheques = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == agreement.ApplicationId);
                foreach (var cheque in applicationWithCheques)
                {
                    ChequeLoanAssoc chequeloanAssoc = new ChequeLoanAssoc();
                    chequeloanAssoc.Cheque = cheque.Cheque;
                    chequeloanAssoc.LoanAccount = loanAccount;
                }
            }
        }

        private void InsertReleaseStatement(LoanDisbursement loandisbursement, int customerPartyID, int id)
        {
            var ownerPartyRole = PartyRole.GetByPartyIdAndRole(customerPartyID, RoleType.OwnerFinancialType);
            var financialAccountRolesOfCustomer = Context.FinancialAccountRoles.Where
               (entity => entity.PartyRole.PartyId == customerPartyID && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id);

            var financialAccountRoles = from farc in financialAccountRolesOfCustomer
                                        join la in Context.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                        join las in Context.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                        where las.IsActive && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                        select farc;


            var loanAccounts = from far in financialAccountRoles
                                    join la in Context.LoanAccounts on far.FinancialAccountId equals la.FinancialAccountId
                                    select la;

            // Loan Release statement should be created here...
            foreach (var loanAccount in loanAccounts)
            {
               // if(loanAccount.FinancialAccountId != id)
                CreateReleaseStatement(loanAccount, loandisbursement, id);
            }
        }

        private bool SetDisbursementVcrStatus(DateTime today, LoanDisbursementVcr loanDisbursementVcr)
        {
            //Set disbursement voucher status
            var approved = Context.DisbursementVcrStatus.SingleOrDefault(entity => entity.LoanDisbursementVcr.Id == LoanDisbursementVcrId && entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.ApprovedType.Id && entity.IsActive == true);
            var partiallyDisbursedNull = Context.DisbursementVcrStatus.SingleOrDefault(entity => entity.LoanDisbursementVcr.Id == LoanDisbursementVcrId && entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id && entity.IsActive == true);
            var partiallyDisbursedRestructed = from ds in Context.DisbursementVcrStatus
                                               join fa in Context.FinancialAccounts on ds.LoanDisbursementVcr.AgreementId equals fa.AgreementId
                                               join ls in Context.LoanAccountStatus on fa.Id equals ls.FinancialAccountId
                                               where ls.IsActive == true && ls.StatusTypeId == LoanAccountStatusType.RestructuredType.Id
                                               && ds.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id
                                               && ds.IsActive == true
                                               select ds;

            if (approved != null && loanDisbursementVcr.Balance != 0)
            {
                var validIndicator = Context.DisbursementVcrStatTypeAssocs.Where(entity => entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.ApprovedType.Id && entity.DisbursementVcrStatusType1.Id == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id
                    && entity.EndDate == null).Count();
                if (validIndicator > 0)
                {
                   CreateDisbursementVcrStatus(today, approved, DisbursementVcrStatusEnum.PartiallyDisbursedType);
                   return false;

                }
            }
            else if (approved != null && loanDisbursementVcr.Balance == 0)
            {
                var validIndicator = Context.DisbursementVcrStatTypeAssocs.Where(entity => entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.ApprovedType.Id && entity.DisbursementVcrStatusType1.Id == DisbursementVcrStatusEnum.FullyDisbursedType.Id
                    && entity.EndDate == null).Count();
                if (validIndicator > 0)
                {
                     CreateDisbursementVcrStatus(today, approved, DisbursementVcrStatusEnum.FullyDisbursedType);
                     return true;

                }
            }
            else if (partiallyDisbursedNull != null && loanDisbursementVcr.Balance == 0)
            {
                var validIndicator = Context.DisbursementVcrStatTypeAssocs.Where(entity => entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id && entity.DisbursementVcrStatusType1.Id == DisbursementVcrStatusEnum.FullyDisbursedType.Id
                   && entity.EndDate == null).Count();
                if (validIndicator > 0)
                {
                    CreateDisbursementVcrStatus(today, partiallyDisbursedNull, DisbursementVcrStatusEnum.FullyDisbursedType);
                    return true;

                }
            }
            else if (partiallyDisbursedRestructed.Count() > 0 && loanDisbursementVcr.Balance != 0)
            {
                var validIndicator = Context.DisbursementVcrStatTypeAssocs.Where(entity => entity.DisbursementVcrStatusType.Id == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id && entity.DisbursementVcrStatusType1.Id == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id
                   && entity.EndDate == null).Count();
                if (validIndicator > 0)
                {
                    CreateDisbursementVcrStatus(today, partiallyDisbursedRestructed.FirstOrDefault(), DisbursementVcrStatusEnum.PartiallyDisbursedType);
                    return false;

                }
            }
            return false;
        }

        private void CreateDisbursementVcrStatus(DateTime today, DisbursementVcrStatu approved, DisbursementVcrStatusType type)
        {
            approved.IsActive = false;
            DisbursementVcrStatu disbursementvcrstatus = new DisbursementVcrStatu();
            disbursementvcrstatus.LoanDisbursementVoucherId = LoanDisbursementVcrId;
            disbursementvcrstatus.DisbursementVcrStatusType = type;
            disbursementvcrstatus.TransitionDateTime = today;
            disbursementvcrstatus.IsActive = true;
        }

        private void UpdateLoanReavailment(DateTime today, LoanDisbursement loanDisbursement, Agreement agreement)
        {
            var loanReavailments = from lr in Context.LoanReAvailments
                                   join las in Context.LoanAccountStatus on lr.FinancialAccountId equals las.FinancialAccountId
                                   where lr.ApplicationId == agreement.ApplicationId && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id
                                   || las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id || las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id) 
                                   && las.IsActive == true
                                   select lr;
            if (Deductions > 0)
            {
                foreach (var loanReavailment in loanReavailments)
                {
                    var payLoanAccount = Context.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == loanReavailment.FinancialAccountId);
                    payLoanAccount.LoanBalance = 0;

                    var activeStatus = payLoanAccount.LoanAccountStatus.FirstOrDefault(entity => entity.IsActive == true);

                    if (activeStatus.StatusTypeId == LoanAccountStatusType.CurrentType.Id
                        || activeStatus.StatusTypeId == LoanAccountStatusType.DelinquentType.Id
                        || activeStatus.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                    {
                        var isValidIndicator = Context.LoanAccountStatusTypeAssocs.Where(entity => entity.FromStatusTypeId == activeStatus.StatusTypeId
                            && entity.ToStatusTypeId == LoanAccountStatusType.PaidOffType.Id && entity.EndDate == null).Count();

                        if (isValidIndicator > 0)
                        {
                            activeStatus.IsActive = false;
                            LoanAccountStatu loanAccountStatus = new LoanAccountStatu();
                            loanAccountStatus.FinancialAccountId = loanReavailment.FinancialAccountId;
                            loanAccountStatus.StatusTypeId = LoanAccountStatusType.PaidOffType.Id;
                            loanAccountStatus.TransitionDateTime = today;
                            loanAccountStatus.IsActive = true;
                            Context.LoanAccountStatus.AddObject(loanAccountStatus);
                        }
                    }
                }
            }
        }

        private void SetCustomerStatus(DateTime today)
        {

            var partyRole = PartyRole.GetById(CustomerId);
            var customerPartyRole = PartyRole.GetByPartyIdAndRole(partyRole.PartyId, RoleType.CustomerType);
                var loanAccountStatus = Party.RetrieveOutstandingLoans(partyRole.PartyId);
                var countOfUnderLitigation = loanAccountStatus.Where(entity => entity.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id).Count();
                var countOfDelinquent = loanAccountStatus.Where(entity => entity.StatusTypeId == LoanAccountStatusType.DelinquentType.Id).Count();
                var customerstatus = Context.CustomerStatus.FirstOrDefault(entity => entity.IsActive == true && entity.PartyRoleId == customerPartyRole.Id);
                if (customerstatus != null)
                {
                    if (countOfDelinquent > 0)
                    {
                        if (customerstatus.CustomerStatusType.Id != CustomerStatusType.SubprimeType.Id)
                        {
                            customerstatus.IsActive = false;
                            CreateCustomerStatus(today, customerstatus, CustomerStatusType.SubprimeType);
                        }
                    }
                    else if (countOfUnderLitigation == 0 && countOfDelinquent > 0)
                    {
                        if (customerstatus.CustomerStatusType.Id != CustomerStatusType.DelinquentType.Id)
                        {
                            customerstatus.IsActive = false;
                            CreateCustomerStatus(today, customerstatus, CustomerStatusType.DelinquentType);
                        }
                    }
                    else if(customerstatus.CustomerStatusTypeId != CustomerStatusType.ActiveType.Id)
                    {
                        customerstatus.IsActive = false;
                        CreateCustomerStatus(today, customerstatus, CustomerStatusType.ActiveType);
                    }
                }

        }

        private void CreateCustomerStatus(DateTime today, CustomerStatu customerstatus, CustomerStatusType type)
        {
           
            CustomerStatu newCusStatus = new CustomerStatu();
            newCusStatus.CustomerStatusType = type;
            newCusStatus.IsActive = true;
            newCusStatus.TransitionDateTime = today;
            newCusStatus.PartyRoleId = CustomerId;
        }
        public void CreateReleaseStatement(LoanAccount loanAccount, LoanDisbursement loandisbursement, int loanid)
        {
            if (loanAccount.LoanBalance > 0)
            {
                decimal TotalLoanBalance = loanAccount.LoanBalance;
                var receivables = Receivable.GetInterestOutstandingReceivables(loanid);
                if (receivables.Count() != 0)
                    TotalLoanBalance += receivables.Sum(entity => entity.Balance);

                ReleaseStatement releaseStatement = new ReleaseStatement();
                releaseStatement.LoanAccount = loanAccount;
                releaseStatement.LoanDisbursement = loandisbursement;
                if (this.LoanDisbursementType.Id != LoanDisbursementType.FirstAvailment.Id && loanAccount.FinancialAccountId == loanid)
                    TotalLoanBalance -= TotalAmountToDisburse;
                releaseStatement.TotalLoanBalance = TotalLoanBalance;
            }

        }
    }
}
