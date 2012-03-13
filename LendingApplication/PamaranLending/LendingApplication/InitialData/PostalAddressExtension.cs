using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LendingApplication.Applications;
using BusinessLogic;

namespace LendingApplication.Applications
{
    public class PostalAddressExtension
    {
        public static PostalAddress CreatePostalAddress(FinancialEntities context, Party party, PostalAddressType type, DateTime now)
        {
            Address address = new Address();
            address.Party = party;
            address.AddressType = AddressTypeEnums.GetPostalAddressType(context);
            address.EffectiveDate = now;


            PostalAddress postalAddress = new PostalAddress();
            postalAddress.Address = address;
            postalAddress.PostalAddressType = type;
            return postalAddress;
        }

        public static PostalAddress GetCurrentPostalAddressV2(FinancialEntities context, Party party, PostalAddressType type, Func<PostalAddress, bool> condition)
        {
            PostalAddress postalAddress = context.PostalAddresses.SingleOrDefault(entity =>entity.Address.PartyId == party.Id && 
                entity.Address.EndDate == null && entity.PostalAddressTypeId == type.Id && condition(entity));
            //Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.PostalAddress.PostalAddressTypeId == type.Id && condition(entity));
            //if (address != null)
            //    return address.PostalAddress;
            return postalAddress;
        }

        public static PostalAddress GetCurrentPostalAddress(FinancialEntities context, Party party, PostalAddressType type, Func<Address, bool> condition)
        {
            Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.PostalAddress.PostalAddressTypeId == type.Id && condition(entity));
            if (address != null)
                return address.PostalAddress;
            return null;
        }

        public static void CreateOrUpdatePostalAddress(FinancialEntities context, PostalAddress currentPostalAddress, PostalAddress newPostalAddress, DateTime now)
        {
            //check if is equal;
            bool equal = true;
            equal &= currentPostalAddress.Barangay == newPostalAddress.Barangay;
            equal &= currentPostalAddress.StreetAddress == newPostalAddress.StreetAddress;
            equal &= currentPostalAddress.Country == newPostalAddress.Country;
            equal &= currentPostalAddress.City == newPostalAddress.City;
            equal &= currentPostalAddress.Municipality == newPostalAddress.Municipality;
            equal &= currentPostalAddress.PostalCode == newPostalAddress.PostalCode;
            equal &= currentPostalAddress.Province == newPostalAddress.Province;
            equal &= currentPostalAddress.State == newPostalAddress.State;

            if (equal == false)
            {
                currentPostalAddress.Address.EndDate = now;

                Address address = new Address();
                address.Party = currentPostalAddress.Address.Party;
                address.AddressType = currentPostalAddress.Address.AddressType;
                address.EffectiveDate = now;

                newPostalAddress.Address = address;
                newPostalAddress.PostalAddressType = currentPostalAddress.PostalAddressType;
                context.PostalAddresses.AddObject(newPostalAddress);
            }
        }
    }
}