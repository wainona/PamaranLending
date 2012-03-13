using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public partial class LoanApplicationForm
    {

        private void SubmitForApproval(LoanApplication loanApplication)
        {
            //check use case: Submit Loan Application Directly for Approval : to know if selected loan application
            //can be submitted directly for approval :)

            //end current status of loan application
            var activeLoanApplication = LoanApplicationStatu.GetActive(loanApplication);
            activeLoanApplication.TransitionDateTime = DateTime.Now;
            activeLoanApplication.IsActive = false;

            //change status to pending: approval
            var approvedLoanAppStatusId = LoanApplicationStatusType.GetByName(LoanApplicationStatusType.PendingApprovalType);
            LoanApplicationStatu loanApplicationStatus = new LoanApplicationStatu();
            loanApplicationStatus.StatusTypeId = approvedLoanAppStatusId.Id;
            loanApplicationStatus.ApplicationId = loanApplication.ApplicationId;
            loanApplicationStatus.TransitionDateTime = DateTime.Now;
            loanApplicationStatus.IsActive = true;
        }

        private void SubmitForAppraisal(int selectedLoanApplicationId)
        {
            //if CheckAppraisableCollateral is true, display confirmation box.
            //if Yes in confirmation box, proceed..
            //var pendingAppraisal = LoanApplicationStatusType.GetByName(LoanApplicationStatusType.);
        }

        private bool CheckAppraisableCollateral(int id)
        {
            //determine if the application has one or more appraisable collateral 
            var loanAppAsset = Context.AssetTypes.SingleOrDefault(entity => entity.IsAppraisableIndicator == true);
            var appraisableCollateral = Context.Assets.Where(entity => entity.ApplicationId == id
                && entity.AssetTypeId == loanAppAsset.Id);
            if (appraisableCollateral != null)
                return true;
            else
                return false;
        }
    }
}
