using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class ConsolidateLoan : ActivityPageBase
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
                int selectedLoanId1 = int.Parse(Request.QueryString["id1"]);
                int selectedLoanId2 = int.Parse(Request.QueryString["id2"]);
                hdnSelectedLoanId1.Text = selectedLoanId1.ToString();
                hdnSelectedLoanId2.Text = selectedLoanId2.ToString();
                datLoanReleaseDate.SelectedDate = DateTime.Now;
                datLoanReleaseDate.DisabledDays = ApplicationSettings.DisabledDays;
                datPaymentStartDate.DisabledDays = ApplicationSettings.DisabledDays;

                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                this.hdnSelectedLoanId1.Value = Request.QueryString["id1"];
                this.hdnSelectedLoanId2.Value = Request.QueryString["id2"];
                this.hiddenResourceGUID.Value = this.ResourceGuid;
                this.hiddenCustomerId.Value = Request.QueryString["cid"];
                this.hiddenRandomKey1.Value = Request.QueryString["b1"];
                this.hiddenRandomKey2.Value = Request.QueryString["b2"];
                this.ParentResourceGuid = Request.QueryString["guid"];

                LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
                LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey1.Text);
                LoanRestructureListModel model2 = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey2.Text);
                decimal total = model.TotalLoanBalance + model2.TotalLoanBalance;
                this.hiddenBalance.Value = model.TotalLoanBalance + model2.TotalLoanBalance;
                decimal loanAmount1 = model.LoanAmount;
                decimal loanAmount2 = model2.LoanAmount;

                FillReceivables(model, model2, oldForm);

                //FillBalance(loanAmount1, loanAmount2);
                
                //this.txtCarryOverBalance.Text = total.ToString("N");

                var unitOfMeasure = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType);
                UnitOfMeasureStore.DataSource = unitOfMeasure.ToList();
                UnitOfMeasureStore.DataBind();

                var paymentMode = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType);
                PaymentModeStore.DataSource = paymentMode.ToList();
                PaymentModeStore.DataBind();

                var interestComputationMode = ProductFeature.All(ProductFeatureCategory.InterestComputationModeType);
                InterestComputationModeStore.DataSource = interestComputationMode.ToList();
                InterestComputationModeStore.DataBind();

                var methodOfChargingInterest = ProductFeature.All(ProductFeatureCategory.MethodofChargingInterestType).Where(entity => entity.Id != ProductFeature.DiscountedInterestType.Id);
                MethodOfChargingInterestStore.DataSource = methodOfChargingInterest.ToList();
                MethodOfChargingInterestStore.DataBind();

                var interestRate = ProductFeature.All(ProductFeatureCategory.InterestRateType);
                InterestRateDescriptionStore.DataSource = interestRate.ToList();
                InterestRateDescriptionStore.DataBind();
                cmbInterestRate1.SelectedItem.Value = ProductFeature.MonthlyInterestRateType.Id.ToString();
                //nfInterestRate.Text = ApplicationSettings.DefaultRestructuredInterestRate.ToString();

                var collateralRequirement = ProductFeature.All(ProductFeatureCategory.CollateralRequirementType);

                var financialProduct = Context.FinancialProducts.FirstOrDefault();

                FillFinancialProductDetails1(financialProduct.Id);

            }
        }

        protected void FillReceivables(LoanRestructureListModel model, LoanRestructureListModel model2, LoanRestructureForm oldForm)
        {
            FinancialAccount financialAccount1 = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model.LoanID && entity.Agreement.EndDate == null);

            var loanAccount1 = financialAccount1.LoanAccount;

            var totalReceivable1 = 0M;

            totalReceivable1 = oldForm.ComputeTotalLoanReceivables(loanAccount1);

            txtCarryOverReceivables1.Text = totalReceivable1.ToString("N");
            nfReceivableAdd1.MaxValue = double.Parse(totalReceivable1.ToString());
            //if (totalReceivable1 == 0)
                nfReceivableAdd1.Number = 0;

            FinancialAccount financialAccount2 = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model2.LoanID && entity.Agreement.EndDate == null);

            var loanAccount2 = financialAccount2.LoanAccount;

            var totalReceivable2 = 0M;

            totalReceivable2 = oldForm.ComputeTotalLoanReceivables(loanAccount2);

            txtCarryOverReceivables2.Text = totalReceivable2.ToString("N");
            nfReceivableAdd2.MaxValue = double.Parse(totalReceivable2.ToString());

            //if (totalReceivable2 == 0)
                nfReceivableAdd2.Number = 0;
        }

        [DirectMethod]
        public void Fill()
        {
            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey1.Text);
            LoanRestructureListModel model2 = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey2.Text);

            decimal loanAmount1 = model.LoanAmount;
            decimal loanAmount2 = model2.LoanAmount;

            var receivableAdd1 = Decimal.Parse(nfReceivableAdd1.Number.ToString());
            var receivableAdd2 = Decimal.Parse(nfReceivableAdd2.Number.ToString());

            loanAmount1 += receivableAdd1;
            loanAmount2 += receivableAdd2;

            FillBalance(loanAmount1, loanAmount2);
        }

        private void FillBalance(decimal loanAmount1, decimal loanAmount2)
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            int FinancialAccountId1 = int.Parse(hdnSelectedLoanId1.Value.ToString());
            int FinancialAccountId2 = int.Parse(hdnSelectedLoanId2.Value.ToString());
            FinancialAccount financialAccount1 = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == FinancialAccountId1 && entity.Agreement.EndDate == null);
            Agreement agreement1 = financialAccount1.Agreement;
            AgreementItem agreementItem1 = agreement1.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var type1 = agreementItem1.InterestComputationMode;

            FinancialAccount financialAccount2 = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == FinancialAccountId2 && entity.Agreement.EndDate == null);
            Agreement agreement2 = financialAccount2.Agreement;
            AgreementItem agreementItem2 = agreement2.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var type2 = agreementItem2.InterestComputationMode;

            var interestRateDescription1 = agreementItem1.InterestRateDescription;
            var interestRateDescription2 = agreementItem2.InterestRateDescription;

            var loanAccount1 = financialAccount1.LoanAccount;
            var loanAccount2 = financialAccount2.LoanAccount;
            var newInterestRate = Decimal.Parse(string.IsNullOrWhiteSpace(nfInterestRate.Text) ? "0" : nfInterestRate.Text);

            var type = cmbInterestComputationMode1.SelectedItem.Text;

            var loanBalance1 = oldForm.ComputeNewLoanBalance(loanAccount1, newInterestRate, type, interestRateDescription1);
            var loanBalance2 = oldForm.ComputeNewLoanBalance(loanAccount2, newInterestRate, type, interestRateDescription2);

            var total = Math.Floor(loanBalance1 + loanBalance2);


            txtCarryOverBalance.Text = total.ToString("N");
        }

        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;

            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                GenerateConsolidatedLoanSchedule();
                form.SaveConsolidateLoanAmortizationSchedule(today);
            }
        }

        [DirectMethod]
        public int GenerateConsolidatedLoanSchedule()
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(this.hdnSelectedLoanId1.Text);
            form.FinancialAccountId2 = int.Parse(this.hdnSelectedLoanId2.Text);
            form.CustomerId = int.Parse(this.hiddenCustomerId.Value.ToString());
            form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            form.FinancialProductId1 = int.Parse(this.hiddenProductId1.Value.ToString());

            form.ConsolidateLoanAccount.BalanceToCarryOver = Decimal.Parse(this.txtCarryOverBalance.Text);
            form.ConsolidateLoanAccount.NewLoanAmount = form.ConsolidateLoanAccount.BalanceToCarryOver;
            form.ConsolidateLoanAccount.LoanReleaseDate = this.datLoanReleaseDate.SelectedDate;
            form.ConsolidateLoanAccount.PaymentStartDate = this.datPaymentStartDate.SelectedDate;
            form.ConsolidateLoanAccount.InterestComputationMode = this.cmbInterestComputationMode1.SelectedItem.Text;
            form.ConsolidateLoanAccount.Term = int.Parse(this.txtTerm.Text);
            form.ConsolidateLoanAccount.Unit = this.cmbUnit1.SelectedItem.Text;
            form.ConsolidateLoanAccount.UnitId = int.Parse(this.cmbUnit1.SelectedItem.Value);
            form.ConsolidateLoanAccount.InterestRate = (decimal)this.nfInterestRate.Number;
            form.ConsolidateLoanAccount.InterestRateDescription = cmbInterestRate1.SelectedItem.Text;
            form.ConsolidateLoanAccount.InterestRateDescriptionId = int.Parse(cmbInterestRate1.SelectedItem.Value);
            form.ConsolidateLoanAccount.PaymentMode = this.cmbPaymentMode1.SelectedItem.Text;
            form.ConsolidateLoanAccount.PaymentModeId = int.Parse(this.cmbPaymentMode1.SelectedItem.Value);
            form.ConsolidateLoanAccount.MethodOfChargingInterest = this.cmbMethodOfChargingInterest1.SelectedItem.Text;
            form.ConsolidateLoanAccount.MethodOfChargingInterestId = int.Parse(this.cmbMethodOfChargingInterest1.SelectedItem.Value.ToString());
            form.ConsolidateLoanAccount.InterestComputationModeId = int.Parse(cmbInterestComputationMode1.Value.ToString());
            form.ConsolidateLoanAccount.CollateralRequirementId = int.Parse(cmbCollateralRequirement1.Value.ToString());
            form.ConsolidateLoanAccount.CollateralRequirement = this.cmbCollateralRequirement1.SelectedItem.Text;

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
            txtTerm.MaxValue = maximumLoanTerm;
            txtTerm.ReadOnly = false;
            txtTerm.Number = txtTerm.Number;

            if (txtTerm.Number == 0)
            {
                btnGenerate.Hidden = true;
                btnSave.Hidden = false;
            }
            else
            {
                btnGenerate.Hidden = false;
                btnSave.Hidden = true;
            }

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

            CollateralRequirementStore.DataSource = collateralRequirements;
            CollateralRequirementStore.DataBind();
            cmbCollateralRequirement1.SelectedIndex = 0;

            InterestComputationModeStore.DataSource = interestComputationModes;
            InterestComputationModeStore.DataBind();
            cmbInterestComputationMode1.SelectedIndex = 0;

            MethodOfChargingInterestStore.DataSource = methodOfChargingInterests;
            MethodOfChargingInterestStore.DataBind();
            cmbMethodOfChargingInterest1.SelectedIndex = 0;

            var loanReleaseDate = DateTime.Today.AddMonths(-1);
            var paymentMode = UnitOfMeasure.MonthlyType.Name;

            var dtManager1 = new DateTimeOperationManager(paymentMode, loanReleaseDate);
            datPaymentStartDate.SelectedDate = dtManager1.Increment();
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

        protected decimal GetLoanBalance(int loanId)
        {
            var loanAccount = ObjectContext.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == loanId);
            var loanBalance = loanAccount.LoanBalance;
            return loanBalance;
        }

        protected decimal GetReceivableBalance(int loanId)
        {
            var receivable = ObjectContext.Receivables.SingleOrDefault(entity => entity.FinancialAccountId == loanId);
            var receivableBalance = receivable.Balance;
            return receivableBalance;
        }

        protected void SetAllReceivablesToCancelled(int loanId) 
        {
            var receivable = ObjectContext.Receivables.Where(entity => entity.FinancialAccountId == loanId);
            foreach (var item in receivable)
            {
                //set receivables to cancelled
            }
        }

        protected FinancialAccount CreateNewFinancialAccount()
        {
            FinancialAccount newFinancialAccount = new FinancialAccount();
            
            return newFinancialAccount;
        }
    }
}