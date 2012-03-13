using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using Ext.Net;

namespace BusinessLogic.FullfillmentForms
{
    public class LoanApplicationOperations
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public static void Cancel(LoanApplication loanApplication, DateTime today, int processedByPartyId) 
        {
            if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.CancelledType))
            {
                RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
                PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
                PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);
                var customerStatus = customerPartyRole.Customer.CurrentStatus;
                //if customer status is active
                if (customerStatus.CustomerStatusType == CustomerStatusType.ActiveType)
                {
                    if ((CheckIfHasNoOutstandingLoans(borrowerRoleType, customerPartyRole)) && (CheckIfHasNoLoanApplications(loanApplication, borrowerRoleType, customerPartyRole.Party)))
                        CustomerStatu.ChangeStatus(customerPartyRole, CustomerStatusType.InactiveType, today); //change customer status
                    //else do not update customer status
                }
                //else: do not update customer status
                var status = loanApplication.CurrentStatus;
                if (status.StatusTypeId == LoanApplicationStatusType.ApprovedType.Id
                    || status.StatusTypeId == LoanApplicationStatusType.PendingApprovalType.Id)
                {
                    var agreement = loanApplication.Application.Agreements.FirstOrDefault(entity => entity.EndDate == null);
                    if (agreement != null)
                    {
                        var vcrStatuses = from vcr in agreement.LoanDisbursementVcrs
                                          join vcrStatus in Context.DisbursementVcrStatus on vcr.Id equals vcrStatus.LoanDisbursementVoucherId
                                          where vcrStatus.IsActive == true &&
                                          (vcrStatus.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PendingType.Id
                                          || vcrStatus.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.ApprovedType.Id
                                          || vcrStatus.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.RejectedType.Id)
                                          select vcrStatus;

                        foreach (var vcrStatus in vcrStatuses)
                        {
                            vcrStatus.IsActive = false;
                        }

                        foreach (var vcr in agreement.LoanDisbursementVcrs)
                        {
                            var vcrStatus = DisbursementVcrStatu.Create(vcr, DisbursementVcrStatusEnum.CancelledType, today);
                            vcrStatus.Remarks = "Loan Application is cancelled";
                        }
                    }
                }

                LoanApplicationStatu.ChangeStatus(loanApplication, LoanApplicationStatusType.CancelledType, today);

                //Insert loan clerk who processed the application
                if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
                {
                    var party = Party.GetById(processedByPartyId);
                    PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                        (loanApplication, party, RoleType.ProcessedByApplicationType, today);
                }

                //Cancel Cheques associated and receipts
                var remarks = "Loan Application was cancelled.";
                var assoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
                foreach (var item in assoc)
                {
                    //TODO:: Changed to new payment.
                    var cheque = Cheque.GetById(item.ChequeId);
                    var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == cheque.PaymentId);
                    var receipt = rpAssoc.Receipt;
                    
                    ChequeStatu.ChangeStatus(cheque, ChequeStatusType.CancelledType, remarks);
                    ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.CancelledReceiptStatusType, remarks);
                }
            }
        }

        public static void Close(LoanApplication loanApplication, DateTime today, int processedByPartyId)
        {
            LoanApplicationStatu.ChangeStatus(loanApplication, LoanApplicationStatusType.ClosedType, today);
            //Insert loan clerk who processed the application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                var party = Party.GetById(processedByPartyId);
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplication, party, RoleType.ProcessedByApplicationType, today);
            }
        }

        public static void Reject(LoanApplication loanApplication, DateTime today, int processedByPartyId)
        {
            if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.RejectedType))
            {
                var newStatus = LoanApplicationStatu.ChangeStatus(loanApplication, LoanApplicationStatusType.RejectedType, today);
                
                //Insert loan clerk who processed the application
                if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
                {
                    var party = Party.GetById(processedByPartyId);
                    PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                        (loanApplication, party, RoleType.ProcessedByApplicationType, today);
                }

                RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
                PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
                PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);
                var customerStatus = customerPartyRole.Customer.CurrentStatus;
                //if customer status is active
                if (customerStatus.CustomerStatusType == CustomerStatusType.ActiveType)
                {
                    if ((CheckIfHasNoOutstandingLoans(borrowerRoleType, customerPartyRole)) && (CheckIfHasNoLoanApplications(loanApplication, borrowerRoleType, customerPartyRole.Party)))
                        CustomerStatu.ChangeStatus(customerPartyRole, CustomerStatusType.InactiveType, today); //change customer status
                    //else do not update customer status
                }
                //else: do not update customer status

                //Cancel Cheques associated and receipts
                var remarks = "Loan Application was rejected.";
                var assoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
                foreach (var item in assoc)
                {
                    //TODO:: Changed to new payment.
                    var cheque = Cheque.GetById(item.ChequeId);
                    var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == cheque.PaymentId);
                    var receipt = rpAssoc.Receipt;

                    ChequeStatu.ChangeStatus(cheque, ChequeStatusType.CancelledType, remarks);
                    ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.CancelledReceiptStatusType, remarks);
                }
            }
        }

        //if customer has no loan application with status of: //‘Pending: Approval’, ‘Approved’ or ‘Pending: In Funding’
        private static bool CheckIfHasNoLoanApplications(LoanApplication loanApplication, RoleType borrowerRoleType, Party party)
        {
            var allLoanApplications = LoanApplication.GetAllLoanApplicationOf(party, borrowerRoleType); //get all loan applications under the borrower
            var all = from application in allLoanApplications
                      join status in Context.LoanApplicationStatus on application.ApplicationId equals status.ApplicationId
                      where status.IsActive && (status.StatusTypeId == LoanApplicationStatusType.PendingApprovalType.Id
                      || status.StatusTypeId == LoanApplicationStatusType.ApprovedType.Id
                      || status.StatusTypeId == LoanApplicationStatusType.PendingInFundingType.Id)
                      && application.ApplicationId != loanApplication.ApplicationId
                      select application;

            return all.Count() == 0;
        }

        //if customer has no outstandling loans
        private static bool CheckIfHasNoOutstandingLoans(RoleType borrowerRoleType, PartyRole customerPartyRole)
        {
            var financialAccountRoles = Context.FinancialAccountRoles.Where(entity => entity.PartyRoleId == customerPartyRole.Id);

            if(financialAccountRoles.Count() == 0)
                return true;

            var all = from far in financialAccountRoles
                      join fa in Context.FinancialAccounts on far.FinancialAccountId equals fa.Id
                      join agreement in Context.Agreements on fa.AgreementId equals agreement.Id
                      join agreementRole in Context.AgreementRoles on agreement.Id equals agreementRole.AgreementId
                      join pr in Context.PartyRoles on agreementRole.PartyRoleId equals pr.Id
                      join prt in Context.RoleTypes on pr.PartyRoleType.RoleType.Id equals prt.Id
                      join ams in Context.AmortizationSchedules on agreement.Id equals ams.AgreementId
                      join amsi in Context.AmortizationScheduleItems on ams.Id equals amsi.AmortizationScheduleId
                      where ams.EndDate == null && agreement.EndDate == null && prt.Id == RoleType.BorrowerAgreementType.Id
                      select far;

            return all.Count() == 0;
        }

        public static void Approve(LoanApplication loanApplication, DateTime today, DateTime loanReleaseDate, DateTime paymentStartDate)
        {
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);

            LoanApplicationStatu loanStatus = LoanApplicationStatu.CreateOrUpdateCurrent(loanApplication, LoanApplicationStatusType.ApprovedType, today);

            var agreement = Agreement.Create(loanApplication, AgreementType.LoanAgreementType, today);

            //Create new Loan Agreement Record
            LoanAgreement loanAgreement = new LoanAgreement();
            loanAgreement.Agreement = agreement;
            Context.LoanAgreements.AddObject(loanAgreement);

            var agreementItem = Create(loanApplication, agreement);
            Context.AgreementItems.AddObject(agreementItem);

            var borrower = RoleType.BorrowerAgreementType;
            var newBorrowerPartyRole = PartyRole.Create(borrower, customerPartyRole.Party, today);
            Context.PartyRoles.AddObject(newBorrowerPartyRole);

            var newBorrowerAgreementRole = CreateAgreementRole(agreement, newBorrowerPartyRole);
            Context.AgreementRoles.AddObject(newBorrowerAgreementRole);

            CreateAgreementForCoBorrowers(loanApplication, agreement, today);
            CreateAgreementForGuarantor(loanApplication, agreement, today);

            var pendingdvst = DisbursementVcrStatusEnum.PendingType;
            var disbursement = LoanDisbursementVcr.Create(loanApplication, agreement, today);
            DisbursementVcrStatu.CreateOrUpdateCurrent(disbursement, pendingdvst, today);

            //TODO :: Generate Amortization Schedule....
            AmortizationSchedule schedule = new AmortizationSchedule();
            schedule.DateGenerated = today;
            schedule.EffectiveDate = today;
            schedule.LoanReleaseDate = today;
            schedule.PaymentStartDate = paymentStartDate;
            schedule.LoanAgreement = loanAgreement;
            Context.AmortizationSchedules.AddObject(schedule);

            LoanCalculatorOptions options = new LoanCalculatorOptions();
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(loanApplication.Application, ProductFeatureCategory.InterestComputationModeType);
            options.InterestComputationMode = aiInterestComputationMode.ProductFeatureApplicability.ProductFeature.Name;
            options.LoanAmount = loanApplication.LoanAmount;
            options.LoanTerm = loanApplication.LoanTermLength;
            options.LoanTermId = loanApplication.LoanTermUomId;
            options.InterestRate = loanApplication.InterestRate ?? 0;
            options.InterestRateDescription = loanApplication.InterestRateDescription;
            options.InterestRateDescriptionId = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == loanApplication.InterestRateDescription).Id;
            options.PaymentModeId = loanApplication.PaymentModeUomId;
            options.PaymentMode = UnitOfMeasure.GetByID(options.PaymentModeId).Name;
            options.PaymentStartDate = paymentStartDate;
            options.LoanReleaseDate = loanReleaseDate;

            LoanCalculator loanCalculator = new LoanCalculator();
            var models = loanCalculator.GenerateLoanAmortization(options);
            foreach (var model in models)
            {
                AmortizationScheduleItem item = new AmortizationScheduleItem();
                item.AmortizationSchedule = schedule;
                item.ScheduledPaymentDate = model.ScheduledPaymentDate;
                item.PrincipalPayment = model.PrincipalPayment;
                item.InterestPayment = model.InterestPayment;
                item.PrincipalBalance = model.PrincipalBalance;
                item.TotalLoanBalance = model.TotalLoanBalance;
                item.IsBilledIndicator = false;
                Context.AmortizationScheduleItems.AddObject(item);
            }
            Context.SaveChanges();
        }

        public static void Approve(LoanApplication loanApplication, DateTime today, List<AmortizationScheduleModel> model, DateTime loanReleaseDate)
        {
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);

            LoanApplicationStatu loanStatus = LoanApplicationStatu.CreateOrUpdateCurrent(loanApplication, LoanApplicationStatusType.ApprovedType, today);

            var agreement = Agreement.Create(loanApplication, AgreementType.LoanAgreementType, today);

            //Create new Loan Agreement Record
            LoanAgreement loanAgreement = new LoanAgreement();
            loanAgreement.Agreement = agreement;
            Context.LoanAgreements.AddObject(loanAgreement);

            var agreementItem = Create(loanApplication, agreement);
            Context.AgreementItems.AddObject(agreementItem);

            var borrower = RoleType.BorrowerAgreementType;
            var newBorrowerPartyRole = PartyRole.Create(borrower, customerPartyRole.Party, today);
            Context.PartyRoles.AddObject(newBorrowerPartyRole);

            var newBorrowerAgreementRole = CreateAgreementRole(agreement, newBorrowerPartyRole);
            Context.AgreementRoles.AddObject(newBorrowerAgreementRole);

            CreateAgreementForCoBorrowers(loanApplication, agreement, today);
            CreateAgreementForGuarantor(loanApplication, agreement, today);

            var pendingdvst = DisbursementVcrStatusEnum.PendingType;
            var disbursement = LoanDisbursementVcr.Create(loanApplication, agreement, today);
            DisbursementVcrStatu.CreateOrUpdateCurrent(disbursement, pendingdvst, today);

            AmortizationSchedule schedule = new AmortizationSchedule();
            schedule.DateGenerated = today;
            schedule.EffectiveDate = today;
            schedule.LoanReleaseDate = today;
            schedule.PaymentStartDate = model.First().ScheduledPaymentDate;
            schedule.LoanAgreement = loanAgreement;
            Context.AmortizationSchedules.AddObject(schedule);

            foreach (var models in model)
            {
                AmortizationScheduleItem item = new AmortizationScheduleItem();
                item.AmortizationSchedule = schedule;
                item.ScheduledPaymentDate = models.ScheduledPaymentDate;
                item.PrincipalPayment = models.PrincipalPayment;
                item.InterestPayment = models.InterestPayment;
                item.PrincipalBalance = models.PrincipalBalance;
                item.TotalLoanBalance = models.TotalLoanBalance;
                item.IsBilledIndicator = false;
                Context.AmortizationScheduleItems.AddObject(item);
            }
            Context.SaveChanges();
        }

        private static void CreateAgreementForCoBorrowers(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            var applicationCoborrower = RoleType.CoBorrowerApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == applicationCoborrower.Id
                && entity.PartyRole.EndDate == null);

            var agreementCoBorrower = RoleType.CoBorrowerAgreementType;
            foreach (var role in roles)
            {
                PartyRole partyRole = role.PartyRole;
                var newpartyRole = PartyRole.Create(agreementCoBorrower, partyRole.Party, today);
                Context.PartyRoles.AddObject(newpartyRole);

                var agreementRole = CreateAgreementRole(agreement, newpartyRole);
                Context.AgreementRoles.AddObject(agreementRole);
            }
            
        }

        private static void CreateAgreementForGuarantor(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            var applicationGuarantor = RoleType.GuarantorApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == applicationGuarantor.Id
                 && entity.PartyRole.EndDate == null);

            var agreementGuarantor = RoleType.GuarantorAgreementType;
            foreach (var role in roles)
            {
                PartyRole partyRole = role.PartyRole;
                var newpartyRole = PartyRole.Create(agreementGuarantor, partyRole.Party, today);
                Context.PartyRoles.AddObject(newpartyRole);

                var agreementRole = CreateAgreementRole(agreement, newpartyRole);
                Context.AgreementRoles.AddObject(agreementRole);
            }
        }

        public static AgreementItem Create(LoanApplication loanApplication, Agreement agreement)
        {
            AgreementItem agreementItem = new AgreementItem();
            agreementItem.Agreement = agreement;
            agreementItem.LoanAmount = loanApplication.LoanAmount;

            if (loanApplication.IsInterestProductFeatureInd)
            {
                agreementItem.InterestRate = loanApplication.InterestRate ?? 0;
                agreementItem.InterestRateDescription = loanApplication.InterestRateDescription;
            }

            if (loanApplication.IsPastDueProductFeatureInd)
            {
                agreementItem.PastDueInterestRateDescript = loanApplication.PastDueInterestDescription;
                agreementItem.PastDueInterestRate = loanApplication.PastDueInterestRate;
            }

            //descriptive values
            var uom = Context.UnitOfMeasures.SingleOrDefault(entity =>
                entity.Id == loanApplication.LoanTermUomId);
            var payment = Context.LoanApplications.FirstOrDefault(entity =>
                entity.UnitOfMeasure.Id == loanApplication.PaymentModeUomId);
            var featureAppMode = ProductFeatureApplicability.RetrieveFeature(ProductFeatureCategory.InterestComputationModeType, loanApplication);
            var fetaureAppMethod = ProductFeatureApplicability.RetrieveFeature(ProductFeatureCategory.MethodofChargingInterestType, loanApplication);

            agreementItem.LoanTermLength = loanApplication.LoanTermLength;
            agreementItem.LoanTermUom = uom.Name;
            agreementItem.PaymentMode = payment.UnitOfMeasure.Name;
            agreementItem.InterestComputationMode = featureAppMode.ProductFeature.Name;
            agreementItem.MethodOfChargingInterest = fetaureAppMethod.ProductFeature.Name;

            return agreementItem;
        }

        public static AgreementRole CreateAgreementRole(Agreement agreement, PartyRole partyRole)
        {
            AgreementRole agreeRole = new AgreementRole();
            agreeRole.Agreement = agreement;
            agreeRole.PartyRole = partyRole;
            
            return agreeRole;
        }
    }

    public class DateTimeOperationManager
    {
        private DateTime referenceDate;
        private int dayOfMonth;
        private bool EndOfMonth;
        private DateTime beforeAdjustment;

        private DateTimeAddHandle Handle { get; set; }
        private bool IncludeSaturday { get; set;}
        private string TermOption { get; set; }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public static DateTime AdjustPaymentStartDate(DateTime referenceDate)
        {
            int endOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
            var endOfMonthDate = new DateTime(referenceDate.Year, referenceDate.Month, endOfMonth);
            DateTime paymentStartDate;
            var temp = endOfMonthDate.Day;
            if (temp > 30) temp = 30;
            int diff = temp - referenceDate.Day;
            if (diff >= SystemSetting.GracePeriod) paymentStartDate = endOfMonthDate;
            else
            {
                int psdEndOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.AddMonths(1).Month);
                paymentStartDate = new DateTime(referenceDate.Year, referenceDate.AddMonths(1).Month, psdEndOfMonth);
            }
            paymentStartDate = AdjustPaymentToNextWorkingDay(paymentStartDate);
            if (paymentStartDate.Month < referenceDate.Month)
                paymentStartDate = paymentStartDate.AddYears(1);

            return paymentStartDate;
        }

        public static DateTime AdjustPaymentStartDate2(DateTime referenceDate)
        {
            DateTime paymentStartDate = referenceDate.AddMonths(1);
            AdjustPaymentToNextWorkingDay(paymentStartDate);
            return paymentStartDate;
        }

        private static DateTime AdjustPaymentToNextWorkingDay(DateTime referenceDate)
        {
            bool valid = false;
            do
            {
                int toAdd = 0;
                if (referenceDate.DayOfWeek == DayOfWeek.Sunday)
                    toAdd = 1;

                valid = toAdd == 0;
                if (toAdd != 0)
                    referenceDate = referenceDate.AddDays(toAdd);

            } while (valid == false);
            return referenceDate;
        }

        public DateTimeOperationManager(string paymentMode, DateTime referenceDate, bool includeSaturday = false, string termOption = "")
        {
            this.TermOption = termOption;
            this.IncludeSaturday = includeSaturday;
            this.referenceDate = referenceDate;
            this.dayOfMonth = referenceDate.Day;
            this.beforeAdjustment = new DateTime(referenceDate.Year, referenceDate.Month, referenceDate.Day);
            InitAddHandle(paymentMode);
        }
        
        public DateTime Current
        {
            get 
            {
                return referenceDate;
            }
        }

        public DateTime Increment()
        {
            referenceDate = new DateTime(beforeAdjustment.Year, beforeAdjustment.Month, beforeAdjustment.Day);
            Handle();
            beforeAdjustment = new DateTime(referenceDate.Year, referenceDate.Month, referenceDate.Day);
            //AdjustToNextWorkingDayIfInvalid();
            return referenceDate;
        }

        private void AdjustToNextWorkingDayIfInvalid()
        {
            bool valid = false;
            var holidays = Context.Holidays;
            do
            {
                var holiday = holidays.FirstOrDefault(entity => entity.Date == referenceDate);
                if (holiday != null)
                    referenceDate = GetNextDay();

                int toAdd = 0;
                if (referenceDate.DayOfWeek == DayOfWeek.Saturday && this.IncludeSaturday)
                    toAdd = 2;
                if(referenceDate.DayOfWeek == DayOfWeek.Sunday)
                    toAdd = 1;

                valid = toAdd == 0;
                if(toAdd != 0)
                    referenceDate = referenceDate.AddDays(toAdd);

            } while (valid == false);
        }

        private void InitAddHandle(string paymentMode)
        {
            if (paymentMode == UnitOfMeasure.DailyType.Name)
                Handle = GetNextDay;
            else if (paymentMode == UnitOfMeasure.WeeklyType.Name)
                Handle = GetNextWeek;
            else if (paymentMode == UnitOfMeasure.SemiMonthlyType.Name)
            {
                Handle = GetNextPayday;
                Adjust();
                EndOfMonth = (referenceDate.Day == 15) == false;
            }
            else if (paymentMode == UnitOfMeasure.MonthlyType.Name)
                Handle = GetNextMonth;
            //else if (paymentMode == UnitOfMeasure.QuarterlyType.Name)
            //    Handle = GetNextQuerty;
            else if (paymentMode == UnitOfMeasure.AnnuallyType.Name)
                Handle = GetNextYear;
        }

        private DateTime GetNextDay() 
        {
            referenceDate = referenceDate.AddDays(1);
            return referenceDate;
        }
        private DateTime GetNextWeek()
        {
            referenceDate = referenceDate.AddDays(7);
            return referenceDate;
        }
        private DateTime GetNextPayday()
        {
            if (EndOfMonth == false)
            {
                int endOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, endOfMonth);
                
            }
            else
            {
                referenceDate = referenceDate.AddDays(15);
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, 15);
            }

            EndOfMonth = !EndOfMonth;
            return referenceDate;
        }
        private DateTime Adjust()
        {
            if (referenceDate.Day <= 15)
            {
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, 15);
            }
            else
            {
                int endOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, endOfMonth);
            }
            return referenceDate;
        }
        private DateTime GetNextMonth()
        {
            if (TermOption == ProductFeature.StartToEndOfMonthType.Name)
            {
                if (referenceDate.Day == 1 || referenceDate.Day == 2)
                {
                    AdjustDayOfMonth2();
                }
                else
                {
                    referenceDate = referenceDate.AddMonths(1);
                    AdjustDayOfMonth2();
                }
            }
            else if (TermOption == ProductFeature.AnyDayToSameDayOfNextMonth.Name)
            {
                referenceDate = referenceDate.AddMonths(1);
                AdjustDayOfMonth();
            }
            
            return referenceDate;
        }
        private DateTime GetNextQuerty()
        {
            referenceDate = referenceDate.AddMonths(3);
            AdjustDayOfMonth();
            return referenceDate;
        }
        private DateTime GetNextYear()
        {
            referenceDate = referenceDate.AddYears(1);
            AdjustDayOfMonth();
            return referenceDate;
        }

        private void AdjustDayOfMonth()
        {
            int endOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
            if (dayOfMonth > endOfMonth)
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, endOfMonth);
            else
                referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, dayOfMonth);
        }

        private void AdjustDayOfMonth2()
        {
            int endOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
            //if (dayOfMonth == endOfMonth)
            //{
            //    //referenceDate = referenceDate.AddMonths(1);
            //    //int newEndOfMonth = DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month);
            //    //referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, newEndOfMonth);
            //}

            referenceDate = new DateTime(referenceDate.Year, referenceDate.Month, endOfMonth);

        }
    }

    public delegate DateTime DateTimeAddHandle();
}
