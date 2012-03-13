using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class ForeignExchangeForm : FullfillmentForm<FinancialEntities>
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal AmountReleased { get; set; }
        public int CurrencyReceivedId { get; set; }
        public int CurrencyReleasedId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ExchangeRateTypeId { get; set; }
        public decimal CashAmount { get; set; }
        public bool IsSpot { get; set; }
        //public int ReceiptID { get; set; }
        private List<ForExDenominationModel> ForExDenominations_Received;
        private List<ForExDenominationModel> ForExDenominations_Released;
        private List<ForExChequeModel> Checks;

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

        public ForeignExchangeForm()
        {
            ForExDenominations_Received = new List<ForExDenominationModel>();
            ForExDenominations_Released = new List<ForExDenominationModel>();
            Checks = new List<ForExChequeModel>();
        }

        public void AddCheck(ForExChequeModel model)
        {
            if (this.Checks.Contains(model))
                return;
            this.Checks.Add(model);
        }
        public void RemoveCheck(ForExChequeModel model)
        {
            if (this.Checks.Contains(model) == true)
            {
                if (model.IsNew)
                    Checks.Remove(model);
                else
                    model.MarkDeleted();
            }
        }
        public void RemoveCheck(string randomKey)
        {
            ForExChequeModel model = this.Checks.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCheck(model);
        }
        public IEnumerable<ForExChequeModel> AvailableChecks
        {
            get
            {
                return this.Checks.Where(model => model.ToBeDeleted == false);
            }
        }

        public void AddDenominationReceived(ForExDenominationModel model)
        {
            if (this.ForExDenominations_Received.Contains(model))
                return;
            this.ForExDenominations_Received.Add(model);
        }
        public void RemoveDenominationReceived(ForExDenominationModel model)
        {
            if (this.ForExDenominations_Received.Contains(model) == true)
            {
                if (model.IsNew)
                    ForExDenominations_Received.Remove(model);
                else
                    model.MarkDeleted();
            }
        }
        public void RemoveDenominationReceived(string randomKey)
        {
            ForExDenominationModel model = this.ForExDenominations_Received.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveDenominationReceived(model);
        }
        public IEnumerable<ForExDenominationModel> AvailableDenominationsReceived
        {
            get
            {
                return this.ForExDenominations_Received.Where(model => model.ToBeDeleted == false);
            }
        }

        public void AddDenominationReleased(ForExDenominationModel model)
        {
            if (this.ForExDenominations_Released.Contains(model))
                return;
            this.ForExDenominations_Released.Add(model);
        }
        public void RemoveDenominationReleased(ForExDenominationModel model)
        {
            if (this.ForExDenominations_Released.Contains(model) == true)
            {
                if (model.IsNew)
                    ForExDenominations_Released.Remove(model);
                else
                    model.MarkDeleted();
            }
        }
        public void RemoveDenominationReleased(string randomKey)
        {
            ForExDenominationModel model = this.ForExDenominations_Released.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveDenominationReleased(model);
        }
        public IEnumerable<ForExDenominationModel> AvailableDenominationsReleased
        {
            get
            {
                return this.ForExDenominations_Released.Where(model => model.ToBeDeleted == false);
            }
        }

        public override void Retrieve(int id)
        {

        }

        public override void PrepareForSave()
        {
            var now = DateTime.Now;
            var employeeId = UserAccount.GetAssociatedEmployee(this.UserId).PartyRoleId;
            
            //Foreign Exchange record
            ForeignExchange newForeignExchange = new ForeignExchange();
            newForeignExchange.ProcessedToPartyRoleId = this.CustomerId;
            newForeignExchange.ProcessedByPartyRoleId = employeeId;
            newForeignExchange.Rate = this.Rate;
            newForeignExchange.AmountReceived = this.AmountReceived;
            newForeignExchange.ReceivedCurrencyId = this.CurrencyReceivedId;
            newForeignExchange.AmountReleased = this.AmountReleased;
            newForeignExchange.ReleasedCurrencyId = this.CurrencyReleasedId;
            newForeignExchange.EntryDate = now;
            newForeignExchange.TransactionDate = this.TransactionDate;//Add object
            newForeignExchange.ExchangeRateId = this.ExchangeRateTypeId;
            newForeignExchange.IsSpot = this.IsSpot;
            ObjectContext.ForeignExchanges.AddObject(newForeignExchange);

            //ForExDetail Original Amount
            ForExDetail parentForExDetail = new ForExDetail();
            parentForExDetail.Amount = this.AmountReceived;
            parentForExDetail.CurrencyId = this.CurrencyReceivedId;
            parentForExDetail.PaymentMethodTypeId = PaymentMethodType.CashType.Id;
            ObjectContext.ForExDetails.AddObject(parentForExDetail);
            //ObjectContext.SaveChanges();

            ForeignExchangeDetailAssoc forExDetailsAssoc = new ForeignExchangeDetailAssoc();
            forExDetailsAssoc.ForeignExchange = newForeignExchange;
            forExDetailsAssoc.ForExDetail = parentForExDetail;
            ObjectContext.ForeignExchangeDetailAssocs.AddObject(forExDetailsAssoc);

            if (this.ForExDenominations_Received.Count > 0)
            {
                foreach (var item in this.ForExDenominations_Received)
                {
                    Denomination denom = new Denomination();
                    denom.BillAmount = item.BillAmount;
                    denom.SerialNumber = item.SerialNumber;
                    denom.ForExDetail = parentForExDetail;

                    ObjectContext.Denominations.AddObject(denom);
                }
            }

            //Cash ForExDetail
            if (this.CashAmount != 0)
            {
                ForExDetail cashForExDetail = new ForExDetail();
                cashForExDetail.Amount = this.CashAmount;
                cashForExDetail.CurrencyId = this.CurrencyReleasedId;
                cashForExDetail.PaymentMethodTypeId = PaymentMethodType.CashType.Id;
                cashForExDetail.ForExDetail2 = parentForExDetail;
                ObjectContext.ForExDetails.AddObject(cashForExDetail);

                if (this.ForExDenominations_Released.Count > 0)
                {
                    foreach (var item in this.ForExDenominations_Released)
                    {
                        Denomination denom = new Denomination();
                        denom.BillAmount = item.BillAmount;
                        denom.SerialNumber = item.SerialNumber;
                        denom.ForExDetail = cashForExDetail;

                        ObjectContext.Denominations.AddObject(denom);
                    }
                }
            }

            if (this.Checks.Count > 0)
            {
                foreach (var item in this.Checks)
                {
                    //Check ForExDetail
                    ForExDetail checkForExDetail = new ForExDetail();
                    checkForExDetail.Amount = item.Amount;
                    checkForExDetail.CurrencyId = this.CurrencyReleasedId;
                    checkForExDetail.PaymentMethodTypeId = PaymentMethodType.PersonalCheckType.Id;
                    checkForExDetail.ForExDetail2 = parentForExDetail;
                    ObjectContext.ForExDetails.AddObject(checkForExDetail);

                    ForExCheque newForExCheque = new ForExCheque();
                    newForExCheque.ForExDetail = checkForExDetail;
                    newForExCheque.BankPartyRoleId = item.BankPartyRoleId;
                    newForExCheque.CheckDate = item.CheckDate;
                    newForExCheque.CheckNumber = item.CheckNumber;
                    ObjectContext.ForExCheques.AddObject(newForExCheque);
                }
            }
        }

        
        //    public override void PrepareForSave(ForExDetailModel forExDetailModel)
        //    {
        //    }
    }

    public class ForExDenominationModel : BusinessObjectModel
    {
        public int BillAmountId { get; set; }
        public string Type 
        {
            get
            {
                if (this.SerialNumber == null)
                {
                    return "Coins";
                }
                return "Bill";
            }
        }
        public decimal BillAmount { get; set; }
        public string SerialNumber { get; set; }

        public ForExDenominationModel()
        {
            this.IsNew = true;
        }

        public ForExDenominationModel(Denomination denomination)
        {
            this.BillAmount = denomination.BillAmount;
            this.SerialNumber = denomination.SerialNumber;
        }

    }

    public class ForExChequeModel : BusinessObjectModel
    {
        public decimal Amount { get; set; }
        public int BankPartyRoleId { get; set; }
        public string BankName { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string _CheckDate
        {
            get
            {
                return this.CheckDate.ToString("MMM dd, yyyy");
            }
        }
    }
}
