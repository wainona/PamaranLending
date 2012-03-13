using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PartyRelationship
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        private static  FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static PartyRelationship GetPartyRelationship(int firstPartyRoleId, int secondPartyRoleId)
        {
            return Context.PartyRelationships.SingleOrDefault(entity => entity.EndDate == null && entity.FirstPartyRoleId == firstPartyRoleId 
                                                                        && entity.SecondPartyRoleId == secondPartyRoleId);
        }

    }
}
