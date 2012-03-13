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

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class AddCheque : ActivityPageBase
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                return allowed;
            }
        }

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
                var rcvdType = ChequeStatusType.ReceivedType;
                dtTransactionDate.MaxDate = DateTime.Now;
                dtTransactionDate.Text = DateTime.Now.ToString();
                txtChequeStatus.Text = rcvdType.Name;
                hdnCustomerId.Value = Request.QueryString["CustomerId"];
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                hdnIndex.Value = Request.QueryString["C"];
                hdnRandomKey.Value = Request.QueryString["key"];
                int index = int.Parse(hdnIndex.Value.ToString());
                string type = Request.QueryString["T"];
                string randomKey = hdnRandomKey.Value.ToString();
                LoanRestructureForm form = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);

                FillChecks(type, randomKey, form);
            }
        }

        private void FillChecks(string type, string randomKey, LoanRestructureForm form)
        {
            List<ChequeModel> chequeList = new List<ChequeModel>();
            ChequeModel model = new ChequeModel();
            ChequeModel mod = new ChequeModel();
            if (type == "splitLoan")
            {
                if (form.RetrieveCheque1(randomKey) != null)
                {
                    model = form.RetrieveCheque1(randomKey);
                    mod = form.Cheques1.FirstOrDefault();
                    chequeList = form.Cheques1;
                    hdnChequeType.Value = 1;
                }
                else
                {
                    model = form.RetrieveCheque2(randomKey);
                    mod = form.Cheques2.FirstOrDefault();
                    chequeList = form.Cheques2;
                    hdnChequeType.Value = 2;
                }
            }
            else
            {
                model = form.RetrieveCheque(randomKey);
                mod = form.Cheques.FirstOrDefault();
                chequeList = form.Cheques;
                hdnChequeType.Value = 0;
            }

            if (model.RandomKey != mod.RandomKey)
            {
                //btnBankBrowse.Hidden = true;
                txtBank.Text = mod.BankName;
                hdnBankID.Value = mod.BankId;
            }
            else if (model.RandomKey == mod.RandomKey)
            {
                hdnBankID.Value = model.BankId;
                txtBank.Text = model.BankName;
            }

            txtAmount.Text = model.Amount.ToString("N");
            dtCheckDate.SelectedDate = model.ChequeDate;
            txtCheckNumber.Text = model.ChequeNumber;
            txtPaymentMethod.Text = PaymentMethodType.PersonalCheckType.Name;
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber, string randomKey)
        {
            int result = 1;
            LoanRestructureForm form = this.Retrieve<LoanRestructureForm>(ParentResourceGuid);

            int type = 0;
            if (hdnChequeType.Value.ToString() == "0")
                type = 1;
            else if (hdnChequeType.Value.ToString() == "1" || hdnChequeType.Value.ToString() == "2")
                type = 2;

            if (form.ValidateCheckNumber(inputCheckNumber, type, randomKey) == 0 && Cheque.ValidateCheckNumber(inputCheckNumber) == 0)
                result = 0;
            return result;
        }

        /**protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            var paymentMethodType = PaymentMethodType.PersonalCheckType;
            var bank = Bank.GetById(Convert.ToInt32(hdnBankID.Value));
            //Create Payment
            Payment payment = CreatePayment(today, paymentMethodType);
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
            ChequeModel cheque = this.RetrieveBOM<ChequeModel>();
            cheque.Amount = Convert.ToDecimal(txtAmount.Text);
            cheque.BankId = bank.PartyRoleId;
            cheque.BankName = bank.PartyRole.Party.Name;
            cheque.ChequeNumber = txtCheckNumber.Text;
            cheque.PaymentId = payment.Id;
            cheque.TransactionDate = dtTransactionDate.SelectedDate;
            cheque.Remarks = txtaCheckRemarks.Text;

            form.AddCheque(cheque);
        }**/

        /**[DirectMethod]
        public void CreatePayment(DateTime today, PaymentMethodType paymentMethodType)
        {
            Payment newPayment = new Payment();
            newPayment.ProcessedByPartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            newPayment.ProcessedToPartyRoleId = Convert.ToInt32(hdnCustomerId.Value);
            newPayment.PaymentTypeId = PaymentType.ReceiptType.Id;
            newPayment.PaymentMethodTypeId = paymentMethodType.Id;
            newPayment.TransactionDate = dtTransactionDate.SelectedDate;
            newPayment.EntryDate = DateTime.Now;
            newPayment.TotalAmount = Convert.ToDecimal(txtAmount.Text);
            newPayment.PaymentReferenceNumber = txtCheckNumber.Text;

            ObjectContext.Payments.AddObject(newPayment);
            hdnPaymentId.Value = newPayment.Id;
        }**/
    }
}