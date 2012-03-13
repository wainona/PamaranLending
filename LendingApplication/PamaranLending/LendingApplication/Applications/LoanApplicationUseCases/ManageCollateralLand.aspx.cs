using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LendingApplication;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication
{
    public partial class ManageCollateralLand : ActivityPageBase
    {
        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
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

        public void Fill(LandCollateral collateral)
        {
            this.txtCollateralDesc.Text = collateral.Description;

            this.chkMortgaged.Checked = collateral.IsPropertyMortgage;
            this.chkNotMortgaged.Checked = collateral.IsPropertyMortgage == false;
            this.btnPickMortgagee.Disabled = collateral.IsPropertyMortgage == false;
            this.hiddenMortgageeId.Value = collateral.MortgageeId;
            this.txtMortgageeName.Text = collateral.Mortgagee;
            this.txtMortgageeName.AllowBlank = !this.chkMortgaged.Checked;
            this.cmbLandType.SetValueAndFireSelect(collateral.LandType);
            this.nfLandArea.Number = (double)collateral.LandArea;
            this.cmbUnitOfMeasure.SetValueAndFireSelect(collateral.LandUOM);
            this.txtTCTNumber.Text = collateral.TCTNumber;

            cmbCountry.SetValueAndFireSelect(collateral.CountryId);
            txtStreetAddress.Text = collateral.StreetAddress;
            txtBarangay.Text = collateral.Barangay;
            txtProvince.Text = collateral.Province;
            //txtState.Text = collateral.State;
            txtPostalCode.Text = collateral.PostalCode;

            if (string.IsNullOrWhiteSpace(collateral.Municipality) == false)
            {
                radioMunicipality.Checked = true;
                txtCityOrMunicipality.Text = collateral.Municipality;
            }
            else
            {
                txtCityOrMunicipality.Text = collateral.City;
                radioCity.Checked = true;
            }

            txtPropertyLocation.Text = StringConcatUtility.Build(", ",
                            txtStreetAddress.Text,
                            txtBarangay.Text,
                            txtCityOrMunicipality.Text,
                            txtProvince.Text,
                            //txtState.Text,
                            cmbCountry.SelectedItem.Text,
                            txtPostalCode.Text);

            StorePropertyOwner.DataSource = collateral.AvailablePropertyOwners;
            StorePropertyOwner.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var countries = ObjectContext.Countries;
            foreach (var item in countries)
            {
                Ext.Net.ListItem listItem = new Ext.Net.ListItem();
                listItem.Text = item.Name;
                listItem.Value = item.Id.ToString();
                cmbCountry.Items.Add(listItem);
            }
            cmbCountry.SelectedItem.Text = "Philippines";
            txtCityOrMunicipality.Text = ApplicationSettings.DefaultCity;
            txtPostalCode.Text = ApplicationSettings.DefaultPostalCode;

            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);

                var landTypes = LandType.All();
                storeLandType.DataSource = landTypes;
                storeLandType.DataBind();

                var uom = UnitOfMeasure.All(UnitOfMeasureType.LengthUnitType);
                storeUnitOfMeasureType.DataSource = uom;
                storeUnitOfMeasureType.DataBind();

                hiddenMode.Value = mode;
                if (mode == "add")
                {
                    LandCollateral collateral = this.CreateOrRetrieveBOM<LandCollateral>();
                }
                else
                {
                    hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                    LandCollateral collateral = (LandCollateral)form.RetrieveCollateral(hiddenRandomKey.Text);
                    this.Register(collateral);

                    Fill(collateral);
                }
            }
        }

        [DirectMethod]
        public void AddPropertyOwner(int partyId, string name, string address, int percentOwned)
        {
            LandCollateral collateral = this.RetrieveBOM<LandCollateral>();
            PropertyOwner model = new PropertyOwner();
            model.PartyId = partyId;
            model.Name = name;
            model.Address = address;
            model.PercentOwned = percentOwned;

            collateral.AddPropertyOwner(model);
            StorePropertyOwner.DataSource = collateral.AvailablePropertyOwners;
            StorePropertyOwner.DataBind();
        }

        [DirectMethod]
        public void EditPropertyOwner(string randomKey, int partyId, string name, string address, int percentOwned)
        {
            LandCollateral collateral = this.RetrieveBOM<LandCollateral>();
            PropertyOwner model = collateral.Retrieve(randomKey);
            if (model == null)
                model = new PropertyOwner();

            model.PartyId = partyId;
            model.Name = name;
            model.Address = address;
            model.PercentOwned = percentOwned;
            model.MarkEdited();

            StorePropertyOwner.DataSource = collateral.AvailablePropertyOwners;
            StorePropertyOwner.DataBind();
        }

        protected void btnDeletePropertyOwner_Click(object sender, DirectEventArgs e)
        {
            LandCollateral collateral = this.RetrieveBOM<LandCollateral>();
            SelectedRowCollection rows = this.SelectionModelPropertyOwners.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                collateral.RemovePropertyOwner(row.RecordID);
            }
            StorePropertyOwner.DataSource = collateral.AvailablePropertyOwners;
            StorePropertyOwner.DataBind();
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
            LandCollateral collateral = this.RetrieveBOM<LandCollateral>();
            collateral.Description = this.txtCollateralDesc.Text;

            collateral.IsPropertyMortgage = this.chkMortgaged.Checked;
            if (collateral.IsPropertyMortgage)
            {
                collateral.MortgageeId = int.Parse(this.hiddenMortgageeId.Text);
                collateral.Mortgagee = txtMortgageeName.Text;
            }
            collateral.LandType = int.Parse(this.cmbLandType.SelectedItem.Value);
            collateral.LandArea = (int)this.nfLandArea.Number;
            collateral.LandUOM = int.Parse(this.cmbUnitOfMeasure.SelectedItem.Value);
            collateral.TCTNumber = this.txtTCTNumber.Text;
            collateral.CountryId = int.Parse(this.cmbCountry.SelectedItem.Value);
            collateral.StreetAddress = txtStreetAddress.Text;
            collateral.Barangay = txtBarangay.Text;
            collateral.Province = txtProvince.Text;
            //collateral.State = txtState.Text;
            collateral.PostalCode = txtPostalCode.Text;
            collateral.City = collateral.Municipality = "";
            if (radioMunicipality.Checked)
                collateral.Municipality = txtCityOrMunicipality.Text;
            else
                collateral.City = txtCityOrMunicipality.Text;

            collateral.MarkEdited();
            if (hiddenMode.Text == "add")
                form.AddCollateral(collateral);
        }

        //When Ok button on the Address detail window is clicked
        protected void wndAddressDetail_btnAdd_Click(object sender, DirectEventArgs e)
        {
            txtPropertyLocation.Text = StringConcatUtility.Build(", ",
                            txtStreetAddress.Text,
                            txtBarangay.Text,
                            txtCityOrMunicipality.Text,
                            txtProvince.Text,
                            //txtState.Text,
                            cmbCountry.SelectedItem.Text,
                            txtPostalCode.Text);

            wndAddressDetail.Hidden = true;
        }
    }
}