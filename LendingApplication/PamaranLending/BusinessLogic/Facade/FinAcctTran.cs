using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FinAcctTran
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        public static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static FinAcctTran CreateFinAcctTran(int financialAccountId, Payment payment, DateTime transactionDate, DateTime entryDate, FinlAcctTransType finAcctTransactionType) {
            FinAcctTran newFinAcctTran = new FinAcctTran();
            newFinAcctTran.FinancialAccountId = financialAccountId;
            newFinAcctTran.PaymentId = payment.Id;
            newFinAcctTran.TransactionDate = transactionDate;
            newFinAcctTran.EntryDate = entryDate;
            newFinAcctTran.FinancialAcctTransTypeId = finAcctTransactionType.Id;
            newFinAcctTran.Amount = payment.TotalAmount;

            return newFinAcctTran;
        }
    }
}
