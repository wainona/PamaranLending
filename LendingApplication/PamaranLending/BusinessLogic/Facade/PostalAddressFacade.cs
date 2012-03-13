using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace BusinessLogic
{
    public partial class PostalAddress
    {
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

        public static PostalAddress CreatePostalAddress(Party party, PostalAddressType type, DateTime now)
        {
            Address address = new Address();
            address.Party = party;
            address.AddressType = AddressType.PostalAddressType;
            address.EffectiveDate = now;


            PostalAddress postalAddress = new PostalAddress();
            postalAddress.Address = address;
            postalAddress.PostalAddressType = type;
            return postalAddress;
        }

        public static PostalAddress GetCurrentPostalAddressV2(Party party, PostalAddressType type, Func<PostalAddress, bool> condition)
        {
            PostalAddress postalAddress = Context.PostalAddresses.SingleOrDefault(entity => entity.Address.PartyId == party.Id &&
                entity.Address.EndDate == null && entity.PostalAddressTypeId == type.Id && condition(entity));
            return postalAddress;
        }

        public static PostalAddress GetCurrentPostalAddress(Party party, PostalAddressType type, Func<Address, bool> condition)
        {
            Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.PartyId == party.Id 
                    && entity.AddressTypeId == AddressType.PostalAddressType.Id
                    && entity.PostalAddress.PostalAddressTypeId == type.Id && condition(entity));
            if (address != null)
                return address.PostalAddress;
            return null;
        }

        public static PostalAddress GetCurrentPostalAddress(Party party, PostalAddressType type, Asset asset)
        {
            Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                    && entity.PostalAddress.PostalAddressTypeId == type.Id && entity.AssetId == asset.Id);
            if (address != null)
                return address.PostalAddress;
            return null;
        }

        public static PostalAddress GetCurrentPostalAddress(Party party, PostalAddressType type, bool IsPrimaryPostalAddress)
        {
            Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                    && entity.PostalAddress.PostalAddressTypeId == type.Id && entity.PostalAddress.IsPrimary == IsPrimaryPostalAddress);
            if (address != null)
                return address.PostalAddress;
            return null;
        }

        public static void CreateOrUpdatePostalAddress(PostalAddress currentPostalAddress, PostalAddress newPostalAddress, DateTime now)
        {
            newPostalAddress.Barangay = string.IsNullOrWhiteSpace(newPostalAddress.Barangay) ? null : newPostalAddress.Barangay;
            newPostalAddress.StreetAddress = string.IsNullOrWhiteSpace(newPostalAddress.StreetAddress) ? null : newPostalAddress.StreetAddress;
            newPostalAddress.City = string.IsNullOrWhiteSpace(newPostalAddress.City) ? null : newPostalAddress.City;
            newPostalAddress.Municipality = string.IsNullOrWhiteSpace(newPostalAddress.Municipality) ? null : newPostalAddress.Municipality;
            newPostalAddress.PostalCode = string.IsNullOrWhiteSpace(newPostalAddress.PostalCode) ? null : newPostalAddress.PostalCode;
            newPostalAddress.Province = string.IsNullOrWhiteSpace(newPostalAddress.Province) ? null : newPostalAddress.Province;
            newPostalAddress.State = string.IsNullOrWhiteSpace(newPostalAddress.State) ? null : newPostalAddress.State;


            //check if is equal;
            bool isEqual = true;
            isEqual = currentPostalAddress.Barangay == newPostalAddress.Barangay;
            isEqual = isEqual && currentPostalAddress.StreetAddress == newPostalAddress.StreetAddress;
            isEqual = isEqual && currentPostalAddress.CountryId == newPostalAddress.CountryId;
            isEqual = isEqual && currentPostalAddress.City == newPostalAddress.City;
            isEqual = isEqual && currentPostalAddress.Municipality == newPostalAddress.Municipality;
            isEqual = isEqual && currentPostalAddress.PostalCode == newPostalAddress.PostalCode;
            isEqual = isEqual && currentPostalAddress.Province == newPostalAddress.Province;
            isEqual = isEqual && currentPostalAddress.State == newPostalAddress.State;

            if (isEqual == false)
            {
                currentPostalAddress.Address.EndDate = now;

                Address address = new Address();
                address.Party = currentPostalAddress.Address.Party;
                address.AddressType = currentPostalAddress.Address.AddressType;
                address.EffectiveDate = now;

                newPostalAddress.Address = address;
                newPostalAddress.PostalAddressType = currentPostalAddress.PostalAddressType;
                Context.PostalAddresses.AddObject(newPostalAddress);
            }
            else
            {
                if (newPostalAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newPostalAddress);
            }
        }

        public static void CreateOrUpdatePostalAddress(Party party, PostalAddress newPostalAddress, PostalAddressType addressType, DateTime now, bool isPrimary)
        {
            var address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id 
                                                            && entity.PostalAddress.PostalAddressTypeId == addressType.Id 
                                                            && entity.PartyId == party.Id
                                                            && entity.PostalAddress.IsPrimary == isPrimary);

            newPostalAddress.Barangay = string.IsNullOrWhiteSpace(newPostalAddress.Barangay) ? null : newPostalAddress.Barangay;
            newPostalAddress.StreetAddress = string.IsNullOrWhiteSpace(newPostalAddress.StreetAddress) ? null : newPostalAddress.StreetAddress;
            newPostalAddress.City = string.IsNullOrWhiteSpace(newPostalAddress.City) ? null : newPostalAddress.City;
            newPostalAddress.Municipality = string.IsNullOrWhiteSpace(newPostalAddress.Municipality) ? null : newPostalAddress.Municipality;
            newPostalAddress.PostalCode = string.IsNullOrWhiteSpace(newPostalAddress.PostalCode) ? null : newPostalAddress.PostalCode;
            newPostalAddress.Province = string.IsNullOrWhiteSpace(newPostalAddress.Province) ? null : newPostalAddress.Province;
            newPostalAddress.State = string.IsNullOrWhiteSpace(newPostalAddress.State) ? null : newPostalAddress.State;

            bool isNull = true;

            isNull &= newPostalAddress.Barangay == null;
            isNull &= newPostalAddress.City == null;
            isNull &= newPostalAddress.Municipality == null;
            isNull &= newPostalAddress.PostalCode == null;
            isNull &= newPostalAddress.Province == null;
            isNull &= (newPostalAddress.CountryId == null || newPostalAddress.CountryId == 0);

            if (isNull)
            {
                if (newPostalAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newPostalAddress);
                return;
            }

            bool isEqual = false;
            if (address != null)
            {
                isEqual = true;
                

                var currentPostalAddress = address.PostalAddress;
                isEqual = isEqual && currentPostalAddress.Barangay == newPostalAddress.Barangay;
                isEqual = isEqual && currentPostalAddress.StreetAddress == newPostalAddress.StreetAddress;
                isEqual = isEqual && currentPostalAddress.CountryId == newPostalAddress.CountryId;
                isEqual = isEqual && currentPostalAddress.City == newPostalAddress.City;
                isEqual = isEqual && currentPostalAddress.Municipality == newPostalAddress.Municipality;
                isEqual = isEqual && currentPostalAddress.PostalCode == newPostalAddress.PostalCode;
                isEqual = isEqual && currentPostalAddress.Province == newPostalAddress.Province;
                isEqual = isEqual && currentPostalAddress.State == newPostalAddress.State;

                if (isEqual == false)
                    address.EndDate = now;
            }

            if (isEqual == false)
            {
                Address newAddress = new Address();
                newAddress.Party = party;
                newAddress.AddressType = AddressType.PostalAddressType;
                newAddress.EffectiveDate = now;

                newPostalAddress.Address = newAddress;
                newPostalAddress.PostalAddressType = addressType;
                Context.PostalAddresses.AddObject(newPostalAddress);
            }
            else
            {
                if (newPostalAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newPostalAddress);
            }
        }
    }
}
