using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class AmortizationScheduleChangeInterest : ActivityPageBase
    {
        //public override List<string> UserTypesAllowed
        //{
        //    get
        //    {
        //        List<string> allowed = new List<string>();
        //        allowed.Add("Super Admin");
        //        allowed.Add("Loan Clerk");
        //        allowed.Add("Admin");
        //        return allowed;
        //    }
        //}

        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            hdnSelectedLoanID.Value = Request.QueryString["loanId"];
            hiddenResourceGUID.Value = this.ResourceGuid;
            hiddenCustomerId.Value = Request.QueryString["cid"];
            hiddenRandomKey.Value = Request.QueryString["b"];
            ParentResourceGuid = Request.QueryString["guid"];

            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(hiddenRandomKey.Value.ToString());
            this.hiddenBalance.Value = model.TotalLoanBalance;
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;

            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());

            form.ChangeIcmAccount.BalanceToCarryOver = Decimal.Parse(hiddenBalance.Value.ToString());
            form.GenerateChangeIcmAmortizationSchedule(today);

            storeAmortizationSchedule.DataSource = form.ChangeIcmAmortizationSchedule.ToList();
            storeAmortizationSchedule.DataBind();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;

            using (var unitOfWork = new UnitOfWorkScope())
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                form.SaveChangeIcmAmortizationSchedule(today);
            }

        }
    }
}