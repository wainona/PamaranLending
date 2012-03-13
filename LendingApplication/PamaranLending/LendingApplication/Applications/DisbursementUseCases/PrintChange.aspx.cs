using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintChange : ActivityPageBase
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
           
            var details = from d in ObjectContext.Disbursements
                          join p in ObjectContext.Payments on d.PaymentId equals p.Id
                            where d.PaymentId == id
                            select p;
            if (details.Count() != 0)
            {
                var disbursement = details.FirstOrDefault();
                FillForm(disbursement);
            }

            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
            lblLenderNameHeader1.Text = organization.OrganizationName;
            lblLenderNameHeader2.Text = organization.OrganizationName;


            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);

            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);

            lblPrimTelNumber1.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber1.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber1.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress1.Text = PrintFacade.GetEmailAddress(party);

            lblPrimTelNumber2.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber2.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber2.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress2.Text = PrintFacade.GetEmailAddress(party);
        }

        private void FillForm(Payment disbursement)
        {
            var currencySymbol = Currency.CurrencySymbolByPaymentId(disbursement.Id);
            var currencyDescription = Currency.CurrencyDescriptionByPaymentId(disbursement.Id);

            lblReceiptId.Text = ": " + disbursement.ParentPaymentId.ToString();
            lblReceiptId1.Text = ": " + disbursement.ParentPaymentId.ToString();
            lblReceiptId2.Text = ": " + disbursement.ParentPaymentId.ToString();

            var payment = ObjectContext.Payments.FirstOrDefault(entity => entity.Id == disbursement.ParentPaymentId);
            lblReceiptAmount.Text = ": " + payment.TotalAmount.ToString("N");
            lblReceiptAmount1.Text = ": " + payment.TotalAmount.ToString("N");
            lblReceiptAmount2.Text = ": " + payment.TotalAmount.ToString("N");

            lblDateDisbursed.Text = String.Format("{0:MMMM dd, yyyy}", disbursement.TransactionDate);
            lblDateDisbursed1.Text = String.Format("{0:MMMM dd, yyyy}", disbursement.TransactionDate);
            lblDateDisbursed2.Text = String.Format("{0:MMMM dd, yyyy}", disbursement.TransactionDate);

            lblAmountInWords.Text = ": " + ConvertNumbers.EnglishFromNumber((double)disbursement.TotalAmount) + currencyDescription;
            lblAmountInWords1.Text = ": " + ConvertNumbers.EnglishFromNumber((double)disbursement.TotalAmount) + currencyDescription;
            lblAmountInWords2.Text = ": " + ConvertNumbers.EnglishFromNumber((double)disbursement.TotalAmount) + currencyDescription;
            
            lblDisbursedTo.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId);
            lblDisbursedTo1.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId);
            lblDisbursedTo2.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId);

            lblAmountDisbursed.Text = "( " + disbursement.TotalAmount.ToString("N") + " " + currencySymbol+ " )";
            lblAmountDisbursed1.Text = "( " + disbursement.TotalAmount.ToString("N") + " " + currencySymbol + " )";
            lblAmountDisbursed2.Text = "( " + disbursement.TotalAmount.ToString("N") + " " + currencySymbol + " )";
            
            lblDisbursedBy.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedByPartyRoleId);
            lblDisbursedBy1.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedByPartyRoleId);
            lblDisbursedBy2.Text = ": " + Person.GetPersonFullName((int)disbursement.ProcessedByPartyRoleId);
            
            lblReceivedby.Text = Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId).ToUpper();
            lblReceivedby1.Text = Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId).ToUpper();
            lblReceivedby2.Text = Person.GetPersonFullName((int)disbursement.ProcessedToPartyRoleId).ToUpper();

            var formDetail = FormDetail.GetByPaymentId(payment.Id);

            if (formDetail != null)
                imgSignature.ImageUrl = formDetail.Signature;

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

            lblStreetAddress1.Text = postalAddress.StreetAddress;
            lblBarangay1.Text = postalAddress.Barangay;
            lblCity1.Text = postalAddress.City;
            lblMunicipality1.Text = postalAddress.Municipality;
            lblProvince1.Text = postalAddress.Province;
            lblCountry1.Text = postalAddress.Country.Name;
            lblPostalCode1.Text = postalAddress.PostalCode;

            lblStreetAddress2.Text = postalAddress.StreetAddress;
            lblBarangay2.Text = postalAddress.Barangay;
            lblCity2.Text = postalAddress.City;
            lblMunicipality2.Text = postalAddress.Municipality;
            lblProvince2.Text = postalAddress.Province;
            lblCountry2.Text = postalAddress.Country.Name;
            lblPostalCode2.Text = postalAddress.PostalCode;
        }
    }
}