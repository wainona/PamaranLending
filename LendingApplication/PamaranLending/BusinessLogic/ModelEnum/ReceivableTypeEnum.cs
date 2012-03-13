using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ReceivableType
    {
        private const string Principal = "Principal";
        private const string Interest = "Interest";
        private const string PastDue = "Past Due";

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

        public static ReceivableType PrincipalType
        {
            get
            {
                var type = Context.ReceivableTypes.SingleOrDefault(entity => entity.Name == Principal);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableType>(type);

                return type;
            }
        }

        public static ReceivableType InterestType
        {
            get
            {
                var type = Context.ReceivableTypes.SingleOrDefault(entity => entity.Name == Interest);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableType>(type);

                return type;
            }
        }

        public static ReceivableType PastDueType
        {
            get
            {
                var type = Context.ReceivableTypes.SingleOrDefault(entity => entity.Name == PastDue);
                InitialDatabaseValueChecker.ThrowIfNull<ReceivableType>(type);

                return type;
            }
        }
    }
}
