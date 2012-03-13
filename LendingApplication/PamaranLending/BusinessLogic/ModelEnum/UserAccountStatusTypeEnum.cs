using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UserAccountStatusType
    {
        private const string Active = "Active";
        private const string Inactive = "Inactive";


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

        public static UserAccountStatusType ActiveType
        {
            get
            {
                var type = Context.UserAccountStatusTypes.SingleOrDefault(entity => entity.Name == Active);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountStatusType>(type);

            return type;}
        }

        public static UserAccountStatusType InactiveType
        {
            get
            {
                var type = Context.UserAccountStatusTypes.SingleOrDefault(entity => entity.Name == Inactive);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountStatusType>(type);

            return type;}
        }
     
    }
}