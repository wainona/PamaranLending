using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.LoanUseCases
{
    public partial class PrintLoanApplicationRecord : ActivityPageBase
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
                int selectedLoanId = int.Parse(Request.QueryString["id"]);
                hdnLoanId.Text = selectedLoanId.ToString();

                //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderName(party);
                var postalAddress = SetAndGetPostalAddress(party);
                lblPrimTelNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetPrimaryPhoneNumber(party);
                lblSecTelNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetSecondaryPhoneNumber(party);
                lblFaxNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetFaxNumber(party);
                lblEmailAddress.Text = GetEmailAddress(party);

                //Loan Account Owner
                lblAccountOwner.Text = "";
                lblDistrictOwner.Text = "";
                lblStationNumberOwner.Text = "";
                lblAddressOwner.Text = "";
                lblCellPhoneNumberOwner.Text = "";
                lblPrimTelNumberOwner.Text = "";
                lblEmailAddressOwner.Text = "";

                //Payment History
                var paymentHistoryView = ObjectContext.PaymentHistoryViewLists.Where(entity => entity.FinancialAccountId == selectedLoanId);
                grdPaymentHisotry.DataSource = paymentHistoryView.ToList();
                grdPaymentHisotry.DataBind();

                //DisplayOwner(selectedLoanId);
                //DisplayDatePaidOff(selectedLoanId);
            }
        }

        public void FillLenderName(Party party)
        {
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
        }

        public PostalAddress SetAndGetPostalAddress(Party party)
        {
            var postalAddressType = PostalAddressType.BusinessAddressType.Name;
            var addressType = ObjectContext.AddressTypes.First(entity => entity.Name.Equals("Postal Address"));
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id &&
                                entity.PostalAddress.PostalAddressType.Name == postalAddressType && entity.PostalAddress.IsPrimary == true);
            var pa = address.PostalAddress;

            lblStreetAddress.Text = pa.StreetAddress;
            lblBarangay.Text = pa.Barangay;
            lblCity.Text = pa.City;
            lblMunicipality.Text = pa.Municipality;
            lblProvince.Text = pa.Province;
            lblCountry.Text = pa.Country.Name;
            lblPostalCode.Text = pa.PostalCode;

            return pa;

        }

        public string GetPrimaryPhoneNumber(Party party)
        {
            string primPhoneNUmber = string.Empty;
            //Primary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
            var telecomNumberTypeBusinessPhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressTypTelecom);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeBusinessPhoneNumber);


            var primaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary);

            // ICHECK KO UGMA
            if (primaryNumber != null)
            {
                var pN = primaryNumber.TelecommunicationsNumber;
                primPhoneNUmber = "(" + pN.AreaCode + ") " + pN.PhoneNumber;

            }
            return primPhoneNUmber;
        }

        public string GetSecondaryPhoneNumber(Party party)
        {
            string secPhoneNUmber = string.Empty;
            //Secondary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
            var telecomNumberTypeBusinessPhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

            var secondaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary == false);

            if (secondaryNumber != null)
            {
                var sN = secondaryNumber.TelecommunicationsNumber;
                secPhoneNUmber = "(" + sN.AreaCode + ") " + sN.PhoneNumber;
            }
            return secPhoneNUmber;
        }

        public string GetFaxNumber(Party party)
        {
            string faxPhoneNUmber = string.Empty;
            var telecomNumberTypeFax = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessFaxNumber));
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeFax);
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));

            var faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeFax.Id && entity.TelecommunicationsNumber.IsPrimary);

            if (faxNumber != null)
            {
                var fN = faxNumber.TelecommunicationsNumber;
                faxPhoneNUmber = "(" + fN.AreaCode + ") " + fN.PhoneNumber;

            }
            return faxPhoneNUmber;
        }

        public string GetEmailAddress(Party party)
        {
            string emailAddress = string.Empty;

            var addressTypeId = AddressType.ElectronicAddressType.Id;
            var emailTypeId = ElectronicAddressType.BusinessEmailAddressType.Id;
            var primaryEmail = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypeId
                                                 && entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeId
                                                 && entity.ElectronicAddress.IsPrimary);

            if (primaryEmail != null)
            {
                var ea = primaryEmail.ElectronicAddress;
                //Email Address
                emailAddress = ea.ElectronicAddressString;
            }

            return emailAddress;
        }

        public void DisplayOwner(int selectedFinancialAccountID)
        {
            var loanview = ObjectContext.LoanViewLists.SingleOrDefault(entity => entity.LoanId == selectedFinancialAccountID);
            lblAccountOwner.Text = loanview.Name;
        }

        public void DisplayDatePaidOff(int selectedFinancialAccountID)
        {
            var loanstatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccountID
                                   && entity.IsActive == true && entity.StatusTypeId == LoanAccountStatusType.PaidOffType.Id);
            
        }
    }
}