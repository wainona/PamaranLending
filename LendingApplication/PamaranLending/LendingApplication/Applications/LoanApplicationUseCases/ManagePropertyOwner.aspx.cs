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
    public partial class ManagePropertyOwner : ActivityPageBase
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
                string mode = Request.QueryString["mode"].ToLower();
                hiddenMode.Value = mode;
                if (mode == "edit")
                {
                    hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                    hiddenPartyId.Value = Request.QueryString["PartyId"];
                    txtPropertyOwner.Value = Request.QueryString["Name"];
                    txtAddress.Value = Request.QueryString["Address"];
                    nfPercentOwned.Value = Request.QueryString["PercentOwned"];
                    btnPickPropertyOwner.Disabled = true;
                }
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            
        }
    }
}