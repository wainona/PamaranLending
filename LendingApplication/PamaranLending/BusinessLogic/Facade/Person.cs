using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Person
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

        private int CalculateAge()
        {
            int now = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            if (this.Birthdate.HasValue)
            {
                int dob = int.Parse(this.Birthdate.Value.ToString("yyyyMMdd"));
                string dif = (now - dob).ToString();
                string age = dif.Substring(0, dif.Length - 4);
                return int.Parse(age);
            }
            return 1;
        }

        public int Age
        {
            get
            {
                return CalculateAge();
            }
        }

        public PersonName Title
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.TitleType);
            }
        }

        public PersonName LastName
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.LastNameType);
            }
        }

        public PersonName FirstName
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.FirstNameType);
            }
        }

        public PersonName MiddleName
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.MiddleNameType);
            }
        }

        public PersonName NameSuffix
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.NameSuffixType);
            }
        }

        public PersonName NickName
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.NickNameType);
            }
        }

        public PersonName MothersMaidenName
        {
            get
            {
                return Person.GetPersonNameByTypeEntity(this, PersonNameType.MothersMaidenNameType);
            }
        }

        public string TitleString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.TitleType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string LastNameString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.LastNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string FirstNameString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.FirstNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string MiddleNameString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.MiddleNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string MiddleInitialString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.MiddleNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Substring(0,1) + ".";
            }
        }

        public string NameSuffixString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.NameSuffixType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string NickNameString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.NickNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public string MothersMaidenNameString
        {
            get
            {
                PersonName name = Person.GetPersonNameByTypeEntity(this, PersonNameType.MothersMaidenNameType);
                if (name == null)
                    return string.Empty;
                else
                    return name.Name.Trim();
            }
        }

        public static void CreatePersonName(Person Person, DateTime Today, string title, string firstName, string middleName, string lastName, string suffix, string nickName)
        {
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.TitleType, title, Today);
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.FirstNameType, firstName, Today);
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.MiddleNameType, middleName, Today);
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.LastNameType, lastName, Today);
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.NameSuffixType, suffix, Today);
            Person.CreateOrUpdatePersonNames(Person, PersonNameType.NickNameType, nickName, Today);
        }

        private static PersonName CreatePersonName(Person person, PersonNameType nameType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            PersonName personName = new PersonName();
            personName.Name = name;
            personName.PersonNameType = nameType;
            personName.Person = person;
            personName.EffectiveDate = DateTime.Now;

            Context.PersonNames.AddObject(personName);

            return personName;
        }

        private static PersonName CreatePersonName(Person person, PersonNameType nameType, string name, DateTime today)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            PersonName personName = new PersonName();
            personName.Name = name;
            personName.PersonNameType = nameType;
            personName.Person = person;
            personName.EffectiveDate = today;

            Context.PersonNames.AddObject(personName);

            return personName;
        }

        private static PersonName GetPersonNameByTypeEntity(Person person, PersonNameType type)
        {
            var personName = person.PersonNames.SingleOrDefault(entity => entity.PersonNameTypeId == type.Id && entity.EndDate == null);
            return personName;
        }

        public static void CreateOrUpdatePersonNames(Person person, PersonNameType type, string name, DateTime today)
        {
            PersonName personName = GetPersonNameByTypeEntity(person, type);

            if (personName == null || name != personName.Name)
            {
                if (personName != null)
                    personName.EndDate = today;
                CreatePersonName(person, type, name, today);
            }
        }

        public HomeOwnership CurrentHomeOwnership
        {
            get
            {
                HomeOwnership homeOwnership = this.HomeOwnerships.SingleOrDefault(entity => entity.PartyId == this.PartyId && entity.EndDate == null);
                if (homeOwnership != null)
                    return homeOwnership;
                return null;
            }
        }

        public IEnumerable<PersonIdentification> PersonIdentificationsList
        {
            get
            {
                var personIds = this.PersonIdentifications.Where(entity => entity.PartyId == this.PartyId).ToList();
                if (personIds != null)
                    return personIds;
                return null;
            }
        }

        public static PartyRole CheckPersonName(Party party)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();

            var people = Context.People.ToList();
            bool equal = true;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();

            partyRel = Context.PartyRelationships.Where(entity =>
                entity.EndDate == null).ToList();

            if (partyRel != null)
            {
                foreach (var relation in partyRel)
                {
                    Person person = relation.PartyRole.Party.Person;

                    if (person != null)
                        equal = ValidatePersonNameIfEqual(person, party.Person);

                    if (equal)
                    {
                        equalParty = relation.PartyRole.Party;
                        equalPartyRole = relation.PartyRole;
                        break;
                    }
                }
            }
            if (equalParty.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckPersonNameParty(Party party, Party employeeParty)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();

            var people = Context.People.ToList();
            bool equal = true;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();

            partyRel = Context.PartyRelationships.Where(entity => 
                           (entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.ContactType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.EmployeeType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.SpouseType.Id 
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CoBorrowerAgreementType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CoBorrowerApplicationType.Id 
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.GuarantorAgreementType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.GuarantorApplicationType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.GuarantorFinancialType.Id)
                           && entity.PartyRole.Party.PartyTypeId == PartyType.PersonType.Id
                           && entity.EndDate == null
                           && entity.PartyRole.EndDate == null 
                           && entity.PartyRole.PartyId != employeeParty.Id
                           && entity.PartyRole.Party.PartyRoles.Where(c => c.RoleTypeId == RoleType.CustomerType.Id 
                                        && entity.EndDate == null).Count() == 0).ToList();

            if (partyRel != null)
            {
                foreach (var relation in partyRel)
                {
                    Person person = relation.PartyRole.Party.Person;
                    if(person != null)
                        equal = ValidatePersonNameIfEqual(person, party.Person);

                    if (equal)
                    {
                        equalPartyRole = relation.PartyRole;

                        break;
                    }
                }
            }

            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckCustomerPersonName(Party party)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();

            var people = Context.People.ToList();
            bool equal = true;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();

            partyRel = Context.PartyRelationships.Where(entity => entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id 
                                                        && entity.PartyRole.Party.PartyTypeId == PartyType.PersonType.Id
                                                        && entity.EndDate == null
                                                        && entity.PartyRole.EndDate == null).ToList();

            if (partyRel != null)
            {
                foreach (var relation in partyRel)
                {
                    Person person = relation.PartyRole.Party.Person;
                    if(person != null)
                        equal = ValidatePersonNameIfEqual(person, party.Person);

                    if (equal)
                    {
                        equalParty = relation.PartyRole.Party;
                        equalPartyRole = relation.PartyRole;
                        break;
                    }
                }
            }

            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckEmployerPersonName(PartyRole partyRole)
        {
            List<PartyRole> partyRel = new List<PartyRole>();

            var people = Context.People.ToList();
            bool equal = true;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();
            var party = partyRole.Party;

            if (partyRole.Party.PartyType == PartyType.PersonType)
            {
                partyRel = Context.PartyRoles.Where(entity => entity.PartyRoleType.RoleTypeId == RoleType.EmployerType.Id
                                                            && entity.Party.PartyTypeId == PartyType.PersonType.Id
                                                            && entity.EndDate == null).ToList();

                if (partyRel != null)
                {
                    foreach (var relation in partyRel)
                    {
                        Person person = relation.Party.Person;
                        if (person != null)
                            equal = ValidatePersonNameIfEqual(person, party.Person);

                        if (equal)
                        {
                            equalParty = relation.Party;
                            equalPartyRole = relation;
                            break;
                        }
                    }
                }
            }
            else
            {
                partyRel = Context.PartyRoles.Where(entity => entity.PartyRoleType.RoleTypeId == RoleType.EmployerType.Id
                                                            && entity.Party.PartyTypeId == PartyType.OrganizationType.Id
                                                            && entity.EndDate == null).ToList();

                if (partyRel != null)
                {
                    foreach (var relation in partyRel)
                    {
                        Organization organization = relation.Party.Organization;
                        if (organization == null)
                        {
                            organization = relation.Party.Organization;
                        }
                        equal &= organization.OrganizationName == party.Organization.OrganizationName;

                        if (equal)
                        {
                            equalParty = organization.Party;
                            equalPartyRole = relation;
                            break;
                        }
                    }
                }
            }


            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckEmployerName(PartyRole partyRole)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();
            Party party = partyRole.Party;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();
            bool equal = true;

            if (partyRole.Party.PartyType == PartyType.PersonType)
            {
                partyRel = Context.PartyRelationships.Where(entity => (entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.ContactType.Id 
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id 
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.EmployeeType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.SpouseType.Id) 
                           && entity.PartyRole.Party.PartyTypeId == PartyType.PersonType.Id 
                           && entity.EndDate == null
                           && entity.PartyRole.EndDate == null).ToList();
            
                if (partyRel != null)
                {
                    foreach (var relation in partyRel)
                    {
                        Person person = relation.PartyRole.Party.Person;
                        if(person != null)
                            equal = ValidatePersonNameIfEqual(person, party.Person);

                        if (equal)
                        {
                            equalParty = person.Party;
                            equalPartyRole = relation.PartyRole;
                            break;
                        }
                    }
                }
            }
            else
            {
                partyRel = Context.PartyRelationships.Where(entity => (entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.ContactType.Id 
                            || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id 
                            || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.BankType.Id
                            || entity.PartyRole1.PartyRoleType.RoleTypeId == RoleType.LendingInstitutionType.Id 
                            || entity.PartyRole1.PartyRoleType.RoleTypeId == RoleType.BankType.Id) 
                            && entity.EndDate == null
                            && entity.PartyRole1.EndDate == null).ToList();

                if (partyRel != null)
                {
                    foreach (var relation in partyRel)
                    {
                        Organization organization = relation.PartyRole1.Party.Organization;
                        if (organization == null)
                        {
                            organization = relation.PartyRole1.Party.Organization;
                        }
                        equal &= organization.OrganizationName == party.Organization.OrganizationName;

                        if (equal)
                        {
                            equalParty = organization.Party;
                            equalPartyRole = relation.PartyRole1;
                            break;
                        }
                    }
                }
            }

            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckEmployeePersonName(PartyRole partyRole)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();

            var lenderPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.EndDate == null && entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var people = Context.People.ToList();
            bool equal = true;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();
            var party = partyRole.Party;

            partyRel = Context.PartyRelationships.Where(entity => entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.EmployeeType.Id
                                                        && entity.PartyRole.Party.PartyTypeId == PartyType.PersonType.Id
                                                        && entity.SecondPartyRoleId == lenderPartyRole.Id
                                                        && entity.EndDate == null
                                                        && entity.PartyRole.EndDate == null).ToList();

            if (partyRel != null)
            {
                foreach (var relation in partyRel)
                {
                    Person person = relation.PartyRole.Party.Person;
                    if (person != null)
                        equal = ValidatePersonNameIfEqual(person, party.Person);

                    if (equal)
                    {
                        equalParty = relation.PartyRole.Party;
                        equalPartyRole = relation.PartyRole;
                        break;
                    }
                }
            }


            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static PartyRole CheckAllowedEmployeeName(PartyRole partyRole)
        {
            List<PartyRelationship> partyRel = new List<PartyRelationship>();
            Party party = partyRole.Party;
            Party equalParty = new Party();
            PartyRole equalPartyRole = new PartyRole();
            bool equal = true;

            if (partyRole.Party.PartyType == PartyType.PersonType)
            {
                partyRel = Context.PartyRelationships.Where(entity => (entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.ContactType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id
                           || entity.PartyRole.PartyRoleType.RoleTypeId == RoleType.EmployerType.Id)
                           && entity.PartyRole.Party.PartyTypeId == PartyType.PersonType.Id
                           && entity.EndDate == null
                           && entity.PartyRole.EndDate == null).ToList();

                if (partyRel != null)
                {
                    foreach (var relation in partyRel)
                    {
                        Person person = relation.PartyRole.Party.Person;
                        if (person != null)
                            equal = ValidatePersonNameIfEqual(person, party.Person);

                        if (equal)
                        {
                            equalParty = person.Party;
                            equalPartyRole = relation.PartyRole;
                            break;
                        }
                    }
                }
            }

            if (equalPartyRole.Id != 0)
                return equalPartyRole;
            return null;
        }

        public static bool ValidatePersonNameIfEqual(Person person1, Person person2)
        {
            bool isEqual = true;

            isEqual &= person1.TitleString.ToLower() == person2.TitleString.ToLower();
            isEqual &= person1.FirstNameString.ToLower() == person2.FirstNameString.ToLower();
            isEqual &= person1.LastNameString.ToLower() == person2.LastNameString.ToLower();
            isEqual &= person1.MiddleNameString.ToLower() == person2.MiddleNameString.ToLower();
            isEqual &= person1.NameSuffixString.ToLower() == person2.NameSuffixString.ToLower();
            isEqual &= person1.NickNameString.ToLower() == person2.NickNameString.ToLower();
            //isEqual &= person1.MothersMaidenNameString.ToLower() == person2.MothersMaidenNameString.ToLower();

            return isEqual;
        }

        public static string GetPersonFullName(Party party)
        {
            Person person = party.Person;
            string title = person.TitleString;
            string firstName = person.FirstNameString;
            string lastName = person.LastNameString;
            string middleName = person.MiddleNameString;
            string suffix = person.NameSuffixString;

            string middleInitial = string.IsNullOrWhiteSpace(middleName) ? "" : middleName[0] + ".";
            string fullName = StringConcatUtility.Build(" ", title,
                                                                  lastName + ",",
                                                                  firstName,
                                                                  middleInitial,
                                                                  suffix);
            return fullName;
        }

        public static string GetPersonFullNameV2(Party party)
        {
            Person person = party.Person;
            string title = person.TitleString;
            string firstName = person.FirstNameString;
            string lastName = person.LastNameString;
            string middleName = person.MiddleNameString;
            string suffix = person.NameSuffixString;

            string middleInitial = string.IsNullOrWhiteSpace(middleName) ? "" : middleName[0] + ".";
            string fullName = StringConcatUtility.Build(" ",      lastName + ",",
                                                                  firstName,
                                                                  middleInitial,
                                                                  suffix);
            return fullName;
        }

        public static string GetPersonFullName(int partyRoleId)
        {
            string name = string.Empty;
            var party = Context.PartyRoles.FirstOrDefault(entity => entity.Id == partyRoleId).Party;

            if (party != null)
                name = GetPersonFullName(party);

            return name;
        }
     

        public static string GetPersonFullName(Person person)
        {
            string title = person.TitleString;
            string firstName = person.FirstNameString;
            string lastName = person.LastNameString;
            string middleName = person.MiddleNameString;
            string suffix = person.NameSuffixString;

            string middleInitial = string.IsNullOrWhiteSpace(middleName) ? "" : middleName[0] + ".";
            string fullName = StringConcatUtility.Build(" ", title,
                                                                  lastName + ",",
                                                                  firstName,
                                                                  middleInitial,
                                                                  suffix);
            return fullName;
        }
    }
}
