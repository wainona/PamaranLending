using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PaymentMethodType
    {
        private const string Cash = "Cash";
        private const string PayCheck = "Pay Check";
        private const string PersonalCheck = "Personal Check";
        private const string ATM = "ATM";

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

        public static PaymentMethodType CashType
        {
            get
            {
                var type = Context.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == Cash);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentMethodType>(type);

                return type;
            }
        }

        public static PaymentMethodType PayCheckType
        {
            get
            {
                var type = Context.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == PayCheck);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentMethodType>(type);

                return type;
            }
        }

        public static PaymentMethodType PersonalCheckType
        {
            get
            {
                var type = Context.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == PersonalCheck);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentMethodType>(type);

                return type;
            }
        }

        public static PaymentMethodType ATMType
        {
            get
            {
                var type = Context.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == ATM);
                InitialDatabaseValueChecker.ThrowIfNull<PaymentMethodType>(type);

                return type;
            }
        }

        public static IQueryable<PaymentMethodType> All()
        {
            return Context.PaymentMethodTypes;
        }
    }
}