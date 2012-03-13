using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace BusinessLogic.FullfillmentForms
{

    public partial class LoanRestructureForm : FullfillmentForm<FinancialEntities>
    {
        #region Loan Restructure properties initialization
        public int CustomerId { get; set; }
        public int ModifiedBy { get; set; }
        public int FinancialProductId1 { get; set; }
        public int FinancialProductId2 { get; set; }
        public int FinancialAccountId { get; set; }
        public int FinancialAccountId2 { get; set; }
        public int InterestTypeId { get; set; }
        public string InterestTypeS { get; set; }
        public List<ChequeModel> Cheques;
        public List<ChequeModel> Cheques1;
        public List<ChequeModel> Cheques2;
        private List<LoanFeeModel> Fees;
        private List<Collateral> Collaterals;
        private List<PersonnalPartyModel> Guarantors;
        private List<PersonnalPartyModel> CoBorrowers;
        private List<SubmittedDocumentModel> SubmittedDocuments;
        public AmortizationItemsModel SplitLoanItemsAccount1 { get; set; }
        public AmortizationItemsModel SplitLoanItemsAccount2 { get; set; }
        public AmortizationItemsModel AdditionalLoanAccount { get; set; }
        public AmortizationItemsModel ChangeIcmAccount { get; set; }
        public AmortizationItemsModel ChangeInterestAccount { get; set; }
        public AmortizationItemsModel ConsolidateLoanAccount { get; set; }
        public List<AmortizationScheduleModel> SplitLoanAmortizationSchedule1 { get; set; }
        public List<AmortizationScheduleModel> SplitLoanAmortizationSchedule2 { get; set; }
        public List<AmortizationScheduleModel> AdditionalLoanAmortizationSchedule { get; set; }
        public List<AmortizationScheduleModel> ChangeIcmAmortizationSchedule { get; set; }
        public List<AmortizationScheduleModel> ChangeInterestAmortizationSchedule { get; set; }
        public List<AmortizationScheduleModel> ConsolidateLoanAmortizationSchedule { get; set; }
        public List<LoanRestructureListModel> LoanRestructuredView { get; set; }
        #endregion

        /// <summary>
        /// Gets the first day of the month.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <returns>the first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(DateTime givenDate)
        {
            return new DateTime(givenDate.Year, givenDate.Month, 1);
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="givenDate">The given date.</param>
        /// <returns>the last day of the month</returns>
        public static DateTime GetTheLastDayOfMonth(DateTime givenDate)
        {
            return GetFirstDayOfMonth(givenDate).AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0, 0));
        }

        public IEnumerable<ChequeModel> AvailableCheques
        {
            get
            {
                return this.Cheques.Where(model => model.ToBeDeleted == false);
            }
        }

        public IEnumerable<ChequeModel> AvailableCheques1
        {
            get
            {
                return this.Cheques1.Where(model => model.ToBeDeleted == false);
            }
        }

        public IEnumerable<ChequeModel> AvailableCheques2
        {
            get
            {
                return this.Cheques2.Where(model => model.ToBeDeleted == false);
            }
        }

        public override void Retrieve(int id)
        {
            var today = DateTime.Now;
            GenerateSplitAccountAmortizationSchedule(today);
        }

        public void GenerateSplitAccountAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId);
            LoanAccount loanAccount = financialAccount.LoanAccount;

            //For Split Account 1
            var loanBalance = this.SplitLoanItemsAccount1.BalanceToCarryOver;
            //var balance1 = loanBalance * (this.SplitLoanItemsAccount1.Percentage / 100);
            //balance1 += (balance1 * (this.SplitLoanItemsAccount1.InterestRate / 100));
            //balance1 = balance1 - this.SplitLoanItemsAccount1.LessPayment;
            //this.SplitLoanItemsAccount1.NewLoanAmount = balance1;
            var term1 = this.SplitLoanItemsAccount1.Term;
            int termUnit1 = this.SplitLoanItemsAccount1.UnitId;
            int interestTerm1 = this.SplitLoanItemsAccount1.InterestRateDescriptionId;
            string paymentModeName1 = this.SplitLoanItemsAccount1.PaymentMode;
            int paymentMode1 = this.SplitLoanItemsAccount1.PaymentModeId;
            this.SplitLoanItemsAccount1.LoanReleaseDate = (DateTime)loanAccount.LoanReleaseDate;
            var loanReleaseDate = this.SplitLoanItemsAccount1.LoanReleaseDate;
            //var dtManager1 = new DateTimeOperationManager(paymentModeName1, loanReleaseDate);
            //this.SplitLoanItemsAccount1.PaymentStartDate = dtManager1.Increment();
            var paymentStartDate1 = this.SplitLoanItemsAccount1.PaymentStartDate;
            var dtManager = new DateTimeOperationManager(paymentModeName1, paymentStartDate1);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.SplitLoanItemsAccount1.InterestComputationMode;
            options.InterestRate = this.SplitLoanItemsAccount1.InterestRate;
            options.InterestRateDescription = this.SplitLoanItemsAccount1.InterestRateDescription;
            options.InterestRateDescriptionId = this.SplitLoanItemsAccount1.InterestRateDescriptionId;
            options.LoanAmount = this.SplitLoanItemsAccount1.NewLoanAmount;
            options.LoanReleaseDate = this.SplitLoanItemsAccount1.LoanReleaseDate;
            options.LoanTerm = this.SplitLoanItemsAccount1.Term;
            options.LoanTermId = this.SplitLoanItemsAccount1.UnitId;
            options.PaymentMode = this.SplitLoanItemsAccount1.PaymentMode;
            options.PaymentModeId = this.SplitLoanItemsAccount1.PaymentModeId;
            options.PaymentStartDate = this.SplitLoanItemsAccount1.PaymentStartDate;

            LoanCalculator calc = new LoanCalculator();
            var model = calc.CalculateLoan(options);
            int i = 0;

            foreach (var item in model)
            {
                //Amortization Schedule Items for Split Account 1
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddAmortizationScheduleItem1(amortizationModel);
                i++;

                this.SplitLoanItemsAccount1.InterestPaymentTotal = item.InterestPaymentTotal;
            }

            

            //For Split Account 2
            //var balance2 = loanBalance2 * (this.SplitLoanItemsAccount2.Percentage / 100);
            //balance2 += (balance2 * (this.SplitLoanItemsAccount2.InterestRate / 100));
            //balance2 = balance2 - this.SplitLoanItemsAccount2.LessPayment;
            //this.SplitLoanItemsAccount2.NewLoanAmount = balance2;
            var loanBalance2 = this.SplitLoanItemsAccount2.BalanceToCarryOver;
            var term2 = this.SplitLoanItemsAccount2.Term;
            int termUnit2 = this.SplitLoanItemsAccount2.UnitId;
            int interestTerm2 = this.SplitLoanItemsAccount1.InterestRateDescriptionId;
            string paymentModeName2 = this.SplitLoanItemsAccount2.PaymentMode;
            int paymentMode2 = this.SplitLoanItemsAccount2.PaymentModeId;
            this.SplitLoanItemsAccount2.LoanReleaseDate = (DateTime)loanAccount.LoanReleaseDate;
            var loanReleaseDate2 = this.SplitLoanItemsAccount2.LoanReleaseDate;
            //var dtManager3 = new DateTimeOperationManager(paymentModeName2, loanReleaseDate2);
            //this.SplitLoanItemsAccount2.PaymentStartDate = dtManager3.Increment();
            var paymentStartDate2 = this.SplitLoanItemsAccount2.PaymentStartDate;
            var dtManager2 = new DateTimeOperationManager(paymentModeName2, paymentStartDate2);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options2 = new LoanCalculatorOptions();
            options2.InterestComputationMode = this.SplitLoanItemsAccount2.InterestComputationMode;
            options2.InterestRate = this.SplitLoanItemsAccount2.InterestRate;
            options2.InterestRateDescription = this.SplitLoanItemsAccount2.InterestRateDescription;
            options2.InterestRateDescriptionId = this.SplitLoanItemsAccount2.InterestRateDescriptionId;
            options2.LoanAmount = this.SplitLoanItemsAccount2.NewLoanAmount;
            options2.LoanReleaseDate = this.SplitLoanItemsAccount2.LoanReleaseDate;
            options2.LoanTerm = this.SplitLoanItemsAccount2.Term;
            options2.LoanTermId = this.SplitLoanItemsAccount2.UnitId;
            options2.PaymentMode = this.SplitLoanItemsAccount2.PaymentMode;
            options2.PaymentModeId = this.SplitLoanItemsAccount2.PaymentModeId;
            options2.PaymentStartDate = this.SplitLoanItemsAccount2.PaymentStartDate;

            LoanCalculator calc2 = new LoanCalculator();
            var model2 = calc2.CalculateLoan(options2);
            
            i = 0;

            foreach (var item in model2)
            {
                //Amortization Schedule Items for Split Account 1
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager2.Current : dtManager2.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddAmortizationScheduleItem2(amortizationModel);

                i++;

                this.SplitLoanItemsAccount2.InterestPaymentTotal = item.InterestPaymentTotal;
            }
        }

        public void GenerateAdditionalLoanAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);

            //this.AdditionalLoanAccount.BalanceToCarryOver = agreementItem.LoanAmount;
            this.AdditionalLoanAccount.InterestComputationMode = agreementItem.InterestComputationMode;
            var icmId = ProductFeature.All(ProductFeatureCategory.InterestComputationModeType).SingleOrDefault(entity =>
                        entity.Name == this.AdditionalLoanAccount.InterestComputationMode).Id;
            this.AdditionalLoanAccount.InterestComputationModeId = Context.ProductFeatureApplicabilities.FirstOrDefault(entity => entity.ProductFeatureId == icmId 
                        && entity.EndDate == null).Id;
            this.AdditionalLoanAccount.InterestRate = agreementItem.InterestRate;
            this.AdditionalLoanAccount.Term = agreementItem.LoanTermLength;
            this.AdditionalLoanAccount.Unit = agreementItem.LoanTermUom;
            this.AdditionalLoanAccount.UnitId = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType).SingleOrDefault(entity =>
                        entity.Name == this.AdditionalLoanAccount.Unit).Id;
            this.AdditionalLoanAccount.InterestRateDescription = agreementItem.InterestRateDescription;
            this.AdditionalLoanAccount.InterestRateDescriptionId = ProductFeature.All(ProductFeatureCategory.InterestRateType).SingleOrDefault(entity =>
                        entity.Name == this.AdditionalLoanAccount.InterestRateDescription).Id;
            this.AdditionalLoanAccount.PaymentMode = agreementItem.PaymentMode;
            this.AdditionalLoanAccount.PaymentModeId = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType).SingleOrDefault(entity =>
                        entity.Name == this.AdditionalLoanAccount.PaymentMode).Id;
            this.AdditionalLoanAccount.MethodOfChargingInterest = agreementItem.MethodOfChargingInterest;
            var mociId = ProductFeature.All(ProductFeatureCategory.MethodofChargingInterestType).SingleOrDefault(entity =>
                        entity.Name == this.AdditionalLoanAccount.MethodOfChargingInterest).Id;
            this.AdditionalLoanAccount.MethodOfChargingInterestId = Context.ProductFeatureApplicabilities.FirstOrDefault(entity => entity.ProductFeatureId == mociId 
                        && entity.EndDate == null).Id;
            this.AdditionalLoanAccount.PaymentStartDate = today;

            //Initial Values
            var loanBalance = this.AdditionalLoanAccount.BalanceToCarryOver + this.AdditionalLoanAccount.AdditionalAmount;
            if(this.AdditionalLoanAccount.Term == 0)
                this.AdditionalLoanAccount.NewLoanAmount = loanBalance;
            else
                this.AdditionalLoanAccount.NewLoanAmount = this.AdditionalLoanAccount.AdditionalAmount;
            var term1 = this.AdditionalLoanAccount.Term;
            int termUnit1 = this.AdditionalLoanAccount.UnitId;
            int interestTerm1 = this.AdditionalLoanAccount.InterestRateDescriptionId;
            int paymentMode1 = this.AdditionalLoanAccount.PaymentModeId;
            string paymentModeName = this.AdditionalLoanAccount.PaymentMode;
            var loanReleaseDate = this.AdditionalLoanAccount.LoanReleaseDate.AddMonths(-1);
            var dtManager1 = new DateTimeOperationManager(paymentModeName, loanReleaseDate);
            //this.AdditionalLoanAccount.PaymentStartDate = dtManager1.Increment();
            this.AdditionalLoanAccount.PaymentStartDate = DateTimeOperationManager.AdjustPaymentStartDate(loanReleaseDate.AddMonths(1));
            var paymentStartDate = this.AdditionalLoanAccount.PaymentStartDate;
            var dtManager = new DateTimeOperationManager(paymentModeName, paymentStartDate);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.AdditionalLoanAccount.InterestComputationMode;
            options.InterestRate = this.AdditionalLoanAccount.InterestRate;
            options.InterestRateDescription = this.AdditionalLoanAccount.InterestRateDescription;
            options.InterestRateDescriptionId = this.AdditionalLoanAccount.InterestRateDescriptionId;
            options.LoanAmount = this.AdditionalLoanAccount.NewLoanAmount;
            options.LoanReleaseDate = this.AdditionalLoanAccount.LoanReleaseDate;
            options.LoanTerm = this.AdditionalLoanAccount.Term;
            options.LoanTermId = this.AdditionalLoanAccount.UnitId;
            options.PaymentMode = this.AdditionalLoanAccount.PaymentMode;
            options.PaymentModeId = this.AdditionalLoanAccount.PaymentModeId;
            options.PaymentStartDate = this.AdditionalLoanAccount.PaymentStartDate;

            LoanCalculator calc = new LoanCalculator();
            var model = calc.CalculateLoan(options);
            int i = 0;

            foreach (var item in model)
            {
                //Amortization Schedule Items
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddAdditionalLoanAmortizationScheduleItem(amortizationModel);
                i++;

                this.AdditionalLoanAccount.InterestPaymentTotal = item.InterestPaymentTotal;
            }
        }

        public void GenerateChangeInterestAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var amortizationSchedule = agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.EndDate == null);

            //var amortItems = amortizationSchedule.AmortizationScheduleItems.FirstOrDefault(entity => entity.IsBilledIndicator == false);

            Application oldApplication = agreement.Application;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.CollateralRequirementType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.MethodofChargingInterestType);
            var aiInterestRateDescription = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestRateType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestComputationModeType);
            this.FinancialProductId1 = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;
            var financialProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct;

            FillAmortizationItemModel(this.ChangeInterestAccount, agreementItem);
            this.ChangeInterestAccount.InterestRate = this.ChangeInterestAccount.NewInterestRate;
            this.ChangeInterestAccount.InterestComputationMode = agreementItem.InterestComputationMode;
            this.ChangeInterestAccount.InterestComputationModeId = aiInterestComputationMode.ProductFeatureApplicability.Id;
            this.ChangeInterestAccount.CollateralRequirement = aiCollateralRequirement.ProductFeatureApplicability.ProductFeature.Name;
            this.ChangeInterestAccount.CollateralRequirementId = aiCollateralRequirement.ProductFeatureApplicability.Id;
            this.ChangeInterestAccount.MethodOfChargingInterest = aiMethodOfChargingInterest.ProductFeatureApplicability.ProductFeature.Name;
            this.ChangeInterestAccount.MethodOfChargingInterestId = aiMethodOfChargingInterest.ProductFeatureApplicability.Id;
            this.ChangeInterestAccount.InterestRateDescriptionId = aiInterestRateDescription.ProductFeatureApplicability.ProductFeatureId;
            this.ChangeInterestAccount.Unit = UnitOfMeasure.MonthsType.Name;
            this.ChangeInterestAccount.UnitId = UnitOfMeasure.MonthsType.Id;
            this.ChangeInterestAccount.PaymentMode = UnitOfMeasure.MonthlyType.Name;
            this.ChangeInterestAccount.PaymentModeId = UnitOfMeasure.MonthlyType.Id;
            this.ChangeInterestAccount.LoanReleaseDate = amortizationSchedule.LoanReleaseDate;//.AddMonths(-1);

            ////this.ChangeInterestAccount.BalanceToCarryOver = agreementItem.LoanAmount;
            //this.ChangeInterestAccount.InterestComputationMode = agreementItem.InterestComputationMode;
            //this.ChangeInterestAccount.InterestRate = agreementItem.InterestRate;
            //this.ChangeInterestAccount.Term = agreementItem.LoanTermLength;
            //this.ChangeInterestAccount.Unit = agreementItem.LoanTermUom;
            //this.ChangeInterestAccount.UnitId = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType).SingleOrDefault(entity =>
            //            entity.Name == this.ChangeInterestAccount.Unit).Id;
            //this.ChangeInterestAccount.InterestRateDescription = agreementItem.InterestRateDescription;
            //this.ChangeInterestAccount.InterestRateDescriptionId = ProductFeature.All(ProductFeatureCategory.InterestRateType).SingleOrDefault(entity =>
            //            entity.Name == this.ChangeInterestAccount.InterestRateDescription).Id;
            //this.ChangeInterestAccount.PaymentMode = agreementItem.PaymentMode;
            //this.ChangeInterestAccount.PaymentModeId = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType).SingleOrDefault(entity =>
            //            entity.Name == this.ChangeInterestAccount.PaymentMode).Id;
            //this.ChangeInterestAccount.MethodOfChargingInterest = agreementItem.MethodOfChargingInterest;
            //this.ChangeInterestAccount.PaymentStartDate = amortItems.ScheduledPaymentDate;

            //Initial Values
            var loanBalance = this.ChangeInterestAccount.BalanceToCarryOver;
            this.ChangeInterestAccount.NewLoanAmount = loanBalance;
            var term1 = this.ChangeInterestAccount.Term;
            int termUnit1 = this.ChangeInterestAccount.UnitId;
            int interestTerm1 = this.ChangeInterestAccount.InterestRateDescriptionId;
            int paymentMode1 = this.ChangeInterestAccount.PaymentModeId;
            string paymentMode = this.ChangeInterestAccount.PaymentMode;
            var loanReleaseDate = this.ChangeInterestAccount.LoanReleaseDate.AddMonths(-1);
            var dtManager1 = new DateTimeOperationManager(paymentMode, loanReleaseDate);
            //if (dtManager1.Increment() == this.ChangeIcmAccount.PaymentStartDate)
            //this.ChangeInterestAccount.PaymentStartDate = dtManager1.Increment();
            this.ChangeInterestAccount.PaymentStartDate = DateTimeOperationManager.AdjustPaymentStartDate(loanReleaseDate.AddMonths(1));
            var paymentStartDate = this.ChangeInterestAccount.PaymentStartDate;
            var dtManager = new DateTimeOperationManager(paymentMode, paymentStartDate);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.ChangeInterestAccount.InterestComputationMode;
            options.InterestRate = this.ChangeInterestAccount.NewInterestRate;
            options.InterestRateDescription = this.ChangeInterestAccount.InterestRateDescription;
            options.InterestRateDescriptionId = this.ChangeInterestAccount.InterestRateDescriptionId;
            options.LoanAmount = this.ChangeInterestAccount.NewLoanAmount;
            options.LoanReleaseDate = this.ChangeInterestAccount.LoanReleaseDate;//.AddMonths(-5);
            options.LoanTerm = this.ChangeInterestAccount.RemainingPayments;
            options.LoanTermId = this.ChangeInterestAccount.UnitId;
            options.PaymentMode = this.ChangeInterestAccount.PaymentMode;
            options.PaymentModeId = this.ChangeInterestAccount.PaymentModeId;
            options.PaymentStartDate = this.ChangeInterestAccount.PaymentStartDate;

            LoanCalculator calc = new LoanCalculator();
            var model = calc.CalculateLoan(options);
            int i = 0;

            foreach (var item in model)
            {
                //Amortization Schedule Items
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddChangeInterestAmortizationScheduleItem(amortizationModel);
                i++;

                this.ChangeInterestAccount.InterestPaymentTotal = item.InterestPaymentTotal;
            }
        }

        public void GenerateChangeIcmAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId 
                                                && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var amortizationSchedule = agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.EndDate == null);
            var amortItems = amortizationSchedule.AmortizationScheduleItems.FirstOrDefault(entity => entity.IsBilledIndicator == false);

            Application oldApplication = agreement.Application;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.CollateralRequirementType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.MethodofChargingInterestType);
            var aiInterestRateDescription = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestRateType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestComputationModeType);
            this.FinancialProductId1 = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;
            var financialProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct; 


            FillAmortizationItemModel(this.ChangeIcmAccount, agreementItem);
            this.ChangeIcmAccount.InterestRate = this.ChangeIcmAccount.NewInterestRate;
            this.ChangeIcmAccount.InterestComputationModeId = aiInterestComputationMode.ProductFeatureApplicability.Id;
            this.ChangeIcmAccount.CollateralRequirement = aiCollateralRequirement.ProductFeatureApplicability.ProductFeature.Name;
            this.ChangeIcmAccount.CollateralRequirementId = aiCollateralRequirement.ProductFeatureApplicability.Id;
            this.ChangeIcmAccount.MethodOfChargingInterest = aiMethodOfChargingInterest.ProductFeatureApplicability.ProductFeature.Name;
            this.ChangeIcmAccount.MethodOfChargingInterestId = aiMethodOfChargingInterest.ProductFeatureApplicability.Id;
            this.ChangeIcmAccount.InterestRateDescriptionId = aiInterestRateDescription.ProductFeatureApplicability.ProductFeatureId;
            this.ChangeIcmAccount.Unit = UnitOfMeasure.MonthlyType.Name;
            this.ChangeIcmAccount.UnitId = UnitOfMeasure.MonthlyType.Id;
            this.ChangeIcmAccount.Unit = UnitOfMeasure.MonthsType.Name;
            this.ChangeIcmAccount.UnitId = UnitOfMeasure.MonthsType.Id;
            this.ChangeIcmAccount.PaymentMode = UnitOfMeasure.MonthlyType.Name;
            this.ChangeIcmAccount.PaymentModeId = UnitOfMeasure.MonthlyType.Id;
            if (agreementItem.InterestComputationMode == ProductFeature.StraightLineMethodType.Name)
            {
                this.ChangeIcmAccount.InterestComputationMode = ProductFeature.DiminishingBalanceMethodType.Name;
                this.ChangeIcmAccount.InterestComputationModeId = Context.ProductFeatureApplicabilities.FirstOrDefault(entity => entity.FinancialProductId == financialProduct.Id && entity.ProductFeatureId == ProductFeature.DiminishingBalanceMethodType.Id).Id;
            }
            else if (agreementItem.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name)
            {
                this.ChangeIcmAccount.InterestComputationMode = ProductFeature.StraightLineMethodType.Name;
                this.ChangeIcmAccount.InterestComputationModeId = Context.ProductFeatureApplicabilities.FirstOrDefault(entity => entity.FinancialProductId == financialProduct.Id && entity.ProductFeatureId == ProductFeature.StraightLineMethodType.Id).Id;
            }
            this.ChangeIcmAccount.LoanReleaseDate = amortizationSchedule.LoanReleaseDate;//.AddMonths(-1);

            ////this.ChangeIcmAccount.BalanceToCarryOver = agreementItem.LoanAmount;
            //this.ChangeIcmAccount.InterestComputationMode = agreementItem.InterestComputationMode;

            //if (agreementItem.InterestComputationMode == ProductFeature.StraightLineMethodType.Name)
            //    this.ChangeIcmAccount.InterestComputationMode = ProductFeature.DiminishingBalanceMethodType.Name;
            //else if (agreementItem.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name)
            //    this.ChangeIcmAccount.InterestComputationMode = ProductFeature.StraightLineMethodType.Name;

            //this.ChangeIcmAccount.InterestRate = agreementItem.InterestRate;
            //this.ChangeIcmAccount.Term = agreementItem.LoanTermLength;
            //this.ChangeIcmAccount.Unit = agreementItem.LoanTermUom;
            //this.ChangeIcmAccount.UnitId = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType).SingleOrDefault(entity =>
            //                                                entity.Name == this.ChangeIcmAccount.Unit).Id;
            //this.ChangeIcmAccount.InterestRateDescription = agreementItem.InterestRateDescription;
            //this.ChangeIcmAccount.InterestRateDescriptionId = ProductFeature.All(ProductFeatureCategory.InterestRateType).SingleOrDefault(entity =>
            //                                                entity.Name == this.ChangeIcmAccount.InterestRateDescription).Id;
            //this.ChangeIcmAccount.PaymentMode = agreementItem.PaymentMode;
            //this.ChangeIcmAccount.PaymentModeId = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType).SingleOrDefault(entity =>
            //                                                entity.Name == this.ChangeIcmAccount.PaymentMode).Id;
            //this.ChangeIcmAccount.MethodOfChargingInterest = agreementItem.MethodOfChargingInterest;
            //this.ChangeIcmAccount.PaymentStartDate = amortItems.ScheduledPaymentDate;
            //this.ChangeIcmAccount.LoanReleaseDate = amortizationSchedule.LoanReleaseDate;

            //Initial Values
            var loanBalance = this.ChangeIcmAccount.BalanceToCarryOver;
            this.ChangeIcmAccount.NewLoanAmount = loanBalance;
            var term1 = this.ChangeIcmAccount.Term;
            int termUnit1 = this.ChangeIcmAccount.UnitId;
            int interestTerm1 = this.ChangeIcmAccount.InterestRateDescriptionId;
            int paymentMode1 = this.ChangeIcmAccount.PaymentModeId;
            string paymentMode = this.ChangeIcmAccount.PaymentMode;
            var loanReleaseDate = this.ChangeIcmAccount.LoanReleaseDate.AddMonths(-1);
            var dtManager1 = new DateTimeOperationManager(paymentMode, loanReleaseDate);
            //if(dtManager1.Increment() == this.ChangeIcmAccount.PaymentStartDate)
            //this.ChangeIcmAccount.PaymentStartDate = dtManager1.Increment();
            this.ChangeIcmAccount.PaymentStartDate = DateTimeOperationManager.AdjustPaymentStartDate(loanReleaseDate.AddMonths(1));
            var paymentStartDate = this.ChangeIcmAccount.PaymentStartDate;
            var dtManager = new DateTimeOperationManager(paymentMode, paymentStartDate);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.ChangeIcmAccount.InterestComputationMode;
            options.InterestRate = this.ChangeIcmAccount.InterestRate;
            options.InterestRateDescription = this.ChangeIcmAccount.InterestRateDescription;
            options.InterestRateDescriptionId = this.ChangeIcmAccount.InterestRateDescriptionId;
            options.LoanAmount = this.ChangeIcmAccount.NewLoanAmount;
            options.LoanReleaseDate = this.ChangeIcmAccount.LoanReleaseDate;
            options.LoanTerm = this.ChangeIcmAccount.RemainingPayments;
            options.LoanTermId = this.ChangeIcmAccount.UnitId;
            options.PaymentMode = this.ChangeIcmAccount.PaymentMode;
            options.PaymentModeId = this.ChangeIcmAccount.PaymentModeId;
            options.PaymentStartDate = this.ChangeIcmAccount.PaymentStartDate;

            LoanCalculator calc = new LoanCalculator();
            var model = calc.CalculateLoan(options);
            int i = 0;

            foreach (var item in model)
            {
                //Amortization Schedule Items
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddChangeIcmAmortizationScheduleItem(amortizationModel);
                i++;

                this.ChangeIcmAccount.InterestPaymentTotal = item.InterestPaymentTotal;
            }
        }

        public void GenerateConsolidateLoanAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId);
            LoanAccount loanAccount = financialAccount.LoanAccount;

            //Consolidate
            var loanBalance = this.ConsolidateLoanAccount.BalanceToCarryOver;
            this.ConsolidateLoanAccount.NewLoanAmount = loanBalance;
            var term = this.ConsolidateLoanAccount.Term;
            int termUnit = this.ConsolidateLoanAccount.UnitId;
            int interestTerm = this.ConsolidateLoanAccount.InterestRateDescriptionId;
            int paymentMode = this.ConsolidateLoanAccount.PaymentModeId;
            string paymentModeName = this.ConsolidateLoanAccount.PaymentMode;
            this.ConsolidateLoanAccount.LoanReleaseDate = (DateTime)loanAccount.LoanReleaseDate;
            var loanReleaseDate = this.ConsolidateLoanAccount.LoanReleaseDate;
            //var dtManager1 = new DateTimeOperationManager(paymentModeName, loanReleaseDate);
            //this.ConsolidateLoanAccount.PaymentStartDate = dtManager1.Increment();
            var paymentStartDate = this.ConsolidateLoanAccount.PaymentStartDate;
            var dtManager = new DateTimeOperationManager(paymentModeName, paymentStartDate);

            //Loan Calculator Options Initialization
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.ConsolidateLoanAccount.InterestComputationMode;
            options.InterestRate = this.ConsolidateLoanAccount.InterestRate;
            options.InterestRateDescription = this.ConsolidateLoanAccount.InterestRateDescription;
            options.InterestRateDescriptionId = this.ConsolidateLoanAccount.InterestRateDescriptionId;
            options.LoanAmount = this.ConsolidateLoanAccount.NewLoanAmount;
            options.LoanReleaseDate = this.ConsolidateLoanAccount.LoanReleaseDate;
            options.LoanTerm = this.ConsolidateLoanAccount.Term;
            options.LoanTermId = this.ConsolidateLoanAccount.UnitId;
            options.PaymentMode = this.ConsolidateLoanAccount.PaymentMode;
            options.PaymentModeId = this.ConsolidateLoanAccount.PaymentModeId;
            options.PaymentStartDate = this.ConsolidateLoanAccount.PaymentStartDate;

            LoanCalculator calc = new LoanCalculator();
            var model = calc.CalculateLoan(options);
            int i = 0;

            foreach (var item in model)
            {
                //Amortization Schedule Items
                AmortizationScheduleModel amortizationModel = new AmortizationScheduleModel();
                amortizationModel.Counter = item.ItemNumber;
                amortizationModel.InterestPayment = item.InterestPayment;
                amortizationModel.InterestPaymentTotal = item.InterestPaymentTotal;
                amortizationModel.IsBilledIndicator = false;
                amortizationModel.PrincipalBalance = item.PrincipalBalance;
                amortizationModel.PrincipalPayment = item.PrincipalPayment;
                amortizationModel.ScheduledPaymentDate = (i == 0) ? dtManager.Current : dtManager.Increment();
                amortizationModel.TotalLoanBalance = item.TotalLoanBalance;
                amortizationModel.TotalPayment = item.TotalPayment;

                AddConsolidateLoanAmortizationScheduleItem(amortizationModel);
                i++;

                this.ConsolidateLoanAccount.InterestPaymentTotal = item.InterestPaymentTotal;
            }
        }

        public LoanRestructureListModel RetrieveLoanRestructureList(string randomKey)
        {
            return this.LoanRestructuredView.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public void AddLoanRestructureList(LoanRestructureListModel model)
        {
            if (this.LoanRestructuredView.Contains(model) == false)
                this.LoanRestructuredView.Add(model);
        }

        public void AddAmortizationScheduleItem1(AmortizationScheduleModel model)
        {
            if (this.SplitLoanAmortizationSchedule1.Contains(model) == false)
                this.SplitLoanAmortizationSchedule1.Add(model);
        }

        public void AddAmortizationScheduleItem2(AmortizationScheduleModel model)
        {
            if (this.SplitLoanAmortizationSchedule2.Contains(model) == false)
                this.SplitLoanAmortizationSchedule2.Add(model);
        }

        public void AddAdditionalLoanAmortizationScheduleItem(AmortizationScheduleModel model)
        {
            if (this.AdditionalLoanAmortizationSchedule.Contains(model) == false)
                this.AdditionalLoanAmortizationSchedule.Add(model);
        }

        public void AddChangeInterestAmortizationScheduleItem(AmortizationScheduleModel model)
        {
            if (this.ChangeInterestAmortizationSchedule.Contains(model) == false)
                this.ChangeInterestAmortizationSchedule.Add(model);
        }

        public void AddChangeIcmAmortizationScheduleItem(AmortizationScheduleModel model)
        {
            if (this.ChangeIcmAmortizationSchedule.Contains(model) == false)
                this.ChangeIcmAmortizationSchedule.Add(model);
        }

        public void AddConsolidateLoanAmortizationScheduleItem(AmortizationScheduleModel model)
        {
            if (this.ConsolidateLoanAmortizationSchedule.Contains(model) == false)
                this.ConsolidateLoanAmortizationSchedule.Add(model);
        }

        public void AddChequesListItem(ChequeModel model)
        {
            if (this.Cheques.Contains(model) == false)
                this.Cheques.Add(model);
        }

        public void AddCheques1(ChequeModel model)
        {
            if (this.Cheques1.Contains(model) == false)
                this.Cheques1.Add(model);
        }

        public void AddCheques2(ChequeModel model)
        {
            if (this.Cheques2.Contains(model) == false)
                this.Cheques2.Add(model);
        }

        public int ValidateCheckNumber(string checkNumber, int type, string randomKey)
        {
            //if valid return 0, else return 1
            int value = 0;
            //List<ChequeModel> model = new List<ChequeModel>();
            if (type == 1)
            {
                foreach (var item in this.Cheques)
                {
                    if (item.ChequeNumber == checkNumber && item.RandomKey != randomKey)
                    {
                        value = 1;
                        break;
                    }
                }
            }
            else if (type == 2)
            {
                foreach (var item in this.Cheques1)
                {
                    if (item.ChequeNumber == checkNumber && item.RandomKey != randomKey)
                    {
                        value = 1;
                        break;
                    }
                }

                foreach (var item in this.Cheques2)
                {
                    if (item.ChequeNumber == checkNumber && item.RandomKey != randomKey)
                    {
                        value = 1;
                        break;
                    }
                }
            }

            return value;
        }

        public LoanRestructureForm()
        {
            LoanRestructuredView = new List<LoanRestructureListModel>();
            SplitLoanAmortizationSchedule1 = new List<AmortizationScheduleModel>();
            SplitLoanAmortizationSchedule2 = new List<AmortizationScheduleModel>();
            AdditionalLoanAmortizationSchedule = new List<AmortizationScheduleModel>();
            ChangeIcmAmortizationSchedule = new List<AmortizationScheduleModel>();
            ChangeInterestAmortizationSchedule = new List<AmortizationScheduleModel>();
            ConsolidateLoanAmortizationSchedule = new List<AmortizationScheduleModel>();
            SplitLoanItemsAccount1 = new AmortizationItemsModel();
            SplitLoanItemsAccount2 = new AmortizationItemsModel();
            ChangeIcmAccount = new AmortizationItemsModel();
            ChangeInterestAccount = new AmortizationItemsModel();
            AdditionalLoanAccount = new AmortizationItemsModel();
            ConsolidateLoanAccount = new AmortizationItemsModel();
            Fees = new List<LoanFeeModel>();
            Collaterals = new List<Collateral>();
            Guarantors = new List<PersonnalPartyModel>();
            CoBorrowers = new List<PersonnalPartyModel>();
            SubmittedDocuments = new List<SubmittedDocumentModel>();
            Cheques = new List<ChequeModel>();
            Cheques1 = new List<ChequeModel>();
            Cheques2 = new List<ChequeModel>();
        }

        public void RetrieveLoanRestructureList(int id)
        {
            //retrieve financial accounts of the customer
            var customer = PartyRole.GetById(id);
            this.CustomerId = customer.Id;

            var allowedLoanAccountStatus = Context.LoanAccountStatusTypeAssocs.Where(entity => 
                                                    entity.ToStatusTypeId == LoanAccountStatusType.RestructuredType.Id 
                                                    && entity.EndDate == null && entity.FromStatusTypeId != LoanAccountStatusType.RestructuredType.Id);

            var financialAccounts = from fa in Context.FinancialAccounts.Where(entity =>
                                            entity.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id)
                                    join far in Context.FinancialAccountRoles on fa.Id equals far.FinancialAccountId
                                    join pr in Context.PartyRoles on far.PartyRoleId equals pr.Id
                                    join las in Context.LoanAccountStatus on fa.Id equals las.FinancialAccountId
                                    join als in allowedLoanAccountStatus on las.StatusTypeId equals als.FromStatusTypeId
                                    join agrmt in Context.Agreements on fa.AgreementId equals agrmt.Id
                                    join ldv in Context.LoanDisbursementVcrs on agrmt.Id equals ldv.AgreementId
                                    join ldvs in Context.DisbursementVcrStatus on ldv.Id equals ldvs.LoanDisbursementVoucherId
                                    join app in Context.Applications on agrmt.ApplicationId equals app.Id
                                    join lapp in Context.LoanApplications on app.Id equals lapp.ApplicationId
                                    where las.IsActive == true && ldvs.IsActive == true 
                                    && ldvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.FullyDisbursedType.Id
                                    && agrmt.EndDate == null && agrmt.AgreementTypeId == AgreementType.LoanAgreementType.Id 
                                    && app.ApplicationType == ApplicationType.LoanApplicationType.Id
                                    && pr.PartyId == customer.PartyId && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                                    && pr.EndDate == null
                                    select new { FAs = fa, Agrmnts = agrmt, Apps = app };

            ////financialAccounts.Count();
            //var loanAccounts = from la in Context.FinancialAccountRoles
            //                       join o in owner on la.PartyRoleId equals o.Id
            //                       join fa in financialAccounts on la.FinancialAccountId equals fa.FAs.Id
            //                       select new { FAs = fa.FAs, Agrmnts = fa.Agrmnts, Apps = fa.Apps };



            foreach (var loanRes in financialAccounts)
            {
                LoanRestructureListModel loanRestructured = new LoanRestructureListModel();
                //var count = loanRes.Agrmnts.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => 
                //                            entity.EndDate == null).AmortizationScheduleItems.Where(entity => entity.IsBilledIndicator == false).Count();
                //if (count != 0)
                //{
                    loanRestructured.LoanID = loanRes.FAs.Id;
                    loanRestructured.LoanAmount = loanRes.FAs.LoanAccount.LoanAmount;
                    //loanRestructured.MaturityDate = loanRes.FAs.LoanAccount.MaturityDate;
                    loanRestructured.MaturityDate = loanRes.FAs.LoanAccount.LoanReleaseDate;
                    loanRestructured.LoanReleaseDate = loanRes.FAs.LoanAccount.LoanReleaseDate.Value.ToString("yyyy-MM-dd");
                    //loanRestructured.ProductTerm = loanRes.Apps.LoanApplication.LoanTermLength.ToString() + " " + loanRes.Agrmnts.AgreementItems.SingleOrDefault(entity => entity.AgreementId == loanRes.Agrmnts.Id && entity.IsActive == true).LoanTermUom;
                    loanRestructured.TotalLoanBalance = loanRes.FAs.LoanAccount.LoanBalance;
                    var loanAccount = loanRes.FAs.LoanAccount;

                    #region Commented Code
                    //var noOfInstallmentsz = loanRes.FAs.LoanAccount.FinancialAccount.Agreement.LoanAgreement.AmortizationSchedules.FirstOrDefault(entity => entity.EndDate == null);
                    //if (noOfInstallmentsz == null)
                    //    noOfInstallmentsz = loanRes.FAs.LoanAccount.FinancialAccount.Agreement.LoanAgreement.AmortizationSchedules.FirstOrDefault(entity => entity.EndDate != null);
                    //int noOfInstallments = 0;
                    //if (noOfInstallmentsz != null)
                    //    noOfInstallments = (int)noOfInstallmentsz.AmortizationScheduleItems.Count();
                    //var paidInstallments = Math.Round((loanAccount.LoanAmount - loanAccount.LoanBalance) / (loanAccount.LoanAmount / noOfInstallments), 0);
                    #endregion

                    var agreementItem = loanRes.Agrmnts.AgreementItems.FirstOrDefault(entity => entity.AgreementId == loanRes.Agrmnts.Id && entity.IsActive == true);
                    loanRestructured.InterestComputationMode = agreementItem.InterestComputationMode;
                    if (loanAccount.InterestType != null)
                    {
                        if (loanAccount.InterestTypeId == InterestType.FixedInterestTYpe.Id)
                            loanRestructured.InterestRate = "Php " + loanAccount.InterestItems.FirstOrDefault(entity => entity.IsActive == true).Amount.ToString();
                        else if (loanAccount.InterestTypeId == InterestType.ZeroInterestTYpe.Id)
                            loanRestructured.InterestRate = "0.00";
                        else if (loanAccount.InterestTypeId == InterestType.PercentageInterestTYpe.Id)
                            loanRestructured.InterestRate = agreementItem.InterestRate.ToString("N") + "%";
                    }
                    else
                    {
                        loanRestructured.InterestRate = agreementItem.InterestRate.ToString("N") + "%";
                    }

                    #region Commented Code
                    //var receivableTotal = (from sar in loanRes.FAs.LoanAccount.Receivables
                    //                       join sars in Context.ReceivableStatus on sar.Id equals sars.ReceivableId
                    //                       where (sar.ReceivableTypeId == ReceivableType.InterestType.Id ||
                    //                       sar.ReceivableTypeId == ReceivableType.PastDueType.Id)
                    //                       && (sars.StatusTypeId == ReceivableStatusType.OpenType.Id || sars.StatusTypeId == ReceivableStatusType.PartiallyPaidType.Id)
                    //                       && sars.IsActive == true
                    //                       group sar by sar.FinancialAccountId into g
                    //                       select new { TotalReceivableBalance = g.Sum(entity => entity.Balance) }).SingleOrDefault();

                    //var remainingPayments = (from asi in loanRes.Agrmnts.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity =>
                    //                            entity.EndDate == null).AmortizationScheduleItems
                    //                         where asi.IsBilledIndicator == false
                    //                         group asi by asi.AmortizationScheduleId into a
                    //                         select new { TotalRemainingPayment = a.Count(), LoanBalance = a.Sum(entity => entity.PrincipalPayment) }).SingleOrDefault();

                    //if(receivableTotal != null)
                    //   loanRestructured.TotalLoanBalance += receivableTotal.TotalReceivableBalance;
                    //loanRestructured.RemainingPayments = remainingPayments.TotalRemainingPayment.ToString();
                    //var newLoanBalance = ComputeNewLoanBalance(loanAccount, 4.00M);
                    //loanRestructured.TotalLoanBalance = newLoanBalance;
                    //loanRestructured.RemainingPayments = (noOfInstallments - paidInstallments).ToString();
                    #endregion

                    AddLoanRestructureList(loanRestructured);
                //}
            }
        }

        public override void PrepareForSave()
        {
            throw new NotImplementedException();
        }

        public void SaveSplitAccountAmortizationSchedule(DateTime today)
        {
            //Old Financial Account Instance
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId);
            financialAccount.LoanAccount.CurrentStatus.LoanAccountStatusType = LoanAccountStatusType.RestructuredType;
            financialAccount.LoanAccount.CurrentStatus.TransitionDateTime = today;

            LoanApplication oldLoanApplication = financialAccount.Agreement.Application.LoanApplication;

            //end old loan application status and change to 'restructured'
            LoanApplicationStatu oldLoanApplicationStatus = oldLoanApplication.CurrentStatus;
            oldLoanApplicationStatus.IsActive = false;

            LoanApplicationStatu newLoanApplicationStatusOld = new LoanApplicationStatu();
            newLoanApplicationStatusOld.LoanApplication = oldLoanApplication;
            newLoanApplicationStatusOld.LoanApplicationStatusType = LoanApplicationStatusType.RestructuredType;
            newLoanApplicationStatusOld.TransitionDateTime = today;
            newLoanApplicationStatusOld.IsActive = true;

            PartyRole customerPartyRole = PartyRole.GetById(this.CustomerId);
            
            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication);
            RetrieveCoBorrower(oldLoanApplication);
            RetrieveCollaterals(oldLoanApplication, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication);
            RetrieveSubmittedDocumentsForm(oldLoanApplication);

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole ownerPartyRole = PartyRole.GetById(this.CustomerId);
            ownerPartyRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.OwnerFinancialType);
            PartyRole customerBorrowerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, borrowerRoleType);
            PartyRole customerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.CustomerType);

            Customer customer = customerRole.Customer;
            LoanAccount loanAccountOld = financialAccount.LoanAccount;

            ChangeCustomerStatus(customer, loanAccountOld, today);

            Agreement oldAgreement = financialAccount.Agreement;

            var receivables = financialAccount.LoanAccount.Receivables;

            //cancel previous receivables
            CancelReceivables(loanAccountOld, today);
            
            //New Application instance for Split Account1
            Application applicationNew1 = CreateNewApplication(today);

            var financialProduct1 = FinancialProduct.GetById(this.FinancialProductId1);

            //Add Application Items for Split Account
            AddApplicationItems(today, applicationNew1, financialProduct1, this.SplitLoanItemsAccount1);

            //New Loan Application instance for Split Account 1
            LoanApplication loanApplicationNew1 = CreateLoanApplication(applicationNew1, today, this.SplitLoanItemsAccount1);

            //Save coborrowers, collaterals, guarantors, and submitted documents for split account 1
            SaveFees(loanApplicationNew1, today);
            SaveCoBorrower(loanApplicationNew1, today);
            SaveCollaterals(loanApplicationNew1, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew1, today);
            SaveSubmittedDocumentsForm(loanApplicationNew1, today);

            var partyRole = PartyRole.GetById(this.ModifiedBy);
            var party = Party.GetById(partyRole.PartyId);

            //Insert loan clerk who processed the loan application 1
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower1 = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ApprovedByAgreementType, today);
            }

            //New Loan Application Status instance for Split Account 1
            LoanApplicationStatu loanApplicationStatus1 = CreateLoanApplicationStatus(loanApplicationNew1, today);

            Context.Applications.AddObject(applicationNew1);

            //New Application instance for Split Account2
            Application applicationNew2 = CreateNewApplication(today);

            var financialProduct2 = FinancialProduct.GetById(this.FinancialProductId2);

            //Add Application Items for Split Account
            AddApplicationItems(today, applicationNew2, financialProduct2, this.SplitLoanItemsAccount2);

            //New Loan Application instance for Split Account 2
            LoanApplication loanApplicationNew2 = CreateLoanApplication(applicationNew2, today, this.SplitLoanItemsAccount2);

            //Save coborrowers, collaterals, guarantors, and submitted documents for split account 2
            SaveFees(loanApplicationNew2, today);
            SaveCoBorrower(loanApplicationNew2, today);
            SaveCollaterals(loanApplicationNew2, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew2, today);
            SaveSubmittedDocumentsForm(loanApplicationNew2, today);

            //New Loan Application Status instance for Split Account 2
            LoanApplicationStatu loanApplicationStatus2 = CreateLoanApplicationStatus(loanApplicationNew2, today);

            //Insert loan clerk who processed the loan application 1
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy2 = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew2, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower2 = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew2, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy2 = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew2, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement2 = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew2, party, RoleType.ApprovedByAgreementType, today);
            }

            Context.Applications.AddObject(applicationNew2);

            //Agreement instance for Split Account 1
            Agreement agreement1 = CreateNewAgreementWithParent(applicationNew1, today, financialAccount);

            //Agreement instance for Split Account 2
            Agreement agreement2 = CreateNewAgreementWithParent(applicationNew2, today, financialAccount);

            LoanDisbursementVcr voucher1 = CreateLoanDisbursementVoucher(agreement1, this.SplitLoanItemsAccount1, today);
            DisbursementVcrStatu voucherStatus1 = CreateVoucherStatus(voucher1, today);

            LoanDisbursementVcr voucher2 = CreateLoanDisbursementVoucher(agreement2, this.SplitLoanItemsAccount2, today);
            DisbursementVcrStatu voucherStatus2 = CreateVoucherStatus(voucher2, today);

            //Borrower Party Role for Split Account 1
            PartyRole borrowerRole1 = new PartyRole();
            borrowerRole1.Party = ownerPartyRole.Party;
            borrowerRole1.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole1.EffectiveDate = today;

            //Borrower Party Role for Split Account 2
            PartyRole borrowerRole2 = new PartyRole();
            borrowerRole2.Party = ownerPartyRole.Party;
            borrowerRole2.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole2.EffectiveDate = today;

            //Agreement Role Split Account 1
            AgreementRole agreementRole1 = CreateAgreementRole(agreement1, borrowerRole1);

            //Agreement Role Split Account 2
            AgreementRole agreementRole2 = CreateAgreementRole(agreement2, borrowerRole2);

            //CoBorrowers Agreement Role
            var coBorrowers = SaveCoOwnerAgreement(today);
            foreach (var coBorrower in coBorrowers)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, coBorrower);

                AgreementRole agreementRole2s = CreateAgreementRole(agreement2, coBorrower);

                Context.AgreementRoles.AddObject(agreementRole1s);
                Context.AgreementRoles.AddObject(agreementRole2s);
            }

            //Guarantors Agreement Role
            var guarantorsA = SaveGuarantorAgreement(today);
            foreach (var guarantor in guarantorsA)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, guarantor);

                AgreementRole agreementRole2s = CreateAgreementRole(agreement2, guarantor);

                Context.AgreementRoles.AddObject(agreementRole1s);
                Context.AgreementRoles.AddObject(agreementRole2s);
            }

            //Agreement Item instance for Split Account 1
            AgreementItem agreementItemNew1 = CreateAgreementItem(agreement1, today, this.SplitLoanItemsAccount1);

            //Agreement Item instance for Split Account 2
            AgreementItem agreementItemNew2 = CreateAgreementItem(agreement2, today, this.SplitLoanItemsAccount2);

            //Loan Agreement instance for Split Account 1
            LoanAgreement loanAgreementNew1 = new LoanAgreement();
            loanAgreementNew1.Agreement = agreement1;

            //Loan Agreement instance for Split Account 2
            LoanAgreement loanAgreementNew2 = new LoanAgreement();
            loanAgreementNew2.Agreement = agreement2;

            //Financial Account instance for Split Account 1
            FinancialAccount financialAccountNew1 = CreateFinancialAccountWithParent(agreement1, today, financialAccount);

            //Financial Account instance for Split Account 2
            FinancialAccount financialAccountNew2 = CreateFinancialAccountWithParent(agreement2, today, financialAccount);

            //Financial Account Product for Split Account 1
            FinancialAccountProduct financialAccountProductNew1 = CreateFinancialAccountProduct(financialAccountNew1, financialProduct1, today);

            //Financial Account Product for Split Account 2
            FinancialAccountProduct financialAccountProductNew2 = CreateFinancialAccountProduct(financialAccountNew2, financialProduct2, today);

            //Party Role for Split Account 1
            PartyRole ownerPartyRole1 = new PartyRole();
            ownerPartyRole1.Party = ownerPartyRole.Party;
            ownerPartyRole1.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole1.EffectiveDate = today;

            //Financial Account Role Split Account 1
            FinancialAccountRole financialAccountRole1 = CreateFinancialAccountRole(financialAccountNew1, ownerPartyRole1);

            //Party Role for Split Account 2
            PartyRole ownerPartyRole2 = new PartyRole();
            ownerPartyRole2.Party = ownerPartyRole.Party;
            ownerPartyRole2.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole2.EffectiveDate = today;

            //CoOwners Financial Account Role
            var coOwners = SaveCoOwnerFinancial(today);
            foreach (var owners in coOwners)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, owners);

                FinancialAccountRole financialAccountRole2s = CreateFinancialAccountRole(financialAccountNew2, owners);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
                Context.FinancialAccountRoles.AddObject(financialAccountRole2s);
            }

            //Guarantors Financial Account Role
            var guarantors = SaveGuarantorFinancial(today);
            foreach (var guarantor in guarantors)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, guarantor);

                FinancialAccountRole financialAccountRole2s = CreateFinancialAccountRole(financialAccountNew2, guarantor);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
                Context.FinancialAccountRoles.AddObject(financialAccountRole2s);
            }

            //Financial Account Role Split Account 2
            FinancialAccountRole financialAccountRole2 = CreateFinancialAccountRole(financialAccountNew2, ownerPartyRole2);

            var maturityDate1 = DateTime.Today;
            if (this.SplitLoanAmortizationSchedule1.Count() != 0)
                maturityDate1 = this.SplitLoanAmortizationSchedule1.ToList().Last().ScheduledPaymentDate;
            var maturityDate2 = DateTime.Today;
            if (this.SplitLoanAmortizationSchedule2.Count() != 0)
                maturityDate2 = this.SplitLoanAmortizationSchedule2.ToList().Last().ScheduledPaymentDate;
            var lastSchedAccount1 = maturityDate1;
            var lastSchedAccount2 = maturityDate2;

            //Loan Account instance for Split Account 1
            LoanAccount loanAccountNew1 = CreateLoanAccount(financialAccountNew1, this.SplitLoanItemsAccount1, lastSchedAccount1, today);

            //CarryOverReceivables(loanAccountOld, loanAccountNew1, today);
            //Loan Account instance for Split Account 2
            LoanAccount loanAccountNew2 = CreateLoanAccount(financialAccountNew2, this.SplitLoanItemsAccount2, lastSchedAccount2, today);

            //Loan Account Status instance for Split Account 1
            LoanAccountStatu loanAccountStatus1 = CreateLoanAccountStatus(loanAccountNew1, today);


            //Loan Account Status instance for Split Account 2
            LoanAccountStatu loanAccountStatus2 = CreateLoanAccountStatus(loanAccountNew2, today);


            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance for both Split Accounts
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.SplitType);


            //Loan Modification Status instance for both Split Accounts
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);

            //Loan Modification Prev Item instance for both Split Accounts
            LoanModificationPrevItem loanModificationPrevItem = CreateLoanModificationPrevItem(loanModification, financialAccount);
            
            Context.LoanModifications.AddObject(loanModification);

            //Loan Modification New Item instance for Split Account 1

            
            //Loan Modification New Item instance for Split Account 2


            //Old Amortization Schedule
            AmortizationSchedule oldAmortizationSched = financialAccount.Agreement.LoanAgreement.AmortizationSchedules
                                                        .SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId
                                                        && entity.EndDate == null);
            oldAmortizationSched.EndDate = today;

            //Amortization Schedule for Split Account 1
            AmortizationSchedule amortizationScheduleNew1 = CreateAmortizationScheduleWithParent(loanAgreementNew1, this.SplitLoanItemsAccount1, today, oldAmortizationSched);

            //Amortization Schedule for Split Account 2
            AmortizationSchedule amortizationScheduleNew2 = CreateAmortizationScheduleWithParent(loanAgreementNew2, this.SplitLoanItemsAccount2, today, oldAmortizationSched);

            //Amortization Schedule Items for Split Account 1
            //add foreach here <<<<<<<<
            //>>>>>>>>>>>>>>>>>>>
            var amoItems1 = this.SplitLoanAmortizationSchedule1;
            
            
            foreach (var amortizationItem in amoItems1)
            {
                AmortizationScheduleItem amortizationItemSplitAcct1 = CreateAmortizationScheduleItem(amortizationScheduleNew1, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemSplitAcct1);
            }

            //Amortization Schedule Items for Split Account 2
            //add foreach here <<<<<<<<
            //>>>>>>>>>>>>>>>>>>>
            var amoItems2 = this.SplitLoanAmortizationSchedule2;
            

            foreach (var amortizationItem in amoItems2)
            {
                AmortizationScheduleItem amortizationItemSplitAcct2 = CreateAmortizationScheduleItem(amortizationScheduleNew2, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemSplitAcct2);
            }

            SetChequeStatusToCancelled(financialAccount, today);

            SaveCheques1(financialAccountNew1, customerPartyRole, today);

            SaveCheques2(financialAccountNew2, customerPartyRole, today);
        }

        public void SaveAdditionalLoanAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;

            var loanBalance = this.AdditionalLoanAccount.BalanceToCarryOver + this.AdditionalLoanAccount.AdditionalAmount;
            //this.AdditionalLoanAccount.NewLoanAmount = loanBalance;

            LoanDisbursementVcr loanDisbursementVoucher = agreement.LoanDisbursementVcrs.FirstOrDefault(entity => entity.AgreementId == agreement.Id);
            loanDisbursementVoucher.Amount += this.AdditionalLoanAccount.AdditionalAmount;
            loanDisbursementVoucher.Balance += this.AdditionalLoanAccount.AdditionalAmount;
            loanDisbursementVoucher.CurrentStatus.IsActive = false;

            DisbursementVcrStatu disbursementVoucherStatus = new DisbursementVcrStatu();
            disbursementVoucherStatus.LoanDisbursementVcr = loanDisbursementVoucher;
            disbursementVoucherStatus.DisbursementVcrStatusType = DisbursementVcrStatusEnum.PartiallyDisbursedType;
            disbursementVoucherStatus.TransitionDateTime = today;
            disbursementVoucherStatus.IsActive = true;

            AgreementItem agreementItemOld = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            agreementItemOld.IsActive = false;

            var receivables = financialAccount.LoanAccount.Receivables;

            AgreementItem agreementItemNew = CreateAgreementItemFromOld(agreement, today, this.AdditionalLoanAccount, agreementItemOld);

            agreement.Application.LoanApplication.LoanAmount = this.AdditionalLoanAccount.NewLoanAmount;

            LoanApplication loanApplication = agreement.Application.LoanApplication;

            LoanAgreement loanAgreement = financialAccount.Agreement.LoanAgreement;

            //Amortization Schedule
            //AmortizationSchedule amortizationScheduleNew = CreateAmortizationScheduleWithParent(loanAgreement, this.AdditionalLoanAccount, today, oldAmortizationSched);

            //Amortization Schedule Items
            //add foreach here <<<<<<<<
            //>>>>>>>>>>>>>>>>>>>
            //var amoItems2 = this.AdditionalLoanAmortizationSchedule;
            
            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance for Additional Loan
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.AdditionalLoanType);

            //Loan Modification Status instance for Additional Loan
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);

            //Addendum
            Addendum addendum = new Addendum();
            addendum.Agreement = agreement;
            addendum.AgreementItem = agreementItemNew;

            Context.Addenda.AddObject(addendum);

            Context.LoanModifications.AddObject(loanModification);

            //Context.AmortizationSchedules.AddObject(amortizationScheduleNew);
        }

        public void SaveAdditionalLoanAmortizationScheduleWithTerm(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);

            PartyRole customerPartyRole = PartyRole.GetById(this.CustomerId);
            LoanApplication oldLoanApplication1 = financialAccount.Agreement.Application.LoanApplication;

            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication1);
            RetrieveCoBorrower(oldLoanApplication1);
            RetrieveCollaterals(oldLoanApplication1, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication1);
            RetrieveSubmittedDocumentsForm(oldLoanApplication1);

            LoanAccount oldLoanAccount = financialAccount.LoanAccount;

            Agreement agreement = financialAccount.Agreement;
            //agreement.Application.LoanApplication.InterestRate = this.ChangeInterestAccount.NewInterestRate;

            AgreementItem agreementItemOld = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            //agreementItemOld.IsActive = false;
            Application oldApplication = agreement.Application;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.CollateralRequirementType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.MethodofChargingInterestType);
            var aiInterestRateDescription = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestRateType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestComputationModeType);
            this.FinancialProductId1 = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;
            var financialProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct;
            this.AdditionalLoanAccount.CollateralRequirement = aiCollateralRequirement.ProductFeatureApplicability.ProductFeature.Name;
            this.AdditionalLoanAccount.CollateralRequirementId = aiCollateralRequirement.ProductFeatureApplicability.Id;
            //Application instance
            Application applicationNew1 = CreateNewApplication(today);

            var applicationItems = Context.ApplicationItems.FirstOrDefault(entity => entity.ApplicationId == oldApplication.Id && entity.EndDate == null).ProductFeatureApplicability;

            AddApplicationItems(today, applicationNew1, financialProduct, this.AdditionalLoanAccount);

            LoanApplication loanApplicationNew = CreateLoanApplication(applicationNew1, today, this.AdditionalLoanAccount);

            SaveFees(loanApplicationNew, today);
            SaveCoBorrower(loanApplicationNew, today);
            SaveCollaterals(loanApplicationNew, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew, today);
            SaveSubmittedDocumentsForm(loanApplicationNew, today);

            LoanApplicationStatu loanApplicationStatusNew = CreateLoanApplicationStatus(loanApplicationNew, today);

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole ownerPartyRole = PartyRole.GetById(this.CustomerId);
            PartyRole customerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.CustomerType);

            Customer customer = customerRole.Customer;
            LoanAccount loanAccountOld = financialAccount.LoanAccount;

            //Loan Application Role
            var partyRole = PartyRole.GetById(this.ModifiedBy);
            var party = Party.GetById(partyRole.PartyId);

            //Insert loan clerk who processed the loan application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByAgreementType, today);
            }

            Context.Applications.AddObject(applicationNew1);

            //Agreement instance
            Agreement agreement1 = CreateNewAgreement(applicationNew1, today);


            LoanDisbursementVcr voucher1 = CreateLoanDisbursementVoucher(agreement1, this.AdditionalLoanAccount, today, this.AdditionalLoanAccount.NewLoanAmount);
            DisbursementVcrStatu voucherStatus1 = CreateVoucherStatus(voucher1, today, DisbursementVcrStatusEnum.ApprovedType);
            //DisbursementVcrStatu voucherStatus1 = new DisbursementVcrStatu()
            //CancelReceivables(financialAccount.LoanAccount, today);

            AgreementItem agreementItemNew = CreateAgreementItem(agreement1, today, this.AdditionalLoanAccount);

            //Borrower Party Role
            PartyRole borrowerRole1 = new PartyRole();
            borrowerRole1.Party = ownerPartyRole.Party;
            borrowerRole1.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole1.EffectiveDate = today;

            //Agreement Role
            AgreementRole agreementRole1 = CreateAgreementRole(agreement1, borrowerRole1);

            //CoBorrowers Agreement Role
            var coBorrowers = SaveCoOwnerAgreement(today);
            foreach (var coBorrower in coBorrowers)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, coBorrower);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Guarantors Agreement Account Role
            var guarantorsA = SaveGuarantorAgreement(today);
            foreach (var guarantor in guarantorsA)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, guarantor);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Old Amortization Schedule
            AmortizationSchedule oldAmortizationSched = financialAccount.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId
                    && entity.EndDate == null);
            //oldAmortizationSched.EndDate = today;

            LoanAgreement loanAgreement = financialAccount.Agreement.LoanAgreement;

            //Loan Agreement instance
            LoanAgreement loanAgreementNew1 = new LoanAgreement();
            loanAgreementNew1.Agreement = agreement1;

            //Financial Account instance for the Consolidated Loans
            FinancialAccount financialAccountNew1 = CreateFinancialAccountWithParent(agreement1, today, financialAccount);

            //Financial Account Product for Split Account 1
            FinancialAccountProduct financialAccountProductNew1 = CreateFinancialAccountProduct(financialAccountNew1, financialProduct, today);

            //Party Role
            PartyRole ownerPartyRole1 = new PartyRole();
            ownerPartyRole1.Party = ownerPartyRole.Party;
            ownerPartyRole1.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole1.EffectiveDate = today;

            //Financial Account Role
            FinancialAccountRole financialAccountRole1 = CreateFinancialAccountRole(financialAccountNew1, ownerPartyRole1);

            //CoOwners Financial Account Role
            var coOwners = SaveCoOwnerFinancial(today);
            foreach (var owners in coOwners)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, owners);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            //Guarantors Financial Account Role
            var guarantors = SaveGuarantorFinancial(today);
            foreach (var guarantor in guarantors)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, guarantor);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            var maturityDate1 = DateTime.Today;
            if (this.AdditionalLoanAmortizationSchedule.Count() != 0)
                maturityDate1 = this.AdditionalLoanAmortizationSchedule.ToList().Last().ScheduledPaymentDate;

            var lastSchedAccount1 = maturityDate1;


            //Loan Account instance
            LoanAccount loanAccountNew1 = CreateAdditionalLoanAccount(financialAccountNew1, this.AdditionalLoanAccount, lastSchedAccount1, today);

            //CarryOverReceivables(loanAccountOld, loanAccountNew1, today);

            //Loan Account Status instance
            LoanAccountStatu loanAccountStatus1 = CreateLoanAccountStatus(loanAccountNew1, today);

            //this.AdditionalLoanAccount.LoanReleaseDate = oldAmortizationSched.LoanReleaseDate;


            //Amortization Schedule
            AmortizationSchedule amortizationScheduleNew = CreateAmortizationScheduleWithParent(loanAgreementNew1, this.AdditionalLoanAccount, today, oldAmortizationSched);

            ////Amortization Schedule Items
            ////add foreach here <<<<<<<<
            ////>>>>>>>>>>>>>>>>>>>
            var amoItems2 = this.AdditionalLoanAmortizationSchedule;


            foreach (var amortizationItem in amoItems2)
            {
                AmortizationScheduleItem amortizationItemChangeInterest = CreateAmortizationScheduleItem(amortizationScheduleNew, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemChangeInterest);
            }

            //financialAccount.LoanAccount.MaturityDate = amoItems2.LastOrDefault().ScheduledPaymentDate;

            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.AdditionalLoanType);

            //Loan Modification Status instance
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);

            //Loan Modification Prev Items instance
            LoanModificationPrevItem loanModificationPrevItem = CreateLoanModificationPrevItem(loanModification, financialAccount);

            //Addendum
            Addendum addendum = new Addendum();
            addendum.Agreement = agreement;
            addendum.AgreementItem = agreementItemNew;

            //SetChequeStatusToCancelled(financialAccount, today);

            SaveCheques(financialAccountNew1, customerPartyRole, today);

            Context.Addenda.AddObject(addendum);

            Context.LoanModifications.AddObject(loanModification);
        }

        public void SaveChangeInterestAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);
            financialAccount.LoanAccount.CurrentStatus.IsActive = false;
            //financialAccount.LoanAccount.LoanBalance = this.ChangeInterestAccount.NewLoanAmount;

            PartyRole customerPartyRole = PartyRole.GetById(this.CustomerId);
            LoanApplication oldLoanApplication1 = financialAccount.Agreement.Application.LoanApplication;
            LoanApplicationStatu oldLoanApplicationStatus = oldLoanApplication1.CurrentStatus;
            oldLoanApplicationStatus.IsActive = false;

            LoanApplicationStatu newLoanApplicationStatusOld = new LoanApplicationStatu();
            newLoanApplicationStatusOld.LoanApplication = oldLoanApplication1;
            newLoanApplicationStatusOld.LoanApplicationStatusType = LoanApplicationStatusType.RestructuredType;
            newLoanApplicationStatusOld.TransitionDateTime = today;
            newLoanApplicationStatusOld.IsActive = true;



            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication1);
            RetrieveCoBorrower(oldLoanApplication1);
            RetrieveCollaterals(oldLoanApplication1, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication1);
            RetrieveSubmittedDocumentsForm(oldLoanApplication1);

            LoanAccount oldLoanAccount = financialAccount.LoanAccount;
            LoanAccountStatu newLoanAccountStatus = new LoanAccountStatu();
            newLoanAccountStatus.LoanAccount = oldLoanAccount;
            newLoanAccountStatus.LoanAccountStatusType = LoanAccountStatusType.RestructuredType;
            newLoanAccountStatus.TransitionDateTime = today;
            newLoanAccountStatus.IsActive = true;

            CancelReceivables(oldLoanAccount, today);

            Agreement agreement = financialAccount.Agreement;
            agreement.Application.LoanApplication.InterestRate = this.ChangeInterestAccount.NewInterestRate;

            AgreementItem agreementItemOld = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            //agreementItemOld.IsActive = false;
            Application oldApplication = agreement.Application;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.CollateralRequirementType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.MethodofChargingInterestType);
            var aiInterestRateDescription = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestRateType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestComputationModeType);
            this.FinancialProductId1 = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;
            var financialProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct;

            //Application instance
            Application applicationNew1 = CreateNewApplication(today);

            var applicationItems = Context.ApplicationItems.FirstOrDefault(entity => entity.ApplicationId == oldApplication.Id && entity.EndDate == null).ProductFeatureApplicability;

            AddApplicationItems(today, applicationNew1, financialProduct, this.ChangeInterestAccount);

            LoanApplication loanApplicationNew = CreateLoanApplication(applicationNew1, today, this.ChangeInterestAccount);

            SaveFees(loanApplicationNew, today);
            SaveCoBorrower(loanApplicationNew, today);
            SaveCollaterals(loanApplicationNew, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew, today);
            SaveSubmittedDocumentsForm(loanApplicationNew, today);

            LoanApplicationStatu loanApplicationStatusNew = CreateLoanApplicationStatus(loanApplicationNew, today);

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole ownerPartyRole = PartyRole.GetById(this.CustomerId);
            PartyRole customerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.CustomerType);

            Customer customer = customerRole.Customer;
            LoanAccount loanAccountOld = financialAccount.LoanAccount;

            //Loan Application Role
            var partyRole = PartyRole.GetById(this.ModifiedBy);
            var party = Party.GetById(partyRole.PartyId);

            //Insert loan clerk who processed the loan application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByAgreementType, today);
            }

            Context.Applications.AddObject(applicationNew1);

            //Agreement instance
            Agreement agreement1 = CreateNewAgreement(applicationNew1, today);


            LoanDisbursementVcr voucher1 = CreateLoanDisbursementVoucher(agreement1, this.ChangeInterestAccount, today);
            DisbursementVcrStatu voucherStatus1 = CreateVoucherStatus(voucher1, today);

            //CancelReceivables(financialAccount.LoanAccount, today);

            AgreementItem agreementItemNew = CreateAgreementItem(agreement1, today, this.ChangeInterestAccount);

            //Borrower Party Role
            PartyRole borrowerRole1 = new PartyRole();
            borrowerRole1.Party = ownerPartyRole.Party;
            borrowerRole1.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole1.EffectiveDate = today;

            //Agreement Role
            AgreementRole agreementRole1 = CreateAgreementRole(agreement1, borrowerRole1);

            //CoBorrowers Agreement Role
            var coBorrowers = SaveCoOwnerAgreement(today);
            foreach (var coBorrower in coBorrowers)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, coBorrower);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Guarantors Agreement Account Role
            var guarantorsA = SaveGuarantorAgreement(today);
            foreach (var guarantor in guarantorsA)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, guarantor);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Old Amortization Schedule
            AmortizationSchedule oldAmortizationSched = financialAccount.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId
                    && entity.EndDate == null);
            oldAmortizationSched.EndDate = today;

            LoanAgreement loanAgreement = financialAccount.Agreement.LoanAgreement;

            //Loan Agreement instance
            LoanAgreement loanAgreementNew1 = new LoanAgreement();
            loanAgreementNew1.Agreement = agreement1;

            //Financial Account instance for the Consolidated Loans
            FinancialAccount financialAccountNew1 = CreateFinancialAccountWithParent(agreement1, today, financialAccount);

            //Financial Account Product for Split Account 1
            FinancialAccountProduct financialAccountProductNew1 = CreateFinancialAccountProduct(financialAccountNew1, financialProduct, today);

            //Party Role
            PartyRole ownerPartyRole1 = new PartyRole();
            ownerPartyRole1.Party = ownerPartyRole.Party;
            ownerPartyRole1.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole1.EffectiveDate = today;

            //Financial Account Role
            FinancialAccountRole financialAccountRole1 = CreateFinancialAccountRole(financialAccountNew1, ownerPartyRole1);

            //CoOwners Financial Account Role
            var coOwners = SaveCoOwnerFinancial(today);
            foreach (var owners in coOwners)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, owners);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            //Guarantors Financial Account Role
            var guarantors = SaveGuarantorFinancial(today);
            foreach (var guarantor in guarantors)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, guarantor);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            var maturityDate1 = DateTime.Today;
            if (this.ChangeInterestAmortizationSchedule.Count() != 0)
                maturityDate1 = this.ChangeInterestAmortizationSchedule.ToList().Last().ScheduledPaymentDate;

            var lastSchedAccount1 = maturityDate1;


            //Loan Account instance
            this.ChangeInterestAccount.LoanReleaseDate = this.ChangeInterestAccount.LoanReleaseDate.AddMonths(1);
            LoanAccount loanAccountNew1 = CreateLoanAccount(financialAccountNew1, this.ChangeInterestAccount, lastSchedAccount1, today);

            //CarryOverReceivables(loanAccountOld, loanAccountNew1, today);

            //Loan Account Status instance
            LoanAccountStatu loanAccountStatus1 = CreateLoanAccountStatus(loanAccountNew1, today);

            this.ChangeInterestAccount.LoanReleaseDate = oldAmortizationSched.LoanReleaseDate;

            //Amortization Schedule
            AmortizationSchedule amortizationScheduleNew = CreateAmortizationScheduleWithParent(loanAgreementNew1, this.ChangeInterestAccount, today, oldAmortizationSched); 

            ////Amortization Schedule Items
            ////add foreach here <<<<<<<<
            ////>>>>>>>>>>>>>>>>>>>
            var amoItems2 = this.ChangeInterestAmortizationSchedule;


            foreach (var amortizationItem in amoItems2)
            {
                AmortizationScheduleItem amortizationItemChangeInterest = CreateAmortizationScheduleItem(amortizationScheduleNew, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemChangeInterest);
            }

            //financialAccount.LoanAccount.MaturityDate = amoItems2.LastOrDefault().ScheduledPaymentDate;

            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.ChangeInterestType);

            //Loan Modification Status instance
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);

            //Loan Modification Prev Items instance
            LoanModificationPrevItem loanModificationPrevItem = CreateLoanModificationPrevItem(loanModification, financialAccount);

            //Addendum
            Addendum addendum = new Addendum();
            addendum.Agreement = agreement;
            addendum.AgreementItem = agreementItemNew;

            SetChequeStatusToCancelled(financialAccount, today);

            SaveCheques(financialAccountNew1, customerPartyRole, today);

            Context.Addenda.AddObject(addendum);

            Context.LoanModifications.AddObject(loanModification);

            //Context.AmortizationSchedules.AddObject(amortizationScheduleNew);
        }

        public void SaveChangeIcmAmortizationSchedule(DateTime today)
        {
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId && entity.Agreement.EndDate == null);
            financialAccount.LoanAccount.CurrentStatus.IsActive = false;
            //financialAccount.LoanAccount.LoanBalance = this.ChangeIcmAccount.NewLoanAmount;

            LoanAccount oldLoanAccount = financialAccount.LoanAccount;

            LoanAccountStatu newLoanAccountStatus = new LoanAccountStatu();
            newLoanAccountStatus.LoanAccount = oldLoanAccount;
            newLoanAccountStatus.LoanAccountStatusType = LoanAccountStatusType.RestructuredType;
            newLoanAccountStatus.TransitionDateTime = today;
            newLoanAccountStatus.IsActive = true;

            CancelReceivables(oldLoanAccount, today);

            Agreement agreement = financialAccount.Agreement;

            PartyRole customerPartyRole = PartyRole.GetById(this.CustomerId);
            LoanApplication oldLoanApplication1 = financialAccount.Agreement.Application.LoanApplication;

            LoanApplicationStatu oldLoanApplicationStatus = oldLoanApplication1.CurrentStatus;
            oldLoanApplicationStatus.IsActive = false;
            LoanApplicationStatu newLoanApplicationStatusOld = new LoanApplicationStatu();
            newLoanApplicationStatusOld.LoanApplication = oldLoanApplication1;
            newLoanApplicationStatusOld.LoanApplicationStatusType = LoanApplicationStatusType.RestructuredType;
            newLoanApplicationStatusOld.TransitionDateTime = today;
            newLoanApplicationStatusOld.IsActive = true;

            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication1);
            RetrieveCoBorrower(oldLoanApplication1);
            RetrieveCollaterals(oldLoanApplication1, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication1);
            RetrieveSubmittedDocumentsForm(oldLoanApplication1);

            //agreement.Application.LoanApplication.InterestRate = this.ChangeIcmAccount.NewInterestRate;

            AgreementItem agreementItemOld = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            //agreementItemOld.IsActive = false;
            Application oldApplication = agreement.Application;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.CollateralRequirementType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.MethodofChargingInterestType);
            var aiInterestRateDescription = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestRateType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(oldApplication, ProductFeatureCategory.InterestComputationModeType);
            this.FinancialProductId1 = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;
            var financialProduct = aiCollateralRequirement.ProductFeatureApplicability.FinancialProduct; 

            //Application instance
            Application applicationNew1 = CreateNewApplication(today);

            var applicationItems = Context.ApplicationItems.FirstOrDefault(entity => entity.ApplicationId == oldApplication.Id && entity.EndDate == null).ProductFeatureApplicability;

            AddApplicationItems(today, applicationNew1, financialProduct, this.ChangeIcmAccount);

            LoanApplication loanApplicationNew = CreateLoanApplication(applicationNew1, today, this.ChangeIcmAccount);

            SaveFees(loanApplicationNew, today);
            SaveCoBorrower(loanApplicationNew, today);
            SaveCollaterals(loanApplicationNew, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew, today);
            SaveSubmittedDocumentsForm(loanApplicationNew, today);

            LoanApplicationStatu loanApplicationStatusNew = CreateLoanApplicationStatus(loanApplicationNew, today);

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole ownerPartyRole = PartyRole.GetById(this.CustomerId);
            PartyRole customerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.CustomerType);

            Customer customer = customerRole.Customer;
            LoanAccount loanAccountOld = financialAccount.LoanAccount;

            //Loan Application Role
            var partyRole = PartyRole.GetById(this.ModifiedBy);
            var party = Party.GetById(partyRole.PartyId);

            //Insert loan clerk who processed the loan application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew, party, RoleType.ApprovedByAgreementType, today);
            }

            Context.Applications.AddObject(applicationNew1);

            //Agreement instance
            Agreement agreement1 = CreateNewAgreement(applicationNew1, today);


            LoanDisbursementVcr voucher1 = CreateLoanDisbursementVoucher(agreement1, this.ChangeIcmAccount, today);
            DisbursementVcrStatu voucherStatus1 = CreateVoucherStatus(voucher1, today);

            //CancelReceivables(financialAccount.LoanAccount, today);

            AgreementItem agreementItemNew = CreateAgreementItem(agreement1, today, this.ChangeIcmAccount);

            //Borrower Party Role
            PartyRole borrowerRole1 = new PartyRole();
            borrowerRole1.Party = ownerPartyRole.Party;
            borrowerRole1.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole1.EffectiveDate = today;

            //Agreement Role
            AgreementRole agreementRole1 = CreateAgreementRole(agreement1, borrowerRole1);

            //CoBorrowers Agreement Role
            var coBorrowers = SaveCoOwnerAgreement(today);
            foreach (var coBorrower in coBorrowers)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, coBorrower);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Guarantors Agreement Account Role
            var guarantorsA = SaveGuarantorAgreement(today);
            foreach (var guarantor in guarantorsA)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, guarantor);

                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Old Amortization Schedule
            AmortizationSchedule oldAmortizationSched = financialAccount.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId
                    && entity.EndDate == null);
            oldAmortizationSched.EndDate = today;

            LoanAgreement loanAgreement = financialAccount.Agreement.LoanAgreement;

            //Loan Agreement instance
            LoanAgreement loanAgreementNew1 = new LoanAgreement();
            loanAgreementNew1.Agreement = agreement1;

            //Financial Account instance for the Consolidated Loans
            FinancialAccount financialAccountNew1 = CreateFinancialAccountWithParent(agreement1, today, financialAccount);

            //Financial Account Product for Split Account 1
            FinancialAccountProduct financialAccountProductNew1 = CreateFinancialAccountProduct(financialAccountNew1, financialProduct, today);

            //Party Role
            PartyRole ownerPartyRole1 = new PartyRole();
            ownerPartyRole1.Party = ownerPartyRole.Party;
            ownerPartyRole1.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole1.EffectiveDate = today;

            //Financial Account Role
            FinancialAccountRole financialAccountRole1 = CreateFinancialAccountRole(financialAccountNew1, ownerPartyRole1);

            //CoOwners Financial Account Role
            var coOwners = SaveCoOwnerFinancial(today);
            foreach (var owners in coOwners)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, owners);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            //Guarantors Financial Account Role
            var guarantors = SaveGuarantorFinancial(today);
            foreach (var guarantor in guarantors)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, guarantor);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            var maturityDate1 = DateTime.Today;
            if (this.ChangeIcmAmortizationSchedule.Count() != 0)
                maturityDate1 = this.ChangeIcmAmortizationSchedule.ToList().Last().ScheduledPaymentDate;

            var lastSchedAccount1 = maturityDate1;

            //Loan Account instance
            this.ChangeIcmAccount.LoanReleaseDate = this.ChangeIcmAccount.LoanReleaseDate.AddMonths(1);
            LoanAccount loanAccountNew1 = CreateLoanAccount(financialAccountNew1, this.ChangeIcmAccount, lastSchedAccount1, today);

            //CarryOverReceivables(loanAccountOld, loanAccountNew1, today);

            //Loan Account Status instance
            LoanAccountStatu loanAccountStatus1 = CreateLoanAccountStatus(loanAccountNew1, today);


            this.ChangeIcmAccount.LoanReleaseDate = oldAmortizationSched.LoanReleaseDate;


            ////Amortization Schedule
            AmortizationSchedule amortizationScheduleNew = CreateAmortizationScheduleWithParent(loanAgreementNew1, this.ChangeIcmAccount, today, oldAmortizationSched);


            //Amortization Schedule Items
            //add foreach here <<<<<<<<
            //>>>>>>>>>>>>>>>>>>>
            var amoItems2 = this.ChangeIcmAmortizationSchedule;


            foreach (var amortizationItem in amoItems2)
            {
                AmortizationScheduleItem amortizationItemChangeIcm = CreateAmortizationScheduleItem(amortizationScheduleNew, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemChangeIcm);
            }

            //financialAccount.LoanAccount.MaturityDate = amoItems2.LastOrDefault().ScheduledPaymentDate;

            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.ChangeIcmType);

            //Loan Modification Status instance
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);

            //Loan Modification Prev Items instance
            LoanModificationPrevItem loanModificationPrevItem = CreateLoanModificationPrevItem(loanModification, financialAccount);

            //Addendum
            Addendum addendum = new Addendum();
            addendum.Agreement = agreement;
            addendum.AgreementItem = agreementItemNew;

            SetChequeStatusToCancelled(financialAccount, today);

            SaveCheques(financialAccountNew1, customerPartyRole, today);

            Context.Addenda.AddObject(addendum);

            Context.LoanModifications.AddObject(loanModification);

            //Context.AmortizationSchedules.AddObject(amortizationScheduleNew);
        }

        public void SaveConsolidateLoanAmortizationSchedule(DateTime today)
        {
            //Old Financial Account1 Instance
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId);
            financialAccount.LoanAccount.CurrentStatus.IsActive = false;


            LoanAccount oldLoanAccount1 = financialAccount.LoanAccount;

            LoanAccountStatu newLoanAccountStatus = new LoanAccountStatu();
            newLoanAccountStatus.LoanAccount = oldLoanAccount1;
            newLoanAccountStatus.LoanAccountStatusType = LoanAccountStatusType.RestructuredType;
            newLoanAccountStatus.TransitionDateTime = today;
            newLoanAccountStatus.IsActive = true;

            CancelReceivables(oldLoanAccount1, today);

            PartyRole customerPartyRole = PartyRole.GetById(this.CustomerId);
            LoanApplication oldLoanApplication1 = financialAccount.Agreement.Application.LoanApplication;
            oldLoanApplication1.CurrentStatus.IsActive = false;

            LoanApplicationStatu newLoanApplicationStatusOld = new LoanApplicationStatu();
            newLoanApplicationStatusOld.LoanApplication = oldLoanApplication1;
            newLoanApplicationStatusOld.LoanApplicationStatusType = LoanApplicationStatusType.RestructuredType;
            newLoanApplicationStatusOld.TransitionDateTime = today;
            newLoanApplicationStatusOld.IsActive = true;

            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication1);
            RetrieveCoBorrower(oldLoanApplication1);
            RetrieveCollaterals(oldLoanApplication1, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication1);
            RetrieveSubmittedDocumentsForm(oldLoanApplication1);

            var receivables = financialAccount.LoanAccount.Receivables;

            //CancelReceivables(financialAccount.LoanAccount, today);
            

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole ownerPartyRole = PartyRole.GetById(this.CustomerId);
            PartyRole customerRole = PartyRole.GetByPartyIdAndRole(ownerPartyRole.PartyId, RoleType.CustomerType);

            var outstandingLoanAccounts = Party.RetrieveOutstandingLoans(ownerPartyRole.PartyId);
            var loanReAvailmentCustomerUnderLit = outstandingLoanAccounts.Where(entity => entity.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id);
            var loanReAvailmentCustomerDelinquent = outstandingLoanAccounts.Where(entity => entity.StatusTypeId == LoanAccountStatusType.DelinquentType.Id);

            Customer customer = customerRole.Customer;
            LoanAccount loanAccountOld = financialAccount.LoanAccount;

            ////Change Customer Status
            ChangeCustomerStatus(customer, loanAccountOld, today);

            //Old Financial Account2 Instance
            FinancialAccount financialAccount2 = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == this.FinancialAccountId2);


            LoanApplication oldLoanApplication2 = financialAccount2.Agreement.Application.LoanApplication;

            //Retrieve previous loan application coborrowers, collaterals, guarantors, and submitted documents
            RetrieveFees(oldLoanApplication2);
            RetrieveCoBorrower(oldLoanApplication2);
            RetrieveCollaterals(oldLoanApplication2, customerPartyRole);
            RetrieveGuarantor(oldLoanApplication2);
            RetrieveSubmittedDocumentsForm(oldLoanApplication2);

            oldLoanApplication2.CurrentStatus.IsActive = false;

            LoanApplicationStatu newLoanApplicationStatusOld2 = new LoanApplicationStatu();
            newLoanApplicationStatusOld2.LoanApplication = oldLoanApplication2;
            newLoanApplicationStatusOld2.LoanApplicationStatusType = LoanApplicationStatusType.RestructuredType;
            newLoanApplicationStatusOld2.TransitionDateTime = today;
            newLoanApplicationStatusOld2.IsActive = true;

            var receivables2 = financialAccount2.LoanAccount.Receivables;


            //CancelReceivables(financialAccount2.LoanAccount, today);
            LoanAccount loanAccountOld2 = financialAccount2.LoanAccount;
            loanAccountOld2.CurrentStatus.IsActive = false;

            LoanAccountStatu newLoanAccountStatus2 = new LoanAccountStatu();
            newLoanAccountStatus2.LoanAccount = loanAccountOld2;
            newLoanAccountStatus2.LoanAccountStatusType = LoanAccountStatusType.RestructuredType;
            newLoanAccountStatus2.TransitionDateTime = today;
            newLoanAccountStatus2.IsActive = true;

            CancelReceivables(loanAccountOld2, today);

            //New Application instance for the Consolidated Loans
            Application applicationNew1 = CreateNewApplication(today);


            var financialProduct1 = FinancialProduct.GetById(this.FinancialProductId1);

            //Add Application Items for Split Account
            AddApplicationItems(today, applicationNew1, financialProduct1, this.ConsolidateLoanAccount);

            //New Loan Application instance for the Consolidated Loans
            LoanApplication loanApplicationNew1 = CreateLoanApplication(applicationNew1, today, this.ConsolidateLoanAccount);

            //Save coborrowers, collaterals, guarantors, and submitted documents
            SaveFees(loanApplicationNew1, today);
            SaveCoBorrower(loanApplicationNew1, today);
            SaveCollaterals(loanApplicationNew1, customerPartyRole, today);
            SaveGuarantor(loanApplicationNew1, today);
            SaveSubmittedDocumentsForm(loanApplicationNew1, today);

            //New Loan Application Status instance for Split Account 1
            LoanApplicationStatu loanApplicationStatus1 = CreateLoanApplicationStatus(loanApplicationNew1, today);


            //Loan Application Role
            var partyRole = PartyRole.GetById(this.ModifiedBy);
            var party = Party.GetById(partyRole.PartyId);

            //Insert loan clerk who processed the loan application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ProcessedByApplicationType, today);
            }

            if (RoleType.BorrowerApplicationType.PartyRoleType != null)
            {
                PartyRole borrower = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, customer.PartyRole.Party, RoleType.BorrowerApplicationType, today);
            }

            if (RoleType.ApprovedByApplicationType.PartyRoleType != null)
            {
                PartyRole approvedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ApprovedByApplicationType, today);
            }

            if (RoleType.ApprovedByAgreementType.PartyRoleType != null)
            {
                PartyRole approvedByAgreement = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplicationNew1, party, RoleType.ApprovedByAgreementType, today);
            }

            Context.Applications.AddObject(applicationNew1);

            //Agreement instance for the Consolidated Loans
            Agreement agreement1 = CreateNewAgreement(applicationNew1, today);


            LoanDisbursementVcr voucher1 = CreateLoanDisbursementVoucher(agreement1, this.ConsolidateLoanAccount, today);
            DisbursementVcrStatu voucherStatus1 = CreateVoucherStatus(voucher1, today);


            //Borrower Party Role for Split Account 1
            PartyRole borrowerRole1 = new PartyRole();
            borrowerRole1.Party = ownerPartyRole.Party;
            borrowerRole1.RoleTypeId = RoleType.BorrowerAgreementType.Id;
            borrowerRole1.EffectiveDate = today;

            //Agreement Role for the Consolidated Loans
            AgreementRole agreementRole1 = CreateAgreementRole(agreement1, borrowerRole1);


            //CoBorrowers Agreement Role
            var coBorrowers = SaveCoOwnerAgreement(today);
            foreach (var coBorrower in coBorrowers)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, coBorrower);


                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Guarantors Agreement Account Role
            var guarantorsA = SaveGuarantorAgreement(today);
            foreach (var guarantor in guarantorsA)
            {
                AgreementRole agreementRole1s = CreateAgreementRole(agreement1, guarantor);


                Context.AgreementRoles.AddObject(agreementRole1s);
            }

            //Agreement Item instance for the Consolidated Loans
            AgreementItem agreementItemNew1 = CreateAgreementItem(agreement1, today, this.ConsolidateLoanAccount);


            //Loan Agreement instance for the Consolidated Loans
            LoanAgreement loanAgreementNew1 = new LoanAgreement();
            loanAgreementNew1.Agreement = agreement1;

            //Financial Account instance for the Consolidated Loans
            FinancialAccount financialAccountNew1 = CreateFinancialAccount(agreement1, today);


            //Financial Account Product for Split Account 1
            FinancialAccountProduct financialAccountProductNew1 = CreateFinancialAccountProduct(financialAccountNew1, financialProduct1, today);


            //Party Role for Split Account 1
            PartyRole ownerPartyRole1 = new PartyRole();
            ownerPartyRole1.Party = ownerPartyRole.Party;
            ownerPartyRole1.RoleTypeId = RoleType.OwnerFinancialType.Id;
            ownerPartyRole1.EffectiveDate = today;

            //Financial Account Role Split Account 1
            FinancialAccountRole financialAccountRole1 = CreateFinancialAccountRole(financialAccountNew1, ownerPartyRole1);


            //CoOwners Financial Account Role
            var coOwners = SaveCoOwnerFinancial(today);
            foreach (var owners in coOwners)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, owners);

                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            //Guarantors Financial Account Role
            var guarantors = SaveGuarantorFinancial(today);
            foreach (var guarantor in guarantors)
            {
                FinancialAccountRole financialAccountRole1s = CreateFinancialAccountRole(financialAccountNew1, guarantor);


                Context.FinancialAccountRoles.AddObject(financialAccountRole1s);
            }

            var maturityDate1 = DateTime.Today;
            if (this.ConsolidateLoanAmortizationSchedule.Count() != 0)
                maturityDate1 = this.ConsolidateLoanAmortizationSchedule.ToList().Last().ScheduledPaymentDate;

            var lastSchedAccount1 = maturityDate1;


            //Loan Account instance for the Consolidated Loans
            LoanAccount loanAccountNew1 = CreateLoanAccount(financialAccountNew1, this.ConsolidateLoanAccount, lastSchedAccount1, today);


            CarryOverReceivables(loanAccountOld, loanAccountNew1, today);
            CarryOverReceivables(loanAccountOld2, loanAccountNew1, today);

            //Loan Account Status instance for Split Account 1
            LoanAccountStatu loanAccountStatus1 = CreateLoanAccountStatus(loanAccountNew1, today);


            PartyRole modifiedBy = PartyRole.GetById(this.ModifiedBy);

            //Loan Modification instance for the Consolidated Loans
            LoanModification loanModification = CreateLoanModification(modifiedBy, LoanModificationType.ConsolidateType);


            //Loan Modification Status instance for the Consolidated Loans
            LoanModificationStatu loanModificationStatus = CreateLoanModificationStatus(loanModification, today);


            //Loan Modification Prev Item instance for the Consolidated Loans
            LoanModificationPrevItem loanModificationPrevItem = CreateLoanModificationPrevItem(loanModification, financialAccount);


            //Loan Modification Prev Item instance for the Consolidated Loans
            LoanModificationPrevItem loanModificationPrevItem2 = CreateLoanModificationPrevItem(loanModification, financialAccount2);


            Context.LoanModifications.AddObject(loanModification);

            //Loan Modification New Item instance for the Consolidated Loans


            //Loan Modification New Item instance for Split Account 2


            //Old Amortization Schedule Account 1
            AmortizationSchedule oldAmortizationSched = financialAccount.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId
                    && entity.EndDate == null);
            oldAmortizationSched.EndDate = today;

            //Old Amortization Schedule Account 2
            AmortizationSchedule oldAmortizationSched2 = financialAccount2.Agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount2.AgreementId
                    && entity.EndDate == null);
            oldAmortizationSched2.EndDate = today;

            //Amortization Schedule for the Consolidated Loans
            AmortizationSchedule amortizationScheduleNew1 = CreateAmortizationSchedule(loanAgreementNew1, this.ConsolidateLoanAccount, today);



            //Amortization Schedule Items for the Consolidated Loans
            //add foreach here <<<<<<<<
            //>>>>>>>>>>>>>>>>>>>
            var amoItems1 = this.ConsolidateLoanAmortizationSchedule;
            

            foreach (var amortizationItem in amoItems1)
            {
                AmortizationScheduleItem amortizationItemConsolidatedLoan = CreateAmortizationScheduleItem(amortizationScheduleNew1, today, amortizationItem);

                Context.AmortizationScheduleItems.AddObject(amortizationItemConsolidatedLoan);
            }

            SetChequeStatusToCancelled(financialAccount, today);

            SetChequeStatusToCancelled(financialAccount2, today);

            SaveCheques(financialAccountNew1, customerPartyRole, today);
        }

        private void AddApplicationItems(DateTime today, Application application, FinancialProduct financialProduct, AmortizationItemsModel amortizationItem)
        {
            var icmId = amortizationItem.InterestComputationModeId;
            var productFeatureAppICM = ProductFeatureApplicability.GetById(icmId);
            ApplicationItem itemICM = CreateApplicationItem(application, productFeatureAppICM, today);

            var mociId = amortizationItem.MethodOfChargingInterestId;
            var productFeatureAppMOCI = ProductFeatureApplicability.GetById(mociId);
            ApplicationItem itemMOCI = CreateApplicationItem(application, productFeatureAppMOCI, today);

            var irId = amortizationItem.InterestRateDescriptionId;
            var productFeatureIR = CreateOrRetrieveInterestRate(irId, financialProduct.Id, amortizationItem.InterestRate);
            ApplicationItem itemIR = CreateApplicationItem(application, productFeatureIR, today);

            var crId = amortizationItem.CollateralRequirementId;
            var productFeatureAppCR = ProductFeatureApplicability.GetById(crId);
            ApplicationItem itemCR = CreateApplicationItem(application, productFeatureAppCR, today);
        }

        private void SaveFees(LoanApplication loanApplication, DateTime today)
        {
            foreach (var fee in Fees)
            {
                fee.PrepareForSave(loanApplication, today);
            }
        }

        private void SaveCollaterals(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            foreach (var collateral in Collaterals)
            {
                collateral.PrepareForSave(loanApplication, customerPartyRole, today);
            }
        }

        private void SaveCoBorrower(LoanApplication loanApplication, DateTime today)
        {
            var coborrower = RoleType.CoBorrowerApplicationType;
            foreach (PersonnalPartyModel model in CoBorrowers)
            {
                PersonnalPartyModel newModel = new PersonnalPartyModel();
                newModel = model;
                newModel.PrepareForSave(loanApplication, coborrower, today);
            }
        }

        private void SaveGuarantor(LoanApplication loanApplication, DateTime today)
        {
            var guarantor = RoleType.GuarantorApplicationType;
            foreach (PersonnalPartyModel model in Guarantors)
            {
                PersonnalPartyModel newModel = new PersonnalPartyModel();
                newModel = model;
                newModel.PrepareForSave(loanApplication, guarantor, today);
            }
        }

        private void SaveSubmittedDocumentsForm(LoanApplication loanApplication, DateTime today)
        {
            foreach (SubmittedDocumentModel model in SubmittedDocuments)
            {
                SubmittedDocumentModel newModel = new SubmittedDocumentModel();
                newModel = model;
                newModel.PrepareForSave(loanApplication, today);
            }
        }

        private void SaveCheques(FinancialAccount financialAccount, PartyRole customerPartyRole, DateTime today)
        {
            var employeePartyRole = Context.PartyRoles.FirstOrDefault(entity => entity.Id == this.ModifiedBy);
            foreach (ChequeModel model in Cheques)
            {
                ChequeModelPrepareForSave(financialAccount, customerPartyRole, employeePartyRole.Id, today, model);
                //model.PrepareForSave(loanApplication, customerPartyRole, employeePartyRole.Id);
            }
        }

        private void SaveCheques1(FinancialAccount financialAccount, PartyRole customerPartyRole, DateTime today)
        {
            var employeePartyRole = Context.PartyRoles.FirstOrDefault(entity => entity.Id == this.ModifiedBy);
            foreach (ChequeModel model in Cheques1)
            {
                ChequeModelPrepareForSave(financialAccount, customerPartyRole, employeePartyRole.Id, today, model);
                //model.PrepareForSave(loanApplication, customerPartyRole, employeePartyRole.Id);
            }
        }

        private void SaveCheques2(FinancialAccount financialAccount, PartyRole customerPartyRole, DateTime today)
        {
            var employeePartyRole = Context.PartyRoles.FirstOrDefault(entity => entity.Id == this.ModifiedBy);
            foreach (ChequeModel model in Cheques2)
            {
                ChequeModelPrepareForSave(financialAccount, customerPartyRole, employeePartyRole.Id, today, model);
                //model.PrepareForSave(loanApplication, customerPartyRole, employeePartyRole.Id);
            }
        }

        private List<PartyRole> SaveCoOwnerFinancial(DateTime today)
        {
            List<PartyRole> partyRoles = new List<PartyRole>();
            var coowner = RoleType.CoOwnerFinancialType;
            foreach (var owner in CoBorrowers)
            {
                var party = Context.Parties.SingleOrDefault(entity => entity.Id == owner.PartyId);
                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.RoleTypeId = coowner.Id;
                partyRole.EffectiveDate = today;

                partyRoles.Add(partyRole);

                Context.PartyRoles.AddObject(partyRole);
            }

            return partyRoles;
        }

        private List<PartyRole> SaveGuarantorFinancial(DateTime today)
        {
            List<PartyRole> partyRoles = new List<PartyRole>();
            var guarantorFinancial = RoleType.GuarantorFinancialType;
            foreach (var guarantor in Guarantors)
            {
                var party = Context.Parties.SingleOrDefault(entity => entity.Id == guarantor.PartyId);
                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.RoleTypeId = guarantorFinancial.Id;
                partyRole.EffectiveDate = today;

                partyRoles.Add(partyRole);

                Context.PartyRoles.AddObject(partyRole);
            }

            return partyRoles;
        }

        private List<PartyRole> SaveCoOwnerAgreement(DateTime today)
        {
            List<PartyRole> partyRoles = new List<PartyRole>();
            var coborrowerA = RoleType.CoBorrowerAgreementType;
            foreach (var coborrower in CoBorrowers)
            {
                var party = Context.Parties.SingleOrDefault(entity => entity.Id == coborrower.PartyId);
                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.RoleTypeId = coborrowerA.Id;
                partyRole.EffectiveDate = today;

                partyRoles.Add(partyRole);

                Context.PartyRoles.AddObject(partyRole);
            }

            return partyRoles;
        }

        private List<PartyRole> SaveGuarantorAgreement(DateTime today)
        {
            List<PartyRole> partyRoles = new List<PartyRole>();
            var guarantorA = RoleType.GuarantorAgreementType;
            foreach (var guarantor in Guarantors)
            {
                var party = Context.Parties.SingleOrDefault(entity => entity.Id == guarantor.PartyId);
                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.RoleTypeId = guarantorA.Id;
                partyRole.EffectiveDate = today;

                partyRoles.Add(partyRole);

                Context.PartyRoles.AddObject(partyRole);
            }

            return partyRoles;
        }

        private void RetrieveFees(LoanApplication loanApplication)
        {
            var fees = Context.LoanApplicationFees.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
            foreach (LoanApplicationFee fee in fees)
            {
                this.Fees.Add(new LoanFeeModel(fee));
            }
        }

        private void RetrieveCoBorrower(LoanApplication loanApplication)
        {
            var coborrower = RoleType.CoBorrowerApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == coborrower.Id
                && entity.PartyRole.EndDate == null);
            foreach (LoanApplicationRole role in roles)
            {
                this.CoBorrowers.Add(new PersonnalPartyModel(role));
            }
        }

        private void RetrieveGuarantor(LoanApplication loanApplication)
        {
            var guarantor = RoleType.GuarantorApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == guarantor.Id
                 && entity.PartyRole.EndDate == null);
            foreach (LoanApplicationRole role in roles)
            {
                this.Guarantors.Add(new PersonnalPartyModel(role));
            }
        }

        private void RetrieveCollaterals(LoanApplication loanApplication, PartyRole customerPartyRole)
        {
            foreach (var asset in loanApplication.Assets)
            {
                this.Collaterals.Add(Collateral.CreateCollateral(asset, customerPartyRole));
            }
        }

        private void RetrieveSubmittedDocumentsForm(LoanApplication loanApplication)
        {
            foreach (var model in loanApplication.SubmittedDocuments)
            {
                this.SubmittedDocuments.Add(new SubmittedDocumentModel(model));
            }
        }

        private void RetrieveAssociatedCheques(LoanApplication loanApplication)
        {
            var assoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
            foreach (ChequeApplicationAssoc association in assoc)
            {
                var cheques = Context.Cheques.Single(entity => entity.Id == association.ChequeId);
                this.Cheques.Add(new ChequeModel(cheques));
            }
        }

        public List<ChequeModel> RetrieveAssociatedCheques(int financialAccountId)
        {
            List<ChequeModel> chequeList = new List<ChequeModel>();
            var assoc = Context.ChequeLoanAssocs.Where(entity => entity.FinancialAccountId == financialAccountId);
            foreach (ChequeLoanAssoc association in assoc)
            {
                var cheques = Context.Cheques.Single(entity => entity.Id == association.ChequeId);
                chequeList.Add(new ChequeModel(cheques));
                //this.Cheques.Add(new ChequeModel(cheques));
            }

            return chequeList;
        }

        public AmortizationScheduleModel RetrieveCheque(int index, string restructureType)
        {
            AmortizationScheduleModel amortModel = new AmortizationScheduleModel();

            if (restructureType == "changeIcm")
            {
                amortModel = this.ChangeIcmAmortizationSchedule.ElementAt(index);
            }
            else if(restructureType == "changeInterest")
            {
                amortModel = this.ChangeInterestAmortizationSchedule.ElementAt(index);
            }
            else if (restructureType == "splitLoan")
            {
                amortModel = this.SplitLoanAmortizationSchedule1.ElementAt(index);
            }
            else if (restructureType == "consolidate")
            {
                amortModel = this.ConsolidateLoanAmortizationSchedule.ElementAt(index);
            }

            return amortModel;
        }

        public void RemoveCheque(string randomKey)
        {
            ChequeModel model = this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCheque(model);
        }

        public void RemoveCheque(ChequeModel model)
        {
            if (this.Cheques.Contains(model) == true)
            {
                if (model.IsNew)
                    Cheques.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public ChequeModel RetrieveCheque(string randomKey)
        {
            ChequeModel model = this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                return model;
            else
                return null;
        }

        public ChequeModel RetrieveCheque1(string randomKey)
        {
            ChequeModel model = this.Cheques1.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                return model;
            else
                return null;
        }

        public ChequeModel RetrieveCheque2(string randomKey)
        {
            ChequeModel model = this.Cheques2.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                return model;
            else
                return null;
        }

        private void FillAmortizationItemModel(AmortizationItemsModel item, AgreementItem agreementItem)
        {
            item.InterestRateDescription = agreementItem.InterestRateDescription;
            if(item.Term == agreementItem.LoanTermLength)
                item.Term = agreementItem.LoanTermLength;
            item.Unit = agreementItem.LoanTermUom;
            item.MethodOfChargingInterest = agreementItem.MethodOfChargingInterest;
            item.PaymentMode = agreementItem.PaymentMode;
        }
    }
}