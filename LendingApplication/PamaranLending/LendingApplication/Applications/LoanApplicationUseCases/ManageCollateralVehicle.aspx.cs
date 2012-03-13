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
    public partial class ManageCollateralVehicle : ActivityPageBase
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

        public void Fill(VehicleCollateral collateral)
        {
            this.txtCollateralDesc.Text = collateral.Description;

            this.chkIsMortgaged.Checked = collateral.IsPropertyMortgage;
            this.hiddenMortgageeId.Value = collateral.MortgageeId;
            this.txtMortgageeName.Text = collateral.Mortgagee;
            this.txtMortgageeName.AllowBlank = !this.chkIsMortgaged.Checked;
            this.nfAcquisitionCost.Number = (double)collateral.AcquisitionCost;
            this.cmbVehicleType.SetValueAndFireSelect(collateral.VehicleTypeId);
            this.txtBrandName.Text = collateral.Brand;
            this.txtModel.Text = collateral.Model;

            StorePropertyOwner.DataSource = collateral.AvailablePropertyOwners;
            StorePropertyOwner.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                string mode = Request.QueryString["mode"].ToLower();
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);

                hiddenMode.Value = mode;
                if (mode == "add")
                {
                    VehicleCollateral collateral = this.CreateOrRetrieveBOM<VehicleCollateral>();
                }
                else
                {
                    hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                    VehicleCollateral collateral = (VehicleCollateral)form.RetrieveCollateral(hiddenRandomKey.Text);
                    this.Register(collateral);

                    Fill(collateral);
                }

                var vehicleType = ObjectContext.VehicleTypes.ToList();
                storeVehicleType.DataSource = vehicleType;
                storeVehicleType.DataBind();
            }
        }

        [DirectMethod]
        public void AddPropertyOwner(int partyId, string name, string address, int percentOwned)
        {
            VehicleCollateral collateral = this.RetrieveBOM<VehicleCollateral>();
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
            VehicleCollateral collateral = this.RetrieveBOM<VehicleCollateral>();
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
            VehicleCollateral collateral = this.RetrieveBOM<VehicleCollateral>();
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
            VehicleCollateral collateral = this.RetrieveBOM<VehicleCollateral>();
            collateral.Description = this.txtCollateralDesc.Text;

            collateral.IsPropertyMortgage = this.chkIsMortgaged.Checked;
            if (collateral.IsPropertyMortgage)
            {
                collateral.MortgageeId = int.Parse(this.hiddenMortgageeId.Text);
                collateral.Mortgagee = txtMortgageeName.Text;
            }
            collateral.VehicleTypeId = int.Parse(this.cmbVehicleType.SelectedItem.Value);
            collateral.Brand = this.txtBrandName.Text;
            collateral.Model = this.txtModel.Text;
            collateral.AcquisitionCost = (decimal)nfAcquisitionCost.Number;
            collateral.MarkEdited();
            if (hiddenMode.Text == "add")
                form.AddCollateral(collateral);
        }
    }
}