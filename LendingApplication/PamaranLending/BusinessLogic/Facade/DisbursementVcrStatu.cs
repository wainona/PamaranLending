using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementVcrStatu
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

        public static DisbursementVcrStatu GetById(int id)
        {
            return Context.DisbursementVcrStatus.SingleOrDefault(entity => entity.Id == id);
        }
        
        public static DisbursementVcrStatu GetActive(LoanDisbursementVcr loanDisbursementVoucher)
        {
            var result = loanDisbursementVoucher.DisbursementVcrStatus.Where(entity => entity.IsActive);
            if (result.Count() > 0)
                return result.First();
            else
                return null;
        }

        public static DisbursementVcrStatu RetrieveStatus(LoanDisbursementVcr loanDisbursementVoucher, DisbursementVcrStatusType statusType)
        {
            return Context.DisbursementVcrStatus.SingleOrDefault(entity => entity.LoanDisbursementVoucherId == loanDisbursementVoucher.Id
                && entity.TransitionDateTime == GetActive(loanDisbursementVoucher).TransitionDateTime && entity.DisbursementVoucherStatTypId == statusType.Id);
        }

        public static DisbursementVcrStatu ChangeStatus(LoanDisbursementVcr loanDisbursementVoucher, DisbursementVcrStatusType statusType, DateTime today)
        {
            DisbursementVcrStatu disbursementStatus = GetActive(loanDisbursementVoucher);
            if (CanChangeStatus(disbursementStatus.DisbursementVcrStatusType, statusType, today))
                return CreateOrUpdateCurrent(loanDisbursementVoucher, statusType, today);
            return disbursementStatus;
        }

        public static bool CanChangeStatus(DisbursementVcrStatusType statusFrom, DisbursementVcrStatusType statusTo, DateTime today)
        {
            //check if change status is allowed through the DisbursementVcrStatusTypeAssoc
            var checkStatus = Context.DisbursementVcrStatTypeAssocs.SingleOrDefault(entity =>
                entity.FromStatusTypeId == statusFrom.Id && entity.ToStatusTypeId == statusTo.Id &&
                entity.EndDate == null);
            if (checkStatus != null)
                return true;
            else
                return false;

        }

        public static DisbursementVcrStatu CreateOrUpdateCurrent(LoanDisbursementVcr loanDisbursementVoucher, DisbursementVcrStatusType statusType, DateTime today)
        {
            DisbursementVcrStatu disbursementStatus = GetActive(loanDisbursementVoucher);
            if (disbursementStatus != null && disbursementStatus.DisbursementVcrStatusType.Id != statusType.Id)
                disbursementStatus.IsActive = false;

            if (disbursementStatus == null || disbursementStatus.DisbursementVcrStatusType.Id != statusType.Id)
            {
                DisbursementVcrStatu newDisbursementStatus = new DisbursementVcrStatu();
                newDisbursementStatus.DisbursementVoucherStatTypId = statusType.Id;
                newDisbursementStatus.LoanDisbursementVcr = loanDisbursementVoucher;
                newDisbursementStatus.Remarks = null;
                newDisbursementStatus.TransitionDateTime = today;
                newDisbursementStatus.IsActive = true;

                Context.DisbursementVcrStatus.AddObject(newDisbursementStatus);

                return newDisbursementStatus;
            }
            return disbursementStatus;
        }

        public static DisbursementVcrStatu Create(LoanDisbursementVcr loanDisbursementVoucher, DisbursementVcrStatusType statusType, DateTime today)
        {
            DisbursementVcrStatu status = new DisbursementVcrStatu();
            status.DisbursementVoucherStatTypId = statusType.Id;
            status.LoanDisbursementVcr = loanDisbursementVoucher;
            status.Remarks = null;
            status.TransitionDateTime = today;
            status.IsActive = true;

            Context.DisbursementVcrStatus.AddObject(status);
            return status;
        }
    }
}

