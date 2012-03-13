using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class CustomerStatusType
    {
        private const string Active = "Active";
        private const string Delinquent = "Delinquent";
        private const string Subprime = "Subprime";
        private const string Inactive = "Inactive";
        private const string New = "New";

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

        public static CustomerStatusType ActiveType
        {
            get
            {
                var type = Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == Active);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerStatusType>(type);

                return type;
            }
        }

        public static CustomerStatusType DelinquentType
        {
            get
            {
                var type = Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == Delinquent);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerStatusType>(type);

                return type;
            }
        }

        public static CustomerStatusType SubprimeType
        {
            get
            {
                var type = Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == Subprime);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerStatusType>(type);

                return type;
            }
        }

        public static CustomerStatusType InactiveType
        {
            get
            {
                var type = Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == Inactive);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerStatusType>(type);

                return type;
            }
        }

        public static CustomerStatusType NewType
        {
            get
            {
                var type = Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == New);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerStatusType>(type);

                return type;
            }
        }
    }
}