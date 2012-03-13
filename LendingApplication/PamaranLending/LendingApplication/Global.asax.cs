using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;

namespace LendingApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Server.Transfer("/GlobalExceptionHandler.aspx");
            try
            {
                Session["Error"] = Server.GetLastError();
                Session["PageErrorOccured"] = Request.CurrentExecutionFilePath;

                Server.ClearError();
                Response.Redirect("/GlobalExceptionHandler.aspx");
            }
            catch (Exception)
            {
                Response.Write("We apologize, but an unrecoverable error has occurred. Please click the back button on your browser and try again.");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Server.Transfer("/Security/LoginPage.aspx");
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}