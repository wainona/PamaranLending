using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class COVTransactionType
    {
        private const string DepositToVault = "Deposit To Vault";
        private const string WithdrawFromVault = "Withdraw From Vault";
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

        public static COVTransactionType DepositToVaultType
        {
            get
            {
                var type = Context.COVTransactionTypes.SingleOrDefault(entity => entity.Name == DepositToVault);
                InitialDatabaseValueChecker.ThrowIfNull<COVTransactionType>(type);

                return type;
            }
        }
        public static COVTransactionType WithdrawFromVaultType
        {
            get
            {
                var type = Context.COVTransactionTypes.SingleOrDefault(entity => entity.Name == WithdrawFromVault);
                InitialDatabaseValueChecker.ThrowIfNull<COVTransactionType>(type);

                return type;
            }
        }

    }
}
