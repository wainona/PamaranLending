using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases
{
    public partial class AddCustomerClassification : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                return allowed;
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
            var districtTypes = ObjectContext.DistrictTypes;
            if (districtTypes != null)
            {
                strDistrictType.DataSource = districtTypes.ToList();
                strDistrictType.DataBind();
            }
        }

        protected void checkDistrict(object sender, RemoteValidationEventArgs e)
        {
            int count = 0;
            using (var context = new FinancialEntities())
            {
                count = context.ClassificationTypes.Where(entity => entity.District == this.District.Text).Count();
            }
            e.Success = count == 0;
            if (count > 0)
                e.ErrorMessage = "Customer classification record already exists.";

        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {                
                ClassificationType classification = new ClassificationType();
                classification.District = this.District.Text;
                classification.StationNumber = this.StationNumber.Text;

                var selectedDistrict = this.cmbDistrictType.SelectedIndex;
                if (selectedDistrict != -1)
                {
                    classification.DistrictTypeId = int.Parse(this.cmbDistrictType.SelectedItem.Value);
                }

                context.ClassificationTypes.AddObject(classification);
                context.SaveChanges();
            }
        }
    }
}