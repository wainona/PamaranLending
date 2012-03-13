using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace BusinessLogic
{
    public partial class LoanApplication
    {
        public LoanApplicationStatu CurrentStatus
        {
            get
            {
                return LoanApplicationStatu.GetActive(this);
            }
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

        public static LoanApplication GetById(int id)
        {
            return Context.LoanApplications.SingleOrDefault(entity => entity.Application.Id == id);
        }

        public static void Delete(int id)
        {
            var item = GetById(id);
            var today = DateTime.Now;
            Delete(item, today);
        }

        public static void Delete(LoanApplication loanApplication, DateTime today)
        {
            List<ApplicationItem> applicationItems = loanApplication.Application.ApplicationItems.ToList();
            List<LoanApplicationStatu> loanApplicationStatuses = loanApplication.LoanApplicationStatus.ToList();
            List<LoanReAvailment> loanReAvailments = loanApplication.LoanReAvailments.ToList();
            List<LoanApplicationFee> loanApplicationFees = loanApplication.LoanApplicationFees.ToList();
            
            var chequesAssoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
            List<Payment> loanApplicationChequePayment = new List<Payment>();
            foreach (var item in chequesAssoc)
            {
                var cheques = Context.Cheques.SingleOrDefault(entity => entity.Id == item.ChequeId);
                var payment = Context.Payments.SingleOrDefault(entity => entity.Id == cheques.PaymentId);
                loanApplicationChequePayment.Add(payment);
            }

            foreach (var item in chequesAssoc.ToList())
            {
                Context.DeleteObject(item);
            }

            foreach (var item in applicationItems)
            {
                Context.ApplicationItems.DeleteObject(item);
            }

            foreach (var item in loanApplicationStatuses)
            {
                Context.LoanApplicationStatus.DeleteObject(item);
            }

            foreach (var item in loanReAvailments)
            {
                Context.LoanReAvailments.DeleteObject(item);
            }

            foreach (var item in loanApplicationFees)
            {
                Context.LoanApplicationFees.DeleteObject(item);
            }

            //-------------------------------------------------//

            List<SubmittedDocument> submittedDocuments = loanApplication.SubmittedDocuments.ToList();
            foreach (var item in submittedDocuments)
            {
                List<DocumentPage> documentPage = item.DocumentPages.ToList();
                foreach (var doc in documentPage)
                {
                    Context.DocumentPages.DeleteObject(doc);
                }

                List<SubmittedDocumentStatu> status = item.SubmittedDocumentStatus.ToList();
                foreach (var stat in status)
                {
                    Context.SubmittedDocumentStatus.DeleteObject(stat);
                }

                Context.SubmittedDocuments.DeleteObject(item);
            }

            //-------------------------------------------------//
            foreach (var loanApplicationRole in loanApplication.LoanApplicationRoles.ToList())
            {
                loanApplicationRole.PartyRole.EndDate = today;
                Context.LoanApplicationRoles.DeleteObject(loanApplicationRole);
            }

            foreach (var asset in loanApplication.Assets.ToList())
            {
                if (asset.AssetType == AssetType.LandType)
                {
                    foreach (var address in asset.Addresses.ToList())
                    {
                        Context.PostalAddresses.DeleteObject(address.PostalAddress);
                        Context.Addresses.DeleteObject(address);
                    }

                    Context.Lands.DeleteObject(asset.Land);
                }
                else if (asset.AssetType == AssetType.BankAccountType)
                {
                    Context.BankAccounts.DeleteObject(asset.BankAccount);
                }

                foreach (var assetRole in asset.AssetRoles.ToList())
                {
                    assetRole.PartyRole.EndDate = today;
                    Context.AssetRoles.DeleteObject(assetRole);
                }
                Context.Assets.DeleteObject(asset);
            }

            //-------------------------------------------------//
            
            foreach (var pay in loanApplicationChequePayment)
            {
                //TODO:: Changed to new payment
                var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == pay.Id);
                var cheque = Context.Cheques.SingleOrDefault(entity => entity.PaymentId == pay.Id);
                Context.Cheques.DeleteObject(cheque);
                Context.Receipts.DeleteObject(rpAssoc.Receipt);
                Context.Payments.DeleteObject(rpAssoc.Payment);
                Context.ReceiptPaymentAssocs.DeleteObject(rpAssoc);
            }

            var agreement = Context.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
            var loanAgreement = Context.LoanAgreements.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
            var schedule = Context.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
            
            Context.AmortizationSchedules.DeleteObject(schedule);
            Context.LoanAgreements.DeleteObject(loanAgreement);
            Context.Agreements.DeleteObject(agreement);

            Application application = loanApplication.Application;
            Context.LoanApplications.DeleteObject(loanApplication);
            Context.Applications.DeleteObject(application);
        }

        public static void DeleteCheques(int LoanApplicationId)
        {
            var loanChequeAssoc = Context.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == LoanApplicationId);
            foreach (var item in loanChequeAssoc)
            {
                //TODO:: Changed to new payment
                var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == item.ChequeId);
                var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == cheque.PaymentId);
                
                Context.Receipts.DeleteObject(rpAssoc.Receipt);
                Context.ChequeApplicationAssocs.DeleteObject(item);
                Context.Cheques.DeleteObject(cheque);
                Context.Payments.DeleteObject(rpAssoc.Payment);
                Context.ReceiptPaymentAssocs.DeleteObject(rpAssoc);
            }
        }

        public static List<AmortizationScheduleModel> CreateScheduleItems(LoanApplication loanApplication, AmortizationSchedule schedule)
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

            return items;
        }

        public static IQueryable<LoanApplication> GetAllLoanApplicationOf(Party party, RoleType role)
        {
            var applications = from lar in Context.LoanApplicationRoles.Where(entity => entity.PartyRole.PartyId == party.Id
               && entity.PartyRole.RoleTypeId == role.Id
               && entity.PartyRole.EndDate == null)
                              join la in Context.LoanApplications on lar.ApplicationId equals la.ApplicationId
                              select la;

            return applications;
        }
    }
}