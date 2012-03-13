using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class SourceOfIncome
    {
        public static SourceOfIncome GetById(int id)
        {
            return Context.SourceOfIncomes.SingleOrDefault(entity => entity.Id == id);
        }

        public static IQueryable<SourceOfIncome> All(CustomerSourceOfIncome sourceOfIncome)
        {
            return Context.SourceOfIncomes.Where(entity => entity.Id == sourceOfIncome.SourceOfIncomeId);
        }

        public static IQueryable<SourceOfIncome> All()
        {
            return Context.SourceOfIncomes;
        }
    }
}
