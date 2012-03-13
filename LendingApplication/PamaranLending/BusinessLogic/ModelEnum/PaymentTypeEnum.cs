using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PaymentType
    {
        private const string _Disbursement = "Disbursement";
        private const string _Receipt = "Receipt";
        private const string _FeePayment = "Fee Payment";
        private const string _LoanPayment = "Loan Payment";


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

        public static PaymentType Disbursement
        {
            get
            {
                var type = Context.PaymentTypes.SingleOrDefault(entity => entity.Name == _Disbursement);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentType>(type);

                return type;
            }
        }
        public static PaymentType LoanPayment
        {
            get
            {
                var type = Context.PaymentTypes.SingleOrDefault(entity => entity.Name == _LoanPayment);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentType>(type);

                return type;
            }
        }
        public static PaymentType FeePayment
        {
            get
            {
                var type = Context.PaymentTypes.SingleOrDefault(entity => entity.Name == _FeePayment);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentType>(type);

                return type;
            }
        }
        public static PaymentType Receipt
        {
            get
            {
                var type = Context.PaymentTypes.SingleOrDefault(entity => entity.Name == _Receipt);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentType>(type);

                return type;
            }
        }
    }
}
