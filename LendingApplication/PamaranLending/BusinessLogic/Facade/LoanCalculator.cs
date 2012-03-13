using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public class LoanCalculatorScheduleModel : BusinessObjectModel
    {
        public string ItemNumber { get; set; }
        public decimal PrincipalPayment { get; set; }
        public decimal InterestPayment { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal TotalLoanBalance { get; set; }
        public decimal InterestPaymentTotal { get; set; }
    }

    public class LoanCalculatorOptions
    {
        public string InterestComputationMode { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTerm { get; set; }
        public int LoanTermId { get; set; }
        public decimal InterestRate { get; set; }
        public string InterestRateDescription { get; set; }
        public int InterestRateDescriptionId { get; set; }
        public string PaymentMode { get; set; }
        public int PaymentModeId { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime LoanReleaseDate { get; set; }
        public bool IncludeSaturday { get; set; }
        public string TermOption { get; set; }

        public LoanCalculatorOptions()
        {
            IncludeSaturday = false;
        }
    }

    [Serializable]
    public class LoanCalculator : FullfillmentForm<FinancialEntities>
    {
        List<AmortizationScheduleModel> amortizationSchedule;
        public LoanCalculator()
        {
            amortizationSchedule = new List<AmortizationScheduleModel>();
        }

        public List<LoanCalculatorScheduleModel> CalculateLoan(LoanCalculatorOptions loanOptions)
        {
            List<LoanCalculatorScheduleModel> loanCalculatorSchedule = new List<LoanCalculatorScheduleModel>();

            var loanAmount = loanOptions.LoanAmount;
            var loanTerm = loanOptions.LoanTerm;
            var loanTermId = loanOptions.LoanTermId;
            var amortizationScheduleItemsCount = 0;
            var interestRate = loanOptions.InterestRate;
            var interestRateDescription = loanOptions.InterestRateDescription;
            var interestRateDescriptionId = loanOptions.InterestRateDescriptionId;
            var paymentModeId = loanOptions.PaymentModeId;
            var paymentMode = loanOptions.PaymentMode;
            var interestComputationMode = loanOptions.InterestComputationMode;
            var itemNumber = "";
            var releaseDate = loanOptions.LoanReleaseDate;
            var paymentStartDate = loanOptions.PaymentStartDate;
            var paymentDateDay = paymentStartDate.Day;
            var rdDay = releaseDate.Day;
            //var daysDiff = 0;

            //compute days diff
            if (paymentDateDay > 30 || (paymentDateDay > 27 && paymentDateDay <= 29) )
            {
                paymentDateDay = 30;
            }

            //Amortization Schedule Items Count
            amortizationScheduleItemsCount = (int)TimeUnitConversion.Convert(loanTermId, paymentModeId, loanTerm);
            var count = amortizationScheduleItemsCount;

            if (loanOptions.InterestRateDescription == ProductFeature.MonthlyInterestRateType.Name)
                interestRateDescriptionId = UnitOfMeasure.MonthsType.Id;
            else if (loanOptions.InterestRateDescription == ProductFeature.AnnualInterestRateType.Name)
                interestRateDescriptionId = UnitOfMeasure.YearsType.Id;

            //Interest Rate Multiplier
            var interestRateMultiplier = (TimeUnitConversion.GetMultiplier(interestRateDescriptionId, paymentModeId) / TimeUnitConversion.GetOffset(interestRateDescriptionId, paymentModeId));

            var newInterestRate = 0.00M;

            //New Interest Rate
            if (interestRateMultiplier >= 1)
                newInterestRate = ((decimal)interestRate / 100) / interestRateMultiplier;
            else
                newInterestRate = ((decimal)interestRate / 100) / interestRateMultiplier;

            var previousPrincipalBalance = loanAmount;
            var interestPaymentTotal = 0.00M;
            var previousTotalLoanBalance = 0.00M;


            //Loop for adding the amortization schedule items
            for (int i = 0; i < amortizationScheduleItemsCount; i++)
            {
                itemNumber = GetItemType(paymentMode) + " " + (i+1).ToString();
                //Amortization Schedule Items
                LoanCalculatorScheduleModel amortizationModel = new LoanCalculatorScheduleModel();
                amortizationModel.ItemNumber = itemNumber;

                amortizationModel.PrincipalPayment = loanAmount / amortizationScheduleItemsCount;
                

                if (i == (amortizationScheduleItemsCount - 1))
                {
                    decimal partialPrincipalTotal = Math.Floor(loanAmount / amortizationScheduleItemsCount) * (amortizationScheduleItemsCount - 1);
                    amortizationModel.PrincipalPayment = loanAmount - Math.Floor(partialPrincipalTotal);
                }

                amortizationModel.PrincipalPayment = Math.Floor(amortizationModel.PrincipalPayment);
                amortizationModel.PrincipalBalance = previousPrincipalBalance - amortizationModel.PrincipalPayment;

                //var interestAmountFirst = 0.00M;
                //var monthsDiff = 0;

                //If Interest Computation Mode is Straight-Line
                if (interestComputationMode == ProductFeature.StraightLineMethodType.Name)
                {
                    #region comment
                    //if (i == 0)
                    //{
                    //    var interest = loanAmount * newInterestRate;
                    //    interest = interest / 30;
                    //    if (releaseDate.Month != paymentStartDate.Month)
                    //    {
                    //        daysDiff = 31 - releaseDate.Day;
                    //        if (daysDiff <= SystemSetting.GracePeriod)
                    //        {
                    //            daysDiff = 0;
                    //        }

                    //        if (paymentStartDate.Day <= 30)
                    //            daysDiff += paymentStartDate.Day;
                    //        else if (paymentStartDate.Day == 31)
                    //            daysDiff += 31;
                    //        monthsDiff = paymentStartDate.Month - releaseDate.Month;

                    //        if (monthsDiff > 1)
                    //        {
                    //            daysDiff += ((monthsDiff - 1) * 30);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (paymentDateDay == 30)
                    //        {
                    //            daysDiff = (paymentDateDay) - releaseDate.Day;
                    //        }
                    //        else if(paymentDateDay == 28 || paymentDateDay == 29 || paymentDateDay == 31)
                    //        {
                    //            daysDiff = 31 - releaseDate.Day;
                    //        }
                    //    }
                    //    interest = (interest * daysDiff);
                    //    amortizationModel.InterestPayment = interest;
                    //    interestAmountFirst = amortizationModel.InterestPayment;
                    //}
                    //else
                    //{
                    #endregion

                    var interestPaymentTemp = loanAmount * newInterestRate;
                    amortizationModel.InterestPayment = AdjustInterestRateValue(interestPaymentTemp);
                    //}

                    //interestPaymentTotal = (loanAmount * newInterestRate) * (count - 1);
                    interestPaymentTotal = AdjustInterestRateValue(interestPaymentTemp) *(count);
                    //interestPaymentTotal += interestAmountFirst;
                }
                else if (interestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name)
                {
                    #region comment
                    //interestAmountFirst = 0.00M;
                    //if (i == 0)
                    //{
                    //    //var interest = loanAmount * newInterestRate;
                    //    //interest = interest / 30;
                    //    //if (releaseDate.Month != paymentStartDate.Month)
                    //    //{
                    //    //    daysDiff = 30 - releaseDate.Day;
                    //    //    if (paymentStartDate.Day <= 30)
                    //    //        daysDiff += paymentStartDate.Day;
                    //    //    else if (paymentStartDate.Day == 31)
                    //    //        daysDiff += 30;
                    //    //}
                    //    //else
                    //    //{
                    //    //    if (paymentDateDay <= 30)
                    //    //        daysDiff = paymentDateDay - releaseDate.Day;
                    //    //    else if (paymentDateDay == 31)
                    //    //        daysDiff = 30 - releaseDate.Day;
                    //    //}
                    //    //interest = (interest * daysDiff);
                    //    //amortizationModel.InterestPayment = interest;

                    //    var interest = loanAmount * newInterestRate;
                    //    interest = interest / 30;
                    //    if (releaseDate.Month != paymentStartDate.Month)
                    //    {
                    //        daysDiff = 30 - releaseDate.Day;
                    //        if (daysDiff <= SystemSetting.GracePeriod)
                    //        {
                    //            daysDiff = 0;
                    //        }

                    //        if (paymentStartDate.Day <= 30)
                    //            daysDiff += paymentStartDate.Day;
                    //        else if (paymentStartDate.Day == 30)
                    //            daysDiff += 30;
                    //        monthsDiff = paymentStartDate.Month - releaseDate.Month;

                    //        if (monthsDiff > 1)
                    //        {
                    //            daysDiff += ((monthsDiff - 1) * 30);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (paymentDateDay == 30)
                    //        {
                    //            daysDiff = (paymentDateDay) - releaseDate.Day;
                    //        }
                    //        else if (paymentDateDay == 28 || paymentDateDay == 29 || paymentDateDay == 31)
                    //        {
                    //            daysDiff = 30 - releaseDate.Day;
                    //        }
                    //    }
                    //    interest = (interest * daysDiff);
                    //    amortizationModel.InterestPayment = interest;
                    //    interestAmountFirst = amortizationModel.InterestPayment;
                    //}
                    //else
                    //{
                    //amortizationModel.InterestPayment = previousPrincipalBalance * newInterestRate;
                    //}
                    #endregion

                    amortizationModel.InterestPayment = previousPrincipalBalance * newInterestRate;

                    var interestPaymentTemp = amortizationModel.InterestPayment;
                    var tempPrevBal = amortizationModel.PrincipalBalance;
                    var temp2 = amortizationModel.PrincipalBalance;

                    for (int j = 1; j < count; j++)
                    {
                        tempPrevBal = tempPrevBal - amortizationModel.PrincipalPayment;
                        interestPaymentTemp = interestPaymentTemp + (temp2 * newInterestRate);
                        //interestPaymentTemp = Math.Floor(interestPaymentTemp);
                        interestPaymentTemp = AdjustInterestRateValue(interestPaymentTemp);
                        temp2 = tempPrevBal;
                    }

                    interestPaymentTotal = interestPaymentTemp;
                }
                
                amortizationModel.PrincipalBalance = Math.Floor(amortizationModel.PrincipalBalance);
                //amortizationModel.InterestPayment = Math.Floor(amortizationModel.InterestPayment);
                amortizationModel.InterestPayment = AdjustInterestRateValue(amortizationModel.InterestPayment);
                amortizationModel.TotalPayment = amortizationModel.PrincipalPayment + amortizationModel.InterestPayment;
                if (i == 0)
                {
                    
                    amortizationModel.TotalLoanBalance = (loanAmount + interestPaymentTotal) -
                                amortizationModel.TotalPayment;
                }
                else
                {
                    #region comment
                    //if (i == (amortizationScheduleItemsCount - 2))
                    //{
                    //    //amortizationModel.TotalPayment = amortizationModel.PrincipalPayment + amortizationModel.InterestPayment;
                    //    amortizationModel.TotalLoanBalance = previousTotalLoanBalance - amortizationModel.TotalPayment;
                    //    var remaining = amortizationModel.TotalLoanBalance - (amortizationModel.TotalPayment - amortizationModel.InterestPayment);
                    //    if (loanOptions.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name)
                    //    {
                    //        if (remaining > 0)
                    //        {
                    //            amortizationModel.TotalLoanBalance += remaining;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    //amortizationModel.TotalPayment = amortizationModel.PrincipalPayment + amortizationModel.InterestPayment;
                    //    amortizationModel.TotalLoanBalance = previousTotalLoanBalance - amortizationModel.TotalPayment;
                    //}

                    //if (i == (amortizationScheduleItemsCount - 1))
                    //{
                    //    amortizationModel.TotalLoanBalance = 0.00M;
                    //}
                    #endregion

                    amortizationModel.TotalLoanBalance = previousTotalLoanBalance - amortizationModel.TotalPayment;
                }

                previousTotalLoanBalance = amortizationModel.TotalLoanBalance;

                previousPrincipalBalance = amortizationModel.PrincipalBalance;
                amortizationModel.InterestPaymentTotal = interestPaymentTotal;

                #region comment
                //amortizationModel.PrincipalPayment = Math.Round(amortizationModel.PrincipalPayment, 2);
                //amortizationModel.PrincipalBalance = Math.Round(amortizationModel.PrincipalBalance, 2);
                //amortizationModel.InterestPayment = Math.Round(amortizationModel.InterestPayment, 2);
                //amortizationModel.TotalPayment = Math.Round(amortizationModel.TotalPayment, 2);
                //amortizationModel.TotalLoanBalance = Math.Round(amortizationModel.TotalLoanBalance, 2);
                //amortizationModel.InterestPaymentTotal = Math.Round(amortizationModel.InterestPaymentTotal, 2);


                //amortizationModel.TotalPayment = Math.Floor(amortizationModel.TotalPayment);
                #endregion

                amortizationModel.TotalLoanBalance = Math.Floor(amortizationModel.TotalLoanBalance);
                amortizationModel.InterestPaymentTotal = Math.Floor(amortizationModel.InterestPaymentTotal);

                loanCalculatorSchedule.Add(amortizationModel);
            }

            return loanCalculatorSchedule;
        }

        public List<AmortizationScheduleModel> GenerateLoanAmortization(LoanCalculatorOptions loanOptions)
        {
            List<AmortizationScheduleModel> loanApplicationAmortizationModel = new List<AmortizationScheduleModel>();

            var model = CalculateLoan(loanOptions);
            var amortizationScheduleItemsCount = model.Count;
            var paymentStartDate = loanOptions.PaymentStartDate;
            var paymentMode = loanOptions.PaymentMode;
            var dtManager = new DateTimeOperationManager(paymentMode, paymentStartDate, loanOptions.IncludeSaturday, loanOptions.TermOption);
            int i = 0;

            foreach (var item in model)
            {

                //Amortization Schedule Items
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.TotalPayment = item.TotalPayment;
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;

                loanApplicationAmortizationModel.Add(amortizationModel);

                i++;
            }

            amortizationSchedule = loanApplicationAmortizationModel;

            return loanApplicationAmortizationModel;
        }

        private static string GetItemType(string unitOfMeasure)
        {
            string type = "";

            if (unitOfMeasure == UnitOfMeasure.DailyType.Name)
                type = "Day";
            else if (unitOfMeasure == UnitOfMeasure.MonthlyType.Name)
                type = "Month";
            else if (unitOfMeasure == UnitOfMeasure.SemiMonthlyType.Name)
                type = "Semi-Month";
            else if (unitOfMeasure == UnitOfMeasure.WeeklyType.Name)
                type = "Week";
            else if (unitOfMeasure == UnitOfMeasure.AnnuallyType.Name)
                type = "Year";

            return type;
        }

        public AmortizationScheduleModel RetrieveSchedule(string randomKey)
        {
            return this.amortizationSchedule.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public override void Retrieve(int id)
        {
        }

        public override void PrepareForSave()
        {
        }

        public decimal AdjustInterestRateValue(decimal interestRate)
        {
            var result = interestRate;
            var whole = Math.Floor(interestRate);
            var diff = interestRate - whole;

            if (diff == 0)
                result = whole;
            else if (diff > 0 && diff <= 0.25M)
                result = whole + 0.25M;
            else if (diff > 0.25M && diff <= 0.5M)
                result = whole + 0.5M;
            else if (diff > 0.5M && diff <= 0.75M)
                result = whole + 0.75M;
            else if (diff > 0.75M && diff <= 1M)
                result = whole + 1M;

            return result;
        }
    }
}
