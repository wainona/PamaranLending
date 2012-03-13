using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.Reports
{
    public partial class ScheduleOfOutstandingLoans : ActivityPageBase
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
                //HEADER LENDER INFORMATION
                hdnTotal.Value = 0;
                dtMonthStart.SelectedDate = DateTime.Now;
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderInformation(party);
            }
        }

        public void FillLenderInformation(Party party)
        {
            //Fill Lender Name
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
            //Fill Lender Address
            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);
            //Fill Lender Numbers
            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        public void FillPostalAddress(PostalAddress postalAddress)
        {
            lblStreetAddress.Text = postalAddress.StreetAddress;
            lblBarangay.Text = postalAddress.Barangay;
            lblCity.Text = postalAddress.City;
            lblMunicipality.Text = postalAddress.Municipality;
            lblProvince.Text = postalAddress.Province;
            lblCountry.Text = postalAddress.Country.Name;
            lblPostalCode.Text = postalAddress.PostalCode;
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            lblDates.Text = String.Format("{0:MMMM dd, yyyy}", dtMonthStart.SelectedDate);
            var models = OutstandingLoans();
            strOutstandingLoans.DataSource = models;
            strOutstandingLoans.DataBind();
        }

        private IEnumerable<OutstandingLoansModel> OutstandingLoans()
        {
            var date = dtMonthStart.SelectedDate;
            var outstandingLoans = LoanAccount.RetrieveOutstandingLoansAsOf(date);
            decimal total = outstandingLoans.Sum(entity=>entity.LoanBalance);
            //lblTotal.Text = "₱ " + total.ToString("N");
            return outstandingLoans;
        }
    }
}