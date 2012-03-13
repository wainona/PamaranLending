using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class SystemSettingType
    {
        private const string GracePeriod = "Grace Period";
        private const string InvoiceGenerationTiming = "Invoice Generation Timing";
        private const string DemandCollectionAfter = "Demand Collection After";
        private const string AgeLimitofBorrower = "Age Limit of Borrower";
        private const string AllowableNumberofDiminishingBalanceLoanPerCustomer = "Allowable Number of Diminishing Balance Loan Per Customer";
        private const string AllowableNumberofStraightLineLoanPerCustomer = "Allowable Number of Straight Line Loan Per Customer";
        private const string PercentageofLoanAmountPaid = "Percentage of Loan Amount Paid";
        private const string PeriodinCalculatingPenalty = "Period in Calculating Penalty";
        private const string YearsofLoanstobeDeleted = "Years of Loans to be Deleted";
        private const string RemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoans = "remaining number of payments for the remaining payments to be paid in full during pay off for amortizated loans";
        private const string DatePaymentOption = "Date Payment Option";
        private const string ClerksMaximumHonorableAmount = "Clerk's Maximum Honorable Amount";
        public const string AdvanceChangeNoInterestStartDay = "Advance Change No Interest Start Day";
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

        public static SystemSettingType GracePeriodType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == GracePeriod);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType InvoiceGenerationTimingType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == InvoiceGenerationTiming);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType DemandCollectionAfterType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == DemandCollectionAfter);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType AgeLimitofBorrowerType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AgeLimitofBorrower);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType AllowableNumberofDiminishingBalanceLoanPerCustomerType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AllowableNumberofDiminishingBalanceLoanPerCustomer);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType AllowableNumberofStraightLineLoanPerCustomerType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AllowableNumberofStraightLineLoanPerCustomer);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType PercentageofLoanAmountPaidType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == PercentageofLoanAmountPaid);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType PeriodinCalculatingPenaltyType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == PeriodinCalculatingPenalty);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType YearsofLoanstobeDeletedType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == YearsofLoanstobeDeleted);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType RemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoansType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == RemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoans);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }
        }

        public static SystemSettingType DatePaymentOptionType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == DatePaymentOption);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }

        }

        public static SystemSettingType ClerksMaximumHonorableAmountType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == ClerksMaximumHonorableAmount);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }

        }

        public static SystemSettingType AdvanceChangeNoInterestStartDayType
        {
            get
            {
                var type = Context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AdvanceChangeNoInterestStartDay);
                InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

                return type;
            }

        }
    }
}