using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class ProductStatusType
    {
        public static IQueryable<ProductStatusType> All
        {
            get
            {
                return Context.ProductStatusTypes;
            }
        }
    }
}
