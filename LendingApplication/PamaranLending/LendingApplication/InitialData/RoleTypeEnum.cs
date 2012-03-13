using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic;

namespace LendingApplication
{
    public class RoleTypeEnums
    {
        public const string Customer = "Customer";
        public const string Contact = "Contact";
        public const string Employee = "Employee";
        public const string Employer = "Employer";
        public const string Spouse = "Spouse";
        public const string Taxpayer = "Taxpayer";
        public const string Bank = "Bank";
        public const string LendingInstitution = "Lending Institution";
        public const string PropertyLocation = "Property Location";
        public const string Borrower = "Borrower";
        public const string CoBorrower = "Co-Borrower";
        public const string Guarantor = "Guarantor";
        public const string ProcessedBy = "Processed By";
        public const string ApprovedBy = "Approved By";
        public const string FullyOwn = "Fully Own";
        public const string PartiallyOwn = "Partially Own";
        public const string Mortgagee = "Mortgagee";
        public const string Owner = "Owner";
        public const string WitnessedBy = "Witnessed By";
        public const string LegalCouncil = "Legal Council";
        public const string DomainApplication = "Domain Application";
        public static RoleType GetBorrowerRoleType(FinancialEntities context)
        {
            var type = context.RoleTypes.SingleOrDefault(entity => entity.Name == Borrower);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
            return type;
        }
        public static RoleType GetDomainApplicationRoleType(FinancialEntities context)
        {
            var type = context.RoleTypes.SingleOrDefault(entity => entity.Name == DomainApplication);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
            return type;
        }
        public static RoleType GetCoBorrowerRoleType(FinancialEntities context)
        {
            var domainApplication = GetDomainApplicationRoleType(context);
            var type = context.RoleTypes.SingleOrDefault(entity => entity.Name == CoBorrower && entity.ParentRoleTypeId == domainApplication.Id);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
            return type;
        }
        public static RoleType GetBankRoleType(FinancialEntities context)
        {
            var type = context.RoleTypes.SingleOrDefault(entity => entity.Name == Bank);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
            return type;
        }
        public static RoleType GetGuarantorRoleType(FinancialEntities context)
        {
            var domainApplication = GetDomainApplicationRoleType(context);
            var type = context.RoleTypes.SingleOrDefault(entity => entity.Name == Guarantor && entity.ParentRoleTypeId == domainApplication.Id);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
            return type;
        }
    }
}










