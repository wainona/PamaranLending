using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class ViewSubmittedDocument : ActivityPageBase
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
            //if current displayed page is  the first page, disable btnPreviousPage
            //if current displayed page is the last page, disable btnNextPageS
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                SubmittedDocumentModel model = (SubmittedDocumentModel)form.RetrieveSubmittedDocument(hiddenRandomKey.Text);
                hdnTotalPages.Value = 1;
                RetrieveSubmittedDocument(model);
            }
        }

        public void RetrieveSubmittedDocument(SubmittedDocumentModel model)
        {
            var totalPages = model.AvailableSubmitDocument.Count();
            var submittedDocument = model;
            lblDateSubmitted.Text = String.Format("{0:MMMM d, yyyy}", model.DateSubmitted);
            lblDescription.Text = model.DocumentDescription;
            panelDocument.Title = model.ProductRequiredDocumentName;
            hdnTotalPages.Value = model.AvailableSubmitDocument.Count();
            strPageGridPanel.DataSource = model.AvailableSubmitDocument;
            strPageGridPanel.DataBind();
        }

        //public void NextPage(SubmittedDocumentModel model)
        //{
        //    var totalPages = model.AvailableSubmitDocument.Count();
        //    int count = Convert.ToInt32(hdnTotalPages.Value);
        //    if (count <= totalPages)
        //    {
        //        var urlPath = new Uri(model.AvailableSubmitDocument.SingleOrDefault(a => a.DocumentId == count).FilePath);
        //        var urlRoot = new Uri(Server.MapPath("~") + "/");
        //        string relative = urlRoot.MakeRelativeUri(urlPath).ToString();
        //        imgPersonPicture.ImageUrl = "../" + relative;
        //        count++;
        //    }
        //    else
        //    {
        //        btnNextPage.Disabled = true;
        //    }
        //}

        protected void btnDeleteLastPage_Click(object sender, DirectEventArgs e)
        {
        }

        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            //same as approve button in create loan aplication
        }

        protected void btnReject_Click(object sender, DirectEventArgs e)
        {
            //same as reject button in create loan aplication
        }
    }
}