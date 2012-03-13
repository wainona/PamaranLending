using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UserAccount
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        internal static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public UserAccountStatu CurrentStatus
        {
            get
            {
                return this.UserAccountStatus.SingleOrDefault(entity => entity.EndDate == null);
            }
        }

        public static UserAccount GetById(int id)
        {
            return ObjectContext.UserAccounts.SingleOrDefault(entity => entity.Id == id);
        }
        public static Employee GetAssociatedEmployee(int id)
        {
            var userAccount = ObjectContext.UserAccounts.SingleOrDefault(entity => entity.Id ==id && entity.EndDate == null);
            var employeePartyRole = PartyRole.GetByPartyIdAndRole(userAccount.PartyId.Value, RoleType.EmployeeType);
            return employeePartyRole.Employee;
        }
        public static IQueryable<UserAccount> All()
        {
            var userAccounts = from ua in ObjectContext.UserAccounts
                               join uas in ObjectContext.UserAccountStatus on ua.Id equals uas.UserAccountId
                               where uas.EndDate == null && ua.EndDate == null
                               select ua;

            return userAccounts;
        }

        public static IEnumerable<UserAccountViewModel> AllInModel()
        {
            var userAccounts = from ua in ObjectContext.UserAccounts
                               join uas in ObjectContext.UserAccountStatus on ua.Id equals uas.UserAccountId
                               where uas.EndDate == null && ua.EndDate == null
                               select ua;

            List<UserAccountViewModel> list = new List<UserAccountViewModel>();
            foreach (var userAccount in userAccounts.ToList())
            {
                list.Add(new UserAccountViewModel(userAccount));
            }
            return list;
        }
    }

    public class UserAccountViewModel
    {
        public int UserAccountId { get; set; }
        public string NameOfUser { get; set; }
        public string Username { get; set; }
        public string UserAccountStatus { get; set; }
        public int UserAccountTypeId { get; set; }
        public string UserAccountType { get; set; }
        public UserAccount UserAccount { get; private set; }

        public UserAccountViewModel(UserAccount userAccount)
        {
            this.UserAccount = userAccount;
            this.UserAccountId = userAccount.Id;
            this.Username = userAccount.Username;
            this.UserAccountTypeId = userAccount.UserAccountTypeId;
            this.UserAccountType = userAccount.UserAccountType.Name;
            this.UserAccountStatus = userAccount.CurrentStatus.UserAccountStatusType.Name;
            if (userAccount.PartyId.HasValue)
            {
                Party party = UserAccount.ObjectContext.Parties.SingleOrDefault(entity => entity.Id == userAccount.PartyId);
                if (party.PartyTypeId == PartyType.PersonType.Id && party.Person != null)
                {
                    Person person = party.Person;
                    this.NameOfUser = StringConcatUtility.Build(" ", person.LastNameString + ","
                        , person.FirstNameString, person.MiddleInitialString,
                        person.NameSuffixString);
                }
            }
        }
    }
}
