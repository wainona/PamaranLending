using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.AgingOfAccountsUseCases
{
    public partial class AgingSummaryInNumberOfAccounts : ActivityPageBase
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
                datSelectedDate.SelectedDate = DateTime.Now;
                DateTime dateSelected = datSelectedDate.SelectedDate;
                FillHeader();
                FillAgingAccounts(dateSelected);
            }
        }

        protected void FillHeader()
        {
            //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                Organization organization = party.Organization;
                lblLenderNameHeader.Text = organization.OrganizationName;

                var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
                FillPostalAddress(postalAddress);

                lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
                lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
                lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
                lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        [DirectMethod]
        public void FillTable()
        {
            var date = datSelectedDate.SelectedDate.Date;
            FillAgingAccounts(date);
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

        protected void FillAgingAccounts(DateTime date)
        {
            List<AgingAccountsModel> accounts = new List<AgingAccountsModel>();
            DateTime selectedDate = date;
            lblSelectedDate.Text = date.Date.ToString("MMMM dd, yyyy");

            var agingAccounts = (from la in ObjectContext.LoanAccounts
                                 join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                 where la.LoanBalance > 0 && las.IsActive == true &&
                                    (las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                 select la.FinancialAccount).Distinct();

            foreach (var acc in agingAccounts)
            {
                accounts.Add(new AgingAccountsModel(acc, selectedDate));
            }

            if (agingAccounts.Count() != 0)
            {
                var sum = (decimal)accounts.Count;
                var currentTotal = (decimal)accounts.Where(entity => entity.Current != 0).Count();
                var total130 = (decimal)accounts.Where(entity => entity.OneToThirty != 0).Count();
                var total3160 = (decimal)accounts.Where(entity => entity.ThirtyOneToSixty != 0).Count();
                var total6190 = (decimal)accounts.Where(entity => entity.SixtyOneToNinety != 0).Count();
                var totalOver90 = (decimal)accounts.Where(entity => entity.OverNinety != 0).Count();

                lblNoAccountsTotal.Text = sum.ToString();
                lblNumberCurrent.Text = currentTotal.ToString();
                lblNumber130.Text = total130.ToString();
                lblNumber3160.Text = total3160.ToString();
                lblNumber6190.Text = total6190.ToString();
                lblNumberOver90.Text = totalOver90.ToString();

                lblRatioTotal.Text = "100%";
                lblRatioCurrent.Text = (Math.Round((currentTotal / sum) * 100, 3)).ToString() + "%";
                lblRatio130.Text = (Math.Round((total130 / sum) * 100, 3)).ToString() + "%";
                lblRatio3160.Text = (Math.Round((total3160 / sum) * 100, 3)).ToString() + "%";
                lblRatio6190.Text = (Math.Round((total6190 / sum) * 100, 3)).ToString() + "%";
                lblRatioOver90.Text = (Math.Round((totalOver90 / sum) * 100, 3)).ToString() + "%";
            }
            else
            {
                lblNoAccountsTotal.Text = "0";
                lblNumberCurrent.Text = "0";
                lblNumber130.Text = "0";
                lblNumber3160.Text = "0";
                lblNumber6190.Text = "0";
                lblNumberOver90.Text = "0";

                lblRatioTotal.Text = "0%";
                lblRatioCurrent.Text = "0%";
                lblRatio130.Text = "0%";
                lblRatio3160.Text = "0%";
                lblRatio6190.Text = "0%";
                lblRatioOver90.Text = "0%";
            }
        }

        private class AgingAccountsModel
        {
            public string AccountName { get; set; }
            public DateTime? DueDate1 { get; set; }
            public string DueDate { get; set; }
            public DateTime? LoanReleaseDate1 { get; set; }
            public string LoanReleaseDate { get; set; }
            public DateTime? LastPaymentDate1 { get; set; }
            public string LastPaymentDate { get; set; }
            public decimal Balance { get; set; }
            public decimal Current { get; set; }
            public decimal OneToThirty { get; set; }
            public decimal ThirtyOneToSixty { get; set; }
            public decimal SixtyOneToNinety { get; set; }
            public decimal OverNinety { get; set; }
            public DateTime SelectedDate { get; set; }

            public AgingAccountsModel(FinancialAccount financialAccount, DateTime selectedDate)
            {
                var owner = ObjectContext.FinancialAccountRoles.FirstOrDefault(entity => entity.FinancialAccountId == financialAccount.Id
                                            && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);

                //var person = owner.PartyRole.Party.Person;
                var party = owner.PartyRole.Party;

                this.AccountName = party.Name;

                this.SelectedDate = selectedDate;

                var loanAccount = financialAccount.LoanAccount;
                var payments = loanAccount.FinancialAccount.FinAcctTrans.Where(entity => entity.FinancialAcctTransTypeId == FinlAcctTransType.AccountPaymentType.Id).OrderByDescending(entity => entity.TransactionDate).FirstOrDefault();

                if (payments != null)
                {
                    this.DueDate1 = payments.TransactionDate;
                    this.LastPaymentDate1 = this.DueDate1;
                    this.DueDate = this.DueDate1.Value.ToString("MMMM dd, yyyy");
                    this.LastPaymentDate = this.DueDate;
                }
                else
                {
                    this.LastPaymentDate = "None";
                    this.DueDate1 = loanAccount.LoanReleaseDate;
                    this.DueDate = this.DueDate1.Value.ToString("MMMM dd, yyyy");
                    this.LoanReleaseDate = this.DueDate;
                    this.LoanReleaseDate1 = this.DueDate1;
                }

                this.Balance = loanAccount.LoanBalance;
                var dateDiff = 0;

                if (this.SelectedDate.Date.CompareTo(this.DueDate1.Value.Date) <= 0)
                {
                    this.Current = this.Balance;
                }
                else
                {
                    dateDiff = this.SelectedDate.Date.Subtract(this.DueDate1.Value.Date).Days;

                    if (dateDiff >= 1 && dateDiff <= 30)
                    {
                        this.OneToThirty = this.Balance;
                    }
                    else if (dateDiff >= 31 && dateDiff <= 60)
                    {
                        this.ThirtyOneToSixty = this.Balance;
                    }
                    else if (dateDiff >= 61 && dateDiff <= 90)
                    {
                        this.SixtyOneToNinety = this.Balance;
                    }
                    else if (dateDiff > 90)
                    {
                        this.OverNinety = this.Balance;
                    }
                }
            }
        }

    }
}