using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.BackgroundUseCases
{
    public partial class GenerateBill : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                dtGenerationDate.SelectedDate = DateTime.Today;
                //dtGenerationDate.MinDate = DateTime.Today;
            }
        }
        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                BusinessLogic.GenerateBillFacade.GenerateReceivable(dtGenerationDate.SelectedDate);
            }
            using (var unitOfWork = new UnitOfWorkScope(true))
            {

                LoanAccount.UpdateLoanAccountStatus(dtGenerationDate.SelectedDate);
            }
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                DemandLetter.UpdateDemandLetterStatus(dtGenerationDate.SelectedDate);
            }
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                Customer.UpdateCustomerStatus(dtGenerationDate.SelectedDate);
            }
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
             
                FinancialProduct.UpdateStatus();
               
            }
        }
    }
}