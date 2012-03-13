using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.AgingOfAccountsUseCases
{
    public partial class ListAgingOfAccounts : ActivityPageBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                hiddenDate.Value = DateTime.Now;
            }
        }

        [DirectMethod]
        public void FillDate()
        {
            hiddenDate.Value = this.datSelectedDate.SelectedDate.Date.ToString("mm-dd-yyyy");
        }
    }
}