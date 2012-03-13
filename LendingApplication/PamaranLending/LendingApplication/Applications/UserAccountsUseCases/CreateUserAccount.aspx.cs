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
    public partial class CreateUserAccount : ActivityPageBase
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
                AssignUserAccountTypes();
            }
        }

        private void AssignUserAccountTypes()
        {
            switch (this.LoginInfo.UserType)
            {
                case "Admin":
                    strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Name.Contains("Admin") == false);
                    break;
                case "Super Admin":
                    strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Id != UserAccountType.SuperAdmin.Id);
                    break;
                default:
                    break;
            }
            
            strUserAccountType.DataBind();
        }

        [DirectMethod]
        public void FillNameOfSelectedPartyId()
        {
            int PartyId = int.Parse(hdnPartyId.Text);
            var allowedUserView = ObjectContext.UsersViewLists.SingleOrDefault(entity => entity.PartyId == PartyId);
            txtName.Text = allowedUserView.Name;
        }

        protected void ValidateUsernameOnDatabase(object sender, RemoteValidationEventArgs e)
        {
            string username = txtUsername.Text;
            var usernameInstance = ObjectContext.UserAccounts.SingleOrDefault(entity => entity.Username == username && entity.EndDate == null);
            if (usernameInstance != null)
            {
                e.Success = false;
                e.ErrorMessage = "The username exists in the database. Please select another username.";
            }
            else
            {
                e.Success = true;
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var userAccount = CreateNewUserAccount(now);
            UserAccountStatu.CreateOrUpdateCurrent(userAccount, UserAccountStatusType.ActiveType, now);
            ObjectContext.SaveChanges();
        }

        protected UserAccount CreateNewUserAccount(DateTime now)
        {
            UserAccount newUserAccount = new UserAccount();
            newUserAccount.PartyId = int.Parse(hdnPartyId.Text);//int.Parse(hdnPartyId.Text);//TEMPORARY
            newUserAccount.UserAccountTypeId = int.Parse(cmbUserAccountType.SelectedItem.Value.ToString());
            newUserAccount.Username = txtUsername.Text;
            newUserAccount.Password = SimplifiedHash.ComputeHash(txtPassword.Text, SimplifiedHash.HashType.sha1);
            newUserAccount.SecurityQuestion = cmbPasswordQuestion.Text;
            newUserAccount.SecurityAnswer = txtPasswordAnswer.Text;
            newUserAccount.DateCreated = now;
            newUserAccount.EffectiveDate = now;
            newUserAccount.EndDate = null;

            ObjectContext.UserAccounts.AddObject(newUserAccount);
            return newUserAccount;
        }
    }
}