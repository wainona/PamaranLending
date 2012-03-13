using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace BusinessLogic.FullfillmentForms
{
    public class EmployerForm : FullfillmentForm<FinancialEntities>
    {
        public NameModel EmployerPersonName { get; set; }
        public PostalAddressModel EmployerBusinessAddress { get; set; }
        public TelecommunicationNumberModel EmployerBusinessPhoneNumber { get; set; }
        public TelecommunicationNumberModel EmployerBusinessFaxNumber { get; set; }
        public ElectronicAddressModel EmployerBusinessEmailAddress { get; set; }

        public bool IsNew { get; set; }
        public string EmployerOrganizationName { get; set; }
        public int EmployerId { get; set; }
        public int ExistingParty { get; set; }
        public int ExistingPartyRole { get; set; }
        public string EmployerPartyType { get; set; }
        public bool UseExistingParty { get; set; }
        public string CountryCode { get; set; }

        public EmployerForm()
        {
            this.EmployerId = -1;
            EmployerPersonName = new NameModel();
            EmployerBusinessPhoneNumber = new TelecommunicationNumberModel();
            EmployerBusinessFaxNumber = new TelecommunicationNumberModel();
            EmployerBusinessEmailAddress = new ElectronicAddressModel();
            EmployerBusinessAddress = new PostalAddressModel();
        }

        private PartyRole InsertBasicInformation(DateTime today)
        {
            Party party = new Party();
            if (this.EmployerPartyType == PartyType.PersonType.Name)
            {
                if (this.UseExistingParty)
                {
                    if (this.ExistingParty != -1)
                        party = Context.Parties.SingleOrDefault(entity => entity.Id == this.ExistingParty);
                    else if (this.ExistingPartyRole != -1)
                        party = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.ExistingPartyRole).Party;
                }
                else
                {
                    party.PartyType = PartyType.PersonType;

                    Person person = new Person();
                    person.Party = party;

                    //Insert Employer name
                    Person.CreatePersonName(person, today, this.EmployerPersonName.Title,
                                            this.EmployerPersonName.FirstName, 
                                            this.EmployerPersonName.MiddleName,
                                            this.EmployerPersonName.LastName, 
                                            this.EmployerPersonName.NameSuffix,
                                            this.EmployerPersonName.NickName);

                    //Context.Parties.AddObject(party);
                }
            }
            else
            {
                if (this.UseExistingParty)
                {
                    if (this.ExistingParty != -1)
                        party = Context.Parties.SingleOrDefault(entity => entity.Id == this.ExistingParty);
                    else if (this.ExistingPartyRole != -1)
                        party = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.ExistingPartyRole).Party;
                }
                else
                {
                    party.PartyType = PartyType.OrganizationType;

                    Organization organization = new Organization();
                    organization.Party = party;
                    organization.OrganizationName = this.EmployerOrganizationName;

                    //Context.Parties.AddObject(party);
                }
            }

            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = RoleType.EmployerType.Id;
            partyRole.EffectiveDate = today;

            //if (this.UseExistingParty)
            //    Context.PartyRoles.AddObject(partyRole);
            //else if (!this.UseExistingParty && partyRole.EntityState == System.Data.EntityState.Added)
            if (!this.UseExistingParty)
            {
                Context.Parties.AddObject(party);
            }
            else
            {
                Context.PartyRoles.AddObject(partyRole);
            }

            return partyRole;
        }

        private void InsertContactInformation(PartyRole partyRole, DateTime today)
        {
            Address address = new Address();
            address.Party = partyRole.Party;
            address.AddressType = AddressType.PostalAddressType;
            address.EffectiveDate = today;

            PostalAddress postalAddress = new PostalAddress();
            postalAddress.Address = address;
            postalAddress.PostalAddressType = PostalAddressType.BusinessAddressType;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.StreetAddress))
                postalAddress.StreetAddress = this.EmployerBusinessAddress.StreetAddress;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Barangay))
                postalAddress.Barangay = this.EmployerBusinessAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.City))
                postalAddress.City = this.EmployerBusinessAddress.City;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Municipality))
                postalAddress.Municipality = this.EmployerBusinessAddress.Municipality;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Province))
                postalAddress.Province = this.EmployerBusinessAddress.Province;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.State))
                postalAddress.State = this.EmployerBusinessAddress.State;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.PostalCode))
                postalAddress.PostalCode = this.EmployerBusinessAddress.PostalCode;
            if (this.EmployerBusinessAddress.CountryId != null)
                postalAddress.CountryId = this.EmployerBusinessAddress.CountryId;
            postalAddress.IsPrimary = true;

            //Context.Addresses.AddObject(address);

            if (this.EmployerBusinessFaxNumber != null)
            {

                if (this.EmployerBusinessFaxNumber.AreaCode != null && this.EmployerBusinessFaxNumber.PhoneNumber != null)
                {
                    Address employerFaxNumber = new Address();
                    employerFaxNumber.AddressType = AddressType.TelecommunicationNumberType;
                    employerFaxNumber.Party = partyRole.Party;
                    employerFaxNumber.EffectiveDate = today;

                    TelecommunicationsNumber faxNumber = new TelecommunicationsNumber();
                    faxNumber.Address = employerFaxNumber;
                    faxNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessFaxNumberType;
                    faxNumber.AreaCode = this.EmployerBusinessFaxNumber.AreaCode;
                    faxNumber.PhoneNumber = this.EmployerBusinessFaxNumber.PhoneNumber;
                    faxNumber.IsPrimary = true;

                    //Context.Addresses.AddObject(employerFaxNumber);
                }
            }

            if (this.EmployerBusinessPhoneNumber != null)
            {
                //Context.Addresses.AddObject(customerTelephoneNumber);
                //Context.SaveChanges();

                if (this.EmployerBusinessPhoneNumber.AreaCode != null && this.EmployerBusinessPhoneNumber.PhoneNumber != null)
                {
                    Address businessTelephoneNumber = new Address();
                    businessTelephoneNumber.AddressType = AddressType.TelecommunicationNumberType;
                    businessTelephoneNumber.Party = partyRole.Party;
                    businessTelephoneNumber.EffectiveDate = today;

                    TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
                    telephoneNumber.Address = businessTelephoneNumber;
                    telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessPhoneNumberType;
                    telephoneNumber.AreaCode = this.EmployerBusinessPhoneNumber.AreaCode;
                    telephoneNumber.PhoneNumber = this.EmployerBusinessPhoneNumber.PhoneNumber;
                    telephoneNumber.IsPrimary = true;
                    //Context.TelecommunicationsNumbers.AddObject(telephoneNumber);

                    //Context.Addresses.AddObject(businessTelephoneNumber);
                }
            }

            if (this.EmployerBusinessEmailAddress != null)
            {
                if (this.EmployerBusinessEmailAddress.EletronicAddressString != null)
                {
                    Address primEmailAddress = new Address();
                    primEmailAddress.AddressType = AddressType.ElectronicAddressType;
                    primEmailAddress.Party = partyRole.Party;
                    primEmailAddress.EffectiveDate = today;

                    ElectronicAddress primEmailAdd = new ElectronicAddress();
                    primEmailAdd.Address = primEmailAddress;
                    primEmailAdd.ElectronicAddressType = ElectronicAddressType.BusinessEmailAddressType;
                    primEmailAdd.ElectronicAddressString = this.EmployerBusinessEmailAddress.EletronicAddressString;
                    primEmailAdd.IsPrimary = true;

                    //Context.ElectronicAddresses.AddObject(primEmailAdd);
                    //Context.Addresses.AddObject(primEmailAddress);
                }
            }
        }

        private void RetrieveBasicInformation(PartyRole partyRole)
        {
            Party party = partyRole.Party;
            this.EmployerPartyType = party.PartyType.Name;

            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                Person person = party.Person;

                this.EmployerPersonName.NameConcat = StringConcatUtility.Build(" ", person.LastNameString + ","
                        , person.FirstNameString + " ", person.MiddleInitialString,
                        person.NameSuffixString);

                if (!string.IsNullOrWhiteSpace(person.TitleString))
                    this.EmployerPersonName.Title = person.TitleString;
                this.EmployerPersonName.FirstName = person.FirstNameString;
                this.EmployerPersonName.LastName = person.LastNameString;
                if (!string.IsNullOrWhiteSpace(person.MiddleNameString))
                    this.EmployerPersonName.MiddleName = person.MiddleNameString;
                this.EmployerPersonName.MothersMaidenName = person.MothersMaidenNameString;
                if (!string.IsNullOrWhiteSpace(person.NameSuffixString))
                    this.EmployerPersonName.NameSuffix = person.NameSuffixString;
                if (!string.IsNullOrWhiteSpace(person.NickNameString))
                    this.EmployerPersonName.NickName = person.NickNameString;
            }
            else
            {
                Organization organization = party.Organization;
                this.EmployerOrganizationName = organization.OrganizationName;
            }
        }

        private void RetrieveContactInformation(PartyRole partyRole)
        {
            Party party = partyRole.Party;

            //Primary Business Address
            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.BusinessAddressType.Id && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                this.EmployerBusinessAddress.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                postalAddressSpecific.Barangay,
                                postalAddressSpecific.Municipality,
                                postalAddressSpecific.City,
                                postalAddressSpecific.Province,
                                postalAddressSpecific.State,
                                postalAddressSpecific.Country.Name,
                                postalAddressSpecific.PostalCode);

                this.EmployerBusinessAddress.StreetAddress = postalAddressSpecific.StreetAddress;
                this.EmployerBusinessAddress.State = postalAddressSpecific.State;
                this.EmployerBusinessAddress.Province = postalAddressSpecific.Province;
                this.EmployerBusinessAddress.PostalCode = postalAddressSpecific.PostalCode;
                this.EmployerBusinessAddress.Municipality = postalAddressSpecific.Municipality;
                this.EmployerBusinessAddress.City = postalAddressSpecific.City;
                this.EmployerBusinessAddress.Barangay = postalAddressSpecific.Barangay;
                this.EmployerBusinessAddress.CountryId = postalAddressSpecific.CountryId;

                this.CountryCode = postalAddressSpecific.Country.CountryTelephoneCode;
            }

            //Fax Number
            Address empFaxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.BusinessFaxNumberType
                                && entity.TelecommunicationsNumber.IsPrimary);

            if (empFaxNumber != null && empFaxNumber.TelecommunicationsNumber != null)
            {

                TelecommunicationsNumber faxNumberSpecific = empFaxNumber.TelecommunicationsNumber;
                this.EmployerBusinessFaxNumber.PhoneNumber = faxNumberSpecific.PhoneNumber;
                this.EmployerBusinessFaxNumber.AreaCode = faxNumberSpecific.AreaCode;
            }

            //Telephone Number
            Address primTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.BusinessPhoneNumberType
                                && entity.TelecommunicationsNumber.IsPrimary);

            if (primTelecomNumber != null && primTelecomNumber.TelecommunicationsNumber != null)
            {
                TelecommunicationsNumber primNumberSpecific = primTelecomNumber.TelecommunicationsNumber;
                this.EmployerBusinessPhoneNumber.PhoneNumber = primNumberSpecific.PhoneNumber;
                this.EmployerBusinessPhoneNumber.AreaCode = primNumberSpecific.AreaCode;
            }

            //Primary Email Address
            Address primEmailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.BusinessEmailAddressType
                                && entity.ElectronicAddress.IsPrimary);
            if (primEmailAddress != null && primEmailAddress.ElectronicAddress != null)
            {
                ElectronicAddress primaryEmailAddressSpecific = primEmailAddress.ElectronicAddress;
                this.EmployerBusinessEmailAddress.EletronicAddressString = primaryEmailAddressSpecific.ElectronicAddressString;
            }

        }

        private void UpdateBasicInformation(PartyRole partyRole, DateTime today)
        {
            Party party = partyRole.Party;
            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                Person person = party.Person;

                //Update Employer name
                Person.CreatePersonName(person, today, this.EmployerPersonName.Title,
                                            this.EmployerPersonName.FirstName,
                                            this.EmployerPersonName.MiddleName,
                                            this.EmployerPersonName.LastName,
                                            this.EmployerPersonName.NameSuffix,
                                            this.EmployerPersonName.NickName);

            }
            else
            {

                Organization organization = party.Organization;

                organization.OrganizationName = this.EmployerOrganizationName;

            }
        }

        private void UpdateContactInformation(PartyRole partyRole, DateTime today)
        {
            Party party = partyRole.Party;

            //Update Primary Business Address
            PostalAddress currentPrimAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.BusinessAddressType, entity => entity.PostalAddress.IsPrimary);
            PostalAddress newPostalAddress = new PostalAddress();
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.StreetAddress))
                newPostalAddress.StreetAddress = this.EmployerBusinessAddress.StreetAddress;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Barangay))
                newPostalAddress.Barangay = this.EmployerBusinessAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.City))
                newPostalAddress.City = this.EmployerBusinessAddress.City;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Municipality))
                newPostalAddress.Municipality = this.EmployerBusinessAddress.Municipality;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.Province))
                newPostalAddress.Province = this.EmployerBusinessAddress.Province;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.State))
                newPostalAddress.State = this.EmployerBusinessAddress.State;
            if (!string.IsNullOrWhiteSpace(this.EmployerBusinessAddress.PostalCode))
                newPostalAddress.PostalCode = this.EmployerBusinessAddress.PostalCode;
            if (this.EmployerBusinessAddress.CountryId != null)
                newPostalAddress.CountryId = this.EmployerBusinessAddress.CountryId;
            newPostalAddress.IsPrimary = true;

            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, PostalAddressType.BusinessAddressType, today, true);

            //Update Fax Number
                TelecommunicationsNumber faxNumber = new TelecommunicationsNumber();
                faxNumber.AreaCode = this.EmployerBusinessFaxNumber.AreaCode;
                faxNumber.PhoneNumber = this.EmployerBusinessFaxNumber.PhoneNumber;
                faxNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessFaxNumberType;
                faxNumber.IsPrimary = true;

                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, faxNumber, 
                                                TelecommunicationsNumberType.BusinessFaxNumberType, today, true);


            //Update Telephone number
                TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
                telephoneNumber.AreaCode = this.EmployerBusinessPhoneNumber.AreaCode;
                telephoneNumber.PhoneNumber = this.EmployerBusinessPhoneNumber.PhoneNumber;
                telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessPhoneNumberType;
                telephoneNumber.IsPrimary = true;

                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, telephoneNumber,
                                                TelecommunicationsNumberType.BusinessPhoneNumberType, today, true);

   

            //Update Email Address
                if (this.EmployerBusinessEmailAddress.EletronicAddressString != null)
                {
                    ElectronicAddress primEmailAdd = new ElectronicAddress();
                    primEmailAdd.ElectronicAddressString = this.EmployerBusinessEmailAddress.EletronicAddressString;
                    primEmailAdd.ElectronicAddressType = ElectronicAddressType.BusinessEmailAddressType;
                    primEmailAdd.IsPrimary = true;

                    ElectronicAddress.CreateOrUpdateElectronicAddress(party, primEmailAdd, ElectronicAddressType.BusinessEmailAddressType,
                                                                        entity => entity.ElectronicAddress.IsPrimary, today);
                }
        }

        public override void Retrieve(int id)
        {
            PartyRole partyRole = PartyRole.GetById(id);
            RetrieveBasicInformation(partyRole);
            RetrieveContactInformation(partyRole);
        }

        public override void PrepareForSave()
        {
            var today = DateTime.Now;
            if (this.EmployerId == -1 && this.IsNew == true)
            {
                PartyRole partyRole = InsertBasicInformation(today);
                this.IsNew = true;
                InsertContactInformation(partyRole, today);
                //Context.SaveChanges();
            }
            else if (this.EmployerId != -1 && this.IsNew != true)
            {
                PartyRole partyRole = PartyRole.GetById(this.EmployerId);
                this.IsNew = false;
                UpdateBasicInformation(partyRole, today);
                UpdateContactInformation(partyRole, today);
                //Context.SaveChanges();
            }
            else if (this.EmployerId != -1 && this.IsNew == true)
            {
            }
        }
    }
}
