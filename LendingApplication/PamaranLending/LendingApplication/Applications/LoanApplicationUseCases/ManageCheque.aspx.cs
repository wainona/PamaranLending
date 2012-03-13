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

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class ManageCheque : ActivityPageBase
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
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                txtPaymentMethod.Text = PaymentMethodType.PersonalCheckType.Name;
            }
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
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