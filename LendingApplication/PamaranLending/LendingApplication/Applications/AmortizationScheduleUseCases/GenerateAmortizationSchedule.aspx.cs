using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.AmortizationScheduleUseCases
{
    public partial class GenerateAmortizationSchedule : ActivityPageBase
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var interestRates = ProductFeature.All(ProductFeatureCategory.InterestRateType);
                storeInterestRate.DataSource = interestRates;
                storeInterestRate.DataBind();

                datLoanReleaseDate.MinDate = DateTime.Today;
                datPaymentStartDate.MinDate = DateTime.Today;

                this.ResourceGuid = Request.QueryString["ResourceGuid"];
                hdnLoanTermIndicator.Value = Request.QueryString["termIndicator"];
                if (string.IsNullOrWhiteSpace(this.ResourceGuid) == false)
                {
                    ApplyLoanOptions();
                    btnApply.Hidden = false;
                    btnCreateLoanApplication.Hidden = true;
                }
            }
        }

        protected void btnErrorTrue_Click(object sender, DirectEventArgs e)
        {
            string errorMessage = "<br/>Generate Amortization Schedule Error";
            throw new NotImplementedException(errorMessage);
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;

            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            options.PaymentStartDate = datPaymentStartDate.SelectedDate;
            options.LoanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            options.LoanTerm = (int)nfLoanTerm.Number;
            options.LoanTermId = int.Parse(hiddenLoanTermTimeUnitId.Text);
            options.PaymentMode = cmbPaymentMode.SelectedItem.Text;
            options.PaymentModeId = int.Parse(cmbPaymentMode.SelectedItem.Value);
            options.InterestComputationMode = cmbInterestComputationMode.SelectedItem.Text;
            options.InterestRateDescription = cmbInterestRate.SelectedItem.Text;
            options.InterestRate = (decimal)nfInterestRate.Number;
            options.InterestRateDescriptionId = int.Parse(cmbInterestRate.SelectedItem.Value);

            LoanCalculator calculator = new LoanCalculator();
            var models = calculator.GenerateLoanAmortization(options);

            storeAmortizationSchedule.DataSource = models;
            storeAmortizationSchedule.DataBind();
        }

        protected void btnCreateLoanApplication_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.FinancialProductId = int.Parse(hiddenProductId.Text);
            form.InterestRateDescription = cmbInterestRate.SelectedItem.Text;
            form.InterestRate = (decimal)nfInterestRate.Number;
            int featureId = int.Parse(cmbInterestRate.SelectedItem.Value);
            form.PFAInterestRateId = CreateOrRetrieveInterestRate(featureId, form.FinancialProductId, form.InterestRate);

            form.LoanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            form.LoanTerm = (int)nfLoanTerm.Number;
            form.LoanTermUomId = int.Parse(hiddenLoanTermTimeUnitId.Text);
            form.CollateralRequirementId = int.Parse(cmbCollateralRequirement.SelectedItem.Value);
            form.InterestComputationModeId = int.Parse(cmbInterestComputationMode.SelectedItem.Value);
            form.PaymentModeUomId = int.Parse(cmbPaymentMode.SelectedItem.Value);
            form.MethodOfChargingInterestId = int.Parse(cmbMethodOfChargingInterest.SelectedItem.Value);

            hiddenResourceGuid.Text = this.ResourceGuid;
        }

        private int CreateOrRetrieveInterestRate(int featureId, int financialProductId, decimal rate)
        {
            ProductFeatureApplicability featureApplicability;
            ProductFeature feature = ProductFeature.GetById(featureId);
            FinancialProduct financialProduct = FinancialProduct.GetById(financialProductId);
            var pfaSameValue = from pfa in ProductFeatureApplicability.GetAllActive(feature, financialProduct)
                               where pfa.Value.HasValue && pfa.Value.Value == rate
                               select pfa;
            if (pfaSameValue.Count() > 0)
                featureApplicability = pfaSameValue.First();
            else
            {
                featureApplicability = ProductFeatureApplicability.Create(feature, financialProduct, rate, DateTime.Today);
                ObjectContext.SaveChanges();
            }
            return featureApplicability.Id;
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving financial product details...")]
        public void FillFinancialProductDetails(int financialProductId)
        {
            hiddenProductId.Value = financialProductId;
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            this.txtLoanProductName.Text = product.Name;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            hiddenLoanTermTimeUnitId.Value = minimumTerm.LoanTerm.UomId;
            this.nfLoanTerm.FieldLabel = string.Format("Loan Term ({0})", minimumTerm.LoanTerm.UnitOfMeasure.Name);

            nfLoanTerm.MinValue = minimumLoanTerm;
            nfLoanTerm.MaxValue = maximumLoanTerm;
            nfLoanTerm.ReadOnly = false;

            var collateralRequirements = from pfa in
                                             ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.CollateralRequirementType, product)
                                         select new ProductFeatureApplicabilityModel(pfa);

            var interestComputationModes = from pfa in
                                               ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestComputationModeType, product)
                                           select new ProductFeatureApplicabilityModel(pfa);

            var methodOfChargingInterests = from pfa in
                                                ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.MethodofChargingInterestType, product)
                                            select new ProductFeatureApplicabilityModel(pfa);

            var paymentMode = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType);
            storePaymentMethod.DataSource = paymentMode;
            storePaymentMethod.DataBind();
            cmbPaymentMode.SelectedIndex = 3;

            storeCollateralRequirement.DataSource = collateralRequirements;
            storeCollateralRequirement.DataBind();
            cmbCollateralRequirement.SelectedIndex = 0;

            storeInterestComputationMode.DataSource = interestComputationModes;
            storeInterestComputationMode.DataBind();
            cmbInterestComputationMode.SelectedIndex = 0;

            storeMethodOfChargingInterest.DataSource = methodOfChargingInterests;
            storeMethodOfChargingInterest.DataBind();
            cmbMethodOfChargingInterest.SelectedIndex = 0;
        }

        public void ApplyLoanOptions()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            this.FillFinancialProductDetails(form.FinancialProductId);
            var termIndicator = Convert.ToInt32(hdnLoanTermIndicator.Value);
            if (form.PFAInterestRateId != -1)
            {
                ProductFeatureApplicability pfa = ProductFeatureApplicability.GetById(form.PFAInterestRateId);
                cmbInterestRate.Value = pfa.ProductFeatureId;
                nfInterestRate.Number = (double)form.InterestRate;
            }
            nfLoanAmount.Text = form.LoanAmount.ToString("N");
            if (termIndicator == 1)
            {
                nfLoanTerm.Number = (double)form.LoanTerm;
                cmbPaymentMode.Value = form.PaymentModeUomId;
            }
            else
            {
                nfLoanTerm.Number = 0;
                cmbPaymentMode.Value = UnitOfMeasure.MonthlyType.Id;
            }

            cmbCollateralRequirement.Value = form.CollateralRequirementId;
            cmbInterestComputationMode.Value = form.InterestComputationModeId;
            cmbMethodOfChargingInterest.Value = form.MethodOfChargingInterestId;
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

        [DirectMethod]
        public bool setPaymentDate()
        {
            if (string.IsNullOrWhiteSpace(this.txtLoanProductName.Text) == false)
            {
                var dateManager = new DateTimeOperationManager(cmbPaymentMode.SelectedItem.Text, datLoanReleaseDate.SelectedDate);
                datPaymentStartDate.SelectedDate = dateManager.Increment();
                return true;
            }
            else
                return false;
        }
    }
}