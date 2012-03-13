using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class LoanPaymentForm : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string loanPaymentIdStr = Request.QueryString["loanPaymentId"];
                int loanPaymentId = int.Parse(loanPaymentIdStr);
                FillHeader();
                FillInfo(loanPaymentId);
            }
        }

        protected void FillHeader()
        {
            //HEADER LENDER INFORMATION
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
        }

        protected void FillInfo(int loanPaymentId)
        {
            Payment loanPayment = Payment.GetById(loanPaymentId);
            PrintLoanPayment printForm = PrintLoanPayment.Create(loanPayment);

            lblLabelStationNumber.Text = printForm.StationNumber;
            lblDate.Text = printForm.TransactionDate.ToString("MMM dd, yyy");
            lblReceivedFrom.Text = printForm.ReceiveFrom;
            lblTeller.Text = printForm.Teller;
            lblLoanAmountWords.Text = ConvertNumbers.EnglishFromNumber((double)printForm.AmountTendered) + " Pesos Only";
            lblLoanAmount.Text = printForm.AmountTendered.ToString("N");
            lblPrincipal.Text = printForm.TotalPrincipalDue.ToString("N");
            lblInterest.Text = printForm.TotalInterestDue.ToString("N");
            //lblOthers.Text = printForm.totalPastDue.ToString("N");
            lblTotal.Text = printForm.Total.ToString("N");
            lblChange.Text = printForm.Change.ToString("N");
            lblCustomerSignature.Text = printForm.ReceiveFrom.ToUpper();

            lblOwnerBalance.Text = printForm.CustomerLoanBalance.ToString("N");
            lblCoOwnerBalance.Text = printForm.CustomerAsCoOwnerLoanBalance.ToString("N");
            lblLoanBalance.Text = printForm.TotalLoanBalance.ToString("N");

            IEnumerable<PaymentModel> payments = printForm.CashReceiptsFromPreviousTransaction.Concat(
                printForm.Cheques).Concat(printForm.CashPayments).Concat(printForm.ATMPayments);

            this.lblControlNumber.Text = string.Format("{0:00000}", printForm.ControlNumber);

            GridPanelPaymentsStore.DataSource = payments;
            GridPanelPaymentsStore.DataBind();
        }
    }

    public class PaymentModel : BusinessObjectModel
    {
        public string PaymentMethod { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string CheckNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string _TransactionDate { get { return TransactionDate.ToString("MM/dd/yyyy"); } }
        public decimal AmountTendered { get; set; }
        public decimal AmountApplied { get; set; }
    }

    public class PrintLoanPayment
    {
        public string ReceiveFrom { get; set; }
        public string Teller { get; set; }
        public string StationNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalPrincipalDue { get; set; }
        public decimal TotalInterestDue { get; set; }
        //public decimal TotalPastDue { get; set; }

        //= TotalPrincipalDue + TotalInterestDue + TotalPastDue;
        public decimal Total { get; set; }
        public decimal CustomerLoanBalance { get; set; }
        public decimal CustomerAsCoOwnerLoanBalance { get; set; }
        public decimal TotalLoanBalance { get; set; }

        public List<PaymentModel> CashReceiptsFromPreviousTransaction { get; set; }
        public List<PaymentModel> Cheques { get; set; }
        public List<PaymentModel> CashPayments { get; set; }
        public List<PaymentModel> ATMPayments { get; set; }
        public decimal AmountTendered { get; set; }
        //= AmountTendered - Total;
        public decimal Change { get; set; }
        public int ControlNumber { get; set; }

        public void CalculateAmountTendered()
        {
            decimal totalFromCheque = this.Cheques.Sum(x => x.AmountTendered);
            decimal totalFromPreviousReceipts = this.CashReceiptsFromPreviousTransaction.Sum(x => x.AmountTendered);
            decimal totalFromCash = this.CashPayments.Sum(x => x.AmountTendered);
            decimal totalATM = this.ATMPayments.Sum(x => x.AmountTendered);

            this.AmountTendered = totalFromCheque + totalFromPreviousReceipts + totalFromCash +totalATM;
        }

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

        public static PrintLoanPayment Create(Payment loanPayment)
        {
            if (loanPayment.LoanPayment == null)
                throw new ArgumentException("Please supply the parent of the payments.");

            var customer = PartyRole.GetById(loanPayment.ProcessedToPartyRoleId.Value);
            var teller = PartyRole.GetById(loanPayment.ProcessedByPartyRoleId);
            var district = Context.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == customer.Id);
            decimal ownerOutstandingLoan = loanPayment.LoanPayment.OwnerOutstandingLoan + loanPayment.LoanPayment.OwnerOutstandingInterest;
            decimal coOwnerOutsandingLoan = loanPayment.LoanPayment.CoOwnerOutstandingInterest + loanPayment.LoanPayment.CoOwnerOutstandingLoan;
            PrintLoanPayment model = new PrintLoanPayment();
            model.ReceiveFrom = customer.Party.Name;
            model.Teller = teller.Party.Name;
            model.StationNumber = district != null ? district.ClassificationType.StationNumber : "";
            model.TransactionDate = loanPayment.TransactionDate;
            model.Total = loanPayment.TotalAmount;
            model.TotalPrincipalDue = GetTotalPrincipalDue(loanPayment);
            model.TotalInterestDue = GetTotalInterestDue(loanPayment);
            model.Total = model.TotalPrincipalDue + model.TotalInterestDue;
            model.CustomerLoanBalance = ownerOutstandingLoan;
            model.CustomerAsCoOwnerLoanBalance = coOwnerOutsandingLoan;
            model.TotalLoanBalance = model.CustomerLoanBalance + model.CustomerAsCoOwnerLoanBalance;

            model.Cheques = GetListOfCheques(loanPayment);
            model.ATMPayments = GetATMPayments(loanPayment);
            FillListOfCashReceiptsUsed(loanPayment, model);
            model.CalculateAmountTendered();

            model.Change = model.AmountTendered - model.Total;
            var controlNumber = ControlNumberFacade.GetByPaymentId(loanPayment.Id, FormType.PaymentFormType);
            model.ControlNumber = controlNumber.LastControlNumber;
            return model;
        }

        private static decimal GetTotalPrincipalDue(Payment loanPayment)
        {
            var principalPayments = loanPayment.PaymentApplications.Where(x => x.FinancialAccountId.HasValue);
            return principalPayments.Sum(x => x.AmountApplied);
        }

        private static decimal GetTotalInterestDue(Payment loanPayment)
        {
            var interestPayments = loanPayment.PaymentApplications.Where(x => x.ReceivableId.HasValue);
            return interestPayments.Sum(x => x.AmountApplied);
        }

        private static decimal GetBorrowerLoanBalance(PartyRole customer, Payment loanPayment)
        {
            decimal totalPayments = 0;
            decimal totalDisbursements = 0;
            var loanAccounts = from farc in Context.FinancialAccountRoles
                               join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                               join ls in Context.LoanAccountStatus on farc.FinancialAccountId equals ls.FinancialAccountId
                               join vcr in Context.LoanDisbursementVcrs on farc.FinancialAccount.AgreementId equals vcr.AgreementId
                                                  where pr.PartyId == customer.PartyId
                                                  && pr.EndDate == null
                                                  && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                                                  && ls.IsActive == true && ls.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                                  && farc.FinancialAccount.LoanAccount.LoanReleaseDate < loanPayment.EntryDate
                               select new { loanAccount = farc.FinancialAccount.LoanAccount, voucher = vcr };
            var paymentsBefore = from la in loanAccounts
                           join pa in Context.PaymentApplications on la.loanAccount.FinancialAccountId equals pa.FinancialAccountId
                           join p in Context.Payments on pa.PaymentId equals p.Id
                           where p.Id > loanPayment.Id
                           select pa;
            if (paymentsBefore.Count() != 0)
                totalPayments = paymentsBefore.Sum(e => e.AmountApplied);

            var disbursements = from la in loanAccounts
                                join pa in Context.PaymentApplications on la.voucher.Id equals pa.LoanDisbursementVoucherId
                                join p in Context.Payments on pa.PaymentId equals p.Id
                                where p.Id > loanPayment.Id
                                select pa;

            if (disbursements.Count() != 0)
                totalDisbursements = disbursements.Sum(e => e.AmountApplied);

            decimal balance = 0;
            if (loanAccounts.Count() > 0)
                balance = loanAccounts.Sum(entity => entity.loanAccount.LoanBalance);
            balance += totalPayments;
            balance -= totalDisbursements;
            return balance;
            
        }

        private static decimal GetCoBorrowerLoanBalance(PartyRole customer,Payment loanPayment)
        {
            decimal totalPayments = 0;
            decimal totalDisbursements = 0;
            var loanAccounts = from farc in Context.FinancialAccountRoles
                               join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                               join ls in Context.LoanAccountStatus on farc.FinancialAccountId equals ls.FinancialAccountId
                               join vcr in Context.LoanDisbursementVcrs on farc.FinancialAccount.AgreementId equals vcr.AgreementId
                               where pr.PartyId == customer.PartyId
                               && pr.EndDate == null
                               && (pr.RoleTypeId == RoleType.CoOwnerFinancialType.Id
                               || pr.RoleTypeId == RoleType.GuarantorFinancialType.Id)
                               && ls.IsActive == true && ls.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                   && farc.FinancialAccount.LoanAccount.LoanReleaseDate < loanPayment.EntryDate
                               select new { loanAccount = farc.FinancialAccount.LoanAccount, voucher = vcr};
            var payments = from la in loanAccounts
                           join pa in Context.PaymentApplications on la.loanAccount.FinancialAccountId equals pa.FinancialAccountId
                           join p in Context.Payments on pa.PaymentId equals p.Id
                           where p.Id > loanPayment.Id
                           select pa;
            var disbursements = from la in loanAccounts
                                join pa in Context.PaymentApplications on la.voucher.Id equals pa.LoanDisbursementVoucherId
                                join p in Context.Payments on pa.PaymentId equals p.Id
                                where p.Id > loanPayment.Id
                                select pa;

            if (disbursements.Count() != 0)
                totalDisbursements = disbursements.Sum(e => e.AmountApplied);

            if (payments.Count() != 0)
                totalPayments = payments.Sum(e => e.AmountApplied);

            decimal balance = 0;
            if (loanAccounts.Count() > 0)
                balance = loanAccounts.Sum(entity => entity.loanAccount.LoanBalance);
            balance += totalPayments;
            balance -= totalDisbursements;
            return balance;
        }

        private static List<PaymentModel> GetListOfCheques(Payment loanPayment)
        {
            var chequeReceiptPayments = from p in Context.Payments
                                 join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                                 where p.ParentPaymentId == loanPayment.Id
                                 && (p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id 
                                 || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                 && p.PaymentTypeId == PaymentType.LoanPayment.Id
                                 select rpa;

            var customerChequePayments = from crp in chequeReceiptPayments
                                         join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                         join p in Context.Payments on rpa.PaymentId equals p.Id
                                         join c in Context.Cheques on p.Id equals c.PaymentId
                                         where p.PaymentTypeId == PaymentType.Receipt.Id
                                         select new {ActualPayment = crp.Payment, ReceivePayment = p, Cheque = c};

            var models = new List<PaymentModel>();
            foreach (var chequePayment in customerChequePayments)
            {
                var amountTendered = chequeReceiptPayments.FirstOrDefault(entity => entity.PaymentId == chequePayment.ActualPayment.Id);
                var bankPartyRole = PartyRole.GetById(chequePayment.Cheque.BankPartyRoleId);
                Bank bank = Bank.GetById(bankPartyRole.Id);
                PaymentModel model = new PaymentModel();
                model.PaymentMethod = "Cheque";
                model.BankName = bankPartyRole.Party.Name;
                model.BankBranch = bank.Branch;
                model.CheckNumber = chequePayment.ReceivePayment.PaymentReferenceNumber;
                model.TransactionDate = chequePayment.Cheque.CheckDate;
                model.AmountTendered = amountTendered.Amount;
                model.AmountApplied = chequePayment.ActualPayment.TotalAmount;
                models.Add(model);
            }

            return models;
        }
        private static List<PaymentModel> GetATMPayments(Payment loanPayment)
        {
            var atmReceiptPayments = from p in Context.Payments
                                     join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                                     where p.ParentPaymentId == loanPayment.Id
                                     && (p.PaymentMethodTypeId == PaymentMethodType.ATMType.Id)
                                     && p.PaymentTypeId == PaymentType.LoanPayment.Id
                                     select rpa;

            var customerAtmPayments = from crp in atmReceiptPayments
                                      join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                      join p in Context.Payments on rpa.PaymentId equals p.Id
                                      where p.PaymentTypeId == PaymentType.Receipt.Id
                                      select new { ActualPayment = crp.Payment, ReceivePayment = p};

            var models = new List<PaymentModel>();
            foreach (var atmPayment in customerAtmPayments)
            {
                var amountTendered = atmReceiptPayments.FirstOrDefault(entity => entity.PaymentId == atmPayment.ActualPayment.Id);
                PaymentModel model = new PaymentModel();
                model.PaymentMethod = atmPayment.ActualPayment.PaymentMethodType.Name;
                model.BankName = "NA";
                model.BankBranch = "NA";
                model.CheckNumber = atmPayment.ReceivePayment.PaymentReferenceNumber;
                model.TransactionDate = atmPayment.ReceivePayment.TransactionDate;
                model.AmountTendered = amountTendered.Amount;
                model.AmountApplied = atmPayment.ActualPayment.TotalAmount;
                models.Add(model);
            }

            return models;
        }

        private static void FillListOfCashReceiptsUsed(Payment loanPayment, PrintLoanPayment loanPaymentModel)
        {
            var cashPayments = from p in Context.Payments
                               join rpa in Context.ReceiptPaymentAssocs on p.Id equals rpa.PaymentId
                               where p.ParentPaymentId == loanPayment.Id
                               && p.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                               && p.PaymentTypeId == PaymentType.LoanPayment.Id
                               select rpa;

            var customerCashPayments = from crp in cashPayments
                                       join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                       join p in Context.Payments on rpa.PaymentId equals p.Id
                                       where p.PaymentTypeId == PaymentType.Receipt.Id
                                       && p.TransactionDate < loanPayment.TransactionDate
                                       select new { ActualPayment = crp.Payment, ReceivePayment = p};

            var customerCashPaymentsToday = from crp in cashPayments
                                            join rpa in Context.ReceiptPaymentAssocs on crp.ReceiptId equals rpa.ReceiptId
                                            join p in Context.Payments on rpa.PaymentId equals p.Id
                                       where p.PaymentTypeId == PaymentType.Receipt.Id
                                       && p.TransactionDate == loanPayment.TransactionDate
                                            select new { ActualPayment = crp.Payment, ReceivePayment = p };

            var modelsFromPreviousReceipts = new List<PaymentModel>();
            foreach (var cashPayment in customerCashPayments)
            {
                var amountTendered = cashPayments.FirstOrDefault(entity => entity.PaymentId == cashPayment.ActualPayment.Id);

                PaymentModel paymentCash = new PaymentModel();
                paymentCash.PaymentMethod = "Cash Previous Receipt";
                paymentCash.BankName = "NA";
                paymentCash.BankBranch = "NA";
                paymentCash.CheckNumber = "NA";
                paymentCash.TransactionDate = cashPayment.ReceivePayment.TransactionDate;
                paymentCash.AmountTendered = amountTendered.Amount;
                paymentCash.AmountApplied = cashPayment.ActualPayment.TotalAmount;
                modelsFromPreviousReceipts.Add(paymentCash);
            }

            loanPaymentModel.CashReceiptsFromPreviousTransaction = modelsFromPreviousReceipts;

            var models = new List<PaymentModel>();
            foreach (var cashPayment in customerCashPaymentsToday)
            {
                var amountTendered = cashPayments.FirstOrDefault(entity => entity.PaymentId == cashPayment.ActualPayment.Id);
                PaymentModel paymentCash = new PaymentModel();
                paymentCash.PaymentMethod = "Cash";
                paymentCash.BankName = "NA";
                paymentCash.BankBranch = "NA";
                paymentCash.CheckNumber = "NA";
                paymentCash.TransactionDate = cashPayment.ReceivePayment.TransactionDate;
                paymentCash.AmountTendered = amountTendered.Amount;
                paymentCash.AmountApplied = cashPayment.ActualPayment.TotalAmount;
                models.Add(paymentCash);
            }

            loanPaymentModel.CashPayments = models;
        }
    }
}