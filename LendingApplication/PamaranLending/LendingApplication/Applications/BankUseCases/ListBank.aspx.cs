using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.BankUseCases
{
    public partial class ListBank : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;
            FilterBy filterBy = (FilterBy)cmbFilter.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            if (cmbFilter.SelectedItem.Text == "All") dataSource.FilterString = "";
            else dataSource.FilterString = cmbFilter.SelectedItem.Text;
            dataSource.FilterBy = filterBy;
            dataSource.SearchBy = searchBy;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.OrganizationName); //change to Name ang Branch tab:D
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
                //{
                //    btnDeactivate.Hidden = true;
                //    btnActivate.Hidden = true;
                //    btnDelete.Hidden = true;
                //    btnAdd.Hidden = true;
                //    btnActivateSeparator.Hidden = true;
                //    btnDeactivateSeparator.Hidden = true;
                //    btnOpenSeparator.Hidden = true;
                //    btnDeleteSeparator.Hidden = true;
                //}
            }

        }

        protected void btnDeactivate_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                var today = DateTime.Now;
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                BankStatu newbankStatus = new BankStatu();
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    var bank = context.Banks.SingleOrDefault(entity => entity.PartyRoleId == id);
                    if (bank != null)
                    {
                        var oldBankStatus = context.BankStatus.SingleOrDefault(entity => entity.PartyRoleId == bank.PartyRoleId && entity.EndDate == null);
                        if (oldBankStatus != null)
                        {
                            oldBankStatus.EndDate = today;
                            var bankStatusType = context.BankStatusTypes.SingleOrDefault(entity => entity.Name == BankStatusTypeEnum.Inactive);
                            var bankRoleType = context.RoleTypes.SingleOrDefault(entity => entity.Name == RoleTypeEnums.Bank);
                            newbankStatus.PartyRoleId = bank.PartyRoleId;
                            newbankStatus.BankStatusType = bankStatusType;
                            newbankStatus.EffectiveDate = today;
                            context.BankStatus.AddObject(newbankStatus);
                          
                        }
                    }
                }
                context.SaveChanges();
            }
            
              this.PageGridPanelStore.DataBind();
        }

        protected void btnActivate_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                var today = DateTime.Now;
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                BankStatu newbankStatus = new BankStatu();
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    var bank = context.Banks.SingleOrDefault(entity => entity.PartyRoleId == id);
                    if (bank != null)
                    {
                        var oldBankStatus = context.BankStatus.SingleOrDefault(entity => entity.PartyRoleId == bank.PartyRoleId && entity.EndDate == null);
                        if (oldBankStatus != null)
                        {
                            oldBankStatus.EndDate = today;
                            var bankStatusType = context.BankStatusTypes.SingleOrDefault(entity => entity.Name == BankStatusTypeEnum.Active);
                            newbankStatus.PartyRoleId = bank.PartyRoleId;
                            newbankStatus.BankStatusType = bankStatusType;
                            newbankStatus.EffectiveDate = today;
                            context.BankStatus.AddObject(newbankStatus);
                        }

                    }
                }
                context.SaveChanges();
            }

           this.PageGridPanelStore.DataBind();
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
                    PartyRole partyRole = context.PartyRoles.SingleOrDefault(entity => entity.Id == id);
                    if (partyRole != null)
                    {
                        partyRole.EndDate = today;
                    }
                    var bankStatu = from b in context.BankStatus where b.PartyRoleId == partyRole.Id select b;
                   foreach (var b in bankStatu)
                   {
                       context.BankStatus.DeleteObject(b);
                   }
                    Bank deleteObject = context.Banks.SingleOrDefault(entity => entity.PartyRoleId == id);
                    if(deleteObject!= null)
                    context.Banks.DeleteObject(deleteObject);
                
                }
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public bool CanDeleteBank(int id)
        {
            int count = 0;
            using (var context = new FinancialEntities())
            {
                count = context.Cheques.Where(entity => entity.BankPartyRoleId == id).Count();
                count += context.BankAccounts.Where(entity => entity.BankPartyRoleId == id).Count();
            }
            return count == 0;
        }

        [DirectMethod]
        public bool CanDeactivateBank(int id)
        {
            int count = 0;

            using (var context = new FinancialEntities())
            {
                var inactive = context.BankStatusTypes.SingleOrDefault(entity => entity.Name == BankStatusTypeEnum.Inactive);
                count = context.BankStatus.Where(entity => entity.PartyRoleId == id && entity.BankStatusTypeId == inactive.Id && entity.EndDate == null).Count();
            }
            return count == 0;
        }
        [DirectMethod]
        public bool CanActivateBank(int id)
        {
            int count = 0;

            using (var context = new FinancialEntities())
            {
                var active = context.BankStatusTypes.SingleOrDefault(entity => entity.Name == BankStatusTypeEnum.Active);
                count = context.BankStatus.Where(entity => entity.PartyRoleId == id && entity.BankStatusTypeId == active.Id && entity.EndDate == null).Count();
            }
            return count == 0;
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DataBind();
        }

        protected void cmbFilter_Select(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        public enum SearchBy
        {
            Name = 0,
            Branch = 1,
            Address = 2,
            None = -1
        }
        public enum FilterBy
        {

            Active=0,
            Inactive = 1,
            None = -1
        }
        private class BankModel
        {
            public int Id{ get; set; }
            public string BranchName { get; set; }
            public string Acronym { get; set; }
            public string OrganizationName { get; set; }
            public string Status { get; set; }
            public string Address{ get; set; }
        }
        private class DataSource : IPageAbleDataSource<BankModel>
        {
            public string SearchString {get; set; }
            public string FilterString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public DataSource()
            {
                this.SearchString = string.Empty;
                this.SearchBy = ListBank.SearchBy.None;
                this.FilterBy = ListBank.FilterBy.None;
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
                                    Acronym = p.Acronym,
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