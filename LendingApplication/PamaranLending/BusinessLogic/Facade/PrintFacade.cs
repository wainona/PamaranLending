using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PrintFacade
    {
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static PostalAddress SetAndGetPostalAddress(Party party)
        {
            var postalAddressType = PostalAddressType.BusinessAddressType.Name;
            var addressType = ObjectContext.AddressTypes.First(entity => entity.Name.Equals("Postal Address"));
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id &&
                                entity.PostalAddress.PostalAddressType.Name == postalAddressType && entity.PostalAddress.IsPrimary == true);
            var pa = address.PostalAddress;

            return pa;

        }

        public static string GetPrimaryPhoneNumber(Party party, PostalAddress postalAddress)
        {
            string primPhoneNumber = null;
            //Primary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name == AddressType.TelecommunicationNumberType.Name);

            var primaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value ==  TelecommunicationsNumberType.BusinessPhoneNumberType.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary);

            if (primaryNumber != null)
            {
                var pN = primaryNumber.TelecommunicationsNumber;
                primPhoneNumber = postalAddress.Country.CountryTelephoneCode + " " + "(" + pN.AreaCode + ") " + pN.PhoneNumber;
            }

            return primPhoneNumber;
        }

        public static string GetSecondaryPhoneNumber(Party party, PostalAddress postalAddress)
        {
            string secPhoneNUmber = string.Empty;
            //Secondary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name == AddressType.TelecommunicationNumberType.Name);
            var telecomNumberTypeBusinessPhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberType.BusinessPhoneNumberType.Name);

            var secondaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary == false);

            if (secondaryNumber != null)
            {
                var sN = secondaryNumber.TelecommunicationsNumber;
                secPhoneNUmber = postalAddress.Country.CountryTelephoneCode + " " + "(" + sN.AreaCode + ") " + sN.PhoneNumber + secPhoneNUmber;
            }

            return secPhoneNUmber;
        }

        public static string GetFaxNumber(Party party, PostalAddress postalAddress)
        {
            string faxPhoneNUmber = string.Empty;
            var telecomNumberTypeFax = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberType.BusinessFaxNumberType.Name);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeFax);
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name == AddressType.TelecommunicationNumberType.Name);

            var faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeFax.Id && entity.TelecommunicationsNumber.IsPrimary);

            if (faxNumber != null)
            {
                var fN = faxNumber.TelecommunicationsNumber;
                faxPhoneNUmber = postalAddress.Country.CountryTelephoneCode + " " + "(" + fN.AreaCode + ") " + fN.PhoneNumber;

            }
            return faxPhoneNUmber;
        }

        public static string GetEmailAddress(Party party)
        {
            string emailAddress = string.Empty;

            var addressTypeId = AddressType.ElectronicAddressType.Id;
            var emailTypeId = ElectronicAddressType.BusinessEmailAddressType.Id;
            var primaryEmail = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypeId
                                                 && entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeId
                                                 && entity.ElectronicAddress.IsPrimary);

            if (primaryEmail != null)
            {
                var ea = primaryEmail.ElectronicAddress;
                //Email Address
                emailAddress = ea.ElectronicAddressString;
            }

            return emailAddress;
        }
    }
}
