using System;
using System.Collections.Generic;
using System.Linq;using FirstPacific.UIFramework;
using System.Web;
using BusinessLogic;

namespace BusinessLogic
{
    public partial class RoleType
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        private const string _LoanApplicationRoleType = "Loan Application Role Type";

        public static RoleType LoanApplicationRoleType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == _LoanApplicationRoleType);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string _AgreementRoleType = "Agreement Role Type";

        public static RoleType AgreementRoleType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == _AgreementRoleType);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }
        private const string _FinancialAccountRoleType = "Financial Account Role Type";

        public static RoleType FinancialAccountRoleType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == _FinancialAccountRoleType);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Customer = "Customer";

        public static RoleType CustomerType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Customer);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Contact = "Contact";

        public static RoleType ContactType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Contact);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Employee = "Employee";

        public static RoleType EmployeeType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Employee);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Employer = "Employer";

        public static RoleType EmployerType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Employer);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Spouse = "Spouse";

        public static RoleType SpouseType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Spouse);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Taxpayer = "Taxpayer";

        public static RoleType TaxpayerType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Taxpayer);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Bank = "Bank";

        public static RoleType BankType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Bank);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string LendingInstitution = "Lending Institution";

        public static RoleType LendingInstitutionType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == LendingInstitution);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Borrower = "Borrower";

        public static RoleType BorrowerApplicationType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Borrower && entity.ParentRoleTypeId == LoanApplicationRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType BorrowerAgreementType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Borrower && entity.ParentRoleTypeId == AgreementRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string CoBorrower = "Co-Borrower";

        public static RoleType CoBorrowerApplicationType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == CoBorrower && entity.ParentRoleTypeId == LoanApplicationRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType CoBorrowerAgreementType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == CoBorrower && entity.ParentRoleTypeId == AgreementRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Guarantor = "Guarantor";

        public static RoleType GuarantorApplicationType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Guarantor && entity.ParentRoleTypeId == LoanApplicationRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType GuarantorAgreementType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Guarantor && entity.ParentRoleTypeId == AgreementRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string ProcessedBy = "Processed By";

        public static RoleType ProcessedByAgreementType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == ProcessedBy && entity.ParentRoleTypeId == AgreementRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType ProcessedByApplicationType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == ProcessedBy && entity.ParentRoleTypeId == LoanApplicationRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string FullyOwn = "Fully Own";

        public static RoleType FullyOwnType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == FullyOwn);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string PartiallyOwn = "Partially Own";

        public static RoleType PartiallyOwnType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == PartiallyOwn);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Mortgagee = "Mortgagee";

        public static RoleType MortgageeType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Mortgagee);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string Owner = "Owner";

        public static RoleType OwnerType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Owner);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string CoOwner = "Co-Owner";

        public static RoleType CoOwnerType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == CoOwner);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType OwnerFinancialType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Owner && entity.ParentRoleTypeId == FinancialAccountRoleType.Id );
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);
                return type;
            }
        }


        public static RoleType CoOwnerFinancialType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == CoOwner && entity.ParentRoleTypeId == FinancialAccountRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }
        public static RoleType GuarantorFinancialType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == Guarantor && entity.ParentRoleTypeId == FinancialAccountRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }
        private const string ApprovedBy = "Approved By";

        public static RoleType ApprovedByApplicationType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == ApprovedBy && entity.ParentRoleTypeId == LoanApplicationRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        public static RoleType ApprovedByAgreementType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == ApprovedBy && entity.ParentRoleTypeId == AgreementRoleType.Id);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string WitnessedBy = "Witnessed By";

        public static RoleType WitnessedByType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == WitnessedBy);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string LegalCouncil = "Legal Council";

        public static RoleType LegalCouncilType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == LegalCouncil);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

        private const string AssetRole = "Asset Role Type";

        public static RoleType AssetRoleType
        {
            get
            {
                var type = Context.RoleTypes.SingleOrDefault(entity => entity.Name == AssetRole);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(type);

                return type;
            }
        }

    }
}