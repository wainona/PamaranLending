using System;
using System.Collections.Generic;
using System.Linq;
using FirstPacific.UIFramework;
using System.Web;
using BusinessLogic;

namespace BusinessLogic
{
    public partial class PostalAddressType
    {
        private const string HomeAddress = "Home Address";
        private const string BusinessAddress = "Business Address";
        private const string BillingAddress = "Billing Address";
        private const string BirthPlace = "Birthplace";
        private const string PropertyLocation = "Property Location";

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

        public static PostalAddressType HomeAddressType
        {
            get
            {
                var type = Context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == HomeAddress);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(type);

                return type;
            }
        }

        public static PostalAddressType BusinessAddressType
        {
            get
            {
                var type = Context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == BusinessAddress);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(type);

                return type;
            }
        }

        public static PostalAddressType BillingAddressType
        {
            get
            {
                var type = Context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == BillingAddress);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(type);

                return type;
            }
        }

        public static PostalAddressType BirthPlaceType
        {
            get
            {
                var type = Context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == BirthPlace);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(type);

                return type;
            }
        }

        public static PostalAddressType PropertyLocationType
        {
            get
            {
                var type = Context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PropertyLocation);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(type);

                return type;
            }
        }
    }
}