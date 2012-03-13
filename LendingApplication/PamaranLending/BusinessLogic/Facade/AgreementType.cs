using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class AgreementType
    {
        public static AgreementType GetById(int id)
        {
            return Context.AgreementTypes.SingleOrDefault(entity => entity.Id == id);
        }
    }
}
