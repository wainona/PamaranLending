using System;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Collections.Generic;
using System.IO;

namespace BusinessLogic.FullfillmentForms
{
    public class SPADocumentModel
    {
        public SignatureModel Borrower { get; set; }
        public SignatureModel Lender { get; set; }
        public SignatureModel Witness1 { get; set; }
        public SignatureModel Witness2 { get; set; }
        public SignatureModel Witness3 { get; set; }
        public SignatureModel Witness4 { get; set; }

        public SPADocumentModel()
        {
            Borrower = new SignatureModel();
            Lender = new SignatureModel();
            Witness1 = new SignatureModel();
            Witness2 = new SignatureModel();
            Witness3 = new SignatureModel();
            Witness4 = new SignatureModel();
        }
    }

    public class SignatureModel
    {
        public string RoleName { get; set; }
        public string FilePath { get; set; }
        public string PersonName { get; set; }

        public SignatureModel()
        {
            RoleName = "";
            FilePath = "";
            PersonName = "";
        }
    }

    public class LoanApprovalForm : FullfillmentForm<FinancialEntities>
    {
        public List<OutstandingLoan> SelectedLoansToPayOff { get; private set; }
        public List<AmortizationScheduleModel> AmortizationSchedules{ get; private set; }

        public SPADocumentModel SPADocumentDetails { get; set; }

        public int LoanApplicationId { get; private set; }

        public int CustomerPartyRoleId { get; private set; }

        public decimal? CustomerCreditLimit { get; private set; }

        public int FinancialProductId { get; private set; }

        public int PFAInterestRateId { get; private set; }

        public int InterestRateDescriptionId { get; private set; }

        public string InterestRateDescription { get; private set; }

        public decimal InterestRate { get; private set; }

        public decimal LoanAmount { get; set; }

        public int LoanTerm { get; set; }

        public int LoanTermUomId { get; private set; }

        public int CollateralRequirementId { get; private set; }

        public int InterestComputationModeId { get; private set; }

        public string InterestComputationMode { get; private set; }

        public int PaymentModeUomId { get; private set; }

        public string PaymentModeName { get; private set; }

        public int MethodOfChargingInterestId { get; private set; }

        public bool PayOutstandingLoanIndicator { get; private set; }

        public decimal TotalOfSelectedOutstandingLoans { get; private set; }

        public DateTime PaymentStartDate { get; set; }

        public DateTime LoanReleaseDate { get; set; }

        public int ProcessedByPartyId { get; set; }

        public bool HasSignatures { get; set; }

        public LoanApprovalForm()
        {
            this.CustomerPartyRoleId = -1;
            this.FinancialProductId = -1;
            this.PFAInterestRateId = -1;
            this.LoanApplicationId = -1;
            this.TotalOfSelectedOutstandingLoans = 0;

            SelectedLoansToPayOff = new List<OutstandingLoan>();
            AmortizationSchedules = new List<AmortizationScheduleModel>();
            SPADocumentDetails = new SPADocumentModel();
        }

        public override void Retrieve(int id)
        {
            this.LoanApplicationId = id;
            LoanApplication loanApplication = LoanApplication.GetById(id);
            if (loanApplication == null)
                throw new NotSupportedException("Loan application id doesn't exist in the database.");

            this.LoanTermUomId = loanApplication.LoanTermUomId;
            this.PaymentModeUomId = loanApplication.PaymentModeUomId;
            this.PaymentModeName = UnitOfMeasure.GetByID(this.PaymentModeUomId).Name;
            this.LoanTerm = loanApplication.LoanTermLength;
            this.LoanAmount = loanApplication.LoanAmount;
            this.PayOutstandingLoanIndicator = loanApplication.IsReloanIndicator;

            var application = loanApplication.Application;
            if (loanApplication.IsInterestProductFeatureInd)
            {
                this.InterestRate = loanApplication.InterestRate ?? 0;
                this.InterestRateDescription = loanApplication.InterestRateDescription;

                var interestRate = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.InterestRateType);
                this.PFAInterestRateId = interestRate.ProdFeatApplicabilityId;
                this.InterestRateDescriptionId = interestRate.ProductFeatureApplicability.ProductFeature.Id;
            }

