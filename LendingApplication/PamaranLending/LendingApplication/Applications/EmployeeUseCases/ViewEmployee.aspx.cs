using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;
using System.IO;

namespace LendingApplication.Applications.EmployeeUseCases
{
    public partial class ViewOrEditEmployee : ActivityPageBase
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

                int partyRoleId = int.Parse(Request.QueryString["id"]);
                var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);

                if(PartyRelationship.GetPartyRelationship(partyRoleId, lenderPartyRole.Id) == null)
                    throw new AccessToDeletedRecordException("The employee record has been deleted by another user.");
                hdnPickedPartyRoleId.Text = partyRoleId.ToString();
                
                
                var countries = ObjectContext.Countries;
                countries.ToList();
                strCountry.DataSource = countries;
                strCountry.DataBind();

                dtBirthDate.MaxDate = DateTime.Now.AddYears(-18);

                var employeehistory = Employee.GetEmployeeHistory(partyRoleId);
                PageGridPanelStore.DataSource = employeehistory.ToList();
                PageGridPanelStore.DataBind();

                EmployeePositionStore.DataSource = EmployeePositionType.All;
                EmployeePositionStore.DataBind();

                RetrieveInfo(partyRoleId);
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            int partyRoleId = int.Parse(hdnPickedPartyRoleId.Text);
            var partyRole = PartyRole.GetById(partyRoleId);

            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                UpdateOrNot(partyRole);
            }
        }

        protected void onUpload_Click(object sender, DirectEventArgs e)
        {
            string msg = "";
            // Check that a file is actually being submitted.
            if (flupPersonImage.PostedFile.FileName == "")
            {
                X.MessageBox.Alert("Alert", "No file specified.").Show();
            }

            else
            {
                // Check the extension.
                string extension = Path.GetExtension(flupPersonImage.PostedFile.FileName);
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
                string serverFileName = Path.GetFileName(flupPersonImage.PostedFile.FileName);
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

                    flupPersonImage.PostedFile.SaveAs(file);
                    msg = "File uploaded successfully.";
                }
                catch (Exception err)
                {
                    msg = err.Message;
                }

                X.MessageBox.Alert("Status", msg).Show();
                imageFilename = "../../../Uploaded/Images/" + flupPersonImage.PostedFile.FileName;
                imgPersonPicture.ImageUrl = imageFilename;
                this.hdnPersonPicture.Value = imageFilename;
            }
        }

        protected void txtEmployeeName_Focus(object sender, DirectEventArgs e) {
            txtTitle.Text = hdnTitle.Text;
            txtFirstName.Text = hdnFirstName.Text;
            txtMiddleName.Text = hdnMiddleName.Text;
            txtLastName.Text = hdnLastName.Text;
            txtSuffix.Text = hdnSuffix.Text;
            txtNickName.Text = hdnNickName.Text;

            wndNameDetailBox.Hidden = false;
        }

        protected void btnDoneNameDetail_Click(object sender, DirectEventArgs e)
        {
            hdnTitle.Text = txtTitle.Text;
            hdnFirstName.Text = txtFirstName.Text;
            hdnMiddleName.Text = txtMiddleName.Text;
            hdnLastName.Text = txtLastName.Text;
            hdnSuffix.Text = txtSuffix.Text;
            hdnNickName.Text = txtNickName.Text;

            string middleInitial = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? "" : txtMiddleName.Text[0] + ".";
            txtEmployeeName.Text = StringConcatUtility.Build(" ", txtTitle.Text,
                                                                  txtLastName.Text + ",",
                                                                  txtFirstName.Text,
                                                                  middleInitial,
                                                                  txtSuffix.Text);

            wndNameDetailBox.Hidden = true;
        }

        [DirectMethod]
        public void BrowseAddress_Click()
        {
            wndAddressDetail.Show();
            txtStreetAddress.Text = hdnStreetAddress.Text;
            txtBarangay.Text = hdnBarangay.Text;

            if (hdnCity.Text != null && string.IsNullOrWhiteSpace(hdnMunicipality.Text))
            {
                txtCityOrMunicipality.Text = hdnCity.Text;
                radioCity.Checked = true;
            }
            else if(hdnMunicipality.Text != null && string.IsNullOrWhiteSpace(hdnCity.Text))
            {
                txtCityOrMunicipality.Text = hdnMunicipality.Text;
                radioMunicipality.Checked = true;
            }

            txtProvince.Text = hdnProvince.Text;
            txtPostalCode.Text = hdnPostalCode.Text;
            cmbCountry.SelectedItem.Value = hdnCountryId.Text;
        }

        protected void wndAddressDetail_btnAdd_Click(object sender, DirectEventArgs e)
        {
            hdnStreetAddress.Text = txtStreetAddress.Text;
            hdnBarangay.Text = txtBarangay.Text;
            if (radioCity.Checked)
            {
                hdnCity.Text = txtCityOrMunicipality.Text;
                hdnMunicipality.Text = null;
            }
            else
            {
                hdnCity.Text = null;
                hdnMunicipality.Text = txtCityOrMunicipality.Text;
            }

            hdnProvince.Text = txtProvince.Text;
            hdnPostalCode.Text = txtPostalCode.Text;
            hdnCountryId.Text = cmbCountry.SelectedItem.Value;

            txtPrimaryHomeAddress.Text = StringConcatUtility.Build(", ",
                                                                   txtStreetAddress.Text,
                                                                   txtBarangay.Text,
                                                                   hdnCity.Text,
                                                                   hdnMunicipality.Text,
                                                                   txtProvince.Text,
                                                                   cmbCountry.SelectedItem.Text,
                                                                   txtPostalCode.Text);
            wndAddressDetail.Hidden = true;
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

        public void RetrieveInfo(int partyRoleId)
        {
            //int partyRoleId = int.Parse(hdnPickedPartyRoleId.Text);
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);//Lending Institution
            var EmployeeView = ObjectContext.EmployeeViewLists.SingleOrDefault(entity => entity.EmployeeId == partyRoleId);
            var party = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId).Party;
            var person = party.Person;
            var employee = ObjectContext.Employees.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);
            
            var partyRelationShip = ObjectContext.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == partyRoleId && entity.SecondPartyRoleId == lenderPartyRole.Id
                                                                                        && entity.EndDate == null);
            
            var employment = ObjectContext.Employments.SingleOrDefault(entity => entity.PartyRelationshipId == partyRelationShip.Id);
            var taxPayer = ObjectContext.Taxpayers.SingleOrDefault(entity => entity.PartyRoleId == partyRoleId);

            if (!string.IsNullOrWhiteSpace(person.ImageFilename))
            {
                imgPersonPicture.ImageUrl = person.ImageFilename;
                hdnPersonPicture.Value = person.ImageFilename;
            } 

            txtEmployeeName.Text = EmployeeView.Name;//name

            hdnTitle.Text = person.TitleString;
            hdnFirstName.Text = person.FirstNameString;
            hdnMiddleName.Text = person.MiddleNameString;
            hdnLastName.Text = person.LastNameString;
            hdnSuffix.Text = person.NameSuffixString;
            hdnNickName.Text = person.NickNameString;

            if (person.GenderType == GenderType.MaleType) radioMale.Checked = true;
            else radioFemale.Checked = true;

            dtBirthDate.SelectedDate = (DateTime)person.Birthdate;//birthdate
            txtEmployeeIdNumber.Text = employee.EmployeeIdNumber;//employeeidnumber
            txtPosition.Text = employee.Position;//position
            cmbPosition.SelectedItem.Text = employee.Position;
            cmbEmploymentStatus.SelectedItem.Text = employment.EmploymentStatus;//employmentstatus
            txtSalary.Text = employment.Salary;//salary
            if (taxPayer != null)
            {
                string[] TinArray;
                TinArray = taxPayer.Tin.Split('-');
                txtTIN.Text = TinArray[0];
                txtTIN1.Text = TinArray[1];
                txtTIN2.Text = TinArray[2];
                txtTIN3.Text = TinArray[3];
            }//tin
            txtSSSNumber.Text = employee.SssNumber;//sssnumber
            txtPHICNumber.Text = employee.PhicNumber;//phicnumber

            /**  CONTACT INFO DETAILS  **/

            var postalAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, true);
            txtPrimaryHomeAddress.Text = StringConcatUtility.Build(", ",
                                                                   postalAddress.StreetAddress,
                                                                   postalAddress.Barangay,
                                                                   postalAddress.Municipality,
                                                                   postalAddress.City,
                                                                   postalAddress.Province,
                                                                   postalAddress.Country.Name,
                                                                   postalAddress.PostalCode);
            
            //transfer Address specifics to hiddenFields
            hdnStreetAddress.Text = postalAddress.StreetAddress;
            hdnBarangay.Text = postalAddress.Barangay;
            hdnCity.Text = postalAddress.City;
            hdnMunicipality.Text = postalAddress.Municipality;
            hdnProvince.Text = postalAddress.Province;
            hdnState.Text = postalAddress.State;
            hdnPostalCode.Text = postalAddress.PostalCode;
            hdnCountryId.Text = postalAddress.Country.Id.ToString();

            txtTeleCountryCode.Text = postalAddress.Country.CountryTelephoneCode;
            txtCellCountryCode.Text = postalAddress.Country.CountryTelephoneCode;

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

        public void UpdateOrNot(PartyRole partyRole)
        {
            DateTime today = DateTime.Now;
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);//Lending Institution
            var EmployeeView = ObjectContext.EmployeeViewLists.SingleOrDefault(entity => entity.EmployeeId == partyRole.Id);
            var party = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRole.Id && entity.EndDate == null).Party;
            var person = party.Person;
            var employee = ObjectContext.Employees.SingleOrDefault(entity => entity.PartyRoleId == partyRole.Id);
            var partyRelationShip = ObjectContext.PartyRelationships.SingleOrDefault(entity => entity.FirstPartyRoleId == partyRole.Id && entity.SecondPartyRoleId == lenderPartyRole.Id                                                  && entity.EndDate == null);
            var employment = ObjectContext.Employments.SingleOrDefault(entity => entity.PartyRelationshipId == partyRelationShip.Id);
            string concatinatedTIN = ConcatinateTIN(txtTIN.Text, txtTIN1.Text, txtTIN2.Text, txtTIN3.Text);

            Person.CreateOrUpdatePersonNames(person, PersonNameType.TitleType, hdnTitle.Text, today);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.FirstNameType, hdnFirstName.Text, today);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.MiddleNameType, hdnMiddleName.Text, today);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.LastNameType, hdnLastName.Text, today);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.NameSuffixType, hdnSuffix.Text, today);
            Person.CreateOrUpdatePersonNames(person, PersonNameType.NickNameType, hdnNickName.Text, today);

            /*------------------------------------------------------------------------------------------------------------------*/
            GenderType genderType;
            if(radioFemale.Checked) genderType = GenderType.FemaleType;
            else genderType = GenderType.MaleType;

            if(person.GenderType != genderType) person.GenderType = genderType;//gender
            if(person.Birthdate != dtBirthDate.SelectedDate) person.Birthdate = dtBirthDate.SelectedDate;//birthdate
            
            string stringPersinPictureFile  = hdnPersonPicture.Value.ToString();
            if (person.ImageFilename != stringPersinPictureFile) person.ImageFilename = hdnPersonPicture.Value.ToString();//imagepicture

            if (employee.EmployeeIdNumber != txtEmployeeIdNumber.Text)
            {
                if (string.IsNullOrWhiteSpace(txtEmployeeIdNumber.Text))
                {
                    employee.EmployeeIdNumber = null;
                }
                else
                {
                    employee.EmployeeIdNumber = txtEmployeeIdNumber.Text;//employeeidnumber
                }
            }
            if (employee.Position != cmbPosition.SelectedItem.Text) employee.Position = cmbPosition.SelectedItem.Text;//position
            if(employment.EmploymentStatus != cmbEmploymentStatus.SelectedItem.Text) employment.EmploymentStatus = cmbEmploymentStatus.SelectedItem.Text;//employmentstatus
            if(employment.Salary != txtSalary.Text) employment.Salary = txtSalary.Text;//salary

            Taxpayer.CreateOrUpdateTaxPayer(partyRole, concatinatedTIN);//tin

            if(employee.SssNumber != txtSSSNumber.Text) employee.SssNumber = txtSSSNumber.Text;//sssnumber
            if(employee.PhicNumber != txtPHICNumber.Text) employee.PhicNumber = txtPHICNumber.Text;//phicnumber
            /*------------------------------------------------------------------------------------------------------------------*/

            /**  CONTACT INFO DETAILS  **/

            /*------------------------------------------------------------------------------------------------------------------*/
            //Primary Home Address
            PostalAddress newPostalAddress = new PostalAddress();
            newPostalAddress.StreetAddress = hdnStreetAddress.Text;
            newPostalAddress.Barangay = hdnBarangay.Text;
            newPostalAddress.City = hdnCity.Text;
            newPostalAddress.Municipality = hdnMunicipality.Text; 
            newPostalAddress.Province = hdnProvince.Text;
            newPostalAddress.State = hdnState.Text;
            newPostalAddress.PostalCode = hdnPostalCode.Text;
            newPostalAddress.CountryId = int.Parse(hdnCountryId.Text);
            newPostalAddress.IsPrimary = true;
            PostalAddress.CreateOrUpdatePostalAddress(party, newPostalAddress, PostalAddressType.HomeAddressType, today, true);

            //Home Phone
            TelecommunicationsNumber newTelePhoneNumber = new TelecommunicationsNumber();
            newTelePhoneNumber.AreaCode = txtTeleAreaCode.Text;
            newTelePhoneNumber.PhoneNumber = txtTelePhoneNumber.Text;
            newTelePhoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.HomePhoneNumberType;
            newTelePhoneNumber.IsPrimary = true;
            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newTelePhoneNumber, TelecommunicationsNumberType.HomePhoneNumberType, today, true);

            //Personal Mobile
            TelecommunicationsNumber newCellPhoneNumber = new TelecommunicationsNumber();
            newCellPhoneNumber.AreaCode = txtCellAreaCode.Text;
            newCellPhoneNumber.PhoneNumber = txtCellPhoneNumber.Text;
            newCellPhoneNumber.TelecommunicationsNumberType = TelecommunicationsNumberType.PersonalMobileNumberType;
            newCellPhoneNumber.IsPrimary = true;
            TelecommunicationsNumber.CreateOrUpdateTeleCommNumberAddress(party, newCellPhoneNumber, TelecommunicationsNumberType.PersonalMobileNumberType, today, true);

            //Electronic Address
            ElectronicAddress newEmailAddress = new ElectronicAddress();
            newEmailAddress.ElectronicAddressString = txtPrimaryEmailAddress.Text;
            newEmailAddress.ElectronicAddressType = ElectronicAddressType.PersonalEmailAddressType;
            newEmailAddress.IsPrimary = true;
            ElectronicAddress.CreateOrUpdateElectronicAddress(party, newEmailAddress, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true, today);
        }

    }
}