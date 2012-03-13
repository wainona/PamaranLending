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
using System.IO;

namespace LendingApplication.Applications
{

    public partial class ModifyLoanApplication : ActivityPageBase
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

        [DirectMethod(ShowMask = true, Msg = "Checking loan term...")]//[c]
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

        [DirectMethod(ShowMask = true, Msg = "Checking credit...")]//[d]
        public bool CheckCredit()
        {
            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;

            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            decimal? creditLimit = 0;
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text))
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
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text))
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
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text))
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
            if (string.IsNullOrWhiteSpace(txtCreditLimit.Text) == false)
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
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.PartyRoleId = partyRoleId;
            CustomerDetailsModel model = new CustomerDetailsModel(partyRoleId);
            this.txtCustomerName.Text = model.Name;
            this.hiddenCustomerID.Value = partyRoleId;
            if (string.IsNullOrWhiteSpace(model.ImageUrl) == false)
                this.imgPersonPicture.ImageUrl = model.ImageUrl;
            else
                this.imgPersonPicture.ImageUrl = "../../../Resources/images/noimage.jpg";
            this.txtDistrict.Text = model.District;
            this.txtStationNumber.Text = model.StationNumber;
            this.txtCustomerType.Text = model.CustomerType;
            this.txtCustomerStatus.Text = model.Status;
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

            chkPayOutstandingLoan.Checked = false;
            chkPayOutstandingLoan.Disabled = true;
            if (form.LoanApplicationStatus == LoanApplicationStatusType.PendingApprovalType.Name)
            {
                var list = form.RetrieveOutstandingLoans();
                if (list != null)
                {
                    storePayOutstandingLoan.DataSource = list;
                    storePayOutstandingLoan.DataBind();
                    chkPayOutstandingLoan.Disabled = list.Count == 0;
                }
            }

            SetAllInterestComputationModes();
        }

        [DirectMethod(ShowMask = true, Msg = "Retrieving financial product details...")]
        public void FillFinancialProductDetails(int financialProductId)
        {
            hiddenProductId.Value = financialProductId;
            FinancialProduct product = FinancialProduct.GetById(financialProductId);
            this.txtLoanProductName.Text = product.Name;
            var paymentUom = UnitOfMeasure.MonthlyType.Name;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            var minimumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, product);
            var maximumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, product);

            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            this.hiddenLoanAmountLimit.Number = (double)form.LoanAmount;
            if (minimumLoanable != null)
                this.hiddenLoanAmountLimit.MinValue = (double) (minimumLoanable.Value ?? 0);
            if (maximumLoanable != null)
                this.hiddenLoanAmountLimit.MaxValue = (double) (maximumLoanable.Value ?? 0);

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

            var termOption = (from pfa in ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.TermOptionType, product)
                              select pfa).FirstOrDefault();

            var termOptionName = termOption.ProductFeature.Name;
            hdnTermOption.Text = termOptionName;
            hdnTermOptionId.Value = termOption.Id;
            txtTermOption.Text = termOptionName;
            setPaymentDate(paymentUom);

            var paymentMode = UnitOfMeasure.All(UnitOfMeasureType.TimeFrequencyType);
            storePaymentMethod.DataSource = paymentMode;
            storePaymentMethod.DataBind();
            cmbPaymentMode.SelectedIndex = 3;

            storeCollateralRequirement.DataSource = collateralRequirements;
            storeCollateralRequirement.DataBind();
            cmbCollateralRequirement.SelectedIndex = 0;

            storeInterestComputationMode.DataSource = interestComputationModes;
            storeInterestComputationMode.DataBind();
            cmbInterestComputationMode.SelectedIndex = 1;

            storeMethodOfChargingInterest.DataSource = methodOfChargingInterests;
            storeMethodOfChargingInterest.DataBind();
            cmbMethodOfChargingInterest.SelectedIndex = 0;
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

        protected void btnDeleteCheque_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            SelectedRowCollection rows = this.SelectionModelCheque.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCheque(row.RecordID);
            }
            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();
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

        [DirectMethod]
        public void Fill(int loanApplicationId)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            this.hiddenResourceGuid.Value = this.ResourceGuid;
            form.Retrieve(loanApplicationId);

            this.FillCustomerDetails(form.PartyRoleId);
            this.FillFinancialProductDetails(form.FinancialProductId);

            form.SetSelectedOutstandingLoansToPayoff(loanApplicationId, form.RetrieveOutstandingLoans());
            var ids = from ol in form.SelectedLoansToPayOff
                      select new SelectedRow { RecordID = ol.LoanId.ToString() };

            RowSelectionOutstandingLoans.SelectedRows.AddRange(ids);
            RowSelectionOutstandingLoans.UpdateSelection();
            this.txtStatusComment.Text = form.StatusComment;

            ProductFeatureApplicability pfa = ProductFeatureApplicability.GetById(form.PFAInterestRateId);
            cmbInterestRate.Value = pfa.ProductFeatureId;
            nfInterestRate.Number = (double)form.InterestRate;

            hiddenPastDueInterestRate.Text = form.PFAPastDueInterestRateId.ToString();
            txtPastDueDescription.Text = form.PastDueInterestRateDescription;
            nfPastDueRate.Number = (double)form.PastDueInterestRate;

            txtStatusComment.Text = form.StatusComment;
            dtApplicationDate.SelectedDate = form.LoanApplicationDate ;
            nfLoanAmount.Text = ((double)form.LoanAmount).ToString("N");
            var termOption = ProductFeatureApplicability.GetById(form.TermOptionId).ProductFeature.Name;
            txtTermOption.Text = termOption;
            if (termOption == ProductFeature.NoTermType.Name)
            {
                btnGenerate.Disabled = false;
                nfLoanTerm.Disabled = true;
                nfLoanTerm.Number = 0;
            }
            else
            {
                nfLoanTerm.Number = (double)form.LoanTerm;
            }
            //if (form.WithLoanTermIndicator == true)
            //{
            //    chckWithTerm.Checked = true;
            //    nfLoanTerm.Number = (double)form.LoanTerm;
            //}
            //else
            //{
            //    chckWithTerm.Checked = false;
            //    btnGenerate.Disabled = false;
            //    nfLoanTerm.Disabled = true;
            //    nfLoanTerm.Number = 0;
            //}
            txtLoanPurpose.Text = form.LoanPurpose;

            cmbCollateralRequirement.Value = form.CollateralRequirementId;
            cmbInterestComputationMode.Value = form.InterestComputationModeId;
            cmbPaymentMode.Value = form.PaymentModeUomId;
            cmbMethodOfChargingInterest.Value = form.MethodOfChargingInterestId;
            chkPayOutstandingLoan.Checked = form.PayOutstandingLoanIndicator;

            if (form.WithCheckIndicator == true)
                chckCheck.Checked = true;
            else
                chckCheck.Checked = false;

            datLoanReleaseDate.SelectedDate = DateTime.Parse(form.LoanReleaseDate);
            datPaymentStartDate.SelectedDate = DateTime.Parse(form.PaymentStartDate);

            StoreFee.DataSource = form.AvailableFees;
            StoreFee.DataBind();

            StoreCoBorrower.DataSource = form.AvailableCoBorrowers;
            StoreCoBorrower.DataBind();

            StoreGuarantor.DataSource = form.AvailableGuarantors;
            StoreGuarantor.DataBind();

            StoreCollaterals.DataSource = form.AvailableCollaterals;
            StoreCollaterals.DataBind();

            StoreSubmittedDocuments.DataSource = form.AvailableSubmittedDocuments;
            StoreSubmittedDocuments.DataBind();

            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();

            LoanApplication loanApplication = LoanApplication.GetById(loanApplicationId);

            var agr = ObjectContext.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplicationId);
            var schedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agr.Id);
            var items = ObjectContext.AmortizationScheduleItems.Where(entity => entity.AmortizationScheduleId == schedule.Id);
            var scheduleItems = LoanApplication.CreateScheduleItems(loanApplication, schedule);

            if (items.Count() == 0)
            {
                storeAmortizationSchedule.DataSource = scheduleItems;
                storeAmortizationSchedule.DataBind();
            }
            else
            {
                var type = UnitOfMeasure.GetByID(loanApplication.PaymentModeUomId).Name;
                int i = 0;
                foreach (var model in items)
                {
                    AmortizationScheduleModel newModel = new AmortizationScheduleModel();
                    newModel.Counter = GetItemType(type) + " " + (i + 1).ToString();
                    newModel.InterestPayment = model.InterestPayment;
                    newModel.IsBilledIndicator = model.IsBilledIndicator;
                    newModel.PrincipalBalance = model.PrincipalBalance;
                    newModel.PrincipalPayment = model.PrincipalPayment;
                    newModel.ScheduledPaymentDate = model.ScheduledPaymentDate;
                    newModel.TotalLoanBalance = model.TotalLoanBalance;
                    newModel.TotalPayment = model.PrincipalPayment + model.InterestPayment;
                    i++;

                    form.AddAmortizationModel(newModel);
                }

                storeAmortizationSchedule.DataSource = form.AmortizationSchedules;
                storeAmortizationSchedule.DataBind();
            }

            
            EnableValidActivity(loanApplication);
        }

        [Serializable]
        private class InterestComputationModeModel
        {
            public int DiminishingCount { get; set; }
            public int StraightLineCount { get; set; }
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

                hdnStraightLineCount.Value = -1;
                hdnDiminishingCount.Value = -1;

                int loanApplicationId = int.Parse(Request.QueryString["id"]);
                hdnLoanApplicationId.Value = loanApplicationId;
                var loan = LoanApplication.GetById(loanApplicationId);
                if (loan == null)
                    throw new AccessToDeletedRecordException("The loan application has been deleted by another user.");
                hiddenApplicationId.Value = loanApplicationId;
                Fill(loanApplicationId);
            }
        }

        protected void btnRefresh_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            decimal loanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            int loanTerm = 0;
            if (form.WithLoanTermIndicator == true)
                loanTerm = (int)this.nfLoanTerm.Number;
            int paymentMode = Convert.ToInt32(cmbPaymentMode.SelectedItem.Value);
            int loanTermInMonth = UnitOfMeasure.LoanTermToMonth(paymentMode, loanTerm);

            foreach (var fee in form.AvailableFees)
            {
                fee.CalculateCharge(loanAmount, loanTermInMonth);
            }

            StoreFee.DataSource = form.AvailableFees;
            StoreFee.DataBind();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {   
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
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
                form.LoanAmount = decimal.Parse(nfLoanAmount.Text);
                //if (chckWithTerm.Checked == true)
                //{
                //    form.LoanTerm = (int)nfLoanTerm.Number;
                //    form.WithLoanTermIndicator = true;
                //}
                //else
                //{
                //    form.LoanTerm = (int)nfLoanTerm.Number;
                //    form.WithLoanTermIndicator = false;
                //    LoanApplication.DeleteCheques(form.LoanApplicationId);
                //}
                form.LoanPurpose = txtLoanPurpose.Text;
                form.CollateralRequirementId = int.Parse(cmbCollateralRequirement.Value.ToString());
                form.InterestComputationModeId = int.Parse(cmbInterestComputationMode.Value.ToString());
                form.PaymentModeUomId = int.Parse(cmbPaymentMode.SelectedItem.Value);
                form.MethodOfChargingInterestId = int.Parse(cmbMethodOfChargingInterest.Value.ToString());
                form.PayOutstandingLoanIndicator = chkPayOutstandingLoan.Checked;
                form.ProcessedByPartyId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;

                SelectedRowCollection rows = this.RowSelectionOutstandingLoans.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int loanId = int.Parse(row.RecordID);
                    form.AddToSelectedLoansToPayOff(loanId);
                }

                form.PrepareForSave();
            }
        }

        protected void btnApprove_Click(object sender, DirectEventArgs e)
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
            form.LoanTerm = (int)nfLoanTerm.Number;
            form.LoanTermUomId = int.Parse(hiddenLoanTermTimeUnitId.Text);
            form.CollateralRequirementId = int.Parse(cmbCollateralRequirement.SelectedItem.Value);
            form.InterestComputationModeId = int.Parse(cmbInterestComputationMode.SelectedItem.Value);
            form.PaymentModeUomId = int.Parse(cmbPaymentMode.SelectedItem.Value);
            form.MethodOfChargingInterestId = int.Parse(cmbMethodOfChargingInterest.SelectedItem.Value);
        }

        protected void btnCancel_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                var loanApplication = LoanApplication.GetById(form.LoanApplicationId);
                var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                LoanApplicationOperations.Cancel(loanApplication, DateTime.Today, currentUser);
                EnableValidActivity(loanApplication);
            }
        }

        protected void btnReject_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                var loanApplication = LoanApplication.GetById(form.LoanApplicationId);
                var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                LoanApplicationOperations.Reject(loanApplication, DateTime.Today, currentUser);
                EnableValidActivity(loanApplication);
            }
        }

        public static string GetItemType(string unitOfMeasure)
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

        private void EnableValidActivity(LoanApplication loanApplication)
        {
            btnApprove.Disabled = true;
            btnCancel.Disabled = true;
            btnReject.Disabled = true;
            btnPrintPromissory.Disabled = true;
            btnPrintTempSched.Disabled = true;
            btnPrintSPA.Disabled = true;
            //btnClose.Disabled = true;

            var status = loanApplication.CurrentStatus;
            if (status == null) return;

            if (status.StatusTypeId != LoanApplicationStatusType.PendingApprovalType.Id)
            {
                if (btnSaveSeparator.Hidden == false)
                    btnSaveSeparator.Hidden = true;

                if (btnSave.Hidden == false)
                    btnSave.Hidden = true;

                if (btnOpen.Hidden == false)
                    btnOpen.Hidden = true;

                if (btnOpenSeparator.Hidden == false)
                    btnOpenSeparator.Hidden = true;

                if (btnApprove.Hidden == false)
                    btnApprove.Hidden = true;

                btnPrintPromissory.Disabled = false;
                btnPrintTempSched.Disabled = false;
                btnPrintSPA.Disabled = false;
            }

            if (LoanApplicationStatu.CanChangeStatusTo(status.LoanApplicationStatusType, LoanApplicationStatusType.CancelledType))
                btnCancel.Disabled = false;
            if (LoanApplicationStatu.CanChangeStatusTo(status.LoanApplicationStatusType, LoanApplicationStatusType.PendingInFundingType))
                btnApprove.Disabled = false;
            if (LoanApplicationStatu.CanChangeStatusTo(status.LoanApplicationStatusType, LoanApplicationStatusType.RejectedType))
                btnReject.Disabled = false;
            //if (LoanApplicationStatu.CanChangeStatusTo(status.LoanApplicationStatusType, LoanApplicationStatusType.ClosedType))
            //    btnClose.Disabled = false;
            this.txtLoanApplicationStatus.Text = status.LoanApplicationStatusType.Name;

            if (loanApplication.LoanTermLength <= 0)
                btnPrintTempSched.Disabled = true;
            else btnPrintTempSched.Disabled = false;
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
        //protected void FillCheques(object sender, DirectEventArgs e)
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

            storeCheques.DataSource = form.Cheques;
            storeCheques.DataBind();
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        public void RetrieveCheque(ChequeModel model)
        {
            txtAmount.Text = model.Amount.ToString();
            txtBank.Text = model.BankName;
            //txtCheckNumber.Text = model.ChequeNumber;
            dtCheckDate.SelectedDate = model.ChequeDate;
            var payment = ObjectContext.Payments.SingleOrDefault(e => e.Id == model.PaymentId);

            if (payment != null)
            {
                txtPaymentMethod.Text = payment.PaymentMethodType.Name;
                txtCheckNumber.Text = payment.PaymentReferenceNumber;
            }
            else
            {
                txtPaymentMethod.Text = PaymentMethodType.PersonalCheckType.Name;
            }

        }

        protected void btnSaveNewCheque_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            ChequeModel model = form.RetrieveCheque((this.hdnRandomKey.Value).ToString());
            if (string.IsNullOrWhiteSpace(hdnBankID.Value.ToString()))
                hdnBankID.Value = model.BankId;
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

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.CreateOrRetrieve<LoanApplicationForm>();
            form.AmortizationSchedules.Clear();
            form.Cheques.Clear();

            List<ChequeModel> cheque = new List<ChequeModel>();

            var today = DateTime.Now;
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            options.PaymentStartDate = datPaymentStartDate.SelectedDate;
            options.LoanAmount = Convert.ToDecimal(this.nfLoanAmount.Value);
            options.LoanTerm = (int)nfLoanTerm.Number;
            options.LoanTermId = form.LoanTermUomId;
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

            var loanTerm = int.Parse(nfLoanTerm.Text);
            if (chckCheck.Checked)
            {
                foreach (AmortizationScheduleModel model in models)
                {
                    form.AddAmortizationModel(model);
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
                foreach (AmortizationScheduleModel model in models)
                {
                    this.SelectionModelCheque.SelectAll();
                    grdpnlCheque.DeleteSelected();
                    form.Cheques.Clear();
                    form.AddAmortizationModel(model);
                }
            }

            hdnOnChangeDates.Value = "0";
        }

        //Close Loan Application
        /**protected void btnClose_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                var loanApplication = LoanApplication.GetById(form.LoanApplicationId);
                var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                LoanApplicationOperations.Close(loanApplication, DateTime.Today, currentUser);
                EnableValidActivity(loanApplication);
            }
        }
        **/

    }
}