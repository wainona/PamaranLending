using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications
{
    public partial class CreateLoanApplication : ActivityPageBase
    {
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

        public int StraightLineCount
        {
            get
            {
                return int.Parse(hdnStraightLineCount.Value.ToString());
            }
        }

        public int DiminishingCount
        {
            get
            {
                return int.Parse(hdnDiminishingCount.Value.ToString());
            }
        }

        public void SetAllInterestComputationModes()
        {
            int id = Convert.ToInt32(hiddenCustomerID.Value);
            PartyRole customerPartyRole = PartyRole.GetById(id);
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            var applications = LoanApplication.GetAllLoanApplicationOf(customerPartyRole.Party, borrowerRoleType);
            var interestComputationModes = from application in applications
                                           join agreement in ObjectContext.Agreements on application.ApplicationId equals agreement.ApplicationId
                                           join agreementItem in ObjectContext.AgreementItems on agreement.Id equals agreementItem.AgreementId
                                           where agreement.EndDate == null
                                           group agreementItem by agreementItem.InterestComputationMode into icm
                                           select new { InterestComputationMode = icm.Key, Count = icm.Count() };

            var diminishingCount = from icm in interestComputationModes
                                   where icm.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name
                                   select icm.Count;

            var straightLineCount = from icm in interestComputationModes
                                    where icm.InterestComputationMode == ProductFeature.StraightLineMethodType.Name
                                    select icm.Count;

            hdnStraightLineCount.Value = straightLineCount.FirstOrDefault();
            hdnDiminishingCount.Value = diminishingCount.FirstOrDefault();
        }
        
        [DirectMethod(ShowMask=true, Msg="Checking loan term...")]//[c]
        public bool CheckLoanTerm()
        {
            var financialProductId = Convert.ToInt32(hiddenProductId.Value);
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);

            int loanTerm = Convert.ToInt32(nfLoanTerm.Number);
            int paymentMode = Convert.ToInt32(cmbPaymentMode.SelectedItem.Value);
            int termUnit = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType).SingleOrDefault(a => a.Name == minimumTerm.LoanTerm.UnitOfMeasure.Name).Id;

            var value = TimeUnitConversion.Convert(termUnit, paymentMode, loanTerm);
            decimal abs = (int)value;
            return abs == value;
        }

        [DirectMethod(ShowMask=true, Msg="Checking credit...")]//[d]
        public bool CheckCredit()
        {
            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;
            
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            decimal? creditLimit = 0;
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text) || txtCreditLimit.Text == "Unlimited")
                creditLimit = null;
            else
                creditLimit = Convert.ToDecimal(txtCreditLimit.Text);

            if (creditLimit != null && ((this.StraightLineCount < straightLineLimit)
                || (this.DiminishingCount < diminishingLineLimit)) && loanAmount > creditLimit)
                return true; //confirmation message
            else
                return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking diminishing loans of customer...")]//[e]
        public bool CheckDiminishing()
        {
            var interestComputationMode = cmbInterestComputationMode.SelectedItem.Text;
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            decimal? creditLimit = 0;
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text) || txtCreditLimit.Text == "Unlimited")
                creditLimit = decimal.MaxValue;
            else
                creditLimit = Convert.ToDecimal(txtCreditLimit.Text);

            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;
            if (interestComputationMode == "Diminishing Balance Method" && DiminishingCount == diminishingLineLimit)
            {
                if (creditLimit != 0 && loanAmount <= creditLimit)
                    return true;
            }
            return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking straight line loans of customer...")]//[f]
        public bool CheckStraightLine()
        {
            var interestComputationMode = cmbInterestComputationMode.SelectedItem.Text;
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            decimal? creditLimit = 0;
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text) || txtCreditLimit.Text == "Unlimited")
                creditLimit = decimal.MaxValue;
            else
                creditLimit = Convert.ToDecimal(txtCreditLimit.Text);

            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            if (interestComputationMode == "Straight Line Method" && (StraightLineCount == straightLineLimit))
            {
                if (creditLimit != 0 && loanAmount <= creditLimit)
                    return true;
            }
            return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking credit limit...")]//[g]
        public bool CheckCreditLimit()
        {
            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;

            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            bool result = true;
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text) == false && txtCreditLimit.Text != "Unlimited")
            {
                decimal creditLimit = Convert.ToDecimal(txtCreditLimit.Text);
                result = loanAmount > creditLimit;
            }
            else
                result = false;

            var interestComputationMode = cmbInterestComputationMode.SelectedItem.Text;
            if (interestComputationMode == "Straight Line Method" && (StraightLineCount == straightLineLimit))
                result &= StraightLineCount >= straightLineLimit;
            else
                result &= DiminishingCount >= diminishingLineLimit;

            return result;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking age validility of customer...")]
        public bool CheckCustomerAgeValidity(int partyRoleId)
        {
            int ageLimit = SystemSetting.AgeLimitOfBorrower;

            PartyRole partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
            if (partyRole == null)
                return false;

            Person person = partyRole.Party.Person;
            return person.Age >= ageLimit;
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving customer details...")]
        public void FillCustomerDetails(int partyRoleId)
        {
            storePayOutstandingLoan.RemoveAll();
            var imageFilename = "../../../Uploaded/Images/";
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.PartyRoleId = partyRoleId;
            CustomerDetailsModel model = new CustomerDetailsModel(partyRoleId);
            this.txtCustomerName.Text = model.Name;
            this.hiddenCustomerID.Value = partyRoleId;
            if (string.IsNullOrWhiteSpace(model.ImageUrl) == false)
                this.imgPersonPicture.ImageUrl = model.ImageUrl;
            else
                this.imgPersonPicture.ImageUrl = imageFilename + "../../../Resources/images/noimage.jpg";
            this.txtDistrict.Text = model.District;
            this.txtStationNumber.Text = model.StationNumber;
            this.txtCustomerStatus.Text = model.Status;
            this.txtCustomerType.Text = model.CustomerType;
            this.txtGender.Text = model.Gender;
            this.txtAge.Value = model.Age;
            if (model.CreditLimit != null)
                this.txtCreditLimit.Text = ((decimal)model.CreditLimit).ToString("N");
            this.txtPrimaryHomeAddress.Text = model.PrimaryHomeAddress;
            this.txtCellNumberCountryCode.Text = model.CountryCode;
            this.txtPrimaryTelCountryCode.Text = model.CountryCode;
            this.nfCellNumberAreaCode.Text = model.CellphoneAreaCode;
            this.nfCellNumberPhoneNumber.Text = model.CellphoneNumber;
            this.txtPrimaryTelAreaCode.Text = model.TelephoneNumberAreaCode;
            this.txtPrimaryTelPhoneNumber.Text = model.TelephoneNumber;
            this.txtPrimaryEmailAddress.Text = model.PrimaryEmailAddress;
            var list = form.RetrieveOutstandingLoans();
            if (list != null)
            {
                storePayOutstandingLoan.DataSource = list;
                storePayOutstandingLoan.DataBind();
                chkPayOutstandingLoan.Disabled = list.Count == 0;
            }
            else
            {
                chkPayOutstandingLoan.Checked = false;
                chkPayOutstandingLoan.Disabled = true;
            }

            SetAllInterestComputationModes();
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving financial product details...")]
        public void FillFinancialProductDetails(int financialProductId)
        {
            txtTermOption.Text = "";
            hiddenProductId.Value = financialProductId;
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            this.txtLoanProductName.Text = product.Name;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            var minimumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, product);
            var maximumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, product);

            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            this.hiddenLoanAmountLimit.Number = (double)form.LoanAmount;
            if (minimumLoanable != null)
                this.hiddenLoanAmountLimit.MinValue = (double)(minimumLoanable.Value ?? 0);
            if (maximumLoanable != null)
                this.hiddenLoanAmountLimit.MaxValue = (double)(maximumLoanable.Value ?? 0);

            hiddenLoanTermTimeUnitId.Value = minimumTerm.LoanTerm.UomId;
            this.nfLoanTerm.FieldLabel = string.Format("Loan Term ({0})", minimumTerm.LoanTerm.UnitOfMeasure.Name);

            nfLoanTerm.MinValue = minimumLoanTerm;
            nfLoanTerm.MaxValue = maximumLoanTerm;
            nfLoanTerm.ReadOnly = false;
            //if (chckWithTerm.Checked == true)
            nfLoanTerm.Number = nfLoanTerm.Number;

            var collateralRequirements = from pfa in
                                             ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.CollateralRequirementType, product)
                                         select new ProductFeatureApplicabilityModel(pfa);

            var interestComputationModes = from pfa in
                                               ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestComputationModeType, product)
                                           select new ProductFeatureApplicabilityModel(pfa);

            var methodOfChargingInterests = from pfa in
                                                ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.MethodofChargingInterestType, product)
                                            select new ProductFeatureApplicabilityModel(pfa);

            var interestRate = (from pfa in ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestRateType, product)
                                select pfa.Value).FirstOrDefault();

            var paymentMode = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType);
            var paymentUom = UnitOfMeasure.MonthlyType.Name;
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

            datLoanReleaseDate.SelectedDate = DateTime.Today;

            var termOption = (from pfa in ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.TermOptionType, product)
                              select pfa).FirstOrDefault();
            var termOptionName = termOption.ProductFeature.Name;
            hdnTermOption.Text = termOptionName;
            txtTermOption.Text = termOptionName;
            hdnTermOptionId.Value = termOption.Id;
            setPaymentDate(paymentUom);

            cmbInterestRate.SelectedIndex = 0;
            nfInterestRate.Text = interestRate.ToString();
        }

        [DirectMethod]
        public void AddFee(string productFeatureApplicabilityId, string feeName, string value)
        {
            int pfaId = -1;
            if (string.IsNullOrWhiteSpace(productFeatureApplicabilityId) == false)
                pfaId = int.Parse(productFeatureApplicabilityId);

            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            
            decimal loanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            int loanTerm = (int)this.nfLoanTerm.Number;
            int termUnit = Convert.ToInt32(hiddenLoanTermTimeUnitId.Value);
            int loanTermInMonth = UnitOfMeasure.LoanTermToMonth(termUnit, loanTerm);

            LoanFeeModel model;
            if (pfaId != -1)
            {
                ProductFeatureApplicability pfa = ProductFeatureApplicability.GetById(pfaId);
                model = new LoanFeeModel(pfa, loanAmount, loanTermInMonth);
            }
            else
            {
                decimal amount = decimal.Parse(value);
                int id = int.Parse(hiddenProductId.Text);
                model = new LoanFeeModel(feeName, amount, id, loanAmount, loanTermInMonth);
            }
           

            form.AddFee(model);
            StoreFee.DataSource = form.AvailableFees;
            StoreFee.DataBind();
        }

        [DirectMethod]
        public void AddGuarantor(int partyId, string name, string address)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            PersonnalPartyModel model = new PersonnalPartyModel();
            model.PartyId = partyId;
            model.Name = name;
            model.Address = address;

            form.AddGuarantor(model);
            StoreGuarantor.DataSource = form.AvailableGuarantors;
            StoreGuarantor.DataBind();
        }

        [DirectMethod]
        public void AddCoBorrower(int partyId, string name, string address)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            PersonnalPartyModel model = new PersonnalPartyModel();
            model.PartyId = partyId;
            model.Name = name;
            model.Address = address;

            form.AddCoBorrower(model);
            StoreCoBorrower.DataSource = form.AvailableCoBorrowers;
            StoreCoBorrower.DataBind();
        }

        [DirectMethod]
        public void UpdateCollaterals()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            StoreCollaterals.DataSource = form.AvailableCollaterals;
            StoreCollaterals.DataBind();
        }

        [DirectMethod]
        public void UpdateRequiredDocuments()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            StoreSubmittedDocuments.DataSource = form.AvailableSubmittedDocuments;
            StoreSubmittedDocuments.DataBind();
        }

        [DirectMethod]
        public void AddCheque(string id, string amount, string chequeNumber, DateTime date, string remarks, DateTime cheque)
        {
            var BankId = int.Parse(id);
            var Bank = ObjectContext.Banks.SingleOrDefault(entity => entity.PartyRoleId == BankId);
            decimal chequeAmount = decimal.Parse(amount);
            DateTime transactionDate = date;

            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            ChequeModel model = new ChequeModel(Bank, chequeAmount, chequeNumber, transactionDate, remarks, cheque);
            form.AddCheque(model);
            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();
        }
        
        protected void btnDeleteFee_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelFee.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveFee(row.RecordID);
            }

            StoreFee.DataSource = form.AvailableFees;
            StoreFee.DataBind();
        }

        protected void btnDeleteGuarantor_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelGuarantor.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveGuarantor(row.RecordID);
            }

            StoreGuarantor.DataSource = form.AvailableGuarantors;
            StoreGuarantor.DataBind();
        }

        protected void btnDeleteCollateral_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelCollaterals.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCollateral(row.RecordID);
            }

            StoreCollaterals.DataSource = form.AvailableCollaterals;
            StoreCollaterals.DataBind();
        }

        protected void btnDeleteCoBorrower_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelCoBorrower.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCoBorrower(row.RecordID);
            }

            StoreCoBorrower.DataSource = form.AvailableCoBorrowers;
            StoreCoBorrower.DataBind();
        }

        protected void btnDeleteDocument_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.RowSelectionModelDocuments.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveSubmittedDocument(row.RecordID);
            }

            StoreSubmittedDocuments.DataSource = form.AvailableSubmittedDocuments;
            StoreSubmittedDocuments.DataBind();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var interestRates = ProductFeature.All(ProductFeatureCategory.InterestRateType);
                storeInterestRate.DataSource = interestRates;
                storeInterestRate.DataBind();

                this.imgPersonPicture.ImageUrl = "../../../Resources/images/noimage.jpg";
                hdnStraightLineCount.Value = -1;
                hdnDiminishingCount.Value = -1;
                hdnLoanTermIndicator.Value = 0;
                this.dtApplicationDate.SelectedDate = DateTime.Today;
                this.txtLoanApplicationStatus.Text = "Pending: Approval";

                datLoanReleaseDate.MinDate = DateTime.Today;
                datPaymentStartDate.MinDate = DateTime.Today;

                this.ResourceGuid = Request.QueryString["ResourceGuid"];

                if (string.IsNullOrWhiteSpace(this.ResourceGuid))
                {
                    this.ResourceGuid = null;
                    this.CreateOrRetrieve<LoanApplicationForm>();
                }
                // else ApplyLoanOptions();

                this.hiddenResourceGuid.Value = this.ResourceGuid;
                txtPaymentMethod.Text = PaymentMethodType.PersonalCheckType.Name;

                var financialProduct = ObjectContext.FinancialProducts.FirstOrDefault();
                if (financialProduct != null)
                    FillFinancialProductDetails(financialProduct.Id);
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
                form.LoanReleaseDate = datLoanReleaseDate.SelectedDate.ToString();
                form.PaymentStartDate = datPaymentStartDate.SelectedDate.ToString();

                form.PartyRoleId = int.Parse(this.hiddenCustomerID.Text);
                form.FinancialProductId = int.Parse(hiddenProductId.Text);
                form.InterestRateDescription = cmbInterestRate.SelectedItem.Text;
                form.InterestRate = (decimal)nfInterestRate.Number;
                int featureId = int.Parse(cmbInterestRate.SelectedItem.Value);
                form.PFAInterestRateId = CreateOrRetrieveInterestRate(featureId, form.FinancialProductId, form.InterestRate);
                if (string.IsNullOrWhiteSpace(hiddenPastDueInterestRate.Text) == false)
                {
                    form.PFAPastDueInterestRateId = int.Parse(hiddenPastDueInterestRate.Text);
                    form.PastDueInterestRateDescription = txtPastDueDescription.Text;
                    form.PastDueInterestRate = (decimal)nfPastDueRate.Number;
                }

                form.StatusComment = txtStatusComment.Text;
                form.LoanApplicationDate = dtApplicationDate.SelectedDate;
                form.LoanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
                if (txtTermOption.Text == ProductFeature.NoTermType.Name) form.LoanTerm = 0;
                else form.LoanTerm = (int)nfLoanTerm.Number;
                //if (chckWithTerm.Checked == true)
                //{
                //    form.LoanTerm = (int)nfLoanTerm.Number;
                //    form.WithLoanTermIndicator = true;
                //}
                //else
                //{
                //    form.LoanTerm = 0;
                //    form.WithLoanTermIndicator = false;
                //}
                form.LoanTermUomId = int.Parse(hiddenLoanTermTimeUnitId.Text);
                form.LoanPurpose = txtLoanPurpose.Text;
                form.CollateralRequirementId = int.Parse(cmbCollateralRequirement.SelectedItem.Value);
                form.InterestComputationModeId = int.Parse(cmbInterestComputationMode.SelectedItem.Value);
                form.PaymentModeUomId = int.Parse(cmbPaymentMode.SelectedItem.Value);
                form.MethodOfChargingInterestId = int.Parse(cmbMethodOfChargingInterest.SelectedItem.Value);
                form.PayOutstandingLoanIndicator = chkPayOutstandingLoan.Checked;
                form.ProcessedByPartyId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                form.TermOptionId = int.Parse(hdnTermOptionId.Value.ToString());

                SelectedRowCollection rows = this.RowSelectionOutstandingLoans.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int loanId = int.Parse(row.RecordID);
                    form.AddToSelectedLoansToPayOff(loanId);
                }

                if (chckCheck.Checked)
                    form.WithCheckIndicator = true;
                else
                    form.WithCheckIndicator = false;

                //setPaymentDate();
                form.PaymentStartDate = datPaymentStartDate.SelectedDate.ToString();
                form.LoanReleaseDate = datLoanReleaseDate.SelectedDate.ToString();

                form.PrepareForSave();
            }
        }

        protected void btnRefresh_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            decimal loanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            int loanTerm = (int)this.nfLoanTerm.Number;
            int paymentMode = Convert.ToInt32(cmbPaymentMode.SelectedItem.Value);
            int loanTermInMonth = UnitOfMeasure.LoanTermToMonth(paymentMode, loanTerm);

            foreach (var fee in form.AvailableFees)
            {
                fee.CalculateCharge(loanAmount, loanTermInMonth);
            }

            StoreFee.DataSource = form.AvailableFees;
            StoreFee.DataBind();
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

        //Schedule and Cheques

        [DirectMethod]
        public bool setPaymentDate(string paymentMode)
        {
            hdnOnChangeDates.Value = "1";
            string termOption = hdnTermOption.Value.ToString();
            if (!string.IsNullOrWhiteSpace(this.txtLoanProductName.Text) &&
                !string.IsNullOrWhiteSpace(termOption))
            {
                if (string.IsNullOrWhiteSpace(paymentMode))
                        paymentMode = cmbPaymentMode.SelectedItem.Text;

                var dateManager = DateTimeOperationManager.AdjustPaymentStartDate(datLoanReleaseDate.SelectedDate);
                datPaymentStartDate.SelectedDate = dateManager;

                if (termOption == ProductFeature.AnyDayToSameDayOfNextMonth.Name)
                {
                    var paymentStartDate = datLoanReleaseDate.SelectedDate.AddMonths(1);
                    datPaymentStartDate.SelectedDate = paymentStartDate;
                    datPaymentStartDate.ReadOnly = true;
                }
                else if (termOption == ProductFeature.StartToEndOfMonthType.Name)
                {
                    var selectedDate = datLoanReleaseDate.SelectedDate;
                    var firstDayDate = new DateTime(selectedDate.Year, selectedDate.AddMonths(1).Month, 1);
                    datLoanReleaseDate.MinDate = firstDayDate;
                    datLoanReleaseDate.SelectedDate = firstDayDate;

                    dateManager = DateTimeOperationManager.AdjustPaymentStartDate(datLoanReleaseDate.SelectedDate);
                    datPaymentStartDate.SelectedDate = dateManager;

                    datLoanReleaseDate.ReadOnly = true;
                    datPaymentStartDate.ReadOnly = true;
                }

                return true;
            }
            else
                return false;
        }

        [DirectMethod]
        public void SetBank()
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            Bank bank = new Bank();
            if (chckBank.Checked)
            {
                //hdnChequeBankId.Value = hdnBankID.Value;
                bank = Bank.GetById(Convert.ToInt32(hdnBankID.Value));
                foreach (var item in form.AvailableCheques)
                {
                    item.BankId = bank.PartyRoleId;
                    item.BankName = bank.PartyRole.Party.Organization.OrganizationName;
                }
            }
        }

        [DirectMethod]
        public void FillCheques()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelCheque.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                hdnRandomKey.Value = row.RecordID;
                ChequeModel model = (ChequeModel)form.RetrieveCheque(row.RecordID);
                RetrieveCheque(model);
            }
        }

        [DirectMethod]
        public void ClearForm()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.AmortizationSchedules.Clear();
            form.Cheques.Clear();

            storeAmortizationSchedule.DataSource = form.AvailableSchedule;
            storeAmortizationSchedule.DataBind();

            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        protected void btnSaveNewCheque_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            ChequeModel model = form.RetrieveCheque((this.hdnRandomKey.Value).ToString());
            var bank = Bank.GetById(Convert.ToInt32(hdnBankID.Value));
            model.BankId = bank.PartyRoleId;
            model.BankName = bank.PartyRole.Party.Organization.OrganizationName;
            model.Amount = decimal.Parse(txtAmount.Text);
            model.ChequeNumber = txtCheckNumber.Text;
            model.TransactionDate = DateTime.Now;
            model.Remarks = string.Empty;
            model.ChequeDate = dtCheckDate.SelectedDate;

            if (chckBank.Checked)
            {
                SetBank();
            }

            form.AddCheque(model);
            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();
        }

        public void RetrieveCheque(ChequeModel model)
        {
            txtAmount.Text = model.Amount.ToString();
            txtBank.Text = model.BankName;
            txtCheckNumber.Text = model.ChequeNumber;
            dtCheckDate.SelectedDate = model.ChequeDate;
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.AmortizationSchedules.Clear();
            form.Cheques.Clear();

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
            options.PaymentStartDate = datPaymentStartDate.SelectedDate;
            options.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            options.TermOption = txtTermOption.Text;

            LoanCalculator calculator = new LoanCalculator();
            var models = calculator.GenerateLoanAmortization(options);



            storeAmortizationSchedule.DataSource = models;
            storeAmortizationSchedule.DataBind();

            List<AmortizationScheduleModel> sched = new List<AmortizationScheduleModel>();
            foreach (var item in models)
            {
                form.AddSchedule(item);
                sched.Add(item);
            }

            if (chckCheck.Checked)
            {
                List<ChequeModel> cheque = new List<ChequeModel>();

                foreach (AmortizationScheduleModel model in models)
                {
                    ChequeModel cheques = new ChequeModel();
                    cheques.Amount = model.TotalPayment;
                    cheques.ChequeDate = model.ScheduledPaymentDate;
                    form.AddCheque(cheques);
                    cheque.Add(cheques);
                }

                storeCheques.DataSource = cheque;
                storeCheques.DataBind();
            }
            else if (chckCheck.Checked == false)
            {
                this.SelectionModelCheque.SelectAll();
                grdpnlCheque.DeleteSelected();
                form.Cheques.Clear();
            }

            hdnOnChangeDates.Value = "0";
        }

        //Loan Options - Loan Calculator
        /**
        [DirectMethod(ShowMask = true, Msg = "Applying values from loan calculator...")]
        public void ApplyLoanOptions()
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            this.FillFinancialProductDetails(form.FinancialProductId);

            ProductFeatureApplicability pfa = ProductFeatureApplicability.GetById(form.PFAInterestRateId);
            cmbInterestRate.Value = pfa.ProductFeatureId;
            nfInterestRate.Number = (double)form.InterestRate;
            nfLoanAmount.Text = form.LoanAmount.ToString("N");
            if (chckWithTerm.Checked == true)
                nfLoanTerm.Number = (double)form.LoanTerm;
            else
                nfLoanTerm.Number = 0;
            cmbCollateralRequirement.Value = form.CollateralRequirementId;
            cmbInterestComputationMode.Value = form.InterestComputationModeId;
            cmbPaymentMode.Value = form.PaymentModeUomId;
            cmbMethodOfChargingInterest.Value = form.MethodOfChargingInterestId;
        }

        protected void btnApplyLoanOption_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.FinancialProductId = int.Parse(hiddenProductId.Text);

            if (string.IsNullOrWhiteSpace(cmbInterestRate.SelectedItem.Text) == false)
            {
                form.InterestRateDescription = cmbInterestRate.SelectedItem.Text;
                form.InterestRate = (decimal)nfInterestRate.Number;
                int featureId = int.Parse(cmbInterestRate.SelectedItem.Value);
                form.PFAInterestRateId = CreateOrRetrieveInterestRate(featureId, form.FinancialProductId, form.InterestRate);
            }
            form.LoanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            if (chckWithTerm.Checked == true)
            {
                form.LoanTerm = (int)nfLoanTerm.Number;
                form.WithLoanTermIndicator = true;
            }
            else
            {
                form.LoanTerm = 0;
                form.WithLoanTermIndicator = false;
            }
            form.LoanTermUomId = int.Parse(hiddenLoanTermTimeUnitId.Text);
            form.CollateralRequirementId = int.Parse(cmbCollateralRequirement.SelectedItem.Value);
            form.InterestComputationModeId = int.Parse(cmbInterestComputationMode.SelectedItem.Value);
            form.PaymentModeUomId = int.Parse(cmbPaymentMode.SelectedItem.Value);
            form.MethodOfChargingInterestId = int.Parse(cmbMethodOfChargingInterest.SelectedItem.Value);
        }
        **/
    }
}