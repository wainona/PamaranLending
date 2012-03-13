using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PersonIdentification
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

        public static PersonIdentification GetPersonIdentification(Party party, IdentificationType identificationType)
        {
            var personIdentification = Context.PersonIdentifications.SingleOrDefault(entity => entity.PartyId == party.Id 
                                                                                && entity.IdentificationTypeId == identificationType.Id);

            return personIdentification;
        }
    }
}
