using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class BankAccountType
    {
        private const string Savings = "Savings";
        private const string Current = "Current";
        private const string TimeDeposit = "Time Deposit";

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

        public static BankAccountType SavingsType
        {
            get
            {
                var type = Context.BankAccountTypes.SingleOrDefault(entity => entity.Name == Savings);
                InitialDatabaseValueChecker.ThrowIfNull<BankAccountType>(type);

                return type;
            }
        }

        public static BankAccountType CurrentType
        {
            get
            {
                var type = Context.BankAccountTypes.SingleOrDefault(entity => entity.Name == Current);
                InitialDatabaseValueChecker.ThrowIfNull<BankAccountType>(type);

                return type;
            }
        }

        public static BankAccountType TimeDepositType
        {
            get
            {
                var type = Context.BankAccountTypes.SingleOrDefault(entity => entity.Name == TimeDeposit);
                InitialDatabaseValueChecker.ThrowIfNull<BankAccountType>(type);

                return type;
            }
        }

        public static IQueryable<BankAccountType> All()
        {
            return Context.BankAccountTypes;
        }

    }
}