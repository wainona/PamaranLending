using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class MaritalStatu
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


        public static MaritalStatu GetByID(int id)
        {
            return Context.MaritalStatus.SingleOrDefault(entity => entity.Id == id);
        }

        public static MaritalStatu GetActive(Person person)
        {
            return person.MaritalStatus.SingleOrDefault(entity => entity.IsActive == true && entity.PartyId == person.PartyId);
        }
    }
}
