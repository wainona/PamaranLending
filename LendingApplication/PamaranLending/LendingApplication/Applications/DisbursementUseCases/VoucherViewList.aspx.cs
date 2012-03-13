using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class VoucherViewList : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                allowed.Add("Cashier");
                return allowed;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
              //  txtUserID.Value = Request.QueryString["id"];
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
                using (var context = new FinancialEntities())
                {
                    var query = from p in context.ProductStatus where p.ProductStatusType.Name =="Active" select 
                                    new{Id = p.FinancialProduct.Id,
                                        Name = p.FinancialProduct.Name};
                    if (query != null)
                    {
                        storeFilter.DataSource = query.ToList();
                        storeFilter.DataBind();

                    }
                }
            }
        }
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            if(string.IsNullOrEmpty(cmbFilter.SelectedItem.Text)==false)dataSource.FilterString = cmbFilter.SelectedItem.Text;
            dataSource.UserID = int.Parse(txtUserID.Value.ToString());

            e.Total = dataSource.Count;
            this.storeVoucherList.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity=>entity.CustomerName); //change to Name ang Branch tab:D
            this.storeVoucherList.DataBind();
        }
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.storeVoucherList.DataBind();
        }

        private class VoucherModel
        {
            public int VoucherID { get; set; }
            public int AgreementID { get; set; }
            public string CustomerName
            {
                get
                {
                    if (this.Party != null)
                        return Party.Name;
                    else
                        return string.Empty;
                }
            }
            public int CustomerID {get; set;}
            public decimal? LoanAmount {get; set;}
            public string LoanProduct { get; set; }
            public int LoanProductID { get; set; }
            public Party Party { get; set; }

            
        }
        private class DataSource : IPageAbleDataSource<VoucherModel>
        {
            public string SearchString { get; set; }
            public string FilterString { get; set; }
            public int UserID { get; set; }

            public DataSource()
            {
                this.SearchString = string.Empty;
                this.FilterString = string.Empty;
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
            private IEnumerable<VoucherModel> CreateQuery(FinancialEntities context)
            {                       
                var tomorrow = DateTime.Today.AddDays(1);

                var query = from lvc in context.LoanDisbursementVcrs 
                            join lvs in context.DisbursementVcrStatus on lvc.Id equals lvs.LoanDisbursementVoucherId
                            join a in context.Agreements on lvc.AgreementId equals a.Id
                            join las in context.LoanApplicationStatus on a.ApplicationId equals las.ApplicationId
                            join ar in context.AgreementRoles on lvc.AgreementId equals ar.AgreementId
                            join pf in context.ProductFeatureApplicabilities on a.Application.ApplicationItems.FirstOrDefault().ProdFeatApplicabilityId equals pf.Id
                            join f in context.FinancialProducts on pf.FinancialProductId equals f.Id
                            join am in context.AmortizationSchedules on a.Id equals am.AgreementId
                            where ar.PartyRole.RoleTypeId == RoleType.BorrowerAgreementType.Id && lvs.IsActive == true
                            && (lvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.ApprovedType.Id ||lvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id)
                            && am.LoanReleaseDate <tomorrow && am.EndDate == null && a.EndDate == null && ar.PartyRole.PartyId != UserID
                            && las.IsActive == true && las.LoanApplicationStatusType.Id != LoanApplicationStatusType.CancelledType.Id
                            && las.LoanApplicationStatusType.Id != LoanApplicationStatusType.RejectedType.Id
                            select new VoucherModel()
                            {
                                VoucherID = lvc.Id,
                                AgreementID = a.Id,
                                CustomerID = ar.PartyRole.Id,
                                LoanAmount = lvc.Balance,
                                Party = ar.PartyRole.Party,
                                LoanProduct = f.Name,
                                LoanProductID = f.Id

                            };

                IEnumerable<VoucherModel> result = query.ToList();
                if (string.IsNullOrWhiteSpace(FilterString)==false)
                {
                    result = result.Where(entity => entity.LoanProduct.Contains(FilterString));
                }

                if(string.IsNullOrWhiteSpace(SearchString) == false)
                    result = result.Where(entity => entity.CustomerName.Contains(SearchString));

                return result;
            }

            public override List<VoucherModel> SelectAll(int start, int limit, Func<VoucherModel, string> orderBy)
            {
                List<VoucherModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }
                return collection;
            }
        }
    }
}