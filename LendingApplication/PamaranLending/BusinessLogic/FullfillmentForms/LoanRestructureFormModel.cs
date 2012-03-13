using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class LoanRestructureListModel : BusinessObjectModel
    {
        public int LoanID { get; set; }
        public decimal LoanAmount { get; set; }
        public string ProductTerm { get; set; }
        public string RemainingPayments { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string LoanReleaseDate { get; set; }
        public decimal TotalLoanBalance { get; set; }
        public string InterestComputationMode { get; set; }
        public string InterestRate { get; set; }
    }

    [Serializable]
    public class AmortizationScheduleModel : BusinessObjectModel
    {
        public string Counter { get; set; }
        public AmortizationSchedule AmortizationScheduleParent { get; set; }
        public DateTime ScheduledPaymentDate { get; set; }
        public string _ScheduledPaymentDate 
        {
            get
            {
                return this.ScheduledPaymentDate.ToString("MM/dd/yyyy");
            }
        }
        public decimal PrincipalPayment { get; set; }
        public decimal InterestPayment { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal TotalLoanBalance { get; set; }
        public bool IsBilledIndicator { get; set; }
        public decimal InterestPaymentTotal { get; set; }

        public AmortizationScheduleModel()
        {
            this.IsNew = true;
            this.PrincipalPayment = 0;
            this.InterestPayment = 0;
            this.TotalPayment = 0;
            this.PrincipalBalance = 0;
            this.TotalLoanBalance = 0;
            this.InterestPaymentTotal = 0;
        }

        public AmortizationScheduleModel(AmortizationScheduleItem items)
        {
            this.IsNew = false;
            this.ScheduledPaymentDate = items.ScheduledPaymentDate;
            this.PrincipalBalance = items.PrincipalBalance;
            this.InterestPayment = items.InterestPayment;
            this.TotalLoanBalance = items.TotalLoanBalance;
            this.IsBilledIndicator = items.IsBilledIndicator;
            this.PrincipalPayment = items.PrincipalPayment;
        }

        public AmortizationScheduleModel(AmortizationScheduleModel items)
        {
            this.IsNew = false;
            this.Counter = items.Counter;
            this.ScheduledPaymentDate = items.ScheduledPaymentDate;
            this.PrincipalBalance = items.PrincipalBalance;
            this.InterestPayment = items.InterestPayment;
            this.TotalLoanBalance = items.TotalLoanBalance;
            this.IsBilledIndicator = items.IsBilledIndicator;
            this.PrincipalPayment = items.PrincipalPayment;
            this.TotalPayment = items.PrincipalPayment + items.InterestPayment;
        }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public void PrepareForSave(LoanApplication app, AmortizationSchedule sched, AmortizationScheduleModel models)
        {
            if (this.IsNew)
            {
                if (app.LoanTermLength != 0)
                {
                    AmortizationScheduleItem item = new AmortizationScheduleItem();
                    item.AmortizationSchedule = sched;
                    item.ScheduledPaymentDate = models.ScheduledPaymentDate;
                    item.PrincipalPayment = models.PrincipalPayment;
                    item.InterestPayment = models.InterestPayment;
                    item.PrincipalBalance = models.PrincipalBalance;
                    item.TotalLoanBalance = models.TotalLoanBalance;
                    item.IsBilledIndicator = false;
                    Context.AmortizationScheduleItems.AddObject(item);
                }
            }
            else
            {
                if (app.LoanTermLength != 0)
                {
                    var items = Context.AmortizationScheduleItems.Where(entity => entity.AmortizationScheduleId == sched.Id);
                    foreach (var schedule in items)
                    {
                        Context.AmortizationScheduleItems.DeleteObject(schedule);
                    }

                    AmortizationScheduleItem item = new AmortizationScheduleItem();
                    item.AmortizationSchedule = sched;
                    item.ScheduledPaymentDate = models.ScheduledPaymentDate;
                    item.PrincipalPayment = models.PrincipalPayment;
                    item.InterestPayment = models.InterestPayment;
                    item.PrincipalBalance = models.PrincipalBalance;
                    item.TotalLoanBalance = models.TotalLoanBalance;
                    item.IsBilledIndicator = false;
                    Context.AmortizationScheduleItems.AddObject(item);

                    //int count = Convert.ToInt32(models.Counter.ElementAt(models.Counter.Length - 1));
                    //int i = 0;
                    //foreach (var item in items)
                    //{
                    //    if (i != count)
                    //        i++;
                    //    else
                    //    {
                    //        DateTime date = models.ScheduledPaymentDate;
                    //        item.ScheduledPaymentDate = date;
                    //        item.PrincipalPayment = models.PrincipalPayment;
                    //        item.InterestPayment = models.InterestPayment;
                    //        item.PrincipalBalance = models.PrincipalBalance;
                    //        item.TotalLoanBalance = models.TotalLoanBalance;
                    //        item.IsBilledIndicator = false;
                    //        break;
                    //    }
                    //}
                }
            }
        }
    }

    public class AmortizationItemsModel : BusinessObjectModel
    {
        public decimal BalanceToCarryOver { get; set; }
        public decimal AdditionalAmount { get; set; }
        public decimal NewLoanAmount { get; set; }
        public decimal Percentage { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime LoanReleaseDate { get; set; }
        public string InterestComputationMode { get; set; }
        public int InterestComputationModeId { get; set; }
        public string NewInterestComputationMode { get; set; }
        public string InterestRateDescription { get; set; }
        public int InterestRateDescriptionId { get; set; }
        public int Term { get; set; }
        public string Unit { get; set; }
        public int UnitId { get; set; }
        public decimal InterestRate { get; set; }
        public decimal NewInterestRate { get; set; }
        public string PaymentMode { get; set; }
        public int PaymentModeId { get; set; }
        public string MethodOfChargingInterest { get; set; }
        public int MethodOfChargingInterestId { get; set; }
        public decimal InterestPaymentTotal { get; set; }
        public string CollateralRequirement { get; set; }
        public int CollateralRequirementId { get; set; }
        public int RemainingPayments { get; set; }
        public decimal LessPayment { get; set; }
        public bool AddPostDatedChecks { get; set; }

        public AmortizationItemsModel()
        {
            this.AddPostDatedChecks = false;
        }
    }
}
