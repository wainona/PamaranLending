using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using System.IO;

namespace LendingApplication.Applications.EmployeeUseCases
{
    public partial class AddEmployee : ActivityPageBase
    {
        private string uploadDirectory;

        protected string imageFilename;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Images");
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                var countries = ObjectContext.Countries;
                countries.ToList();
                strCountry.DataSource = countries;
                strCountry.DataBind();

                cmbEmploymentStatus.SelectedItem.Text = "Employed";

                radioCity.Checked = true;
                radioFemale.Checked = true;
                dtBirthDate.SelectedDate = DateTime.Now.AddYears(-18);
                dtBirthDate.MaxDate = DateTime.Now.AddYears(-18);
                cmbCountry.SelectedItem.Value = Country.Philippines.Id.ToString();
                txtCityOrMunicipality.Text = ApplicationSettings.DefaultCity;
                txtCellAreaCode.Text = ApplicationSettings.DefaultAreaCode;
                txtTeleAreaCode.Text = ApplicationSettings.DefaultAreaCode;
                txtPostalCode.Text = ApplicationSettings.DefaultPostalCode;

                EmployeePositionStore.DataSource = EmployeePositionType.All;
                EmployeePositionStore.DataBind();
            }
        }
        
        /******** CREATE NEW EMPLOYEE ********/

        //New Party
        protected Party CreateParty()
        {
            Party NewParty = new Party();
            NewParty.PartyTypeId = PartyType.PersonType.Id;

            ObjectContext.Parties.AddObject(NewParty);
            return NewParty;
        }
        //New PartyRole
        protected PartyRole CreateEmployeePartyRole(Party Party, DateTime Today)
        {
            PartyRole NewPartyRole = new PartyRole();
            NewPartyRole.Party = Party;
            NewPartyRole.RoleTypeId = RoleType.EmployeeType.Id;
            NewPartyRole.EffectiveDate = Today;
            NewPartyRole.EndDate = null;
            ObjectContext.PartyRoles.AddObject(NewPartyRole);

            return NewPartyRole;
        }
        //New Person
        protected Person CreatePerson(Party Party, DateTime BirthDate, GenderType GenderType, string ImageFileName)
        {
            Person NewPerson = new Person();
            NewPerson.Party = Party;
            NewPerson.GenderType = GenderType;
            NewPerson.NationalityType = null;
            NewPerson.EducAttainmentType = EducAttainmentType.CollegeGraduateType;
            NewPerson.Birthdate = BirthDate;
            NewPerson.ImageFilename = ImageFileName;

            ObjectContext.People.AddObject(NewPerson);
            return NewPerson;
        }
        //New Employee
        protected void CreateEmployee(PartyRole PartyRole, string EmployeeIdNumber,string Position, string SssNumber, string GsisNumber, string OwaNumber, string PhicNumber)
        {
            Employee NewEmployee = new Employee();
            NewEmployee.PartyRole = PartyRole;
            NewEmployee.Position = Position;
            if (!string.IsNullOrWhiteSpace(EmployeeIdNumber)) NewEmployee.EmployeeIdNumber = EmployeeIdNumber;
            if (!string.IsNullOrWhiteSpace(SssNumber)) NewEmployee.SssNumber = SssNumber;
            if (!string.IsNullOrWhiteSpace(GsisNumber)) NewEmployee.GsisNumber = GsisNumber;
            if (!string.IsNullOrWhiteSpace(OwaNumber)) NewEmployee.OwaNumber = OwaNumber;
            if (!string.IsNullOrWhiteSpace(PhicNumber)) NewEmployee.PhicNumber = PhicNumber;

            ObjectContext.Employees.AddObject(NewEmployee);
        }
        //New PartyRelationship
        protected PartyRelationship CreatePartyRelationship(PartyRole FirstPartyRole, PartyRole SecondPartyRole, DateTime Today)
        {
            PartyRelationship NewPartyRelationship = new PartyRelationship();
            NewPartyRelationship.FirstPartyRoleId = FirstPartyRole.Id;
            NewPartyRelationship.SecondPartyRoleId = SecondPartyRole.Id;
            NewPartyRelationship.PartyRelType = PartyRelType.EmploymentType;
            NewPartyRelationship.EffectiveDate = Today;
            NewPartyRelationship.EndDate = null;

            ObjectContext.PartyRelationships.AddObject(NewPartyRelationship);
            return NewPartyRelationship;
        }
        //New Employment
        protected void CreateEmployment(PartyRelationship PartyRelationship, string EmploymentStatus, string Salary)
        {
            Employment NewEmployment = new Employment();
            NewEmployment.PartyRelationship = PartyRelationship;
            NewEmployment.EmploymentStatus = EmploymentStatus;
            NewEmployment.Salary = Salary;

            ObjectContext.Employments.AddObject(NewEmployment);
        }
        //New Address
        protected Address CreateAddress(Party Party, AddressType AddressType, DateTime Today)
        {
            Address NewAddress = new Address();
            NewAddress.Party = Party;
            NewAddress.AddressType = AddressType;
            NewAddress.EffectiveDate = Today;
            NewAddress.EndDate = null;

            ObjectContext.Addresses.AddObject(NewAddress);
            return NewAddress;
        }
        //New Postal Address
        protected void CreatePostalAddress(Address Address, 
                                            PostalAddressType PostalAddressType, 
                                            Country Country, 
                                            string StreetAddress, 
                                            string Barangay, 
                                            string Municipality, 
                                            string City, 
                                            string Province, 
                                            string State, 
                                            string PostalCode)
        {
            PostalAddress NewPostalAddress = new PostalAddress();
            NewPostalAddress.Address = Address;
            NewPostalAddress.PostalAddressType = PostalAddressType;
            NewPostalAddress.Country = Country;

            if (string.IsNullOrWhiteSpace(StreetAddress)) NewPostalAddress.StreetAddress = null;
            else NewPostalAddress.StreetAddress = StreetAddress;

            NewPostalAddress.Barangay = Barangay;

            if (string.IsNullOrWhiteSpace(Municipality)) NewPostalAddress.Municipality = null;
            else NewPostalAddress.Municipality = Municipality;

            if (string.IsNullOrWhiteSpace(City)) NewPostalAddress.City = null;
            else NewPostalAddress.City = City;

            if (string.IsNullOrWhiteSpace(Province)) NewPostalAddress.Province = null;
            else NewPostalAddress.Province = Province;

            if (string.IsNullOrWhiteSpace(State)) NewPostalAddress.State = null;
            else NewPostalAddress.State = State;

            NewPostalAddress.PostalCode = PostalCode;
            NewPostalAddress.IsPrimary = true;

            ObjectContext.PostalAddresses.AddObject(NewPostalAddress);
        }
        //New Cellphone Number
        protected void CreateTelecomeNumber(Address Address, 
                                            TelecommunicationsNumberType TelecomNumberType, 
                                            Country Country, 
                                            string AreaCode,
                                            string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(AreaCode) && string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ObjectContext.Detach(Address);
                return;
            }
            TelecommunicationsNumber NewTelecomNumber = new TelecommunicationsNumber();
            NewTelecomNumber.Address = Address;
            NewTelecomNumber.TelecommunicationsNumberType = TelecomNumberType;
            NewTelecomNumber.AreaCode = AreaCode;
            NewTelecomNumber.PhoneNumber = PhoneNumber;
            NewTelecomNumber.IsPrimary = true;

            ObjectContext.TelecommunicationsNumbers.AddObject(NewTelecomNumber);

        }
        //New Email Address
        protected void CreateEmailAddress(Address Address, ElectronicAddressType ElectronicAddressType, string ElectronicAddressString)
        {
            if (string.IsNullOrWhiteSpace(ElectronicAddressString))
            {
                ObjectContext.Detach(Address);
                return;
            } 

            ElectronicAddress NewEmailAddress = new ElectronicAddress();
            NewEmailAddress.Address = Address;
            NewEmailAddress.ElectronicAddressType = ElectronicAddressType;
            NewEmailAddress.ElectronicAddressString = ElectronicAddressString;
            NewEmailAddress.IsPrimary = true;

            ObjectContext.ElectronicAddresses.AddObject(NewEmailAddress);
        }

        protected string ConcatinateTIN(string tin1, string tin2, string tin3, string tin4)
        {
            if (string.IsNullOrWhiteSpace(tin1) ||
                string.IsNullOrWhiteSpace(tin2) ||
                string.IsNullOrWhiteSpace(tin3)) return string.Empty;

            if (string.IsNullOrWhiteSpace(tin4)) tin4 = "000";

            string concatinatedTIN = tin1 + "-" + tin2 + "-" + tin3 + "-" + tin4;

            return concatinatedTIN;
        }
        /*************************************/

        protected void CheckIfHasAnotherEmployer(Party party)
        {
            var partyRole = PartyRole.GetByPartyIdAndRole(party.Id, RoleType.EmployeeType);
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.EndDate == null && entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var today = DateTime.Now;

            if (partyRole != null)
            {
                var partyRel = ObjectContext.PartyRelationships.SingleOrDefault(entity => entity.EndDate == null && entity.PartyRelTypeId == PartyRelType.EmploymentType.Id
                                                                               && entity.FirstPartyRoleId == partyRole.Id && entity.SecondPartyRoleId != lenderPartyRole.Id);

                if (partyRel != null)
                {
                    partyRole.EndDate = today;
                    partyRel.EndDate = today;
                }
            }
        }

        //MAIN BUTTONS
        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            DateTime today = DateTime.Now;
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
            Party party;
            string concatinatedTIN = ConcatinateTIN(txtTIN.Text, txtTIN1.Text, txtTIN2.Text, txtTIN3.Text);

            if (!string.IsNullOrWhiteSpace(hdnPickedPartyId.Text))//if the Employee is in the allowed employees
            {
                int PickedPartyId = int.Parse(hdnPickedPartyId.Text);
                party = Party.GetById(PickedPartyId);
                //Check if the new person has an employment record with another employer && end it.
                CheckIfHasAnotherEmployer(party);
                var person = party.Person;
                PartyRole partyRole = CreateEmployeePartyRole(party, today);
                CreateEmployee(partyRole, txtEmployeeIdNumber.Text, cmbPosition.SelectedItem.Text, txtSSSNumber.Text, string.Empty, string.Empty, txtPHICNumber.Text);

                Person.CreateOrUpdatePersonNames(person, PersonNameType.TitleType, txtTitle.Text, today);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.FirstNameType, txtFirstName.Text, today);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.MiddleNameType, txtMiddleName.Text, today);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.LastNameType, txtLastName.Text, today);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.NameSuffixType, txtSuffix.Text, today);
                Person.CreateOrUpdatePersonNames(person, PersonNameType.NickNameType, txtNickName.Text, today);

                Taxpayer.CreateOrUpdateTaxPayer(partyRole, concatinatedTIN);

                PartyRelationship partyRelationship = CreatePartyRelationship(partyRole, lenderPartyRole, today);
                CreateEmployment(partyRelationship, cmbEmploymentStatus.SelectedItem.Text, txtSalary.Text);

                PostalAddress newPostalAddress = new PostalAddress();
                newPostalAddress.StreetAddress = txtStreetAddress.Text;
                newPostalAddress.Barangay = txtBarangay.Text;
                newPostalAddress.City = hdnCity.Text;
                newPostalAddress.Municipality = hdnMunicipality.Text;
                newPostalAddress.Province = txtProvince.Text;
                newPostalAddress.PostalCode = txtPostalCode.Text;
                newPostalAddress.CountryId = int.Parse(cmbCountry.SelectedItem.Value);
                newPostalAddress.IsPrimary = true;
                PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, PostalAddressType.HomeAddressType, today, true);


                TelecommunicationsNumber newCellNumber = new TelecommunicationsNumber();
                newCellNumber.AreaCode = txtCellAreaCode.Text;
                newCellNumber.PhoneNumber = txtCellPhoneNumber.Text;
                newCellNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
                newCellNumber.IsPrimary = true;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newCellNumber, TelecommunicationsNumberType.PersonalMobileNumberType, today, true);

                TelecommunicationsNumber newTelePhoneNumber = new TelecommunicationsNumber();
                newTelePhoneNumber.AreaCode = txtTeleAreaCode.Text;
                newTelePhoneNumber.PhoneNumber = txtTelePhoneNumber.Text;
                newTelePhoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
                newTelePhoneNumber.IsPrimary = true;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newTelePhoneNumber, TelecommunicationsNumberType.HomePhoneNumberType, today, true);

                ElectronicAddress newEmailAddress = new ElectronicAddress();
                newEmailAddress.ElectronicAddressString = txtPrimaryEmailAddress.Text;
                newEmailAddress.ElectronicAddressType = ElectronicAddressType.PersonalEmailAddressType;
                newEmailAddress.IsPrimary = true;
                ElectronicAddress.CreateOrUpdateElectronicAddress(party, newEmailAddress, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true, today);
            } 
            else//if Employee to be added is a new Employee with no records
            {
                int selectedCountryId = int.Parse(cmbCountry.SelectedItem.Value);
                var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == selectedCountryId);

                party = CreateParty();
                //get selected gender
                GenderType genderType;
                if(radioFemale.Checked) genderType = GenderType.FemaleType;
                else genderType = GenderType.MaleType;

                Person person = CreatePerson(party, dtBirthDate.SelectedDate, genderType, hdnPersonPicture.Value.ToString());
                Person.CreatePersonName(person, today, txtTitle.Text, txtFirstName.Text, txtMiddleName.Text, txtLastName.Text, txtSuffix.Text, txtNickName.Text);
                
                PartyRole partyRole = CreateEmployeePartyRole(party, today);
                CreateEmployee(partyRole, txtEmployeeIdNumber.Text, cmbPosition.SelectedItem.Text, txtSSSNumber.Text, string.Empty, string.Empty, txtPHICNumber.Text);
                Taxpayer.CreateOrUpdateTaxPayer(partyRole, concatinatedTIN);

                PartyRelationship partyRelationship = CreatePartyRelationship(partyRole, lenderPartyRole, today);
                CreateEmployment(partyRelationship, cmbEmploymentStatus.SelectedItem.Text, txtSalary.Text);

                PostalAddress newPostalAddress = new PostalAddress();
                newPostalAddress.StreetAddress = txtStreetAddress.Text;
                newPostalAddress.Barangay = txtBarangay.Text;
                newPostalAddress.City = hdnCity.Text;
                newPostalAddress.Municipality = hdnMunicipality.Text;
                newPostalAddress.Province = txtProvince.Text;
                newPostalAddress.PostalCode = txtPostalCode.Text;
                newPostalAddress.CountryId = int.Parse(cmbCountry.SelectedItem.Value);
                newPostalAddress.IsPrimary = true;
                PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, PostalAddressType.HomeAddressType, today, true);


                TelecommunicationsNumber newCellNumber = new TelecommunicationsNumber();
                newCellNumber.AreaCode = txtCellAreaCode.Text;
                newCellNumber.PhoneNumber = txtCellPhoneNumber.Text;
                newCellNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
                newCellNumber.IsPrimary = true;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newCellNumber, TelecommunicationsNumberType.PersonalMobileNumberType, today, true);

                TelecommunicationsNumber newTelePhoneNumber = new TelecommunicationsNumber();
                newTelePhoneNumber.AreaCode = txtTeleAreaCode.Text;
                newTelePhoneNumber.PhoneNumber = txtTelePhoneNumber.Text;
                newTelePhoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
                newTelePhoneNumber.IsPrimary = true;
                TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newTelePhoneNumber, TelecommunicationsNumberType.HomePhoneNumberType, today, true);
                
                ElectronicAddress newEmailAddress = new ElectronicAddress();
                newEmailAddress.ElectronicAddressString = txtPrimaryEmailAddress.Text;
                newEmailAddress.ElectronicAddressType = ElectronicAddressType.PersonalEmailAddressType;
                newEmailAddress.IsPrimary = true;
                ElectronicAddress.CreateOrUpdateElectronicAddress(party, newEmailAddress, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true, today);
            }

            ObjectContext.SaveChanges();
        }

        public void btnBrowseAddress_Click(object sender, DirectEventArgs e)
        {
            wndAddressDetail.Show();
        }

        //WINDOW BUTTON
        public void btnDone_Click(object sender, DirectEventArgs e)
        {
            string middleInitial = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? "" : txtMiddleName.Text[0] + ".";
            txtEmployeeName.Text = StringConcatUtility.Build(" ", txtTitle.Text, 
                                                                  txtLastName.Text+",",
                                                                  txtFirstName.Text,
                                                                  middleInitial,
                                                                  txtSuffix.Text);

            //hdnTitleName.Text = txtTitle.Text;
            //hdnFirstName.Text = txtFirstName.Text;
            //hdnMiddleName.Text = txtMiddleName.Text;
            //hdnLastName.Text = txtLastName.Text;
            //hdnSuffixName.Text = txtSuffix.Text;
            //hdnNickName.Text = txtNickName.Text;

            wndNameDetailBox.Hide();
            checkName(txtEmployeeName.Text);
        }

        protected void wndAddressDetail_btnAdd_Click(object sender, DirectEventArgs e)
        {
            if (radioCity.Checked)
            {
                hdnCity.Text = txtCityOrMunicipality.Text;
                hdnMunicipality.Text = null;
            }
            else
            {
                hdnMunicipality.Text = txtCityOrMunicipality.Text;
                hdnCity.Text = null;
            }

            wndAddressDetail.Hide();
            txtPrimaryHomeAddress.Text = StringConcatUtility.Build(", ",
                                                                   txtStreetAddress.Text,
                                                                   txtBarangay.Text,
                                                                   hdnCity.Text,
                                                                   hdnMunicipality.Text,
                                                                   txtProvince.Text,
                                                                   cmbCountry.SelectedItem.Text,
                                                                   txtPostalCode.Text);
            var selectedCountryId = int.Parse(cmbCountry.SelectedItem.Value);
            var country = ObjectContext.Countries.SingleOrDefault(entity => entity.Id == selectedCountryId);
            txtCellCountryCode.Text = country.CountryTelephoneCode;
            txtTeleCountryCode.Text = country.CountryTelephoneCode;
        }

        protected void onUpload_Click(object sender, DirectEventArgs e)
        {
            string msg = "";
            // Check that a file is actually being submitted.
            if (flupCustomerImage.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
            }

            else
            {
                // Check the extension.
                string extension = Path.GetExtension(flupCustomerImage.PostedFile.FileName);
                switch (extension.ToLower())
                {
                    case ".bmp":
                    case ".gif":
                    case ".jpg":
                    case ".png":
                    case ".tiff":
                    case ".jpeg":
                    case ".tif":
                        break;
                    default:
                        X.MessageBox.Alert("Alert", "This file type is not allowed.").Show();
                        return;
                }
                // Using this code, the saved file will retain its original
                // file name when it's placed on the server.
                string serverFileName = Path.GetFileName(flupCustomerImage.PostedFile.FileName);
                string fullUploadPath = Path.Combine(uploadDirectory, serverFileName);
                string file = "";
                string fileName = "";
                try
                {
                    if (File.Exists(fullUploadPath))
                    {
                        file = Path.GetFileNameWithoutExtension(serverFileName);
                        file += DateTime.Now.ToString("M-dd-yyyy hhmmss.ff") + Path.GetExtension(serverFileName);
                        fileName = file;
                        file = Path.Combine(uploadDirectory, file);
                    }
                    else
                    {
                        file = fullUploadPath;
                        fileName = serverFileName;
                    }

                    flupCustomerImage.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Images/" + flupCustomerImage.PostedFile.FileName;
                imgPersonPicture.ImageUrl = imageFilename;
                this.hdnPersonPicture.Value = imageFilename;
            }
        }

        //RADIO BUTTONS
        //protected void rgMunicipalityOrCity_DirectChange(object sender, DirectEventArgs e)
        //{
        //    if (radioCity.Checked)
        //    {
        //        txtProvince.AllowBlank = true;
        //    }
        //    else if (radioMunicipality.Checked)
        //    {
        //        txtProvince.AllowBlank = false;
        //    }
        //}

        //
        public void checkName(string name)
        {
            var sameNameEmployee =ObjectContext.EmployeeViewLists.SingleOrDefault(entity => entity.Name == name);
            
            //
            if (sameNameEmployee != null)
            {
                X.Msg.Confirm("Message", "Employee record with the same name already exists in the list. Do you want to create another employee record with the same name?",
                        new JFunction("Employee.AddSameName(result, \"" + sameNameEmployee.EmployeeId + "\");", "result")).Show();
            }
         //   var sameNameNotEmployee = ObjectContext.all

        }

        //DIRECT METHODS
        [DirectMethod(Namespace = "Employee")]
        public void AddSameName(string result, string name)
        {
            if (result == "yes")
            {
                hdnEmployee.Text = null;
            }
            else
            {

            }

        }

        [DirectMethod]
        public void FillPersonInformationFields()
        {
            int partyId = int.Parse(hdnPickedPartyId.Text);
            var party = ObjectContext.Parties.SingleOrDefault(entity => entity.Id == partyId);
            var person = party.Person;

            var Name = party.Name;
            txtEmployeeName.Text = Name;

            if (person.TitleString != null) txtTitle.Text = person.TitleString;
            if (person.FirstNameString != null) txtFirstName.Text = person.FirstNameString;
            if (person.MiddleNameString != null) txtMiddleName.Text = person.MiddleNameString;
            if (person.LastNameString != null) txtLastName.Text = person.LastNameString;
            if (person.NameSuffixString != null) txtSuffix.Text = person.NameSuffixString;
            if (person.NickNameString != null) txtNickName.Text = person.NickNameString;

            var gender = party.Person.GenderType;
            if (gender == GenderType.MaleType) radioMale.Checked = true;
            else if (gender == GenderType.FemaleType) radioFemale.Checked = true;

            var birthDate = party.Person.Birthdate;
            if(birthDate != null) dtBirthDate.SelectedDate = (DateTime) birthDate;

            
            if(person.ImageFilename != null) imgPersonPicture.ImageUrl = person.ImageFilename;
            
            //var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id 
            //var taxpayer = ObjectContext.Taxpayers.SingleOrDefault(entity => entity.PartyRoleId == 1);

            var postalAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, true);

            if (postalAddress != null)
            {
                txtPrimaryHomeAddress.Text = StringConcatUtility.Build(", ",
                                                                       postalAddress.StreetAddress,
                                                                       postalAddress.Barangay,
                                                                       postalAddress.Municipality,
                                                                       postalAddress.City,
                                                                       postalAddress.Province,
                                                                       postalAddress.Country.Name,
                                                                       postalAddress.PostalCode);

                txtTeleCountryCode.Text = postalAddress.Country.CountryTelephoneCode;
                txtCellCountryCode.Text = postalAddress.Country.CountryTelephoneCode;

                txtStreetAddress.Text = postalAddress.StreetAddress;
                txtBarangay.Text = postalAddress.Barangay;
                if (postalAddress.Municipality == null || string.IsNullOrEmpty(postalAddress.Municipality))
                {
                    txtCityOrMunicipality.Text = postalAddress.City;
                    hdnCity.Text = postalAddress.City;
                    radioCity.Checked = true;
                }
                else if (postalAddress.City == null || string.IsNullOrEmpty(postalAddress.City))
                {
                    txtCityOrMunicipality.Text = postalAddress.Municipality;
                    hdnMunicipality.Text = postalAddress.Municipality;
                    radioMunicipality.Checked = true;
                }
                txtProvince.Text = postalAddress.Province;
                cmbCountry.SelectedItem.Value = postalAddress.Country.Id.ToString();
                txtPostalCode.Text = postalAddress.PostalCode;
            }

            var addressAsHomePhone = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.HomePhoneNumberType, true);
            if (addressAsHomePhone != null)
            {
                txtTeleAreaCode.Text = addressAsHomePhone.AreaCode;
                txtTelePhoneNumber.Text = addressAsHomePhone.PhoneNumber;
            }

            
            var addressAsMobile = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.PersonalMobileNumberType, true);
            if (addressAsMobile != null)
            {
                txtCellAreaCode.Text = addressAsMobile.AreaCode;
                txtCellPhoneNumber.Text = addressAsMobile.PhoneNumber;
            }

            var addressAsEmail = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true);
            if (addressAsEmail != null)
            {
                txtPrimaryEmailAddress.Text = addressAsEmail.ElectronicAddressString;
            }
        }

        [DirectMethod(ShowMask=true, Msg="Checking employee name...")]
        public int CheckEmployeeName()
        {
            var today = DateTime.Now;
            Party party = new Party();
            Person person = new Person();

            PersonName employeeTitleName = new PersonName();
            PersonName employeeFirstName = new PersonName();
            PersonName employeeMiddleName = new PersonName();
            PersonName employeeLastName = new PersonName();
            PersonName employeeSuffixName = new PersonName();
            PersonName employeeNickName = new PersonName();

            party.PartyType = PartyType.PersonType;
            person.Party = party;

            employeeTitleName.Person = person;
            employeeTitleName.PersonNameType = PersonNameType.TitleType;
            employeeTitleName.Name = txtTitle.Text;
            employeeTitleName.EffectiveDate = today;

            employeeFirstName.Person = person;
            employeeFirstName.PersonNameType = PersonNameType.FirstNameType;
            employeeFirstName.Name = txtFirstName.Text;
            employeeFirstName.EffectiveDate = today;

            employeeMiddleName.Person = person;
            employeeMiddleName.PersonNameType = PersonNameType.MiddleNameType;
            employeeMiddleName.Name = txtMiddleName.Text;
            employeeMiddleName.EffectiveDate = today;

            employeeLastName.Person = person;
            employeeLastName.PersonNameType = PersonNameType.LastNameType;
            employeeLastName.Name = txtLastName.Text;
            employeeLastName.EffectiveDate = today;

            employeeSuffixName.Person = person;
            employeeSuffixName.PersonNameType = PersonNameType.NameSuffixType;
            employeeSuffixName.Name = txtSuffix.Text;
            employeeSuffixName.EffectiveDate = today;

            employeeNickName.Person = person;
            employeeNickName.PersonNameType = PersonNameType.NickNameType;
            employeeNickName.Name = txtNickName.Text;
            employeeNickName.EffectiveDate = today;

            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = RoleType.EmployeeType.Id;

            var res = Person.CheckEmployeePersonName(partyRole);

            int result = 0;
            //var SameEmployeeName = ObjectContext.EmployeeViewLists.SingleOrDefault(entity => entity.Name.Contains(txtEmployeeName.Text));
            if (res != null) //Exists in the Employee Record
            {
                int partyRoleId = res.Id;
                //var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
                hdnEmployeeID.Value = partyRoleId;
                //hdnPickedPartyId.Text = partyRole.PartyId.ToString();
                result = 1;
            }
            else
            {
                var SameAllowedEmployeeName = Person.CheckAllowedEmployeeName(partyRole);
                if (SameAllowedEmployeeName != null)
                {
                    hdnPickedPartyId.Text = SameAllowedEmployeeName.PartyId.ToString();
                    result = 2;
                }
            }

            return result;
        }
    }

    //MODELS
    public class PersonNameModel
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Nickname { get; set; }
    }
}