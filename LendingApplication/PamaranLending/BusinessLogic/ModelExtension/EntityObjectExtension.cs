using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Data.Objects.DataClasses;

namespace BusinessLogic
{
    public static class EntityObjectExtension
    {
        public static FinancialEntities GetContext(this EntityObject entity)
        {
            if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
            else
                return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
        }
    }
}
