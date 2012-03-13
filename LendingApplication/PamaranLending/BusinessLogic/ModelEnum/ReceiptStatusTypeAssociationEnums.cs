using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ReceiptStatusTypeAssoc
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
        public static ReceiptStatusTypeAssoc OpenToAppliedReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
        public static ReceiptStatusTypeAssoc OpenToClosedReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
        public static ReceiptStatusTypeAssoc OpenToCancelledReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.CancelledReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
        public static ReceiptStatusTypeAssoc AppliedToClosedReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
        public static ReceiptStatusTypeAssoc AppliedToCancelledReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.CancelledReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
        public static ReceiptStatusTypeAssoc ClosedToCancelledReceiptTypeAssociation
        {
            get
            {
                var type = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity => entity.FromStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id
                    && entity.ToStatusTypeId == ReceiptStatusType.CancelledReceiptStatusType.Id && entity.EndDate == null);
                InitialDatabaseValueChecker.ThrowIfNull<ReceiptStatusTypeAssoc>(type);

                return type;
            }
        }
   
    }
}
