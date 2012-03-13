using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class AddCheques : ActivityPageBase
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
            }
        }

        //[DirectMethod]
        //public int FillCheque()
        //{
        //    AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
        //    ChequesModel model = new ChequesModel();
        //    model.BankName = txtBankName.Text;
        //    model.Branch = txtBankBranch.Text;
        //    model.CheckType = cmbCheckType.SelectedItem.Text;
        //    model.CheckNumber = txtCheckNumber.Text;
        //    model.CheckDate = dtCheckDate.SelectedDate;
        //    model.Amount = decimal.Parse(txtAmount.Text);
 
        //    form.AddCheques(model);

        //    return 1;
        //}

        [DirectMethod]
        public int ValidateCheckNumber(string chequeNumber)
        {
            return Cheque.ValidateCheckNumber(chequeNumber);
        }

        [DirectMethod]
        public int ValidateNewCheckNumber(string chequeNumber)
        {
            int value = 0;
            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
            var cheques = form.AvailableCheques;
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