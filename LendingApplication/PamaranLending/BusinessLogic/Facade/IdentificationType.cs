using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class IdentificationType
    {
        public static IQueryable<IdentificationType> All()
        {
            return Context.IdentificationTypes;
        }
    }
}
