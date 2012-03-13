using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ChequeEditorUseCases
{
    public partial class ListChequeEditor : ActivityPageBase
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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DateTime now = DateTime.Now;
            DataSource dataSource = new DataSource();
            
            e.Total = dataSource.Count;
            PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dtTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
            dtCheckDate.DisabledDays = ApplicationSettings.DisabledDays;
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var checkStatusTypes = ObjectContext.ChequeStatusTypes;
                checkStatusTypes.ToList();
                strCheckStatus.DataSource = checkStatusTypes;
                strCheckStatus.DataBind();
            }

            string hehe = hdnReceiptId.Text;

            cmbCheckStatus.SelectedItem.Value = ChequeStatusType.ReceivedType.Id.ToString();
            cmbCheckStatus.ReadOnly = true;
            dtTransactionDate.SelectedDate = DateTime.Now;
            FillReceivedBy();
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            foreach (SelectedRow rows in selected)
            {
                int id = int.Parse(rows.RecordID);
                

                
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        protected void btnEdit_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel row = this.PageGridPanelSelectionModel;
            SelectedRowCollection selected = row.SelectedRows;
            var receiptId = int.Parse(selected[0].RecordID);
            hdnReceiptId.Text = receiptId.ToString();

            var paymentRecord = Payment.GetReceiptPayment(receiptId);

            var chequeRecord = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentRecord.Id);
            var chequeStatus = chequeRecord.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true);
            
            var receivedFromPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == paymentRecord.ProcessedToPartyRoleId && entity.EndDate == null);
            var receivedFromName = Person.GetPersonFullName(receivedFromPartyRole.Party);

            var receivedByPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == paymentRecord.ProcessedByPartyRoleId && entity.EndDate == null);
            var receivedByName = Person.GetPersonFullName(receivedByPartyRole.Party);

            var bank = ObjectContext.BankViewLists.SingleOrDefault(entity => entity.PartyRoleID == chequeRecord.BankPartyRoleId);

            txtReceivedFrom.Text = receivedFromName;
            txtAmount.Text = paymentRecord.TotalAmount.ToString("N");
            txtReceivedBy.Text = receivedByName;
            cmbCheckStatus.SelectedItem.Value = chequeStatus.CheckStatusTypeId.ToString();
            dtTransactionDate.SelectedDate = paymentRecord.TransactionDate;
            cmbPaymentMethod.Text = paymentRecord.PaymentMethodType.Name;

            txtBank.Text = bank.Organization_Name;
            hdnBankID.Text = bank.PartyRoleID.ToString();
            txtCheckNumber.Text = paymentRecord.PaymentReferenceNumber;
            txtCheckRemarks.Text = chequeStatus.Remarks;
            dtCheckDate.SelectedDate = chequeRecord.CheckDate;

            hdnReceiptId.Text = receiptId.ToString();
        }

        protected void btnSearch_DirectClick(object sender, DirectEventArgs e)
        {
            this.PageGridPanelStore.DataBind();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var chequePaymentMethod = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);

            if (string.IsNullOrWhiteSpace(hdnReceiptId.Text))
            {
                DateTime now = DateTime.Now;
                Receipt newrec = Receipt.CreateReceipt(null, decimal.Parse(txtAmount.Text));
                ReceiptStatu newRecStatu = ReceiptStatu.Create(newrec, now, ReceiptStatusType.OpenReceiptStatusType, true);
                Payment newpay = Payment.CreatePayment(now,
                                                        dtTransactionDate.SelectedDate,
                                                        int.Parse(hdnCustomerID.Text),
                                                        int.Parse(hdnLoggedInPartyRoleId.Text),
                                                        decimal.Parse(txtAmount.Text),
                                                        PaymentType.Receipt.Id,
                                                        chequePaymentMethod.Id,
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        txtCheckNumber.Text);

                ReceiptPaymentAssoc newRecPayAssoc = Receipt.CreateReceiptPaymentAssoc(newrec, newpay, newrec.ReceiptBalance.Value);
                Cheque newChe = Cheque.CreateCheque(newpay, int.Parse(hdnBankID.Text), dtCheckDate.SelectedDate);
                ChequeStatu newChqStatu = ChequeStatu.CreateChequeStatus(newChe, int.Parse(cmbCheckStatus.SelectedItem.Value), now, txtCheckRemarks.Text, true);

                ObjectContext.Receipts.AddObject(newrec);
                ObjectContext.ReceiptStatus.AddObject(newRecStatu);
                ObjectContext.Payments.AddObject(newpay);
                ObjectContext.ReceiptPaymentAssocs.AddObject(newRecPayAssoc);
                ObjectContext.Cheques.AddObject(newChe);
                ObjectContext.ChequeStatus.AddObject(newChqStatu);
            }
            else
            {
                UpdateRecord();
            }

            ObjectContext.SaveChanges();
            ClearChequeFields();
            this.PageGridPanelStore.DataBind();
        }

        private void ClearChequeFields()
        {
            DateTime now = DateTime.Now;

            txtReceivedFrom.Clear();
            dtTransactionDate.SelectedDate = now;
            txtAmount.Clear();
            cmbPaymentMethod.Clear();
            txtBank.Clear();
            txtCheckNumber.Clear();
            txtCheckRemarks.Clear();
            dtCheckDate.SelectedDate = now;
        }

        private void FillReceivedBy()
        {
            var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);

            string fullName = Person.GetPersonFullName(employee.PartyRole.Party.Person);
            txtReceivedBy.Text = fullName;
            hdnLoggedInPartyRoleId.Text = employee.PartyRoleId.ToString();
        }

        private void UpdateRecord()
        {
            DateTime now = DateTime.Now;
            var receiptId = int.Parse(hdnReceiptId.Text);
            var chequePaymentMethod = ObjectContext.PaymentMethodTypes.SingleOrDefault(entity => entity.Name == cmbPaymentMethod.Text);
            //PAYMENT TABLE
            var paymentRecord = Payment.GetReceiptPayment(receiptId);
            if(paymentRecord.PaymentReferenceNumber != txtCheckNumber.Text) paymentRecord.PaymentReferenceNumber = txtCheckNumber.Text;
            if(paymentRecord.TotalAmount != decimal.Parse(txtAmount.Text)) paymentRecord.TotalAmount = decimal.Parse(txtAmount.Text);
            if(paymentRecord.TransactionDate != dtTransactionDate.SelectedDate) paymentRecord.TransactionDate = dtTransactionDate.SelectedDate;
            if (paymentRecord.PaymentMethodTypeId != chequePaymentMethod.Id) paymentRecord.PaymentMethodTypeId = chequePaymentMethod.Id;
            //CHEQUE TABLE
            var chequeRecord = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentRecord.Id);
            if(chequeRecord.BankPartyRoleId != int.Parse(hdnBankID.Text)) chequeRecord.BankPartyRoleId = int.Parse(hdnBankID.Text);
            if(chequeRecord.CheckDate != dtCheckDate.SelectedDate) chequeRecord.CheckDate = dtCheckDate.SelectedDate;
            //RECEIPT TABLE
            var receiptRecord = Receipt.GetById(receiptId);
            if (receiptRecord.ReceiptBalance != decimal.Parse(txtAmount.Text)) receiptRecord.ReceiptBalance = decimal.Parse(txtAmount.Text);
            //CHEQUE STATUS
            var checkStatus = ObjectContext.ChequeStatus.SingleOrDefault(entity => entity.CheckId == chequeRecord.Id && entity.IsActive == true);

            int newCheckStatusTypeId = int.Parse(cmbCheckStatus.SelectedItem.Value);
            var checkstatusType = ObjectContext.ChequeStatusTypes.SingleOrDefault(entity => entity.Id == newCheckStatusTypeId);
            if (checkStatus.CheckStatusTypeId != newCheckStatusTypeId)
            {
                checkStatus.IsActive = false;
                CreateCheckStatus(chequeRecord, now, checkstatusType);
            }
            else
            {
                checkStatus.Remarks = txtCheckRemarks.Text;
            }

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

        void EditCheckAndStatus(int paymentId)
        {
            DateTime now = DateTime.Now;
            var payment = ObjectContext.Payments.SingleOrDefault(entity => entity.Id == paymentId);
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
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

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        private class DataSource : IPageAbleDataSource<ChequeViewModel>
        {
            public DataSource()
            {
               
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    var query = CreateQuery();
                    count = query.Count();

                    return count;
                }
            }

            private IQueryable<ChequeViewModel> CreateQuery()
            {
                DateTime now = DateTime.Now.Date;
                var query = from c in ObjectContext.Cheques
                            join p in ObjectContext.Payments on c.PaymentId equals p.Id
                            join cs in ObjectContext.ChequeStatus on c.Id equals cs.CheckId
                            join pmt in ObjectContext.PaymentMethodTypes on p.PaymentMethodTypeId equals pmt.Id
                            join cst in ObjectContext.ChequeStatusTypes on cs.CheckStatusTypeId equals cst.Id
                            where cs.IsActive == true && p.PaymentTypeId == PaymentType.Receipt.Id
                            select new ChequeViewModel()
                            {
                                Payment = p,
                                Cheque = c,
                                ChequeNumber = p.PaymentReferenceNumber,
                                DateReceived = p.TransactionDate,
                                Amount = p.TotalAmount,
                                Status = cst.Name,
                                ChequeDate = c.CheckDate,
                                PaymentMethodType = pmt.Name,
                                Remarks = cs.Remarks
                            };
                query = query.Where(entity => entity.DateReceived == now);
                return query;
            }

            public override List<ChequeViewModel> SelectAll(int start, int limit, Func<ChequeViewModel, string> orderBy)
            {
                List<ChequeViewModel> collection;
                
                var query = CreateQuery();
                collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                
                return collection;
            }
        }

        public class ChequeViewModel
        {
            public Payment Payment { get; set; }
            public Cheque Cheque { get; set; }
            public int ReceiptID
            {
                get
                {
                    var recPayAssoc = ObjectContext.ReceiptPaymentAssocs.SingleOrDefault(entity => entity.PaymentId == this.Payment.Id);
                    return recPayAssoc.ReceiptId;
                }
            }
            public string ChequeNumber { get; set; }
            public string Bank
            {
                get
                {
                    return this.Cheque.PartyRole.Party.Name;
                }
            }
            public DateTime DateReceived { get; set; }
            public string ReceivedFrom
            {
                get
                {
                    return this.Payment.PartyRole1.Party.Name;
                }
            }
            public decimal Amount { get; set; }
            public string Status { get; set; }
            public DateTime ChequeDate { get; set; }
            public string _ChequeDate 
            {
                get
                {
                    return this.ChequeDate.ToString("MMMM dd, yyyy");
                }
            }
            public string PaymentMethodType { get; set; }
            public string Remarks { get; set; }
        }
    }

    
}