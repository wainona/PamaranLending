using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace BusinessLogic
{
    public partial class Party
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        private static  FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public string Name 
        {
            get
            {
                string name;
                if (this.PartyType.Id == PartyType.PersonType.Id)
                    name = Person.GetPersonFullName(this);
                else
                    name = this.Organization.OrganizationName;

                return name;
            }
        }

        public string NameV2
        {
            get
            {
                string name;
                if (this.PartyType.Id == PartyType.PersonType.Id)
                    name = Person.GetPersonFullNameV2(this);
                else
                    name = this.Organization.OrganizationName;

                return name;
            }
        }

        public static Party GetById(int id)
        {
            return Context.Parties.SingleOrDefault(entity => entity.Id == id);
        }
        /// <summary>
        /// Retrieve all the customers with loans excluding the employee logging in as customer.
        /// </summary>
        /// <param name="UserID">Exclude the loans of the current user.</param>
        /// <returns></returns>
        public static IEnumerable<PersonWithLoan> GetAllCustomersWithLoan(int UserID)
        {

            var financialRoleAccount = from la in Context.LoanApplicationRoles
                                       join pr in Context.PartyRoles on la.PartyRoleId equals pr.Id
                                       where (pr.RoleTypeId == RoleType.BorrowerApplicationType.Id ||
                                         pr.RoleTypeId == RoleType.CoBorrowerApplicationType.Id ||
                                         pr.RoleTypeId == RoleType.GuarantorApplicationType.Id) &&
                                         pr.EndDate == null && pr.PartyId != UserID
                                       select la;


            var parties = from la in financialRoleAccount
                          join ag in Context.Agreements on la.ApplicationId equals ag.ApplicationId
                          join fa in Context.FinancialAccounts on ag.Id equals fa.AgreementId
                          join ls in Context.LoanAccountStatus on fa.Id equals ls.FinancialAccountId
                          where ls.IsActive == true && (ls.StatusTypeId == LoanAccountStatusType.CurrentType.Id
                          || ls.StatusTypeId == LoanAccountStatusType.DelinquentType.Id
                          || ls.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id) 
                          group la by la.PartyRole.Party into uniqueParty
                          select uniqueParty.Key;

            List<PersonWithLoan> personWithLoans = new List<PersonWithLoan>();
            foreach (var party in parties)
            {
                personWithLoans.Add(new PersonWithLoan(party));
            }

            return personWithLoans;
        }
        public static IEnumerable<CustomerWithLoan> GetAllCustomersWithLoan(RoleType role)
        {
            //var parties = from la in Context.LoanApplications
            //              join lar in Context.LoanApplicationRoles.Where(entity =>
            //    entity.PartyRole.RoleTypeId == role.Id && entity.PartyRole.EndDate == null) on la.ApplicationId equals lar.ApplicationId
            //              join status in Context.LoanApplicationStatus on la.ApplicationId equals status.ApplicationId
            //              join ag in Context.Agreements on la.ApplicationId equals ag.ApplicationId
            //              join fa in Context.FinancialAccounts on ag.Id equals fa.AgreementId
            //              join r in Context.Receivables on fa.Id equals r.FinancialAccountId
            //              join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
            //              where rs.IsActive == true && (rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
            //               || rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id) && status.IsActive == true &&
            //               (status.LoanApplicationStatusType.Id == LoanApplicationStatusType.ClosedType.Id ||
            //               status.LoanApplicationStatusType.Id == LoanApplicationStatusType.PendingInFundingType.Id)
            //              group lar by lar.PartyRole.Party into uniqueParty
            //              select uniqueParty.Key;
            var parties = from la in Context.LoanApplications
                          join lar in Context.LoanApplicationRoles.Where(entity =>
                entity.PartyRole.RoleTypeId == role.Id && entity.PartyRole.EndDate == null) on la.ApplicationId equals lar.ApplicationId
                          join status in Context.LoanApplicationStatus on la.ApplicationId equals status.ApplicationId
                          join ag in Context.Agreements on la.ApplicationId equals ag.ApplicationId
                          join fa in Context.FinancialAccounts on ag.Id equals fa.AgreementId
                          join ls in Context.LoanAccountStatus on fa.Id equals ls.FinancialAccountId
                            where  ls.IsActive == true && ls.StatusTypeId != LoanAccountStatusType.PaidOffType.Id &&  status.IsActive == true &&
                           (status.LoanApplicationStatusType.Id == LoanApplicationStatusType.ClosedType.Id ||
                           status.LoanApplicationStatusType.Id == LoanApplicationStatusType.PendingInFundingType.Id)
                          group lar by lar.PartyRole.Party into uniqueParty
                          select uniqueParty.Key;

            List<CustomerWithLoan> customerWithLoans = new List<CustomerWithLoan>();
            foreach (var party in parties)
            {
                customerWithLoans.Add(new CustomerWithLoan(party));
            }

            return customerWithLoans;
        }

        public static IEnumerable<CustomerWithLoan> GetAllCustomersWithLoanOriginal(RoleType role)
        {
            var parties = from la in Context.LoanApplications
                          join lar in Context.LoanApplicationRoles on la.ApplicationId equals lar.ApplicationId
                          join status in Context.LoanApplicationStatus on la.ApplicationId equals status.ApplicationId
                          join ag in Context.Agreements on la.ApplicationId equals ag.ApplicationId
                          join fa in Context.FinancialAccounts on ag.Id equals fa.AgreementId
                          join r in Context.Receivables on fa.Id equals r.FinancialAccountId
                          join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                          where status.IsActive == true && (status.LoanApplicationStatusType.Id == LoanApplicationStatusType.ClosedType.Id
                          || status.LoanApplicationStatusType.Id == LoanApplicationStatusType.PendingInFundingType.Id)
                          && lar.PartyRole.RoleTypeId == role.Id && lar.PartyRole.EndDate == null
                          group lar by lar.PartyRole.Party into uniqueParty
                          select uniqueParty.Key;

            List<CustomerWithLoan> customerWithLoans = new List<CustomerWithLoan>();
            foreach (var party in parties)
            {
                customerWithLoans.Add(new CustomerWithLoan(party));
            }

            return customerWithLoans;
        }

        public static IEnumerable<CustomerWithLoan> GetAllCustomersWithBilledLoans(RoleType role, DateTime today)
        {
            var yesterday = today.AddDays(-1);
            var tommorow = today.AddDays(1);
            var parties = from la in Context.LoanApplications
                          join lar in Context.LoanApplicationRoles.Where(entity =>
                entity.PartyRole.RoleTypeId == role.Id && entity.PartyRole.EndDate == null) on la.ApplicationId equals lar.ApplicationId
                          join status in Context.LoanApplicationStatus on la.ApplicationId equals status.ApplicationId
                          join ag in Context.Agreements on la.ApplicationId equals ag.ApplicationId
                          join fa in Context.FinancialAccounts on ag.Id equals fa.AgreementId
                          join r in Context.Receivables on fa.Id equals r.FinancialAccountId
                          join rs in Context.ReceivableStatus on r.Id equals rs.ReceivableId
                          where rs.IsActive == true && (rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
                           || rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id) && status.IsActive == true &&
                           (status.LoanApplicationStatusType.Id == LoanApplicationStatusType.ClosedType.Id ||
                           status.LoanApplicationStatusType.Id == LoanApplicationStatusType.PendingInFundingType.Id)
                           && r.Date > yesterday && r.Date < tommorow 
                          group lar by lar.PartyRole.Party into uniqueParty
                          select uniqueParty.Key;

            List<CustomerWithLoan> customerWithLoans = new List<CustomerWithLoan>();
            foreach (var party in parties)
            {
                customerWithLoans.Add(new CustomerWithLoan(party));
            }

            return customerWithLoans;

        }

        public static IEnumerable<CustomerWithLoan> GetAllCustomersWithDisbursedLoans(RoleType role, int employeePartyId)
        {
            var parties = from la in Context.LoanAccounts
                          join fa in Context.FinancialAccounts.Where(entity => entity.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id)
                                 on la.FinancialAccountId equals fa.Id
                          join far in Context.FinancialAccountRoles.Where(entity => entity.PartyRole.RoleTypeId == role.Id)
                                        on fa.Id equals far.FinancialAccountId
                          join ag in Context.Agreements on fa.AgreementId equals ag.Id
                          join ldv in Context.LoanDisbursementVcrs on ag.Id equals ldv.AgreementId
                          join dvs in Context.DisbursementVcrStatus on ldv.Id equals dvs.LoanDisbursementVoucherId
                          join las in Context.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                          join ags in Context.AgreementTypes on ag.AgreementTypeId equals ags.Id
                          where las.IsActive == true && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id 
                                                        || las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id
                                                        || las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                            && ags.Id == AgreementType.LoanAgreementType.Id && la.LoanBalance != 0 && ag.EndDate == null
                            && dvs.IsActive == true && (dvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.FullyDisbursedType.Id 
                                || dvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id) 
                            && far.PartyRole.PartyId != employeePartyId
                          group far by far.PartyRole.Party into uniqueParty
                          select uniqueParty.Key;

            List<CustomerWithLoan> customerWithLoans = new List<CustomerWithLoan>();
            foreach (var party in parties)
            {
                customerWithLoans.Add(new CustomerWithLoan(party));
            }

            return customerWithLoans;
        }

        public static IEnumerable<LoanAccountStatu> RetrieveOutstandingLoans(int partyId)
        {
            var financialAccountRolesOfCustomer = Context.FinancialAccountRoles.Where
                (entity => entity.PartyRole.PartyId == partyId && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id);

            var loanAccounts = from farc in financialAccountRolesOfCustomer
                                        join la in Context.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                        join las in Context.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                        where las.IsActive && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                        select las;
            return loanAccounts;
        }
    }
    public class PersonWithLoan
    {
        public int CustomerID { get; set; }
        public string Addresses { get; set; }
        public string Names { get; set; }
        public string PartyTypes { get; set; }
        public PersonWithLoan(Party party)
        {
            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                Person personAsCustomer = party.Person;
                this.Names = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                    , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                    personAsCustomer.NameSuffixString);
            }
            this.PartyTypes = party.PartyType.Name;
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(party.Id, RoleType.CustomerType);
            if (customerPartyRole == null) customerPartyRole = PartyRole.GetByPartyIdAndRole(party.Id, RoleType.EmployeeType);
            if (customerPartyRole == null) customerPartyRole = PartyRole.GetByPartyIdAndRole(party.Id, RoleType.ContactType);
            if (customerPartyRole != null)
            {
                this.CustomerID = customerPartyRole.Id;
            }

            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                this.Addresses = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);
            }
        }
    }
    public class CustomerWithLoan
    {
        public int CustomerID { get; set; }
        public string Addresses { get; set; }
        public string Names { get; set; }
        public string OrgName { get; set; }
        public string PartyTypes { get; set; }
        public string Status { get; set; }

        public CustomerWithLoan(Party party)
        {
            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                Person personAsCustomer = party.Person;
                this.Names = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                    , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                    personAsCustomer.NameSuffixString);
            }
            else
            {
                this.OrgName = party.Organization.OrganizationName;
            }

            this.PartyTypes = party.PartyType.Name;
            PartyRole customerPartyRole = PartyRole.GetByPartyIdAndRole(party.Id, RoleType.CustomerType);
            if (customerPartyRole.Customer != null)
            {
                this.CustomerID = customerPartyRole.Id;
                this.Status = customerPartyRole.Customer.CurrentStatus.CustomerStatusType.Name;
            }

            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                this.Addresses = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);
            }
        }

    }
}
