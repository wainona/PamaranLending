using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.CustomerClassificationUseCases
{
    public partial class CustomerType : ActivityPageBase
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
            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
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
                CustomerCategoryType districtType = ObjectContext.CustomerCategoryTypes.SingleOrDefault(entity => entity.Id == id);
                if (districtType != null) //other user may have del
                    ObjectContext.CustomerCategoryTypes.DeleteObject(districtType);
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

        [DirectMethod(ShowMask=true,Msg="Saving customer type record...")]
        public void SaveNewType()
        {
            int id = int.Parse(hdnCustomerTypeId.Text);
            if (id == -1)
            {
                CustomerCategoryType type = new CustomerCategoryType();
                type.Name = txtCustomerType.Text;
                ObjectContext.CustomerCategoryTypes.AddObject(type);

            }
            else
            {
                CustomerCategoryType type = ObjectContext.CustomerCategoryTypes.FirstOrDefault(entity => entity.Id == id);
                type.Name = txtCustomerType.Text;

            }
            ObjectContext.SaveChanges();
        }

        [DirectMethod]
        public bool CheckExistence()
        {
            var customerType = txtCustomerType.Text;

            int id = int.Parse(hdnCustomerTypeId.Text);
            CustomerCategoryType type = ObjectContext.CustomerCategoryTypes.FirstOrDefault(entity => entity.Id == id);
            if (type != null)
                if (customerType.ToLower().Equals(type.Name.ToLower())) return false;

            var exist = ObjectContext.CustomerCategoryTypes.Where(x => x.Name.ToLower().Equals(customerType.ToLower()));
          
            if (exist.Count() > 0)
                return true;
            return false;
        }

        [DirectMethod]
        public bool CanDeleteCustomerType(int id)
        {
            int count = 0;
            count = ObjectContext.CustomerCategories.Where(entity => entity.CustomerCategoryType== id).Count();
            return count == 0;
        }

        [DirectMethod]
        public void OpenCustomerType(int id)
        {
            var customerType = ObjectContext.CustomerCategoryTypes.FirstOrDefault(entity => entity.Id == id);
            txtCustomerType.Text = customerType.Name;
            hdnCustomerTypeId.Value = id;
        }
        private class DataSource : IPageAbleDataSource<CustomerTypeModel>
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
            private IQueryable<CustomerTypeModel> CreateQuery(FinancialEntities context)
            {
                var query = from ct in context.CustomerCategoryTypes
                            select new CustomerTypeModel
                            {
                                Id = ct.Id,
                                Name = ct.Name
                            };

                if(string.IsNullOrWhiteSpace(SearchString) == false)
                    query = query.Where(e => e.Name.ToLower().Contains(SearchString.ToLower()));

                return query;
            }

            public override List<CustomerTypeModel> SelectAll(int start, int limit, Func<CustomerTypeModel, string> orderBy)
            {
                List<CustomerTypeModel> classification;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    classification = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return classification;
            }
        }

        private class CustomerTypeModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}