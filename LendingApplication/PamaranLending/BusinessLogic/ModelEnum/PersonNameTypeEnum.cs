using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PersonNameType
    {
        private const string Title = "Title";
        private const string FirstName = "First Name";
        private const string MiddleName = "Middle Name";
        private const string LastName = "Last Name";
        private const string NickName = "NIck Name";
        private const string NameSuffix = "Name Suffix";
        private const string MothersMaidenName = "Mother's Maiden Name";

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

        public static PersonNameType TitleType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == Title);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType FirstNameType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == FirstName);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType MiddleNameType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == MiddleName);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType LastNameType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == LastName);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType NickNameType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == NickName);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType NameSuffixType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == NameSuffix);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
        public static PersonNameType MothersMaidenNameType
        {
            get
            {
                var type = Context.PersonNameTypes.SingleOrDefault(entity => entity.Name == MothersMaidenName);
                InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(type);

                return type;
            }
        }
    }
}