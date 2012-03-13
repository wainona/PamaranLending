using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class ReceiptPickList : ActivityPageBase
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
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
            }
        }


        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            SearchBy searchBy = (SearchBy)cmbSearchBy.SelectedIndex;

            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearch.Text;
            dataSource.SearchBy = searchBy;
            dataSource.UserID = int.Parse(txtUserID.Value.ToString());

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }


        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }

        public enum SearchBy
        {
            Name = 0,
            None = -1
        }

        private class CustomerReceiptModel
        {
            public int ReceiptID { get; set; }
            public int? PartyRoleId { get; set; }
            public Party Party {get;set;}
            public string Name
            {
                get
                {
                    if (this.Party != null)
                        return Party.Name;
                    else
                        return string.Empty;
                }
            }
            public decimal? Balance { get; set; }
        }
        private class DataSource : IPageAbleDataSource<CustomerReceiptModel>
        {
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public int UserID { get; set; }


            public DataSource()
            {
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
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

            private IEnumerable<CustomerReceiptModel> CreateQuery(FinancialEntities context)
            {
                var query = from r in context.Receipts
                            join ra in context.ReceiptPaymentAssocs on r.Id equals ra.ReceiptId
                            join p in context.Payments on ra.PaymentId equals p.Id
                            join pr in context.PartyRoles on p.ProcessedToPartyRoleId equals pr.Id
                            join par in context.Parties on pr.PartyId equals par.Id
                            join rs in context.ReceiptStatus on r.Id equals rs.ReceiptId
                            where rs.ReceiptStatusTypeId == ReceiptStatusType.AppliedReceiptStatusType.Id
                            && rs.IsActive==true && pr.PartyId != UserID && p.PaymentTypeId == PaymentType.Receipt.Id
                            select new CustomerReceiptModel()
                            {
                               PartyRoleId = p.ProcessedToPartyRoleId,
                               Party = par,
                                Balance = r.ReceiptBalance,
                                ReceiptID = r.Id
                            };
                IEnumerable<CustomerReceiptModel> result = query.ToList();

              if(string.IsNullOrWhiteSpace(SearchString) == false)
                    result = result.Where(entity => entity.Name.Contains(SearchString));

                return result;
            }

            public override List<CustomerReceiptModel> SelectAll(int start, int limit, Func<CustomerReceiptModel, string> orderBy)
            {
                List<CustomerReceiptModel> collection;
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