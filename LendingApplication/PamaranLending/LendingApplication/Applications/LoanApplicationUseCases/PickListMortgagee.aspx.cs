using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication
{
    public partial class PickListMortgagee : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource ds = new DataSource();


            e.Total = ds.Count;
            StoreMortgagee.DataSource = ds.SelectAll(e.Start, e.Limit, entity => entity.Name);
            StoreMortgagee.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.PageGridPanelSelectionModel.SingleSelect = mode == "single" || string.IsNullOrWhiteSpace(mode);
            }
        }

        private class DataSource : IPageAbleDataSource<AllowedMortgageeViewList>
        {
            public DataSource()
            {
                
            }

            public override int Count
            {
                get
                {
                    return ObjectContext.AllowedMortgageeViewLists.Count();
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

            public override List<AllowedMortgageeViewList> SelectAll(int start, int limit, Func<AllowedMortgageeViewList, string> orderBy)
            {
                return ObjectContext.AllowedMortgageeViewLists.ToList();
            }
        }
    }
}