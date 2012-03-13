using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class AgreementType
    {
        private const string LoanAgreement = "Loan Agreement";
        private const string CompromiseAgreement = "Compromise Agreement";

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

        public static AgreementType LoanAgreementType
        {
            get
            {
                var type = Context.AgreementTypes.SingleOrDefault(entity => entity.Name == LoanAgreement);
                InitialDatabaseValueChecker.ThrowIfNull<AgreementType>(type);

                return type;
            }
        }

        public static AgreementType CompromiseAgreementType
        {
            get
            {
                var type = Context.AgreementTypes.SingleOrDefault(entity => entity.Name == CompromiseAgreement);
                InitialDatabaseValueChecker.ThrowIfNull<AgreementType>(type);

                return type;
            }
        }
    }
}
