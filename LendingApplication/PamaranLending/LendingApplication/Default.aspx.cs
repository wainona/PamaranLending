using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using System.Web.Security;
using BusinessLogic;

namespace LendingApplication
{
    public partial class Default : ActivityPageBase
    {
        #region ..NavigationNodes
        public List<NavigationNodes> navigationNodes = new List<NavigationNodes>()
        {
            new NavigationNodes() { NodeName="Loan Products", NodeId="LoanCalculator", NodeHref="/BestPractice/ListFinancialProducts.aspx"},
            new NavigationNodes() { NodeName="Customers", NodeId="Customers", NodeHref="/Applications/CustomerUseCases/ListCustomers.aspx"},
            new NavigationNodes() { NodeName="Loan Applications", NodeId="LoanApplications", NodeHref="/Applications/LoanApplicationUseCases/ListLoanApplication.aspx"},
            new NavigationNodes() { NodeName="Loan Accounts", NodeId="LoanAccounts", NodeHref="/Applications/LoanUseCases/ListLoans.aspx" },
            new NavigationNodes() { NodeName="Loan Restructure", NodeId="LoanRestructure", NodeHref="/Applications/LoanRestructureUseCases/ListLoanRestructure.aspx" },
            new NavigationNodes() { NodeName="Advance Change", NodeId="AdvanceChange", NodeHref="/Applications/AdvanceChangeUseCases/ListAdvanceChange.aspx" },
            new NavigationNodes() { NodeName="Additional Loan", NodeId="AdditionalLoan", NodeHref="/Applications/LoanRestructureUseCases/ListAdditionalLoan.aspx" },
            new NavigationNodes() { NodeName="Disbursements", NodeId="LoanDisbursements", NodeHref="/Applications/DisbursementUseCases/ViewDisbursementList.aspx" },
            //new NavigationNodes() { NodeName="Billing", NodeId="Billing", NodeHref="/Applications/BackgroundUseCases/GenerateBill.aspx" },
            new NavigationNodes() { NodeName="Receipts", NodeId="Receipts", NodeHref="/Applications/ReceiptUseCases/ListReceipts.aspx" },
            new NavigationNodes() { NodeName="Cheques", NodeId="Cheques", NodeHref="/Applications/ChequeUseCases/ListCheques.aspx" },
            new NavigationNodes() { NodeName="Cheque Editor", NodeId="ChequeEditor", NodeHref="/Applications/ChequeEditorUseCases/ListChequeEditor.aspx" },
            new NavigationNodes() { NodeName="Payment", NodeId="Payment", NodeHref="/Applications/CollectionUseCases/ListCollection.aspx" },
            new NavigationNodes() { NodeName="Employees", NodeId="Employee", NodeHref="/Applications/EmployeeUseCases/ListEmployee.aspx" },
            new NavigationNodes() { NodeName="Contacts", NodeId="Contact", NodeHref="/Applications/ContactUseCases/ListContact.aspx" },
            new NavigationNodes() { NodeName="Banks", NodeId="Banks", NodeHref="/Applications/BankUseCases/ListBank.aspx" },
            new NavigationNodes() { NodeName="User Accounts", NodeId="UserAccount", NodeHref="/Applications/UserAccountsUseCases/ListUserAccounts.aspx" },
            new NavigationNodes() { NodeName="Holidays", NodeId="Holidays", NodeHref="/Applications/HolidayUseCases/ListHoliday.aspx" },
            new NavigationNodes() { NodeName="Cash On Vault", NodeId="CashOnVault", NodeHref="/Applications/CashOnVaultUseCases/AddCashOnVault.aspx" },
            //new NavigationNodes() { NodeName="Currency Conversion", NodeId="CurrencyConversion", NodeHref="/Applications/CurrencyUseCases/ListCurrencyConversion.aspx" },
            new NavigationNodes() { NodeName="Customer Classification", NodeId="CustomerClassification", NodeHref="/Applications/CustomerClassificationUseCases/ListCustomerClassification.aspx" },
            new NavigationNodes() { NodeName="Required Document Type", NodeId="RequiredDocumentType", NodeHref="/Applications/RequiredDocumentTypeUseCases/ListRequiredDocumentTypes.aspx" },
            new NavigationNodes() { NodeName="System Settings", NodeId="SystemSettings", NodeHref="/Applications/SystemSettingsUseCases/ListSystemSettings.aspx" },
            new NavigationNodes() { NodeName="Income Statement", NodeId="IncomeStatementReport", NodeHref="/Applications/Reports/IncomeStatementReport.aspx" },
            new NavigationNodes() { NodeName="Daily Transaction Report", NodeId="DailyReceivedAndReleasedReport", NodeHref="/Applications/Reports/DailyReceivedAndReleasedReport.aspx" },
            new NavigationNodes() { NodeName="Transaction Report", NodeId="TransactionReport", NodeHref="/Applications/Reports/TransactionReport.aspx" },
            new NavigationNodes() { NodeName="Aging of Accounts", NodeId="AgingOfAccounts", NodeHref="/Applications/AgingOfAccountsUseCases/ListAgingOfAccounts.aspx" },
            new NavigationNodes() { NodeName="Demand Letter", NodeId="DemandLetter", NodeHref="/Applications/DemandLetterUseCases/DemandLetterList.aspx" },
            new NavigationNodes() { NodeName="Bad Debts", NodeId="BadDebts", NodeHref="/Applications/Reports/BadDebtsReport.aspx" },
            //new NavigationNodes() { NodeName="Bill Statement", NodeId="BillStatement", NodeHref="/Applications/BackgroundUseCases/CustomersWithNewBill.aspx" },
            new NavigationNodes() { NodeName="Outstanding Loan Report", NodeId="OutstandingLoanReport", NodeHref="/Applications/Reports/OutstandingLoansReport.aspx" },
            new NavigationNodes() { NodeName="Summary of Paid Off Loans", NodeId="PaidOffLoans", NodeHref="/Applications/Reports/SummaryOfPaidOffLoans.aspx" },
            new NavigationNodes() { NodeName="Summary of Loans Granted", NodeId="SummaryOfLoansGranted", NodeHref="/Applications/Reports/SummaryOfLoansGranted.aspx" },
            new NavigationNodes() { NodeName="Outstanding Loans Schedule", NodeId="ScheduleOfOutstandingLoans", NodeHref="/Applications/Reports/ScheduleOfOutstandingLoans.aspx" },
            new NavigationNodes() { NodeName="Daily Checks Report", NodeId="DailyChecksReceived", NodeHref="/Applications/Reports/DailyChequesReceivedReport.aspx" },
            new NavigationNodes() { NodeName="Post Dated Checks Report", NodeId="PostDatedChecksReport", NodeHref="/Applications/Reports/PostDatedChecksReport.aspx" },
            new NavigationNodes() { NodeName="Teacher's Checks Report", NodeId="TeachersChecksReceived", NodeHref="/Applications/Reports/TeachersChequesReceivedReport.aspx" },
            new NavigationNodes() { NodeName="ForEx Transactions", NodeId="ForExTransactions", NodeHref="/ForeignExchangeApplication/ForExTransactionUseCases/ListForExTransactions.aspx" },
            new NavigationNodes() { NodeName="Foreign Disbursements", NodeId="ForeignDisbursements", NodeHref="/ForeignExchangeApplication/ForeignDisbursementUseCases/ForeignDisbursements.aspx" },
            new NavigationNodes() { NodeName="Exchange Rates", NodeId="ExchangeRates", NodeHref="/ForeignExchangeApplication/ExchangeRateUseCases/ListExchangeRates.aspx" },
            new NavigationNodes() { NodeName="Currency", NodeId="Currency", NodeHref="/ForeignExchangeApplication/CurrencyUseCases/ListCurrency.aspx" },
            new NavigationNodes() { NodeName="Foreign Exchange Report", NodeId="ForExReport", NodeHref="/ForeignExchangeApplication/ForExTransactionUseCases/ForExReport.aspx" }
        };
        #endregion

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
            //Session.Timeout = 4;
            //this.CheckSessionTimeout();
            this.keepAlive();
            if (X.IsAjaxRequest == false && this.IsPostBack == false) {
            var root = this.NavigationTree.Root;
            if (this.LoginInfo.IsAuthenticated)
            {
                var partyId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
                var userType = ObjectContext.UserAccounts.SingleOrDefault(entity => entity.EndDate == null && entity.PartyId == partyId).UserAccountType;
                NavigationManager navigationManager = new NavigationManager(userType);
                root.Add(navigationManager.NavigationRoot);

                var party = ObjectContext.Parties.SingleOrDefault(entity => entity.Id == partyId);
                lblNameAndTypeOfUser.Text = party.Name + " [" + userType.Name + "]";

                cmbSetHomePageStore.DataSource = FilterNodesByUserType(this.LoginInfo.UserType);
                cmbSetHomePageStore.DataBind();
            }
            }
        }

