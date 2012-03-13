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
    public partial class ViewOrEditEmployer : ActivityPageBase
    {
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
                EmployerForm form = this.CreateOrRetrieve<EmployerForm>();

                int id = int.Parse(Request.QueryString["id"]);
                var countries = Country.All();
                EmployerID.Value = id;

                CountryStore.DataSource = countries;
                CountryStore.DataBind();

                PartyRole partyRole = PartyRole.GetById(id);
                if(partyRole == null)
                    throw new AccessToDeletedRecordException("The employer has been deleted by another user.");
                Fill(id);

                if (this.txtEmployerPartyType.Text == "Person")
                {
                    cmfPersonName.Show();
                    txtOrganizationName.AllowBlank = true;
                }
                else
                {
                    txtOrganizationName.Show();
                    txtPersonName.AllowBlank = true;
                }
                //int id = int.Parse(Request.QueryString["id"]);
                //using (var context = new BusinessModelEntities())
                //{
                //    Customer customer = context.Customers.SingleOrDefault(entity => entity.ID == 0);
                //    if (customer != null)
                //    {
                //        this.RecordID.Value = customer.ID;
                //        this.Name.Text = customer.Name;
                //        this.PhoneNumber.Text = customer.PhoneNumber;
                //        this.EmailAddress.Text = customer.EmailAddress;
                //        this.Address.Text = customer.Address;
                //    }
                //    else
                //    {
                //        // TODO:: Don't know how to handle for now.
                //    }
                //}
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

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                EmployerForm form = this.CreateOrRetrieve<EmployerForm>();
                form.IsNew = false;
                form.EmployerPartyType = this.txtEmployerPartyType.Text;
                form.EmployerId = int.Parse(EmployerID.Text);

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

        [DirectMethod]
        public void FillDetails(int id)
        {
            Fill(id);
        }

        protected void Fill(int partyRoleId)
        {
            EmployerForm form = this.CreateOrRetrieve<EmployerForm>();
            form.EmployerId = partyRoleId;
            form.Retrieve(partyRoleId);
            this.txtEmployerPartyType.Text = form.EmployerPartyType;

            if (form.EmployerPartyType == "Person")
            {
                txtPersonName.Text = form.EmployerPersonName.NameConcat;
                txtPersonFirstName.Text = form.EmployerPersonName.FirstName;
                txtPersonLastName.Text = form.EmployerPersonName.LastName;
                txtPersonMiddleName.Text = form.EmployerPersonName.MiddleName;
                txtPersonMothersMaidenName.Text = form.EmployerPersonName.MothersMaidenName;
                txtPersonNameSuffix.Text = form.EmployerPersonName.NameSuffix;
                txtPersonNickName.Text = form.EmployerPersonName.NickName;
                txtPersonTitle.Text = form.EmployerPersonName.Title;

                txtFirstNameP.Text = form.EmployerPersonName.FirstName;
                txtLastNameP.Text = form.EmployerPersonName.LastName;
                txtMiddleNameP.Text = form.EmployerPersonName.MiddleName;
                txtMothersMaidenNameP.Text = form.EmployerPersonName.MothersMaidenName;
                txtNameSuffixP.Text = form.EmployerPersonName.NameSuffix;
                txtNickNameP.Text = form.EmployerPersonName.NickName;
                txtTitleP.Text = form.EmployerPersonName.Title;
            }
            else
            {
                txtOrganizationName.Text = form.EmployerOrganizationName;
            }

            //<--Contact Information
            //Primary Home Address
            txtBusinessAddress.Text = form.EmployerBusinessAddress.AddressConcat;
            txtStreetAdd1.Text = form.EmployerBusinessAddress.StreetAddress;
            txtProvince1.Text = form.EmployerBusinessAddress.Province;
            txtBarangay1.Text = form.EmployerBusinessAddress.Barangay;
            txtCity1.Text = form.EmployerBusinessAddress.City;
            txtMunicipality1.Text = form.EmployerBusinessAddress.Municipality;
            txtPostal1.Text = form.EmployerBusinessAddress.PostalCode;
            txtState1.Text = form.EmployerBusinessAddress.State;

            //txtStreetNumberD1.Text = form.EmployerBusinessAddress.StreetAddress;
            //txtProvinceD1.Text = form.EmployerBusinessAddress.Province;
            //txtBarangayD1.Text = form.EmployerBusinessAddress.Barangay;
            //txtCityD1.Text = form.EmployerBusinessAddress.City;
            //txtMunicipalityD1.Text = form.EmployerBusinessAddress.Municipality;
            //txtPostalCodeD1.Text = form.EmployerBusinessAddress.PostalCode;
            //if (form.EmployerBusinessAddress.CountryId != 0)
            //    cmbCountryD1.SelectedItem.Value = form.EmployerBusinessAddress.CountryId.ToString();

            txtStreetAddressA1.Text = form.EmployerBusinessAddress.StreetAddress;
            txtProvinceA1.Text = form.EmployerBusinessAddress.Province;
            txtBarangayA1.Text = form.EmployerBusinessAddress.Barangay;
            if (!string.IsNullOrWhiteSpace(form.EmployerBusinessAddress.City))
            {
                txtCityOrMunicipalityA1.Text = form.EmployerBusinessAddress.City;
                radioMunicipalityA1.Checked = false;
                radioCityA1.Checked = true;
            }
            else if (!string.IsNullOrWhiteSpace(form.EmployerBusinessAddress.Municipality))
            {
                txtCityOrMunicipalityA1.Text = form.EmployerBusinessAddress.Municipality;
                radioCityA1.Checked = false;
                radioMunicipalityA1.Checked = true;
            }
            txtPostalCodeA1.Text = form.EmployerBusinessAddress.PostalCode;
            if (form.EmployerBusinessAddress.CountryId != 0)
                cmbCountryA1.SelectedItem.Value = form.EmployerBusinessAddress.CountryId.ToString();

            //Fax Number

            txtFaxTelCode.Text = form.CountryCode;
            if(!string.IsNullOrWhiteSpace(form.EmployerBusinessFaxNumber.AreaCode))
                nfFaxAreaCode.Text = form.EmployerBusinessFaxNumber.AreaCode;
            if (!string.IsNullOrWhiteSpace(form.EmployerBusinessFaxNumber.PhoneNumber))
                nfFaxPhoneNumber.Text = form.EmployerBusinessFaxNumber.PhoneNumber;


            //Telephone Number
            txtTelCode.Text = form.CountryCode;
            if (!string.IsNullOrWhiteSpace(form.EmployerBusinessPhoneNumber.AreaCode))
                nfAreaCode.Text = form.EmployerBusinessPhoneNumber.AreaCode;
            if (!string.IsNullOrWhiteSpace(form.EmployerBusinessPhoneNumber.PhoneNumber))
                nfPhoneNumber.Text = form.EmployerBusinessPhoneNumber.PhoneNumber;


            //Primary Email Address
            txtBusinessEmailAddress.Text = form.EmployerBusinessEmailAddress.EletronicAddressString;
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
                txtBusinessAddress.Text = txtStreetAdd1.Text;
            if (!(string.IsNullOrWhiteSpace(txtBarangay1.Text)))
                txtBusinessAddress.Text += ", " + txtBarangay1.Text;
            if (!(string.IsNullOrWhiteSpace(txtCity1.Text)) && string.IsNullOrWhiteSpace(txtMunicipality1.Text))
                txtBusinessAddress.Text += ", " + txtCity1.Text;
            else if (!(string.IsNullOrWhiteSpace(txtMunicipality1.Text)) && string.IsNullOrWhiteSpace(txtCity1.Text))
                txtBusinessAddress.Text += ", " + txtMunicipality1.Text;
            if (!(string.IsNullOrWhiteSpace(txtProvince1.Text)))
                txtBusinessAddress.Text += ", " + txtProvince1.Text;
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

        protected void checkEmployerName()
        {
            var personName = txtPersonName.Text;
            //bool res = true;
            var sameName = ObjectContext.EmployersViewLists.SingleOrDefault(entity => entity.Name == personName);

            if (sameName != null)
            {
                X.Msg.Confirm("Message", "An employer record with the same name already exists in the list. Do you want to create another employer record with the same name?", new JFunction("Employer.AddSameEmployer(result);", "result")).Show();
            }
            else
            {
                X.Msg.Confirm("Message", "A party record with the same name already exists. Do you want to use this record to create the new employer record?", new JFunction("Employer.AddSameParty(result);", "result")).Show();
            }
        }

        [DirectMethod]
        public void FillAddress(int partyId)
        {
            Party party = Party.GetById(partyId);
            //Postal Address
            Address postalAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.BusinessAddressType.Id && entity.PostalAddress.IsPrimary == true);

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

                //Business Telephone Number
                Address telecomNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.TelecommunicationNumberType.Id
                                    && entity.TelecommunicationsNumber.TypeId == TelecommunicationsNumberType.BusinessPhoneNumberType.Id
                                    && entity.TelecommunicationsNumber.IsPrimary == true);

                if (telecomNumber != null && telecomNumber.TelecommunicationsNumber != null)
                {

                    TelecommunicationsNumber telecomNumberSpecific = telecomNumber.TelecommunicationsNumber;
                    nfPhoneNumber.Text = telecomNumberSpecific.PhoneNumber;
                    nfAreaCode.Text = telecomNumberSpecific.AreaCode;
                }

                //Business Fax Number
                Address faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.TelecommunicationNumberType
                                    && entity.TelecommunicationsNumber.TelecommunicationsNumberType == TelecommunicationsNumberType.BusinessFaxNumberType
                                    && entity.TelecommunicationsNumber.IsPrimary);

                if (faxNumber != null && faxNumber.TelecommunicationsNumber != null)
                {
                    TelecommunicationsNumber faxNumberSpecific = faxNumber.TelecommunicationsNumber;
                    nfFaxPhoneNumber.Text = faxNumberSpecific.PhoneNumber;
                    nfFaxAreaCode.Text = faxNumberSpecific.AreaCode;
                }
            }

            //Email Address
            Address emailAddress = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressType == AddressType.ElectronicAddressType
                                && entity.ElectronicAddress.ElectronicAddressType == ElectronicAddressType.BusinessEmailAddressType
                                && entity.ElectronicAddress.IsPrimary);
            if (emailAddress != null && emailAddress.ElectronicAddress != null)
            {
                ElectronicAddress primaryEmailAddressSpecific = emailAddress.ElectronicAddress;
                txtBusinessEmailAddress.Text = primaryEmailAddressSpecific.ElectronicAddressString;
            }


        }

        //For Customer Record with Same Name
        [DirectMethod(Namespace = "Employer")]
        public void AddSameEmployer(string result)
        {
            if (result == "no")
            {
                hiddenUsesExistRecord.Text = "no";
                txtPersonName.Text = "";
                wndPersonNameDetail.Show();
            }
            else hiddenUsesExistRecord.Text = "yes";

        }

        [DirectMethod(Namespace = "Employer")]
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
    }
}