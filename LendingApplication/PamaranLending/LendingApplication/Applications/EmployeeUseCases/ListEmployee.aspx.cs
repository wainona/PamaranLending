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

    public partial class ListEmployee : ActivityPageBase
    {
        public const string Name = "Name";
        public const string EmployeeIdNumber = "Employee ID Number";
        public const string Employed = "Employed";
        public const string Retired = "Retired";
        public const string All = "All";

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
            string filter = cmbFilterBy.SelectedItem.Text;
            if (filter == All) filter = string.Empty;
            dataSource.FilterBy = filter;
            dataSource.SearchBy = cmbSearchBy.SelectedItem.Text;
           
            var currentEmployee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
            if (currentEmployee != null)
            {
                dataSource.EmployeeId = currentEmployee.PartyRoleId;
               //  dataSource.EmployeeTypeId = currentEmployee.PartyRole.Party.Person.UserAccounts.FirstOrDefault(entity => entity.EndDate == null).UserAccountType.Id;
            }
            
            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisableAddButtonBecauseLenderInformationIsNotSet();
            
        }

        protected void DisableAddButtonBecauseLenderInformationIsNotSet()
        {
            using(var ctx = new FinancialEntities()) 
            {
                var lendingInstitution = ctx.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
                if (lendingInstitution == null) return;
               
                btnAdd.Disabled = false;
                X.QuickTips.Disable();
            }
        }

        public bool CanDeleteEmployee(int id)
        {
            bool result = true;
            return result;
        }

        [DirectMethod]
        public bool CanDeleteEmployee(int[] ids)
        {
            bool result = true;
            foreach (int id in ids)
            {
                result = CanDeleteEmployee(id);
                if (result == false)
                    break;
            }

            return result;
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            DateTime today = DateTime.Now;
            RowSelectionModel sm = this.PageGridPanelSelectionModel;
            SelectedRowCollection rows = sm.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                int partyRoleId = int.Parse(row.RecordID);
                if (PartyRelationship.GetPartyRelationship(partyRoleId, lenderPartyRole.Id) == null)
                    throw new AccessToDeletedRecordException("The employee record has been deleted by another user.");
                //Employee employee = ObjectContext.Employees.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
                //if (employee != null) //other user may have del
                //    ObjectContext.Employees.DeleteObject(employee);

                var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId && entity.EndDate == null);
                var partyRel = ObjectContext.PartyRelationships.SingleOrDefault(entity => entity.EndDate == null && entity.FirstPartyRoleId == partyRole.Id
                                            && entity.SecondPartyRoleId == lenderPartyRole.Id && entity.PartyRelTypeId == PartyRelType.EmploymentType.Id);
                var userAccount = ObjectContext.UserAccounts.SingleOrDefault(entity => entity.EndDate == null && entity.PartyId == partyRole.PartyId);
                if (userAccount != null)
                    userAccount.EndDate = today;
                partyRel.EndDate = today;
            }

            ObjectContext.SaveChanges();
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void DeleteUserAccount(UserAccount userAccount)
        {
            DateTime today = DateTime.Today;
            var currentUserAccountStatus = ObjectContext.UserAccountStatus.SingleOrDefault(entity => entity.UserAccountId == userAccount.Id && entity.EndDate == null);
            currentUserAccountStatus.EndDate = today;
            userAccount.EndDate = today;
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanelStore.DataBind();
        }

        private class EmployeeModel
        {
            public int EmployeeId { get; set; }
            public string EmployeeIdNumber { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string EmploymentStatus { get; set; }
        }

        private class DataSource : IPageAbleDataSource<EmployeeModel>
        {
            public string SearchString { get; set; }
            public string FilterBy { get; set; }
            public string SearchBy { get; set; }
            public int EmployeeId { get; set; }
     

            public DataSource()
            {
                this.SearchString = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    var query = CreateQuery();
                    count = query.Count();
                    return count;
                }
            }

            public IQueryable<EmployeeModel> CreateQuery()
            {
                
                var query = from em in ObjectContext.EmployeeViewLists
                            select new EmployeeModel
                            {
                                EmployeeId = em.EmployeeId,
                                EmployeeIdNumber = em.EmployeeIdNumber,
                                Name = em.Name,
                                Address = em.Address,
                                EmploymentStatus = em.EmploymentStatus,
                            };
         
                switch (SearchBy)
                {
                    case EmployeeIdNumber:
                        query = query.Where(entity => entity.EmployeeIdNumber.Contains(SearchString) && entity.EmploymentStatus.Contains(FilterBy));
                        break;
                    case Name:
                        query = query.Where(entity => entity.Name.Contains(SearchString) && entity.EmploymentStatus.Contains(FilterBy));
                        break;
                    default:
                        query = query.Where(entity => entity.EmploymentStatus.Contains(FilterBy));
                        break;
                }

                return query;
            }

            public override List<EmployeeModel> SelectAll(int start, int limit, Func<EmployeeModel, string> orderBy)
            {
                List<EmployeeModel> employees;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery();
                    employees = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return employees;
            }
        }
    }
}