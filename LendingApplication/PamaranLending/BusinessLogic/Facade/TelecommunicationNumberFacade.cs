using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class TelecommunicationsNumber
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

        public static TelecommunicationsNumber CreateTelecommNumberAddress(Party party, TelecommunicationsNumberType type, DateTime now)
        {
            Address address = new Address();
            address.Party = party;
            address.AddressType = AddressType.TelecommunicationNumberType;
            address.EffectiveDate = now;


            TelecommunicationsNumber telecomAddress = new TelecommunicationsNumber();
            telecomAddress.Address = address;
            telecomAddress.TelecommunicationsNumberType = type;
            return telecomAddress;
        }

        public static TelecommunicationsNumber GetCurrentTelecommNumberAddressV2(Party party, TelecommunicationsNumberType type, Func<TelecommunicationsNumber, bool> condition)
        {
            TelecommunicationsNumber telecomAddress = Context.TelecommunicationsNumbers.SingleOrDefault(entity => entity.Address.PartyId == party.Id &&
                entity.Address.EndDate == null && entity.TelecommunicationsNumberType.Id == type.Id && condition(entity));
            if(telecomAddress != null)
                return telecomAddress;
            return null;
        }

        public static TelecommunicationsNumber GetCurrentTelecommNumberAddress(Party party, TelecommunicationsNumberType type, Func<Address, bool> condition)
        {
            Address address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id 
                && entity.TelecommunicationsNumber.TelecommunicationsNumberType.Id == type.Id && condition(entity));
            if (address != null)
                return address.TelecommunicationsNumber;
            return null;
        }

        public static TelecommunicationsNumber GetCurrentTelecommNumberAddress(Party party, TelecommunicationsNumberType type, bool isPrimaryTelecommunicationNumber)
        {
            Address address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                && entity.TelecommunicationsNumber.TelecommunicationsNumberType.Id == type.Id && entity.TelecommunicationsNumber.IsPrimary == isPrimaryTelecommunicationNumber);
            if (address != null)
                return address.TelecommunicationsNumber;
            return null;
        }

        public static void CreateOrUpdateTeleCommNumberAddress(TelecommunicationsNumber currentTelecommNumbercAddress, TelecommunicationsNumber newTelecommNumberAddress, DateTime now)
        {
            //check if is equal;
            bool equal = true;
            equal &= currentTelecommNumbercAddress.AreaCode == newTelecommNumberAddress.AreaCode;
            equal &= currentTelecommNumbercAddress.PhoneNumber == newTelecommNumberAddress.PhoneNumber;


            if (equal == false)
            {
                currentTelecommNumbercAddress.Address.EndDate = now;

                Address address = new Address();
                address.Party = currentTelecommNumbercAddress.Address.Party;
                address.AddressType = currentTelecommNumbercAddress.Address.AddressType;
                address.EffectiveDate = now;

                newTelecommNumberAddress.Address = address;
                newTelecommNumberAddress.TelecommunicationsNumberType = currentTelecommNumbercAddress.TelecommunicationsNumberType;
                Context.TelecommunicationsNumbers.AddObject(newTelecommNumberAddress);
            }
            else
                Context.Detach(newTelecommNumberAddress);
        }
        public static void CreateOrUpdateTeleCommNumberAddress(Party party, TelecommunicationsNumber newTelecommNumberAddress, TelecommunicationsNumberType type, DateTime now, bool IsPrimary)
        {
            var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id && entity.TelecommunicationsNumber.IsPrimary == IsPrimary
                && entity.TelecommunicationsNumber.TelecommunicationsNumberType.Id == type.Id);

            newTelecommNumberAddress.AreaCode = string.IsNullOrWhiteSpace(newTelecommNumberAddress.AreaCode) ? null : newTelecommNumberAddress.AreaCode;
            newTelecommNumberAddress.PhoneNumber = string.IsNullOrWhiteSpace(newTelecommNumberAddress.PhoneNumber) ? null : newTelecommNumberAddress.PhoneNumber;

            if (newTelecommNumberAddress.AreaCode == null && newTelecommNumberAddress.PhoneNumber == null)
            {
                if (address != null)
                    address.EndDate = now;
                Context.Detach(newTelecommNumberAddress);
                return;
            }

            bool isEqual = false;
            if (address != null)
            {
                isEqual = true;
                var currentTeleComm = address.TelecommunicationsNumber;
                isEqual = isEqual && currentTeleComm.AreaCode == newTelecommNumberAddress.AreaCode;
                isEqual = isEqual && currentTeleComm.PhoneNumber == newTelecommNumberAddress.PhoneNumber;
                if (isEqual == false)
                    address.EndDate = now;
            }

            if (isEqual == false)
            {
                Address newAddress = new Address();
                newAddress.Party = party;
                newAddress.AddressType = AddressType.TelecommunicationNumberType;
                newAddress.EffectiveDate = now;

                newTelecommNumberAddress.Address = newAddress;
                newTelecommNumberAddress.TelecommunicationsNumberType = type;
                Context.TelecommunicationsNumbers.AddObject(newTelecommNumberAddress);
            }
            else
                Context.Detach(newTelecommNumberAddress);
           
        }
    }
}
