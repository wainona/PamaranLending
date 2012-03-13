using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ReceiptStatusType
    {
        private const string Open = "Open";
        private const string Applied = "Applied";
        private const string Closed = "Closed";
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

        public static ReceiptStatusType OpenReceiptStatusType
        {
            get
            {
                var type = Context.ReceiptStatusTypes.SingleOrDefault(entity => entity.Name == Open);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusType>(type);

                return type;   
            }
        }
        public static ReceiptStatusType AppliedReceiptStatusType
        {
            get
            {
                var type = Context.ReceiptStatusTypes.SingleOrDefault(entity => entity.Name == Applied);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusType>(type);

                return type;
            }
        }
        public static ReceiptStatusType ClosedReceiptStatusType
        {
            get
            {
                var type = Context.ReceiptStatusTypes.SingleOrDefault(entity => entity.Name == Closed);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusType>(type);

                return type;
            }
        }
        public static ReceiptStatusType CancelledReceiptStatusType
        {
            get
            {
                var type = Context.ReceiptStatusTypes.SingleOrDefault(entity => entity.Name == Cancelled);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusType>(type);

                return type;
            }
        }
    }
}
