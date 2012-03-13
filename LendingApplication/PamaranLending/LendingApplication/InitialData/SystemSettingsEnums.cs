using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingApplication
{
    public class SystemSettingsEnums
    {
        public const string GracePeriod = "Grace Period";
        public const string InvoiceGenerationTiming = "Invoice Generation Timing";
        public const string DemandCollectionAfter = "Demand Collection After";
        public const string AgeLimitOfBorrower = "Age Limit of Borrower";
        public const string PercentageOfLoanAmountPaid = "Percentage of Loan Amount Paid";
        public const string CalculatePenalty = "Period in Calculating Penalty";
        public const string AllowDeleteOnLoansWithAge = "Years of Loans to be Deleted";
        public const string StraightLineLoan = "Allowable Number of Straight Line Loan Per Customer";
        public const string DiminishingBalanceLoan = "Allowable Number of Diminishing Balance Loan Per Customer";
        public const string ClerksMaximumHonorableAmount = "Clerk's Maximum Honorable Amount";
        public const string AdvanceChangeNoInterestStartDay = "Advance Change No Interest Start Day";
    }
}