using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Receivable
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
        public ReceivableStatu CurrentStatus
        {
            get
            {
                var status =  this.ReceivableStatus.FirstOrDefault(entity => entity.IsActive == true && entity.ReceivableId == this.Id && entity.Receivable.FinancialAccountId == this.FinancialAccountId);
                if (status != null) return status; 
                else return null;
            }
        }
        public static IQueryable<Receivable> GetInterestOutstandingReceivables(int loanId)
        {
            var receivables = from r in Context.Receivables.Where(entity => entity.FinancialAccountId == loanId && entity.Balance > 0)
                              join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                              where rs.IsActive == true && (rs.StatusTypeId == ReceivableStatusType.OpenType.Id 
                              || rs.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id)
                              select r;
            return receivables;
        }
        public static void InsertReceivableStatus(int receivableID, decimal balance, DateTime today)
        {
            var receivable = Context.Receivables.FirstOrDefault(entity => entity.Id == receivableID);
            if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.OpenType.Id && balance != 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.PartiallyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.OpenType.Id && balance == 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.FullyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id && balance != 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.PartiallyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
            else if (receivable.CurrentStatus.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id && balance == 0)
            {
                receivable.CurrentStatus.IsActive = false;
                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.FullyPaidType;
                receivableStatus.TransitionDateTime = today;
                receivableStatus.IsActive = true;
                Context.ReceivableStatus.AddObject(receivableStatus);
            }
          //  Context.SaveChanges();
        }
    }
}