using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Currency
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public static Currency CreateCurrency(string name, string description)
        {
            Currency newCurrency = new Currency();
            newCurrency.Symbol = name;
            newCurrency.Description = description;

            ObjectContext.Currencies.AddObject(newCurrency);
            return newCurrency;
        }
        public static void CreatePaymentCurrencyAssoc(Payment payment, int CurrencyId)
        {
            if (CurrencyId != -1)
            {
                PaymentCurrencyAssoc assoc = new PaymentCurrencyAssoc();
                assoc.Payment = payment;
                assoc.CurrencyId = CurrencyId;
                ObjectContext.PaymentCurrencyAssocs.AddObject(assoc);
            }
        }
        public static Currency GetCurrencyByPaymentId(int paymentId)
        {
            var assoc = GetCurrencyPaymentAssocByPaymentId(paymentId);
            if (assoc != null) return assoc.Currency;
            else return null;
        }
        public static string CurrencySymbolByPaymentId(int paymentId)
        {
            var assoc = GetCurrencyPaymentAssocByPaymentId(paymentId);
            if (assoc != null) return assoc.Currency.Symbol;
            else return "PHP";
        }
        public static string CurrencyDescriptionByPaymentId(int paymentId) {
            var assoc = GetCurrencyPaymentAssocByPaymentId(paymentId);
            if (assoc != null) return " "+assoc.Currency.Description+"s";
            else return " Pesos";
        }
        public static PaymentCurrencyAssoc GetCurrencyPaymentAssocByPaymentId(int paymentId)
        {
            return ObjectContext.PaymentCurrencyAssocs.FirstOrDefault(e => e.PaymentId == paymentId);
        }

        public static Currency GetCurrencyById(int currencyId)
        {
            var currency = ObjectContext.Currencies.SingleOrDefault(e => e.Id == currencyId);
            return currency;
        }
        public static Currency GetCurrencyBySymbol(string symbol)
        {
            var currency = ObjectContext.Currencies.SingleOrDefault(e => e.Symbol == symbol);
            return currency;
        }
        public static List<CurrencyModel> GetCurrencies()
        {
            var query = from c in ObjectContext.Currencies
                        select new CurrencyModel()
                        {
                            Id = c.Id,
                            Name = c.Symbol,
                            Desciption = c.Description
                        };

            return query.ToList();
        }
        public static List<CurrencySymbolModel> GetCurrencySymbols()
        {
            var query = from c in ObjectContext.Currencies
                        select new CurrencySymbolModel()
                        {
                            Id = c.Id,
                            Symbol = c.Symbol
                        };

            return query.ToList();
        }

    }
    public class CurrencySymbolModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
    }
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public string NameDescription
        {
            get
            {
                return (this.Name + " - " + this.Desciption);
            }
        }
    }

}
