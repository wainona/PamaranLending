using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Agreement
    {
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

        public static Agreement GetById(int id)
        {
            return Context.Agreements.SingleOrDefault(entity => entity.Id == id);
        }

        public static Agreement GetByApplicationId(int id)
        {
            return Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == id);
        }

        public static Agreement Create(LoanApplication loanApplication, AgreementType loanAgreementType, DateTime today)
        {
            Agreement agreement = new Agreement();
            agreement.AgreementType = loanAgreementType;
            agreement.Application = loanApplication.Application;
            agreement.AgreementDate = today;
            agreement.EffectiveDate = today;
            Context.Agreements.AddObject(agreement);
            //Context.SaveChanges();
            return agreement;
        }
    }
}
