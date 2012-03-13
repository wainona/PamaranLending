using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductStatusType
    {
        private const string Active = "Active";
        private const string Inactive = "Inactive";
        private const string Retired = "Retired";

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

        public static ProductStatusType ActiveType
        {
            get
            {
                var type = Context.ProductStatusTypes.SingleOrDefault(entity => entity.Name == Active);
                InitialDatabaseValueChecker.ThrowIfNull<ProductStatusType>(type);

                return type;
            }
        }

        public static ProductStatusType InactiveType
        {
            get
            {
                var type = Context.ProductStatusTypes.SingleOrDefault(entity => entity.Name == Inactive);
                InitialDatabaseValueChecker.ThrowIfNull<ProductStatusType>(type);

                return type;
            }
        }

        public static ProductStatusType RetiredType
        {
            get
            {
                var type = Context.ProductStatusTypes.SingleOrDefault(entity => entity.Name == Retired);
                InitialDatabaseValueChecker.ThrowIfNull<ProductStatusType>(type);

                return type;
            }
        }

    }
}