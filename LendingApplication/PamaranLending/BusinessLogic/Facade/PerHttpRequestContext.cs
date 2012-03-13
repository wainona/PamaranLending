using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace BusinessLogic
{
    public class PerHttpRequestContext<T> where T : EntityObject
    {
        public static IQueryable<T> All(Func<T, bool> filter)
        {
            return Context.CreateObjectSet<T>().Where(entity => filter(entity));
        }

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
    }
}
