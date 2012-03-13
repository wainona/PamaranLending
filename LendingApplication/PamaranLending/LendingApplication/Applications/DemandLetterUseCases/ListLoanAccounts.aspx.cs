using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DemandLetterUseCases
{
    public partial class ListLoanAccounts : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                allowed.Add("Admin");
                return allowed;
            }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.FilterBy = cmbFilterByStatus.SelectedItem.Text;
            if (dtfFrom.SelectedValue != null)
                dataSource.MinDate = dtfFrom.SelectedDate;
            if (dtfTo.SelectedValue != null)
                dataSource.MinDate = dtfTo.SelectedDate;
            dataSource.SearchString = txtSearch.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var loanStatusTypes = ObjectContext.LoanAccountStatusTypes;
                strFilterByStatus.DataSource = loanStatusTypes.ToList();
                strFilterByStatus.DataBind();

                DateTime now = DateTime.Now;
                dtfTo.MaxDate = now;
                dtfFrom.MaxDate = now;

                cmbDemandLetterType.Text = "First Demand Letter";
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

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void DisplayLoans(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }

        private IQueryable<CustomerWithLoanModel> QueryCustomerWithLoan()
        {
            var query = (from lar in ObjectContext.FinancialAccountRoles
                         join fa in ObjectContext.FinancialAccounts on lar.FinancialAccountId equals fa.Id
                         join la in ObjectContext.LoanAccounts on fa.Id equals la.FinancialAccountId
                         join lv in ObjectContext.LoanViewLists on la.FinancialAccountId equals lv.LoanId
                         join pr in ObjectContext.PartyRoles on lar.PartyRoleId equals pr.Id
                         join p in ObjectContext.Parties.Distinct() on pr.PartyId equals p.Id
                         where pr.RoleTypeId == RoleType.OwnerFinancialType.Id && pr.EndDate == null
                         select new CustomerWithLoanModel
                         {
                             CustomerId = lar.PartyRole.PartyId,
                             Name = lv.Name
                         });
            return query;
        }

        private class DataSource : IPageAbleDataSource<LoanListModel>
        {
            public string Name { get; set; }
            public string SearchString { get; set; }
            public string FilterBy { get; set; }
            public DateTime? MinDate { get; set; }
            public DateTime? MaxDate { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
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

            private IEnumerable<LoanListModel> CreateQuery(FinancialEntities context)
            {
                DateTime now = DateTime.Now;

                var query = from la in context.LoanAccounts
                            join las in context.LoanAccountStatus.Where(entity => entity.StatusTypeId != LoanAccountStatusType.PaidOffType.Id && entity.StatusTypeId != LoanAccountStatusType.WrittenOffType.Id)
                                on la.FinancialAccountId equals las.FinancialAccountId
                            join far in context.FinancialAccountRoles on la.FinancialAccountId equals far.FinancialAccountId// financialAccountRole
                            join pr in context.PartyRoles on far.PartyRoleId equals pr.Id
                            join fa in context.FinancialAccounts on la.FinancialAccountId equals fa.Id
                            join ag in context.Agreements on fa.AgreementId equals ag.Id//agreement
                            join ai in context.AgreementItems on ag.Id equals ai.AgreementId
                            join fap in context.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId//financialAccountProduct;
                            join fp in context.FinancialProducts on fap.FinancialProductId equals fp.Id
                            where ag.EndDate == null && ai.IsActive == true
                            && las.IsActive && fap.EndDate == null
                            && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                            && pr.EndDate == null
                            select new LoanListModel
                            {
                                LoanAccount = la,
                                AgreementItem = ai,
                                Product = fp,
                                LoanStatus = las,
                                PartyRole = pr
                            };

                //var query = from dl in ObjectContext.DemandLetters
                //            join la in ObjectContext.LoanAccounts on dl.FinancialAccountId equals la.FinancialAccountId
                //            join dls in ObjectContext.DemandLetterStatus on dl.Id equals dls.DemandLetterId


                var result = query.ToList().AsEnumerable();

                result = result.Where(entity => entity.Name.ToLower().Contains(SearchString.ToLower()));

                var years = SystemSetting.YearsofLoanstobeDeleted;
                if (years.HasValue)
                {
                    TimeSpan limit = new TimeSpan();
                    double ageOfLoansToBeDeleted = (years.Value * 365);
                    limit = TimeSpan.FromDays(ageOfLoansToBeDeleted);
                    result = result.Where(entity => entity.AgeOfLoan < limit);
                }

                result = result.Where(entity => entity.Status.Contains(FilterBy));

                if (MaxDate.HasValue && MinDate.HasValue)
                {
                    result = result.Where(entity => entity.LoanReleaseDate >= MinDate && entity.LoanReleaseDate <= MaxDate);
                }
                return result;
            }

            public override List<LoanListModel> SelectAll(int start, int limit, Func<LoanListModel, string> orderBy)
            {
                List<LoanListModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return collection;
            }
        }

        private class LoanListModel
        {
            public int LoanId
            {
                get
                {
                    if (this.LoanAccount != null)
                        return this.LoanAccount.FinancialAccountId;
                    else
                        return -1;
                }
            }
            public DateTime? LoanReleaseDate
            {
                get
                {
                    if (this.LoanAccount != null)
                        return this.LoanAccount.LoanReleaseDate;
                    else
                        return null;
                }
            }
            public string Name
            {
                get
                {
                    if (this.PartyRole != null)
                        return this.PartyRole.Party.Name;
                    else
                        return string.Empty;
                }
            }
            public string InterestComputationMode
            {
                get
                {
                    if (this.AgreementItem != null)
                        return this.AgreementItem.InterestComputationMode;
                    else
                        return string.Empty;
                }
            }
            public string LoanProduct
            {
                get
                {
                    if (this.Product != null)
                        return this.Product.Name;
                    else
                        return string.Empty;
                }
            }
            public string Status
            {
                get
                {
                    if (this.LoanStatus != null)
                        return this.LoanStatus.LoanAccountStatusType.Name;
                    else
                        return string.Empty;
                }
            }
            public TimeSpan AgeOfLoan
            {
                get
                {
                    return this.LoanReleaseDate.HasValue ? DateTime.Now - this.LoanReleaseDate.Value : new TimeSpan();
                }
            }

            public PartyRole PartyRole { get; set; }
            public LoanAccount LoanAccount { get; set; }
            public FinancialProduct Product { get; set; }
            public AgreementItem AgreementItem { get; set; }
            public LoanAccountStatu LoanStatus { get; set; }

            public LoanListModel()
            {

            }

            public LoanListModel(LoanAccount loanAccount)
            {
                //this.LoanId = loanAccount.FinancialAccountId; //
                //this.LoanReleaseDate = loanAccount.LoanReleaseDate; //

                //var financialAccountRole = loanAccount.FinancialAccount.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId
                //                                && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);

                //var party = financialAccountRole.PartyRole.Party; //
                //this.Name = Person.GetPersonFullName(party);

                //var agreement = loanAccount.FinancialAccount.Agreement;
                //var agreementItem = agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
                //this.InterestComputationMode = agreementItem.InterestComputationMode;//

                //var financialAccountProduct = loanAccount.FinancialAccount.FinancialAccountProducts.SingleOrDefault(entity => entity.EndDate == null);
                //this.LoanProduct = financialAccountProduct.FinancialProduct.Name;//

                //var loanAccountStatus = loanAccount.LoanAccountStatus.SingleOrDefault(entity => entity.IsActive == true);
                //this.Status = loanAccountStatus.LoanAccountStatusType.Name;//
            }
        }

        private class CustomerWithLoanModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }

        }
    }
}