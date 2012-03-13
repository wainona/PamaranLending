using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class HomeOwnershipType
    {
        public static IQueryable<HomeOwnershipType> All()
        {
            return Context.HomeOwnershipTypes;
        }
    }
}
