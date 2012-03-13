using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class AddressType
    {
        private const string PostalAddress = "Postal Address";
        private const string TelecommunicationNumber = "Telecommunication Number";
        private const string ElectronicAddress = "Electronic Address";

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

        public static AddressType PostalAddressType
        {
            get
            {
                var type = Context.AddressTypes.SingleOrDefault(entity => entity.Name == PostalAddress);
                InitialDatabaseValueChecker.ThrowIfNull<AddressType>(type);

                return type;
            }
        }

        public static AddressType TelecommunicationNumberType
        {
            get
            {
                var type = Context.AddressTypes.SingleOrDefault(entity => entity.Name == TelecommunicationNumber);
                InitialDatabaseValueChecker.ThrowIfNull<AddressType>(type);

                return type;
            }
        }

        public static AddressType ElectronicAddressType
        {
            get
            {
                var type = Context.AddressTypes.SingleOrDefault(entity => entity.Name == ElectronicAddress);
                InitialDatabaseValueChecker.ThrowIfNull<AddressType>(type);

                return type;
            }
        }
    }
}