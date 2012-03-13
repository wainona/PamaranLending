using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FormType
    {
        private const string TransactionSlip = "Transaction Slip";
        private const string PaymentForm = "Payment Form";
        private const string PromissoryNote = "Promissory Note";
        private const string SPA = "SPA";
        private const string EncashmentForm = "Encashment Form";
        private const string RediscountingForm = "Rediscounting Form";
        private const string ChangeForm = "Change Form";
        private const string OtherDisbursementForm = "Other Disbursement Form";

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

        public static FormType TransactionSlipType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == TransactionSlip);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }

        public static FormType PaymentFormType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == PaymentForm);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType PromissoryNoteType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == PromissoryNote);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType SPAType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == SPA);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType EncashmentFormType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == EncashmentForm);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType RediscountingFormType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == RediscountingForm);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType ChangeFormType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == ChangeForm);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


        public static FormType OtherDisbursementFormType
        {
            get
            {
                var type = Context.FormTypes.SingleOrDefault(entity => entity.Name == OtherDisbursementForm);
                InitialDatabaseValueChecker.ThrowIfNull<FormType>(type);

                return type;
            }
        }


    }
}
