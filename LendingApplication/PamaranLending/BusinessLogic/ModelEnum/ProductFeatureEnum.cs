using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductFeature
    {
        private const string Secured = "Secured";
        private const string Unsecured = "Unsecured";
        private const string MaximumLoanableAmount = "Maximum Loanable Amount";
        private const string MinimumLoanableAmount = "Minimum Loanable Amount";
        private const string MaximumLoanTerm = "Maximum Loan Term";
        private const string MinimumLoanTerm = "Minimum Loan Term";
        private const string StraightLineMethod = "Straight Line Method";
        private const string DiminishingBalanceMethod = "Diminishing Balance Method";
        private const string AddonInterest = "Add-on Interest";
        private const string DiscountedInterest = "Discounted Interest";
        private const string MonthlyInterestRate = "Monthly Interest Rate";
        private const string AnnualInterestRate = "Annual Interest Rate";
        private const string MonthlyPastDueInterestRate = "Monthly Past Due Interest Rate";
        private const string AnnualPastDueInterestRate = "Annual Past Due Interest Rate";
        private const string DocumentaryStampTax = "Documentary Stamp Tax";
        private const string ServiceCharge = "Service Charge";
        private const string LoanGuaranteeFee = "Loan Guarantee Fee";
        private const string NoTerm = "No Term";
        private const string StartToEndOfMonth = "Start to End of the Month";
        private const string _AnyDayToSameDayOfNextMonth = "Any Day of the Month to Same Day of the Next Month";

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
        public static ProductFeature NoTermType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == NoTerm);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }
        public static ProductFeature StartToEndOfMonthType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == StartToEndOfMonth);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }
        public static ProductFeature AnyDayToSameDayOfNextMonth
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == _AnyDayToSameDayOfNextMonth);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }
        public static ProductFeature SecuredType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == Secured);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature UnsecuredType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == Unsecured);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MaximumLoanableAmountType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MaximumLoanableAmount);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MinimumLoanableAmountType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MinimumLoanableAmount);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MaximumLoanTermType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MaximumLoanTerm);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MinimumLoanTermType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MinimumLoanTerm);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature StraightLineMethodType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == StraightLineMethod);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature DiminishingBalanceMethodType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == DiminishingBalanceMethod);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature AddonInterestType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == AddonInterest);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature DiscountedInterestType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == DiscountedInterest);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MonthlyInterestRateType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MonthlyInterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature AnnualInterestRateType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == AnnualInterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature MonthlyPastDueInterestRateType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == MonthlyPastDueInterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature AnnualPastDueInterestRateType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == AnnualPastDueInterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature DocumentaryStampTaxType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == DocumentaryStampTax);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature ServiceCharype
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == ServiceCharge);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

        public static ProductFeature LoanGuaranteeFeeType
        {
            get
            {
                var type = Context.ProductFeatures.SingleOrDefault(entity => entity.Name == LoanGuaranteeFee);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeature>(type);

                return type;
            }
        }

    }
}
