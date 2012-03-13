using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Runtime.Remoting.Contexts;

namespace BusinessLogic
{
    public partial class ExchangeRateType
    {
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

        private const string _Buying = "Buying";
        private const string _Selling = "Selling";
        private const string _Average = "Average";
        private const string _Spot = "Spot";

        public static ExchangeRateType Buying
        {
            get
            {
                var type = ObjectContext.ExchangeRateTypes.SingleOrDefault(entity => entity.Name == _Buying);
                InitialDatabaseValueChecker.ThrowIfNull<ExchangeRateType>(type);

                return type;
            }
        }

        public static ExchangeRateType Selling
        {
            get
            {
                var type = ObjectContext.ExchangeRateTypes.SingleOrDefault(entity => entity.Name == _Selling);
                InitialDatabaseValueChecker.ThrowIfNull<ExchangeRateType>(type);

                return type;
            }
        }

        public static ExchangeRateType Average
        {
            get
            {
                var type = ObjectContext.ExchangeRateTypes.SingleOrDefault(entity => entity.Name == _Average);
                InitialDatabaseValueChecker.ThrowIfNull<ExchangeRateType>(type);

                return type;
            }
        }

        public static ExchangeRateType Spot
        {
            get
            {
                var type = ObjectContext.ExchangeRateTypes.SingleOrDefault(entity => entity.Name == _Spot);
                InitialDatabaseValueChecker.ThrowIfNull<ExchangeRateType>(type);

                return type;
            }
        }
    }
}
