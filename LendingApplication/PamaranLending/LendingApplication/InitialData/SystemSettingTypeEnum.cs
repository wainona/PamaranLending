using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LendingApplication.Applications;
using BusinessLogic;

namespace LendingApplication
{
    public class SystemSettingTypeEnum
    {
        public const string GracePeriod = "Grace Period";
        public const string InvoiceGenerationTiming = "Invoice Generation Timing";
        public const string DemandCollectionAfter = "Demand Collection After";
        public const string Allotment = "Age Limit of Borrower";
        public const string AllowableNumberofDiminishingBalanceLoanPerCustomer = "Allowable Number of Diminishing Balance Loan Per Customer";
        public const string AllowableNumberofStraightLineLoanPerCustomer = "Allowable Number of Straight Line Loan Per Customer";
        public const string PercentageofLoanAmountPaid = "Percentage of Loan Amount Paid";
        public const string PeriodinCalculatingPenalty = "Period in Calculating Penalty";
        public const string YearsofLoanstobeDeleted = "Years of Loans to be Deleted";
        public const string RemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoans = "remaining number of payments for the remaining payments to be paid in full during pay off for amortizated loans";

        public static SystemSettingType GetGracePeriodType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == GracePeriod);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetInvoiceGenerationTimingType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == InvoiceGenerationTiming);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetDemandCollectionAfterType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == DemandCollectionAfter);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetAllotmentType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == Allotment);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetAllowableNumberofDiminishingBalanceLoanPerCustomerType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AllowableNumberofDiminishingBalanceLoanPerCustomer);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetAllowableNumberofStraightLineLoanPerCustomerType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == AllowableNumberofStraightLineLoanPerCustomer);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetPercentageofLoanAmountPaidType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == PercentageofLoanAmountPaid);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetPeriodinCalculatingPenaltyType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == PeriodinCalculatingPenalty);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetYearsofLoanstobeDeletedType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == YearsofLoanstobeDeleted);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }

        public static SystemSettingType GetRemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoansType(FinancialEntities context)
        {
            var type = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == RemainingNumberOfPaymentsForTheRemainingPaymentsToBePaidInFullDuringPayoffForAmortizatedLoans);
            InitialDatabaseValueChecker.ThrowIfNull<SystemSettingType>(type);

            return type;
        }
    }
}