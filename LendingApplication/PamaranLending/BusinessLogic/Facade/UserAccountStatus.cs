using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UserAccountStatu
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

        public static UserAccountStatu GetActive(UserAccount userAccount)
        {
            return userAccount.UserAccountStatus.FirstOrDefault(entity => entity.EndDate == null);
        }

        public static UserAccountStatu CreateOrUpdateCurrent(UserAccount userAccount, UserAccountStatusType statusType, DateTime today)
        {
            UserAccountStatu currentStatus = GetActive(userAccount);
            if (currentStatus != null && currentStatus.StatusTypeId != statusType.Id)
                currentStatus.EndDate = today;

            if (currentStatus == null || currentStatus.StatusTypeId != statusType.Id)
            {
                UserAccountStatu newUserAccountStatu = new UserAccountStatu();
                newUserAccountStatu.UserAccount = userAccount;
                newUserAccountStatu.UserAccountStatusType = statusType;
                newUserAccountStatu.EffectiveDate = today;
                newUserAccountStatu.EndDate = null;

                Context.UserAccountStatus.AddObject(newUserAccountStatu);
                return newUserAccountStatu;
            }
            return currentStatus;
        }
    }
}
