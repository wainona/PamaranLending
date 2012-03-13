using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductFeatureCategory
    {
        private const string CollateralRequirement = "Collateral Requirement";
        private const string LoanLimit = "Loan Limit";
        private const string LoanTerm = "Loan Term";
        private const string InterestComputationMode = "Interest Computation Mode";
        private const string MethodofChargingInterest = "Method of Charging Interest";
        private const string InterestRate = "Interest Rate";
        private const string PastDueInterestRate = "Past Due Interest Rate";
        private const string Fee = "Fee";
        private const string TermOption = "Term Option";


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

        public static ProductFeatureCategory CollateralRequirementType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == CollateralRequirement);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }
        public static ProductFeatureCategory TermOptionType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == TermOption);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory LoanLimitType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == LoanLimit);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory LoanTermType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == LoanTerm);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory InterestComputationModeType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == InterestComputationMode);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory MethodofChargingInterestType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == MethodofChargingInterest);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory InterestRateType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == InterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory PastDueInterestRateType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == PastDueInterestRate);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }

        public static ProductFeatureCategory FeeType
        {
            get
            {
                var type = Context.ProductFeatureCategories.SingleOrDefault(entity => entity.Name == Fee);
                InitialDatabaseValueChecker.ThrowIfNull<ProductFeatureCategory>(type);

                return type;
            }
        }
    }
}