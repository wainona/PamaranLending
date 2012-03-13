using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class CustomerDetailsModel
    {
        public string ImageUrl { get; set; }

        public int PartyRoleId { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Name { get; set; }

        public string District { get; set; }

        public string StationNumber { get; set; }

        public string Status { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public decimal? CreditLimit { get; set; }

        public string CustomerType { get; set; }

        public string CountryCode { get; set; }

        public string TelephoneNumberAreaCode { get; set; }

        public string CellphoneAreaCode { get; set; }

        public string PrimaryHomeAddress { get; set; }

        public string CellphoneNumber { get; set; }

        public string TelephoneNumber { get; set; }

        public string PrimaryEmailAddress { get; set; }

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

        public CustomerDetailsModel()
        {
            this.TelephoneNumber = string.Empty;
            this.TelephoneNumberAreaCode = string.Empty;
            this.CellphoneAreaCode = string.Empty;
            this.TelephoneNumber = string.Empty;
        }

        public CustomerDetailsModel(int partyRoleId)
        {
            this.TelephoneNumber = string.Empty;
            this.TelephoneNumberAreaCode = string.Empty;
            this.CellphoneAreaCode = string.Empty;
            this.TelephoneNumber = string.Empty;

            this.PartyRoleId = partyRoleId;
            PartyRole partyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
            Party party = partyRole.Party;
            Person personAsCustomer = party.Person;
            Customer customer = partyRole.Customer;

            this.Gender = personAsCustomer.GenderType.Name;
            this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                personAsCustomer.NameSuffixString);
            this.ImageUrl = personAsCustomer.ImageFilename;
            this.Age = personAsCustomer.Age;
            this.BirthDate = personAsCustomer.Birthdate;

            this.CreditLimit = customer.CreditLimit;
            this.Status = customer.CurrentStatus.CustomerStatusType.Name;
            
            this.CustomerType = Context.CustomerCategoryTypes.SingleOrDefault(entity =>
                entity.Id == customer.CurrentCustomerCategory.CustomerCategoryType).Name;

            if (customer.CurrentClassification != null)
            {
                this.District = customer.CurrentClassification.ClassificationType.District;
                this.StationNumber = customer.CurrentClassification.ClassificationType.StationNumber;
            }

            InitializeAddresses(party);
        }

        public void InitializeAddresses(Party party)
        {
            //Postal Address
            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                this.CountryCode = postalAddressSpecific.Country.CountryTelephoneCode;

                this.PrimaryHomeAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);

                //Business Mobile Number
                Address cellTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                    && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.PersonalMobileNumberType
                                    && entity.TelecommunicationsNumber.IsPrimary);

                if (cellTelecomNumber != null && cellTelecomNumber.TelecommunicationsNumber != null)
                {

                    TelecommunicationsNumber cellTelecomNumberSpecific = cellTelecomNumber.TelecommunicationsNumber;
                    this.CellphoneNumber = cellTelecomNumberSpecific.PhoneNumber;
                    this.CellphoneAreaCode = cellTelecomNumberSpecific.AreaCode;
                }

                //Business Telephone Number
                Address primTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                    && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.HomePhoneNumberType
                                    && entity.TelecommunicationsNumber.IsPrimary);

                if (primTelecomNumber != null && primTelecomNumber.TelecommunicationsNumber != null)
                {
                    TelecommunicationsNumber primNumberSpecific = primTelecomNumber.TelecommunicationsNumber;
                    this.TelephoneNumber = primNumberSpecific.PhoneNumber;
                    this.TelephoneNumberAreaCode = primNumberSpecific.AreaCode;
                }
            }

            //Email Address
            Address emailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.PersonalEmailAddressType
                                && entity.ElectronicAddress.IsPrimary);
            if (emailAddress != null && emailAddress.ElectronicAddress != null)
            {
                ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                this.PrimaryEmailAddress = primaryEmailAddressSpecific.ElectronicAddressString;
            }
        }
    }

    public class PersonnalPartyModel : BusinessObjectModel
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PartyRoleId { get; set; }
        public int LoanApplicationRoleId { get; set; }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        protected static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public PersonnalPartyModel()
        {
            this.IsNew = true;
        }

        public PersonnalPartyModel(LoanApplicationRole role)
        {
            this.IsNew = false;
            this.PartyRoleId = role.PartyRoleId;
            this.LoanApplicationRoleId = role.Id;

            PartyRole partyRole = role.PartyRole;
            Party party = partyRole.Party;
            this.PartyId = party.Id;
            Person personAsCustomer = party.Person;
            Customer customer = partyRole.Customer;

            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                personAsCustomer.NameSuffixString);
            }
            else
            {
                Organization employerOrg = party.Organization;
                this.Name = employerOrg.OrganizationName;
            }            

            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);
            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                this.Address = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);
            }
        }

        public void PrepareForSave(LoanApplication loanApplication, RoleType roleType, DateTime today)
        {
            if (this.IsNew)
            {
                var party = Context.Parties.SingleOrDefault(entity => entity.Id == this.PartyId);
                var partyRole = PartyRole.CreateLoanApplicationRole(loanApplication, roleType, party, today);
                Context.PartyRoles.AddObject(partyRole);
            }
            else if (this.ToBeDeleted)
            {
                var role = Context.LoanApplicationRoles.SingleOrDefault(entity => entity.Id ==LoanApplicationRoleId);
                PartyRole partyRole = role.PartyRole;
                partyRole.EndDate = today;
                Context.LoanApplicationRoles.DeleteObject(role);
            }
            //else if (this.IsNew == false && this.ToBeDeleted == false)
            //{
            //    var party = Context.Parties.SingleOrDefault(entity => entity.Id == this.PartyId);
            //    var partyRole = PartyRole.CreateLoanApplicationRole(loanApplication, roleType, party, today);
            //    Context.PartyRoles.AddObject(partyRole);
            //}
        }
    }

    public class PropertyOwner : BusinessObjectModel
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PartyRoleId { get; set; }
        public int PercentOwned { get; set; }
        public int AssetRoleId { get; set; }


        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        protected static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public PropertyOwner()
        {
            this.IsNew = true;
        }

        public PropertyOwner(AssetRole role)
        {
            this.IsNew = false;
            this.AssetRoleId = role.Id;
            this.PartyRoleId = role.PartyRoleId;
            this.PercentOwned = (int)(role.EquityValue ?? 0);

            PartyRole partyRole = role.PartyRole;
            Party party = partyRole.Party;
            this.PartyId = party.Id;
            Person personAsCustomer = party.Person;
            Customer customer = partyRole.Customer;

            if (party.PartyType.Id == PartyType.PersonType.Id)
            {
                this.Name = StringConcatUtility.Build(" ", personAsCustomer.LastNameString + ","
                , personAsCustomer.FirstNameString, personAsCustomer.MiddleInitialString,
                personAsCustomer.NameSuffixString);
            }
            else
            {
                Organization employerOrg = party.Organization;
                this.Name = employerOrg.OrganizationName;
            }

            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                this.Address = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);
            }
        }

        public void Add(Asset asset, DateTime today) 
        {
            //var ownerPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.PartyId == this.PartyId);
            RoleType roleType = RoleType.PartiallyOwnType;
            if (this.PercentOwned == 100)
                roleType = RoleType.FullyOwnType;
            PartyRole propertyOwner = new PartyRole();
            propertyOwner.PartyId = this.PartyId;
            propertyOwner.PartyRoleType = roleType.PartyRoleType;
            propertyOwner.EffectiveDate = today;

            AssetRole propertyOwnerAssetRole = new AssetRole();
            propertyOwnerAssetRole.Asset = asset;
            propertyOwnerAssetRole.PartyRole = propertyOwner;
            propertyOwnerAssetRole.EquityValue = this.PercentOwned;

            Context.PartyRoles.AddObject(propertyOwner);
            Context.AssetRoles.AddObject(propertyOwnerAssetRole);
        }

        public void Remove(DateTime today)
        {
            AssetRole role = Context.AssetRoles.SingleOrDefault(entity => entity.Id == this.AssetRoleId);
            PartyRole propertyOwnerPartyRole = role.PartyRole;
            propertyOwnerPartyRole.EndDate = today;
            Context.AssetRoles.DeleteObject(role);
        }

        public void Remove(AssetRole role, DateTime today)
        {
            PartyRole propertyOwnerPartyRole = role.PartyRole;
            propertyOwnerPartyRole.EndDate = today;
            Context.AssetRoles.DeleteObject(role);
        }

        public void Save(Asset asset, DateTime today)
        {
            if (this.IsNew)
            {
                Add(asset, today);
            }
            else if (this.ToBeDeleted)
            {
                Remove(today);
            }
            else if (this.IsEdited)
            {
                AssetRole role = Context.AssetRoles.SingleOrDefault(entity => entity.Id == this.AssetRoleId);
                role.EquityValue = this.PercentOwned;
            }
        }
    }

    public abstract class Collateral : BusinessObjectModel
    {
        protected List<PropertyOwner> PropertyOwners;

        public IEnumerable<PropertyOwner> AvailablePropertyOwners
        {
            get
            {
                return this.PropertyOwners.Where(model => model.ToBeDeleted == false);
            }
        }

        public void AddPropertyOwner(PropertyOwner model)
        {
            if (this.PropertyOwners.Contains(model))
                return;
            this.PropertyOwners.Add(model);
        }

        public void RemovePropertyOwner(string randomKey)
        {
            PropertyOwner model = this.PropertyOwners.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemovePropertyOwner(model);
        }

        public void RemovePropertyOwner(PropertyOwner model)
        {
            if (this.PropertyOwners.Contains(model) == true)
            {
                if (model.IsNew)
                    PropertyOwners.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public PropertyOwner Retrieve(string randomKey)
        {
            return this.PropertyOwners.SingleOrDefault(entity => entity.RandomKey == randomKey);
        }

        public int AssetId { get; set; }
        public abstract int AssetTypeId { get; }
        public abstract string AssetTypeName { get; }
        public string Description { get; set; }
        public int CurrentMortgageeAssetRoleId { get; set; }
        public bool IsPropertyMortgage { get; set; }
        public int MortgageeId { get; set; }
        public string Mortgagee { get; set; }
        public decimal? AcquisitionCost { get; set; }
        //public string ResourceGuid { get; set; }

        //public abstract void Retrieve(int id);

        //public abstract void PrepareForSave();

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

        public Collateral()
        {
            this.IsPropertyMortgage = false;
            this.CurrentMortgageeAssetRoleId = -1;
            this.MortgageeId = -1;
            this.AssetId = -1;
            PropertyOwners = new List<PropertyOwner>();
        }

        private void CreateMortgagee(Asset asset, DateTime today)
        {
            //if isMortgaged is true, provide Mortgagee
            if (this.IsPropertyMortgage)
            {
                var mortgageRoleType = RoleType.GetType(RoleType.AssetRoleType, RoleType.MortgageeType);

                PartyRole mortgagee = new PartyRole();
                mortgagee.PartyId = this.MortgageeId;
                mortgagee.RoleTypeId = mortgageRoleType.Id;
                mortgagee.EffectiveDate = today;

                AssetRole assetRole = new AssetRole();
                assetRole.Asset = asset;
                assetRole.PartyRole = mortgagee;
                Context.PartyRoles.AddObject(mortgagee);
            }
        }

        protected void RemoveCurrentMortgagee(Asset asset, DateTime today)
        {
            if (this.CurrentMortgageeAssetRoleId != -1)
            {
                AssetRole role = asset.AssetRoles.SingleOrDefault(entity => entity.Id == this.CurrentMortgageeAssetRoleId);
                PartyRole propertyOwnerPartyRole = role.PartyRole;
                propertyOwnerPartyRole.EndDate = today;
                Context.AssetRoles.DeleteObject(role);
            }
        }

        public abstract void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today);

        public Asset AddAsset(LoanApplication loanApplication, DateTime today)
        {
            Asset asset = new Asset();
            asset.AssetTypeId = this.AssetTypeId;
            asset.LoanApplication = loanApplication;
            asset.Description = Description;
            asset.IsMortgaged = this.IsPropertyMortgage;
            asset.AcquisitionCost = this.AcquisitionCost;
            Context.Assets.AddObject(asset);
            CreateMortgagee(asset, today);

            return asset;
        }

        public void UpdateAsset(Asset asset, DateTime today)
        {
            RemoveCurrentMortgagee(asset, today);
            asset.Description = this.Description;
            asset.IsMortgaged = this.IsPropertyMortgage;
            CreateMortgagee(asset, today);
        }

        private static void Init(Asset asset, Collateral collateral)
        {
            collateral.AssetId = asset.Id;
            collateral.Description = asset.Description;
            collateral.AcquisitionCost = asset.AcquisitionCost;
            var mortgageRoleType = RoleType.GetType(RoleType.AssetRoleType, RoleType.MortgageeType);
            AssetRole assetRole = asset.AssetRoles.SingleOrDefault(entity => entity.PartyRole.RoleTypeId == mortgageRoleType.Id && entity.PartyRole.EndDate == null);
            if (assetRole != null)
            {
                collateral.IsPropertyMortgage = true;
                collateral.CurrentMortgageeAssetRoleId = assetRole.Id;
                collateral.MortgageeId = assetRole.PartyRoleId;
                Party party = assetRole.PartyRole.Party;
                collateral.Mortgagee = party.Name;
            }

            var assetRoles = asset.AssetRoles.Where(entity =>
            (entity.PartyRole.RoleTypeId == RoleType.PartiallyOwnType.Id
            || entity.PartyRole.RoleTypeId == RoleType.FullyOwnType.Id)
            && entity.PartyRole.EndDate == null);

            foreach (var role in assetRoles)
            {
                collateral.PropertyOwners.Add(new PropertyOwner(role));
            }
        }

        public static Collateral CreateCollateral(Asset asset, PartyRole customerPartyRole)
        {
            Collateral collateral = null;
            if (asset.AssetTypeId == AssetType.BankAccountType.Id)
                collateral = BankAccountCollateral.HandleCreate(asset, customerPartyRole);
            else if (asset.AssetTypeId == AssetType.LandType.Id)
                collateral = LandCollateral.HandleCreate(asset, customerPartyRole);
            else if (asset.AssetTypeId == AssetType.JewelryType.Id)
                collateral = JewelryCollateral.HandleCreate(asset, customerPartyRole);
            else if (asset.AssetTypeId == AssetType.MachineType.Id)
                collateral = MachineCollateral.HandleCreate(asset, customerPartyRole);
            else if (asset.AssetTypeId == AssetType.VehicleType.Id)
                collateral = VehicleCollateral.HandleCreate(asset, customerPartyRole);
            else if (asset.AssetTypeId == AssetType.OthersType.Id)
                collateral = JewelryCollateral.HandleCreate(asset, customerPartyRole);
            Init(asset, collateral);
            collateral.IsNew = false;
            return collateral;
        }
        
    }

    public class BankAccountCollateral : Collateral
    {
        private AssetType type = AssetType.BankAccountType;
        public override int AssetTypeId
        {
            get
            {
                return type.Id;
            }
        }
        public override string AssetTypeName
        {
            get
            {
                return type.Name;
            }
        }
        public int BankAccountType { get; set; }
        public string BankName { get; set; }
        public int BankPartyRoleId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }

        public BankAccountCollateral()
        {
            this.IsNew = true;   
        }

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

        public override void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            if (this.IsNew)
            {
                var asset = AddAsset(loanApplication, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Add(asset, today);
                }

                BankAccount bank = new BankAccount();
                bank.Asset = asset;
                bank.BankAccountTypeId = this.BankAccountType;
                bank.BankPartyRoleId = this.BankPartyRoleId;
                bank.AccountNumber = this.BankAccountNumber; 
                bank.AccountName = this.BankAccountName;
            }
            else if (this.ToBeDeleted)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                RemoveCurrentMortgagee(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Remove(today);
                }                
                
                if (asset.BankAccount != null)
                    Context.BankAccounts.DeleteObject(asset.BankAccount);

                Context.Assets.DeleteObject(asset);
            }
            else if (this.IsEdited)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                UpdateAsset(asset, today);
                BankAccount bank = asset.BankAccount;
                bank.BankAccountTypeId = this.BankAccountType;
                bank.BankPartyRoleId = this.BankPartyRoleId;
                bank.AccountNumber = this.BankAccountNumber;
                bank.AccountName = this.BankAccountName;

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Save(asset, today);
                }
            }
            else if (this.IsNew == false && this.ToBeDeleted == false && this.IsEdited == false)
            {
                if (this.AssetId == 0)
                {
                    var asset = AddAsset(loanApplication, today);

                    foreach (PropertyOwner owner in PropertyOwners)
                    {
                        owner.Add(asset, today);
                    }

                    BankAccount bank = new BankAccount();
                    bank.Asset = asset;
                    bank.BankAccountTypeId = this.BankAccountType;
                    bank.BankPartyRoleId = this.BankPartyRoleId;
                    bank.AccountNumber = this.BankAccountNumber;
                    bank.AccountName = this.BankAccountName;
                }
            }
        }

        public static Collateral HandleCreate(Asset asset, PartyRole customerPartyRole)
        {
            var bankAccount = new BankAccountCollateral();
            BankAccount ba = asset.BankAccount;
            bankAccount.BankAccountType = ba.BankAccountTypeId;
            bankAccount.BankPartyRoleId = ba.BankPartyRoleId ?? 0;
            bankAccount.BankAccountNumber = ba.AccountNumber;
            bankAccount.BankAccountName = ba.AccountName;
            PartyRole partyRole = PartyRole.GetById(bankAccount.BankPartyRoleId);
            bankAccount.BankName = partyRole.Party.Organization.OrganizationName;

            return bankAccount;
        }
    }

    public class JewelryCollateral : Collateral
    {

        //private AssetType type = AssetType.JewelryType;
        public AssetType type { get; set; }
        public override int AssetTypeId
        {
            get
            {
                return type.Id;
            }
        }
        public override string AssetTypeName
        {
            get
            {
                return type.Name;
            }
        }
        public decimal AcquisitionCost { get; set; }

        public JewelryCollateral()
        {
            PropertyOwners = new List<PropertyOwner>();
        }

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

        public override void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            //throw new NotImplementedException();
            if (this.IsNew)
            {
                var asset = AddAsset(loanApplication, today);
                asset.Description = this.Description;
                asset.AcquisitionCost = this.AcquisitionCost;

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Add(asset, today);
                }

            }
            else if (this.ToBeDeleted)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                RemoveCurrentMortgagee(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Remove(today);
                }

                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);

                if (asset.Vehicle != null)
                {
                    Context.Vehicles.DeleteObject(asset.Vehicle);
                }

                Context.Assets.DeleteObject(asset);
            }
            else if (this.IsEdited)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                UpdateAsset(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Save(asset, today);
                }
            }
            else if (this.IsNew == false && this.IsEdited == false && this.ToBeDeleted == false)
            {
                if (this.AssetId == 0)
                {
                    var asset = AddAsset(loanApplication, today);
                    asset.Description = this.Description;
                    asset.AcquisitionCost = this.AcquisitionCost;

                    foreach (PropertyOwner owner in PropertyOwners)
                    {
                        owner.Add(asset, today);
                    }
                }
            }

        }

        public static Collateral HandleCreate(Asset asset, PartyRole customerPartyRole)
        {
            var jewelryOthers = new JewelryCollateral();
            jewelryOthers.AssetId = asset.Id;
            jewelryOthers.type = asset.AssetType;
            jewelryOthers.AcquisitionCost = asset.AcquisitionCost ?? 0;
            jewelryOthers.Description = asset.Description;

            return jewelryOthers;
        }
    }

    public class LandCollateral : Collateral
    {
        private AssetType type = AssetType.LandType;
        public override int AssetTypeId
        {
            get
            {
                return type.Id;
            }
        }
        public override string AssetTypeName
        {
            get
            {
                return type.Name;
            }
        }
        public int LandType { get; set; }
        public decimal LandArea { get; set; }
        public int LandUOM { get; set; }
        public string TCTNumber { get; set; }
        //Property Address
        public int CountryId { get; set; }
        public string StreetAddress { get; set; }
        public string Barangay { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        //public decimal AcquisitionCost { get; set; }

        public LandCollateral()
        {
            this.IsNew = true;
        }

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

        public override void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            //throw new NotImplementedException();
            if (this.IsNew)
            {
                var asset = AddAsset(loanApplication, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Add(asset, today);
                }

                Land land = new Land();
                land.Asset = asset;
                land.UomId = this.LandUOM;
                land.LandTypeId = this.LandType;
                land.OctTctNumber = this.TCTNumber;
                land.LandArea = this.LandArea;

                //add property location
                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
                var propertyLocation = PostalAddress.CreatePostalAddress(borrowerParty, PostalAddressType.PropertyLocationType, today);
                propertyLocation.StreetAddress = this.StreetAddress;
                propertyLocation.Barangay = this.Barangay;
                propertyLocation.Municipality = this.Municipality;
                propertyLocation.CountryId = this.CountryId;
                propertyLocation.City = this.City;
                propertyLocation.Province = this.Province;
                propertyLocation.PostalCode = this.PostalCode;
                propertyLocation.Address.Asset = asset;
            }
            else if (this.ToBeDeleted)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                RemoveCurrentMortgagee(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Remove(today);
                }

                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
                var propertyLocation = PostalAddress.GetCurrentPostalAddress(borrowerParty, PostalAddressType.PropertyLocationType, asset);

                if (asset.Land != null)
                {
                    Address address = propertyLocation.Address;
                    Context.Lands.DeleteObject(asset.Land);
                    Context.PostalAddresses.DeleteObject(propertyLocation);
                    Context.Addresses.DeleteObject(address);
                }

                Context.Assets.DeleteObject(asset);
            }
            else if (this.IsEdited)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                UpdateAsset(asset, today);

                Land land = asset.Land;
                land.UomId = this.LandUOM;
                land.LandTypeId = this.LandType;
                land.OctTctNumber = this.TCTNumber;
                land.LandArea = this.LandArea;

                //add property location
                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
                var currentPropertyLocation = PostalAddress.GetCurrentPostalAddress(borrowerParty, PostalAddressType.PropertyLocationType, asset);
                var propertyLocation = new PostalAddress();
                propertyLocation.StreetAddress = this.StreetAddress;
                propertyLocation.Barangay = this.Barangay;
                propertyLocation.Municipality = this.Municipality;
                propertyLocation.CountryId = this.CountryId;
                propertyLocation.City = this.City;
                propertyLocation.Province = this.Province;
                propertyLocation.PostalCode = this.PostalCode;

                PostalAddress.CreateOrUpdatePostalAddress(currentPropertyLocation, propertyLocation, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Save(asset, today);
                }
            }else if(this.IsNew == false && this.ToBeDeleted == false && this.IsEdited == false)
            {
                if (this.AssetId == 0)
                {
                    var asset = AddAsset(loanApplication, today);

                    foreach (PropertyOwner owner in PropertyOwners)
                    {
                        owner.Add(asset, today);
                    }

                    Land land = new Land();
                    land.Asset = asset;
                    land.UomId = this.LandUOM;
                    land.LandTypeId = this.LandType;
                    land.OctTctNumber = this.TCTNumber;
                    land.LandArea = this.LandArea;

                    //add property location
                    var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
                    //var currentPropertyLocation = PostalAddress.GetCurrentPostalAddress(borrowerParty, PostalAddressType.PropertyLocationType, asset);
                    var propertyLocation = PostalAddress.CreatePostalAddress(borrowerParty, PostalAddressType.PropertyLocationType, today);
                    propertyLocation.StreetAddress = this.StreetAddress;
                    propertyLocation.Barangay = this.Barangay;
                    propertyLocation.Municipality = this.Municipality;
                    propertyLocation.CountryId = this.CountryId;
                    propertyLocation.City = this.City;
                    propertyLocation.Province = this.Province;
                    propertyLocation.PostalCode = this.PostalCode;
                    propertyLocation.Address.Asset = asset;
                }
            }
        }

        public static Collateral HandleCreate(Asset asset, PartyRole customerPartyRole)
        {
            LandCollateral collateral = new LandCollateral();
            Land land = asset.Land;
            collateral.LandUOM = land.UomId;
            collateral.LandType = land.LandTypeId;
            collateral.TCTNumber = land.OctTctNumber;
            collateral.LandArea = land.LandArea;

            //add property location
            var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
            var propertyLocation = PostalAddress.GetCurrentPostalAddress(borrowerParty, 
                PostalAddressType.PropertyLocationType, asset);
            if (propertyLocation != null)
            {
                collateral.StreetAddress = propertyLocation.StreetAddress;
                collateral.Barangay = propertyLocation.Barangay;
                collateral.Municipality = propertyLocation.Municipality;
                collateral.CountryId = propertyLocation.CountryId ?? 0;
                collateral.City = propertyLocation.City;
                collateral.Province = propertyLocation.Province;
                collateral.PostalCode = propertyLocation.PostalCode;
            }
            return collateral;
        }
    }

    public class MachineCollateral : Collateral
    {
        private AssetType type = AssetType.MachineType;
        public override int AssetTypeId
        {
            get
            {
                return type.Id;
            }
        }
        public override string AssetTypeName
        {
            get
            {
                return type.Name;
            }
        }
        public string MachineName { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Capacity { get; set; }

        public MachineCollateral()
        {
            //PropertyOwners = new List<PropertyOwner>();
            this.IsNew = true;
        }

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

        public override void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            //throw new NotImplementedException();
            if (this.IsNew)
            {
                var asset = AddAsset(loanApplication, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Add(asset, today);
                }

                Machine machine = new Machine();
                machine.Asset = asset;
                machine.MachineName = this.MachineName;
                machine.Brand = this.Brand;
                machine.Model = this.Model;
                machine.Capacity = this.Capacity;
                asset.AcquisitionCost = this.AcquisitionCost;
            }
            else if (this.ToBeDeleted)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                RemoveCurrentMortgagee(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Remove(today);
                }

                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);
                
                if (asset.Machine != null)
                {
                    Context.Machines.DeleteObject(asset.Machine);
                }

                Context.Assets.DeleteObject(asset);
            }
            else if (this.IsEdited)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                UpdateAsset(asset, today);
                Machine machine = asset.Machine;
                machine.MachineName = this.MachineName;
                machine.Brand = this.Brand;
                machine.Model = this.Model;
                machine.Capacity = this.Capacity;
                asset.AcquisitionCost = this.AcquisitionCost;

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Save(asset, today);
                }
            }
            else if (this.IsNew == false && this.IsEdited == false && this.ToBeDeleted == false)
            {
                if (this.AssetId == 0)
                {
                    var asset = AddAsset(loanApplication, today);

                    foreach (PropertyOwner owner in PropertyOwners)
                    {
                        owner.Add(asset, today);
                    }

                    Machine machine = new Machine();
                    machine.Asset = asset;
                    machine.MachineName = this.MachineName;
                    machine.Brand = this.Brand;
                    machine.Model = this.Model;
                    machine.Capacity = this.Capacity;
                    asset.AcquisitionCost = this.AcquisitionCost;
                }
            }
        }

        public static Collateral HandleCreate(Asset asset, PartyRole customerPartyRole)
        {
            //throw new NotImplementedException();
            var machine = new MachineCollateral();
            Machine ma = asset.Machine;
            machine.MachineName = ma.MachineName;
            machine.Brand = ma.Brand;
            machine.Model = ma.Model;
            machine.Capacity = ma.Capacity;
            machine.AcquisitionCost = asset.AcquisitionCost ?? 0;

            return machine;
        }
    }

    public class VehicleCollateral : Collateral
    {

        private AssetType type = AssetType.VehicleType;
        public override int AssetTypeId
        {
            get
            {
                return type.Id;
            }
        }
        public override string AssetTypeName
        {
            get
            {
                return type.Name;
            }
        }
        public int VehicleTypeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal AcquisitionCost { get; set; }

        public VehicleCollateral()
        {
            PropertyOwners = new List<PropertyOwner>();
        }

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

        public override void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, DateTime today)
        {
            //throw new NotImplementedException();
            if (this.IsNew)
            {
                var asset = AddAsset(loanApplication, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Add(asset, today);
                }

                Vehicle vehicle = new Vehicle();
                vehicle.Asset = asset;
                vehicle.VehicleTypeId = this.VehicleTypeId;
                vehicle.Brand = this.Brand;
                vehicle.Model = this.Model;
                asset.AcquisitionCost = this.AcquisitionCost;
            }
            else if (this.ToBeDeleted)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                RemoveCurrentMortgagee(asset, today);

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Remove(today);
                }

                var borrowerParty = Context.Parties.SingleOrDefault(entity => entity.Id == customerPartyRole.PartyId);

                if (asset.Vehicle != null)
                {
                    Context.Vehicles.DeleteObject(asset.Vehicle);
                }

                Context.Assets.DeleteObject(asset);
            }
            else if (this.IsEdited)
            {
                Asset asset = Context.Assets.SingleOrDefault(entity => entity.Id == this.AssetId);
                UpdateAsset(asset, today);

                Vehicle vehicle = asset.Vehicle;
                vehicle.VehicleTypeId = this.VehicleTypeId;
                vehicle.Brand = this.Brand;
                vehicle.Model = this.Model;
                asset.AcquisitionCost = this.AcquisitionCost;

                foreach (PropertyOwner owner in PropertyOwners)
                {
                    owner.Save(asset, today);
                }
            }
            else if (this.IsNew == false && this.IsEdited == false && this.ToBeDeleted == false)
            {
                if (this.AssetId == 0)
                {
                    var asset = AddAsset(loanApplication, today);

                    foreach (PropertyOwner owner in PropertyOwners)
                    {
                        owner.Add(asset, today);
                    }

                    Vehicle vehicle = new Vehicle();
                    vehicle.Asset = asset;
                    vehicle.VehicleTypeId = this.VehicleTypeId;
                    vehicle.Brand = this.Brand;
                    vehicle.Model = this.Model;
                    asset.AcquisitionCost = this.AcquisitionCost;
                }
            }
        }

        public static Collateral HandleCreate(Asset asset, PartyRole customerPartyRole)
        {
            //throw new NotImplementedException();
            var vehicle = new VehicleCollateral();
            Vehicle va = asset.Vehicle;
            vehicle.VehicleTypeId = va.VehicleTypeId;
            vehicle.Brand = va.Brand;
            vehicle.Model = va.Model;
            vehicle.AcquisitionCost = asset.AcquisitionCost ?? 0;

            return vehicle;
        }
    }

    public class LoanFeeModel : BusinessObjectModel
    {
        private bool IsFromPickList
        {
            get 
            {
                return ProductFeatureApplicabilityId != -1;
            }
        }
        public int ProductFeatureApplicabilityId { get; private set; }
        public string Name { get; set; }
        public decimal Amount { get; private set; }
        public decimal ChargeAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal Rate { get; set; }
        public int LoanApplicationFeeId { get; set; }
        public decimal TotalChargePerFee { get; private set; }
        public int FinancialProductId { get; private set; }

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

        private LoanFeeModel()
        {
            this.IsNew = true;
            this.ProductFeatureApplicabilityId = -1;
            this.Name = string.Empty;

            this.ChargeAmount =  0;
            this.BaseAmount = 0;
            this.Rate = 0;
        }

        public LoanFeeModel(ProductFeatureApplicability featureApplicability, decimal loanAmount, int loanTermInMonth)
        {
            this.IsNew = true;
            this.ProductFeatureApplicabilityId = featureApplicability.Id;
            this.Name = featureApplicability.ProductFeature.Name;

            Fee fee = featureApplicability.Fee;
            this.ChargeAmount = fee.ChargeAmount ?? 0;
            this.BaseAmount = fee.BaseAmount ?? 0;
            this.Rate = fee.PercentageRate ?? 0;
            CalculateCharge(featureApplicability, loanAmount, loanTermInMonth);
        }

        public LoanFeeModel(string feeName, decimal amount, int financialProductId, decimal loanAmount, int loanTermInMonth)
            : this()
        {
            this.IsNew = true;
            this.Name = feeName;
            this.Amount = amount;
            this.FinancialProductId = financialProductId;
            CalculateCharge(loanAmount, loanTermInMonth);
        }

        public LoanFeeModel(LoanApplicationFee applicationFee)
            : this()
        {
            this.IsNew = false;
            var application = applicationFee.LoanApplication.Application;
            var productFeature = ProductFeature.GetByName(applicationFee.Particular);
            this.LoanApplicationFeeId = applicationFee.Id;
            var applicationItem = ApplicationItem.Get(application, productFeature);

            this.Name = applicationFee.Particular;
            this.TotalChargePerFee = applicationItem.FeeComputedValue ?? 0;
            this.Amount = applicationFee.Amount;

            this.ProductFeatureApplicabilityId = applicationItem.ProdFeatApplicabilityId;
            var feature = applicationItem.ProductFeatureApplicability;
            if (feature != null && feature.Fee != null)
            {
                Fee fee = feature.Fee;
                this.ChargeAmount = fee.ChargeAmount ?? 0;
                this.BaseAmount = fee.BaseAmount ?? 0;
                this.Rate = fee.PercentageRate ?? 0;
            }
        }

        public void PrepareForSave(LoanApplication loanApplication, DateTime today)
        {
            if (this.IsNew)
            {
                ProductFeatureApplicability featureApp;
                if (this.IsFromPickList == false)
                {
                    ProductFeature feature = ProductFeature.GetByName(this.Name);
                    if (feature == null)
                    {
                        feature = new ProductFeature();
                        feature.Name = this.Name;
                        feature.ProductFeatureCategory = ProductFeatureCategory.FeeType;
                        Context.ProductFeatures.AddObject(feature);
                    }

                    FinancialProduct financialProduct = FinancialProduct.GetById(this.FinancialProductId);
                    featureApp = ProductFeatureApplicability.GetActive(feature, financialProduct);
                    if (featureApp == null)
                        featureApp = ProductFeatureApplicability.Create(feature, financialProduct, today);
                    
                }
                else
                {
                    featureApp = ProductFeatureApplicability.GetById(this.ProductFeatureApplicabilityId);
                }

                ApplicationItem item = new ApplicationItem();
                item.Application = loanApplication.Application;
                item.ProductFeatureApplicability = featureApp;
                item.FeeComputedValue = this.TotalChargePerFee;
                item.EffectiveDate = today;

                LoanApplicationFee fee = new LoanApplicationFee();
                fee.LoanApplication = loanApplication;
                fee.Particular = this.Name;
                fee.Amount = this.Amount;
                //fee.ApplicationItem = item;

                Context.LoanApplicationFees.AddObject(fee);
                Context.ApplicationItems.AddObject(item);
            }
            else if (this.ToBeDeleted)
            {
                LoanApplicationFee applicationFee = Context.LoanApplicationFees.SingleOrDefault(entity => entity.Id == this.LoanApplicationFeeId);
                if (applicationFee != null)
                {
                    var application = applicationFee.LoanApplication.Application;
                    var productFeature = ProductFeature.GetByName(applicationFee.Particular);
                    var applicationItem = ApplicationItem.Get(application, productFeature);
                    applicationItem.EndDate = today;
                    Context.LoanApplicationFees.DeleteObject(applicationFee);
                }
            }
            else if (this.IsNew == false && this.ToBeDeleted == false)
            {
                ProductFeatureApplicability featureApp;
                if (this.IsFromPickList == false)
                {
                    ProductFeature feature = ProductFeature.GetByName(this.Name);
                    if (feature == null)
                    {
                        feature = new ProductFeature();
                        feature.Name = this.Name;
                        feature.ProductFeatureCategory = ProductFeatureCategory.FeeType;
                        Context.ProductFeatures.AddObject(feature);
                    }

                    FinancialProduct financialProduct = FinancialProduct.GetById(this.FinancialProductId);
                    featureApp = ProductFeatureApplicability.GetActive(feature, financialProduct);
                    if (featureApp == null)
                        featureApp = ProductFeatureApplicability.Create(feature, financialProduct, today);

                }
                else
                {
                    featureApp = ProductFeatureApplicability.GetById(this.ProductFeatureApplicabilityId);
                }

                ApplicationItem item = new ApplicationItem();
                item.Application = loanApplication.Application;
                item.ProductFeatureApplicability = featureApp;
                item.FeeComputedValue = this.TotalChargePerFee;
                item.EffectiveDate = today;

                LoanApplicationFee fee = new LoanApplicationFee();
                fee.LoanApplication = loanApplication;
                fee.Particular = this.Name;
                fee.Amount = this.Amount;
                //fee.ApplicationItem = item;

                Context.LoanApplicationFees.AddObject(fee);
                Context.ApplicationItems.AddObject(item);
            }
        }

        public void CalculateCharge(decimal loanAmount, int loanTermInMonth)
        {
            if (this.IsFromPickList == false)
            {
                this.TotalChargePerFee = this.Amount;
                return;
            }

            ProductFeatureApplicability featureApplicability = ProductFeatureApplicability.GetById(this.ProductFeatureApplicabilityId);
            ProductFeature feature = featureApplicability.ProductFeature;
            if (feature.Name == ProductFeature.ServiceCharype.Name)
            {
                if (this.Rate > 0)
                    this.TotalChargePerFee = (this.Rate / 100M) * loanAmount;
                else if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
            else if (feature.Name == ProductFeature.LoanGuaranteeFeeType.Name)
            {
                if (this.Rate > 0)
                    this.TotalChargePerFee = (this.Rate / 100M) * loanAmount * loanTermInMonth;
                else if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount * loanTermInMonth;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
            else if (feature.Name == ProductFeature.DocumentaryStampTaxType.Name)
            {
                if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
        }

        public void CalculateCharge(ProductFeatureApplicability featureApplicability, decimal loanAmount, int loanTermInMonth)
        {
            if (this.IsFromPickList == false)
            {
                this.TotalChargePerFee = this.Amount;
                return;
            }

            ProductFeature feature = featureApplicability.ProductFeature;
            if (feature.Name == ProductFeature.ServiceCharype.Name)
            {
                if (this.Rate > 0)
                    this.TotalChargePerFee = (this.Rate / 100M) * loanAmount;
                else if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
            else if (feature.Name == ProductFeature.LoanGuaranteeFeeType.Name)
            {
                if (this.Rate > 0)
                    this.TotalChargePerFee = (this.Rate / 100M) * loanAmount * loanTermInMonth;
                else if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount * loanTermInMonth;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
            else if (feature.Name == ProductFeature.DocumentaryStampTaxType.Name)
            {
                if (this.ChargeAmount > 0 && this.BaseAmount > 0)
                    this.TotalChargePerFee = (loanAmount / this.BaseAmount) * this.ChargeAmount;
                else if (this.ChargeAmount > 0)
                    this.TotalChargePerFee = this.ChargeAmount;
            }
        }
    }

    public class ChequeModel : BusinessObjectModel
    {
        public int ChequeId { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string _ChequeDate
        {
            get
            {
                return this.ChequeDate.ToString("MMMM dd, yyyy");
            }
        }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public int PaymentId { get; set; }
        public int ReceiptId { get; set; }
        public string Remarks { get; set; }

        public ChequeModel()
        {
            this.IsNew = true;
            this.Amount = 0;
            this.BankId = 0;
            this.Remarks = string.Empty;
            this.BankName = string.Empty;
            this.ChequeNumber = string.Empty;
        }

        public ChequeModel(Cheque cheque)
        {
            this.IsNew = false;
            this.Amount = cheque.Payment.TotalAmount;
            var Bank = Context.Banks.SingleOrDefault(entity => entity.PartyRoleId == cheque.BankPartyRoleId);
            this.BankId = Bank.PartyRoleId;
            this.BankName = Bank.PartyRole.Party.Organization.OrganizationName;
            this.ChequeId = cheque.Id;
            this.ChequeNumber = cheque.Payment.PaymentReferenceNumber;
            this.TransactionDate = cheque.Payment.TransactionDate;
            this.ChequeDate = cheque.CheckDate;
            this.PaymentId = cheque.PaymentId;
            this.ReceiptId = cheque.PaymentId;
            var chequeStatus = Context.ChequeStatus.SingleOrDefault(entity => entity.CheckId == cheque.Id && entity.IsActive == true);
            if (chequeStatus != null)
            {
                this.Status = chequeStatus.ChequeStatusType.Name;
            }
        }

        public ChequeModel(Bank bank, decimal Amount, string chequeNumber, DateTime date, string Remarks, DateTime chequeDate)
        {
            this.IsNew = true;
            this.BankId = bank.PartyRoleId;
            this.TransactionDate = date;
            this.Amount = Amount;
            //this.PaymentId = 
            this.Remarks = Remarks;
            this.BankName = bank.PartyRole.Party.Organization.OrganizationName;
            this.ChequeNumber = chequeNumber;
            this.ChequeDate = chequeDate;
        }

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

        public void PrepareForSave(LoanApplication loanApplication, PartyRole customerPartyRole, int employeePartyRole, DateTime today)
        {
            if (this.IsNew)
            {
                var assoc = Context.ChequeApplicationAssocs.Where(e => e.ApplicationId == loanApplication.ApplicationId);
                if (assoc != null)
                {
                    foreach (var item in assoc)
                    {
                        var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == item.ChequeId);
                        var payments = Payment.GetById(cheque.PaymentId);
                        var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == payments.Id);
                        Receipt receipts = Context.Receipts.SingleOrDefault(entity => entity.Id == rpAssoc.ReceiptId);

                        Context.ChequeApplicationAssocs.DeleteObject(item);
                        Context.Cheques.DeleteObject(cheque);
                        Context.Receipts.DeleteObject(receipts);
                        Context.Payments.DeleteObject(payments);
                        Context.ReceiptPaymentAssocs.DeleteObject(rpAssoc);
                    }
                }

                //create Payment
                var transactionDate = DateTime.Today;
                Payment payment = Payment.CreatePayment(today, transactionDate, customerPartyRole.Id, employeePartyRole,
                    this.Amount, PaymentType.Receipt, PaymentMethodType.PersonalCheckType,
                    SpecificPaymentType.LoanPaymentType, this.ChequeNumber);
                Context.Payments.AddObject(payment);

                //create cheque
                Cheque newCheck = new Cheque();
                newCheck.BankPartyRoleId = this.BankId;
                newCheck.CheckDate = this.ChequeDate;
                newCheck.Payment = payment;
                Context.Cheques.AddObject(newCheck);

                //create cheque association
                ChequeApplicationAssoc chequeAssoc = new ChequeApplicationAssoc();
                chequeAssoc.Cheque = newCheck;
                chequeAssoc.Application = loanApplication.Application;

                //create receipt and receipt payment assoc
                Receipt receipt = Receipt.CreateReceipt(null, this.Amount);
                Receipt.CreateReceiptPaymentAssoc(receipt, payment);

                //Context.SaveChanges();
            }
            else if (this.ToBeDeleted)
            {
                var payment = Payment.GetById(this.PaymentId);
                //TODO:: Changed to new payment
                var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == this.ChequeId && entity.PaymentId == payment.Id);
                var rpAssoc = Context.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == payment.Id);

                var assoc = Context.ChequeApplicationAssocs.SingleOrDefault(entity => entity.ChequeId == cheque.Id);
                //TODO:: Changed to new payment
                Context.ChequeApplicationAssocs.DeleteObject(assoc);
                Context.Cheques.DeleteObject(cheque);
                Context.Receipts.DeleteObject(rpAssoc.Receipt);
                Context.Payments.DeleteObject(rpAssoc.Payment);
                Context.ReceiptPaymentAssocs.DeleteObject(rpAssoc);
            }
            
            else if (this.IsNew == false && this.ToBeDeleted == false)
            {
                var payment = Context.Payments.SingleOrDefault(entity => entity.Id == this.PaymentId);
                payment.TransactionDate = this.TransactionDate;
                payment.PaymentReferenceNumber = this.ChequeNumber;

                var cheque = Context.Cheques.SingleOrDefault(entity => entity.Id == this.ChequeId && entity.PaymentId == payment.Id);
                cheque.BankPartyRoleId = this.BankId;
            }
        }

        /**
        public static Collateral HandleCreate()
        {
        }
        **/
    }
}
