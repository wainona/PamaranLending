using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class OtherDisbursementParticular
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

        public static IQueryable<OtherDisbursementParticular> All()
        {
            return Context.OtherDisbursementParticulars;
        }

        public static OtherDisbursementParticular CreateParticular(string name)
        {
            OtherDisbursementParticular particular = new OtherDisbursementParticular();
            particular.Name = name;

            Context.OtherDisbursementParticulars.AddObject(particular);


            return particular;
        }

        public static OtherDisbursementParticular GetByName(string name)
        {
            return Context.OtherDisbursementParticulars.FirstOrDefault(entity => entity.Name == name);
        }
    }
}
