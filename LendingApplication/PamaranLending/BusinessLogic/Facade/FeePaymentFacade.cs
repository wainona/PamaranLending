using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic.FullfillmentForms;

namespace BusinessLogic
{
    public partial class FeePayment
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

        public static FeePayment GetById(int id)
        {
            return Context.FeePayments.FirstOrDefault(entity => entity.Id == id);
        }

        public static FeePayment GetByPaymentId(int id)
        {
            return Context.FeePayments.SingleOrDefault(entity => entity.PaymentId == id);
        }
        public static IEnumerable<FeePayment> GetFeesById(int paymentid)
        {
            return Context.FeePayments.Where(e => e.PaymentId == paymentid);
        }

        public static FeePayment Create(int paymentId, string particular, decimal amount)
        {
            FeePayment feeitem = new FeePayment();
            feeitem.PaymentId = paymentId;
            feeitem.Particular = particular;
            feeitem.FeeAmount = amount;

            Context.FeePayments.AddObject(feeitem);
            return feeitem;
        }

        public static FeePayment Update(int id, int paymentId, string particular, decimal amount)
        {
            FeePayment feeitem = FeePayment.GetById(id);
            if (feeitem != null)
            {
                feeitem.Particular = particular;
                feeitem.FeeAmount = amount;
                feeitem.PaymentId = paymentId;
            }
            return feeitem;
        }

    }
}
