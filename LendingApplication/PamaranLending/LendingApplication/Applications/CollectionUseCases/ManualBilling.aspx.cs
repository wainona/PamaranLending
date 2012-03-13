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

namespace LendingApplication.Applications.FinancialManagement.CollectionUseCases
{
    public partial class ManualBilling : ActivityPageBase
    {
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
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
                hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                hiddenType.Value = Request.QueryString["RoleType"];
                dtBill.SelectedDate = DateTime.Today;
                var date = dtBill.SelectedDate;
                if (hiddenType.Text == "Borrower")
                {
                    LoanPaymentModel borrower = (LoanPaymentModel)form.RetrieveBorrower(hiddenRandomKey.Text);
                    this.Register(borrower);
                    
                    Fill(borrower,date);
                }
                else if (hiddenType.Text == "Guarantor")
                {
                    LoanPaymentModel guarantor = (LoanPaymentModel)form.RetrieveGuaCoBorower(hiddenRandomKey.Text);
                    this.Register(guarantor);
                    Fill(guarantor,date);
                }
                
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var date = dtBill.SelectedDate;
            var lastDayOfTheMonth = GenerateBillFacade.LastDayOfMonthFromDateTime(date);
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(date);
            int id = int.Parse(hiddenLoanID.Value.ToString());
            int agreementid = int.Parse(hiddenAgreementID.Value.ToString());
            var loanAccount = ObjectContext.LoanAccounts.FirstOrDefault(entity => entity.FinancialAccountId == id);
            if (loanAccount.InterestTypeId == InterestType.FixedInterestTYpe.Id
               && loanAccount.InterestTypeId == InterestType.ZeroInterestTYpe.Id)
                validDueDate = lastDayOfTheMonth;
            var agreementitem = ObjectContext.AgreementItems.FirstOrDefault(entity => entity.AgreementId == agreementid && entity.IsActive == true);
            if (txtInterestPayment.Text != "0.00")
            {
                using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
                {
                    GenerateBillFacade.GenerateAndSaveInterest(loanAccount, agreementitem, GenerateBillFacade.ManualBillingSave, date, validDueDate, date);
                    GenerateBillFacade.GenerateInterestForLastMonth(date, loanAccount, agreementitem, GenerateBillFacade.ManualBillingSave);

                }

                UpdateList(date, loanAccount);
            }
        }

        [DirectMethod]
        public void GenerateBill()
        {
            var date = dtBill.SelectedDate;
            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
            if (hiddenType.Text == "Borrower")
            {
                LoanPaymentModel borrower = (LoanPaymentModel)form.RetrieveBorrower(hiddenRandomKey.Text);
                this.Register(borrower);

                Fill(borrower, date);
            }
            else if (hiddenType.Text == "Guarantor")
            {
                LoanPaymentModel guarantor = (LoanPaymentModel)form.RetrieveGuaCoBorower(hiddenRandomKey.Text);
                this.Register(guarantor);
                Fill(guarantor, date);
            }
        }

        private void UpdateList(DateTime now,LoanAccount loanAccount)
        {
            var receivables = from l in ObjectContext.LoanAccounts
                              join r in ObjectContext.Receivables on l.FinancialAccountId equals r.FinancialAccountId
                              where r.Date == now && l.FinancialAccountId == loanAccount.FinancialAccountId
                              select r;
  
            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
            LoanPaymentModel borrower = this.RetrieveBOM<LoanPaymentModel>();
            
            //foreach receivable newly generated
            if (txtInterestPayment.Text != "0.00")
            {
                //borrower.InterestReceivables.Clear();

                foreach(var interestReceivable in receivables){
                    ReceivableInfo interestInfo = new ReceivableInfo();
                    interestInfo.Amount = interestReceivable.Amount;
                    interestInfo.ReceivableId = interestReceivable.Id;
                    interestInfo.DueDate = interestReceivable.DueDate;
                    borrower.InterestReceivables.Add(interestInfo);
                }
            }
            borrower.UpdateAll();
        }

        protected void Fill(LoanPaymentModel model, DateTime selectedDate)
        {
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(selectedDate);
            hiddenLoanID.Value = model.LoanID;
            decimal interest = 0;
            //loanbalance,loanreleasedate, agreementid,
            var query = from f in ObjectContext.FinancialAccounts
                              join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                              join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                              where f.Id == model.LoanID && a.EndDate == null && ai.IsActive == true
                              select new 
                              {
                                  loanaccount = f.LoanAccount,
                                  agreementitem = ai
                                    };
            if (query.Count() != 0)
            {
                var loanAccount = query.FirstOrDefault().loanaccount;
                dtBill.MinDate = (DateTime)loanAccount.LoanReleaseDate;
                var agreementitem = query.FirstOrDefault().agreementitem;
                if (selectedDate > loanAccount.LoanReleaseDate)
                {
                    interest += GenerateBillFacade.GenerateAndSaveInterest(loanAccount, agreementitem, GenerateBillFacade.ManualBillingDisplay, selectedDate, validDueDate, selectedDate);
                    interest += GenerateBillFacade.GenerateInterestForLastMonth(selectedDate, loanAccount, agreementitem, GenerateBillFacade.ManualBillingDisplay);
                }
                txtInterestPayment.Text = interest.ToString("N");
                var balance = loanAccount.LoanBalance;
                txtTotalLoanBalance.Text = balance.ToString("N"); ;
                hiddenAgreementID.Value = agreementitem.AgreementId;
            }

        }
    }
}