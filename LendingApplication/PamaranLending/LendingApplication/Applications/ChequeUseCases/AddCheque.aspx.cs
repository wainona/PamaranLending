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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                dtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                dtCheckDate.DisabledDays = ApplicationSettings.DisabledDays;
                dtTransactionDate.Text = DateTime.Now.ToString();
                dtTransactionDate.MaxDate = DateTime.Now;

                var checkStatusTypes = ObjectContext.ChequeStatusTypes;
                checkStatusTypes.ToList();
                strCheckStatus.DataSource = checkStatusTypes;
                strCheckStatus.DataBind();
                
                cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ReceivedType.Id.ToString();
                var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
                txtReceivedBy.Text = Person.GetPersonFullName(employee.PartyRole.Party);
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var chequePaymentMethod = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);
            var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
            DateTime now = DateTime.Now;
            Receipt newReceipt = Receipt.CreateReceipt(null, decimal.Parse(txtAmount.Text));
            ReceiptStatu newRecStatu = ReceiptStatu.Create(newReceipt, now, ReceiptStatusType.OpenReceiptStatusType, true);

            Payment newPayment = Payment.CreatePayment(now,
                                                        dtTransactionDate.SelectedDate,
                                                        int.Parse(hdnCustomerID.Text),
                                                        employee.PartyRoleId,
                                                        decimal.Parse(txtAmount.Text),
                                                        PaymentType.Receipt.Id,
                                                        chequePaymentMethod.Id,
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        txtCheckNumber.Text);
            ReceiptPaymentAssoc newRecPayAssoc = Receipt.CreateReceiptPaymentAssoc(newReceipt, newPayment);

            Cheque newChe = Cheque.CreateCheque(newPayment, int.Parse(hdnBankID.Text), dtCheckDate.SelectedDate);
            ChequeStatu newChqStatu = ChequeStatu.CreateChequeStatus(newChe, int.Parse(cmbCheckStatus.SelectedItem.Value), now, txtCheckRemarks.Text, true);

            ObjectContext.Receipts.AddObject(newReceipt);
            ObjectContext.ReceiptStatus.AddObject(newRecStatu);
            ObjectContext.Payments.AddObject(newPayment);
            ObjectContext.ReceiptPaymentAssocs.AddObject(newRecPayAssoc);
            ObjectContext.Cheques.AddObject(newChe);
            ObjectContext.ChequeStatus.AddObject(newChqStatu);
            ObjectContext.SaveChanges();
        }

        protected void cmbPaymentMethod_OnSelect(object sender, DirectEventArgs e)
        {
            if (cmbPaymentMethod.SelectedItem.Text == "Pay Check")
            {
                cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ClearedType.Id.ToString();
            }
            else
            {
                cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ReceivedType.Id.ToString();
            }
        }

        [DirectMethod]
        public void FillDistrictAndStation(string selectedPartyRoleId)
        {
            //var newSelectedId = int.Parse(selectedPartyRoleId);
            //var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == newSelectedId);
            //var ClassificationType = ObjectContext.ClassificationTypes.SingleOrDefault(entity => entity.Id == customerClassification.ClassificationTypeId);
            //this.txtDistrictStation.Text = ClassificationType.District + " " + ClassificationType.StationNumber;
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }
    }
}