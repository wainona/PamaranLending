using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class BankPickList : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;
            
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.OrganizationName); //change to Name ang Branch tab:D
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }

        public enum SearchBy
        {
            Name = 0,
            Branch = 1,
            Address = 2,
            None = -1
        }

        private class BankModel
        {
            public int Id{ get; set; }
            public string BranchName { get; set; }
            public string OrganizationName { get; set; }
            public string Status { get; set; }
            public string Address{ get; set; }
        }

        private class DataSource : IPageAbleDataSource<BankModel>
        {
            public string SearchString {get; set; }
            public SearchBy SearchBy { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.SearchBy = BankPickList.SearchBy.None;
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
                            select new BankModel()
                                {
                                    Id = p.PartyRoleID,
                                    OrganizationName = p.Organization_Name,
                                    BranchName = p.Branch,
                                    Status = p.Status,
                                    Address = p.Address

                                };

                switch (SearchBy)
                {
                    case SearchBy.Name:
                        query = query.Where(entity => entity.OrganizationName.Contains(SearchString)); 
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