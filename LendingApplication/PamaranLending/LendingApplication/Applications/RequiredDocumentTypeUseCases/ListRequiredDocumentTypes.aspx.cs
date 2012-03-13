using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.RequiredDocumentTypeUseCases
{
    public partial class ListRequiredDocumentTypes : ActivityPageBase
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            //dataSource.Name = "";

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    RequiredDocumentType customer = context.RequiredDocumentTypes.SingleOrDefault(entity => entity.Id == id);
                    if (customer != null) //other user may have del
                    {
                        var usedDocumentType = context.ProductRequiredDocuments.FirstOrDefault(entity => entity.RequiredDocumentTypeId == customer.Id && entity.EndDate == null);
                        if (usedDocumentType == null)
                        {
                            context.RequiredDocumentTypes.DeleteObject(customer);
                        }
                        else
                        {
                            X.Msg.Alert("Alert", customer.Name + "cannot be deleted. Only unused required document types can be deleted.").Show();
                            e.Success = false;
                        }
                    }
                    else
                    {
                        e.Success = false;
                        throw new AccessToDeletedRecordException("The required document type has been deleted by another user.");
                        
                    }
                }
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        private class DataSource : IPageAbleDataSource<RequiredDocumentType>
        {
            public string Name { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        count = context.RequiredDocumentTypes.Where(entity => entity.Name.Contains(Name)).Count();
                    }

                    return count;
                }
            }

            public override List<RequiredDocumentType> SelectAll(int start, int limit, Func<RequiredDocumentType, string> orderBy)
            {
                List<RequiredDocumentType> customers;
                using (var context = new FinancialEntities())
                {
                    var query = context.RequiredDocumentTypes.Where(entity => entity.Name.Contains(Name)).OrderBy(orderBy).Skip(start).Take(limit);
                    customers = query.ToList();
                }

                return customers;
            }
        }
    }
}