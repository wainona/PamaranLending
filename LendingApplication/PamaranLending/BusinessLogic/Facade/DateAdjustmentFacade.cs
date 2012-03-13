using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DateAdjustment
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
        public static DateTime LastDayOfMonth(DateTime referenceDate)
        {
            DateTime firstDayOfTheMonth = new DateTime(referenceDate.Year, referenceDate.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static DateTime AdjustToNextWorkingDayIfInvalid(DateTime referenceDate)
        {
            bool valid = false;
            var holidays = Context.Holidays;
            do
            {
                var holiday = holidays.FirstOrDefault(entity => entity.Date == referenceDate);
                if (holiday != null)
                    referenceDate = GetNextDay(referenceDate);

                int toAdd = 0;
                if (referenceDate.DayOfWeek == DayOfWeek.Saturday)
                    toAdd = 2;
                else if (referenceDate.DayOfWeek == DayOfWeek.Sunday)
                    toAdd = 1;

                valid = toAdd == 0;
                if (toAdd != 0)
                    referenceDate = referenceDate.AddDays(toAdd);

            } while (valid == false);

            return referenceDate;
        }

        public static DateTime GetNextDay(DateTime referenceDate)
        {
            referenceDate = referenceDate.AddDays(1);
            return referenceDate;
        }

    }
}
