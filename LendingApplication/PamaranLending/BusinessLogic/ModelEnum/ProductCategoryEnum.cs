using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductCategory
    {
        private const string LoanProduct = "Loan Product";

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

        public static ProductCategory LoanProductType
        {
            get
            {
                var type = Context.ProductCategories.SingleOrDefault(entity => entity.Name == LoanProduct);
                InitialDatabaseValueChecker.ThrowIfNull<ProductCategory>(type);

                return type;
            }
        }
    }
}