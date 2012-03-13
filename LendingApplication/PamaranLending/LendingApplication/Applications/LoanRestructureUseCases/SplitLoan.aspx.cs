using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class SplitLoan : ActivityPageBase
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

        //public override List<string> UserTypesAllowed
        //{
        //    get
        //    {
        //        List<string> allowed = new List<string>();
        //        allowed.Add("Super Admin");
        //        allowed.Add("Loan Clerk");
        //        allowed.Add("Admin");
        //        return allowed;
        //    }
        //}

        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                hdnSelectedLoanID.Value = Request.QueryString["loanId"];
                hiddenResourceGUID.Value = this.ResourceGuid;
                hiddenCustomerId.Value = Request.QueryString["cid"];
                hiddenRandomKey.Value = Request.QueryString["b"];
                ParentResourceGuid = Request.QueryString["guid"];
                datLoanReleaseDate1.SelectedDate = DateTime.Now;
                datLoanReleaseDate2.SelectedDate = DateTime.Now;
                datLoanReleaseDate1.DisabledDays = ApplicationSettings.DisabledDays;
                datLoanReleaseDate2.DisabledDays = ApplicationSettings.DisabledDays;
                datPaymentStartDate.DisabledDays = ApplicationSettings.DisabledDays;
                datPaymentStartDate2.DisabledDays = ApplicationSettings.DisabledDays;

                LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
                LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(hiddenRandomKey.Value.ToString());
                hiddenBalance.Value = model.LoanAmount;
                decimal total = model.LoanAmount;

                this.txtCarryOverBalance.Text = total.ToString("N");
                FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model.LoanID && entity.Agreement.EndDate == null);
                decimal totalPayments = oldForm.GetPaymentsTotal(financialAccount.LoanAccount);
                txtTotalPayments.Text = totalPayments.ToString("N");
                var loanAccount = financialAccount.LoanAccount;

                var totalReceivable = 0M;

                totalReceivable = oldForm.ComputeTotalLoanReceivables(loanAccount);

                txtCarryOverReceivables.Text = totalReceivable.ToString("N");
                int paymentsCount = oldForm.GetPaymentsTotalCount(financialAccount.LoanAccount);
                txtNumberOfPayments.Text = paymentsCount.ToString();

                txtNumberofDeduction1.MaxValue = paymentsCount;
                //if (totalReceivable == 0)
                //{
                nfReceivableAdd1.Number = 0;
                nfReceivableAdd2.Number = 0;
                //}

                if (paymentsCount == 0)
                {
                    txtNumberofDeduction1.ReadOnly = true;
                    txtNumberofDeduction1.Text = "0";
                    txtNumberofDeduction2.Text = "0";
                    txtLessPayment1.Text = "0.00";
                    txtLessPayment2.Text = "0.00";
                }

                var unitOfMeasure = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType);
                UnitOfMeasureStore.DataSource = unitOfMeasure.ToList();
                UnitOfMeasureStore.DataBind();

                var paymentMode = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType);
                PaymentModeStore1.DataSource = paymentMode.ToList();
                PaymentModeStore1.DataBind();

                PaymentModeStore2.DataSource = paymentMode.ToList();
                PaymentModeStore2.DataBind();

                var interestComputationMode = ProductFeature.All(ProductFeatureCategory.InterestComputationModeType);
                InterestComputationModeStore1.DataSource = interestComputationMode.ToList();
                InterestComputationModeStore1.DataBind();

                var methodOfChargingInterest = ProductFeature.All(ProductFeatureCategory.MethodofChargingInterestType).Where(entity => entity.Id != ProductFeature.DiscountedInterestType.Id);
                MethodOfChargingInterestStore1.DataSource = methodOfChargingInterest.ToList();
                MethodOfChargingInterestStore1.DataBind();

                MethodOfChargingInterestStore2.DataSource = methodOfChargingInterest.ToList();
                MethodOfChargingInterestStore2.DataBind();

                var interestRate = ProductFeature.All(ProductFeatureCategory.InterestRateType);
                InterestRateDescriptionStore.DataSource = interestRate.ToList();
                InterestRateDescriptionStore.DataBind();
                cmbInterestRate1.SelectedItem.Value = ProductFeature.MonthlyInterestRateType.Id.ToString();
                cmbInterestRate2.SelectedItem.Value = ProductFeature.MonthlyInterestRateType.Id.ToString();
                nfInterestRate.Text = ApplicationSettings.DefaultRestructuredInterestRate.ToString();
                nfInterestRate2.Text = ApplicationSettings.DefaultRestructuredInterestRate.ToString();

                var collateralRequirement = ProductFeature.All(ProductFeatureCategory.CollateralRequirementType);
                CollateralRequirementStore1.DataSource = collateralRequirement.ToList();
                CollateralRequirementStore1.DataBind();

                CollateralRequirementStore2.DataSource = collateralRequirement.ToList();
                CollateralRequirementStore2.DataBind();

                var financialProduct = Context.FinancialProducts.FirstOrDefault();

                FillFinancialProductDetails1(financialProduct.Id);
                FillFinancialProductDetails2(financialProduct.Id);
                
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;

                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
                form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());
                form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.FinancialProductId1 = int.Parse(hiddenProductId1.Value.ToString());
                form.FinancialProductId2 = int.Parse(hiddenProductId2.Value.ToString());

                form.SplitLoanItemsAccount1.BalanceToCarryOver = Decimal.Parse(this.txtCarryOverBalance.Text);
                //form.SplitLoanItemsAccount1.Percentage = int.Parse(hiddenPercentage1.Value.ToString());
                form.SplitLoanItemsAccount1.LoanReleaseDate = datLoanReleaseDate1.SelectedDate;
                form.SplitLoanItemsAccount1.PaymentStartDate = this.datPaymentStartDate.SelectedDate;
                form.SplitLoanItemsAccount1.InterestComputationMode = this.cmbInterestComputationMode1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.Term = int.Parse(this.txtTerm.Text);
                form.SplitLoanItemsAccount1.Unit = this.cmbUnit1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.UnitId = int.Parse(this.cmbUnit1.SelectedItem.Value);
                form.SplitLoanItemsAccount1.InterestRate = (decimal)this.nfInterestRate.Number;
                form.SplitLoanItemsAccount1.InterestRateDescription = cmbInterestRate1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.InterestRateDescriptionId = int.Parse(cmbInterestRate1.SelectedItem.Value);
                form.SplitLoanItemsAccount1.PaymentMode = this.cmbPaymentMode1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.PaymentModeId = int.Parse(cmbPaymentMode1.SelectedItem.Value);
                form.SplitLoanItemsAccount1.MethodOfChargingInterest = this.cmbMethodOfChargingInterest1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.MethodOfChargingInterestId = int.Parse(this.cmbMethodOfChargingInterest1.SelectedItem.Value.ToString());
                form.SplitLoanItemsAccount1.InterestComputationModeId = int.Parse(cmbInterestComputationMode1.Value.ToString());
                form.SplitLoanItemsAccount1.CollateralRequirementId = int.Parse(cmbCollateralRequirement1.Value.ToString());
                form.SplitLoanItemsAccount1.CollateralRequirement = this.cmbCollateralRequirement1.SelectedItem.Text;
                form.SplitLoanItemsAccount1.LessPayment = Decimal.Parse(txtLessPayment1.Text);
                form.SplitLoanItemsAccount1.NewLoanAmount = Decimal.Parse(txtCarryOverBalance1.Text);
                form.SplitLoanItemsAccount1.AddPostDatedChecks = chkAddChecks1.Checked;

                form.SplitLoanItemsAccount2.BalanceToCarryOver = Decimal.Parse(this.txtCarryOverBalance.Text);
                //form.SplitLoanItemsAccount2.Percentage = int.Parse(hiddenPercentage2.Value.ToString());
                form.SplitLoanItemsAccount2.LoanReleaseDate = datLoanReleaseDate2.SelectedDate;
                form.SplitLoanItemsAccount2.PaymentStartDate = this.datPaymentStartDate2.SelectedDate;
                form.SplitLoanItemsAccount2.InterestComputationMode = this.cmbInterestComputationMode2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.Term = int.Parse(this.txtTerm2.Text);
                form.SplitLoanItemsAccount2.Unit = this.cmbUnit2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.UnitId = int.Parse(this.cmbUnit2.SelectedItem.Value);
                form.SplitLoanItemsAccount2.InterestRate = (decimal)this.nfInterestRate2.Number;
                form.SplitLoanItemsAccount2.InterestRateDescription = cmbInterestRate2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.InterestRateDescriptionId = int.Parse(cmbInterestRate2.SelectedItem.Value);
                form.SplitLoanItemsAccount2.PaymentMode = this.cmbPaymentMode2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.PaymentModeId = int.Parse(cmbPaymentMode2.SelectedItem.Value);
                form.SplitLoanItemsAccount2.MethodOfChargingInterest = this.cmbMethodOfChargingInterest2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.MethodOfChargingInterestId = int.Parse(this.cmbMethodOfChargingInterest2.SelectedItem.Value.ToString());
                form.SplitLoanItemsAccount2.InterestComputationModeId = int.Parse(cmbInterestComputationMode2.Value.ToString());
                form.SplitLoanItemsAccount2.CollateralRequirementId = int.Parse(cmbCollateralRequirement2.Value.ToString());
                form.SplitLoanItemsAccount2.CollateralRequirement = this.cmbCollateralRequirement2.SelectedItem.Text;
                form.SplitLoanItemsAccount2.LessPayment = Decimal.Parse(txtLessPayment2.Text);
                form.SplitLoanItemsAccount2.NewLoanAmount = Decimal.Parse(txtCarryOverBalance2.Text);
                form.SplitLoanItemsAccount2.AddPostDatedChecks = chkAddChecks2.Checked;

                form.SplitLoanAmortizationSchedule1.Clear();
                form.SplitLoanAmortizationSchedule2.Clear();
                form.GenerateSplitAccountAmortizationSchedule(today);

                form.SaveSplitAccountAmortizationSchedule(today);
            }
        }

        [DirectMethod]
        public void ComputeBalanceLessPayment(decimal newBalance, decimal payment, int count)
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            var loanAccount = Context.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == form.FinancialAccountId);
            string type = "";
            decimal interestRate = 0M;
            int counter = 0;
            bool isFirst = true;
            decimal amount = 0M;
            decimal lessPayment = 0M;
            decimal receivableAdd = 0M;
            string interestRateDescription = "";
            if (count == 1)
            {
                type = cmbInterestComputationMode1.SelectedItem.Text;
                interestRate = Decimal.Parse(string.IsNullOrWhiteSpace(nfInterestRate.Text) ? "0" : nfInterestRate.Text);
                counter = int.Parse(string.IsNullOrWhiteSpace(txtNumberofDeduction1.Text) ? "0" : txtNumberofDeduction1.Text);
                amount = Decimal.Parse(string.IsNullOrWhiteSpace(txtCarryOverAmount1.Text) ? "0" : txtCarryOverAmount1.Text);
                receivableAdd = Decimal.Parse(string.IsNullOrWhiteSpace(nfReceivableAdd1.Number.ToString()) ? "0" : nfReceivableAdd1.Number.ToString());
                if (cmbInterestRate1.SelectedIndex != -1)
                    interestRateDescription = cmbInterestRate1.SelectedItem.Text;
                else
                    interestRateDescription = ProductFeature.MonthlyInterestRateType.Name;
                isFirst = true;
                
            }
            else
            {
                type = cmbInterestComputationMode2.SelectedItem.Text;
                interestRate = Decimal.Parse(string.IsNullOrWhiteSpace(nfInterestRate2.Text) ? "0" : nfInterestRate2.Text);
                counter = int.Parse(string.IsNullOrWhiteSpace(txtNumberofDeduction2.Text) ? "0" : txtNumberofDeduction2.Text);
                amount = Decimal.Parse(string.IsNullOrWhiteSpace(txtCarryOverAmount2.Text) ? "0" : txtCarryOverAmount2.Text);
                receivableAdd = Decimal.Parse(nfReceivableAdd2.Number.ToString());

                if (cmbInterestRate2.SelectedIndex != -1)
                    interestRateDescription = cmbInterestRate1.SelectedItem.Text;
                else
                    interestRateDescription = ProductFeature.MonthlyInterestRateType.Name;

                isFirst = false;
            }
            lessPayment = form.GetLessPayment(loanAccount, counter, isFirst);
            amount += receivableAdd;

            var newLoanBalance = Math.Floor(form.ComputeNewLoanBalance(loanAccount, amount, interestRate, type, counter, isFirst, interestRateDescription));

            if (count == 1)
            {
                txtCarryOverBalance1.Text = newLoanBalance.ToString("N");
                txtLessPayment1.Text = lessPayment.ToString("N");
            }
            else
            {
                txtCarryOverBalance2.Text = newLoanBalance.ToString("N");
                txtLessPayment2.Text = lessPayment.ToString("N");
            }
        }

        [DirectMethod]
        public int FillAmortizationDetails()
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());
            form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            form.FinancialProductId1 = int.Parse(hiddenProductId1.Value.ToString());
            form.FinancialProductId2 = int.Parse(hiddenProductId2.Value.ToString());

            form.SplitLoanItemsAccount1.BalanceToCarryOver = Decimal.Parse(this.txtCarryOverBalance.Text);
            //form.SplitLoanItemsAccount1.Percentage = int.Parse(hiddenPercentage1.Value.ToString());
            form.SplitLoanItemsAccount1.LoanReleaseDate = datLoanReleaseDate1.SelectedDate;
            form.SplitLoanItemsAccount1.PaymentStartDate = this.datPaymentStartDate.SelectedDate;
            form.SplitLoanItemsAccount1.InterestComputationMode = this.cmbInterestComputationMode1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.Term = int.Parse(this.txtTerm.Text);
            form.SplitLoanItemsAccount1.Unit = this.cmbUnit1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.UnitId = int.Parse(this.cmbUnit1.SelectedItem.Value);
            form.SplitLoanItemsAccount1.InterestRate = (decimal)this.nfInterestRate.Number;
            form.SplitLoanItemsAccount1.InterestRateDescription = cmbInterestRate1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.InterestRateDescriptionId = int.Parse(cmbInterestRate1.SelectedItem.Value);
            form.SplitLoanItemsAccount1.PaymentMode = this.cmbPaymentMode1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.PaymentModeId = int.Parse(cmbPaymentMode1.SelectedItem.Value);
            form.SplitLoanItemsAccount1.MethodOfChargingInterest = this.cmbMethodOfChargingInterest1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.MethodOfChargingInterestId = int.Parse(this.cmbMethodOfChargingInterest1.SelectedItem.Value.ToString());
            form.SplitLoanItemsAccount1.InterestComputationModeId = int.Parse(cmbInterestComputationMode1.Value.ToString());
            form.SplitLoanItemsAccount1.CollateralRequirementId = int.Parse(cmbCollateralRequirement1.Value.ToString());
            form.SplitLoanItemsAccount1.CollateralRequirement = this.cmbCollateralRequirement1.SelectedItem.Text;
            form.SplitLoanItemsAccount1.LessPayment = Decimal.Parse(txtLessPayment1.Text);
            form.SplitLoanItemsAccount1.NewLoanAmount = Decimal.Parse(txtCarryOverBalance1.Text);
            form.SplitLoanItemsAccount1.AddPostDatedChecks = chkAddChecks1.Checked;

            form.SplitLoanItemsAccount2.BalanceToCarryOver = Decimal.Parse(this.txtCarryOverBalance.Text);
            //form.SplitLoanItemsAccount2.Percentage = int.Parse(hiddenPercentage2.Value.ToString());
            form.SplitLoanItemsAccount2.LoanReleaseDate = datLoanReleaseDate2.SelectedDate;
            form.SplitLoanItemsAccount2.PaymentStartDate = this.datPaymentStartDate2.SelectedDate;
            form.SplitLoanItemsAccount2.InterestComputationMode = this.cmbInterestComputationMode2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.Term = int.Parse(this.txtTerm2.Text);
            form.SplitLoanItemsAccount2.Unit = this.cmbUnit2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.UnitId = int.Parse(this.cmbUnit2.SelectedItem.Value);
            form.SplitLoanItemsAccount2.InterestRate = (decimal)this.nfInterestRate2.Number;
            form.SplitLoanItemsAccount2.InterestRateDescription = cmbInterestRate2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.InterestRateDescriptionId = int.Parse(cmbInterestRate2.SelectedItem.Value);
            form.SplitLoanItemsAccount2.PaymentMode = this.cmbPaymentMode2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.PaymentModeId = int.Parse(cmbPaymentMode2.SelectedItem.Value);
            form.SplitLoanItemsAccount2.MethodOfChargingInterest = this.cmbMethodOfChargingInterest2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.MethodOfChargingInterestId = int.Parse(this.cmbMethodOfChargingInterest2.SelectedItem.Value.ToString());
            form.SplitLoanItemsAccount2.InterestComputationModeId = int.Parse(cmbInterestComputationMode2.Value.ToString());
            form.SplitLoanItemsAccount2.CollateralRequirementId = int.Parse(cmbCollateralRequirement2.Value.ToString());
            form.SplitLoanItemsAccount2.CollateralRequirement = this.cmbCollateralRequirement2.SelectedItem.Text;
            form.SplitLoanItemsAccount2.LessPayment = Decimal.Parse(txtLessPayment2.Text);
            form.SplitLoanItemsAccount2.NewLoanAmount = Decimal.Parse(txtCarryOverBalance2.Text);
            form.SplitLoanItemsAccount2.AddPostDatedChecks = chkAddChecks2.Checked;

            return 1;
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving financial product details...")]
        public void FillFinancialProductDetails1(int financialProductId)
        {
            hiddenProductId1.Value = financialProductId;
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            this.txtLoanProductName1.Text = product.Name;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            hiddenLoanTermTimeUnitId1.Value = minimumTerm.LoanTerm.UomId;
            this.cmbUnit1.SelectedItem.Value = minimumTerm.LoanTerm.UomId.ToString();
            this.cmbUnit1.ReadOnly = true;

            //txtTerm.MinValue = minimumLoanTerm;
            //txtTerm.MaxValue = maximumLoanTerm;
            txtTerm.ReadOnly = false;
            txtTerm.Number = 1;


            var collateralRequirements = from pfa in
                                             ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.CollateralRequirementType, product)
                                         select new ProductFeatureApplicabilityModel(pfa);

            var interestComputationModes = from pfa in
                                               ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestComputationModeType, product)
                                           select new ProductFeatureApplicabilityModel(pfa);

            var methodOfChargingInterests = from pfa in
                                                ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.MethodofChargingInterestType, product)
                                            where pfa.ProductFeatureId != ProductFeature.DiscountedInterestType.Id
                                            select new ProductFeatureApplicabilityModel(pfa);


            cmbPaymentMode1.SelectedIndex = 3;
            cmbPaymentMode1.ReadOnly = true;

            CollateralRequirementStore1.DataSource = collateralRequirements;
            CollateralRequirementStore1.DataBind();
            cmbCollateralRequirement1.SelectedIndex = 0;

            InterestComputationModeStore1.DataSource = interestComputationModes;
            InterestComputationModeStore1.DataBind();
            cmbInterestComputationMode1.SelectedIndex = 0;

            MethodOfChargingInterestStore1.DataSource = methodOfChargingInterests;
            MethodOfChargingInterestStore1.DataBind();
            cmbMethodOfChargingInterest1.SelectedIndex = 0;

            var loanReleaseDate = DateTime.Today;
            var paymentMode = UnitOfMeasure.MonthlyType.Name;

            var dtManager1 = new DateTimeOperationManager(paymentMode, loanReleaseDate);
            datPaymentStartDate.SelectedDate = dtManager1.Increment();
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving financial product details...")]
        public void FillFinancialProductDetails2(int financialProductId)
        {
            hiddenProductId2.Value = financialProductId;
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            this.txtLoanProductName2.Text = product.Name;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            hiddenLoanTermTimeUnitId2.Value = minimumTerm.LoanTerm.UomId;
            this.cmbUnit2.SelectedItem.Value = minimumTerm.LoanTerm.UomId.ToString();
            this.cmbUnit2.ReadOnly = true;

            //txtTerm2.MinValue = minimumLoanTerm;
            //txtTerm2.MaxValue = maximumLoanTerm;
            txtTerm2.ReadOnly = false;
            txtTerm2.Number = txtTerm2.Number;

            var collateralRequirements = from pfa in
                                             ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.CollateralRequirementType, product)
                                         select new ProductFeatureApplicabilityModel(pfa);

            var interestComputationModes = from pfa in
                                               ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestComputationModeType, product)
                                           select new ProductFeatureApplicabilityModel(pfa);

            var methodOfChargingInterests = from pfa in
                                                ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.MethodofChargingInterestType, product)
                                            where pfa.ProductFeatureId != ProductFeature.DiscountedInterestType.Id
                                            select new ProductFeatureApplicabilityModel(pfa);


            cmbPaymentMode2.SelectedIndex = 3;
            cmbPaymentMode2.ReadOnly = true;

            CollateralRequirementStore2.DataSource = collateralRequirements;
            CollateralRequirementStore2.DataBind();
            cmbCollateralRequirement2.SelectedIndex = 0;

            InterestComputationModeStore2.DataSource = interestComputationModes;
            InterestComputationModeStore2.DataBind();
            cmbInterestComputationMode2.SelectedIndex = 0;

            MethodOfChargingInterestStore2.DataSource = methodOfChargingInterests;
            MethodOfChargingInterestStore2.DataBind();
            cmbMethodOfChargingInterest2.SelectedIndex = 0;

            var loanReleaseDate = DateTime.Today;
            var paymentMode = UnitOfMeasure.MonthlyType.Name;

            var dtManager1 = new DateTimeOperationManager(paymentMode, loanReleaseDate);
            datPaymentStartDate2.SelectedDate = dtManager1.Increment();
        }

        protected void OnRefreshDataOutstandingLoan(object sender, StoreRefreshDataEventArgs e)
        {
            
        }

        private class ProductFeatureApplicabilityModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public ProductFeatureApplicabilityModel(ProductFeatureApplicability entity)
            {
                this.Id = entity.Id;
                this.Name = entity.ProductFeature.Name;
            }
        }
    }
}