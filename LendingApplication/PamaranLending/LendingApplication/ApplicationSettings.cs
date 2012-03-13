using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingApplication
{
    public class ApplicationSettings
    {
        public static int[] DisabledDays
        {
            get
            {
                return new int[] {  };
            }
        }

        public static string DefaultPostalCode
        {
            get
            {
                return "7016";
            }
        }

        public static string DefaultCity
        {
            get
            {
                return "Pagadian";
            }
        }

        public static string DefaultAreaCode
        {
            get
            {
                return "62";
            }
        }

        public static decimal DefaultRestructuredInterestRate
        {
            get
            {
                return 5.00M;
            }
        }
    }
}