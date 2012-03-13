using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using System.IO;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class AddSubmitRequiredDocument : ActivityPageBase
    {
        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

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
            if ((X.IsAjaxRequest == false && this.IsPostBack == false))
            {
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                int id = int.Parse(Request.QueryString["financialProductId"]);

                dtDateSubmitted.MaxDate = DateTime.Now;
                dtDateSubmitted.DisabledDays = ApplicationSettings.DisabledDays;
                hdnPage.Value = Convert.ToInt32(hdnPage.Value);
                SubmittedDocumentModel model = this.CreateOrRetrieveBOM<SubmittedDocumentModel>();

                var prds = from prd in ObjectContext.ProductRequiredDocuments.Where(entity => entity.FinancialProductId == id)
                           select new {Id = prd.Id, Name = prd.RequiredDocumentType.Name };
                storeDocumentPage.DataSource = prds;
                storeDocumentPage.DataBind();
            }
            else
            {
                hdnPage.Value = Convert.ToInt32(hdnPage.Value) + 1;
            }
        }

        protected void btnSubmit_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
            SubmittedDocumentModel model = this.CreateOrRetrieveBOM<SubmittedDocumentModel>();
            model.ProductRequiredDocumentId = int.Parse(cmbDocumentName.SelectedItem.Value);
            model.ProductRequiredDocumentName = cmbDocumentName.SelectedItem.Text;
            model.DateSubmitted = dtDateSubmitted.SelectedDate;
            model.DocumentDescription = txtDocDescription.Text;

            form.AddSubmittedDocument(model);
        }

        protected void UploadClick(object sender, DirectEventArgs e)
        {
            string msg = "";
            int pageNumber = Convert.ToInt32(hdnPage.Value);
            string fileName = fileUpload.PostedFile.FileName;
            try
            {
                if (fileUpload.HasFile)
                {
                    string svrFilename = DateTime.Now.ToString("M-dd-yyyy hhmmss.ff") + Path.GetExtension(fileName);
                    string svrFullPath = Path.Combine(Server.MapPath("~//Uploaded//Documents"), svrFilename);
                    string serverActualPath = @"/Uploaded/Documents/" + svrFilename;
                    fileUpload.PostedFile.SaveAs(svrFullPath);

                    SubmittedDocumentModel form = this.CreateOrRetrieveBOM<SubmittedDocumentModel>();
                    form.AddSubmitDocument(new DocumentPageModel(pageNumber, txtDocumentName.Text, serverActualPath));
                    strDocumentPage.DataSource = form.AvailableSubmitDocument;
                    strDocumentPage.DataBind();

                    msg = "File uploaded successfully.";
                }
            }
            catch (Exception err)
            {
                msg = err.Message;
            }
            X.MessageBox.Alert("Status", msg).Show();
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            //delete uploaded record in SUBMIT REQUIRED DOCUMENTS TAB
            SubmittedDocumentModel form = this.CreateOrRetrieveBOM<SubmittedDocumentModel>();
            DocumentPageModel model = form.AvailableSubmitDocument.LastOrDefault();
            if (model == null)
            {
                return;
            }
            int count = Convert.ToInt32(hdnPage.Value);
            hdnPage.Value = count - 2;
            form.RemoveSubmitDocument(model);
            strDocumentPage.DataSource = form.AvailableSubmitDocument;
            strDocumentPage.DataBind();
        }
       
    }
}