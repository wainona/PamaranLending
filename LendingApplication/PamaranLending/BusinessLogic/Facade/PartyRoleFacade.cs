using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class PartyRole
    {
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

        public static PartyRole GetById(int id)
        {
            return Context.PartyRoles.SingleOrDefault(entity => entity.Id == id && entity.EndDate == null);
        }

        public static PartyRole GetByPartyIdAndRole(int partyId, RoleType role)
        {
            return Context.PartyRoles.FirstOrDefault(entity => entity.PartyId == partyId && entity.RoleTypeId == role.Id && entity.EndDate == null);
        }

        public static IQueryable<PartyRole> GetAllByPartyIdAndRole(int partyId, RoleType role)
        {
            return Context.PartyRoles.Where(entity => entity.PartyId == partyId && entity.RoleTypeId == role.Id && entity.EndDate == null);
        }

        public PartyRelationship CurrentEmploymentRelationship
        {
            get
            {
                PartyRelationship partyRel = this.PartyRelationships.SingleOrDefault(entity => entity.PartyRelType.Id == PartyRelType.EmploymentType.Id
                                && entity.EndDate == null && entity.FirstPartyRoleId == this.Id);
                if (partyRel != null)
                    return partyRel;
                return null;
            }
        }

        public PartyRelationship CurrentSpousalRelationship
        {
            get
            {
                PartyRelationship spouseRel = this.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == this.Id && entity.PartyRelType.Id == PartyRelType.SpousalRelationshiptType.Id
                                && entity.EndDate == null);
                if (spouseRel != null)
                    return spouseRel;
                return null;
            }
        }

        public static PartyRole Create(RoleType roletype, Party party, DateTime today)
        {
            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = roletype.Id;
            partyRole.EffectiveDate = today;
            partyRole.EndDate = null;

            return partyRole;
        }

        public static PartyRole Create(RoleType roletype, int party, DateTime today)
        {
            PartyRole partyRole = new PartyRole();
            partyRole.PartyId = party;
            partyRole.RoleTypeId = roletype.Id;
            partyRole.EffectiveDate = today;
            partyRole.EndDate = null;

            return partyRole;
        }

        public static PartyRole CreateOrRetrieve(RoleType roletype, Party party, DateTime today)
        {
            var role = GetByPartyIdAndRole(party.Id, roletype);
            if (role == null)
                role = Create(roletype, party, today);
            return role;
        }

        public static PartyRole CreateOrRetrieve(RoleType roletype, int party, DateTime today)
        {
            var role = GetByPartyIdAndRole(party, roletype);
            if (role == null)
                role = Create(roletype, party, today);
            return role;
        }

        public static PartyRole CreateChequeRole(Cheque cheque, RoleType roleType, Party party, DateTime today)
        {
            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = roleType.Id;
            partyRole.EffectiveDate = today;
            partyRole.EndDate = null;
            cheque.PartyRole = partyRole;
            Context.Cheques.AddObject(cheque);
            return partyRole;
        }

        public static PartyRole CreateLoanApplicationRole(LoanApplication loanApplication, RoleType roletype, Party party, DateTime today)
        {
            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = roletype.Id;
            partyRole.EffectiveDate = today;
            partyRole.EndDate = null;

            LoanApplicationRole loanApplicationRole = new LoanApplicationRole();
            loanApplicationRole.LoanApplication = loanApplication;
            loanApplicationRole.PartyRole = partyRole;

            Context.LoanApplicationRoles.AddObject(loanApplicationRole);

            return partyRole;
        }

        public static PartyRole GetPartyRoleFromLoanApplication(LoanApplication loanApplication, RoleType roletype)
        {
            var loanApplicationRole = loanApplication.LoanApplicationRoles.FirstOrDefault(entity => entity.PartyRole.RoleTypeId == roletype.Id
                && entity.PartyRole.EndDate == null);
            if (loanApplicationRole != null)
                return loanApplicationRole.PartyRole;
            else
                return null;
        }

        public static PartyRole CreateAndUpdateCurrentLoanApplicationRole(LoanApplication loanApplication, Party party, RoleType roletype, DateTime today)
        {
            PartyRole role = GetPartyRoleFromLoanApplication(loanApplication, roletype);
            if (role != null)
                role.EndDate = today;

            return CreateLoanApplicationRole(loanApplication, roletype, party, today);
        }

        public static IQueryable<PartyRole> GetAllRoleTypeFromLoanApplication(LoanApplication loanApplication, RoleType roletype)
        {
            return from lar in Context.LoanApplicationRoles.Where(entity => entity.PartyRole.RoleTypeId == roletype.Id
                && entity.PartyRole.EndDate == null && entity.ApplicationId == loanApplication.ApplicationId)
                                 select lar.PartyRole;
        }
    }
}
