using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class PrintFeePayment : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
                return allowed;
            }
        }
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
            int id = int.Parse(Request.QueryString["id"]);
            FillHeaders();
            FillForm(id);
            FillItems(id);
      
        }
        private void FillItems(int id)
        {
            var items = FeePayment.GetFeesById(id);
            grdOtherDisbursement.DataSource = items;
            grdOtherDisbursement.DataBind();

        }
        private void FillHeaders()
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
        private void FillForm(int id)
        {
            var details = ObjectContext.Payments.FirstOrDefault(e => e.Id == id);
            var currencySymbol = Currency.CurrencySymbolByPaymentId(details.Id);
            var currencyDescription = Currency.CurrencyDescriptionByPaymentId(details.Id);

            lblTotalAmount.Text = details.TotalAmount.ToString("N")+" "+currencySymbol;
            lblDateDisbursed.Text = String.Format("{0:MMMM dd, yyyy}", details.TransactionDate);
            lblDisbursedTo.Text = ": " + Person.GetPersonFullName((int)details.ProcessedToPartyRoleId);

            lblDisbursedBy.Text = ": " + Person.GetPersonFullName(details.ProcessedByPartyRoleId);
            lblReceivedby.Text = Person.GetPersonFullName((int)details.ProcessedToPartyRoleId).ToUpper();
            lblAmountReceived.Text = ": " + ConvertNumbers.EnglishFromNumber((double)details.TotalAmount) + currencyDescription;

            var breakdown = Payment.GetBreakdown(id, PaymentType.FeePayment);
            strBreakdown.DataSource = breakdown.ToList();
            strBreakdown.DataBind();
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
    }
}