using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class COVTransModel
    {
        public int COVTransId { get; set; }
        public DateTime TransactDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string _TransactDate
        {
            get
            {
                return this.TransactDate.ToString("yyyy-MM-dd");
            }
        }
        public string _EntryDate { 
            get
            {
                return this.EntryDate.ToString("yyyy-MM-dd hh:mm:ss tt");
            }
        }
        public decimal Amount { get; set; }
        public string TransType { get; set; }
        public string ProcessedBy { get; set; }
        public string Remarks { get; set; }
        public COVTransModel(COVTransaction trans)
        {
            this.Amount = trans.Amount;
            this.ProcessedBy = Person.GetPersonFullName(trans.ProcessedByPartyRoleId);
            this.TransType = CashOnVault.GetCOVTransType(trans.COVTransTypeId).Name;
            this.Remarks = trans.Remarks;
            this.TransactDate = trans.TransactionDate;
            this.COVTransId = trans.Id;
            this.EntryDate = trans.EntryDate;
        }
    }
    public partial class COVHistoryModel
    {
        public DateTime DateClosed { get; set; }
        public string _DateClosed
        {
            get
            {
                return this.DateClosed.ToString("yyyy-MM-dd hh:mm:ss tt");
            }
        }
        public decimal Amount { get; set; }
        public string ClosedBy { get; set; }
        public int Id { get; set; }
        public COVHistoryModel(CashOnVault vault)
        {
            this.Amount = vault.Amount;
            this.ClosedBy = Person.GetPersonFullName(vault.ClosedByPartyRoleId);
            this.DateClosed = vault.TransitionDateTime;
            this.Id = vault.Id;
        }
    }
    public partial class CashOnVault
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
        public static IEnumerable<COVHistoryModel> GetCOVHistory(int currencyId)
        {
            var query = from c in Context.CashOnVaults
                        where c.CurrencyId == currencyId
                        orderby c.TransitionDateTime
                        select c;

            List<COVHistoryModel> covHistory = new List<COVHistoryModel>();
            foreach (var item in query)
            {
                covHistory.Add(new COVHistoryModel(item));
            }
            return covHistory;
        }
        public static IEnumerable<COVTransModel> GetCOVTransactions(int currencyId)
        {
            List<COVTransModel> covTransactions = new List<COVTransModel>();

            var query = from c in Context.COVTransactions
                           where c.CurrencyId == currencyId
                           orderby c.TransactionDate
                           select c;
            foreach (var item in query)
            {
                covTransactions.Add(new COVTransModel(item));
            }
            return covTransactions;
        }
        public static CashOnVault GetActiveVault(int currencyId)
        {
            return Context.CashOnVaults.FirstOrDefault(e => e.CurrencyId == currencyId && e.IsActive == true);
        }
        public static CashOnVault CreateCOV(int curId,decimal amount, int closedBy, DateTime date, bool isActive)
        {
            var activeCov = GetActiveVault(curId);
            if (activeCov != null)
                activeCov.IsActive = false;

            CashOnVault cov = new CashOnVault();
            cov.CurrencyId = curId;
            cov.Amount = amount;
            cov.ClosedByPartyRoleId = closedBy;
            cov.TransitionDateTime = date;
            cov.IsActive = isActive;

            Context.CashOnVaults.AddObject(cov);
            return cov;
        }

        public static COVTransaction CreateCOVTrans(int processedBy, decimal amount, int currId, int covTransTypeId, string remarks, DateTime transactDate)
        {
            COVTransaction covTrans = new COVTransaction();
            covTrans.ProcessedByPartyRoleId = processedBy;
            covTrans.Amount = amount;
            covTrans.CurrencyId = currId;
            covTrans.COVTransTypeId = covTransTypeId;
            covTrans.Remarks = remarks;
            covTrans.TransactionDate = transactDate;
            covTrans.EntryDate = DateTime.Now;

            Context.COVTransactions.AddObject(covTrans);
            return covTrans;
        }
        public static COVTransactionType GetCOVTransType(int transtypeId)
        {
            return Context.COVTransactionTypes.FirstOrDefault(e => e.Id == transtypeId);
        }
        public static IQueryable<COVTransactionType> GetCOVTransTypes()
        {
            return Context.COVTransactionTypes;
        }
    }
}
