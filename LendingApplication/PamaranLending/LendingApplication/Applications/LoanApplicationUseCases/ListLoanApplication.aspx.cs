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

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class ListLoanApplication : ActivityPageBase
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
            SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;
            FilterBy filterBy = (FilterBy)cmbFilterBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;
            dataSource.FilterBy = filterBy;
            dataSource.FilterString = cmbFilter.SelectedItem.Text;
            dataSource.FromDate = dtRangeFrom.SelectedDate;
            dataSource.ToDate = dtRangeTo.SelectedDate.AddDays(1);
            dataSource.LogInId = int.Parse(hdnLogInId.Value.ToString());

            e.Total = dataSource.Count;
            this.strPageGridPanel.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.strPageGridPanel.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //dtRangeFrom.MaxDate = DateTime.Now;
                //dtRangeTo.MaxDate = DateTime.Now;
                dtRangeTo.SelectedDate = DateTime.Now.AddDays(1);
            }
            hdnLogInId.Value = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
            this.LogInId = hdnLogInId.Value.ToString();
        }

        protected void OnSelectedItemChange_Click(object sender, DirectEventArgs e)
        {
            cmbFilter.Clear();
            if (cmbFilterBy.SelectedItem.Text == "All")
            {
                this.strPageGridPanel.DataBind();
            }
            else if (cmbFilterBy.SelectedItem.Text == "Collateral Requirement")
            {
                //var query = Context.ProductFeatures.Where(entity => entity.ProductFeatureCategory.Name == "Collateral Requirement");
                //product = query.ToList();
                //strFilter.DataSource = product;

                var products = ProductFeature.All(ProductFeatureCategory.CollateralRequirementType);
                strFilter.DataSource = products;

            } else if (cmbFilterBy.SelectedItem.Text == "Status")
            {
                var cmbStatus = ObjectContext.LoanApplicationStatusTypes.Where(entity => entity.Name != "Pending: Endorsement");
                //LoanApplicationStatusType.All(LoanApplicationStatusType.PendingEndorsementType);
                strFilter.DataSource = cmbStatus;
                //var query1 = ObjectContext.LoanApplicationStatus.Where(entity => entity.LoanApplicationStatusType.Name != "Pending: Endorsement");
                //var status = query1.ToList();
                //strFilter.DataSource = status;
            } else {
            }
            strFilter.DataBind();
        }

        protected void OnSelectItem(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.strPageGridPanel.DataBind();
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.PageGridPanelSelectionModel;
            SelectedRowCollection rows = sm.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                int id = int.Parse(row.RecordID);
                if (CanDeleteLoanApplication(id))
                {
                    using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
                    {
                        LoanApplication.Delete(id);
                    }
                }
            }
            this.PageGridPanel.DeleteSelected();
            this.strPageGridPanel.DataBind();
        }

        public bool CanDeleteLoanApplication(int id)
        {
            bool result = false;
            var loanApplication = LoanApplication.GetById(id);
            LoanApplicationStatu status = LoanApplicationStatu.GetActive(loanApplication);
            if (status != null && status.LoanApplicationStatusType.Id == LoanApplicationStatusType.PendingApprovalType.Id)
            {
                int count = 0;
                result = count == 0;
            }
            return result;
        }

        [DirectMethod]
        public bool CanDeleteLoanApplication(int[] ids)
        {
            bool result = true;
            foreach (int id in ids)
            {
                result = CanDeleteLoanApplication(id);
                if (result == false)
                    break;
            }

            return result;
        }

        protected void SearchLoanApplication_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.strPageGridPanel.DataBind();
        }

        //protected void btnApprove_Click(object sender, DirectEventArgs e)
        [DirectMethod(ShowMask = true, Msg = "Updating loan application status...")]
        public bool Approve_Click()
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;
                RowSelectionModel row = this.PageGridPanelSelectionModel;
                SelectedRowCollection selected = row.SelectedRows;
                foreach (var rows in selected)
                {
                    int id = int.Parse(rows.RecordID);
                    var loanApplication = LoanApplication.GetById(id);
                    if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.PendingInFundingType))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        
        //protected void btnCancel_Click(object sender, DirectEventArgs e)
        [DirectMethod(ShowMask = true, Msg = "Updating loan application status...")]
        public bool Cancel_Click()
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;
                RowSelectionModel row = this.PageGridPanelSelectionModel;
                SelectedRowCollection selected = row.SelectedRows;
                foreach (var rows in selected)
                {
                    int id = int.Parse(rows.RecordID);
                    var loanApplication = LoanApplication.GetById(id);
                    var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;

                    if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.CancelledType) &&
                        LoanApplicationStatu.GetActive(loanApplication).StatusTypeId == LoanApplicationStatusType.PendingInFundingType.Id)
                    {   
                        var agreement = loanApplication.Application.Agreements.FirstOrDefault();
                        if (agreement != null && agreement.EndDate == null)
                        {
                            var voucher = ObjectContext.LoanDisbursementVcrs.SingleOrDefault(entity =>
                                entity.AgreementId == agreement.Id);
                            var am = ObjectContext.AmortizationSchedules.SingleOrDefault(entity =>
                                    entity.EndDate == null && entity.AgreementId == agreement.Id);
                            if (voucher != null)
                            {
                                DisbursementVcrStatu.ChangeStatus(voucher, DisbursementVcrStatusEnum.CancelledType, today);
                            }
                            if (am != null && am.EndDate == null)
                                am.EndDate = today;
                            agreement.EndDate = today;
                        }
                        LoanApplicationOperations.Cancel(loanApplication, today, currentUser);
                    }
                    else if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.CancelledType) &&
                        LoanApplicationStatu.GetActive(loanApplication).StatusTypeId == LoanApplicationStatusType.PendingApprovalType.Id)
                    {
                        LoanApplicationOperations.Cancel(loanApplication, today, currentUser);
                    }
                    else
                    {
                        return false;
                    }
                }
            } return true;
        }

        //protected void btnReject_Click(object sender, DirectEventArgs e)
        [DirectMethod(ShowMask = true, Msg = "Updating loan application status...")]
        public bool Reject_Click()
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;
                RowSelectionModel row = this.PageGridPanelSelectionModel;
                SelectedRowCollection selected = row.SelectedRows;
                foreach (var rows in selected)
                {
                    int id = int.Parse(rows.RecordID);
                    var loanApplication = LoanApplication.GetById(id);
                    if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.RejectedType))
                    {
                        var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                        LoanApplicationOperations.Reject(loanApplication, today, currentUser);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        //protected void btnClose_Click(object sender, DirectEventArgs e)
        [DirectMethod(ShowMask = true, Msg = "Updating loan application status...")]
        public bool Close_Click()
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;
                RowSelectionModel row = this.PageGridPanelSelectionModel;
                SelectedRowCollection selected = row.SelectedRows;
                foreach (var rows in selected)
                {
                    int id = int.Parse(rows.RecordID);
                    var loanApplication = LoanApplication.GetById(id);
                    if (LoanApplicationStatu.CanChangeStatusTo(loanApplication, LoanApplicationStatusType.ClosedType))
                    {
                        var currentUser = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                        LoanApplicationOperations.Close(loanApplication, today, currentUser);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #region DataSource
        public enum SearchBy
        {
            BorrowersName = 0,
            LoanProduct = 1,
            None = -1
        }

        public enum FilterBy
        {
            All = 0,
            CollateralRequirement = 1,
            Status = 2,
            None = -1
        }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        private class LoanApplicationModel
        {
            public int LoanApplicationId { get; set; }
            public string ApplicationDate
            {
                get
                {
                    return this._ApplicationDate.ToString("yyyy-MM-dd");//String.Format("{0:MMMM dd, yyyy}", this._ApplicationDate);
                }
            }
            public DateTime _ApplicationDate { get; set; }
            public string BorrowersName { get; set; }
            public string LoanProduct { get; set; }
            public string CollateralRequirement { get; set; }
            public string Status { get; set; }
        }

        private class DataSource : IPageAbleDataSource<LoanApplicationModel>
        {
            public string SearchString { get; set; }
            public string FilterString { get; set; }
            public SearchBy SearchBy { get; set; }
            public FilterBy FilterBy { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public int LogInId { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.FilterString = string.Empty;
                this.SearchBy = SearchBy.None;
                this.FilterBy = FilterBy.None;
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

            private IQueryable<LoanApplicationModel> CreateQuery()
            {
                int UserId = this.LogInId;
                var query = from la in ObjectContext.LoanApplicationViewLists
                            join lar in ObjectContext.LoanApplicationRoles on la.LoanApplicationId equals lar.ApplicationId
                            where lar.PartyRole.RoleTypeId == RoleType.BorrowerApplicationType.Id && lar.PartyRole.PartyId != UserId
                            orderby la.LoanApplicationId descending
                            select new LoanApplicationModel
                            {
                                LoanApplicationId = la.LoanApplicationId,
                                _ApplicationDate = la.ApplicationDate,
                                BorrowersName = la.BorrowersName,
                                LoanProduct = la.LoanProduct,
                                CollateralRequirement = la.CollateralRequirement,
                                Status = la.Status,
                            };

                switch (SearchBy)
                {
                    case SearchBy.BorrowersName:
                        query = query.Where(entity => entity.BorrowersName.Contains(SearchString));
                        break;

                    case SearchBy.LoanProduct:
                        query = query.Where(entity => entity.LoanProduct.Contains(SearchString));
                        break;

                    case SearchBy.None:
                        query = query.Where(entity => entity.BorrowersName.Contains(SearchString));
                        break;
                    default:
                        query = query.Where(entity => entity.BorrowersName.Contains(SearchString));
                        break;
                }

                switch (FilterBy)
                {
                    case FilterBy.CollateralRequirement:
                        query = query.Where(entity => entity.CollateralRequirement == FilterString);
                        break;
                    case FilterBy.Status:
                        query = query.Where(entity => entity.Status == FilterString);
                        break;
                    case FilterBy.All:
                        break;
                    case FilterBy.None:
                        break;
                    default:
                        break;
                }

                if (FromDate != null && ToDate != null)
                {
                    query = query.Where(entity => entity._ApplicationDate >= FromDate && entity._ApplicationDate <= ToDate);
                }

                return query;
            }

            public override List<LoanApplicationModel> SelectAll(int start, int limit, Func<LoanApplicationModel, string> orderBy)
            {
                List<LoanApplicationModel> loanApplication;
                var query = CreateQuery();
                loanApplication = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                return loanApplication;
            }
        }
        #endregion

    }
}