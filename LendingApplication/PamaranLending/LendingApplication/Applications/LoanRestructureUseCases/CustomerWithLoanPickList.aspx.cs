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

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class CustomerWithLoanPickList : ActivityPageBase
    {
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;
            //FilterBy filterBy = (FilterBy)cmbFilterBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            //dataSource.FilterBy = filterBy;
            dataSource.filterString2 = cmbFilterBy2.SelectedItem.Text;
            //dataSource.Name = "";
            dataSource.LogInId = int.Parse(hdnLogInId.Value.ToString());
            dataSource.SearchBy = searchBy;


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
            hdnLogInId.Value = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
            this.LogInId = hdnLogInId.Value.ToString();
            List<CustomerStatusType> cusStatType = new List<CustomerStatusType>();
            using (var context = new FinancialEntities())
            {
                cusStatType = context.CustomerStatusTypes.ToList();
            }

            FilterStore.DataSource = cusStatType;
            FilterStore.DataBind();
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
            //if (cmbFilterBy.SelectedItem.Text.Equals("Status"))
            //{
            //    cmbFilterBy2.EmptyText = "Status...";
            //    FilterStore.DataSource = cusStatType;
            //    //cmbFilterBy2.Show();
            //    //cmbFilterBy2.Text = "";
                
            //}
            //else if (cmbFilterBy.SelectedItem.Text.Equals("Party Type"))
            //{
            //    cmbFilterBy2.EmptyText = "Party Type...";
            //    FilterStore.DataSource = partyType;
            //    //cmbFilterBy2.Text = "";
                
            //    //cmbFilterBy2.Show();
            //}
            //else
            //{
            //    cmbFilterBy2.EmptyText = "All";
            //    cmbFilterBy2.Text = "";
                
            //    //cmbFilterBy2.Hide();
            //}

            FilterStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        public enum SearchBy
        {
            ID = 0,
            Name = 1,
            None = -1
        }

        public enum FilterBy
        {
            All = 0,
            Status = 1,
            PartyType = 2,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<CustomerWithLoan>
        {
            public string Name { get; set; }
            public string filterString2 { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public int LogInId { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.filterString2 = string.Empty;
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
                this.FilterBy = FilterBy.None;
                this.LogInId = 0;
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

            private IEnumerable<CustomerWithLoan> CreateQuery(FinancialEntities context)
            {
                var query = Party.GetAllCustomersWithDisbursedLoans(RoleType.OwnerFinancialType, this.LogInId);

                //switch (SearchBy)
                //{
                //    case SearchBy.ID:
                //        if (SearchString != "")
                //        {
                //            int id = int.Parse(SearchString);
                //            query = query.Where(entity => entity.CustomerID.Equals(id));
                //        }
                //        break;

                //    case SearchBy.Name:
                //        query = query.Where(entity => entity.Names.Contains(SearchString));
                //        break;

                //    case SearchBy.None:
                //        break;
                //    default:
                //        break;
                //}

                if (!string.IsNullOrWhiteSpace(filterString2) && filterString2 != "All")
                {
                    query = query.Where(entity => entity.Status.Equals(filterString2));
                }

                if (SearchString != null)
                {
                    query = query.Where(entity => entity.Names.Contains(SearchString));
                }


                return query;
            }

            public override List<CustomerWithLoan> SelectAll(int start, int limit, Func<CustomerWithLoan, string> orderBy)
            {
                List<CustomerWithLoan> collection;
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