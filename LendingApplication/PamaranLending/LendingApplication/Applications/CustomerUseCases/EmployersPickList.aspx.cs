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
    public partial class EmployersPickList : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            //dataSource.Name = "";

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                List<PartyType> partyType = new List<PartyType>();
                using (var context = new FinancialEntities())
                {
                    partyType = context.PartyTypes.ToList();
                }
                FilterStore.DataSource = partyType;
                FilterStore.DataBind();
            }
        }

        private class EmployersListModel
        {
            public int EmployerID { get; set; }

            public string OrgName { get; set; }
            public string PartyType { get; set; }
            public string Names { get; set; }
            public string Addresses { get; set; }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            int count = 0;
            using (var context = new FinancialEntities())
            {
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    var query = context.PartyRelationships.Where(entity => (entity.SecondPartyRoleId == id) ||
                                entity.SecondPartyRoleId == id && entity.EndDate == null);
                    count = query.Count();

                    if (count == 0)
                    {
                        PartyRole partyRole = context.PartyRoles.SingleOrDefault(entity => entity.Id == id && entity.EndDate == null);

                        if (partyRole != null)
                        { //other user may have del
                            partyRole.EndDate = today;
                            X.Msg.Alert("Status", "The selected employer record is successfully deleted.").Show();
                        }
                        else
                        {
                            throw new AccessToDeletedRecordException("The employer has been deleted by another user.");
                        }
                    }
                    else
                    {
                        X.Msg.Alert("Alert", "The selected employer record is currently used by one or more customer records. Please select another employer record to delete.").Show();
                    }
                }
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public bool CheckIfDeletable(int id)
        {
            int count = 0;
            using (var context = new FinancialEntities())
            {
                var query = context.PartyRelationships.Where(entity => (entity.FirstPartyRoleId == id) || 
                    entity.FirstPartyRoleId == id && entity.EndDate == null);
                count = query.Count();

            }

            return count == 0;
        }

        [DirectMethod]
        public bool CheckBeforeAdd(int id)
        {
            int count = 0;
            using (var context = new FinancialEntities())
            {
                var query = context.PartyRelationships.Where(entity => (entity.FirstPartyRoleId == id) ||
                    entity.FirstPartyRoleId == id && entity.EndDate == null);
                count = query.Count();
            }

            return count == 0;
        }

        public enum SearchBy
        {
            ID = 0,
            Name = 1,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<EmployersListModel>
        {
            public string Name { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
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

            private IQueryable<EmployersListModel> CreateQuery(FinancialEntities context)
            {
                var query = from empList in context.EmployersViewLists
                            select new EmployersListModel()
                            {
                                EmployerID = empList.EmployerID,
                                Names = empList.Name,
                                Addresses = empList.BusinessAddress,
                                PartyType = empList.PartyType
                            };


                query = query.Where(entity => entity.Names.Contains(SearchString));

                return query;
            }

            public override List<EmployersListModel> SelectAll(int start, int limit, Func<EmployersListModel, string> orderBy)
            {
                List<EmployersListModel> collection;
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