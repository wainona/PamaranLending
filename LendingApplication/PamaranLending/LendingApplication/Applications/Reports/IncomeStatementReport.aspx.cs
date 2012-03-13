using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.Reports
{
    public partial class IncomeStatementReport: ActivityPageBase
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

        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                return allowed;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var today = DateTime.Today.AddMonths(-1);
                dtDate.SelectedDate = today;
                dtDate.MaxDate = today;
                
                FillHeader(today);
                btnGenerate.FireEvent("Click");
            }
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Parse(hdnDate.Value.ToString());

            DateTime startOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            DateTime startOfNextMonth = startOfCurrentMonth.AddMonths(1);

            var revenues = GetRevenues(startOfCurrentMonth, startOfNextMonth).ToList();
            var expenses = GetExpenses(startOfCurrentMonth, startOfNextMonth).ToList();

            decimal interest = revenues.Sum(x => x.InterestPaid);
            decimal expense = expenses.Sum(x => x.Amount);
            decimal tithes = (interest * 10) / 100;

            decimal interestlesstithes = interest - tithes;
            decimal additionalCaptial = (interestlesstithes - expense) / 2;
            decimal NetIncomePerChildren = Math.Round(additionalCaptial / 6);


            lblTithes.Text = tithes.ToString();
            lblTotalInterestEarned.Text = interest.ToString();
            lblAdditionalCapital.Text = additionalCaptial.ToString();
            lblIncomePerChildren.Text = NetIncomePerChildren.ToString();
            lblLessTithes.Text = interestlesstithes.ToString();
            lblMonth.Text = string.Format("{0:MMMM yyyy}", today);


            strRevenue.DataSource = revenues;
            strRevenue.DataBind();
            strExpenses.DataSource = expenses;
            strExpenses.DataBind();
        }

        public IEnumerable<RevenueModel> GetRevenues(DateTime startCurrentMonth, DateTime startOfNextMonth)
        {
            var ForeignPayments = from p in ObjectContext.Payments
                           join pc in ObjectContext.PaymentCurrencyAssocs on p.Id equals pc.PaymentId
                           select p;


            var interestPaidAsCashWithStation = from r in ObjectContext.Receivables
                                                join pa in ObjectContext.PaymentApplications on r.Id equals pa.ReceivableId
                                                join p in ObjectContext.Payments.Except(ForeignPayments) on pa.PaymentId equals p.Id
                                                join cc in ObjectContext.CustomerClassifications on p.ProcessedToPartyRoleId equals cc.PartyRoleId
                                                join ct in ObjectContext.ClassificationTypes on cc.ClassificationTypeId equals ct.Id
                                                join dt in ObjectContext.DistrictTypes on ct.DistrictTypeId equals dt.Id
                                                where p.EntryDate >= startCurrentMonth && p.EntryDate < startOfNextMonth
                                                group new { dt, pa } by dt.Name into ip
                                                select new RevenueModel
                                                {
                                                    Station = ip.Key,
                                                    InterestPaid = ip.Sum(e => e.pa.AmountApplied),
                                                    CapitalPaid = 0,
                                                    RemainingLoanBalances = 0
                                                };

            
            var capitalPaidWithStation = from pa in ObjectContext.PaymentApplications
                                         join la in ObjectContext.LoanAccounts on pa.FinancialAccountId equals la.FinancialAccountId
                                         join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                         join p in ObjectContext.Payments.Except(ForeignPayments) on pa.PaymentId equals p.Id
                                         join cc in ObjectContext.CustomerClassifications on p.ProcessedToPartyRoleId equals cc.PartyRoleId
                                         join ct in ObjectContext.ClassificationTypes on cc.ClassificationTypeId equals ct.Id
                                         join dt in ObjectContext.DistrictTypes on ct.DistrictTypeId equals dt.Id
                                         where p.EntryDate >=  startCurrentMonth && p.EntryDate <startOfNextMonth
                                         && las.IsActive && las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                         group new { dt, la, pa } by dt.Name into ip
                                         select new RevenueModel
                                         {
                                             Station = ip.Key,
                                             InterestPaid =0,
                                             CapitalPaid = ip.Sum(e => e.pa.AmountApplied),
                                             RemainingLoanBalances = 0
                                         };
            var loansWithNoPayments = from la in ObjectContext.LoanAccounts
                                      join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                      join fa in ObjectContext.FinancialAccountRoles on la.FinancialAccountId equals fa.FinancialAccountId
                                      join pr in ObjectContext.PartyRoles on fa.PartyRole.PartyId equals pr.PartyId
                                      join cc in ObjectContext.CustomerClassifications on pr.Id equals cc.PartyRoleId
                                      join ct in ObjectContext.ClassificationTypes on cc.ClassificationTypeId equals ct.Id
                                      join dt in ObjectContext.DistrictTypes on ct.DistrictTypeId equals dt.Id
                                      where pr.RoleTypeId == RoleType.CustomerType.Id
                                      && las.IsActive && las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                      group new { dt, la } by dt.Name into ip
                                      select new RevenueModel
                                      {
                                          Station = ip.Key,
                                          InterestPaid = 0,
                                          CapitalPaid = 0,
                                          RemainingLoanBalances = ip.Sum( e => e.la.LoanBalance)
                                      };

            var feePayments = from fp in ObjectContext.FeePayments
                              join p in ObjectContext.Payments.Except(ForeignPayments) on fp.PaymentId equals p.Id
                              where p.EntryDate >= startCurrentMonth && p.EntryDate < startOfNextMonth
                              group fp by fp.Particular into ip
                              select new RevenueModel
                              {
                                  Station = ip.Key,
                                  InterestPaid = ip.Sum( e => e.FeeAmount),
                                  CapitalPaid = 0,
                                  RemainingLoanBalances = 0
                              };
            var query = from i in interestPaidAsCashWithStation.Concat(capitalPaidWithStation)
                        group i by i.Station into ip
                        select new RevenueModel
                        {
                            Station = ip.Key,
                            InterestPaid = ip.Sum(e => e.InterestPaid),
                            CapitalPaid = ip.Sum( e => e.CapitalPaid),
                            RemainingLoanBalances = ip.Sum(e => e.RemainingLoanBalances)
                        };
            query = from i in query.Concat(loansWithNoPayments)
                    group i by i.Station into ip
                    select new RevenueModel
                    {
                        Station = ip.Key,
                        InterestPaid = ip.Sum(e => e.InterestPaid),
                        CapitalPaid = ip.Sum(e => e.CapitalPaid),
                        RemainingLoanBalances = ip.Sum(e => e.RemainingLoanBalances)
                    };
            query = from i in query.Concat(feePayments)
                    group i by i.Station into ip
                    select new RevenueModel
                    {
                        Station = ip.Key,
                        InterestPaid = ip.Sum(e => e.InterestPaid),
                        CapitalPaid = ip.Sum(e => e.CapitalPaid),
                        RemainingLoanBalances = ip.Sum(e => e.RemainingLoanBalances)
                    };
            return query;
                             
        }
        public IEnumerable<ExpensesModel> GetExpenses(DateTime startCurrentMonth, DateTime startOfNextMonth)
        {
            var ForeignPayments = from p in ObjectContext.Payments
                                  join pc in ObjectContext.PaymentCurrencyAssocs on p.Id equals pc.PaymentId
                                  select p;
            var otherdisbursements = from d in ObjectContext.Disbursements
                                     join di in ObjectContext.DisbursementItems on d.PaymentId equals di.PaymentId
                                     join p in ObjectContext.Payments.Except(ForeignPayments) on d.PaymentId equals p.Id
                                     where d.DisbursementTypeId == DisbursementType.OtherLoanDisbursementType.Id
                                     group di by di.Particular into od
                                     select new ExpensesModel
                                     {
                                         Particular = od.Key,
                                         Amount = od.Sum( e => e.PerItemAmount)
                                     };
            return otherdisbursements;
        }

        public void FillHeader(DateTime today)
        {
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
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
    }

    public class RevenueModel
    {
        public string Station { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal CapitalPaid { get; set; }
        public decimal RemainingLoanBalances { get; set; }
    }
    public class ExpensesModel
    {
        public string Particular { get; set; }
        public decimal Amount { get; set; }
    }
}