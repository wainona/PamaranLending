using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.Reports
{
    public partial class DailyReceivedAndReleasedReport : ActivityPageBase
    {
        public const string LoanRelated = "LoanRelated";

        public const string NonLoanRelated = "NonLoanRelated";

        /**
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                return allowed;
            }
        }
        **/

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
            //DateTime today = DateTime.Today.Date;
            //var queryList = CreateQuery(today, today);

            //TransactionReportStore.DataSource = queryList;
            //TransactionReportStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                DateTime today = DateTime.Today.Date;
                strCurrency.DataSource = Currency.GetCurrencies().ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = 0;
                lblDailyTrasanctionLabel.Text = "Received and Released Report for " + today.ToString("MMMM dd, yyyy");
                RetreiveHeaderDetails();

                //var queryList = CreateQuery(today, today);
                //queryList.OrderBy(entity => entity.EntryDate.Date);

                //TransactionReportStore.DataSource = queryList;
                //TransactionReportStore.DataBind();

                //var cashOnVault = from c in ObjectContext.CashOnVaults
                //                  where c.TransitionDateTime < today
                //                  orderby c.TransitionDateTime descending
                //                  select c.Amount;
                //if (cashOnVault.Count() != 0)
                //{
                //    var vaultamount = cashOnVault.FirstOrDefault();
                //    txtCashOnVault.Text = "Yesterday's Cash on Vault: " + vaultamount.ToString("N");
                //    hdnCashOnVault.Text = vaultamount.ToString();
                //}
            }
        }

        private void RetreiveHeaderDetails()
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            var lenderName = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == lenderPartyRole.Party.Id).OrganizationName;
            lblLenderName.Text = lenderName;

            var lenderAddress = PostalAddress.GetCurrentPostalAddress(lenderPartyRole.Party, PostalAddressType.BusinessAddressType, true);
            lblLenderAddress.Text = StringConcatUtility.Build(", ", lenderAddress.StreetAddress, lenderAddress.Barangay,
                                                                    lenderAddress.City, lenderAddress.Municipality);

            lblLenderCountryAndPostal.Text = StringConcatUtility.Build(", ", lenderAddress.Province, lenderAddress.Country.Name, lenderAddress.PostalCode);

            lblLenderTelephoneNumber.Text = "Tel. No. " + PrintFacade.GetPrimaryPhoneNumber(lenderPartyRole.Party, lenderAddress);
            lblLenderFaxNumber.Text = "Fax " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }

        [DirectMethod]
        public void GenerateReport()
        {
            DateTime today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var phCurrencyId = Currency.GetCurrencyBySymbol("PHP").Id;
          

            var currency = Currency.GetCurrencyById(currencyId);
            lblDailyTrasanctionLabel.Text = currency.Description + " Received and Released Report for " + today.ToString("MMMM dd, yyyy");

            var cashOnVault = from c in ObjectContext.CashOnVaults
                              where c.TransitionDateTime < today
                              && c.CurrencyId == currencyId
                              orderby c.TransitionDateTime descending
                              select c;
            if (cashOnVault.Count() != 0)
            {
                var vaultamount = cashOnVault.FirstOrDefault().Amount; 
                var vaultDate = cashOnVault.FirstOrDefault().TransitionDateTime;
             //   txtCashOnVault.Text = "Yesterday's Cash on Vault: " + vaultamount.ToString("N");
                hdnCashOnVault.Text = vaultamount.ToString();
                today = cashOnVault.FirstOrDefault().TransitionDateTime;
                txtCashOnVault.Text = "Yesterday's Vault Closed On:" + today;
            }
            else
            {
                //txtCashOnVault.Text = "Yesterday's Cash on Vault: 0.00";
                txtCashOnVault.Text = "Yesterday's Vault Closed On:" + "N/A";
            }

            var canclose = CanCloseVault();
            if(canclose == true && currencyId != phCurrencyId){
                btnSaveToCashOnVault.Disabled = false;
            }else {
                btnSaveToCashOnVault.Disabled = true;
            }
            //If already closed, must display only transactions that are lesser than today:D
            if (canclose == false)
            {
                var alreadyClosedVaultDate = ObjectContext.CashOnVaults.FirstOrDefault(entity => entity.TransitionDateTime >= DateTime.Today
                    && entity.TransitionDateTime < tomorrow && entity.CurrencyId == currencyId);
                if (alreadyClosedVaultDate != null)
                    tomorrow = alreadyClosedVaultDate.TransitionDateTime;
            }

            var queryList = CreateQuery(today, tomorrow, currencyId);
            TransactionReportStore.DataSource = queryList;
            TransactionReportStore.DataBind();
           
            
        }

        //public int checkIfVaultIsClosed() 
        //{
        //    var cashOnVault = ObjectContext.CashOnVaults.SingleOrDefault(entity => entity.TransitionDateTime == DateTime.Today && entity.CurrencyId == int.Parse(cmbCurrency.SelectedItem.Value));
        //    if (cashOnVault != null)
        //    {
        //        return 1;
        //    }
        //    return 0;
        //}

        [DirectMethod]
        public bool CanCloseVault()
        {
            // true when can close, false when already closed
            var flag = false;
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var tomorrow = DateTime.Today.AddDays(1);
            var cashOnVault = ObjectContext.CashOnVaults.SingleOrDefault(entity => entity.TransitionDateTime >= DateTime.Today && entity.TransitionDateTime < tomorrow && entity.CurrencyId == currencyId);
            if (cashOnVault == null)
            {
              
                flag = true;
            }
          
            return flag;
        }

     
        protected void btnSaveToCashOnVault_DirectClick(object sender, DirectEventArgs e)
        {
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var phCurrencyId = Currency.GetCurrencyBySymbol("PHP").Id;
            var currentCashOnVault = ObjectContext.CashOnVaults.SingleOrDefault(entity => entity.IsActive == true && entity.CurrencyId == currencyId);
            if(currentCashOnVault != null)
                currentCashOnVault.IsActive = false;

            CashOnVault newCashOnVault = new CashOnVault();
            if (currencyId == phCurrencyId)
                newCashOnVault.Amount = decimal.Parse(txtTotalCashSummary.Text);
            else
                newCashOnVault.Amount = decimal.Parse(txtTotalCash.Text);
            newCashOnVault.TransitionDateTime = DateTime.Now;
            newCashOnVault.IsActive = true;
            newCashOnVault.CurrencyId = currencyId;
            newCashOnVault.ClosedByPartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            //newCashOnVault.ProcessedByPartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;

            ObjectContext.CashOnVaults.AddObject(newCashOnVault);
            ObjectContext.SaveChanges();

            btnSaveToCashOnVault.Disabled = true;
        }

        private List<TransactionReportModel> CreateQuery(DateTime startDate, DateTime endDate, int currencyId)
        {
            
            IEnumerable<Payment> Receipts;
            IEnumerable<Payment> Disbursements;
            var today = DateTime.Today;
            //var tommorrow = DateTime.Today.AddDays(1);
            //var yesterday = DateTime.Today.AddDays(-1);

            //closed Cash on vaults
            //var closedCashOnVaultToday = ObjectContext.CashOnVaults.SingleOrDefault(entity => entity.TransitionDateTime >= today && entity.TransitionDateTime <= tommorrow && entity.CurrencyId == currencyId);
            //var closedCashOnVaultYesterday = ObjectContext.CashOnVaults.SingleOrDefault(entity => entity.TransitionDateTime >= yesterday && entity.TransitionDateTime <= today && entity.CurrencyId == currencyId);

            //if (closedCashOnVaultYesterday != null)
            //{
            //    startDate = closedCashOnVaultYesterday.TransitionDateTime;
            //}

            //if (closedCashOnVaultToday != null)
            //{
            //    endDate = closedCashOnVaultToday.TransitionDateTime;
            //}

            #region Receipts Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allPayments = from pymnt in ObjectContext.Payments
                                  join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                  join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                  where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                  && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                  && pymnt.ParentPaymentId == null
                                  select pymnt;

                var foreignPayments = from pymnt in ObjectContext.Payments
                                      join rpa in ObjectContext.ReceiptPaymentAssocs on pymnt.Id equals rpa.PaymentId
                                      join rc in ObjectContext.Receipts on rpa.ReceiptId equals rc.Id
                                      join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                      where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.PaymentTypeId == PaymentType.Receipt.Id
                                       && pymnt.ParentPaymentId == null
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
                            && pymnt.ParentPaymentId == null
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
                                            select pymnt;
            Receipts = Receipts.Except(cancelledpostdatedcheques);

            var receiptPayments =   from pymnt in Receipts
                                    where pymnt.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                  select new TransactionReportModel()
                                  {
                                      Payment = pymnt,
                                      ForExDetail = null,
                                      COVTransaction = null,
                                      _PaymentsCash = pymnt.TotalAmount,
                                      _PaymentsCheck = 0,
                                      _ReleasedCash = 0,
                                      _ReleasedCheck = 0,
                                      Remarks= string.Empty
                                  };

            var receiptPaymentsCheck = from pymnt in Receipts
                                      where (pymnt.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id || pymnt.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id)
                                      select new TransactionReportModel()
                                      {
                                          Payment = pymnt,
                                          ForExDetail = null,
                                          COVTransaction = null,
                                          _PaymentsCash = 0,
                                          _PaymentsCheck = pymnt.TotalAmount,
                                          _ReleasedCash = 0,
                                          _ReleasedCheck = 0,
                                          Remarks = string.Empty
                                      };
            #endregion
            #region Disbursements Region
            if (currencyId == Currency.GetCurrencyBySymbol("PHP").Id)
            {
                var allDisbursements = from pymnt in ObjectContext.Payments
                                       join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                       where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                       && pymnt.ParentPaymentId != null
                                       && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                       select pymnt;

                var foreignDisbursements = from pymnt in ObjectContext.Payments
                                           join pca in ObjectContext.PaymentCurrencyAssocs on pymnt.Id equals pca.PaymentId
                                           join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                           where pymnt.EntryDate >= startDate && pymnt.EntryDate <= endDate
                                           && pymnt.ParentPaymentId != null
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
                                && pymnt.ParentPaymentId != null
                                && pca.CurrencyId == currencyId
                                && pymnt.PaymentTypeId == PaymentType.Disbursement.Id
                                select pymnt;
            }

            var disbursementPayments = from pymnt in Disbursements
                                       where pymnt.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                       select new TransactionReportModel()
                                       {
                                           Payment = pymnt,
                                           ForExDetail = null,
                                           COVTransaction = null,
                                           _PaymentsCash = 0,
                                           _PaymentsCheck = 0,
                                           _ReleasedCash = pymnt.TotalAmount,
                                           _ReleasedCheck = 0,
                                           Remarks= string.Empty
                                       };

            var disbursementPaymentsCheck = from pymnt in Disbursements
                                            where (pymnt.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id || pymnt.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id)
                                            select new TransactionReportModel()
                                            {
                                                Payment = pymnt,
                                                ForExDetail = null,
                                                COVTransaction = null,
                                                _PaymentsCash = 0,
                                                _PaymentsCheck = 0,
                                                _ReleasedCash = 0,
                                                _ReleasedCheck = pymnt.TotalAmount,
                                                Remarks = string.Empty
                                            };
            #endregion
            #region Cash On Vault
            // cash on vault transactions
            var covDeposited = from covt in ObjectContext.COVTransactions
                               where covt.CurrencyId == currencyId
                               && covt.EntryDate >= startDate && covt.EntryDate <= endDate
                               && covt.COVTransTypeId == COVTransactionType.DepositToVaultType.Id
                               select new TransactionReportModel()
                               {
                                   Payment = null,
                                   ForExDetail = null,
                                   COVTransaction = covt,
                                   _PaymentsCash = covt.Amount,
                                   _PaymentsCheck = 0,
                                   _ReleasedCash = 0,
                                   _ReleasedCheck = 0,
                                   Remarks = covt.Remarks
                               };

            var covWithdrawn = from covt in ObjectContext.COVTransactions
                               where covt.CurrencyId == currencyId
                               && covt.EntryDate >= startDate && covt.EntryDate <= endDate 
                               && covt.COVTransTypeId == COVTransactionType.WithdrawFromVaultType.Id
                               select new TransactionReportModel()
                               {
                                   Payment = null,
                                   ForExDetail = null,
                                   COVTransaction = covt,
                                   _PaymentsCash = 0,
                                   _PaymentsCheck = 0,
                                   _ReleasedCash = covt.Amount,
                                   _ReleasedCheck = 0,
                                   Remarks = covt.Remarks
                               };
            #endregion
            #region Foreign Exchange Transactions
            //foreign exchange
            var forExCashReceived = from fed in ObjectContext.ForExDetails
                                    join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.Id equals fda.ForExDetailId
                                    join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                    where fed.ParentForExDetailId == null && fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                     && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                     && fed.CurrencyId == currencyId
                                    select new TransactionReportModel()
                                    {
                                        Payment = null,
                                        ForExDetail = fed,
                                        COVTransaction = null,
                                        _PaymentsCash = fed.Amount,
                                        _PaymentsCheck = 0,
                                        _ReleasedCash = 0,
                                        _ReleasedCheck = 0,
                                        Remarks = string.Empty
                                    };

            var forExCashReleased = from fed in ObjectContext.ForExDetails
                                    join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.ParentForExDetailId equals fda.ForExDetailId
                                    join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                    where fed.ParentForExDetailId != null && fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                     && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                     && fed.CurrencyId == currencyId
                                    select new TransactionReportModel()
                                    {
                                        Payment = null,
                                        ForExDetail = fed,
                                        COVTransaction = null,
                                        _PaymentsCash = 0,
                                        _PaymentsCheck = 0,
                                        _ReleasedCash = fed.Amount,
                                        _ReleasedCheck = 0,
                                        Remarks = string.Empty
                                    };

            var forExCheckReleased = from fed in ObjectContext.ForExDetails
                                    join fda in ObjectContext.ForeignExchangeDetailAssocs on fed.ParentForExDetailId equals fda.ForExDetailId
                                    join fe in ObjectContext.ForeignExchanges on fda.ForExId equals fe.Id
                                    where fed.ParentForExDetailId != null && fed.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id
                                     && fe.EntryDate >= startDate && fe.EntryDate <= endDate
                                     && fed.CurrencyId == currencyId
                                    select new TransactionReportModel()
                                    {
                                        Payment = null,
                                        ForExDetail = fed,
                                        COVTransaction = null,
                                        _PaymentsCash = 0,
                                        _PaymentsCheck = 0,
                                        _ReleasedCash = 0,
                                        _ReleasedCheck = fed.Amount,
                                        Remarks = string.Empty
                                    };
            #endregion

            List<TransactionReportModel> transactions = new List<TransactionReportModel>();
            decimal vaultamount = 0;
            var cashOnVault = from c in ObjectContext.CashOnVaults
                              where c.TransitionDateTime < today
                              && c.CurrencyId == currencyId
                              orderby c.TransitionDateTime descending
                              select c;
            TransactionReportModel vault = new TransactionReportModel();            
            if (cashOnVault.Count() != 0)
            {
                vault.CashOnVault = cashOnVault.FirstOrDefault().Amount;
                vault.Payment = null;
                vault.ForExDetail = null;
                vault.COVTransaction = null;
                vault._PaymentsCash = 0;
                vault._PaymentsCheck = 0;
                vault._ReleasedCash = 0;
                vault._ReleasedCheck = 0;
                vaultamount = cashOnVault.FirstOrDefault().Amount;
            }
            transactions.Add(vault);
            var receivedAndReleased = receiptPayments.Concat(receiptPaymentsCheck).Concat(disbursementPayments).Concat(disbursementPaymentsCheck).Concat(forExCashReceived).Concat(forExCashReleased).Concat(forExCheckReleased)
                .Concat(covDeposited).Concat(covWithdrawn).ToList();
            transactions.AddRange(receivedAndReleased);
        

            var DRCash = transactions.Sum(entity => entity._PaymentsCash);
            var CRCash = transactions.Sum(entity => entity._ReleasedCash);
            var DRCheck = transactions.Sum(entity => entity._PaymentsCheck);
            var CRCheck = transactions.Sum(entity => entity._ReleasedCheck);
            var TotalCash = vaultamount + (DRCash - CRCash);
            var TotalCheck = DRCheck - CRCheck;

            txtDRCash.Text = DRCash.ToString("N");
            txtCRCash.Text = CRCash.ToString("N");
            txtDRCheck.Text = DRCheck.ToString("N");
            txtCRCheck.Text = CRCheck.ToString("N");

            txtTotalCash.Text = TotalCash.ToString("N");
            txtTotalCheck.Text = TotalCheck.ToString("N");

            return transactions;
        }

        private class TransactionReportModel
        {
            public Payment Payment { get; set; }
            public ForExDetail ForExDetail { get; set; }
            public COVTransaction COVTransaction { get; set; }

            public string StationNumber 
            {
                get
                {
                    //var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == this.Payment.ProcessedToPartyRoleId && entity.EndDate == null);
                    if (this.Payment != null)
                    {
                        var customer = ObjectContext.Customers.SingleOrDefault(entity => entity.PartyRoleId == this.Payment.ProcessedToPartyRoleId);
                        if (customer != null)
                            return customer.CurrentClassification.ClassificationType.StationNumber;
                        else return string.Empty;
                    }
                    else
                        return string.Empty;
                }
            }
            public string Name 
            {
                get
                {
                    if (this.Payment != null)
                    {
                        if (this.Payment.Id != 0)
                            return this.Payment.PartyRole1.Party.Name;
                        else return "Cash On Vault";
                    }
                    else if (this.ForExDetail != null) 
                    {
                        ForeignExchangeDetailAssoc forExDetailsAssoc;
                        if (this.ForExDetail.ParentForExDetailId == null)
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.Id);
                        else
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.ParentForExDetailId.Value);
                        
                        return forExDetailsAssoc.ForeignExchange.PartyRole1.Party.Name;
                    }
                    else
                        return "Cash On Vault";
                }
            }
            public string PaymentsCash 
            {
                get
                {
                    if (this._PaymentsCash != 0)
                        return this._PaymentsCash.ToString("N");
                    else
                        return string.Empty;
                }
            }
            public string PaymentsCheck 
            {
                get
                {
                    if (this._PaymentsCheck != 0)
                        return this._PaymentsCheck.ToString("N");
                    else
                        return string.Empty;
                }
            }
            public string ReleasedCash 
            {
                get
                {
                    if (this._ReleasedCash != 0)
                        return this._ReleasedCash.ToString("N");
                    else
                        return string.Empty;
                }
            }
            public string ReleasedCheck 
            {
                get
                {
                    if (this._ReleasedCheck != 0)
                        return this._ReleasedCheck.ToString("N");
                    else
                        return string.Empty;
                }
            }
            public string Bank 
            {
                get
                {
                    if (this.Cheque != null && this.Cheque.Id != 0)
                        return this.Cheque.PartyRole.Bank.Acronym;
                    else if (this.ForExDetail != null)
                    {
                        if (this.ForExDetail.ForExCheques.Count() > 0)
                            return this.ForExDetail.ForExCheques.SingleOrDefault().PartyRole.Bank.Acronym;
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                }
            }
            public string CheckNumber {
                get
                {
                    if (this.Cheque != null && this.Cheque.Id != 0)
                        return this.Cheque.Payment.PaymentReferenceNumber;
                    else if (this.ForExDetail != null)
                    {
                        if (this.ForExDetail.ForExCheques.Count() > 0)
                            return this.ForExDetail.ForExCheques.SingleOrDefault().CheckNumber;
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                }
            }
            public string _EntryDate {
                get
                {
                    if (this.EntryDate != null)
                        return this.EntryDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                    else return string.Empty;
                }
            }
            public DateTime? EntryDate 
            {
                get
                {
                    if (this.Payment != null)
                    {
                        return this.Payment.EntryDate;
                    }
                    else if (this.ForExDetail != null)
                    {
                        ForeignExchangeDetailAssoc forExDetailsAssoc;
                        if (this.ForExDetail.ParentForExDetailId == null)
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.Id);
                        else
                            forExDetailsAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(entity => entity.ForExDetailId == this.ForExDetail.ParentForExDetailId.Value);

                        return forExDetailsAssoc.ForeignExchange.EntryDate;
                    } 
                    else if (this.COVTransaction != null)
                    {
                        return this.COVTransaction.EntryDate;
                    }
                    return null;
                }
            }
            public DateTime? TransactionDate 
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
                    else if (this.COVTransaction != null)
                    {
                        return this.COVTransaction.TransactionDate;
                    }


                    return null;
                }
            }
            public string _TransactionDate
            {
                get
                {
                    if (this.TransactionDate != null)
                        return this.TransactionDate.Value.ToString("MM/dd/yyyy");
                    else return string.Empty;
                }
            }
            public string Remarks { get; set; }
            public decimal? CashOnVault { get; set; }
            public decimal _PaymentsCash { get; set; }
            public decimal _PaymentsCheck { get; set; }
            public decimal _ReleasedCash { get; set; }
            public decimal _ReleasedCheck { get; set; }
            
            public Cheque Cheque
            {
                get
                {
                    if (this.Payment != null)
                    {
                        if (this.Payment.Id != 0)
                        {
                            Cheque cheque;
                            //int Id = 0;
                            //if (this.Payment.ParentPaymentId.HasValue)
                            //    Id = this.Payment.ParentPaymentId.Value;
                            //else
                            //    Id = this.Payment.Id;

                            cheque = ObjectContext.Cheques.SingleOrDefault(entity => entity.PaymentId == this.Payment.Id);
                            return cheque;
                        }
                        else return new Cheque();
                    }
                    else return new Cheque();
                }
            }

            public TransactionReportModel()
            {
                CashOnVault = null;
                // TODO: Complete member initialization
            }
        }

        public class DateRange {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}