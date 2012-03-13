using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class ChangeInterest : ActivityPageBase
    {
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

        //public override List<string> UserTypesAllowed
        //{
        //    get
        //    {
        //        List<string> allowed = new List<string>();
        //        allowed.Add("Super Admin");
        //        allowed.Add("Loan Clerk");
        //        allowed.Add("Admin");
        //        return allowed;
        //    }
        //}

        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                hdnSelectedLoanID.Value = Request.QueryString["loanId"];
                hiddenResourceGUID.Value = this.ResourceGuid;
                hiddenCustomerId.Value = Request.QueryString["cid"];
                hiddenRandomKey.Value = Request.QueryString["b"];
                ParentResourceGuid = Request.QueryString["guid"];
                datLoanReleaseDate.SelectedDate = DateTime.Now;
                datLoanReleaseDate.DisabledDays = ApplicationSettings.DisabledDays;

                LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
                LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey.Value.ToString());
                this.hiddenBalance.Value = model.TotalLoanBalance;

                FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model.LoanID && entity.Agreement.EndDate == null);
                LoanAccount loanAccount = financialAccount.LoanAccount;

                AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.FirstOrDefault(enity => enity.IsActive == true);
                
                if (loanAccount.InterestType != null)
                {
                    if (loanAccount.InterestTypeId == InterestType.FixedInterestTYpe.Id)
                        dfOldInterestRate.Text = "Php " + loanAccount.InterestItems.FirstOrDefault(entity => entity.IsActive == true).Amount.ToString();
                    else if (loanAccount.InterestTypeId == InterestType.ZeroInterestTYpe.Id)
                        dfOldInterestRate.Text = "0.00";
                    else if (loanAccount.InterestTypeId == InterestType.PercentageInterestTYpe.Id)
                        dfOldInterestRate.Text = agreementItem.InterestRate.ToString("N") + "%";
                }
                else
                {
                    dfOldInterestRate.Text = agreementItem.InterestRate.ToString() + "%";
                }

                var totalReceivable = 0M;

                totalReceivable = oldForm.ComputeTotalLoanReceivables(loanAccount);

                txtCarryOverReceivables.Text = totalReceivable.ToString("N");
                nfReceivableAdd.MaxValue = double.Parse(totalReceivable.ToString());
                //if (totalReceivable == 0)
                    nfReceivableAdd.Number = 0;
                nfLoanTerm.Number = agreementItem.LoanTermLength;
                HideShowElements();

       
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            int isValid = IfChequeHasCheckNumber();
            hiddenStatus.Value = 0;
            if (isValid == 0)
            {
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                    if(form.Cheques.Count() == 0)
                        btnGenerate_Click(sender, e);
                    
                    //form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
                    //form.CustomerId = int.Parse(this.hiddenCustomerId.Value.ToString());
                    //form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

                    ////form.ChangeInterestAccount.RemainingPayments = int.Parse(model.RemainingPayments);
                    //form.ChangeInterestAccount.BalanceToCarryOver = Decimal.Parse(hiddenNewBalance.Value.ToString());
                    //form.ChangeInterestAccount.NewLoanAmount = form.ChangeInterestAccount.BalanceToCarryOver;
                    //form.ChangeInterestAccount.NewInterestRate = Decimal.Parse(nfNewInterestRate.Number.ToString());
                    //form.ChangeInterestAccount.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
                    form.SaveChangeInterestAmortizationSchedule(today);
                }
            }
            else
            {
                hiddenStatus.Value = 2;
                X.MessageBox.Alert("Cannot Continue Saving", "Cheques should have a check number.").Show();
                e.Success = false;
                
            }
        }

        protected void HideShowElements()
        {
            
            int FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            var loanAccount = financialAccount.LoanAccount;

            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var amortizationSchedule = agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.EndDate == null);

            int partyRoleId = int.Parse(hiddenCustomerId.Value.ToString());
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;
            //type == CustomerCategoryType.TeacherType.Name && 
            //if (type == CustomerCategoryType.TeacherType.Name)
            //{
            //    pnlCheque.Hidden = true;
            //}
            //else
            //{
            //    if (agreementItem.LoanTermLength == 0)
            //    {
            //        nfLoanTerm.AllowBlank = true;
            //        nfLoanTerm.Hidden = true;
            //        btnGenerate.Hidden = true;
            //        PageTabPanel.Hidden = true;
            //    }
            //}
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            var count = form.RetrieveAssociatedCheques(financialAccount.Id).Count();

            if (count != 0)
                chkAddChecks.Checked = true;
        }

        [DirectMethod]
        public void computeNewLoanBalance()
        {
            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey.Value.ToString());
            var loanAccount = Context.LoanAccounts.FirstOrDefault(entity => entity.FinancialAccountId == model.LoanID);
            var loanAmount = model.LoanAmount;
            var receivableToCarryOver = Decimal.Parse(nfReceivableAdd.Number.ToString());
            loanAmount += receivableToCarryOver;

            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model.LoanID && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var interestComputationMode = agreementItem.InterestComputationMode;

            var interestRateDescription = agreementItem.InterestRateDescription;
            var newInterestRate = decimal.Parse(string.IsNullOrWhiteSpace(nfNewInterestRate.Text) ? "0" : nfNewInterestRate.Text);


            var newLoanBalance = Math.Round(Math.Floor(oldForm.ComputeNewLoanBalance(loanAccount, receivableToCarryOver, newInterestRate, interestComputationMode, interestRateDescription)), 2);

            txtCarryOverBalance.Text = newLoanBalance.ToString("N");
            hiddenNewBalance.Value = newLoanBalance;
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey.Value.ToString());
            
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            form.CustomerId = int.Parse(this.hiddenCustomerId.Value.ToString());
            form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            //form.ChangeInterestAccount.RemainingPayments = int.Parse(model.RemainingPayments);
            form.ChangeInterestAccount.RemainingPayments = int.Parse(nfLoanTerm.Number.ToString());
            form.ChangeInterestAccount.Term = form.ChangeInterestAccount.RemainingPayments;
            form.ChangeInterestAccount.BalanceToCarryOver = Decimal.Parse(hiddenNewBalance.Value.ToString());
            form.ChangeInterestAccount.NewLoanAmount = form.ChangeInterestAccount.BalanceToCarryOver;
            form.ChangeInterestAccount.NewInterestRate = Decimal.Parse(nfNewInterestRate.Number.ToString());
            form.ChangeInterestAccount.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            form.ChangeInterestAmortizationSchedule.Clear();
            form.GenerateChangeInterestAmortizationSchedule(today);

            storeAmortizationSchedule.DataSource = form.ChangeInterestAmortizationSchedule.ToList();
            storeAmortizationSchedule.DataBind();

            int partyRoleId = int.Parse(hiddenCustomerId.Value.ToString());
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;

            //if (type == CustomerCategoryType.OthersType.Name)
            //{
                //List<ChequeModel> chequesTemp = form.Cheques;
            var count = form.RetrieveAssociatedCheques(form.FinancialAccountId).Count();

            if (chkAddChecks.Checked)
            {
                form.Cheques.Clear();
                int i = 0;
                foreach (var items in form.ChangeInterestAmortizationSchedule)
                {
                    ChequeModel chequeModel = new ChequeModel();
                    chequeModel.BankName = "";
                    chequeModel.ChequeNumber = "";
                    chequeModel.ChequeDate = items.ScheduledPaymentDate;
                    
                    chequeModel.Status = ChequeStatusType.ReceivedType.Name;
                    chequeModel.TransactionDate = today;
                    chequeModel.Amount = items.TotalPayment;

                    form.AddChequesListItem(chequeModel);

                    i++;
                }

                storeCheques.DataSource = form.AvailableCheques;
                storeCheques.DataBind();

                if (form.AvailableCheques.Count() != 0)
                    grdpnlCheque.Disabled = false;
            }
            else
            {
                form.Cheques.Clear();
                storeCheques.RemoveAll();
                grdpnlCheque.Disabled = true;
            }
            //btnAddCheque.Disabled = true;
        }

        protected void btnDeleteCheque_Click(object sender, DirectEventArgs e)
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            SelectedRowCollection rows = this.SelectionModelCheque.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCheque(row.RecordID);
            }
            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();

            if (form.AvailableCheques.Count() < int.Parse(nfLoanTerm.Text))
            {
                btnAddCheque.Disabled = false;
            }

        }

        [DirectMethod]
        public void AddCheque(string id, string amount, string chequeNumber, DateTime date, string remarks, DateTime cheque, string randomKey)
        {
            var BankId = int.Parse(id);
            var Bank = Context.Banks.SingleOrDefault(entity => entity.PartyRoleId == BankId);
            decimal chequeAmount = decimal.Parse(amount);
            DateTime transactionDate = date;

            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            //ChequeModel model = new ChequeModel(Bank, chequeAmount, chequeNumber, transactionDate, remarks, cheque);
            //if (form.AvailableCheques.Count() == 0 || form.AvailableCheques.Count() < int.Parse(nfLoanTerm.Text))
            //    form.AddChequesListItem(model);
            //if (form.AvailableCheques.Count() == int.Parse(nfLoanTerm.Text))
            //    btnAddCheque.Disabled = true;
            ChequeModel model = form.RetrieveCheque(randomKey);
            model.BankId = BankId;
            model.BankName = Bank.PartyRole.Party.Name;
            model.ChequeNumber = chequeNumber;

            var mod = form.Cheques.FirstOrDefault();
            var chcks = form.Cheques;

            if (mod.RandomKey == model.RandomKey)
            {
                foreach (var ch in chcks)
                {
                    if (ch.RandomKey != mod.RandomKey)
                    {
                        ch.BankId = mod.BankId;
                        ch.BankName = mod.BankName;
                    }
                }
            }

            storeCheques.DataSource = form.AvailableCheques;
            storeCheques.DataBind();

        }

        [DirectMethod]
        public int GetChequeCount()
        {
            int count = 0;
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            count = form.AvailableCheques.Count();

            return count;
        }

        [DirectMethod]
        public int IfChequeHasCheckNumber()
        {
            int result = 0;
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            var cheques = form.AvailableCheques;

            foreach (var cheque in cheques)
            {
                if (string.IsNullOrWhiteSpace(cheque.ChequeNumber))
                {
                    result = 1;
                    break;
                }
            }

            return result;
        }
    }
}