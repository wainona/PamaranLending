using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UnitOfMeasureType
    {
        private const string TimeUnit = "Time Unit";
        private const string LengthUnit = "Length Unit";
        private const string DerivedUnit = "Derived Unit";
        private const string TimeFrequency = "Time Frequency";

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

        public static UnitOfMeasureType TimeUnitType
        {
            get{ var type = Context.UnitOfMeasureTypes.SingleOrDefault(entity => entity.Name == TimeUnit);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasureType>(type);

            return type;}
        }
        public static UnitOfMeasureType LengthUnitType
        {
            get{ var type = Context.UnitOfMeasureTypes.SingleOrDefault(entity => entity.Name == LengthUnit);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasureType>(type);

            return type;}
        }
        public static UnitOfMeasureType DerivedUnitType
        {
            get{ var type = Context.UnitOfMeasureTypes.SingleOrDefault(entity => entity.Name == DerivedUnit);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasureType>(type);

            return type;}
        }
        public static UnitOfMeasureType TimeFrequencyType
        {
            get{ var type = Context.UnitOfMeasureTypes.SingleOrDefault(entity => entity.Name == TimeFrequency);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasureType>(type);

            return type;}
        }
    }
}