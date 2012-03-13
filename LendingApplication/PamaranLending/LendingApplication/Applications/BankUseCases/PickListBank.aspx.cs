using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication
{
    public partial class PickListBank : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;
            FilterBy filterBy = (FilterBy)cmbFilter.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            // if (string.IsNullOrWhiteSpace(cmbFilter.Value.ToString()) == false)
            if (cmbFilter.SelectedItem.Text == "All") dataSource.FilterString = "";
            else dataSource.FilterString = cmbFilter.SelectedItem.Text;
            dataSource.FilterBy = filterBy;
            dataSource.SearchBy = searchBy;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name); //change to Name ang Branch tab:D
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);
            }
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }
        protected void cmbFilter_Select(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }
        private enum SearchBy
        {
            Name = 0,
            Branch = 1,
            Address = 2,
            None = -1
        }
        private enum FilterBy
        {

            Active = 0,
            Inactive = 1,
            None = -1
        }
        private class BankModel
        {
            public int Id { get; set; }
            public string BranchName { get; set; }
            public string Acronym { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Address { get; set; }
        }
        private class DataSource : IPageAbleDataSource<BankModel>
        {
            public string SearchString { get; set; }
            public string FilterString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public DataSource()
            {
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
            private IQueryable<BankModel> CreateQuery(FinancialEntities context)
            {
                var query = from p in context.BankViewLists
                            join bs in context.BankStatus on p.PartyRoleID equals bs.PartyRoleId
                            where bs.EndDate == null && bs.BankStatusTypeId == BankStatusType.ActiveType.Id
                            select new BankModel()
                            {
                                Id = p.PartyRoleID,
                                Name = p.Organization_Name,
                                Acronym = p.Acronym,
                                BranchName = p.Branch,
                                Status = p.Status,
                                Address = p.Address
                            };

                switch (SearchBy)
                {
                    case SearchBy.Name:
                        query = query.Where(entity => entity.Name.Contains(SearchString));
                        break;
                    case SearchBy.Branch:
                        query = query.Where(entity => entity.BranchName.Contains(SearchString));
                        break;
                    case SearchBy.Address:
                        query = query.Where(entity => entity.Address.Contains(SearchString));
                        break;
                    case SearchBy.None:
                        break;
                    default:
                        break;
                }
                switch (FilterBy)
                {
                    case FilterBy.Active:
                        query = query.Where(entity => entity.Status.Equals(FilterString));
                        break;
                    case FilterBy.Inactive:
                        query = query.Where(entity => entity.Status.Equals(FilterString));
                        break;
                    case FilterBy.None:
                        break;
                    default:
                        break;
                }
                return query;
            }

            public override List<BankModel> SelectAll(int start, int limit, Func<BankModel, string> orderBy)
            {
                List<BankModel> collection;
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