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

namespace LendingApplication.Applications.CustomerUseCases
{
    public partial class AllowedCustomersPickList : ActivityPageBase
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

            DataSource dataSource = new DataSource();
            ////dataSource.Name = "";
            dataSource.SearchString = txtSearchKey.Text;
            dataSource.LogInId = int.Parse(hdnLogInId.Value.ToString());

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnLogInId.Value = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
            this.LogInId = hdnLogInId.Value.ToString();
        }

        private class AllowedCustomersModel 
        {
            public int PartyID { get; set; }
            public int PartyRoleID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string RoleType { get; set; }
        }

        public enum SearchBy
        {
            Name = 1,
            None = -1
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            //using (var context = new BusinessModelEntities())
            //{
            //    RowSelectionModel sm = this.PageGridPanelSelectionModel;
            //    SelectedRowCollection rows = sm.SelectedRows;
            //    foreach (SelectedRow row in rows)
            //    {
            //        //int id = int.Parse(row.RecordID);
            //        //Customer customer = context.Customers.SingleOrDefault(entity => entity.ID == id);
            //        //if (customer != null) //other user may have del
            //        //    context.Customers.DeleteObject(customer);
            //    }
            //    context.SaveChanges();
            //}
            //this.PageGridPanel.DeleteSelected();
            //this.PageGridPanelStore.DataBind();
        }

        private class DataSource : IPageAbleDataSource<AllowedCustomersModel>
        {
            public string Name { get; set; }
            public int LogInId { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.LogInId = 0;
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

            private IQueryable<AllowedCustomersModel> CreateQuery(FinancialEntities context)
            {
                var partyTypeID = context.PartyTypes.SingleOrDefault(entity => entity.Name == PartyTypeEnums.Person);
                InitialDatabaseValueChecker.ThrowIfNull<PartyType>(partyTypeID);
                int id = this.LogInId;

                var query = from acl in context.AllowedCustomersViews
                            join p in context.PartyRoles on acl.PartyRoleId equals p.Id
                            join pt in context.Parties on p.PartyId equals pt.Id
                            where acl.PartyId != id && p.RoleTypeId != RoleType.CustomerType.Id
                                && (pt.PartyRoles.Select(entity => entity.PartyRoleType.RoleType.Name).Contains(RoleType.CustomerType.Name) == false
                                        && p.EndDate == null)
                            select new AllowedCustomersModel()
                            {
                                PartyRoleID = acl.PartyRoleId,
                                PartyID = acl.PartyId,
                                Name = acl.Owner,
                                Address = acl.Address,
                                RoleType = p.PartyRoleType.RoleType.Name
                            };

                        query = query.Where(entity => entity.Name.Contains(SearchString));


                return query;
            }

            public override List<AllowedCustomersModel> SelectAll(int start, int limit, Func<AllowedCustomersModel, string> orderBy)
            {
                List<AllowedCustomersModel> collection;
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