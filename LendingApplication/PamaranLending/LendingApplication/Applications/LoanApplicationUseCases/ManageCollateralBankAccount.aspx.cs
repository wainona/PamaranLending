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
    public partial class ManageCollateralBankAccount : ActivityPageBase
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

        public void Fill(BankAccountCollateral collateral)
        {
            //collateral.AssetTypeId = AssetType.BankAccount;
            this.txtCollateralDesc.Text = collateral.Description ;

            this.chkMortgaged.Checked = collateral.IsPropertyMortgage;
            this.chkNotMortgaged.Checked = collateral.IsPropertyMortgage == false;
            this.btnPickMortgagee.Disabled = collateral.IsPropertyMortgage == false;
            this.hiddenMortgageeId.Value = collateral.MortgageeId;
            this.txtMortgageeName.Text = collateral.Mortgagee;
            this.txtMortgageeName.AllowBlank = !this.chkMortgaged.Checked;
            this.cmbBankAccountType.SetValueAndFireSelect(collateral.BankAccountType);
            this.hiddenBankPartyRoleId.Value = collateral.BankPartyRoleId;
            this.txtBankName.Text= collateral.BankName ;
            this.txtAccountNumber.Text = collateral.BankAccountNumber ;
            this.txtAccountName.Text = collateral.BankAccountName ;

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

                var bankAccountTypes = BankAccountType.All();
                storeBankAccountType.DataSource = bankAccountTypes;
                storeBankAccountType.DataBind();

                hiddenMode.Value = mode;
                if (mode == "add")
                {
                    BankAccountCollateral collateral = this.CreateOrRetrieveBOM<BankAccountCollateral>();
                }
                else
                {
                    hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                    BankAccountCollateral collateral = (BankAccountCollateral)form.RetrieveCollateral(hiddenRandomKey.Text);
                    this.Register(collateral);

                    Fill(collateral);
                }
            }
        }
        
        [DirectMethod]
        public void AddPropertyOwner(int partyId, string name, string address, int percentOwned)
        {
            BankAccountCollateral collateral = this.RetrieveBOM<BankAccountCollateral>();
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
            BankAccountCollateral collateral = this.RetrieveBOM<BankAccountCollateral>();
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
            BankAccountCollateral collateral = this.RetrieveBOM<BankAccountCollateral>();
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
            BankAccountCollateral collateral = this.RetrieveBOM<BankAccountCollateral>();
            collateral.Description = this.txtCollateralDesc.Text;

            collateral.IsPropertyMortgage = this.chkMortgaged.Checked;
            if (collateral.IsPropertyMortgage)
            {
                collateral.MortgageeId = int.Parse(this.hiddenMortgageeId.Text);
                collateral.Mortgagee = txtMortgageeName.Text;
            }
            collateral.BankAccountType = int.Parse(this.cmbBankAccountType.SelectedItem.Value);
            collateral.BankPartyRoleId = int.Parse(this.hiddenBankPartyRoleId.Text);
            collateral.BankName = this.txtBankName.Text;
            collateral.BankAccountNumber = this.txtAccountNumber.Text;
            collateral.BankAccountName = this.txtAccountName.Text;
            collateral.MarkEdited();
            if (hiddenMode.Text == "add")
                form.AddCollateral(collateral);
        }
    }
}