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
    public partial class DailyChequesReleasedReport : ActivityPageBase
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
                var zero = 0;
                DateTime today = DateTime.Now.Date;
                lblCurrentDate.Text = today.ToString("MMMM dd, yyyy");
                lblTotalAmount.Text = "Total Amount: " + zero.ToString();
                lblTotalNumberOfCheques.Text = "Total Number of Checks Received: "+zero.ToString();
                lblTitle.Text = "Check(s) Received for ";
                strCurrency.DataSource = Currency.GetCurrencies().ToList();
                strCurrency.DataBind();
                cmbCurrency.SelectedIndex = 0;
                cmbReportType.SelectedIndex = 0;
                RetreiveHeaderDetails();
            }
         
        
       
        }

        [DirectMethod]
        public void GenerateReport()
        {
            var philCurrency = Currency.GetCurrencyBySymbol("PHP");
            var currencyId = int.Parse(cmbCurrency.SelectedItem.Value);
            var selectedType = cmbReportType.SelectedItem.Value;
            string reportType = "Released";
            PaymentType paymenttype = PaymentType.Disbursement;
            if (selectedType == "Received")
            {
                paymenttype = PaymentType.Receipt;
                reportType = "Received";
            }


            if (currencyId != philCurrency.Id)
            {
                var queryList = Cheque.GetForeignChequesByCurrency(currencyId, paymenttype, DateTime.Today);
                FillReport(queryList, currencyId, reportType);
            }
            else
            {
                var queryList = Cheque.GetPhilippineCheques(paymenttype, DateTime.Today);
                FillReport(queryList, currencyId, reportType);
            }
           
        }
        private void FillReport(IEnumerable<ChequesTransactionModel> queryList, int currencyId, string ReportType)
        {
            var currency = Currency.GetCurrencyById(currencyId);
            ChequesReportStore.DataSource = queryList;
            ChequesReportStore.DataBind();
            lblTotalAmount.Text = "Total Amount:" + queryList.Sum(e => e.Amount).ToString("N") + " " + currency.Symbol;
            lblTotalNumberOfCheques.Text = "Total Number of Checks "+ReportType+": " + queryList.Count().ToString();
            lblTitle.Text = currency.Description + " Check(s) "+ReportType+" for ";
        }

        private void RetreiveHeaderDetails()
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            var lenderName = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == lenderPartyRole.Party.Id).OrganizationName;
            lblLenderName.Text = lenderName;

            var lenderAddress = PostalAddress.GetCurrentPostalAddress(lenderPartyRole.Party, PostalAddressType.BusinessAddressType, true);
            lblLenderAddress.Text = StringConcatUtility.Build(", ", lenderAddress.StreetAddress, lenderAddress.Barangay,
                                                                    lenderAddress.City, lenderAddress.Municipality);

            lblLenderTelephoneNumber.Text = "Tel. No. " + PrintFacade.GetPrimaryPhoneNumber(lenderPartyRole.Party, lenderAddress);
            lblLenderFaxNumber.Text = "Fax " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }


    
    }
}