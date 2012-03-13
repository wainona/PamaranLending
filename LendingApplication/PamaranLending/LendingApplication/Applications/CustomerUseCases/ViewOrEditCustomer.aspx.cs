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
    public partial class ViewOrEditCustomer : ActivityPageBase
    {
        //string mode = "";

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

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                
                CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
                int ageLimit = SystemSetting.AgeLimitOfBorrower;

                datResidentSince.MaxDate = DateTime.Now ;
                datDateIssued.MaxDate = DateTime.Now;
                datDateIssued.DisabledDays = ApplicationSettings.DisabledDays;
                datBirthdate.MaxDate = DateTime.Now.AddYears(-ageLimit);
                datBirthdate.SelectedDate = DateTime.Now.AddYears(-ageLimit);
                datSpouseBirthdate.MaxDate = DateTime.Now.AddYears(-ageLimit);

                int id = int.Parse(Request.QueryString["id"]);
                //this.btnBrowseCustomer.Disabled = true;

                if(!string.IsNullOrWhiteSpace(imageFilename))
                    PersonImageFile.ImageUrl = imageFilename;
                else
                    PersonImageFile.ImageUrl = imageFilename + "../../../Resources/images/noimage.jpg";

                //mode = Request.QueryString["mode"];
                this.CustomerID.Value = -1;
                RecordID.Value = -1;
                this.hiddenNewEmployerID.Value = -1;
                this.hiddenEmployerID.Value = -1;
                Customer customer = Customer.GetById(id);
                if (customer != null)
                {
                    RecordID.Value = customer.PartyRoleId;
                }
                else
                {
                    // TODO:: Don't know how to handle for now.
                }
                 
                //this.CustomerID.Value = id;
                //RetrieveCustomerInformation(context);

                var maritalStat = MaritalStatusType.All();
                var educAttainment = EducAttainmentType.All();
                var homeOwner = HomeOwnershipType.All();
                var idType = IdentificationType.All();
                var soi = SourceOfIncome.All();
                var countries = Country.All();
                var nationalities = NationalityType.All();
                var classTypes = ClassificationType.All();
                var customerTypes = CustomerCategoryType.All();

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

                PartyRole lending = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.PartyRoleType.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
                PartyRelationship party = ObjectContext.PartyRelationships.FirstOrDefault(entity => entity.FirstPartyRoleId == id && entity.SecondPartyRoleId == lending.Id && entity.EndDate == null);
                        
                if (party == null)
                    throw new AccessToDeletedRecordException("The customer has been deleted by another user.");
                Fill(id);

                if (cmbMaritalStatus.SelectedItem.Text == MaritalStatusType.MarriedType.Name)
                {
                    txtSpouseName.AllowBlank = false;

                }
                else
                {
                    txtSpouseName.AllowBlank = true;
                }
            }
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
                PersonImageFile.ImageUrl = imageFilename;
                this.hiddenImageUrl.Value = imageFilename;
            }
        }

        [DirectMethod]
        public void FillDetails(int id)
        {
            Fill(id);
        }

        protected void Fill(int customerId)
        {
            CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
            form.Retrieve(customerId);
            RecordID.Value = customerId;

            //<-- Basic Information
            if(!string.IsNullOrWhiteSpace(form.ImgUrl))
                PersonImageFile.ImageUrl = form.ImgUrl;
            //Customer Name
            txtPersonName.Text = form.CustomerName.NameConcat;
            txtPersonFirstName.Text = form.CustomerName.FirstName;
            txtPersonLastName.Text = form.CustomerName.LastName;
            txtPersonMiddleName.Text = form.CustomerName.MiddleName;
            txtMothersMaidenName.Text = form.CustomerName.MothersMaidenName;
            txtPersonNameSuffix.Text = form.CustomerName.NameSuffix;
            txtPersonNickName.Text = form.CustomerName.NickName;
            txtPersonTitle.Text = form.CustomerName.Title;

            txtFirstNameP.Text = form.CustomerName.FirstName;
            txtLastNameP.Text = form.CustomerName.LastName;
            txtMiddleNameP.Text = form.CustomerName.MiddleName;
            txtNameSuffixP.Text = form.CustomerName.NameSuffix;
            txtNickNameP.Text = form.CustomerName.NickName;
            txtTitleP.Text = form.CustomerName.Title;

            //Customer Classification Type
            hiddenClassificationTypeID.Value = form.DistrictClassificationTypeId;
            txtDistrict.Text = form.District;
            txtStationNumber.Text = form.StationNumber;

            //Customer Status
            txtCustomerStatus.Text = form.CustomerStatus;

            //Customer Gender
            int genderId = form.GenderTypeId;
            if (genderId == 1)
                rdFemale.Checked = true;
            else
                rdMale.Checked = true;
            
            //Customer Classification 
            cmbCustomerType.SelectedItem.Value = form.CustomerCategoryTypeId.ToString();

            //Birthdate
            datBirthdate.SelectedDate = form.Birthdate;

            //Birthplace
            txtBirthPlace.Text = form.Birthplace.AddressConcat;
            txtStreetAdd3.Text = form.Birthplace.StreetAddress;
            txtProvince3.Text = form.Birthplace.Province;
            txtBarangay3.Text = form.Birthplace.Barangay;
            txtCity3.Text = form.Birthplace.City;
            txtMunicipality3.Text = form.Birthplace.Municipality;
            txtPostal3.Text = form.Birthplace.PostalCode;
            txtState3.Text = form.Birthplace.State;


            //txtStreetNumberB.Text = form.Birthplace.StreetAddress;
            //txtProvinceB.Text = form.Birthplace.Province;
            //txtBarangayB.Text = form.Birthplace.Barangay;
            //txtCityB.Text = form.Birthplace.City;
            //txtMunicipalityB.Text = form.Birthplace.Municipality;
            //txtPostalCodeB.Text = form.Birthplace.PostalCode;
            //if (form.Birthplace.CountryId != 0)
            //    cmbCountry.SelectedItem.Value = form.Birthplace.CountryId.ToString();
            txtStreetAddressB1.Text = form.Birthplace.StreetAddress;
            txtProvinceB1.Text = form.Birthplace.Province;
            txtBarangayB1.Text = form.Birthplace.Barangay;
            if (!string.IsNullOrWhiteSpace(form.Birthplace.City))
            {
                txtCityOrMunicipalityB1.Text = form.Birthplace.City;
                radioMunicipalityB1.Checked = false;
                radioCityB1.Checked = true;
            }
            else if (!string.IsNullOrWhiteSpace(form.Birthplace.Municipality))
            {
                txtCityOrMunicipalityB1.Text = form.Birthplace.Municipality;
                radioCityB1.Checked = false;
                radioMunicipalityB1.Checked = true;
            }
            txtPostalCodeB1.Text = form.Birthplace.PostalCode;
            if (form.Birthplace.CountryId != 0)
                cmbCountryB1.SelectedItem.Value = form.Birthplace.CountryId.ToString();

            cmbMaritalStatus.SelectedItem.Value = form.MaritalStatusId.ToString();
            if (form.MaritalStatusId != MaritalStatusType.MarriedType.Id)
                SpouseInformationPanel.Disabled = true;
            else
                SpouseInformationPanel.Enabled = false;

            nfNumberOfDependents.Number = form.NumberOfDependents;
            cmbEducationalAttainment.SelectedItem.Value = form.EducationalAttainmentId.ToString();
            cmbHomeOwnership.SelectedItem.Value = form.HomeOwnershipId.ToString();
            datResidentSince.SelectedDate = form.ResidentSince;

            if(form.NationalityId != 0)
                cmbNationality.SelectedItem.Value = form.NationalityId.ToString();
            
            string[] result;
            if (form.Tin != null)
            {
                result = form.Tin.Split('-');
                txtTin1.Text = result[0];
                txtTin2.Text = result[1];
                txtTin3.Text = result[2];
                txtTin4.Text = result[3];
            }

            if (!string.IsNullOrEmpty(form.CtcNumber) && !string.IsNullOrEmpty(form.PlaceIssued))
            {
                txtCtc.Text = form.CtcNumber;
                txtPlaceIssued.Text = form.PlaceIssued;
            }
            if (!string.IsNullOrEmpty(form.CtcNumber))
                datDateIssued.SelectedDate = form.DateIssued;
            else
                datDateIssued.Text = "";
            
            if(form.CreditLimit != null)
                txtCreditLimit.Text = ((decimal)form.CreditLimit).ToString("N");
            //-->Basic Information end

            //<-- Id's
            if (form.IdType1Id != 0)
            {
                cmbIdType1.SelectedItem.Value = form.IdType1Id.ToString();
                txtIDNumber1.Text = form.IdNumber1;
            }
            if (form.IdType2Id != 0)
            {
                cmbIdType2.SelectedItem.Value = form.IdType2Id.ToString();
                txtIDNumber2.Text = form.IdNumber2;
            }
            //--> Id's end

            //<--Contact Information
            //Primary Home Address
            txtPrimaryHomeAddress.Text = form.CustomerPrimaryHomeAddress.AddressConcat;
            txtStreetAdd1.Text = form.CustomerPrimaryHomeAddress.StreetAddress;
            txtProvince1.Text = form.CustomerPrimaryHomeAddress.Province;
            txtBarangay1.Text = form.CustomerPrimaryHomeAddress.Barangay;
            txtCity1.Text = form.CustomerPrimaryHomeAddress.City;
            txtMunicipality1.Text = form.CustomerPrimaryHomeAddress.Municipality;
            txtPostal1.Text = form.CustomerPrimaryHomeAddress.PostalCode;
            txtState1.Text = form.CustomerPrimaryHomeAddress.State;

            //txtStreetNumberD1.Text = form.CustomerPrimaryHomeAddress.StreetAddress;
            //txtProvinceD1.Text = form.CustomerPrimaryHomeAddress.Province;
            //txtBarangayD1.Text = form.CustomerPrimaryHomeAddress.Barangay;
            //txtCityD1.Text = form.CustomerPrimaryHomeAddress.City;
            //txtMunicipalityD1.Text = form.CustomerPrimaryHomeAddress.Municipality;
            //txtPostalCodeD1.Text = form.CustomerPrimaryHomeAddress.PostalCode;
            //if (form.CustomerPrimaryHomeAddress.CountryId != 0)
            //    cmbCountryD1.SelectedItem.Value = form.CustomerPrimaryHomeAddress.CountryId.ToString();
            txtStreetAddressA1.Text = form.CustomerPrimaryHomeAddress.StreetAddress;
            txtProvinceA1.Text = form.CustomerPrimaryHomeAddress.Province;
            txtBarangayA1.Text = form.CustomerPrimaryHomeAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(form.CustomerPrimaryHomeAddress.City))
            {
                txtCityOrMunicipalityA1.Text = form.CustomerPrimaryHomeAddress.City;
                radioMunicipalityA1.Checked = false;
                radioCityA1.Checked = true;
            }
            else if (!string.IsNullOrWhiteSpace(form.CustomerPrimaryHomeAddress.Municipality))
            {
                txtCityOrMunicipalityA1.Text = form.CustomerPrimaryHomeAddress.Municipality;
                radioCityA1.Checked = false;
                radioMunicipalityA1.Checked = true;
            }
            txtPostalCodeA1.Text = form.CustomerPrimaryHomeAddress.PostalCode;
            if (form.CustomerPrimaryHomeAddress.CountryId != 0)
                cmbCountryA1.SelectedItem.Value = form.CustomerPrimaryHomeAddress.CountryId.ToString();

            //Secondary Home Address
            txtSecondaryHomeAddress.Text = form.CustomerSecondaryHomeAddress.AddressConcat;
            txtStreetAdd2.Text = form.CustomerSecondaryHomeAddress.StreetAddress;
            txtProvince2.Text = form.CustomerSecondaryHomeAddress.Province;
            txtBarangay2.Text = form.CustomerSecondaryHomeAddress.Barangay;
            txtCity2.Text = form.CustomerSecondaryHomeAddress.City;
            txtMunicipality2.Text = form.CustomerSecondaryHomeAddress.Municipality;
            txtPostal2.Text = form.CustomerSecondaryHomeAddress.PostalCode;
            txtState2.Text = form.CustomerSecondaryHomeAddress.State;

            //txtStreetNumberD2.Text = form.CustomerSecondaryHomeAddress.StreetAddress;
            //txtProvinceD2.Text = form.CustomerSecondaryHomeAddress.Province;
            //txtBarangayD2.Text = form.CustomerSecondaryHomeAddress.Barangay;
            //txtCityD2.Text = form.CustomerSecondaryHomeAddress.City;
            //txtMunicipalityD2.Text = form.CustomerSecondaryHomeAddress.Municipality;
            //txtPostalCodeD2.Text = form.CustomerSecondaryHomeAddress.PostalCode;
            //if (form.CustomerSecondaryHomeAddress.CountryId != 0)
            //    cmbCountryD2.SelectedItem.Value = form.CustomerSecondaryHomeAddress.CountryId.ToString();
            txtStreetAddressA2.Text = form.CustomerSecondaryHomeAddress.StreetAddress;
            txtProvinceA2.Text = form.CustomerSecondaryHomeAddress.Province;
            txtBarangayA2.Text = form.CustomerSecondaryHomeAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(form.CustomerSecondaryHomeAddress.City))
            {
                txtCityOrMunicipalityA2.Text = form.CustomerSecondaryHomeAddress.City;
                radioMunicipalityA2.Checked = false;
                radioCityA2.Checked = true;
            }
            else if (!string.IsNullOrWhiteSpace(form.CustomerSecondaryHomeAddress.Municipality))
            {
                txtCityOrMunicipalityA2.Text = form.CustomerSecondaryHomeAddress.Municipality;
                radioCityA2.Checked = false;
                radioMunicipalityA2.Checked = true;
            }
            txtPostalCodeA2.Text = form.CustomerSecondaryHomeAddress.PostalCode;
            if (form.CustomerSecondaryHomeAddress.CountryId != 0)
                cmbCountryA2.SelectedItem.Value = form.CustomerSecondaryHomeAddress.CountryId.ToString();

            //Cellphone Number
            txtCellTelCodes.Text = form.CustomerCountryCode;
            if (!string.IsNullOrWhiteSpace(form.CustomerCellNumber.AreaCode))
                txtCellAreaCode.Text = form.CustomerCellNumber.AreaCode;
            if (!string.IsNullOrWhiteSpace(form.CustomerCellNumber.PhoneNumber))
                txtCellPhoneNumber.Text = form.CustomerCellNumber.PhoneNumber;

            //Telephone Number
            txtTelCodes.Text = form.CustomerCountryCode;
            if (!string.IsNullOrWhiteSpace(form.CustomerTelephoneNumber.AreaCode))
                txtTelAreaCode.Text = form.CustomerTelephoneNumber.AreaCode;
            if (!string.IsNullOrWhiteSpace(form.CustomerTelephoneNumber.PhoneNumber))
                txtTelPhoneNumber.Text = form.CustomerTelephoneNumber.PhoneNumber;

            //Primary Email Address
            txtPrimaryEmailAddress.Text = form.CustomerPrimaryEmailAddress.EletronicAddressString;

            //Secondary Email Address
            txtSecondaryEmailAddress.Text = form.CustomerSecondaryEmailAddress.EletronicAddressString;
            //---> Contact Information End

            //<-- Employment Information
            this.hiddenEmployerID.Value = form.EmployerId;
            this.hiddenNewEmployerID.Value = form.NewEmployerId;

            //Employer Name
            txtEmployerName.Text = form.EmployerName;

            //Employer Contact Details
            txtEmploymentAddress.Text = form.EmploymentAddress;

            txtEmpTelCode.Text = form.EmployerCountryCode;
            txtEmpAreaCode.Text = form.EmployerTelephoneAreaCode;
            txtEmpPhoneNumber.Text = form.EmployerTelephoneNumber;

            txtEmpFaxTelCode.Text = form.EmployerCountryCode;
            txtEmpFaxAreaCode.Text = form.EmployerFaxAreaCode;
            txtEmpFaxNumber.Text = form.EmployerFaxNumber;

            txtEmpEmailAddress.Text = form.EmployerEmailAddress;

            //Employee Details
            txtEmpIDNumber.Text = form.EmployeeIdNumber;
            txtEmpPosition.Text = form.EmployeePosition;
            txtEmploymentStatus.Text = form.EmploymentStatus;
            txtSalary.Text = form.Salary;
            txtSssNumber.Text = form.SssNumber;
            txtGsisNumber.Text = form.GsisNumber;
            txtOWANumber.Text = form.OwaNumber;
            txtPhicNumber.Text = form.PhicNumber;
            //--> Employment Information End

            //<-- Customer Sources of Income
            PageGridPanelStore.DataSource = form.AvailableSourcesOfIncome;
            PageGridPanelStore.DataBind();
            //--> Customer Sources of Income end

            //<-- Spouse Information
            //Spouse Name
            txtSpouseName.Text = form.SpouseName.NameConcat;
            txtSpouseFirstName.Text = form.SpouseName.FirstName;
            txtSpouseLastName.Text = form.SpouseName.LastName;
            txtSpouseMiddleName.Text = form.SpouseName.MiddleName;
            txtSpouseMothersMaidenName.Text = form.SpouseName.MothersMaidenName;
            txtSpouseNameSuffix.Text = form.SpouseName.NameSuffix;
            txtSpouseNickName.Text = form.SpouseName.NickName;
            txtSpouseTitle.Text = form.SpouseName.Title;

            txtFirstNameS.Text = form.SpouseName.FirstName;
            txtLastNameS.Text = form.SpouseName.LastName;
            txtMiddleNameS.Text = form.SpouseName.MiddleName;
            txtNameSuffixS.Text = form.SpouseName.NameSuffix;
            txtNickNameS.Text = form.SpouseName.NickName;
            txtTitleS.Text = form.SpouseName.Title;

            //Spouse Birthdate
            if(form.SpouseBirthDate != null)
                datSpouseBirthdate.SelectedDate = (DateTime)form.SpouseBirthDate;

            //Remarks
            txtRemarks.Text = form.Remarks;

            //--> Spouse Information End

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

        protected void checkCustomerName(object sender, RemoteValidationEventArgs e)
        {
            
            var personName = txtPersonName.Text;
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
                    int partyId = 1;
                    var sameNameAndParty = context.CustomerViewLists.FirstOrDefault(entity => entity.Name == personName && entity.PartyId == partyId);
                    if (sameNameAndParty != null)
                    {
                        X.Msg.Confirm("Message", "A party record with the same name already exists in the pick list. Do you want to create another party record with the same name?", new JFunction("Customer.AddSameParty(result);", "result")).Show();
                    }
                    //res = false;
                }

            }
            e.Success = true;
            System.Threading.Thread.Sleep(2000);
        }

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
                //e.Success = false;
                //e.ErrorMessage = "Too young to apply for a loan.";
                X.Msg.Alert("Status", "Age must be greater than or equal to 18.").Show();
                datBirthdate.Text = "";
            }
            else
            {
                //e.Success = true;
            }
            //e.Success = false;
            //e.ErrorMessage = "Error";
        }

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

        }

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

        protected void SetToDefaultView(int id)
        {

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

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            if (btnOpen.Text == "Edit")
            {
                using (var unitOfWork = new UnitOfWorkScope(true))
                {
                    CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
                    form.IsNew = false;
                    if (this.CustomerID.Text == "-1")
                    {
                        form.NewCustomerId = -1;
                    }
                    else
                    {
                        form.NewCustomerId = int.Parse(this.CustomerID.Text);
                    }
                    //if(this.hiddenNewEmployerID.Text != "-1")
                    //    form.NewEmployerId = int.Parse(hiddenNewEmployerID.Text);
                    //if (this.hiddenEmployerID.Text != "-1")
                    //    form.EmployerId = int.Parse(hiddenEmployerID.Text);
                    if (!string.IsNullOrWhiteSpace(this.hiddenImageUrl.Text))
                        form.ImgUrl = this.hiddenImageUrl.Text;

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
                    form.EducationalAttainmentId = int.Parse(cmbEducationalAttainment.SelectedItem.Value);
                    form.DistrictClassificationTypeId = int.Parse(hiddenClassificationTypeID.Text);
                    form.CustomerStatus = txtCustomerStatus.Text;
                    form.CustomerCategoryTypeId = int.Parse(cmbCustomerType.SelectedItem.Value);

                    if (rdFemale.Checked)
                    {
                        form.GenderTypeId = 1;
                    }
                    else
                    {
                        form.GenderTypeId = 2;
                    }
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

                    form.MaritalStatusId = int.Parse(cmbMaritalStatus.SelectedItem.Value);
                    form.NumberOfDependents = int.Parse(nfNumberOfDependents.Text);
                    form.HomeOwnershipId = int.Parse(cmbHomeOwnership.SelectedItem.Value);
                    form.ResidentSince = datResidentSince.SelectedDate;
                    if(!string.IsNullOrWhiteSpace(cmbNationality.SelectedItem.Value))
                        form.NationalityId = int.Parse(cmbNationality.SelectedItem.Value);
                    if (txtTin1.Text != "" && txtTin2.Text != "" && txtTin3.Text != "")
                    {
                        form.Tin = txtTin1.Text + "-" + txtTin2.Text + "-" + txtTin3.Text + "-";

                        if (txtTin4.Text == "")
                            form.Tin += "000";
                        else
                            form.Tin += txtTin4.Text;
                    }
                    else
                    {
                        form.Tin = null;
                    }

                    if (!string.IsNullOrWhiteSpace(txtCtc.Text))
                    {
                        form.CtcNumber = txtCtc.Text;
                        form.DateIssued = datDateIssued.SelectedDate;
                        form.PlaceIssued = txtPlaceIssued.Text;
                    }
                    else
                    {
                        form.CtcNumber = null;
                        form.PlaceIssued = null;

                    }

                    if (!string.IsNullOrWhiteSpace(txtCreditLimit.Text))
                        form.CreditLimit = Decimal.Parse(txtCreditLimit.Text);
                    else
                        form.CreditLimit = null;

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

                    form.EmployerId = int.Parse(this.hiddenEmployerID.Value.ToString());
                    form.NewEmployerId = int.Parse(this.hiddenNewEmployerID.Value.ToString());
                    form.EmployeeIdNumber = txtEmpIDNumber.Text;
                    form.EmployeePosition = txtEmpPosition.Text;
                    form.EmploymentStatus = txtEmploymentStatus.Text;
                    form.Salary = txtSalary.Text;
                    form.SssNumber = txtSssNumber.Text;
                    form.GsisNumber = txtGsisNumber.Text;
                    form.OwaNumber = txtOWANumber.Text;
                    form.PhicNumber = txtPhicNumber.Text;

                    form.SpouseName.Title = txtSpouseTitle.Text;
                    form.SpouseName.NameSuffix = txtSpouseNameSuffix.Text;
                    form.SpouseName.NickName = txtSpouseNickName.Text;
                    form.SpouseName.MothersMaidenName = txtSpouseMothersMaidenName.Text;
                    form.SpouseName.MiddleName = txtSpouseMiddleName.Text;
                    form.SpouseName.LastName = txtSpouseLastName.Text;
                    form.SpouseName.FirstName = txtSpouseFirstName.Text;

                    if (datSpouseBirthdate.SelectedValue != null)
                        form.SpouseBirthDate = datSpouseBirthdate.SelectedDate;

                    form.Remarks = txtRemarks.Text;

                    form.PrepareForSave();

                }
            }
        }

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
            else if ((string.IsNullOrWhiteSpace(txtCityD1.Text) && string.IsNullOrWhiteSpace(txtMunicipalityD1.Text))
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
            }
        }

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
            }

        }

        protected void btnSaveSourceIncome_Click(object sender, DirectEventArgs e)
        {
            int sourceOfIncomeId = int.Parse(cmbSourceOfIncome.SelectedItem.Value);
            decimal amount = Decimal.Parse(txtAmount.Text);

            CustomerForm form = this.CreateOrRetrieve<CustomerForm>();
            if (string.IsNullOrWhiteSpace(this.hiddenRandomKey.Text))
            {
                if (amount == 0)
                {
                    X.MessageBox.Alert("Error", "Amount should be greater than 0.").Show();
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
            else
            {
                if (amount == 0)
                {
                    X.MessageBox.Alert("Status", "Amount should be greater than 0.").Show();
                    manageSourcesOfIncome.Show();
                }
                else if (amount > 0)
                {
                    CustomerSourceOfIncomeModel model = form.GetSourceOfIncome(this.hiddenRandomKey.Text);
                    if (model.SourceOfIncomeId == sourceOfIncomeId)
                    {
                        model.Amount = amount;
                        model.SourceOfIncomeId = sourceOfIncomeId;
                        SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(sourceOfIncomeId);
                        model.CustomerSourceOfIncome = sourceOfIncome.Name;
                        model.MarkEdited();
                        X.MessageBox.Alert("Status", "Source of income updated.").Show();
                    }
                    else
                    {
                        if (!form.SourcesOfIncomeContains(sourceOfIncomeId))
                        {
                            model.Amount = amount;
                            model.SourceOfIncomeId = sourceOfIncomeId;
                            SourceOfIncome sourceOfIncome = SourceOfIncome.GetById(sourceOfIncomeId);
                            model.CustomerSourceOfIncome = sourceOfIncome.Name;
                            model.MarkEdited();
                            X.MessageBox.Alert("Status", "Source of income updated.").Show();
                        }
                        else
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

        protected void openEdit_DirectClick(object sender, DirectEventArgs e)
        {
            //if (this.hidePanel.Disabled == true)
            //{
            //    hidePanel.Disabled = false;
            //    btnOpen.Disabled = true;
            //}
            //else
            //{
            //    hidePanel.Disabled = false;
            //    btnOpen.Enabled = true;
            //}
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

        [DirectMethod]
        public void FillCustomerDetails(int partyId)
        {
            var today = DateTime.Now;

            PartyRole partyRole = InsertNewPartyRole(partyId, today);
            CustomerDetailsModel model = new CustomerDetailsModel(partyRole.Id);
            this.txtPersonName.Text = model.Name;
            this.CustomerID.Value = model.PartyRoleId;

            //this.PersonImageFile.ImageUrl = model.ImageUrl;
            this.txtMothersMaidenName.Text = model.MothersMaidenName;
            this.datBirthdate.SelectedDate = model.Birthdate;
            this.txtPrimaryHomeAddress.Text = model.PrimaryHomeAddress;
            this.txtCellAreaCode.Text = model.CellphoneAreaCode;
            this.txtCellTelCodes.Text = model.CountryCode;
            this.txtCellPhoneNumber.Text = model.CellphoneNumber;
            this.txtTelAreaCode.Text = model.TelephoneNumberAreaCode;
            this.txtTelCodes.Text = model.CountryCode;
            this.txtTelPhoneNumber.Text = model.TelephoneNumber;
            this.txtPrimaryEmailAddress.Text = model.PrimaryEmailAddress;

            PartyRelationship employmentPartyRel = partyRole.CurrentEmploymentRelationship;

            FillEmploymentDetails(employmentPartyRel.SecondPartyRoleId);
        }

        private PartyRole InsertNewPartyRole(int partyId, DateTime today)
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

        [DirectMethod]
        public void FillEmploymentDetails(int empPartyRoleId)
        {
            txtEmploymentStatus.AllowBlank = false;
            txtEmpPosition.AllowBlank = false;
            txtSalary.AllowBlank = false;

            int custPartyRoleId = int.Parse(RecordID.Value.ToString());
            EmploymentDetailsModel model = new EmploymentDetailsModel(empPartyRoleId, custPartyRoleId);
            if (this.CustomerID.Text != "-1")
            {
                this.hiddenEmployerID.Value = model.PartyRoleId;
                model.NewEmployerId = int.Parse(this.hiddenEmployerID.Text);
            }
            else
            {
                model.NewEmployerId = -1;
            }

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

            public string Name { get; set; }

            public DateTime Birthdate { get; set; }

            public string Gender { get; set; }

            public int Age { get; set; }

            public string CountryCode { get; set; }

            public string TelephoneNumberAreaCode { get; set; }

            public string CellphoneAreaCode { get; set; }

            public string PrimaryHomeAddress { get; set; }

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
                this.PartyRoleId = partyRoleId;
                PartyRole partyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
                Party party = partyRole.Party;
                Person personAsCustomer = party.Person;

                this.Gender = personAsCustomer.GenderType.Name;
                this.Name = StringConcatUtility.Build("", personAsCustomer.LastNameString + ", "
                    , personAsCustomer.FirstNameString + " ", personAsCustomer.MiddleInitialString + " ",
                    personAsCustomer.NameSuffixString);

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

                    this.PrimaryHomeAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                  postalAddressSpecific.Barangay,
                                  postalAddressSpecific.Municipality,
                                  postalAddressSpecific.City,
                                  postalAddressSpecific.Province,
                                  postalAddressSpecific.State,
                                  postalAddressSpecific.Country.Name,
                                  postalAddressSpecific.PostalCode);

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

            public EmploymentDetailsModel(int empPartyRoleId, int custPartyRoleId)
            {
                this.PartyRoleId = empPartyRoleId;
                PartyRole empPartyRole = Context.PartyRoles.SingleOrDefault(entity => entity.Id == empPartyRoleId);
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

                if (this.NewEmployerId == -1)
                {
                    PartyRelationship employmentPartyRel = Context.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == custPartyRoleId
                        && entity.SecondPartyRoleId == empPartyRoleId && entity.PartyRelType == PartyRelType.EmploymentType && entity.EndDate == null);
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
                    Address telecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
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

        protected void SaveCustomer(FinancialEntities context)
        {
            var today = DateTime.Now;
            Party newParty = new Party();
            var personPartyType = context.PartyTypes.FirstOrDefault(entity => entity.Name == PartyTypeEnums.Person);
            if (string.IsNullOrWhiteSpace(this.CustomerID.Text) == false)
            {
                newParty.Id = int.Parse(this.CustomerID.Text);
            }
            else
            {
                newParty.PartyType = personPartyType;
            }

            var custRoleType = context.RoleTypes.FirstOrDefault(entity => entity.Name == RoleTypeEnums.Customer);
            var partyRole = context.PartyRoles.SingleOrDefault(entity => entity.Id == newParty.Id);

            var newStatusType = context.CustomerStatusTypes.SingleOrDefault(entity => entity.Name == "New");

            var titleID = GetPersonNameType(context, PersonNameTypeEnums.Title);
            var firstNameID = GetPersonNameType(context, PersonNameTypeEnums.FirstName);
            var middleNameID = GetPersonNameType(context, PersonNameTypeEnums.MiddleName);
            var lastNameID = GetPersonNameType(context, PersonNameTypeEnums.LastName);
            var nickNameID = GetPersonNameType(context, PersonNameTypeEnums.NickName);
            var nameSuffixID = GetPersonNameType(context, PersonNameTypeEnums.NameSuffix);
            var motherMaidenNameID = GetPersonNameType(context, PersonNameTypeEnums.MothersMaidenName);

            Person person = new Person();
            person.Party = newParty;

            if (!string.IsNullOrWhiteSpace(datBirthdate.Text) && datBirthdate.SelectedDate == today)
                person.Birthdate = datBirthdate.SelectedDate;

            if (cmbEducationalAttainment.SelectedItem != null)
                person.EducAttainmentTypeId = int.Parse(cmbEducationalAttainment.SelectedItem.Value);

            if (cmbNationality.SelectedIndex != -1)
                person.NationalityTypeId = int.Parse(cmbNationality.SelectedItem.Value);
            if (rdFemale.Checked == true)
            {
                person.GenderTypeId = (int)rdFemale.Value;
            }
            else
            {
                 person.GenderTypeId = (int)rdMale.Value;
            }

            //insert Customer's complete Name
            PersonName personFirstName = new PersonName(); //first name
            personFirstName.PartyId = newParty.Id;
            personFirstName.EffectiveDate = today;
            personFirstName.Name = txtPersonFirstName.Text;
            personFirstName.PersonNameType = firstNameID;

            PersonName personLastName = new PersonName(); //last name
            personLastName.PartyId = newParty.Id;
            personLastName.EffectiveDate = today;
            personLastName.Name = txtPersonLastName.Text;
            personLastName.PersonNameType = lastNameID;

            if (txtPersonMiddleName.Text != "")
            {
                PersonName personMName = new PersonName(); //middle name
                personMName.PartyId = newParty.Id;
                personMName.EffectiveDate = today;
                personMName.Name = txtPersonMiddleName.Text;
                personMName.PersonNameType = middleNameID;
            }

            if (txtPersonTitle.Text != "")
            {
                PersonName personTitleName = new PersonName(); //title name
                personTitleName.PartyId = newParty.Id;
                personTitleName.EffectiveDate = today;
                personTitleName.Name = txtPersonTitle.Text;
                personTitleName.PersonNameType = titleID;
            }

            if (txtPersonNameSuffix.Text != "")
            {
                PersonName personNameSuffix = new PersonName(); //name suffix
                personNameSuffix.PartyId = newParty.Id;
                personNameSuffix.EffectiveDate = today;
                personNameSuffix.Name = txtPersonNameSuffix.Text;
                personNameSuffix.PersonNameType = nameSuffixID;
            }

            if (txtPersonNickName.Text != "")
            {
                PersonName personNickName = new PersonName(); //nick name
                personNickName.PartyId = newParty.Id;
                personNickName.EffectiveDate = today;
                personNickName.Name = txtPersonNickName.Text;
                personNickName.PersonNameType = nickNameID;
            }

            if (txtMothersMaidenName.Text != "")
            {
                PersonName personMotherMaidenName = new PersonName(); //mother's maiden name
                personMotherMaidenName.PartyId = newParty.Id;
                personMotherMaidenName.EffectiveDate = today;
                personMotherMaidenName.Name = txtMothersMaidenName.Text;
                personMotherMaidenName.PersonNameType = motherMaidenNameID;
            }

            HomeOwnership homeOwnership = new HomeOwnership();
            if (cmbHomeOwnership.SelectedIndex != -1)
            {
                homeOwnership.PartyId = newParty.Id;
                homeOwnership.EffectiveDate = today;
                homeOwnership.HomeOwnershipTypeId = int.Parse(cmbHomeOwnership.SelectedItem.Value);
                homeOwnership.ResidenceSince = datResidentSince.SelectedDate;
            }

            MaritalStatu personMaritalStatus = new MaritalStatu();
            personMaritalStatus.PartyId = newParty.Id;
            personMaritalStatus.MaritalStatusTypeId = int.Parse(cmbMaritalStatus.SelectedItem.Value);
            personMaritalStatus.NumberOfDependents = int.Parse(nfNumberOfDependents.Text);

            //insert Customer party role record
            PartyRole partRole = new PartyRole();
            partRole.RoleTypeId = custRoleType.Id;
            partRole.Party = newParty;
            partRole.EffectiveDate = today;

            Customer cust = new Customer();
            cust.PartyRole = partRole;
            if(!string.IsNullOrWhiteSpace(txtCreditLimit.Text))
                cust.CreditLimit = Int64.Parse(txtCreditLimit.Text);

            CustomerStatu custoStatus = new CustomerStatu();
            custoStatus.CustomerStatusTypeId = newStatusType.Id;
            custoStatus.PartyRoleId = partRole.Id;

            CustomerClassification customerClassification = new CustomerClassification();
            customerClassification.PartyRoleId = partRole.Id;
            if (string.IsNullOrWhiteSpace(this.hiddenClassificationTypeID.Text))
            {
                customerClassification.ClassificationTypeId = int.Parse(hiddenClassificationTypeID.Text);
                customerClassification.EffectiveDate = today;
            }

            PartyRelationship partyRel = new PartyRelationship();
            var lendingInstitution = context.PartyRoles.SingleOrDefault(entity => entity.PartyRoleType.RoleType.Name == RoleTypeEnums.LendingInstitution);
            var customerRelType = context.PartyRelTypes.SingleOrDefault(entity => entity.Name == PartyRelationshipTypeEnum.CustomerRelationship);
            partyRel.FirstPartyRoleId = partRole.Id;
            partyRel.SecondPartyRoleId = lendingInstitution.Id;
            partyRel.PartyRelTypeId = customerRelType.Id;
            partyRel.EffectiveDate = today;

            //insert Taxpayer party role record
            PartyRole taxpayerPartRole = new PartyRole();
            var taxpayerRoleType = context.PartyRoles.SingleOrDefault(entity => entity.PartyRoleType.RoleType.Name == RoleTypeEnums.Taxpayer);
            taxpayerPartRole.RoleTypeId = taxpayerRoleType.RoleTypeId;
            taxpayerPartRole.PartyId = partyRole.PartyId;
            taxpayerPartRole.EffectiveDate = today;

            if (txtTin1.Text != "" && txtTin2.Text != "" && txtTin3.Text != "")
            {
                Taxpayer taxPayer = new Taxpayer();
                taxPayer.PartyRoleId = taxpayerRoleType.Id;
                taxPayer.Tin = txtTin1.Text + "-" + txtTin2.Text + "-" + txtTin3.Text + "-";

                if (txtTin4.Text == "")
                {
                    taxPayer.Tin += "000";
                }
                else
                {
                    taxPayer.Tin += txtTin4.Text;
                }
            }

            Ctc cTc = new Ctc();
            cTc.PartyRoleId = partRole.Id;
            if (string.IsNullOrWhiteSpace(txtCtc.Text) == false)
            {
                cTc.CtcNumber = txtCtc.Text;

            }

            if (string.IsNullOrWhiteSpace(datDateIssued.Text) == false)
            {
                cTc.DateIssued = datDateIssued.SelectedDate;
            }

            if (string.IsNullOrWhiteSpace(txtPlaceIssued.Text) == false)
            {
                cTc.IssuedWhere = txtPlaceIssued.Text;
            }

            var postalBirthCatType = context.AddressTypes.FirstOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);
            Address postalAddBirth = AddressUtility.AddAddress(newParty, postalBirthCatType, today);
            var birthPlaceCat = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.Birthplace);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(birthPlaceCat);
            
            //BirthPlace
            PostalAddress birthPlaceAdd = AddressUtility.AddPostal(postalAddBirth, birthPlaceCat, false);
            Country countryB = context.Countries.SingleOrDefault(entity => entity.Name == txtCountry3.Text);
            if (string.IsNullOrWhiteSpace(txtStreetAdd3.Text) == false)
                birthPlaceAdd.StreetAddress = txtStreetAdd3.Text;
            if (string.IsNullOrWhiteSpace(txtBarangay3.Text) == false)
                birthPlaceAdd.Barangay = txtBarangay3.Text;
            if (string.IsNullOrWhiteSpace(txtCity3.Text) == false)
                birthPlaceAdd.City = txtCity3.Text;
            if (string.IsNullOrWhiteSpace(txtState3.Text) == false)
                birthPlaceAdd.State = txtState3.Text;
            if (string.IsNullOrWhiteSpace(txtPostal3.Text) == false)
                birthPlaceAdd.PostalCode = txtPostal3.Text;
            birthPlaceAdd.Country = countryB;
            if (string.IsNullOrWhiteSpace(txtMunicipality3.Text) == false)
                birthPlaceAdd.Municipality = txtMunicipality3.Text;
            if (string.IsNullOrWhiteSpace(txtProvince3.Text) == false)
                birthPlaceAdd.Province = txtProvince3.Text;

            
            PersonIdentification personId1 = new PersonIdentification();
            if (string.IsNullOrWhiteSpace(cmbIdType1.Text) == false && string.IsNullOrWhiteSpace(txtIDNumber1.Text))
            {
                personId1.IdentificationTypeId = int.Parse(cmbIdType1.SelectedItem.Value);
                personId1.PartyId = newParty.Id;
                personId1.IdNumber = txtIDNumber1.Text;
            }


            PersonIdentification personId2 = new PersonIdentification();
            if (string.IsNullOrWhiteSpace(cmbIdType2.Text) == false && string.IsNullOrWhiteSpace(txtIDNumber2.Text))
            {
                personId2.IdentificationTypeId = int.Parse(cmbIdType2.SelectedItem.Value);
                personId2.PartyId = newParty.Id;
                personId2.IdNumber = txtIDNumber2.Text;
            }

            //insert Contact Information
            var postalAddCatType = context.AddressTypes.FirstOrDefault(entity => entity.Name == AddressTypeEnums.PostalAddress);
            Address postalAddPrim = AddressUtility.AddAddress(newParty, postalAddCatType, today);

            var postalAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == PostalAddressTypeEnums.BusinessAddress);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(postalAddressType);
            
            //Primary Home Address
            PostalAddress specificPostalAddress1 = AddressUtility.AddPostal(postalAddPrim, postalAddressType, true);
            Country country = context.Countries.SingleOrDefault(entity => entity.Name == txtCountry1.Text);
            if (string.IsNullOrWhiteSpace(txtStreetAdd1.Text) == false)
                specificPostalAddress1.StreetAddress = txtStreetAdd1.Text;
            if (string.IsNullOrWhiteSpace(txtBarangay1.Text) == false)
                specificPostalAddress1.Barangay = txtBarangay1.Text;
            if (string.IsNullOrWhiteSpace(txtCity1.Text) == false)
                specificPostalAddress1.City = txtCity1.Text;
            if (string.IsNullOrWhiteSpace(txtState1.Text) == false)
                specificPostalAddress1.State = txtState1.Text;
            if (string.IsNullOrWhiteSpace(txtPostal1.Text) == false)
                specificPostalAddress1.PostalCode = txtPostal1.Text;
            specificPostalAddress1.Country = country;
            if (string.IsNullOrWhiteSpace(txtMunicipality1.Text) == false)
                specificPostalAddress1.Municipality = txtMunicipality1.Text;
            if (string.IsNullOrWhiteSpace(txtProvince1.Text) == false)
                specificPostalAddress1.Province = txtProvince1.Text;


            //Secondary Home Address
            PostalAddress specificPostalAddress2 = AddressUtility.AddPostal(postalAddPrim, postalAddressType, false);
            Country country2 = context.Countries.SingleOrDefault(entity => entity.Name == txtCountry2.Text);
            if (string.IsNullOrWhiteSpace(txtStreetAdd2.Text) == false)
                specificPostalAddress2.StreetAddress = txtStreetAdd2.Text;
            if (string.IsNullOrWhiteSpace(txtBarangay2.Text) == false)
                specificPostalAddress2.Barangay = txtBarangay2.Text;
            if (string.IsNullOrWhiteSpace(txtCity2.Text) == false)
                specificPostalAddress2.City = txtCity2.Text;
            if (string.IsNullOrWhiteSpace(txtState2.Text) == false)
                specificPostalAddress2.State = txtState2.Text;
            if (string.IsNullOrWhiteSpace(txtPostal2.Text) == false)
                specificPostalAddress2.PostalCode = txtPostal2.Text;
            specificPostalAddress2.Country = country2;
            if (string.IsNullOrWhiteSpace(txtMunicipality2.Text) == false)
                specificPostalAddress2.Municipality = txtMunicipality2.Text;
            if (string.IsNullOrWhiteSpace(txtProvince2.Text) == false)
                specificPostalAddress2.Province = txtProvince2.Text;


            //Cellphone Number
            var telAddressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.TelecommunicationNumber);
            Address telCommAddress = AddressUtility.AddAddress(newParty, telAddressType, today);
            var telCellType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.PersonalMobileNumber);
            TelecommunicationsNumber specificCellAddress = AddressUtility.AddTelNum(telCommAddress, telCellType, false);
            if (string.IsNullOrWhiteSpace(txtCellAreaCode.Text) == false)
                specificCellAddress.AreaCode = txtCellAreaCode.Text;
            if (string.IsNullOrWhiteSpace(txtCellPhoneNumber.Text) == false)
                specificCellAddress.PhoneNumber = txtCellPhoneNumber.Text;

            //Telephone Number
            var telType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == TelecommunicationsNumberTypeEnums.HomePhoneNumber);
            TelecommunicationsNumber specificTelephoneAddress = AddressUtility.AddTelNum(telCommAddress, telType, true);
            if (string.IsNullOrWhiteSpace(txtTelAreaCode.Text) == false)
                specificTelephoneAddress.AreaCode = txtTelAreaCode.Text;
            if (string.IsNullOrWhiteSpace(txtTelPhoneNumber.Text) == false)
                specificTelephoneAddress.PhoneNumber = txtTelPhoneNumber.Text;

            var emailAddType = context.AddressTypes.SingleOrDefault(entity => entity.Name == AddressTypeEnums.ElectronicAddress);
            var emailType = context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == ElectronicAddressTypeEnums.PersonalEmailAddress);
            Address email = AddressUtility.AddAddress(partyRole.Party, emailAddType, today);

            if (txtPrimaryEmailAddress.Text != "")
            {
                //Primary Email
                ElectronicAddress specificEmail1 = AddressUtility.AddEmail(email, emailType, true);
                specificEmail1.ElectronicAddressString = txtPrimaryEmailAddress.Text;
            }
            if (txtSecondaryEmailAddress.Text != "")
            {
                //Secondary Email
                ElectronicAddress specificEmail2 = AddressUtility.AddEmail(email, emailType, false);
                specificEmail2.ElectronicAddressString = txtSecondaryEmailAddress.Text;
            }


            //Insert Employment Information
            var empPartyRelType = context.PartyRelTypes.FirstOrDefault(entity => entity.Name == PartyRelationshipTypeEnum.Employment);
            var empRoleType = context.RoleTypes.FirstOrDefault(entity => entity.Name == RoleTypeEnums.Employee);

            //Update party Relationship
            PartyRelationship updatedPartyRel = context.PartyRelationships.FirstOrDefault(entity => entity.Id == partyRel.Id);
            updatedPartyRel.EndDate = today;

            PartyRole partyRoleEmp = new PartyRole();
            partyRoleEmp.PartyId = newParty.Id;
            partyRoleEmp.RoleTypeId = empRoleType.Id;
            partyRoleEmp.EffectiveDate = today;

            PartyRelationship partyRelEmp = new PartyRelationship();
            partyRelEmp.FirstPartyRoleId = partyRoleEmp.Id;
            partyRelEmp.SecondPartyRoleId = int.Parse(this.hiddenEmployerID.Text);
            partyRelEmp.PartyRelTypeId = empPartyRelType.Id;
            partyRelEmp.EffectiveDate = today;

            Employee emp = new Employee();
            emp.PartyRole= partyRoleEmp;
            emp.EmployeeIdNumber = txtEmpIDNumber.Text;
            emp.Position = txtEmpPosition.Text;
            emp.SssNumber = txtSssNumber.Text;
            emp.GsisNumber = txtGsisNumber.Text;
            emp.OwaNumber = txtOWANumber.Text;
            emp.PhicNumber = txtPhicNumber.Text;

            Employment employ = new Employment();
            employ.PartyRelationshipId = partyRelEmp.Id;
            employ.EmploymentStatus = txtEmploymentStatus.Text;
            employ.Salary = txtSalary.Text;

            //Other Sources of Income - New Record
            //var sourceOfIncomeID =

            //Spouse Information
            var spouseRelType = context.PartyRelTypes.FirstOrDefault(entity => entity.Name == PartyRelationshipTypeEnum.SpousalRelationship);
            var spouseRoleType = context.RoleTypes.FirstOrDefault(entity => entity.Name == RoleTypeEnums.Spouse);

            //Insert spouse into table
            Party spouseParty = new Party();
            var personRelType = context.PartyTypes.FirstOrDefault(entity => entity.Name == PartyTypeEnums.Person);
            spouseParty.PartyTypeId = personRelType.Id;

            Person spousePerson = new Person();
            if (rdFemale.Checked == true)
            {
                spousePerson.GenderTypeId = (int)rdMale.Value;
            }
            else
            {
                spousePerson.GenderTypeId = (int)rdFemale.Value;
            }
            spousePerson.Birthdate = datSpouseBirthdate.SelectedDate;

            //Spouse Name
            PersonName spouseFirstName = new PersonName(); //first name
            spouseFirstName.PartyId = spouseParty.Id;
            spouseFirstName.EffectiveDate = today;
            spouseFirstName.Name = txtSpouseFirstName.Text;
            spouseFirstName.PersonNameType = firstNameID;

            PersonName spouseLastName = new PersonName(); //last name
            spouseLastName.PartyId = spouseParty.Id;
            spouseLastName.EffectiveDate = today;
            spouseLastName.Name = txtSpouseLastName.Text;
            spouseLastName.PersonNameType = lastNameID;

            if (txtSpouseMiddleName.Text != "")
            {
                PersonName spouseMName = new PersonName(); //middle name
                spouseMName.PartyId = spouseParty.Id;
                spouseMName.EffectiveDate = today;
                spouseMName.Name = txtSpouseMiddleName.Text;
                spouseMName.PersonNameType = middleNameID;
            }

            if (txtSpouseTitle.Text != "")
            {
                PersonName spouseTitleName = new PersonName(); //title name
                spouseTitleName.PartyId = spouseParty.Id;
                spouseTitleName.EffectiveDate = today;
                spouseTitleName.Name = txtSpouseTitle.Text;
                spouseTitleName.PersonNameType = titleID;
            }

            if (txtSpouseNameSuffix.Text != "")
            {
                PersonName spouseNameSuffix = new PersonName(); //name suffix
                spouseNameSuffix.PartyId = spouseParty.Id;
                spouseNameSuffix.EffectiveDate = today;
                spouseNameSuffix.Name = txtSpouseNameSuffix.Text;
                spouseNameSuffix.PersonNameType = nameSuffixID;
            }

            if (txtSpouseNickName.Text != "")
            {
                PersonName spouseNickName = new PersonName(); //nick name
                spouseNickName.PartyId = spouseParty.Id;
                spouseNickName.EffectiveDate = today;
                spouseNickName.Name = txtSpouseNickName.Text;
                spouseNickName.PersonNameType = nickNameID;
            }

            if (txtSpouseMothersMaidenName.Text != "")
            {
                PersonName spouseMotherMaidenName = new PersonName(); //mother's maiden name
                spouseMotherMaidenName.PartyId = spouseParty.Id;
                spouseMotherMaidenName.EffectiveDate = today;
                spouseMotherMaidenName.Name = txtSpouseMothersMaidenName.Text;
                spouseMotherMaidenName.PersonNameType = motherMaidenNameID;
            }

            PartyRole spousePartyRole = new PartyRole();
            spousePartyRole.PartyId = spouseParty.Id;
            spousePartyRole.RoleTypeId = spouseRoleType.Id;
            spousePartyRole.EffectiveDate = today;

            PartyRelationship spousePartRelationship = new PartyRelationship();
            spousePartRelationship.FirstPartyRoleId = newParty.Id;
            spousePartRelationship.SecondPartyRoleId = spousePartyRole.Id;
            spousePartRelationship.PartyRelTypeId = spouseRelType.Id;
            spousePartRelationship.EffectiveDate = today;

            
            Customer customr = context.Customers.FirstOrDefault(entity => entity.PartyRoleId == cust.PartyRoleId);
            if (!string.IsNullOrWhiteSpace(txtRemarks.Text))
                customr.Remarks = txtRemarks.Text;
            if (!string.IsNullOrWhiteSpace(txtCreditLimit.Text))
            customr.CreditLimit = Int64.Parse(txtCreditLimit.Text);


            //end contact relationship if the user uses the existing contact 
            //record to create the Customer Record
            //int c = 0;
            if (this.hiddenUsesExistRecord.Text == "true")
            {
                var contactRelType = context.PartyRelTypes.FirstOrDefault(entity => entity.Name == PartyRelationshipTypeEnum.ContactRelationship);
                //update data in table
                int custID = int.Parse(this.CustomerID.Text);
                PartyRelationship contactPartyRel = context.PartyRelationships.FirstOrDefault(entity => entity.EndDate == null 
                    && entity.PartyRelTypeId == contactRelType.Id && entity.FirstPartyRoleId == custID);
                //and first_party_role_id = 
                //party role id of the selected allowed user with a role type of ‘Contact’
                contactPartyRel.EndDate = today;

            }

            context.SaveChanges();
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
                txtBirthPlace.Text = txtStreetAdd3.Text;
            if (!(string.IsNullOrWhiteSpace(txtBarangay3.Text)))
                txtBirthPlace.Text += ", " + txtBarangay3.Text;
            if (!(string.IsNullOrWhiteSpace(txtCity3.Text)) && string.IsNullOrWhiteSpace(txtMunicipality3.Text))
                txtBirthPlace.Text += ", " + txtCity3.Text;
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality3.Text)) && string.IsNullOrWhiteSpace(txtCity3.Text))
                txtBirthPlace.Text += ", " + txtMunicipality3.Text;
            if (!(string.IsNullOrWhiteSpace(txtProvince3.Text)))
                txtBirthPlace.Text += ", " + txtProvince3.Text;
            if (!(string.IsNullOrWhiteSpace(txtCountry3.Text)))
                txtBirthPlace.Text += ", " + txtCountry3.Text;
            if (!(string.IsNullOrWhiteSpace(txtPostal3.Text)))
                txtBirthPlace.Text += " " + txtPostal3.Text;

            wndBirthPlaceAddDetail.Hide();
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

            txtPersonName.Text = null;
            txtPersonName.Text = txtPersonLastName.Text + ", " + txtPersonFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtPersonMiddleName.Text)))
                txtPersonName.Text += " " + txtPersonMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtPersonNameSuffix.Text)))
                txtPersonName.Text += " " + txtPersonNameSuffix.Text;

            
            wndPersonNameDetail.Hide();

            //checkCustomerName();
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

            txtSpouseName.Text = null;
            txtSpouseName.Text = txtSpouseLastName.Text + ", " + txtSpouseFirstName.Text;
            if (!(string.IsNullOrWhiteSpace(txtSpouseMiddleName.Text)))
                txtSpouseName.Text += " " + txtSpouseMiddleName.Text[0] + ".";
            if (!(string.IsNullOrWhiteSpace(txtSpouseNameSuffix.Text)))
                txtSpouseName.Text += " " + txtPersonNameSuffix.Text;

            wndSpouseNameDetail.Hide();
        }

        protected void checkCustomerName()
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
        }

        private void RetrieveExistingCustomerInfo(FinancialEntities context, int partyID)
        {
            this.hiddenUsesExistRecord.Value = "true";
            Party party = context.Parties.FirstOrDefault(entity => entity.Id == partyID);
            Person person = context.People.FirstOrDefault(entity => entity.PartyId == partyID);
            int genderId = -1;
            genderId = (int)person.GenderTypeId;
            if (genderId != -1)
            {
                if (genderId == 1)
                {
                    rdFemale.Checked = true;
                }
                else
                {
                    rdMale.Checked = true;
                }
            }

            if(person.Birthdate != null)
                datBirthdate.SelectedDate = (DateTime)person.Birthdate;
            if(person.ImageFilename != null)
                PersonImageFile.ImageUrl = person.ImageFilename;

            var employmentRelType = context.PartyRelTypes.FirstOrDefault(entity => entity.Name == PartyRelationshipTypeEnum.Employment);

            RetrieveEmploymentInfo(context, party, employmentRelType, true);

            PartyRole existingCustPartyRole = context.PartyRoles.FirstOrDefault(entity => entity.PartyId == party.Id && entity.EndDate == null);
            this.CustomerID.Value = existingCustPartyRole.Id;
            var countryID = -1;

            //Customer Primary Home Address
            if (existingCustPartyRole.Party != null)
            {
                PostalAddress primaryPostalAddress = PostalAddressExtension.GetCurrentPostalAddress(context, existingCustPartyRole.Party,
                    PostalAddressTypeEnums.GetBusinessAddressType(context), entity=>entity.PostalAddress.IsPrimary);
                
                if (primaryPostalAddress != null)
                {
                    hiddenPostalAddressId.Value = primaryPostalAddress.AddressId;

                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.StreetAddress))
                    {
                        txtPrimaryHomeAddress.Text = primaryPostalAddress.StreetAddress;
                        txtStreetAdd1.Text = primaryPostalAddress.StreetAddress;
                    }
                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.Barangay))
                    {
                        txtPrimaryHomeAddress.Text += ", " + primaryPostalAddress.Barangay;
                        txtBarangay1.Text = primaryPostalAddress.Barangay;
                    }
                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.City) && string.IsNullOrWhiteSpace(primaryPostalAddress.Municipality))
                    {
                        txtPrimaryHomeAddress.Text += ", " + primaryPostalAddress.City;
                        txtCity1.Text = primaryPostalAddress.City;
                    }
                    else if (!string.IsNullOrWhiteSpace(primaryPostalAddress.Municipality) && string.IsNullOrWhiteSpace(primaryPostalAddress.City))
                    {
                        txtPrimaryHomeAddress.Text += ", " + primaryPostalAddress.Municipality;
                        txtMunicipality1.Text = primaryPostalAddress.Municipality;
                    }
                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.Province))
                    {
                        txtPrimaryHomeAddress.Text += ", " + primaryPostalAddress.Province;
                        txtProvince1.Text = primaryPostalAddress.Province;
                    }
                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.Country.Name))
                    {
                        txtPrimaryHomeAddress.Text += ", " + primaryPostalAddress.Country.Name;
                        txtCountry1.Text = primaryPostalAddress.CountryId.ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(primaryPostalAddress.PostalCode))
                    {
                        txtPrimaryHomeAddress.Text += " " + primaryPostalAddress.PostalCode;
                        txtPostal1.Text = primaryPostalAddress.PostalCode;
                    }

                    countryID = (int)primaryPostalAddress.CountryId;
                }

                PostalAddress secondaryPostalAddress = PostalAddressExtension.GetCurrentPostalAddress(context, existingCustPartyRole.Party,
                    PostalAddressTypeEnums.GetBusinessAddressType(context), entity => entity.PostalAddress.IsPrimary == false);

                if (secondaryPostalAddress != null)
                {
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.StreetAddress))
                    {
                        txtSecondaryHomeAddress.Text = secondaryPostalAddress.StreetAddress;
                        txtStreetAdd2.Text = secondaryPostalAddress.StreetAddress;
                    }
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.Barangay))
                    {
                        txtSecondaryHomeAddress.Text += ", " + secondaryPostalAddress.Barangay;
                        txtBarangay2.Text = secondaryPostalAddress.Barangay;
                    }
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.City) && string.IsNullOrWhiteSpace(secondaryPostalAddress.Municipality))
                    {
                        txtSecondaryHomeAddress.Text += ", " + secondaryPostalAddress.City;
                        txtCity2.Text = secondaryPostalAddress.City;
                    }
                    else if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.Municipality) && string.IsNullOrWhiteSpace(secondaryPostalAddress.City))
                    {
                        txtSecondaryHomeAddress.Text += ", " + secondaryPostalAddress.Municipality;
                        txtMunicipality2.Text = secondaryPostalAddress.Municipality;
                    }
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.Province))
                    {
                        txtSecondaryHomeAddress.Text += ", " + secondaryPostalAddress.Province;
                        txtProvince2.Text = secondaryPostalAddress.Province;
                    }
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.Country.Name))
                    {
                        txtSecondaryHomeAddress.Text += ", " + secondaryPostalAddress.Country.Name;
                        txtCountry2.Text = secondaryPostalAddress.CountryId.ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(secondaryPostalAddress.PostalCode))
                    {
                        txtSecondaryHomeAddress.Text += " " + secondaryPostalAddress.PostalCode;
                        txtPostal2.Text = secondaryPostalAddress.PostalCode;
                    }

                    countryID = (int)secondaryPostalAddress.CountryId;
                }
            }

            //existing customer name
            List<PersonName> personName = context.PersonNames.Where(entity => entity.PartyId == party.Id && entity.EndDate == null).OrderBy(entity =>
                    entity.PersonNameTypeId).ToList();

            PersonName title = new PersonName();
            PersonName lastName = new PersonName();
            PersonName firstName = new PersonName();
            PersonName middleName = new PersonName();
            PersonName nameSuffix = new PersonName();
            PersonName nickName = new PersonName();
            PersonName mothersMaidenName = new PersonName();

            title = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.Title);
            lastName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.LastName);
            firstName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.FirstName);
            middleName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.MiddleName);
            nameSuffix = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.NameSuffix);
            nickName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.NickName);
            mothersMaidenName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.MothersMaidenName);

            if (lastName != null)
                txtPersonName.Text = lastName.Name;
            if (firstName != null)
                txtPersonName.Text += ", " + firstName.Name;
            if (middleName != null)
                txtPersonName.Text += " " + middleName.Name[0] + ".";
            if (nameSuffix != null)
                txtPersonName.Text += " " + nameSuffix.Name;

            //Customer Telephone Number
            var telecomAddType = context.Addresses.FirstOrDefault(entity => entity.AddressType.Name == AddressTypeEnums.TelecommunicationNumber
                && entity.Party.Id == party.Id && entity.EndDate == null);

            if (telecomAddType != null)
            {
                var primTelNum = context.TelecommunicationsNumbers.FirstOrDefault(entity => entity.TelecommunicationsNumberType.Name
                    == TelecommunicationsNumberTypeEnums.HomePhoneNumber
                    && entity.AddressId == telecomAddType.AddressId && entity.IsPrimary == true);
                if (countryID != -1)
                {
                    var telCode = context.Countries.FirstOrDefault(entity => entity.Id == countryID);
                    if (telCode != null)
                        txtTelCodes.Text = telCode.CountryTelephoneCode;
                }
                if (primTelNum != null)
                {
                    txtTelAreaCode.Text = primTelNum.AreaCode;
                    txtTelPhoneNumber.Text = primTelNum.PhoneNumber;
                }

                var cellAddType = context.TelecommunicationsNumbers.FirstOrDefault(entity =>
                    entity.TelecommunicationsNumberType.Name == TelecommunicationsNumberTypeEnums.PersonalMobileNumber
                    && entity.AddressId == telecomAddType.AddressId && entity.IsPrimary == true);

                if (cellAddType != null)
                {
                    if (countryID != -1)
                    {
                        var telCode = context.Countries.FirstOrDefault(entity => entity.Id == countryID);
                        if (telCode != null)
                            txtCellTelCodes.Text = telCode.CountryTelephoneCode;
                    }

                    txtCellAreaCode.Text = cellAddType.AreaCode;
                    txtCellPhoneNumber.Text = cellAddType.PhoneNumber;
                }

            }

            //Customer Email Address
            var electronicAddType = context.Addresses.FirstOrDefault(entity => entity.AddressType.Name == AddressTypeEnums.ElectronicAddress
               && entity.Party.Id == party.Id && entity.EndDate == null);

            if (electronicAddType != null)
            {
                var primCustomerEmail = context.ElectronicAddresses.FirstOrDefault(entity => entity.ElectronicAddressType.Name
                    == ElectronicAddressTypeEnums.PersonalEmailAddress
                    && entity.AddressId == electronicAddType.AddressId && entity.IsPrimary == true);

                if (primCustomerEmail != null)
                {
                    txtPrimaryEmailAddress.Text = primCustomerEmail.ElectronicAddressString;

                }

                var secCustomerEmail = context.ElectronicAddresses.FirstOrDefault(entity => entity.ElectronicAddressType.Name
                    == ElectronicAddressTypeEnums.PersonalEmailAddress
                    && entity.AddressId == electronicAddType.AddressId && entity.IsPrimary == false);

                if (secCustomerEmail != null)
                {
                    txtPrimaryEmailAddress.Text = secCustomerEmail.ElectronicAddressString;

                }
            }

        }

        private void RetrieveEmploymentInfo(FinancialEntities context, Party party, PartyRelType empRelType, bool isExisting)
        {
            PartyRole partyRole = context.PartyRoles.FirstOrDefault(entity => entity.PartyId == party.Id && entity.EndDate == null);
            PartyRelationship empPartyRel = context.PartyRelationships.FirstOrDefault(entity => entity.FirstPartyRoleId == partyRole.Id
                && entity.PartyRelType.Id == empRelType.Id && entity.EndDate == null);

            this.hiddenEmployerID.Value = empPartyRel.SecondPartyRoleId;
            if (partyRole.Employee.EmployeeIdNumber != null)
                txtEmpIDNumber.Text = partyRole.Employee.EmployeeIdNumber;
            if (partyRole.Employee.Position != null)
                txtEmpPosition.Text = partyRole.Employee.Position;
            if (partyRole.Employee.SssNumber != null)
                txtSssNumber.Text = partyRole.Employee.SssNumber;
            if (partyRole.Employee.GsisNumber != null)
                txtGsisNumber.Text = partyRole.Employee.GsisNumber;
            if (partyRole.Employee.OwaNumber != null)
                txtOWANumber.Text = partyRole.Employee.OwaNumber;
            if (partyRole.Employee.PhicNumber != null)
                txtPhicNumber.Text = partyRole.Employee.PhicNumber;

            //employment information
            if (empPartyRel.Employment.EmploymentStatus != null)
                txtEmploymentStatus.Text = empPartyRel.Employment.EmploymentStatus;
            if (empPartyRel.Employment.Salary != null)
                txtSalary.Text = empPartyRel.Employment.Salary;

            //employer address
            PartyRole empPartyRole = context.PartyRoles.FirstOrDefault(entity => entity.Id == empPartyRel.SecondPartyRoleId && entity.EndDate == null);
            Party empParty = context.Parties.FirstOrDefault(entity => entity.Id == empPartyRole.PartyId);

            var countryID = -1;
            var postalAddType = context.Addresses.FirstOrDefault(entity => entity.AddressType.Name == AddressTypeEnums.PostalAddress && entity.Party.Id == empParty.Id 
                && entity.EndDate == null);

            if (postalAddType != null)
            {
                var businessAddType = context.PostalAddresses.FirstOrDefault(entity => entity.PostalAddressType.Name == PostalAddressTypeEnums.BusinessAddress 
                    && entity.Address.AddressId == postalAddType.AddressId && entity.IsPrimary == true);

                if (businessAddType != null)
                {
                    if (!string.IsNullOrWhiteSpace(businessAddType.StreetAddress))
                        txtEmploymentAddress.Text = businessAddType.StreetAddress;
                    if (!string.IsNullOrWhiteSpace(businessAddType.Barangay))
                        txtEmploymentAddress.Text += ", " + businessAddType.Barangay;
                    if (!string.IsNullOrWhiteSpace(businessAddType.City) && string.IsNullOrWhiteSpace(businessAddType.Municipality))
                        txtEmploymentAddress.Text += ", " + businessAddType.City;
                    else if (!string.IsNullOrWhiteSpace(businessAddType.Municipality) && string.IsNullOrWhiteSpace(businessAddType.City))
                        txtEmploymentAddress.Text += ", " + businessAddType.Municipality;
                    if (!string.IsNullOrWhiteSpace(businessAddType.Province))
                        txtEmploymentAddress.Text += ", " + businessAddType.Province;
                    if (!string.IsNullOrWhiteSpace(businessAddType.Country.Name))
                        txtEmploymentAddress.Text += ", " + businessAddType.Country.Name;
                    if (!string.IsNullOrWhiteSpace(businessAddType.PostalCode))
                        txtEmploymentAddress.Text += " " + businessAddType.PostalCode;

                    countryID = (int)businessAddType.CountryId;
                }
            }

            //Employer Telephone Number
            var telecomAddType = context.Addresses.FirstOrDefault(entity => entity.AddressType.Name == AddressTypeEnums.TelecommunicationNumber 
                && entity.Party.Id == empParty.Id && entity.EndDate == null);

            //Employer Name
            if (empParty.PartyType.Name == PartyTypeEnums.Organization) //if employer party type is organization
            {
                Organization org = context.Organizations.FirstOrDefault(entity => entity.PartyId == empParty.Id);
                if (!string.IsNullOrWhiteSpace(org.OrganizationName))
                    txtEmployerName.Text = org.OrganizationName;
            }
            else if (empParty.PartyType.Name == PartyTypeEnums.Person) //if employer party type is person
            {
                Person person = context.People.FirstOrDefault(entity => entity.PartyId == empParty.Id);
                List<PersonName> personName = context.PersonNames.Where(entity => entity.PartyId == empParty.Id && entity.EndDate == null).OrderBy(entity => 
                    entity.PersonNameTypeId).ToList();

                PersonName title = new PersonName();
                PersonName lastName = new PersonName();
                PersonName firstName = new PersonName();
                PersonName middleName = new PersonName();
                PersonName nameSuffix = new PersonName();
                PersonName nickName = new PersonName();
                PersonName mothersMaidenName = new PersonName();

                title = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.Title);
                lastName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.LastName);
                firstName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.FirstName);
                middleName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.MiddleName);
                nameSuffix = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.NameSuffix);
                nickName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.NickName);
                mothersMaidenName = PersonNameExtension.GetPersonNameByTypeEntity(context, person, PersonNameTypeEnums.MothersMaidenName);

                if (lastName != null)
                    txtEmployerName.Text = lastName.Name;
                if (firstName != null)
                    txtEmployerName.Text += ", " + firstName.Name;
                if (middleName != null)
                    txtEmployerName.Text += " " + middleName.Name[0] + ".";
                if (nameSuffix != null)
                    txtEmployerName.Text += " " + nameSuffix.Name;
            }

            //Employer Telecommunication Numbers
            if (telecomAddType != null)
            {
                var primTelNum = context.TelecommunicationsNumbers.FirstOrDefault(entity => entity.TelecommunicationsNumberType.Name
                    == TelecommunicationsNumberTypeEnums.BusinessPhoneNumber
                    && entity.AddressId == telecomAddType.AddressId && entity.IsPrimary == true);
                if (countryID != -1)
                {
                    var telCode = context.Countries.FirstOrDefault(entity => entity.Id == countryID);
                    if (telCode != null)
                        txtEmpTelCode.Text = telCode.CountryTelephoneCode;
                }
                if (primTelNum != null)
                {
                    txtEmpAreaCode.Text = primTelNum.AreaCode;
                    txtEmpPhoneNumber.Text = primTelNum.PhoneNumber;
                }

                var businessFaxAddType = context.TelecommunicationsNumbers.FirstOrDefault(entity =>
                    entity.TelecommunicationsNumberType.Name == TelecommunicationsNumberTypeEnums.BusinessFaxNumber
                    && entity.AddressId == telecomAddType.AddressId && entity.IsPrimary == true);

                if (businessFaxAddType != null)
                {
                    if (countryID != -1)
                    {
                        var telCode = context.Countries.FirstOrDefault(entity => entity.Id == countryID);
                        if (telCode != null)
                            txtEmpFaxTelCode.Text = telCode.CountryTelephoneCode;
                    }

                    txtEmpFaxAreaCode.Text = businessFaxAddType.AreaCode;
                    txtEmpFaxNumber.Text = businessFaxAddType.PhoneNumber;
                }

            }

            //Employer Email Address
            var electronicAddType = context.Addresses.FirstOrDefault(entity => entity.AddressType.Name == AddressTypeEnums.ElectronicAddress
               && entity.Party.Id == empParty.Id && entity.EndDate == null);

            if (electronicAddType != null)
            {
                var primEmpEmail = context.ElectronicAddresses.FirstOrDefault(entity => entity.ElectronicAddressType.Name
                    == ElectronicAddressTypeEnums.BusinessEmailAddress
                    && entity.AddressId == electronicAddType.AddressId && entity.IsPrimary == true);

                if (primEmpEmail != null)
                {
                    txtEmpEmailAddress.Text = primEmpEmail.ElectronicAddressString;

                }
            }


        }

        private void RetrieveCustomerInformation(FinancialEntities context)
        {
            int selectedPartyID = (int)this.RecordID.Value;

            var partyRole = context.PartyRoles.SingleOrDefault(entity => entity.Id == selectedPartyID);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRole>(partyRole);

            var party = context.Parties.SingleOrDefault(entity => entity.Id == partyRole.PartyId);
            InitialDatabaseValueChecker.ThrowIfNull<Party>(party);

            var person = context.People.SingleOrDefault(entity => entity.PartyId == partyRole.PartyId);
            InitialDatabaseValueChecker.ThrowIfNull<Person>(person);

            var genderType = context.GenderTypes.SingleOrDefault(entity => entity.Id == person.GenderTypeId);
            InitialDatabaseValueChecker.ThrowIfNull<GenderType>(genderType);

            if (genderType.Name == "Female")
            {
                rdFemale.Checked = true;
            }
            else
            {
                rdMale.Checked = true;
            }

            datBirthdate.SelectedDate = (DateTime)person.Birthdate;
            PersonImageFile.ImageUrl = person.ImageFilename;


            var personName = context.PersonNames.Where(entity => entity.PartyId == selectedPartyID && entity.EndDate == null).OrderBy(entity => entity.PersonNameTypeId).ToList();
            var title = "";
            var firstName = "";
            var middleName = "";
            var lastName = "";
            var nickName = "";
            var nameSuffix = "";
            var motherMaidenName = "";
            var titleID = GetPersonNameType(context, PersonNameTypeEnums.Title);
            var firstNameID = GetPersonNameType(context, PersonNameTypeEnums.FirstName);
            var middleNameID = GetPersonNameType(context, PersonNameTypeEnums.MiddleName);
            var lastNameID = GetPersonNameType(context, PersonNameTypeEnums.LastName);
            var nickNameID = GetPersonNameType(context, PersonNameTypeEnums.NickName);
            var nameSuffixID = GetPersonNameType(context, PersonNameTypeEnums.NameSuffix);
            var motherMaidenNameID = GetPersonNameType(context, PersonNameTypeEnums.MothersMaidenName);

            foreach (var name in personName)
            {
                if(name.PersonNameTypeId == titleID.Id){
                    title = name.Name;
                }
                else if(name.PersonNameTypeId == firstNameID.Id)
                {
                    firstName = name.Name;
                }
                else if(name.PersonNameTypeId == middleNameID.Id)
                {
                    middleName = name.Name;
                }
                else if (name.PersonNameTypeId == lastNameID.Id)
                {
                    lastName = name.Name;
                }
                else if (name.PersonNameTypeId == nickNameID.Id)
                {
                    nickName = name.Name;
                }
                else if (name.PersonNameTypeId == nameSuffixID.Id)
                {
                    nameSuffix = name.Name;
                }
                else if (name.PersonNameTypeId == motherMaidenNameID.Id)
                {
                    motherMaidenName = name.Name;
                }
            }

            txtPersonName.Text = lastName + ", " + firstName;
            if (middleName != "")
            {
                txtPersonName.Text += " " + middleName[0] + ".";
            }

            if (nameSuffix != "")
            {
                txtPersonName.Text += " " + nameSuffix;
            }

            //Retrieve Contact Information
            //Primary Home Address
            var postalAddressType = GetAddressType(context, AddressTypeEnums.PostalAddress);
            var addressPostal = context.Addresses.SingleOrDefault(entity => entity.PartyId == selectedPartyID && entity.AddressTypeId == postalAddressType.Id);
            InitialDatabaseValueChecker.ThrowIfNull<Address>(addressPostal);

            var homeAddressType = GetPostalAddressType(context, PostalAddressTypeEnums.HomeAddress);

            var postalAddress = context.PostalAddresses.Where(entity => entity.AddressId == addressPostal.AddressId && entity.PostalAddressTypeId == homeAddressType.Id);
            PostalAddress primaryHomeAddress = new PostalAddress();
            PostalAddress secondaryHomeAddress = new PostalAddress();

            foreach (var add in postalAddress)
            {
                if (add.IsPrimary == true)
                {
                    primaryHomeAddress = add;
                }
                else
                {
                    secondaryHomeAddress = add;
                }
            }


            var telcomAddressType = GetAddressType(context, AddressTypeEnums.TelecommunicationNumber);

            var cellNumberTelCommType = GetTelcomNumberType(context, TelecommunicationsNumberTypeEnums.PersonalMobileNumber);
            var telNumberTelCommType = GetTelcomNumberType(context, TelecommunicationsNumberTypeEnums.HomePhoneNumber);

            var electronicAddressType = GetAddressType(context, AddressTypeEnums.ElectronicAddress);

            var emailElectronicAddressType = GetElectronicAddressType(context, ElectronicAddressTypeEnums.PersonalEmailAddress);
            var employmentPartyRelType = GetPartyRelType(context, PartyRelationshipTypeEnum.Employment);

            var employeeRoleType = GetRoleType(context, RoleTypeEnums.Employee);
            var employerRoleType = GetRoleType(context, RoleTypeEnums.Employer);

            var partyRoleArray = context.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == employerRoleType.Id && entity.PartyId == selectedPartyID && entity.EndDate == null);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRole>(partyRoleArray);

            //Retrieve current party relationship record
            var partyRel = context.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == partyRoleArray.Id && entity.EndDate == null && entity.PartyRelTypeId == employmentPartyRelType.Id);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRelationship>(partyRel);

            var employerPartyRoleId = context.PartyRelationships.SingleOrDefault(entity => entity.Id == partyRel.Id);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRelationship>(employerPartyRoleId);

            var employeePartyRoleId = context.PartyRelationships.SingleOrDefault(entity => entity.Id == partyRel.Id);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRelationship>(employeePartyRoleId);

            //Retrieve PartyId of the Employer or Company where the Customer is currently working
            var employerPartyId = context.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == employerRoleType.Id && entity.Id == employerPartyRoleId.SecondPartyRoleId);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRole>(employerPartyId);

            //Retrieve Employer Person Name
            var employmentPersonName = context.PersonNames.SingleOrDefault(entity => entity.PartyId == employerPartyId.Id && entity.EndDate == null);
            InitialDatabaseValueChecker.ThrowIfNull<PersonName>(employmentPersonName);

            var employeeEmploymentInfo = context.Employees.SingleOrDefault(entity => entity.PartyRoleId == employeePartyRoleId.FirstPartyRoleId);
            InitialDatabaseValueChecker.ThrowIfNull<Employee>(employeeEmploymentInfo);

            var employmentInfo = context.Employments.SingleOrDefault(entity => entity.PartyRelationshipId == partyRel.Id);
            InitialDatabaseValueChecker.ThrowIfNull<Employment>(employmentInfo);

            //Employment Address
            var busAddressType = GetPostalAddressType(context, PostalAddressTypeEnums.BusinessAddress);
            var busAddress = context.Addresses.SingleOrDefault(entity => entity.PartyId == employerPartyId.Id && entity.AddressTypeId == busAddressType.Id);
            InitialDatabaseValueChecker.ThrowIfNull<Address>(busAddress);



        }

        private AddressType GetAddressType(FinancialEntities context, string addressTypeName)
        {
            AddressType addressType = new AddressType();
            addressType = context.AddressTypes.SingleOrDefault(entity => entity.Name == addressTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            return addressType;
        }

        private PostalAddressType GetPostalAddressType(FinancialEntities context, string postalAddressTypeName)
        {
            PostalAddressType postalAddressType = new PostalAddressType();
            postalAddressType = context.PostalAddressTypes.SingleOrDefault(entity => entity.Name == postalAddressTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<PostalAddressType>(postalAddressType);
            return postalAddressType;
        }

        private TelecommunicationsNumberType GetTelcomNumberType(FinancialEntities context, string postalAddressTypeName)
        {
            TelecommunicationsNumberType postalAddressType = new TelecommunicationsNumberType();
            postalAddressType = context.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name == postalAddressTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(postalAddressType);
            return postalAddressType;
        }

        private ElectronicAddressType GetElectronicAddressType(FinancialEntities context, string postalAddressTypeName)
        {
            ElectronicAddressType postalAddressType = new ElectronicAddressType();
            postalAddressType = context.ElectronicAddressTypes.SingleOrDefault(entity => entity.Name == postalAddressTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<ElectronicAddressType>(postalAddressType);
            return postalAddressType;
        }

        private PartyRelType GetPartyRelType(FinancialEntities context, string partyRelTypeName)
        {
            PartyRelType partyRelType = new PartyRelType();
            partyRelType = context.PartyRelTypes.SingleOrDefault(entity => entity.Name == partyRelTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<PartyRelType>(partyRelType);
            return partyRelType;
        }

        private RoleType GetRoleType(FinancialEntities context, string roleTypeName)
        {
            RoleType roleType = new RoleType();
            roleType = context.RoleTypes.SingleOrDefault(entity => entity.Name == roleTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<RoleType>(roleType);
            return roleType;
        }

        private PersonNameType GetPersonNameType(FinancialEntities context, string roleTypeName)
        {
            PersonNameType roleType = new PersonNameType();
            roleType = context.PersonNameTypes.SingleOrDefault(entity => entity.Name == roleTypeName);
            InitialDatabaseValueChecker.ThrowIfNull<PersonNameType>(roleType);
            return roleType;
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