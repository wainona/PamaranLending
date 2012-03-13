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
    public partial class ListReceipts : ActivityPageBase
    {
        List<CurrencyModelForFilter> currencyModelForFilterList;

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
            DataSource dataSource = new DataSource();
            dataSource.InputSearchString = txtSearchInput.Text;
            dataSource.FilterCategoryBy = cmbFilterCategories.Text;
            dataSource.FilterItemBy = cmbFilterItems.SelectedItem.Text;
            dataSource.SearchBy = cmbSearchBy.SelectedItem.Text;
            //Initial Date Check
            dataSource.FromDate = dtFrom.SelectedDate;
            dataSource.ToDate = dtTo.SelectedDate;

            e.Total = dataSource.Count;
            var orderedDataSource = dataSource.Select(e.Start, e.Limit);
            this.PageGridPanelStore.DataSource = orderedDataSource;
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //dtTo.SelectedDate = DateTime.Now;
                //dtFrom.MaxDate = DateTime.Now;
                //dtTo.MaxDate = DateTime.Now;
                btnOpen.Disabled = true;
                btnCancel.Disabled = true;
            }
        }

        protected void btnSave_onDirectEventClick(object sender, DirectEventArgs e)
        {
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var cancelledReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
                if (!EndCurrentReceiptStatus(receiptId, cancelledReceiptStatusTypeId)) return;

                ReceiptStatu newReceiptStatus = new ReceiptStatu();
                DateTime now = DateTime.Now;

                newReceiptStatus.TransitionDateTime = now;
                newReceiptStatus.IsActive = true;
                newReceiptStatus.ReceiptStatusTypeId = cancelledReceiptStatusTypeId;
                newReceiptStatus.ReceiptId = receiptId;
                newReceiptStatus.Remarks = txtRemarks.Text;

                int paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                if (check != null)
                {
                    var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
                    checkStatus.IsActive = false;

                    ChequeStatu newCheckStatus = new ChequeStatu();

                    newCheckStatus.CheckId = check.Id;
                    newCheckStatus.ChequeStatusType = ChequeStatusType.CancelledType;
                    newCheckStatus.TransitionDateTime = now;
                    newCheckStatus.Remarks = txtRemarks.Text;
                    newCheckStatus.IsActive = true;

                    ObjectContext.ChequeStatus.AddObject(newCheckStatus);
                }
                ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
            }

            ObjectContext.SaveChanges();
        }

        protected void btnApplyAsPayment_Click(object sender, DirectEventArgs e)
        {

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

        protected void cmbFilterCategories_TextChanged(object sender, DirectEventArgs e)
        {
            string category = cmbFilterCategories.Text;
            currencyModelForFilterList = new List<CurrencyModelForFilter>();
            using (var context = new FinancialEntities())
            {
                switch (category)
                {
                    case "All":
                        strFilterItems.RemoveAll();
                        cmbFilterItems.Clear();
                        PageGridPanelStore.DataBind();
                        break;

                    case "Status":
                        var receipttypes = context.ReceiptStatusTypes;
                        receipttypes.ToList();
                        strFilterItems.DataSource = receipttypes;
                        strFilterItems.DataBind();
                        break;

                    case "Payment Method":
                        var paymentmethodtypes = context.PaymentMethodTypes.Where(entity => entity.Id != PaymentMethodType.ATMType.Id);
                        paymentmethodtypes.ToList();
                        strFilterItems.DataSource = paymentmethodtypes;
                        strFilterItems.DataBind();
                        break;
                    case "Currency":
                        foreach (var currency in ObjectContext.Currencies)
                        {
                            var currencyModelForFilter = new CurrencyModelForFilter(currency);
                            currencyModelForFilterList.Add(currencyModelForFilter);
                        }
                        strFilterItems.DataSource = currencyModelForFilterList;
                        strFilterItems.DataBind();
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnSearch_DirectClick(object sender, DirectEventArgs e)
        {
            this.PageGridPanelPagingToolBar.PageIndex = 0;
            this.PageGridPanelStore.DataBind();
        }

        protected void btnClose_DirectClick(object sender, DirectEventArgs e)
        {
            var now = DateTime.Now;
            var rsm = this.PageGridPanelSelectionModel.SelectedRows;
            int selectedRecordId = int.Parse(rsm[0].RecordID);
            var receipt = ObjectContext.Receipts.SingleOrDefault(entity => entity.Id == selectedRecordId);
            var receiptStatus = ObjectContext.ReceiptStatus.SingleOrDefault(entity => entity.IsActive == true && entity.ReceiptId == selectedRecordId);

            receiptStatus.IsActive = false;

            ReceiptStatu newReceiptStatus = new ReceiptStatu();
            newReceiptStatus.ReceiptId = selectedRecordId;
            newReceiptStatus.ReceiptStatusTypeId = ReceiptStatusType.ClosedReceiptStatusType.Id;
            newReceiptStatus.TransitionDateTime = now;
            newReceiptStatus.IsActive = true;

            ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
            ObjectContext.SaveChanges();
        }

        /*******************************************************************************************************/
        private class DataSource : IPageAbleDataSource<ReceiptViewModel>
        {
            public string InputSearchString { get; set; }
            public string FilterItemBy { get; set; }
            public string FilterCategoryBy { get; set; }
            public string SearchBy { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

            public DataSource()
            {
                this.InputSearchString = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery();
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IEnumerable<ReceiptViewModel> CreateQuery()
            {
                #region Query
                //All Receipts
                var queryAll = from receipt in ObjectContext.Receipts
                               join rpassoc in ObjectContext.ReceiptPaymentAssocs on receipt.Id equals rpassoc.ReceiptId
                               join payment in ObjectContext.Payments on rpassoc.PaymentId equals payment.Id
                               join receipstatus in ObjectContext.ReceiptStatus on receipt.Id equals receipstatus.ReceiptId
                               where receipstatus.IsActive == true &&
                                   payment.PaymentTypeId == PaymentType.Receipt.Id
                               select new { receipt, payment, receipstatus };

                //Foreign Receipts
                var queryForeign = from receipt in ObjectContext.Receipts
                                   join rpassoc in ObjectContext.ReceiptPaymentAssocs on receipt.Id equals rpassoc.ReceiptId
                                   join payment in ObjectContext.Payments on rpassoc.PaymentId equals payment.Id
                                   join pcassoc in ObjectContext.PaymentCurrencyAssocs on payment.Id equals pcassoc.PaymentId
                                   join receipstatus in ObjectContext.ReceiptStatus on receipt.Id equals receipstatus.ReceiptId
                                   where receipstatus.IsActive == true &&
                                       payment.PaymentTypeId == PaymentType.Receipt.Id
                                   select new { receipt, payment, receipstatus };

                //Peso Receipts
                var queryPeso = queryAll.Except(queryForeign);

                var pesoReceipts = from i in queryPeso
                                   select new ReceiptViewModel()
                                   {
                                        Receipt = i.receipt,
                                        ReceiptID = i.receipt.Id,
                                        CurrencyId = 0,
                                        Payment = i.payment,
                                        _DateReceived = i.payment.TransactionDate,
                                        Amount = i.payment.TotalAmount,
                                        Status = i.receipstatus.ReceiptStatusType.Name,
                                        ReceiptBalance = i.receipt.ReceiptBalance.Value
                                    };

                var foreignReceipts = from i in queryForeign
                                      join pcassoc in ObjectContext.PaymentCurrencyAssocs on i.payment.Id equals pcassoc.PaymentId
                                      select new ReceiptViewModel()
                                      {
                                         Receipt = i.receipt,
                                         ReceiptID = i.receipt.Id,
                                         CurrencyId = pcassoc.CurrencyId,
                                         Payment = i.payment,
                                         _DateReceived = i.payment.TransactionDate,
                                         Amount = i.payment.TotalAmount,
                                         Status = i.receipstatus.ReceiptStatusType.Name,
                                         ReceiptBalance = i.receipt.ReceiptBalance.Value
                                      };

                IEnumerable<ReceiptViewModel> query = pesoReceipts.Concat(foreignReceipts);
                #endregion

                switch (SearchBy)
                {
                    case "Received From":
                        query = query.Where(entity => entity.ReceivedFrom.Contains(InputSearchString));
                        break;

                    case "Received By":
                        query = query.Where(entity => entity.ReceivedBy.Contains(InputSearchString));
                        break;

                    default:
                        break;
                }

                switch (FilterCategoryBy)
                {
                    case "Payment Method":
                        query = query.Where(entity => entity.PaymentMethod.Contains(FilterItemBy));
                        break;

                    case "Status":
                        query = query.Where(entity => entity.Status.Contains(FilterItemBy));
                        break;
                    case "Currency":
                        query = query.Where(entity => entity.Currency.Contains(FilterItemBy));
                        break;
                    default:
                        break;
                }

                if (!FromDate.Equals(DateTime.MinValue)&& !ToDate.Equals(DateTime.MinValue))
                {
                    if (ToDate.Equals(DateTime.MinValue))
                        ToDate = DateTime.MaxValue;
                    query = query.Where(entity => entity._DateReceived >= FromDate && entity._DateReceived <= ToDate);
                }

                return query;
            }

            public override List<ReceiptViewModel> SelectAll(int start, int limit, Func<ReceiptViewModel, string> orderBy)
            {
                List<ReceiptViewModel> newList = null;
                return newList;
            }

            public List<ReceiptViewModel> Select(int start, int limit)
            {
                List<ReceiptViewModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery();
                    collection = query.OrderBy(entity => entity.ReceiptID).Skip(start).Take(limit).ToList();
                }
                return collection;
            }
        }

        public class ReceiptViewModel
        {
            public Receipt Receipt { get; set; }
            public Payment Payment { get; set; }
            public int ReceiptID { get; set; }
            public DateTime _DateReceived { get; set; }
            public string DateReceived
            {
                get
                {
                    return _DateReceived.ToString("yyyy-MM-dd");
                }
            }
            public int CurrencyId { get; set; }
            public string Currency
            {
                get
                {
                    if (this.CurrencyId == 0)
                    {
                        return "PHP";
                    }
                    else
                    {
                        var currency = BusinessLogic.Currency.GetCurrencyById(this.CurrencyId);
                        return currency.Symbol;
                    }
                }
            }
            public string ReceivedFrom
            {
                get
                {
                    var partyRole = PartyRole.GetById(this.Payment.ProcessedToPartyRoleId.Value);
                    return partyRole.Party.Name;
                }
            }
            public decimal Amount { get; set; }
            public string PaymentMethod
            {
                get
                {
                    return this.Payment.PaymentMethodType.Name;
                }
            }
            public string Status { get; set; }
            public string ReceivedBy
            {
                get
                {
                    var partyRole = PartyRole.GetById(this.Payment.ProcessedByPartyRoleId);
                    return partyRole.Party.Name;
                }
            }
            public decimal ReceiptBalance { get; set; }

            public string ReceiptType
            {
                get
                {
                    if (Receipt.SalaryReceipt != null)
                        return "Salary";
                    else if (Payment.Cheques.Count() != 0){
                        var cheque = Payment.Cheques.FirstOrDefault();

                        if (cheque.ChequeLoanAssocs.Count() != 0)
                            return "Collateral";
                        else return "Walk-In";
                    }
                    else return "Walk-In";
                }
            }
        }
    }

    public class CurrencyModelForFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CurrencyModelForFilter(Currency currency)
        {
            this.Id = currency.Id;
            this.Name = currency.Symbol;
        }
    }
}