using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.UserAccountsUseCases
{
    public partial class ListUserAccounts : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                return allowed;
            }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.FilterByStatus = cmbFilterByStatus.SelectedItem.Text;
            dataSource.FilterByUserType = cmbFilterByUserType.SelectedItem.Text;
            dataSource.SearchBy = cmbSearchBy.SelectedItem.Text;
            dataSource.InputString = txtSearchInput.Text;
            dataSource.LoginInfo = this.LoginInfo;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.NameOfUser);
            this.PageGridPanelStore.DataBind();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            var userAccountStatusTypes = ObjectContext.UserAccountStatusTypes;
            userAccountStatusTypes.ToList();
            strFilterByStatus.DataSource = userAccountStatusTypes;
            strFilterByStatus.DataBind();

            var userTypes = ObjectContext.UserAccountTypes as IQueryable<UserAccountType>;
            if (this.LoginInfo.UserType == UserAccountType.SuperAdmin.Name)
            {
                userTypes = userTypes.Where(ut => ut.Id != UserAccountType.SuperAdmin.Id);
            }
            else
            {
                userTypes = userTypes.Where(ut => ut.Id != UserAccountType.SuperAdmin.Id && ut.Id != UserAccountType.Admin.Id);
            }

            strFilterByUserType.DataSource = userTypes.ToList();
            strFilterByUserType.DataBind();

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                btnActivate.Disabled = true;
                btnDeactivate.Disabled = true;
            }
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanelStore.DataBind();
        }

        protected void btnActivate_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            //var rsm = this.PageGridPanelSelectionModel.SelectedRow;
            //int selectedUserAccountId = int.Parse(rsm.RecordID);

            var rsm = this.PageGridPanelSelectionModel.SelectedRows;

            foreach (var item in rsm)
            {
                int id = int.Parse(item.RecordID);
                var userAccount = UserAccount.GetById(id);
                UserAccountStatu.CreateOrUpdateCurrent(userAccount, UserAccountStatusType.ActiveType, now); 
            }
            
            ObjectContext.SaveChanges();
        }

        protected void btnDeactivate_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            //var rsm = this.PageGridPanelSelectionModel.SelectedRow;
            //int selectedUserAccountId = int.Parse(rsm.RecordID);
            var rsm = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (var item in rsm)
            {
                int id = int.Parse(item.RecordID);
                var userAccount = UserAccount.GetById(id);
                UserAccountStatu.CreateOrUpdateCurrent(userAccount, UserAccountStatusType.InactiveType, now);
            }

            ObjectContext.SaveChanges();
        }


        public class DataSource : IPageAbleDataSource<UserAccountViewModel>
        {
            public string SearchBy { get; set; }
            public string FilterByStatus { get; set; }
            public string FilterByUserType { get; set; }
            public string InputString { get; set; }
            public LoginSessionInformation LoginInfo { get; set; }

            public DataSource()
            {

            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    count = CreateQuery().Count();
                    return count;
                }
            }

            public override List<UserAccountViewModel> SelectAll(int start, int limit, Func<UserAccountViewModel, string> orderBy)
            {
                List<UserAccountViewModel> userAccounts;
                {
                    var query = CreateQuery().OrderBy(orderBy).Skip(start).Take(limit);
                    userAccounts = query.ToList();
                }

                return userAccounts;
            }

            public IEnumerable<UserAccountViewModel> CreateQuery()
            {
                var query = UserAccount.AllInModel().Where(ua=>ua.Username != LoginInfo.Username);

                if (LoginInfo.UserType != UserAccountType.SuperAdmin.Name)
                    query = query.Where(ua => ua.UserAccountTypeId == UserAccountType.Accountant.Id || ua.UserAccountTypeId == UserAccountType.Teller.Id);

                //if (FilterBy == "All")
                //{
                //    FilterBy = string.Empty;
                //}

                switch (SearchBy)
                {
                    case "Name of User":
                        query = query.Where(entity => entity.NameOfUser.ToLower().Contains(InputString.ToLower()) && entity.UserAccountStatus.Contains(FilterByStatus) && entity.UserAccountType.Contains(FilterByUserType));
                        break;

                    case "Username":
                        query = query.Where(entity => entity.Username.ToLower().Contains(InputString.ToLower()) && entity.UserAccountStatus.Contains(FilterByStatus) && entity.UserAccountType.Contains(FilterByUserType));
                        break;

                    default:
                        query = query.Where(entity => entity.UserAccountStatus.Contains(FilterByStatus) && entity.UserAccountType.Contains(FilterByUserType));
                        break;
                }

                return query;
            }
        }
    }
}