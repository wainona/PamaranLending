using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementItem
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public static DisbursementItem GetById(int id)
        {
            return Context.DisbursementItems.FirstOrDefault(entity => entity.Id == id);
        }
        public static DisbursementItem GetByPaymentId(int id)
        {
            return Context.DisbursementItems.SingleOrDefault(entity => entity.PaymentId == id);
        }

        public static DisbursementItem Create(int paymentId, string particular, decimal amount)
        {
            DisbursementItem disbursementitem = new DisbursementItem();
            disbursementitem.PaymentId = paymentId;
            disbursementitem.Particular = particular;
            disbursementitem.PerItemAmount = amount;

            Context.DisbursementItems.AddObject(disbursementitem);
            return disbursementitem;
        }
        public static DisbursementItem Update(int id, int paymentId, string particular, decimal amount)
        {
            DisbursementItem disbursementItem = DisbursementItem.GetById(id);
            if (disbursementItem != null)
            {
                disbursementItem.Particular = particular;
                disbursementItem.PerItemAmount = amount;
                disbursementItem.PaymentId = paymentId;
            }
            return disbursementItem;
        }

    }
}
