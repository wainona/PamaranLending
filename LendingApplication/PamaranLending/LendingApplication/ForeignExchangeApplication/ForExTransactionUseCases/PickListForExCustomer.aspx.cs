using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;
using System.Text;

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class PickListForExCustomer : ActivityPageBase
    {

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
            int id = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
            DataSource dataSource = new DataSource();
            dataSource.PartyId = id;
            e.Total = dataSource.Count;
            var list = dataSource.SelectAll(e.Start, e.Limit, (entity => entity.Name));
            this.PageGridPanelStore.DataSource = list;
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //hdnPartyRoleId.Text = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId.ToString();
            }
        }

        public class DataSource : IPageAbleDataSource<ForExCustomerViewModel>
        {
            public int PartyRoleId { get; set; }
            public int PartyId { get; set; }

            private IEnumerable<ForExCustomerViewModel> CreateQuery()
            {
                var queryCustomers = (from pr in ObjectContext.PartyRoles
                                     where pr.RoleTypeId == RoleType.CustomerType.Id
                                     select new ForExCustomerViewModel()
                                     {
                                         PartyId = pr.PartyId,
                                         PartyRoleId = pr.Id,
                                     }).ToList();

                var queryContact = (from pr in ObjectContext.PartyRoles
                                   where pr.RoleTypeId == RoleType.ContactType.Id
                                   select new ForExCustomerViewModel()
                                   {
                                       PartyId = pr.PartyId,
                                       PartyRoleId = pr.Id,
                                   }).ToList();

                return queryCustomers.Concat(queryContact);
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery();
                        count = query.Count();
                    }

                    return count;
                }
            }

            public override List<ForExCustomerViewModel> SelectAll(int start, int limit, Func<ForExCustomerViewModel, string> orderBy)
            {
                var query = CreateQuery().ToList();
                query.OrderBy(orderBy).Skip(start).Take(limit);
                return query;
            }
        }

        public class ForExCustomerViewModel
        {
            public int PartyId { get; set; }
            public int PartyRoleId { get; set; }
            public string Name
            {
                get
                {
                    return Party.GetById(this.PartyId).Name;
                }
            }
            public string Address
            {
                get
                {
                    var postalAddress = PostalAddress.GetCurrentPostalAddress(Party.GetById(this.PartyId), PostalAddressType.HomeAddressType, true);
                    var muncity = string.Empty;
                    if(string.IsNullOrWhiteSpace(postalAddress.City))
                        muncity = postalAddress.Municipality;
                    else
                        muncity = postalAddress.City;

                    return StringConcatUtility.Build(", ", postalAddress.StreetAddress, postalAddress.Barangay, muncity, postalAddress.Province, postalAddress.Country.Name, postalAddress.PostalCode);
                }
            }
        }
    }
}