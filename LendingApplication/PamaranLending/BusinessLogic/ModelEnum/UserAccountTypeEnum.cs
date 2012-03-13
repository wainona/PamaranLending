using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UserAccountType
    {
        private const string _Admin = "Admin";
        private const string _SuperAdmin = "Super Admin";
        private const string _Accountant = "Accountant";
        private const string _Teller = "Teller";

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

        public static UserAccountType Admin
        {
            get
            {
                var type = Context.UserAccountTypes.SingleOrDefault(entity => entity.Name == _Admin);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountType>(type);

                return type;
            }
        }

        public static UserAccountType SuperAdmin
        {
            get
            {
                var type = Context.UserAccountTypes.SingleOrDefault(entity => entity.Name == _SuperAdmin);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountType>(type);

                return type;
            }
        }

        public static UserAccountType Teller
        {
            get
            {
                var type = Context.UserAccountTypes.SingleOrDefault(entity => entity.Name == _Teller);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountType>(type);

                return type;
            }
        }

        public static UserAccountType Accountant
        {
            get
            {
                var type = Context.UserAccountTypes.SingleOrDefault(entity => entity.Name == _Accountant);
                InitialDatabaseValueChecker.ThrowIfNull<UserAccountType>(type);

                return type;
            }
        }

    }
}
