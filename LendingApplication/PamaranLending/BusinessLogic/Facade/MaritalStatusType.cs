using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class MaritalStatusType
    {
        public static MaritalStatusType GetByID(int id)
        {
            return Context.MaritalStatusTypes.SingleOrDefault(entity => entity.Id == id);
        }


        public static IQueryable<MaritalStatusType> All()
        {
            return Context.MaritalStatusTypes;
        }
    }
}
