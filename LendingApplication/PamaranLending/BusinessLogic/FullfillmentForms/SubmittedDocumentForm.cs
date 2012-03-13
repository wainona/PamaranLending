using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class DocumentPageModel : BusinessObjectModel
    {
        public int DocumentId { get; set; }
        public int PageNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public DocumentPageModel(int pageNumber, string fileName, string filePath)
        {
            this.PageNumber = pageNumber;
            this.FileName = fileName;
            this.FilePath = filePath;
        }

        public DocumentPageModel(DocumentPage page)
        {
            this.IsNew = false;
            this.FileName = page.ImageFilename;
            this.FilePath = page.ImageFilePath;
            this.PageNumber = page.Id;
            this.DocumentId = page.Id;
        }
    }

    public class SubmittedDocumentModel : BusinessObjectModel
    {
        public int SubmittedDocumentId { get; set; }
        public int ProductRequiredDocumentId { get; set; }
        public string ProductRequiredDocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get { return "Approved"; } }


        List<DocumentPageModel> List = new List<DocumentPageModel>();

        public void AddSubmitDocument(DocumentPageModel item)
        {
            if (this.List.Contains(item) == false)
                this.List.Add(item);
        }

        public void RemoveSubmitDocument(string randomKey)
        {
            DocumentPageModel doc = this.List.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (doc != null)
                RemoveSubmitDocument(doc);
        }

        public void RemoveSubmitDocument(DocumentPageModel item)
        {
            if (this.List.Contains(item) == true)
            {
                if (item.IsNew)
                    List.Remove(item);
                else
                    item.MarkDeleted();
            }
        }

        public IEnumerable<DocumentPageModel> AvailableSubmitDocument
        {
            get
            {
                return this.List.Where(model => model.ToBeDeleted == false);
            }
        }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        private static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public SubmittedDocumentModel()
        {
            this.IsNew = true;
        }

        public SubmittedDocumentModel(SubmittedDocument submittedDocument)
        {
            this.IsNew = false;
            this.SubmittedDocumentId = submittedDocument.Id;
            this.ProductRequiredDocumentId = submittedDocument.ProductRequiredDocumentId;
            this.ProductRequiredDocumentName = submittedDocument.ProductRequiredDocument.RequiredDocumentType.Name;
            this.DocumentDescription = submittedDocument.Description;
            this.DateSubmitted = submittedDocument.DateSubmitted;

            foreach (var item in submittedDocument.DocumentPages)
            {
                List.Add(new DocumentPageModel(item));
            }
        }

        public void PrepareForSave(LoanApplication loanApplication, DateTime today)
        {
            if (this.IsNew)
            {
                SubmittedDocument submittedDocument = new SubmittedDocument();
                submittedDocument.ProductRequiredDocumentId = this.ProductRequiredDocumentId;
                submittedDocument.ApplicationId = loanApplication.ApplicationId;
                submittedDocument.DateSubmitted = this.DateSubmitted;
                submittedDocument.Description = this.DocumentDescription;

                SaveDocumentPage(submittedDocument);

                SubmittedDocumentStatu documentStatus = new SubmittedDocumentStatu();
                var statusType = Context.SubmittedDocumentStatusTypes.SingleOrDefault(entity => entity.Name == "Approved");
                documentStatus.StatusTypeId = statusType.Id;
                documentStatus.SubmittedDocument = submittedDocument;
                documentStatus.TransitionDateTime = today;
                documentStatus.IsActive = true;

                Context.SubmittedDocuments.AddObject(submittedDocument);
                Context.SubmittedDocumentStatus.AddObject(documentStatus);
            }
            else if (this.ToBeDeleted)
            {
                SubmittedDocument submittedDocument = Context.SubmittedDocuments.SingleOrDefault(entity => entity.Id == this.SubmittedDocumentId);
                foreach (var status in submittedDocument.SubmittedDocumentStatus.ToList())
                {
                    Context.SubmittedDocumentStatus.DeleteObject(status);
                }
                foreach (var documentPage in submittedDocument.DocumentPages.ToList())
                {
                    Context.DocumentPages.DeleteObject(documentPage);
                }

                Context.SubmittedDocuments.DeleteObject(submittedDocument);
            }else if(this.IsNew == false && this.ToBeDeleted == false)
            {
                SubmittedDocument submittedDocument = new SubmittedDocument();
                submittedDocument.ProductRequiredDocumentId = this.ProductRequiredDocumentId;
                submittedDocument.LoanApplication = loanApplication;
                submittedDocument.DateSubmitted = this.DateSubmitted;
                submittedDocument.Description = this.DocumentDescription;

                SaveDocumentPage(submittedDocument);

                SubmittedDocumentStatu documentStatus = new SubmittedDocumentStatu();
                var statusType = Context.SubmittedDocumentStatusTypes.SingleOrDefault(entity => entity.Name == "Approved");
                documentStatus.StatusTypeId = statusType.Id;
                documentStatus.SubmittedDocument = submittedDocument;
                documentStatus.TransitionDateTime = today;
                documentStatus.IsActive = true;

                Context.SubmittedDocuments.AddObject(submittedDocument);
                Context.SubmittedDocumentStatus.AddObject(documentStatus);
            }
        }

        private void SaveDocumentPage(SubmittedDocument submittedDocument)
        {
            foreach (DocumentPageModel model in List)
            {
                if (model.IsNew)
                {
                    DocumentPage documentPage = new DocumentPage();
                    documentPage.SubmittedDocument = submittedDocument;
                    documentPage.ImageFilename = model.FileName;
                    documentPage.ImageFilePath = model.FilePath;
                    Context.DocumentPages.AddObject(documentPage);
                }
            }
        }
    }
}
