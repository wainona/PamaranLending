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
    public partial class EditContactPerson : ActivityPageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Country> countries;
            using (var Context = new FinancialEntities())
            {
                var query = Context.Countries;
                countries = query.ToList();
            }
            storeCountry.DataSource = countries;
            storeCountry.DataBind();
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string partyRoleId = Request.QueryString["id"];
                string mode = Request.QueryString["mode"];
                int id ;
                if (int.TryParse(partyRoleId, out id))
                    RetrieveAndAssignDatabaseRecords(id);

                if(PartyRole.GetById(int.Parse(partyRoleId)) == null)
                    throw new AccessToDeletedRecordException("The selected conact has already been deleted by another user.");
            }
        }

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        /// 
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


        private string GetPersonNameByType(Person person, string type)
        {
            var personNameType = ObjectContext.PersonNameTypes.SingleOrDefault(entity => entity.Name == type);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(personNameType);

            var personName = person.PersonNames.SingleOrDefault(entity => entity.PersonNameTypeId == personNameType.Id && entity.EndDate == null);

            if (personName != null)
                return personName.Name;
            else
                return "";
        }

        private PersonName GetPersonNameByTypeEntity(Person person, string type)
        {
            var personNameType = ObjectContext.PersonNameTypes.SingleOrDefault(entity => entity.Name == type);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(personNameType);

            var personName = person.PersonNames.SingleOrDefault(entity => entity.PersonNameTypeId == personNameType.Id && entity.EndDate == null);
            return personName;
        }

        private void RetrieveAndAssignDatabaseRecords(int partyRoleId)
        {
                hiddenPartyRoleID.Value = partyRoleId;
                PartyRole role = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
                if ( role.Party.Person != null)
                {
                    var person = role.Party.Person;
                    if (person.GenderType.Name == GenderTypeEnums.Male)
                        radMale.Checked = true;
                    else
                        radFemale.Checked = true;
                    

                    hiddenTitle.Text = GetPersonNameByType(person, PersonNameTypeEnums.Title);
                    cmbTitle.SelectedItem.Text = GetPersonNameByType(person, PersonNameTypeEnums.Title);
                    hiddenFirstName.Text = GetPersonNameByType(person, PersonNameTypeEnums.FirstName);
                    txtFirstName.Text = GetPersonNameByType(person, PersonNameTypeEnums.FirstName);
                    hiddenLastName.Text = GetPersonNameByType(person, PersonNameTypeEnums.LastName);
                    txtLastName.Text = GetPersonNameByType(person, PersonNameTypeEnums.LastName);
                    hiddenMiddleName.Text = GetPersonNameByType(person, PersonNameTypeEnums.MiddleName);
                    txtMiddleName.Text = GetPersonNameByType(person, PersonNameTypeEnums.MiddleName);
                    hiddenNickName.Text = GetPersonNameByType(person, PersonNameTypeEnums.NickName);
                    txtNickName.Text = GetPersonNameByType(person, PersonNameTypeEnums.NickName);
                    hiddenNameSuffix.Text = GetPersonNameByType(person, PersonNameTypeEnums.NameSuffix);
                    txtNameSuffix.Text = GetPersonNameByType(person, PersonNameTypeEnums.NameSuffix);
                    hiddenMaidenName.Text = GetPersonNameByType(person, PersonNameTypeEnums.MothersMaidenName);
                    txtMothersMaidenName.Text = GetPersonNameByType(person, PersonNameTypeEnums.MothersMaidenName);
                    txtName.Text = hiddenLastName.Text + ", " + hiddenFirstName.Text;
                    if (!(string.IsNullOrWhiteSpace(txtMiddleName.Text)))
                        txtName.Text += " " + txtMiddleName.Text[0] + ".";
                    if (!(string.IsNullOrWhiteSpace(txtNameSuffix.Text)))
                        txtName.Text += " " + txtNameSuffix.Text;

                    if (dtBirthdate.SelectedDate != null)
                    {
                        dtBirthdate.SelectedDate = person.Birthdate.Value;
                    }
                    else
                    {
                        dtBirthdate.Value = null;
                    }
                   

                    var addressType = ObjectContext.AddressTypes.First(entity => entity.Name == AddressTypeEnums.PostalAddress);
                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
                    var postalAddress = role.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id);
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

                    txtHomeAddress.Text = fullAddress;
                    if (string.IsNullOrWhiteSpace(hiddenMunicipality.Text) == false)
                       radioMunicipality.Checked = true;
                    else
                        radioCity.Checked = true;

                    //Primary Home Phone Number
                    var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
                    var telecomNumberTypeHomePhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.HomePhoneNumber));

                    //Primary Personal Cellphone Number
                    var telecomNumberTypePersonalCellphoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.PersonalMobileNumber));

                    InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressTypTelecom);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeHomePhoneNumber);
                    InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypePersonalCellphoneNumber);

                    var primaryNumber = role.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                    && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeHomePhoneNumber.Id && entity.TelecommunicationsNumber != null
                    && entity.TelecommunicationsNumber.IsPrimary);
                    txtTelephoneCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                    var primaryCellphoneNumber = role.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                   && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypePersonalCellphoneNumber.Id && entity.TelecommunicationsNumber != null
                   && entity.TelecommunicationsNumber.IsPrimary);
                    txtCellphoneCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                    // ICHECK KO UGMA
                    if (primaryNumber != null)
                    {
                        var primaryNumberSpecific = primaryNumber.TelecommunicationsNumber;
                        txtTelephoneAreaCode.Text = primaryNumberSpecific.AreaCode;
                        txtTelephoneNumber.Text = primaryNumberSpecific.PhoneNumber;
                    }

                    if (primaryCellphoneNumber != null)
                    {
                        var primaryCellphoneNumberSpecific = primaryCellphoneNumber.TelecommunicationsNumber;
                        txtCellphoneAreaCode.Text = primaryCellphoneNumberSpecific.AreaCode;
                        txtCellphoneNumber.Text = primaryCellphoneNumberSpecific.PhoneNumber;
                    }


                    var addressTypeElectonicAddress = ObjectContext.AddressTypes.SingleOrDefault(entity => entity.Name.Equals(AddressTypeEnums.ElectronicAddress));
                    var emailTypeBusinessEmail = ObjectContext.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name.Equals(ElectronicAddressTypeEnums.PersonalEmailAddress));
                    InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(emailTypeBusinessEmail);
                    var primaryEmail = role.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypeElectonicAddress.Id
                    && entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeBusinessEmail.Id && entity.ElectronicAddress.IsPrimary);

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
        }

        private void CreateOrUpdateContactInfo(Party party, Country country, DateTime today)
        {
            TelecommunicationsNumber cellNumber = new TelecommunicationsNumber();
            cellNumber.AreaCode = this.txtCellphoneAreaCode.Text;
            cellNumber.PhoneNumber = this.txtCellphoneNumber.Text;
            cellNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
            cellNumber.IsPrimary = true;
            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, cellNumber,
                TelecommunicationsNumberType.PersonalMobileNumberType, today, true);

            TelecommunicationsNumber telephoneNumber = new TelecommunicationsNumber();
            telephoneNumber.AreaCode = this.txtTelephoneAreaCode.Text;
            telephoneNumber.PhoneNumber = this.txtTelephoneNumber.Text;
            telephoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
            telephoneNumber.IsPrimary = true;

            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, telephoneNumber,
                TelecommunicationsNumberType.HomePhoneNumberType, today, true);

            ElectronicAddress emailAddress = new ElectronicAddress();
            emailAddress.ElectronicAddressString = txtEmailAddress.Text;
            emailAddress.IsPrimary = true;

            ElectronicAddress.CreateOrUpdateElectronicAddress(party, emailAddress, 
                ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary, today);
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
            if (role.Party.Person != null)
            {
                GenderType genderType = radMale.Checked ? GenderType.MaleType : GenderType.FemaleType;

                var person = role.Party.Person;
                person.GenderType = genderType;
                person.Birthdate = dtBirthdate.SelectedDate;

                Person.CreatePersonName(person, today, cmbTitle.Text, txtFirstName.Text,
                    txtMiddleName.Text, txtLastName.Text, txtNameSuffix.Text, txtNickName.Text);

                Person.CreateOrUpdatePersonNames(person, PersonNameType.MothersMaidenNameType, txtMothersMaidenName.Text, today);

                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == countryId);
                CreateOrUpdatePostalAddress(role.Party, country, today);
                CreateOrUpdateContactInfo(role.Party, country, today);
            }
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
            txtHomeAddress.Text = StringConcatUtility.Build(", ", hiddenStreet.Text,
                hiddenBarangay.Text, hiddenMunicipality.Text, hiddenCity.Text, hiddenProvince.Text,
                hiddenCountry.Text, hiddenPostalCode.Text);
            winAddressDetail.Hidden = true;


            using (var Context = new FinancialEntities())
            {
                int countryId = int.Parse(cmbCountry.SelectedItem.Value.ToString());
                var country = Context.Countries.SingleOrDefault(entity => entity.Id == countryId);
                txtCellphoneCode.Text = country.CountryTelephoneCode;
                txtTelephoneCode.Text = country.CountryTelephoneCode;
            }
        }

        protected void btnDonePersonName_DirectClick(object sender, DirectEventArgs e)
        {
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

            winPersonName.Hidden = true;
        }

        public void btnOpen_Click(object sender, DirectEventArgs e)
        {
            btnBrowseAddress.Disabled = true;
            btnBrowseName.Disabled = true;
            btnOpen.Hidden = true;
            btnEdit.Hidden = false;
            Panel1.Disabled = true;
            Panel2.Disabled = true;
            Panel3.Disabled = true;
            //Panel4.Disabled = true;
        }

        protected void btnEdit_Click(object sender, DirectEventArgs e)
        {
            btnBrowseAddress.Disabled = false;
            btnBrowseName.Disabled = false;
            btnEdit.Hidden = true;
            btnOpen.Hidden = false;
            Panel1.Disabled = false;
            Panel2.Disabled = false;
            Panel3.Disabled = false;
            //Panel4.Disabled = false;
        }
    }
}


