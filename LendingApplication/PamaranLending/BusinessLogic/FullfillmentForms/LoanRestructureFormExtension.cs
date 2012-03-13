using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public partial class LoanRestructureForm
    {
        private LoanDisbursementVcr CreateLoanDisbursementVoucher(Agreement agreement, AmortizationItemsModel itemModel, DateTime today)
        {
            LoanDisbursementVcr voucher = new LoanDisbursementVcr();
            voucher.Agreement = agreement;
            voucher.Amount = itemModel.NewLoanAmount;
            voucher.Date = today;
            voucher.Balance = 0;

            Context.LoanDisbursementVcrs.AddObject(voucher);
            return voucher;
        }

        private LoanDisbursementVcr CreateLoanDisbursementVoucher(Agreement agreement, AmortizationItemsModel itemModel, DateTime today, decimal amountBalance)
        {
            LoanDisbursementVcr voucher = new LoanDisbursementVcr();
            voucher.Agreement = agreement;
            voucher.Amount = itemModel.NewLoanAmount;
            voucher.Date = today;
            voucher.Balance = amountBalance;

            Context.LoanDisbursementVcrs.AddObject(voucher);
            return voucher;
        }

        private DisbursementVcrStatu CreateVoucherStatus(LoanDisbursementVcr voucher, DateTime today)
        {
            DisbursementVcrStatu voucherStatus = new DisbursementVcrStatu();
            voucherStatus.DisbursementVcrStatusType = DisbursementVcrStatusEnum.FullyDisbursedType;
            voucherStatus.IsActive = true;
            voucherStatus.LoanDisbursementVcr = voucher;
            voucherStatus.TransitionDateTime = today;

            Context.DisbursementVcrStatus.AddObject(voucherStatus);
            return voucherStatus;
        }

        private DisbursementVcrStatu CreateVoucherStatus(LoanDisbursementVcr voucher, DateTime today, DisbursementVcrStatusType type)
        {
            DisbursementVcrStatu voucherStatus = new DisbursementVcrStatu();
            voucherStatus.DisbursementVcrStatusType = type;
            voucherStatus.IsActive = true;
            voucherStatus.LoanDisbursementVcr = voucher;
            voucherStatus.TransitionDateTime = today;

            Context.DisbursementVcrStatus.AddObject(voucherStatus);
            return voucherStatus;
        }

        private DisbursementVcrStatu CreateVoucherStatusWithType(LoanDisbursementVcr voucher, DateTime today, DisbursementVcrStatusType type)
        {
            DisbursementVcrStatu voucherStatus = new DisbursementVcrStatu();
            voucherStatus.DisbursementVcrStatusType = type;
            voucherStatus.IsActive = true;
            voucherStatus.LoanDisbursementVcr = voucher;
            voucherStatus.TransitionDateTime = today;

            Context.DisbursementVcrStatus.AddObject(voucherStatus);
            return voucherStatus;
        }

        private void CancelReceivables(LoanAccount loanAccount, DateTime today)
        {
            var receivables = loanAccount.Receivables.ToList();

            if (receivables.Count() != 0)
            {
                foreach (var res in receivables)
                {
                    //if (res.ReceivableTypeId == ReceivableType.PrincipalType.Id)
                    //{
                    if (res.CurrentStatus != null)
                        res.CurrentStatus.IsActive = false;

                    ReceivableStatu receivableStatus = new ReceivableStatu();
                    receivableStatus.Receivable = res;
                    receivableStatus.ReceivableStatusType = ReceivableStatusType.CancelledType;
                    receivableStatus.TransitionDateTime = today;
                    receivableStatus.IsActive = true;
                    //}
                }
            }
        }

        private void CarryOverReceivables(LoanAccount loanAccount, LoanAccount newLoanAccount, DateTime today)
        {
            var receivables = loanAccount.Receivables.ToList();

            if (receivables.Count() != 0)
            {
                foreach (var res in receivables)
                {
                    res.LoanAccount = newLoanAccount;
                }
            }
        }

        private void ChangeCustomerStatus(Customer customer, LoanAccount loanAccount, DateTime today)
        {
            var outstandingLoanAccounts = Party.RetrieveOutstandingLoans(customer.PartyRole.PartyId);
            var loanReAvailmentCustomerUnderLit = outstandingLoanAccounts.Where(entity => entity.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id).Count();
            var loanReAvailmentCustomerDelinquent = outstandingLoanAccounts.Where(entity => entity.StatusTypeId == LoanAccountStatusType.DelinquentType.Id).Count();
            //var loanReAvailmentCustomerUnderLit = loanAccount.LoanReAvailments.Where(entity => entity.LoanBalance > 0 && entity.LoanApplication.CurrentStatus.IsActive == true && entity.LoanAccount.CurrentStatus.LoanAccountStatusType.Id == LoanAccountStatusType.UnderLitigationType.Id);
            //var loanReAvailmentCustomerDelinquent = loanAccount.LoanReAvailments.Where(entity => entity.LoanBalance > 0 && entity.LoanApplication.CurrentStatus.IsActive == true && entity.LoanAccount.CurrentStatus.LoanAccountStatusType.Id == LoanAccountStatusType.DelinquentType.Id);

            //Change Customer Status
            if (customer != null)
            {
                if (customer.CurrentStatus.CustomerStatusType.Name == CustomerStatusType.SubprimeType.Name
                    && loanReAvailmentCustomerUnderLit == 0)
                {
                    if (loanReAvailmentCustomerDelinquent > 0)
                    {
                        customer.CurrentStatus.IsActive = false;

                        CustomerStatu customerStatusNew = new CustomerStatu();
                        customerStatusNew.Customer = customer;
                        customerStatusNew.CustomerStatusType = CustomerStatusType.DelinquentType;
                        customerStatusNew.TransitionDateTime = today;
                        customerStatusNew.IsActive = true;
                    }
                    else if (loanReAvailmentCustomerDelinquent == 0)
                    {
                        customer.CurrentStatus.IsActive = false;

                        CustomerStatu customerStatusNew = new CustomerStatu();
                        customerStatusNew.Customer = customer;
                        customerStatusNew.CustomerStatusType = CustomerStatusType.ActiveType;
                        customerStatusNew.TransitionDateTime = today;
                        customerStatusNew.IsActive = true;
                    }
                }
                else if (customer.CurrentStatus.CustomerStatusType.Name == CustomerStatusType.DelinquentType.Name
                   && loanReAvailmentCustomerDelinquent == 0)
                {
                    customer.CurrentStatus.IsActive = false;

                    CustomerStatu customerStatusNew = new CustomerStatu();
                    customerStatusNew.Customer = customer;
                    customerStatusNew.CustomerStatusType = CustomerStatusType.ActiveType;
                    customerStatusNew.TransitionDateTime = today;
                    customerStatusNew.IsActive = true;
                }
            }
        }

        private LoanApplicationStatu ChangeLoanApplicationStatusToCancelled(LoanApplication oldLoanApplication, DateTime today)
        {
            LoanApplicationStatu oldLoanApplicationStatus = new LoanApplicationStatu();
            oldLoanApplicationStatus.LoanApplication = oldLoanApplication;
            oldLoanApplicationStatus.LoanApplicationStatusType = LoanApplicationStatusType.CancelledType;
            oldLoanApplicationStatus.IsActive = true;
            oldLoanApplicationStatus.TransitionDateTime = today;

            return oldLoanApplicationStatus;
        }

        private Application CreateNewApplication(DateTime today)
        {
            Application applicationNew = new Application();
            applicationNew.ApplicationType1 = ApplicationType.LoanApplicationType;
            applicationNew.ApplicationDate = today;

            return applicationNew;
        }

        private Agreement CreateNewAgreementWithParent(Application application, DateTime today, FinancialAccount financialAccount)
        {
            Agreement agreement = new Agreement();
            agreement.Application = application;
            agreement.AgreementType = AgreementType.LoanAgreementType;
            agreement.EffectiveDate = today;
            agreement.AgreementDate = today;
            agreement.ParentAgreementId = financialAccount.AgreementId;

            return agreement;
        }

        private Agreement CreateNewAgreement(Application application, DateTime today)
        {
            Agreement agreement = new Agreement();
            agreement.Application = application;
            agreement.AgreementType = AgreementType.LoanAgreementType;
            agreement.EffectiveDate = today;
            agreement.AgreementDate = today;

            return agreement;
        }

        private LoanApplication CreateLoanApplication(Application application, DateTime today, AmortizationItemsModel itemModel)
        {
            LoanApplication loanApplicationNew1 = new LoanApplication();
            loanApplicationNew1.Application = application;
            loanApplicationNew1.InterestRate = itemModel.InterestRate;
            loanApplicationNew1.LoanTermUomId = itemModel.UnitId;
            loanApplicationNew1.LoanTermLength = itemModel.Term;
            loanApplicationNew1.PaymentModeUomId = itemModel.PaymentModeId;
            loanApplicationNew1.LoanAmount = itemModel.NewLoanAmount;
            loanApplicationNew1.InterestRateDescription = itemModel.InterestRateDescription;
            loanApplicationNew1.IsInterestProductFeatureInd = true;

            return loanApplicationNew1;
        }

        private AgreementItem CreateAgreementItem(Agreement agreement, DateTime today, AmortizationItemsModel item)
        {
            AgreementItem agreementItemNew1 = new AgreementItem();
            agreementItemNew1.Agreement = agreement;
            agreementItemNew1.InterestComputationMode = item.InterestComputationMode;
            agreementItemNew1.InterestRateDescription = item.InterestRateDescription;
            agreementItemNew1.InterestRate = item.InterestRate;
            agreementItemNew1.LoanAmount = item.NewLoanAmount;
            agreementItemNew1.LoanTermLength = item.Term;
            agreementItemNew1.LoanTermUom = item.Unit;
            agreementItemNew1.MethodOfChargingInterest = item.MethodOfChargingInterest;
            agreementItemNew1.PaymentMode = item.PaymentMode;
            agreementItemNew1.TransitionDateTime = today;
            agreementItemNew1.IsActive = true;

            return agreementItemNew1;
        }

        private AgreementItem CreateAgreementItemFromOld(Agreement agreement, DateTime today, AmortizationItemsModel item, AgreementItem oldItem)
        {
            AgreementItem agreementItemNew1 = new AgreementItem();
            agreementItemNew1.Agreement = agreement;
            agreementItemNew1.InterestComputationMode = oldItem.InterestComputationMode;
            agreementItemNew1.InterestRateDescription = oldItem.InterestRateDescription;
            agreementItemNew1.InterestRate = oldItem.InterestRate;
            if (item != null)
                agreementItemNew1.LoanAmount = item.NewLoanAmount;
            else
                agreementItemNew1.LoanAmount = oldItem.LoanAmount;
            agreementItemNew1.LoanTermLength = oldItem.LoanTermLength;
            agreementItemNew1.LoanTermUom = oldItem.LoanTermUom;
            agreementItemNew1.MethodOfChargingInterest = oldItem.MethodOfChargingInterest;
            agreementItemNew1.PaymentMode = oldItem.PaymentMode;
            agreementItemNew1.TransitionDateTime = today;
            agreementItemNew1.IsActive = true;

            return agreementItemNew1;
        }

        private AgreementItem CreateAgreementItemFromOldInterest(Agreement agreement, DateTime today, AmortizationItemsModel item, AgreementItem oldItem)
        {
            AgreementItem agreementItemNew1 = new AgreementItem();
            agreementItemNew1.Agreement = agreement;
            agreementItemNew1.InterestComputationMode = oldItem.InterestComputationMode;
            agreementItemNew1.InterestRateDescription = oldItem.InterestRateDescription;
            agreementItemNew1.InterestRate = item.NewInterestRate;
            agreementItemNew1.LoanAmount = oldItem.LoanAmount;
            agreementItemNew1.LoanTermLength = oldItem.LoanTermLength;
            agreementItemNew1.LoanTermUom = oldItem.LoanTermUom;
            agreementItemNew1.MethodOfChargingInterest = oldItem.MethodOfChargingInterest;
            agreementItemNew1.PaymentMode = oldItem.PaymentMode;
            agreementItemNew1.TransitionDateTime = today;
            agreementItemNew1.IsActive = true;

            return agreementItemNew1;
        }

        private AgreementItem CreateAgreementItemFromOldIcm(Agreement agreement, DateTime today, AmortizationItemsModel item, AgreementItem oldItem)
        {
            AgreementItem agreementItemNew1 = new AgreementItem();
            agreementItemNew1.Agreement = agreement;
            agreementItemNew1.InterestComputationMode = item.InterestComputationMode;
            agreementItemNew1.InterestRateDescription = oldItem.InterestRateDescription;
            agreementItemNew1.InterestRate = oldItem.InterestRate;
            agreementItemNew1.LoanAmount = oldItem.LoanAmount;
            agreementItemNew1.LoanTermLength = oldItem.LoanTermLength;
            agreementItemNew1.LoanTermUom = oldItem.LoanTermUom;
            agreementItemNew1.MethodOfChargingInterest = oldItem.MethodOfChargingInterest;
            agreementItemNew1.PaymentMode = oldItem.PaymentMode;
            agreementItemNew1.TransitionDateTime = today;
            agreementItemNew1.IsActive = true;

            return agreementItemNew1;
        }

        private AgreementRole CreateAgreementRole(Agreement agreement, PartyRole role)
        {
            AgreementRole agreementRole = new AgreementRole();
            agreementRole.Agreement = agreement;
            agreementRole.PartyRole = role;

            return agreementRole;
        }

        private LoanApplicationStatu CreateLoanApplicationStatus(LoanApplication loanApplication, DateTime today)
        {
            LoanApplicationStatu loanApplicationStatus = new LoanApplicationStatu();
            loanApplicationStatus.LoanApplication = loanApplication;
            loanApplicationStatus.LoanApplicationStatusType = LoanApplicationStatusType.ClosedType;
            loanApplicationStatus.TransitionDateTime = today;
            loanApplicationStatus.IsActive = true;

            Context.LoanApplicationStatus.AddObject(loanApplicationStatus);
            return loanApplicationStatus;
        }

        private LoanAccount CreateLoanAccount(FinancialAccount financialAccount, AmortizationItemsModel item, DateTime maturityDate, DateTime today)
        {
            LoanAccount loanAccountNew = new LoanAccount();
            loanAccountNew.FinancialAccount = financialAccount;
            loanAccountNew.LoanAmount = item.NewLoanAmount;
            loanAccountNew.LoanBalance = item.NewLoanAmount;
            //loanAccountNew.LoanReleaseDate = today.Date;
            loanAccountNew.LoanReleaseDate = item.LoanReleaseDate;
            loanAccountNew.InterestType = InterestType.PercentageInterestTYpe;
            if(item.Term != 0)
                loanAccountNew.MaturityDate = maturityDate;

            return loanAccountNew;
        }

        private LoanAccount CreateAdditionalLoanAccount(FinancialAccount financialAccount, AmortizationItemsModel item, DateTime maturityDate, DateTime today)
        {
            LoanAccount loanAccountNew = new LoanAccount();
            loanAccountNew.FinancialAccount = financialAccount;
            loanAccountNew.LoanAmount = 0;
            loanAccountNew.LoanBalance = 0;
            loanAccountNew.LoanReleaseDate = today.Date;
            loanAccountNew.InterestType = InterestType.PercentageInterestTYpe;
            if (item.Term != 0)
                loanAccountNew.MaturityDate = maturityDate;

            return loanAccountNew;
        }

        private LoanAccountStatu CreateLoanAccountStatus(LoanAccount loanAccount, DateTime today)
        {
            LoanAccountStatu loanAccountStatus1 = new LoanAccountStatu();
            loanAccountStatus1.LoanAccount = loanAccount;
            loanAccountStatus1.LoanAccountStatusType = LoanAccountStatusType.CurrentType;
            loanAccountStatus1.TransitionDateTime = today;
            loanAccountStatus1.IsActive = true;

            return loanAccountStatus1;
        }

        private InterestItem CreateInterestItem(LoanAccount loanAccount, decimal newInterest, DateTime today)
        {
            InterestItem newInterestItem = new InterestItem();
            newInterestItem.LoanAccount = loanAccount;
            newInterestItem.Amount = newInterest;
            newInterestItem.IsActive = true;

            return newInterestItem;
        }

        private FinancialAccount CreateFinancialAccountWithParent(Agreement agreement, DateTime today, FinancialAccount financialAccountParent)
        {
            FinancialAccount financialAccountNew1 = new FinancialAccount();
            financialAccountNew1.Agreement = agreement;
            financialAccountNew1.FinancialAccountType = FinancialAccountType.LoanAccountType;
            financialAccountNew1.ParentFinancialAccountId = financialAccountParent.Id;

            return financialAccountNew1;
        }

        private FinancialAccount CreateFinancialAccount(Agreement agreement, DateTime today)
        {
            FinancialAccount financialAccountNew1 = new FinancialAccount();
            financialAccountNew1.Agreement = agreement;
            financialAccountNew1.FinancialAccountType = FinancialAccountType.LoanAccountType;

            return financialAccountNew1;
        }

        private FinancialAccountRole CreateFinancialAccountRole(FinancialAccount financialAccount, PartyRole role)
        {
            FinancialAccountRole financialAccountRole = new FinancialAccountRole();
            financialAccountRole.FinancialAccount = financialAccount;
            financialAccountRole.PartyRole = role;

            return financialAccountRole;
        }

        private FinancialAccountProduct CreateFinancialAccountProduct(FinancialAccount financialAccount, FinancialProduct financialProduct, DateTime today)
        {
            FinancialAccountProduct financialAccountProductNew1 = new FinancialAccountProduct();
            financialAccountProductNew1.FinancialAccount = financialAccount;
            financialAccountProductNew1.FinancialProduct = financialProduct;
            financialAccountProductNew1.EffectiveDate = today;

            return financialAccountProductNew1;
        }

        private LoanModification CreateLoanModification(PartyRole role, LoanModificationType type)
        {
            LoanModification loanModification = new LoanModification();
            loanModification.PartyRole = role;
            loanModification.LoanModificationType = type;

            return loanModification;
        }

        private LoanModificationStatu CreateLoanModificationStatus(LoanModification loanModification, DateTime today)
        {
            LoanModificationStatu loanModificationStatus = new LoanModificationStatu();
            loanModificationStatus.LoanModificationDatetime = today;
            loanModificationStatus.LoanModification = loanModification;
            loanModificationStatus.LoanModificationStatusType = LoanModificationStatusType.ApprovedType;
            loanModificationStatus.IsActive = true;

            return loanModificationStatus;
        }

        private LoanModificationPrevItem CreateLoanModificationPrevItem(LoanModification loanModification, FinancialAccount financialAccount)
        {
            LoanModificationPrevItem loanModificationPrevItem = new LoanModificationPrevItem();
            loanModificationPrevItem.FinancialAccount = financialAccount;
            loanModificationPrevItem.LoanModification = loanModification;

            return loanModificationPrevItem;
        }

        private AmortizationSchedule CreateAmortizationSchedule(LoanAgreement loanAgreement, AmortizationItemsModel schedule, DateTime today)
        {
            AmortizationSchedule amortizationScheduleNew1 = new AmortizationSchedule();
            amortizationScheduleNew1.LoanAgreement = loanAgreement;
            amortizationScheduleNew1.LoanReleaseDate = today.Date;
            amortizationScheduleNew1.PaymentStartDate = schedule.PaymentStartDate;
            amortizationScheduleNew1.DateGenerated = today;
            amortizationScheduleNew1.EffectiveDate = today;

            return amortizationScheduleNew1;
        }

        private AmortizationSchedule CreateAmortizationScheduleWithParent(LoanAgreement loanAgreement, AmortizationItemsModel schedule, DateTime today, AmortizationSchedule parentSchedule)
        {
            AmortizationSchedule amortizationScheduleNew1 = new AmortizationSchedule();
            amortizationScheduleNew1.LoanAgreement = loanAgreement;
            amortizationScheduleNew1.LoanReleaseDate = schedule.LoanReleaseDate;
            amortizationScheduleNew1.PaymentStartDate = schedule.PaymentStartDate;
            amortizationScheduleNew1.DateGenerated = today;
            amortizationScheduleNew1.EffectiveDate = today;
            amortizationScheduleNew1.ParentAmortizationScheduleId = parentSchedule.Id;

            return amortizationScheduleNew1;
        }

        private AmortizationScheduleItem CreateAmortizationScheduleItem(AmortizationSchedule schedule, DateTime today, AmortizationScheduleModel item)
        {
            AmortizationScheduleItem amortizationItem = new AmortizationScheduleItem();

            amortizationItem.AmortizationSchedule = schedule;
            amortizationItem.InterestPayment = item.InterestPayment;
            amortizationItem.IsBilledIndicator = item.IsBilledIndicator;
            amortizationItem.PrincipalBalance = item.PrincipalBalance;
            amortizationItem.PrincipalPayment = item.PrincipalPayment;
            amortizationItem.ScheduledPaymentDate = item.ScheduledPaymentDate;
            amortizationItem.TotalLoanBalance = item.TotalLoanBalance;

            return amortizationItem;
        }

        public decimal ComputeNewLoanBalance(LoanAccount loanAccount, decimal newInterestRate, string type, string interestRateDescription)
        {
            decimal newLoanBalance = 0;
            var loanAmount = loanAccount.LoanAmount;
            var agreementItem = loanAccount.FinancialAccount.Agreement.AgreementItems.FirstOrDefault(entity => entity.IsActive == true);
            
            newLoanBalance = loanAmount;
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity => entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).ToList();

            if (payments.Count() != 0)
            {
                foreach (var pay in payments)
                {
                    var balance = 0M;
                    if (type == ProductFeature.DiminishingBalanceMethodType.Name)
                        balance = ComputeInterestAmount(newLoanBalance, newInterestRate, interestRateDescription); //11/09/11
                        //balance = ComputeInterestAmount(newLoanBalance, newInterestRate, interestRateDescription, pay.TransactionDate, agreementItem, loanAccount, DateTime.Now);
                    else
                        balance = ComputeInterestAmount(loanAmount, newInterestRate, interestRateDescription);
                        //balance = ComputeInterestAmount(loanAmount, newInterestRate, interestRateDescription, pay.TransactionDate, agreementItem, loanAccount, DateTime.Now);
                    newLoanBalance = (newLoanBalance + balance);
                    var amountPaid = pay.Amount;
                    newLoanBalance -= amountPaid;
                }
            }
            return newLoanBalance;
        }

        public decimal ComputeNewLoanBalance(LoanAccount loanAccount, decimal receivableAdd, decimal newInterestRate, string type, string interestRateDescription)
        {
            var agreementItem = new AgreementItem();

            decimal newLoanBalance = 0;
            var loanAmount = loanAccount.LoanAmount;
            loanAmount += receivableAdd;

            newLoanBalance = loanAmount;
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity => entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).ToList();
            var groupedPayments = from p in payments
                                  group p by p.TransactionDate.Month into g
                                  select new { Amount = g.Sum(entity => entity.Amount) };
            if (payments.Count() != 0)
            {
                foreach (var pay in payments)
                {
                    var balance = 0M;
                    if (type == ProductFeature.DiminishingBalanceMethodType.Name)
                        balance = ComputeInterestAmount(newLoanBalance, newInterestRate, interestRateDescription);
                    else
                        balance = ComputeInterestAmount(loanAmount, newInterestRate, interestRateDescription);
                    newLoanBalance = (newLoanBalance + balance);
                    var amountPaid = pay.Amount;
                    newLoanBalance -= amountPaid;
                }
            }
            return newLoanBalance;
        }

        public decimal ComputeNewLoanBalance(decimal loanAmount, decimal less, decimal newInterestRate, string type, string interestRateDescription)
        {
            decimal newLoanBalance = 0;

            newLoanBalance = loanAmount;

            var balance = 0M;
            if (type == ProductFeature.DiminishingBalanceMethodType.Name)
                balance = ComputeInterestAmount(newLoanBalance, newInterestRate, interestRateDescription);
            else
                balance = ComputeInterestAmount(loanAmount, newInterestRate, interestRateDescription);
            newLoanBalance = (newLoanBalance + balance);
            var amountPaid = less;
            newLoanBalance -= amountPaid;

            return newLoanBalance;
        }

        public decimal ComputeNewLoanBalance(LoanAccount loanAccount, decimal loanAmount, decimal newInterestRate, string type, int count, bool isFirst, string interestRateDescription)
        {
            decimal newLoanBalance = 0;
            //var loanAmount = loanAccount.LoanAmount;

            newLoanBalance = loanAmount;
            var baseCount = count;
            int paymentsCount = GetPaymentsTotalCount(loanAccount);
            int startAt = 1;
            if (!isFirst)
            {
                baseCount = paymentsCount;
                startAt = (paymentsCount - count) + 1;
            }
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity => entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).ToList();
            if (payments.Count() != 0)
            {
                var counter = 0;
                foreach (var pay in payments)
                {
                    counter++;
                    if ( counter >= startAt && counter <= baseCount)
                    {
                        var balance = 0M;
                        if (type == ProductFeature.DiminishingBalanceMethodType.Name)
                            balance = ComputeInterestAmount(newLoanBalance, newInterestRate, interestRateDescription);
                        else
                            balance = ComputeInterestAmount(loanAmount, newInterestRate, interestRateDescription);
                        newLoanBalance = (newLoanBalance + balance);
                        var amountPaid = pay.Amount;
                        newLoanBalance -= amountPaid;
                    }
                }
            }
            return newLoanBalance;
        }

        private decimal ComputeInterestAmount(decimal amount, decimal interestRate, string interestRateDescription)
        {
            var paymentModeId = UnitOfMeasure.MonthlyType.Id;
            var interestRateDescriptionId = 0;
            if (interestRateDescription == ProductFeature.MonthlyInterestRateType.Name)
                interestRateDescriptionId = UnitOfMeasure.MonthsType.Id;
            else if (interestRateDescription == ProductFeature.AnnualInterestRateType.Name)
                interestRateDescriptionId = UnitOfMeasure.YearsType.Id;

            var multiplier = (TimeUnitConversion.GetMultiplier(interestRateDescriptionId, paymentModeId) / TimeUnitConversion.GetOffset(interestRateDescriptionId, paymentModeId));

            var interestAmount = amount * (interestRate / 100) / multiplier;
            //if (interestRateDescription == ProductFeature.MonthlyInterestRateType.Name)
            //    interestAmount = amount * (interestRate / 100);
            //else if (interestRateDescription == ProductFeature.AnnualInterestRateType.Name)
            //    interestAmount = (amount * (interestRate / 100)) / 360;

            return interestAmount;
        }

        private decimal ComputeInterestAmount(decimal amount, 
                                                decimal interestRate, 
                                                string interestRateDescription, 
                                                DateTime paymentDate, 
                                                AgreementItem agreementItem, 
                                                LoanAccount loanAccount,
                                                DateTime today)
        {

            agreementItem.InterestRate = interestRate;
            var newLoanAccount = new LoanAccount();
            newLoanAccount = loanAccount;
            //newLoanAccount.LoanBalance = amount;

            decimal interestAmount = GenerateBillFacade.GenerateAndSaveInterest(newLoanAccount, 
                                                                                    agreementItem, 
                                                                                    GenerateBillFacade.ManualBillingDisplay, 
                                                                                    paymentDate, 
                                                                                    paymentDate, 
                                                                                    today);

            return interestAmount;
        }

        public decimal ComputeTotalLoanReceivables(LoanAccount loanAccount)
        {
            var totalReceivable = 0M;
            var rec = loanAccount.Receivables;

            var receivables = from r in rec
                              join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                              where rs.IsActive = true && (rs.StatusTypeId == ReceivableStatusType.OpenType.Id
                                        || rs.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id)
                              select r;

            if (receivables.Count() != 0)
            {
                totalReceivable = receivables.Sum(entity => entity.Balance);
            }

            return totalReceivable;
        }

        private DateTime? GetLastPaymentDate(LoanAccount loanAccount)
        {
            var lastPaymentDate = new DateTime?();
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity => 
                                        entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).OrderByDescending(entity => 
                                                    entity.TransactionDate).ToList().FirstOrDefault();

            if (payments != null)
                lastPaymentDate = payments.TransactionDate;
            else
                lastPaymentDate = null;

            return lastPaymentDate;
        }

        public decimal GetPaymentsTotal(LoanAccount loanAccount)
        {
            decimal totalPayments = 0M;
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity =>
                                        entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).ToList();
            totalPayments = payments.Sum(entity => entity.Amount);
            return totalPayments;
        }

        public int GetPaymentsTotalCount(LoanAccount loanAccount)
        {
            int totalPayments = 0;
            var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity =>
                                        entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).ToList();
            totalPayments = payments.Count();
            return totalPayments;
        }

        public decimal GetLessPayment(LoanAccount loanAccount, int count, bool isFirst)
        {
            decimal totalPayments = 0M;
            if (isFirst)
            {
                var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity =>
                                            entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).Take(count).ToList();
                totalPayments = payments.Sum(entity => entity.Amount);
            }
            else
            {
                var paymentCount = GetPaymentsTotalCount(loanAccount);
                count = paymentCount - count;
                var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity =>
                                            entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).Skip(count).ToList();
                totalPayments = payments.Sum(entity => entity.Amount);
            }
            return totalPayments;
        }

        public static void SetChequeStatusToCancelled(FinancialAccount financialAccount, DateTime today)
        {
            var loanAccount = financialAccount.LoanAccount;
            var application = financialAccount.Agreement.Application;

            var chequesAssoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == application.Id).ToList();

            foreach (var cheque in chequesAssoc)
            {
                var check = cheque.Cheque;
                var currentChequeStatus = check.CurrentStatus;
                currentChequeStatus.IsActive = false;

                ChequeStatu newChequeStatus = CreateCancelledChequeStatus(check, today);

                var payment = check.Payment;
                //var receipt = payment.Receipt;
                var receipt = Context.ReceiptPaymentAssocs.FirstOrDefault(entity => entity.PaymentId == payment.Id).Receipt;
                
                var receiptCurrentStatus = receipt.CurrentStatus;
                receiptCurrentStatus.IsActive = false;

                ReceiptStatu newReceiptStatus = CreateCancelledReceiptStatus(receipt, today);
            }
        }

        private static ChequeStatu CreateCancelledChequeStatus(Cheque cheque, DateTime today)
        {
            ChequeStatu chequeStatus = new ChequeStatu();
            chequeStatus.Cheque = cheque;
            chequeStatus.ChequeStatusType = ChequeStatusType.CancelledType;
            chequeStatus.Remarks = "Cancelled due to loan restructure";
            chequeStatus.TransitionDateTime = today;
            chequeStatus.IsActive = true;

            Context.ChequeStatus.AddObject(chequeStatus);

            return chequeStatus;
        }

        private static ReceiptStatu CreateCancelledReceiptStatus(Receipt receipt, DateTime today)
        {
            ReceiptStatu receiptStatus = new ReceiptStatu();
            receiptStatus.Receipt = receipt;
            receiptStatus.ReceiptStatusType = ReceiptStatusType.CancelledReceiptStatusType;
            receiptStatus.Remarks = "Cancelled due to loan restructure";
            receiptStatus.TransitionDateTime = today;
            receiptStatus.IsActive = true;

            Context.ReceiptStatus.AddObject(receiptStatus);

            return receiptStatus;
        }

        public Payment CreatePayment(PartyRole customerPartyRole, int employeePartyRoleId, ChequeModel model, DateTime today)
        {
            //Payment newPayment = new Payment();
            //newPayment.ProcessedByPartyRoleId = employeePartyRoleId;
            //newPayment.ProcessedToPartyRoleId = customerPartyRole.Id;
            //newPayment.PaymentType = PaymentType.Receipt;
            //newPayment.PaymentMethodType = PaymentMethodType.PersonalCheckType;
            //newPayment.TransactionDate = model.TransactionDate.Date;
            //newPayment.EntryDate = today;
            //newPayment.TotalAmount = model.Amount;
            //newPayment.PaymentReferenceNumber = model.ChequeNumber;

            Payment newPayment = Payment.CreatePayment(today, model.TransactionDate.Date, customerPartyRole.Id, 
                                                        employeePartyRoleId, model.Amount, PaymentType.Receipt, 
                                                        PaymentMethodType.PersonalCheckType, SpecificPaymentType.LoanPaymentType, 
                                                        model.ChequeNumber);

            Context.Payments.AddObject(newPayment);

            return newPayment;
        }

        public Receipt CreateReceipt(PartyRole customerPartyRole, Payment payment, ChequeModel model)
        {
            Receipt newReceipt = new Receipt();
            newReceipt.ReceiptBalance = model.Amount;
            Context.Receipts.AddObject(newReceipt);
            
            
            return newReceipt;
        }

        public ReceiptPaymentAssoc CreateReceiptPaymentAssoc(Payment payment, Receipt receipt)
        {

            ReceiptPaymentAssoc newReceiptAssoc = new ReceiptPaymentAssoc();
            newReceiptAssoc.Payment = payment;
            newReceiptAssoc.Receipt = receipt;

            Context.ReceiptPaymentAssocs.AddObject(newReceiptAssoc);

            return newReceiptAssoc;
        }

        private void CreateReceiptStatus(Receipt receipt, ChequeModel model, DateTime today)
        {
            ReceiptStatu receiptStatus = new ReceiptStatu();
            receiptStatus.Receipt = receipt;
            receiptStatus.ReceiptStatusType = ReceiptStatusType.OpenReceiptStatusType;
            receiptStatus.Remarks = model.Remarks;
            receiptStatus.TransitionDateTime = today;
            receiptStatus.IsActive = true;

            Context.ReceiptStatus.AddObject(receiptStatus);
        }

        private void CreateChequeStatus(Cheque cheque, ChequeModel model, DateTime today)
        {
            ChequeStatu chequeStatus = new ChequeStatu();
            chequeStatus.Cheque = cheque;
            chequeStatus.ChequeStatusType = ChequeStatusType.ReceivedType;
            chequeStatus.Remarks = model.Remarks;
            chequeStatus.TransitionDateTime = today;
            chequeStatus.IsActive = true;

            Context.ChequeStatus.AddObject(chequeStatus);
        }

        private ApplicationItem CreateApplicationItem(Application application, ProductFeatureApplicability productFeature, DateTime today)
        {
            ApplicationItem applicationItem = new ApplicationItem();
            applicationItem.Application = application;
            applicationItem.ProductFeatureApplicability = productFeature;
            applicationItem.EffectiveDate = today;

            return applicationItem;
        }

        private ProductFeatureApplicability CreateOrRetrieveInterestRate(int featureId, int financialProductId, decimal rate)
        {
            ProductFeatureApplicability featureApplicability;
            ProductFeature feature = ProductFeature.GetById(featureId);
            FinancialProduct financialProduct = FinancialProduct.GetById(financialProductId);
            var pfaSameValue = from pfa in ProductFeatureApplicability.GetAllActive(feature, financialProduct)
                               where pfa.Value.HasValue && pfa.Value.Value == rate
                               select pfa;
            if (pfaSameValue.Count() > 0)
                featureApplicability = pfaSameValue.First();
            else
            {
                featureApplicability = ProductFeatureApplicability.Create(feature, financialProduct, rate, DateTime.Today);
            }
            return featureApplicability;
        }

        private void ChequeModelPrepareForSave(FinancialAccount financialAccount, PartyRole customerPartyRole, int employeePartyRoleId, DateTime today, ChequeModel model)
        {
            if (model.IsNew)
            {
                var application = financialAccount.Agreement.Application;
                var loanAccount = financialAccount.LoanAccount;

                //new payment
                Payment payment = CreatePayment(customerPartyRole, employeePartyRoleId, model, today);

                //new cheque
                Cheque newCheck = new Cheque();
                newCheck.BankPartyRoleId = model.BankId;
                newCheck.CheckDate = model.ChequeDate;
                newCheck.Payment = payment; 

                //new cheque association
                ChequeApplicationAssoc chequeAssoc = new ChequeApplicationAssoc();
                chequeAssoc.Cheque = newCheck;
                chequeAssoc.Application = application;

                //new cheque loan association
                ChequeLoanAssoc chequeLoanAssoc = new ChequeLoanAssoc();
                chequeLoanAssoc.Cheque = newCheck;
                chequeLoanAssoc.LoanAccount = loanAccount;

                //new receipt
                Receipt newReceipt = CreateReceipt(customerPartyRole, payment, model);

                //new receipt payment assoc
                ReceiptPaymentAssoc newReceiptAssoc = CreateReceiptPaymentAssoc(payment, newReceipt);

                //new receipt status
                CreateReceiptStatus(newReceipt, model, today);

                //new cheque status
                CreateChequeStatus(newCheck, model, today);

                Context.Cheques.AddObject(newCheck);
            }
            else if (model.ToBeDeleted)
            {
                //throw new NotImplementedException();
                var payment = Context.Payments.SingleOrDefault(entity => entity.Id == model.PaymentId);
                var receiptPaymentAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == payment.Id);
                var receipts = receiptPaymentAssoc.Receipt;
                var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == model.ChequeId && entity.PaymentId == payment.Id);

                var assoc = Context.ChequeApplicationAssocs.SingleOrDefault(entity => entity.ChequeId == cheque.Id);

                Context.ReceiptPaymentAssocs.DeleteObject(receiptPaymentAssoc);
                Context.Receipts.DeleteObject(receipts);
                Context.ChequeApplicationAssocs.DeleteObject(assoc);
                Context.Cheques.DeleteObject(cheque);
                Context.Payments.DeleteObject(payment);
            }
        }
    }
}