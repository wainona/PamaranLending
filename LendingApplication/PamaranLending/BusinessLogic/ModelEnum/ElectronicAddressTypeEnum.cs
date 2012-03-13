using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ElectronicAddressType
    {
        private const string PersonalEmailAddress = "Personal Email Address";
        private const string BusinessEmailAddress = "Business Email Address";

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

        public static ElectronicAddressType PersonalEmailAddressType
        {
            get
            {
                var type = Context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == PersonalEmailAddress);
                InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(type);

                return type;
            }
        }

        public static ElectronicAddressType BusinessEmailAddressType
        {
            get
            {
                var type = Context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == BusinessEmailAddress);
                InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(type);

                return type;
            }
        }
    }
}