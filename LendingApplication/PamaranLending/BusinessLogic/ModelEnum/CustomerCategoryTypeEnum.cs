using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class CustomerCategoryType
    {
        private const string Teacher = "Teacher";
        private const string Others = "Others";


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

        public static CustomerCategoryType TeacherType
        {
            get
            {
                var type = Context.CustomerCategoryTypes.SingleOrDefault(entity => entity.Name == Teacher);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerCategoryType>(type);

                return type;
            }
        }
        public static CustomerCategoryType OthersType
        {
            get
            {
                var type = Context.CustomerCategoryTypes.SingleOrDefault(entity => entity.Name == Others);
                InitialDatabaseValueChecker.ThrowIfNull<CustomerCategoryType>(type);

                return type;
            }
        }

        public static IQueryable<CustomerCategoryType> All()
        {
            return Context.CustomerCategoryTypes;
        }
    }
}
