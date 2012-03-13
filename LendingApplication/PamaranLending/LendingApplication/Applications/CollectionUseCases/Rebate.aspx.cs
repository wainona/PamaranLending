using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class Rebate : ActivityPageBase
    {
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
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
                hiddenRandomKey.Value = Request.QueryString["RandomKey"];

                LoanPaymentModel borrower = (LoanPaymentModel)form.RetrieveBorrower(hiddenRandomKey.Text);
                this.Register(borrower);
                FillBorrower(borrower);

                var rebateType = ObjectContext.AdjustmentTypes.SingleOrDefault(entity => entity.Id == AdjustmentType.RebateType.Id);
            }

            if (decimal.Parse(txtPrincipalDue.Text) == 0)
            {
                txtPrincipalDue2.Value = 0.00;
                txtPrincipalDue2.ReadOnly = true;
            }
            if (decimal.Parse(txtInterestDue.Text) == 0)
            {
                txtInterestDue2.Value = 0.00;
                txtInterestDue2.ReadOnly = true;
            }

        }

        public void FillBorrower(LoanPaymentModel borrower)
        {
            this.txtPrincipalDue.Text = borrower.PrincipalDue.ToString("N");
            this.txtInterestDue.Text = borrower.InterestDue.ToString("N");
            this.txtTotalLoanBalance.Text = borrower.TotalAmountDue.ToString("N");
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
            LoanPaymentModel borrower = this.RetrieveBOM<LoanPaymentModel>();
            if (borrower.RebateInterest >= 0)
                borrower.RebateInterest += decimal.Parse(txtInterestDue2.Text);
                borrower.RebateInterest = decimal.Parse(txtInterestDue2.Text);
            if (borrower.RebatePrincipalDue >= 0)
                borrower.RebatePrincipalDue += decimal.Parse(txtPrincipalDue2.Text);
            borrower.UpdateAll();
        }

        protected void OnRefreshDataOutstandingLoan(object sender, StoreRefreshDataEventArgs e)
        {
        }
    }
}