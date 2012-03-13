using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.BankUseCases
{
    public partial class AddBank : ActivityPageBase
    {
        //public override List<string> UserTypesAllowed
        //{
        //    get
        //    {
        //        List<string> allowed = new List<string>();
        //        allowed.Add("Super Admin");
        //        allowed.Add("Loan Clerk");
        //        allowed.Add("Admin");
        //        return allowed;
        //    }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                List<Country> countries;
                using (var context = new FinancialEntities())
                {
                    var query = context.Countries;
                    countries = query.ToList();
                }
                hiddenPartyId.Value = "";
                storeCountry.DataSource = countries;
                storeCountry.DataBind();

                SetDefaults();
            }
        }

        public void SetDefaults()
        {
            txtFaxAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtCityOrMunicipality.Text = ApplicationSettings.DefaultCity;
            txtPostalCode.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountry.SelectedItem.Value = Country.Philippines.Id.ToString(); 
        }

        [DirectMethod]
        public int checkBranch()
        {
            int result = 0;
            var name = txtName.Text;
            using (var context = new FinancialEntities())
            {
                // check if inputed name equal to organization name
                var bankRoleType = context.RoleTypes.SingleOrDefault(entity => entity.Name == RoleTypeEnums.Bank);
                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(bankRoleType);

                var sameBankName = context.BankViewLists.FirstOrDefault(entity => entity.Organization_Name == name);
                if (sameBankName != null)
                {
                    hiddenID.Value = sameBankName.PartyRoleID;
                    result = 1;
                }
                else
                {
                    var sameNameNotBank = context.PartyRoles.FirstOrDefault(entity => entity.Party.Organization.OrganizationName == name && entity.EndDate == null
                        && entity.RoleTypeId != RoleType.BankType.Id);
                    if (sameNameNotBank != null)
                    {
                        hiddenID.Value = sameNameNotBank.PartyId;
                        result = 2;
                    }
                }
            }
            return result;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                var today = DateTime.Now;
                Bank bank = new Bank();
                Organization org = null;
                Party party = null;
                if (string.IsNullOrWhiteSpace(hiddenPartyId.Text) != false)
                {
                   party = new Party();
                   org = new Organization();
                   org.Party = party;
                   org.OrganizationName = txtName.Text;
                }
                else
                {
                   int partyId = Convert.ToInt32(hiddenPartyId.Text);
                   party = context.Parties.FirstOrDefault(entity => entity.Id == partyId);
                   org = context.Organizations.FirstOrDefault(entity => entity.PartyId == partyId);
                   org.OrganizationName = txtName.Text;
                  hiddenPartyId.Value = "";
                }
                BankStatu bankStatus = new BankStatu();

                var bankRoleType = context.RoleTypes.SingleOrDefault(entity => entity.Name == RoleTypeEnums.Bank);
                var orgPartyType = context.PartyTypes.SingleOrDefault(entity => entity.Name == PartyTypeEnums.Organization);
                var bankStatusType = context.BankStatusTypes.SingleOrDefault(entity => entity.Name == BankStatusTypeEnum.Active);

                InitialDatabaseValueChecker.ThrowIfNull<RoleType>(bankRoleType);
                InitialDatabaseValueChecker.ThrowIfNull<PartyType>(orgPartyType);
                InitialDatabaseValueChecker.ThrowIfNull<BankStatusType>(bankStatusType);

                party.PartyType = orgPartyType;

                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.PartyRoleType = bankRoleType.PartyRoleType;
                partyRole.EffectiveDate = today;

                bankStatus.BankStatusType = bankStatusType;
                bankStatus.PartyRoleId = partyRole.Id;
                bankStatus.EffectiveDate = today;

                bank.Branch = this.txtBranch.Text;

                if (string.IsNullOrWhiteSpace(this.txtAcronym.Text)) bank.Acronym = bank.PartyRole.Party.Organization.OrganizationName;
                else bank.Acronym = this.txtAcronym.Text;

                var addressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);
                Address postalAdd = AddressBusinessUtility.AddAddress(party, addressType, today);
              
                var postalAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BusinessAddress);
                InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(postalAddressType);
                PostalAddress specificPostalAddress2 = AddressBusinessUtility.AddPostal(postalAdd, postalAddressType, true);
                int countryId = int.Parse(cmbCountry.Value.ToString());
                var country = context.Countries.FirstOrDefault(entity => entity.Id == countryId);
                InitialDatabaseValueChecker.ThrowIfNull<Country>(country);

                if(string.IsNullOrWhiteSpace(hiddenStreetName.Text)==false)specificPostalAddress2.StreetAddress = hiddenStreetName.Text;
                specificPostalAddress2.Barangay = hiddenBarangay.Text;
                specificPostalAddress2.PostalCode = hiddenPostalCode.Text;
                specificPostalAddress2.Country = country;
                if (string.IsNullOrWhiteSpace(hiddenCity.Text) == false) specificPostalAddress2.City = hiddenCity.Text;
                if (string.IsNullOrWhiteSpace(hiddenMunicipality.Text) == false) specificPostalAddress2.Municipality = hiddenMunicipality.Text;
                if (string.IsNullOrWhiteSpace(hiddenProvince.Text) == false) specificPostalAddress2.Province = hiddenProvince.Text;
                var telAddressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.TelecommunicationNumber);
                if (string.IsNullOrWhiteSpace(txtAreaCode.Text) ==false)
                {
                    Address telephoneAddress = AddressBusinessUtility.AddAddress(party, telAddressType, today);
                    var telType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.BusinessPhoneNumber);
                    TelecommunicationsNumber specificTelephoneAddress = AddressBusinessUtility.AddTelNum(telephoneAddress, telType, true);
                    specificTelephoneAddress.AreaCode = txtAreaCode.Text;
                    specificTelephoneAddress.PhoneNumber = txtPhoneNum.Text;
                }
                if (string.IsNullOrWhiteSpace(txtFaxAreaCode.Text)== false)
                {
                    Address faxAddress = AddressBusinessUtility.AddAddress(party, telAddressType, today);
                    var faxType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.BusinessFaxNumber);
                    TelecommunicationsNumber specificTelephoneAddress1 = AddressBusinessUtility.AddTelNum(faxAddress, faxType, false);
                    specificTelephoneAddress1.AreaCode = txtFaxAreaCode.Text;
                    specificTelephoneAddress1.PhoneNumber = txtFaxNum.Text;
                }
                if (string.IsNullOrWhiteSpace(txtEmailAdd.Text) == false)
                {
                    var emailAddType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.ElectronicAddress);
                    var emailType = context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == ElectronicAddressTypeEnums.BusinessEmailAddress);
                    Address email = AddressBusinessUtility.AddAddress(party, emailAddType, today);
                    ElectronicAddress specificEmail = AddressBusinessUtility.AddEmail(email, emailType, true);
                    specificEmail.ElectronicAddressString = txtEmailAdd.Text;
                }

                context.Banks.AddObject(bank);
                context.SaveChanges();
            }
            X.Msg.Alert("Message", "Bank record successfully created").Show();
        }

        protected void btnDoneAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStreet.Text) == false) hiddenStreetName.Text = txtStreet.Text;
            hiddenBarangay.Text = txtBarangay1.Text;
            if (radioCity.Checked)
            {
                hiddenCity.Text = txtCityOrMunicipality.Text;
                hiddenMunicipality.Text = null;
            }
            else
            {
                hiddenMunicipality.Text = txtCityOrMunicipality.Text;
                hiddenCity.Text = null;
            }
            if(string.IsNullOrWhiteSpace(txtProvince1.Text) ==false)
                     hiddenProvince.Text = txtProvince1.Text;
            hiddenCountry.Text = cmbCountry.SelectedItem.Text;
            hiddenPostalCode.Text = txtPostalCode.Text;
            txtAddress.Text = StringConcatUtility.Build(", ", hiddenStreetName.Text,
            hiddenBarangay.Text, hiddenMunicipality.Text,hiddenCity.Text, hiddenProvince.Text,
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