using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.DemandLetterUseCases
{
    public partial class SecondDemandLetter : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                allowed.Add("Admin");
                return allowed;
            }
        }

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
                var selectedLoanId = int.Parse(Request.QueryString["id"]);
                RetreiveHeaderDetails();
                FillFields(selectedLoanId);
            }
            lblDateToday.Text = DateTime.Now.Date.ToString("MMMM dd, yyyy");
        }

        private void RetreiveHeaderDetails()
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            var lenderName = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == lenderPartyRole.Party.Id).OrganizationName;
            lblLenderName.Text = lenderName;
            lblLenderName2.Text = lenderName;

            var lenderAddress = PostalAddress.GetCurrentPostalAddress(lenderPartyRole.Party, PostalAddressType.BusinessAddressType, true);
            lblLenderAddress.Text = StringConcatUtility.Build(", ", lenderAddress.StreetAddress, lenderAddress.Barangay,
                                                                    lenderAddress.City, lenderAddress.Municipality);

            lblLenderTelephoneNumber.Text = "Tel. No. " + PrintFacade.GetPrimaryPhoneNumber(lenderPartyRole.Party, lenderAddress);
            lblLenderFaxNumber.Text = "Fax " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }

        private void FillFields(int selectedLoanId)
        {
            var dueDateAndAmount = CreateQueryAmortizationSchedule(selectedLoanId);
            InstallmentsDueStore.DataSource = dueDateAndAmount.ToList();
            InstallmentsDueStore.DataBind();

            lblTotalIntallmentsDue.Text = dueDateAndAmount.Sum(entity => entity.AmountDue).ToString("N");

            var financialAccountRole = ObjectContext.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == selectedLoanId
                                                        && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.PartyRole.EndDate == null);
            var party = financialAccountRole.PartyRole.Party;
            var pa = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, true);
            lblOwnerName.Text = Person.GetPersonFullName(party);
            lblOwnerName2.Text = Person.GetPersonFullName(party);
            lblOwnerAddress.Text = StringConcatUtility.Build(", ", pa.StreetAddress, pa.Barangay, pa.City, pa.Municipality, pa.Province, pa.Country.Name, pa.PostalCode);

            lblOwnerNameAddressed.Text = party.Person.LastNameString;
            lblLoanAmount.Text = ConvertNumbers.EnglishFromNumber((double)financialAccountRole.FinancialAccount.LoanAccount.LoanAmount) + " PESOS";
            lblOutstandingBalance.Text =  financialAccountRole.FinancialAccount.LoanAccount.LoanBalance.ToString("N");

            var agreement = financialAccountRole.FinancialAccount.Agreement;
            lblAgreementDate.Text = agreement.AgreementDate.ToString("MMMM dd, yyyy");

            var coownerGuarantorList = CreateQueryCoownerGuarantor(selectedLoanId);
            List<CoOwnerGuarantorModel> coOwnerGuarantors = new List<CoOwnerGuarantorModel>();

            foreach (var item in coownerGuarantorList)
            {
                CoOwnerGuarantorModel coOwnerGuarantor = new CoOwnerGuarantorModel();
                coOwnerGuarantor.NameAddress = item.Name;
                coOwnerGuarantors.Add(coOwnerGuarantor);
                CoOwnerGuarantorModel coOwnerGuarantor2 = new CoOwnerGuarantorModel();
                coOwnerGuarantor2.NameAddress = item.Address;
                coOwnerGuarantors.Add(coOwnerGuarantor2);
                CoOwnerGuarantorModel coOwnerGuarantor3 = new CoOwnerGuarantorModel();
                coOwnerGuarantor3.NameAddress = item.Spacer;
                coOwnerGuarantors.Add(coOwnerGuarantor3);
            }

            grdCoOwner.DataSource = coOwnerGuarantors.ToList();
            grdCoOwner.DataBind();
        }

        private List<DueDatesModel> CreateQueryAmortizationSchedule(int selectedFinancialAccountID)
        {
            List<DueDatesModel> dueDate = new List<DueDatesModel>();
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == selectedFinancialAccountID);
            var date = DateTime.Now;
            var query = from la in ObjectContext.LoanAccounts
                        join r in ObjectContext.Receivables on la.FinancialAccountId equals r.FinancialAccountId
                        join rs in ObjectContext.ReceivableStatus on r.Id equals rs.ReceivableId
                        where la.FinancialAccountId == selectedFinancialAccountID
                        && r.ReceivableTypeId != ReceivableType.PastDueType.Id && r.DueDate <= date && r.Balance > 0
                        && rs.IsActive == true && rs.StatusTypeId != ReceivableStatusType.CancelledType.Id
                        group r by r.DueDate into dd
                        select new DueDatesModel()
                        {
                            DueDate = dd.Key,
                            AmountDue = dd.Sum(entity => entity.Balance)
                        };

            query = query.OrderBy(entity => entity.DueDate);
            dueDate = query.ToList();
            return dueDate;
        }

        public class DueDatesModel
        {
            public DateTime DueDate { get; set; }
            public decimal AmountDue { get; set; }
        }

        private class CoOwnerGuarantorModel 
        {
            public string NameAddress { get; set; }
        }

        private class NameAddressModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Spacer { get; set; }

            public NameAddressModel(PartyRole partyRole)
            {

                this.Name = Person.GetPersonFullName(partyRole.Party);

                Address postalAddress = partyRole.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                    && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

                if (postalAddress != null && postalAddress.PostalAddress != null)
                {
                    PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                    this.Address = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                  postalAddressSpecific.Barangay,
                                  postalAddressSpecific.Municipality,
                                  postalAddressSpecific.City,
                                  postalAddressSpecific.Province,
                                  postalAddressSpecific.State,
                                  postalAddressSpecific.Country.Name,
                                  postalAddressSpecific.PostalCode);
                }

                this.Spacer = "";
            }
        }

        private List<NameAddressModel> CreateQueryCoownerGuarantor(int selectedFinancialAccountID)
        {
            var partyRoleQuery = from far in ObjectContext.FinancialAccountRoles
                                 join pr in ObjectContext.PartyRoles.Where(entity => entity.RoleTypeId == RoleType.CoOwnerFinancialType.Id
                                                                             || entity.RoleTypeId == RoleType.GuarantorFinancialType.Id && entity.EndDate == null)
                                     on far.PartyRoleId equals pr.Id
                                 where far.FinancialAccountId == selectedFinancialAccountID && pr.EndDate == null
                                 select pr;

            List<NameAddressModel> coOwnerGuarantorList = new List<NameAddressModel>();

            foreach (var item in partyRoleQuery)
            {
                coOwnerGuarantorList.Add(new NameAddressModel(item));
            }

            //query = query.OrderBy(entity => entity.FinancialAccountRole);
            return coOwnerGuarantorList;
        }

    }
}