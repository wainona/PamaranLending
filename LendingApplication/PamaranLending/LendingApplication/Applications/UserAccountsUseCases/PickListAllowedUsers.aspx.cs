using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.UserAccountsUseCases
{
    public partial class PickListAllowedUsers : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                return allowed;
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.Name = txtSearch.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSelect_Click(object sender, DirectEventArgs e)
        {

        }

        private class AllowedUsersModel
        {
            public int PartyId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }

        private class DataSource : IPageAbleDataSource<AllowedUsersModel>
        {
            public string Name { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    count = CreateQuery().Count();
                    return count;
                }
            }

            private IQueryable<AllowedUsersModel> CreateQuery()
            {
                var query = from uv in ObjectContext.UsersViewLists
                            select new AllowedUsersModel
                            {
                                PartyId = uv.PartyId,
                                Name = uv.Name,
                                Address = uv.Address
                            };

                query = query.Where(entity => entity.Name.Contains(Name));
                return query;
            }

            public override List<AllowedUsersModel> SelectAll(int start, int limit, Func<AllowedUsersModel, string> orderBy)
            {
                List<AllowedUsersModel> allowedUsers;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery().OrderBy(entity => entity.PartyId);
                    allowedUsers = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return allowedUsers;
            }
        }
    }
}