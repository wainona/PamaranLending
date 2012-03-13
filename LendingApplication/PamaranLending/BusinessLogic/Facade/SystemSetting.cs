using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class SystemSetting
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

        public static SystemSetting CreateOrEditSystemSetting(FinancialEntities context, string systemSettingName, string value)
        {
            var today = DateTime.Now;
            var currentSetting = context.SystemSettings.SingleOrDefault(entity => entity.EndDate == null && entity.SystemSettingType.Name == systemSettingName);
            if (currentSetting != null)
            {
                if (currentSetting.Value == value)
                    return currentSetting;
                else
                    currentSetting.EndDate = today;
            }

            if (string.IsNullOrWhiteSpace(value))
                return null;

            var settingType = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == systemSettingName);

            SystemSetting setting = new SystemSetting();
            setting.Value = value;
            setting.SystemSettingType = settingType;
            setting.EffectiveDate = today;
            context.SystemSettings.AddObject(setting);

            return setting;
        }

        public static SystemSetting GetActive(SystemSettingType type)
        {
            return Context.SystemSettings.SingleOrDefault(entity => entity.EndDate == null && entity.SystemSettingTypeId == type.Id);
        }

        public static int AllowableDiminishingLoans
        {
            get
            {
                var diminishing = SystemSetting.GetActive(SystemSettingType.AllowableNumberofDiminishingBalanceLoanPerCustomerType);
                if (diminishing == null || string.IsNullOrWhiteSpace(diminishing.Value) == true)
                    return int.MaxValue;
                else
                    return int.Parse(diminishing.Value);
            }
        }

        public static int AllowableStraightLineLoans
        {
            get
            {
                var straight = SystemSetting.GetActive(SystemSettingType.AllowableNumberofStraightLineLoanPerCustomerType);
                if (straight == null || string.IsNullOrWhiteSpace(straight.Value) == true)
                    return int.MaxValue;
                else
                    return int.Parse(straight.Value);

            }
        }

        public static int AgeLimitOfBorrower
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.AgeLimitofBorrowerType);
                int ageLimit = 18;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    ageLimit = int.Parse(setting.Value);

                return ageLimit;
            }
        }

        public static int GracePeriod
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.GracePeriodType);
                int value = 0;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    value = int.Parse(setting.Value);

                return value;
            }
        }

        public static int InvoiceGenerationTiming
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.InvoiceGenerationTimingType);
                int value = 7;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    value = int.Parse(setting.Value);

                return value;
            }
        }

        public static int DemandCollectionAfter
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.DemandCollectionAfterType);
                int value = 1;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    value = int.Parse(setting.Value);

                return value;
            }
        }

        public static int PercentageOfLoanAmountPaid
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.InvoiceGenerationTimingType);
                int value = 0;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    value = int.Parse(setting.Value);

                return value;
            }
        }

        public static string PeriodinCalculatingPenalty
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.PeriodinCalculatingPenaltyType);
                return setting.Value;
            }
        }

        public static int? YearsofLoanstobeDeleted
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.YearsofLoanstobeDeletedType);
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    return int.Parse(setting.Value);
                else
                    return null;
            }
        }

        public static int DatePaymentOption
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.DatePaymentOptionType);
                int value = 1;
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    value = int.Parse(setting.Value);

                return value;
            }
        }

        public static decimal ClerksMaximumHonorableAmount
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.ClerksMaximumHonorableAmountType);
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    return decimal.Parse(setting.Value);
                else
                    return 50000;
            }
        }

        public static int AdvanceChangeNoInterestStartDay
        {
            get
            {
                SystemSetting setting = SystemSetting.GetActive(SystemSettingType.AdvanceChangeNoInterestStartDayType);
                if (setting != null && string.IsNullOrWhiteSpace(setting.Value) == false)
                    return int.Parse(setting.Value);
                else
                    return 20;
            }
        }
    }
}
