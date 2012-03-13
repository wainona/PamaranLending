using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingApplication
{
    public class ProductFeatureEnums
    {
        public const string  Secured = "Secured";
        public const string Unsecured = "Unsecured";
        public const string MaximumLoanableAmount = "Maximum Loanable Amount";
        public const string MinimumLoanableAmount = "Minimum Loanable Amount";
        public const string MaximumLoanTerm ="Maximum Loan Term";
        public const string MinimumLoanTerm = "Minimum Loan Term";
        public const string StraightLineMethod = "Straight Line Method";
        public const string DiminishingBalanceMethod = "Diminishing Balance Method";
        public const string AddOnInterest = "Add-on Interest";
        public const string DiscountedInterest = "Discounted Interest";
        public const string MonthlyInterestRate = "Monthly Interest Rate";
        public const string AnnualInterestRate = "Annual Interest Rate";
        public const string MonthlyPastDueInterestRate = "Monthly Past Due Interest Rate";
        public const string AnnualPastDueInterestRate= "Annual Past Due  Interest Rate";
        public const string DocumentaryStampTax = "Documentary Stamp Tax";
        public const string ServiceCharge = "Service Charge";
        public const string LoanGuaranteeFee = "Loan Guarantee Fee";
    }
}