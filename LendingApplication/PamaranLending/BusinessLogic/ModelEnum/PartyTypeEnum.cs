using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PartyType
    {
        private const string Person = "Person";
        private const string Organization = "Organization";

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

        public static PartyType PersonType
        {
            get
            {
                var type = Context.PartyTypes.SingleOrDefault(entity => entity.Name == Person);
                InitialDatabaseValueChecker.ThrowIfNull<PartyType>(type);

                return type;
            }
        }

        public static PartyType OrganizationType
        {
            get
            {
                var type = Context.PartyTypes.SingleOrDefault(entity => entity.Name == Organization);
                InitialDatabaseValueChecker.ThrowIfNull<PartyType>(type);

                return type;
            }
        }
    }
}