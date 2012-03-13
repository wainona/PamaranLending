using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PartyRelType
    {
        private const string Employment = "Employment";
        private const string CustomerRelationship = "Customer Relationship";
        private const string ContactRelationship = "Contact Relationship";
        private const string SpousalRelationship = "Spousal Relationship";

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

        public static PartyRelType EmploymentType
        {
            get
            {
                var type = Context.PartyRelTypes.SingleOrDefault(entity => entity.Name == Employment);
                InitialDatabaseValueChecker.ThrowIfNull<PartyRelType>(type);

                return type;
            }
        }

        public static PartyRelType CustomerRelationshipType
        {
            get
            {
                var type = Context.PartyRelTypes.SingleOrDefault(entity => entity.Name == CustomerRelationship);
                InitialDatabaseValueChecker.ThrowIfNull<PartyRelType>(type);

                return type;
            }
        }

        public static PartyRelType ContactRelationshipType
        {
            get
            {
                var type = Context.PartyRelTypes.SingleOrDefault(entity => entity.Name == ContactRelationship);
                InitialDatabaseValueChecker.ThrowIfNull<PartyRelType>(type);

                return type;
            }
        }

        public static PartyRelType SpousalRelationshiptType
        {
            get
            {
                var type = Context.PartyRelTypes.SingleOrDefault(entity => entity.Name == SpousalRelationship);
                InitialDatabaseValueChecker.ThrowIfNull<PartyRelType>(type);

                return type;
            }
        }
    }
}