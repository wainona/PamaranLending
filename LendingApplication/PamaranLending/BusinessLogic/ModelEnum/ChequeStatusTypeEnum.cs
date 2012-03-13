using System.Linq;using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ChequeStatusType
    {
        private const string Received = "Received";
        private const string Deposited = "Deposited";
        private const string Cleared = "Cleared";
        private const string Bounced = "Bounced";
        private const string Cancelled = "Cancelled";
        private const string OnHold = "On Hold";

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

        public static ChequeStatusType ReceivedType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == Received);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

        public static ChequeStatusType DepositedType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == Deposited);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

        public static ChequeStatusType ClearedType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == Cleared);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

        public static ChequeStatusType BouncedType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == Bounced);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

        public static ChequeStatusType CancelledType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == Cancelled);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

        public static ChequeStatusType OnHoldType
        {
            get
            {
                var type = Context.ChequeStatusTypes.SingleOrDefault(entity => entity.Name == OnHold);
                InitialDatabaseValueChecker.ThrowIfNull<ChequeStatusType>(type);

                return type;
            }
        }

    }
}