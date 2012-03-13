using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.SystemSettingsUseCases
{
    public partial class EditSystemSettings : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                return allowed;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                using (var context = new FinancialEntities())
                {
                    txtGracePeriod.Text = GetCurrentSetting(context, SystemSettingsEnums.GracePeriod);
                    txtInvoice.Text = GetCurrentSetting(context, SystemSettingsEnums.InvoiceGenerationTiming);
                    txtDemand.Text = GetCurrentSetting(context, SystemSettingsEnums.DemandCollectionAfter);
                    txtAgeLimit.Text = GetCurrentSetting(context, SystemSettingsEnums.AgeLimitOfBorrower);

                    //
                    txtPercentage.Text = GetCurrentSetting(context, SystemSettingsEnums.PercentageOfLoanAmountPaid);
                    if (txtPercentage.Text.Equals(""))
                    {
                        rdApplyNo.Checked = true;
                        txtPercentage.Disabled = true;
                    }
                    else
                    {
                        rdApplyYes.Checked = true;
                    }

                    cbCalculate.Text = GetCurrentSetting(context, SystemSettingsEnums.CalculatePenalty);

                    //
                    txtAllowDelWithAge.Text = GetCurrentSetting(context, SystemSettingsEnums.AllowDeleteOnLoansWithAge);
                    if (txtAllowDelWithAge.Text.Equals(""))
                    {
                        rdAllowNo.Checked = true;
                        txtAllowDelWithAge.Disabled = true;
                    }
                    else
                    {
                        rdAllowYes.Checked = true;
                    }
                    
                    var paymentDateOption = GetCurrentSetting(context, SystemSettingType.DatePaymentOptionType.Name);
                    if (paymentDateOption == "1") radioAfter.Checked = true;
                    else if(paymentDateOption == "0") radioBefore.Checked = true;

                    txtMaxAmountAppovableByClerk.Text = GetCurrentSetting(context, SystemSettingsEnums.ClerksMaximumHonorableAmount);
                    txtStraightLine.Text = GetCurrentSetting(context, SystemSettingsEnums.StraightLineLoan);
                    txtDiminishing.Text = GetCurrentSetting(context, SystemSettingsEnums.DiminishingBalanceLoan);

                    //var advanceChangeDay = GetCurrentSetting(context, SystemSettingsEnums.AdvanceChangeNoInterestStartDay);
                    cmbAdvanceChangeNoInterestStartDay.SelectedItem.Value = GetCurrentSetting(context, SystemSettingsEnums.AdvanceChangeNoInterestStartDay);
                }
            }
        }

        private string GetCurrentSetting(FinancialEntities context, string systemSettingName)
        {
            var settingType = context.SystemSettingTypes.SingleOrDefault(entity => entity.Name == systemSettingName);
            if (settingType == null)
                throw new NotSupportedException(string.Format("Setting type {0} should be initially added into the database.", systemSettingName));

            var today = DateTime.Now;
            var currentSetting = settingType.SystemSettings.SingleOrDefault(entity => entity.EndDate == null);
            if (currentSetting != null)
                return currentSetting.Value;
            else
                return string.Empty;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                int uomidDays = UnitOfMeasureEnum.GetDaysType(context).Id;
                int uomidYears = UnitOfMeasureEnum.GetYearsType(context).Id;

                SystemSetting gracePeriod = SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.GracePeriod, txtGracePeriod.Text);
                gracePeriod.UomId = uomidDays;

                SystemSetting invoiceGeneration = SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.InvoiceGenerationTiming, txtInvoice.Text);
                invoiceGeneration.UomId = uomidDays;

                SystemSetting demandCollectionAfter = SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.DemandCollectionAfter, txtDemand.Text);
                demandCollectionAfter.UomId = uomidDays;

                SystemSetting ageLimitOfBorrower = SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.AgeLimitOfBorrower, txtAgeLimit.Text);
                ageLimitOfBorrower.UomId = uomidYears;

                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.PercentageOfLoanAmountPaid, txtPercentage.Text);
                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.CalculatePenalty, cbCalculate.Text);

                SystemSetting allowDeleteOnLoansWithAge = SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.AllowDeleteOnLoansWithAge, txtAllowDelWithAge.Text);
                if (allowDeleteOnLoansWithAge != null) allowDeleteOnLoansWithAge.UomId = uomidYears;

                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.ClerksMaximumHonorableAmount, txtMaxAmountAppovableByClerk.Text);
                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.StraightLineLoan, txtStraightLine.Text);
                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.DiminishingBalanceLoan, txtDiminishing.Text);

                string datePaymentOptionValue = string.Empty;
                if (radioAfter.Checked == true)
                    datePaymentOptionValue = "1";
                else if (radioBefore.Checked == true)
                    datePaymentOptionValue = "0";

                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingType.DatePaymentOptionType.Name, datePaymentOptionValue);

                SystemSetting.CreateOrEditSystemSetting(context, SystemSettingsEnums.AdvanceChangeNoInterestStartDay, cmbAdvanceChangeNoInterestStartDay.SelectedItem.Text);
                context.SaveChanges();
            }
        }

        protected void rgdAllow_DirectChange(object sender, DirectEventArgs e)
        {
            if (rdAllowYes.Checked)
            {
                txtAllowDelWithAge.Disabled = false;
            }
            else
            {
                txtAllowDelWithAge.Text = null;
                txtAllowDelWithAge.Disabled = true;
            }
        }

        protected void rdgApply_DirectChange(object sender, DirectEventArgs e)
        {
            if (rdApplyYes.Checked)
            {
                txtPercentage.Disabled = false;
            }
            else
            {
                txtPercentage.Text = null;
                txtPercentage.Disabled = true;
            }
        }
    }
}