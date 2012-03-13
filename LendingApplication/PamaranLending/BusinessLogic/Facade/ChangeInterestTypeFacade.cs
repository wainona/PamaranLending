using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ChangeInterestTypeFacade
    {
        public static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }
        public static bool SaveInterestType(int loanId, InterestType type,decimal amount)
        {

            var loan = ObjectContext.LoanAccounts.FirstOrDefault(entity => entity.FinancialAccountId == loanId);
            if (loan.InterestTypeId == type.Id)
                return false;
            else
            {
                var olditem = ObjectContext.InterestItems.FirstOrDefault(entity => entity.LoanId == loanId && entity.IsActive == true);
                loan.InterestTypeId = type.Id;

                if (olditem != null)
                {
                    olditem.IsActive = false;
                }

                InterestItem item = new InterestItem();
                item.LoanAccount = loan;
                item.IsActive = true;
                item.Amount = amount;
                ObjectContext.InterestItems.AddObject(item);
                return true;
            }
        }
    }
}
