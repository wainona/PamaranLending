using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.Facade
{
    public partial class NumberFacade
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

        public static string DaySuffix(DateTime date)
        {
            var lastNumber = date.Day.ToString().Last();
            var len = date.Day.ToString().Length;
            string numSuffix = "";
            switch (lastNumber)
            {
                case '1':
                    if (len == 1)
                    {
                        numSuffix = "st";
                    }
                    else
                    {
                        if (date.Day.ToString().First() == '1')
                            numSuffix = "th";
                        else
                            numSuffix = "st";
                    }
                    break;

                case '2':
                    if (len == 1)
                    {
                        numSuffix = "nd";
                    }
                    else
                    {
                        if (date.Day.ToString().First() == '1')
                            numSuffix = "th";
                        else
                            numSuffix = "nd";
                    }
                    break;

                case '3':
                    if (len == 1)
                    {
                        numSuffix = "rd";
                    }
                    else
                    {
                        if (date.Day.ToString().First() == '1')
                            numSuffix = "th";
                        else
                            numSuffix = "rd";
                    }
                    break;

                default:
                    numSuffix = "th";
                    break;
            }

            return numSuffix;
        }

    }
}
