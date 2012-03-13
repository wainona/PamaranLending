using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public partial class LoanApplicationForm : FullfillmentForm<FinancialEntities>
    {
        public List<OutstandingLoan> SelectedLoansToPayOff { get; private set; }
        private List<LoanFeeModel> Fees;
        private List<Collateral> Collaterals;
        private List<PersonnalPartyModel> Guarantors;
        private List<PersonnalPartyModel> CoBorrowers;
        private List<SubmittedDocumentModel> SubmittedDocuments;
        public List<ChequeModel> Cheques;
        public List<AmortizationScheduleModel> AmortizationSchedules;
        public List<AmortizationItemsModel> AmortizationItems;

        public IEnumerable<LoanFeeModel> AvailableFees
        {
            get
            {
                return this.Fees.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<Collateral> AvailableCollaterals
        {
            get
            {
                return this.Collaterals.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<PersonnalPartyModel> AvailableGuarantors
        {
            get
            {
                return this.Guarantors.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<PersonnalPartyModel> AvailableCoBorrowers
        {
            get
            {
                return this.CoBorrowers.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<SubmittedDocumentModel> AvailableSubmittedDocuments
        {
            get
            {
                return this.SubmittedDocuments.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<ChequeModel> AvailableCheques
        {
            get
            {
                return this.Cheques.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<AmortizationScheduleModel> AvailableSchedule
        {
            get
            {
                return this.AmortizationSchedules.Where(model => model.ToBeDeleted == false);
            }
        }
        public IEnumerable<AmortizationItemsModel> AvailableItems
        {
            get
            {
                return this.AmortizationItems.Where(model => model.ToBeDeleted == false);
            }
        }

        public int LoanApplicationId { get; set; }

        public int PartyRoleId{ get; set; }

        public int FinancialProductId { get; set; }

        public int PFAInterestRateId { get; set; }

        public string InterestRateDescription { get; set; }

        public decimal InterestRate { get; set; }

        public int PFAPastDueInterestRateId { get; set; }

        public string PastDueInterestRateDescription { get; set; }

        public decimal PastDueInterestRate { get; set; }

        public string StatusComment { get; set; }

        public string LoanApplicationStatus { get; private set; }

        public DateTime LoanApplicationDate { get; set; }

        public decimal LoanAmount { get; set; }

        public int LoanTerm { get; set; }

        public int LoanTermUomId { get; set; }

        public string LoanPurpose { get; set; }

        public int CollateralRequirementId { get; set; }

        public int InterestComputationModeId { get; set; }

        public int PaymentModeUomId { get; set; }

        public int MethodOfChargingInterestId { get; set; }

        public bool PayOutstandingLoanIndicator { get; set; }

        public int ProcessedByPartyId { get; set; }

        public bool WithLoanTermIndicator { get; set; }

        public bool WithCheckIndicator { get; set; }

        public string LoanReleaseDate { get; set; }

        public string PaymentStartDate { get; set; }

        public int TermOptionId { get; set; }

        public LoanApplicationForm()
        {
            this.PartyRoleId = -1;
            this.FinancialProductId = -1;
            this.PFAInterestRateId = -1;
            this.PFAPastDueInterestRateId = -1;
            this.LoanApplicationId = -1;
            Fees = new List<LoanFeeModel>();
            Collaterals = new List<Collateral>();
            Guarantors = new List<PersonnalPartyModel>();
            CoBorrowers = new List<PersonnalPartyModel>();
            SubmittedDocuments = new List<SubmittedDocumentModel>();
            AmortizationSchedules = new List<AmortizationScheduleModel>();
            Cheques = new List<ChequeModel>();

            SelectedLoansToPayOff = new List<OutstandingLoan>();
        }

        public void AddFee(LoanFeeModel model)
        {
            if (model.ProductFeatureApplicabilityId != -1)
            {
                int count = this.Fees.Where(fee => fee.ProductFeatureApplicabilityId == model.ProductFeatureApplicabilityId).Count();
                if (count > 0)
                    return;
            }
            if (this.Fees.Contains(model))
                return;
            this.Fees.Add(model);
        }

        public void RemoveFee(string randomKey)
        {
            LoanFeeModel model = this.Fees.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveFee(model);
        }

        public void RemoveFee(LoanFeeModel model)
        {
            if (this.Fees.Contains(model) == true)
            {
                if (model.IsNew)
                    Fees.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void AddCollateral(Collateral model)
        {
            if (this.Collaterals.Contains(model))
                return;
            this.Collaterals.Add(model);
        }

        public void RemoveCollateral(string randomKey)
        {
            var model = this.Collaterals.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCollateral(model);
        }

        public void RemoveCollateral(Collateral model)
        {
            if (this.Collaterals.Contains(model) == true)
            {
                if (model.IsNew)
                    Collaterals.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public Collateral RetrieveCollateral(string randomKey)
        {
            return this.Collaterals.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public void AddGuarantor(PersonnalPartyModel model)
        {
            if (this.Guarantors.Contains(model))
                return;
            this.Guarantors.Add(model);
        }

        public void RemoveGuarantor(string randomKey)
        {
            PersonnalPartyModel model = this.Guarantors.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveGuarantor(model);
        }

        public void RemoveGuarantor(PersonnalPartyModel model)
        {
            if (this.Guarantors.Contains(model) == true)
            {
                if (model.IsNew)
                    Guarantors.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void AddCoBorrower(PersonnalPartyModel model)
        {
            if (this.CoBorrowers.Contains(model))
                return;
            this.CoBorrowers.Add(model);
        }

        public void RemoveCoBorrower(string randomKey)
        {
            PersonnalPartyModel model = this.CoBorrowers.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveCoBorrower(model);
        }

        public void RemoveCoBorrower(PersonnalPartyModel model)
        {
            if (this.CoBorrowers.Contains(model) == true)
            {
                if (model.IsNew)
                    CoBorrowers.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void AddSubmittedDocument(SubmittedDocumentModel model)
        {
            if (this.SubmittedDocuments.Contains(model))
                return;
            this.SubmittedDocuments.Add(model);
        }

        public void RemoveSubmittedDocument(string randomKey)
        {
            SubmittedDocumentModel model = this.SubmittedDocuments.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveSubmittedDocument(model);
        }

        public void RemoveSubmittedDocument(SubmittedDocumentModel model)
        {
            if (this.SubmittedDocuments.Contains(model) == true)
            {
                if (model.IsNew)
                    SubmittedDocuments.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public SubmittedDocumentModel RetrieveSubmittedDocument(string randomKey)
        {
            return this.SubmittedDocuments.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public void AddCheque(ChequeModel model)
        {
            if (this.Cheques.Contains(model))
                return;
            this.Cheques.Add(model);
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

        public void AddSchedule(AmortizationScheduleModel model)
        {
            if (this.AmortizationSchedules.Contains(model))
                return;
            this.AmortizationSchedules.Add(model);
        }

        public void RemoveSchedule(string randomKey)
        {
            AmortizationScheduleModel model = this.AmortizationSchedules.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveSchedule(model);
        }

        public void RemoveSchedule(AmortizationScheduleModel model)
        {
            if (this.AmortizationSchedules.Contains(model) == true)
            {
                if (model.IsNew)
                    AmortizationSchedules.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public ChequeModel RetrieveCheque(string randomKey)
        {
            return this.Cheques.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }
        
        public override void Retrieve(int id)
        {
            this.LoanApplicationId = id;
            LoanApplication loanApplication = Context.LoanApplications.SingleOrDefault(entity => entity.ApplicationId == id);
            if (loanApplication == null)
                throw new NotSupportedException("Loan application id doesn't exist in the database.");

            this.LoanApplicationDate = loanApplication.Application.ApplicationDate;
            this.LoanTermUomId = loanApplication.LoanTermUomId;
            this.PaymentModeUomId = loanApplication.PaymentModeUomId;
            this.LoanAmount = loanApplication.LoanAmount;
            this.LoanPurpose = loanApplication.Purpose;
            this.PayOutstandingLoanIndicator = loanApplication.IsReloanIndicator;
            this.LoanTerm = loanApplication.LoanTermLength;
            if (loanApplication.LoanTermLength != 0)
                this.WithLoanTermIndicator = true;
            else
            {
                this.WithLoanTermIndicator = false;
            }

            var application = loanApplication.Application;
            if (loanApplication.IsInterestProductFeatureInd)
            {
                this.InterestRate = loanApplication.InterestRate ?? 0;
                this.InterestRateDescription = loanApplication.InterestRateDescription;

                var interestRate = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.InterestRateType);
                this.PFAInterestRateId = interestRate.ProdFeatApplicabilityId;
            }
            if (loanApplication.IsPastDueProductFeatureInd)
            {
                this.PastDueInterestRate = loanApplication.PastDueInterestRate ?? 0;
                this.PastDueInterestRateDescription = loanApplication.PastDueInterestDescription;

                var pastDueRate = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.PastDueInterestRateType);
                this.PFAPastDueInterestRateId = pastDueRate.ProdFeatApplicabilityId;
            }

            
            var status = loanApplication.CurrentStatus;
            if (status != null)
            {
                this.StatusComment = status.Remarks;
                this.LoanApplicationStatus = status.LoanApplicationStatusType.Name;
            }

            // retrieval of the customer as borrower party role id.
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);
            this.PartyRoleId = customerPartyRole.Id;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.CollateralRequirementType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.InterestComputationModeType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.MethodofChargingInterestType);
            var aiTermOption = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.TermOptionType);

            this.FinancialProductId = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;

            if (aiCollateralRequirement != null)
                this.CollateralRequirementId = aiCollateralRequirement.ProdFeatApplicabilityId;
            if (aiInterestComputationMode != null)
                this.InterestComputationModeId = aiInterestComputationMode.ProdFeatApplicabilityId;
            if (aiMethodOfChargingInterest != null)
                this.MethodOfChargingInterestId = aiMethodOfChargingInterest.ProdFeatApplicabilityId;
            if (aiTermOption != null)
                this.TermOptionId = aiTermOption.ProdFeatApplicabilityId;

            var processedBy = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, RoleType.ProcessedByApplicationType);
            if (processedBy != null)
                this.ProcessedByPartyId = processedBy.PartyId;

            var agreement = Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
            var amort = Context.AmortizationSchedules.SingleOrDefault(entity =>
                entity.AgreementId == agreement.Id);
            this.LoanReleaseDate = amort.LoanReleaseDate.ToString();
            this.PaymentStartDate = amort.PaymentStartDate.ToString();

            //Insert to table FOR EACH Fees in the grid.
            RetrieveFees(loanApplication);

            //Insert to table FOR EACH Co-borrower in the grid.
            RetrieveCoBorrower(loanApplication);

            //Insert to table FOR EACH Guarantor in the grid.
            RetrieveGuarantor(loanApplication);

            //Insert to table FOR EACH Collaterals in the grid.
            RetrieveCollaterals(loanApplication, customerPartyRole);

            //Insert to table FOR EACH SubmittedDocuments in the grid.
            RetrieveSubmittedDocumentsForm(loanApplication);

            //Insert to table FOR EACH Cheque in the grid.
            RetrieveCheques(loanApplication);
            if (Cheques.Count > 0)
                WithCheckIndicator = true;


            //Insert to table FOR EACH AmortizationSchedule in the grid.
            //RetrieveScehduleItems(loanApplication);
        }

        public override void PrepareForSave()
        {
            var today = DateTime.Now;

            if (this.LoanApplicationId == -1)
            {
                SaveLoanInformation(today);
            }
            else
            {
                LoanApplication loanApplication = Context.LoanApplications.SingleOrDefault(entity => entity.ApplicationId == this.LoanApplicationId);
                if (loanApplication == null)
                    throw new NotSupportedException("Loan application id doesn't exist in the database.");
                UpdateLoanInformation(loanApplication,  today);
            }
        }

        public void SetSelectedOutstandingLoansToPayoff(int loanApplicationId, List<OutstandingLoan> outstandingLoans)
        {
            LoanApplication loanApplication = Context.LoanApplications.SingleOrDefault(entity => entity.ApplicationId == loanApplicationId);
            if (this.PayOutstandingLoanIndicator)
            {
                var selected = from la in loanApplication.LoanReAvailments
                               join ol in outstandingLoans on la.FinancialAccountId equals ol.LoanId
                               select ol;
                this.SelectedLoansToPayOff.AddRange(selected);
            }
        }

        private void SaveOutstandingLoansToPayOff(LoanApplication loanApplication)
        {
            if (this.PayOutstandingLoanIndicator)
            {
                var toAddEntities = this.SelectedLoansToPayOff.Where(sl => 
                    loanApplication.LoanReAvailments.Count(lar=>lar.FinancialAccountId == sl.LoanId) == 0);

                var toRemoveEntities = loanApplication.LoanReAvailments.Where(lar => 
                    this.SelectedLoansToPayOff.Count(sl => sl.LoanId == lar.FinancialAccountId) == 0);

                foreach (var outstandingLoan in toAddEntities)
                {
                    LoanReAvailment reAvailment = new LoanReAvailment();
                    reAvailment.FinancialAccountId = outstandingLoan.LoanId;
                    reAvailment.LoanApplication = loanApplication;
                    reAvailment.LoanBalance = outstandingLoan.LoanBalance;
                    reAvailment.NoOfInstallments = outstandingLoan.NoOfInstallments;

                    Context.LoanReAvailments.AddObject(reAvailment);
                }

                foreach (var entity in toRemoveEntities)
                {
                    Context.LoanReAvailments.DeleteObject(entity);
                }
            }
            else
            {
                foreach (var entity in loanApplication.LoanReAvailments.ToList())
                {
                    Context.LoanReAvailments.DeleteObject(entity);
                }
            }
        }

        public void UpdateLoanInformation(LoanApplication loanApplication, DateTime today)
        {
            var pendingApproval = LoanApplicationStatusType.PendingApprovalType;

            Application application = loanApplication.Application;
            application.ApplicationDate = this.LoanApplicationDate;

            loanApplication.LoanTermUomId = this.LoanTermUomId;
            loanApplication.PaymentModeUomId = this.PaymentModeUomId;
            if (this.WithLoanTermIndicator == true)
                loanApplication.LoanTermLength = this.LoanTerm;
            else
                loanApplication.LoanTermLength = 0;
            loanApplication.LoanAmount = this.LoanAmount;
            loanApplication.Purpose = this.LoanPurpose;
            loanApplication.IsReloanIndicator = this.PayOutstandingLoanIndicator;
            loanApplication.IsInterestProductFeatureInd = (this.PFAInterestRateId != -1);
            loanApplication.IsPastDueProductFeatureInd = (this.PFAPastDueInterestRateId != -1);

            if (loanApplication.IsInterestProductFeatureInd)
            {
                loanApplication.InterestRate = this.InterestRate;
                loanApplication.InterestRateDescription = this.InterestRateDescription;

                ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.InterestRateType, this.PFAInterestRateId, today);
            }

            if (loanApplication.IsPastDueProductFeatureInd)
            {
                loanApplication.PastDueInterestRate = this.PastDueInterestRate;
                loanApplication.PastDueInterestDescription = this.PastDueInterestRateDescription;

                ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.PastDueInterestRateType, this.PFAPastDueInterestRateId, today);
            }

            var status = loanApplication.CurrentStatus;
            if (status != null)
                status.Remarks = this.StatusComment;

            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.CollateralRequirementType, this.CollateralRequirementId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.InterestComputationModeType, this.InterestComputationModeId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.MethodofChargingInterestType, this.MethodOfChargingInterestId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.TermOptionType, this.TermOptionId, today);

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);

            SaveOutstandingLoansToPayOff(loanApplication);

            //Insert loan clerk who processed the application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                var party = Party.GetById(this.ProcessedByPartyId);
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplication, party, RoleType.ProcessedByApplicationType, today);
            }

            //Insert to table FOR EACH Fees in the grid.
            SaveFees(loanApplication, today);

            //Insert to table FOR EACH Co-borrower in the grid.
            SaveCoBorrower(loanApplication, today);

            //Insert to table FOR EACH Guarantor in the grid.
            SaveGuarantor(loanApplication, today);

            //Insert to table FOR EACH Collaterals in the grid.
            SaveCollaterals(loanApplication, customerPartyRole, today);

            //Insert to table FOR EACH SubmittedDocuments in the grid.
            SaveSubmittedDocumentsForm(loanApplication, today);

            //Insert to table FOR EACH Cheques in the grid.
            SaveCheques(loanApplication, customerPartyRole, today);

            var loanReleaseDate = DateTime.Parse(this.LoanReleaseDate);
            var paymentStartDate = DateTime.Parse(this.PaymentStartDate);
            var agreement = Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
            var schedule = Context.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
            schedule.LoanReleaseDate = loanReleaseDate;
            schedule.PaymentStartDate = paymentStartDate;

            //Insert to table FOR EACH Amortization Schedule Item in the grid.
            SaveSchedule(loanApplication, schedule);
        }

        public void AddToSelectedLoansToPayOff(int loanId)
        {
            if (cache == null)
                return;

            var model = this.cache.SingleOrDefault(entity => entity.LoanId == loanId);
            if(model != null)
            this.SelectedLoansToPayOff.Add(model);
        }

        private void SaveLoanInformation(DateTime today)
        {
            var pendingApproval = LoanApplicationStatusType.PendingApprovalType;

            Application application = new Application();
            application.ApplicationType1 = ApplicationType.LoanApplicationType;
            application.ApplicationDate = this.LoanApplicationDate;

            Context.Applications.AddObject(application);

            LoanApplication loanApplication = new LoanApplication();
            loanApplication.Application = application;
            loanApplication.LoanTermUomId = this.LoanTermUomId;
            loanApplication.PaymentModeUomId = this.PaymentModeUomId;
            loanApplication.LoanTermLength = this.LoanTerm;
            if (loanApplication.LoanTermLength != 0)
                this.WithLoanTermIndicator = true;
            else
                this.WithLoanTermIndicator = false;
            loanApplication.LoanAmount = this.LoanAmount;
            loanApplication.Purpose = this.LoanPurpose;
            loanApplication.IsReloanIndicator = this.PayOutstandingLoanIndicator;
            loanApplication.IsInterestProductFeatureInd = (this.PFAInterestRateId != -1);
            loanApplication.IsPastDueProductFeatureInd = (this.PFAPastDueInterestRateId != -1);            

            if (loanApplication.IsInterestProductFeatureInd)
            {
                loanApplication.InterestRate = this.InterestRate;
                loanApplication.InterestRateDescription = this.InterestRateDescription;

                ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.InterestRateType, this.PFAInterestRateId, today);
            }

            if (loanApplication.IsPastDueProductFeatureInd)
            {
                loanApplication.PastDueInterestRate = this.PastDueInterestRate;
                loanApplication.PastDueInterestDescription = this.PastDueInterestRateDescription;

                ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.PastDueInterestRateType, this.PFAPastDueInterestRateId, today);
            }

            LoanApplicationStatu status = new LoanApplicationStatu();
            status.LoanApplicationStatusType = pendingApproval;
            status.LoanApplication = loanApplication;
            status.Remarks = this.StatusComment;
            status.TransitionDateTime = today;
            status.IsActive = true;

            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole customerPartyRole = PartyRole.GetById(this.PartyRoleId);
            PartyRole borrowerPartyRole = PartyRole.CreateLoanApplicationRole(loanApplication, borrowerRoleType, customerPartyRole.Party, today);

            if (customerPartyRole.Customer != null)
            {
                var customerStatus = customerPartyRole.Customer.CurrentStatus;
                if (customerStatus.CustomerStatusTypeId == CustomerStatusType.NewType.Id
                || customerStatus.CustomerStatusTypeId == CustomerStatusType.InactiveType.Id)
                {
                    CustomerStatu.ChangeStatus(customerPartyRole, CustomerStatusType.ActiveType, today);
                }
            }

            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.CollateralRequirementType, this.CollateralRequirementId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.InterestComputationModeType, this.InterestComputationModeId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.MethodofChargingInterestType, this.MethodOfChargingInterestId, today);
            ApplicationItem.CreateOrUpdate(application, ProductFeatureCategory.TermOptionType, this.TermOptionId, today);

            if (this.PayOutstandingLoanIndicator)
            {
                foreach (var outstandingLoan in this.SelectedLoansToPayOff)
                {
                    LoanReAvailment reAvailment = new LoanReAvailment();
                    reAvailment.FinancialAccountId = outstandingLoan.LoanId;
                    reAvailment.LoanApplication = loanApplication;
                    reAvailment.LoanBalance = outstandingLoan.LoanBalance;
                    reAvailment.NoOfInstallments = outstandingLoan.NoOfInstallments;

                    Context.LoanReAvailments.AddObject(reAvailment);
                }
            }

            //Insert loan clerk who processed the application
            if (RoleType.ProcessedByApplicationType.PartyRoleType != null)
            {
                var party = Party.GetById(this.ProcessedByPartyId);
                PartyRole processedBy = PartyRole.CreateAndUpdateCurrentLoanApplicationRole
                    (loanApplication, party, RoleType.ProcessedByApplicationType, today);
            }

            //Insert to table FOR EACH Fees in the grid.
            SaveFees(loanApplication, today);

            //Insert to table FOR EACH Co-borrower in the grid.
            SaveCoBorrower(loanApplication, today);

            //Insert to table FOR EACH Guarantor in the grid.
            SaveGuarantor(loanApplication, today);

            //Insert to table FOR EACH Collaterals in the grid.
            SaveCollaterals(loanApplication, customerPartyRole, today);

            //Insert to table FOR EACH SubmittedDocuments in the grid.
            SaveSubmittedDocumentsForm(loanApplication, today);

            //Insert to table FOR EACH Cheque in the grid.
            SaveCheques(loanApplication, customerPartyRole, today);

            //Save Amortization Schedule
            var loanReleaseDate = DateTime.Parse(this.LoanReleaseDate);
            var paymentStartDate = DateTime.Parse(this.PaymentStartDate);
            var agreement = Agreement.Create(loanApplication, AgreementType.LoanAgreementType, today);

            LoanAgreement loanAgreement = new LoanAgreement();
            loanAgreement.Agreement = agreement;
            Context.LoanAgreements.AddObject(loanAgreement);

            AmortizationSchedule schedule = new AmortizationSchedule();
            schedule.DateGenerated = today;
            schedule.EffectiveDate = today;
            schedule.LoanReleaseDate = loanReleaseDate;
            schedule.PaymentStartDate = paymentStartDate;
            schedule.LoanAgreement = loanAgreement;

            if (Cheques.Count > 0)
                this.WithCheckIndicator = true;

            //Insert to table FOR EACH Amortization Schedule in the grid.
            SaveSchedule(loanApplication, schedule);
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

        private void RetrieveCheques(LoanApplication loanApplication)
        {
            var assoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
            foreach (ChequeApplicationAssoc association in assoc)
            {
                var cheques = Context.Cheques.Single(entity => entity.Id == association.ChequeId);
                this.Cheques.Add(new ChequeModel(cheques));
            }
        }

        private void RetrieveScehduleItems(LoanApplication loanApplication)
        {
            var agreement = Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
            var schedule = Context.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
            var items = Context.AmortizationScheduleItems.Where(entity => entity.AmortizationScheduleId == schedule.Id);
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

                this.AmortizationSchedules.Add(new AmortizationScheduleModel(newModel));
            }
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
                model.PrepareForSave(loanApplication, coborrower, today);
            }
        }

        private void SaveGuarantor(LoanApplication loanApplication, DateTime today)
        {
            var guarantor = RoleType.GuarantorApplicationType;
            foreach (PersonnalPartyModel model in Guarantors)
            {
                model.PrepareForSave(loanApplication, guarantor, today);
            }
        }

        private void SaveSubmittedDocumentsForm(LoanApplication loanApplication, DateTime today)
        {
            foreach (SubmittedDocumentModel model in SubmittedDocuments)
            {
                model.PrepareForSave(loanApplication, today);
            }
        }

        private void SaveCheques(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            var employeePartyRole = Context.PartyRoles.FirstOrDefault(entity => entity.PartyId == this.ProcessedByPartyId);
            foreach (ChequeModel model in Cheques)
            {
                model.PrepareForSave(loanApplication, customerPartyRole, employeePartyRole.Id, today);
            }
        }

        private void SaveSchedule(LoanApplication la, AmortizationSchedule schedule)
        {
            var items = Context.AmortizationScheduleItems.Where(e => e.AmortizationScheduleId == schedule.Id);
            foreach (var item in items)
            {
                Context.AmortizationScheduleItems.DeleteObject(item);
            }

            foreach (AmortizationScheduleModel model in AmortizationSchedules)
            {
                model.PrepareForSave(la, schedule, model);
            }
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

        public AmortizationScheduleModel RetrieveSchedule(string RandomKey)
        {
            return this.AmortizationSchedules.SingleOrDefault(entity => entity.RandomKey == RandomKey);
        }

        public void AddAmortizationModel(AmortizationScheduleModel model)
        {
            if (this.AmortizationSchedules.Contains(model))
                return;
            this.AmortizationSchedules.Add(model);
        }

    }
}
