using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintReleaseStatement : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                allowed.Add("Cashier");
                return allowed;
            }
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

        private class FeesModel
        {
            public string Fees { get; set; }
            public decimal Amount { get; set; }
        }

        private class PendingLoansModel
        {
            public string ProductName { get; set; }
            public decimal Amount { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request.QueryString["id"]);
            int oldestid = int.Parse(Request.QueryString["oldestid"]);
            hiddenPaymentId.Value = id;
            hdnAgreementId.Value = int.Parse(Request.QueryString["agreementid"]);
            FillHeader();
            FillLoanReleaseStatement(id, oldestid);
        }

        protected void FillHeader()
        {
            //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                Organization organization = party.Organization;
                lblLenderNameHeader.Text = organization.OrganizationName;

                var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
                FillPostalAddress(postalAddress);

                lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
                lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
                lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
                lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        public void FillPostalAddress(PostalAddress postalAddress)
        {
            lblStreetAddress.Text = postalAddress.StreetAddress;
            lblBarangay.Text = postalAddress.Barangay;
            lblCity.Text = postalAddress.City;
            lblMunicipality.Text = postalAddress.Municipality;
            lblProvince.Text = postalAddress.Province;
            lblCountry.Text = postalAddress.Country.Name;
            lblPostalCode.Text = postalAddress.PostalCode;
        }

        protected void FillLoanReleaseStatement(int id, int oldestDisId)
        {
            decimal loanbalance = 0;

            var formDetail = FormDetail.GetByPaymentId(id);
            imgSignature.ImageUrl = formDetail.Signature;

            var query = (from d in ObjectContext.DisbursementViewLists
                        join p in ObjectContext.Payments on d.DisbursementId equals p.Id
                        where d.DisbursementId == id
                        select new { customerId = p.ProcessedToPartyRoleId, customer = d.DisbursedTo, paymentAmount = p.TotalAmount, disbursedate = p.TransactionDate }).FirstOrDefault();
            int customerId = (int)query.customerId;
            decimal deductions = 0M;
            var agreementId = int.Parse(hdnAgreementId.Value.ToString());
            var loanAccount = ObjectContext.FinancialAccounts.FirstOrDefault(entity => entity.AgreementId == agreementId).LoanAccount;


            lblName.Text = query.customer;
            lblBorrowerName.Text = query.customer.ToUpper();

            var customer = PartyRole.GetById(customerId);
            if (customer.Customer != null)
            {
                var customerClassification = customer.Customer.CurrentClassification;
                lblStationNo.Text = customerClassification.ClassificationType.StationNumber;
            }
            FillPendingLoans(id,loanAccount.FinancialAccountId);

            //var oldestDisbursementId = 

            var ParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == oldestDisId && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);
            var totalFees = 0M;
            DateTime paymentTransitionDateTime = DateTime.Now;

            if (ParentPayment != null)
            {
                var loanParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.LoanPaymentType.Id);
                var feeParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);

                //for loan payments
                if (loanParentPayment != null)
                {
                        loanbalance = loanParentPayment.TotalAmount;
                        lblTotalPreviousLoan.Text = loanbalance.ToString("N");
                        deductions += loanbalance;

                    
                }
                
                //for fee payments
                if (feeParentPayment != null)
                {
             
                    var feePayments = ObjectContext.FeePayments.Where(entity => entity.PaymentId == feeParentPayment.Id);
                    List<FeesModel> feeModel = new List<FeesModel>();
                    if (feePayments.Count() != 0)
                    {
                        foreach (var items in feePayments)
                        {
                            FeesModel fee = new FeesModel();
                            fee.Fees = items.Particular;
                            fee.Amount = items.FeeAmount;
                            feeModel.Add(fee);
                        }
                    }
                    totalFees = feeModel.Sum(entity => entity.Amount);
                    lblTotalFees.Text = totalFees.ToString("N");
                    deductions += totalFees;
                    grdFees.DataSource = feeModel.ToList();
                    grdFees.DataBind();
                }

            }

            //var paymentDate = (from ldv in ObjectContext.LoanDisbursementVcrs.Where(entity => entity.AgreementId == agreementId)
            //                   join lpa in ObjectContext.PaymentApplications.OrderBy(entity => entity.PaymentId) on ldv.Id equals lpa.LoanDisbursementVoucherId
            //                   join p in ObjectContext.Payments on lpa.PaymentId equals p.Id
            //                   where p.ParentPaymentId == null && lpa.AmountApplied == query.paymentAmount
            //                   select new { 
            //                       Date = p.EntryDate, 
            //                       LDVId = ldv.Id 
            //                   }).OrderByDescending(entity => entity.Date).FirstOrDefault();

            //var voucherStatus = ObjectContext.DisbursementVcrStatus.Where(entity => (entity.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PendingType.Id
            //                        || entity.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id) 
            //                        && entity.TransitionDateTime < paymentDate.Date 
            //                        && entity.LoanDisbursementVoucherId == paymentDate.LDVId).OrderByDescending(entity => entity.TransitionDateTime).FirstOrDefault();

            //if(voucherStatus!=null)paymentTransitionDateTime = voucherStatus.TransitionDateTime;
            //var agreement = ObjectContext.Agreements.FirstOrDefault(entity => entity.Id == agreementId);
            //var amortization = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreementId && entity.DateGenerated == paymentTransitionDateTime);
            //if(amortization == null)
            //    amortization = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreementId && entity.DateGenerated < paymentTransitionDateTime);

            
            var ammortization = ObjectContext.AmortizationSchedules.Where(entity => entity.AgreementId == agreementId).OrderBy(entity => entity.DateGenerated).FirstOrDefault();
            if (ammortization != null)
            {
                var principalLoan = loanAccount.LoanAmount;
                lblNetAmountReceived.Text = (principalLoan - deductions).ToString("N");
                lblTotalDeductions.Text = deductions.ToString("N");
                lblGranted.Text = query.disbursedate.ToString("MMMM dd, yyyy");
                lblPrincipalAmount.Text = principalLoan.ToString("N");
                lblToStartOn.Text = ammortization.PaymentStartDate.ToString("MMMM dd, yyyy");
               
            }
          

        }

        protected void FillPendingLoans(int id, int loanId)
        {
            var releaseStatement = ObjectContext.ReleaseStatements.Where(entity => entity.PaymentId == id && entity.FinancialAccountId != loanId);
            if (releaseStatement.Count() != 0)
            {
                List<PendingLoansModel> pendingLoanModel = new List<PendingLoansModel>();

                foreach (var statement in releaseStatement)
                {
                    PendingLoansModel pendingLoan = new PendingLoansModel();
                    pendingLoan.Amount = statement.TotalLoanBalance;
                    var productName = from fp in ObjectContext.FinancialProducts
                                      join fap in ObjectContext.FinancialAccountProducts.Where(entity => entity.EndDate == null)
                                                    on fp.Id equals fap.FinancialProductId
                                      where fap.FinancialAccountId == statement.FinancialAccountId
                                      select new { name = fp.Name };

                    pendingLoan.ProductName = productName.FirstOrDefault().name;
                    pendingLoanModel.Add(pendingLoan);
                }

                grdPendingLoans.DataSource = pendingLoanModel;
                grdPendingLoans.DataBind();

                var totalAmountPendingLoans = pendingLoanModel.Sum(entity => entity.Amount);
                lblTotalAmountPending.Text = totalAmountPendingLoans.ToString();
            }
        }
    }
}