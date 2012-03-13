using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;


namespace LendingApplication.Applications.BankUseCases
{
    public partial class EditBank : ActivityPageBase
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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
            List<Country> countries;
            using (var context = new FinancialEntities())
            {
                var query = context.Countries;
                countries = query.ToList();
            }
            storeCountry.DataSource = countries;
            storeCountry.DataBind();

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                //if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
                //{
                //    btnEdit.Hidden = true;
                //    btnSave.Hidden = true;
                //    btnEditSeparator.Hidden = true;
                //    btnEditFill.Hidden = true;
                //}
                int id = int.Parse(Request.QueryString["id"]);
                using (var context = new FinancialEntities())
                {
                    var partyRole = context.PartyRoles.FirstOrDefault(entity => entity.Id == id && entity.EndDate ==null);
                    if (partyRole == null)
                        throw new AccessToDeletedRecordException("The bank has been deleted by another user.");
                    else FillBank(id);
                }
           
            }
        }

        private void FillBank(int id)
        {
            hiddenPartyRoleId.Text = id.ToString();
            using (var context = new FinancialEntities())
            {
                var partyRole = context.PartyRoles.FirstOrDefault(entity => entity.Id == id && entity.EndDate == null);
                var bank = context.Banks.FirstOrDefault(entity => entity.PartyRoleId == id);
                if (bank != null)
                {
                    txtBranch.Text = bank.Branch;
                    txtAcronym.Text = bank.Acronym;
                }
                var org = context.Organizations.FirstOrDefault(entity => entity.PartyId == partyRole.PartyId);
                if (org != null)
                {
                    txtName.Text = org.OrganizationName;
                    var bankAdress = context.Addresses.Where(entity => entity.PartyId == org.PartyId && entity.EndDate == null);
                    var addressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);

                    var postalAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BusinessAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(postalAddressType);
                    var postalAddress = bankAdress.FirstOrDefault(entity => entity.AddressTypeId.Value == addressType.Id
                        && entity.PostalAddress.PostalAddressTypeId == postalAddressType.Id && entity.PostalAddress.IsPrimary == true);

                    if (postalAddress != null)
                    {
                        var postalAddressSpecific = postalAddress.PostalAddress;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.StreetAddress) == false)
                            hiddenStreetName.Text = postalAddressSpecific.StreetAddress;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.Barangay) == false)
                            hiddenBarangay.Text = postalAddressSpecific.Barangay;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.Municipality) == false)
                            hiddenMunicipality.Text = postalAddressSpecific.Municipality;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.City) == false)
                            hiddenCity.Text = postalAddressSpecific.City;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.Province) == false)
                            hiddenProvince.Text = postalAddressSpecific.Province;
                        hiddenCountry.Text = postalAddressSpecific.Country.Name;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.Country.CountryTelephoneCode) == false)
                            hiddenCountryCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
                        if (string.IsNullOrWhiteSpace(postalAddressSpecific.PostalCode) == false)
                            hiddenPostalCode.Text = postalAddressSpecific.PostalCode;
                        txtAddress.Text = StringConcatUtility.Build(", ", hiddenStreetName.Text,
                            hiddenBarangay.Text, hiddenMunicipality.Text, hiddenProvince.Text,
                            hiddenCountry.Text, hiddenPostalCode.Text);
                        
                        txtCountryCode.Text = hiddenCountryCode.Text;
                        txtFaxCode.Text = hiddenCountryCode.Text;
                        cmbCountry.SetValueAndFireSelect(postalAddressSpecific.Country.Id);
                        FillAddress();
                    }

                    //business telephone number
                    var telAddressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.TelecommunicationNumber);
                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(telAddressType);
                    var businessPhoneNumber = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.BusinessPhoneNumber);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(businessPhoneNumber);

                    var phoneNumber = bankAdress.FirstOrDefault(entity => entity.AddressTypeId.Value == telAddressType.Id
                    && entity.TelecommunicationsNumber.TypeId == businessPhoneNumber.Id && entity.TelecommunicationsNumber.IsPrimary == true);
                    if (phoneNumber != null)
                    {
                        var phoneNumberSpecific = phoneNumber.TelecommunicationsNumber;
                        txtAreaCode.Text = phoneNumberSpecific.AreaCode;
                        txtPhoneNum.Text = phoneNumberSpecific.PhoneNumber;
                        txtCountryCode.Text = hiddenCountryCode.Text;
                    }

                    // fax number might not be primary
                    var faxType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.BusinessFaxNumber);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(faxType);
                    var faxNumber = bankAdress.FirstOrDefault(entity => entity.AddressTypeId.Value == telAddressType.Id
                    && entity.TelecommunicationsNumber.TypeId == faxType.Id && entity.TelecommunicationsNumber.IsPrimary == true);
                    if (faxNumber != null)
                    {
                        var faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                        txtFaxAreaCode.Text = faxNumberSpecific.AreaCode;
                        txtFaxNum.Text = faxNumberSpecific.PhoneNumber;
                        txtFaxCode.Text = hiddenCountryCode.Text;
                    }
                    else
                    {
                        faxNumber = bankAdress.FirstOrDefault(entity => entity.AddressTypeId.Value == telAddressType.Id
                         && entity.TelecommunicationsNumber.TypeId == faxType.Id);
                        if (faxNumber != null)
                        {
                            var faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                            txtFaxAreaCode.Text = faxNumberSpecific.AreaCode;
                            txtFaxNum.Text = faxNumberSpecific.PhoneNumber;
                            txtFaxCode.Text = hiddenCountryCode.Text;
                        }
                    }

                    //email address
                    var emailAddType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.ElectronicAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(emailAddType);
                    var emailType = context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == ElectronicAddressTypeEnums.BusinessEmailAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(emailType);
                    var emailAdd = bankAdress.FirstOrDefault(entity => entity.AddressTypeId.Value == emailAddType.Id
                   && entity.ElectronicAddress.ElectronicAddressTypeId == emailType.Id && entity.ElectronicAddress.IsPrimary == true);
                    if (emailAdd != null)
                    {
                        var emailAddSpecific = emailAdd.ElectronicAddress;
                        txtEmailAdd.Text = emailAddSpecific.ElectronicAddressString;
                    }
                }
            }
        }

        private void CreateOrUpdatePostalAddress(Party party, Country country, DateTime today)
        {
            AddressType addressType = AddressType.PostalAddressType;
            PostalAddressType postalAddressType = PostalAddressType.BusinessAddressType;

            //Create an Address with AddressType as Postal Address
            PostalAddress newPostalAddress = new PostalAddress();
            newPostalAddress.Country = country;
            newPostalAddress.Barangay = hiddenBarangay.Text;
            newPostalAddress.PostalCode = hiddenPostalCode.Text;
            newPostalAddress.Province = hiddenProvince.Text;
            newPostalAddress.City = hiddenCity.Text;
            newPostalAddress.Municipality = hiddenMunicipality.Text;
            newPostalAddress.StreetAddress = hiddenStreetName.Text;
            newPostalAddress.IsPrimary = true;

            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, postalAddressType, today, true);
        }

        private void CreateOrUpdateTelecommuncationNumber(FinancialEntities context, Party party, Country country, DateTime today)
        {
            var addressType = AddressType.TelecommunicationNumberType;
            var businessNumberAddressType = TelecommunicationsNumberType.BusinessPhoneNumberType;
            var faxNumberAddressType = TelecommunicationsNumberType.BusinessFaxNumberType;

                TelecommunicationsNumber newTelNum = new TelecommunicationsNumber();
                newTelNum.IsPrimary = true;
                newTelNum.AreaCode = txtAreaCode.Text;
                newTelNum.PhoneNumber = txtPhoneNum.Text;
                newTelNum.TelecommunicationsNumberType = businessNumberAddressType;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newTelNum, businessNumberAddressType, today, true);


                TelecommunicationsNumber newFaxNum = new TelecommunicationsNumber();
                newFaxNum.IsPrimary = true;
                newFaxNum.AreaCode = txtFaxAreaCode.Text;
                newFaxNum.PhoneNumber = txtFaxNum.Text;
                newFaxNum.TelecommunicationsNumberType = faxNumberAddressType;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newFaxNum, faxNumberAddressType, today, true);

        }

        private void CreateOrUpdateElectronicAddress( Party party, DateTime today)
        {
                bool isEqual = false;
                var addressType = AddressType.ElectronicAddressType;
                var electronicAddressType = ElectronicAddressType.BusinessEmailAddressType;
            
                var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id && entity.ElectronicAddress.IsPrimary);
                if (address != null)
                {
                    isEqual = true;
                    var electronicAddress = address.ElectronicAddress;
                    isEqual = isEqual &&electronicAddress.ElectronicAddressString == txtEmailAdd.Text;

                    if (isEqual == false)
                        address.EndDate = today;
                }

                if (isEqual == false)
                {
                    if (string.IsNullOrWhiteSpace(txtEmailAdd.Text) == false)
                    {
                        Address newAddressAsEmail = AddressBusinessUtility.AddAddress(party, addressType, today);
                        ElectronicAddress newEmailAddress = AddressBusinessUtility.AddEmail(newAddressAsEmail, electronicAddressType, true);
                        newEmailAddress.ElectronicAddressString = txtEmailAdd.Text;
                    }
                }
            
        }

        protected void btnBrowseAddress_Click(object sender, DirectEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(hiddenStreetName.Text)==false)txtStreet.Text = hiddenStreetName.Text;
            txtBarangay1.Text = hiddenBarangay.Text;
            if (string.IsNullOrWhiteSpace(hiddenCity.Text) == false)
            {
                txtCityOrMunicipality.Text = hiddenCity.Text;
                radioCity.Checked = true;
            }
            else if (string.IsNullOrWhiteSpace(hiddenMunicipality.Text) == false)
            {
                txtCityOrMunicipality.Text = hiddenMunicipality.Text;
                radioMunicipality.Checked = true;
            }
            txtProvince1.Text = hiddenProvince.Text;
            cmbCountry.SelectedItem.Text = hiddenCountry.Text;
            txtPostalCode.Text = hiddenPostalCode.Text;
            txtAddress.Text = StringConcatUtility.Build(", ", hiddenStreetName.Text,
            hiddenBarangay.Text, hiddenMunicipality.Text,hiddenCity.Text, hiddenProvince.Text,
            hiddenCountry.Text, hiddenPostalCode.Text);
        }
        public void FillAddress()
        {
            if (string.IsNullOrWhiteSpace(hiddenStreetName.Text) == false) txtStreet.Text = hiddenStreetName.Text;
            txtBarangay1.Text = hiddenBarangay.Text;
            if (string.IsNullOrWhiteSpace(hiddenCity.Text) == false)
            {
                txtCityOrMunicipality.Text = hiddenCity.Text;
                radioCity.Checked = true;
            }
            else if (string.IsNullOrWhiteSpace(hiddenMunicipality.Text) == false)
            {
                txtCityOrMunicipality.Text = hiddenMunicipality.Text;
                radioMunicipality.Checked = true;
            }
            txtProvince1.Text = hiddenProvince.Text;
            cmbCountry.SelectedItem.Text = hiddenCountry.Text;
            txtPostalCode.Text = hiddenPostalCode.Text;
            txtAddress.Text = StringConcatUtility.Build(", ", hiddenStreetName.Text,
            hiddenBarangay.Text, hiddenMunicipality.Text, hiddenCity.Text, hiddenProvince.Text,
            hiddenCountry.Text, hiddenPostalCode.Text);
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var today = DateTime.Now;
            int partyRoleId = Convert.ToInt32(hiddenPartyRoleId.Text);
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var bank = ObjectContext.Banks.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
                if (bank != null)
                {
                    bank.Branch = txtBranch.Text;
                    if (string.IsNullOrWhiteSpace(txtAcronym.Text)) bank.Acronym = bank.PartyRole.Party.Organization.OrganizationName;
                    else bank.Acronym = txtAcronym.Text;
                }
                var org = ObjectContext.Organizations.FirstOrDefault(entity => entity.PartyId == bank.PartyRole.Party.Id);
                if (org != null) org.OrganizationName = txtName.Text;
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = ObjectContext.Countries.FirstOrDefault(entity => entity.Id == countryId);
                var party = ObjectContext.Parties.FirstOrDefault(entity => entity.Id == org.PartyId);
                CreateOrUpdatePostalAddress(party, country, today);
                CreateOrUpdateTelecommuncationNumber(ObjectContext, party, country, today);
                CreateOrUpdateElectronicAddress(party, today);
            }
        }

        protected void btnDoneAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStreet.Text) == false) hiddenStreetName.Text = txtStreet.Text;
            else hiddenStreetName.Text = null;
            hiddenBarangay.Text = txtBarangay1.Text;
            if (radioCity.Checked)
            {
                hiddenCity.Text = txtCityOrMunicipality.Text;
                hiddenMunicipality.Text = null;
            }
            else if(radioMunicipality.Checked)
            {
                hiddenMunicipality.Text = txtCityOrMunicipality.Text;
                hiddenCity.Text = null;
            }
            hiddenProvince.Text = txtProvince1.Text;
            hiddenCountry.Text = cmbCountry.SelectedItem.Text;
            hiddenPostalCode.Text = txtPostalCode.Text;
            txtAddress.Text = StringConcatUtility.Build(", ", hiddenStreetName.Text,
            hiddenBarangay.Text, hiddenMunicipality.Text, hiddenProvince.Text,
            hiddenCountry.Text, hiddenPostalCode.Text);
            winAddressDetail.Hidden = true;


            using (var context = new FinancialEntities())
            {
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = context.Countries.SingleOrDefault(entity => entity.Id == countryId);
                txtCountryCode.Text = country.CountryTelephoneCode;
                txtFaxCode.Text = country.CountryTelephoneCode;
             }

        }

        protected void btnCancelAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            winAddressDetail.Hidden = true;
        }
    }
}