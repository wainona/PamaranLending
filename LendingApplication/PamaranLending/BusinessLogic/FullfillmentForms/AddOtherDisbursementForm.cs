using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class OtherItemsModel : BusinessObjectModel
    {
        public decimal Amount { get; set; }
        public string Particular { get; set; }
        public int OtherItemsId { get; set; }

        public OtherItemsModel(string particular, decimal amount)
        {
            this.OtherItemsId = -1;
            this.Amount = amount;
            this.Particular = particular;
        }

        public OtherItemsModel(OtherItemsModel otherItemsModel)
        {
            this.IsNew = false;
            this.OtherItemsId = otherItemsModel.OtherItemsId;
            this.Particular = otherItemsModel.Particular;
            this.Amount = otherItemsModel.Amount;
        }

    }
    public class OtherDisbursementForm : FullfillmentForm<FinancialEntities>
    {
        public DateTime now { get; set; }
        public DateTime TransactionDate {get; set;} 
        public int CustomerID { get; set; }
        public int DisbursedBy { get; set; }
        public decimal AmountToDisburse { get; set; }
        public decimal CashAmountToDisburse { get; set; }
        public decimal CheckAmountToDisburse { get; set; }
        public int CurrencyId { get; set; }
        public decimal DisId { get; set; }
        public string SignatureString { get; set; }

        private List<AddChequesModel> Cheques;
        private List<OtherItemsModel> OtherItems;
        public OtherDisbursementForm()
        {
            Cheques = new List<AddChequesModel>();
            OtherItems = new List<OtherItemsModel>();
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

        public IEnumerable<OtherItemsModel> AvailableOtherItems
        {
            get
            {
                return this.OtherItems.Where(model => model.ToBeDeleted == false);
            }
        }

         public void AddOtherItems(OtherItemsModel model)
         {
             if (this.OtherItems.Contains(model) == false)
                 this.OtherItems.Add(model);
         }
         public OtherItemsModel GetOtherItems(string randomKey)
         {
             return this.OtherItems.SingleOrDefault(entity => entity.RandomKey == randomKey);
         }
         public void RemoveOtherItems(string randomKey)
         {
             OtherItemsModel model = this.OtherItems.SingleOrDefault(entity => entity.RandomKey == randomKey);
             if (model != null)
                 RemoveOtherItems(model);
         }
         public void RemoveOtherItems(OtherItemsModel model)
         {
             if (this.OtherItems.Contains(model) == true)
             {
                 if (model.IsNew)
                     OtherItems.Remove(model);
                 else
                     model.MarkDeleted();
             }
         }

         private void SaveOtherItems(int paymentId)
         {
             foreach (OtherItemsModel item in OtherItems)
             {
                 if (item.IsNew)
                 {
                     var disbursementItem = DisbursementItem.Create(paymentId, item.Particular, item.Amount);
                     Context.DisbursementItems.AddObject(disbursementItem);
                 }
                 else if (item.ToBeDeleted)
                 {
                     DisbursementItem disbursementitem = DisbursementItem.GetById(item.OtherItemsId);
                     Context.DisbursementItems.DeleteObject(disbursementitem);
                 }
                 else if (item.IsEdited)
                 {
                     DisbursementItem.Update(item.OtherItemsId, paymentId, item.Particular, item.Amount);
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
            Payment payment = new Payment();
            payment.PaymentMethodType = PaymentMethodType.CashType;
            payment.ProcessedByPartyRoleId = DisbursedBy;
            payment.ProcessedToPartyRoleId = CustomerID;
            payment.PaymentType = PaymentType.Disbursement;
            payment.TransactionDate = TransactionDate;
            payment.EntryDate = now;
            payment.TotalAmount = AmountToDisburse;
            payment.SpecificPaymentType = SpecificPaymentType.OtherDisbursementType;
            Context.Payments.AddObject(payment);
            Context.SaveChanges();

            if (CashAmountToDisburse > 0)
                DisbursementFacade.SaveToDisburseCash(TransactionDate, now, payment, CustomerID, DisbursedBy, CashAmountToDisburse, SpecificPaymentType.OtherDisbursementType, DisbursementType.OtherLoanDisbursementType, string.Empty, CurrencyId);
            if (CheckAmountToDisburse > 0)
                DisbursementFacade.SaveToDisbursementCheques(TransactionDate, now, payment, Cheques, CustomerID, DisbursedBy, SpecificPaymentType.OtherDisbursementType, DisbursementType.OtherLoanDisbursementType, string.Empty, CurrencyId);

            Disbursement disbursement = new Disbursement();
            disbursement.Payment = payment;
            disbursement.DisbursementTypeId = DisbursementType.OtherLoanDisbursementType.Id;

            SaveOtherItems(payment.Id);
            DisId = payment.Id;

            var employeeRole = PartyRole.GetById(this.CustomerID);
            var employeeParty = employeeRole.Party;

            //form details
            var formDetail = FormDetail.Create(FormType.OtherDisbursementFormType.Id, payment, "Employee", employeeParty.NameV2, this.SignatureString);

            Context.FormDetails.AddObject(formDetail);
        }
        

    }
}
