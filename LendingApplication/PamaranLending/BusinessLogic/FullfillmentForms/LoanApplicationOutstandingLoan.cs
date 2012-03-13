using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.FullfillmentForms
{
    public class OutstandingLoan
    {
        public int LoanId { get; set; }
        public string LoanProductName { get; set; }
        public string InterestComputationMode { get; set; }
        public DateTime? _MaturityDate { get; set; }
        public string MaturityDate
        {
            get
            {
                if (this._MaturityDate.HasValue)
                    return this._MaturityDate.Value.ToString("MMM dd, yyy");
                else
                    return string.Empty;
            }
        }
        public int NoOfInstallments { get; set; }
        public string PaymentMode { get; set; }
        public decimal ScheduledAmortization { get; set; }
        public decimal LoanBalance { get; set; }
        public decimal PaidInstallments { get; set; }
    }

    public partial class LoanApplicationForm
    {

        //Select * 
        //From dbo.FinancialAccountRole far
        //join FinancialAccount fa on fa.Id = far.FinancialAccountId
        //Join dbo.Agreement a on fa.AgreementId = a.Id
        //Join dbo.AgreementItem ai on a.Id = ai.AgreementId
        //Join dbo.AgreementRole ar on a.Id = ar.AgreementId
        //Join dbo.PartyRole pr on ar.PartyRoleId = pr.Id
        //Join dbo.RoleType rt on pr.RoleTypeId = rt.Id
        //Join dbo.AmortizationSchedule ams on a.Id = ams.AgreementId
        //Join dbo.AmortizationScheduleItem amsi on ams.Id = amsi.AmortizationScheduleId
        //Join dbo.FinancialAccountProduct fap on fa.Id = fap.FinancialAccountId
        //Join dbo.FinancialProduct fp on fap.FinancialProductId = fp.Id
        //where ams.EndDate is NULL and a.EndDate is NULL and fap.EndDate is NULL and rt.Id = 25

        private int PartyRoleIdCache = -1;
        private List<OutstandingLoan> cache;

        public List<OutstandingLoan> RetrieveOutstandingLoans()
        {
            if (PartyRoleId == -1)
                throw new NotSupportedException("Party role id is not yet initialized.");

            if (this.PartyRoleIdCache == this.PartyRoleId && this.cache != null)
                return cache;

            var customerPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.PartyRoleId);
            var ownerPartyRole = PartyRole.GetByPartyIdAndRole(customerPartyRole.PartyId, RoleType.OwnerFinancialType);

            var financialAccountRolesOfCustomer = from farc in Context.FinancialAccountRoles
                                                  join pr in Context.PartyRoles on farc.PartyRoleId equals pr.Id
                                                  where pr.PartyId == customerPartyRole.PartyId
                                                  && pr.EndDate == null
                                                  && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                                                  select farc;

            var financialAccountRoles = from farc in financialAccountRolesOfCustomer
                                        join la in Context.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                        join las in Context.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                        where las.IsActive && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.RestructuredType.Id)
                                        select farc;



            if(financialAccountRoles.Count() == 0)
                return null;

            var all = from far in financialAccountRoles
                      join fa in Context.FinancialAccounts on far.FinancialAccountId equals fa.Id
                      join agreement in Context.Agreements on fa.AgreementId equals agreement.Id
                      join agreementRole in Context.AgreementRoles on agreement.Id equals agreementRole.AgreementId
                      join pr in Context.PartyRoles on agreementRole.PartyRoleId equals pr.Id
                      join prt in Context.RoleTypes on pr.PartyRoleType.RoleType.Id equals prt.Id
                      join ams in Context.AmortizationSchedules on agreement.Id equals ams.AgreementId
                      //join amsi in Context.AmortizationScheduleItems on ams.Id equals amsi.AmortizationScheduleId
                      join fap in Context.FinancialAccountProducts on fa.Id equals fap.FinancialAccountId
                      join fp in Context.FinancialProducts on fap.FinancialProductId equals fp.Id
                      where 
                      //ams.EndDate == null && agreement.EndDate == null && 
                      fap.EndDate == null && prt.Id == RoleType.BorrowerAgreementType.Id
                      select new
                      {
                          FinancialAccount = fa,
                          Agreement = agreement,
                          Amortization = ams,
                          //AmortizationItem = amsi,
                          FinancialAccountProduct = fap,
                          FinancialProduct = fp,
                      };

            var financialAccounts = from far in financialAccountRoles
                                    join fa in Context.FinancialAccounts on far.FinancialAccountId equals fa.Id
                                    select fa;

            var receivables = from fa in financialAccounts
                              join r in Context.Receivables on fa.Id equals r.FinancialAccountId
                              join rt in Context.ReceivableTypes on r.ReceivableTypeId equals rt.Id
                              where rt.Id == ReceivableType.InterestType.Id
                              group r by r.FinancialAccountId into rfid
                              select new { FinancialAccountId = rfid.Key, InterestBalance = rfid.Sum(entity => entity.Balance) };

            SystemSetting setting = SystemSetting.GetActive(SystemSettingType.PercentageofLoanAmountPaidType);

            int percentExpected = 0;
            if(setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                percentExpected = int.Parse(setting.Value);

            List<OutstandingLoan> loans = new List<OutstandingLoan>();
            foreach (var financialAccount in financialAccounts)
            {
                var current = all.SingleOrDefault(a => a.FinancialAccount.Id == financialAccount.Id);
                var loanAccount = financialAccount.LoanAccount;
                if (loanAccount == null)
                    continue;

                AgreementItem agreementItem = null;
                if (loanAccount.FinancialAccount.Agreement != null)
                {
                    agreementItem = loanAccount.FinancialAccount.Agreement.AgreementItems.FirstOrDefault(entity => entity.IsActive == true);
                    if (agreementItem == null)
                        continue;
                }

                var receivable = receivables.FirstOrDefault(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId);
                decimal receivableInterestBalance = receivable != null ? receivable.InterestBalance : 0;

                OutstandingLoan loan = new OutstandingLoan();
                loan.ScheduledAmortization = 0;
                loan._MaturityDate = loanAccount.MaturityDate;
                if (agreementItem.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name)
                    loan.LoanBalance = receivableInterestBalance + loanAccount.LoanBalance;

                var amortizationScheduleItems = current.Amortization.AmortizationScheduleItems.Where(entity => entity.IsBilledIndicator == false);

                if (agreementItem.InterestComputationMode == ProductFeature.StraightLineMethodType.Name && amortizationScheduleItems.Count() > 0)
                {
                    var minScheduleDate = amortizationScheduleItems.Min(a => a.ScheduledPaymentDate);
                    var firstScheduleItem = amortizationScheduleItems.SingleOrDefault(entity=>entity.ScheduledPaymentDate == minScheduleDate);
                    loan.ScheduledAmortization = firstScheduleItem.InterestPayment + firstScheduleItem.PrincipalPayment;
                }

                if (agreementItem.InterestComputationMode == ProductFeature.StraightLineMethodType.Name)
                {
                    int percentage = (int)Math.Floor(((loanAccount.LoanAmount - loanAccount.LoanBalance) * 100) / loanAccount.LoanAmount);
                    if (percentage >= percentExpected)
                        loan.LoanBalance = receivableInterestBalance + loanAccount.LoanBalance;
                    else
                    {
                        var interestPayment = amortizationScheduleItems.Sum(a => a.InterestPayment);
                        var interestBalance = interestPayment + receivableInterestBalance;
                        loan.LoanBalance = interestBalance + loanAccount.LoanBalance;
                    }
                }

                loan.LoanId = loanAccount.FinancialAccountId;
                loan.InterestComputationMode = agreementItem.InterestComputationMode;
                loan.PaymentMode = agreementItem.PaymentMode;
                loan.LoanProductName = current.FinancialProduct.Name;
                loan.NoOfInstallments = current.Amortization.AmortizationScheduleItems.Count();
                if (loan.NoOfInstallments == 0)
                {
                    var transactions = Context.FinAcctTrans.Where(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId);
                    loan.PaidInstallments = transactions.Count();
                }
                else
                {
                    loan.PaidInstallments = (loanAccount.LoanAmount - loanAccount.LoanBalance) / (loanAccount.LoanAmount / loan.NoOfInstallments);
                }
                loans.Add(loan);
            }

            cache = loans;
            this.PartyRoleIdCache = this.PartyRoleId;
            return loans;
        }

        private void RetrieveOutstandingLoansNotUsed()
        {
            if (PartyRoleId == -1)
                throw new NotSupportedException("Party role id is not yet initialized.");

            var customerPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == this.PartyRoleId);
            var financialAccountRoles = Context.FinancialAccountRoles.Where(entity => entity.PartyRoleId == customerPartyRole.Id);
            var financialAccounts = from far in financialAccountRoles
                                    join fa in Context.FinancialAccounts on far.FinancialAccountId equals fa.Id
                                    select fa;

            var withFinancialProduct = from fp in Context.FinancialProducts
                                       join fap in Context.FinancialAccountProducts on fp.Id equals fap.FinancialAccountId
                                       join fa in financialAccounts on fap.FinancialAccountId equals fa.Id
                                       select new { FinancialAccountProduct = fap, FinancialProduct = fp, FinancialAccount = fa };

            var withAgreementRole = from war in Context.AgreementRoles
                                    join agreement in Context.Agreements on war.AgreementId equals agreement.Id
                                    join fa in financialAccounts on agreement.Id equals fa.AgreementId
                                    where war.PartyRoleId == customerPartyRole.Id
                                    select new { Agreement = agreement, FinancialAccount = fa, AgreementRole = war };

            //var withAgreement = from agreement in Context.Agreements
            //                    join fa in financialAccounts on agreement.Id equals fa.AgreementId
            //                    select new { Agreement = agreement, FinancialAccount = fa };

            var withAmortizationSchedule = from ams in Context.AmortizationSchedules
                                           join amsi in Context.AmortizationScheduleItems on ams.Id equals amsi.AmortizationScheduleId
                                           join all in withAgreementRole on ams.AgreementId equals all.Agreement.Id
                                           select new
                                           {
                                               Amortization = ams,
                                               AmortizationItem = amsi,
                                               Agreement = all.Agreement,
                                               FinancialAccount = all.FinancialAccount,
                                               AgreementRole = all.AgreementRole
                                           };


        }
    }
}
 