using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using LendingApplication.Applications.ReceiptUseCases;

namespace LendingApplication.Applications.ChequeUseCases
{
    public partial class ListCheques : ActivityPageBase
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
            DataSource dataSource = new DataSource();
            dataSource.InputSearchString = txtSearchInput.Text;
            dataSource.FilterCategoryBy = cmbFilterCategories.SelectedItem.Value;
            dataSource.FilterItemBy = cmbFilterItems.SelectedItem.Text;
            dataSource.SearchBy = cmbSearchBy.SelectedItem.Value;
            //Initial Date Check
            if (dtFrom.SelectedValue != null)
                dataSource.FromDate = dtFrom.SelectedDate;
            if (dtTo.SelectedValue != null)
                dataSource.ToDate = dtTo.SelectedDate;

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, (entity => entity.ReceiptID.ToString()));
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                dtTo.SelectedDate = DateTime.Today.AddDays(2);
            }
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

        protected void cmbFilterCategories_TextChanged(object sender, DirectEventArgs e)
        {
            string category = cmbFilterCategories.Text;
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
                        var chequeStatuTypes = context.ChequeStatusTypes;
                        chequeStatuTypes.ToList();
                        strFilterItems.DataSource = chequeStatuTypes;
                        strFilterItems.DataBind();
                        break;

                    case "Bank":
                        var banks = from bnk in context.BankViewLists
                                    where bnk.Status == "Active"
                                    select new BankList()
                                    {
                                        Id = bnk.PartyRoleID,
                                        Name = bnk.Organization_Name
                                    };
                        banks.ToList();
                        strFilterItems.DataSource = banks;
                        strFilterItems.DataBind();
                        break;
                    case "Currency":
                        List<CurrencyModelForFilter> currencyModelForFilterList = new List<CurrencyModelForFilter>();
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

        protected void btnDeposit_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);

                checkStatus.IsActive = false;

                CreateCheckStatus(check, now, ChequeStatusType.DepositedType);

                //var cancelledReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
                //if (EndCurrentReceiptStatus(paymentId, cancelledReceiptStatusTypeId))
                //{
                //    ReceiptStatu newReceiptStatus = new ReceiptStatu();

                //    newReceiptStatus.TransitionDateTime = now;
                //    newReceiptStatus.IsActive = true;
                //    newReceiptStatus.ReceiptStatusTypeId = cancelledReceiptStatusTypeId;
                //    newReceiptStatus.PaymentId = paymentId;

                //    ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
                //}
            }

            ObjectContext.SaveChanges();
        }

        protected void btnClear_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;

            //ClearCheck();
            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
                checkStatus.IsActive = false;
                CreateCheckStatus(check, now, ChequeStatusType.ClearedType);
            }

            ObjectContext.SaveChanges();
        }

        protected void btnBounced_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;

            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                int paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);

                checkStatus.IsActive = false;

                CreateCheckStatus(check, now, ChequeStatusType.BouncedType);

                var cancelledReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
                if (EndCurrentReceiptStatus(receiptId, cancelledReceiptStatusTypeId))
                {
                    ReceiptStatu newReceiptStatus = new ReceiptStatu();

                    newReceiptStatus.TransitionDateTime = now;
                    newReceiptStatus.IsActive = true;
                    newReceiptStatus.ReceiptStatusTypeId = cancelledReceiptStatusTypeId;
                    newReceiptStatus.ReceiptId = receiptId;

                    ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
                }
            }
            ObjectContext.SaveChanges();
        }

        protected void btnOnHold_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;

            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);

                checkStatus.IsActive = false;

                CreateCheckStatus(check, now, ChequeStatusType.OnHoldType);
            }

            ObjectContext.SaveChanges();
        }

        protected void btnCancel_Click(object sender, DirectEventArgs e)
        {
            wndCheckRemarks.Show();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;

            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);

                checkStatus.IsActive = false;

                CreateCheckStatus(check, now, ChequeStatusType.CancelledType);

                if (EndCurrentReceiptStatus(receiptId, ReceiptStatusType.CancelledReceiptStatusType.Id))
                {
                    ReceiptStatu newReceiptStatus = new ReceiptStatu();

                    newReceiptStatus.TransitionDateTime = now;
                    newReceiptStatus.IsActive = true;
                    newReceiptStatus.ReceiptStatusTypeId = ReceiptStatusType.CancelledReceiptStatusType.Id;
                    newReceiptStatus.ReceiptId = receiptId;
                    newReceiptStatus.Remarks = txtCheckRemarks.Text;

                    ObjectContext.ReceiptStatus.AddObject(newReceiptStatus);
                }
            }

            ObjectContext.SaveChanges();
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

        protected void ApplyCheckAsPayment(object sender, DirectEventArgs e)
        {
            DateTime now = DateTime.Now;
            var firstSelectedCheque = this.PageGridPanelSelectionModel.SelectedRows.First();
            var receipt = Receipt.GetById(int.Parse(firstSelectedCheque.RecordID));
            var interestPayment = decimal.Parse(wntxtInterestPayment.Text);
            var loanPayment = decimal.Parse(wntxtLoanPayment.Text);
            var loanId = int.Parse(hdnLoanId.Text);
            var additionalInterest = decimal.Parse(wntxtAddIntererst.Text);
            var transactDate = wnDtTransactionDate.SelectedDate;
            var query = from f in ObjectContext.FinancialAccounts
                        join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                        join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                        where f.Id == loanId && a.EndDate == null && ai.IsActive == true
                        select new
                        {
                            loanaccount = f.LoanAccount,
                            agreementitem = ai
                        };

            var loanAccount = query.FirstOrDefault().loanaccount;
            var agreementItem = query.FirstOrDefault().agreementitem;

            if (additionalInterest > 0)
            {
                using (var scope1 = new UnitOfWorkScope(true))
                {
                    var interest = GenerateBillFacade.GenerateAndSaveInterest(loanAccount, agreementItem, GenerateBillFacade.ManualBillingSave, dtGenerationDate.SelectedDate, dtGenerationDate.SelectedDate, now);
                }
            }

            Cheque.ApplyPostdatedCheckAsPayment(now, transactDate, receipt.Id, interestPayment, loanPayment, (interestPayment + loanPayment));
            if (receipt.ReceiptBalance == 0)
                btnApplyAsPayment.Disabled = true;
        }

        public string CheckIfPostedCheck(int id)
        {
            //var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;
            int receiptId = id;
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var cheque = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            if (cheque != null)
            {
                var chequeLoanAssoc = ObjectContext.ChequeLoanAssocs.SingleOrDefault(entity => entity.ChequeId == cheque.Id);
                if (chequeLoanAssoc != null)
                {
                    return "1";
                }
            }
            return "0";
        }

        public void ClearNormalCheck()
        {
            DateTime now = DateTime.Now;
            var selectedRows = this.PageGridPanelSelectionModel.SelectedRows;

            //ClearCheck();
            foreach (var item in selectedRows)
            {
                int receiptId = int.Parse(item.RecordID);
                var paymentId = Payment.GetReceiptPayment(receiptId).Id;
                var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
                checkStatus.IsActive = false;
                CreateCheckStatus(check, now, ChequeStatusType.ClearedType);
            }

            ObjectContext.SaveChanges();
        }

        [DirectMethod]
        public int ReceiptHasBalance(int ReceiptId)
        {
            int flag = 0;
            var balance = ObjectContext.Receipts.FirstOrDefault(e => e.Id == ReceiptId).ReceiptBalance;
            if (balance > 0)
                flag = 1;
            return flag;
        }

        [DirectMethod]
        public int FillApplyAsPaymentWindow()
        {
            wnDtTransactionDate.SelectedDate = DateTime.Today;
            var firstSelectedCheque = this.PageGridPanelSelectionModel.SelectedRows.First();
            int receiptId = int.Parse(firstSelectedCheque.RecordID);
            int paymentId = Payment.GetReceiptPayment(receiptId).Id;
            var check = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
            var checkStatus = check.ChequeStatus.SingleOrDefault(entity => entity.IsActive == true && entity.CheckId == check.Id);
            var checkLoanAssoc = ObjectContext.ChequeLoanAssocs.SingleOrDefault(entity => entity.ChequeId == check.Id);
            var loanAccount = LoanAccount.GetById(checkLoanAssoc.FinancialAccountId);
            var receipt = Receipt.GetById(receiptId);
            var tempReceiptBalance = receipt.ReceiptBalance;
            var loanBalance = loanAccount.LoanBalance;
            decimal existingReceivable = 0;
            dtGenerationDate.MinDate = (DateTime)loanAccount.LoanReleaseDate;


            var AllReceivables = from r in ObjectContext.Receivables
                                 join rs in ObjectContext.ReceivableStatus on r.Id equals rs.ReceivableId
                                 where rs.IsActive == true && r.FinancialAccountId == loanAccount.FinancialAccountId
                                 orderby r.ValidityPeriod descending
                                 select new { r, rs };





            wntxtChkAmount.Text = tempReceiptBalance.Value.ToString();
            wntxtLoanBalance.Text = loanBalance.ToString();

            if (AllReceivables.Count() != 0)
            {
                DateTime mindate = AllReceivables.First().r.ValidityPeriod.AddDays(1);
                dtGenerationDate.MinDate = mindate;
                wntxtExistInterestDate.Text = AllReceivables.First().r.ValidityPeriod.ToString();
                var receivables = from r in AllReceivables
                                  where (r.rs.ReceivableStatusType.Id == ReceivableStatusType.OpenType.Id
                                 || r.rs.ReceivableStatusType.Id == ReceivableStatusType.PartiallyPaidType.Id)
                                  select r.r;
                if (receivables.Count() != 0)
                    existingReceivable = receivables.Sum(e => e.Balance);
            }
            else
                wntxtExistInterestDate.Text = "N/A";
            wntxtExistInterest.Text = existingReceivable.ToString();
            hdnLoanId.Value = loanAccount.FinancialAccountId;

            return 1;
        }

        [DirectMethod]
        public void GenerateAdditionalInterest()
        {
            var selectedDate = dtGenerationDate.SelectedDate;
            var loanId = int.Parse(hdnLoanId.Text);
            var validDueDate = DateAdjustment.AdjustToNextWorkingDayIfInvalid(selectedDate);
            decimal interest = 0;

            //loanbalance,loanreleasedate, agreementid,
            var query = from f in ObjectContext.FinancialAccounts
                        join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                        join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                        where f.Id == loanId && a.EndDate == null && ai.IsActive == true
                        select new
                        {
                            loanaccount = f.LoanAccount,
                            agreementitem = ai
                        };

            if (query.Count() != 0)
            {
                var loanAccount = query.FirstOrDefault().loanaccount;
                var agreementitem = query.FirstOrDefault().agreementitem;
                if (selectedDate > loanAccount.LoanReleaseDate)
                {
                    interest += GenerateBillFacade.GenerateAndSaveInterest(loanAccount, agreementitem, GenerateBillFacade.ManualBillingDisplay, selectedDate, validDueDate, selectedDate);
                    interest += GenerateBillFacade.GenerateInterestForLastMonth(selectedDate, loanAccount, agreementitem, GenerateBillFacade.ManualBillingDisplay);
                }
            }

            wntxtAddIntererst.Text = interest.ToString();
        }

        private class DataSource : IPageAbleDataSource<ChequeViewModel>
        {
            public string InputSearchString { get; set; }
            public string FilterItemBy { get; set; }
            public string FilterCategoryBy { get; set; }
            public string SearchBy { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }

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

            private IEnumerable<ChequeViewModel> CreateQuery()
            {

                var queryAll = from c in ObjectContext.Cheques
                               join p in ObjectContext.Payments on c.PaymentId equals p.Id
                               join cs in ObjectContext.ChequeStatus on c.Id equals cs.CheckId
                               join pmt in ObjectContext.PaymentMethodTypes on p.PaymentMethodTypeId equals pmt.Id
                               join cst in ObjectContext.ChequeStatusTypes on cs.CheckStatusTypeId equals cst.Id
                               where cs.IsActive == true && p.PaymentTypeId == PaymentType.Receipt.Id
                               select new { p, c, cst, pmt, cs };

                var queryForeign = from c in ObjectContext.Cheques
                                   join p in ObjectContext.Payments on c.PaymentId equals p.Id
                                   join pcassoc in ObjectContext.PaymentCurrencyAssocs on p.Id equals pcassoc.PaymentId
                                   join cs in ObjectContext.ChequeStatus on c.Id equals cs.CheckId
                                   join pmt in ObjectContext.PaymentMethodTypes on p.PaymentMethodTypeId equals pmt.Id
                                   join cst in ObjectContext.ChequeStatusTypes on cs.CheckStatusTypeId equals cst.Id
                                   where cs.IsActive == true && p.PaymentTypeId == PaymentType.Receipt.Id
                                   select new { p, c, cst, pmt, cs };

                var queryPeso = queryAll.Except(queryForeign);

                var pesoCheques = from i in queryPeso
                                  select new ChequeViewModel()
                                  {
                                      Payment = i.p,
                                      Cheque = i.c,
                                      ChequeNumber = i.p.PaymentReferenceNumber,
                                      _DateReceived = i.p.TransactionDate,
                                      _Amount = i.p.TotalAmount,
                                      Status = i.cst.Name,
                                      _ChequeDate = i.c.CheckDate,
                                      PaymentMethodType = i.pmt.Name,
                                      Remarks = i.cs.Remarks,
                                      CurrencyId = 0
                                  };

                var foreignCheques = from i in queryForeign
                                     join pcassoc in ObjectContext.PaymentCurrencyAssocs on i.p.Id equals pcassoc.PaymentId
                                     select new ChequeViewModel()
                                     {
                                         Payment = i.p,
                                         Cheque = i.c,
                                         ChequeNumber = i.p.PaymentReferenceNumber,
                                         _DateReceived = i.p.TransactionDate,
                                         _Amount = i.p.TotalAmount,
                                         Status = i.cst.Name,
                                         _ChequeDate = i.c.CheckDate,
                                         PaymentMethodType = i.pmt.Name,
                                         Remarks = i.cs.Remarks,
                                         CurrencyId = pcassoc.CurrencyId
                                     };

                var query = pesoCheques.Concat(foreignCheques);

                IEnumerable<ChequeViewModel> result = query.ToList();

                switch (SearchBy)
                {
                    case "ReceivedFrom":
                        result = result.Where(entity => entity.ReceivedFrom.Contains(InputSearchString));
                        break;

                    case "CheckNumber":
                        result = result.Where(entity => entity.ChequeNumber.Contains(InputSearchString));
                        break;

                    default:
                        break;
                }

                switch (FilterCategoryBy)
                {
                    case "Status":
                        result = result.Where(entity => entity.Status.Contains(FilterItemBy));
                        break;

                    case "Bank":
                        result = result.Where(entity => entity.Bank.Contains(FilterItemBy));
                        break;

                    case "Currency":
                        result = result.Where(entity => entity.Currency.Contains(FilterItemBy));
                        break;

                    default:
                        break;
                }

                if (FromDate.HasValue && ToDate.HasValue)
                {
                    result = result.Where(entity => entity._ChequeDate >= FromDate && entity._ChequeDate <= ToDate);
                }

                return result;
            }

            public override List<ChequeViewModel> SelectAll(int start, int limit, Func<ChequeViewModel, string> orderBy)
            {
                List<ChequeViewModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery();
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }
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
            public DateTime _DateReceived { get; set; }
            public string DateReceived
            {
                get
                {
                    return this._DateReceived.ToString("yyyy-MM-dd");
                }
            }
            public string ReceivedFrom
            {
                get
                {
                    return this.Payment.PartyRole1.Party.Name;
                }
            }
            public decimal _Amount { get; set; }
            public string Status { get; set; }
            public DateTime _ChequeDate { get; set; }
            public string ChequeDate
            {
                get
                {
                    return this._ChequeDate.ToString("yyyy-MM-dd");
                }
            }
            public string PaymentMethodType { get; set; }
            public string Remarks { get; set; }
            public string IsPostDated
            {
                get
                {
                    int receiptId = this.ReceiptID;
                    int paymentId = Payment.GetReceiptPayment(receiptId).Id;
                    var cheque = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == paymentId);
                    if (cheque != null)
                    {
                        var chequeLoanAssoc = ObjectContext.ChequeLoanAssocs.SingleOrDefault(entity => entity.ChequeId == cheque.Id);
                        if (chequeLoanAssoc != null)
                        {
                            return "1";
                        }
                    }
                    return "0";
                }
            }
            public string Amount
            {
                get
                {
                    return this.Currency +" "+this._Amount.ToString("N"); 
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
            public string ReceiptType
            {
                get
                {
                  var receipt = Receipt.GetById(this.ReceiptID);
                  if (this.Cheque.ChequeLoanAssocs.Count() != 0)
                      return "Collateral";
                  else if (receipt.SalaryReceipt != null)
                      return "Salary";
                  else return "Walk-In";
                }
            }
        }

        public class BankList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }


}