using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class AllowedCoBorrowerGuarantor
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

        public static IEnumerable<AllowedCoBorrowerGuarantorModel> AllowedCoBorrowerGuarantors(List<int> id, int LogInId)
        {
            var query = from p in ObjectContext.Parties
                        join pr in ObjectContext.PartyRoles on p.Id equals pr.PartyId
                        join rt in ObjectContext.RoleTypes on pr.RoleTypeId equals rt.Id
                        join pt in ObjectContext.PartyRelationships on pr.Id equals pt.FirstPartyRoleId
                        where p.Id != LogInId && (rt.Name == RoleType.ContactType.Name ||
                                rt.Name == RoleType.EmployeeType.Name ||
                                rt.Name == RoleType.CustomerType.Name) &&
                                p.PartyType.Name == "Person" &&
                                pt.EndDate == null && id.Contains(p.Id) == false
                        select new { Party = p };

            List<AllowedCoBorrowerGuarantorModel> models = new List<AllowedCoBorrowerGuarantorModel>();
            foreach (var item in query.ToList())
            {
                models.Add(new AllowedCoBorrowerGuarantorModel(item.Party));
            }
            return models;
        }
    }
    public class AllowedCoBorrowerGuarantorModel
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public AllowedCoBorrowerGuarantorModel(Party party)
        {
            this.PartyId = party.Id;
            if (party.PartyTypeId == PartyType.PersonType.Id)
            {
                Person personAsCustomer = party.Person;
                this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                    , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                    personAsCustomer.NameSuffixString);
            }
            var postalAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, true);

            if (postalAddress != null)
            {
                this.Address = StringConcatUtility.Build(",", postalAddress.StreetAddress,
                    postalAddress.Barangay,
                    postalAddress.Municipality,
                    postalAddress.City,
                    postalAddress.Province,
                    postalAddress.Country.Name,
                    postalAddress.PostalCode);
            }
        }
    }
}
