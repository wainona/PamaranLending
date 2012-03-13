using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LendingApplication.Applications;
using BusinessLogic;

namespace LendingApplication.Applications
{
    public class PersonNameExtension
    {
        public static PersonName CreatePersonName(FinancialEntities context, string nameType, string name, Person person)
        {
            var personNameType = context.PersonNameTypes.SingleOrDefault(entity => entity.Name == nameType);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(personNameType);

            PersonName personName = new PersonName();
            personName.Name = name;
            personName.PersonNameType = personNameType;
            personName.Person = person;
            personName.EffectiveDate = DateTime.Now;
            return personName;
        }

        public static PersonName GetPersonNameByTypeEntity(FinancialEntities context, Person person, string type)
        {
            var personNameType = context.PersonNameTypes.SingleOrDefault(entity => entity.Name == type);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(personNameType);

            var personName = person.PersonNames.SingleOrDefault(entity => entity.PersonNameTypeId == personNameType.Id && entity.EndDate == null);
            return personName;
        }

        public static void CreateOrUpdatePersonNames(FinancialEntities context, Person person, string type, string name, DateTime today)
        {
            PersonName personName = GetPersonNameByTypeEntity(context, person, type);

            if (personName == null || name != personName.Name)
            {
                if (personName != null)
                    personName.EndDate = today;
                PersonName newPersonName = CreatePersonName(context, type, name, person);
                context.PersonNames.AddObject(newPersonName);
            }
        }
    }
}