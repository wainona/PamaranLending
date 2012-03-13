using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ElectronicAddress
    {
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

        public static ElectronicAddress CreateElectronicAddress(Party party, ElectronicAddressType type, DateTime now)
        {
            Address address = new Address();
            address.Party = party;
            address.AddressType = AddressType.ElectronicAddressType;
            address.EffectiveDate = now;


            ElectronicAddress electronicAddress = new ElectronicAddress();
            electronicAddress.Address = address;
            electronicAddress.ElectronicAddressType = type;
            return electronicAddress;
        }

        public static ElectronicAddress GetCurrentPostalAddressV2(Party party, ElectronicAddressType type, Func<ElectronicAddress, bool> condition)
        {
            ElectronicAddress electronicAddress = Context.ElectronicAddresses.SingleOrDefault(entity => entity.Address.PartyId == party.Id &&
                entity.Address.EndDate == null && entity.ElectronicAddressTypeId == type.Id && condition(entity));
            return electronicAddress;
        }

        public static ElectronicAddress GetCurrentElectronicAddress(Party party, ElectronicAddressType type, Func<Address, bool> condition)
        {
            Address address = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.ElectronicAddressType.Id
                    && entity.ElectronicAddress.ElectronicAddressTypeId == type.Id && condition(entity));
            if (address != null)
                return address.ElectronicAddress;
            return null;
        }

        public static void CreateOrUpdateElectronicAddress(ElectronicAddress currentElectronicAddress, ElectronicAddress newElectronicAddress, DateTime now)
        {
            //check if is equal;
            bool equal = true;
            equal &= currentElectronicAddress.ElectronicAddressString == newElectronicAddress.ElectronicAddressString;


            if (equal == false)
            {
                currentElectronicAddress.Address.EndDate = now;

                Address address = new Address();
                address.Party = currentElectronicAddress.Address.Party;
                address.AddressType = AddressType.ElectronicAddressType;
                address.EffectiveDate = now;

                newElectronicAddress.Address = address;
                newElectronicAddress.ElectronicAddressType = currentElectronicAddress.ElectronicAddressType;
                Context.ElectronicAddresses.AddObject(newElectronicAddress);
            }
            else
            {
                if (newElectronicAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newElectronicAddress);
            }
        }

        public static void CreateOrUpdateElectronicAddress(Party party, ElectronicAddress newElectronicAddress , ElectronicAddressType type, Func<Address, bool> condition, DateTime now)
        {
            var currentElectronicAddress = ElectronicAddress.GetCurrentElectronicAddress(party, type, condition);
            if (string.IsNullOrWhiteSpace(newElectronicAddress.ElectronicAddressString))
            {
                if (currentElectronicAddress != null)
                    currentElectronicAddress.Address.EndDate = now;
                if (newElectronicAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newElectronicAddress);
                return;
            }

            bool isEqual = false;
            if (currentElectronicAddress != null)
            {
                isEqual = currentElectronicAddress.ElectronicAddressString == newElectronicAddress.ElectronicAddressString;
                if (isEqual == false)
                    currentElectronicAddress.Address.EndDate = now;
            }

            if (isEqual == false)
            {
                Address address = new Address();
                address.Party = party;
                address.AddressType = AddressType.ElectronicAddressType;
                address.EffectiveDate = now;

                newElectronicAddress.Address = address;
                newElectronicAddress.ElectronicAddressType = type;
                Context.ElectronicAddresses.AddObject(newElectronicAddress);
            }
            else
            {
                if (newElectronicAddress.EntityState == System.Data.EntityState.Added)
                    Context.Detach(newElectronicAddress);
            }
        }
    }
}
