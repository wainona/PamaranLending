using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FeePaymentParticular
    {
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

        public static NonProductFee GetParticularByName(string particular)
        {
            return Context.NonProductFees.SingleOrDefault(entity => entity.Name == particular);
        }

        public static List<NonProductFee> GetAllFeeParticulars()
        {
            return Context.NonProductFees.ToList();
        }

        public static NonProductFee CreateFeeParticular(string particular)
        {
            NonProductFee feeParticular = new NonProductFee();
            feeParticular.Name = particular;

            Context.NonProductFees.AddObject(feeParticular);
            return feeParticular;
        }
    }
}
