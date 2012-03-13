using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class IdentificationType
    {
        private const string DriversLicense = "Drivers License";
        private const string SSSId = "SSS ID";

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

        public static IdentificationType DriversLicenseType
        {
            get
            {
                var type = Context.IdentificationTypes.SingleOrDefault(entity => entity.Name == DriversLicense);
                InitialDatabaseValueChecker.ThrowIfNull<IdentificationType>(type);

                return type;
            }
        }

        public static IdentificationType SSSIdType
        {
            get
            {
                var type = Context.IdentificationTypes.SingleOrDefault(entity => entity.Name == SSSId);
                InitialDatabaseValueChecker.ThrowIfNull<IdentificationType>(type);

                return type;
            }
        }
    }
}