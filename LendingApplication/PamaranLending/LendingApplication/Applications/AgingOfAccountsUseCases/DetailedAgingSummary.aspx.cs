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
    public partial class DetailedAgingSummary : ActivityPageBase
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
            //var date = DateTime.Parse(Request.QueryString["dat"]);
            //hiddenDate.Value = date;
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

        [DirectMethod]
        public void FillTable()
        {
            var date = datSelectedDate.SelectedDate.Date;
            FillAgingAccounts(date);
        }

        public void FillAgingAccounts(DateTime date)
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

            AgingAccountsModel accs = new AgingAccountsModel();
            accs.AccountName = "Total";
            accs.Balance = accounts.Sum(entity => entity.Balance);
            accs.Current = accounts.Sum(entity => entity.Current) == 0 ? -1M : accounts.Sum(entity => entity.Current);
            accs.OneToThirty = accounts.Sum(entity => entity.OneToThirty) == 0 ? -1M : accounts.Sum(entity => entity.OneToThirty);
            accs.OverNinety = accounts.Sum(entity => entity.OverNinety) == 0 ? -1M : accounts.Sum(entity => entity.OverNinety);
            accs.SixtyOneToNinety = accounts.Sum(entity => entity.SixtyOneToNinety) == 0 ? -1M : accounts.Sum(entity => entity.SixtyOneToNinety);
            accs.ThirtyOneToSixty = accounts.Sum(entity => entity.ThirtyOneToSixty) == 0 ? -1M : accounts.Sum(entity => entity.ThirtyOneToSixty);

            accounts.Add(accs);

            //lblCurrentTotal.Text = accounts.Sum(entity => entity.Current).ToString();
            //lblBalanceTotal.Text = accounts.Sum(entity => entity.Balance).ToString();
            //lblOneThirtyTotal.Text = accounts.Sum(entity => entity.OneToThirty).ToString();
            //lblThirtyOneSixtyTotal.Text = accounts.Sum(entity => entity.ThirtyOneToSixty).ToString();
            //lblSixtyOneNinetyTotal.Text = accounts.Sum(entity => entity.SixtyOneToNinety).ToString();
            //lblOverNinetyTotal.Text = accounts.Sum(entity => entity.OverNinety).ToString();

            //grdAgingAccounts.DataSource = accounts.ToList();
            //grdAgingAccounts.DataBind();
            PageGridPanelStore.DataSource = accounts.ToList();
            PageGridPanelStore.DataBind();
            var count = 0;
            PageGridPanelSelectionModel.SelectedIndex = - 1;
            count = accounts.Count;
            PageGridPanelSelectionModel.SelectedIndex = count - 1;
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
            public string _Current
            {
                get
                {
                    if (this.Current == 0)
                        return string.Empty;
                    else if (this.Current == -1)
                        return "0.00";
                    else
                        return this.Current.ToString("N");
                }
            }
            public string _OneToThirty
            {
                get
                {
                    if (this.OneToThirty == 0)
                        return string.Empty;
                    else if (this.OneToThirty == -1)
                        return "0.00";
                    else
                        return this.OneToThirty.ToString("N");
                }
            }
            public string _ThirtyOneToSixty
            {
                get
                {
                    if (this.ThirtyOneToSixty == 0)
                        return string.Empty;
                    else if (this.ThirtyOneToSixty == -1)
                        return "0.00";
                    else
                        return this.ThirtyOneToSixty.ToString("N");
                }
            }
            public string _SixtyOneToNinety
            {
                get
                {
                    if (this.SixtyOneToNinety == 0)
                        return string.Empty;
                    else if (this.SixtyOneToNinety == -1)
                        return "0.00";
                    else
                        return this.SixtyOneToNinety.ToString("N");
                }
            }
            public string _OverNinety
            {
                get
                {
                    if (this.OverNinety == 0)
                        return string.Empty;
                    else if (this.OverNinety == -1)
                        return "0.00";
                    else
                        return this.OverNinety.ToString("N");
                }
            }
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

                //this.DueDate1 = loanAccount.MaturityDate.Value;
                if (payments != null)
                {
                    this.DueDate1 = payments.TransactionDate;
                    this.LastPaymentDate1 = this.DueDate1;
                    this.DueDate = this.DueDate1.Value.ToString("MMM dd, yyyy");
                    this.LastPaymentDate = this.DueDate;
                    this.LoanReleaseDate = loanAccount.LoanReleaseDate.Value.ToString("MMM dd, yyyy");
                    this.LoanReleaseDate1 = loanAccount.LoanReleaseDate;
                }
                else
                {
                    this.LastPaymentDate = "None";
                    this.DueDate1 = loanAccount.LoanReleaseDate;
                    this.DueDate = this.DueDate1.Value.ToString("MMM dd, yyyy");
                    this.LoanReleaseDate = this.DueDate1.Value.ToString("MMM dd, yyyy");
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

            public AgingAccountsModel()
            {

            }
        }
    }
}