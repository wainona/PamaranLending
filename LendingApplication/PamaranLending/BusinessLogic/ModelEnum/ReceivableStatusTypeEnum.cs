using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ReceivableStatusType
    {
        private const string Open = "Open";
        private const string PartiallyPaid = "Partially Paid";
        private const string FullyPaid = "Fully Paid";
        private const string Cancelled = "Cancelled";

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

        public static ReceivableStatusType OpenType
        {
            get
            {
                var type = Context.ReceivableStatusTypes.SingleOrDefault(entity => entity.Name == Open);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableStatusType>(type);

                return type;
            }
        }

        public static ReceivableStatusType PartiallyPaidType
        {
            get
            {
                var type = Context.ReceivableStatusTypes.SingleOrDefault(entity => entity.Name == PartiallyPaid);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableStatusType>(type);

                return type;
            }
        }

        public static ReceivableStatusType FullyPaidType
        {
            get
            {
                var type = Context.ReceivableStatusTypes.SingleOrDefault(entity => entity.Name == FullyPaid);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableStatusType>(type);

                return type;
            }
        }

        public static ReceivableStatusType CancelledType
        {
            get
            {
                var type = Context.ReceivableStatusTypes.SingleOrDefault(entity => entity.Name == Cancelled);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableStatusType>(type);

                return type;
            }
        }
    }
}
