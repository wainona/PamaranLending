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
    public partial class PickListFinancialProduct : ActivityPageBase
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

        private class FinancialProductUIModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string IntroductionDate
            {
                get
                {
                    return this._IntroductionDate.ToString("MMM dd, yyyy");
                }
            }
            public string SalesDiscontinuationDate
            {
                get
                {
                    if (this._SalesDiscontinuationDate.HasValue)
                        return this._SalesDiscontinuationDate.Value.ToString("MMM dd, yyy");
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
                            where ps.IsActive == true &&
                                  ps.StatusTypeId == ProductStatusType.ActiveType.Id
                            select new FinancialProductUIModel
                            {
                                ID = fp.Id,
                                Name = fp.Name,
                                _IntroductionDate = fp.IntroductionDate,
                                _SalesDiscontinuationDate = fp.SalesDiscontinuationDate,
                                Status = pst.Name
                            };

                var result = query.Where(entity => entity.Name.Contains(this.Name));

                return result;
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