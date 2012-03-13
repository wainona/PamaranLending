using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic;

namespace LendingApplication
{
    public class AddressTypeEnums
    {
        public const string PostalAddress = "Postal Address";
        public const string TelecommunicationNumber = "Telecommunication Number";
        public const string ElectronicAddress = "Electronic Address";

        public static AddressType GetPostalAddressType(FinancialEntities context)
        {
            AddressType postalAddressType = new AddressType();
            postalAddressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(postalAddressType);
            return postalAddressType;
        }

        public static AddressType GetTelecommunicationNumberType(FinancialEntities context)
        {
            AddressType telecommunicationNumberType = new AddressType();
            telecommunicationNumberType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.TelecommunicationNumber);
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(telecommunicationNumberType);
            return telecommunicationNumberType;
        }

        public static AddressType GetElectronicAddressType(FinancialEntities context)
        {
            AddressType electronicAddressType = new AddressType();
            electronicAddressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.ElectronicAddress);
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(electronicAddressType);
            return electronicAddressType;
        }
    }
}