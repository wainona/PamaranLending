using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class LoanModificationStatusType
    {
        private const string Approved = "Approved";
        private const string Rejected = "Rejected";
        private const string PendingApproval = "Pending: Approval";

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

        public static LoanModificationStatusType ApprovedType
        {
            get
            {
                var type = Context.LoanModificationStatusTypes.SingleOrDefault(entity => entity.Name == Approved);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationStatusType>(type);

                return type;
            }
        }

        public static LoanModificationStatusType RejectedType
        {
            get
            {
                var type = Context.LoanModificationStatusTypes.SingleOrDefault(entity => entity.Name == Rejected);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationStatusType>(type);

                return type;
            }
        }

        public static LoanModificationStatusType PendingApprovalType
        {
            get
            {
                var type = Context.LoanModificationStatusTypes.SingleOrDefault(entity => entity.Name == PendingApproval);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationStatusType>(type);

                return type;
            }
        }
    }
}
