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
    public partial class PickListInterestRate : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                int id = int.Parse(Request.QueryString["financialProductId"]);

                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);

                var financialProduct = FinancialProduct.GetById(id);
                var rates = ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestRateType, financialProduct);
                List<InterestRateModel> interestRates = new List<InterestRateModel>();
                foreach (ProductFeatureApplicability item in rates)
                {
                    interestRates.Add(new InterestRateModel(item));
                }

                storeInterestRate.DataSource = interestRates;
                storeInterestRate.DataBind();
            }
        }
    }
}