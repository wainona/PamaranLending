using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ChequeUseCases
{
    public partial class EditCheque : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int selectedReceiptId = int.Parse(Request.QueryString["id"]);
                hdnSelectedReceiptID.Text = selectedReceiptId.ToString();
                var payment = Payment.GetReceiptPayment(selectedReceiptId);
                var cheque = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == payment.Id);
                var chequeStatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.CheckId == cheque.Id && entity.IsActive);

                //CHECK IF THE STATUS IS OPEN
                if (chequeStatus.CheckStatusTypeId == ChequeStatusType.ReceivedType.Id)
                {
                    btnEdit.Show();
                }


                dtTransactionDate.MaxDate = DateTime.Now;
                dtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                //dtCheckDate.DisabledDays = ApplicationSettings.DisabledDays;

                var checkStatusTypes = ObjectContext.ChequeStatusTypes;
                checkStatusTypes.ToList();
                strCheckStatus.DataSource = checkStatusTypes;

                strCheckStatus.DataBind();

                RetrieveAndFillReceiptRecord(selectedReceiptId);

                
                if(cheque != null) {
                    var checkLoanAssoc = ObjectContext.ChequeLoanAssocs.SingleOrDefault(entity => entity.ChequeId == cheque.Id);
                    if (checkLoanAssoc != null)
                    {
                        txtAmount.ReadOnly = true;
                        dtCheckDate.ReadOnly = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            SaveReceiptRecord(receiptId);
            ObjectContext.SaveChanges();
        }

        //EDIT RECEIPT RECORD
        protected void SaveReceiptRecord(int receiptId)
        {
            DateTime now = DateTime.Now;
            EditOrNotReceipt(receiptId);//Edit Receipt is edit payment
            EditCheckAndStatus(receiptId);//Edit Check //Edit Status
        }

        protected void RetrieveAndFillReceiptRecord(int receiptId)
        {
            var receipt = Receipt.GetById(receiptId);
            var payment = Payment.GetReceiptPayment(receiptId);
            //
            txtReceivedFrom.Text = payment.PartyRole1.Party.Name;
     
            var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == payment.ProcessedToPartyRoleId);
            var ClassificationType = ObjectContext.ClassificationTypes.SingleOrDefault(entity => entity.Id == customerClassification.ClassificationTypeId);
            //

            dtTransactionDate.Text = payment.TransactionDate.Date.ToString();
            cmbPaymentMethod.Text = payment.PaymentMethodType.Name;//test
            txtAmount.Text = payment.TotalAmount.ToString("N");

            var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == payment.ProcessedByPartyRoleId && entity.EndDate == null);
            txtReceivedBy.Text = payment.PartyRole.Party.Name;

            /*********       CHECK       *********/
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == payment.Id);

            var bankViewList = ObjectContext.BankViewLists.SingleOrDefault(entity => entity.PartyRoleID == check.BankPartyRoleId);
            var checkstatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
            var checkStatusTypeId = checkstatus.ChequeStatusType.Id;
            var checkStatusType = ObjectContext.ChequeStatusTypes.SingleOrDefault(entity => entity.Id == checkStatusTypeId);

            txtBank.Text = bankViewList.Organization_Name;
            hdnBankID.Text = bankViewList.PartyRoleID.ToString();
            txtCheckNumber.Text = payment.PaymentReferenceNumber;
            dtCheckDate.Text = check.CheckDate.Date.ToString();
            cmbCheckStatus.SelectedItem.Value = checkStatusTypeId.ToString();
            //cmbCheckStatus.Text = checkStatusType.Name + " on " + checkstatus.TransitionDateTime.ToString("MMMM dd, yyyy");
            hdnStatus.Text = checkstatus.ChequeStatusType.Name;
            txtCheckRemarks.Text = checkstatus.Remarks;
            //if (checkstatus.ChequeStatusType.Id == ChequeStatusType.ReceivedType.Id || checkstatus.ChequeStatusType.Id == ChequeStatusType.BouncedType.Id)
            //    btnDeposit.Hidden = false;

            //if (checkstatus.CheckStatusTypeId == ChequeStatusType.DepositedType.Id)
            //    btnClear.Disabled = false;
        }

        //BUTTON HANDLERS

        protected void btnDeposit_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            var paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.Cheque == check);

            checkStatus.IsActive = false;
            
            CreateCheckStatus(check, now, ChequeStatusType.DepositedType);

            var cancelledReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
            if (!EndCurrentReceiptStatus(receiptId, cancelledReceiptStatusTypeId)) return;

            //ReceiptStatu newReceiptStatus = new ReceiptStatu();

            //newReceiptStatus.TransitionDateTime = now;
            //newReceiptStatus.IsActive = true;
            //newReceiptStatus.ReceiptStatusTypeId = cancelledReceiptStatusTypeId;
            //newReceiptStatus.ReceiptId = receiptId;

            //ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
            ObjectContext.SaveChanges();
        }

        protected void btnClear_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.Cheque == check);

            checkStatus.IsActive = false;

            CreateCheckStatus(check, now, ChequeStatusType.ClearedType);
            ObjectContext.SaveChanges();
        }

        protected void btnBounced_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.Cheque == check);

            checkStatus.IsActive = false;

            CreateCheckStatus(check, now, ChequeStatusType.BouncedType);
            ObjectContext.SaveChanges();
        }

        protected void btnCancel_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.Cheque == check);

            checkStatus.IsActive = false;

            CreateCheckStatus(check, now, ChequeStatusType.CancelledType);
            ObjectContext.SaveChanges();
        }

        void EditOrNotReceipt(int receiptId)
        {
            var payment = Payment.GetReceiptPayment(receiptId);
            var paymentMethodType = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);
            var receipt = Receipt.GetById(receiptId);

            payment.PaymentMethodTypeId = paymentMethodType.Id;
            if (payment.TransactionDate != dtTransactionDate.SelectedDate) payment.TransactionDate = dtTransactionDate.SelectedDate;
            if (payment.TotalAmount != decimal.Parse(txtAmount.Text)) payment.TotalAmount = decimal.Parse(txtAmount.Text);
            if (receipt.ReceiptBalance != decimal.Parse(txtAmount.Text)) receipt.ReceiptBalance = decimal.Parse(txtAmount.Text);
        }

        void EditCheckAndStatus(int receiptId)
        {
            DateTime now = DateTime.Now;
            var payment = Payment.GetReceiptPayment(receiptId);
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == payment.Id);
            var checkStatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.CheckId == check.Id && entity.IsActive == true);

            check.BankPartyRoleId = int.Parse(hdnBankID.Text);
            check.CheckDate = DateTime.Parse(dtCheckDate.Text);
            payment.PaymentReferenceNumber = txtCheckNumber.Text;

            int newCheckStatusTypeId = int.Parse(cmbCheckStatus.SelectedItem.Value);
            var checkstatusType = ObjectContext.ChequeStatusTypes.SingleOrDefault(entity => entity.Id == newCheckStatusTypeId);
            if (checkStatus.CheckStatusTypeId != newCheckStatusTypeId)
            {
                checkStatus.IsActive = false;
                CreateCheckStatus(check, now, checkstatusType);
            }
            else
            {
                checkStatus.Remarks = txtCheckRemarks.Text;
            }
        }

        void DeleteCheckAndStatuses(int paymentId)
        {
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatuses = ObjectContext.ChequeStatus.Where(entity => entity.CheckId == check.Id);

            foreach (var item in checkStatuses)
            {
                ObjectContext.ChequeStatus.DeleteObject(item);
            }

            ObjectContext.Cheques.DeleteObject(check);
        }

        protected Cheque CreateCheck(int paymentId)
        {
            Cheque newCheck = new Cheque();

            newCheck.BankPartyRoleId = int.Parse(hdnBankID.Text);
            newCheck.CheckDate = dtCheckDate.SelectedDate;
            newCheck.PaymentId = paymentId;

            ObjectContext.Cheques.AddObject(newCheck);
            return newCheck;
        }

        protected void CreateCheckStatus(Cheque check, DateTime now, ChequeStatusType CheckStatusType)
        {
            ChequeStatu newCheckStatus = new ChequeStatu();

            newCheckStatus.CheckId = check.Id;
            newCheckStatus.ChequeStatusType = CheckStatusType;
            newCheckStatus.TransitionDateTime = now;
            newCheckStatus.Remarks = txtCheckRemarks.Text;
            newCheckStatus.IsActive = true;

            ObjectContext.ChequeStatus.AddObject(newCheckStatus);
        }

        protected bool EndCurrentReceiptStatus(int selectedReceiptId, int receiptStatusTypeId)
        {
            var currentReceiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.IsActive == true && entity.ReceiptId == selectedReceiptId);
            var receiptStatusTypeAssoc = ObjectContext.ReceiptStatusTypeAssocs.Where(entity => entity.EndDate == null
                                         && entity.FromStatusTypeId == currentReceiptStatus.ReceiptStatusTypeId &&
                                         entity.ToStatusTypeId == receiptStatusTypeId);

            if (receiptStatusTypeAssoc == null)
            {
                return false;
            }

            currentReceiptStatus.IsActive = false;
            ObjectContext.SaveChanges();
            return true;
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }
    }
}