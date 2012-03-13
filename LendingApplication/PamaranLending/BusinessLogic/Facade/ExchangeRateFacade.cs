using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ExchangeRate
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        internal static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static ExchangeRate CreateExchangeRate(int currencyFromId, int currencyToId, decimal rate, DateTime date, int exchangeRateTypeId)
        {
            ExchangeRate newExchangeRate = new ExchangeRate();
            newExchangeRate.CurrencyFromId = currencyFromId;
            newExchangeRate.CurrencyToId = currencyToId;
            newExchangeRate.Rate = rate;
            newExchangeRate.IsActive = true;
            newExchangeRate.Date = date.Date;
            newExchangeRate.ExchangeRateTypeId = exchangeRateTypeId;

            ObjectContext.ExchangeRates.AddObject(newExchangeRate);
            return newExchangeRate;
        }

        public static ExchangeRate GetById(int id)
        {
            var exchangeRate = ObjectContext.ExchangeRates.SingleOrDefault(entity => entity.Id == id);
            return exchangeRate;
        }

        public static bool ExchangeRateExists(int currencyFromId, int currencyToId, int exchangeRateTypeId)
        {
            var exchangeRate = ObjectContext.ExchangeRates.Where(entity => entity.CurrencyFromId == currencyFromId && entity.CurrencyToId == currencyToId && entity.ExchangeRateTypeId == exchangeRateTypeId);
            if (exchangeRate.Count() > 0)
                return true;

            return false;
        }

        public static bool CompareExRateIfEqual(ExchangeRate ExRate, int currencyFromId, int currencyToId, int exchangeRateTypeId)
        {
            if (ExRate.CurrencyFromId == currencyFromId && ExRate.CurrencyToId == currencyToId && ExRate.ExchangeRateTypeId == exchangeRateTypeId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
