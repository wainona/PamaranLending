using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class LoanModificationType
    {
        private const string Split = "Split";
        private const string Consolidate = "Consolidate";
        private const string ChangeIcm = "Change ICM";
        private const string AdditionalLoan = "Additional Loan";
        private const string ChangeInterest = "Change Interest";


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

        public static LoanModificationType SplitType
        {
            get
            {
                var type = Context.LoanModificationTypes.SingleOrDefault(entity => entity.Name == Split);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationType>(type);

                return type;
            }
        }

        public static LoanModificationType ConsolidateType
        {
            get
            {
                var type = Context.LoanModificationTypes.SingleOrDefault(entity => entity.Name == Consolidate);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationType>(type);

                return type;
            }
        }

        public static LoanModificationType ChangeIcmType
        {
            get
            {
                var type = Context.LoanModificationTypes.SingleOrDefault(entity => entity.Name == ChangeIcm);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationType>(type);

                return type;
            }
        }

        public static LoanModificationType AdditionalLoanType
        {
            get
            {
                var type = Context.LoanModificationTypes.SingleOrDefault(entity => entity.Name == AdditionalLoan);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationType>(type);

                return type;
            }
        }

        public static LoanModificationType ChangeInterestType
        {
            get
            {
                var type = Context.LoanModificationTypes.SingleOrDefault(entity => entity.Name == ChangeInterest);
                InitialDatabaseValueChecker.ThrowIfNull<LoanModificationType>(type);

                return type;
            }
        }
    }
}
