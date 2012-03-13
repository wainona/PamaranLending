using System.Linq;
using FirstPacific.UIFramework;


namespace BusinessLogic
{
    public partial class HomeOwnershipType
    {
        private const string Owned = "Owned";
        private const string Rented = "Rented";
        private const string LivingwithRelatives = "Living with Relatives";
        private const string CompanyProvided = "Company Provided";

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

        public static HomeOwnershipType OwnedType
        {
            get
            {
                var type = Context.HomeOwnershipTypes.SingleOrDefault(entity => entity.Name == Owned);
                InitialDatabaseValueChecker.ThrowIfNull<HomeOwnershipType>(type);

                return type;
            }
        }

        public static HomeOwnershipType RentedType
        {
            get
            {
                var type = Context.HomeOwnershipTypes.SingleOrDefault(entity => entity.Name == Rented);
                InitialDatabaseValueChecker.ThrowIfNull<HomeOwnershipType>(type);

                return type;
            }
        }

        public static HomeOwnershipType LivingwithRelativesType
        {
            get
            {
                var type = Context.HomeOwnershipTypes.SingleOrDefault(entity => entity.Name == LivingwithRelatives);
                InitialDatabaseValueChecker.ThrowIfNull<HomeOwnershipType>(type);

                return type;
            }
        }

        public static HomeOwnershipType CompanyProvidedType
        {
            get
            {
                var type = Context.HomeOwnershipTypes.SingleOrDefault(entity => entity.Name == CompanyProvided);
                InitialDatabaseValueChecker.ThrowIfNull<HomeOwnershipType>(type);

                return type;
            }
        }
    }
}