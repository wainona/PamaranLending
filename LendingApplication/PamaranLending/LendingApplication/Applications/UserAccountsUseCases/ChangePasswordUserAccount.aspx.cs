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
    public partial class ChangePasswordUserAccount : ActivityPageBase
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
                FillUserAccountDetails(selectedUserAccountId);
            }
        }

        protected void UpdateUserAccount(int selectedUserAccountId)
        {
            var currentUserAccount = UserAccount.GetById(selectedUserAccountId);
            currentUserAccount.Password = SimplifiedHash.ComputeHash(txtPassword.Text, SimplifiedHash.HashType.sha1);
            ObjectContext.SaveChanges();
        }

        protected void FillUserAccountDetails(int selectedUserAccountId) 
        {
            var userAccount = UserAccount.GetById(selectedUserAccountId);
            var userAccountModel = new UserAccountViewModel(userAccount);

            txtName.Text = userAccountModel.NameOfUser;
            txtUsername.Text = userAccount.Username;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            UpdateUserAccount(int.Parse(hdnUserAccountId.Text));
        }
    }
}