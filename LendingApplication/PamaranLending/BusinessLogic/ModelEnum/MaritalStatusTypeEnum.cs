using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class MaritalStatusType
    {
        private const string Single = "Single";
        private const string Married = "Married";
        private const string LegallySeparated = "Legally Separated";
        private const string Divorced = "Divorced";
        private const string Annulled = "Annulled";
        private const string Widowed = "Widowed";

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


        public static MaritalStatusType SingleType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == Single);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }

        public static MaritalStatusType MarriedType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == Married);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }

        public static MaritalStatusType LegallySeparatedType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == LegallySeparated);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }

        public static MaritalStatusType DivorcedType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == Divorced);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }

        public static MaritalStatusType AnnulledType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == Annulled);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }

        public static MaritalStatusType WidowedType
        {
            get
            {
                var type = Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Name == Widowed);
                InitialDatabaseValueChecker.ThrowIfNull<MaritalStatusType>(type);

                return type;
            }
        }
    }
}