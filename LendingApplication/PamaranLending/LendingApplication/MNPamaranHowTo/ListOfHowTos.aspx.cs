using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.MNPamaranHowTo
{
    public partial class ListOfHowTos : ActivityPageBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //hiddenDate.Value = DateTime.Now;
                var root = this.NavigationTree.Root;
                var treePanel = NavigationManager.BuildHelpTree();
                root.Add(treePanel);
            }
        }
    }
}