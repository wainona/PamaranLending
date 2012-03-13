using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Globalization;

namespace LendingApplication
{
    public partial class GlobalExceptionHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs evt)
        {
            StringBuilder message = new StringBuilder("");

            Exception e = Session["Error"] as Exception;

            while (e != null)
            {
                // add handling code if it makes sense

                message.Append(e.Message.ToString());
                WriteError(e.ToString());

                e = e.InnerException;
            }
            message.Append("<br/>");
            // log, send email, and/or notify another way
            lblErrorMessage.Text = message.ToString();
            Server.ClearError();
        }

        protected void WriteError(string errorMessage)
        {
            try
            {
                string path = "/Errors/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                  ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }

        }
    }
}