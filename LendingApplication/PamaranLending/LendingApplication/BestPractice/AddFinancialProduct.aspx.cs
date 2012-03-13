using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;


namespace LendingApplication
{
    public partial class AddFinancialProduct : ActivityPageBase
    {
        //public FinancialProductForm CreateOrRetrieve()
        //{
        //    FinancialProductForm form = null;
        //    if (base.ResourceGuid == null)
        //    {
        //        form = new FinancialProductForm();
        //        base.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
        //    }
        //    else
        //    {
        //        form = (FinancialProductForm)SessionResourceManager.Instance.RetrieveResource(base.ResourceGuid);
        //        SessionResourceManager.Instance.ExtendLifetimeOfResource(base.ResourceGuid);
        //        if (form == null)
        //        {
        //            form = new FinancialProductForm();
        //            base.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
        //        }
        //    }
        //    return form;
        //}
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
                FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
                var unitOfMeasures = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType);
                TimeUnitStore.DataSource = unitOfMeasures;
                TimeUnitStore.DataBind();
                cmbTimeUnit.SelectedIndex = 2;

                var fees = ProductFeature.All(ProductFeatureCategory.FeeType);
                newFeeStore.DataSource = fees;
                newFeeStore.DataBind();

                var interestRates = ProductFeature.All(ProductFeatureCategory.InterestRateType);
                InterestRateStore.DataSource = interestRates;
                InterestRateStore.DataBind();

                var pastDueInterest = ProductFeature.All(ProductFeatureCategory.PastDueInterestRateType);
                PastDueRateStore.DataSource = pastDueInterest;
                PastDueRateStore.DataBind();

