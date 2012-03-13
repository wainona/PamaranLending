using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class InterestType
    {
        private const string FixedInterest = "Fixed";
        private const string ZeroInterest = "Zero";
        private const string PercentageInterest = "Percentage";

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

        public static InterestType FixedInterestTYpe
        {
            get
            {
                var type = Context.InterestTypes.SingleOrDefault(entity => entity.Name == FixedInterest);
                InitialDatabaseValueChecker.ThrowIfNull<InterestType>(type);

                return type;
            }
        }
        public static InterestType ZeroInterestTYpe
        {
            get
            {
                var type = Context.InterestTypes.SingleOrDefault(entity => entity.Name == ZeroInterest);
                InitialDatabaseValueChecker.ThrowIfNull<InterestType>(type);

                return type;
            }
        }
        public static InterestType PercentageInterestTYpe
        {
            get
            {
                var type = Context.InterestTypes.SingleOrDefault(entity => entity.Name == PercentageInterest);
                InitialDatabaseValueChecker.ThrowIfNull<InterestType>(type);

                return type;
            }
        }

    }
}
