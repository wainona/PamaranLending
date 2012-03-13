using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class BankStatusType
    {
        private const string Inactive = "Inactive";
        private const string Active = "Active";

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

        public static BankStatusType InactiveType
        {
            get
            {
                var type = Context.BankStatusTypes.SingleOrDefault(entity => entity.Name == Inactive);
                InitialDatabaseValueChecker.ThrowIfNull<BankStatusType>(type);

                return type;
            }
        }

        public static BankStatusType ActiveType
        {
            get
            {
                var type = Context.BankStatusTypes.SingleOrDefault(entity => entity.Name == Active);
                InitialDatabaseValueChecker.ThrowIfNull<BankStatusType>(type);

                return type;
            }
        }
    }
}