using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.EmployeeUseCases
{
    public partial class EmployeePickList : ActivityPageBase
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
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Teller");
                allowed.Add("Super Admin");
                return allowed;
            }
        }
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.UserID = int.Parse(txtUserID.Value.ToString());
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
            }
        }

        private class EmployeeModel
        {
            public int PartyRoleId { get; set; }
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
            public string Address
            {
                get
                {
                    Address postalAddress = Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                              && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

                    if (postalAddress != null && postalAddress.PostalAddress != null)
                    {
                        PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                        return StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                      postalAddressSpecific.Barangay,
                                      postalAddressSpecific.Municipality,
                                      postalAddressSpecific.City,
                                      postalAddressSpecific.Province,
                                      postalAddressSpecific.State,
                                      postalAddressSpecific.Country.Name,
                                      postalAddressSpecific.PostalCode);
                    }
                    else return string.Empty;
                }
            }
            public Party Party { get; set; }
        }

        private class DataSource : IPageAbleDataSource<EmployeeModel>
        {
            public string Name { get; set; }
            public int UserID { get; set; }
            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    count = CreateQuery().Count();
                    return count;
                }
            }

            private IQueryable<EmployeeModel> CreateQuery()
            {
                var lendingPartyRole = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
                var query = from pr in ObjectContext.PartyRelationships
                            join p in ObjectContext.PartyRoles on pr.FirstPartyRoleId equals p.Id
                            where pr.SecondPartyRoleId == lendingPartyRole.Id && p.PartyId != UserID
                            && pr.PartyRelTypeId == PartyRelType.EmploymentType.Id
                            select new EmployeeModel
                            {
                                PartyRoleId = p.Id,
                                Party = p.Party,
                            };

                IEnumerable<EmployeeModel> result = query.ToList();
                return query;
            }

            public override List<EmployeeModel> SelectAll(int start, int limit, Func<EmployeeModel, string> orderBy)
            {
                List<EmployeeModel> allowedUsers;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery().OrderBy(entity => entity.PartyRoleId);
                    allowedUsers = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return allowedUsers;
            }
        }
    }
}