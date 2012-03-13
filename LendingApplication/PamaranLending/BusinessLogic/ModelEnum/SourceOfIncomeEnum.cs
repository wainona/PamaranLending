using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class SourceOfIncome
    {
        private const string Business = "Business";
        private const string Pension = "Pension";
        private const string Allowance = "Allowance";
        private const string Allotment = "Allotment";

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

        public static SourceOfIncome BusinessType
        {
            get
            {
                var type = Context.SourceOfIncomes.SingleOrDefault(entity => entity.Name == Business);
                InitialDatabaseValueChecker.ThrowIfNull<SourceOfIncome>(type);

                return type;
            }
        }

        public static SourceOfIncome PensionType
        {
            get
            {
                var type = Context.SourceOfIncomes.SingleOrDefault(entity => entity.Name == Pension);
                InitialDatabaseValueChecker.ThrowIfNull<SourceOfIncome>(type);

                return type;
            }
        }

        public static SourceOfIncome AllowanceType
        {
            get
            {
                var type = Context.SourceOfIncomes.SingleOrDefault(entity => entity.Name == Allowance);
                InitialDatabaseValueChecker.ThrowIfNull<SourceOfIncome>(type);

                return type;
            }
        }

        public static SourceOfIncome AllotmentType
        {
            get
            {
                var type = Context.SourceOfIncomes.SingleOrDefault(entity => entity.Name == Allotment);
                InitialDatabaseValueChecker.ThrowIfNull<SourceOfIncome>(type);

                return type;
            }
        }
    }
}