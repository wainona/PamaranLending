using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class GenerateAmortizationSchedule : ActivityPageBase
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
                LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
                hiddenResourceGUID.Value = this.ResourceGuid;
                hiddenCustomerId.Value = form.CustomerId;
                hdnSelectedLoanID.Value = form.FinancialAccountId;

                Fill(form);
            }
        }

        private void Fill(LoanRestructureForm form)
        {
            LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
            newForm.CustomerId = form.CustomerId;
            newForm.FinancialAccountId = form.FinancialAccountId;
            newForm.FinancialAccountId2 = form.FinancialAccountId2;
            newForm.ConsolidateLoanAccount = form.ConsolidateLoanAccount;
            newForm.ModifiedBy = form.ModifiedBy;
            newForm.FinancialProductId1 = form.FinancialProductId1;

            var today = DateTime.Now;
            newForm.ConsolidateLoanAmortizationSchedule.Clear();
            newForm.GenerateConsolidateLoanAmortizationSchedule(today);

            storeAmortizationSchedule.DataSource = newForm.ConsolidateLoanAmortizationSchedule.ToList();
            storeAmortizationSchedule.DataBind();

            int partyRoleId = newForm.CustomerId;
            var customer = Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            var type = customer.CurrentCustomerCategory.CustomerCategoryType1.Name;

            //if (type == CustomerCategoryType.OthersType.Name)
            //{
                newForm.Cheques.Clear();
                foreach (var items in newForm.ConsolidateLoanAmortizationSchedule)
                {
                    ChequeModel chequeModel = new ChequeModel();
                    chequeModel.BankName = "";
                    chequeModel.ChequeDate = items.ScheduledPaymentDate;
                    chequeModel.ChequeNumber = "";
                    chequeModel.Status = ChequeStatusType.ReceivedType.Name;
                    chequeModel.TransactionDate = today;
                    chequeModel.Amount = items.TotalPayment;

                    newForm.AddChequesListItem(chequeModel);
                }

                storeCheques.DataSource = newForm.AvailableCheques;
                storeCheques.DataBind();

                if (newForm.AvailableCheques.Count() != 0)
                    grdpnlCheque.Disabled = false;
            //}
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            hiddenStatus.Value = 0;
            int isValid = IfChequeHasCheckNumber();
            if (isValid == 0)
            {
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
                    newForm.SaveConsolidateLoanAmortizationSchedule(today);
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
        public int IfChequeHasCheckNumber()
        {
            int result = 0;
            LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
            var cheques = newForm.AvailableCheques;

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

        [DirectMethod]
        public void AddCheque(string id, string amount, string chequeNumber, DateTime date, string remarks, DateTime cheque, string randomKey)
        {
            var BankId = int.Parse(id);
            var Bank = Context.Banks.SingleOrDefault(entity => entity.PartyRoleId == BankId);
            decimal chequeAmount = decimal.Parse(amount);
            DateTime transactionDate = date;

            LoanRestructureForm newForm = this.CreateOrRetrieve<LoanRestructureForm>();
            ChequeModel model = newForm.RetrieveCheque(randomKey);
            model.BankId = BankId;
            model.BankName = Bank.PartyRole.Party.Name;
            model.ChequeNumber = chequeNumber;

            var mod = newForm.Cheques.FirstOrDefault();
            var chcks = newForm.Cheques;

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

            storeCheques.DataSource = newForm.AvailableCheques;
            storeCheques.DataBind();

        }

    }
}