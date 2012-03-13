using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementVcrStatusEnum
    {
        private const string Pending = "Pending";
        private const string Approved = "Approved";
        private const string Rejected = "Rejected";
        private const string Cancelled = "Cancelled";
        private const string PartiallyDisbursed = "Partially Disbursed";
        private const string FullyDisbursed = "Fully Disbursed";

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

        public static DisbursementVcrStatusType PendingType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == Pending);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }

        public static DisbursementVcrStatusType ApprovedType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == Approved);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }

        public static DisbursementVcrStatusType RejectedType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == Rejected);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }

        public static DisbursementVcrStatusType CancelledType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == Cancelled);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }

        public static DisbursementVcrStatusType PartiallyDisbursedType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == PartiallyDisbursed);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }

        public static DisbursementVcrStatusType FullyDisbursedType
        {
            get
            {
                var type = Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == FullyDisbursed);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementVcrStatusType>(type);

                return type;
            }
        }
    }
}