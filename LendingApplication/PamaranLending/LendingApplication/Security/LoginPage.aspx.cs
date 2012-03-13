using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Web.Security;
using FirstPacific.UIFramework;

namespace LendingApplication.Security
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_DirectClick(object sender, DirectEventArgs e)
        {
            LoginSessionInformation sessionInformation = LoginSessionInformation.Instance;
            UserAccountAuthenticationProvider authProvider = new UserAccountAuthenticationProvider();
            if (sessionInformation.SignIn(txtUsername.Text, txtPassword.Text, authProvider))
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                PageFormPanelStatusBar.Text = "Enter correct username and password.";
            }
        }
    }
}