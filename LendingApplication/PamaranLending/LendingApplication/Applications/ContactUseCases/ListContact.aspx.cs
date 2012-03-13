using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ContactUseCases
{
    public partial class ListContact : ActivityPageBase
    {
        private static FinancialEntities Context
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
            dataSource.SearchString = txtSearch.Text;
            dataSource.FilterBy = cmbFilterBy.SelectedItem.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<PartyType> partyTypes;
            using (var context = new FinancialEntities())
            {
                var query = context.PartyTypes;
                partyTypes = query.ToList();
            }
            storePartyType.DataSource = partyTypes;
            storePartyType.DataBind();

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
                {
                    btnOpen.Hidden = true;
                    btnNew.Hidden = true;
                    btnDelete.Hidden = true;
                    btnDeleteSeparator.Hidden = true;
                    btnOpenSeparator.Hidden = true;
                }

                string mode = Request.QueryString["mode"];
            }
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                DeleteContactInformation();
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }
        protected void btnOpen_Click(object sender, DirectEventArgs e)
        {

        }

        private void ViewSelectedContactRecord()
        {

        }

        [DirectMethod]
        public void DeleteContactInformation()
        {
            RowSelectionModel sm = this.PageGridPanelSelectionModel;
            SelectedRowCollection rows = sm.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                int partyRoleId = int.Parse(row.RecordID);
                var today = DateTime.Now;

                //retrieve partyRelationshipType from PartyRelationshipType
                var contactRelationshipType = Context.PartyRelTypes.SingleOrDefault(entity =>
                    entity.Name == PartyRelationTypesEnums.ContactRelationship);

                //retrieve partyRole from Party Role
                var lendingInstitutionPartyRole = Context.PartyRoles.SingleOrDefault(entity =>
                    entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);

                //update data in table
                var partyRelationship = Context.PartyRelationships.First(entity =>
                    entity.FirstPartyRoleId == partyRoleId
                    && entity.SecondPartyRoleId == lendingInstitutionPartyRole.Id
                    && entity.PartyRelTypeId == contactRelationshipType.Id
                    && entity.EndDate == null);

                var partyRole = Context.PartyRoles.SingleOrDefault(entity =>
                    entity.Id == partyRoleId && entity.EndDate == null);

                if (partyRelationship != null)
                    partyRelationship.EndDate = today;

                if (partyRole != null)
                    partyRole.EndDate = today;
            }
        }

        [DirectMethod]
        public bool CanDeleteContact(int PartyRoleId)
        {
            var forExPerson = from fex in Context.ForeignExchanges
                        where fex.ProcessedToPartyRoleId == PartyRoleId
                        select fex.PartyRole1;

            var disbursementPerson = from p in Context.Payments
                                     where (p.SpecificPaymentTypeId == SpecificPaymentType.EncashmentType.Id 
                                     || p.SpecificPaymentTypeId == SpecificPaymentType.RediscountingType.Id)
                                     && p.ProcessedToPartyRoleId == PartyRoleId
                                     select p.PartyRole1;

            if ((forExPerson.Count() != 0) || (disbursementPerson.Count() != 0)) return false;

            return true;
        }

        private class DataSource : IPageAbleDataSource<ContactViewList>
        {
            public string SearchString { get; set; }

            public string FilterBy { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        count = context.ContactViewLists.Where(entity => entity.PartyType.Contains(FilterBy) && entity.Name.Contains(SearchString)).Count();
                    }

                    return count;
                }
            }

            public override List<ContactViewList> SelectAll(int start, int limit, Func<ContactViewList, string> orderBy)
            {
                List<ContactViewList> collection;
                using (var context = new FinancialEntities())
                {
                    var query = context.ContactViewLists.Where(entity => entity.PartyType.Contains(FilterBy) && entity.Name.Contains(SearchString)).OrderBy(orderBy).Skip(start).Take(limit);
                    collection = query.ToList();
                }
                
                return collection;
            }
        }
    }
}