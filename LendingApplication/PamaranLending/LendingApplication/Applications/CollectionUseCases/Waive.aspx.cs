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
    public partial class Waive : ActivityPageBase
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
                hiddenType.Value = Request.QueryString["RoleType"];
                if (hiddenType.Text == "Borrower")
                {
                    LoanPaymentModel borrower = (LoanPaymentModel)form.RetrieveBorrower(hiddenRandomKey.Text);
                    this.Register(borrower);
                    FillBorrower(borrower);
                }
                else if (hiddenType.Text == "Guarantor")
                {
                    LoanPaymentModel guarantor = (LoanPaymentModel)form.RetrieveGuaCoBorower(hiddenRandomKey.Text);
                    this.Register(guarantor);
                    FillBorrower(guarantor);
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
                if (decimal.Parse(txtPastDue.Text) == 0)
                {
                    txtPastDue2.Value = 0.00;
                    txtPastDue2.ReadOnly = true;
                }
            }
        }
        public void FillBorrower(LoanPaymentModel borrower)
        {
            this.txtInterestDue.Text = borrower.InterestDue.ToString("N");
            this.txtPastDue.Text = borrower.PastDue.ToString("N");
            this.txtPrincipalDue.Text = borrower.PrincipalDue.ToString("N");
            this.txtTotalLoanBalance.Text = borrower.TotalAmountDue.ToString("N");
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
            LoanPaymentModel borrower = this.RetrieveBOM<LoanPaymentModel>();
            if (borrower.WaiveInterest >= 0)
                borrower.WaiveInterest += decimal.Parse(txtInterestDue2.Text);
            if (borrower.WaivePastDue >= 0)
                borrower.WaivePastDue += decimal.Parse(txtPastDue2.Text);
            if (borrower.WaivePrincipalDue >= 0)
                borrower.WaivePrincipalDue += decimal.Parse(txtPrincipalDue2.Text);
            borrower.UpdateAll();
        }

        protected void OnRefreshDataOutstandingLoan(object sender, StoreRefreshDataEventArgs e)
        { 
            
        }
      
        public void txtPastDue2_Blur(object sender, DirectEventArgs e)
        {
            decimal totalBalance = 0;
            if (string.IsNullOrWhiteSpace(txtInterestDue2.Text) == false)
                totalBalance += decimal.Parse(txtInterestDue2.Text);
            if (string.IsNullOrWhiteSpace(txtPastDue.Text) == false)
                totalBalance += decimal.Parse(txtPastDue2.Text);
            totalBalance += decimal.Parse(txtPrincipalDue.Text);
            txtTotalLoanBalance2.Text = totalBalance.ToString();
        }

    }
}