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
    public partial class AddContactOrganization : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                allowed.Add("Admin");
                return allowed;
            }
        }
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
            radioMunicipality.Checked = true;
            dtDateEstablished.MaxDate = DateTime.Now;
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                List<Country> countries;
                List<OrganizationType> organizationTypes;
                string mode = Request.QueryString["mode"];
                using (var context = new FinancialEntities())
                {
                    var query = context.OrganizationTypes;
                    organizationTypes = query.ToList();

                    var query1 = context.Countries;
                    countries = query1.ToList();
                }
                storeOrganizationType.DataSource = organizationTypes;
                storeOrganizationType.DataBind();

                storeCountry.DataSource = countries;
                storeCountry.DataBind();

                SetDefaults();
            }
        }

        public void SetDefaults()
        {
            radioCity.Checked = true;
            txtFaxAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtTelephoneAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtCityOrMunicipality.Text = ApplicationSettings.DefaultCity;
            txtPostalCode.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountry.SelectedItem.Value = Country.Philippines.Id.ToString();
        }

        [DirectMethod]
        public int checkOrganizationName()
        {
            int result = 0;
            var name = txtName.Text;
            using (var context =  new FinancialEntities())
            {
                var sameOrgName = context.ContactViewLists.FirstOrDefault(entity =>
                    entity.Name == name);

                if (sameOrgName != null)
                {
                    hiddenID.Value = sameOrgName.PartyRoleId;
                    result = 1;
                }
            }

            return result;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                SaveBasicOrganizationContactInformation();
            }
        }

        private void SaveBasicOrganizationContactInformation()
        {
            var today = DateTime.Now;

            //insert partyTypeId to Party
            PartyType partyType = ObjectContext.PartyTypes.SingleOrDefault(entity => entity.Name == PartyTypeEnums.Organization);
            Party party = new Party();
            party.PartyType = partyType;

            //insert organizationType, organizationName and dateEstablished to Organization
            OrganizationType internalOrganization = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Name == OrganizationTypeEnums.Internal);
            OrganizationType externalOrganization = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Name == OrganizationTypeEnums.External);
            Organization organization = new Organization();
            organization.OrganizationName = txtName.Text;
            if (!string.IsNullOrWhiteSpace(cmbOrganizationType.SelectedItem.Text))
            {
                int organizationTypeId = int.Parse(cmbOrganizationType.Value.ToString());
                var organizationType = ObjectContext.OrganizationTypes.SingleOrDefault(entity => entity.Id == organizationTypeId);
                organization.OrganizationType = organizationType;
            }
            else
            {
                organization.OrganizationTypeId = null;
            }
            if (dtDateEstablished.SelectedValue != null)
                organization.DateEstablished = dtDateEstablished.SelectedDate;
            else
                organization.DateEstablished = null;
           

            //retirieve contactRoleType from Role Type where name = "Contact"
            var contactRoleType = ObjectContext.RoleTypes.SingleOrDefault(entity => entity.Name == RoleTypeEnums.Contact);

            //insert partyId, contactRoleType and effectiveDate to Party Role
            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.PartyRoleType = contactRoleType.PartyRoleType;
            partyRole.EffectiveDate = today;

            //insert firstPartyRoleId, secondPartyRoleId and EffectiveDate to PartyRelationship
            var contactRelationshipType = ObjectContext.PartyRelTypes.SingleOrDefault(entity => entity.Name == PartyRelationTypesEnums.ContactRelationship);
            RoleType lendingInstitution = ObjectContext.RoleTypes.SingleOrDefault(entity => entity.Name == RoleTypeEnums.LendingInstitution);
            PartyRole lendingInstitutionPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == lendingInstitution.Id);
            PartyRelationship partyRelationship = new PartyRelationship();
            partyRelationship.PartyRole = partyRole;
            partyRelationship.PartyRole1 = lendingInstitutionPartyRole;
            partyRelationship.PartyRelType = contactRelationshipType;
            partyRelationship.EffectiveDate = today;

            //retrieve postalAddressCategoryType from AddressType where name = "Postal Address"
            var postalAddressCategoryType = ObjectContext.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);

            //retrieve businessAddressCategory from Address where addressType = postalAddressCategoryType and name = "Business Address"
            var primaryBusinessAddress = ObjectContext.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BusinessAddress);

            //insert address
            int countryId = int.Parse(cmbCountry.Value.ToString());
            var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == countryId);

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
            //retrieve telecommunicationNumberType 

            if (!string.IsNullOrWhiteSpace(txtFaxNumber.Text))
            {
                TelecommunicationsNumber specificCellphoneNumber = TelecommunicationsNumber.CreateTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessFaxNumberType, today);
                specificCellphoneNumber.AreaCode = txtFaxAreaCode.Text;
                specificCellphoneNumber.PhoneNumber = txtFaxNumber.Text;
                specificCellphoneNumber.IsPrimary = true;
            }

            if (!string.IsNullOrWhiteSpace(txtTelephoneNumber.Text))
            {
                TelecommunicationsNumber specificTelephoneNumber = TelecommunicationsNumber.CreateTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessPhoneNumberType, today);
                specificTelephoneNumber.AreaCode = txtTelephoneAreaCode.Text;
                specificTelephoneNumber.PhoneNumber = txtTelephoneNumber.Text;
                specificTelephoneNumber.IsPrimary = true;
            }


            if (!string.IsNullOrWhiteSpace(txtEmailAddress.Text))
            {
                ElectronicAddress specificEmailAddress = ElectronicAddress.CreateElectronicAddress(party, ElectronicAddressType.BusinessEmailAddressType, today);
                specificEmailAddress.ElectronicAddressString = txtEmailAddress.Text;
                specificEmailAddress.IsPrimary = true;
            }

            ObjectContext.Parties.AddObject(party);
        }

        protected void btnCancelAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {

            txtStreetAddress.Text = "";
            txtBarangay.Text = "";
            txtCityOrMunicipality.Text = "";
            txtProvince.Text = "";
            cmbCountry.Text = "";
            txtPostalCode.Text = "";
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


            using (var context = new FinancialEntities())
            {
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = context.Countries.SingleOrDefault(entity => entity.Id == countryId);
                txtFaxCode.Text = country.CountryTelephoneCode;
                txtTelephoneCode.Text = country.CountryTelephoneCode;
            }
        }

    }
}