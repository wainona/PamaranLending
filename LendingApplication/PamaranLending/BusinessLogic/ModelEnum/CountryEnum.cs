using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Country
    {
        private const string _Philippines = "Philippines";
        //private const string _Others = "Others";


        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        ///// </summary>
        //private static FinancialEntities Context
        //{
        //    get
        //    {
        //        if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
        //            return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
        //        else
        //            return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
        //    }
        //}

        public static Country Philippines
        {
            get
            {
                var type = Context.Countries.SingleOrDefault(entity => entity.Name == _Philippines);
                InitialDatabaseValueChecker.ThrowIfNull<Country>(type);

                return type;
            }
        }
    }
}
