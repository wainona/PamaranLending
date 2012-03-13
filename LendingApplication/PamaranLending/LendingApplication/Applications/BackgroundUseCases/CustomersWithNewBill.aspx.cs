using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.BackgroundUseCases
{
    public partial class CustomersWithNewBill : ActivityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                txtUserID.Value = PartyRole.GetById(partyroleid).PartyId;
            }
        }
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Fill();
        }
        protected void Fill()
        {
            var UserId = int.Parse(txtUserID.Value.ToString());
            var query = Party.GetAllCustomersWithLoan(UserId);
            var result = query.ToList().AsEnumerable();
            if (string.IsNullOrWhiteSpace(txtSearch.Text) == false)
            {
                string name = txtSearch.Text;
                result = result.Where(entity => entity.Names.ToLower().Contains(name.ToLower()));
            }

            PageGridPanelStore.DataSource = result;
            this.PageGridPanelStore.DataBind();
        }
        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
            Fill();
        }
    }
}