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
    public partial class AddContactPerson : ActivityPageBase
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
            
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                dtBirthdate.MaxDate = DateTime.Now.AddYears(-(SystemSetting.AgeLimitOfBorrower));
                radMale.Checked = true;
                radioMunicipality.Checked = true;
                dtBirthdate.SelectedValue = DateTime.Now.AddYears(-(SystemSetting.AgeLimitOfBorrower));
                List<Country> countries;
                string mode = Request.QueryString["mode"];
                using (var context = new FinancialEntities())
                {
                    var query = context.Countries;
                    countries = query.ToList();
                }
                storeCountry.DataSource = countries;
                storeCountry.DataBind();

                SetDefaults();
            }
        }

        public void SetDefaults()
        {
            radioCity.Checked = true;
            txtCellphoneAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtTelephoneAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtCityOrMunicipality.Text = ApplicationSettings.DefaultCity;
            txtPostalCode.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountry.SelectedItem.Value = Country.Philippines.Id.ToString();
        }

        [DirectMethod]
        public int checkPersonName(string name)
        {
            int result = 0;
            var names = name;

            var today = DateTime.Now;
            Party party = new Party();
            Person person = new Person();
            PersonName customerFName = new PersonName();
            PersonName customerLName = new PersonName();
            PersonName customerMName = new PersonName();
            PersonName customerNiName = new PersonName();
            PersonName customerNaName = new PersonName();
            PersonName customerTName = new PersonName();
            PersonName customerMMName = new PersonName();

            party.PartyType = PartyType.PersonType;

            person.Party = party;

            customerTName.Person = person;
            customerTName.PersonNameType = PersonNameType.TitleType;
            customerTName.Name = cmbTitle.SelectedItem.Text;
            customerTName.EffectiveDate = today;

            customerFName.Person = person;
            customerFName.PersonNameType = PersonNameType.FirstNameType;
            customerFName.Name = txtFirstName.Text;
            customerFName.EffectiveDate = today;

            customerLName.Person = person;
            customerLName.PersonNameType = PersonNameType.LastNameType;
            customerLName.Name = txtLastName.Text;
            customerLName.EffectiveDate = today;

            customerMMName.Person = person;
            customerMMName.PersonNameType = PersonNameType.MothersMaidenNameType;
            customerMMName.Name = txtMothersMaidenName.Text;
            customerMMName.EffectiveDate = today;

            customerMName.Person = person;
            customerMName.PersonNameType = PersonNameType.MiddleNameType;
            customerMName.Name = txtMiddleName.Text;
            customerMName.EffectiveDate = today;

            customerNaName.Person = person;
            customerNaName.PersonNameType = PersonNameType.NameSuffixType;
            customerNaName.Name = txtNameSuffix.Text;
            customerNaName.EffectiveDate = today;

            customerNiName.Person = person;
            customerNiName.PersonNameType = PersonNameType.NickNameType;
            customerNiName.Name = txtNickName.Text;
            customerNiName.EffectiveDate = today;

            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = RoleType.ContactType.Id;

            //check if input name is equal to existing person name
            var sameName = Person.CheckPersonName(party);
            //InitialDatabaseValueChecker.ThrowIfNull<CustomersViewList>(sameName);

            if (sameName != null)
            {
                //X.Msg.Confirm("Message", "A customer record with the same name already exists. Do you want to create another customer record with the same name?", new JFunction("Customer.AddSameCustomer(result);", "result")).Show();
                hiddenID.Value = sameName.Id;
                result = 1;
            }

            return result;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                var today = DateTime.Now;

                /*_________________________________________________________QUERIES_________________________________________________________________________________________________________*/

                //Gender Types
                GenderType genderType = radMale.Checked ? GenderType.MaleType : GenderType.FemaleType;

                PartyRole lendingInstitutionPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);

                //retrieve telephone country code
                int countryId = int.Parse(cmbCountry.Value.ToString());
                var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == countryId);

                //retreive personName by personNameType
                string personTitle = cmbTitle.Text;
                string personFirstName = txtFirstName.Text;
                string personMiddleName = txtMiddleName.Text;
                string personLastName = txtLastName.Text;
                string personNickName = txtNickName.Text;
                string personNameSuffix = txtNameSuffix.Text;
                string personMothersMaidenName = txtMothersMaidenName.Text;

                /*_________________________________________________________INSERTS______________________________________________________________________________________________________________________________________________________________*/

                //insert partyTypeId to Party
                Party party = new Party();
                party.PartyType = PartyType.PersonType;

                //insert genderTypeId, birthdate to Person 
                Person person = new Person();
                person.GenderType = genderType;
                person.Birthdate = dtBirthdate.SelectedDate;

                Person.CreatePersonName(person, today, personTitle, personFirstName, 
                    personMiddleName, personLastName, personNameSuffix, personNickName);

                Person.CreateOrUpdatePersonNames(person, PersonNameType.MothersMaidenNameType, personMothersMaidenName, today);

                //insert partyId, roleTypeId, effectiveDate to Party Role
                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.PartyRoleType = RoleType.ContactType.PartyRoleType;
                partyRole.EffectiveDate = today;

                //insert firstPartyRoleId, secondPartyRoleId, partyRelationshipTypeId to Party Relationship
                PartyRelationship partyRelationshipType = new PartyRelationship();
                partyRelationshipType.PartyRole = partyRole;
                partyRelationshipType.PartyRole1 = lendingInstitutionPartyRole;
                partyRelationshipType.PartyRelType = PartyRelType.ContactRelationshipType;
                partyRelationshipType.EffectiveDate = today;

                if (!string.IsNullOrWhiteSpace(txtCellphoneNumber.Text))
                {
                    TelecommunicationsNumber specificCellphoneNumber = TelecommunicationsNumber.CreateTelecommNumberAddress(party, TelecommunicationsNumberType.PersonalMobileNumberType, today);
                    specificCellphoneNumber.AreaCode = txtCellphoneAreaCode.Text;
                    specificCellphoneNumber.PhoneNumber = txtCellphoneNumber.Text;
                    specificCellphoneNumber.IsPrimary = true;
                }

                if (!string.IsNullOrWhiteSpace(txtTelephoneNumber.Text))
                {
                    TelecommunicationsNumber specificTelephoneNumber = TelecommunicationsNumber.CreateTelecommNumberAddress(party, TelecommunicationsNumberType.HomePhoneNumberType, today);
                    specificTelephoneNumber.AreaCode = txtTelephoneAreaCode.Text;
                    specificTelephoneNumber.PhoneNumber = txtTelephoneNumber.Text;
                    specificTelephoneNumber.IsPrimary = true;
                }


                if (!string.IsNullOrWhiteSpace(txtEmailAddress.Text))
                {
                    ElectronicAddress specificEmailAddress = ElectronicAddress.CreateElectronicAddress(party, ElectronicAddressType.PersonalEmailAddressType, today);
                    specificEmailAddress.ElectronicAddressString = txtEmailAddress.Text;
                    specificEmailAddress.IsPrimary = true;
                }

                AddressType addressType = AddressType.PostalAddressType;
                PostalAddressType postalAddressType = PostalAddressType.HomeAddressType;

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

                ObjectContext.Parties.AddObject(party);
           
            }
        }

        //Create Person Method
        public PersonName CreatePersonName(FinancialEntities context, string nameType, string name, Person person)
        {
            var personNameType = context.PersonNameTypes.SingleOrDefault(entity => entity.Name == nameType);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(personNameType);

            PersonName personName = new PersonName();
            personName.Name = name;
            personName.PersonNameType = personNameType;
            personName.Person = person;
            personName.EffectiveDate = DateTime.Now;
            return personName;
        }

        protected void btnCancelPersonName_DirectClick(object sender, DirectEventArgs e)
        {
            cmbTitle.Text = null;
            txtFirstName.Text = null;
            txtLastName.Text = null;
            txtMiddleName.Text = null;
            txtNickName.Text = null;
            txtNameSuffix.Text = null;
            txtMothersMaidenName.Text = null;
            winPersonName.Hidden = true;
        }
        protected void btnCancelAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            txtStreetAddress.Text = "";
            txtBarangay.Text = "";
            txtCityOrMunicipality.Text = "";
            cmbCountry.SelectedItem.Text = "";
            txtPostalCode.Text = "";
            txtProvince.Text = "";
            txtState.Text = "";
            winAddressDetail.Hidden = true;
        }
        protected void btnDoneAddressDetail_DirectClick(object sender, DirectEventArgs e)
        {
            hiddenStreet.Text = txtStreetAddress.Text;
            hiddenBarangay.Text = txtBarangay.Text;
            if (radioCity.Checked == true)
            {
                hiddenMunicipality.Text = null;
                hiddenCity.Text = txtCityOrMunicipality.Text;
            }
            else
            {
                hiddenCity.Text = null;
                hiddenMunicipality.Text = txtCityOrMunicipality.Text;
            }

            hiddenProvince.Text = txtProvince.Text;
            hiddenCountry.Text = cmbCountry.SelectedItem.Text;
            hiddenPostalCode.Text = txtPostalCode.Text;
            txtHomeAddress.Text = StringConcatUtility.Build(", ", hiddenStreet.Text,
            hiddenBarangay.Text, hiddenMunicipality.Text, hiddenCity.Text, hiddenProvince.Text, 
            hiddenCountry.Text, hiddenPostalCode.Text);
            winAddressDetail.Hidden = true;


            using (var context = new FinancialEntities())
            {
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = context.Countries.SingleOrDefault(entity => entity.Id == countryId);
                txtCellphoneCode.Text = country.CountryTelephoneCode;
                txtTelephoneCode.Text = country.CountryTelephoneCode;
            }
        }
        [DirectMethod]
        public void btnDonePersonName_Click()
        {
            if(string.IsNullOrWhiteSpace(cmbTitle.Text) == false)
                hiddenTitle.Text = cmbTitle.Text;
            hiddenFirstName.Text = txtFirstName.Text;
            hiddenLastName.Text = txtLastName.Text;
            hiddenMiddleName.Text = txtMiddleName.Text;
            hiddenNickName.Text = txtNickName.Text;
            hiddenNameSuffix.Text = txtNameSuffix.Text;
            hiddenMaidenName.Text = txtMothersMaidenName.Text;
          
            txtName.Text = hiddenLastName.Text + ", " + hiddenFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(hiddenMiddleName.Text)))
                txtName.Text += " " + hiddenMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(hiddenNameSuffix.Text)))
                txtName.Text += " " + hiddenNameSuffix.Text;

            hiddenName.Value = txtName.Text;

            winPersonName.Hidden = true;
        }
    }
}
