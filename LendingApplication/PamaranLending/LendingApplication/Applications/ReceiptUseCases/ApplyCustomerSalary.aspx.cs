using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.ReceiptUseCases
{
    public partial class ApplyCustomerSalary : ActivityPageBase
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

        public string LogInId
        {
            get
            {
                if (ViewState["LogInId"] != null)
                    return ViewState["LogInId"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["LogInId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                this.LogInId = PartyRole.GetById(partyroleid).PartyId.ToString();
            }
        }

        protected void ApplyCheckAsPayment(object sender, DirectEventArgs e)
        {
            var receipt = Receipt.GetById(int.Parse(hdnReceiptId.Text));
            decimal ReceiptBalance = receipt.ReceiptBalance.Value;
            var partyRole = PartyRole.GetById(int.Parse(hdnPartyRoleId.Text));
            var selectedDate = dtGenerationDate.SelectedDate;
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(selectedDate);
            DateTime entryDate = DateTime.Now;
            DateTime transactionDate = wnDtTransactionDate.SelectedDate;
             int ProcessedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
             Payment Payment = Payment.GetReceiptPayment(receipt.Id);

            //Generate Interest
            GenerateInterest(GenerateBillFacade.ManualBillingSave);
            Context.SaveChanges();

            //Create Parent Payment
            Payment ParentPayment = Payment.CreatePayment(entryDate,
                                                       transactionDate,
                                                        ProcessedBy,
                                                        partyRole.Id,
                                                        ReceiptBalance,
                                                        PaymentType.LoanPayment.Id,
                                                        PaymentMethodType.CashType.Id,
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        null);

                Context.Payments.AddObject(ParentPayment);
                Context.SaveChanges();

            LoanPayment newLoanPayment = Payment.CreateLoanPayment(ParentPayment, partyRole);
            Context.LoanPayments.AddObject(newLoanPayment);

            //For Payment Breakdown
            Payment paymentBreakdown = new Payment();
            paymentBreakdown.Payment2 = ParentPayment;
            paymentBreakdown.ProcessedByPartyRoleId = ProcessedBy;
            paymentBreakdown.ProcessedToPartyRoleId = partyRole.Id;
            paymentBreakdown.PaymentType = PaymentType.LoanPayment;
            paymentBreakdown.PaymentMethodTypeId = Payment.PaymentMethodTypeId;
            paymentBreakdown.TransactionDate = transactionDate;
            paymentBreakdown.TotalAmount = ReceiptBalance;
            paymentBreakdown.EntryDate = entryDate;
            paymentBreakdown.SpecificPaymentType = SpecificPaymentType.LoanPaymentType;
            Context.Payments.AddObject(paymentBreakdown);
            Context.SaveChanges();

            ReceiptPaymentAssoc assoc = new ReceiptPaymentAssoc();
            assoc.Receipt = receipt;
            assoc.Payment = paymentBreakdown;
            Context.ReceiptPaymentAssocs.AddObject(assoc);
            Context.SaveChanges();


            //loanbalance,loanreleasedate, agreementid,
            var loanAccounts = RetrieveLoanAccountsOfCustomer(partyRole);

            foreach (var item in loanAccounts)
            {
                if (ReceiptBalance > 0)
                {
                    var loanAccount = LoanAccount.GetById(item.FinancialAccountId);
                    var finAcctTran = FinAcctTran.CreateFinAcctTran(item.FinancialAccountId, ParentPayment, transactionDate, entryDate, FinlAcctTransType.AccountPaymentType);
                    Context.FinAcctTrans.AddObject(finAcctTran);

                    ReceiptBalance = SaveInterestPayment(entryDate, ReceiptBalance, loanAccount, ParentPayment);

                    ReceiptBalance = SaveLoanPayment(entryDate, ReceiptBalance, loanAccount, ParentPayment);

                }
                else break;

            }
            Context.SaveChanges();


            if (ReceiptBalance == 0)
                receipt.SalaryReceipt.IsApplied = true;

            //UpdateReceiptBalance and Status
            receipt.ReceiptBalance = ReceiptBalance;
            var receiptStatus = ReceiptStatu.GetActive(receipt);
            if (receipt.ReceiptBalance == 0 && receiptStatus.ReceiptStatusType.Id != ReceiptStatusType.ClosedReceiptStatusType.Id)
            {
                ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.ClosedReceiptStatusType, string.Empty);
            }
            else if (receipt.ReceiptBalance > 0 && receiptStatus.ReceiptStatusType.Id != ReceiptStatusType.AppliedReceiptStatusType.Id)
            {
                ReceiptStatu.ChangeStatus(receipt, ReceiptStatusType.AppliedReceiptStatusType, string.Empty);
            }

            ControlNumberFacade.Create(FormType.PaymentFormType, ParentPayment);
            Context.SaveChanges();
            
        }


        private static decimal SaveLoanPayment(DateTime entryDate, decimal ReceiptBalance, LoanAccount loanAccount, Payment ParentPayment)
        {
            if (ReceiptBalance > 0)
            {
                decimal amountapplied = 0;
                if (loanAccount.LoanBalance <= ReceiptBalance)
                    amountapplied = loanAccount.LoanBalance;
                else amountapplied = ReceiptBalance;
                PaymentApplication loanpaymentApplication = new PaymentApplication();
                loanpaymentApplication.Payment = ParentPayment;
                loanpaymentApplication.FinancialAccountId = loanAccount.FinancialAccountId;
                loanpaymentApplication.AmountApplied = amountapplied;
                Context.PaymentApplications.AddObject(loanpaymentApplication);


                loanAccount.LoanBalance -= amountapplied;
                ReceiptBalance -= amountapplied;
                LoanAccount.UpdateLoanStatus(entryDate, loanAccount, loanAccount.LoanBalance);


            }
            return ReceiptBalance;
        }

        private static decimal SaveInterestPayment(DateTime entryDate, decimal ReceiptBalance, LoanAccount loanAccount, Payment ParentPayment)
        {
            if (ReceiptBalance > 0)
            {
                var receivables = from r in Context.Receivables
                                  join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                                  where r.FinancialAccountId == loanAccount.FinancialAccountId
                                  && rs.IsActive == true && (rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
                                  || rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id)
                                  orderby r.ValidityPeriod
                                  select r;
                foreach (var receivable in receivables)
                {
                    if (ReceiptBalance > 0)
                    {
                        decimal amountapplied = 0;
                        if (receivable.Balance <= ReceiptBalance)
                            amountapplied = (decimal)receivable.Balance;
                        else if (receivable.Balance > ReceiptBalance)
                            amountapplied = ReceiptBalance;

                        PaymentApplication paymentApplication = new PaymentApplication();
                        paymentApplication.Payment = ParentPayment;
                        paymentApplication.ReceivableId = receivable.Id;
                        paymentApplication.AmountApplied = amountapplied;
                        Context.PaymentApplications.AddObject(paymentApplication);
                        receivable.Balance -= amountapplied;

                        Receivable.InsertReceivableStatus(receivable.Id, receivable.Balance, entryDate);
                        ReceiptBalance -= amountapplied;

                    }

                }
            }
            return ReceiptBalance;
        }

        private decimal AmountToApply(decimal amount, decimal amount1)
        {
            if (amount >= amount1)
                return amount1;
            else return amount;

        }


        [DirectMethod]
        public void GenerateAdditionalInterest()
        {
            decimal interest = GenerateInterest(GenerateBillFacade.ManualBillingDisplay);
            wntxtAddIntererst.Text = interest.ToString();
        }

        private decimal GenerateInterest(string type)
        {
            var partyRole = PartyRole.GetById(int.Parse(hdnPartyRoleId.Text));
            var selectedDate = dtGenerationDate.SelectedDate;
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(selectedDate);
            decimal interest = 0;

            //loanbalance,loanreleasedate, agreementid,
            var loanAccounts = from l in RetrieveLoanAccountsOfCustomer(partyRole)
                               join a in Context.Agreements on l.FinancialAccount.AgreementId equals a.Id
                               join ai in Context.AgreementItems on l.FinancialAccount.AgreementId equals ai.AgreementId
                               where a.EndDate == null && ai.IsActive == true
                               select new
                               {
                                   loanaccount = l,
                                   agreementitem = ai
                               };
            foreach (var item in loanAccounts)
            {
                if (selectedDate > item.loanaccount.LoanReleaseDate)
                {
                    interest += GenerateBillFacade.GenerateAndSaveInterest(item.loanaccount, item.agreementitem, type, selectedDate, validDueDate, selectedDate);
                    interest += GenerateBillFacade.GenerateInterestForLastMonth(selectedDate, item.loanaccount, item.agreementitem, type);
                }
            }
            return interest;
        }
       
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            dataSource.UserId = int.Parse(LogInId);


            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }


        [DirectMethod]
        public int FillApplyAsPaymentWindow(int ReceiptId, int PartyRoleId)
        {
            decimal receivablesBalance = 0;
            decimal loanBalance = 0;
            hdnPartyRoleId.Value = PartyRoleId;
            wnDtTransactionDate.SelectedDate = DateTime.Today;
            var receipt = Receipt.GetById(ReceiptId);
            var receiptBalance = receipt.ReceiptBalance;
            wntxtReceiptAmount.Text = receiptBalance.Value.ToString("N");
            hdnReceiptId.Value = ReceiptId;
            var partyRole = PartyRole.GetById(PartyRoleId);
            var loanAccounts = RetrieveLoanAccountsOfCustomer(partyRole);
            if (loanAccounts.Count() != 0)
                loanBalance = loanAccounts.Sum(entity => entity.LoanBalance);


            var existingReceivables = from r in Context.Receivables
                                      join l in loanAccounts on r.FinancialAccountId equals l.FinancialAccountId
                                      join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                                      where rs.IsActive == true && (rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
                                      || rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id)
                                      orderby r.ValidityPeriod descending
                                      select r;


            wntxtLoanBalance.Text = loanBalance.ToString();

            if (existingReceivables.Count() != 0)
            {
                DateTime mindate = existingReceivables.First().ValidityPeriod.AddDays(1);
                dtGenerationDate.MinDate = mindate;
                wntxtExistInterestDate.Text = existingReceivables.First().ValidityPeriod.ToString();
                receivablesBalance = existingReceivables.Sum(e => e.Balance);
            }
            else
                wntxtExistInterestDate.Text = "N/A";
            wntxtExistInterest.Text = receivablesBalance.ToString();

            return 1;
        }

        private IQueryable<LoanAccount> RetrieveLoanAccountsOfCustomer(PartyRole partyRole)
        {

            var financialAccountRolesOfCustomer = from farc in Context.FinancialAccountRoles
                                                  join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                                                  where pr.PartyId == partyRole.PartyId
                                                  && pr.EndDate == null
                                                  && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                                                  select farc;

            var financialAccounts = from farc in financialAccountRolesOfCustomer
                                    join la in Context.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                    join las in Context.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                    orderby farc.FinancialAccount.LoanAccount.LoanReleaseDate
                                    where las.IsActive && farc.FinancialAccount.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id
                                        && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.PendingEndorsementType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                    select farc.FinancialAccount.LoanAccount;
            return financialAccounts;
        }


        private class ApplyCustomerSalaryModel
        {
            public int ReceiptId { get; set; }
            public int PartyRoleId { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public string PaymentMethodType { get; set; }
            public ApplyCustomerSalaryModel(Payment payment, Receipt receipt)
            {
                ReceiptId = receipt.Id;
                PartyRoleId = payment.ProcessedToPartyRoleId.Value;
                var partyRole = PartyRole.GetById(payment.ProcessedByPartyRoleId);
                Name = partyRole.Party.Name;
                Amount = receipt.ReceiptBalance.Value;
                PaymentMethodType = payment.PaymentMethodType.Name;


            }
        }

        public enum SearchBy
        {
            ID = 0,
            Name = 1,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<ApplyCustomerSalaryModel>
        {
            public string Name { get; set; }
            public string filterString2 { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public int UserId { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IEnumerable<ApplyCustomerSalaryModel> CreateQuery(FinancialEntities context)
            {

                var query = from r in context.SalaryReceipts
                            join rpa in context.ReceiptPaymentAssocs on r.ReceiptId equals rpa.ReceiptId
                            join payment in context.Payments on rpa.PaymentId equals payment.Id
                            join receipstatus in context.ReceiptStatus on r.ReceiptId equals receipstatus.ReceiptId
                            where receipstatus.IsActive == true &&
                                payment.PaymentTypeId == PaymentType.Receipt.Id && r.IsApplied == false
                            select new
                            {
                                r,
                                payment
                            };
                List<ApplyCustomerSalaryModel> result = new List<ApplyCustomerSalaryModel>();
                foreach (var item in query)
                {
                    result.Add(new ApplyCustomerSalaryModel(item.payment, item.r.Receipt));
                }

                return result;
            }

            public override List<ApplyCustomerSalaryModel> SelectAll(int start, int limit, Func<ApplyCustomerSalaryModel, string> orderBy)
            {
                List<ApplyCustomerSalaryModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return collection;
            }
        }
    }
}