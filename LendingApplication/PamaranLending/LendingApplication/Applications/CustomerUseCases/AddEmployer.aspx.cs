using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.CustomerUseCases
{
    public partial class AddEmployer : ActivityPageBase
    {
        public string PersonName
        {
            get
            {
                return hiddenName.Value.ToString();
            }
        }

        public string UsesExistRecord
        {
            get
            {
                return hiddenUsesExistRecord.Value.ToString();
            }
        }

        public int ExistingParty
        {
            get
            {
                return int.Parse(hiddenExistingParty.Value.ToString());
            }
        }

        public int ExistingPartyRole
        {
            get
            {
                return int.Parse(hiddenExistingPartyRole.Value.ToString());
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
            savePersonNameDetails();
            

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                EmployerForm form = this.CreateOrRetrieve<EmployerForm>();
                hiddenExistingParty.Value = -1;
                hiddenExistingPartyRole.Value = -1;
                hiddenUsesExistRecord.Text = "no";

                string partyType = Request.QueryString["pt"];
                this.txtEmployerPartyType.Text = partyType;

                if (partyType == "Person")
                {
                    cmfPersonName.Show();
                    txtOrganizationName.AllowBlank = true;
                }
                else
                {
                    txtOrganizationName.Show();
                    txtPersonName.AllowBlank = true;
                }
                var countries = Country.All();

                CountryStore.DataSource = countries;
                CountryStore.DataBind();

                SetDefaults();
            }
        }

        protected void SetDefaults()
        {

            //primary home address
            
            nfAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            nfFaxAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtCityOrMunicipalityA1.Text = ApplicationSettings.DefaultCity;
            txtPostalCodeA1.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountryA1.SelectedItem.Value = Country.Philippines.Id.ToString();


        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                EmployerForm form = this.CreateOrRetrieve<EmployerForm>();
                form.IsNew = true;
                form.EmployerPartyType = this.txtEmployerPartyType.Text;
                if (form.EmployerPartyType == "Person")
                {
                    if (string.IsNullOrWhiteSpace(txtPersonTitle.Text) == false)
                        form.EmployerPersonName.Title = this.txtPersonTitle.Text;
                    form.EmployerPersonName.FirstName = txtPersonFirstName.Text;
                    form.EmployerPersonName.LastName = txtPersonLastName.Text;
                    if (!string.IsNullOrWhiteSpace(txtPersonMiddleName.Text))
                        form.EmployerPersonName.MiddleName = txtPersonMiddleName.Text;
                    if (!string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text))
                        form.EmployerPersonName.NameSuffix = txtPersonNameSuffix.Text;
                    if (!string.IsNullOrWhiteSpace(txtNickNameP.Text))
                        form.EmployerPersonName.NickName = txtPersonNickName.Text;
                    form.EmployerPersonName.MothersMaidenName = txtPersonMothersMaidenName.Text;
                }
                else
                {
                    form.EmployerOrganizationName = txtOrganizationName.Text;
                }

                if (hiddenUsesExistRecord.Text == "yes")
                {
                    form.UseExistingParty = true;
                    form.ExistingParty = int.Parse(this.hiddenExistingParty.Text);
                    form.ExistingPartyRole = int.Parse(this.hiddenExistingPartyRole.Text);
                }
                else
                {
                    form.UseExistingParty = false;
                    form.ExistingParty = int.Parse(this.hiddenExistingParty.Text);
                    form.ExistingPartyRole = int.Parse(this.hiddenExistingPartyRole.Text);
                }

                form.EmployerBusinessAddress.StreetAddress = txtStreetAdd1.Text;
                form.EmployerBusinessAddress.State = txtState1.Text;
                form.EmployerBusinessAddress.Province = txtProvince1.Text;
                form.EmployerBusinessAddress.PostalCode = txtPostal1.Text;
                form.EmployerBusinessAddress.Municipality = txtMunicipality1.Text;
                if (!string.IsNullOrEmpty(cmbCountryA1.SelectedItem.Value))
                    form.EmployerBusinessAddress.CountryId = int.Parse(cmbCountryA1.SelectedItem.Value);
                form.EmployerBusinessAddress.City = txtCity1.Text;
                form.EmployerBusinessAddress.Barangay = txtBarangay1.Text;
                form.EmployerBusinessAddress.IsPrimary = true;

                if (!string.IsNullOrWhiteSpace(nfAreaCode.Text) && !string.IsNullOrWhiteSpace(nfPhoneNumber.Text))
                {
                    form.EmployerBusinessPhoneNumber.AreaCode = nfAreaCode.Text;
                    form.EmployerBusinessPhoneNumber.PhoneNumber = nfPhoneNumber.Text;
                    form.EmployerBusinessPhoneNumber.IsPrimary = true;
                }

                if (!string.IsNullOrWhiteSpace(nfFaxAreaCode.Text) && !string.IsNullOrWhiteSpace(nfFaxPhoneNumber.Text))
                {
                    form.EmployerBusinessFaxNumber.AreaCode = nfFaxAreaCode.Text;
                    form.EmployerBusinessFaxNumber.PhoneNumber = nfFaxPhoneNumber.Text;
                    form.EmployerBusinessFaxNumber.IsPrimary = true;
                }

                if (!string.IsNullOrWhiteSpace(txtBusinessEmailAddress.Text))
                {
                    form.EmployerBusinessEmailAddress.EletronicAddressString = txtBusinessEmailAddress.Text;
                    form.EmployerBusinessEmailAddress.IsPrimary = true;
                }

                form.PrepareForSave();
                
            }
        }

        protected void onMunicipalityCityChange(object sender, DirectEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMunicipalityD1.Text) && string.IsNullOrWhiteSpace(txtCityD1.Text))
            {
                txtMunicipalityD1.AllowBlank = false;
                txtCityD1.AllowBlank = true;

            }
            else if (!string.IsNullOrWhiteSpace(txtCityD1.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD1.Text))
            {
                txtCityD1.AllowBlank = false;
                txtMunicipalityD1.AllowBlank = true;
            }
            else if ((string.IsNullOrWhiteSpace(txtCityD1.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD1.Text))
                        || (!string.IsNullOrWhiteSpace(txtCityD1.Text) && !string.IsNullOrWhiteSpace(txtMunicipalityD1.Text)))
            {
                //txtMunicipalityD1.Text = null;
                //txtCityD1.Text = null;
                txtCityD1.AllowBlank = true;
                txtMunicipalityD1.AllowBlank = true;
                X.Msg.Alert("Status", "Please specify either City or Municipality.").Show();
                txtMunicipalityD1.Focus();
                txtCityD1.Focus();
            }

        }

        protected void btnSaveAddress_Click(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgAddress1.Source["Postal Code"].Value;

            txtStreetAdd1.Text = txtStreetNumberD1.Text;
            txtBarangay1.Text = txtBarangayD1.Text;
            txtCity1.Text = txtCityD1.Text;
            //txtState1.Text = pgAddress1.Source["State"].Value;
            txtPostal1.Text = txtPostalCodeD1.Text;
            txtCountry1.Text = cmbCountryD1.SelectedItem.Text;
            txtMunicipality1.Text = txtMunicipalityD1.Text;
            txtProvince1.Text = txtProvinceD1.Text;

            txtBusinessAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd1.Text)))
                txtBusinessAddress.Text = txtStreetAdd1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay1.Text)))
                txtBusinessAddress.Text += txtBarangay1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity1.Text)) && string.IsNullOrWhiteSpace(txtMunicipality1.Text))
                txtBusinessAddress.Text += txtCity1.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality1.Text)) && string.IsNullOrWhiteSpace(txtCity1.Text))
                txtBusinessAddress.Text += txtMunicipality1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince1.Text)))
                txtBusinessAddress.Text += txtProvince1.Text;
            if (!(string.IsNullOrWhiteSpace(txtCountry1.Text)))
                txtBusinessAddress.Text += ", " + txtCountry1.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal1.Text)))
                txtBusinessAddress.Text += " " + txtPostal1.Text;

            using (var context = new FinancialEntities())
            {
                int countID = int.Parse(cmbCountryD1.Text);
                Country count = context.Countries.FirstOrDefault(entity => entity.Id == countID);
                txtTelCode.Text = count.CountryTelephoneCode;
                txtFaxTelCode.Text = count.CountryTelephoneCode;
            }

            wndAddressDetail.Hide();
        }

        protected void btnSavePersonNameDetail_Click(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgPersonName.Source["Postal Code"].Value;

            txtPersonTitle.Text = txtTitleP.Text;
            txtPersonFirstName.Text = txtFirstNameP.Text;
            txtPersonLastName.Text = txtLastNameP.Text;
            txtPersonMiddleName.Text = txtMiddleNameP.Text;
            txtPersonNickName.Text = txtNickNameP.Text;
            txtPersonNameSuffix.Text = txtNameSuffixP.Text;
            txtPersonMothersMaidenName.Text = txtMothersMaidenNameP.Text;

            txtPersonName.Text = txtPersonLastName.Text + ", " + txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            wndPersonNameDetail.Hide();
            //txtPersonName.Focus();
        }

        protected void btnDoneAddressDetailA1_DirectClick(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgAddress1.Source["Postal Code"].Value;

            txtStreetAdd1.Text = txtStreetAddressA1.Text;
            txtBarangay1.Text = txtBarangayA1.Text;
            if (radioCityA1.Checked == true)
                txtCity1.Text = txtCityOrMunicipalityA1.Text;
            else if (radioMunicipalityA1.Checked == true)
                txtMunicipality1.Text = txtCityOrMunicipalityA1.Text;
            //txtState1.Text = pgAddress1.Source["State"].Value;
            txtPostal1.Text = txtPostalCodeA1.Text;
            txtCountry1.Text = cmbCountryA1.SelectedItem.Text;

            txtProvince1.Text = txtProvinceA1.Text;

            txtBusinessAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd1.Text)))
                txtBusinessAddress.Text = txtStreetAdd1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay1.Text)))
                txtBusinessAddress.Text += txtBarangay1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity1.Text)) && string.IsNullOrWhiteSpace(txtMunicipality1.Text))
                txtBusinessAddress.Text += txtCity1.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality1.Text)) && string.IsNullOrWhiteSpace(txtCity1.Text))
                txtBusinessAddress.Text += txtMunicipality1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince1.Text)))
                txtBusinessAddress.Text += txtProvince1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry1.Text)))
                txtBusinessAddress.Text += txtCountry1.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal1.Text)))
                txtBusinessAddress.Text += " " + txtPostal1.Text;

            using (var context = new FinancialEntities())
            {
                int countID = int.Parse(cmbCountryA1.SelectedItem.Value);
                Country count = context.Countries.FirstOrDefault(entity => entity.Id == countID);
                txtTelCode.Text = count.CountryTelephoneCode;
                txtFaxTelCode.Text = count.CountryTelephoneCode;
            }

            winAddressDetailA1.Hide();
        }

        [DirectMethod]
        public void btnSavePersonNameDetails()
        {
            //PrimaryHomeAddress.Text = pgPersonName.Source["Postal Code"].Value;

            txtPersonTitle.Text = txtTitleP.Text;
            txtPersonFirstName.Text = txtFirstNameP.Text;
            txtPersonLastName.Text = txtLastNameP.Text;
            txtPersonMiddleName.Text = txtMiddleNameP.Text;
            txtPersonNickName.Text = txtNickNameP.Text;
            txtPersonNameSuffix.Text = txtNameSuffixP.Text;
            txtPersonMothersMaidenName.Text = txtMothersMaidenNameP.Text;

            txtPersonName.Text = txtPersonLastName.Text + ", " + txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            hiddenName.Value = txtPersonName.Text;
            if (string.IsNullOrWhiteSpace(txtPersonName.Text))
                hiddenName.Value = txtOrganizationName.Text;
            
            
            wndPersonNameDetail.Hide();
            //txtPersonName.Focus();
        }

        public void savePersonNameDetails()
        {
            //PrimaryHomeAddress.Text = pgPersonName.Source["Postal Code"].Value;

            txtPersonTitle.Text = txtTitleP.Text;
            txtPersonFirstName.Text = txtFirstNameP.Text;
            txtPersonLastName.Text = txtLastNameP.Text;
            txtPersonMiddleName.Text = txtMiddleNameP.Text;
            txtPersonNickName.Text = txtNickNameP.Text;
            txtPersonNameSuffix.Text = txtNameSuffixP.Text;
            txtPersonMothersMaidenName.Text = txtMothersMaidenNameP.Text;

            if (!(string.IsNullOrWhiteSpace(txtPersonLastName.Text)))
                txtPersonName.Text = txtPersonLastName.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtPersonFirstName.Text)))
                txtPersonName.Text += txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            hiddenName.Value = txtPersonName.Text;
            if (string.IsNullOrWhiteSpace(txtPersonName.Text))
                hiddenName.Value = txtOrganizationName.Text;
            
            wndPersonNameDetail.Hide();
            //txtPersonName.Focus();
        }

        protected void checkEmployerNames()
        {
            var today = DateTime.Now;
            var personName = txtPersonName.Text;
            //bool res = true;
            var sameName = ObjectContext.EmployersViewLists.SingleOrDefault(entity => entity.Name == personName);

            if (sameName != null)
            {
                int id = sameName.EmployerID;
                X.Msg.Confirm("Message", "An employer record with the same name already exists in the list. Do you want to create another employer record with the same name?", new JFunction("Employer.AddSameEmployer(result, id);", "result")).Show();
            }
            else
            {
                Party party = new Party();
                Person person = new Person();
                Organization org = new Organization();
                PersonName employerFName = new PersonName();
                PersonName employerLName = new PersonName();
                PersonName employerMName = new PersonName();
                PersonName employerNiName = new PersonName();
                PersonName employerNaName = new PersonName();
                PersonName employerTName = new PersonName();
                PersonName employerMMName = new PersonName();

                if (this.txtEmployerPartyType.Text == "Person")
                {
                    party.PartyType = PartyType.PersonType;

                    person.Party = party;

                    employerFName.Person = person;
                    employerFName.PersonNameType = PersonNameType.FirstNameType;
                    employerFName.Name = txtPersonFirstName.Text;
                    employerFName.EffectiveDate = today;
                    
                    employerLName.Person = person;
                    employerLName.PersonNameType = PersonNameType.LastNameType;
                    employerLName.Name = txtPersonLastName.Text;
                    employerLName.EffectiveDate = today;

                    employerMMName.Person = person;
                    employerMMName.PersonNameType = PersonNameType.MothersMaidenNameType;
                    employerMMName.Name = txtPersonMothersMaidenName.Text;
                    employerMMName.EffectiveDate = today;

                    employerMName.Person = person;
                    employerMName.PersonNameType = PersonNameType.MiddleNameType;
                    employerMName.Name = txtPersonMiddleName.Text;
                    employerMName.EffectiveDate = today;

                    employerNaName.Person = person;
                    employerNaName.PersonNameType = PersonNameType.NameSuffixType;
                    employerNaName.Name = txtPersonNameSuffix.Text;
                    employerNaName.EffectiveDate = today;

                    employerNiName.Person = person;
                    employerNiName.PersonNameType = PersonNameType.NickNameType;
                    employerNiName.Name = txtPersonNickName.Text;
                    employerNiName.EffectiveDate = today;


                }
                else
                {
                    party.PartyType = PartyType.OrganizationType;

                    org.OrganizationName = txtOrganizationName.Text;
                    org.Party = party;
                }

                PartyRole partyRole = new PartyRole();
                partyRole.Party = party;
                partyRole.PartyRoleType.RoleType = RoleType.EmployerType;

                var res = Person.CheckEmployerName(partyRole);

                if(res != null)
                    X.Msg.Confirm("Message", "A party record with the same name already exists. Do you want to use this record to create the new employer record?", new JFunction("Employer.AddSameName(result, res);", "result")).Show();
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Checking employer name...")]
        public int checkEmployerName()
        {
            var result = 0;
            var today = DateTime.Now;
            var personName = this.PersonName;

            Party party = new Party();
            Person person = new Person();
            Organization org = new Organization();
            PersonName employerFName = new PersonName();
            PersonName employerLName = new PersonName();
            PersonName employerMName = new PersonName();
            PersonName employerNiName = new PersonName();
            PersonName employerNaName = new PersonName();
            PersonName employerTName = new PersonName();
            PersonName employerMMName = new PersonName();

            party.PartyType = PartyType.PersonType;

            person.Party = party;

            if (this.txtEmployerPartyType.Text == "Person")
            {
                party.PartyType = PartyType.PersonType;

                person.Party = party;

                employerFName.Person = person;
                employerFName.PersonNameType = PersonNameType.FirstNameType;
                employerFName.Name = txtPersonFirstName.Text;
                employerFName.EffectiveDate = today;

                employerLName.Person = person;
                employerLName.PersonNameType = PersonNameType.LastNameType;
                employerLName.Name = txtPersonLastName.Text;
                employerLName.EffectiveDate = today;

                employerMMName.Person = person;
                employerMMName.PersonNameType = PersonNameType.MothersMaidenNameType;
                employerMMName.Name = txtPersonMothersMaidenName.Text;
                employerMMName.EffectiveDate = today;

                employerMName.Person = person;
                employerMName.PersonNameType = PersonNameType.MiddleNameType;
                employerMName.Name = txtPersonMiddleName.Text;
                employerMName.EffectiveDate = today;

                employerNaName.Person = person;
                employerNaName.PersonNameType = PersonNameType.NameSuffixType;
                employerNaName.Name = txtPersonNameSuffix.Text;
                employerNaName.EffectiveDate = today;

                employerNiName.Person = person;
                employerNiName.PersonNameType = PersonNameType.NickNameType;
                employerNiName.Name = txtPersonNickName.Text;
                employerNiName.EffectiveDate = today;

                employerTName.Person = person;
                employerTName.PersonNameType = PersonNameType.TitleType;
                employerTName.Name = txtPersonTitle.Text;
                employerTName.EffectiveDate = today;
            }
            else
            {
                party.PartyType = PartyType.OrganizationType;

                org.OrganizationName = txtOrganizationName.Text;
                org.Party = party;
            }

            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = RoleType.EmployerType.Id;

            //bool res = true;
            var sameName = Person.CheckEmployerPersonName(partyRole);

            if (sameName != null)
            {
                int id = sameName.Id;
                //X.Msg.Confirm("Message", "An employer record with the same name already exists in the list. Do you want to create another employer record with the same name?", new JFunction("Employer.AddSameEmployer(result, id);", "result")).Show();
                this.hiddenID.Value = id;
                result = 1;
            }
            else
            {

                var res = Person.CheckEmployerName(partyRole);

                if (res != null)
                {
                    //X.Msg.Confirm("Message", "A party record with the same name already exists. Do you want to use this record to create the new employer record?", new JFunction("Employer.AddSameName(result, res);", "result")).Show();
                    result = 2;
                    hiddenID.Value = res.Id;
                    hiddenExistingParty.Value = res.PartyId;
                    hiddenExistingPartyRole.Value = res.Id;

                }
            }

            return result;
        }

        //For Customer Record with Same Name
        [DirectMethod(Namespace = "Employer")]
        public void AddSameEmployer(string result, int partyRoleId)
        {
            if (result == "no")
            {
                hiddenUsesExistRecord.Text = "yes";
                hiddenExistingPartyRole.Value = partyRoleId;
                FillEmploymentDetails(partyRoleId);
            }
            else hiddenUsesExistRecord.Text = "no";

        }

        [DirectMethod(Namespace = "Employer")]
        public void AddSameName(string result, Party party)
        {
            if (result == "yes")
            {
                hiddenUsesExistRecord.Text = "yes";
                this.hiddenExistingParty.Value = party.Id;

            }
            else
            {
                hiddenUsesExistRecord.Text = "no";
                //txtPersonName.Text = "";
                //wndPersonNameDetail.Show();
            }
        }

        [DirectMethod]
        public void FillAddress(int partyId)
        {
            Party party = Party.GetById(partyId);
            //Postal Address
            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary == true);

            if (postalAddress != null && postalAddress.PostalAddress != null)
            {
                PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                txtBusinessAddress.Text = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                              postalAddressSpecific.Barangay,
                              postalAddressSpecific.Municipality,
                              postalAddressSpecific.City,
                              postalAddressSpecific.Province,
                              postalAddressSpecific.State,
                              postalAddressSpecific.Country.Name,
                              postalAddressSpecific.PostalCode);

                txtTelCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
                txtFaxTelCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;

                txtBarangay1.Text = postalAddressSpecific.Barangay;
                txtBarangayA1.Text = postalAddressSpecific.Barangay;
                if (!string.IsNullOrWhiteSpace(postalAddressSpecific.City))
                {
                    txtCity1.Text = postalAddressSpecific.City;
                    radioCityA1.Checked = true;
                    txtCityOrMunicipalityA1.Text = postalAddressSpecific.City;
                }
                else if (!string.IsNullOrWhiteSpace(postalAddressSpecific.Municipality))
                {
                    txtMunicipality1.Text = postalAddressSpecific.Municipality;
                    radioMunicipalityA1.Checked = true;
                    txtCityOrMunicipalityA1.Text = postalAddressSpecific.Municipality;
                }
                txtCountry1.Text = postalAddressSpecific.Country.Name;
                cmbCountryA1.SelectedItem.Value = postalAddressSpecific.CountryId.ToString();
                txtPostal1.Text = postalAddressSpecific.PostalCode;
                txtPostalCodeA1.Text = postalAddressSpecific.PostalCode;
                txtProvince1.Text = postalAddressSpecific.Province;
                txtProvinceA1.Text = postalAddressSpecific.Province;
                txtStreetAdd1.Text = postalAddressSpecific.StreetAddress;
                txtStreetAddressA1.Text = postalAddressSpecific.StreetAddress;

                //Business Telephone Number
                Address telecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                                    && entity.TelecommunicationsNumber.TypeId == TelecommunicationsNumberType.HomePhoneNumberType.Id
                                    && entity.TelecommunicationsNumber.IsPrimary == true);

                if (telecomNumber != null && telecomNumber.TelecommunicationsNumber != null)
                {

                    TelecommunicationsNumber telecomNumberSpecific = telecomNumber.TelecommunicationsNumber;
                    nfPhoneNumber.Text = telecomNumberSpecific.PhoneNumber;
                    nfAreaCode.Text = telecomNumberSpecific.AreaCode;
                }
            }

            //Email Address
            Address emailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.PersonalEmailAddressType
                                && entity.ElectronicAddress.IsPrimary);
            if (emailAddress != null && emailAddress.ElectronicAddress != null)
            {
                ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                txtBusinessEmailAddress.Text = primaryEmailAddressSpecific.ElectronicAddressString;
            }


        }

        [DirectMethod]
        public void FillEmploymentDetails(int empPartyRoleId)
        {

            EmploymentDetailsModel model = new EmploymentDetailsModel(empPartyRoleId);

            this.txtBusinessAddress.Text = model.EmploymentAddress.AddressConcat;
            this.txtBusinessEmailAddress.Text = model.EmploymentEmailAddress;
            this.txtTelCode.Text = model.EmpCountryCode;
            this.nfAreaCode.Text = model.EmpTelephoneAreaCode;
            this.nfPhoneNumber.Text = model.EmploymentTelephoneNumber;
            this.txtFaxTelCode.Text = model.EmpCountryCode;
            this.nfFaxAreaCode.Text = model.EmploymentFaxNumberAreaCode;
            this.nfFaxPhoneNumber.Text = model.EmploymentFaxNumber;

        }

        private class EmploymentDetailsModel
        {

            public int PartyRoleId { get; set; }

            public string Name { get; set; }

            public int NewEmployerId { get; set; }

            public PostalAddressModel EmploymentAddress { get; set; }

            public string EmpCountryCode { get; set; }

            public string EmpTelephoneAreaCode { get; set; }

            public string EmploymentTelephoneNumber { get; set; }

            public string EmploymentFaxNumberAreaCode { get; set; }

            public string EmploymentFaxNumber { get; set; }

            public string EmployeeIdNumber { get; set; }

            public string EmployeePosition { get; set; }

            public string EmploymentStatus { get; set; }

            public string EmploymentEmailAddress { get; set; }

            private static FinancialEntities Context
            {
                get
                {
                    if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                        return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                    else
                        return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
                }
            }

            public EmploymentDetailsModel()
            {
                EmploymentAddress = new PostalAddressModel();
            }

            public EmploymentDetailsModel(int empPartyRoleId)
            {
                this.PartyRoleId = empPartyRoleId;
                PartyRole empPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == empPartyRoleId);
                Party empParty = empPartyRole.Party;
                if (empParty.PartyType == PartyType.PersonType)
                {
                    Person employerPerson = empParty.Person;
                    this.Name = StringConcatUtility.Build("", 
                                employerPerson.LastNameString + ", ", 
                                employerPerson.FirstNameString, 
                                employerPerson.MiddleInitialString + " ",
                                employerPerson.NameSuffixString);
                }
                else
                {
                    Organization employerOrg = empParty.Organization;
                    this.Name = employerOrg.OrganizationName;
                }

                InitializeAddresses(empParty);
            }

            private void InitializeAddresses(Party party)
            {
                //Postal Address
                Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.PostalAddressType
                                    && entity.PostalAddress.PostalAddressType == PostalAddressType.BusinessAddressType && entity.PostalAddress.IsPrimary);

                if (postalAddress != null && postalAddress.PostalAddress != null)
                {
                    PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                    this.EmploymentAddress.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                                            postalAddressSpecific.Barangay,
                                                            postalAddressSpecific.Municipality,
                                                            postalAddressSpecific.City,
                                                            postalAddressSpecific.Province,
                                                            postalAddressSpecific.State,
                                                            postalAddressSpecific.Country.Name,
                                                            postalAddressSpecific.PostalCode);

                    this.EmpCountryCode = postalAddressSpecific.Country.CountryTelephoneCode;
                    //Business Telephone Number
                    Address telecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null 
                                            && entity.AddressType == AddressType.TelecommunicationNumberType
                                            && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.BusinessPhoneNumberType
                                            && entity.TelecommunicationsNumber.IsPrimary);

                    if (telecomNumber != null && telecomNumber.TelecommunicationsNumber != null)
                    {

                        TelecommunicationsNumber telecomNumberSpecific = telecomNumber.TelecommunicationsNumber;
                        this.EmploymentTelephoneNumber = telecomNumberSpecific.PhoneNumber;
                        this.EmpTelephoneAreaCode = telecomNumberSpecific.AreaCode;
                    }

                    //Business Fax Number
                    Address faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                        && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.BusinessFaxNumberType
                                        && entity.TelecommunicationsNumber.IsPrimary);

                    if (faxNumber != null && faxNumber.TelecommunicationsNumber != null)
                    {
                        TelecommunicationsNumber faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                        this.EmploymentFaxNumber = faxNumberSpecific.PhoneNumber;
                        this.EmploymentFaxNumberAreaCode = faxNumberSpecific.AreaCode;
                    }
                }

                //Email Address
                Address emailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                        && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.BusinessEmailAddressType
                                        && entity.ElectronicAddress.IsPrimary);

                if (emailAddress != null && emailAddress.ElectronicAddress != null)
                {
                    ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                    this.EmploymentEmailAddress = primaryEmailAddressSpecific.ElectronicAddressString;
                }
            }
        }
    }
}