#region Old COde
//private void CreateOrUpdatePersonNames(FinancialEntities Context, Person person)
//{
//    DateTime today = DateTime.Now;
//    PersonName title = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.Title);
//    PersonName firstName = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.FirstName);
//    PersonName lastName = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.LastName);
//    PersonName middleName = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.MiddleName);
//    PersonName nickName = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.NickName);
//    PersonName nameSuffix = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.NameSuffix);
//    PersonName mothersMaidenName = GetPersonNameByTypeEntity(Context, person, PersonNameTypeEnums.MothersMaidenName);

//    if (title == null || cmbTitle.Text != title.Name)
//    {
//        if (title != null)
//            title.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.Title, cmbTitle.Text, person);
//    }

//    if (firstName == null || cmbTitle.Text != firstName.Name)
//    {
//        if (firstName != null)
//            firstName.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.FirstName, txtFirstName.Text, person);
//    }

//    if (lastName == null || cmbTitle.Text != lastName.Name)
//    {
//        if (lastName != null)
//            lastName.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.LastName, txtLastName.Text, person);
//    }

//    if (middleName == null || cmbTitle.Text != middleName.Name)
//    {
//        if (middleName != null)
//            middleName.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.MiddleName,  txtMiddleName.Text, person);
//    }

//    if (nickName == null || cmbTitle.Text != nickName.Name)
//    {
//        if (nickName != null)
//            nickName.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.NickName, txtNickName.Text, person);
//    }

//    if (nameSuffix == null || cmbTitle.Text != nameSuffix.Name)
//    {
//        if (nameSuffix != null)
//            nameSuffix.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.NameSuffix, txtNameSuffix.Text, person);
//    }

//    if (mothersMaidenName == null || cmbTitle.Text != mothersMaidenName.Name)
//    {
//        if (mothersMaidenName != null)
//            mothersMaidenName.EndDate = today;
//        CreatePersonName(Context, PersonNameTypeEnums.MothersMaidenName, txtMothersMaidenName.Text, person);
//    }
//}
#endregion
