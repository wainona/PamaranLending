using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class ChangeForm : FullfillmentForm<FinancialEntities>
    {
        public int CurrencyId { get; set; }
        public int CustomerID { get; set; }
        public int UserID { get; set; }
        public string DisbursedToName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal CashAmountToDisburse { get; set; }
        public decimal CheckAmountToDisburse { get; set; }
        public decimal AmountDisbursed { get; set; }
        public int ReceiptID { get; set; }
        public string SignatureString { get; set; }
        private List<AddChequesModel> Cheques;

       public ChangeForm()
        {
            Cheques = new List<AddChequesModel>();
            CurrencyId = -1;
        }

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
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }
        public override void Retrieve(int id)
        {

        }

        public override void PrepareForSave()
        {
            var now = DateTime.Now;
            var today = DateTime.Today;
            var receipt = ObjectContext.Receipts.FirstOrDefault( e => e.Id == ReceiptID);
            var receiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.IsActive == true && entity.ReceiptId == receipt.Id);
            
            Payment newPayment = new Payment();
            newPayment.PaymentMethodTypeId = PaymentMethodType.CashType.Id;
            newPayment.ProcessedByPartyRoleId = UserID;
            newPayment.ProcessedToPartyRoleId = CustomerID;
            newPayment.TransactionDate = TransactionDate;
            newPayment.EntryDate = now;
            newPayment.TotalAmount = AmountDisbursed;
            newPayment.PaymentTypeId = PaymentType.Disbursement.Id;
            newPayment.SpecificPaymentType = SpecificPaymentType.ChangeType;
            ObjectContext.Payments.AddObject(newPayment);

            ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
            receiptpaymentassoc2.Payment = newPayment;
            receiptpaymentassoc2.Receipt = receipt;

             if (CashAmountToDisburse > 0)
                 DisbursementFacade.SaveToDisburseCash(TransactionDate, now, newPayment, string.Empty, CustomerID, UserID, CashAmountToDisburse, SpecificPaymentType.ChangeType,DisbursementType.ChangeType,DisbursedToName,CurrencyId);
            if (CheckAmountToDisburse > 0)
                DisbursementFacade.SaveToDisbursementCheques(TransactionDate, now, newPayment, Cheques, CustomerID, UserID, SpecificPaymentType.ChangeType, DisbursementType.ChangeType, DisbursedToName,CurrencyId);
           
            Disbursement newDisbursement = new Disbursement();
            newDisbursement.Payment = newPayment;
            newDisbursement.DisbursementTypeId = DisbursementType.ChangeType.Id;
            if (string.IsNullOrEmpty(DisbursedToName) == false)
                newDisbursement.DisbursedToName = DisbursedToName;
            ObjectContext.Disbursements.AddObject(newDisbursement);

            receipt.ReceiptBalance -=AmountDisbursed;
            var balance = (decimal)receipt.ReceiptBalance;
            if (receipt.ReceiptBalance == 0)
            {
                
                Receipt.ChangeReceiptStatusFrom(receipt, now.Date, balance, receipt.CurrentStatus.ReceiptStatusType);
                receipt.CurrentStatus.IsActive = false;
            }

            var customerRole = PartyRole.GetById(this.CustomerID);
            var customerParty = customerRole.Party;

            //form details
            var formDetail = FormDetail.Create(FormType.ChangeFormType.Id, newPayment, "Customer", customerParty.NameV2, this.SignatureString);

            Context.FormDetails.AddObject(formDetail);
        }


    }
}
