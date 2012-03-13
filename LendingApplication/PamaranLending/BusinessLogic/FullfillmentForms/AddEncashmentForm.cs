using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class EncashmentForm : FullfillmentForm<FinancialEntities>
    {
        public int CurrencyId { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public int ProcessedToCustomerId { get; set; }
        public string CheckNumber { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public DateTime CheckDate { get; set; }
        public decimal CheckAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal CashAmountToDisburse { get; set; }
        public decimal CheckAmountToDisburse { get; set; }
        public decimal AmountToDisburse { get; set; }
        public int EmployeePartyRoleID { get; set; }
        public string DisbursedToName { get; set; }
        public string SignatureString { get; set; }
        private List<AddChequesModel> Cheques;

        public EncashmentForm()
        {
            CurrencyId = -1;
            Cheques = new List<AddChequesModel>();
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

        public override void Retrieve(int id)
        {

        }

        public override void PrepareForSave()
        {
            var now = DateTime.Now;
            PaymentMethodType PaymentMethodType = Context.PaymentMethodTypes.FirstOrDefault(e => e.Id == PaymentMethodTypeId);

            //First Payment Record (Cheque)
            Payment payment = new Payment();
            payment.ProcessedByPartyRoleId = EmployeePartyRoleID;
            payment.ProcessedToPartyRoleId = ProcessedToCustomerId;
            payment.PaymentMethodType = PaymentMethodType;
            payment.PaymentType = PaymentType.Receipt;
            payment.SpecificPaymentType = SpecificPaymentType.CheckForEncashmentType;
            payment.TransactionDate = TransactionDate;
            payment.EntryDate = now;
            payment.TotalAmount = CheckAmount;
            payment.PaymentReferenceNumber = CheckNumber;

            //Receipt Table
            Receipt receipt = new Receipt();
            receipt.ReceiptBalance = 0;

            ReceiptStatu.Create(receipt, now, ReceiptStatusType.OpenReceiptStatusType, false);
            Receipt.ChangeReceiptStatusFrom(receipt, now, 0, ReceiptStatusType.OpenReceiptStatusType);

            ReceiptPaymentAssoc receiptpaymentassoc = new ReceiptPaymentAssoc();
            receiptpaymentassoc.Payment = payment;
            receiptpaymentassoc.Receipt = receipt;

            //Cheque Table
            Cheque cheque = new Cheque();
            cheque.BankPartyRoleId = BankId;
            cheque.Payment = payment;
            cheque.CheckDate = CheckDate;

            ChequeStatu chequeStatus = new ChequeStatu();
            chequeStatus.Cheque = cheque;
            chequeStatus.ChequeStatusType = ChequeStatusType.ReceivedType;
            chequeStatus.TransitionDateTime = now;
            chequeStatus.IsActive = true;

            Currency.CreatePaymentCurrencyAssoc(payment, CurrencyId);


            //Second Payment Record
            Payment payment2 = new Payment();
            payment2.PaymentMethodType = PaymentMethodType.CashType;
            payment2.ProcessedByPartyRoleId = EmployeePartyRoleID;
            payment2.ProcessedToPartyRoleId = ProcessedToCustomerId;
            payment2.PaymentType = PaymentType.Disbursement;
            payment2.SpecificPaymentType = SpecificPaymentType.EncashmentType;
            payment2.TransactionDate = TransactionDate;
            payment2.EntryDate = now;
            payment2.TotalAmount = AmountToDisburse;
            payment2.PaymentReferenceNumber = CheckNumber;

            Currency.CreatePaymentCurrencyAssoc(payment2, CurrencyId);

            ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
            receiptpaymentassoc2.Payment = payment2;
            receiptpaymentassoc2.Receipt = receipt;

            if (CashAmountToDisburse > 0)
                DisbursementFacade.SaveToDisburseCash(TransactionDate, now, payment2, CheckNumber, ProcessedToCustomerId, EmployeePartyRoleID, CashAmountToDisburse, SpecificPaymentType.EncashmentType,DisbursementType.EncashmentType,DisbursedToName,CurrencyId);
            if (CheckAmountToDisburse > 0)
                DisbursementFacade.SaveToDisbursementCheques(TransactionDate, now, payment2, Cheques, ProcessedToCustomerId, EmployeePartyRoleID, SpecificPaymentType.EncashmentType, DisbursementType.EncashmentType, DisbursedToName,CurrencyId);

            Disbursement disbursement = new Disbursement();
            disbursement.Payment = payment2;
            disbursement.DisbursementType = DisbursementType.EncashmentType;
            if(string.IsNullOrEmpty(DisbursedToName) == false)
             disbursement.DisbursedToName = DisbursedToName;
            
            Encashment encashment = new Encashment();
            encashment.Disbursement = disbursement;
            encashment.Surcharge = 0;



            var customerRole = PartyRole.GetById(this.ProcessedToCustomerId);
            var customerParty = customerRole.Party;

            var formDetail = FormDetail.Create(FormType.EncashmentFormType.Id, payment2, "Customer", customerParty.NameV2, this.SignatureString);

            Context.FormDetails.AddObject(formDetail);

        }
      
    }

}
