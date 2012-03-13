using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases
{
    public partial class ListCustomerClassification : ActivityPageBase
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.District);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.PageGridPanelSelectionModel;
            SelectedRowCollection rows = sm.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                int id = int.Parse(row.RecordID);
                ClassificationType classification = ObjectContext.ClassificationTypes.SingleOrDefault(entity => entity.Id == id);
                if (classification != null) //other user may have del
                    ObjectContext.ClassificationTypes.DeleteObject(classification);
            }
            ObjectContext.SaveChanges();
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanel.DataBind();
        }

        [DirectMethod]
        public bool CanDeleteCustomerClassifications(int id)
        {
            int count = 0;
            count = ObjectContext.CustomerClassifications.Where(entity => entity.ClassificationTypeId == id).Count();
            return count == 0;
        }

        public enum SearchBy
        { 
            District = 0,
            StationNumber = 1,
            DistrictType = 2,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<ClassificationModel>
        {
            public string SearchString { get; set; }

            public SearchBy SearchBy { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.SearchBy = ListCustomerClassification.SearchBy.None;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        count = query.Count();
                    }

                    return count;
                }
            }

            //private Func<ClassificationType, bool> CreateQuery()
            private IQueryable<ClassificationModel> CreateQuery(FinancialEntities context)
            {
                var query = from ct in context.ClassificationTypes
                            select new ClassificationModel
                            {
                                Id = ct.Id,
                                District = ct.District,
                                StationNumber = ct.StationNumber,
                                Type = ct.DistrictType.Name
                            };

                switch (SearchBy)
                {
                    case SearchBy.District:
                        query = query.Where(entity => entity.District.Contains(SearchString));
                        break;
                    case SearchBy.None:
                        query = query.Where(entity => entity.District.Contains(SearchString));
                        break;
                    case SearchBy.StationNumber:
                        query = query.Where(entity => entity.StationNumber.Contains(SearchString));
                        break;
                    case SearchBy.DistrictType:
                        query = query.Where(entity => entity.Type.Contains(SearchString));
                        break;
                    default:
                        break;
                }
                return query;
            }

            public override List<ClassificationModel> SelectAll(int start, int limit, Func<ClassificationModel, string> orderBy)
            {
                List<ClassificationModel> classification;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    classification = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return classification;
            }
        }

        public class ClassificationModel
        {
            public int Id { get; set; }
            public string District { get; set; }
            public string StationNumber { get; set; }
            public string Type { get; set; }
        }
    }
}