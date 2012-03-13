using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DisbursementType
    {
        private const string LoanDisbursement = "Loan Disbursement";
        private const string Encashment = "Encashment";
        private const string OtherLoanDisbursement = "Other Loan Disbursement";
        private const string Change = "Change";
        private const string Rediscounting = "Rediscounting";
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

        public static DisbursementType LoanDisbursementType
        {
            get
            {
                var type = Context.DisbursementTypes.SingleOrDefault(entity => entity.Name == LoanDisbursement);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementType>(type);

                return type;
            }
        }
        public static DisbursementType EncashmentType
        {
            get
            {
                var type = Context.DisbursementTypes.SingleOrDefault(entity => entity.Name == Encashment);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementType>(type);

                return type;
            }
        }
        public static DisbursementType RediscountingType
        {
            get
            {
                var type = Context.DisbursementTypes.SingleOrDefault(entity => entity.Name == Rediscounting);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementType>(type);

                return type;
            }
        }
        public static DisbursementType OtherLoanDisbursementType
        {
            get
            {
                var type = Context.DisbursementTypes.SingleOrDefault(entity => entity.Name == OtherLoanDisbursement);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementType>(type);

                return type;
            }
        }
        public static DisbursementType ChangeType
        {
            get
            {
                var type = Context.DisbursementTypes.SingleOrDefault(entity => entity.Name == Change);
                InitialDatabaseValueChecker.ThrowIfNull<DisbursementType>(type);

                return type;
            }
        }
    }
    public partial class LoanDisbursementType
    {
        private const string _FirstAvailment = "First Availment";
        private const string _Additional = "Additional";
        private const string _ACCheque = "AC Cheque";
        private const string _ACATM = "AC ATM";
        private const string _Change = "Change";

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

        public static LoanDisbursementType FirstAvailment
        {
            get
            {
                var type = Context.LoanDisbursementTypes.SingleOrDefault(entity => entity.Name == _FirstAvailment);
                InitialDatabaseValueChecker.ThrowIfNull<LoanDisbursementType>(type);

                return type;
            }
        }
        public static LoanDisbursementType Additional
        {
            get
            {
                var type = Context.LoanDisbursementTypes.SingleOrDefault(entity => entity.Name == _Additional);
                InitialDatabaseValueChecker.ThrowIfNull<LoanDisbursementType>(type);

                return type;
            }
        }
        public static LoanDisbursementType ACCheque
        {
            get
            {
                var type = Context.LoanDisbursementTypes.SingleOrDefault(entity => entity.Name == _ACCheque);
                InitialDatabaseValueChecker.ThrowIfNull<LoanDisbursementType>(type);

                return type;
            }
        }
        public static LoanDisbursementType ACATM
        {
            get
            {
                var type = Context.LoanDisbursementTypes.SingleOrDefault(entity => entity.Name == _ACATM);
                InitialDatabaseValueChecker.ThrowIfNull<LoanDisbursementType>(type);

                return type;
            }
        }
        public static LoanDisbursementType Change
        {
            get
            {
                var type = Context.LoanDisbursementTypes.SingleOrDefault(entity => entity.Name == _Change);
                InitialDatabaseValueChecker.ThrowIfNull<LoanDisbursementType>(type);

                return type;
            }
        }
    }
}
