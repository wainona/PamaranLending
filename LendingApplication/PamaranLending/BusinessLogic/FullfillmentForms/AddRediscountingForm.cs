using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class RediscountingForm : FullfillmentForm<FinancialEntities>
    {
        public int CurrencyId { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string DisbursedToName { get; set; }
        public int ProcessedToCustomerId { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public decimal CheckAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int SurchargeRate { get; set; }
        public decimal SurchargeFee { get; set; }
        public decimal AmountToDisburse { get; set; }
        public decimal CashAmountToDisburse { get; set; }
        public decimal CheckAmountToDisburse { get; set; }
        public int EmployeePartyRoleID { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public string Signature { get; set; }
        private List<AddChequesModel> Cheques;

       public RediscountingForm()
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


        public override void Retrieve(int id)
        {

        }

        public override void PrepareForSave()
        {
            var now = DateTime.Now;
            PaymentMethodType PaymentMethodType = Context.PaymentMethodTypes.FirstOrDefault(e => e.Id == PaymentMethodTypeId);

            //First Payment Record (Cheque)
            Payment payment = new Payment();
            payment.PaymentMethodType = PaymentMethodType;
            payment.ProcessedByPartyRoleId = EmployeePartyRoleID;
            payment.ProcessedToPartyRoleId = ProcessedToCustomerId;
            payment.PaymentMethodType = PaymentMethodType;
            payment.PaymentType = PaymentType.Receipt;
            payment.SpecificPaymentType = SpecificPaymentType.CheckForRediscountingType;
            payment.TransactionDate = TransactionDate;
            payment.EntryDate = now;
            payment.TotalAmount = CheckAmount;
            payment.PaymentReferenceNumber = CheckNumber;

       
            Currency.CreatePaymentCurrencyAssoc(payment, CurrencyId);

            //Receipt Table
            Receipt receipt = new Receipt();
            receipt.ReceiptBalance = 0;

            ReceiptStatu.Create(receipt, now, ReceiptStatusType.OpenReceiptStatusType, false);
            Receipt.ChangeReceiptStatusFrom(receipt, now, 0, ReceiptStatusType.OpenReceiptStatusType);

            ReceiptPaymentAssoc receiptpaymentassoc = new ReceiptPaymentAssoc();
            receiptpaymentassoc.Payment = payment;
            receiptpaymentassoc.Receipt = receipt;
            Context.ReceiptPaymentAssocs.AddObject(receiptpaymentassoc);
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


            //Second Payment Record
            Payment payment2 = new Payment();
            payment2.PaymentMethodType = PaymentMethodType.CashType;
            payment2.ProcessedByPartyRoleId = EmployeePartyRoleID;
            payment2.ProcessedToPartyRoleId = ProcessedToCustomerId;
            payment2.PaymentType = PaymentType.Disbursement;
            payment2.SpecificPaymentType = SpecificPaymentType.RediscountingType;
            payment2.TransactionDate = TransactionDate;
            payment2.EntryDate = now;
            payment2.TotalAmount = AmountToDisburse;
            payment2.PaymentReferenceNumber = CheckNumber;

            Currency.CreatePaymentCurrencyAssoc(payment2, CurrencyId);

            var formDetail = FormDetail.Create(FormType.RediscountingFormType.Id, payment2, "Disbursed To", DisbursedToName, Signature);
            Context.FormDetails.AddObject(formDetail);

            ReceiptPaymentAssoc receiptpaymentassoc2 = new ReceiptPaymentAssoc();
            receiptpaymentassoc2.Payment = payment2;
            receiptpaymentassoc2.Receipt = receipt;

            Context.ReceiptPaymentAssocs.AddObject(receiptpaymentassoc2);
          
            var cheques = AvailableCheques.ToList();
            if (CashAmountToDisburse > 0)
                DisbursementFacade.SaveToDisburseCash(TransactionDate, now, payment2, CheckNumber, ProcessedToCustomerId, EmployeePartyRoleID, CashAmountToDisburse, SpecificPaymentType.RediscountingType,DisbursementType.RediscountingType,DisbursedToName,CurrencyId);
            if (CheckAmountToDisburse > 0)
                DisbursementFacade.SaveToDisbursementCheques(TransactionDate, now, payment2, cheques, ProcessedToCustomerId, EmployeePartyRoleID, SpecificPaymentType.RediscountingType, DisbursementType.RediscountingType, DisbursedToName,CurrencyId);

            Disbursement disbursement = new Disbursement();
            disbursement.Payment = payment2;
            disbursement.DisbursementType = DisbursementType.RediscountingType;
            if (string.IsNullOrEmpty(DisbursedToName) == false)
                disbursement.DisbursedToName = DisbursedToName;

            Encashment encashment = new Encashment();
            encashment.Disbursement = disbursement;
            encashment.Surcharge = SurchargeFee;


            //Third Payment
            Payment payment3 = new Payment();
            payment3.PaymentMethodType = PaymentMethodType.CashType;
            payment3.ProcessedByPartyRoleId = EmployeePartyRoleID;
            payment3.ProcessedToPartyRoleId = ProcessedToCustomerId;
            payment3.PaymentType = PaymentType.FeePayment;
            payment3.SpecificPaymentType = SpecificPaymentType.FeePaymentType;
            payment3.TransactionDate = TransactionDate;
            payment3.EntryDate = now;
            payment3.TotalAmount = SurchargeFee;
            payment3.PaymentReferenceNumber = CheckNumber;

            Currency.CreatePaymentCurrencyAssoc(payment3, CurrencyId);


            ReceiptPaymentAssoc receiptpaymentassoc3 = new ReceiptPaymentAssoc();
            receiptpaymentassoc3.Payment = payment3;
            receiptpaymentassoc3.Receipt = receipt;
         
            FeePayment feePayment = new FeePayment();
            feePayment.Payment = payment3;
            feePayment.Particular = "Surcharge Fee";
            feePayment.FeeAmount = payment3.TotalAmount;

        }
    }
}
