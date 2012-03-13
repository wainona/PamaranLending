using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Holiday
    {
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static Holiday GetById(int id)
        {
            return ObjectContext.Holidays.SingleOrDefault(entity => entity.Id == id);
        }

        public static DateTime[] DisabledDates
        {
            get
            {
                var today = DateTime.Now;
                return new DateTime[] { today };
            }
        }

    }
}
