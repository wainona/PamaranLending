using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FinlAcctTransType
    {
        private const string AccountPayment = "Account Payment";
        private const string AccountFee = "Account Fee";
        private const string Deposit = "Deposit";
        private const string Withdrawal = "Withdrawal";
        private const string FinancialAccountAdjustment = "Financial Account Adjustment";

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

        public static FinlAcctTransType AccountPaymentType
        {
            get
            {
                var type = Context.FinlAcctTransTypes.SingleOrDefault(entity => entity.Name == AccountPayment);
                InitialDatabaseValueChecker.ThrowIfNull<FinlAcctTransType>(type);

                return type;
            }
        }

        public static FinlAcctTransType AccountFeeType
        {
            get
            {
                var type = Context.FinlAcctTransTypes.SingleOrDefault(entity => entity.Name == AccountFee);
                InitialDatabaseValueChecker.ThrowIfNull<FinlAcctTransType>(type);

                return type;
            }
        }

        public static FinlAcctTransType DepositType
        {
            get
            {
                var type = Context.FinlAcctTransTypes.SingleOrDefault(entity => entity.Name == Deposit);
                InitialDatabaseValueChecker.ThrowIfNull<FinlAcctTransType>(type);

                return type;
            }
        }

        public static FinlAcctTransType WithdrawalType
        {
            get
            {
                var type = Context.FinlAcctTransTypes.SingleOrDefault(entity => entity.Name == Withdrawal);
                InitialDatabaseValueChecker.ThrowIfNull<FinlAcctTransType>(type);

                return type;
            }
        }

        public static FinlAcctTransType FinancialAccountAdjustmentType
        {
            get
            {
                var type = Context.FinlAcctTransTypes.SingleOrDefault(entity => entity.Name == FinancialAccountAdjustment);
                InitialDatabaseValueChecker.ThrowIfNull<FinlAcctTransType>(type);

                return type;
            }
        }

    }
}