using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using System.Text;
using System.Globalization;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.RequiredDocumentTypeUseCases
{
    public partial class AddRequiredDocumentType : ActivityPageBase
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

        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                RequiredDocumentType doc = new RequiredDocumentType();
                TextInfo info = CultureInfo.CurrentCulture.TextInfo;
                //string name = info.ToTitleCase(this.Name.Text);
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
                doc.Name = name;

                context.RequiredDocumentTypes.AddObject(doc);
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

                if (customer == null)
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