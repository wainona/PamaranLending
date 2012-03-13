using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class TimeUnitConversion
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

        public static decimal GetMultiplier(int sourceUomId, int targetUomId)
        {
            var conversion = Context.TimeUnitConversions.FirstOrDefault(entity => entity.SourceUomId == sourceUomId
                && entity.TargetUomId == targetUomId);

            if (conversion != null)
                return conversion.Multiplier;

            return 0;
        }

        public static decimal GetOffset(int sourceUomId, int targetUomId)
        {
            var conversion = Context.TimeUnitConversions.FirstOrDefault(entity => entity.SourceUomId == sourceUomId
                && entity.TargetUomId == targetUomId);

            if (conversion != null)
                return (decimal)conversion.Offset;

            return 1;
        }

        public static decimal Convert(int sourceUomId, int targetUomId, int given)
        {
            var conversion = Context.TimeUnitConversions.FirstOrDefault(entity => entity.SourceUomId == sourceUomId
                && entity.TargetUomId == targetUomId);

            if (conversion != null)
            {
                var offset = conversion.Offset ?? 1;
                return (given * conversion.Multiplier) / offset;
            }

            return 0;
        }

        public static decimal Convert(int sourceUomId, int targetUomId, decimal given)
        {
            var conversion = Context.TimeUnitConversions.FirstOrDefault(entity => entity.SourceUomId == sourceUomId
                && entity.TargetUomId == targetUomId);

            if (conversion != null)
            {
                var offset = conversion.Offset ?? 1;
                return (given * conversion.Multiplier) / offset;
            }

            return 0;
        }
    }
}
