using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ChequeStatu
    {
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

        public static ChequeStatu CreateChequeStatus(Cheque cheque, int chequeStatusTypeId, DateTime transitionDateTime, string remarks, bool isActive)
        {
            ChequeStatu newCheckStatus = new ChequeStatu();

            newCheckStatus.Cheque = cheque;
            newCheckStatus.CheckStatusTypeId = chequeStatusTypeId;
            newCheckStatus.TransitionDateTime = transitionDateTime;
            newCheckStatus.Remarks = remarks;
            newCheckStatus.IsActive = isActive;

            return newCheckStatus;
        }

        public static ChequeStatu GetActive(Cheque cheque)
        {
            return cheque.ChequeStatus.FirstOrDefault(entity => entity.IsActive);
        }

        public static ChequeStatu ChangeStatus(Cheque cheque, ChequeStatusType chequeStatusTo, string remarks)
        {
            var statusFrom = GetActive(cheque);
            if (statusFrom != null && statusFrom.IsActive == true)
            {
                statusFrom.IsActive = false;
            }

            ChequeStatu status = new ChequeStatu();
            if (statusFrom == null || statusFrom.IsActive == false)
                status = CreateChequeStatus(cheque, chequeStatusTo.Id, DateTime.Now, remarks, true);

            Context.ChequeStatus.AddObject(status);
            return status;
        }
    }
}
