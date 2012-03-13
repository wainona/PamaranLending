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
    public partial class WriteOffLoan : ActivityPageBase
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
                hdnLoanAccountId.Text = selectedLoanId.ToString();
            }
        }

        protected void btnSave_onDirectClick(object sender, DirectEventArgs e)
        {
            var loanId = int.Parse(hdnLoanAccountId.Text);
            var LoanStatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.LoanAccount.FinancialAccountId == loanId && entity.IsActive == true);
            var loanAccountStatusTypeAssoc = ObjectContext.LoanAccountStatusTypeAssocs.Where(entity => entity.EndDate == null
                                                && entity.FromStatusTypeId == LoanStatus.LoanAccountStatusType.Id
                                                && entity.ToStatusTypeId == LoanAccountStatusType.WrittenOffType.Id);

            if (loanAccountStatusTypeAssoc == null)
            {
                X.Msg.Alert("Error!", "Cannot change status to Written-Off.").Show();
                return;
            }
            LoanStatus.IsActive = false;
            
            LoanAccountStatu newLoanAccountStatus = new LoanAccountStatu();
            newLoanAccountStatus.FinancialAccountId = loanId;
            newLoanAccountStatus.LoanAccountStatusType = LoanAccountStatusType.WrittenOffType;
            newLoanAccountStatus.Remarks = txtComment.Text;
            newLoanAccountStatus.TransitionDateTime = DateTime.Now;
            newLoanAccountStatus.IsActive = true;
            ObjectContext.LoanAccountStatus.AddObject(newLoanAccountStatus);
            ObjectContext.SaveChanges();
        }
    }
}