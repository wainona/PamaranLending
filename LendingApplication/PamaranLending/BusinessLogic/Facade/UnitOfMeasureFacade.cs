using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UnitOfMeasure
    {
        public static UnitOfMeasure GetByID(int id)
        {
            return Context.UnitOfMeasures.SingleOrDefault(entity => entity.Id == id);
        }

        public static IQueryable<UnitOfMeasure> All(Func<UnitOfMeasure, bool> filter)
        {
            return Context.UnitOfMeasures.Where(entity => filter(entity));
        }

        public static IQueryable<UnitOfMeasure> All(UnitOfMeasureType type)
        {
            return Context.UnitOfMeasures.Where(entity => entity.UomTypeId == type.Id);
        }

    }

    //public class UnitOfMeasureFacade
    //{
    //    public static FinancialEntities Context
    //    {
    //        get
    //        {
    //            return ObjectContextManager<FinancialEntities>.CreateFromConfig().ObjectContext;
    //        }
    //    }

    //    public static IQueryable<UnitOfMeasure> All(Func<UnitOfMeasure, bool> filter)
    //    {
    //        return Context.CreateObjectSet<T>().Where(entity => filter(entity));
    //    }
    //    public static IQueryable<UnitOfMeasure> All(UnitOfMeasureType type)
    //    {
    //        return Context.UnitOfMeasures.Where(entity => entity.UomTypeId == type.Id);
    //    }
    //}
}
