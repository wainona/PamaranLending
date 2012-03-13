using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using LendingApplication.Applications;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using System.IO;


namespace LendingApplication.Applications.CustomerUseCases
{
    public partial class ManageCustomer : ActivityPageBase
    {
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

        public string PersonName
        {
            get
            {
                return hiddenName.Value.ToString();
            }
        }

        private string uploadDirectory;

        protected string imageFilename;

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            //dataSource.Name = "";

            int id = int.Parse(this.RecordID.Text);
            e.Total = dataSource.Count;
            dataSource.ID = id;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public void RefreshClassification()
        {
            var classTypes = ClassificationType.All();

            CustomerClassStore.DataSource = classTypes;
            CustomerClassStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Images");
            hiddenName.Value = txtPersonName.Text;
            savePersonNameDetails();

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {

                CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
                int ageLimit = SystemSetting.AgeLimitOfBorrower;

                datResidentSince.MaxDate = DateTime.Now;
                datDateIssued.MaxDate = DateTime.Now;
                datDateIssued.DisabledDays = ApplicationSettings.DisabledDays;
                datBirthdate.MaxDate = DateTime.Now.AddYears(-ageLimit);
                datBirthdate.SelectedDate = DateTime.Now.AddYears(-ageLimit);
                datSpouseBirthdate.MaxDate = DateTime.Now.AddYears(-ageLimit);

                PersonImageFile.ImageUrl = imageFilename + "../../../Resources/images/noimage.jpg";

                this.CustomerID.Value = -1;
                this.RecordID.Value = -1;
                this.hiddenNewEmployerID.Value = -1;
                this.hiddenEmployerID.Value = -1;

                //initialize the combobox store
                var maritalStat = MaritalStatusType.All();
                var educAttainment = EducAttainmentType.All();
                var homeOwner = HomeOwnershipType.All();
                var idType = IdentificationType.All();
                var soi = SourceOfIncome.All();
                var countries = Country.All();
                var nationalities = NationalityType.All();
                var classTypes = ClassificationType.All();
                var customerTypes = CustomerCategoryType.All();

                //bind the datasources
                MaritalStatusStore.DataSource = maritalStat;
                MaritalStatusStore.DataBind();

                EducAttainmentStore.DataSource = educAttainment;
                EducAttainmentStore.DataBind();

                HomeOwnershipStore.DataSource = homeOwner;
                HomeOwnershipStore.DataBind();

                IDTypeStore.DataSource = idType;
                IDTypeStore.DataBind();

                IDTypeStore2.DataSource = idType;
                IDTypeStore2.DataBind();

                NationalityStore.DataSource = nationalities;
                NationalityStore.DataBind();

                SourceOfIncomeStore.DataSource = soi;
                SourceOfIncomeStore.DataBind();

                CountryStore.DataSource = countries;
                CountryStore.DataBind();

                CustomerClassStore.DataSource = classTypes;
                CustomerClassStore.DataBind();

                CustomerTypeStore.DataSource = customerTypes;
                CustomerTypeStore.DataBind();

                SetDefaults();
            }
        }

        //Set default values
        protected void SetDefaults()
        {
            //fill default values
            cmbCustomerType.SelectedItem.Value = CustomerCategoryType.TeacherType.Id.ToString();
            cmbMaritalStatus.SelectedItem.Value = MaritalStatusType.SingleType.Id.ToString();
            nfNumberOfDependents.Text = "0";
            cmbEducationalAttainment.SelectedItem.Value = EducAttainmentType.CollegeGraduateType.Id.ToString();
            cmbHomeOwnership.SelectedItem.Value = HomeOwnershipType.OwnedType.Id.ToString();
            cmbNationality.SelectedItem.Value = NationalityTye.FilipinoType.Id.ToString();

            //birthplace
            txtCityOrMunicipalityB1.Text = ApplicationSettings.DefaultCity;
            txtPostalCodeB1.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountryB1.SelectedItem.Value = Country.Philippines.Id.ToString();

            //area code defaults
            txtCellAreaCode.Text = ApplicationSettings.DefaultAreaCode;
            txtTelAreaCode.Text = ApplicationSettings.DefaultAreaCode;

            //primary home address
            txtCityOrMunicipalityA1.Text = ApplicationSettings.DefaultCity;
            txtPostalCodeA1.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountryA1.SelectedItem.Value = Country.Philippines.Id.ToString();

            //secondary home address
            txtCityOrMunicipalityA2.Text = ApplicationSettings.DefaultCity;
            txtPostalCodeA2.Text = ApplicationSettings.DefaultPostalCode;
            cmbCountryA2.SelectedItem.Value = Country.Philippines.Id.ToString();

            
        }

        protected void onUpload_Click(object sender, DirectEventArgs e)
        {
            string msg = "";

            // Check that a file is actually being submitted.
            if (flupCustomerImage.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
            }
            else //else if file exists
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
                    //check if file already exists
                    if(File.Exists(fullUploadPath)){
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

                    //save file
                    flupCustomerImage.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Images/" + flupCustomerImage.PostedFile.FileName;
                PersonImageFile.ImageUrl = imageFilename;
                this.hiddenImageUrl.Value = imageFilename;
            }
        }

        #region Obsolete Methods
        //IGNORE THIS
        protected void checkCustomerName(object sender, RemoteValidationEventArgs e)
        {
            TextField txtName = (TextField)sender;
            var personName = txtName.Text;
            //bool res = true;
            using (var context = new FinancialEntities())
            {
                //check if input name is equal to existing person name
                var sameName = context.CustomerViewLists.FirstOrDefault(entity => entity.Name == personName);
                //InitialDatabaseValueChecker.ThrowIfNull<CustomersViewList>(sameName);

                if(sameName != null){
                    X.Msg.Confirm("Message", "A customer record with the same name already exists. Do you want to create another customer record with the same name?", new JFunction("Customer.AddSameCustomer(result);", "result")).Show();
                }
                else
                {
                    //int partyId = int.Parse(this.CustomerID.Text);
                    //int partyId = 1;
                    //check if input name 
                    var sameNameAndParty = context.AllowedCustomersViews.FirstOrDefault(entity => entity.Owner == personName);
                    if (sameNameAndParty != null)
                    {
                        X.Msg.Confirm("Message", "A party record with the same name already exists in the pick list. Do you want to create another party record with the same name?", new JFunction("Customer.AddSameParty(result);", "result")).Show();
                    }
                    //res = false;
                }

            }
            e.Success = true;
            //System.Threading.Thread.Sleep(2000);
        }

        //IGNORE THIS
        protected void checkCustomerNames()
        {
            var personName = txtPersonName.Text;
            //bool res = true;
            using (var context = new FinancialEntities())
            {
                //check if input name is equal to existing person name
                var sameName = context.CustomerViewLists.FirstOrDefault(entity => entity.Name == personName);
                //InitialDatabaseValueChecker.ThrowIfNull<CustomersViewList>(sameName);

                if (sameName != null)
                {
                    X.Msg.Confirm("Message", "A customer record with the same name already exists. Do you want to create another customer record with the same name?", new JFunction("Customer.AddSameCustomer(result);", "result")).Show();
                }
                else
                {
                    //int partyId = int.Parse(this.CustomerID.Text);
                    //int partyId = 1;
                    var sameNameAndParty = context.AllowedCustomersViews.FirstOrDefault(entity => entity.Owner == personName);
                    if (sameNameAndParty != null)
                    {
                        X.Msg.Confirm("Message", "A party record with the same name already exists in the pick list. Do you want to create another party record with the same name?", new JFunction("Customer.AddSameParty(result);", "result")).Show();
                    }
                    //res = false;
                }

            }
            //e.Success = true;
            //System.Threading.Thread.Sleep(2000);
            wndPersonNameDetail.Hide();
        }
        #endregion

