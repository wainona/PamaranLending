using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.LoanUseCases
{
    public partial class UnderLitigationLoan : ActivityPageBase
    {
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
                int selectedLoanId = int.Parse(Request.QueryString["id"]);
                hdnLoanId.Text = selectedLoanId.ToString();
            }
        }

        protected void btnSave_OnDirectEvent(object sender, DirectEventArgs e)
        {
            int loanId = int.Parse(hdnLoanId.Text);
            var LoanStatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.LoanAccount.FinancialAccountId == loanId && entity.IsActive == true);
            var loanAccountStatusTypeAssoc = ObjectContext.LoanAccountStatusTypeAssocs.Where(entity => entity.EndDate == null
                                                && entity.FromStatusTypeId == LoanStatus.LoanAccountStatusType.Id
                                                && entity.ToStatusTypeId == LoanAccountStatusType.UnderLitigationType.Id);

            if (loanAccountStatusTypeAssoc == null) 
            {
                X.Msg.Alert("Error!", "Cannot change status to Under Litigation.").Show();
                return;
            }
            LoanStatus.IsActive = false;

            LoanAccountStatu newLoanAccountStatus = new LoanAccountStatu();
            newLoanAccountStatus.FinancialAccountId = loanId;
            newLoanAccountStatus.LoanAccountStatusType = LoanAccountStatusType.UnderLitigationType;
            newLoanAccountStatus.Remarks = txtComment.Text;
            newLoanAccountStatus.TransitionDateTime = DateTime.Now;
            newLoanAccountStatus.IsActive = true;
            ObjectContext.LoanAccountStatus.AddObject(newLoanAccountStatus);

            ChangeCustomerStatusToUnderLitigation(loanId);
            
            ObjectContext.SaveChanges();
        }

        private void ChangeCustomerStatusToUnderLitigation(int loanAccountId)
        {
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == loanAccountId);
            var ownerFinAcctRole = financialAccount.FinancialAccountRoles.SingleOrDefault(entity => entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);
            var partyId = ownerFinAcctRole.PartyRole.PartyId;
            var customerPartyRole = PartyRole.GetByPartyIdAndRole(partyId, RoleType.CustomerType);
            var customer = customerPartyRole.Customer;
            var customerStatus = customer.CurrentStatus;

            if (customerStatus.CustomerStatusType == CustomerStatusType.DelinquentType)
            {
                customerStatus.IsActive = false;
                CustomerStatu newCustomerStatus = new CustomerStatu();
                newCustomerStatus.PartyRoleId = customer.PartyRoleId;
                newCustomerStatus.CustomerStatusTypeId = CustomerStatusType.SubprimeType.Id;
                newCustomerStatus.TransitionDateTime = DateTime.Now;
                newCustomerStatus.IsActive = true;

                ObjectContext.CustomerStatus.AddObject(newCustomerStatus);
            }
        }
    }
}