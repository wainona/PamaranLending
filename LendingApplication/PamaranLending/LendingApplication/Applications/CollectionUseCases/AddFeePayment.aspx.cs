using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class AddFeePayment : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
                return allowed;
            }
        }


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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
                hiddenResourceGUID.Text = this.ResourceGuid;
                var today = DateTime.Now;
                txtCashAmount.SetValue(0);
                txtCheckAmount.SetValue(0);
                txtATMAmount.SetValue(0);
                txtAmountDisbursed.SetValue(0);
                txtTransactionDate.MaxDate = today;
                txtTransactionDate.SelectedDate = today;
                txtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;


                storeParticulars.DataSource = FeePaymentParticular.GetAllFeeParticulars();
                storeParticulars.DataBind();
            }

        }
        public void btnSaveATM_Click(object sender, DirectEventArgs e)
        {
            string referencenumber = txtATMReferenceNum.Text;
            decimal amount = Decimal.Parse(txtATMAmount1.Text);
            FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
            if (string.IsNullOrWhiteSpace(HiddenRandomKey2.Text))
            {
                ATMPaymentModel model = new ATMPaymentModel();
                model.Amount = amount;
                model.ATMReferenceNumber = referencenumber;
                form.AddATM(model);
   
            }
            else
            {
                ATMPaymentModel model = form.GetATMs(HiddenRandomKey2.Text);
                model.ATMReferenceNumber = referencenumber;
                model.Amount = amount;
                model.MarkEdited();
            }

            grdPnlATMStore.DataSource = form.AvailableATMs;
            grdPnlATMStore.DataBind();
        }

        public void onBtnATMDelete_Click(object sender, DirectEventArgs e)
        {
            FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
            SelectedRowCollection rows = this.grdPnlATMSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveATMs(row.RecordID);
            }
            grdPnlATMStore.DataSource = form.AvailableATMs;
            grdPnlATMStore.DataBind();
        }
        public void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
                form.TransactionDate = txtTransactionDate.SelectedDate;
                form.ReceivedFrom = txtDisbursedTo.Text;
                form.CustomerID = int.Parse(txtCustId.Text);
                form.ReceivedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                form.Remarks = string.Empty;
                form.TotalAmountPaid = decimal.Parse(txtTotalAmountPaid.Text);
                form.TotalAmountReceived = decimal.Parse(txtAmountDisbursed.Text);
                form.ATMAmount = decimal.Parse(txtATMAmount.Text);
                form.CashAmount = decimal.Parse(txtCashAmount.Text);
                form.CheckAmount = decimal.Parse(txtCheckAmount.Text);
                form.PrepareForSave();
            }
        }

        [DirectMethod]
        public void ChangeToMoneyFormat(decimal amount)
        {
            txtTotalAmountPaid.Text = amount.ToString("N");
        }

        public void btnSaveOtherItems_Click(object sender, DirectEventArgs e)
        {
            //string particular = txtParticular.Text;
            string particular = cmbParticulars.SelectedItem.Text;
            decimal amount = Decimal.Parse(txtAmount.Text);
            FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
            if (string.IsNullOrWhiteSpace(hiddenRandomKey.Text))
            {
                form.AddOtherItems(new FeeItemsModel(particular, amount));
            }
            else
            {
                FeeItemsModel model = form.GetOtherItems(hiddenRandomKey.Text);
                model.Particular = particular;
                model.Amount = amount;
                model.MarkEdited();
            }
            PageGridPanelStore.DataSource = form.AvailableOtherItems;
            PageGridPanelStore.DataBind();
        }

        public void otherItemsDelete_Click(object sender, DirectEventArgs e)
        {
            FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
            SelectedRowCollection rows = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveOtherItems(row.RecordID);
            }
            PageGridPanelStore.DataSource = form.AvailableOtherItems;
            PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        [DirectMethod]
        public string AddNewParticular()
        {
            var particular = txtParticularA.Text;
            if (!string.IsNullOrWhiteSpace(particular))
            {
                FeePaymentParticular.CreateFeeParticular(particular);
                ObjectContext.SaveChanges();
            }
            return particular;
        }

        [DirectMethod]
        public bool CheckIfValid()
        {
            var allParticulars = FeePaymentParticular.GetAllFeeParticulars();
            var count = allParticulars.Where(x => x.Name == txtParticularA.Text);

            return count.Count() == 0;
        }

        [DirectMethod]
        public void LoadCombo(string particular)
        {
            cmbParticulars.SelectedItem.Text = particular;
            storeParticulars.DataSource = FeePaymentParticular.GetAllFeeParticulars();
            storeParticulars.DataBind();
        }

        [DirectMethod]
        public void AddChequesManually(string bankName, string bankBranch, string checkNumber, string checkType, DateTime checkDate, decimal checkAmount, int BankId)
        {
            FeePaymentForm form = CreateOrRetrieve<FeePaymentForm>();
            AddChequesModel model = new AddChequesModel();
            model.BankName = bankName;
            model.Branch = bankBranch;
            model.BankPartyRoleId = BankId;
            model.CheckType = checkType;
            model.CheckNumber = checkNumber;
            model.CheckDate = checkDate;
            model.Amount = checkAmount;

            form.AddCheques(model);
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }

        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            FeePaymentForm form = CreateOrRetrieve<FeePaymentForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            FeePaymentForm form = this.CreateOrRetrieve<FeePaymentForm>();
            SelectedRowCollection rows = this.ChequesSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCheques(row.RecordID);
            }

            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
            btnDeleteCheck.Disabled = true;
        }

    }
}