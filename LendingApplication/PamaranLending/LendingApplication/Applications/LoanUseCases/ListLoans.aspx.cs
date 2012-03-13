using System;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.LoanUseCases
{
    public partial class ListLoans : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.SelectedName = cmbSearchCustomer.SelectedItem.Text;
            dataSource.FilterBy = cmbFilterByStatus.SelectedItem.Text;
            if (dtfFrom.SelectedValue != null)
                dataSource.MinDate = dtfFrom.SelectedDate;
            if (dtfTo.SelectedValue != null)
                dataSource.MaxDate = dtfTo.SelectedDate;
            dataSource.SearchString = cmbLoanProducts.SelectedItem.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.FilterLoansOfCurrentUser(e.Start, e.Limit, entity => entity.Name, this.LoginInfo.UserId);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var loanStatusTypes = ObjectContext.LoanAccountStatusTypes;
                strFilterByStatus.DataSource = loanStatusTypes.ToList();
                strFilterByStatus.DataBind();

                var loanProducts = ObjectContext.FinancialProducts;
                strLoanProducts.DataSource = loanProducts.ToList();
                strLoanProducts.DataBind();

                DateTime now = DateTime.Now;
                dtfTo.MaxDate = now;
                dtfFrom.MaxDate = now;

                this.PageGridPanelStore.DataBind();
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
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanelStore.DataBind();
        }

        protected void SearchCustomerWithLoan(object sender, DirectEventArgs e)
        {
            var searchString = cmbSearchCustomer.SelectedItem.Text;
            var query = QueryCustomerWithLoan().Where(entity => entity.Name.Contains(searchString));
            List<CustomerWithLoanModel> customers = query.ToList();

            strCustomersWithLoan.DataSource = customers;
            strCustomersWithLoan.DataBind();
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

        protected class DataSource : IPageAbleDataSource<LoanListModel>
        {
            public string SelectedName { get; set; }
            public string SearchString { get; set; }
            public string FilterBy { get; set; }
            public DateTime? MinDate { get; set; }
            public DateTime? MaxDate { get; set; }

            public DataSource()
            {
                this.SelectedName = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery();
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IEnumerable<LoanListModel> CreateQuery()
            {
                DateTime now = DateTime.Now;
                
                var query = from la in ObjectContext.LoanAccounts
                            join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                            join far in ObjectContext.FinancialAccountRoles on la.FinancialAccountId equals far.FinancialAccountId// financialAccountRole
                            join pr in ObjectContext.PartyRoles on far.PartyRoleId equals pr.Id
                            join fa in ObjectContext.FinancialAccounts on la.FinancialAccountId equals fa.Id
                            join ag in ObjectContext.Agreements on fa.AgreementId equals ag.Id//agreement
                            join ai in ObjectContext.AgreementItems on ag.Id equals ai.AgreementId
                            join fap in ObjectContext.FinancialAccountProducts on la.FinancialAccountId equals fap.FinancialAccountId//financialAccountProduct;
                            join fp in ObjectContext.FinancialProducts on fap.FinancialProductId equals fp.Id
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

                var result = query.ToList().AsEnumerable();
                //var result = loanListModel.AsEnumerable();

                result = result.Where(entity => entity.Name.Contains(SelectedName) && entity.LoanProduct.ToLower().Contains(SearchString.ToLower()));

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
                    result = result.Where(entity => entity.LoanReleaseDate.Value >= MinDate.Value && entity.LoanReleaseDate.Value <= MaxDate.Value);
                }
                return result;
            }

            public List<LoanListModel> FilterLoansOfCurrentUser(int start, int limit, Func<LoanListModel, string> orderBy, int userAccountId)
            {
                var employee = UserAccount.GetAssociatedEmployee(userAccountId);
                var partyId = employee.PartyRole.Party.Id;

                List<LoanListModel> collection;
                var query = CreateQuery();
                query = query.Where(entity => entity.PartyRole.PartyId != partyId);
                collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();


                return collection;
            }

            public override List<LoanListModel> SelectAll(int start, int limit, Func<LoanListModel, string> orderBy)
            {
                List<LoanListModel> collection;
                
                    var query = CreateQuery();
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                

                return collection;
            }
        }

        protected class LoanListModel
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
            public string _LoanReleaseDate
            {
                get
                {
                    if (this.LoanReleaseDate.HasValue)
                        return this.LoanReleaseDate.Value.ToString("yyyy-MM-dd");
                    else
                        return string.Empty;
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
            public decimal LoanAmount
            {
                get
                {
                    return this.LoanAccount.LoanAmount;
                }
            }
            public decimal LoanBalance
            {
                get
                {
                    return this.LoanAccount.LoanBalance;
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

        protected class CustomerWithLoanModel
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }

        }
    }
}