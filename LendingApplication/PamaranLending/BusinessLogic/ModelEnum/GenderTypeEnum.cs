using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{

    public partial class GenderType
    {
        private const string Female = "Female";
        private const string Male = "Male";

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

        public static GenderType FemaleType
        {
            get
            {
                var type = Context.GenderTypes.SingleOrDefault(entity => entity.Name == Female);
                InitialDatabaseValueChecker.ThrowIfNull<GenderType>(type);

                return type;
            }
        }

        public static GenderType MaleType
        {
            get
            {
                var type = Context.GenderTypes.SingleOrDefault(entity => entity.Name == Male);
                InitialDatabaseValueChecker.ThrowIfNull<GenderType>(type);

                return type;
            }
        }
    }
}