        protected void signout_Click(object sender, EventArgs e)
        {
            this.LoginInfo.SignOut();
            Response.Redirect("~/Security/LoginPage.aspx");
        }

        protected void setHomePage_Click(object sender, EventArgs e)
        {
            var userAccount = UserAccount.GetById(LoginInfo.UserId);
            if (!string.IsNullOrWhiteSpace(userAccount.HomePage))
            {
                cmbSetHomePage.SelectedItem.Value = userAccount.HomePage;
            }
            wdnSetHomePage.Hidden = false;
        }

        protected void btnSaveSetHomePage_Click(object sender, EventArgs e)
        {
            var userAccount = UserAccount.GetById(LoginInfo.UserId);
            var selectedValue = cmbSetHomePage.SelectedItem.Value;
            if (selectedValue != null)
            {
                userAccount.HomePage = selectedValue;
            }
            ObjectContext.SaveChanges();
        }

        [DirectMethod]
        public void RetrieveAndShowUserAccount()
        {
            var id = this.LoginInfo.UserId;
            var userAccount = UserAccount.GetById(id);
            if(userAccount != null) {
                var party = ObjectContext.Parties.SingleOrDefault(entity => entity.Id == userAccount.PartyId);
                lblEmployeeName.Text = party.Name;
            }
            
            lblUsername.Text = this.LoginInfo.Username;
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            cmbPasswordQuestion.Clear();
            txtPasswordAnswer.Clear();

            wndChangePassword.Show();
        }

