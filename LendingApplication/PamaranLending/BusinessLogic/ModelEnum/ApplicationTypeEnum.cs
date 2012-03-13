using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ApplicationType
    {
        private const string LoanApplication = "Loan Application";

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

        public static ApplicationType LoanApplicationType
        {
            get
            {
                var type = Context.ApplicationTypes.SingleOrDefault(entity => entity.Name == LoanApplication);
                InitialDatabaseValueChecker.ThrowIfNull<ApplicationType>(type);

                return type;
            }
        }
    }
}