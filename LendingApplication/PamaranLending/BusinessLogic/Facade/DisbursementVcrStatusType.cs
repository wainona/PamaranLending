using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementVcrStatusType
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

        public static DisbursementVcrStatusType GetById(int id)
        {
            return Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Id == id);
        }

        public static IQueryable<DisbursementVcrStatusType> All(DisbursementVcrStatusType status)
        {
            return Context.DisbursementVcrStatusTypes.Where(entity => entity.Name == status.Name);
        }

        public static DisbursementVcrStatusType GetByName(string name)
        {
            return Context.DisbursementVcrStatusTypes.SingleOrDefault(entity => entity.Name == name);
        }
    }
}
