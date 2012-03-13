using System.Linq;using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class UnitOfMeasure
    {
        private const string Days = "Day/s";
        private const string Weeks = "Week/s";
        private const string Months = "Month/s";
        private const string Years = "Year/s";
        private const string Hectares = "Hectares";
        private const string SquareMeter = "Square Meter";
        private const string Daily = "Daily";
        private const string Weekly = "Weekly";
        private const string Quarterly = "Quarterly";
        private const string SemiMonthly = "Semi-Monthly";
        private const string Monthly = "Monthly";
        private const string Annually = "Annually";

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

        public static UnitOfMeasure DaysType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Days);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }

        public static UnitOfMeasure WeeksType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Weeks);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure MonthsType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Months);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure YearsType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Years);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure HectaresType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Hectares);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure SquareMeterType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == SquareMeter);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure DailyType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Daily);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure WeeklyType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Weekly);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure SemiMonthlyType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == SemiMonthly);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure MonthlyType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Monthly);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }
        public static UnitOfMeasure AnnuallyType
        {
            get
            {
                var type = Context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Annually);
                InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

                return type;
            }
        }

        public static int LoanTermToMonth(int termUnit, int loanTerm)
        {
            if (UnitOfMeasure.DaysType.Id == termUnit)
                return loanTerm / 30;
            else if (UnitOfMeasure.WeeksType.Id == termUnit)
                return loanTerm / 4;
            else if (UnitOfMeasure.MonthsType.Id == termUnit)
                return loanTerm;
            else if (UnitOfMeasure.YearsType.Id == termUnit)
                return loanTerm * 12;
            else
                return loanTerm;
        }

        public static int NumberOfDaysOfType(int paymentMode)
        {
            if (UnitOfMeasure.DailyType.Id == paymentMode)
                return 1;
            else if (UnitOfMeasure.WeeklyType.Id == paymentMode)
                return 7;
            else if (UnitOfMeasure.SemiMonthlyType.Id == paymentMode)
                return 15;
            else if (UnitOfMeasure.MonthlyType.Id == paymentMode)
                return 30;
            else if (UnitOfMeasure.AnnuallyType.Id == paymentMode)
                return 360;
            else
                return 0;
        }

        public static int LoanTermToDay(int termUnit, int loanTerm)
        {
            if (UnitOfMeasure.DaysType.Id == termUnit)
                return loanTerm;
            else if (UnitOfMeasure.WeeksType.Id == termUnit)
                return loanTerm * 7;
            else if (UnitOfMeasure.MonthsType.Id == termUnit)
                return loanTerm * 30;
            else if (UnitOfMeasure.YearsType.Id == termUnit)
                return loanTerm * 360;
            else
                return loanTerm;
        }

        //public static bool Validate(int paymentMode, int termUnit, int loanTerm)
        //{
        //    var multiplier = Context.TimeUnitConversions.Where
        //}

        //public static decimal Convert(int from, int to, decimal value)
        //{
        //    return TimeUnitConversion.Convert(from, to, value);
        //}

        //public static decimal Convert(UnitOfMeasure from, UnitOfMeasure to, decimal value)
        //{
        //    return TimeUnitConversion.Convert(from.Id, to.Id, value);
        //}
    }
}