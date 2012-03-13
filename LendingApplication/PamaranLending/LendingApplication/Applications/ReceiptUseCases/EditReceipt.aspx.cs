using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ReceiptUseCases
{
    public partial class EditReceipt : ActivityPageBase
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

                var receiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.ReceiptId == selectedReceiptId && entity.IsActive == true);
                var status = receiptStatus.ReceiptStatusType.Name;

                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == selectedReceiptId);

                //CHECK IF THE STATUS IS OPEN
                if (status == "Open")
                {
                    btnEdit.Disabled = false;
                    btnCancelRecord.Disabled = false;
                }

                dtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                dtTransactionDate.MaxDate = DateTime.Now;
                //dtCheckDate.DisabledDays = ApplicationSettings.DisabledDays;

                    var checkStatusTypes = ObjectContext.ChequeStatusTypes;
                    checkStatusTypes.ToList();
                    strCheckStatus.DataSource = checkStatusTypes;

                    var paymenttypes = ObjectContext.PaymentMethodTypes.Where(entity => entity.Id != PaymentMethodType.ATMType.Id);
                    paymenttypes.ToList();
                    strPaymentMethod.DataSource = paymenttypes;

                    strCheckStatus.DataBind();
                    strPaymentMethod.DataBind();

                    RetrieveAndFillReceiptRecord(selectedReceiptId);
                
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            int receiptId = int.Parse(hdnSelectedReceiptID.Text);
            SaveReceiptRecord(receiptId);
        }

        //EDIT RECEIPT RECORD
        protected void SaveReceiptRecord(int receiptId)
        {
            DateTime now = DateTime.Now;
            var newPaymentMethodId = int.Parse(cmbPaymentMethod.SelectedItem.Value);
            var paymentId = Payment.GetReceiptPayment(receiptId).Id;

            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);

            if (check != null) //if previous payment is Pay Check or Personal Check
            {
                //if NEW Receipt is Pay Check or Personal Check
                if (newPaymentMethodId == PaymentMethodType.PayCheckType.Id || newPaymentMethodId == PaymentMethodType.PersonalCheckType.Id)
                {
                    EditOrNotReceipt(paymentId);//Edit Receipt is edit payment
                    EditCheckAndStatus(paymentId);//Edit Check //Edit Status
                }
                //if NEW Receipt is Cash or ATM
                else
                {
                    EditOrNotReceipt(paymentId);//Edit Receipt is edit payment
                    DeleteCheckAndStatuses(paymentId);//Delete All check statuses //Delete Check
                }
            }
            else //if previous payment is Cash or ATM
            {
                //if NEW Receipt is Pay Check or Personal Check
                if (newPaymentMethodId == PaymentMethodType.PayCheckType.Id || newPaymentMethodId == PaymentMethodType.PersonalCheckType.Id)
                {
                    EditOrNotReceipt(paymentId);//Edit Receipt is edit payment
                    Cheque newCheck = CreateCheck(paymentId);
                    CreateCheckStatus(newCheck, now);//Create New Check
                }
                //if NEW Receipt is Cash or ATM
                else
                {
                    EditOrNotReceipt(paymentId);//Edit Receipt is edit payment
                }
            }

            ObjectContext.SaveChanges();
        }

        protected void RetrieveAndFillReceiptRecord(int receiptId)
        {
            var payment = Payment.GetReceiptPayment(receiptId);
            
            txtReceivedFrom.Text = payment.PartyRole.Party.Name;

            var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == payment.ProcessedToPartyRoleId && entity.EndDate == null);
            if (customerClassification != null)
            {
                var ClassificationType = ObjectContext.ClassificationTypes.SingleOrDefault(entity => entity.Id == customerClassification.ClassificationTypeId);
                txtDistrictStation.Text = ClassificationType.District + " " + ClassificationType.StationNumber;
            }
            
            dtTransactionDate.Text = payment.TransactionDate.ToString("MMMM dd, yyyy");
            cmbPaymentMethod.SelectedItem.Value = payment.PaymentMethodType.Id.ToString();//test
            txtAmount.Text = payment.TotalAmount.ToString("N");
            txtReceivedBy.Text = payment.PartyRole1.Party.Name;//name of the TELLER 

            txtCheckNumber.AllowBlank = true;
            txtBank.AllowBlank = true;
            dtCheckDate.AllowBlank = true;

            /*********       CHECK       *********/
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == payment.Id);
            if (check != null)
            {
                var bankViewList = ObjectContext.BankViewLists.SingleOrDefault(entity => entity.PartyRoleID == check.BankPartyRoleId);
                var checkstatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
                var chestatusTypeId = checkstatus.ChequeStatusType.Id;

                txtBank.Text = bankViewList.Organization_Name;
                hdnBankID.Text = bankViewList.PartyRoleID.ToString();
                txtCheckNumber.Text = payment.PaymentReferenceNumber;
                dtCheckDate.Text = check.CheckDate.ToString("MMMM dd, yyyy");
                cmbCheckStatus.SelectedItem.Value = chestatusTypeId.ToString();
                txtCheckRemarks.Text = checkstatus.Remarks;
                fsCheckField.Show();

                txtCheckNumber.AllowBlank = false;
                txtBank.AllowBlank = false;
                dtCheckDate.AllowBlank = false;
            }
            /*********    RECEIPT STATUS    *********/
            var receiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.ReceiptId == receiptId && entity.IsActive == true);
            var receiptStatusType = ObjectContext.ReceiptStatusTypes.SingleOrDefault(entity => entity.Id == receiptStatus.ReceiptStatusTypeId);
            txtStatus.Text = receiptStatusType.Name + " on " + receiptStatus.TransitionDateTime.Value.Date.ToString("MMMM dd, yyyy");
            txtRemarks.Text = receiptStatus.Remarks;
        }

        [DirectMethod]
        public void cmbPaymentMethod_Changed()
        {
            var payCheckId = PaymentMethodType.PayCheckType.Id;
            var personalCheckId = PaymentMethodType.PersonalCheckType.Id;

            if (cmbPaymentMethod.SelectedItem.Value == payCheckId.ToString() || cmbPaymentMethod.SelectedItem.Value == personalCheckId.ToString())
            {
                fsCheckField.Show();
                txtCheckNumber.AllowBlank = false;
                txtBank.AllowBlank = false;
                dtCheckDate.AllowBlank = false;
            }
            else
            {
                fsCheckField.Hide();

                txtCheckRemarks.Clear();
                txtCheckNumber.Clear();
                txtCheckNumber.AllowBlank = true;
                txtBank.Clear();
                txtBank.AllowBlank = true;
                dtCheckDate.Clear();
                dtCheckDate.AllowBlank = true;
                hdnBankID.Clear();
            }
        }

        protected void btnEdit_Click(object sender, DirectEventArgs e)
        {
            btnEdit.Hidden = true;
            btnCancelRecord.Hidden = true;
            btnSave.Hidden = false;
            btnCancel.Hidden = false;
        }

        protected void btnCancel_Click(object sender, DirectEventArgs e)
        {
            btnEdit.Hidden = false;
            btnCancelRecord.Hidden = false;
            btnSave.Hidden = true;
            btnCancel.Hidden = true;
        }

        //trial Function
        Object EditOrNot(object firstObj, object secondObj)
        {
            if (firstObj == secondObj)
                return firstObj;
            else
                return secondObj;
        }

        void EditOrNotReceipt(int paymentId)
        {
            var payment = ObjectContext.Payments.SingleOrDefault(entity => entity.Id == paymentId);
            if(payment.PaymentMethodTypeId != int.Parse(cmbPaymentMethod.SelectedItem.Value)) payment.PaymentMethodTypeId = int.Parse(cmbPaymentMethod.SelectedItem.Value);
            if(payment.TransactionDate.Date != dtTransactionDate.SelectedDate.Date) payment.TransactionDate = dtTransactionDate.SelectedDate;
            if(payment.TotalAmount != decimal.Parse(txtAmount.Text)) payment.TotalAmount = decimal.Parse(txtAmount.Text);
            var recPayAssoc = ObjectContext.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var receipt = recPayAssoc.Receipt;
            if (receipt.ReceiptBalance != decimal.Parse(txtAmount.Text)) receipt.ReceiptBalance = decimal.Parse(txtAmount.Text);
        }

        void EditCheckAndStatus(int paymentId)
        {
            DateTime now = DateTime.Now;
            var payment = ObjectContext.Payments.SingleOrDefault(entity => entity.Id == paymentId);
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.CheckId == check.Id && entity.IsActive == true);

            check.BankPartyRoleId = int.Parse(hdnBankID.Text);
            if (check.CheckDate != dtCheckDate.SelectedDate) check.CheckDate = dtCheckDate.SelectedDate;
            if(payment.PaymentReferenceNumber != txtCheckNumber.Text) payment.PaymentReferenceNumber = txtCheckNumber.Text;

            int newCheckStatusTypeId = int.Parse(cmbCheckStatus.SelectedItem.Value);
            if (checkStatus.CheckStatusTypeId != newCheckStatusTypeId)
            {
                checkStatus.IsActive = false;
                CreateCheckStatus(check, now);
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
            var payment = ObjectContext.Payments.SingleOrDefault(entity => entity.Id == paymentId);
            Cheque newCheck = new Cheque();

            newCheck.BankPartyRoleId = int.Parse(hdnBankID.Text);
            newCheck.CheckDate = Convert.ToDateTime(dtCheckDate.Text);
            newCheck.PaymentId = paymentId;
            payment.PaymentReferenceNumber = txtCheckNumber.Text;

            ObjectContext.Cheques.AddObject(newCheck);
            return newCheck;
        }

        protected void CreateCheckStatus(Cheque check, DateTime now)
        {
            ChequeStatu newCheckStatus = new ChequeStatu();

            newCheckStatus.CheckId = check.Id;
            newCheckStatus.CheckStatusTypeId = int.Parse(cmbCheckStatus.SelectedItem.Value);
            newCheckStatus.TransitionDateTime = now;
            newCheckStatus.Remarks = txtCheckRemarks.Text;
            newCheckStatus.IsActive = true;

            ObjectContext.ChequeStatus.AddObject(newCheckStatus);
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }
    }
}