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
    public partial class ListCustomers : ActivityPageBase
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
            //SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;
            //FilterBy filterBy = (FilterBy)cmbFilterBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            //dataSource.FilterBy = filterBy;
            dataSource.filterString2 = cmbFilterBy2.SelectedItem.Text;
            dataSource.LogInId = int.Parse(hdnLogInId.Value.ToString());
            //dataSource.Name = "";
            //dataSource.SearchBy = searchBy;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
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
            //    FilterStore.DataSource = cusStatType;
            //    cmbFilterBy2.Text = "";
            //    //cmbFilterBy2.Show();
            //}
            //else if (cmbFilterBy.SelectedItem.Text.Equals("Party Type"))
            //{
            //    FilterStore.DataSource = partyType;
            //    cmbFilterBy2.Text = "";
            //    //cmbFilterBy2.Show();
            //}
            //else
            //{
            //    this.PageGridPanel.DataBind();
            //    cmbFilterBy2.Text = "";
            //    //cmbFilterBy2.Hide();
            //}

            FilterStore.DataBind();
        }

        protected void OnSelectedIndexChange(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
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

        private class CustomersListModel
        {
            public int CustomerID { get; set; }
            public int PartyId { get; set; }
            public string Addresses { get; set; }
            public string Names { get; set; }
            public string OrgName { get; set; }
            public string PartyType { get; set; }
            public string Status { get; set; }
            public string CustomerType { get; set; }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            using (var context = new FinancialEntities())
            {
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    var customerView = context.CustomerViewLists.FirstOrDefault(entity => entity.CustomerId == id && entity.Status == "New");
                    //if customer exists and has a status of "New"
                    if (customerView != null)
                    {
                        //check if customer-lender relationship exists
                        PartyRole lending = context.PartyRoles.FirstOrDefault(entity => entity.PartyRoleType.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
                        PartyRelationship party = context.PartyRelationships.FirstOrDefault(entity => entity.FirstPartyRoleId == customerView.CustomerId && entity.SecondPartyRoleId == lending.Id && entity.EndDate == null);
                        
                        //if customer-lender party relationship exists
                        if (party != null) //other user may have deleted the record
                        {
                            party.EndDate = today;
                            X.Msg.Alert("Status", "The selected customer record is successfully deleted.").Show();
                        }
                        else //else if customer-lender relationship has ended (meaning the customer has been deleted)
                        {
                            throw new AccessToDeletedRecordException("The customer has been deleted by another user.");
                        }
                    }
                    else //else if customer status is not equal to "New"
                    {
                        customerView = context.CustomerViewLists.FirstOrDefault(entity => entity.CustomerId == id && entity.Status != "New");
                        if (customerView != null)
                            X.Msg.Alert("Alert", "Only customers with a status of ‘New’ can be deleted.").Show();
                    }
                }
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        //CustomerListModel Datasource
        private class DataSource : IPageAbleDataSource<CustomersListModel>
        {
            
            public string Name { get; set; }
            public string filterString2 { get; set; }
            public string SearchString { get; set; }
            public int LogInId { get; set; }
            //public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.filterString2 = string.Empty;
                this.SearchString = string.Empty;
                //this.SearchBy = SearchBy.None;
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

            private IQueryable<CustomersListModel> CreateQuery(FinancialEntities context)
            {
                int id = this.LogInId;
               
                //var lendingRoleId = context.PartyRoles.SingleOrDefault(entity => entity.PartyRoleType.RoleType.Name == RoleTypeEnums.LendingInstitution && entity.EndDate == null);

                var query = from cust in context.CustomerViewLists
                            join custCat in context.CustomerCategories on cust.CustomerId equals custCat.PartyRoleId
                            where custCat.EndDate == null
                            //&& cust.PartyId != id
                            select new CustomersListModel()
                            {
                                CustomerID = cust.CustomerId,
                                PartyType = cust.PartyType,
                                Addresses = cust.Address,
                                Names =  cust.Name,
                                Status = cust.Status,
                                PartyId = cust.PartyId,
                                CustomerType = custCat.CustomerCategoryType1.Name
                            };

                //switch (SearchBy)
                //{
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

                //switch (FilterBy)
                //{
                //    case FilterBy.All:
                //        break;
                //    case FilterBy.Status:
                //            if(filterString2 != "All")
                //            query = query.Where(entity => entity.Status.Contains(filterString2));
                //        break;

                //    case FilterBy.PartyType:
                //        if (filterString2 != "All")
                //        query = query.Where(entity => entity.PartyType.Contains(filterString2));
                //        break;

                //    case FilterBy.None:
                //        break;
                //    default:
                //        break;
                //}

                return query;
            }

            public override List<CustomersListModel> SelectAll(int start, int limit, Func<CustomersListModel, string> orderBy)
            {
                List<CustomersListModel> collection;
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