using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class SpecificPaymentType
    {
        private const string CheckForEncashment = "Check for Encashment";
        private const string ReloanDeduction = "Reloan Deduction";
        private const string LoanPayment = "Loan Payment";
        private const string FeePayment = "Fee Payment";
        private const string Encashment = "Encashment";
        private const string InterestPayment = "Interest Payment";
        private const string CheckForRediscounting = "Check for Rediscounting";
        private const string Rediscounting = "Rediscounting";
        private const string LoanDisbursement = "Loan Disbursement";
        private const string OtherDisbursement = "Other Disbursement";
        private const string Change = "Change";

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

        public static SpecificPaymentType ChangeType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == Change);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType CheckForEncashmentType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == CheckForEncashment);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType LoanDisbursementType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == LoanDisbursement);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }

        public static SpecificPaymentType OtherDisbursementType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == OtherDisbursement);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }

        public static SpecificPaymentType CheckForRediscountingType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == CheckForRediscounting);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType RediscountingType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == Rediscounting);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }


        public static SpecificPaymentType EncashmentType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == Encashment);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }

        public static SpecificPaymentType InterestPaymentType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == InterestPayment);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType ReloanDeductionType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == ReloanDeduction);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType LoanPaymentType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == LoanPayment);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }
        public static SpecificPaymentType FeePaymentType
        {
            get
            {
                var type = Context.SpecificPaymentTypes.SingleOrDefault(entity => entity.Name == FeePayment);
                InitialDatabaseValueChecker.ThrowIfNull<SpecificPaymentType>(type);

                return type;
            }
        }

    }
}
