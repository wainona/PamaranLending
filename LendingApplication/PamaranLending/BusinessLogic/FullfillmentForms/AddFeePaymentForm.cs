using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{

    public class FeeItemsModel : BusinessObjectModel
    {
        public decimal Amount { get; set; }
        public string Particular { get; set; }
        public int OtherItemsId { get; set; }

        public FeeItemsModel(string particular, decimal amount)
        {
            this.OtherItemsId = -1;
            this.Amount = amount;
            this.Particular = particular;
        }

        public FeeItemsModel(FeeItemsModel feeItemsModel)
        {
            this.IsNew = false;
            this.OtherItemsId = feeItemsModel.OtherItemsId;
            this.Particular = feeItemsModel.Particular;
            this.Amount = feeItemsModel.Amount;
        }

    }
    public class ATMPaymentModel : BusinessObjectModel
    {
     
        public decimal Amount { get; set; }
        public string ATMReferenceNumber { get; set; }
    }
    public class FeePaymentForm : FullfillmentForm<FinancialEntities>
    {
        public DateTime TransactionDate {get; set;} //payment transac date
        public string ReceivedFrom { get; set; } //payment processed_to_name, customer
        public int CustomerID { get; set; }
        public int ReceivedBy { get; set; } //processed by user
        public decimal TotalAmountReceived { get; set;}
        public decimal TotalAmountPaid { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CheckAmount { get; set; }
        public decimal ATMAmount { get; set; }
        public string Remarks { get; set; }
        private List<AddChequesModel> Cheques;
        private List<FeeItemsModel> FeeItems;
        private List<ATMPaymentModel> ATMPayments;

        public void AddCheques(AddChequesModel model)
        {
            if (this.Cheques.Contains(model))
                return;
            this.Cheques.Add(model);

        }

        public void RemoveCheques(AddChequesModel model)
        {
            if (this.Cheques.Contains(model) == true)
            {
                if (model.IsNew)
                    Cheques.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void RemoveCheques(string randomKey)
        {
            AddChequesModel model = this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCheques(model);
        }

        public IEnumerable<AddChequesModel> AvailableCheques
        {
            get
            {
                return this.Cheques.Where(model => model.ToBeDeleted == false);
            }
        }

        public void AddATM(ATMPaymentModel model)
        {
            if (this.ATMPayments.Contains(model))
                return;
            this.ATMPayments.Add(model);

        }

        public void RemoveATMs(ATMPaymentModel model)
        {
            if (this.ATMPayments.Contains(model) == true)
            {
                if (model.IsNew)
                    ATMPayments.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void RemoveATMs(string randomKey)
        {
            ATMPaymentModel model = this.ATMPayments.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveATMs(model);
        }
        public ATMPaymentModel GetATMs(string randomKey)
        {
            return this.ATMPayments.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }
        public IEnumerable<ATMPaymentModel> AvailableATMs
        {
            get
            {
                return this.ATMPayments.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<FeeItemsModel> AvailableOtherItems
        {
            get
            {
                return this.FeeItems.Where(model => model.ToBeDeleted == false);
            }
        }
    
        public FeePaymentForm()
        {
            FeeItems = new List<FeeItemsModel>();
               Cheques = new List<AddChequesModel>();
               ATMPayments = new List<ATMPaymentModel>();
         }

        public void AddOtherItems(FeeItemsModel model)
         {
             if (this.FeeItems.Contains(model) == false)
                 this.FeeItems.Add(model);
         }
        public FeeItemsModel GetOtherItems(string randomKey)
         {
             return this.FeeItems.SingleOrDefault(entity => entity.RandomKey == randomKey);
         }
         public void RemoveOtherItems(string randomKey)
         {
             FeeItemsModel model = this.FeeItems.SingleOrDefault(entity => entity.RandomKey == randomKey);
             if (model != null)
                 RemoveOtherItems(model);
         }
         public void RemoveOtherItems(FeeItemsModel model)
         {
             if (this.FeeItems.Contains(model) == true)
             {
                 if (model.IsNew)
                     FeeItems.Remove(model);
                 else
                     model.MarkDeleted();
             }
         }

         private void SaveOtherItems(int paymentId, PaymentMethodType paymentMethodType)
         {
             var today = DateTime.Today;
             foreach (FeeItemsModel item in FeeItems)
             {
                 if (item.IsNew)
                 {
                     //Fee Payment
                     var feeitem = FeePayment.Create(paymentId, item.Particular, item.Amount);
                     Context.FeePayments.AddObject(feeitem);

                     Context.SaveChanges();
                 }
                 else if (item.ToBeDeleted)
                 {
                     FeePayment feeitem = FeePayment.GetById(item.OtherItemsId);
                     Context.FeePayments.DeleteObject(feeitem);
                 }
                 else if (item.IsEdited)
                 {
                     FeePayment.Update(item.OtherItemsId, paymentId, item.Particular, item.Amount);
                 }
             }
         }
        public override void Retrieve(int id)
        {
        }

        public void InsertChequewithStatus(Payment payment, int bankid, DateTime checkDate, DateTime today)
        {
            Cheque cheque = new Cheque();
            cheque.Payment = payment;
            cheque.BankPartyRoleId = bankid;
            cheque.CheckDate = checkDate;

            ChequeStatu chequeStatus = new ChequeStatu();
            chequeStatus.Cheque = cheque;
            chequeStatus.ChequeStatusType = ChequeStatusType.ClearedType;
            chequeStatus.TransitionDateTime = today;
            chequeStatus.IsActive = true;

        }

        public override void PrepareForSave()
        {
            var today = DateTime.Now;
            PaymentMethodType paymentMethodType = PaymentMethodType.CashType;

            Payment payment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                            TotalAmountPaid, PaymentType.FeePayment, paymentMethodType,
                            SpecificPaymentType.FeePaymentType, string.Empty);
            Context.Payments.AddObject(payment);
            Context.SaveChanges();
            if (CheckAmount > 0)
                foreach (var cheque in Cheques) { 
                    CreateCheque(today, cheque, payment);
                  
                    }
            if (CashAmount > 0)
                CreateCash(today, payment);
            if (ATMAmount > 0)
                foreach (var atm in ATMPayments)
                {
                    CreateATM(today, atm,payment);
                }

            Context.SaveChanges();
            SaveOtherItems(payment.Id, paymentMethodType);
        }

        private void CreateCheque(DateTime today, AddChequesModel cheque, Payment parentPayment)
        {
            var checkamount = cheque.Amount;
            Payment newcheckpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                    cheque.Amount, PaymentType.Receipt, cheque.PaymentType,
                    SpecificPaymentType.FeePaymentType, cheque.CheckNumber);
            Context.Payments.AddObject(newcheckpayment);
            Cheque newcheque = Cheque.CreateCheque(newcheckpayment, cheque.BankPartyRoleId, cheque.CheckDate);
            Context.Cheques.AddObject(newcheque);
            ChequeStatu newchequestatu = ChequeStatu.CreateChequeStatus(newcheque, ChequeStatusType.ClearedType.Id, today, Remarks, true);
            Context.ChequeStatus.AddObject(newchequestatu);
            Receipt newreceipt = Receipt.CreateReceipt(Remarks, cheque.Amount);
            Context.Receipts.AddObject(newreceipt);
            ReceiptStatu newreceiptStatus = ReceiptStatu.Create(newreceipt, TransactionDate, ReceiptStatusType.OpenReceiptStatusType);
            Context.ReceiptStatus.AddObject(newreceiptStatus);
            ReceiptPaymentAssoc newassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newcheckpayment);
            Context.ReceiptPaymentAssocs.AddObject(newassoc);
            Context.SaveChanges();
            if (TotalAmountPaid > 0)
            {
                decimal amountApplied = 0;
                if (TotalAmountPaid >= checkamount)
                    amountApplied = checkamount;
                else amountApplied = TotalAmountPaid;

                Payment newpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                    amountApplied, PaymentType.FeePayment, cheque.PaymentType,
                    SpecificPaymentType.FeePaymentType, cheque.CheckNumber);
                newpayment.Payment2 = parentPayment;
                ReceiptPaymentAssoc receiptpaymentassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newpayment);
             
                newreceipt.ReceiptBalance = (checkamount - amountApplied);
                TotalAmountPaid -= amountApplied;
            
                if (newreceipt.ReceiptBalance == 0)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                else if (newreceipt.ReceiptBalance > 0 && newreceipt.ReceiptBalance < cheque.Amount)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;

                Context.Payments.AddObject(newpayment);
                Context.ReceiptPaymentAssocs.AddObject(receiptpaymentassoc);
                Context.SaveChanges();
            }
        
         
        }
        
        private void CreateCash(DateTime today,Payment parentPayment)
        {
            var cashamount = CashAmount;
            Payment newcashpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                   CashAmount, PaymentType.Receipt, PaymentMethodType.CashType,
                   SpecificPaymentType.FeePaymentType, string.Empty);
            Receipt newreceipt = Receipt.CreateReceipt(Remarks, CashAmount);
            ReceiptStatu newreceiptStatus = ReceiptStatu.Create(newreceipt, TransactionDate, ReceiptStatusType.OpenReceiptStatusType);
            ReceiptPaymentAssoc newassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newcashpayment);
            Context.Payments.AddObject(newcashpayment);
            Context.Receipts.AddObject(newreceipt);
            Context.ReceiptPaymentAssocs.AddObject(newassoc);
            Context.ReceiptStatus.AddObject(newreceiptStatus);

            if (TotalAmountPaid > 0)
            {
                decimal amountApplied = 0;
                if (TotalAmountPaid >= cashamount)
                    amountApplied = cashamount;
                else amountApplied = TotalAmountPaid;

                Payment newpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                    amountApplied, PaymentType.FeePayment, PaymentMethodType.CashType,
                    SpecificPaymentType.FeePaymentType, string.Empty);
                newpayment.Payment2 = parentPayment;
                ReceiptPaymentAssoc receiptpaymentassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newpayment);

                newreceipt.ReceiptBalance = (cashamount - amountApplied);
                TotalAmountPaid -= amountApplied;

                if (newreceipt.ReceiptBalance == 0)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                else if (newreceipt.ReceiptBalance > 0 && newreceipt.ReceiptBalance < CashAmount)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;

                Context.Payments.AddObject(newpayment);
                Context.ReceiptPaymentAssocs.AddObject(receiptpaymentassoc);
            }
          
        }
        private void CreateATM(DateTime today,ATMPaymentModel atm, Payment parentPayment)
        {
            var atmamount = atm.Amount;
            Payment newatmpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                   atm.Amount, PaymentType.Receipt, PaymentMethodType.ATMType,
                   SpecificPaymentType.FeePaymentType, atm.ATMReferenceNumber);
            Receipt newreceipt = Receipt.CreateReceipt(Remarks, atm.Amount);
            ReceiptStatu newreceiptStatus = ReceiptStatu.Create(newreceipt, TransactionDate, ReceiptStatusType.OpenReceiptStatusType);
            ReceiptPaymentAssoc newassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newatmpayment);
            Context.Payments.AddObject(newatmpayment);
            Context.Receipts.AddObject(newreceipt);
            Context.ReceiptPaymentAssocs.AddObject(newassoc);
            Context.ReceiptStatus.AddObject(newreceiptStatus);

            if (TotalAmountPaid > 0)
            {
                decimal amountApplied = 0;
                if (TotalAmountPaid >= atmamount)
                    amountApplied = atmamount;
                else amountApplied = TotalAmountPaid;

                Payment newpayment = Payment.CreatePayment(today, TransactionDate, CustomerID, ReceivedBy,
                    amountApplied, PaymentType.FeePayment, PaymentMethodType.ATMType,
                    SpecificPaymentType.FeePaymentType, atm.ATMReferenceNumber);
                newpayment.Payment2 = parentPayment;
                ReceiptPaymentAssoc receiptpaymentassoc = Receipt.CreateReceiptPaymentAssoc(newreceipt, newpayment);

                newreceipt.ReceiptBalance = (atmamount - amountApplied);
                TotalAmountPaid -= amountApplied;

                if (newreceipt.ReceiptBalance == 0)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.ClosedReceiptStatusType;
                else if (newreceipt.ReceiptBalance > 0 && newreceipt.ReceiptBalance < atm.Amount)
                    newreceiptStatus.ReceiptStatusType = ReceiptStatusType.AppliedReceiptStatusType;

                Context.Payments.AddObject(newpayment);
                Context.ReceiptPaymentAssocs.AddObject(receiptpaymentassoc);
            }

        }
    }
}
