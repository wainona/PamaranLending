using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class CustomerStatusType
    {
        public static CustomerStatusType GetById(int id)
        {
            return Context.CustomerStatusTypes.SingleOrDefault(entity => entity.Id == id);
        }
    }
}
