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
    public partial class ManageCollateralMachine : ActivityPageBase
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

        public void Fill(MachineCollateral collateral)
        {
            this.txtCollateralDesc.Text = collateral.Description;

            this.chkIsMortgaged.Checked = collateral.IsPropertyMortgage;
            this.hiddenMortgageeId.Value = collateral.MortgageeId;
            this.txtMortgageeName.Text = collateral.Mortgagee;
            this.txtMortgageeName.AllowBlank = !this.chkIsMortgaged.Checked;
            this.nfAcquisitionCost.Number = (double)collateral.AcquisitionCost;
            this.txtMachineName.Text = collateral.MachineName;
            this.txtBrandName.Text = collateral.Brand;
            this.txtModel.Text = collateral.Model;
            this.txtCapacity.Text = collateral.Capacity;

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
                    MachineCollateral collateral = this.CreateOrRetrieveBOM<MachineCollateral>();
                }
                else
                {
                    hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                    MachineCollateral collateral = (MachineCollateral)form.RetrieveCollateral(hiddenRandomKey.Text);
                    this.Register(collateral);

                    Fill(collateral);
                }
            }
        }

        [DirectMethod]
        public void AddPropertyOwner(int partyId, string name, string address, int percentOwned)
        {
            MachineCollateral collateral = this.RetrieveBOM<MachineCollateral>();
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
            MachineCollateral collateral = this.RetrieveBOM<MachineCollateral>();
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
            MachineCollateral collateral = this.RetrieveBOM<MachineCollateral>();
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
            MachineCollateral collateral = this.RetrieveBOM<MachineCollateral>();
            collateral.Description = this.txtCollateralDesc.Text;

            collateral.IsPropertyMortgage = this.chkIsMortgaged.Checked;
            if (collateral.IsPropertyMortgage)
            {
                collateral.MortgageeId = int.Parse(this.hiddenMortgageeId.Text);
                collateral.Mortgagee = txtMortgageeName.Text;
            }
            collateral.MachineName = this.txtMachineName.Text;
            collateral.Brand = this.txtBrandName.Text;
            collateral.Model = this.txtModel.Text;
            collateral.Capacity = this.txtCapacity.Text;
            collateral.AcquisitionCost = (decimal)nfAcquisitionCost.Number;
            collateral.MarkEdited();
            if (hiddenMode.Text == "add")
                form.AddCollateral(collateral);
        }
    }
}