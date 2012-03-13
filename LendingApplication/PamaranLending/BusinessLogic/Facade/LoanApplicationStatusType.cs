using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class LoanApplicationStatusType
    {
        public static LoanApplicationStatusType GetById(int id)
        {
            return Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Id == id);
        }

        public static IQueryable<LoanApplicationStatusType> All(LoanApplicationStatusType status)
        {
            return Context.LoanApplicationStatusTypes.Where(entity => entity.Name == status.Name);
        }

        public static LoanApplicationStatusType GetByName(LoanApplicationStatusType status)
        {
            return Context.LoanApplicationStatusTypes.SingleOrDefault(entity => entity.Name == status.Name);
        }
    }
}
