using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class CustomerCoBorrowerViewList : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                return allowed;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
               // txtUserID.Value = Request.QueryString["id"];
            }
            
        }
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;
            dataSource.UserID = int.Parse(txtUserID.Value.ToString());

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }
        public enum SearchBy
        {
            Name = 0,
            None = -1
        }

        private class CustomerCoBorrowerModel
        {
            public int PartyId { get; set; }
            public string Role { get; set; }
            public int PartyRoleId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }
        private class DataSource : IPageAbleDataSource<CustomerCoBorrowerModel>
        {
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public int UserID { get; set; }


            public DataSource()
            {
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

            private IQueryable<CustomerCoBorrowerModel> CreateQuery(FinancialEntities context)
            {
                
                var query = from cust in context.CustomerAndCoBorrowerListViews
                            where cust.PartyId != UserID
                            select new CustomerCoBorrowerModel()
                            {
                               PartyId = cust.PartyId,
                               PartyRoleId = cust.PartyRoleId,
                               Name = cust.Name,
                               Address = cust.Address,
                               Role = cust.Role
                            };
                 
                switch (SearchBy)
                {
                    case SearchBy.Name:
                        query = query.Where(entity => entity.Name.Contains(SearchString));
                        break;
                    case SearchBy.None:
                        break;
                    default:
                        break;
                }

                return query;
            }

            public override List<CustomerCoBorrowerModel> SelectAll(int start, int limit, Func<CustomerCoBorrowerModel, string> orderBy)
            {
                List<CustomerCoBorrowerModel> collection;
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