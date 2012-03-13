using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.CustomerClassificationUseCases
{
    public partial class DistrictTypes : ActivityPageBase
    {
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

        public class DistrictTypeModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //SearchBy searchBy = (SearchBy)cmbSearch.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            //dataSource.SearchBy = searchBy;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSaveNewDistrictType_Click(object sender, DirectEventArgs e)
        {
            var districtType = txtDistrictType.Text;


            DistrictType type = new DistrictType();
            type.Name = districtType;

            ObjectContext.DistrictTypes.AddObject(type);
            ObjectContext.SaveChanges();
        }

        [DirectMethod]
        public bool CheckExistence()
        {
            var districtType = txtDistrictType.Text;
            var exist = ObjectContext.DistrictTypes.Where(x => x.Name.ToLower() == districtType.ToLower());
            //foreach (var x in exist)
            //{
            //    if( x.Name.to

            //}
            if (exist.Count() > 0)
                return true;
            return false;
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = this.PageGridPanelSelectionModel;
            SelectedRowCollection rows = sm.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                int id = int.Parse(row.RecordID);
                DistrictType districtType = ObjectContext.DistrictTypes.SingleOrDefault(entity => entity.Id == id);
                if (districtType != null) //other user may have del
                    ObjectContext.DistrictTypes.DeleteObject(districtType);
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
        public bool CanDeleteDistrictType(int id)
        {
            int count = 0;
            count = ObjectContext.ClassificationTypes.Where(entity => entity.DistrictTypeId == id).Count();
            return count == 0;
        }

        private class DataSource : IPageAbleDataSource<DistrictTypeModel>
        {
            public string SearchString { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
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
            private IQueryable<DistrictTypeModel> CreateQuery(FinancialEntities context)
            {
                var query = from ct in context.DistrictTypes
                            select new DistrictTypeModel
                            {
                                Id = ct.Id,
                                Name = ct.Name
                            };

                query = query.Where(entity => entity.Name.Contains(SearchString));
                return query;
            }

            public override List<DistrictTypeModel> SelectAll(int start, int limit, Func<DistrictTypeModel, string> orderBy)
            {
                List<DistrictTypeModel> classification;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    classification = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return classification;
            }
        }
    }
}