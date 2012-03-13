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

namespace LendingApplication
{
    public partial class SimpleAddEditTemplate : ActivityPageBase
    {
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
                int id = int.Parse(Request.QueryString["id"]);
                AssetType asset = ObjectContext.AssetTypes.SingleOrDefault(entity => entity.Id == id);
                if (asset != null)
                {
                    this.RecordID.Value = id;
                    this.txtName.Text = asset.Name;
                    if (asset.IsAppraisableIndicator)
                        this.radTrue.Checked = true;
                    else
                        this.radFalse.Checked = true;
                }
                else
                {
                    // TODO:: Don't know how to handle for now.
                }
            }
        }

        protected void checkEdit(object sender, RemoteValidationEventArgs e)
        {
            int id = int.Parse(this.RecordID.Text);
            int count = ObjectContext.AssetTypes.Where(entity => entity.Name == this.txtName.Text && entity.Id != id).Count();
            e.Success = count == 0;
            if (count > 0)
                e.ErrorMessage = "Asset type name already exists.";
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                int id = int.Parse(this.RecordID.Text);
                AssetType asset = ObjectContext.AssetTypes.SingleOrDefault(entity => entity.Id == id);
                asset.Name = this.txtName.Text;
                asset.IsAppraisableIndicator = this.radTrue.Checked;
            }
        }
    }
}