            // retrieval of the customer as borrower party role id.
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);
            this.CustomerPartyRoleId = customerPartyRole.Id;
            this.CustomerCreditLimit = customerPartyRole.Customer.CreditLimit;

            var aiCollateralRequirement = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.CollateralRequirementType);
            var aiInterestComputationMode = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.InterestComputationModeType);
            var aiMethodOfChargingInterest = ApplicationItem.GetFirstActive(application, ProductFeatureCategory.MethodofChargingInterestType);

            this.FinancialProductId = aiCollateralRequirement.ProductFeatureApplicability.FinancialProductId;

            if (aiCollateralRequirement != null)
                this.CollateralRequirementId = aiCollateralRequirement.ProdFeatApplicabilityId;

            if (aiInterestComputationMode != null)
            {
                this.InterestComputationModeId = aiInterestComputationMode.ProdFeatApplicabilityId;
                InterestComputationMode = aiInterestComputationMode.ProductFeatureApplicability.ProductFeature.Name;
            }

            if (aiMethodOfChargingInterest != null)
                this.MethodOfChargingInterestId = aiMethodOfChargingInterest.ProdFeatApplicabilityId;

            if (this.PayOutstandingLoanIndicator)
            {
                this.TotalOfSelectedOutstandingLoans = loanApplication.LoanReAvailments.Sum(entity => entity.LoanBalance);
            }

            //form details
            var spaForm = FormDetail.GetByLoanAppIdAndType(this.LoanApplicationId, FormType.SPAType);

            foreach (var form in spaForm)
            {
                switch (form.RoleString)
	            {
                    case "Lender":
                        this.SPADocumentDetails.Lender.FilePath = form.Signature;
                        this.SPADocumentDetails.Lender.PersonName = form.PersonString;
                        break;
                    case "Borrower":
                        this.SPADocumentDetails.Borrower.FilePath = form.Signature;
                        break;
                    case "Witness1":
                        this.SPADocumentDetails.Witness1.FilePath = form.Signature;
                        this.SPADocumentDetails.Witness1.PersonName = form.PersonString;
                        break;
                    case "Witness2":
                        this.SPADocumentDetails.Witness2.FilePath = form.Signature;
                        this.SPADocumentDetails.Witness2.PersonName = form.PersonString;
                        break;
                    case "Witness3":
                        this.SPADocumentDetails.Witness3.FilePath = form.Signature;
                        this.SPADocumentDetails.Witness3.PersonName = form.PersonString;
                        break;
                    case "Witness4":
                        this.SPADocumentDetails.Witness4.FilePath = form.Signature;
                        this.SPADocumentDetails.Witness4.PersonName = form.PersonString;
                        break;
		            default:
                        break;
	            }
            }
        }

        public override void PrepareForSave()
        {
            DateTime today = DateTime.Now;
            var loanApplication = LoanApplication.GetById(LoanApplicationId);
            loanApplication.LoanAmount = this.LoanAmount;
            loanApplication.LoanTermLength = this.LoanTerm;
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            PartyRole borrowerPartyRole = PartyRole.GetPartyRoleFromLoanApplication(loanApplication, borrowerRoleType);
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(borrowerPartyRole.PartyId, RoleType.CustomerType);

            LoanApplicationStatu loanStatus = LoanApplicationStatu.CreateOrUpdateCurrent(loanApplication, LoanApplicationStatusType.PendingInFundingType, today);

            var agreement = Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);

            //Create new Loan Agreement Record
            //LoanAgreement loanAgreement = new LoanAgreement();
            //loanAgreement.Agreement = agreement;
            //Context.LoanAgreements.AddObject(loanAgreement);

            var agreementItem = CreateAgreementItem(loanApplication, agreement, today);
            Context.AgreementItems.AddObject(agreementItem);

            var borrower = RoleType.BorrowerAgreementType;
            var newBorrowerPartyRole = PartyRole.Create(borrower, customerPartyRole.Party, today);
            Context.PartyRoles.AddObject(newBorrowerPartyRole);

            var newBorrowerAgreementRole = CreateAgreementRole(agreement, newBorrowerPartyRole);
            Context.AgreementRoles.AddObject(newBorrowerAgreementRole);

            CreateAgreementForCoBorrowers(loanApplication, agreement, today);
            CreateAgreementForGuarantor(loanApplication, agreement, today);

            var pendingdvst = DisbursementVcrStatusEnum.PendingType;
            var disbursement = LoanDisbursementVcr.Create(loanApplication, agreement, today);
            var disbursementStatus = DisbursementVcrStatu.CreateOrUpdateCurrent(disbursement, pendingdvst, today);

            //approve loan disbursement voucher
            var approvedStatus = DisbursementVcrStatusEnum.ApprovedType;
            if (DisbursementVcrStatu.CanChangeStatus(disbursementStatus.DisbursementVcrStatusType, approvedStatus, today))
            {
                var changed = DisbursementVcrStatu.ChangeStatus(disbursement, approvedStatus, today);
            }

            //Insert loan clerk who processed the application
            PartyRole processedBy = PartyRole.CreateOrRetrieve(RoleType.ApprovedByAgreementType, this.ProcessedByPartyId, today);
            LoanApplicationRole loanApplicationRole = new LoanApplicationRole();
            loanApplicationRole.LoanApplication = loanApplication;
            loanApplicationRole.PartyRole = processedBy;

            //TODO:: Changed to new payment
            //Create Cheque status and receipt status
            var chequeAssocs = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
            foreach (var chequeAssoc in chequeAssocs)
            {
                var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == chequeAssoc.ChequeId);
                var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == cheque.PaymentId);
                Payment payment = rpAssoc.Payment;
                Receipt receipt = rpAssoc.Receipt;

                //CreateChequeStatus(cheque);
                //CreateReceiptStatus(payment);
                var receivedTypeId = ChequeStatusType.ReceivedType.Id;
                var openReceiptType = ReceiptStatusType.OpenReceiptStatusType;
                var chequeStatus = ChequeStatu.CreateChequeStatus(cheque, receivedTypeId, today, null, true);
                var receiptStatus = ReceiptStatu.Create(receipt, today, openReceiptType, true);

                Context.ChequeStatus.AddObject(chequeStatus);
                Context.ReceiptStatus.AddObject(receiptStatus);
                
            }

            //form details
            if (HasSignatures == true)
            {
                FormDetail lender = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Lender", SPADocumentDetails.Lender.PersonName, SPADocumentDetails.Lender.FilePath);
                FormDetail customer = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Borrower", SPADocumentDetails.Borrower.PersonName, SPADocumentDetails.Borrower.FilePath);
                FormDetail witness1 = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Witness1", SPADocumentDetails.Witness1.PersonName, SPADocumentDetails.Witness1.FilePath);
                FormDetail witness2 = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Witness2", SPADocumentDetails.Witness2.PersonName, SPADocumentDetails.Witness2.FilePath);
                FormDetail witness3 = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Witness3", SPADocumentDetails.Witness3.PersonName, SPADocumentDetails.Witness3.FilePath);
                FormDetail witness4 = FormDetail.Create(FormType.SPAType.Id, loanApplication, "Witness4", SPADocumentDetails.Witness4.PersonName, SPADocumentDetails.Witness4.FilePath);
            }
            //var schedule = Context.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
            //CreateScheduleItems(loanApplication, schedule);

            //if (agreementItem.MethodOfChargingInterest == ProductFeature.DiscountedInterestType.Name)
            //{
            //    if (AmortizationSchedules.Count > 1)
            //        disbursement.Balance -= AmortizationSchedules.First().InterestPaymentTotal;
            //    else
            //        throw new NotImplementedException("Loan without amortization schedule is currently not supported.");
            //    //todo: no amortization schedule
            //}
        }

        public void DeleteExistingSignature(string roleString, string newFilePath)
        {
            var detail = FormDetail.GetByLoanAppIdAndType(this.LoanApplicationId, FormType.SPAType).Where(entity => entity.RoleString == roleString);
            foreach (var item in detail)
            {
                if (File.Exists(item.Signature))
                {
                    File.Delete(item.Signature);
                }
                item.Signature = newFilePath;
            }
            Context.SaveChanges();
        }

        public void CreateScheduleItems(LoanApplication loanApplication, AmortizationSchedule schedule)
        {
            var today = DateTime.Now;
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.LoanReleaseDate = schedule.LoanReleaseDate;
            options.PaymentStartDate = schedule.PaymentStartDate;
            options.LoanAmount = loanApplication.LoanAmount;
            options.LoanTerm = loanApplication.LoanTermLength;
            options.LoanTermId = loanApplication.LoanTermUomId;
            var paymentMode = UnitOfMeasure.GetByID(loanApplication.PaymentModeUomId);
            options.PaymentMode = paymentMode.Name;
            options.PaymentModeId = paymentMode.Id;

            var aiInterestComputationMode = ApplicationItem.GetFirstActive(loanApplication.Application, ProductFeatureCategory.InterestComputationModeType);
            int interestComputation = 0;
            if (aiInterestComputationMode != null)
                interestComputation = aiInterestComputationMode.ProductFeatureApplicability.ProductFeatureId;
            var interestComputationMode = ProductFeature.GetById(interestComputation);

            var interestRate = ProductFeature.GetByName(loanApplication.InterestRateDescription);
            options.InterestComputationMode = interestComputationMode.Name;
            options.InterestRateDescription = interestRate.Name;
            options.InterestRate = loanApplication.InterestRate ?? 0;
            options.InterestRateDescriptionId = interestRate.Id;

            LoanCalculator calculator = new LoanCalculator();
            var items = calculator.GenerateLoanAmortization(options);

            foreach (var models in items)
	        {
                AmortizationScheduleItem item = new AmortizationScheduleItem();
                item.AmortizationSchedule = schedule;
                item.ScheduledPaymentDate = models.ScheduledPaymentDate;
                item.PrincipalPayment = models.PrincipalPayment;
                item.InterestPayment = models.InterestPayment;
                item.PrincipalBalance = models.PrincipalBalance;
                item.TotalLoanBalance = models.TotalLoanBalance;
                item.IsBilledIndicator = false;
                Context.AmortizationScheduleItems.AddObject(item);
	        }
        }

        public static IQueryable<LoanApplication> BorrowerLoans(Party customerParty)
        {
            var borrowerHasLoans = from loans in LoanApplication.GetAllLoanApplicationOf(customerParty, RoleType.BorrowerApplicationType)
                                   join loanStatus in Context.LoanApplicationStatus on loans.ApplicationId equals loanStatus.ApplicationId
                                   where loanStatus.IsActive == true &&
                                         (loanStatus.StatusTypeId == LoanApplicationStatusType.ApprovedType.Id ||
                                            loanStatus.StatusTypeId == LoanApplicationStatusType.ClosedType.Id ||
                                            loanStatus.StatusTypeId == LoanApplicationStatusType.PendingInFundingType.Id ||
                                            loanStatus.StatusTypeId == LoanApplicationStatusType.RestructuredType.Id)
                                   select loans;
            return borrowerHasLoans;
        }

        /// <summary>
        /// Clears the current amortization schedule and generate based on the properties of this object.
        /// </summary>
        public void ClearAndGenerateSchedule()
        {
            LoanCalculatorOptions options = new LoanCalculatorOptions();
            options.InterestComputationMode = this.InterestComputationMode;
            options.LoanAmount = this.LoanAmount;
            options.LoanTerm = this.LoanTerm;
            options.LoanTermId = this.LoanTermUomId;
            options.InterestRate = this.InterestRate;
            options.InterestRateDescription = this.InterestRateDescription;
            options.InterestRateDescriptionId = this.InterestRateDescriptionId;
            options.PaymentModeId = this.PaymentModeUomId;
            options.PaymentMode = this.PaymentModeName;
            options.PaymentStartDate = this.PaymentStartDate;
            options.LoanReleaseDate = this.LoanReleaseDate;

            LoanCalculator loanCalculator = new LoanCalculator();
            this.AmortizationSchedules = loanCalculator.GenerateLoanAmortization(options);
        }

        private void CreateAgreementForCoBorrowers(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            var applicationCoborrower = RoleType.CoBorrowerApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == applicationCoborrower.Id
                && entity.PartyRole.EndDate == null);

            var agreementCoBorrower = RoleType.CoBorrowerAgreementType;
            foreach (var role in roles)
            {
                PartyRole partyRole = role.PartyRole;
                var newpartyRole = PartyRole.Create(agreementCoBorrower, partyRole.Party, today);
                Context.PartyRoles.AddObject(newpartyRole);

                var agreementRole = CreateAgreementRole(agreement, newpartyRole);
                Context.AgreementRoles.AddObject(agreementRole);
            }

        }

        private void CreateAgreementForGuarantor(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            var applicationGuarantor = RoleType.GuarantorApplicationType;
            var roles = Context.LoanApplicationRoles.Where(entity => entity.ApplicationId == loanApplication.ApplicationId && entity.PartyRole.RoleTypeId == applicationGuarantor.Id
                 && entity.PartyRole.EndDate == null);

            var agreementGuarantor = RoleType.GuarantorAgreementType;
            foreach (var role in roles)
            {
                PartyRole partyRole = role.PartyRole;
                var newpartyRole = PartyRole.Create(agreementGuarantor, partyRole.Party, today);
                Context.PartyRoles.AddObject(newpartyRole);

                var agreementRole = CreateAgreementRole(agreement, newpartyRole);
                Context.AgreementRoles.AddObject(agreementRole);
            }
        }

        public AgreementRole CreateAgreementRole(Agreement agreement, PartyRole partyRole)
        {
            AgreementRole agreeRole = new AgreementRole();
            agreeRole.Agreement = agreement;
            agreeRole.PartyRole = partyRole;

            return agreeRole;
        }

        public AgreementItem CreateAgreementItem(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            AgreementItem agreementItem = new AgreementItem();
            agreementItem.Agreement = agreement;
            agreementItem.LoanAmount = loanApplication.LoanAmount;

            if (loanApplication.IsInterestProductFeatureInd)
            {
                agreementItem.InterestRate = loanApplication.InterestRate ?? 0;
                agreementItem.InterestRateDescription = loanApplication.InterestRateDescription;
            }

            if (loanApplication.IsPastDueProductFeatureInd)
            {
                agreementItem.PastDueInterestRateDescript = loanApplication.PastDueInterestDescription;
                agreementItem.PastDueInterestRate = loanApplication.PastDueInterestRate;
            }

            //descriptive values
            var uom = Context.UnitOfMeasures.SingleOrDefault(entity =>
                entity.Id == loanApplication.LoanTermUomId);
            var payment = Context.LoanApplications.FirstOrDefault(entity =>
                entity.UnitOfMeasure.Id == loanApplication.PaymentModeUomId);
            var featureAppMode = ProductFeatureApplicability.RetrieveFeature(ProductFeatureCategory.InterestComputationModeType, loanApplication);
            var fetaureAppMethod = ProductFeatureApplicability.RetrieveFeature(ProductFeatureCategory.MethodofChargingInterestType, loanApplication);

            agreementItem.LoanTermLength = loanApplication.LoanTermLength;
            agreementItem.LoanTermUom = uom.Name;
            agreementItem.PaymentMode = payment.UnitOfMeasure.Name;
            agreementItem.InterestComputationMode = featureAppMode.ProductFeature.Name;
            agreementItem.MethodOfChargingInterest = fetaureAppMethod.ProductFeature.Name;
            agreementItem.TransitionDateTime = today;
            agreementItem.IsActive = true;

            return agreementItem;
        }

        public AmortizationScheduleModel RetrieveSchedule(string counter)
        {
            return this.AmortizationSchedules.SingleOrDefault(entity => entity.Counter == counter);
        }
    }
}
