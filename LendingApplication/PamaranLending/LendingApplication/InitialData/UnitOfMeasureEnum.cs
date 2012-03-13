using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LendingApplication;
using LendingApplication.Applications;
using BusinessLogic;

namespace LendingApplication
{
    public class UnitOfMeasureEnum
    {
        public const string Days = "Day/s";
        public const string Weeks = "Week/s";
        public const string Months = "Month/s";
        public const string Years = "Year/s";
        public const string Hectares = "Hectares";
        public const string SquareMeter = "Square Meter";
        public const string Daily = "Daily";
        public const string Weekly = "Weekly";
        public const string SemiMonthly = "Semi-Monthly";
        public const string Monthly = "Monthly";
        public const string Annually = "Annually";

        public static UnitOfMeasure GetDaysType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Days);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetWeeksType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Weeks);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetMonthsType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Months);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetYearsType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Years);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetHectaresType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Hectares);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetSquareMeterType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == SquareMeter);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetDailyType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Daily);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetWeeklyType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Weekly);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetSemiMonthlyType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == SemiMonthly);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetMonthlyType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Monthly);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
        public static UnitOfMeasure GetAnnuallyType(FinancialEntities context)
        {
            var type = context.UnitOfMeasures.SingleOrDefault(entity => entity.Name == Annually);
            InitialDatabaseValueChecker.ThrowIfNull<UnitOfMeasure>(type);

            return type;
        }
    }
}