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
    public partial class AddReceipt : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                dtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                //dtCheckDate.DisabledDays = ApplicationSettings.DisabledDays;
                dtTransactionDate.Text = DateTime.Now.ToString();
                dtTransactionDate.MaxDate = DateTime.Now;

                
                    var checkStatusTypes = ObjectContext.ChequeStatusTypes;
                    checkStatusTypes.ToList();
                    strCheckStatus.DataSource = checkStatusTypes;

                    var paymenttypes = ObjectContext.PaymentMethodTypes.Where(entity => entity.Id != PaymentMethodType.ATMType.Id);
                    paymenttypes.ToList();
                    strPaymentMethod.DataSource = paymenttypes;

                    strCheckStatus.DataBind();
                    strPaymentMethod.DataBind();
                

                txtStatus.Text = "Open";
                cmbPaymentMethod.SelectedItem.Value = PaymentMethodType.CashType.Id.ToString();
                cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ReceivedType.Id.ToString();
                var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
                FillReceivedBy();
            }
        }

        private void FillReceivedBy()
        {
            var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
            string fullName = Person.GetPersonFullName(employee.PartyRole.Party.Person);
            txtReceivedBy.Text = fullName;
            hdnLoggedInPartyRoleId.Text = employee.PartyRoleId.ToString();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
            var payCheckId = PaymentMethodType.PayCheckType.Id;
            var personalCheckId = PaymentMethodType.PersonalCheckType.Id;
            DateTime now = DateTime.Now;
            Receipt newReceipt = Receipt.CreateReceipt(null, decimal.Parse(txtAmount.Text));
            ReceiptStatu newReceiptStatu = ReceiptStatu.Create(newReceipt, now, ReceiptStatusType.OpenReceiptStatusType, true);
            Payment newPayment = Payment.CreatePayment( now,
                                                        dtTransactionDate.SelectedDate,
                                                        int.Parse(hdnCustomerID.Text),
                                                        employee.PartyRoleId,
                                                        decimal.Parse(txtAmount.Text),
                                                        PaymentType.Receipt.Id,
                                                        int.Parse(cmbPaymentMethod.SelectedItem.Value),
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        null);
            ReceiptPaymentAssoc newRecPayAssoc = Receipt.CreateReceiptPaymentAssoc(newReceipt, newPayment);

            if (cmbPaymentMethod.SelectedItem.Value == payCheckId.ToString() || cmbPaymentMethod.SelectedItem.Value == personalCheckId.ToString())
            {
                Cheque newChe = Cheque.CreateCheque(newPayment, int.Parse(hdnBankID.Text), dtCheckDate.SelectedDate);
                ChequeStatu newChqStatu = ChequeStatu.CreateChequeStatus(newChe, int.Parse(cmbCheckStatus.SelectedItem.Value), now, txtCheckRemarks.Text, true);
                newPayment.PaymentReferenceNumber = txtCheckNumber.Text;

                //Add objects to ObjectContext for Checks
                ObjectContext.Cheques.AddObject(newChe);
                ObjectContext.ChequeStatus.AddObject(newChqStatu);
            }
            
            //Add objects to ObjectContext
            ObjectContext.Receipts.AddObject(newReceipt);
            ObjectContext.ReceiptStatus.AddObject(newReceiptStatu);
            ObjectContext.Payments.AddObject(newPayment);
            ObjectContext.ReceiptPaymentAssocs.AddObject(newRecPayAssoc);
            ObjectContext.SaveChanges();
        }

        [DirectMethod]
        public void cmbPaymentMethod_Changed()
        {
            
                if (cmbPaymentMethod.SelectedItem.Value == PaymentMethodType.PayCheckType.Id.ToString())
                    cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ClearedType.Id.ToString();
                else
                    cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ReceivedType.Id.ToString();
            
            
        }

        [DirectMethod]
        public void FillDistrictAndStation(string selectedPartyRoleId)
        {
            var newSelectedId = int.Parse(selectedPartyRoleId);
            var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == newSelectedId && entity.EndDate == null);
            var ClassificationType = ObjectContext.ClassificationTypes.SingleOrDefault(entity => entity.Id == customerClassification.ClassificationTypeId);
            this.txtDistrictStation.Text = ClassificationType.District + " " + ClassificationType.StationNumber;
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }
    }
}