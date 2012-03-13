using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FinancialAccountType
    {
        private const string LoanAccount = "Loan Account";
        private const string DepositAccount = "Deposit Account";

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

        public static FinancialAccountType LoanAccountType
        {
            get
            {
                var type = Context.FinancialAccountTypes.SingleOrDefault(entity => entity.Name == LoanAccount);
                InitialDatabaseValueChecker.ThrowIfNull<FinancialAccountType>(type);

                return type;
            }
        }

        public static FinancialAccountType DepositAccountType
        {
            get
            {
                var type = Context.FinancialAccountTypes.SingleOrDefault(entity => entity.Name == DepositAccount);
                InitialDatabaseValueChecker.ThrowIfNull<FinancialAccountType>(type);

                return type;
            }
        }


    }
}