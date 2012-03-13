using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.EmployeeUseCases
{
    public partial class ListAllowedEmployees : ActivityPageBase
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
                allowed.Add("Super Admin");
                return allowed;
            }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.Name = "";

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private class DataSource : IPageAbleDataSource<AllowedEmployeeModel>
        {
            public string Name { get; set; }

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

            public IQueryable<AllowedEmployeeModel> CreateQuery()
            {
               //var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var query = from ae in ObjectContext.AllowedEmployeeViewLists
                            select new AllowedEmployeeModel()
                            {
                                PartyId = ae.PartyId,
                                Name = ae.Name
                            };

                //var newquery = from p in ObjectContext.Parties.Where(p => p.PartyTypeId == PartyType.PersonType.Id)
                //               join prole in ObjectContext.PartyRoles on p.Id equals prole.PartyId
                //               join prel in ObjectContext.PartyRelationships on prole.Id equals prel.FirstPartyRoleId
                //               where (prole.RoleTypeId == RoleType.CustomerType.Id
                //                   || prole.RoleTypeId == RoleType.ContactType.Id
                //                   || prole.RoleTypeId == RoleType.EmployerType.Id)
                //                   && prole.EndDate == null && prel.EndDate == null
                //               select p;

                //var allowed = new List<AllowedEmployeeModel>();
                //foreach (var item in newquery)
                //{
                //    allowed.Add(new AllowedEmployeeModel(item));
                //}

                return query;
            }

            public override List<AllowedEmployeeModel> SelectAll(int start, int limit, Func<AllowedEmployeeModel, string> orderBy)
            {
                List<AllowedEmployeeModel> collection;
                var query = CreateQuery().OrderBy(orderBy).Skip(start).Take(limit);
                collection = query.ToList();

                return collection;
            }
        }

        public class AllowedEmployeeModel
        {
            public int PartyId { get; set; }
            public string Name { get; set; }

            public AllowedEmployeeModel(Party party)
            {
                this.PartyId = party.Id;
                this.Name = party.Name;
            }

            public AllowedEmployeeModel()
            {

            }
        }
    }
}