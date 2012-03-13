using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace BusinessLogic.FullfillmentForms
{
    public class CustomerSourceOfIncomeModel : BusinessObjectModel
    {
        public int CustomerSourceOfIncomeId { get; private set; }
        public int SourceOfIncomeId { get; set; }
        public decimal Amount { get; set; }
        public string CustomerSourceOfIncome { get; set; }
        public int CustomerId { get; set; }

        public CustomerSourceOfIncomeModel(int sourceOfIncomeId, decimal amount)
        {
            this.CustomerSourceOfIncomeId = -1;
            SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(sourceOfIncomeId);
            this.CustomerSourceOfIncome = sourceOfIncome.Name;
            this.Amount = amount;
            this.SourceOfIncomeId = sourceOfIncomeId;
        }

        public CustomerSourceOfIncomeModel(CustomerSourceOfIncome customerSourceOfIncome)
        {
            this.IsNew = false;
            this.CustomerSourceOfIncomeId = customerSourceOfIncome.Id;
            SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(customerSourceOfIncome.SourceOfIncomeId);
            this.CustomerSourceOfIncome = sourceOfIncome.Name;
            this.Amount = customerSourceOfIncome.Income;
            this.CustomerId = customerSourceOfIncome.PartyRoleId;
            this.SourceOfIncomeId = customerSourceOfIncome.SourceOfIncomeId;
        }

    }

    public class NameModel : BusinessObjectModel
    {
        public int PersonPartyId { get; set; }
        public string NameConcat { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string NameSuffix { get; set; }
        public string NickName { get; set; }
        public string MothersMaidenName { get; set; }
        public int PersonNameTypeId { get; set; }
    }

    public class PostalAddressModel : BusinessObjectModel
    {
        public int AddressId { get; set; }
        public int? CountryId { get; set; }
        public int PostalAddressTypeId { get; set; }
        public string StreetAddress { get; set; }
        public string Barangay { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressConcat { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class TelecommunicationNumberModel : BusinessObjectModel
    {
        public int AddressId { get; set; }
        public int TypeId { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ElectronicAddressModel : BusinessObjectModel
    {
        public int AddressId { get; set; }
        public int ElectronicAddressTypeId { get; set; }
        public string EletronicAddressString { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class CustomerForm : FullfillmentForm<FinancialEntities>
    {
        
        private List<CustomerSourceOfIncomeModel> SourcesOfIncome;
        public NameModel CustomerName { get; set; }
        public NameModel SpouseName { get; set; }
        public PostalAddressModel CustomerPrimaryHomeAddress { get; set; }
        public PostalAddressModel CustomerSecondaryHomeAddress { get; set; }
        public TelecommunicationNumberModel CustomerCellNumber { get; set; }
        public TelecommunicationNumberModel CustomerTelephoneNumber { get; set; }
        public ElectronicAddressModel CustomerPrimaryEmailAddress { get; set; }
        public ElectronicAddressModel CustomerSecondaryEmailAddress { get; set; }
        public PostalAddressModel Birthplace { get; set; }

        public IEnumerable<CustomerSourceOfIncomeModel> AvailableSourcesOfIncome
        {
            get
            {
                return this.SourcesOfIncome.Where(model => model.ToBeDeleted == false);
            }
        }
        public int PersonPartyId { get; set; }
        public int NewCustomerId { get; set; }
        public bool IsNew { get; set; }
        public Address CustomerAddress { get; set; }
        public int GenderTypeId { get; set; }
        public int CustomerId { get; set; }
        public string District { get; set; }
        public int DistrictClassificationTypeId { get; set; }
        public string StationNumber { get; set; }
        public string CustomerCountryCode { get; set; }
        public string CustomerStatus { get; set; }
        public int MaritalStatusId { get; set; }
        public DateTime Birthdate { get; set; }
        public int NumberOfDependents { get; set; }
        public int EducationalAttainmentId { get; set; }
        public int HomeOwnershipId { get; set; }
        public DateTime ResidentSince { get; set; }
        public int? NationalityId { get; set; }
        public string Tin { get; set; }
        public string CtcNumber { get; set; }
        public DateTime DateIssued { get; set; }
        public string PlaceIssued { get; set; }
        public decimal? CreditLimit { get; set; }
        public int IdType1Id { get; set; }
        public string IdNumber1 { get; set; }
        public int IdType2Id { get; set; }
        public string ImgUrl { get; set; }
        public string IdNumber2 { get; set; }
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public string EmploymentAddress { get; set; }
        public string EmployerTelephoneNumber { get; set; }
        public string EmployerTelephoneAreaCode { get; set; }
        public string EmployerFaxNumber { get; set; }
        public string EmployerFaxAreaCode { get; set; }
        public string EmployerCountryCode { get; set; }
        public string EmployerEmailAddress { get; set; }
        public int NewEmployerId { get; set; }
        public string EmployeeIdNumber { get; set; }
        public string EmployeePosition { get; set; }
        public string EmploymentStatus { get; set; }
        public string Salary { get; set; }
        public string SssNumber { get; set; }
        public string GsisNumber { get; set; }
        public string OwaNumber { get; set; }
        public string PhicNumber { get; set; }
        public int SpouseId { get; set; }
        public DateTime? SpouseBirthDate { get; set; }
        public string Remarks { get; set; }
        public int CustomerCategoryTypeId { get; set; }

        public CustomerForm()
        {
            this.CustomerId = -1;
            this.NewCustomerId = -1;
            this.EmployerId = -1;
            this.NewEmployerId = -1;
            SourcesOfIncome = new List<CustomerSourceOfIncomeModel>();
            CustomerName = new NameModel();
            SpouseName = new NameModel();
            CustomerCellNumber = new TelecommunicationNumberModel();
            CustomerTelephoneNumber = new TelecommunicationNumberModel();
            CustomerPrimaryEmailAddress = new ElectronicAddressModel();
            CustomerPrimaryHomeAddress = new PostalAddressModel();
            CustomerSecondaryEmailAddress = new ElectronicAddressModel();
            CustomerSecondaryHomeAddress = new PostalAddressModel();
            Birthplace = new PostalAddressModel();
        }

        public bool SourcesOfIncomeContains(int sourceOfIncomeId)
        {
            bool res = false;
            if (this.SourcesOfIncome.Select(entity => entity.SourceOfIncomeId).Contains(sourceOfIncomeId) == true)
            {
                res = true;
            }

            return res;
        }

        public bool AddSourceOfIncome(CustomerSourceOfIncomeModel model)
        {
            bool res = false;
            if (this.SourcesOfIncome.Select(entity => entity.SourceOfIncomeId).Contains(model.SourceOfIncomeId) == false)
            {
                this.SourcesOfIncome.Add(model);
                res = true;
            }

            return res;
        }

        public void RemoveSourceOfIncome(string randomKey)
        {
            CustomerSourceOfIncomeModel model = this.SourcesOfIncome.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveSourceOfIncome(model);
        }

        public CustomerSourceOfIncomeModel GetSourceOfIncome(string randomKey)
        {
            return this.SourcesOfIncome.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public void RemoveSourceOfIncome(CustomerSourceOfIncomeModel model)
        {
            if (this.SourcesOfIncome.Contains(model) == true)
            {
                if (model.IsNew)
                    SourcesOfIncome.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void EditSourceOfIncome(string randomKey, int sourceOfIncomeId, decimal amount)
        {
            CustomerSourceOfIncomeModel model = this.SourcesOfIncome.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
            {
                CustomerSourceOfIncomeModel mod = new CustomerSourceOfIncomeModel(sourceOfIncomeId, amount);
                model = mod;
            }
        }

        private PartyRole InsertBasicInformation(DateTime today)
        {
            Party party = new Party();
            if (this.NewCustomerId == -1)
            {
                party.PartyType = PartyType.PersonType;
            }
            else
            {
                var partyOldPartyRole = Context.PartyRoles.SingleOrDefault(entity =>
                                        entity.Id == this.NewCustomerId && entity.EndDate == null);
                party = partyOldPartyRole.Party;
                var contactPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == party.Id
                                        && entity.RoleTypeId == RoleType.ContactType.Id && entity.EndDate == null);
                PartyRelationship oldPartyRel = Context.PartyRelationships.SingleOrDefault(entity =>
                                                entity.PartyRelTypeId == PartyRelType.ContactRelationshipType.Id
                                                && entity.FirstPartyRoleId == partyOldPartyRole.Id && entity.EndDate == null);
                if (oldPartyRel != null)
                    oldPartyRel.EndDate = today;
            }
            //Context.SaveChanges();

            PartyRole partyRole = new PartyRole();

            //if (this.NewCustomerId == -1)
            //{
                partyRole.Party = party;
                partyRole.RoleTypeId = RoleType.CustomerType.Id;
                partyRole.EffectiveDate = today;
            //}
            //else
            //{
            //    partyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.NewCustomerId && entity.EndDate == null);
            //}

            //Context.SaveChanges();

            PartyRelationship customerPartyRel = new PartyRelationship();
            PartyRole lendingInstitution = Context.PartyRoles.SingleOrDefault(entity => 
                                           entity.PartyRoleType.RoleType.Name == RoleType.LendingInstitutionType.Name);

            customerPartyRel.PartyRole = partyRole;
            customerPartyRel.PartyRole1 = lendingInstitution;
            customerPartyRel.PartyRelType = PartyRelType.CustomerRelationshipType;
            customerPartyRel.EffectiveDate = today;

            Customer customer = new Customer();
            customer.PartyRole = partyRole;
            customer.CreditLimit = this.CreditLimit;
            if (this.Remarks != null)
                customer.Remarks = this.Remarks;

            CustomerCategory customerCategory = new CustomerCategory();
            customerCategory.Customer = customer;
            customerCategory.CustomerCategoryType = this.CustomerCategoryTypeId;
            customerCategory.EffectiveDate = today;

            Person person = new Person();
            if (this.NewCustomerId == -1)
            {

                person.Party = party;
                person.GenderTypeId = this.GenderTypeId;
                person.NationalityTypeId = this.NationalityId;
                person.EducAttainmentTypeId = this.EducationalAttainmentId;
                person.Birthdate = this.Birthdate;
                if (!string.IsNullOrWhiteSpace(this.ImgUrl))
                    person.ImageFilename = this.ImgUrl;

                //Insert customer name
                Person.CreatePersonName(person, today, this.CustomerName.Title,
                                        this.CustomerName.FirstName, this.CustomerName.MiddleName,
                                        this.CustomerName.LastName, this.CustomerName.NameSuffix,
                                        this.CustomerName.NickName);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.MothersMaidenNameType, this.CustomerName.MothersMaidenName, today);
                //Context.SaveChanges();
            }
            else
            {
                person = party.Person;
                person.GenderTypeId = this.GenderTypeId;
                person.NationalityTypeId = this.NationalityId;
                person.EducAttainmentTypeId = this.EducationalAttainmentId;
                person.Birthdate = this.Birthdate;
                if (!string.IsNullOrWhiteSpace(this.ImgUrl))
                    person.ImageFilename = this.ImgUrl;

                //Insert customer name
                Person.CreatePersonName(person, today, this.CustomerName.Title,
                                        this.CustomerName.FirstName, this.CustomerName.MiddleName,
                                        this.CustomerName.LastName, this.CustomerName.NameSuffix,
                                        this.CustomerName.NickName);
            }

            CustomerClassification customerClassification = new CustomerClassification();
            customerClassification.Customer = customer;
            customerClassification.ClassificationTypeId = this.DistrictClassificationTypeId;
            customerClassification.EffectiveDate = today;

            //Context.CustomerClassifications.AddObject(customerClassification);

            CustomerStatusType customerNew = CustomerStatusType.NewType;
            CustomerStatu customerStatus = new CustomerStatu();
            customerStatus.CustomerStatusType = customerNew;
            customerStatus.Customer = customer;
            customerStatus.TransitionDateTime = today;
            customerStatus.IsActive = true;

            //Context.CustomerStatus.AddObject(customerStatus);
            //Context.SaveChanges();
            
                MaritalStatu customerMaritalStatus = new MaritalStatu();
                customerMaritalStatus.Person = person;
                customerMaritalStatus.MaritalStatusTypeId = this.MaritalStatusId;
                customerMaritalStatus.NumberOfDependents = this.NumberOfDependents;
                customerMaritalStatus.TransitionDateTime = today;
                customerMaritalStatus.IsActive = true;

                //Context.MaritalStatus.AddObject(customerMaritalStatus);
                if (this.NewCustomerId == -1)
                {
                    HomeOwnership custHomeOwnership = new HomeOwnership();
                    custHomeOwnership.Person = person;
                    custHomeOwnership.HomeOwnershipTypeId = this.HomeOwnershipId;
                    custHomeOwnership.ResidenceSince = this.ResidentSince;
                    custHomeOwnership.EffectiveDate = today;

                    //Context.HomeOwnerships.AddObject(custHomeOwnership);
                    //Context.SaveChanges();
                }
                else
                {
                    var currentHomeOwnership = party.Person.CurrentHomeOwnership;
                    if (currentHomeOwnership != null)
                    {
                        currentHomeOwnership.EndDate = today;

                        HomeOwnership custHomeOwnership = new HomeOwnership();
                        custHomeOwnership.Person = person;
                        custHomeOwnership.HomeOwnershipTypeId = this.HomeOwnershipId;
                        custHomeOwnership.ResidenceSince = this.ResidentSince;
                        custHomeOwnership.EffectiveDate = today;

                        //Context.HomeOwnerships.AddObject(custHomeOwnership);
                    }
                    else
                    {
                        HomeOwnership custHomeOwnership = new HomeOwnership();
                        custHomeOwnership.Person = person;
                        custHomeOwnership.HomeOwnershipTypeId = this.HomeOwnershipId;
                        custHomeOwnership.ResidenceSince = this.ResidentSince;
                        custHomeOwnership.EffectiveDate = today;
                    }
                }

                if (this.Tin != null)
                {
                    Taxpayer custTin = new Taxpayer();
                    custTin.Tin = this.Tin;
                    custTin.PartyRole = partyRole;

                    //Context.Taxpayers.AddObject(custTin);
                    //Context.SaveChanges();


                    if (this.CtcNumber != null && this.DateIssued != null && this.PlaceIssued != null)
                    {
                        Ctc customerCtc = new Ctc();
                        customerCtc.Taxpayer = custTin;
                        customerCtc.CtcNumber = this.CtcNumber;
                        customerCtc.DateIssued = this.DateIssued;
                        customerCtc.IssuedWhere = this.PlaceIssued;

                        //Context.Ctcs.AddObject(customerCtc);
                        //Context.SaveChanges();
                    }
                }
            if (this.Birthplace.CountryId != null && this.Birthplace.CountryId != 0)
            {
                Address address = new Address();
                address.Party = party;
                address.AddressType = AddressType.PostalAddressType;
                address.EffectiveDate = today;

                this.CustomerAddress = address;

                //Context.SaveChanges();

                PostalAddress postalAddress = new PostalAddress();
                postalAddress.Address = address;
                postalAddress.PostalAddressType = PostalAddressType.BirthPlaceType;
                if(!string.IsNullOrWhiteSpace(this.Birthplace.StreetAddress))
                    postalAddress.StreetAddress = this.Birthplace.StreetAddress;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Barangay))
                    postalAddress.Barangay = this.Birthplace.Barangay;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.City))
                    postalAddress.City = this.Birthplace.City;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Municipality))
                    postalAddress.Municipality = this.Birthplace.Municipality;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Province))
                    postalAddress.Province = this.Birthplace.Province;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.State))
                    postalAddress.State = this.Birthplace.State;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.PostalCode))
                    postalAddress.PostalCode = this.Birthplace.PostalCode;
                if (this.Birthplace.CountryId != null)
                    postalAddress.CountryId = this.Birthplace.CountryId;
                postalAddress.IsPrimary = true;

                //Context.SaveChanges();
            }

            if(party.Id == 0)
                Context.Parties.AddObject(party);
            //Context.SaveChanges();
            return partyRole;
        }

        private void InsertContactInformation(PartyRole partyRole, DateTime today)
        {
            if (this.NewCustomerId == -1)
            {
                Address address = new Address();
                address.Party = partyRole.Party;
                address.AddressType = AddressType.PostalAddressType;
                address.EffectiveDate = today;

                //Context.Addresses.AddObject(address);
                //Context.SaveChanges();

                PostalAddress postalAddress = new PostalAddress();
                postalAddress.Address = address;
                postalAddress.PostalAddressType = PostalAddressType.HomeAddressType;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.StreetAddress))
                    postalAddress.StreetAddress = this.CustomerPrimaryHomeAddress.StreetAddress;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Barangay))
                    postalAddress.Barangay = this.CustomerPrimaryHomeAddress.Barangay;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.City))
                    postalAddress.City = this.CustomerPrimaryHomeAddress.City;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Municipality))
                    postalAddress.Municipality = this.CustomerPrimaryHomeAddress.Municipality;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Province))
                    postalAddress.Province = this.CustomerPrimaryHomeAddress.Province;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.State))
                    postalAddress.State = this.CustomerPrimaryHomeAddress.State;
                if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.PostalCode))
                    postalAddress.PostalCode = this.CustomerPrimaryHomeAddress.PostalCode;
                if (this.CustomerPrimaryHomeAddress.CountryId != null)
                    postalAddress.CountryId = this.CustomerPrimaryHomeAddress.CountryId;
                postalAddress.IsPrimary = true;

               // Context.PostalAddresses.AddObject(postalAddress);

                if (this.CustomerSecondaryHomeAddress.CountryId != null)
                {
                    Address address2 = new Address();
                    address2.Party = partyRole.Party;
                    address2.AddressType = AddressType.PostalAddressType;
                    address2.EffectiveDate = today;

                    //Context.Addresses.AddObject(address2);
                    //Context.SaveChanges();

                    PostalAddress postalAddress2 = new PostalAddress();
                    postalAddress2.Address = address2;
                    postalAddress2.PostalAddressType = PostalAddressType.HomeAddressType;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.StreetAddress))
                        postalAddress2.StreetAddress = this.CustomerSecondaryHomeAddress.StreetAddress;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Barangay))
                        postalAddress2.Barangay = this.CustomerSecondaryHomeAddress.Barangay;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.City))
                        postalAddress2.City = this.CustomerSecondaryHomeAddress.City;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Municipality))
                        postalAddress2.Municipality = this.CustomerSecondaryHomeAddress.Municipality;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Province))
                        postalAddress2.Province = this.CustomerSecondaryHomeAddress.Province;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.State))
                        postalAddress2.State = this.CustomerSecondaryHomeAddress.State;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.PostalCode))
                        postalAddress2.PostalCode = this.CustomerSecondaryHomeAddress.PostalCode;
                    if (this.CustomerSecondaryHomeAddress.CountryId != null)
                        postalAddress2.CountryId = this.CustomerSecondaryHomeAddress.CountryId;
                    postalAddress2.IsPrimary = false;

                    //Context.PostalAddresses.AddObject(postalAddress2);
                }

                if (this.CustomerCellNumber != null)
                {

                    //Context.Addresses.AddObject(customerCellNumber);
                    //Context.SaveChanges();

                    if (this.CustomerCellNumber.AreaCode != null && this.CustomerCellNumber.PhoneNumber != null)
                    {
                        Address customerCellNumber = new Address();
                        customerCellNumber.AddressType = AddressType.TelecommunicationNumberType;
                        customerCellNumber.Party = partyRole.Party;
                        customerCellNumber.EffectiveDate = today;

                        TelecommunicationsNumber cellNumber = new TelecommunicationsNumber();
                        cellNumber.Address = customerCellNumber;
                        cellNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
                        cellNumber.AreaCode = this.CustomerCellNumber.AreaCode;
                        cellNumber.PhoneNumber = this.CustomerCellNumber.PhoneNumber;
                        cellNumber.IsPrimary = true;

                        //Context.TelecommunicationsNumbers.AddObject(cellNumber);
                    }
                }

                if (this.CustomerTelephoneNumber != null)
                {
                    //Context.Addresses.AddObject(customerTelephoneNumber);
                    //Context.SaveChanges();

                    if (this.CustomerTelephoneNumber.AreaCode != null && this.CustomerTelephoneNumber.PhoneNumber != null)
                    {
                        Address customerTelephoneNumber = new Address();
                        customerTelephoneNumber.AddressType = AddressType.TelecommunicationNumberType;
                        customerTelephoneNumber.Party = partyRole.Party;
                        customerTelephoneNumber.EffectiveDate = today;

                        TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
                        telephoneNumber.Address = customerTelephoneNumber;
                        telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
                        telephoneNumber.AreaCode = this.CustomerTelephoneNumber.AreaCode;
                        telephoneNumber.PhoneNumber = this.CustomerTelephoneNumber.PhoneNumber;
                        telephoneNumber.IsPrimary = true;
                        //Context.TelecommunicationsNumbers.AddObject(telephoneNumber);
                    }
                }

                if (this.CustomerPrimaryEmailAddress != null)
                {
                    //Context.Addresses.AddObject(primEmailAddress);
                    //Context.SaveChanges();

                    if (this.CustomerPrimaryEmailAddress.EletronicAddressString != null)
                    {
                        Address primEmailAddress = new Address();
                        primEmailAddress.AddressType = AddressType.ElectronicAddressType;
                        primEmailAddress.Party = partyRole.Party;
                        primEmailAddress.EffectiveDate = today;

                        ElectronicAddress primEmailAdd = new ElectronicAddress();
                        primEmailAdd.Address = primEmailAddress;
                        primEmailAdd.ElectronicAddressType = ElectronicAddressType.PersonalEmailAddressType;
                        primEmailAdd.ElectronicAddressString = this.CustomerPrimaryEmailAddress.EletronicAddressString;
                        primEmailAdd.IsPrimary = true;

                        //Context.ElectronicAddresses.AddObject(primEmailAdd);
                    }
                }

                if (this.CustomerSecondaryEmailAddress != null)
                {
                    //Context.Addresses.AddObject(secEmailAddress);
                    //Context.SaveChanges();

                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryEmailAddress.EletronicAddressString))
                    {
                        Address secEmailAddress = new Address();
                        secEmailAddress.AddressType = AddressType.ElectronicAddressType;
                        secEmailAddress.Party = partyRole.Party;
                        secEmailAddress.EffectiveDate = today;

                        ElectronicAddress secEmailAdd = new ElectronicAddress();
                        secEmailAdd.Address = secEmailAddress;
                        secEmailAdd.ElectronicAddressType = ElectronicAddressType.PersonalEmailAddressType;
                        secEmailAdd.ElectronicAddressString = this.CustomerSecondaryEmailAddress.EletronicAddressString;
                        secEmailAdd.IsPrimary = false;

                        //Context.ElectronicAddresses.AddObject(secEmailAdd);
                    }
                }

                //Context.SaveChanges();
            }
        }

        private void InsertPersonIdentification(PartyRole partyRole, DateTime today)
        {
            
            if (this.IdType1Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber1)==false))
            {
                PersonIdentification customerIdentification1 = new PersonIdentification();
                customerIdentification1.IdentificationTypeId = this.IdType1Id;
                customerIdentification1.IdNumber = this.IdNumber1;
                customerIdentification1.Person = partyRole.Party.Person;
                //Context.PersonIdentifications.AddObject(customerIdentification1);
                //Context.SaveChanges();
            }

            
            if (this.IdType2Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber2)==false) )
            {
                PersonIdentification customerIdentification2 = new PersonIdentification();
                customerIdentification2.IdentificationTypeId = this.IdType2Id;
                customerIdentification2.IdNumber = this.IdNumber2;
                customerIdentification2.Person = partyRole.Party.Person;
                //Context.PersonIdentifications.AddObject(customerIdentification2);
                //Context.SaveChanges();
            }

            
        }

        private void InsertEmploymentInformation(PartyRole partyRole, DateTime today)
        {
            if (this.NewEmployerId != -1 && this.EmployerId != -1)
            {
                PartyRole employeeRole2 = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == partyRole.PartyId && entity.PartyRoleType.RoleType.Name == RoleType.EmployeeType.Name);
                if (employeeRole2 != null)
                {
                    PartyRelationship employmentPartyRel2 = Context.PartyRelationships.SingleOrDefault(entity => entity.SecondPartyRoleId == this.EmployerId && entity.EndDate == null
                        && entity.FirstPartyRoleId == employeeRole2.Id);
                    employmentPartyRel2.EndDate = today;

                    PartyRole employerRole = Context.PartyRoles.FirstOrDefault(entity => entity.Id == this.EmployerId && entity.EndDate == null);
                    if (employerRole.RoleTypeId == RoleType.LendingInstitutionType.Id)
                    {
                        UserAccount userAccount = Context.UserAccounts.FirstOrDefault(entity => entity.PartyId == employeeRole2.PartyId && entity.EndDate == null);
                        userAccount.EndDate = today;
                    }
                }
            }

            if (this.NewEmployerId != -1 && this.EmployerId == -1)
            {
                RoleType employeeRoleType = RoleType.EmployeeType;
                PartyRole employeeRole = new PartyRole();
                employeeRole.Party = partyRole.Party;
                employeeRole.RoleTypeId = employeeRoleType.Id;
                employeeRole.EffectiveDate = today;

                //Context.PartyRoles.AddObject(employeeRole);
                //Context.SaveChanges();

                PartyRelationship employmentPartyRel = new PartyRelationship();
                employmentPartyRel.PartyRole = employeeRole;
                employmentPartyRel.SecondPartyRoleId = this.NewEmployerId;
                employmentPartyRel.PartyRelType = PartyRelType.EmploymentType;
                employmentPartyRel.EffectiveDate = today;

                //Context.PartyRelationships.AddObject(employmentPartyRel);

                Employee employee = new Employee();
                employee.PartyRole = employeeRole;
                employee.Position = this.EmployeePosition;
                if (!string.IsNullOrWhiteSpace(this.EmployeeIdNumber))
                    employee.EmployeeIdNumber = this.EmployeeIdNumber;
                if (!string.IsNullOrWhiteSpace(this.SssNumber))
                    employee.SssNumber = this.SssNumber;
                if (!string.IsNullOrWhiteSpace(this.GsisNumber))
                    employee.GsisNumber = this.GsisNumber;
                if (!string.IsNullOrWhiteSpace(this.OwaNumber))
                    employee.OwaNumber = this.OwaNumber;
                if (!string.IsNullOrWhiteSpace(this.PhicNumber))
                    employee.PhicNumber = this.PhicNumber;

                //Context.Employees.AddObject(employee);

                Employment employmentCustomer = new Employment();
                employmentCustomer.PartyRelationship = employmentPartyRel;
                employmentCustomer.Salary = this.Salary;
                employmentCustomer.EmploymentStatus = this.EmploymentStatus;

                //Context.Employments.AddObject(employmentCustomer);
            }
            else if(this.NewEmployerId != -1)
            {
                PartyRole employeeRole2 = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == partyRole.PartyId && entity.PartyRoleType.RoleType.Name == RoleType.EmployeeType.Name);
                if (employeeRole2 != null)
                {
                    PartyRelationship employmentPartyRel2 = Context.PartyRelationships.SingleOrDefault(entity => entity.SecondPartyRoleId == this.EmployerId && entity.EndDate == null
                        && entity.FirstPartyRoleId == employeeRole2.Id);

                    Employee employee = employeeRole2.Employee;
                    if (!string.IsNullOrWhiteSpace(this.EmployeePosition))
                        employee.Position = this.EmployeePosition;
                    if (!string.IsNullOrWhiteSpace(this.EmployeeIdNumber))
                        employee.EmployeeIdNumber = this.EmployeeIdNumber;
                    if (!string.IsNullOrWhiteSpace(this.SssNumber))
                        employee.SssNumber = this.SssNumber;
                    if (!string.IsNullOrWhiteSpace(this.GsisNumber))
                        employee.GsisNumber = this.GsisNumber;
                    if (!string.IsNullOrWhiteSpace(this.OwaNumber))
                        employee.OwaNumber = this.OwaNumber;
                    if (!string.IsNullOrWhiteSpace(this.PhicNumber))
                        employee.PhicNumber = this.PhicNumber;

                    Employment employmentCustomer = employmentPartyRel2.Employment;
                    employmentCustomer.Salary = this.Salary;
                    employmentCustomer.EmploymentStatus = this.EmploymentStatus;
                }
            }
            else if (this.NewEmployerId == -1 && this.EmployerId != -1)
            {
                PartyRole employeeRole2 = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == partyRole.PartyId && entity.PartyRoleType.RoleType.Name == RoleType.EmployeeType.Name);
                PartyRelationship employmentPartyRel2 = Context.PartyRelationships.SingleOrDefault(entity => entity.SecondPartyRoleId == this.EmployerId && entity.EndDate == null
                    && entity.FirstPartyRoleId == employeeRole2.Id);

                Employee employee = employeeRole2.Employee;
                if (!string.IsNullOrWhiteSpace(this.EmployeePosition))
                    employee.Position = this.EmployeePosition;
                if (!string.IsNullOrWhiteSpace(this.EmployeeIdNumber))
                    employee.EmployeeIdNumber = this.EmployeeIdNumber;
                if (!string.IsNullOrWhiteSpace(this.SssNumber))
                    employee.SssNumber = this.SssNumber;
                if (!string.IsNullOrWhiteSpace(this.GsisNumber))
                    employee.GsisNumber = this.GsisNumber;
                if (!string.IsNullOrWhiteSpace(this.OwaNumber))
                    employee.OwaNumber = this.OwaNumber;
                if (!string.IsNullOrWhiteSpace(this.PhicNumber))
                    employee.PhicNumber = this.PhicNumber;

                Employment employmentCustomer = employmentPartyRel2.Employment;
                employmentCustomer.Salary = this.Salary;
                employmentCustomer.EmploymentStatus = this.EmploymentStatus;
            }
            //Context.SaveChanges();
        }

        private void InsertSpouseInformation(PartyRole partyRole, DateTime today)
        {
            MaritalStatusType married = MaritalStatusType.MarriedType;

            if (married.Id == this.MaritalStatusId)
            {
                Party spouseParty = new Party();
                spouseParty.PartyType = PartyType.PersonType;

                RoleType spouseRoleType = RoleType.SpouseType;

                
                //Context.SaveChanges();

                PartyRole spousePartyRole = new PartyRole();
                spousePartyRole.Party = spouseParty;
                spousePartyRole.PartyRoleType = spouseRoleType.PartyRoleType;
                spousePartyRole.EffectiveDate = today;

                //Context.PartyRoles.AddObject(spousePartyRole);

                PartyRole customerAsSpouseRole = new PartyRole();
                customerAsSpouseRole.Party = partyRole.Party;
                customerAsSpouseRole.PartyRoleType = spouseRoleType.PartyRoleType;
                customerAsSpouseRole.EffectiveDate = today;

                Context.PartyRoles.AddObject(customerAsSpouseRole);

                Person spousePerson = new Person();
                spousePerson.Party = spouseParty;

                PartyRelationship spouseRelationship = new PartyRelationship();
                spouseRelationship.PartyRole = customerAsSpouseRole;
                spouseRelationship.PartyRole1 = spousePartyRole;
                spouseRelationship.PartyRelType = PartyRelType.SpousalRelationshiptType;
                spouseRelationship.EffectiveDate = today;

                if (this.GenderTypeId == GenderType.FemaleType.Id)
                {
                    spousePerson.GenderType = GenderType.MaleType;
                }
                else
                {
                    spousePerson.GenderType = GenderType.FemaleType;
                }

                if (this.SpouseBirthDate != null)
                    spousePerson.Birthdate = this.SpouseBirthDate;

                //Context.People.AddObject(spousePerson);

                //Insert Spouse name
                Person.CreatePersonName(spousePerson, today, this.SpouseName.Title, 
                                        this.SpouseName.FirstName, this.SpouseName.MiddleName, 
                                        this.SpouseName.LastName, this.SpouseName.NameSuffix, 
                                        this.SpouseName.NickName);
                Person.CreateOrUpdatePersonNames(spousePerson, PersonNameType.MothersMaidenNameType, this.SpouseName.MothersMaidenName, today);
                Context.Parties.AddObject(spouseParty);
            }

            //Context.SaveChanges();
        }

        private void SaveCustomerSourcesOfIncome(Customer customer, DateTime today)
        {
            foreach (CustomerSourceOfIncomeModel sourceOfIncome in SourcesOfIncome)
            {
                if (sourceOfIncome.IsNew)
                {
                    SourceOfIncome income = SourceOfIncome.GetById(sourceOfIncome.SourceOfIncomeId);
                    var customerSourceOfIncome = CustomerSourceOfIncome.Create(income, customer, today);
                    customerSourceOfIncome.Income = sourceOfIncome.Amount;
                    Context.CustomerSourceOfIncomes.AddObject(customerSourceOfIncome);
                    //Context.SaveChanges();
                }
                else if (sourceOfIncome.ToBeDeleted)
                {
                    CustomerSourceOfIncome customerSourceOfIncome =
                        CustomerSourceOfIncome.GetById(sourceOfIncome.CustomerSourceOfIncomeId);
                    customerSourceOfIncome.EndDate = today;
                    //Context.SaveChanges();
                }
                else if (sourceOfIncome.IsEdited)
                {
                    SourceOfIncome income = SourceOfIncome.GetById(sourceOfIncome.SourceOfIncomeId);
                    var customerSourceOfIncome = CustomerSourceOfIncome.CreateOrUpdate(income, customer, today);
                    customerSourceOfIncome.Income = sourceOfIncome.Amount;
                }
            }
            //Context.SaveChanges();
        }

        private void RetrieveCustomerSourcesOfIncome(Customer customer)
        {
            var amount = CustomerSourceOfIncome.GetAllActive(customer);
            this.SourcesOfIncome = new List<CustomerSourceOfIncomeModel>();
            foreach (CustomerSourceOfIncome item in amount)
            {
                this.SourcesOfIncome.Add(new CustomerSourceOfIncomeModel(item));
            }
        }

        private void RetrieveBasicInformation(PartyRole partyRole)
        {
            this.CustomerCellNumber.AreaCode = string.Empty;
            this.CustomerCellNumber.PhoneNumber = string.Empty;
            this.CustomerTelephoneNumber.AreaCode = string.Empty;
            this.CustomerTelephoneNumber.PhoneNumber = string.Empty;

            this.CustomerId = partyRole.Id;
            Party party = partyRole.Party;
            Person personAsCustomer = party.Person;
            Customer customer = partyRole.Customer;
            CustomerCategory customerCategory = customer.CurrentCustomerCategory;

            this.CustomerCategoryTypeId = customerCategory.CustomerCategoryType;


            this.GenderTypeId = (int)personAsCustomer.GenderTypeId;

            this.CustomerName.NameConcat = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                        , personAsCustomer.FirstNameString + " ", personAsCustomer.MiddleInitialString,
                        personAsCustomer.NameSuffixString);
            if (!string.IsNullOrWhiteSpace(personAsCustomer.TitleString))
                this.CustomerName.Title = personAsCustomer.TitleString;
            this.CustomerName.FirstName = personAsCustomer.FirstNameString;
            this.CustomerName.LastName = personAsCustomer.LastNameString;
            if (!string.IsNullOrWhiteSpace(personAsCustomer.MiddleNameString))
                this.CustomerName.MiddleName = personAsCustomer.MiddleNameString;
            this.CustomerName.MothersMaidenName = personAsCustomer.MothersMaidenNameString;
            if (!string.IsNullOrWhiteSpace(personAsCustomer.NameSuffixString))
                this.CustomerName.NameSuffix = personAsCustomer.NameSuffixString;
            if (!string.IsNullOrWhiteSpace(personAsCustomer.NickNameString))
                this.CustomerName.NickName = personAsCustomer.NickNameString;

            if(!string.IsNullOrWhiteSpace(personAsCustomer.ImageFilename))
                this.ImgUrl = personAsCustomer.ImageFilename;
            this.CreditLimit = customer.CreditLimit;
            this.Remarks = customer.Remarks;

            this.Birthdate = (DateTime)personAsCustomer.Birthdate;
            this.CustomerStatus = customer.CurrentStatus.CustomerStatusType.Name;

            this.MaritalStatusId = MaritalStatu.GetActive(personAsCustomer).MaritalStatusTypeId;
            this.NumberOfDependents = MaritalStatu.GetActive(personAsCustomer).NumberOfDependents;
            this.EducationalAttainmentId = (int)personAsCustomer.EducAttainmentTypeId;

            this.NationalityId = personAsCustomer.NationalityTypeId;

            if (customer.CurrentClassification != null)
            {
                this.DistrictClassificationTypeId = customer.CurrentClassification.ClassificationTypeId;
                this.StationNumber = customer.CurrentClassification.ClassificationType.StationNumber;
                this.District = customer.CurrentClassification.ClassificationType.District;
            }

            this.HomeOwnershipId = personAsCustomer.CurrentHomeOwnership.HomeOwnershipTypeId;
            this.ResidentSince = personAsCustomer.CurrentHomeOwnership.ResidenceSince;

            if (partyRole.Taxpayer != null)
            {
                Taxpayer taxpayer = partyRole.Taxpayer;
                this.Tin = taxpayer.Tin;
            

                if (taxpayer.CurrentCtc != null)
                {
                    this.CtcNumber = taxpayer.CurrentCtc.CtcNumber;
                    this.DateIssued = taxpayer.CurrentCtc.DateIssued;
                    this.PlaceIssued = taxpayer.CurrentCtc.IssuedWhere;
                }
            }
            Address postalBirthplace = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.PartyId == party.Id
                    && entity.AddressTypeId == AddressType.PostalAddressType.Id && entity.PostalAddress.PostalAddressTypeId== PostalAddressType.BirthPlaceType.Id 
                    && entity.PostalAddress.IsPrimary);

            if (postalBirthplace != null && postalBirthplace.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalBirthplace.PostalAddress;

                this.Birthplace.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);

                this.CustomerCountryCode = postalAddressSpecific.Country.CountryTelephoneCode;
                this.Birthplace.StreetAddress = postalAddressSpecific.StreetAddress;
                this.Birthplace.State = postalAddressSpecific.State;
                this.Birthplace.Province = postalAddressSpecific.Province;
                this.Birthplace.PostalCode = postalAddressSpecific.PostalCode;
                this.Birthplace.Municipality = postalAddressSpecific.Municipality;
                this.Birthplace.City = postalAddressSpecific.City;
                this.Birthplace.Barangay = postalAddressSpecific.Barangay;
                this.Birthplace.CountryId = postalAddressSpecific.CountryId;
            }
        }

        private void RetrieveIdentifications(PartyRole partyRole)
        {
            Party party = partyRole.Party;
            Person person = party.Person;

            var ids = person.PersonIdentificationsList;

            if (ids.Count() == 2)
            {
                this.IdType1Id = ids.First().IdentificationTypeId;
                this.IdNumber1 = ids.First().IdNumber;

                this.IdType2Id = ids.Skip(1).Take(1).SingleOrDefault().IdentificationTypeId;
                this.IdNumber2 = ids.Skip(1).Take(1).SingleOrDefault().IdNumber;
            }
            else if (ids.Count() == 1)
            {
                this.IdType1Id = ids.First().IdentificationTypeId;
                this.IdNumber1 = ids.First().IdNumber;
            }


        }

        private void RetrieveContactInformation(PartyRole partyRole)
        {
            Party party = partyRole.Party;
            Person personAsCustomer = party.Person;
            Customer customer = partyRole.Customer;

            //Primary Home Address
            Address postalAddress = party.Addresses.SingleOrDefault(entity => entity.EndDate == null
                                && entity.PartyId == party.Id && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id 
                                && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                this.CustomerPrimaryHomeAddress.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);

                this.CustomerPrimaryHomeAddress.StreetAddress = postalAddressSpecific.StreetAddress;
                this.CustomerPrimaryHomeAddress.State = postalAddressSpecific.State;
                this.CustomerPrimaryHomeAddress.Province = postalAddressSpecific.Province;
                this.CustomerPrimaryHomeAddress.PostalCode = postalAddressSpecific.PostalCode;
                this.CustomerPrimaryHomeAddress.Municipality = postalAddressSpecific.Municipality;
                this.CustomerPrimaryHomeAddress.City = postalAddressSpecific.City;
                this.CustomerPrimaryHomeAddress.Barangay = postalAddressSpecific.Barangay;
                this.CustomerPrimaryHomeAddress.CountryId = postalAddressSpecific.CountryId;

                this.CustomerCountryCode = postalAddressSpecific.Country.CountryTelephoneCode;

            //Secondary Home Address
                Address postalAddressSecond = new Address();
                postalAddressSecond = party.Addresses.SingleOrDefault(entity => entity.EndDate == null 
                                && entity.AddressTypeId == AddressType.PostalAddressType.Id && entity.PostalAddress.IsPrimary == false
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id);

            if (postalAddressSecond != null && postalAddressSecond.PostalAddress != null)
            {
                PostalAddress postalAddressSpecificSecond = postalAddressSecond.PostalAddress;

                this.CustomerSecondaryHomeAddress.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecificSecond.StreetAddress,
                              postalAddressSpecificSecond.Barangay,
                              postalAddressSpecificSecond.Municipality,
                              postalAddressSpecificSecond.City,
                              postalAddressSpecificSecond.Province,
                              postalAddressSpecificSecond.State,
                              postalAddressSpecificSecond.Country.Name,
                              postalAddressSpecificSecond.PostalCode);

                this.CustomerSecondaryHomeAddress.StreetAddress = postalAddressSpecificSecond.StreetAddress;
                this.CustomerSecondaryHomeAddress.State = postalAddressSpecificSecond.State;
                this.CustomerSecondaryHomeAddress.Province = postalAddressSpecificSecond.Province;
                this.CustomerSecondaryHomeAddress.PostalCode = postalAddressSpecificSecond.PostalCode;
                this.CustomerSecondaryHomeAddress.Municipality = postalAddressSpecificSecond.Municipality;
                this.CustomerSecondaryHomeAddress.City = postalAddressSpecificSecond.City;
                this.CustomerSecondaryHomeAddress.Barangay = postalAddressSpecificSecond.Barangay;
                this.CustomerSecondaryHomeAddress.CountryId = postalAddressSpecificSecond.CountryId;
            }
                //Cellphone Number
                Address cellTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                    && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.PersonalMobileNumberType
                                    && entity.TelecommunicationsNumber.IsPrimary);

                if (cellTelecomNumber != null && cellTelecomNumber.TelecommunicationsNumber != null)
                {

                    TelecommunicationsNumber cellTelecomNumberSpecific = cellTelecomNumber.TelecommunicationsNumber;
                    this.CustomerCellNumber.PhoneNumber = cellTelecomNumberSpecific.PhoneNumber;
                    this.CustomerCellNumber.AreaCode = cellTelecomNumberSpecific.AreaCode;
                }

                //Telephone Number
                Address primTelecomNumber = party.Addresses.SingleOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                    && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.HomePhoneNumberType
                                    && entity.TelecommunicationsNumber.IsPrimary);

                if (primTelecomNumber != null && primTelecomNumber.TelecommunicationsNumber != null)
                {
                    TelecommunicationsNumber primNumberSpecific = primTelecomNumber.TelecommunicationsNumber;
                    this.CustomerTelephoneNumber.PhoneNumber = primNumberSpecific.PhoneNumber;
                    this.CustomerTelephoneNumber.AreaCode = primNumberSpecific.AreaCode;
                }
            }

            //Primary Email Address
            Address primEmailAddress = new Address();
            primEmailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.PersonalEmailAddressType
                                && entity.ElectronicAddress.IsPrimary);

            if (primEmailAddress != null && primEmailAddress.ElectronicAddress != null)
            {
                ElectronicAddress primaryEmailAddressSpecific = primEmailAddress.ElectronicAddress;
                this.CustomerPrimaryEmailAddress.EletronicAddressString = primaryEmailAddressSpecific.ElectronicAddressString;
            }

            //Secondary Email Address
            Address secondEmailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.PersonalEmailAddressType
                                && entity.ElectronicAddress.IsPrimary == false);
            if (secondEmailAddress != null && secondEmailAddress.ElectronicAddress != null)
            {
                ElectronicAddress secondEmailAddressSpecific = secondEmailAddress.ElectronicAddress;
                this.CustomerSecondaryEmailAddress.EletronicAddressString = secondEmailAddressSpecific.ElectronicAddressString;
            }
        }

        private void RetrieveEmploymentInformation(PartyRole partyRole)
        {
            Party customerParty = partyRole.Party;
            var employeePartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == customerParty.Id && entity.RoleTypeId == RoleType.EmployeeType.Id
                            && entity.EndDate == null);
            if (employeePartyRole != null)
            {
                PartyRelationship employmentPartyRel = employeePartyRole.CurrentEmploymentRelationship;

                if (employmentPartyRel != null)
                {
                    this.EmployerId = employmentPartyRel.SecondPartyRoleId;
                    var employerPartyRole1 = Context.PartyRoles.SingleOrDefault(entity => entity.Id == employmentPartyRel.SecondPartyRoleId
                        && entity.EndDate == null);
                    PartyRole employerPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == employerPartyRole1.PartyId
                        && entity.EndDate == null && (entity.PartyRoleType.RoleTypeId == RoleType.EmployerType.Id 
                        || entity.PartyRoleType.RoleTypeId == RoleType.LendingInstitutionType.Id));
                    Party employerParty = employerPartyRole.Party;

                    this.EmploymentStatus = employmentPartyRel.Employment.EmploymentStatus;
                    this.Salary = employmentPartyRel.Employment.Salary;

                    Employee employee = employeePartyRole.Employee;
                    this.EmployeeIdNumber = employee.EmployeeIdNumber;
                    this.EmployeePosition = employee.Position;
                    this.SssNumber = employee.SssNumber;
                    this.GsisNumber = employee.GsisNumber;
                    this.OwaNumber = employee.OwaNumber;
                    this.PhicNumber = employee.PhicNumber;

                    //retrieve employer details

                    //Employer Name
                    if (employerParty.PartyTypeId == PartyType.OrganizationType.Id)
                    {
                        Organization employerAsOrg = employerParty.Organization;
                        this.EmployerName = employerAsOrg.OrganizationName;
                    }
                    else
                    {
                        Person employerAsPerson = employerParty.Person;
                        this.EmployerName = StringConcatUtility.Build(" ", employerAsPerson.LastNameString + ","
                                                , employerAsPerson.FirstNameString + " ", employerAsPerson.MiddleInitialString,
                                                employerAsPerson.NameSuffixString);
                    }

                    //Employer Addresses
                    Address postalAddress = employerParty.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                        && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.BusinessAddressType.Id && entity.PostalAddress.IsPrimary);

                    if (postalAddress != null && postalAddress.PostalAddress != null)
                    {
                        PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                        this.EmploymentAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                      postalAddressSpecific.Barangay,
                                      postalAddressSpecific.Municipality,
                                      postalAddressSpecific.City,
                                      postalAddressSpecific.Province,
                                      postalAddressSpecific.State,
                                      postalAddressSpecific.Country.Name,
                                      postalAddressSpecific.PostalCode);

                        this.EmployerCountryCode = postalAddressSpecific.Country.CountryTelephoneCode;
                        //Business Telephone Number
                        Address telecomNumber = employerParty.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                                            && entity.TelecommunicationsNumber.TypeId == TelecommunicationsNumberType.BusinessPhoneNumberType.Id
                                            && entity.TelecommunicationsNumber.IsPrimary);

                        if (telecomNumber != null && telecomNumber.TelecommunicationsNumber != null)
                        {

                            TelecommunicationsNumber telecomNumberSpecific = telecomNumber.TelecommunicationsNumber;
                            this.EmployerTelephoneNumber = telecomNumberSpecific.PhoneNumber;
                            this.EmployerTelephoneAreaCode = telecomNumberSpecific.AreaCode;
                        }

                        //Business Fax Number
                        Address faxNumber = employerParty.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                                            && entity.TelecommunicationsNumber.TypeId == TelecommunicationsNumberType.BusinessFaxNumberType.Id
                                            && entity.TelecommunicationsNumber.IsPrimary);

                        if (faxNumber != null && faxNumber.TelecommunicationsNumber != null)
                        {
                            TelecommunicationsNumber faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                            this.EmployerFaxNumber = faxNumberSpecific.PhoneNumber;
                            this.EmployerFaxAreaCode = faxNumberSpecific.AreaCode;
                        }
                    }

                    //Email Address
                    Address emailAddress = employerParty.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.ElectronicAddressType.Id
                                        && entity.ElectronicAddress.ElectronicAddressTypeId == ElectronicAddressType.BusinessEmailAddressType.Id
                                        && entity.ElectronicAddress.IsPrimary);
                    if (emailAddress != null && emailAddress.ElectronicAddress != null)
                    {
                        ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                        this.EmployerEmailAddress = primaryEmailAddressSpecific.ElectronicAddressString;
                    }
                }
            }
        }

        private void RetrieveSpouseInformation(PartyRole partyRole)
        {
            Party customerParty = partyRole.Party;
            PartyRole spouseRole = Context.PartyRoles.FirstOrDefault(entity => entity.PartyId == customerParty.Id 
                                                                        && entity.RoleTypeId == RoleType.SpouseType.Id 
                                                                        && entity.EndDate == null);


            if (spouseRole != null)
            {

                PartyRelationship spousePartyRel = spouseRole.CurrentSpousalRelationship;

                if (spousePartyRel != null)
                {
                    PartyRole spousePartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == spousePartyRel.SecondPartyRoleId
                        && entity.EndDate == null && entity.PartyRoleType.RoleTypeId == RoleType.SpouseType.Id);
                    Party spouseParty = spousePartyRole.Party;
                    Person spouse = spouseParty.Person;

                    this.SpouseName.NameConcat = StringConcatUtility.Build(" ", spouse.LastNameString + ","
                                        , spouse.FirstNameString + " ", spouse.MiddleInitialString,
                                        spouse.NameSuffixString);
                    if (!string.IsNullOrWhiteSpace(spouse.TitleString))
                        this.SpouseName.Title = spouse.TitleString;
                    this.SpouseName.FirstName = spouse.FirstNameString;
                    this.SpouseName.LastName = spouse.LastNameString;
                    if (!string.IsNullOrWhiteSpace(spouse.MiddleNameString))
                        this.SpouseName.MiddleName = spouse.MiddleNameString;
                    if (!string.IsNullOrWhiteSpace(spouse.MothersMaidenNameString))
                        this.SpouseName.MothersMaidenName = spouse.MothersMaidenNameString;
                    if (!string.IsNullOrWhiteSpace(spouse.NameSuffixString))
                        this.SpouseName.NameSuffix = spouse.NameSuffixString;
                    if (!string.IsNullOrWhiteSpace(spouse.NickNameString))
                        this.SpouseName.NickName = spouse.NickNameString;

                    if (spouse.Birthdate != null)
                        this.SpouseBirthDate = (DateTime)spouse.Birthdate;
                }
            }
        }

        private void UpdateBasicInformation(DateTime today)
        {

            PartyRole oldPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.CustomerId && 
                entity.EndDate == null && entity.PartyRoleType.RoleType.Id == RoleType.CustomerType.Id);
            Party party = oldPartyRole.Party;

            Customer customer = oldPartyRole.Customer;
            customer.CreditLimit = this.CreditLimit;

            CustomerCategory oldCustomerCategory = customer.CurrentCustomerCategory;
            if (oldCustomerCategory.CustomerCategoryType != this.CustomerCategoryTypeId)
            {
                oldCustomerCategory.EndDate = today;

                CustomerCategory customerCategory = new CustomerCategory();
                customerCategory.Customer = customer;
                customerCategory.CustomerCategoryType = this.CustomerCategoryTypeId;
                customerCategory.EffectiveDate = today;
            }

            if (this.Remarks != null)
                customer.Remarks = this.Remarks;

            Person person = party.Person;
            person.GenderTypeId = this.GenderTypeId;
            person.NationalityTypeId = this.NationalityId;
            person.EducAttainmentTypeId = this.EducationalAttainmentId;
            person.Birthdate = this.Birthdate;
            person.ImageFilename = this.ImgUrl;

            //Update customer name
            Person.CreatePersonName(person, today, this.CustomerName.Title,
                                        this.CustomerName.FirstName, this.CustomerName.MiddleName,
                                        this.CustomerName.LastName, this.CustomerName.NameSuffix,
                                        this.CustomerName.NickName);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.MothersMaidenNameType, this.CustomerName.MothersMaidenName, today);
                
            var currentCustomerClassification = customer.CurrentClassification;
            if (currentCustomerClassification.ClassificationTypeId != this.DistrictClassificationTypeId)
            {
                currentCustomerClassification.EndDate = today;

                CustomerClassification customerClassification = new CustomerClassification();
                customerClassification.Customer = customer;
                customerClassification.ClassificationTypeId = this.DistrictClassificationTypeId;
                customerClassification.EffectiveDate = today;

                Context.CustomerClassifications.AddObject(customerClassification);
            }
            

            //CustomerStatusType customerNew = CustomerStatusType.NewType;
            //var currentStatus = customer.CurrentStatus;
            //if (currentStatus.CustomerStatusType.Name != this.CustomerStatus)
            //{
            //    currentStatus.IsActive = false;

            //    CustomerStatu customerStatus = new CustomerStatu();
            //    customerStatus.Customer = customer;
            //    customerStatus.CustomerStatusType = customerNew;
            //    customerStatus.TransitionDateTime = today;
            //    customerStatus.IsActive = true;

            //    Context.CustomerStatus.AddObject(customerStatus);
            //}

            var currentMaritalStatus = MaritalStatu.GetActive(person);
            if (currentMaritalStatus.MaritalStatusTypeId != this.MaritalStatusId)
            {
                currentMaritalStatus.IsActive = false;

                MaritalStatu customerMaritalStatus = new MaritalStatu();
                customerMaritalStatus.Person = person;
                customerMaritalStatus.MaritalStatusTypeId = this.MaritalStatusId;
                customerMaritalStatus.NumberOfDependents = this.NumberOfDependents;
                customerMaritalStatus.TransitionDateTime = today;
                customerMaritalStatus.IsActive = true;

                Context.MaritalStatus.AddObject(customerMaritalStatus);
            }
            else if (currentMaritalStatus.NumberOfDependents != this.NumberOfDependents)
            {
                currentMaritalStatus.NumberOfDependents = this.NumberOfDependents;
            }

            var currentHomeOwnership = person.CurrentHomeOwnership;
            if (currentHomeOwnership.HomeOwnershipTypeId != this.HomeOwnershipId || currentHomeOwnership.ResidenceSince != this.ResidentSince)
            {
                currentHomeOwnership.EndDate = today;

                HomeOwnership custHomeOwnership = new HomeOwnership();
                custHomeOwnership.Person = person;
                custHomeOwnership.HomeOwnershipTypeId = this.HomeOwnershipId;
                custHomeOwnership.ResidenceSince = this.ResidentSince;
                custHomeOwnership.EffectiveDate = today;

                Context.HomeOwnerships.AddObject(custHomeOwnership);
            }

            Taxpayer custTin = oldPartyRole.Taxpayer;
            if (this.Tin != null && custTin != null)
            {
                custTin.Tin = this.Tin;
            }
            else if (custTin == null && this.Tin != null)
            {
                custTin = new Taxpayer();
                custTin.Tin = this.Tin;
                custTin.PartyRole = oldPartyRole;
            }
            else if (custTin != null && this.Tin == null)
            {

                custTin.Tin = this.Tin;
            }

            if (custTin != null && this.CtcNumber == null && custTin.CurrentCtc != null)
            {
                if (custTin.Ctcs.Count() == 1)
                {
                    Context.Ctcs.DeleteObject(custTin.CurrentCtc);
                }
                else if (custTin.Ctcs.Count() > 1)
                {
                    foreach (var ctc in custTin.Ctcs)
                    {
                        Context.Ctcs.DeleteObject(custTin.CurrentCtc);
                    }
                }
            }
            else if (custTin != null && custTin.CurrentCtc != null)
            {
                if (this.CtcNumber != null && this.DateIssued != null && this.PlaceIssued != null)
                {
                    Ctc customerCtc = custTin.CurrentCtc;
                    customerCtc.CtcNumber = this.CtcNumber;
                    customerCtc.DateIssued = this.DateIssued;
                    customerCtc.IssuedWhere = this.PlaceIssued;
                }
            }
            else if (custTin != null && custTin.CurrentCtc == null)
            {
                if (!string.IsNullOrEmpty(this.CtcNumber) && this.DateIssued != null && !string.IsNullOrEmpty(this.PlaceIssued))
                {
                    Ctc customerCtc = new Ctc();
                    customerCtc.Taxpayer = custTin;
                    customerCtc.CtcNumber = this.CtcNumber;
                    customerCtc.DateIssued = this.DateIssued;
                    customerCtc.IssuedWhere = this.PlaceIssued;

                    Context.Ctcs.AddObject(customerCtc);
                }
            }


            PostalAddress currentBirthplaceAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.BirthPlaceType, entity=>entity.PostalAddress.IsPrimary);

            if (currentBirthplaceAddress != null)
            {
                PostalAddress postalAddress = new PostalAddress();
                if (!string.IsNullOrWhiteSpace(this.Birthplace.StreetAddress))
                    postalAddress.StreetAddress = this.Birthplace.StreetAddress;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Barangay))
                    postalAddress.Barangay = this.Birthplace.Barangay;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.City))
                    postalAddress.City = this.Birthplace.City;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Municipality))
                    postalAddress.Municipality = this.Birthplace.Municipality;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.Province))
                    postalAddress.Province = this.Birthplace.Province;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.State))
                    postalAddress.State = this.Birthplace.State;
                if (!string.IsNullOrWhiteSpace(this.Birthplace.PostalCode))
                    postalAddress.PostalCode = this.Birthplace.PostalCode;
                if (this.Birthplace.CountryId != null)
                    postalAddress.CountryId = this.Birthplace.CountryId;
                postalAddress.IsPrimary = true;

                PostalAddress.CreateOrUpdatePostalAddress(party, postalAddress, PostalAddressType.BirthPlaceType, today, true);
            }
            else
            {
                if (this.Birthplace.CountryId != 0)
                {
                    PostalAddress postalAddress = new PostalAddress();
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.StreetAddress))
                        postalAddress.StreetAddress = this.Birthplace.StreetAddress;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.Barangay))
                        postalAddress.Barangay = this.Birthplace.Barangay;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.City))
                        postalAddress.City = this.Birthplace.City;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.Municipality))
                        postalAddress.Municipality = this.Birthplace.Municipality;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.Province))
                        postalAddress.Province = this.Birthplace.Province;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.State))
                        postalAddress.State = this.Birthplace.State;
                    if (!string.IsNullOrWhiteSpace(this.Birthplace.PostalCode))
                        postalAddress.PostalCode = this.Birthplace.PostalCode;
                    if (this.Birthplace.CountryId != null)
                        postalAddress.CountryId = this.Birthplace.CountryId;
                    postalAddress.IsPrimary = true;

                    PostalAddress.CreateOrUpdatePostalAddress(party, postalAddress, PostalAddressType.BirthPlaceType, today, true);
                }
            }

        }

        private void UpdateContactInformation(DateTime today)
        {
            PartyRole oldPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.CustomerId &&
                entity.EndDate == null && entity.PartyRoleType.RoleType.Id == RoleType.CustomerType.Id);
            Party party = oldPartyRole.Party;

            //Update Primary Home Address
            PostalAddress currentPrimAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, entity => entity.PostalAddress.IsPrimary);
            PostalAddress newPostalAddress = new PostalAddress();
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.StreetAddress))
                newPostalAddress.StreetAddress = this.CustomerPrimaryHomeAddress.StreetAddress;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Barangay))
                newPostalAddress.Barangay = this.CustomerPrimaryHomeAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.City))
                newPostalAddress.City = this.CustomerPrimaryHomeAddress.City;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Municipality))
                newPostalAddress.Municipality = this.CustomerPrimaryHomeAddress.Municipality;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.Province))
                newPostalAddress.Province = this.CustomerPrimaryHomeAddress.Province;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.State))
                newPostalAddress.State = this.CustomerPrimaryHomeAddress.State;
            if (!string.IsNullOrWhiteSpace(this.CustomerPrimaryHomeAddress.PostalCode))
                newPostalAddress.PostalCode = this.CustomerPrimaryHomeAddress.PostalCode;
            if (this.CustomerPrimaryHomeAddress.CountryId != null)
                newPostalAddress.CountryId = this.CustomerPrimaryHomeAddress.CountryId;
            newPostalAddress.IsPrimary = true;

            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, PostalAddressType.HomeAddressType, today, true);

            //Update Secondary Postal Address
            PostalAddress currentSecAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, entity => !entity.PostalAddress.IsPrimary);

            if (this.CustomerSecondaryHomeAddress != null)
            {
                PostalAddress postalAddress2 = new PostalAddress();
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.StreetAddress))
                    postalAddress2.StreetAddress = this.CustomerSecondaryHomeAddress.StreetAddress;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Barangay))
                    postalAddress2.Barangay = this.CustomerSecondaryHomeAddress.Barangay;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.City))
                    postalAddress2.City = this.CustomerSecondaryHomeAddress.City;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Municipality))
                    postalAddress2.Municipality = this.CustomerSecondaryHomeAddress.Municipality;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Province))
                    postalAddress2.Province = this.CustomerSecondaryHomeAddress.Province;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.State))
                    postalAddress2.State = this.CustomerSecondaryHomeAddress.State;
                if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.PostalCode))
                    postalAddress2.PostalCode = this.CustomerSecondaryHomeAddress.PostalCode;
                if (this.CustomerSecondaryHomeAddress.CountryId != null)
                    postalAddress2.CountryId = this.CustomerSecondaryHomeAddress.CountryId;
                postalAddress2.IsPrimary = false;

                PostalAddress.CreateOrUpdatePostalAddress(party, postalAddress2, PostalAddressType.HomeAddressType, today, false);
            }
            else
            {
                if (this.CustomerSecondaryHomeAddress.CountryId != 0)
                {
                    PostalAddress postalAddress2 = new PostalAddress();
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.StreetAddress))
                        postalAddress2.StreetAddress = this.CustomerSecondaryHomeAddress.StreetAddress;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Barangay))
                        postalAddress2.Barangay = this.CustomerSecondaryHomeAddress.Barangay;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.City))
                        postalAddress2.City = this.CustomerSecondaryHomeAddress.City;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Municipality))
                        postalAddress2.Municipality = this.CustomerSecondaryHomeAddress.Municipality;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.Province))
                        postalAddress2.Province = this.CustomerSecondaryHomeAddress.Province;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.State))
                        postalAddress2.State = this.CustomerSecondaryHomeAddress.State;
                    if (!string.IsNullOrWhiteSpace(this.CustomerSecondaryHomeAddress.PostalCode))
                        postalAddress2.PostalCode = this.CustomerSecondaryHomeAddress.PostalCode;
                    if (this.CustomerSecondaryHomeAddress.CountryId != null)
                        postalAddress2.CountryId = this.CustomerSecondaryHomeAddress.CountryId;
                    postalAddress2.IsPrimary = false;

                    PostalAddress.CreateOrUpdatePostalAddress(party, postalAddress2, PostalAddressType.HomeAddressType, today, false);
                }
            }

            TelecommunicationsNumber cellNumber = new TelecommunicationsNumber();
            cellNumber.AreaCode = this.CustomerCellNumber.AreaCode;
            cellNumber.PhoneNumber = this.CustomerCellNumber.PhoneNumber;
            cellNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
            cellNumber.IsPrimary = true;

            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, cellNumber, TelecommunicationsNumberType.PersonalMobileNumberType, today, true);


            //Update Telephone Number

            TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
            telephoneNumber.AreaCode = this.CustomerTelephoneNumber.AreaCode;
            telephoneNumber.PhoneNumber = this.CustomerTelephoneNumber.PhoneNumber;
            telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
            telephoneNumber.IsPrimary = true;

            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, telephoneNumber, TelecommunicationsNumberType.HomePhoneNumberType, today, true);



            ElectronicAddress primEmailAdd = new ElectronicAddress();
            primEmailAdd.ElectronicAddressString = this.CustomerPrimaryEmailAddress.EletronicAddressString;
            primEmailAdd.IsPrimary = true;

            ElectronicAddress.CreateOrUpdateElectronicAddress(party, primEmailAdd, ElectronicAddressType.PersonalEmailAddressType, 
                                                                entity => entity.ElectronicAddress.IsPrimary, today);
   

            ElectronicAddress secEmailAdd = new ElectronicAddress();
            secEmailAdd.ElectronicAddressString = this.CustomerSecondaryEmailAddress.EletronicAddressString;
            secEmailAdd.IsPrimary = false;

            ElectronicAddress.CreateOrUpdateElectronicAddress(party, secEmailAdd, ElectronicAddressType.PersonalEmailAddressType, 
                                                                entity => entity.ElectronicAddress.IsPrimary == false, today);

            //-->end
        }

        private void UpdateIdentifications(DateTime today)
        {
            PartyRole oldPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.CustomerId &&
                entity.EndDate == null && entity.PartyRoleType.RoleType.Id == RoleType.CustomerType.Id);
            Party party = oldPartyRole.Party;

            Person person = party.Person;

            List<PersonIdentification> ids = person.PersonIdentificationsList.ToList();

            if (ids.Count() == 2)
            {
                if (this.IdType1Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber1) == false))
                {
                    PersonIdentification customerIdentification1 = ids.First();
                    customerIdentification1.IdentificationTypeId = this.IdType1Id;
                    customerIdentification1.IdNumber = this.IdNumber1;
                }

                if (this.IdType2Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber2) == false))
                {
                    PersonIdentification customerIdentification2 = ids.Skip(1).Take(1).SingleOrDefault();
                    customerIdentification2.IdentificationTypeId = this.IdType2Id;
                    customerIdentification2.IdNumber = this.IdNumber2;
                }

                if (this.IdType1Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber1)))
                {
                    PersonIdentification customerId1 = ids.First();
                    Context.PersonIdentifications.DeleteObject(customerId1);
                }

                if (this.IdType2Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber2)))
                {
                    PersonIdentification customerId2 = ids.Skip(1).Take(1).SingleOrDefault();
                    Context.PersonIdentifications.DeleteObject(customerId2);
                }
            }
            else if (ids.Count() == 1)
            {
                if (this.IdType1Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber1) == false))
                {
                    PersonIdentification customerIdentification1 = ids.First();
                    customerIdentification1.IdentificationTypeId = this.IdType1Id;
                    customerIdentification1.IdNumber = this.IdNumber1;
                }

                if (this.IdType1Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber1)))
                {
                    PersonIdentification customerId1 = ids.First();
                    Context.PersonIdentifications.DeleteObject(customerId1);
                }

                if (this.IdType2Id != 0 && (string.IsNullOrWhiteSpace(this.IdNumber2) == false))
                {
                    PersonIdentification customerIdentification2 = new PersonIdentification();
                    customerIdentification2.IdentificationTypeId = this.IdType2Id;
                    customerIdentification2.IdNumber = this.IdNumber2;
                    customerIdentification2.Person = oldPartyRole.Party.Person;
                    //Context.PersonIdentifications.AddObject(customerIdentification2);
                    //Context.SaveChanges();
                }
            }
            else if (ids.Count() == 0)
            {
                InsertPersonIdentification(oldPartyRole, today);
            }
        }

        private void UpdateEmploymentInformation(DateTime today)
        {
            PartyRole oldPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.CustomerId &&
                entity.EndDate == null && entity.PartyRoleType.RoleType.Id == RoleType.CustomerType.Id);
            Party party = oldPartyRole.Party;

            PartyRole employeePartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.EndDate == null &&
                entity.PartyRoleType.RoleType.Id == RoleType.EmployeeType.Id && entity.PartyId == party.Id);

            PartyRelationship employmentPartyRel = new PartyRelationship();
            if (this.EmployerId != -1)
                employmentPartyRel = employeePartyRole.CurrentEmploymentRelationship;


            if (employmentPartyRel != null && employmentPartyRel.PartyRelType != null)
            {
                Employment employmentCustomer = new Employment();

                if (this.NewEmployerId != -1 && employmentPartyRel.SecondPartyRoleId != this.NewEmployerId)
                {
                    employmentPartyRel.EndDate = today;

                    this.EmployerId = employmentPartyRel.SecondPartyRoleId;
                    PartyRole employerPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.NewEmployerId
                                            && entity.EndDate == null && (entity.PartyRoleType.RoleTypeId == RoleType.EmployerType.Id
                                            || entity.PartyRoleType.RoleTypeId == RoleType.LendingInstitutionType.Id));
                    Party employerParty = employerPartyRole.Party;

                    PartyRelationship newEmploymentPartyRel = new PartyRelationship();
                    newEmploymentPartyRel.PartyRole = employeePartyRole;
                    newEmploymentPartyRel.SecondPartyRoleId = this.NewEmployerId;
                    newEmploymentPartyRel.PartyRelType = PartyRelType.EmploymentType;
                    newEmploymentPartyRel.EffectiveDate = today;

                    Employment newEmployment = new Employment();
                    newEmployment.PartyRelationship = newEmploymentPartyRel;
                    newEmployment.Salary = this.Salary;
                    newEmployment.EmploymentStatus = this.EmploymentStatus;

                    Context.PartyRelationships.AddObject(newEmploymentPartyRel);
                }
                else
                {
                    employmentCustomer = employmentPartyRel.Employment;
                    employmentCustomer.Salary = this.Salary;
                    employmentCustomer.EmploymentStatus = this.EmploymentStatus;
                }

                Employee employee = employeePartyRole.Employee;
                employee.Position = this.EmployeePosition;
                if (this.EmployeeIdNumber != null)
                    employee.EmployeeIdNumber = this.EmployeeIdNumber;
                if (this.SssNumber != null)
                    employee.SssNumber = this.SssNumber;
                if (this.GsisNumber != null)
                    employee.GsisNumber = this.GsisNumber;
                if (this.OwaNumber != null)
                    employee.OwaNumber = this.OwaNumber;
                if (this.PhicNumber != null)
                    employee.PhicNumber = this.PhicNumber;

                
            }
            else if(this.NewEmployerId != -1)
            {
                InsertEmploymentInformation(oldPartyRole, today);
            }
        }

        private void UpdateSpouseInformation(DateTime today)
        {
            PartyRole oldPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.CustomerId &&
                entity.EndDate == null && entity.PartyRoleType.RoleType.Id == RoleType.CustomerType.Id);
            Party customerParty = oldPartyRole.Party;
            PartyRole spouseRole = Context.PartyRoles.FirstOrDefault(entity => entity.PartyId == customerParty.Id
                                                                        && entity.RoleTypeId == RoleType.SpouseType.Id
                                                                        && entity.EndDate == null);

            if (spouseRole != null)
            {
                PartyRelationship spousePartyRel = spouseRole.CurrentSpousalRelationship;

                if (spousePartyRel != null)
                {
                    PartyRole spousePartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == spousePartyRel.SecondPartyRoleId
                        && entity.EndDate == null && entity.PartyRoleType.RoleTypeId == RoleType.SpouseType.Id);
                    Party spouseParty = spousePartyRole.Party;
                    Person spouse = spouseParty.Person;

                    if (this.SpouseBirthDate != null)
                        spouse.Birthdate = this.SpouseBirthDate;

                    //Update Spouse name
                    Person.CreatePersonName(spouse, today, this.SpouseName.Title,
                                            this.SpouseName.FirstName, this.SpouseName.MiddleName,
                                            this.SpouseName.LastName, this.SpouseName.NameSuffix,
                                            this.SpouseName.NickName);

                    Person.CreateOrUpdatePersonNames(spouse, PersonNameType.MothersMaidenNameType, this.SpouseName.MothersMaidenName, today);
                }
                else
                {
                    InsertSpouseInformation(oldPartyRole, today);
                }
            }
            else if (spouseRole == null && !string.IsNullOrWhiteSpace(this.SpouseName.FirstName))
            {
                InsertSpouseInformation(oldPartyRole, today);
            }
            
        }

        public override void Retrieve(int id)
        {
            this.CustomerId = id;
            PartyRole partyRole = PartyRole.GetById(id);
            var customer = partyRole.Customer;
            if(customer != null)
                RetrieveCustomerSourcesOfIncome(customer);
            RetrieveBasicInformation(partyRole);
            RetrieveContactInformation(partyRole);
            RetrieveEmploymentInformation(partyRole);
            RetrieveIdentifications(partyRole);
            RetrieveSpouseInformation(partyRole);
        }

        public override void PrepareForSave()
        {
            var today = DateTime.Now;

            if (this.CustomerId == -1)
            {
                var partyRole = InsertBasicInformation(today);
                this.CustomerId = partyRole.Id;
                this.IsNew = true;
                Customer customer = Context.Customers.FirstOrDefault(entity => entity.PartyRoleId == partyRole.Id);
                InsertContactInformation(partyRole, today);
                InsertPersonIdentification(partyRole, today);
                InsertEmploymentInformation(partyRole, today);
                InsertSpouseInformation(partyRole, today);
                SaveCustomerSourcesOfIncome(customer, today);
                    
            }
            else if (this.CustomerId != -1 && this.IsNew != true)
            {
                this.IsNew = false;
                Customer customer = Context.Customers.FirstOrDefault(entity => entity.PartyRoleId == this.CustomerId);
                UpdateBasicInformation(today);
                UpdateContactInformation(today);
                UpdateEmploymentInformation(today);
                UpdateIdentifications(today);
                UpdateSpouseInformation(today);
                SaveCustomerSourcesOfIncome(customer, today);
            }
            else if (this.CustomerId != -1 && this.IsNew == true)
            {

            }

            //Context.SaveChanges();
        }
    }
}
