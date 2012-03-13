using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class EducAttainmentType
    {
        public static IQueryable<EducAttainmentType> All()
        {
            return Context.EducAttainmentTypes;
        }
    }
}