        [DirectMethod]
        public NavigationNodes RetrieveHomePage()
        {
            var id = this.LoginInfo.UserId;
            var userAccount = UserAccount.GetById(id);
            if (userAccount != null)
            {
                if (!string.IsNullOrWhiteSpace(userAccount.HomePage))
                {
                    var node = navigationNodes.SingleOrDefault(entity => entity.NodeId.Equals(userAccount.HomePage));
                    return node;
                }
            }
            return null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var id = this.LoginInfo.UserId;
            var userAccount = ObjectContext.UserAccounts.SingleOrDefault(entity => entity.Id == id && entity.EndDate == null);

            if (userAccount.Password != txtNewPassword.Text) userAccount.Password = SimplifiedHash.ComputeHash(txtNewPassword.Text, SimplifiedHash.HashType.sha1);
            if (userAccount.SecurityQuestion != cmbPasswordQuestion.Text)userAccount.SecurityQuestion = cmbPasswordQuestion.Text;
            if (userAccount.SecurityAnswer != txtPasswordAnswer.Text) userAccount.SecurityAnswer = txtPasswordAnswer.Text;

            ObjectContext.SaveChanges();
            wndChangePassword.Hide();
        }

        protected void onNode_Click(object sender, EventArgs e)
        {
            onMessageReceived();
        }

        [DirectMethod]
        public void onMessageReceived()
        {
            this.keepAlive();
        }

        protected void CheckField(object sender, RemoteValidationEventArgs e)
        {
            TextField field = (TextField)sender;

            var id = this.LoginInfo.UserId;
            var userAccount = UserAccount.GetById(id);
            if (SimplifiedHash.VerifyHash(field.Text, userAccount.Password, SimplifiedHash.HashType.sha1) == false)
            {
                e.Success = false;
                e.ErrorMessage = "Incorrect Password";
            }
            else
            {
                e.Success = true;
            }
            System.Threading.Thread.Sleep(1000);
        }

