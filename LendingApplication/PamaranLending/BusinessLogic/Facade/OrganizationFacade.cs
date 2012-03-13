using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Organization
    {
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

        public static string GetOrganizationName(int partyRoleId)
        {
            string name = string.Empty;
            var party = Context.PartyRoles.FirstOrDefault(e => e.Id == partyRoleId).Party;
            if (party != null)
            {
                var org = Context.Organizations.FirstOrDefault(e => e.PartyId == party.Id);
                name = org.OrganizationName;
            }
            return name;

        }
    }
}
