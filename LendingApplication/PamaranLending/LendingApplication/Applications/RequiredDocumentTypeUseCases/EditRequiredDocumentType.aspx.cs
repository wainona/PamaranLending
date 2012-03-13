using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using LendingApplication;
using System.Text;
using System.Globalization;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.RequiredDocumentTypeUseCases
{
    public partial class EditRequiredDocumentType : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int id = int.Parse(Request.QueryString["id"]);
                using (var context = new FinancialEntities())
                {
                    RequiredDocumentType customer = context.RequiredDocumentTypes.SingleOrDefault(entity => entity.Id == id);
                    if (customer != null)
                    {
                        this.RecordID.Value = customer.Id;
                        this.Name.Text = customer.Name;
                    }
                    else
                    {
                        throw new AccessToDeletedRecordException("The required document type has been deleted by another user.");
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                int id = int.Parse(this.RecordID.Text);
                RequiredDocumentType customer = context.RequiredDocumentTypes.SingleOrDefault(entity => entity.Id == id);
                TextInfo info = CultureInfo.CurrentCulture.TextInfo;
                var nameTemp = this.Name.Text;
                string name = "";
                string[] temp = nameTemp.Split('\'');
                if (temp.Length > 1)
                {
                    int count = 0;
                    foreach (var n in temp)
                    {
                        if (count == 0)
                        {
                            name = name + info.ToTitleCase(n);
                        }
                        else
                        {
                            var t = info.ToTitleCase(n);
                            name = name + "'" + info.ToLower(t[0]) + t.Substring(1);
                        }

                        count++;
                    }
                }
                else
                {
                    //foreach
                    //string name = info.ToTitleCase(this.Name.Text);
                    name = this.Name.Text;
                }
                customer.Name = name;
   

                context.SaveChanges();
            }
        }

        protected void CheckField(object sender, RemoteValidationEventArgs e)
        {
            TextField field = (TextField)sender;
            using (var ctx = new FinancialEntities())
            {
                string name = this.Name.Text;
                RequiredDocumentType customer = ctx.RequiredDocumentTypes.SingleOrDefault(entity => entity.Name == name);

                if ((customer == null) || (customer != null && customer.Id == int.Parse(this.RecordID.Text)))
                {
                    e.Success = true;
                    btnSave.Disabled = false;
                }
                else
                {
                    e.Success = false;
                    e.ErrorMessage = "Required document type already exists.";
                    btnSave.Disabled = true;
                }

                //System.Threading.Thread.Sleep(1000);
            }
            
        }
    }
}