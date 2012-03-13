using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class AssetType
    {
        private const string Land = "Land";
        private const string Jewelry = "Jewelry";
        private const string Vehicle = "Vehicle";
        private const string BankAccount = "Bank Account";
        private const string Machine = "Machine";
        private const string Others = "Others";

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

        public static AssetType LandType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == Land);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }

        public static AssetType JewelryType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == Jewelry);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }

        public static AssetType VehicleType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == Vehicle);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }

        public static AssetType BankAccountType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == BankAccount);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }

        public static AssetType MachineType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == Machine);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }

        public static AssetType OthersType
        {
            get
            {
                var type = Context.AssetTypes.SingleOrDefault(entity => entity.Name == Others);
                InitialDatabaseValueChecker.ThrowIfNull<AssetType>(type);

                return type;
            }
        }
    }
}
