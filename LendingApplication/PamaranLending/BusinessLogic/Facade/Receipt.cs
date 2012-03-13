using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Receipt
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

        public ReceiptStatu CurrentStatus
        {
            get
            {
                return this.ReceiptStatus.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public static void ChangeReceiptStatusFrom(Receipt receipt, DateTime today, decimal balance, ReceiptStatusType fromstatustype)
        {
            //var receiptstatus = receipt.CurrentStatus;
            ReceiptStatu newreceiptstatus = new ReceiptStatu();

            if (fromstatustype.Id == ReceiptStatusType.OpenReceiptStatusType.Id && balance == 0)
            {
                //create receipt status
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                  entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id &&
                  entity.ToStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                  entity.EndDate == null);
                if (validIndicator != null)
                {
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = false;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
                //create next receipt status
                var validIndicator1 = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                  entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                  entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id &&
                  entity.EndDate == null);
                if (validIndicator1 != null)
                {
                    ReceiptStatu nextreceiptstatus = new ReceiptStatu();
                    nextreceiptstatus.Receipt = receipt;
                    nextreceiptstatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                    nextreceiptstatus.TransitionDateTime = today;
                    nextreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
            }
            else if (fromstatustype.Id == ReceiptStatusType.OpenReceiptStatusType.Id && balance != 0)
            {
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                     entity.FromStatusTypeId == ReceiptStatusType.OpenReceiptStatusType.Id &&
                     entity.ToStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                     entity.EndDate == null);
                if (validIndicator != null)
                {
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
            }
            else if (fromstatustype.Id == ReceiptStatusType.AppliedReceiptStatusType.Id && balance == 0)
            {
                var validIndicator = Context.ReceiptStatusTypeAssocs.SingleOrDefault(entity =>
                      entity.FromStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id &&
                      entity.ToStatusTypeId == ReceiptStatusType.ClosedReceiptStatusType.Id &&
                      entity.EndDate == null);
                if (validIndicator != null)
                {
                    newreceiptstatus.Receipt = receipt;
                    newreceiptstatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                    newreceiptstatus.TransitionDateTime = today;
                    newreceiptstatus.IsActive = true;
                    Context.ReceiptStatus.AddObject(newreceiptstatus);
                }
            }
        }

        public static Receipt GetById(int id)
        {
            return Context.Receipts.SingleOrDefault(entity => entity.Id == id);
        }

        public static Receipt CreateReceipt(string receivedFrom, decimal receiptBalance)
        {
            Receipt receipt = new Receipt();

            receipt.ReceivedFromName = receivedFrom;
            receipt.ReceiptBalance = receiptBalance;

            Context.Receipts.AddObject(receipt);

            return receipt;
        }

        public static ReceiptPaymentAssoc CreateReceiptPaymentAssoc(Receipt receipt, Payment payment, decimal amount)
        {
            ReceiptPaymentAssoc receiptPaymentAssoc = new ReceiptPaymentAssoc();
            receiptPaymentAssoc.Payment = payment;
            receiptPaymentAssoc.Receipt = receipt;
            receiptPaymentAssoc.Amount = amount;

            return receiptPaymentAssoc;
        }
        public static ReceiptPaymentAssoc CreateReceiptPaymentAssoc(Receipt receipt, Payment payment)
        {
            ReceiptPaymentAssoc receiptPaymentAssoc = new ReceiptPaymentAssoc();
            receiptPaymentAssoc.Payment = payment;
            receiptPaymentAssoc.Receipt = receipt;
            receiptPaymentAssoc.Amount = receipt.ReceiptBalance.Value;

            return receiptPaymentAssoc;
        }
    }
}
