using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using Ext.Net;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintTransactionSlip : ActivityPageBase
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int disbursementId = int.Parse(Request.QueryString["id"]);
                int agreementId = int.Parse(Request.QueryString["agreementid"]);
                FillDisbursementDetails(disbursementId, agreementId);
                FillHeader(disbursementId);
                FillFeesAndPreviousBalance(disbursementId);
                FillFooter(disbursementId);
                RetrieveOutstandingLoans(disbursementId);
            }
           
        }
        protected void FillFeesAndPreviousBalance(int id)
        {
            var ParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == id && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);
            decimal totaldeductions = 0;
            decimal loanbalance = 0;
            decimal fees = 0;
            decimal interest = 0;

            if (ParentPayment != null)
            {
                var loanParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.LoanPaymentType.Id);
                var feeParentPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.FeePaymentType.Id);
                var interestPayment = ObjectContext.Payments.FirstOrDefault(entity => entity.ParentPaymentId == ParentPayment.Id && entity.SpecificPaymentType.Id == SpecificPaymentType.InterestPaymentType.Id);

                //interestpayment
                if (interestPayment != null)
                    interest = interestPayment.TotalAmount;

                //for loan payments
                if (loanParentPayment != null)
                {
                    loanbalance = loanParentPayment.TotalAmount;
         
                }

                    if (feeParentPayment != null)
                    {
                        var feePayments = ObjectContext.FeePayments.Where(entity => entity.PaymentId == feeParentPayment.Id);
                        fees = feePayments.Sum(entity => entity.FeeAmount);
                    }
            }
        
            totaldeductions = loanbalance + fees+interest;
            /*Total Deductions*/
            lblDeductions.Text = totaldeductions.ToString("N");

            decimal proceeds = decimal.Parse(lblAmountDisbursed.Text) - totaldeductions;
            txtNetProceeds.Text = proceeds.ToString("N");
         
           
        }
        protected void FillDisbursementDetails(int id, int agreementId)
        {
            var payment = Payment.GetById(id);
            int customerid = (int)payment.ProcessedToPartyRoleId;
            var customer = PartyRole.GetById(customerid);
            string date = payment.TransactionDate.ToString("MMM dd,yyyy");
            string stationnum = string.Empty;
            string receivedfrom = Person.GetPersonFullName(payment.ProcessedToPartyRoleId.Value);
            string sumamount = payment.TotalAmount.ToString("N");
            string cash = payment.TotalAmount.ToString("N");
            string amountdisbursed = payment.TotalAmount.ToString("N");
            string checknum = string.Empty;
            string amount = string.Empty;
            string interestrate = string.Empty;
            string term = "0";
            string receivedby = Person.GetPersonFullName(payment.ProcessedToPartyRoleId.Value);
            string sumamountwords = ConvertNumbers.EnglishFromNumber((double)payment.TotalAmount) + "Pesos Only";

            /*----------------QUERIES---------------------*/
            if (customer.Customer != null)
            {
                var customerClassification = customer.Customer.CurrentClassification;
                stationnum = customerClassification.ClassificationType.StationNumber;
                 
            }

            if (payment.PaymentMethodType.Id == PaymentMethodType.PayCheckType.Id || payment.PaymentMethodType.Id == PaymentMethodType.PersonalCheckType.Id)
                checknum = payment.PaymentReferenceNumber;

            var amortization = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreementId);
            var amortizationSched = ObjectContext.AmortizationScheduleItems.Where(entity => entity.AmortizationScheduleId == amortization.Id);
            var agreementitem = ObjectContext.AgreementItems.FirstOrDefault(entity => entity.AgreementId == agreementId);
            if (amortizationSched.Count() > 0)
                term = amortizationSched.Count().ToString();

            interestrate = agreementitem.InterestRate.ToString();
            var agreement = ObjectContext.Agreements.FirstOrDefault(entity => entity.Id == agreementId);

            amount = payment.Disbursement.LoanDisbursement.LoanAmount.ToString("N");
            var loanbalance = (payment.Disbursement.LoanDisbursement.LoanBalance + payment.Disbursement.LoanDisbursement.InterestBalance).ToString("N");
            
            var formDetail = FormDetail.GetByPaymentId(payment.Id);
            imgSignature.ImageUrl = formDetail.Signature;

            lblTransacDate.Text = date;
            lblStationNum.Text = stationnum;
            lblReceivedFrom.Text = receivedfrom;
            lblSumAmountWords.Text = sumamountwords;
            lblSumAmount.Text = sumamount;
            lblAmount.Text = amount;
            lblLoanBalance.Text = loanbalance;
            lblInterestRate.Text = interestrate;
            lblTerm.Text = term;
            lblAmountDisbursed.Text = amountdisbursed;
            lblReceivedBy.Text = receivedby.ToUpper();
            lblReleasedBy.Text = receivedfrom;
         

            //Fill sa mga endorsedby, approved by
            var processedby = ObjectContext.LoanApplicationRoles.FirstOrDefault(entity => entity.ApplicationId == agreement.ApplicationId && entity.PartyRole.RoleTypeId == RoleType.ProcessedByApplicationType.Id);
            if (processedby != null)
            {
                var party = processedby.PartyRole.Party;
                /*Endorsed By*/
                lblEndorsedBy.Text = Person.GetPersonFullName(party);
            }
            var approvedby = ObjectContext.LoanApplicationRoles.FirstOrDefault(entity => entity.ApplicationId == agreement.ApplicationId && entity.PartyRole.RoleTypeId == RoleType.ApprovedByAgreementType.Id);
            if (approvedby != null)
            {
                var party = approvedby.PartyRole.Party;
                lblApprovedBy.Text = Person.GetPersonFullName(party);
            }
         
        }
        protected void FillHeader(int disbursementid)
        {
           
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;

            var loandisbursement = ObjectContext.LoanDisbursements.FirstOrDefault(e => e.PaymentId == disbursementid);
         
            if (loandisbursement.LoanDisbursementType.Id == LoanDisbursementType.Additional.Id)
            {
                //Additional Loan after Restructuring: Use of restructured
                lblSubAvailment.Text = "X";
                lblAvailment.Text = "_";
                lblFormType.Text = "Transaction Slip - Additional";
            }
            else if (loandisbursement.LoanDisbursementType.Id == LoanDisbursementType.FirstAvailment.Id)
            {
                lblSubAvailment.Text = "_";
                lblAvailment.Text = "X";
                lblFormType.Text = "Transaction Slip - Collection";
            }
            else if (loandisbursement.LoanDisbursementType.Id == LoanDisbursementType.ACCheque.Id
                || loandisbursement.LoanDisbursementType.Id == LoanDisbursementType.ACATM.Id)
            {
                lblSubAvailment.Text = "X";
                lblAvailment.Text = "_";
                lblFormType.Text = "Transaction Slip - Advance Change";
            }
            else if (loandisbursement.LoanDisbursementType.Id == LoanDisbursementType.Change.Id)
            {
                lblSubAvailment.Text = "_";
                lblAvailment.Text = "X";
                lblFormType.Text = "Transaction Slip - Change";
            }

        }
        protected void FillFooter(int id)
        {
       
            var controlNum = ControlNumberFacade.GetByPaymentId(id,FormType.TransactionSlipType);
            if (controlNum != null)
            {
                string controlnum = string.Format("{0:000000}", controlNum.LastControlNumber);
                lblControlNumber.Text = controlnum;
            }
        }
        
        protected void RetrieveOutstandingLoans(int disbursementid)
        {
            decimal outstandingInterest = 0;
            decimal outstandingPrincipal = 0;
            decimal outstandingTotalMinusPayments = 0;
            decimal outstandingTotal = 0;
            decimal payments= 0;
            var payment = Payment.GetById(disbursementid);
            var releaseStatement = ObjectContext.ReleaseStatements.Where(entity => entity.PaymentId == disbursementid);
            if(releaseStatement.Count() > 0)
                outstandingPrincipal = releaseStatement.Sum(entity => entity.TotalLoanBalance);

           outstandingTotalMinusPayments = (outstandingInterest + outstandingPrincipal);
           outstandingTotal = payments + outstandingTotalMinusPayments;
           lblBalance.Text = outstandingTotal.ToString("N");
           lblPayment.Text = payments.ToString("N");
           txtNewBalance.Text = (payment.TotalAmount + outstandingTotalMinusPayments).ToString("N");
           decimal zero = 0;
           txtAvailment.Text = zero.ToString("N");
        }

        [DirectMethod]
        public void CalculateNewOutstanding()
        {
            decimal availment = 0;
            if(string.IsNullOrEmpty(txtAvailment.Text)==false)
             availment = decimal.Parse(txtAvailment.Text);
            txtAvailment.Text = availment.ToString("N");
            decimal newbalance = decimal.Parse(txtNewBalance.Text);
            newbalance += availment;

            txtNewBalance.Text = newbalance.ToString("N");
        }

        [DirectMethod]
        public void CalculateNetProceed()
        {
            decimal deductions = 0;
            deductions = decimal.Parse(lblDeductions.Text);
            decimal amount = decimal.Parse(lblAmountDisbursed.Text);
            amount -= deductions;
            txtNetProceeds.Text = amount.ToString("N");
         
        }

    }
}