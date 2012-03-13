using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class PickListCoBorrower : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.hiddenCustomerId.Value = Request.QueryString["customerId"];//customerId is a partyRoleId
                this.hiddenCoborrowers.Value = Request.QueryString["coborrowers"];
                this.hiddenGuarantors.Value = Request.QueryString["guarantors"];
                this.hiddenUserId.Value = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;

                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);
            }
        }
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;
            dataSource.CustomerId = hiddenCustomerId.Text;
            dataSource.CoBorrowers = hiddenCoborrowers.Text;
            dataSource.Guarantors = hiddenGuarantors.Text;
            dataSource.LogInId = int.Parse(hiddenUserId.Value.ToString());
            dataSource.Initialize();

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name); //change to Name ang Branch tab:D
            this.PageGridPanelStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }
        private enum SearchBy
        {
            Name = 0,
            None = -1
        }
        
        private class DataSource : IPageAbleDataSource<AllowedCoBorrowerGuarantorModel>
        {
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public string CustomerId { get; set; }
            public string CoBorrowers { get; set; }
            public string Guarantors { get; set; }
            public int LogInId { get; set; }

            private List<int> excludeIds;

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
                this.LogInId = 0;
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

            private IEnumerable<AllowedCoBorrowerGuarantorModel> CreateQuery()
            {
                int id = this.LogInId;

                var result = AllowedCoBorrowerGuarantor.AllowedCoBorrowerGuarantors(excludeIds, id);

                switch (SearchBy)
                {
                    case SearchBy.Name:
                        result = result.Where(entity => entity.Name.Contains(SearchString));
                        break;
                    case SearchBy.None:
                        break;
                    default:
                        break;
                }

                return result;
            }

            public override List<AllowedCoBorrowerGuarantorModel> SelectAll(int start, int limit, Func<AllowedCoBorrowerGuarantorModel, string> orderBy)
            {
                List<AllowedCoBorrowerGuarantorModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery();
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }
                return collection;
            }

            internal void Initialize()
            {
                PartyRole partyRole = PartyRole.GetById(int.Parse(this.CustomerId));

                this.excludeIds = new List<int>();
                this.excludeIds.Add(partyRole.PartyId);

                string[] coborrowerParts = this.CoBorrowers.Split(',');
                string[] guaratorParts = this.Guarantors.Split(',');

                foreach (var item in coborrowerParts)
                {
                    int id;
                    if (int.TryParse(item, out id))
                        excludeIds.Add(id);
                }

                foreach (var item in guaratorParts)
                {
                    int id;
                    if (int.TryParse(item, out id))
                        excludeIds.Add(id);
                }
            }
        }
    }
    
}