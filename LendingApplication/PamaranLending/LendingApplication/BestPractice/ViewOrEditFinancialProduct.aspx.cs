using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.BestPractice
{
    public partial class ViewOrEditFinancialProduct : ActivityPageBase
    {
        private void Fill(int financialProductID)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            form.Retrieve(financialProductID);

            txtName.Text = form.Name;
            dtIntroductionDate.SelectedDate = form.IntroductionDate;
            if (form.SalesDiscontinuationDate.HasValue)
                dtSalesDiscontinuationDate.SelectedDate = form.SalesDiscontinuationDate.Value;
            txtComment.Text = form.Comment;
            chkBoxSecured.Checked = form.CollateralSecured;
            chkBoxUnsecured.Checked = form.CollateralUnsecured;
            chkBoxDiminish.Checked = form.DiminishingBalanceMethodType;
            chkBoxStraight.Checked = form.StraightLineMethodType;
            chkBoxAddOn.Checked = form.AddonInterestType;
            chkBoxDiscounted.Checked = form.DiscountedInterestType;

            if (form.NoTermType == true)
            {
                cmbTermOption.SelectedItem.Text = ProductFeature.NoTermType.Name;
                nfMinimumLoanTerm.MinValue = 0;
                nfMaximumLoanTerm.MinValue = 0;
                fsLoanTerm.Disabled = true;
            }
            else if (form.AnyDayToSameDayOfNextMonth == true)
                cmbTermOption.SelectedItem.Text = ProductFeature.AnyDayToSameDayOfNextMonth.Name;
            else if (form.StartToEndOfMonth == true)
                cmbTermOption.SelectedItem.Text = ProductFeature.StartToEndOfMonthType.Name;

            nfMinimumLoanableAmount.Number = (double)form.MinimumLoanableAmount;
            nfMaximumLoanableAmount.Number = (double)form.MaximumLoanableAmount;
            nfMinimumLoanTerm.SetValue(form.MinimumLoanTerm);
            nfMaximumLoanTerm.SetValue(form.MaximumLoanTerm);
            cmbTimeUnit.SetValueAndFireSelect(form.LoanTermTimeUnitId);
            txtProductStatus.Text = form.ProductStatus;

            storeNewInterestRate.DataSource = form.AvailableInterestRates;
            storeNewInterestRate.DataBind();

            storePastDueInterestRate.DataSource = form.AvailablePastDueInterestRates;
            storePastDueInterestRate.DataBind();

            storeFee.DataSource = form.AvailableFees;
            storeFee.DataBind();

            storeRequiredDocumentType.DataSource = form.AvailableRequiredDocuments;
            storeRequiredDocumentType.DataBind();

            var financialProduct = FinancialProduct.GetById(financialProductID);
            EnableValidActivity(ProductStatu.GetActive(financialProduct));
        }

        private void EnableValidActivity(ProductStatu status)
        {
            btnActivate.Disabled = true;
            btnDeactivate.Disabled = true;
            btnRetire.Disabled = true;

            if (status == null)
                return;

            if (status.ProductStatusType.Id != ProductStatusType.ActiveType.Id)
            {
                btnOpenSeparator.Hidden = true;
                btnSaveSeparator.Hidden = true;
                btnSave.Hidden = true;
                btnOpen.Hidden = true;
            }

            if (status.ProductStatusType.Id == ProductStatusType.ActiveType.Id)
            {
                btnActivate.Disabled = true;
                btnDeactivate.Disabled = false;
                btnRetire.Disabled = false;
            }
            else if (status.ProductStatusType.Id == ProductStatusType.InactiveType.Id)
            {
                btnActivate.Disabled = false;
                btnDeactivate.Disabled = true;
                btnRetire.Disabled = true;
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

                int financialProductID = int.Parse(Request.QueryString["id"]);
                if (FinancialProduct.GetById(financialProductID) == null)
                    throw new AccessToDeletedRecordException("The financial product has been deleted by another user.");
                Fill(financialProductID);
            }
        }

        protected void btnActivate_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                ProductStatu status = ProductStatu.ChangeStatus(form.FinancialProductId, ProductStatusType.ActiveType, DateTime.Now);
                EnableValidActivity(status);
            }
        }

        protected void btnRetire_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                ProductStatu status = ProductStatu.ChangeStatus(form.FinancialProductId, ProductStatusType.RetiredType, DateTime.Now);
                status.Remarks = "Retire Manually";
                EnableValidActivity(status);
            }
        }

        protected void btnDeactivate_Click(object sender, DirectEventArgs e)
        {
            FinancialProductForm form = this.CreateOrRetrieve<FinancialProductForm>();
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                ProductStatu status = ProductStatu.ChangeStatus(form.FinancialProductId, ProductStatusType.InactiveType, DateTime.Now);
                EnableValidActivity(status);
            }
        }

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

                if (FinancialProduct.GetById(form.FinancialProductId) == null)
                    throw new AccessToDeletedRecordException("The financial product has been deleted by another user.");

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
                    form.StartToEndOfMonth = false;
                    form.AnyDayToSameDayOfNextMonth = false;
                      form.MinimumLoanTerm = 0;
                      form.MaximumLoanTerm = 0;
                }
                else if (cmbTermOption.SelectedItem.Text == ProductFeature.AnyDayToSameDayOfNextMonth.Name)
                {
                    form.AnyDayToSameDayOfNextMonth = true;
                    form.NoTermType = false;
                    form.StartToEndOfMonth = false;
                }
                else if (cmbTermOption.SelectedItem.Text == ProductFeature.StartToEndOfMonthType.Name)
                {
                    form.StartToEndOfMonth = true;
                    form.NoTermType = false;
                    form.AnyDayToSameDayOfNextMonth = false;
                }
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