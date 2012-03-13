using System.Linq;
using FirstPacific.UIFramework;


namespace BusinessLogic
{
    public partial class LoanApplicationStatusType
    {
        private const string Approved = "Approved";
        private const string Closed = "Closed";
        private const string Cancelled = "Cancelled";
        private const string Rejected = "Rejected";
        private const string PendingApproval = "Pending: Approval";
        private const string PendingEndorsement = "Pending: Endorsement";
        private const string PendingInFunding = "Pending: In Funding";
        private const string PendingCreditEvaluation = "Pending: Credit Evaluation";
        private const string PendingCreditInvestigation = "Pending: Credit Investigation";
        private const string PendingAppraisal = "Pending: Appraisal";
        private const string Restructured = "Restructured";

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

        public static LoanApplicationStatusType ApprovedType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == Approved);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType ClosedType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == Closed);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType CancelledType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == Cancelled);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType RejectedType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == Rejected);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType PendingEndorsementType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == PendingEndorsement);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType PendingApprovalType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == PendingApproval);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType IncompleteType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == PendingEndorsement);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }
        
        public static LoanApplicationStatusType PendingInFundingType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == PendingInFunding);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }

        public static LoanApplicationStatusType RestructuredType
        {
            get
            {
                var type = Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == Restructured);
                InitialDatabaseValueChecker.ThrowIfNull<LoanApplicationStatusType>(type);

                return type;
            }
        }
    }
}