        private void CheckSessionTimeout()
        {
            string msgSession = "Warning: Within the next 3 minutes, if you do not do anything, "+
                       " our system will redirect to the login page. Please save changed data.";
            //time to remind, 3 minutes before session ends
            int int_MilliSecondsTimeReminder = (this.Session.Timeout * 60000) - 3 * 60000; 
            //time to redirect, 5 milliseconds before session ends
            int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 5; 

            string str_Script = @"
                    var myTimeReminder, myTimeOut; 
                    clearTimeout(myTimeReminder); 
                    clearTimeout(myTimeOut); " +
                    "var sessionTimeReminder = " + 
		        int_MilliSecondsTimeReminder.ToString() + "; " +
                    "var sessionTimeout = " + int_MilliSecondsTimeOut.ToString() + ";" +
                    "function doReminder(){ alert('" + msgSession + "'); }" +
                    "function doRedirect(){ window.top.location.href='/Security/LoginPage.aspx'; }" + @"
                    myTimeReminder=setTimeout('doReminder()', sessionTimeReminder); 
                    myTimeOut=setTimeout('doRedirect()', sessionTimeout); ";

             ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), 
                   "CheckSessionOut", str_Script, true);
        }

        protected List<NavigationNodes> FilterNodesByUserType(string userType)
        {
            var nodes = navigationNodes;
            if (userType == UserAccountType.Teller.Name || userType == UserAccountType.Accountant.Name)
            {
                nodes = nodes.Where(entity =>
                    entity.NodeId != "Employee" &&
                    entity.NodeId != "CustomerClassification" &&
                    entity.NodeId != "RequiredDocumentType" &&
                    entity.NodeId != "UserAccount" &&
                    entity.NodeId != "SystemSettings"
                ).ToList();
            }
            return nodes;
        }

        //protected void Page_PreRender(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60)) + "; Url=/Security/LoginPage.aspx?YourSession=expired");
        //}

        //override protected void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    if (Session["UserName"] == null)
        //    {
        //        this.LoginInfo.SignOut();
        //        X.MessageBox.Alert("Information", "Session Expired! Please login again").Show();
        //        Response.Redirect("~/Security/LoginPage.aspx");
        //    }
        //}

        private int GetFormsTimeout()
        {
            System.Xml.XmlDocument x = new System.Xml.XmlDocument();
            x.Load(Request.PhysicalApplicationPath + "/web.config");
            System.Xml.XmlNode node = x.SelectSingleNode("/configuration/system.web/sessionState");
            int Timeout = int.Parse(node.Attributes["timeout"].Value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            return Timeout;
        }

        private void keepAlive()
        {
            //figure out the full url of the logout page configured in web.config + the return url which is the current page

            String logoutUrl = "/Security/LoginPage.aspx";
            String formTimeOut = "";
            String minutesToWarning = "";
            //figure out how many minutes before time out the user wants to get warned. 

            String minutesBeforeLoggedOut = "3"; 
            if (minutesBeforeLoggedOut == null || minutesBeforeLoggedOut == "")
            {
                minutesBeforeLoggedOut = "3";
            }

            //if the user selected "-1", then it never gets timedout. If not, figure out, how many minutes after the page loaded, give the user the warning
            if (!minutesBeforeLoggedOut.Equals("-1"))
            {
                int formsTimeout = GetFormsTimeout();
                formTimeOut = Convert.ToString(formsTimeout);
                minutesToWarning = Convert.ToString(formsTimeout - Convert.ToInt32(minutesBeforeLoggedOut));
            }
            String script = @"
            <script type='text/javascript'>
                var formTimeOut = " + formTimeOut + @";
                var minutesToWarning = " + minutesToWarning + @";
                var minutesBeforeLoggedOut = " + minutesBeforeLoggedOut + @";
                var loginUrl  = '" + logoutUrl + @"';
            </script>
            ";

            if (!ClientScript.IsClientScriptBlockRegistered("WarnTimeOut"))
                ClientScript.RegisterClientScriptBlock(typeof(Page), "WarnTimeOut", script);
        }

        public class NavigationNodes
        {
            public string NodeName { get; set; }
            public string NodeId { get; set; }
            public string NodeHref { get; set; }
        }
    }
}
