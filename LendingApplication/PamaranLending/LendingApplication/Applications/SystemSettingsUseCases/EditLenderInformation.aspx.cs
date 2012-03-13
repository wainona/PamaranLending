using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.FinancialManagement.SystemSettingsUseCases
{
    public partial class EditLenderInformation : ActivityPageBase
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        /// 
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
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
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.IsPostBack == false)
                {
                    cmbOrganizationType.SelectedIndex = 0;
                    cmbCountry.SelectedIndex = 0;
                }

                using (var ctx = new FinancialEntities())
                {
                    var organizationtypes = ctx.OrganizationTypes;
                    strOrganizationType.DataSource = organizationtypes.ToList();
                    strOrganizationType.DataBind();

                    var countries = ctx.Countries;
                    strCountry.DataSource = countries.ToList();
                    strCountry.DataBind();
                }

                Retrieve();
            }
        }

        //When the edit address button is clicked
        protected void btnEditAddress_Click(object sender, DirectEventArgs e)
        {
            cmbCountry.SelectedItem.Value = HiddenCountry.Text;
            txtStreetAddress.Text = HiddenStreetAddress.Text;
            txtBarangay.Text = HiddenBarangay.Text;

            //if city or municipality
            if (radioCity.Checked)
            {
                txtCityOrMunicipality.Text = HiddenCity.Text;
            }
            else 
            {
                txtCityOrMunicipality.Text = HiddenMunicipality.Text;
            }
            
            txtProvince.Text = HiddenProvince.Text;
            txtState.Text = HiddenState.Text;
            txtPostalCode.Text = HiddenPostalCode.Text;
            wndAddressDetail.Hidden = false;
        }

        //When the overall save button is clicked
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            DateTime today = DateTime.Now;
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                Party party = CreateOrRetrieveParty(ObjectContext, today);
                int countryId = int.Parse(cmbCountry.SelectedItem.Value);
                var country = ObjectContext.Countries.FirstOrDefault(entity => entity.Id == countryId);
                CreateOrUpdatePostalAddress(ObjectContext, party, country, today);
                CreateOrUpdateTelecommuncationNumber(ObjectContext, party, country, today);
                CreateOrUpdateElectronicAddress(ObjectContext, party, country, today);
            }
        }

        private Party CreateOrRetrieveParty(FinancialEntities ctx, DateTime today)
        {
            Party party;
            int organizationTypeId = int.Parse(cmbOrganizationType.SelectedItem.Value.ToString());
            if (string.IsNullOrWhiteSpace(HiddenPartyId.Text))
            {
                var partyType = ctx.PartyTypes.First(x => x.Name.Equals("Organization"));

                party = new Party();
                party.PartyTypeId = partyType.Id;


                var roleType = ctx.RoleTypes.SingleOrDefault(entity => entity.Name.Equals("Lending Institution"));
                PartyRole newPartyRole = new PartyRole();
                newPartyRole.Party = party;
                newPartyRole.PartyRoleType = roleType.PartyRoleType;
                newPartyRole.EffectiveDate = today;
                
                Organization organization = new Organization();
                organization.Party = party;//
                organization.OrganizationTypeId = organizationTypeId;
                organization.OrganizationName = txtName.Text;
                organization.DateEstablished = Convert.ToDateTime(dfDateEstablished.Text);
                ctx.Parties.AddObject(party);
            }
            else
            {
                int partyId = int.Parse(HiddenPartyId.Text);
                party = ctx.Parties.SingleOrDefault(entity => entity.Id == partyId);
                Organization organization = party.Organization;
                organization.OrganizationTypeId = organizationTypeId;
                organization.OrganizationName = txtName.Text;
                organization.DateEstablished = Convert.ToDateTime(dfDateEstablished.Text);
            }
            return party;
        }

        private void CreateOrUpdatePostalAddress(FinancialEntities ctx, Party party, Country country, DateTime today)
        {
            AddressType addressType = AddressType.PostalAddressType;
            PostalAddressType postalAddressType = PostalAddressType.BusinessAddressType;

            //Create an Address with AddressType as Postal Address
            PostalAddress newPostalAddress = new PostalAddress();
            newPostalAddress.Country = country;
            newPostalAddress.Barangay = HiddenBarangay.Text;
            newPostalAddress.PostalCode = HiddenPostalCode.Text;
            newPostalAddress.Province = HiddenProvince.Text;
            newPostalAddress.City = HiddenCity.Text;
            newPostalAddress.Municipality = HiddenMunicipality.Text;
            newPostalAddress.StreetAddress = HiddenStreetAddress.Text;
            newPostalAddress.State = HiddenState.Text;
            newPostalAddress.IsPrimary = true;

            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, postalAddressType, today, true);
        }

        private void CreateOrUpdateTelecommuncationNumber(FinancialEntities ctx, Party party, Country country, DateTime today)
        {
            var addressType = ctx.AddressTypes.First(entity => entity.Name.Equals("Telecommunication Number"));
            var businessNumberAddressType = ctx.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals("Business Phone Number"));
            var faxNumberAddressType = ctx.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals("Business Fax Number"));
            

            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(faxNumberAddressType);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(businessNumberAddressType);

            bool primaryNumberIsEqual = false;
            var primaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == businessNumberAddressType.Id && entity.TelecommunicationsNumber != null
                    && entity.TelecommunicationsNumber.IsPrimary);
            if (primaryNumber != null)
            {
                primaryNumberIsEqual = IsEqual(primaryNumber.TelecommunicationsNumber, txtPrimTelAreaCode.Text, txtPrimTelPhoneNumber.Text);

                if (primaryNumberIsEqual == false)
                    primaryNumber.EndDate = today;
            }

            if(primaryNumberIsEqual == false)
            {
                //Create Primary Telecommunication Number
                if (string.IsNullOrWhiteSpace(txtPrimTelPhoneNumber.Text) == false)
                {
                    Address newAddressAsPrimTelecom = AddressBusinessUtility.AddAddress(party, addressType, today);
                    TelecommunicationsNumber newPrimTelecomNumber = AddressBusinessUtility.AddTelNum(newAddressAsPrimTelecom, businessNumberAddressType, true);
                    newPrimTelecomNumber.AreaCode = txtPrimTelAreaCode.Text;
                    newPrimTelecomNumber.PhoneNumber = txtPrimTelPhoneNumber.Text;
                }
            }
            /*******************************************************************************************************************/
            bool secondaryNumberIsEqual = false;
            var secondaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == businessNumberAddressType.Id && entity.TelecommunicationsNumber != null
                    && entity.TelecommunicationsNumber.IsPrimary == false);
            if (secondaryNumber != null)
            {
                secondaryNumberIsEqual = IsEqual(secondaryNumber.TelecommunicationsNumber, txtSecTelAreaCode.Text, txtSecTelPhoneNumber.Text);

                if (secondaryNumberIsEqual == false)
                    secondaryNumber.EndDate = today;
            }

            if (secondaryNumberIsEqual == false)
            {
                //Create Secondary Telecommunication Number
                if (string.IsNullOrWhiteSpace(txtSecTelPhoneNumber.Text) == false)
                {
                    Address newAddressAsSecTelecom = AddressBusinessUtility.AddAddress(party, addressType, today);
                    TelecommunicationsNumber newSecTelecomNumber = AddressBusinessUtility.AddTelNum(newAddressAsSecTelecom, businessNumberAddressType, false);
                    newSecTelecomNumber.AreaCode = txtSecTelAreaCode.Text;
                    newSecTelecomNumber.PhoneNumber = txtSecTelPhoneNumber.Text;
                }
            }
            /*******************************************************************************************************************/
            bool faxNumberIsEqual = false;
            var faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == faxNumberAddressType.Id && entity.TelecommunicationsNumber.IsPrimary);
            if (faxNumber != null)
            {
                faxNumberIsEqual = IsEqual(faxNumber.TelecommunicationsNumber, txtFaxAreaCode.Text, txtFaxPhoneNumber.Text);

                if (faxNumberIsEqual == false)
                    faxNumber.EndDate = today;
            }

            if (faxNumberIsEqual == false)
            {
                //Create Fax Number
                if (string.IsNullOrWhiteSpace(txtFaxPhoneNumber.Text)==false)
                {
                    Address newAddressAsFaxNumber = AddressBusinessUtility.AddAddress(party, addressType, today);
                    TelecommunicationsNumber newfaxTelecomNumber = AddressBusinessUtility.AddTelNum(newAddressAsFaxNumber, faxNumberAddressType, true);
                    newfaxTelecomNumber.AreaCode = txtFaxAreaCode.Text;
                    newfaxTelecomNumber.PhoneNumber = txtFaxPhoneNumber.Text;
                }
            }
        }

        private void CreateOrUpdateElectronicAddress(FinancialEntities ctx, Party party, Country country, DateTime today)
        {
            var addressType = ctx.AddressTypes.First(entity => entity.Name.Equals("Electronic Address"));
            var electronicAddressType = ctx.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name.Equals("Business Email Address"));
            
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(electronicAddressType);

            bool isEqual = false;
            var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id && entity.ElectronicAddress.IsPrimary);
            if (address != null)
            {
                isEqual = true;
                var electronicAddress = address.ElectronicAddress;
                isEqual = electronicAddress.ElectronicAddressString == txtEmailAddress.Text;

                if (isEqual == false)
                    address.EndDate = today;
            }

            if (isEqual == false)
            {
                //Create Electronic Address
                Address newAddressAsEmail = AddressBusinessUtility.AddAddress(party, addressType, today);
                ElectronicAddress newEmailAddress = AddressBusinessUtility.AddEmail(newAddressAsEmail, electronicAddressType, true);
                newEmailAddress.ElectronicAddressString = txtEmailAddress.Text;
            }
        }

        public bool IsEqual(TelecommunicationsNumber number, string areaCode, string phoneNumber)
        {
            bool isEqual = true;

            isEqual = isEqual && number.AreaCode == areaCode;
            isEqual = isEqual && number.PhoneNumber == phoneNumber;

            return isEqual;
        }

        //When Ok button on the Address detail window is clicked
        protected void wndAddressDetail_btnAdd_Click(object sender, DirectEventArgs e)
        {
            HiddenCountry.Text = cmbCountry.SelectedItem.Value;
            int countyId = int.Parse(HiddenCountry.Text);
            Country country = null;
            using(var ctx = new FinancialEntities()) {
                country = ctx.Countries.SingleOrDefault(entity => entity.Id == countyId);
            }
            
            HiddenStreetAddress.Text = txtStreetAddress.Text;
            HiddenBarangay.Text = txtBarangay.Text;
            if (radioCity.Checked)
            {
                HiddenMunicipality.Text = "";
                HiddenCity.Text = txtCityOrMunicipality.Text;
            }
            else
            {
                HiddenMunicipality.Text = txtCityOrMunicipality.Text;
                HiddenCity.Text = "";
            }

            HiddenProvince.Text = txtProvince.Text;
            HiddenState.Text = txtState.Text;
            HiddenPostalCode.Text = txtPostalCode.Text;

            string fullAddress = StringConcatUtility.Build(", ", 
                            HiddenStreetAddress.Text,
                            HiddenBarangay.Text,
                            HiddenMunicipality.Text,
                            HiddenCity.Text,
                            HiddenProvince.Text,
                            HiddenState.Text,
                            country.Name,
                            HiddenPostalCode.Text);

            tareaAddresses.Text = fullAddress;
            
            txtPrimTelCountryCode.Text = country.CountryTelephoneCode;
            txtSecTelCountryCode.Text = country.CountryTelephoneCode;
            txtFaxCountryCode.Text = country.CountryTelephoneCode;

            wndAddressDetail.Hidden = true;
        }

        private void Retrieve()
        {
            using (var ctx = new FinancialEntities())
            {
                var roleType = ctx.RoleTypes.SingleOrDefault(x => x.Name.Equals("Lending Institution"));
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(roleType);
                var partyRole = ctx.PartyRoles.FirstOrDefault(x => x.RoleTypeId == roleType.Id);

                if (partyRole == null)
                    return;

                var party = partyRole.Party;
                //Store PartyId to hidden variable
                HiddenPartyId.Text = party.Id.ToString();

                Organization organization = party.Organization;
                txtName.Text = organization.OrganizationName;
                cmbOrganizationType.SelectedItem.Value = organization.OrganizationTypeId.ToString();
                dfDateEstablished.Text = organization.DateEstablished.ToString();

                //POSTAL ADDRESS
                var postalAddressType = PostalAddressType.BusinessAddressType.Name;
                var addressType = ctx.AddressTypes.First(entity => entity.Name.Equals("Postal Address"));
                InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
                var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id &&
                                    entity.PostalAddress.PostalAddressType.Name == postalAddressType && entity.PostalAddress.IsPrimary == true);
                var postalAddressSpecific = address.PostalAddress;

                //Fill Address Detail Window
                var country = ctx.Countries.SingleOrDefault(entity => entity.Id == postalAddressSpecific.Country.Id);
                HiddenCountry.Text = postalAddressSpecific.Country.Id.ToString();
                HiddenStreetAddress.Text = postalAddressSpecific.StreetAddress;
                HiddenBarangay.Text = postalAddressSpecific.Barangay;
                HiddenMunicipality.Text = postalAddressSpecific.Municipality;
                HiddenCity.Text = postalAddressSpecific.City;
                HiddenProvince.Text = postalAddressSpecific.Province;
                HiddenState.Text = postalAddressSpecific.State;
                HiddenPostalCode.Text = postalAddressSpecific.PostalCode;

                string fullAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);

                if (string.IsNullOrWhiteSpace(HiddenMunicipality.Text) == false)
                    radioMunicipality.Checked = true;
                else 
                    radioCity.Checked = true;

                //Business Address Display
                tareaAddresses.Text = fullAddress;
                //Primary Phone Number Display
                txtPrimTelCountryCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
                //Fax Number Display
                txtFaxCountryCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
                //Secondary Phone Number Display
                txtSecTelCountryCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                //Primary Business Phone Number
                var addressTypTelecom = ctx.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
                var telecomNumberTypeBusinessPhoneNumber = ctx.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

                InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressTypTelecom);
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeBusinessPhoneNumber);


                var primaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null 
                    && entity.TelecommunicationsNumber.IsPrimary);

                // ICHECK KO UGMA
                if (primaryNumber != null)
                {
                    var primaryNumberSpecific = primaryNumber.TelecommunicationsNumber;

                    txtPrimTelAreaCode.Text = primaryNumberSpecific.AreaCode;
                    txtPrimTelPhoneNumber.Text = primaryNumberSpecific.PhoneNumber;
                }

                //Secondary Business Phone Number
                var secondaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null 
                    && entity.TelecommunicationsNumber.IsPrimary == false);

                if (secondaryNumber != null)
                {
                    var secondaryNumberSpecific = secondaryNumber.TelecommunicationsNumber;
                    txtSecTelAreaCode.Text = secondaryNumberSpecific.AreaCode;
                    txtSecTelPhoneNumber.Text = secondaryNumberSpecific.PhoneNumber;
                }

                //*******************************Primary Fax Number
                var telecomNumberTypeFax = ctx.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessFaxNumber));
                InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeFax);

                var faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeFax.Id && entity.TelecommunicationsNumber.IsPrimary);

                if (faxNumber != null)
                {
                    var faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                    
                    txtFaxAreaCode.Text = faxNumberSpecific.AreaCode;
                    txtFaxPhoneNumber.Text = faxNumberSpecific.PhoneNumber;
                }

                var addressTypeElectonicAddress = ctx.AddressTypes.SingleOrDefault(entity => entity.Name.Equals(AddressTypeEnums.ElectronicAddress));
                var emailTypeBusinessEmail = ctx.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name.Equals(ElectronicAddressTypeEnums.BusinessEmailAddress));
                InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(emailTypeBusinessEmail);
                var primaryEmail = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypeElectonicAddress.Id
                && entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeBusinessEmail.Id && entity.ElectronicAddress.IsPrimary);

                if (primaryEmail != null)
                {
                    var primaryEmailSpecific = primaryEmail.ElectronicAddress;
                    //Email Address
                    txtEmailAddress.Text = primaryEmailSpecific.ElectronicAddressString;
                }
            }
        }
    }
}