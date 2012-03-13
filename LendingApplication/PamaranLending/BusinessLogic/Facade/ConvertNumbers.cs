using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class ConvertNumbers
    {
            private static string[] onesMapping =
                new string[] {
            "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
            "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
            };

            private static string[] tensMapping =
                new string[] {
            "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
            };

            private static string[] groupMapping =
                new string[] {
            "Hundred", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillian",
            "Septillion", "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion",
            "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septendecillion", "Octodecillion", "Novemdecillion",
            "Vigintillion", "Unvigintillion", "Duovigintillion", "10^72", "10^75", "10^78", "10^81", "10^84", "10^87",
            "Vigintinonillion", "10^93", "10^96", "Duotrigintillion", "Trestrigintillion"
            };

            // NOTE: 10^303 is approaching the limits of double, as ~1.7e308 is where we are going
            // 10^303 is a centillion and a 10^309 is a duocentillion
            
            private static string EnglishFromNumber(int number)
            {
                return EnglishFromNumber((long)number);
            }

            private static string EnglishFromNumber(long number)
            {
                return EnglishFromNumber((double)number);
            }

            public static String EnglishFromNumber(double number)
            {
                string sign = null;
                if (number < 0)
                {
                    sign = "Negative";
                    number = Math.Abs(number);
                }

                int decimalDigits = 0;
                Console.WriteLine(number);
                while (number < 1 || (number - Math.Floor(number) > 1e-10))
                {
                    number *= 10;
                    decimalDigits++;
                }
                Console.WriteLine("Total Decimal Digits: {0}", decimalDigits);

                string decimalString = null;
                while (decimalDigits-- > 0)
                {
                    int digit = (int)(number % 10); number /= 10;
                    decimalString = onesMapping[digit] + " " + decimalString;
                }

                string retVal = null;
                int group = 0;
                if (number < 1)
                {
                    retVal = onesMapping[0];
                }
                else
                {
                    while (number >= 1)
                    {
                        int numberToProcess = (number >= 1e16) ? 0 : (int)(number % 1000);
                        number = number / 1000;

                        string groupDescription = ProcessGroup(numberToProcess);
                        if (groupDescription != null)
                        {
                            if (group > 0)
                            {
                                retVal = groupMapping[group] + " " + retVal;
                            }
                            retVal = groupDescription + " " + retVal;
                        }

                        group++;
                    }
                }

                return String.Format("{0}{4}{1}{3}{2}",
                    sign,
                    retVal,
                    decimalString,
                    (decimalString != null) ? " and " : "",
                    (sign != null) ? " " : "");
            }

            private static string ProcessGroup(int number)
            {
                int tens = number % 100;
                int hundreds = number / 100;

                string retVal = null;
                if (hundreds > 0)
                {
                    retVal = onesMapping[hundreds] + " " + groupMapping[0];
                }
                if (tens > 0)
                {
                    if (tens < 20)
                    {
                        retVal += ((retVal != null) ? " " : "") + onesMapping[tens];
                    }
                    else
                    {
                        int ones = tens % 10;
                        tens = (tens / 10) - 2; // 20's offset

                        retVal += ((retVal != null) ? " " : "") + tensMapping[tens];

                        if (ones > 0)
                        {
                            retVal += ((retVal != null) ? " " : "") + onesMapping[ones];
                        }
                    }
                }

                return retVal;
            }
        }
}
