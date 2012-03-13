using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases
{
    public partial class EditCustomerClassification : ActivityPageBase
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
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var districtTypes = ObjectContext.DistrictTypes;
                if (districtTypes != null)
                {
                    strDistrictType.DataSource = districtTypes.ToList();
                    strDistrictType.DataBind();
                }

                int id = int.Parse(Request.QueryString["id"]);
                using (var context = new FinancialEntities())
                {
                    ClassificationType classification = context.ClassificationTypes.SingleOrDefault(entity => entity.Id == id);
                    if (classification != null)
                    {
                        this.RecordID.Value = classification.Id;
                        this.District.Text = classification.District;
                        this.StationNumber.Text = classification.StationNumber;
                        if (classification.DistrictTypeId != null)
                        {
                            this.cmbDistrictType.SelectedItem.Text = classification.DistrictType.Name;
                        }
                    }
                    else
                    {
                        throw new AccessToDeletedRecordException(" The customer classification record has been deleted by another user.");
                    }
                }

            }
        }

        protected void checkDistrict(object sender, RemoteValidationEventArgs e)
        {
            int count = 0;
            using (var context = new FinancialEntities())
            {
                int id = int.Parse(this.RecordID.Text);
                count = context.ClassificationTypes.Where(entity => entity.District == this.District.Text && entity.Id != id).Count();
            }
            e.Success = count == 0;
            if (count > 0)
                e.ErrorMessage = "Customer classification record already exists.";

        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                int id = int.Parse(this.RecordID.Text);
                ClassificationType classification = context.ClassificationTypes.SingleOrDefault(entity => entity.Id == id);
                classification.District = this.District.Text;
                classification.StationNumber = this.StationNumber.Text;

                var selectedDistrict = this.cmbDistrictType.SelectedIndex;
                if (selectedDistrict != -1)
                {
                    classification.DistrictTypeId = int.Parse(this.cmbDistrictType.SelectedItem.Value);
                }

                context.SaveChanges();
            }
        }
    }
}