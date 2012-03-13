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
    public partial class OpenForExTransaction : ActivityPageBase
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int selectedId = int.Parse(Request.QueryString["id"]);
                hdnForExId.Text = selectedId.ToString();

                ForeignExchangeForm form = this.CreateOrRetrieve<ForeignExchangeForm>();

                var currencies = ObjectContext.Currencies.ToList();
                storeOriginalCurrency.DataSource = currencies;
                storeOriginalCurrency.DataBind();
                storeConvertedCurrency.DataSource = currencies;
                storeConvertedCurrency.DataBind();
                //cmbCurrencyMain.SelectedItem.Value = 

                /*************************** ForExDetail Window ******************************/
                
                /*****************************************************************************/

                var billAmounts = ObjectContext.BillAmounts.ToList();
                storeBillAmountFrom.DataSource = billAmounts;
                storeBillAmountFrom.DataBind();

                FillFields(selectedId);
            }
        }

        protected void FillFields(int ForExId)
        {
            var forEx = ObjectContext.ForeignExchanges.SingleOrDefault(entity => entity.Id == ForExId);
            //parent forex detail
            var parentForExDetail = (from fea in ObjectContext.ForeignExchangeDetailAssocs
                                    join fed in ObjectContext.ForExDetails on fea.ForExDetailId equals fed.Id
                                    where fea.ForExId == ForExId && fed.ParentForExDetailId == null
                                    select fed).SingleOrDefault();
            //parent forex detail's denomination
            var parentDenom = ObjectContext.Denominations.Where(entity => entity.ForExDetailId == parentForExDetail.Id).ToList();

            //cash forex detail
            var cashForExDetail = (from fed in ObjectContext.ForExDetails
                                 where fed.ParentForExDetailId == parentForExDetail.Id && fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                 select fed).SingleOrDefault();
            if (cashForExDetail != null)
            {
                //cash forex detail's denomination
                var cashDenom = ObjectContext.Denominations.Where(entity => entity.ForExDetailId == cashForExDetail.Id);
                var cashDenomList = cashDenom.ToList();
                storeDenominationConverted.DataSource = ToBillDenominationModel(cashDenom);
                storeDenominationConverted.DataBind();
                txtAmountCashDetail.Text = cashForExDetail.Amount.ToString("N");
            }

            var forExChecks = (from fed in ObjectContext.ForExDetails
                              join fec in ObjectContext.ForExCheques on fed.Id equals fec.ForExDetailId
                              where fed.ParentForExDetailId == parentForExDetail.Id && fed.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id
                              select fec);

            if (forExChecks != null)
            {
                var forExChecksList = forExChecks.ToList();
                storeCheck.DataSource = ToForExCheckModel(forExChecks);
                storeCheck.DataBind();
            }


            decimal totalCheckAmount = 0;
            foreach (var item in forExChecks)
            {
                totalCheckAmount += item.ForExDetail.Amount;
            }

            txtCustomerName.Text = forEx.PartyRole1.Party.Name;
            txtReceivedBy.Text = forEx.PartyRole.Party.Name;
            dtTransactionDate.SelectedDate = forEx.TransactionDate;
            txtExchangeRate.Text = forEx.Currency.Description + " - " + forEx.Currency1.Description;
            txtRate.Text = forEx.Rate.ToString("N");
            txtOriginalAmount.Text = forEx.AmountReceived.ToString("N");
            cmbOriginalCurrency.SelectedItem.Value = forEx.ReceivedCurrencyId.ToString();
            txtConvertedAmount.Text = forEx.AmountReleased.ToString("N");
            cmbConvertedCurrency.SelectedItem.Value = forEx.ReleasedCurrencyId.ToString();
            
            txtAmountCheckDetail.Text = totalCheckAmount.ToString("N");
            //lblConvertedCurrency.Text = forEx.Currency1.Symbol;
            string convertedCurrencySymbol = Currency.GetCurrencyById(forEx.ReleasedCurrencyId).Symbol;
            lblConvertedCurrency.Text = convertedCurrencySymbol;
            lblAmountCashDetailCurrency.Text = convertedCurrencySymbol;
            lblAmountCheckDetailCurrency.Text = convertedCurrencySymbol;

            storeDenominationOriginal.DataSource = ToBillDenominationModel(parentDenom);
            storeDenominationOriginal.DataBind();

        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            form.CustomerId = int.Parse(hdnCustomerContactId.Text);
            form.UserId = this.LoginInfo.UserId;
            form.Rate = decimal.Parse(txtRate.Text);
            form.AmountReceived = decimal.Parse(txtOriginalAmount.Text);
            form.CurrencyReceivedId = int.Parse(cmbOriginalCurrency.SelectedItem.Value);
            form.AmountReleased = decimal.Parse(txtConvertedAmount.Text);
            form.CurrencyReleasedId = int.Parse(cmbConvertedCurrency.SelectedItem.Value);
            form.TransactionDate = dtTransactionDate.SelectedDate;

            form.PrepareForSave();
            ObjectContext.SaveChanges();
        }

        protected void btnSaveAddDenomination_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = this.CreateOrRetrieve<ForeignExchangeForm>();
            ForExDenominationModel model = new ForExDenominationModel();
            model.BillAmount = decimal.Parse(cmbBillAmountFrom.Text);
            model.SerialNumber = txtSerialNumberFrom.Text;

            form.AddDenominationReleased(model);
            storeDenominationConverted.DataSource = form.AvailableDenominationsReleased;
            storeDenominationConverted.DataBind();
        }

        protected void btnSaveAddCheck_Click(object sender, DirectEventArgs e)
        {
            ForeignExchangeForm form = CreateOrRetrieve<ForeignExchangeForm>();
            ForExChequeModel model = new ForExChequeModel();
            model.BankPartyRoleId = int.Parse(hdnBankId.Text);
            model.BankName = txtBank.Text;
            model.Amount = decimal.Parse(txtAmountAddCheck.Text);
            model.CheckDate = dtCheckDate.SelectedDate;
            model.CheckNumber = txtCheckNumber.Text;

            form.AddCheck(model);
            storeCheck.DataSource = form.AvailableChecks;
            storeCheck.DataBind();
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
            cmbOriginalCurrency.SelectedItem.Value = exRate.CurrencyFromId.ToString();
            cmbConvertedCurrency.SelectedItem.Value = exRate.CurrencyToId.ToString();
        }

        [DirectMethod]
        public void FillBankField()
        {
            var bankId = int.Parse(hdnBankId.Text);
            var bank = Bank.GetById(bankId);
            txtBank.Text = bank.PartyRole.Party.Name;
        }

        private List<BillDenomination> ToBillDenominationModel(IEnumerable<Denomination> denoms)
        {
            List<BillDenomination> list = new List<BillDenomination>();
            foreach (var item in denoms)
            {
                BillDenomination billdenom = new BillDenomination(item);
                list.Add(billdenom);
            }
            return list;
        }

        private List<ForExCheckModel> ToForExCheckModel(IEnumerable<ForExCheque> forExCheques)
        {
            List<ForExCheckModel> cheques = new List<ForExCheckModel>();
            foreach (var item in forExCheques)
            {
                ForExCheckModel model = new ForExCheckModel(item);
                cheques.Add(model);
            }
            return cheques;
        }
    }

    public class BillDenomination
    {
        public int Id { get; set; }
        public string Type
        {
            get
            {
                if (this.SerialNumber == null)
                    return "Coins";

                return "Bill";
            }
        }
        public decimal BillAmount { get; set; }
        public string SerialNumber { get; set; }

        public BillDenomination(Denomination denom)
        {
            this.Id = denom.Id;
            this.BillAmount = denom.BillAmount;
            this.SerialNumber = denom.SerialNumber;

        }
    }

    public class ForExCheckModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int BankPartyRoleId { get; set; }
        public string BankName { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }
        public string _CheckDate
        {
            get
            {
                return this.CheckDate.ToString("MMM dd, yyyy");
            }
        }

        public ForExCheckModel(ForExCheque cheque)
        {
            this.Id = cheque.Id;
            this.Amount = cheque.ForExDetail.Amount;
            this.BankPartyRoleId = cheque.BankPartyRoleId;
            this.BankName = cheque.PartyRole.Party.Name;
            this.CheckNumber = cheque.CheckNumber;
            this.CheckDate = cheque.CheckDate;
        }
    }
}