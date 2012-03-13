using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class AdditionalLoan : ActivityPageBase
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
                LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(hiddenRandomKey.Value.ToString());
                hiddenBalance.Value = model.TotalLoanBalance;
                this.hiddenStatus.Value = -1;
                txtCurrentLoanBalance.Text = model.TotalLoanBalance.ToString("N");

                FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == model.LoanID && entity.Agreement.EndDate == null);
                LoanAccount loanAccount = financialAccount.LoanAccount;

                AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.FirstOrDefault(enity => enity.IsActive == true);
                
                if (loanAccount.InterestType != null)
                {
                    if (loanAccount.InterestTypeId == InterestType.FixedInterestTYpe.Id)
                        txtInterestRate.Text = "Php " + loanAccount.InterestItems.FirstOrDefault(entity => entity.IsActive == true).Amount.ToString();
                    else if (loanAccount.InterestTypeId == InterestType.ZeroInterestTYpe.Id)
                        txtInterestRate.Text = "0.00";
                    else if (loanAccount.InterestTypeId == InterestType.PercentageInterestTYpe.Id)
                        txtInterestRate.Text = agreementItem.InterestRate.ToString("N") + "%";
                }
                else
                {
                    txtInterestRate.Text = agreementItem.InterestRate.ToString() + "%";
                }

                txtICM.Text = agreementItem.InterestComputationMode;

                HideShowElements();
            }
        }

        protected void HideShowElements()
        {
            int FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var amortizationSchedule = agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.EndDate == null);


            int partyRoleId = int.Parse(hiddenCustomerId.Value.ToString());
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;
            //type == CustomerCategoryType.TeacherType.Name && 
            if (agreementItem.LoanTermLength == 0)
            {
                btnGenerate.Hidden = true;
                chkAddChecks.Hidden = true;
                tlbSeparatorForCheckBx.Hidden = true;
                PageTabPanel.Hidden = true;
                btnSave2.Hidden = false;
                btnSave.Hidden = true;
                btnClose.Hidden = true;
                btnClose2.Hidden = false;
                datLoanReleaseDate.Hidden = true;
                txtICM.Hide();
                txtInterestRate.Hide();
            }
            else
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                var count = form.RetrieveAssociatedCheques(financialAccount.Id).Count();

                if (count != 0)
                    chkAddChecks.Checked = true;
            }
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            LoanRestructureForm oldForm = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureListModel model = oldForm.RetrieveLoanRestructureList(this.hiddenRandomKey.Value.ToString());

            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());
            form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == form.FinancialAccountId && entity.Agreement.EndDate == null);
            FinancialAccountProduct financialAccountProduct = financialAccount.FinancialAccountProducts.FirstOrDefault(entity => entity.EndDate == null);
            FinancialProduct product = financialAccountProduct.FinancialProduct;

            var minimumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, product);
            var minimumLoanable = (minimumAmount != null) ? minimumAmount.Value : 0;

            var maximumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, product);
            var maximumLoanable = (maximumAmount != null) ? maximumAmount.Value : 0;

            var bal = Decimal.Parse(hiddenBalance.Value.ToString());
            var additionalAmount = 0M;
            if (!string.IsNullOrEmpty(txtAdditionalAmount.Text))
            {
                additionalAmount = Decimal.Parse(txtAdditionalAmount.Text);

                var totalAmount = bal + additionalAmount;
                if (totalAmount <= maximumLoanable)
                {
                    //form.AdditionalLoanAccount.RemainingPayments = int.Parse(model.RemainingPayments);
                    form.AdditionalLoanAccount.BalanceToCarryOver = Decimal.Parse(hiddenBalance.Value.ToString());
                    form.AdditionalLoanAccount.AdditionalAmount = Decimal.Parse(txtAdditionalAmount.Text);
                    form.AdditionalLoanAccount.LoanReleaseDate = datLoanReleaseDate.SelectedDate;

                    form.AdditionalLoanAmortizationSchedule.Clear();
                    form.GenerateAdditionalLoanAmortizationSchedule(today);

                    storeAmortizationSchedule.DataSource = form.AdditionalLoanAmortizationSchedule.ToList();
                    storeAmortizationSchedule.DataBind();

                    btnSave.Disabled = false;

                    int partyRoleId = int.Parse(hiddenCustomerId.Value.ToString());
                    var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
                    var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;

                    //if (type == CustomerCategoryType.OthersType.Name)
                    //{
                    var count = form.RetrieveAssociatedCheques(financialAccount.Id).Count();

                    if (chkAddChecks.Checked)
                    {
                        form.Cheques.Clear();
                        foreach (var items in form.AdditionalLoanAmortizationSchedule)
                        {
                            ChequeModel chequeModel = new ChequeModel();
                            chequeModel.BankName = "";
                            chequeModel.ChequeDate = items.ScheduledPaymentDate;
                            chequeModel.ChequeNumber = "";
                            chequeModel.Status = ChequeStatusType.ReceivedType.Name;
                            chequeModel.TransactionDate = today;
                            chequeModel.Amount = items.TotalPayment;

                            form.AddChequesListItem(chequeModel);
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

                    X.MessageBox.Alert("Status", "Amortization schedule successfully generated.").Show();

                }
                else
                {
                    X.MessageBox.Alert("Cannot generate schedule", "New loan amount exceeds the maximum loanable amount. Please specify new additional amount.").Show();
                    btnSave.Disabled = true;
                }
            }
            else
            {
                X.MessageBox.Alert("Cannot generate schedule", "Please specify additional amount.").Show();
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            int isValid = IfChequeHasCheckNumber();
            this.hiddenStatus.Value = 0;
            if (isValid == 0)
            {
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    
                    LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                    if(form.Cheques.Count() == 0)
                        btnGenerate_Click(sender, e);
                    form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
                    form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());
                    form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

                    FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == form.FinancialAccountId && entity.Agreement.EndDate == null);
                    FinancialAccountProduct financialAccountProduct = financialAccount.FinancialAccountProducts.FirstOrDefault(entity => entity.EndDate == null);
                    FinancialProduct product = financialAccountProduct.FinancialProduct;

                    Agreement agreement = financialAccount.Agreement;
                    AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);

                    if (agreementItem.LoanTermLength == 0)
                    {
                        var minimumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, product);
                        var minimumLoanable = (minimumAmount != null) ? minimumAmount.Value : 0;

                        var maximumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, product);
                        var maximumLoanable = (maximumAmount != null) ? maximumAmount.Value : 0;

                        var bal = Decimal.Parse(hiddenBalance.Value.ToString());
                        var additionalAmount = Decimal.Parse(txtAdditionalAmount.Text);

                        var totalAmount = bal + additionalAmount;
                        if (totalAmount <= maximumLoanable)
                        {
                            //form.AdditionalLoanAccount.RemainingPayments = int.Parse(model.RemainingPayments);
                            form.AdditionalLoanAccount.BalanceToCarryOver = Decimal.Parse(hiddenBalance.Value.ToString());
                            form.AdditionalLoanAccount.AdditionalAmount = Decimal.Parse(txtAdditionalAmount.Text);
                            form.AdditionalLoanAccount.LoanReleaseDate = datLoanReleaseDate.SelectedDate;

                            form.AdditionalLoanAmortizationSchedule.Clear();
                            form.GenerateAdditionalLoanAmortizationSchedule(today);
                            form.SaveAdditionalLoanAmortizationSchedule(today);
                        }
                        else
                        {
                            X.MessageBox.Alert("Cannot Continue Saving", "New loan amount exceeds the maximum loanable amount. Please specify new additional amount.").Show();
                            this.hiddenStatus.Value = 1;
                            //e.Success = false;
                            btnSave.Disabled = true;
                        }
                    }
                    else
                    {
                        form.SaveAdditionalLoanAmortizationScheduleWithTerm(today);
                    }
                }
            }
            else
            {
                this.hiddenStatus.Value = 2;
                X.MessageBox.Alert("Cannot Continue Saving", "Cheques should have a check number.").Show();
                e.Success = false;
               
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Checking loan amount...")]
        public void CheckLoanAmount()
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.FinancialAccountId = int.Parse(hdnSelectedLoanID.Value.ToString());
            form.CustomerId = int.Parse(hiddenCustomerId.Value.ToString());
            form.ModifiedBy = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == form.FinancialAccountId && entity.Agreement.EndDate == null);
            FinancialAccountProduct financialAccountProduct = financialAccount.FinancialAccountProducts.FirstOrDefault(entity => entity.EndDate == null);
            FinancialProduct product = financialAccountProduct.FinancialProduct;

            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);

            var minimumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, product);
            var minimumLoanable = (minimumAmount != null) ? minimumAmount.Value : 0;

            var maximumAmount = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, product);
            var maximumLoanable = (maximumAmount != null) ? maximumAmount.Value : 0;

            var bal = Decimal.Parse(hiddenBalance.Value.ToString());
            var additionalAmount = Decimal.Parse(string.IsNullOrWhiteSpace(txtAdditionalAmount.Text) ? "0" : txtAdditionalAmount.Text);

            var totalAmount = bal + additionalAmount;
            if (totalAmount > maximumLoanable)
            {
                X.MessageBox.Alert("Alert", "New loan amount exceeds the maximum loanable amount. Please specify new additional amount.").Show();
                btnGenerate.Disabled = true;
                btnSave.Disabled = true;
                btnSave2.Disabled = true;
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