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
    public partial class TransactionReport : ActivityPageBase
    {
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
        public const string LoanRelated = "LoanRelated";
        public const string NonLoanRelated = "NonLoanRelated";
        
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
            DateTime startDate = dtStartDate.SelectedDate;
            DateTime endDate = dtEndDate.SelectedDate;
            if (startDate.Date == endDate.Date)
            {
                lblStartDate.Text = startDate.Date.ToString("MMMM dd, yyyy");
                lblEndDate.Text = string.Empty;
            }
            else
            {
                lblStartDate.Text = startDate.Date.ToString("MMMM dd, yyyy") + " -";
                lblEndDate.Text = endDate.Date.ToString("MMMM dd, yyyy");
            }

            var queryList = CreateQuery(startDate, endDate);
            
            TransactionReportStore.DataSource = queryList.OrderBy(entity => entity.Date);
            TransactionReportStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                DateTime today = DateTime.Today.Date;
                dtEndDate.MaxDate = today;
                dtStartDate.MaxDate = today;
                dtEndDate.SelectedDate = today;
                cmbTransactionClassification.Text = "All";
                RetreiveHeaderDetails();
            }
        }

        [DirectMethod]
        public bool CheckDateRange()
        {
            if (dtStartDate.SelectedValue == null)
            {
                return true;
            }

            return false;
        }

        protected void onStartDateSelect(object sender, EventArgs e)
        {
            dtEndDate.MinDate = dtStartDate.SelectedDate.Date;
        }

        protected void onEndDateSelect(object sender, EventArgs e)
        {
            dtStartDate.MaxDate = dtEndDate.SelectedDate.Date;
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

        private List<TransactionReportModel> CreateQuery(DateTime startDate, DateTime endDate)
        {
            var receipts = from r in ObjectContext.Receipts
                           join rp in ObjectContext.ReceiptPaymentAssocs on r.Id equals rp.ReceiptId
                           join p in ObjectContext.Payments on rp.PaymentId equals p.Id
                           where p.PaymentTypeId == PaymentType.Receipt.Id
                                 && p.TransactionDate >=startDate.Date && p.TransactionDate <=endDate.Date
                           select new TransactionReportModel
                           {
                               ActualLoanId = -1,
                               TransactionSubType = p.SpecificPaymentType.Name,
                               Payment = p,
                               Amount = p.TotalAmount,
                               Disbursement = null
                           };

            var feePayments = from p in ObjectContext.Payments
                              join f in ObjectContext.FeePayments on p.Id equals f.PaymentId
                              where p.PaymentTypeId == PaymentType.FeePayment.Id
                              where p.TransactionDate >=startDate.Date && p.TransactionDate <=endDate.Date
                              select new TransactionReportModel
                              {
                                  ActualLoanId = -1,
                                  TransactionSubType = f.Particular,
                                  Payment = p,
                                  Amount = p.TotalAmount,
                                  Disbursement = null
                              };

            var receiptPayments = from pymnt in ObjectContext.Payments
                                  join fat in ObjectContext.FinAcctTrans on pymnt.Id equals fat.PaymentId
                                  join la in ObjectContext.LoanAccounts on fat.FinancialAccountId equals la.FinancialAccountId
                                  join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                  where pymnt.TransactionDate >= startDate.Date && pymnt.TransactionDate <= endDate.Date
                                  && pymnt.PaymentTypeId == PaymentType.LoanPayment.Id && las.IsActive == true && las.StatusTypeId != LoanAccountStatusType.RestructuredType.Id
                                  select new TransactionReportModel
                                  {
                                      ActualLoanId = fat.FinancialAccountId,
                                      TransactionSubType = pymnt.SpecificPaymentType.Name,
                                      Payment = pymnt,
                                      Amount = fat.Amount,
                                      Disbursement = null
                                  };

            var disbursementPayments = from pymnt in ObjectContext.Payments
                                       join dis in ObjectContext.Disbursements on pymnt.Id equals dis.PaymentId
                                       where pymnt.TransactionDate >= startDate.Date && pymnt.TransactionDate <= endDate.Date
                                       && pymnt.PaymentTypeId == PaymentType.Disbursement.Id && pymnt.ParentPaymentId == null
                                       select new TransactionReportModel
                                  {
                                      ActualLoanId = -1,
                                      TransactionSubType = dis.DisbursementType.Name,
                                      Payment = pymnt,
                                      Amount = pymnt.TotalAmount,
                                      Disbursement = dis
                                  };

            List<TransactionReportModel> transactions = new List<TransactionReportModel>();

            switch (cmbTransactionClassification.Value.ToString())
            {
                case LoanRelated:
                    receipts = receipts.Where(entity => entity.Payment.SpecificPaymentTypeId == SpecificPaymentType.LoanPaymentType.Id);
                    receiptPayments = receiptPayments.Where(entity => entity.Payment.PaymentTypeId == PaymentType.LoanPayment.Id);
                    feePayments = feePayments.Where(entity => entity.Payment.PaymentTypeId == PaymentType.LoanPayment.Id);
                    disbursementPayments = disbursementPayments.Where(entity => entity.Payment.Disbursement.DisbursementTypeId == DisbursementType.LoanDisbursementType.Id);
                    TransactionReportGridPanel.ColumnModel.SetHidden(1, false);
                    lblTrasanctionLabel.Text = "Loan Related Transaction Report for";
                    break;

                case NonLoanRelated:
                    receipts = receipts.Where(entity => entity.Payment.SpecificPaymentTypeId != SpecificPaymentType.LoanPaymentType.Id);
                    receiptPayments = receiptPayments.Where(entity => entity.Payment.SpecificPaymentTypeId != SpecificPaymentType.LoanPaymentType.Id);
                    disbursementPayments = disbursementPayments.Where(entity => entity.Payment.Disbursement.DisbursementTypeId != DisbursementType.LoanDisbursementType.Id);
                    TransactionReportGridPanel.ColumnModel.SetHidden(1, true);
                    lblTrasanctionLabel.Text = "Non-loan Related Transaction Report for";
                    break;

                default:
                    lblTrasanctionLabel.Text = "Transaction Report for";
                    break;
            }

            return disbursementPayments.Concat(receipts).Concat(receiptPayments).Concat(feePayments).OrderBy(x => x.Payment.TransactionDate).ThenBy(x => x.Payment.Id).ToList();
        }

        private class TransactionReportModel
        {
            private Payment payment;
            private Disbursement disbursement;
            public string CurrencySymbol { 
                get {
                    if (this.Payment != null)
                        return Currency.CurrencySymbolByPaymentId(Payment.Id);
                    else return "PHP";
                } 
            }
            public string Name { get; set; }
            public string LoanId
            {
                get
                {
                    return this.ActualLoanId < 1 ? "" : this.ActualLoanId.ToString();
                }
            }
            public int ActualLoanId { get; set; }
            public DateTime Date { get; set; }
            public string _Date { get; set; }
            public string TransactionType { get; set; }
            public string TransactionSubType { get; set; }
            public decimal Amount { get; set; }
            public Payment Payment
            {
                get
                {
                    return payment;
                }
                set
                {
                    payment = value;
                    if (payment != null)
                    { 
                        var party = PartyRole.GetById(this.Payment.ProcessedToPartyRoleId.Value).Party;
                        this.Name = Person.GetPersonFullName(party);
                        this.Date = payment.TransactionDate;
                        this._Date = payment.TransactionDate.ToString("MMM dd, yyyy");
                        this.TransactionType = payment.PaymentType.Name;
                    }
                }
            }
            public Disbursement Disbursement 
            {
                get
                {
                    return disbursement;
                }
                set
                {
                    disbursement = value;
                    if (disbursement != null)
                    {
                        if (payment.PaymentApplications.Count > 0)
                        {
                            var paymentAppplication = payment.PaymentApplications.FirstOrDefault();
                            var agreement = paymentAppplication.LoanDisbursementVcr.Agreement;
                            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.AgreementId == agreement.Id);
                            this.ActualLoanId = financialAccount.Id;
                        }
                    }
                }
            }

            public TransactionReportModel()
            {
                // TODO: Complete member initialization
            }
        }

        public class DateRange {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

        }
    }
}