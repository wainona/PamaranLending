using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class DemandLetter
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

        public DemandLetterStatu CurrentStatus
        {
            get
            {
                return this.DemandLetterStatus.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public static void UpdateDemandLetterStatus(DateTime date)
        {
            var delinquentLoanAccounts = from la in Context.LoanAccounts
                                         join ls in Context.LoanAccountStatus on la.FinancialAccountId equals ls.FinancialAccountId
                                         where ls.IsActive == true && (ls.StatusTypeId == LoanAccountStatusType.DelinquentType.Id)
                                         select la;
            var now = DateTime.Now;
            foreach (var delinquentLoanAccount in delinquentLoanAccounts)
            {
                var demandLetter = Context.DemandLetters.FirstOrDefault(entity => entity.FinancialAccountId == delinquentLoanAccount.FinancialAccountId);

                if (demandLetter == null)
                {
                    //var owner = Context.FinancialAccountRoles.FirstOrDefault(entity => entity.FinancialAccountId == delinquentLoanAccount.FinancialAccountId);

                    //Insert FirstDemand Letter
                    DemandLetter firstdemandletter = new DemandLetter();
                    firstdemandletter.FinancialAccountId = delinquentLoanAccount.FinancialAccountId;
                    //firstdemandletter.PartyRoleId = owner.PartyRoleId;

                    //Insert Demand Letter Status
                    DemandLetterStatu demandletterStatus = new DemandLetterStatu();
                    demandletterStatus.DemandLetter = firstdemandletter;
                    demandletterStatus.DemandLetterStatusType = DemandLetterStatusType.RequireFirstDemandLetterType;
                    demandletterStatus.TransitionDateTime = now;
                    demandletterStatus.IsActive = true;
                }
                else
                {
                    var activeStatus = demandLetter.CurrentStatus;
                    
                    if (activeStatus.DemandLetterStatusTypeId == DemandLetterStatusType.FirstDemandLetterSentType.Id && demandLetter.DatePromisedToPay.HasValue)
                    {
                        var dayDiff = date.Subtract(demandLetter.DatePromisedToPay.Value).Days;
                        if (dayDiff > 30)
                        {
                            activeStatus.IsActive = false;
                            DemandLetterStatu demandletterStatus = new DemandLetterStatu();
                            demandletterStatus.DemandLetter = demandLetter;
                            demandletterStatus.DemandLetterStatusType = DemandLetterStatusType.RequireFinalDemandLetterType;
                            demandletterStatus.TransitionDateTime = now;
                            demandletterStatus.IsActive = true;
                        }
          
                    }
                }
            }
        }

        public static List<BadDebtsModel> RetrieveBadDebts(DateTime date)
        {
            var advance2days = date.AddDays(2);
            var delinquentLoanAccounts = from la in Context.LoanAccounts
                                         join ls in Context.LoanAccountStatus on la.FinancialAccountId equals ls.FinancialAccountId
                                         where ls.IsActive == true && 
                                         (ls.StatusTypeId == LoanAccountStatusType.DelinquentType.Id || ls.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                         select la;
            var demandLetter = from dl in delinquentLoanAccounts
                               join d in Context.DemandLetters on dl.FinancialAccountId equals d.FinancialAccountId
                               join f in Context.FinancialAccountRoles on dl.FinancialAccountId equals f.FinancialAccountId
                               where d.DatePromisedToPay.HasValue && d.DatePromisedToPay.Value <= advance2days
                               && f.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id
                               select new BadDebtsModel()
                               {
                                   LoanAccountId = dl.FinancialAccountId,
                                   DatePromiseToPay = d.DatePromisedToPay.Value,
                                   Party = f.PartyRole.Party,
                               };
            return demandLetter.ToList();

        }
    }

    public class BadDebtsModel
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
        public int LoanAccountId { get; set; }
        public DateTime DatePromiseToPay { get; set; }
        public string _DatePromiseToPay { get { return DatePromiseToPay.ToString("MM/dd/yyyy"); } }
        private PartyRole Customer {
            get
            {
                return PartyRole.GetByPartyIdAndRole(Party.Id, RoleType.CustomerType);
            }
        }
        public string StationNumber { 
            get {
                if (this.Customer != null)
                {
                    return Context.CustomerClassifications.FirstOrDefault(entity => entity.PartyRoleId == Customer.Id).ClassificationType.StationNumber;
                }
                else return string.Empty;
            } 
        }
        public string CustomerName
        {
            get
            {
                if (this.Party != null)
                    return Party.Name;
                else
                    return string.Empty;
            }
        }
        public Party Party { get; set; }
    }
}
