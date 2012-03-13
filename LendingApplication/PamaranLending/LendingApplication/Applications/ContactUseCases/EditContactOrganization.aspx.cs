using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.ContactUseCases
{
    public partial class EditContactOrganization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                List<OrganizationType> organizationTypes;
                List<Country> countries;
                using (var context = new FinancialEntities())
                {
                    var query = context.Countries;
                    countries = query.ToList();

                    var query1 = context.OrganizationTypes;
                    organizationTypes = query1.ToList();
                }
                storeCountry.DataSource = countries;
                storeCountry.DataBind();

                storeOrganizationType.DataSource = organizationTypes;
                storeOrganizationType.DataBind();

                if (X.IsAjaxRequest == false && this.IsPostBack == false)
                {
                    string partyRoleId = Request.QueryString["id"];
                    string mode = Request.QueryString["mode"];
                    int id;
                    if (int.TryParse(partyRoleId, out id))
                        RetrieveAndAssignDatabaseRecords(id);

                    if (PartyRole.GetById(int.Parse(partyRoleId)) == null)
                        throw new AccessToDeletedRecordException("The selected contact has already been deleted by another user.");
                }
            }

        }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        public static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }


        private void RetrieveAndAssignDatabaseRecords(int partyRoleId)
        {
                hiddenPartyRoleID.Value = partyRoleId;
                PartyRole role = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
                if (role.Party.Organization != null)
                {

                    var organization = role.Party.Organization;

                    txtName.Text = organization.OrganizationName;
                    if (organization.OrganizationTypeId != null)
                    {
                        cmbOrganizationType.SelectedItem.Value = organization.OrganizationType.Id.ToString();
                    }
                    else
                    {
                        cmbOrganizationType.SelectedItem.Value = null;
                    }

                    if (organization.DateEstablished != null)
                    {
                        dtDateEstablished.SelectedDate = organization.DateEstablished.Value;
                    }
                    else
                    {
                        dtDateEstablished.Value = null;
                    }
                    

                    var addressType = ObjectContext.AddressTypes.First(entity =>
                        entity.Name == AddressTypeEnums.PostalAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
                    var postalAddress = role.Party.Addresses.FirstOrDefault(entity =>
                        entity.EndDate == null && entity.AddressTypeId == addressType.Id);
                    var postalAddressSpecific = postalAddress.PostalAddress;

                    //Fill Address Detail Window
                    cmbCountry.SetValueAndFireSelect(postalAddressSpecific.Country.Id);
                    hiddenCountry.Text = postalAddressSpecific.Country.Name;
                    hiddenStreet.Text = postalAddressSpecific.StreetAddress;
                    txtStreetAddress.Text = postalAddressSpecific.StreetAddress;
                    hiddenBarangay.Text = postalAddressSpecific.Barangay;
                    txtBarangay.Text = postalAddressSpecific.Barangay;
                    if (string.IsNullOrEmpty(postalAddressSpecific.Municipality))
                    {
                        radioCity.Checked = true;
                        hiddenCity.Text = postalAddressSpecific.City;
                        txtCityOrMunicipality.Text = postalAddressSpecific.City;
                    }
                    else
                    {
                        radioMunicipality.Checked = true;
                        hiddenMunicipality.Text = postalAddressSpecific.Municipality;
                        txtCityOrMunicipality.Text = postalAddressSpecific.Municipality;
                    }


                    hiddenProvince.Text = postalAddressSpecific.Province;
                    txtProvince.Text = postalAddressSpecific.Province;
                    hiddenState.Text = postalAddressSpecific.State;
                    txtState.Text = postalAddressSpecific.State;
                    hiddenPostalCode.Text = postalAddressSpecific.PostalCode;
                    txtPostalCode.Text = postalAddressSpecific.PostalCode;

                    string fullAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                  postalAddressSpecific.Barangay,
                                  postalAddressSpecific.Municipality,
                                  postalAddressSpecific.City,
                                  postalAddressSpecific.Province,
                                  postalAddressSpecific.State,
                                  postalAddressSpecific.Country.Name,
                                  postalAddressSpecific.PostalCode);

                    txtBusinessAddress.Text = fullAddress;
                    if (string.IsNullOrWhiteSpace(hiddenMunicipality.Text) == false)
                        radioMunicipality.Checked = true;
                    else
                        radioCity.Checked = true;

                    //Primary Home Phone Number
                    var addressTypTelecom = ObjectContext.AddressTypes.First(entity =>
                        entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
                    var telecomNumberTypeHomePhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity =>
                        entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

                    //Primary Personal Cellphone Number
                    var telecomNumberTypePersonalCellphoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity =>
                        entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessFaxNumber));

                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressTypTelecom);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeHomePhoneNumber);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypePersonalCellphoneNumber);

                    var primaryNumber = role.Party.Addresses.FirstOrDefault(entity =>
                        entity.EndDate == null &&
                        entity.AddressTypeId == addressTypTelecom.Id &&
                        entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeHomePhoneNumber.Id &&
                        entity.TelecommunicationsNumber != null &&
                        entity.TelecommunicationsNumber.IsPrimary);
                    
                    txtTelephoneCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                    var primaryFaxNumber = role.Party.Addresses.FirstOrDefault(entity =>
                        entity.EndDate == null &&
                        entity.AddressTypeId == addressTypTelecom.Id
                   && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypePersonalCellphoneNumber.Id
                   && entity.TelecommunicationsNumber != null
                   && entity.TelecommunicationsNumber.IsPrimary);
                    txtFaxCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                    // ICHECK KO UGMA
                    if (primaryNumber != null)
                    {
                        var primaryNumberSpecific = primaryNumber.TelecommunicationsNumber;
                        txtTelephoneAreaCode.Text = primaryNumberSpecific.AreaCode;
                        txtTelephoneNumber.Text = primaryNumberSpecific.PhoneNumber;
                    }

                    if (primaryFaxNumber != null)
                    {
                        var primaryCellphoneNumberSpecific = primaryFaxNumber.TelecommunicationsNumber;
                        txtFaxAreacode.Text = primaryCellphoneNumberSpecific.AreaCode;
                        txtFaxNumber.Text = primaryCellphoneNumberSpecific.PhoneNumber;
                    }

                    var addressTypeElectonicAddress = ObjectContext.AddressTypes.SingleOrDefault(entity =>
                        entity.Name.Equals(AddressTypeEnums.ElectronicAddress));
                    var emailTypeBusinessEmail = ObjectContext.ElectronicAddressTypes.SingleOrDefault(entity =>
                        entity.Name.Equals(ElectronicAddressTypeEnums.BusinessEmailAddress));
                    InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(emailTypeBusinessEmail);
                    var primaryEmail = role.Party.Addresses.FirstOrDefault(entity =>
                        entity.EndDate == null &&
                        entity.AddressTypeId == addressTypeElectonicAddress.Id &&
                        entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeBusinessEmail.Id &&
                        entity.ElectronicAddress.IsPrimary);

                    if (primaryEmail != null)
                    {
                        var primaryEmailSpecific = primaryEmail.ElectronicAddress;
                        //Email Address
                        txtEmailAddress.Text = primaryEmailSpecific.ElectronicAddressString;
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
            newPostalAddress.StreetAddress = hiddenStreet.Text;
            newPostalAddress.IsPrimary = true;

            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, postalAddressType, today, true);
        }

        private void CreateOrUpdateContactInfo(Party party, Country country, DateTime today)
        {
            TelecommunicationsNumber faxNumber = new TelecommunicationsNumber();
            faxNumber.AreaCode = this.txtFaxAreacode.Text;
            faxNumber.PhoneNumber = this.txtFaxNumber.Text;
            faxNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessFaxNumberType;
            faxNumber.IsPrimary = true;
            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, faxNumber,
                TelecommunicationsNumberType.BusinessFaxNumberType, today, true);

            TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
            telephoneNumber.AreaCode = this.txtTelephoneAreaCode.Text;
            telephoneNumber.PhoneNumber = this.txtTelephoneNumber.Text;
            telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.BusinessPhoneNumberType;
            telephoneNumber.IsPrimary = true;
            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, telephoneNumber,
                TelecommunicationsNumberType.BusinessPhoneNumberType, today, true);

            ElectronicAddress emailAddress = new ElectronicAddress();
            emailAddress.ElectronicAddressString = txtEmailAddress.Text;
            emailAddress.IsPrimary = true;

            ElectronicAddress.CreateOrUpdateElectronicAddress(party, emailAddress,
                ElectronicAddressType.BusinessEmailAddressType, entity => entity.ElectronicAddress.IsPrimary, today);
        }

        public bool IsEqual(TelecommunicationsNumber number, string areaCode, string phoneNumber)
        {
            bool isEqual = true;

            isEqual = isEqual && number.AreaCode == areaCode;
            isEqual = isEqual && number.PhoneNumber == phoneNumber;

            return isEqual;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
                SaveBasicPersonContactInformation();
                ObjectContext.SaveChanges();
        }

        private void SaveBasicPersonContactInformation()
        {
            var today = DateTime.Now;

            int partyRoleId = int.Parse(hiddenPartyRoleID.Value.ToString());
            PartyRole role = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
            if (role.Party.Organization != null)
            {

                OrganizationType internalOrganization = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Name == OrganizationTypeEnums.Internal);
                OrganizationType externalOrganization = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Name == OrganizationTypeEnums.External);

                role.Party.Organization.OrganizationName = txtName.Text;
                role.Party.Organization.DateEstablished = dtDateEstablished.SelectedDate;
                if (cmbOrganizationType.Value != null)
                {
                    int organizationTypeId = int.Parse(cmbOrganizationType.Value.ToString());
                    var organizationType = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Id == organizationTypeId);
                    role.Party.Organization.OrganizationType = organizationType;
                }

                if (dtDateEstablished.SelectedValue != null)
                {
                    role.Party.Organization.DateEstablished = dtDateEstablished.SelectedDate;
                }
                else
                {
                    role.Party.Organization.DateEstablished = null;
                }
                
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == countryId);
                CreateOrUpdatePostalAddress(role.Party, country, today);
                CreateOrUpdateContactInfo(role.Party, country, today);
            }
        }

        protected void btnCancelAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            winAddressDetail.Hidden = true;
        }
        protected void btnDoneAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            hiddenStreet.Text = txtStreetAddress.Text;
            hiddenBarangay.Text = txtBarangay.Text;
            if (radioCity.Checked == true)
            {
                hiddenMunicipality.Text = "";
                hiddenCity.Text = txtCityOrMunicipality.Text;
            }
            else
            {
                hiddenCity.Text = "";
                hiddenMunicipality.Text = txtCityOrMunicipality.Text;
            }

            hiddenProvince.Text = txtProvince.Text;
            hiddenCountry.Text = cmbCountry.SelectedItem.Text;
            hiddenPostalCode.Text = txtPostalCode.Text;
            txtBusinessAddress.Text = StringConcatUtility.Build(", ", hiddenStreet.Text,
                hiddenBarangay.Text, hiddenMunicipality.Text, hiddenCity.Text, hiddenProvince.Text,
                hiddenCountry.Text, hiddenPostalCode.Text);
            winAddressDetail.Hidden = true;

            txtName.Text = txtName.Text;

            using (var context = new FinancialEntities())
            {
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = context.Countries.SingleOrDefault(entity => entity.Id == countryId);
                txtFaxCode.Text = country.CountryTelephoneCode;
                txtTelephoneCode.Text = country.CountryTelephoneCode;
            }
        }

        public void btnOpen_Click(object sender, DirectEventArgs e)
        {
            btnBrowseAddress.Disabled = true;
            btnOpen.Hidden = true;
            btnEdit.Hidden = false;
            Panel1.Disabled = true;
            Panel2.Disabled = true;
            //Panel3.Disabled = true;
        }

        protected void btnEdit_Click(object sender, DirectEventArgs e)
        {
            btnBrowseAddress.Disabled = false;
            btnEdit.Hidden = true;
            btnOpen.Hidden = false;
            Panel1.Disabled = false;
            Panel2.Disabled = false;
            //Panel3.Disabled = false;
        }
    }
}