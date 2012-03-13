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

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class AddForExTransactionSelling : ActivityPageBase
    {
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        //protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        //{
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                ForeignExchangeForm form = this.CreateOrRetrieve<ForeignExchangeForm>();

                var currencies = ObjectContext.Currencies.ToList();
                storeReceivedCurrency.DataSource = currencies;
                storeReceivedCurrency.DataBind();
                storeReleasedCurrency.DataSource = currencies;
                storeReleasedCurrency.DataBind();

                dtTransactionDate.SelectedDate = DateTime.Today;
                cmbCashTypeFrom.SelectedItem.Value = "Bill";
                cmbCashTypeTo.SelectedItem.Value = "Bill";

                txtAmountCashDetail.Text = "0.00";
                txtAmountCheckDetail.Text = "0.00";
                txtAmountReceived.Text = "0.00";
            }
        }
        
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            form.CustomerId = int.Parse(hdnCustomerContactId.Text);
            form.UserId = this.LoginInfo.UserId;
            form.Rate = decimal.Parse(txtRate.Text);
            form.AmountReceived = decimal.Parse(txtAmountReceived.Text);
            form.CurrencyReceivedId = int.Parse(cmbReceivedCurrency.SelectedItem.Value);
            form.AmountReleased = decimal.Parse(txtReleasedAmount.Text);
            form.CurrencyReleasedId = int.Parse(cmbReleasedCurrency.SelectedItem.Value);
            form.TransactionDate = dtTransactionDate.SelectedDate;
            form.CashAmount = decimal.Parse(txtAmountCashDetail.Text);

            form.PrepareForSave();
            ObjectContext.SaveChanges();
        }

        protected void btnSaveAddDenominationReleased_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = this.CreateOrRetrieve<ForeignExchangeForm>();
            ForExDenominationModel model = new ForExDenominationModel();
            model.BillAmount = decimal.Parse(txtBillAmountTo.Text);
            if(cmbCashTypeTo.SelectedItem.Value == "Bill")
                model.SerialNumber = txtSerialNumberTo.Text;

            form.AddDenominationReleased(model);

            decimal totalCash = form.AvailableDenominationsReleased.Sum(entity => entity.BillAmount);
            txtAmountCashDetail.Text = totalCash.ToString("N");

            //decimal totalCash = totalBills + decimal.Parse(txtAmountCoinsDetail.Text);
            //txtAmountCashDetail.Text = totalCash.ToString("N");

            storeDenominationReleased.DataSource = form.AvailableDenominationsReleased;
            storeDenominationReleased.DataBind();

            txtAmountCheckDetail.AllowBlank = true;
        }

        protected void btnSaveAddDenominationReceived_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = this.CreateOrRetrieve<ForeignExchangeForm>();
            ForExDenominationModel model = new ForExDenominationModel();
            model.BillAmount = decimal.Parse(txtBillAmountFrom.Text);
            if (cmbCashTypeFrom.SelectedItem.Value == "Bill")
                model.SerialNumber = txtSerialNumberFrom.Text;

            form.AddDenominationReceived(model);

            decimal totalBills = form.AvailableDenominationsReceived.Sum(entity => entity.BillAmount);
            txtAmountReceived.Text = totalBills.ToString("N");

            //decimal totalCash = totalBills + decimal.Parse(txtAmountCoinsDetail.Text);
            //txtAmountCashDetail.Text = totalCash.ToString("N");

            storeDenominationReceived.DataSource = form.AvailableDenominationsReceived;
            storeDenominationReceived.DataBind();

            txtAmountReceived.ReadOnly = true;

            ConvertOriginal(totalBills);
        }

        protected void cmbCashTypeTo_Select(object sender, DirectEventArgs e) 
        {
            if (cmbCashTypeTo.SelectedItem.Value == "Coins") {
                txtSerialNumberTo.AllowBlank = true;
                txtSerialNumberTo.Hidden = true;
            } else {
                txtSerialNumberTo.AllowBlank = false;
                txtSerialNumberTo.Hidden = false;
            }
        }

        protected void cmbCashTypeFrom_Select(object sender, DirectEventArgs e) 
        {
            if (cmbCashTypeFrom.SelectedItem.Value == "Coins")
            {
                txtSerialNumberFrom.AllowBlank = true;
                txtSerialNumberFrom.Hidden = true;
            } else {
                txtSerialNumberFrom.AllowBlank = false;
                txtSerialNumberFrom.Hidden = false;
            }
        }

        protected void btnSaveAddCheck_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            //var checks = form.AvailableChecks;
            //if(checks.Count() > 0) {
            //    foreach (var item in checks)
            //    {
            //        if (txtCheckNumber.Text == item.CheckNumber)
            //            X.Msg.Alert("Error", "The check number is already used by another check. Please use another check number.");
            //            return;
            //    }
            //}
            ForExChequeModel model = new ForExChequeModel();
            model.BankPartyRoleId = int.Parse(hdnBankId.Text);
            model.BankName = txtBank.Text;
            model.Amount = decimal.Parse(txtAmountAddCheck.Text);
            model.CheckDate = dtCheckDate.SelectedDate;
            model.CheckNumber = txtCheckNumber.Text;

            form.AddCheck(model);
            decimal totalAmountCheck = form.AvailableChecks.Sum(entity => entity.Amount);
            txtAmountCheckDetail.Text = totalAmountCheck.ToString("N");
            storeCheck.DataSource = form.AvailableChecks;
            storeCheck.DataBind();

            txtAmountCashDetail.AllowBlank = true;
        }

        protected void btnDeleteAmountToDenomination_Click(object sender, DirectEventArgs e)
        {
            var rsm = rsmBillDenominationTo.SelectedRows;

            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            form.RemoveDenominationReleased(rsm[0].RecordID);

            decimal totalCash = form.AvailableDenominationsReleased.Sum(entity => entity.BillAmount);
            txtAmountCashDetail.Text = totalCash.ToString("N");

            //decimal totalCash = totalBills + decimal.Parse(txtAmountCoinsDetail.Text);
            //txtAmountCashDetail.Text = totalCash.ToString("N");

            storeDenominationReleased.DataSource = form.AvailableDenominationsReleased;
            storeDenominationReleased.DataBind();

            btnDeleteAmountToDenomination.Disabled = true;
        }

        protected void btnDeleteAmountFromDenomination_Click(object sender, DirectEventArgs e)
        {
            var rsm = rsmBillDenominationFrom.SelectedRows;

            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            form.RemoveDenominationReceived(rsm[0].RecordID);

            decimal totalBills = form.AvailableDenominationsReceived.Sum(entity => entity.BillAmount);
            txtAmountReceived.Text = totalBills.ToString("N");

            //decimal totalCash = totalBills + decimal.Parse(txtAmountCoinsDetail.Text);
            //txtAmountCashDetail.Text = totalCash.ToString("N");

            storeDenominationReceived.DataSource = form.AvailableDenominationsReceived;
            storeDenominationReceived.DataBind();

            btnDeleteAmountFromDenomination.Disabled = true;
            if (form.AvailableDenominationsReceived.Count() == 0)
            {
                txtAmountReceived.Disabled = false;
            }
        }

        protected void btnDeleteCheck_Click(object sender, DirectEventArgs e)
        {
            var rsm = rsmCheck.SelectedRows;
            var id = rsm[0].RecordID;

            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            form.RemoveCheck(id);

            decimal totalAmountCheck = form.AvailableChecks.Sum(entity => entity.Amount);
            txtAmountCheckDetail.Text = totalAmountCheck.ToString("N");
            storeCheck.DataSource = form.AvailableChecks;
            storeCheck.DataBind();

            btnDeleteCheck.Disabled = true;
        }

        private void ConvertOriginal(decimal originalAmount)
        {
            var rate = decimal.Parse(txtRate.Text);
            var converted = originalAmount * rate;
            txtReleasedAmount.Text = converted.ToString("N");
            txtReleasedBalance.Text = converted.ToString("N");
        }

        [DirectMethod]
        public void FillCustomerField()
        {
            var partyRoleId = int.Parse(hdnCustomerContactId.Text);
            var partyRole = PartyRole.GetById(partyRoleId);
            txtCustomerName.Text = partyRole.Party.Name;
        }

        [DirectMethod]
        public void FillExchangeRateField()
        {
            int exRateId = int.Parse(hdnExchangeRateId.Text);
            var exRate = ExchangeRate.GetById(exRateId);

            txtExchangeRate.Text = exRate.Currency.Symbol + " - " + exRate.Currency1.Symbol + " " + exRate.ExchangeRateType.Name;
            txtRate.Text = exRate.Rate.ToString("N");
            hdnRate.Text = exRate.Rate.ToString();
            cmbReceivedCurrency.SelectedItem.Value = exRate.CurrencyFromId.ToString();
            cmbReceivedCurrency.ReadOnly = true;
            cmbReleasedCurrency.SelectedItem.Value = exRate.CurrencyToId.ToString();
            cmbReleasedCurrency.ReadOnly = true;
            txtRate.ReadOnly = true;
            if (exRate.ExchangeRateTypeId == ExchangeRateType.Spot.Id)
            {
                txtRate.ReadOnly = false;
            }
            lblReleasedBalanceCurrency.Text = exRate.Currency1.Symbol;
            lblAmountCashDetailCurrency.Text = exRate.Currency1.Symbol;
            lblAmountCheckDetailCurrency.Text = exRate.Currency1.Symbol;
        }

        [DirectMethod]
        public void FillBankField()
        {
            var bankId = int.Parse(hdnBankId.Text);
            var bank = Bank.GetById(bankId);
            txtBank.Text = bank.PartyRole.Party.Name;
        }

        [DirectMethod]
        public void AllowBlankCheckAmount()
        {
            if (txtAmountCashDetail.Text == string.Empty || decimal.Parse(txtAmountCashDetail.Text) == 0)
            {
                txtAmountCheckDetail.AllowBlank = false;
            }
            else
            {
                txtAmountCheckDetail.AllowBlank = true;
            }
        }
    }
}