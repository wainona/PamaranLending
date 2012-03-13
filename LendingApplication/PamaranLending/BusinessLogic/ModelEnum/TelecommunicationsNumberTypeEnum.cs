using System;
using System.Collections.Generic;
using System.Linq;
using FirstPacific.UIFramework;
using System.Web;
using BusinessLogic;

namespace BusinessLogic
{
    public partial class TelecommunicationsNumberType
    {
        private const string PersonalMobileNumber = "Personal Mobile Number";
        private const string BusinessMobileNumber = "Business Mobile Number";
        private const string BusinessFaxNumber = "Business Fax Number";
        private const string HomePhoneNumber = "Home Phone Number";
        private const string BusinessPhoneNumber = "Business Phone Number";

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

        public static TelecommunicationsNumberType PersonalMobileNumberType
        {
            get
            {
                var type = Context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == PersonalMobileNumber);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(type);

                return type;
            }
        }

        public static TelecommunicationsNumberType BusinessMobileNumberType
        {
            get
            {
                var type = Context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == BusinessMobileNumber);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(type);

                return type;
            }
        }

        public static TelecommunicationsNumberType BusinessFaxNumberType
        {
            get
            {
                var type = Context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == BusinessFaxNumber);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(type);

                return type;
            }
        }

        public static TelecommunicationsNumberType HomePhoneNumberType
        {
            get
            {
                var type = Context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == HomePhoneNumber);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(type);

                return type;
            }
        }

        public static TelecommunicationsNumberType BusinessPhoneNumberType
        {
            get
            {
                var type = Context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == BusinessPhoneNumber);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(type);

                return type;
            }
        }
    }
}

