using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.BestPractice
{
    public partial class PickListRequiredDocumentType : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.Name = txtSearch.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);
            }
        }

        protected void SearchProductList_Click(object sender, DirectEventArgs e)
        {
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
                    count = Context.RequiredDocumentTypes.Where(entity => entity.Name.Contains(Name)).Count();
                    return count;
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

            public override List<RequiredDocumentType> SelectAll(int start, int limit, Func<RequiredDocumentType, string> orderBy)
            {
                List<RequiredDocumentType> requiredDocument;
                var query = Context.RequiredDocumentTypes.Where(entity => entity.Name.Contains(Name)).OrderBy(orderBy).Skip(start).Take(limit);
                requiredDocument = query.ToList();
                return requiredDocument;
            }
        }
    }
}