                var termOption = ProductFeature.All(ProductFeatureCategory.TermOptionType);
                strTermOption.DataSource = termOption;
                strTermOption.DataBind();
                cmbTermOption.SelectedIndex = 1;
            }
        }

        #region NotUsed
        //private void AssignFees()
        //{
        //    var fees = ProductFeature.All(ProductFeatureCategory.FeeType);
        //    FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();

        //    var chosenFees = from chosen in form.AvailableFees
        //                        select chosen.FeatureID;

        //    var available = fees.Where(fee => chosenFees.Contains(fee.Id) == false);

        //    newFeeStore.DataSource = available;
        //    newFeeStore.DataBind();
        //}

        //private void AssignInterestRates()
        //{
        //    var interestRates = ProductFeature.All(ProductFeatureCategory.InterestRateType);
        //    FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();

        //    var chosenRates = from chosen in form.AvailableInterestRates
        //                     select chosen.FeatureID;

        //    var available = interestRates.Where(rate => chosenRates.Contains(rate.Id) == false);

        //    InterestRateStore.DataSource = available;
        //    InterestRateStore.DataBind();
        //}

        //private void AssignPastDueRates()
        //{
        //    var pastDueInterest = ProductFeature.All(ProductFeatureCategory.PastDueInterestRateType);
        //    FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();

        //    var chosenRates = from chosen in form.AvailablePastDueInterestRates
        //                     select chosen.FeatureID;

        //    var available = pastDueInterest.Where(rate => chosenRates.Contains(rate.Id) == false);

        //    PastDueRateStore.DataSource = available;
        //    PastDueRateStore.DataBind();
        //}
        #endregion
        
        protected void btnSaveInterestRate_Click(object sender, DirectEventArgs e)
        {
            int featureId = Convert.ToInt32(cmbNewInterestRate.SelectedItem.Value);
            decimal interestRate = (decimal)nfNewInterestRate.Number;

            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            form.AddInterestRate(new InterestRateModel(featureId, interestRate));
            storeNewInterestRate.DataSource = form.AvailableInterestRates;
            storeNewInterestRate.DataBind();

            //AssignInterestRates();
        }

        protected void btnDeleteInterestRate_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            SelectedRowCollection rows = this.SelectionModelInterestRate.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveInterestRate(row.RecordID);
            }

            storeNewInterestRate.DataSource = form.AvailableInterestRates;
            storeNewInterestRate.DataBind();
            //AssignInterestRates();
        }

        protected void btnSavePastDueInterestRate_Click(object sender, DirectEventArgs e)
        {
            int featureId = Convert.ToInt32(cmbNewPastDueRate.SelectedItem.Value);
            decimal interestRate = (decimal)nfNewPastDueRate.Number;

            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            form.AddPastDueInterest(new InterestRateModel(featureId, interestRate));
            storePastDueInterestRate.DataSource = form.AvailablePastDueInterestRates;
            storePastDueInterestRate.DataBind();

            //AssignPastDueRates();
        }

        protected void btnDeletePastDue_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            SelectedRowCollection rows = this.SelectionModelPastDueInterestRate.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemovePastDueInterest(row.RecordID);
            }

            storePastDueInterestRate.DataSource = form.AvailablePastDueInterestRates;
            storePastDueInterestRate.DataBind();

            //AssignPastDueRates();
        }

        protected void btnSaveNewFee_Click(object sender, DirectEventArgs e)
        {
            int featureId = Convert.ToInt32(cmbChargeableItems.SelectedItem.Value);
            FeeModel model = new FeeModel(featureId);
            model.ChargeAmount = (decimal)nfChargeAmount.Number;
            model.BaseAmount = (decimal)nfBaseAmount.Number;
            model.Rate = (decimal)nfRate.Number;

            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            form.AddFee(model);
            storeFee.DataSource = form.AvailableFees;
            storeFee.DataBind();

            //AssignFees();
        }

        protected void btnDeleteFee_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            SelectedRowCollection rows = this.SelectionModelFee.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveFee(row.RecordID);
            }

            storeFee.DataSource = form.AvailableFees;
            storeFee.DataBind();

            //AssignFees();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
                form.Name = txtName.Text;
                form.IntroductionDate = dtIntroductionDate.SelectedDate;
                if (dtSalesDiscontinuationDate.SelectedValue != null)
                    form.SalesDiscontinuationDate = dtSalesDiscontinuationDate.SelectedDate;
                form.Comment = txtComment.Text;
                form.CollateralSecured = chkBoxSecured.Checked;
                form.CollateralUnsecured = chkBoxUnsecured.Checked;
                form.DiminishingBalanceMethodType = chkBoxDiminish.Checked;
                form.StraightLineMethodType = chkBoxStraight.Checked;
                form.AddonInterestType = chkBoxAddOn.Checked;
                form.DiscountedInterestType = chkBoxDiscounted.Checked;
                form.MinimumLoanableAmount = (decimal)nfMinimumLoanableAmount.Number;
                form.MaximumLoanableAmount = (decimal)nfMaximumLoanableAmount.Number;
                form.MinimumLoanTerm = (int)nfMinimumLoanTerm.Number;
                form.MaximumLoanTerm = (int)nfMaximumLoanTerm.Number;
                form.LoanTermTimeUnitId = Convert.ToInt32(cmbTimeUnit.SelectedItem.Value);
                if (cmbTermOption.SelectedItem.Text == ProductFeature.NoTermType.Name)
                {
                    form.NoTermType = true;
                    form.MinimumLoanTerm = 0;
                    form.MaximumLoanTerm = 0;
                }
                else if (cmbTermOption.SelectedItem.Text == ProductFeature.AnyDayToSameDayOfNextMonth.Name)
                    form.AnyDayToSameDayOfNextMonth = true;
                else if (cmbTermOption.SelectedItem.Text == ProductFeature.StartToEndOfMonthType.Name)
                    form.StartToEndOfMonth = true;
                form.PrepareForSave();
            }
        }

        [DirectMethod]
        public bool AddRequiredDocument(int requiredDocumentId)
        {
            bool result = true;
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            if (form.RequiredDocumentTypeContains(requiredDocumentId) == false)
            {
                form.AddRequiredDocumentType(new RequiredDocumentModel(requiredDocumentId));
                storeRequiredDocumentType.DataSource = form.AvailableRequiredDocuments;
                storeRequiredDocumentType.DataBind();
            }
            else
                result = false;
            return result;
        }

        [DirectMethod]
        public void AddRequiredDocuments(int[] requiredDocumentIds)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            foreach (var requiredDocumentId in requiredDocumentIds)
            {
                if (form.RequiredDocumentTypeContains(requiredDocumentId) == false)
                {
                    form.AddRequiredDocumentType(new RequiredDocumentModel(requiredDocumentId));
                }
            }
            storeRequiredDocumentType.DataSource = form.AvailableRequiredDocuments;
            storeRequiredDocumentType.DataBind();
        }

        public void btnDeleteDocument_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            SelectedRowCollection rows = this.SelectionModelRequiredDocument.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveRequriedDocument(row.RecordID);
            }

            storeRequiredDocumentType.DataSource = form.AvailableRequiredDocuments;
            storeRequiredDocumentType.DataBind();
        }
    }
}