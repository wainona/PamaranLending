using System.Linq;
using FirstPacific.UIFramework;


namespace BusinessLogic
{
    public partial class FinAcctTransStatusType
    {
        private const string Posted = "Posted";
        private const string OnHold = "On Hold";

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

        public static FinAcctTransStatusType PostedType
        {
            get
            {
                var type = Context.FinAcctTransStatusTypes.SingleOrDefault(entity => entity.Name == Posted);
                InitialDatabaseValueChecker.ThrowIfNull<FinAcctTransStatusType>(type);

                return type;
            }
        }

        public static FinAcctTransStatusType OnHoldType
        {
            get
            {
                var type = Context.FinAcctTransStatusTypes.SingleOrDefault(entity => entity.Name == OnHold);
                InitialDatabaseValueChecker.ThrowIfNull<FinAcctTransStatusType>(type);

                return type;
            }
        }
    }
}