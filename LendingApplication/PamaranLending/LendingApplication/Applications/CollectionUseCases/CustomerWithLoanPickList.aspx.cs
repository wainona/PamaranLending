using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using LendingApplication.Applications;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class CustomerWithLoanPickList : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            dataSource.SearchBy = searchBy;
            dataSource.UserId = int.Parse(txtUserID.Value.ToString());


            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void OnSelectedIndexChange(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
            }
        }

        protected void OnChange(object sender, DirectEventArgs e)
        {
            List<PartyType> partyType = new List<PartyType>();
            List<CustomerStatusType> cusStatType = new List<CustomerStatusType>();
            using (var context = new FinancialEntities())
            {
                partyType = context.PartyTypes.ToList();
                cusStatType = context.CustomerStatusTypes.ToList();
            }

            FilterStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        public enum SearchBy
        {
            Name = 0,
            None = -1
        }

        public enum FilterBy
        {
            All = 0,
            Status = 1,
            PartyType = 2,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<PersonWithLoan>
        {
            public string Name { get; set; }
            public string filterString2 { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public int UserId { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.filterString2 = string.Empty;
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
                this.FilterBy = FilterBy.None;
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

            private IEnumerable<PersonWithLoan> CreateQuery(FinancialEntities context)
            {
                var lendingRoleId = context.PartyRoles.SingleOrDefault(entity => entity.PartyRoleType.RoleType.Name == RoleTypeEnums.LendingInstitution && entity.EndDate == null);
                var query = Party.GetAllCustomersWithLoan(this.UserId);

                switch (SearchBy)
                {
                    case SearchBy.Name:
                        query = query.Where(entity => entity.Names.ToLower().Contains(SearchString));
                        break;

                    case SearchBy.None:
                        break;
                    default:
                        break;
                }

                return query;
            }

            public override List<PersonWithLoan> SelectAll(int start, int limit, Func<PersonWithLoan, string> orderBy)
            {
                List<PersonWithLoan> collection;
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