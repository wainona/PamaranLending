using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class LoanAccount
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

        public LoanAccountStatu CurrentStatus
        {
            get
            {
                return this.LoanAccountStatus.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public InterestItem CurrentInterestItem
        {

            get
            {
                return this.InterestItems.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public class LoanHistoryModel
        {
            public DateTime _Date { get; set; }
            public string Date
            {
                get
                {
                    return this._Date.ToString("MMM dd, yyyy");
                }
            }
            public decimal? Disbursement { get; set; }
            public decimal? Interest { get; set; }
            public decimal? Payment { get; set; }
            public decimal? WaiveOrRebate { get; set; }
            public int LoanId { get; set; }
            public int LoanDisType { get; set; }
            public string Remarks
            {
                get
                {
                    if (LoanId != -1)
                    {
                        var parentLoanId = ObjectContext.FinancialAccounts.FirstOrDefault(e => e.Id == LoanId).ParentFinancialAccountId;
                        var loanType = from lmp in ObjectContext.LoanModificationPrevItems
                                                        join lm in ObjectContext.LoanModifications on lmp.LoanModificationId equals lm.Id
                                                        join lmt in ObjectContext.LoanModificationTypes on lm.LoanModificationTypeId equals lmt.Id
                                                        where lmp.OldFinancialAccountId == parentLoanId
                                                        select lmt.Name;
                        if (loanType.Count() != 0 )
                            return "Restructured (" + loanType.FirstOrDefault() + ") to Loan Account: " + LoanId;
                        else return "Restructured to Loan Account: " + LoanId;
                    }
                    else if (LoanDisType != -1)
                    {
                        return ObjectContext.LoanDisbursementTypes.FirstOrDefault(e => e.Id == LoanDisType).Name;
                    }
                    else
                        return string.Empty;
                }
            }

        }
        public static IQueryable<LoanHistoryModel> CreateQueryLoanHistory(int selectedFinancialAccountID)
        {

            var agreementItem = from l in ObjectContext.LoanAccounts
                                join f in ObjectContext.FinancialAccounts on l.FinancialAccountId equals f.Id
                                join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                                join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                                where l.FinancialAccountId == selectedFinancialAccountID && a.EndDate == null && ai.IsActive == true
                                select ai;

            var loanDisbursemntVoucher = ObjectContext.LoanDisbursementVcrs.FirstOrDefault(entity => entity.AgreementId == agreementItem.FirstOrDefault().AgreementId);

            var disbursements = from pa in ObjectContext.PaymentApplications
                                join d in ObjectContext.Disbursements on pa.PaymentId equals d.PaymentId
                                join ld in ObjectContext.LoanDisbursements on d.PaymentId equals ld.PaymentId
                                join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                join par in ObjectContext.Parties on p.PartyRole.PartyId equals par.Id
                                where pa.LoanDisbursementVoucherId == loanDisbursemntVoucher.Id
                                orderby p.TransactionDate ascending
                                select new LoanHistoryModel
                                {
                                   _Date = p.TransactionDate,
                                   Disbursement = pa.AmountApplied,
                                   Interest = null,
                                   Payment=null,
                                   WaiveOrRebate = null,
                                   LoanDisType = ld.LoanDisbursementType.Id,
                                   LoanId = -1
                                };
            var receivables = from r in ObjectContext.Receivables
                              where r.FinancialAccountId == selectedFinancialAccountID
                              orderby r.ValidityPeriod
                              select new LoanHistoryModel
                              {
                                  _Date = r.ValidityPeriod,
                                  Disbursement = null,
                                  Interest = r.Amount,
                                  Payment = null,
                                  WaiveOrRebate = null,
                                  LoanDisType = -1,
                                  LoanId = -1
                              };
            var waiveInterest = from r in ObjectContext.Receivables
                                join ra in ObjectContext.ReceivableAdjustments on r.Id equals ra.ReceivableId
                                where r.FinancialAccountId == selectedFinancialAccountID
                                select new LoanHistoryModel
                                {
                                    _Date = ra.Date,
                                    Disbursement = null,
                                    Interest = null,
                                    Payment = null,
                                    WaiveOrRebate = ra.Amount,
                                    LoanDisType = -1,
                                    LoanId = -1
                                };
            var waiveLoan = from la in ObjectContext.LoanAdjustments
                            where la.FinancialAccountId == selectedFinancialAccountID
                            select new LoanHistoryModel
                            {
                                _Date = la.Date,
                                Disbursement = null,
                                Interest = null,
                                Payment = null,
                                WaiveOrRebate = la.Amount,
                                LoanDisType = -1,
                                LoanId = -1

                            };

            var restructureAccounts = from f in ObjectContext.FinancialAccounts
                                      join ls in ObjectContext.LoanAccountStatus on f.ParentFinancialAccountId equals ls.FinancialAccountId
                                      where f.ParentFinancialAccountId == selectedFinancialAccountID && ls.IsActive == true
                                      select new LoanHistoryModel
                                      {
                                          _Date = ls.TransitionDateTime,
                                          Disbursement = null,
                                          Interest = null,
                                          Payment = null,
                                          WaiveOrRebate = null,
                                          LoanDisType = -1,
                                          LoanId = f.Id
                                      };

            var payment = from pay in ObjectContext.PaymentHistoryViewLists
                        join p in ObjectContext.Payments on pay.PaymentID equals p.Id
                        join par in ObjectContext.Parties on p.PartyRole.PartyId equals par.Id
                        where pay.FinancialAccountId == selectedFinancialAccountID
                        select new LoanHistoryModel
                        {
                            _Date = pay.Date,
                            Disbursement = null,
                            Interest = null,
                            Payment = pay.Amount,
                            WaiveOrRebate = null,
                            LoanDisType = -1,
                            LoanId = -1
                        };

            var query = disbursements.Concat(receivables).Concat(waiveLoan).Concat(waiveInterest).Concat(payment).Concat(restructureAccounts);
            query = query.OrderBy(entity => entity._Date);
            return query;
        }

        public static List<OutstandingLoanSummary> RetrieveOutstandingLoanSummary()
        {
            DateTime now = DateTime.Today;
            DateTime startOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
            DateTime startOfNextMonth = startOfCurrentMonth.AddMonths(1);

            var loanAccounts = from la in ObjectContext.LoanAccounts
                               join ls in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals ls.FinancialAccountId
                               where ls.IsActive == true && (ls.StatusTypeId != LoanAccountStatusType.RestructuredType.Id && ls.StatusTypeId != LoanAccountStatusType.WrittenOffType.Id)
                               select la;

            //Disbursed Amount this month
            var loansGranted = from la in loanAccounts
                               join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                               join fa in ObjectContext.FinancialAccounts on la.FinancialAccountId equals fa.Id
                               join lvcr in ObjectContext.LoanDisbursementVcrs on fa.AgreementId equals lvcr.AgreementId
                               where la.LoanReleaseDate >= startOfCurrentMonth && lvcr.Amount > lvcr.Balance
                               group new { fap, lvcr } by fap.FinancialProductId into fapr
                               select new { FinancialProductId = fapr.Key, Balance = fapr.Sum(entity => entity.lvcr.Amount - (entity.lvcr.Balance ?? 0)) };

            //Balances of loans released not this month
            var loanBalances = from la in loanAccounts
                               join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                               join fa in ObjectContext.FinancialAccounts on fap.FinancialAccountId equals fa.Id
                               join lvcr in ObjectContext.LoanDisbursementVcrs on fa.AgreementId equals lvcr.AgreementId
                               where la.LoanReleaseDate < startOfCurrentMonth
                               group new { fap, la, lvcr } by fap.FinancialProductId into fapr
                               select new { FinancialProductId = fapr.Key, Balance = fapr.Sum(entity => entity.la.LoanBalance - (entity.lvcr.Balance ?? 0)) };

            var principalPayments = from la in loanAccounts
                                    join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                                    join pa in ObjectContext.PaymentApplications on la.FinancialAccountId equals pa.FinancialAccountId
                                    join p in ObjectContext.Payments on pa.PaymentId equals p.Id
                                    where p.TransactionDate < startOfNextMonth && p.TransactionDate >= startOfCurrentMonth
                                    select new { la, pa, fap };

            var payments = from pp in principalPayments
                           group pp by pp.fap.FinancialProductId into lbid
                           select new { FinancialProductId = lbid.Key, Balance = (decimal)lbid.Sum(entity => entity.pa.AmountApplied) };

            var loanAdjustmentsNotThisMonth = from la in loanAccounts
                                              join las in ObjectContext.LoanAdjustments on la.FinancialAccountId equals las.FinancialAccountId
                                              join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                                              where las.Date < startOfCurrentMonth
                                              group new { fap, las } by fap.FinancialProductId into lbid
                                              select new { FinancialProductId = lbid.Key, Balance = (decimal)lbid.Sum(entity => -entity.las.Amount) };

            var loanAdjustmentsForThisMonth = from la in loanAccounts
                                              join las in ObjectContext.LoanAdjustments on la.FinancialAccountId equals las.FinancialAccountId
                                              join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                                              where las.Date < startOfNextMonth && las.Date >= startOfCurrentMonth
                                              group new { fap, las } by fap.FinancialProductId into lbid
                                              select new { FinancialProductId = lbid.Key, Balance = (decimal)lbid.Sum(entity => -entity.las.Amount) };


            var paymentsOfLoanGranted = from pp in principalPayments
                                        where pp.la.LoanReleaseDate >= startOfCurrentMonth
                                        group pp by pp.fap.FinancialProductId into lbid
                                        select new { FinancialProductId = lbid.Key, Balance = lbid.Sum(entity => -entity.pa.AmountApplied) };

            var outstandingLoansCurrentMonth = from lb in loanBalances.Concat(loansGranted).Concat(paymentsOfLoanGranted).Concat(loanAdjustmentsForThisMonth)
                                               group lb by lb.FinancialProductId into lbid
                                               select new { FinancialProductId = lbid.Key, Balance = lbid.Sum(entity => entity.Balance) };


            //amount disbursed this month
            var disbursedThisMonth = from la in loanAccounts
                                     join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId
                                     join fa in ObjectContext.FinancialAccounts on fap.FinancialAccountId equals fa.Id
                                     join lvcr in ObjectContext.LoanDisbursementVcrs on fa.AgreementId equals lvcr.AgreementId
                                     join pa in ObjectContext.PaymentApplications on lvcr.Id equals pa.LoanDisbursementVoucherId
                                     join p in ObjectContext.Payments on pa.PaymentId equals p.Id
                                     join d in ObjectContext.Disbursements on p.Id equals d.PaymentId
                                     where p.TransactionDate >= startOfCurrentMonth && la.LoanReleaseDate < startOfCurrentMonth
                                     group new { fap, p } by fap.FinancialProductId into fapr
                                     select new { FinancialProductId = fapr.Key, Balance = fapr.Sum(entity => -entity.p.TotalAmount) };

            var paymentsForLoansNotThisMonth = from pp in principalPayments
                                               where pp.la.LoanReleaseDate < startOfCurrentMonth
                                               group pp by pp.fap.FinancialProductId into lbid
                                               select new { FinancialProductId = lbid.Key, Balance = lbid.Sum(entity => entity.pa.AmountApplied) };

            var outstandingLoansLastMonth = from ollme in loanBalances.Concat(disbursedThisMonth).Concat(paymentsForLoansNotThisMonth).Concat(loanAdjustmentsNotThisMonth)
                                            group ollme by ollme.FinancialProductId into lbid
                                            select new { FinancialProductId = lbid.Key, Balance = lbid.Sum(entity => entity.Balance) };


            var totalPerProduct = from olme in outstandingLoansLastMonth.Concat(loansGranted)
                                  group olme by olme.FinancialProductId into lbid
                                  select new { FinancialProductId = lbid.Key, Balance = lbid.Sum(entity => entity.Balance) };

       
            

            List<OutstandingLoanSummary> summaries = new List<OutstandingLoanSummary>();
            foreach (var currentOutstandingLoan in outstandingLoansCurrentMonth)
            {
                OutstandingLoanSummary summary = new OutstandingLoanSummary();
                var lastMonth = outstandingLoansLastMonth.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                var granted = loansGranted.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                var total = totalPerProduct.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                var principalPayment = payments.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                var loanbalance = loanBalances.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                var loanadjustment = loanAdjustmentsForThisMonth.SingleOrDefault(entity => entity.FinancialProductId == currentOutstandingLoan.FinancialProductId);
                //var currentoutstanding

                FinancialProduct product = FinancialProduct.GetById(currentOutstandingLoan.FinancialProductId);
                summary.FinancialProductName = product.Name;
                summary.FinancialProductId = product.Id;
                
                summary.OutstandingLoanBalanceAsOfCurrentMonthEnd = currentOutstandingLoan.Balance;
                if (lastMonth != null)
                    summary.OutstandingLoanBalanceAsOfLastMonthEnd = lastMonth.Balance;

                if (granted != null)
                    summary.LoansGranted = granted.Balance;

                if (total != null)
                    summary.Total = total.Balance;

                if (principalPayment != null)
                    summary.PrincipalPayment = principalPayment.Balance;

                if (loanadjustment != null)
                    summary.LoanAdjustments = loanadjustment.Balance * -1;

                summaries.Add(summary);
            }

            if (outstandingLoansCurrentMonth.Count() > 0)
            {
                OutstandingLoanSummary summary2 = new OutstandingLoanSummary();
                summary2.FinancialProductName = "Total";
                if (outstandingLoansCurrentMonth.Count() > 0)
                    summary2.OutstandingLoanBalanceAsOfCurrentMonthEnd = outstandingLoansCurrentMonth.Sum(entity => entity.Balance);
                if (outstandingLoansLastMonth.Count() > 0)
                    summary2.OutstandingLoanBalanceAsOfLastMonthEnd = outstandingLoansLastMonth.Sum(entity => entity.Balance);
                if (loansGranted.Count() > 0)
                    summary2.LoansGranted = loansGranted.Sum(entity => entity.Balance);
                if (totalPerProduct.Count() > 0)
                    summary2.Total = totalPerProduct.Sum(entity => entity.Balance);
                if (principalPayments.Count() > 0)
                    summary2.PrincipalPayment = payments.Sum(entity => entity.Balance);
                if (loanAdjustmentsForThisMonth.Count() > 0)
                    summary2.LoanAdjustments = loanAdjustmentsForThisMonth.Sum(entity => entity.Balance) * -1;
                summaries.Add(summary2);
            }
            return summaries;
        }

        public static IEnumerable<OutstandingLoansModel> RetrieveOutstandingLoansAsOf(DateTime date)
        {
            var nextDay = date.AddDays(1);

            var loanAccount =   from la in ObjectContext.LoanAccounts
                                join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                        where las.IsActive && las.StatusTypeId != LoanAccountStatusType.WrittenOffType.Id &&
                                        las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                        select la.FinancialAccount;

            var query = from fa in loanAccount
                        join agreement in ObjectContext.Agreements on fa.AgreementId equals agreement.Id
                        join agreementRole in ObjectContext.AgreementRoles on agreement.Id equals agreementRole.AgreementId
                        join agreeementItem in ObjectContext.AgreementItems on agreement.Id equals agreeementItem.AgreementId
                        join fap in ObjectContext.FinancialAccountProducts on fa.Id equals fap.FinancialAccountId
                        where fa.LoanAccount.LoanReleaseDate <= nextDay &&
                        (agreement.EndDate == null || (date >= agreement.EffectiveDate && date <= agreement.EndDate))
                        select new
                        {
                            FinancialAccount = fa,
                            Agreement = agreement,
                            AgreementItem = agreeementItem,
                            FinancialAccountProduct = fap,
                        };

            //var financialAccounts = (from all in query
            //                         select all.FinancialAccount).Distinct();

            //var principalPayments = from la in financialAccounts
            //                        join rc in Context.Receivables on la.Id equals rc.FinancialAccountId
            //                        join pa in Context.PaymentApplications on rc.Id equals pa.ReceivableId
            //                        join p in Context.Payments on pa.PaymentId equals p.Id
            //                        where rc.Balance < rc.Amount && p.TransactionDate <= date
            //                         && rc.ReceivableTypeId == ReceivableType.PrincipalType.Id
            //                        group new { la, pa } by la.Id into laid
            //                        select new { FinancialAccountId = laid.Key, Balance = laid.Sum(entity => entity.pa.AmountApplied) };

            var principalPayments = from la in loanAccount
                                    join fap in ObjectContext.FinancialAccountProducts on la.Id equals fap.FinancialAccountId
                                    join pa in ObjectContext.PaymentApplications on la.Id equals pa.FinancialAccountId
                                    join p in ObjectContext.Payments on pa.PaymentId equals p.Id
                                    where p.TransactionDate >date
                                    group new { la, pa } by la.Id into laid
                                    select new { FinancialAccountId = laid.Key, Balance = laid.Sum(entity => entity.pa.AmountApplied) };


            List<OutstandingLoansModel> models = new List<OutstandingLoansModel>();
            foreach (var item in query.ToList())
            {
                var principalPayment = principalPayments.FirstOrDefault(entity => entity.FinancialAccountId == item.FinancialAccount.Id);
                OutstandingLoansModel model;
                if(principalPayment != null)
                    model = new OutstandingLoansModel(item.FinancialAccount, item.Agreement, item.FinancialAccountProduct, item.AgreementItem, principalPayment.Balance, date);
                else
                    model = new OutstandingLoansModel(item.FinancialAccount, item.Agreement, item.FinancialAccountProduct, item.AgreementItem, date);
                models.Add(model);
            }

            return models;
        }
        public static void UpdateLoanStatus(DateTime today, LoanAccount loanAccount, decimal balance)
        {
            var currentstatus = loanAccount.CurrentStatus;

            if ((currentstatus.StatusTypeId == LoanAccountStatusType.CurrentType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.DelinquentType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id
                     || currentstatus.StatusTypeId == LoanAccountStatusType.RestructuredType.Id)
                     && balance == 0)
            {
                var isValidIndicator = ObjectContext.LoanAccountStatusTypeAssocs.Where(entity => entity.FromStatusTypeId == currentstatus.StatusTypeId
                    && entity.ToStatusTypeId == LoanAccountStatusType.PaidOffType.Id && entity.EndDate == null).Count();

                if (isValidIndicator > 0)
                {
                    currentstatus.IsActive = false;
                    LoanAccountStatu loanAccountStatus = new LoanAccountStatu();
                    loanAccountStatus.FinancialAccountId = currentstatus.FinancialAccountId;
                    loanAccountStatus.StatusTypeId = LoanAccountStatusType.PaidOffType.Id;
                    loanAccountStatus.TransitionDateTime = today;
                    loanAccountStatus.IsActive = true;
                    ObjectContext.LoanAccountStatus.AddObject(loanAccountStatus);
                }
            }
        }
        public static void UpdateLoanAccountStatus(DateTime date){
            var dayDiff = 0;

            var loanAccounts = from la in ObjectContext.LoanAccounts
                               join ls in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals ls.FinancialAccountId
                               where ls.IsActive == true && (ls.StatusTypeId == LoanAccountStatusType.CurrentType.Id)
                               select la;
            
            foreach (var loanAccount in loanAccounts)
            {
                var payments = ObjectContext.FinAcctTrans.Where(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId && entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).OrderByDescending(entity => entity.TransactionDate);
                //3 months after last payment date or if no payment since after release, change status to delinquent
                if (payments.Count() > 0)
                {
                    var LastPaymentDate = payments.FirstOrDefault().TransactionDate;
                    dayDiff = date.Subtract(LastPaymentDate).Days;
                }
                else dayDiff = date.Subtract(loanAccount.LoanReleaseDate.Value).Days;

                    if (dayDiff > 90)
                    {
                        var activeStatus = loanAccount.CurrentStatus;
                        var isValidIndicator = ObjectContext.LoanAccountStatusTypeAssocs.Where(entity => entity.FromStatusTypeId == activeStatus.StatusTypeId
                              && entity.ToStatusTypeId == LoanAccountStatusType.DelinquentType.Id && entity.EndDate == null).Count();

                        if (isValidIndicator > 0)
                        {
                            activeStatus.IsActive = false;
                            LoanAccountStatu loanStatus = new LoanAccountStatu();
                            loanStatus.LoanAccount = loanAccount;
                            loanStatus.LoanAccountStatusType = LoanAccountStatusType.DelinquentType;
                            loanStatus.TransitionDateTime = DateTime.Now;
                            loanStatus.IsActive = true;

                            ObjectContext.LoanAccountStatus.AddObject(loanStatus);
                        }
                    }
                
            }
            
        }
        public static LoanAccount GetById(int id)
        {
            return ObjectContext.LoanAccounts.FirstOrDefault(e => e.FinancialAccountId == id);
        }
    }

    public class OutstandingLoansModel
    {
        public int LoanId { get; set; }
        public string Name { get; set; }
        public string LoanProduct { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime? LoanReleaseDate { get; set; }
        public int LoanTerm { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal LoanBalance { get; set; }
        public string Status { get; set; }
        public string _LoanReleaseDate 
        {
            get
            {
                if (this.LoanReleaseDate.HasValue)
                    return this.LoanReleaseDate.Value.ToString("MM/dd/yyyy");
                else return string.Empty;
            }
        }
        public string _MaturityDate 
        {
            get
            {
                if (this.MaturityDate.HasValue)
                    return this.MaturityDate.Value.ToString("MM/dd/yyyy");
                else return string.Empty;
            }
        }

        public OutstandingLoansModel(FinancialAccount fa, Agreement ag, FinancialAccountProduct fap, AgreementItem agreementItem, DateTime date)
        {
            if (agreementItem == null)
                this.InterestRate = agreementItem.InterestRate;

            var loanAccount = fa.LoanAccount;
            this.LoanId = loanAccount.FinancialAccountId;
            this.LoanProduct = fap.FinancialProduct.Name;
            this.LoanAmount = loanAccount.LoanAmount;
            this.LoanTerm = agreementItem.LoanTermLength;
            this.LoanReleaseDate = loanAccount.LoanReleaseDate;
            this.LoanBalance = loanAccount.LoanBalance;
            this.MaturityDate = fa.LoanAccount.MaturityDate;
            this.LoanTerm = agreementItem.LoanTermLength;
            this.InterestRate = agreementItem.InterestRate;
            var status = loanAccount.LoanAccountStatus.OrderByDescending(entity => entity.TransitionDateTime <= date).LastOrDefault();
            this.Status = status.LoanAccountStatusType.Name;

            var partyRole = fa.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == fa.Id &&
                entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id).PartyRole;
            var party = partyRole.Party;
            if (party.PartyTypeId == PartyType.PersonType.Id)
            {
                Person personAsCustomer = party.Person;

                this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                    , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                    personAsCustomer.NameSuffixString);
            }
        }

        public OutstandingLoansModel(FinancialAccount fa, Agreement ag, FinancialAccountProduct fap, AgreementItem agreementItem, decimal totalPayments, DateTime date)
        {
            if (agreementItem == null)
                this.InterestRate = agreementItem.InterestRate;

            var loanAccount = fa.LoanAccount;
            this.LoanId = loanAccount.FinancialAccountId;
            this.LoanProduct = fap.FinancialProduct.Name;
            this.LoanAmount = loanAccount.LoanAmount;
            this.LoanTerm = agreementItem.LoanTermLength;
            this.LoanReleaseDate = loanAccount.LoanReleaseDate;
            this.LoanBalance = loanAccount.LoanBalance + totalPayments;
            this.MaturityDate = fa.LoanAccount.MaturityDate;
            this.LoanTerm = agreementItem.LoanTermLength;
            this.InterestRate = agreementItem.InterestRate;
            var status = loanAccount.LoanAccountStatus.OrderByDescending(entity => entity.TransitionDateTime <= date).FirstOrDefault();
            this.Status = status.LoanAccountStatusType.Name;

            var partyRole = fa.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == fa.Id &&
                entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id).PartyRole;
            var party = partyRole.Party;
            if (party.PartyTypeId == PartyType.PersonType.Id)
            {
                Person personAsCustomer = party.Person;

                this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                    , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                    personAsCustomer.NameSuffixString);
            }
        }
    }

    public class OutstandingLoanSummary
    {
        /* LG - released and disbursed this month
         * loanbalance - remaining loan balance (loanaccount.balance - vcr.balance)
         * OLME = LG + loanbalance - Payments(LG)
         * OLLME = loanbalance - disbursedthismonth(account not this month) + principalpayment(loan balance)
         * Total = OLLME + LG
         */
        /// <summary>
        /// The total of the remaining principal amount for each existing loan per loan product as of the last day of the previous month
        /// </summary>
        public decimal OutstandingLoanBalanceAsOfLastMonthEnd { get; set; }

        /// <summary>
        /// The difference between Total and Less: Payments Principal
        /// </summary>
        public decimal OutstandingLoanBalanceAsOfCurrentMonthEnd { get; set; }

        /// <summary>
        /// The sum of the principal amount of loans granted from first day up to the last day of the current month per loan product
        /// </summary>
        public decimal LoansGranted { get; set; }

        /// <summary>
        /// The sum of Outstanding Loans as of (last month-end) and Add: Granted
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// The sum of the principal payments made from first day up to the last day of the current month per loan product
        /// </summary>
        public decimal PrincipalPayment { get; set; }

          /// <summary>
        /// Total Loan Adjustments
        /// </summary>
        public decimal LoanAdjustments { get; set; }

        public string FinancialProductName { get; set; }

        public int FinancialProductId { get; set; }
 
       

        public OutstandingLoanSummary()
        {
            this.OutstandingLoanBalanceAsOfLastMonthEnd = 0;
            this.OutstandingLoanBalanceAsOfCurrentMonthEnd = 0;
            this.LoansGranted = 0;
            this.Total = 0;
            this.PrincipalPayment = 0;
            this.LoanAdjustments = 0;
        }
    }
}
