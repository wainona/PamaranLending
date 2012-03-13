using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.BackgroundUseCases
{
    public partial class BillStatement : ActivityPageBase
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
            var today = DateTime.Today;
            var customerID =int.Parse(Request.QueryString["id"]);
            FillDetails(customerID,today);
            FillHeader(today);
        }
        public void FillDetails(int id, DateTime today)
        {
           var yesterday = today.AddDays(-1);
            var tommorow = today.AddDays(1);
            var borrowerPartyId = PartyRole.GetById(id).PartyId;
            var customerparty = ObjectContext.Parties.FirstOrDefault(entity => entity.Id == borrowerPartyId);

            /***Borrower's Name***/
            lblBorrowerName.Text = Person.GetPersonFullName(customerparty);
            /***Date Billed***/
            lblDateBilled.Text = String.Format("{0:MMMM dd, yyyy}", today);

            var loanAccounts = from la in ObjectContext.FinancialAccountRoles
                              join pr in ObjectContext.PartyRoles on la.PartyRoleId equals pr.Id
                              join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                              where la.PartyRole.PartyId == borrowerPartyId &&
                              (pr.RoleTypeId == RoleType.OwnerFinancialType.Id ||
                                pr.RoleTypeId == RoleType.CoOwnerFinancialType.Id ||
                                pr.RoleTypeId == RoleType.GuarantorFinancialType.Id) &&
                                pr.EndDate == null && las.IsActive == true
                                && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id 
                                || las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id 
                                || las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                              select la.FinancialAccount.LoanAccount;
           
            var receivables = from la in loanAccounts
                              join r in ObjectContext.Receivables on la.FinancialAccountId equals r.FinancialAccountId
                              join rs in ObjectContext.ReceivableStatus on r.Id equals rs.ReceivableId
                              where rs.IsActive == true && (rs.StatusTypeId != ReceivableStatusType.FullyPaidType.Id
                              || rs.StatusTypeId != ReceivableStatusType.CancelledType.Id)
                              orderby r.DueDate ascending
                              select r;
            

            var interestReceivables = from r in receivables
                                      where r.ReceivableTypeId == ReceivableType.InterestType.Id
                                      group r by r.FinancialAccountId into rfid
                                      select new { FinancialAccountId = rfid.Key, Balance = rfid.Sum(entity=> entity.Balance)};

            var allInterestReceivables = from la in loanAccounts
                                       join r in ObjectContext.Receivables on la.FinancialAccountId equals r.FinancialAccountId
                                       join rs in ObjectContext.ReceivableStatus on r.Id equals rs.ReceivableId
                                       where rs.IsActive == true && rs.StatusTypeId != ReceivableStatusType.CancelledType.Id
                                       && r.ReceivableTypeId == ReceivableType.InterestType.Id
                                       select r;

            decimal sumOfInterestPaid = 0;
            if(allInterestReceivables.Count() !=0)
                allInterestReceivables.Sum(entity => entity.Amount - entity.Balance);

            List<BilledReceivableModel> billedreceivables = new List<BilledReceivableModel>();
            decimal totalinterest = 0;
            decimal totalprincipal = 0;
            decimal totalprincipalPaid = 0;
            foreach (var loanAccount in loanAccounts)
            {
                string type = string.Empty;
                var roles = from far in ObjectContext.FinancialAccountRoles
                           where far.FinancialAccountId == loanAccount.FinancialAccountId
                           && far.PartyRole.PartyId == borrowerPartyId
                           select far;
                if (roles.Count() != 0)
                {
                    var role = roles.FirstOrDefault().PartyRole;
                    if (role != null)
                    {
                        var roletype = ObjectContext.RoleTypes.FirstOrDefault(entity => entity.Id == role.RoleTypeId);
                        type = roletype.Name;
                    }
                }
                decimal interestdue = 0;
                decimal principaldue = 0;
                decimal totaldues = 0;
                decimal additionalInterest = 0;
                
                var interest = interestReceivables.FirstOrDefault(entity => entity.FinancialAccountId == loanAccount.FinancialAccountId);
                if (interest != null) interestdue = interest.Balance;
                var agreementitems = from f in ObjectContext.FinancialAccounts
                                    join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                                    join ai in ObjectContext.AgreementItems on a.Id equals ai.AgreementId
                                    where ai.IsActive == true && a.EndDate == null && f.Id == loanAccount.FinancialAccountId
                                    select ai;
                if(agreementitems.Count() > 0){
                    var agreementitem = agreementitems.FirstOrDefault();
                    additionalInterest = GenerateBillFacade.GenerateAndSaveInterest(loanAccount, agreementitem, GenerateBillFacade.ManualBillingDisplay, today, today, today);
                }

                interestdue += additionalInterest;
                principaldue = loanAccount.LoanBalance;
                totaldues = principaldue + interestdue;
                totalprincipalPaid += (loanAccount.LoanAmount - loanAccount.LoanBalance);
                totalinterest += interestdue;
                totalprincipal += principaldue;
                if (totaldues > 0)
                {
                    BilledReceivableModel billedreceivable = new BilledReceivableModel();
                    billedreceivable.DateReleased = loanAccount.LoanReleaseDate;
                    billedreceivable.LoanID = loanAccount.FinancialAccountId;
                    billedreceivable.PrincipalDue = principaldue;
                    billedreceivable.InterestDue = interestdue;
                    billedreceivable.TotalAmountDue = totaldues;
                    billedreceivable.RoleType = type;
                    billedreceivables.Add(billedreceivable);
                }
            }


            /***Last Payment Made***/
            var lastpayment = from la in loanAccounts
                              join ft in ObjectContext.FinAcctTrans on la.FinancialAccountId equals ft.FinancialAccountId
                              orderby ft.TransactionDate descending
                              select ft;
            
            if (lastpayment.Count() > 0)
            {
                var lastTransactionDate = lastpayment.FirstOrDefault().TransactionDate;
                lblLastPayment.Text = String.Format("{0:MMMM dd, yyyy}", lastTransactionDate);
                lblPrincipalPaid.Text = totalprincipalPaid.ToString("N");
                lblInterestPaid.Text = sumOfInterestPaid.ToString("N");
            }
            else
            {
                lblLastPayment.Text = "None";
                lblPrincipalPaid.Text = "0";
                lblInterestPaid.Text = "0";
            };

                decimal total = totalinterest + totalprincipal;
                lblCurrentDue.Text = total.ToString("N");
                lblCurrentPrincipal.Text = totalprincipal.ToString("N");
                lblCurrentInterest.Text = totalinterest.ToString("N");
         
            strBillStatement.DataSource = billedreceivables.ToList();
            strBillStatement.DataBind();

        }
        public class BilledReceivableModel
        {
           // public DateTime? DueDate{get;set;}
            public int LoanID { get; set; }
            public DateTime? DateReleased { get; set; }
            public decimal PrincipalDue { get; set; }
            public decimal InterestDue { get; set; }
            public decimal TotalAmountDue { get; set; }
            public string RoleType { get; set; }
           // public decimal LoanBalance { get; set; }

        }
        public void FillHeader(DateTime today)
        {
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id);
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
    }
}