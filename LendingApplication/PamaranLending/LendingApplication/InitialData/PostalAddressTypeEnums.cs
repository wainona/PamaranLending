using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic;

namespace LendingApplication
{
    public class PostalAddressTypeEnums
    {
        public const string HomeAddress = "Home Address";
        public const string BusinessAddress = "Business Address";
        public const string BillingAddress = "Billing Address";
        public const string PropertyLocation = "Property Location";
        public const string Birthplace = "Birthplace";

        public static PostalAddressType GetHomeAddressType(FinancialEntities context)
        {
            PostalAddressType homeAddressType = new PostalAddressType();
            homeAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.HomeAddress);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(homeAddressType);
            return homeAddressType;
        }

        public static PostalAddressType GetBusinessAddressType(FinancialEntities context)
        {
            PostalAddressType businessAddressType = new PostalAddressType();
            businessAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BusinessAddress);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(businessAddressType);
            return businessAddressType;
        }

        public static PostalAddressType GetBillingAddressType(FinancialEntities context)
        {
            PostalAddressType billingAddressType = new PostalAddressType();
            billingAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BillingAddress);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(billingAddressType);
            return billingAddressType;
        }

        public static PostalAddressType GetBirthPlaceAddressType(FinancialEntities context)
        {
            PostalAddressType birthplaceType = new PostalAddressType();
            birthplaceType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.Birthplace);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(birthplaceType);
            return birthplaceType;
        }

        public static PostalAddressType GetPropertyLocationAddressType(FinancialEntities context)
        {
            PostalAddressType propertyLocationAddressType = new PostalAddressType();
            propertyLocationAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.PropertyLocation);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(propertyLocationAddressType);
            return propertyLocationAddressType;
        }
    }
}
