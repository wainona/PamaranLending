using System.Linq;
using FirstPacific.UIFramework;


namespace BusinessLogic
{
    public partial class LoanAccountStatusType
    {
        private const string Current = "Current";
        private const string Delinquent = "Delinquent";
        private const string PaidOff = "Paid-Off";
        private const string WrittenOff = "Written-Off";
        private const string UnderLitigation = "Under Litigation";
        private const string Restructured = "Restructured";
        private const string PendingEndorsement = "Pending: Endorsement";

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

        public static LoanAccountStatusType CurrentType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == Current);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType DelinquentType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == Delinquent);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType PaidOffType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == PaidOff);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType WrittenOffType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == WrittenOff);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType UnderLitigationType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == UnderLitigation);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType RestructuredType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == Restructured);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }

        public static LoanAccountStatusType PendingEndorsementType
        {
            get
            {
                var type = Context.LoanAccountStatusTypes.SingleOrDefault(entity => entity.Name == PendingEndorsement);
                InitialDatabaseValueChecker.ThrowIfNull<LoanAccountStatusType>(type);

                return type;
            }
        }
    }
}