using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class EmployeePositionType
    {
        private const string Owner = "Owner";
        private const string Teller = "Teller";
        private const string Accountant = "Accountant";
        private const string SystemAdministrator = "System Administrator";

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

        public static EmployeePositionType OwnerType
        {
            get
            {
                var type = Context.EmployeePositionTypes.SingleOrDefault(entity => entity.Name == Owner);
                InitialDatabaseValueChecker.ThrowIfNull<EmployeePositionType>(type);

                return type;
            }
        }

        public static EmployeePositionType TellerType
        {
            get
            {
                var type = Context.EmployeePositionTypes.SingleOrDefault(entity => entity.Name == Teller);
                InitialDatabaseValueChecker.ThrowIfNull<EmployeePositionType>(type);

                return type;
            }
        }

        public static EmployeePositionType AccountantType
        {
            get
            {
                var type = Context.EmployeePositionTypes.SingleOrDefault(entity => entity.Name == Accountant);
                InitialDatabaseValueChecker.ThrowIfNull<EmployeePositionType>(type);

                return type;
            }
        }

        public static EmployeePositionType SystemAdministratorType
        {
            get
            {
                var type = Context.EmployeePositionTypes.SingleOrDefault(entity => entity.Name == SystemAdministrator);
                InitialDatabaseValueChecker.ThrowIfNull<EmployeePositionType>(type);

                return type;
            }
        }

        public static List<EmployeePositionType> All
        {
            get
            {
                var types = Context.EmployeePositionTypes.ToList();

                return types;
            }
        }
    }
}

