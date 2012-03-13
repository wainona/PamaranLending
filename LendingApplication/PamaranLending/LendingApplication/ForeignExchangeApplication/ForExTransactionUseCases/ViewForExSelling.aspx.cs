using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class ViewForExSelling : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
        }

        
    }
}