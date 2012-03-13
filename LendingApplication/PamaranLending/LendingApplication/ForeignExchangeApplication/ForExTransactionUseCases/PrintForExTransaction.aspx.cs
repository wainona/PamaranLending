using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using Ext.Net;
using System.Data.Objects;

namespace LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases
{
    public partial class PrintForExTransaction : System.Web.UI.Page
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
                int forExId = int.Parse(Request.QueryString["forexid"]);
                var foreignExchange = ObjectContext.ForeignExchanges.SingleOrDefault(x => x.Id == forExId);

                FillLenderInformation();
                FillTransactionDetails(foreignExchange);
            }
        }

        public void FillTransactionDetails(ForeignExchange forEx)
        {
            string amountInWords = ConvertNumbers.EnglishFromNumber((double)forEx.AmountReleased);
            lblCustomerName.Text = forEx.PartyRole1.Party.Name;
            lblAmountReceived.Text = forEx.AmountReceived.ToString("N") + " " + forEx.Currency.Symbol;
            lblCurrencyTo.Text = forEx.AmountReleased.ToString("N") + " " + forEx.Currency1.Symbol;
            lblCurrencyToWords.Text = amountInWords + " ( " + forEx.Currency1.Description + " ) ";
            lblExchangeRate.Text = forEx.Rate.ToString("N") + " " + forEx.Currency1.Symbol;
            lblProcessedBy.Text = forEx.PartyRole.Party.Name;
            lblTotalAmount.Text = forEx.AmountReleased.ToString("N") + " " + forEx.Currency1.Symbol;
            lblReceivedby.Text = forEx.PartyRole1.Party.Name.ToUpper();

            var forExAssoc = ObjectContext.ForeignExchangeDetailAssocs.SingleOrDefault(x => x.ForExId == forEx.Id);

            var cash = from fed in ObjectContext.ForExDetails
                       where fed.PaymentMethodTypeId == PaymentMethodType.CashType.Id &&
                        fed.ParentForExDetailId == forExAssoc.ForExDetailId
                        select new ExchangeBreakdown {
                            PaymentMethod = fed.PaymentMethodType.Name,
                            TotalAmount = fed.Amount,
                            CheckNumber = "N/A",
                            ForExDetail = fed
                        };

            var check = from fed in ObjectContext.ForExDetails
                        join fec in ObjectContext.ForExCheques on fed.Id equals fec.ForExDetailId
                        where (fed.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id ||
                        fed.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id) &&
                        fed.ParentForExDetailId == forExAssoc.ForExDetailId
                        select new ExchangeBreakdown
                        {
                            PaymentMethod = fed.PaymentMethodType.Name,
                            TotalAmount = fed.Amount,
                            CheckNumber = fec.CheckNumber,
                            ForExDetail = fed
                        };

            check = check.Concat(cash);
            strBreakdown.DataSource = check.ToList();
            strBreakdown.DataBind();
        }


        public void FillLenderInformation()
        {
            /*HEADER DETAILS*/
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;

            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);

            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        public void FillPostalAddress(PostalAddress postalAddress)
        {
            lblStreetAddress.Text = postalAddress.StreetAddress;
            lblBarangay.Text = postalAddress.Barangay;
            lblCity.Text = postalAddress.City;
            lblMunicipality.Text = postalAddress.Municipality;
            lblProvince.Text = postalAddress.Province;
            lblCountry.Text = postalAddress.Country.Name;
            lblPostalCode.Text = postalAddress.PostalCode;
        }

        public class ExchangeBreakdown
        {
            public string PaymentMethod { get; set; }
            public string BankBranch
            {
                get
                {
                    if (this.ForExDetail.ForExCheques.FirstOrDefault() != null)
                    {
                        var bank = Bank.GetById(this.ForExDetail.ForExCheques.FirstOrDefault().BankPartyRoleId);
                        if (bank != null && (string.IsNullOrEmpty(bank.Branch) == false))
                            return bank.Branch;
                        else return "N/A";
                    }
                    else return "N/A";
                }
            }
            public string BankName
            {
                get
                {
                    if (this.ForExDetail.ForExCheques.FirstOrDefault() != null)
                        return Organization.GetOrganizationName(this.ForExDetail.ForExCheques.FirstOrDefault().BankPartyRoleId);
                    else return "N/A";
                }
            }
            public string CheckNumber { get; set; }
            public decimal TotalAmount { get; set; }
            public ForExDetail ForExDetail { get; set; }

        }
    }
}