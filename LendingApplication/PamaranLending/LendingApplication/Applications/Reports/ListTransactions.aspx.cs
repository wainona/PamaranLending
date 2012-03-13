using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using BusinessLogic;

namespace LendingApplication.Applications.Reports
{
    public partial class ListTransactions : ActivityPageBase
    {
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
                strCurrency.DataSource = Currency.GetCurrencies().ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = 0;
            }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DateTime today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var phCurrencyId = Currency.GetCurrencyBySymbol("PHP").Id;

            var currency = Currency.GetCurrencyById(currencyId);

            var cheque = RetrieveCheckTransactions(today, tomorrow, currencyId);
            PageGridPanelStore.DataSource = cheque;
            PageGridPanelStore.DataBind();

            CalculateCheckhNetTotal();
        }

        protected void RefreshDataCash(object sender, StoreRefreshDataEventArgs e)
        {
            DateTime today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var phCurrencyId = Currency.GetCurrencyBySymbol("PHP").Id;

            var currency = Currency.GetCurrencyById(currencyId);

            var cash = RetrieveCashTransactions(today, tomorrow, currencyId);
            PageGridPanelStoreCash.DataSource = cash;
            PageGridPanelStoreCash.DataBind();
            
            CalculateCashNetTotal();
        }

        [DirectMethod]
        public void Fill()
        {
            PageGridPanelStoreCash.DataBind();

            PageGridPanelStore.DataBind();

            CalculateCashNetTotal();
            CalculateCheckhNetTotal();
        }

        public void CalculateCashNetTotal()
        {
            DateTime today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);

            var cash = RetrieveCashTransactions(today, tomorrow, currencyId);
            var cashTotal = cash.Sum(x => (x._Received - x._Released));

            txtCashNetTotal.Text = cashTotal.ToString("N");
        }

        public void CalculateCheckhNetTotal()
        {
            DateTime today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);

            var check = RetrieveCheckTransactions(today, tomorrow, currencyId);
            var checkTotal = check.Sum(x => (x._Received - x._Released));

            txtChequeNetTotal.Text = checkTotal.ToString("N");
        }

        private List<TransactionsModel> RetrieveCashTransactions(DateTime startDate, DateTime endDate, int currencyId)
        {
            var employeePartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            IEnumerable<Payment> Receipts;
            IEnumerable<Payment> Disbursements;
            var today = DateTime.Today;

            #region Receipts Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allPayments = from pymnt in ObjectContext.Payments
                                  join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                  join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                  where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                  && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                  && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                  select pymnt;

                var foreignPayments = from pymnt in ObjectContext.Payments
                                      join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                      join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                      join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                      where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                       && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                      select pymnt;
                //Payments in Peso
                Receipts = allPayments.Except(foreignPayments);
            }
            else
            {
                //DAPAT CASH RANI
                Receipts = from pymnt in ObjectContext.Payments
                           join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                           join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                           join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                           where pymnt.EntryDate >= startDate.Date && pymnt.EntryDate <= endDate.Date
                            && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                            && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                            && pca.CurrencyId == currencyId
                           select pymnt;
            }

            var receiptPayments = from pymnt in Receipts
                                  where pymnt.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                  select new TransactionsModel()
                                  {
                                      Payment = pymnt,
                                      ForExDetail = null,
                                      _Received = pymnt.TotalAmount,
                                      _Released = 0
                                  };
            #endregion
            #region Disbursements Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allDisbursements = from pymnt in ObjectContext.Payments
                                       join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                       where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                       && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                       select pymnt;

                var foreignDisbursements = from pymnt in ObjectContext.Payments
                                           join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                           join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                           where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                           && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                           && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                           select pymnt;

                Disbursements = allDisbursements.Except(foreignDisbursements);
            }
            else
            {
                //DAPAT CASH RANI
                Disbursements = from pymnt in ObjectContext.Payments
                                join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                where pymnt.EntryDate >= startDate.Date && pymnt.EntryDate <= endDate.Date
                                && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                && pca.CurrencyId == currencyId
                                && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                select pymnt;
            }

            var disbursementPayments = from pymnt in Disbursements
                                       where pymnt.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                       select new TransactionsModel()
                                       {
                                           Payment = pymnt,
                                           ForExDetail = null,
                                           _Received = 0,
                                           _Released = pymnt.TotalAmount
                                       };
            #endregion
            #region Foreign Exchange Transactions
            //foreign exchange
            var forExCashReceived = from fed in ObjectContext.ForExDetails
                                    join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.Id equals fda.ForExDetailId
                                    join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                    where fed.ParentForExDetailId == null && fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                     && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                     && fed.CurrencyId == currencyId && fe.ProcessedByPartyRoleId == employeePartyRoleId
                                    select new TransactionsModel()
                                    {
                                        Payment = null,
                                        ForExDetail = fed,
                                        _Received = fed.Amount,
                                        _Released = 0
                                    };

            var forExCashReleased = from fed in ObjectContext.ForExDetails
                                    join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.ParentForExDetailId equals fda.ForExDetailId
                                    join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                    where fed.ParentForExDetailId != null && fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                     && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                     && fed.CurrencyId == currencyId && fe.ProcessedByPartyRoleId == employeePartyRoleId
                                    select new TransactionsModel()
                                    {
                                        Payment = null,
                                        ForExDetail = fed,
                                        _Received = 0,
                                        _Released = fed.Amount
                                    };
            #endregion

            var list = new List<TransactionsModel>();

            var receivedAndReleased = receiptPayments.Concat(disbursementPayments).Concat(forExCashReceived).Concat(forExCashReleased).ToList();
            list.AddRange(receivedAndReleased);

            return list;
        }

        private List<TransactionsModel> RetrieveCheckTransactions(DateTime startDate, DateTime endDate, int currencyId)
        {
            var employeePartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            IEnumerable<Payment> Receipts;
            IEnumerable<Payment> Disbursements;
            var today = DateTime.Today;

            #region Receipts Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allPayments = from pymnt in ObjectContext.Payments
                                  join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                  join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                  where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                  && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                  && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                  select pymnt;

                var foreignPayments = from pymnt in ObjectContext.Payments
                                      join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                      join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                      join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                      where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                       && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                      select pymnt;
                //Payments in Peso
                Receipts = allPayments.Except(foreignPayments);
            }
            else
            {
                //DAPAT CASH RANI
                Receipts = from pymnt in ObjectContext.Payments
                           join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                           join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                           join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                           where pymnt.EntryDate >= startDate.Date && pymnt.EntryDate <= endDate.Date
                            && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                            && pymnt.ParentPaymentId == null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                            && pca.CurrencyId == currencyId
                           select pymnt;
            }
            var cancelledpostdatedcheques = from chq in ObjectContext.Cheques
                                            join pymnt in ObjectContext.Payments.Where(entity => entity.PaymentTypeId == PaymentType.Receipt.Id)
                                            on chq.PaymentId equals pymnt.Id
                                            join chqassoc in ObjectContext.ChequeApplicationAssocs on chq.Id equals chqassoc.ChequeId
                                            join las in ObjectContext.LoanApplicationStatus on chqassoc.ApplicationId equals las.ApplicationId
                                            where las.IsActive == true && (las.LoanApplicationStatusType.Id
                                            == LoanApplicationStatusType.RejectedType.Id
                                            || las.LoanApplicationStatusType.Id == LoanApplicationStatusType.CancelledType.Id)
                                             && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                            select pymnt;

            Receipts = Receipts.Except(cancelledpostdatedcheques);


            var receiptPaymentsCheck = from pymnt in Receipts
                                       where (pymnt.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id || pymnt.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id)
                                       select new TransactionsModel()
                                       {
                                           Payment = pymnt,
                                           ForExDetail = null,
                                           _Received = pymnt.TotalAmount,
                                           _Released = 0
                                       };
            #endregion
            #region Disbursements Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allDisbursements = from pymnt in ObjectContext.Payments
                                       join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                       where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                       && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                       select pymnt;

                var foreignDisbursements = from pymnt in ObjectContext.Payments
                                           join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                           join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                           where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                           && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                           && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                           select pymnt;

                Disbursements = allDisbursements.Except(foreignDisbursements);
            }
            else
            {
                //DAPAT CASH RANI
                Disbursements = from pymnt in ObjectContext.Payments
                                join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                where pymnt.EntryDate >= startDate.Date && pymnt.EntryDate <= endDate.Date
                                && pymnt.ParentPaymentId != null && pymnt.ProcessedByPartyRoleId == employeePartyRoleId
                                && pca.CurrencyId == currencyId
                                && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                select pymnt;
            }

            var disbursementPaymentsCheck = from pymnt in Disbursements
                                            where (pymnt.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id || pymnt.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id)
                                            select new TransactionsModel()
                                            {
                                                Payment = pymnt,
                                                ForExDetail = null,
                                                _Received = 0,
                                                _Released = pymnt.TotalAmount
                                            };
            #endregion
            #region Foreign Exchange Transactions
            //foreign exchange
            var forExCheckReleased = from fed in ObjectContext.ForExDetails
                                     join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.ParentForExDetailId equals fda.ForExDetailId
                                     join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                     where fed.ParentForExDetailId != null && fed.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id
                                      && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                      && fed.CurrencyId == currencyId
                                     select new TransactionsModel()
                                     {
                                         Payment = null,
                                         ForExDetail = fed,
                                         _Received = 0,
                                         _Released = fed.Amount
                                     };
            #endregion

            var list = new List<TransactionsModel>();

            var receivedAndReleased = receiptPaymentsCheck.Concat(disbursementPaymentsCheck).Concat(forExCheckReleased).ToList();
            list.AddRange(receivedAndReleased);

            return list;
        }

        public class TransactionsModel
        {
            private Payment payment;
            public ForExDetail ForExDetail { get; set; }
            public string TransactionType { get; set; }
            public string Received
            {
                get
                {
                    return this._Received.ToString("N");
                }
            }
            public string Released
            {
                get
                {
                    return this._Released.ToString("N");
                }
            }
            public string EntryDate
            {
                get
                {
                    return this._EntryDate.Value.ToString("yyyy-MM-dd");
                }
            }
            public DateTime? _EntryDate
            {
                get
                {
                    if (this.Payment != null)
                    {
                        return this.Payment.TransactionDate;
                    }
                    else if (this.ForExDetail != null)
                    {
                        ForeignExchangeDetailAssoc forExDetailsAssoc;
                        if (this.ForExDetail.ParentForExDetailId == null)
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.Id);
                        else
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.ParentForExDetailId.Value);

                        return forExDetailsAssoc.ForeignExchange.TransactionDate;
                    }

                    return null;
                }
            }
            public string CheckNumber { get; set; }
            public Payment Payment
            {
                get
                {
                    return this.payment;
                }

                set
                {
                    payment = value;

                    if (payment != null)
                    {
                        this.TransactionType = payment.PaymentType.Name;
                        if (!string.IsNullOrEmpty(payment.PaymentReferenceNumber))
                            this.CheckNumber = payment.PaymentReferenceNumber;
                    }
                }
            }
            public decimal _Received { get; set; }
            public decimal _Released { get; set; }

            public TransactionsModel()
            {
               
            }
        }
    }
}