        //FINAL Customer Name Checking Method
        [DirectMethod(ShowMask = true, Msg = "Checking customer name...")]
        public int checkCustomerName(string name)
        {
            int result = 0;
            var names = name;
            var personName = this.PersonName;
            //bool res = true;
            Party employeeParty = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.Party;

            var today = DateTime.Now;

            //initialize new Party, Person and PersonName
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

            //Title Name
            customerTName.Person = person;
            customerTName.PersonNameType = PersonNameType.TitleType;
            customerTName.Name = txtPersonTitle.Text;
            customerTName.EffectiveDate = today;

            //First Name
            customerFName.Person = person;
            customerFName.PersonNameType = PersonNameType.FirstNameType;
            customerFName.Name = txtPersonFirstName.Text;
            customerFName.EffectiveDate = today;

            //Last Name
            customerLName.Person = person;
            customerLName.PersonNameType = PersonNameType.LastNameType;
            customerLName.Name = txtPersonLastName.Text;
            customerLName.EffectiveDate = today;

            //Mother's Maiden Name
            customerMMName.Person = person;
            customerMMName.PersonNameType = PersonNameType.MothersMaidenNameType;
            customerMMName.Name = txtPersonMothersMaidenName.Text;
            customerMMName.EffectiveDate = today;

            //Middle Name
            customerMName.Person = person;
            customerMName.PersonNameType = PersonNameType.MiddleNameType;
            customerMName.Name = txtPersonMiddleName.Text;
            customerMName.EffectiveDate = today;

            //Name Suffix
            customerNaName.Person = person;
            customerNaName.PersonNameType = PersonNameType.NameSuffixType;
            customerNaName.Name = txtPersonNameSuffix.Text;
            customerNaName.EffectiveDate = today;

            //Nickname
            customerNiName.Person = person;
            customerNiName.PersonNameType = PersonNameType.NickNameType;
            customerNiName.Name = txtPersonNickName.Text;
            customerNiName.EffectiveDate = today;

            //create new customer partyrole
            PartyRole partyRole = new PartyRole();
            partyRole.Party = party;
            partyRole.RoleTypeId = RoleType.CustomerType.Id;

            //check if input name is equal to existing customer person name
            var sameName = Person.CheckCustomerPersonName(party);

            if (sameName != null)
            {
                hiddenID.Value = sameName.Id;
                result = 1;
            }
            else
            {
                //check if input name is equal to an existing party person name
                var sameNameAndParty = Person.CheckPersonNameParty(party, employeeParty);
                    
                if (sameNameAndParty != null)
                {
                    hiddenID.Value = sameNameAndParty.Id;
                    result = 2;
                }
            }


            return result;
        }

        //Age checking for customer birthdate
        protected void checkAge(object sender, DirectEventArgs e)
        {
            DateField field = (DateField)sender;

            int now = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

            int dob = int.Parse(field.SelectedDate.ToString("yyyyMMdd"));
            string dif = (now - dob).ToString();
            string age = "1";
            int ageLimit = SystemSetting.AgeLimitOfBorrower;

            if (dif.Length >= 4)
                age = dif.Substring(0, dif.Length - 4);

            if (int.Parse(age) < ageLimit)
            {
                X.Msg.Alert("Status", "Age must be greater than or equal to 18.").Show();
                datBirthdate.Text = "";
            }
            else
            {

            }
        }

        //Age checking for spouse birthdate
        protected void checkAge2(object sender, DirectEventArgs e)
        {
            DateField field = (DateField)sender;

            int now = int.Parse(DateTime.Today.ToString("yyyyMMdd"));

            int dob = int.Parse(field.SelectedDate.ToString("yyyyMMdd"));
            string dif = (now - dob).ToString();
            string age = "1";
            int ageLimit = SystemSetting.AgeLimitOfBorrower;

            if (dif.Length >= 4)
                age = dif.Substring(0, dif.Length - 4);

            if (int.Parse(age) < ageLimit)
            {
                //e.Success = false;
                //e.ErrorMessage = "Too young to apply for a loan.";
                X.Msg.Alert("Status", "Age must be greater than or equal to 18.").Show();
                datSpouseBirthdate.Text = "";
            }
            else
            {
                //e.Success = true;
            }
            //e.Success = false;
            //e.ErrorMessage = "Error";
        }

        protected void onMaritalStatusChange(object sender, DirectEventArgs e)
        {
            if (cmbMaritalStatus.SelectedItem.Text == MaritalStatusType.MarriedType.Name)
            {
                int ageLimit = SystemSetting.AgeLimitOfBorrower;

                txtSpouseName.AllowBlank = false;
                datSpouseBirthdate.SelectedDate = DateTime.Now.AddYears(-ageLimit);
                SpouseInformationPanel.Disabled = false;
            }
            else
            {
                txtSpouseName.AllowBlank = true;
                datSpouseBirthdate.Text = "";
                SpouseInformationPanel.Disabled = true;
            }
        }

