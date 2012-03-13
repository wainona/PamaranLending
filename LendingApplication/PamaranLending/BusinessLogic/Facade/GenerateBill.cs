using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class GenerateBillFacade
    {
        public const string ManualBillingDisplay = "Manual Billing Display";
        public const string ManualBillingSave = "Manual Billing Save";
        public const string GenerateBillSave = "Generate Bill Save";

        public static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        private static DateTime ChangeTo30Days(DateTime today)
        {
            if (today.Day == 27) return today.AddDays(3);
            else if (today.Day == 28) return today.AddDays(2);
            else if (today.Day == 29) return today.AddDays(1);
            else if (today.Day == 31) return today.AddDays(-1);
            //if (today.Day >= 27 && today.Day <= 31)
            //    today = new DateTime(today.Year, today.Month, 30);
            else  return today;
        }
        public static DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        private static decimal GenerateInterest(string interestDescription, decimal principalBalance, decimal interestRate)
        {
            decimal interest = principalBalance * (interestRate / 100);
            if (interestDescription == ProductFeature.MonthlyInterestRateType.Name)
                interest /= 30;
            else if (interestDescription == ProductFeature.AnnualInterestRateType.Name)
                interest /= 360;

            return interest;
        }
        private static decimal GenerateInterest(string interestDescription, decimal principalBalance, decimal interestRate, int dayDiff)
        {
            decimal interest = 0;
            if (dayDiff == 30 && interestDescription == ProductFeature.MonthlyInterestRateType.Name)
                interest = principalBalance * (interestRate / 100);
            else
                interest = GenerateInterest(interestDescription, principalBalance, interestRate) * dayDiff;
            return interest;
        }
        private static int GetDays(string paymentMode)
        {
            int days = 30;

            if (paymentMode == UnitOfMeasure.DailyType.Name)
                days = 1;
            else if (paymentMode == UnitOfMeasure.WeeklyType.Name)
                days = 7;
            else if (paymentMode == UnitOfMeasure.SemiMonthlyType.Name)
                days = 15;
            else if (paymentMode == UnitOfMeasure.MonthlyType.Name)
                days = 30;
            else if (paymentMode == UnitOfMeasure.AnnuallyType.Name)
                days = 360;
            return days;
        }
        public static Receivable CreateReceivableWithStatus(int financialAccountID,decimal amount,  int disburseId, string type,DateTime actualDate,DateTime dueDate, DateTime entryDate)
        {
            if (amount > 0 && type != ManualBillingDisplay)
            {
                Receivable receivable = new Receivable();
                receivable.ReceivableType = ReceivableType.InterestType;
                receivable.FinancialAccountId = financialAccountID;
                receivable.Date = entryDate;
                receivable.DueDate = dueDate;
                receivable.ValidityPeriod = actualDate;
                receivable.Amount = amount;
                receivable.Balance = amount;
                if (disburseId != -1) receivable.PaymentId = disburseId;

                ReceivableStatu receivableStatus = new ReceivableStatu();
                receivableStatus.Receivable = receivable;
                receivableStatus.ReceivableStatusType = ReceivableStatusType.OpenType;
                receivableStatus.TransitionDateTime = dueDate;
                receivableStatus.IsActive = true;

                return receivable;
            }
            else return null;
        }
        public static decimal GenerateInterest(LoanAccount loanAccount, AgreementItem agreementItem, int days, decimal balance)
        {
            decimal interest = 0;
            string interestDescription = agreementItem.InterestRateDescription;
            decimal interestRate = agreementItem.InterestRate;

            interest = GenerateInterest(interestDescription, balance, interestRate, days);
            interest = Math.Floor(interest);
            return interest;
        }

        public static void GenerateReceivable(DateTime today)
        {
            var loanAccounts = from la in ObjectContext.LoanAccounts
                               join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                               join last in ObjectContext.LoanAccountStatusTypes on las.StatusTypeId equals last.Id
                               where (last.Id == LoanAccountStatusType.CurrentType.Id
                                   || last.Id == LoanAccountStatusType.UnderLitigationType.Id
                                   || last.Id == LoanAccountStatusType.DelinquentType.Id)
                                   && (las.IsActive == true)
                               select la;

            var financialwithAgreement = from l in loanAccounts
                                         join f in ObjectContext.FinancialAccounts on l.FinancialAccountId equals f.Id
                                         join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                                         select f;

            var loanAgreementItem = from f in financialwithAgreement
                                    join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                                    join a in ObjectContext.Addenda on ai.Id equals a.AgreementId
                                    where ai.Agreement.EndDate == null && ai.IsActive == true
                                    select new { ai, f.LoanAccount };


            var loanAgreementItem2 = from f in financialwithAgreement
                                     join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                                     where ai.Agreement.EndDate == null && ai.IsActive == true
                                     select new { ai, f.LoanAccount };
            loanAgreementItem = loanAgreementItem.Union(loanAgreementItem2);



            var lastDayOfTheMonth = LastDayOfMonthFromDateTime(today);
            var firstDayOfTheMonth = new DateTime(today.Year, today.Month, 1);
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(lastDayOfTheMonth);


            foreach (var l in loanAgreementItem)
            {
                var loanAccount = l.LoanAccount;
                var agreementItem = l.ai;
                if (agreementItem.MethodOfChargingInterest != ProductFeature.DiscountedInterestType.Name)
                {
                    if (today == lastDayOfTheMonth)
                    {
                        GenerateAndSaveInterest(loanAccount, agreementItem, GenerateBillSave, lastDayOfTheMonth, validDueDate,lastDayOfTheMonth);
                    }
                    else
                    {
                        GenerateInterestForLastMonth(today, loanAccount, agreementItem, GenerateBillSave);
                    }

                }
            }

        }
        public static decimal GenerateInterestForLastMonth(DateTime selectedDate, LoanAccount loanAccount, AgreementItem agreementItem, string type)
        {
             decimal interest = 0;
             var diffTodayAndRelease = 0;
             DateTime firstDayOfTheMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
             var lastDayOfLastMonth = firstDayOfTheMonth.AddDays(-1); 

             var receivablesLastMonth = ObjectContext.Receivables.Where(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId
              && entity.ValidityPeriod.Month == lastDayOfLastMonth.Month && entity.ValidityPeriod.Year == lastDayOfLastMonth.Year 
              && entity.ValidityPeriod.Day == lastDayOfLastMonth.Day).OrderByDescending(entity => entity.ValidityPeriod);
             diffTodayAndRelease = lastDayOfLastMonth.Subtract(loanAccount.LoanReleaseDate.Value).Days;

             //If no receivables generated where validity period until end of last month, generated bill
            if (receivablesLastMonth.Count() == 0 && diffTodayAndRelease > 0 )
             {
                 interest = GenerateAndSaveInterest(loanAccount, agreementItem,type,lastDayOfLastMonth,lastDayOfLastMonth,selectedDate);
             }
            return interest;
                         
        }
        public static decimal GenerateAndSaveInterest(LoanAccount loanAccount, AgreementItem agreementItem, string type,DateTime actualDate, DateTime validDueDate, DateTime entryDate)
        {
            DateTime lastDayChangeTo30Days = actualDate;
            var lastDayOfTheMonth = LastDayOfMonthFromDateTime(actualDate);
        

            // Day Difference must still be equal to 30 Days
            if (lastDayChangeTo30Days == lastDayOfTheMonth)
                lastDayChangeTo30Days = ChangeTo30Days(actualDate);


            decimal totalInterest = 0;


            if (loanAccount.InterestTypeId != InterestType.FixedInterestTYpe.Id
                && loanAccount.InterestTypeId != InterestType.ZeroInterestTYpe.Id && agreementItem.MethodOfChargingInterest
                != ProductFeature.DiscountedInterestType.Name)
            {
               
                 totalInterest = GenerateInterestUsingRate(loanAccount, agreementItem,type,actualDate, validDueDate,entryDate);
            }
            else
            {
                    totalInterest = GenerateInterestFixed(loanAccount, type, lastDayOfTheMonth,entryDate);
            }

            return totalInterest;

        }
        private static decimal GenerateInterestUsingRate(LoanAccount loanAccount, AgreementItem agreementItem, string type,DateTime actualDate, DateTime validDueDate,DateTime entryDate)
        {
            DateTime lastDayChangeTo30Days = actualDate;
            var lastDayOfTheMonth = LastDayOfMonthFromDateTime(actualDate);
            var advanceChangeDay = SystemSetting.AdvanceChangeNoInterestStartDay;

            // Day Difference must still be equal to 30 Days
            if (lastDayChangeTo30Days == lastDayOfTheMonth)
                lastDayChangeTo30Days = ChangeTo30Days(actualDate);
               
            /*** INITIALISATIONS****/
            var diffBetweenPaymentAndToday = SystemSetting.GracePeriod + 1;
            var diffBetweenReleaseandToday = SystemSetting.GracePeriod + 1;
            var firstDayOfTheMonth = new DateTime(actualDate.Year, actualDate.Month, 1);
            decimal totalInterest = 0;
            var loanDisbursemntVoucher = ObjectContext.LoanDisbursementVcrs.FirstOrDefault(entity => entity.AgreementId == agreementItem.AgreementId);
            decimal loanBalance = 0;
            if (agreementItem.InterestComputationMode == ProductFeature.StraightLineMethodType.Name
                && agreementItem.MethodOfChargingInterest == ProductFeature.AddonInterestType.Name)
                loanBalance = loanAccount.LoanAmount;
            else if (agreementItem.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name
                && agreementItem.MethodOfChargingInterest == ProductFeature.AddonInterestType.Name)
                loanBalance = loanAccount.LoanBalance;
            /*** INITIALISATIONS****/
           
            //Check for last payment date
            var payments = ObjectContext.FinAcctTrans.Where(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId
                         && entity.TransactionDate.Month == actualDate.Month && entity.TransactionDate.Year == actualDate.Year).OrderByDescending(entity => entity.TransactionDate);
            var latestPayment = payments.FirstOrDefault();

            if (latestPayment != null)
                diffBetweenPaymentAndToday = lastDayChangeTo30Days.Subtract(latestPayment.TransactionDate.Date).Days;
            if (lastDayChangeTo30Days.Month == loanAccount.LoanReleaseDate.Value.Month && lastDayChangeTo30Days.Year == loanAccount.LoanReleaseDate.Value.Year)
                diffBetweenReleaseandToday = lastDayChangeTo30Days.Subtract(loanAccount.LoanReleaseDate.Value.Date).Days;


            // If lastpayment date is < grace period, do not generate interest
            // if loanrelease is lesser < grace period, no interest
            if (diffBetweenPaymentAndToday > SystemSetting.GracePeriod && diffBetweenReleaseandToday > SystemSetting.GracePeriod)
            {
                decimal totalDisbursedAmount = 0;
                decimal interest = 0;
                var dayDiff = GetDays(agreementItem.PaymentMode); // default day difference is 30 days;

                var receivablesForCurrentMonth = ObjectContext.Receivables.Where(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId
                && entity.ValidityPeriod.Month == actualDate.Month && entity.ValidityPeriod.Year == actualDate.Year).OrderByDescending(entity => entity.ValidityPeriod);

                var disbursementsForCurrentMonth = from pa in ObjectContext.PaymentApplications
                                                   join d in ObjectContext.Disbursements on pa.PaymentId equals d.PaymentId
                                                   join ld in ObjectContext.LoanDisbursements on d.PaymentId equals ld.PaymentId
                                                   join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                                   where p.TransactionDate.Month == actualDate.Month
                                                   && p.TransactionDate.Year == p.TransactionDate.Year
                                                   && pa.LoanDisbursementVoucherId == loanDisbursemntVoucher.Id
                                                   orderby p.TransactionDate ascending
                                                   select new { payment = p, loandisbursement = ld };

                if (disbursementsForCurrentMonth.Count() != 0)
                {
                    totalDisbursedAmount = disbursementsForCurrentMonth.Sum(entity => entity.payment.TotalAmount);
                    if (receivablesForCurrentMonth.Count() != 0)
                    {
                        var interestpayment = receivablesForCurrentMonth.Sum(entity => entity.Amount - entity.Balance);

                        if (payments.Count() != 0)
                        {
                          //  totalDisbursedAmount = disbursementsForCurrentMonth.Sum(entity => entity.payment.TotalAmount);
                            loanBalance -= totalDisbursedAmount;
                            if (loanBalance != 0)
                            {
                                var previousReceivable = receivablesForCurrentMonth.FirstOrDefault(entity => entity.PaymentId == null);
                                if (previousReceivable != null)
                                {
                                    if (previousReceivable.ValidityPeriod.Date < lastDayChangeTo30Days)
                                        dayDiff = lastDayChangeTo30Days.Subtract(previousReceivable.ValidityPeriod.Date).Days;
                                    else dayDiff = 0;

                                }
                                interest = GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                CreateReceivableWithStatus(loanAccount.FinancialAccountId,interest,  -1,type,actualDate,validDueDate,entryDate);
                                totalInterest += interest;
                            }
                            foreach (var disbursement in disbursementsForCurrentMonth)
                            {
                                var previousReceivable = receivablesForCurrentMonth.FirstOrDefault(entity => entity.PaymentId == disbursement.payment.Id);
                                if (previousReceivable != null)
                                {
                                    if (previousReceivable.ValidityPeriod.Date < lastDayChangeTo30Days)
                                        dayDiff = lastDayChangeTo30Days.Subtract(previousReceivable.ValidityPeriod.Date).Days;
                                    else dayDiff = 0;
                                }
                                else
                                {
                                    //Check if advance change ang particular disbursement
                                    if (disbursement.loandisbursement.LoanDisbursementTypeId == LoanDisbursementType.ACCheque.Id && disbursement.payment.TransactionDate.Day >= advanceChangeDay)
                                        dayDiff = 0;
                                    else
                                    {
                                        dayDiff = lastDayChangeTo30Days.Subtract(disbursement.payment.TransactionDate).Days;
                                        if (disbursement.payment.TransactionDate.Day == 1) dayDiff += 1;
                                       
                                    }
                                }
                                if (dayDiff > SystemSetting.GracePeriod)
                                {
                                    loanBalance = disbursement.payment.TotalAmount;
                                    interest = GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                    CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, disbursement.payment.Id, type, actualDate, validDueDate, entryDate);
                                    totalInterest += interest;
                                }
                            }
                        }
                        else
                        {
                            //No payments, only additional loan
                            loanBalance -= totalDisbursedAmount;
                            if (loanBalance != 0)
                            {
                                var previousReceivable = receivablesForCurrentMonth.FirstOrDefault(entity => entity.PaymentId == null);
                                if (previousReceivable != null)
                                {

                                    if (previousReceivable.ValidityPeriod.Date < lastDayChangeTo30Days)
                                        dayDiff = lastDayChangeTo30Days.Subtract(previousReceivable.ValidityPeriod.Date).Days;
                                    else dayDiff = 0;
                                }
                                interest += GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                    CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, -1, type, actualDate, validDueDate,entryDate);
                                totalInterest += interest;
                            }
                            foreach (var disbursement in disbursementsForCurrentMonth)
                            {
                                var previousReceivable = receivablesForCurrentMonth.FirstOrDefault(entity => entity.PaymentId == disbursement.payment.Id);
                                if (previousReceivable != null)
                                {
                                    if (previousReceivable.ValidityPeriod.Date < lastDayChangeTo30Days)
                                        dayDiff = lastDayChangeTo30Days.Subtract(previousReceivable.ValidityPeriod.Date).Days;
                                    else dayDiff = 0;
                                }
                                else
                                {
                                    //Check if advance change ang particular disbursement
                                    if (disbursement.loandisbursement.LoanDisbursementTypeId == LoanDisbursementType.ACCheque.Id && disbursement.payment.TransactionDate.Day >= advanceChangeDay)
                                        dayDiff = 0;
                                    else
                                    {
                                        dayDiff = lastDayChangeTo30Days.Subtract(disbursement.payment.TransactionDate).Days;
                                        if (disbursement.payment.TransactionDate.Day == 1) dayDiff += 1;
                                    }
                                }
                                if (dayDiff > SystemSetting.GracePeriod)
                                {
                                    loanBalance = disbursement.payment.TotalAmount;
                                    interest = GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                    CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, disbursement.payment.Id, type, actualDate, validDueDate, entryDate);
                                    totalInterest += interest;
                                }
                            }
                        }
                    }
                    else
                    {
                        //No manual billed receivables
                        totalDisbursedAmount = disbursementsForCurrentMonth.Sum(entity => entity.payment.TotalAmount);
                        loanBalance -= totalDisbursedAmount;
                        if (loanBalance != 0)
                        {
                            interest += GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, -1, type, actualDate, validDueDate,entryDate);
                        }
                        totalInterest += interest;

                        foreach (var disbursement in disbursementsForCurrentMonth)
                        {
                            interest = 0;
                           
                            if (disbursement.loandisbursement.LoanDisbursementTypeId == LoanDisbursementType.ACCheque.Id && disbursement.payment.TransactionDate.Day >= advanceChangeDay)
                                dayDiff = 0;
                            else
                            {
                                dayDiff = lastDayChangeTo30Days.Subtract(disbursement.payment.TransactionDate).Days;
                                if (disbursement.payment.TransactionDate.Day == 1) dayDiff += 1;
                            }
                            if (dayDiff > SystemSetting.GracePeriod)
                            {
                                loanBalance = disbursement.payment.TotalAmount;
                                interest = GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                                CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, disbursement.payment.Id, type, actualDate, validDueDate, entryDate);
                                totalInterest += interest;
                            }
                        }
                    }

                }
                else
                {

                    //No DISBURSEMENTS FOR THIS MONTH, Meaning no additional loan and no payments
                    if (receivablesForCurrentMonth.Count() > 0)
                    {
                        if (receivablesForCurrentMonth.FirstOrDefault() != null)
                        {
                            if (receivablesForCurrentMonth.FirstOrDefault().ValidityPeriod < lastDayChangeTo30Days)
                                dayDiff = lastDayChangeTo30Days.Subtract(receivablesForCurrentMonth.FirstOrDefault().ValidityPeriod).Days;
                            else dayDiff = 0;
                        }
                    }
                    else if (actualDate.Day != lastDayOfTheMonth.Day && actualDate.Month == lastDayOfTheMonth.Month)
                        dayDiff = actualDate.Day;
                        //for those manual billing like nov 28, day diff must be 28
                        //else it must use the default 30 days


                    //if (dayDiff > SystemSetting.GracePeriod)
                    //{
                        interest = GenerateInterest(loanAccount, agreementItem, dayDiff, loanBalance);
                        totalInterest += interest;
                        if (interest > 0)
                            CreateReceivableWithStatus(loanAccount.FinancialAccountId, interest, -1, type, actualDate, validDueDate, entryDate);
                  //  }
                }
            }
            return totalInterest;
        }
        private static decimal GenerateInterestFixed(LoanAccount loanAccount, string type, DateTime lastDayOfTheMonth,DateTime entryDate)
        {
            decimal totalInterest = 0;
            var item = from i in ObjectContext.InterestItems
                       where i.LoanId == loanAccount.FinancialAccountId
                       && i.IsActive == true
                       select i;
            var hasReceivable = from r in ObjectContext.Receivables
                                where r.FinancialAccountId == loanAccount.FinancialAccountId
                                && r.DueDate == lastDayOfTheMonth
                                select r;
            if (item.Count() != 0 && hasReceivable.Count() == 0)
            {
                    totalInterest = item.FirstOrDefault().Amount;
                    CreateReceivableWithStatus(loanAccount.FinancialAccountId, item.FirstOrDefault().Amount, -1, type, lastDayOfTheMonth, lastDayOfTheMonth, entryDate);
            }
            return totalInterest;
        }
    
    }
}
