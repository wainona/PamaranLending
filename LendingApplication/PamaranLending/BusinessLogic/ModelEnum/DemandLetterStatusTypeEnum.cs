using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DemandLetterStatusType
    {
        private const string RequireFirstDemandLetter = "Require First Demand Letter";
        private const string FirstDemandLetterSent = "First Demand Letter Sent";
        private const string RequireFinalDemandLetter = "Require Final Demand Letter";
        private const string FinalDemandLetterSent = "Final Demand Letter Sent";
        
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

        public static DemandLetterStatusType RequireFirstDemandLetterType
        {
            get
            {
                var type = Context.DemandLetterStatusTypes.SingleOrDefault(entity => entity.Name == RequireFirstDemandLetter);
                InitialDatabaseValueChecker.ThrowIfNull<DemandLetterStatusType>(type);

                return type;
            }
        }

        public static DemandLetterStatusType FirstDemandLetterSentType
        {
            get
            {
                var type = Context.DemandLetterStatusTypes.SingleOrDefault(entity => entity.Name == FirstDemandLetterSent);
                InitialDatabaseValueChecker.ThrowIfNull<DemandLetterStatusType>(type);

                return type;
            }
        }

        public static DemandLetterStatusType RequireFinalDemandLetterType
        {
            get
            {
                var type = Context.DemandLetterStatusTypes.SingleOrDefault(entity => entity.Name == RequireFinalDemandLetter);
                InitialDatabaseValueChecker.ThrowIfNull<DemandLetterStatusType>(type);

                return type;
            }
        }

        public static DemandLetterStatusType FinalDemandLetterSentType
        {
            get
            {
                var type = Context.DemandLetterStatusTypes.SingleOrDefault(entity => entity.Name == FinalDemandLetterSent);
                InitialDatabaseValueChecker.ThrowIfNull<DemandLetterStatusType>(type);

                return type;
            }
        }
    }
}