        //event method for primary home address municipality/city change
        protected void onMunicipalityCityChange(object sender, DirectEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMunicipalityD1.Text) && string.IsNullOrWhiteSpace(txtCityD1.Text))
            {
                txtMunicipalityD1.AllowBlank = false;
                txtCityD1.AllowBlank = true;
                txtProvinceD1.AllowBlank = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtCityD1.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD1.Text))
            {
                txtCityD1.AllowBlank = false;
                txtMunicipalityD1.AllowBlank = true;
                txtProvinceD1.AllowBlank = true;
            }
            else if((string.IsNullOrWhiteSpace(txtCityD1.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD1.Text)) 
                        || (!string.IsNullOrWhiteSpace(txtCityD1.Text) && !string.IsNullOrWhiteSpace(txtMunicipalityD1.Text)))
            {
                //txtMunicipalityD1.Text = null;
                //txtCityD1.Text = null;
                txtCityD1.AllowBlank = true;
                txtMunicipalityD1.AllowBlank = true;
                txtProvinceD1.AllowBlank = true;
                X.Msg.Alert("Status", "Please specify either City or Municipality.").Show();
                txtMunicipalityD1.Focus();
                txtCityD1.Focus();
            }

        }

        //event listener method for birthplace municipality/city change
        protected void onMunicipalityCityChangeB(object sender, DirectEventArgs e)
        {
            
            if (!string.IsNullOrWhiteSpace(txtMunicipalityB.Text) && string.IsNullOrWhiteSpace(txtCityB.Text))
            {
                txtMunicipalityB.AllowBlank = false;
                txtCityB.AllowBlank = true;
                txtProvinceB.AllowBlank = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtCityB.Text) && string.IsNullOrWhiteSpace(txtMunicipalityB.Text))
            {
                txtCityB.AllowBlank = false;
                txtMunicipalityB.AllowBlank = true;
                txtProvinceB.AllowBlank = true;
            }
            else if ((string.IsNullOrWhiteSpace(txtCityB.Text) && string.IsNullOrWhiteSpace(txtMunicipalityB.Text))
                        || (!string.IsNullOrWhiteSpace(txtCityB.Text) && !string.IsNullOrWhiteSpace(txtMunicipalityB.Text)))
            {
                //txtMunicipalityB.Text = null;
                //txtCityB.Text = null;
                txtMunicipalityB.AllowBlank = true;
                txtCityB.AllowBlank = true;
                txtProvinceB.AllowBlank = true;
                X.Msg.Alert("Status", "Please specify either City or Municipality.").Show();
                txtMunicipalityB.Focus();
                txtCityB.Focus();
                btnDoneBirthPlaceAddDetail.Disabled = true;
            }
        }

        //event listener method for secondary home address municipality/city change
        protected void onMunicipalityCityChange2(object sender, DirectEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtMunicipalityD2.Text) && string.IsNullOrWhiteSpace(txtCityD2.Text))
            {
                txtMunicipalityD2.AllowBlank = false;
                txtCityD2.AllowBlank = true;
                txtProvinceD2.AllowBlank = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtCityD2.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD2.Text))
            {
                txtCityD2.AllowBlank = false;
                txtMunicipalityD2.AllowBlank = true;
                txtProvinceD2.AllowBlank = true;
            }
            else if ((string.IsNullOrWhiteSpace(txtCityD2.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD2.Text))
                        || (!string.IsNullOrWhiteSpace(txtCityD2.Text) && !string.IsNullOrWhiteSpace(txtMunicipalityD2.Text)))
            {
                //txtMunicipalityD2.Text = null;
                //txtCityD2.Text = null;
                txtMunicipalityD2.AllowBlank = true;
                txtCityD2.AllowBlank = true;
                txtProvinceD2.AllowBlank = true;
                X.Msg.Alert("Status", "Please specify either City or Municipality.").Show();
                txtMunicipalityD2.Focus();
                txtCityD2.Focus();
                btnDoneAddressDetail2.Disabled = true;
            }

        }

        //For Customer Record with Same Name
        [DirectMethod(Namespace = "Customer")]
        public void AddSameCustomer(string result)
        {
            if (result == "no")
            {
                hiddenUsesExistRecord.Text = "no";
                txtPersonName.Text = "";
                wndPersonNameDetail.Show();
            }
            else hiddenUsesExistRecord.Text = "yes";

            wndPersonNameDetail.Hide();
        }

        //For Customer with same name as an existing party 
        [DirectMethod(Namespace = "Customer")]
        public void AddSameName(string result)
        {
            if (result == "yes")
            {
                hiddenUsesExistRecord.Text = "yes";
            }
            else
            {
                hiddenUsesExistRecord.Text = "no";
                txtPersonName.Text = "";
                wndPersonNameDetail.Show();
            }

            wndPersonNameDetail.Hide();
        }

        private void ShowOtherSourcesOfIncome(FinancialEntities context, int customerId)
        {
            var query = from c in context.Customers
                        join csoi in context.CustomerSourceOfIncomes on c.PartyRoleId equals csoi.PartyRoleId
                        join soi in context.SourceOfIncomes on csoi.SourceOfIncomeId equals soi.Id
                        where csoi.EndDate == null && c.PartyRoleId == customerId
                        select new SourcesOfIncomeModel()
                        {
                            Id = c.PartyRoleId,
                            SourceOfIncome = soi.Name,
                            Amount = csoi.Income
                        };

            PageGridPanelStore.DataSource = query.ToList();
            PageGridPanelStore.DataBind();
        }

        private class SourcesOfIncomeModel
        {
            public decimal Amount { get; set; }
            public int Id { get; set; }
            public string SourceOfIncome { get; set; }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
                form.IsNew = true;
                if (this.CustomerID.Text == "-1")
                    form.NewCustomerId = -1;
                else
                    form.NewCustomerId = int.Parse(this.CustomerID.Value.ToString());

                //if customer uses existing party record
                if(hiddenUsesExistRecord.Value.ToString() == "yes"){
                    form.PersonPartyId = int.Parse(hiddenID.Value.ToString());
                }

                //employer ids
                form.NewEmployerId = int.Parse(hiddenNewEmployerID.Text);
                form.EmployerId = int.Parse(hiddenEmployerID.Text);
                if (!string.IsNullOrWhiteSpace(this.hiddenImageUrl.Text))
                    form.ImgUrl = this.hiddenImageUrl.Text;

                //fill in customer name
                if (string.IsNullOrWhiteSpace(txtPersonTitle.Text) == false)
                    form.CustomerName.Title = this.txtPersonTitle.Text;
                form.CustomerName.FirstName = txtPersonFirstName.Text;
                form.CustomerName.LastName = txtPersonLastName.Text;
                if (!string.IsNullOrWhiteSpace(txtPersonMiddleName.Text))
                    form.CustomerName.MiddleName = txtPersonMiddleName.Text;
                if (!string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text))
                    form.CustomerName.NameSuffix = txtPersonNameSuffix.Text;
                if (!string.IsNullOrWhiteSpace(txtNickNameP.Text))
                    form.CustomerName.NickName = txtNickNameP.Text;
                form.CustomerName.MothersMaidenName = txtMothersMaidenName.Text;

                //educational attainment, district, customer category, and customer status
                form.EducationalAttainmentId = int.Parse(cmbEducationalAttainment.SelectedItem.Value);
                form.DistrictClassificationTypeId = int.Parse(hiddenClassificationTypeID.Text);
                form.CustomerStatus = txtCustomerStatus.Text;
                form.CustomerCategoryTypeId = int.Parse(cmbCustomerType.SelectedItem.Value);

                //gender
                if (rdFemale.Checked)
                {
                    form.GenderTypeId = GenderType.FemaleType.Id;
                }
                else
                {
                    form.GenderTypeId = GenderType.MaleType.Id;
                }

                //birthdate and birthplace
                form.Birthdate = datBirthdate.SelectedDate;
                if (!string.IsNullOrWhiteSpace(txtBirthPlace.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtStreetAdd3.Text))
                        form.Birthplace.StreetAddress = txtStreetAdd3.Text;
                    if (!string.IsNullOrWhiteSpace(txtBarangay3.Text))
                        form.Birthplace.Barangay = txtBarangay3.Text;
                    if (!string.IsNullOrWhiteSpace(txtCity3.Text))
                        form.Birthplace.City = txtCity3.Text;
                    if (!string.IsNullOrWhiteSpace(txtMunicipality3.Text))
                        form.Birthplace.Municipality = txtMunicipality3.Text;
                    if (!string.IsNullOrWhiteSpace(txtProvince3.Text))
                        form.Birthplace.Province = txtProvince3.Text;
                    if (!string.IsNullOrWhiteSpace(txtPostal3.Text))
                        form.Birthplace.PostalCode = txtPostal3.Text;
                    form.Birthplace.IsPrimary = true;
                    if (!string.IsNullOrWhiteSpace(txtState3.Text))
                        form.Birthplace.State = txtState3.Text;
                    if (!string.IsNullOrWhiteSpace(cmbCountryB1.SelectedItem.Value))
                        form.Birthplace.CountryId = int.Parse(cmbCountryB1.SelectedItem.Value);
                }

                //martital status, number of dependents
                // home ownership, resident since and nationality
                form.MaritalStatusId = int.Parse(cmbMaritalStatus.SelectedItem.Value);
                form.NumberOfDependents = int.Parse(nfNumberOfDependents.Text);
                form.HomeOwnershipId = int.Parse(cmbHomeOwnership.SelectedItem.Value);
                form.ResidentSince = datResidentSince.SelectedDate;
                if(cmbNationality.SelectedItem.Value != null)
                    form.NationalityId = int.Parse(cmbNationality.SelectedItem.Value);

                //TIN
                if (txtTin1.Text != "" && txtTin2.Text != "" && txtTin3.Text != "")
                {
                    form.Tin = txtTin1.Text + "-" + txtTin2.Text + "-" + txtTin3.Text + "-";

                    if (txtTin4.Text == "")
                        form.Tin += "000";
                    else
                        form.Tin += txtTin4.Text;
                }

                //CTC
                if (!string.IsNullOrWhiteSpace(txtCtc.Text))
                {
                    form.CtcNumber = txtCtc.Text;
                    form.DateIssued = datDateIssued.SelectedDate;
                    form.PlaceIssued = txtPlaceIssued.Text;
                }

                //credit limit
                if (!string.IsNullOrWhiteSpace(txtCreditLimit.Text))
                    form.CreditLimit = Decimal.Parse(txtCreditLimit.Text);

                //primary home address
                form.CustomerPrimaryHomeAddress.StreetAddress = txtStreetAdd1.Text;
                form.CustomerPrimaryHomeAddress.State = txtState1.Text;
                form.CustomerPrimaryHomeAddress.Province = txtProvince1.Text;
                form.CustomerPrimaryHomeAddress.PostalCode = txtPostal1.Text;
                form.CustomerPrimaryHomeAddress.Municipality = txtMunicipality1.Text;
                if (!string.IsNullOrEmpty(cmbCountryA1.SelectedItem.Value))
                    form.CustomerPrimaryHomeAddress.CountryId = int.Parse(cmbCountryA1.SelectedItem.Value);
                form.CustomerPrimaryHomeAddress.City = txtCity1.Text;
                form.CustomerPrimaryHomeAddress.Barangay = txtBarangay1.Text;
                form.CustomerPrimaryHomeAddress.IsPrimary = true;

                //secondary home address
                if (!string.IsNullOrWhiteSpace(txtSecondaryHomeAddress.Text))
                {
                    form.CustomerSecondaryHomeAddress.StreetAddress = txtStreetAdd2.Text;
                    form.CustomerSecondaryHomeAddress.State = txtState2.Text;
                    form.CustomerSecondaryHomeAddress.Province = txtProvince2.Text;
                    form.CustomerSecondaryHomeAddress.PostalCode = txtPostal2.Text;
                    form.CustomerSecondaryHomeAddress.Municipality = txtMunicipality2.Text;
                    if (!string.IsNullOrEmpty(cmbCountryA2.SelectedItem.Value))
                        form.CustomerSecondaryHomeAddress.CountryId = int.Parse(cmbCountryA2.SelectedItem.Value);
                    form.CustomerSecondaryHomeAddress.City = txtCity2.Text;
                    form.CustomerSecondaryHomeAddress.Barangay = txtBarangay2.Text;
                    form.CustomerSecondaryHomeAddress.IsPrimary = true;
                }

                //contact information
                if (!string.IsNullOrWhiteSpace(txtCellAreaCode.Text) && !string.IsNullOrWhiteSpace(txtCellPhoneNumber.Text))
                {
                    form.CustomerCellNumber.AreaCode = txtCellAreaCode.Text;
                    form.CustomerCellNumber.PhoneNumber = txtCellPhoneNumber.Text;
                    form.CustomerCellNumber.IsPrimary = true;
                }

                if (!string.IsNullOrWhiteSpace(txtTelAreaCode.Text) && !string.IsNullOrWhiteSpace(txtTelPhoneNumber.Text))
                {
                    form.CustomerTelephoneNumber.AreaCode = txtTelAreaCode.Text;
                    form.CustomerTelephoneNumber.PhoneNumber = txtTelPhoneNumber.Text;
                    form.CustomerTelephoneNumber.IsPrimary = true;
                }

                form.CustomerPrimaryEmailAddress.EletronicAddressString = txtPrimaryEmailAddress.Text;
                form.CustomerPrimaryEmailAddress.IsPrimary = true;

                form.CustomerSecondaryEmailAddress.EletronicAddressString = txtSecondaryEmailAddress.Text;
                form.CustomerSecondaryEmailAddress.IsPrimary = false;

                //IDs
                if (!string.IsNullOrEmpty(cmbIdType1.SelectedItem.Value))
                {
                    form.IdType1Id = int.Parse(cmbIdType1.SelectedItem.Value);
                    form.IdNumber1 = txtIDNumber1.Text;
                }

                if (!string.IsNullOrEmpty(cmbIdType2.SelectedItem.Value))
                {
                    form.IdType2Id = int.Parse(cmbIdType2.SelectedItem.Value);
                    form.IdNumber2 = txtIDNumber2.Text;
                }

               //// employment information
                //form.EmployerId = int.Parse(hiddenEmployerID.Text);
                //form.NewEmployerId = int.Parse(hiddenNewEmployerID.Text);
                form.EmployeeIdNumber = txtEmpIDNumber.Text;
                form.EmployeePosition = txtEmpPosition.Text;
                form.EmploymentStatus = txtEmploymentStatus.Text;
                form.Salary = txtSalary.Text;
                form.SssNumber = txtSssNumber.Text;
                form.GsisNumber = txtGsisNumber.Text;
                form.OwaNumber = txtOWANumber.Text;
                form.PhicNumber = txtPhicNumber.Text;

                //spouse information
                form.SpouseName.Title = txtSpouseTitle.Text;
                form.SpouseName.NameSuffix = txtSpouseNameSuffix.Text;
                form.SpouseName.NickName = txtSpouseNickName.Text;
                form.SpouseName.MothersMaidenName = txtSpouseMothersMaidenName.Text;
                form.SpouseName.MiddleName = txtSpouseMiddleName.Text;
                form.SpouseName.LastName = txtSpouseLastName.Text;
                form.SpouseName.FirstName = txtSpouseFirstName.Text;

                if (datSpouseBirthdate.SelectedValue != null)
                    form.SpouseBirthDate = datSpouseBirthdate.SelectedDate;

                //remarks
                form.Remarks = txtRemarks.Text;

                //save
                form.PrepareForSave();
                
            }
        }

        protected void btnSaveSourceIncome_Click(object sender, DirectEventArgs e)
        {
            int sourceOfIncomeId = int.Parse(cmbSourceOfIncome.SelectedItem.Value);
            decimal amount = Decimal.Parse(txtAmount.Text);

            CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
            //add source of income
            if (string.IsNullOrWhiteSpace(hiddenRandomKey.Text))
            {
                if (amount == 0)
                {
                    X.MessageBox.Alert("Status", "Amount should be greater than 0.").Show();
                    manageSourcesOfIncome.Show();
                }
                else if (amount > 0)
                {

                        var res = form.AddSourceOfIncome(new CustomerSourceOfIncomeModel(sourceOfIncomeId, amount));
                        if (res)
                            X.MessageBox.Alert("Status", "Source of income added.").Show();
                        else
                        {
                            X.MessageBox.Alert("Error", "Source of income cannot be added.").Show();
                            manageSourcesOfIncome.Show();
                        }

                }
            }
            else //edit source of income
            {
                if (amount == 0)
                {
                    X.MessageBox.Alert("Error", "Amount should be greater than 0.").Show();
                    manageSourcesOfIncome.Show();
                }
                else if (amount > 0)
                {
                    CustomerSourceOfIncomeModel model = form.GetSourceOfIncome(this.hiddenRandomKey.Text);
                    //if updated source of income id is the same as the old source of income id 
                    if (model.SourceOfIncomeId == sourceOfIncomeId)
                    {
                        model.Amount = amount;
                        model.SourceOfIncomeId = sourceOfIncomeId;
                        SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(sourceOfIncomeId);
                        model.CustomerSourceOfIncome = sourceOfIncome.Name;
                        model.MarkEdited();
                        X.MessageBox.Alert("Status", "Source of income updated.").Show();
                    }
                    else // else if the updated source of income is not the same as the old source of income
                    {
                        //if selected source of income is not yet on the existing list of customer source of income
                        if (!form.SourcesOfIncomeContains(sourceOfIncomeId))
                        {
                            model.Amount = amount;
                            model.SourceOfIncomeId = sourceOfIncomeId;
                            SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(sourceOfIncomeId);
                            model.CustomerSourceOfIncome = sourceOfIncome.Name;
                            model.MarkEdited();
                            X.MessageBox.Alert("Status", "Source of income updated.").Show();
                        }
                        else //else if source of income already exists
                        {
                            X.MessageBox.Alert("Error", "Source of income name already exists.").Show();
                            manageSourcesOfIncome.Show();
                        }
                    }
                }
            }
            PageGridPanelStore.DataSource = form.AvailableSourcesOfIncome;
            PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public void onIdChange1()
        {
            int id1 = int.Parse(cmbIdType1.SelectedItem.Value.ToString());
            var idTypes2 = ObjectContext.IdentificationTypes.Where(entity => entity.Id != id1);

            IDTypeStore2.DataSource = idTypes2.ToList();
            IDTypeStore2.DataBind();
        }

        [DirectMethod]
        public void onIdChange2()
        {
            int id2 = int.Parse(cmbIdType2.SelectedItem.Value.ToString());
            var idTypes1 = ObjectContext.IdentificationTypes.Where(entity => entity.Id != id2);

            IDTypeStore.DataSource = idTypes1.ToList();
            IDTypeStore.DataBind();
        }

        protected void btnDeleteSourceIncome_Click(object sender, DirectEventArgs e)
        {
            CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
            SelectedRowCollection rows = this.PageGridPanelSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveSourceOfIncome(row.RecordID);
            }
            PageGridPanelStore.DataSource = form.AvailableSourcesOfIncome;
            PageGridPanelStore.DataBind();
            //GridPanel1.Reload();
        }

        protected void btnSelectDistrict_DirectClick(object sender, DirectEventArgs e)
        {

            RowSelectionModel am = grdDistrictPanelModel;
            SelectedRow rows = am.SelectedRow;
                RowSelectionModel sm = grdDistrictPanelModel;
                SelectedRow row = sm.SelectedRow;
                if (row == null)
                {
                    row.RecordID = "1";
                }
            using (var ctx = new FinancialEntities())
            {
                ClassificationType classType = new ClassificationType();

                    int id = int.Parse(row.RecordID);
                    classType = ctx.ClassificationTypes.FirstOrDefault(entity => entity.Id == id);


                if (classType != null)
                {
                    this.hiddenClassificationTypeID.Value = classType.Id;
                    txtDistrict.Text = classType.District;
                    txtStationNumber.Text = classType.StationNumber;

                    //birthplace
                    txtBarangayB1.Text = classType.District;

                    //primary home address
                    txtBarangayA1.Text = classType.District;

                    //secondary home address
                    txtBarangayA2.Text = classType.District;
                }
            }

            wndDistrictList.Hide();
        }

        [DirectMethod(Namespace = "Customer")]
        protected void btnSelectDistricts(string id)
        {
            int ids = int.Parse(id);
            if (ids != -1)
            {
                using (var ctx = new FinancialEntities())
                {
                    ClassificationType classType = new ClassificationType();

                    classType = ctx.ClassificationTypes.FirstOrDefault(entity => entity.Id == ids);


                    if (classType != null)
                    {
                        this.hiddenClassificationTypeID.Value = classType.Id;
                        txtDistrict.Text = classType.District;
                        txtStationNumber.Text = classType.StationNumber;
                    }
                }
            }

            wndDistrictList.Hide();
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

        //Fill customer details
        [DirectMethod(ShowMask = true, Msg = "Retrieving customer details...")]
        public void FillCustomerDetails(int partyRlId)
        {
            var today = DateTime.Now;

            PartyRole partyRole = PartyRole.GetById(partyRlId);
            CustomerDetailsModel model = new CustomerDetailsModel(partyRole.Id);
            this.txtPersonName.Text = model.Name.NameConcat;
            this.txtTitleP.Text = model.Name.Title;
            this.txtFirstNameP.Text = model.Name.FirstName;
            this.txtLastNameP.Text = model.Name.LastName;
            this.txtMiddleNameP.Text = model.Name.MiddleName;
            this.txtNameSuffixP.Text = model.Name.NameSuffix;
            this.txtNickNameP.Text = model.Name.NickName;

            this.CustomerID.Value = model.PartyRoleId;

            if (model.Gender == GenderType.FemaleType.Name)
                rdFemale.Checked = true;
            else
                rdMale.Checked = true;

            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
                this.PersonImageFile.ImageUrl = model.ImageUrl;
            else
                this.PersonImageFile.ImageUrl = "../../../Resources/images/noimage.jpg";
            this.txtMothersMaidenName.Text = model.MothersMaidenName;
            this.datBirthdate.SelectedDate = model.Birthdate;
            this.txtPrimaryHomeAddress.Text = model.PrimaryHomeAddress.AddressConcat;
            this.txtStreetAddressA1.Text = model.PrimaryHomeAddress.StreetAddress;
            this.txtBarangayA1.Text = model.PrimaryHomeAddress.Barangay;
            //this.txtCityA1.Text = model.PrimaryHomeAddress.City;
            //this.txtMunicipalityD1.Text = model.PrimaryHomeAddress.Municipality;
            if (!string.IsNullOrWhiteSpace(model.PrimaryHomeAddress.City))
            {
                this.txtCityOrMunicipalityA1.Text = model.PrimaryHomeAddress.City;
                radioCityA1.Checked = true;
            }
            else if (!string.IsNullOrWhiteSpace(model.PrimaryHomeAddress.Municipality))
            {
                this.txtCityOrMunicipalityA1.Text = model.PrimaryHomeAddress.Municipality;
                radioMunicipalityA1.Checked = true;
            }

            this.txtProvinceA1.Text = model.PrimaryHomeAddress.Province;
            if(model.PrimaryHomeAddress.CountryId != 0)
                this.cmbCountryA1.SelectedItem.Value = model.PrimaryHomeAddress.CountryId.ToString();
            this.txtPostalCodeA1.Text = model.PrimaryHomeAddress.PostalCode;

            if(!string.IsNullOrWhiteSpace(model.CellphoneAreaCode))
                this.txtCellAreaCode.Text = model.CellphoneAreaCode;
            if (!string.IsNullOrWhiteSpace(model.CountryCode))
                this.txtCellTelCodes.Text = model.CountryCode;
            if (!string.IsNullOrWhiteSpace(model.CellphoneNumber))
                this.txtCellPhoneNumber.Text = model.CellphoneNumber;
            if (!string.IsNullOrWhiteSpace(model.TelephoneNumberAreaCode))
                this.txtTelAreaCode.Text = model.TelephoneNumberAreaCode;
            if (!string.IsNullOrWhiteSpace(model.CountryCode))
                this.txtTelCodes.Text = model.CountryCode;
            if (!string.IsNullOrWhiteSpace(model.TelephoneNumber))
                this.txtTelPhoneNumber.Text = model.TelephoneNumber;
            if (!string.IsNullOrWhiteSpace(model.PrimaryEmailAddress))
                this.txtPrimaryEmailAddress.Text = model.PrimaryEmailAddress;

            int newEmployerId = int.Parse(hiddenNewEmployerID.Value.ToString());
            PartyRelationship employmentPartyRel = partyRole.CurrentEmploymentRelationship;
            
            if(employmentPartyRel != null)
                FillEmploymentDetails(employmentPartyRel.SecondPartyRoleId, newEmployerId);
        }

        private PartyRole GetPartyRole(int partyId, DateTime today)
        {
            var partyRoleType = RoleType.CustomerType;
            PartyRole partyRole = new PartyRole();
            partyRole.PartyId = partyId;
            partyRole.RoleTypeId = partyRoleType.Id;
            partyRole.EffectiveDate = today;

            ObjectContext.PartyRoles.AddObject(partyRole);
            ObjectContext.SaveChanges();

            return partyRole;
        }

        //Fill employment details
        [DirectMethod(ShowMask = true, Msg = "Filling employer details...")]
        public void FillEmploymentDetails(int empPartyRoleId, int newEmployerIds)
        {
            txtEmploymentStatus.AllowBlank = false;
            txtEmpPosition.AllowBlank = false;
            txtSalary.AllowBlank = false;

            int custPartyRoleId = int.Parse(CustomerID.Value.ToString());
            int newEmployerId = -1;

            if (CustomerID.Value.ToString() != "-1")
            {
                hiddenEmployerID.Value = empPartyRoleId;
                newEmployerId = int.Parse(hiddenEmployerID.Value.ToString());
            }
            else if (CustomerID.Value.ToString() == "-1" && hiddenEmployerID.Value.ToString() != "-1")
            {
                newEmployerId = int.Parse(hiddenEmployerID.Value.ToString());
            }
            else
            {
                newEmployerId = -1;
            }

            if (empPartyRoleId == -1 && newEmployerIds != -1)
                empPartyRoleId = newEmployerIds;
            EmploymentDetailsModel model = new EmploymentDetailsModel(empPartyRoleId, custPartyRoleId, newEmployerIds);
            

            this.txtEmployerName.Text = model.Name;
            this.txtEmploymentAddress.Text = model.EmploymentAddress;
            this.txtEmploymentStatus.Text = model.EmploymentStatus;
            this.txtEmpIDNumber.Text = model.EmployeeIdNumber;
            this.txtEmpPosition.Text = model.EmployeePosition;
            this.txtGsisNumber.Text = model.GsisNumber;
            this.txtEmpEmailAddress.Text = model.EmploymentEmailAddress;
            this.txtSssNumber.Text = model.SssNumber;
            this.txtOWANumber.Text = model.OwaNumber;
            this.txtPhicNumber.Text = model.PhicNumber;
            this.txtEmpTelCode.Text = model.EmpCountryCode;
            this.txtEmpAreaCode.Text = model.EmpTelephoneAreaCode;
            this.txtEmpPhoneNumber.Text = model.EmploymentTelephoneNumber;
            this.txtEmpFaxTelCode.Text = model.EmpCountryCode;
            this.txtEmpFaxAreaCode.Text = model.EmploymentFaxNumberAreaCode;
            this.txtEmpFaxNumber.Text = model.EmploymentFaxNumber;
            this.txtSalary.Text = model.Salary;
        }

        private class CustomerDetailsModel
        {
            public string ImageUrl { get; set; }

            public int PartyRoleId { get; set; }

            public string MothersMaidenName { get; set; }

            public NameModel Name { get; set; }

            public DateTime Birthdate { get; set; }

            public string Gender { get; set; }

            public int Age { get; set; }

            public string CountryCode { get; set; }

            public string TelephoneNumberAreaCode { get; set; }

            public string CellphoneAreaCode { get; set; }

            public PostalAddressModel PrimaryHomeAddress { get; set; }

            public string CellphoneNumber { get; set; }

            public string TelephoneNumber { get; set; }

            public string PrimaryEmailAddress { get; set; }

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


            public CustomerDetailsModel(int partyRoleId)
            {
                this.Name = new NameModel();
                this.PrimaryHomeAddress = new PostalAddressModel();

                this.PartyRoleId = partyRoleId;
                PartyRole partyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
                Party party = partyRole.Party;
                Person personAsCustomer = party.Person;

                this.Gender = personAsCustomer.GenderType.Name;
                this.Name.NameConcat = StringConcatUtility.Build("", personAsCustomer.LastNameString + ", ", 
                                        personAsCustomer.FirstNameString + " ", personAsCustomer.MiddleInitialString + " ",
                                        personAsCustomer.NameSuffixString);

                this.Name.LastName = personAsCustomer.LastNameString;
                this.Name.MiddleName = personAsCustomer.MiddleNameString;
                this.Name.MothersMaidenName = personAsCustomer.MothersMaidenNameString;
                this.Name.NameSuffix = personAsCustomer.NameSuffixString;
                this.Name.NickName = personAsCustomer.NickNameString;
                this.Name.Title = personAsCustomer.TitleString;
                this.Name.FirstName = personAsCustomer.FirstNameString;


                this.MothersMaidenName = personAsCustomer.MothersMaidenNameString;
                this.ImageUrl = personAsCustomer.ImageFilename;
                this.Age = personAsCustomer.Age;

                this.Birthdate = (DateTime)personAsCustomer.Birthdate;

                InitializeAddresses(party);
            }

            private void InitializeAddresses(Party party)
            {
                //Postal Address
                Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.PostalAddressType
                                    && entity.PostalAddress.PostalAddressType == PostalAddressType.HomeAddressType && entity.PostalAddress.IsPrimary);

                if (postalAddress != null && postalAddress.PostalAddress != null)
                {
                    PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                    this.PrimaryHomeAddress.AddressConcat = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                                            postalAddressSpecific.Barangay,
                                                            postalAddressSpecific.Municipality,
                                                            postalAddressSpecific.City,
                                                            postalAddressSpecific.Province,
                                                            postalAddressSpecific.State,
                                                            postalAddressSpecific.Country.Name,
                                                            postalAddressSpecific.PostalCode);

                    this.PrimaryHomeAddress.Barangay = postalAddressSpecific.Barangay;
                    this.PrimaryHomeAddress.City = postalAddressSpecific.City;
                    this.PrimaryHomeAddress.CountryId = postalAddressSpecific.CountryId;
                    this.PrimaryHomeAddress.Municipality = postalAddressSpecific.Municipality;
                    this.PrimaryHomeAddress.PostalCode = postalAddressSpecific.PostalCode;
                    this.PrimaryHomeAddress.Province = postalAddressSpecific.Province;
                    this.PrimaryHomeAddress.StreetAddress = postalAddressSpecific.StreetAddress;

                    this.CountryCode = postalAddressSpecific.Country.CountryTelephoneCode;

                    //Cellphone Number
                    Address cellTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                        && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.PersonalMobileNumberType
                                        && entity.TelecommunicationsNumber.IsPrimary);

                    if (cellTelecomNumber != null && cellTelecomNumber.TelecommunicationsNumber != null)
                    {

                        TelecommunicationsNumber cellTelecomNumberSpecific = cellTelecomNumber.TelecommunicationsNumber;
                        this.CellphoneNumber = cellTelecomNumberSpecific.PhoneNumber;
                        this.CellphoneAreaCode = cellTelecomNumberSpecific.AreaCode;
                    }

                    //Telephone Number
                    Address primTelecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                        && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.HomePhoneNumberType
                                        && entity.TelecommunicationsNumber.IsPrimary);

                    if (primTelecomNumber != null && primTelecomNumber.TelecommunicationsNumber != null)
                    {
                        TelecommunicationsNumber primNumberSpecific = primTelecomNumber.TelecommunicationsNumber;
                        this.TelephoneNumber = primNumberSpecific.PhoneNumber;
                        this.TelephoneNumberAreaCode = primNumberSpecific.AreaCode;
                    }
                }

                //Email Address
                Address emailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                    && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.PersonalEmailAddressType
                                    && entity.ElectronicAddress.IsPrimary);
                if (emailAddress != null && emailAddress.ElectronicAddress != null)
                {
                    ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                    this.PrimaryEmailAddress = primaryEmailAddressSpecific.ElectronicAddressString;
                }
            }
        }

        private class EmploymentDetailsModel
        {

            public int PartyRoleId { get; set; }

            public string Name { get; set; }

            public int EmployerId { get; set; }

            public int NewEmployerId { get; set; }

            public string EmploymentAddress { get; set; }

            public string EmpCountryCode { get; set; }

            public string EmpTelephoneAreaCode { get; set; }

            public string EmploymentTelephoneNumber { get; set; }

            public string EmploymentFaxNumberAreaCode { get; set; }

            public string EmploymentFaxNumber { get; set; }

            public string EmployeeIdNumber { get; set; }

            public string EmployeePosition { get; set; }

            public string EmploymentStatus { get; set; }

            public string EmploymentEmailAddress { get; set; }

            public string Salary { get; set; }

            public string SssNumber { get; set; }

            public string GsisNumber { get; set; }

            public string OwaNumber { get; set; }

            public string PhicNumber { get; set; }

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
                this.NewEmployerId = -1;
            }

            public EmploymentDetailsModel(int empPartyRoleId, int custPartyRoleId, int newEmployerId)
            {
                this.PartyRoleId = empPartyRoleId;
                this.EmployerId = empPartyRoleId;
                this.NewEmployerId = newEmployerId;
                var employerPartyRoleId = 0;
                if (this.NewEmployerId != -1 && this.NewEmployerId != this.EmployerId)
                {
                    employerPartyRoleId = newEmployerId;
                }
                else
                {
                    employerPartyRoleId = empPartyRoleId;
                }
                PartyRole empPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == employerPartyRoleId);
                Party empParty = empPartyRole.Party;
                if (empParty.PartyType == PartyType.PersonType)
                {
                    Person employerPerson = empParty.Person;
                    this.Name = StringConcatUtility.Build("", employerPerson.LastNameString + ", "
                    , employerPerson.FirstNameString, employerPerson.MiddleInitialString + " ",
                    employerPerson.NameSuffixString);
                }
                else
                {
                    Organization employerOrg = empParty.Organization;
                    this.Name = employerOrg.OrganizationName;
                }

                if ((this.NewEmployerId == -1 && custPartyRoleId != -1) || (custPartyRoleId != -1 && this.EmployerId != -1 && this.NewEmployerId == -1))
                {
                    PartyRole cust = PartyRole.GetById(custPartyRoleId);
                    PartyRole employeeRole = PartyRole.GetByPartyIdAndRole(cust.PartyId, RoleType.EmployeeType);

                    if (employeeRole != null)
                    {
                        PartyRelationship employmentPartyRel = Context.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == employeeRole.Id
                            && entity.SecondPartyRoleId == empPartyRoleId && entity.PartyRelTypeId == PartyRelType.EmploymentType.Id && entity.EndDate == null);
                        if (employmentPartyRel != null)
                        {
                            PartyRole employeePartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == custPartyRoleId && entity.RoleTypeId == RoleType.EmployeeType.Id);
                            Employee customerAsEmployee = employeePartyRole.Employee;
                            Employment employment = employmentPartyRel.Employment;

                            this.EmployeeIdNumber = customerAsEmployee.EmployeeIdNumber;
                            this.EmployeePosition = customerAsEmployee.Position;
                            this.SssNumber = customerAsEmployee.SssNumber;
                            this.GsisNumber = customerAsEmployee.GsisNumber;
                            this.OwaNumber = customerAsEmployee.OwaNumber;
                            this.PhicNumber = customerAsEmployee.PhicNumber;

                            this.EmploymentStatus = employment.EmploymentStatus;
                            this.Salary = employment.Salary;
                        }
                    }
                }

                InitializeAddresses(empParty);
            }

            private void InitializeAddresses(Party party)
            {
                //Postal Address
                Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                    && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.BusinessAddressType.Id && entity.PostalAddress.IsPrimary == true);

                if (postalAddress != null && postalAddress.PostalAddress != null)
                {
                    PostalAddress postalAddressSpecific = postalAddress.PostalAddress;

                    this.EmploymentAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                  postalAddressSpecific.Barangay,
                                  postalAddressSpecific.Municipality,
                                  postalAddressSpecific.City,
                                  postalAddressSpecific.Province,
                                  postalAddressSpecific.State,
                                  postalAddressSpecific.Country.Name,
                                  postalAddressSpecific.PostalCode);

                    this.EmpCountryCode = postalAddressSpecific.Country.CountryTelephoneCode;
                    //Business Telephone Number
                    Address telecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                                        && entity.TelecommunicationsNumber.TypeId== TelecommunicationsNumberType.BusinessPhoneNumberType.Id
                                        && entity.TelecommunicationsNumber.IsPrimary == true);

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

        //check source of income amount
        protected void checkField(object sender, RemoteValidationEventArgs e)
        {
            TextField field = (TextField)sender;

            if (field.Text != "0" || field.Text != "0.00")
            {
                e.Success = true;
                btnSaveSourceIncome.Enabled = true;
            }
            else
            {
                e.Success = false;
                e.ErrorMessage = "Amount must be greater than 0.";
                btnSaveSourceIncome.Disabled = true;
            }

        }

        #region Obsolete Methods
        //primary home address details (OLD METHOD)
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

            txtPrimaryHomeAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd1.Text)))
                txtPrimaryHomeAddress.Text = txtStreetAdd1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay1.Text)))
                txtPrimaryHomeAddress.Text += txtBarangay1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity1.Text)) && string.IsNullOrWhiteSpace(txtMunicipality1.Text))
                txtPrimaryHomeAddress.Text += txtCity1.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality1.Text)) && string.IsNullOrWhiteSpace(txtCity1.Text))
                txtPrimaryHomeAddress.Text += txtMunicipality1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince1.Text)))
                txtPrimaryHomeAddress.Text += txtProvince1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry1.Text)))
                txtPrimaryHomeAddress.Text += txtCountry1.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal1.Text)))
                txtPrimaryHomeAddress.Text += " " + txtPostal1.Text;

            using (var context = new FinancialEntities())
            {
                int countID = int.Parse(cmbCountryD1.Text);
                Country count = context.Countries.FirstOrDefault(entity => entity.Id == countID);
                txtCellTelCodes.Text = count.CountryTelephoneCode;
                txtTelCodes.Text = count.CountryTelephoneCode;
            }

            wndAddressDetail1.Hide();
        }

        //secondary home address details (OLD METHOD)
        protected void btnSaveAddress2_Click(object sender, DirectEventArgs e)
        {
            //SecondaryHomeAddress.Text = pgAddress2.Source["Postal Code"].Value;

            txtStreetAdd2.Text = txtStreetNumberD2.Text;
            txtBarangay2.Text = txtBarangayD2.Text;
            txtCity2.Text = txtCityD2.Text;
            //txtState2.Text = pgAddress2.Source["State"].Value;
            txtPostal2.Text = txtPostalCodeD2.Text;
            txtCountry2.Text = cmbCountryD2.SelectedItem.Text;
            txtMunicipality2.Text = txtMunicipalityD2.Text;
            txtProvince2.Text = txtProvinceD2.Text;

            txtSecondaryHomeAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd2.Text)))
                txtSecondaryHomeAddress.Text = txtStreetAdd2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay2.Text)))
                txtSecondaryHomeAddress.Text += txtBarangay2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity2.Text)) && string.IsNullOrWhiteSpace(txtMunicipality2.Text))
                txtSecondaryHomeAddress.Text += txtCity2.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality2.Text)) && string.IsNullOrWhiteSpace(txtCity2.Text))
                txtSecondaryHomeAddress.Text += txtMunicipality2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince2.Text)))
                txtSecondaryHomeAddress.Text += txtProvince2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry2.Text)))
                txtSecondaryHomeAddress.Text += txtCountry2.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal2.Text)))
                txtSecondaryHomeAddress.Text += " " + txtPostal2.Text;

            //using (var context = new FinancialEntities())
            //{
            //    int countID = int.Parse(cmbCountryD2.Text);
            //    Country count = context.Countries.FirstOrDefault(entity => entity.Id == countID);
            //    txtCellTelCodes.Text = count.CountryTelephoneCode;
            //    txtTelAreaCode.Text = count.CountryTelephoneCode;
            //}

            wndAddressDetail2.Hide();
        }

        //birthplace details (OLD METHOD)
        protected void btnSaveBirthAdd_Click(object sender, DirectEventArgs e)
        {
            //SecondaryHomeAddress.Text = pgAddressBirth.Source["Postal Code"].Value;

            txtStreetAdd3.Text = txtStreetNumberB.Text;
            txtBarangay3.Text = txtBarangayB.Text;
            txtCity3.Text = txtCityB.Text;
            //txtState3.Text = pgAddressBirth.Source["State"].Value;
            txtPostal3.Text = txtPostalCodeB.Text;
            txtCountry3.Text = cmbCountry.SelectedItem.Text;
            txtMunicipality3.Text = txtMunicipalityB.Text;
            txtProvince3.Text = txtProvinceB.Text;

            txtBirthPlace.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd3.Text)))
                txtBirthPlace.Text = txtStreetAdd3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay3.Text)))
                txtBirthPlace.Text += txtBarangay3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity3.Text)) && string.IsNullOrWhiteSpace(txtMunicipality3.Text))
                txtBirthPlace.Text += txtCity3.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality3.Text)) && string.IsNullOrWhiteSpace(txtCity3.Text))
                txtBirthPlace.Text += txtMunicipality3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince3.Text)))
                txtBirthPlace.Text += txtProvince3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry3.Text)))
                txtBirthPlace.Text += txtCountry3.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal3.Text)))
                txtBirthPlace.Text += " " + txtPostal3.Text;

            wndBirthPlaceAddDetail.Hide();
        }
        #endregion

        //customer name details
        protected void btnSavePersonNameDetail_Click(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgPersonName.Source["Postal Code"].Value;

            txtPersonTitle.Text = txtTitleP.Text;
            txtPersonFirstName.Text = txtFirstNameP.Text;
            txtPersonLastName.Text = txtLastNameP.Text;
            txtPersonMiddleName.Text = txtMiddleNameP.Text;
            txtPersonNickName.Text = txtNickNameP.Text;
            txtPersonNameSuffix.Text = txtNameSuffixP.Text;
            //txtPersonMothersMaidenName.Text = pgPersonName.Source["Mother's Maiden Name"].Value;

            txtPersonName.Text = txtPersonLastName.Text + ", " + txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            hiddenName.Value = txtPersonName.Text;

            wndPersonNameDetail.Hide();
            //checkCustomerName();
            //txtPersonName.Focus();
        }

        //primary home address details FINAL
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

            txtPrimaryHomeAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd1.Text)))
                txtPrimaryHomeAddress.Text = txtStreetAdd1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay1.Text)))
                txtPrimaryHomeAddress.Text += txtBarangay1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity1.Text)) && string.IsNullOrWhiteSpace(txtMunicipality1.Text))
                txtPrimaryHomeAddress.Text += txtCity1.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality1.Text)) && string.IsNullOrWhiteSpace(txtCity1.Text))
                txtPrimaryHomeAddress.Text += txtMunicipality1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince1.Text)))
                txtPrimaryHomeAddress.Text += txtProvince1.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry1.Text)))
                txtPrimaryHomeAddress.Text += txtCountry1.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal1.Text)))
                txtPrimaryHomeAddress.Text += " " + txtPostal1.Text;

            using (var context = new FinancialEntities())
            {
                int countID = int.Parse(cmbCountryA1.SelectedItem.Value);
                Country count = context.Countries.FirstOrDefault(entity => entity.Id == countID);
                txtCellTelCodes.Text = count.CountryTelephoneCode;
                txtTelCodes.Text = count.CountryTelephoneCode;
            }

            winAddressDetailA1.Hide();
        }

        //secondary home address details FINAL
        protected void btnDoneAddressDetailA2_DirectClick(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgAddress1.Source["Postal Code"].Value;

            txtStreetAdd2.Text = txtStreetAddressA2.Text;
            txtBarangay2.Text = txtBarangayA2.Text;
            if (radioCityA2.Checked == true)
                txtCity2.Text = txtCityOrMunicipalityA2.Text;
            else if (radioMunicipalityA2.Checked == true)
                txtMunicipality2.Text = txtCityOrMunicipalityA2.Text;
            //txtState1.Text = pgAddress1.Source["State"].Value;
            txtPostal2.Text = txtPostalCodeA2.Text;
            txtCountry2.Text = cmbCountryA2.SelectedItem.Text;

            txtProvince2.Text = txtProvinceA2.Text;

            txtSecondaryHomeAddress.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd2.Text)))
                txtSecondaryHomeAddress.Text = txtStreetAdd2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay2.Text)))
                txtSecondaryHomeAddress.Text += txtBarangay2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity2.Text)) && string.IsNullOrWhiteSpace(txtMunicipality2.Text))
                txtSecondaryHomeAddress.Text += txtCity2.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality2.Text)) && string.IsNullOrWhiteSpace(txtCity2.Text))
                txtSecondaryHomeAddress.Text += txtMunicipality2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince2.Text)))
                txtSecondaryHomeAddress.Text += txtProvince2.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry2.Text)))
                txtSecondaryHomeAddress.Text += txtCountry2.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal2.Text)))
                txtSecondaryHomeAddress.Text += " " + txtPostal2.Text;

            winAddressDetailA2.Hide();
        }

        //birthplace details FINAL
        protected void btnDoneAddressDetailB1_DirectClick(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgAddress1.Source["Postal Code"].Value;

            txtStreetAdd3.Text = txtStreetAddressB1.Text;
            txtBarangay3.Text = txtBarangayB1.Text;
            if (radioCityB1.Checked == true)
                txtCity3.Text = txtCityOrMunicipalityB1.Text;
            else if (radioMunicipalityB1.Checked == true)
                txtMunicipality3.Text = txtCityOrMunicipalityB1.Text;
            //txtState1.Text = pgAddress1.Source["State"].Value;
            txtPostal3.Text = txtPostalCodeB1.Text;
            txtCountry3.Text = cmbCountryB1.SelectedItem.Text;

            txtProvince3.Text = txtProvinceB1.Text;

            txtBirthPlace.Text = null;
            if (!(string.IsNullOrWhiteSpace(txtStreetAdd3.Text)))
                txtBirthPlace.Text = txtStreetAdd3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtBarangay3.Text)))
                txtBirthPlace.Text += txtBarangay3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCity3.Text)) && string.IsNullOrWhiteSpace(txtMunicipality3.Text))
                txtBirthPlace.Text += txtCity3.Text + ", ";
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality3.Text)) && string.IsNullOrWhiteSpace(txtCity3.Text))
                txtBirthPlace.Text += txtMunicipality3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtProvince3.Text)))
                txtBirthPlace.Text += txtProvince3.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtCountry3.Text)))
                txtBirthPlace.Text += txtCountry3.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal3.Text)))
                txtBirthPlace.Text += " " + txtPostal3.Text;

            winAddressDetailB1.Hide();
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
            //txtPersonMothersMaidenName.Text = pgPersonName.Source["Mother's Maiden Name"].Value;

            txtPersonName.Text = txtPersonLastName.Text + ", " + txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            hiddenName.Value = txtPersonName.Text;

            wndPersonNameDetail.Hide();
            //checkCustomerName();
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
            //txtPersonMothersMaidenName.Text = pgPersonName.Source["Mother's Maiden Name"].Value;

            if (!(string.IsNullOrWhiteSpace(txtPersonLastName.Text)))
                txtPersonName.Text = txtPersonLastName.Text + ", ";
            if (!(string.IsNullOrWhiteSpace(txtPersonFirstName.Text)))
                txtPersonName.Text += txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            hiddenName.Value = txtPersonName.Text;

            wndPersonNameDetail.Hide();
            //checkCustomerName();
            //txtPersonName.Focus();
        }

        protected void btnSaveSpouseNameDetail_Click(object sender, DirectEventArgs e)
        {
            //PrimaryHomeAddress.Text = pgPersonName.Source["Postal Code"].Value;

            txtSpouseTitle.Text = txtTitleS.Text; ;
            txtSpouseFirstName.Text = txtFirstNameS.Text;
            txtSpouseLastName.Text = txtLastNameS.Text;
            txtSpouseMiddleName.Text = txtMiddleNameS.Text;
            txtSpouseNickName.Text = txtNickNameS.Text;
            txtSpouseNameSuffix.Text = txtNameSuffixS.Text;
            txtSpouseMothersMaidenName.Text = txtMothersMaidenNameS.Text;

            txtSpouseName.Text = txtSpouseLastName.Text + ", " + txtSpouseFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtSpouseMiddleName.Text)))
                txtSpouseName.Text += " " + txtSpouseMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtSpouseNameSuffix.Text)))
                txtSpouseName.Text += " " + txtPersonNameSuffix.Text;

            wndSpouseNameDetail.Hide();
        }

        private class DataSource : IPageAbleDataSource<SourcesOfIncomeModel>
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IQueryable<SourcesOfIncomeModel> CreateQuery(FinancialEntities context)
            {
                int id = this.ID;
                var query = from c in context.Customers
                            join csoi in context.CustomerSourceOfIncomes on c.PartyRoleId equals csoi.PartyRoleId
                            join soi in context.SourceOfIncomes on csoi.SourceOfIncomeId equals soi.Id
                            where csoi.EndDate == null && c.PartyRoleId == id
                            select new SourcesOfIncomeModel()
                            {
                                Amount = csoi.Income,
                                Id = c.PartyRoleId,
                                SourceOfIncome = soi.Name
                            };
                return query;
            }

            public override List<SourcesOfIncomeModel> SelectAll(int start, int limit, Func<SourcesOfIncomeModel, string> orderBy)
            {
                List<SourcesOfIncomeModel> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return collection;
            }
        }
    }
}