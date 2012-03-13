using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DemandLetterUseCases
{
    public partial class DemandLetterList : ActivityPageBase
    {
        /**
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Accountant");
                allowed.Add("Admin");
                return allowed;
            }
        }
        **/
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.FilterBy = cmbFilterByStatus.SelectedItem.Text;
            dataSource.SearchString = txtSearch.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.CustomerName);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var demandLetterStatusTypes = ObjectContext.DemandLetterStatusTypes;
                strFilterByStatus.DataSource = demandLetterStatusTypes.ToList();
                strFilterByStatus.DataBind();

                DateTime now = DateTime.Now;

                cmbDemandLetterType.Text = "First Demand Letter";
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

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void DisplayLoans(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public void SendDemandLetter(int demandLetterId)
        {
            DateTime today = DateTime.Now;
            var demandLetter = ObjectContext.DemandLetters.SingleOrDefault(entity => entity.Id == demandLetterId);
            var demandLetterStatus = ObjectContext.DemandLetterStatus.SingleOrDefault(entity => entity.IsActive == true && entity.DemandLetterId == demandLetterId);

            demandLetter.DatePromisedToPay = null;
            demandLetter.DateSent = today;
            demandLetterStatus.IsActive = false;

            DemandLetterStatu newDemandLetterStatus = new DemandLetterStatu();
            newDemandLetterStatus.DemandLetterId = demandLetterId;
            newDemandLetterStatus.TransitionDateTime = today;
            newDemandLetterStatus.IsActive = true;

            if (demandLetterStatus.DemandLetterStatusTypeId == DemandLetterStatusType.RequireFirstDemandLetterType.Id)
            {
                newDemandLetterStatus.DemandLetterStatusTypeId = DemandLetterStatusType.FirstDemandLetterSentType.Id;
            }
            else if (demandLetterStatus.DemandLetterStatusTypeId == DemandLetterStatusType.RequireFinalDemandLetterType.Id)
            {
                newDemandLetterStatus.DemandLetterStatusTypeId = DemandLetterStatusType.FinalDemandLetterSentType.Id;
            }

            ObjectContext.DemandLetterStatus.AddObject(newDemandLetterStatus);
            ObjectContext.SaveChanges();
        }

        private class DataSource : IPageAbleDataSource<DemandLetterListModel>
        {
            public string Name { get; set; }
            public string SearchString { get; set; }
            public string FilterBy { get; set; }
            public DateTime? MinDate { get; set; }
            public DateTime? MaxDate { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    var query = CreateQuery();
                    count = query.Count();
                    

                    return count;
                }
            }

            private IEnumerable<DemandLetterListModel> CreateQuery()
            {
                DateTime now = DateTime.Now;

                var query = from dl in ObjectContext.DemandLetters
                            join la in ObjectContext.LoanAccounts on dl.FinancialAccountId equals la.FinancialAccountId
                            join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                            join dls in ObjectContext.DemandLetterStatus on dl.Id equals dls.DemandLetterId
                            where dls.IsActive == true && las.IsActive == true && (las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id || las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                            select new DemandLetterListModel()
                            {
                                Id = dl.Id,
                                LoanAccount = la,
                                LoanAccountId = la.FinancialAccountId,
                                _DemandLetterStatus = dls
                            };


                var result = query.ToList().AsEnumerable();

                result = result.Where(entity => entity.CustomerName.ToLower().Contains(SearchString.ToLower()));

                result = result.Where(entity => entity.DemandLetterStatus.Contains(FilterBy));

                return result;
            }

            public override List<DemandLetterListModel> SelectAll(int start, int limit, Func<DemandLetterListModel, string> orderBy)
            {
                List<DemandLetterListModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery();
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return collection;
            }
        }

        private class DemandLetterListModel
        {
            public int Id { get; set; }
            public string CustomerName
            {
                get
                {
                    var financialAccountRole = this.LoanAccount.FinancialAccount.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == this.LoanAccount.FinancialAccountId
                                                && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);

                    var party = financialAccountRole.PartyRole.Party;
                    return party.Name;
                }
            }
            public int LoanAccountId { get; set; }
            public string DemandLetterStatus { 
                get 
                {
                    return this._DemandLetterStatus.DemandLetterStatusType.Name;
                } 
            }

            public DemandLetterStatu _DemandLetterStatus { get; set; }
            public LoanAccount LoanAccount { get; set; }
        }
    }
}