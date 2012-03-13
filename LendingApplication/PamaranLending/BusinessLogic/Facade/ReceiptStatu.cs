using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ReceiptStatu
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

        public static ReceiptStatu Create(Receipt receipt, DateTime transitionDateTime, ReceiptStatusType type, bool isActive)
        {
            ReceiptStatu newReceiptStatus = new ReceiptStatu();
            newReceiptStatus.IsActive = isActive;
            newReceiptStatus.Receipt = receipt;
            newReceiptStatus.TransitionDateTime = transitionDateTime;
            newReceiptStatus.ReceiptStatusTypeId = type.Id;

            return newReceiptStatus;
        }

        public static ReceiptStatu Create(Receipt receipt, DateTime transitionDateTime, ReceiptStatusType type)
        {
            ReceiptStatu newReceiptStatus = new ReceiptStatu();
            newReceiptStatus.IsActive = true;
            newReceiptStatus.Receipt = receipt;
            newReceiptStatus.TransitionDateTime = transitionDateTime;
            newReceiptStatus.ReceiptStatusTypeId = type.Id;

            return newReceiptStatus;
        }

        public static ReceiptStatu GetActive(Receipt receipt)
        {
            return receipt.ReceiptStatus.FirstOrDefault(entity => entity.IsActive);
        }

        public static ReceiptStatu ChangeStatus(Receipt receipt, ReceiptStatusType receiptStatusTo, string remarks)
        {
            var statusFrom = GetActive(receipt);
            if (statusFrom != null && statusFrom.IsActive == true)
            {
                statusFrom.IsActive = false;
            }

            ReceiptStatu status = new ReceiptStatu();
            if (statusFrom == null || statusFrom.IsActive == false)
            {
                status = Create(receipt, DateTime.Now, receiptStatusTo, true);
                status.Remarks = remarks;
            }

            Context.ReceiptStatus.AddObject(status);
            return status;
        }
    }
}