using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using BusinessLogic.FullfillmentForms;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class AmortizationScheduleSplitLoan : ActivityPageBase
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
                this.ParentResourceGuid = Request.QueryString["guid"];
                LoanRestructureForm form = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
                hiddenCustomerId.Value = form.CustomerId;
                LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
                HideShowElements();
                Fill();
            }
        }

        protected void HideShowElements()
        {
            LoanRestructureForm form = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            int FinancialAccountId = form.FinancialAccountId;
            FinancialAccount financialAccount = Context.FinancialAccounts.SingleOrDefault(entity => entity.Id == FinancialAccountId && entity.Agreement.EndDate == null);
            Agreement agreement = financialAccount.Agreement;
            AgreementItem agreementItem = financialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);
            var amortizationSchedule = agreement.LoanAgreement.AmortizationSchedules.SingleOrDefault(entity => entity.EndDate == null);


            int partyRoleId = form.CustomerId;
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;
            //type == CustomerCategoryType.TeacherType.Name && 
            //if (type == CustomerCategoryType.TeacherType.Name)
            //{

            //    //nfLoanTerm.AllowBlank = true;
            //    //nfLoanTerm.Hidden = true;
            //    //btnGenerate.Hidden = true;
            //    //PageTabPanel.Hidden = true;
            //    grdpnlCheque1.Hidden = true;
            //    grdpnlCheque2.Hidden = true;
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

            if (!form.SplitLoanItemsAccount1.AddPostDatedChecks)
                grdpnlCheque1.Hidden = true;

            if (!form.SplitLoanItemsAccount2.AddPostDatedChecks)
                grdpnlCheque2.Hidden = true;
        }

        private void Fill()
        {
            LoanRestructureForm form = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);
            LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
            newForm.FinancialAccountId = form.FinancialAccountId;
            newForm.FinancialProductId1 = form.FinancialProductId1;
            newForm.FinancialProductId2 = form.FinancialProductId2;
            newForm.CustomerId = form.CustomerId;
            newForm.SplitLoanItemsAccount1 = form.SplitLoanItemsAccount1;
            newForm.SplitLoanItemsAccount2 = form.SplitLoanItemsAccount2;
            newForm.ModifiedBy = form.ModifiedBy;
            hiddenResourceGUID.Value = this.ResourceGuid;

            var today = DateTime.Now;
            newForm.SplitLoanAmortizationSchedule1.Clear();
            newForm.SplitLoanAmortizationSchedule2.Clear();
            newForm.GenerateSplitAccountAmortizationSchedule(today);

            storeAmortizationSchedule.DataSource = newForm.SplitLoanAmortizationSchedule1.ToList();
            storeAmortizationSchedule.DataBind();

            store1.DataSource = newForm.SplitLoanAmortizationSchedule2.ToList();
            store1.DataBind();

            int partyRoleId = form.CustomerId;
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;

            //if (type == CustomerCategoryType.OthersType.Name)
            //{
            if (form.SplitLoanItemsAccount1.AddPostDatedChecks)
            {
                grdpnlCheque1.Disabled = false;

                newForm.Cheques1.Clear();
                foreach (var items in newForm.SplitLoanAmortizationSchedule1)
                {
                    ChequeModel chequeModel = new ChequeModel();
                    chequeModel.BankName = "";
                    chequeModel.ChequeDate = items.ScheduledPaymentDate;
                    chequeModel.ChequeNumber = "";
                    chequeModel.Status = ChequeStatusType.ReceivedType.Name;
                    chequeModel.TransactionDate = today;
                    chequeModel.Amount = items.TotalPayment;

                    newForm.AddCheques1(chequeModel);
                }

                storeCheques.DataSource = newForm.AvailableCheques1;
                storeCheques.DataBind();
            }

            if (form.SplitLoanItemsAccount2.AddPostDatedChecks)
            {
                grdpnlCheque2.Disabled = false;

                newForm.Cheques2.Clear();
                foreach (var items in newForm.SplitLoanAmortizationSchedule2)
                {
                    ChequeModel chequeModel = new ChequeModel();
                    chequeModel.BankName = "";
                    chequeModel.ChequeDate = items.ScheduledPaymentDate;
                    chequeModel.ChequeNumber = "";
                    chequeModel.Status = ChequeStatusType.ReceivedType.Name;
                    chequeModel.TransactionDate = today;
                    chequeModel.Amount = items.TotalPayment;

                    newForm.AddCheques2(chequeModel);
                }

                storeCheques2.DataSource = newForm.AvailableCheques2;
                storeCheques2.DataBind();
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
                    LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
                    newForm.SaveSplitAccountAmortizationSchedule(today);
                }
            }
            else
            {
                hiddenStatus.Value = 2;
                X.MessageBox.Alert("Cannot Continue Saving", "Cheques should have a check number.").Show();
                e.Success = false;
                
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
            ChequeModel model = new ChequeModel();
            ChequeModel mod = new ChequeModel();
            List<ChequeModel> chcks = new List<ChequeModel>();
            if (form.RetrieveCheque1(randomKey) != null)
            {
                model = form.RetrieveCheque1(randomKey);
                model.BankId = BankId;
                model.BankName = Bank.PartyRole.Party.Name;
                model.ChequeNumber = chequeNumber;

                mod = form.Cheques1.FirstOrDefault();
                chcks = form.Cheques1;

                storeCheques.DataSource = form.AvailableCheques1;
                storeCheques.DataBind();
            }
            else
            {
                model = form.RetrieveCheque2(randomKey);
                model.BankId = BankId;
                model.BankName = Bank.PartyRole.Party.Name;
                model.ChequeNumber = chequeNumber;

                mod = form.Cheques2.FirstOrDefault();
                chcks = form.Cheques2;

                storeCheques2.DataSource = form.AvailableCheques2;
                storeCheques2.DataBind();
            }

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

        }

        [DirectMethod]
        public int IfChequeHasCheckNumber()
        {
            int result = 0;
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            var cheques = form.AvailableCheques1;
            var cheques2 = form.AvailableCheques2;
            foreach (var cheque in cheques)
            {
                if (string.IsNullOrWhiteSpace(cheque.ChequeNumber))
                {
                    result = 1;
                    break;
                }
            }

            foreach (var cheque in cheques2)
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