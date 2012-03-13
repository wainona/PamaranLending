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
    public partial class EditDemandLetter : ActivityPageBase
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
                int selectedDemandLetterId = int.Parse(Request.QueryString["id"]);
                hdnDemandLetterId.Text = Request.QueryString["id"];
                //AssignUserAccountTypes();
                FillDetails(selectedDemandLetterId);

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

        //private void AssignUserAccountTypes()
        //{
        //    switch (this.LoginInfo.UserType)
        //    {
        //        case "Admin":
        //            strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Id != UserAccountType.SuperAdmin.Id && entity.Id != UserAccountType.Admin.Id);
        //            break;
        //        case "Super Admin":
        //            strUserAccountType.DataSource = ObjectContext.UserAccountTypes.Where(entity => entity.Id != UserAccountType.SuperAdmin.Id);
        //            break;
        //        default:
        //            break;
        //    }

        //    strUserAccountType.DataBind();
        //}

        //protected void UpdateUserAccount(int selectedUserAccountId)
        //{
        //    var currentUserAccount = UserAccount.GetById(selectedUserAccountId);

        //    currentUserAccount.UserAccountTypeId = int.Parse(cmbUserAccountType.SelectedItem.Value);
        //    ObjectContext.SaveChanges();
        //}

        protected void FillDetails(int selectedDemandLetterId) 
        {
            var demandLetter = ObjectContext.DemandLetters.SingleOrDefault(entity => entity.Id == selectedDemandLetterId);
            var loanAccount = ObjectContext.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == demandLetter.FinancialAccountId);
            var financialAccountRole = loanAccount.FinancialAccount.FinancialAccountRoles.SingleOrDefault(entity => entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);
            var party = financialAccountRole.PartyRole.Party;
            txtName.Text = party.Name;
            txtLoanId.Text = loanAccount.FinancialAccountId.ToString();
            if(demandLetter.DateSent.HasValue)
                dfDateSent.Text = demandLetter.DateSent.Value.ToString("MMMM dd, yyyy");
            if(demandLetter.DatePromisedToPay.HasValue)
                dfDatePromiseToPay.SelectedDate = demandLetter.DatePromisedToPay.Value;
            txtRemarks.Text = demandLetter.CurrentStatus.Remarks;
        }

        //BUTTON CLICK EVENT HANDLERS
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            int selectedDemandLetterId = int.Parse(hdnDemandLetterId.Text);

            using (var scope = new UnitOfWorkScope(true))
            {
                var demandLetter = ObjectContext.DemandLetters.SingleOrDefault(entity => entity.Id == selectedDemandLetterId);

                demandLetter.DatePromisedToPay = dfDatePromiseToPay.SelectedDate;
                demandLetter.CurrentStatus.Remarks = txtRemarks.Text;
            }
        }
    }
}