using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication
{
    public partial class ListFinancialProducts : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.Name = txtSearch.Text;
            dataSource.ProductStatusName = cmbFilterBy.SelectedItem.Text;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.LoginInfo.UserType == UserAccountType.Accountant.Name ||
                    this.LoginInfo.UserType == UserAccountType.Teller.Name)
                {
                    btnDelete.Hidden = true;
                    btnEdit.Hidden = true;
                    btnNew.Hidden = true;
                    btnActivateProduct.Hidden = true;
                    btnDeactivate.Hidden = true;
                    btnRetire.Hidden = true;
                    btnDeleteSeparator.Hidden = true;
                    btnEditSeparator.Hidden = true;
                    btnNewSeparator.Hidden = true;
                    btnActivateSeparator.Hidden = true;
                    btnDeactivateSeparator.Hidden = true;
                }

                FilterByStore.DataSource = ProductStatusType.All;
                FilterByStore.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            foreach (SelectedRow rows in selected)
            {
                int id = int.Parse(rows.RecordID);
                if (CanDeleteFinancialProduct(id))
                {
                    using (var unitOfWork = new UnitOfWorkScope(true))
                    {
                        FinancialProduct.Delete(id);
                    }
                }
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        public bool CanDeleteFinancialProduct(int id)
        {
            bool result = true;
            var financialProduct = FinancialProduct.GetById(id);
            ProductStatu status = ProductStatu.GetActive(financialProduct);
            var productIsUsed = FinancialProduct.CheckIfProductIsUsed(financialProduct);
            if (productIsUsed == true)
                return false;
            else
            {
                if (status != null && status.ProductStatusType.Id == ProductStatusType.ActiveType.Id)
                {
                    var query = ApplicationItem.All(financialProduct);
                    result = query.Count() == 0;
                }
                return result;
            }
        }

        [DirectMethod]
        public bool CanDeleteFinancialProduct(int[] ids)
        {
            bool result = true;
            foreach (int id in ids)
            {
                result = CanDeleteFinancialProduct(id);
                if (result == false)
                    break;
            }

            return result;
        }

        [DirectMethod]
        public bool CanEditFinancialProduct(int id)
        {
            var financialProduct = FinancialProduct.GetById(id);
            ProductStatu status = ProductStatu.GetActive(financialProduct);
            return status.ProductStatusType.Id != ProductStatusType.RetiredType.Id;
        }

        protected void SearchProductList_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }

        protected void btnActivate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            foreach (SelectedRow rows in selected)
            {
                int id = int.Parse(rows.RecordID);
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    ProductStatu.ChangeStatus(id, ProductStatusType.ActiveType, today);
                }
            }
            this.PageGridPanelStore.DataBind();
        }

        protected void btnRetire_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            foreach (SelectedRow rows in selected)
            {
                int id = int.Parse(rows.RecordID);
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    ProductStatu status = ProductStatu.ChangeStatus(id, ProductStatusType.RetiredType, today);
                    status.Remarks = "Retire Manually";
                }
            }
            this.PageGridPanelStore.DataBind();
        }

        protected void btnDeactivate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            foreach (SelectedRow rows in selected)
            {
                int id = int.Parse(rows.RecordID);
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    ProductStatu.ChangeStatus(id, ProductStatusType.InactiveType, today);
                }
            }
            this.PageGridPanelStore.DataBind();
        }

        private class FinancialProductUIModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string IntroductionDate
            {
                get
                {
                    return this._IntroductionDate.ToString("yyyy-MM-dd");
                }
            }
            public string SalesDiscontinuationDate
            {
                get
                {
                    if (this._SalesDiscontinuationDate.HasValue)
                        return this._SalesDiscontinuationDate.Value.ToString("yyyy-MM-dd");
                    else
                        return string.Empty;
                }
            }

            public DateTime _IntroductionDate { get; set; }
            public DateTime? _SalesDiscontinuationDate { get; set; }
            public string Status { get; set; }

        }

        private class DataSource : IPageAbleDataSource<FinancialProductUIModel>
        {
            public string Name { get; set; }
            public string ProductStatusName { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    return Filter().Count();
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

            private IQueryable<FinancialProductUIModel> Filter()
            {
                var query = from fp in Context.FinancialProducts
                            join ps in Context.ProductStatus on fp.Id equals ps.FinancialProductId
                            join pst in Context.ProductStatusTypes on ps.StatusTypeId equals pst.Id
                            where ps.IsActive == true
                            select new FinancialProductUIModel
                            {
                                ID = fp.Id,
                                Name = fp.Name,
                                _IntroductionDate = fp.IntroductionDate,
                                _SalesDiscontinuationDate = fp.SalesDiscontinuationDate,
                                Status = pst.Name
                            };

                    query = query.Where(entity => entity.Name.Contains(this.Name));
                if (string.IsNullOrWhiteSpace(this.ProductStatusName) == false)
                    query = query.Where(entity=> entity.Status == this.ProductStatusName);
                return query;
            }

            public override List<FinancialProductUIModel> SelectAll(int start, int limit, Func<FinancialProductUIModel, string> orderBy)
            {
                List<FinancialProductUIModel> financialProduct;
                var query = Filter();
                financialProduct = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                
                return financialProduct;
            }
        }
    }
}