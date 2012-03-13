using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ForeignExchangeTransaction
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static ForeignExchangeTransaction Create(DateTime transactionDate, decimal amountFrom, decimal amountTo, int exchangeRateId, int processedToPartyRoleId, int processedByPartyRoleId)
        {
            ForeignExchangeTransaction newForeignExchangeTransaction = new ForeignExchangeTransaction();
            return newForeignExchangeTransaction;
        }
    }
}
