using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.UserAccountUseCases
{
    public partial class EditUserAccount : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int selectedUserAccountId = int.Parse(Request.QueryString["id"]);
                hdnUserAccountId.Text = Request.QueryString["id"];
                AssignUserAccountTypes();
                FillUserAccountDetails(selectedUserAccountId);

                //var currentStatus = ObjectContext.UserAccountStatus.SingleOrDefault(entity => entity.UserAccountId == selectedUserAccountId && entity.EndDate == null);
                //switch (currentStatus.UserAccountStatusType.Name)
                //{
                //    case "Active":
                //        btnDeactivate.Disabled = false;
                //        break;

                //    case "Inactive":
                //        btnActivate.Disabled = false;
                //        break;

                //    default:
                //        break;
                //}
            }
        }

        private void AssignUserAccountTypes()
        {
            switch (this.LoginInfo.UserType)
            {
                case "Admin":
                    strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Id != UserAccountType.SuperAdmin.Id && entity.Id != UserAccountType.Admin.Id);
                    break;
                case "Super Admin":
                    strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Id != UserAccountType.SuperAdmin.Id);
                    break;
                default:
                    break;
            }

            strUserAccountType.DataBind();
        }

        protected void UpdateUserAccount(int selectedUserAccountId)
        {
            var currentUserAccount = UserAccount.GetById(selectedUserAccountId);

            currentUserAccount.UserAccountTypeId = int.Parse(cmbUserAccountType.SelectedItem.Value);
            ObjectContext.SaveChanges();
        }

        protected void FillUserAccountDetails(int selectedUserAccountId) 
        {
            var userAccount = UserAccount.GetById(selectedUserAccountId);
            var userAccountModel = new UserAccountViewModel(userAccount);

            txtName.Text = userAccountModel.NameOfUser;
            txtUsername.Text = userAccount.Username;
            ////this.txtPassword.Text = userAccount.Password;
            ////this.txtConfirmPassword.Text = userAccount.Password;
            //cmbPasswordQuestion.Text = userAccount.SecurityQuestion;
            //txtPasswordAnswer.Text = userAccount.SecurityAnswer;
            cmbUserAccountType.SelectedItem.Value = userAccount.UserAccountTypeId.ToString();
        }

        //BUTTON CLICK EVENT HANDLERS
        protected void btnActivate_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int userAccountId = int.Parse(hdnUserAccountId.Text);
            var userAccount = UserAccount.GetById(userAccountId);
            UserAccountStatu.CreateOrUpdateCurrent(userAccount, UserAccountStatusType.ActiveType, now);
            ObjectContext.SaveChanges();
            X.Msg.Alert("User Account", "Successfully activated the user account.");
        }

        protected void btnDeactivate_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int userAccountId = int.Parse(hdnUserAccountId.Text);
            var userAccount = UserAccount.GetById(userAccountId);
            UserAccountStatu.CreateOrUpdateCurrent(userAccount, UserAccountStatusType.InactiveType, now);
            ObjectContext.SaveChanges();
            X.Msg.Alert("User Account", "Successfully deactivated the user account.");
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            UpdateUserAccount(int.Parse(hdnUserAccountId.Text));
        }
    }
}