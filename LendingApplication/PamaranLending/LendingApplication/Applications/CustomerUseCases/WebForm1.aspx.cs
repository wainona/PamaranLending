using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.FinancialManagement.CollectionUseCases
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnChange(object sender, DirectEventArgs e)
        {
            List<PartyType> partyType = new List<PartyType>();
            List<CustomerStatusType> cusStatType = new List<CustomerStatusType>();
            List<Country> country = new List<Country>();
            using (var context = new FinancialEntities())
            {
                partyType = context.PartyTypes.ToList();
                cusStatType = context.CustomerStatusTypes.ToList();
                country = context.Countries.ToList();
            }
            if (cmbFilterBy.SelectedItem.Text.Equals("Status"))
            {
                FilterStore.DataSource = cusStatType;
                cmbFilterBy2.Show();
            }
            else if (cmbFilterBy.SelectedItem.Text.Equals("Party Type"))
            {
                FilterStore.DataSource = partyType;
                cmbFilterBy2.Show();
            }
            else
            {
                cmbFilterBy2.Hide();
            }
            CountryStore.DataSource = country;
            CountryStore.DataBind();
            FilterStore.DataBind();
            
        }
    }
}