using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class LoanApplicationStatu
    {
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
        
        public static LoanApplicationStatu GetActive(LoanApplication loanApplication)
        {
            return loanApplication.LoanApplicationStatus.FirstOrDefault(entity => entity.IsActive);
        }

        public static LoanApplicationStatu GetStatus(LoanApplication loanApplication, LoanApplicationStatusType type)
        {
            return loanApplication.LoanApplicationStatus.FirstOrDefault(entity => entity.StatusTypeId == type.Id);
        }

        public static bool CanChangeStatusTo(LoanApplication loanApplication, LoanApplicationStatusType statusTo)
        {
            LoanApplicationStatu status = GetActive(loanApplication);

            if (status == null)
                return true;

            return CanChangeStatusTo(status.LoanApplicationStatusType, statusTo);
        }

        public static bool CanChangeStatusTo(LoanApplicationStatusType statusFrom, LoanApplicationStatusType statusTo)
        {
            if (statusTo == LoanApplicationStatusType.CancelledType)
            {
                return (statusFrom.Id == LoanApplicationStatusType.PendingApprovalType.Id
                    || statusFrom.Id == LoanApplicationStatusType.ApprovedType.Id
                    || statusFrom.Id == LoanApplicationStatusType.PendingInFundingType.Id);
            }
            else if(statusTo == LoanApplicationStatusType.RejectedType 
                || statusTo == LoanApplicationStatusType.ApprovedType)
            {
                return statusFrom.Id == LoanApplicationStatusType.PendingApprovalType.Id;
            }
            else if (statusTo == LoanApplicationStatusType.ClosedType)
            {
                return (statusFrom.Id == LoanApplicationStatusType.PendingApprovalType.Id
                    || statusFrom.Id == LoanApplicationStatusType.PendingInFundingType.Id);
            }
            else if (statusTo == LoanApplicationStatusType.PendingInFundingType)
            {
                return (statusFrom.Id == LoanApplicationStatusType.PendingApprovalType.Id);
            }
            return false;
        }

        public static LoanApplicationStatu ChangeStatus(LoanApplication loanApplication, LoanApplicationStatusType statusTo, DateTime today)
        {
            LoanApplicationStatu status = GetActive(loanApplication);
            if (CanChangeStatusTo(status.LoanApplicationStatusType, statusTo))
            {
                return CreateOrUpdateCurrent(status, statusTo, today);
            }
            return status;
        }

        private static LoanApplicationStatu CreateOrUpdateCurrent(LoanApplicationStatu current, LoanApplicationStatusType statusType, DateTime today)
        {
            if (current != null && current.LoanApplicationStatusType.Id != statusType.Id)
                current.IsActive = false;

            if (current == null || current.LoanApplicationStatusType.Id != statusType.Id)
            {
                LoanApplicationStatu loanStatus = new LoanApplicationStatu();
                loanStatus.LoanApplication = current.LoanApplication;
                loanStatus.LoanApplicationStatusType = statusType;
                loanStatus.Remarks = null;
                loanStatus.TransitionDateTime = today;
                loanStatus.IsActive = true;

                Context.LoanApplicationStatus.AddObject(loanStatus);
                return loanStatus;
            }
            return current;
        }

        public static LoanApplicationStatu CreateOrUpdateCurrent(LoanApplication loanApplication, LoanApplicationStatusType statusType, DateTime today)
        {
            LoanApplicationStatu loanAppStatus = GetActive(loanApplication);
            if (loanAppStatus != null && loanAppStatus.LoanApplicationStatusType.Id != statusType.Id)
                loanAppStatus.IsActive = false;

            if (loanAppStatus == null || loanAppStatus.LoanApplicationStatusType.Id != statusType.Id)
            {
                LoanApplicationStatu loanStatus = new LoanApplicationStatu();
                loanStatus.LoanApplication = loanApplication;
                loanStatus.LoanApplicationStatusType = statusType;
                loanStatus.Remarks = null;
                loanStatus.TransitionDateTime = today;
                loanStatus.IsActive = true;

                Context.LoanApplicationStatus.AddObject(loanStatus);
                Context.SaveChanges();
                return loanStatus;
            }
            return loanAppStatus;
        }
    }
}
