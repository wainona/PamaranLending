using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LendingApplication;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication
{
    public partial class ManageApplicationFee : ActivityPageBase
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

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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
                hiddenProductId.Value = Request.QueryString["financialProductId"];
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
            }
        }

        [DirectMethod(ShowMask=true, Msg="Validating Fee name...")]
        public bool ValidateName(string name)
        {
            bool valid = false;

            if (hiddenId.Text == string.Empty)
            {
                int id = int.Parse(hiddenProductId.Value.ToString());
                var feature = ProductFeature.GetByName(name);
                if(feature != null)
                {
                    var pfa = ProductFeatureApplicability.GetActive(feature, FinancialProduct.GetById(id));
                    valid = pfa == null || pfa.Fee == null;
                }
                else
                    valid = true;
                txtFeeName.InvalidText = "The fee name already exist in the product feature table.";
            }

            if (valid == true || string.IsNullOrWhiteSpace(hiddenId.Text) == false)
            {
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                //check if already has that name in the added items.
                valid = form.AvailableFees.Where(entity => entity.Name == name).Count() == 0;
                txtFeeName.InvalidText = "The fee name already added in the loan application.";
            }

            if( valid == false)
                txtFeeName.MarkInvalid(txtFeeName.InvalidText);

            return valid;
        }

        public void CheckFeeNameChanged(object sender, RemoteValidationEventArgs e)
        {
            e.Success = false;
            //check fee name
            string name = txtFeeName.Text;
            if (hiddenId.Text == string.Empty)
            {
                //check productcategory
                int id = int.Parse(hiddenProductId.Value.ToString());
                var feature = ProductFeature.GetByName(name);
                if (feature != null)
                {
                    var pfa = ProductFeatureApplicability.GetActive(feature, FinancialProduct.GetById(id));
                    e.Success = pfa == null || pfa.Fee == null;
                }
                else
                    e.Success = true;

                if (e.Success == false)
                {
                    e.ErrorMessage = "The fee name already exist in the product feature table.";
                    txtFeeName.InvalidText = "The fee name already exist in the product feature table.";
                }
            }

            if (e.Success == true || string.IsNullOrWhiteSpace(hiddenId.Text) == false)
            {
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                //check if already has that name in the added items.
                e.Success = form.AvailableFees.Where(entity => entity.Name == name).Count() == 0;
                if (e.Success == false)
                {
                    e.ErrorMessage = "The fee name already added in the loan application.";
                    txtFeeName.InvalidText = "The fee name already added in the loan application.";
                }
            }

            if (e.Success == false)
                txtFeeName.MarkInvalid(txtFeeName.InvalidText);
        }
    }
}