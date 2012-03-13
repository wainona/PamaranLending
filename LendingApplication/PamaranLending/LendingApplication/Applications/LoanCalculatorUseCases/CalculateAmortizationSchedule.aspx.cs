using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.FinancialManagement.LoanCalculatorUseCases
{
    public partial class CalculateAmortizationSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {

                using (var context = new FinancialEntities())
                {
                    var loanTerm = from uom in context.UnitOfMeasures
                                   where uom.UnitOfMeasureType.Name == "Time Unit"
                               select new 
                               {
                                    Id = uom.Id,
                                    Name = uom.Name
                               };
                    var paymentMode = from uom in context.UnitOfMeasures
                                   where uom.UnitOfMeasureType.Name == "Time Frequency"
                                   select new
                                   {
                                       Id = uom.Id,
                                       Name = uom.Name
                                   };
                    var interestMode = from pf in context.ProductFeatures
                                      where pf.ProductFeatureCategory.Name == "Interest Computation Mode"
                                      select new
                                      {
                                          Id = pf.Id,
                                          Name = pf.Name
                                      };
                    var methodOfCharging = from pf in context.ProductFeatures
                                           where pf.ProductFeatureCategory.Name == "Method of Charging Interest"
                                           select new
                                           {
                                               Id = pf.Id,
                                               Name = pf.Name
                                           };
                    StoreLoanTerm.DataSource = loanTerm.ToList();
                    StorePaymentMode.DataSource = paymentMode.ToList();
                    StoreInterest.DataSource = interestMode.ToList();
                    StoreMethodOfCharging.DataSource = methodOfCharging.ToList();
                    StoreLoanTerm.DataBind();
                    StorePaymentMode.DataBind();
                    StoreInterest.DataBind();
                    StoreMethodOfCharging.DataBind();
                }

                
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {

        }

    }
}