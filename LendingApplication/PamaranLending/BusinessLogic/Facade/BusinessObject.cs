using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BusinessLogic
{
    public class AddressBusinessUtility
    {
        public static Address AddAddress(Party party, AddressType addressType, DateTime today)
        {
            Address address = new Address();
            address.Party = party;
            address.AddressType = addressType;
            address.EffectiveDate = today;
            return address;
        }

        public static PostalAddress AddPostal(Address address, PostalAddressType type, bool isPrimary)
        {
            PostalAddress postalAddress = new PostalAddress();
            postalAddress.Address = address;
            postalAddress.PostalAddressType = type;
            postalAddress.IsPrimary = isPrimary;

            return postalAddress;
        }

        public static TelecommunicationsNumber AddTelNum(Address address, TelecommunicationsNumberType type, bool isPrimary)
        {
            TelecommunicationsNumber telNum = new TelecommunicationsNumber();

            telNum.Address = address;
            telNum.TelecommunicationsNumberType = type;
            telNum.IsPrimary = isPrimary;
            return telNum;
        }

        public static ElectronicAddress AddEmail(Address address, ElectronicAddressType type, bool isPrimary)
        {
            ElectronicAddress email = new ElectronicAddress();
            email.Address = address;
            email.ElectronicAddressType = type;
            email.IsPrimary = isPrimary;
            return email;
        }

        internal static PostalAddress AddPostal(Address postalAddress, Address primaryHomeAddress, bool p)
        {
            throw new NotImplementedException();
        }
    }
}