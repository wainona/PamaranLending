using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication
{
    public partial class PickListFees : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                int id = int.Parse(Request.QueryString["financialProductId"]);

                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);

                var financialProduct = FinancialProduct.GetById(id);
                var fees = ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.FeeType, financialProduct);
                List<FeeModel> feesModels = new List<FeeModel>();
                foreach (ProductFeatureApplicability item in fees.Where(entity=>entity.Fee != null))
                {
                    feesModels.Add(new FeeModel(item));
                }

                storeFees.DataSource = feesModels;
                storeFees.DataBind();
            }
        }
    }
}