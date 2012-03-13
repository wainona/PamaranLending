using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class NationalityTye
    {
        private const string _Filipino = "Filipino";
        private const string _Others = "Others";


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

        public static NationalityType FilipinoType
        {
            get
            {
                var type = Context.NationalityTypes.SingleOrDefault(entity => entity.Name == _Filipino);
                InitialDatabaseValueChecker.ThrowIfNull<NationalityType>(type);

                return type;
            }
        }

        public static NationalityType OtherType
        {
            get
            {
                var type = Context.NationalityTypes.SingleOrDefault(entity => entity.Name == _Others);
                InitialDatabaseValueChecker.ThrowIfNull<NationalityType>(type);

                return type;
            }
        }
    }
}
