using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class AddDisbursementCheque : ActivityPageBase
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                hdnType.Value = Request.QueryString["type"];
            }

        }
        [DirectMethod]
        public int ValidateCheckNumber(string chequeNumber)
        {
            return Cheque.ValidateCheckNumber(chequeNumber);
        }


        [DirectMethod]
        public int ValidateNewCheckNumber(string chequeNumber)
        {
            int value = 0;
            IEnumerable<AddChequesModel> cheques = null;

            if (hdnType.Text == "Rediscounting")
            {
                RediscountingForm form = this.Retrieve<RediscountingForm>(ParentResourceGuid);
               cheques = form.AvailableCheques;
               
            }
            else if (hdnType.Text == "Encashment")
            {
                EncashmentForm form = this.Retrieve<EncashmentForm>(ParentResourceGuid);
                cheques = form.AvailableCheques;

            }
            else if (hdnType.Text == "LoanDisbursement")
            {
                LoanDisbursementForm form = this.Retrieve<LoanDisbursementForm>(ParentResourceGuid);
                cheques = form.AvailableCheques;

            }
            else if (hdnType.Text == "OtherLoanDisbursement")
            {
                OtherDisbursementForm form = this.Retrieve<OtherDisbursementForm>(ParentResourceGuid);
                cheques = form.AvailableCheques;
            }
            else if (hdnType.Text == "Change")
            {
                ChangeForm form = this.Retrieve<ChangeForm>(ParentResourceGuid);
                cheques = form.AvailableCheques;
            }
            else if (hdnType.Text == "FeePayment")
            {
                FeePaymentForm form = this.Retrieve<FeePaymentForm>(ParentResourceGuid);
                cheques = form.AvailableCheques;
            }

            foreach (var cheque in cheques)
            {
                if (chequeNumber == cheque.CheckNumber)
                {
                    value = 1;
                    break;
                }
            }
            return value;
        }
    }
}