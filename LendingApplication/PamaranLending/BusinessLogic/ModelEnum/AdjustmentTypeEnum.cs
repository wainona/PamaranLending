using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class AdjustmentType
    {
        private const string WaivedPenalty = "Waived Penalty";
        private const string Rebate = "Rebate";
        private const string AdditionalInterest = "Additional Interest";
        private const string Waive = "Waive";

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

        public static AdjustmentType WaivedPenaltyType
        {
            get
            {
                var type = Context.AdjustmentTypes.SingleOrDefault(entity => entity.Name == WaivedPenalty);
                InitialDatabaseValueChecker.ThrowIfNull<AdjustmentType>(type);

                return type;
            }
        }
        public static AdjustmentType WaivedType
        {
            get
            {
                var type = Context.AdjustmentTypes.SingleOrDefault(entity => entity.Name == Waive);
                InitialDatabaseValueChecker.ThrowIfNull<AdjustmentType>(type);

                return type;
            }
        }
        public static AdjustmentType AdditionalInterestType
        {
            get
            {
                var type = Context.AdjustmentTypes.SingleOrDefault(entity => entity.Name == AdditionalInterest);
                InitialDatabaseValueChecker.ThrowIfNull<AdjustmentType>(type);

                return type;
            }
        }

        public static AdjustmentType RebateType
        {
            get
            {
                var type = Context.AdjustmentTypes.SingleOrDefault(entity => entity.Name == Rebate);
                InitialDatabaseValueChecker.ThrowIfNull<AdjustmentType>(type);

                return type;
            }
        }
    }
}
