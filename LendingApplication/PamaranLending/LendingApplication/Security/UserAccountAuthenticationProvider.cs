using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Security
{
    public class UserAccountAuthenticationProvider : IAuthenticationProvider
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

        public string Username { get; private set; }

        public string UserType { get; private set; }

        public int UserId { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool Authenticate(string username, string password)
        {
            var userAccount = ObjectContext.UserAccounts.FirstOrDefault(entity => entity.Username == username && entity.EndDate == null);
            if (userAccount == null || userAccount.CurrentStatus.StatusTypeId == UserAccountStatusType.InactiveType.Id) return false;

            if (SimplifiedHash.VerifyHash(password, userAccount.Password, SimplifiedHash.HashType.sha1) == false)
            {
                this.IsAuthenticated = false;
                return false;
            }

            this.Username = userAccount.Username;
            this.UserType = userAccount.UserAccountType.Name;
            this.UserId = userAccount.Id;
            this.IsAuthenticated = true;

            return this.IsAuthenticated;
        }
    }
}