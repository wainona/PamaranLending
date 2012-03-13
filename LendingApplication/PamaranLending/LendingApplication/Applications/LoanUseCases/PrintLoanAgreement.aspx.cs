using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.LoanUseCases
{
    public partial class PrintLoanAgreement : ActivityPageBase
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
                var selectedLoanId = int.Parse(Request.QueryString["id"]);
                var amortizationscheduleList = CreateQueryAmortizationSchedule(selectedLoanId);
                grdAmortScheduleLoanAgreement.DataSource = amortizationscheduleList.ToList();
                grdAmortScheduleLoanAgreement.DataBind();
                RetrieveLenderInformation();
                RetrieveLoanAccountDetails(selectedLoanId);
            }
        }

        protected void RetrieveLenderInformation()
        {
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            var organization = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == party.Id);

            lblLenderName.Text = organization.OrganizationName;
            lblLenderNameForCheck.Text = organization.OrganizationName;
            lblpLenderName.Text = organization.OrganizationName;
            lblHeaderLenderName.Text = organization.OrganizationName;//Header

            var postalAddress = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.BusinessAddressType, entity => entity.PostalAddress.IsPrimary == true);
            var primNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessPhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
            var secNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessPhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == false);
            var faxNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessFaxNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);

            if (postalAddress != null)
            {
                lblLenderStreetAddress.Text = postalAddress.StreetAddress;
                lblLenderCity.Text = postalAddress.City;
                lblLenderState.Text = postalAddress.Province;
                lblLenderZip.Text = postalAddress.PostalCode;
                if (primNumberSpecific != null)
                {
                    lblLenderAreaCodeTelephoneNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + primNumberSpecific.AreaCode + " " + primNumberSpecific.PhoneNumber;
                    lblHeaderLenderPrimPhone.Text = postalAddress.Country.CountryTelephoneCode + " " + primNumberSpecific.AreaCode + " " + primNumberSpecific.PhoneNumber;
                }

                if (secNumberSpecific != null)
                    lblHeaderLenderSecPhone.Text = postalAddress.Country.CountryTelephoneCode + " " + secNumberSpecific.AreaCode + " " + secNumberSpecific.PhoneNumber;

                if (faxNumberSpecific != null)
                    lblHeaderLenderFaxNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + faxNumberSpecific.AreaCode + " " + faxNumberSpecific.PhoneNumber;
            }

            //Loan Agreement Header Address
            lblHeaderLenderStreetAddress.Text = postalAddress.StreetAddress;
            lblHeaderBarangay.Text = postalAddress.Barangay;
            lblHeaderCity.Text = postalAddress.City;
            lblHeaderMunicipality.Text = postalAddress.Municipality;
            if (postalAddress.Province != null || postalAddress.Province != string.Empty)
                lblHeaderProvince.Text = postalAddress.Province;
            lblHeaderCountry.Text = postalAddress.Country.Name;
            lblHeaderPostalCode.Text = postalAddress.PostalCode;

            var primaryEmailAddressSpecific = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.BusinessEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true);
            if (primaryEmailAddressSpecific != null)
                lblHeaderLenderEmail.Text = primaryEmailAddressSpecific.ElectronicAddressString;
        }

        protected void RetrieveLoanAccountDetails(int selectedFinancialAccoutnID)
        {
            var loanAccount = ObjectContext.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID);
            var financialAccountProduct = ObjectContext.FinancialAccountProducts.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID);
            var financialProduct = financialAccountProduct.FinancialProduct;//FOR: Loan Product Name
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Agreement.AgreementType.Name == "Loan Agreement"
                                    && entity.Id == loanAccount.FinancialAccountId);

            var agreement = financialAccount.Agreement;

            int agreementID = agreement.Id;//AgreementId

            var agreementItem = ObjectContext.AgreementItems.Single(entity => entity.AgreementId == agreementID && entity.IsActive == true);//agreementItem
            var loanAccountStatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID && entity.IsActive == true);
            var statusType = loanAccountStatus.LoanAccountStatusType;
            var amortizationSchedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreementID);
            var amortizationScheduleItems = amortizationSchedule.AmortizationScheduleItems.OrderBy(entity => entity.ScheduledPaymentDate).ToList();
            var count = amortizationScheduleItems.Count();

            //lblAmortLabelLoanAgreement.Text = "Amortization Schedule";

            decimal addOnInterestSum = 0;
            decimal monthlyPrincipalPayment = 0;
            DateTime? firstPaymentDate = null;
            DateTime? lastPaymentDate = null;
            if (count != 0)
            {
                monthlyPrincipalPayment = amortizationScheduleItems.Skip(1).Take(1).FirstOrDefault().PrincipalPayment + amortizationScheduleItems.Skip(1).Take(1).FirstOrDefault().InterestPayment;
                firstPaymentDate = amortizationScheduleItems.First().ScheduledPaymentDate;
                lastPaymentDate = amortizationScheduleItems.ElementAt(count - 1).ScheduledPaymentDate;
                //lblAmortLabelLoanAgreement.Text = string.Empty;
            }

            //if status is restructured
            var rec = from r in ObjectContext.Receivables
                      join pa in ObjectContext.PaymentApplications on r.Id equals pa.ReceivableId
                      join rs in ObjectContext.ReceivableStatus on r.Id equals rs.ReceivableId
                      where r.ReceivableTypeId == ReceivableType.InterestType.Id && r.FinancialAccountId == selectedFinancialAccoutnID
                      && rs.StatusTypeId == ReceivableStatusType.CancelledType.Id
                      group r by r.FinancialAccountId into sr
                      select new
                      {
                          sum = sr.Sum(entity => entity.Amount - entity.Balance)
                      };

            foreach (var item in amortizationScheduleItems)
            {
                addOnInterestSum = addOnInterestSum + item.InterestPayment;
            }

            if (rec.Count() != 0)
            {
                foreach (var item in rec)
                {
                    addOnInterestSum = addOnInterestSum + item.sum;
                }
            }

           
            string monthlyPayment = "";
            if (agreementItem.InterestComputationMode == "Straight Line Method")
            {
                monthlyPayment = monthlyPrincipalPayment.ToString("N");
            }
            else
            {
                monthlyPayment = "_____________";
            }

            string paymentMode = agreementItem.PaymentMode;
            //Loan Agreement
            if (agreementItem.LoanTermLength != 0)
            {
                if (paymentMode == "Annually") paymentMode = "Annual";
                lblAmortizationSchedLoanAgreement.Text = "Amortization Schedule";
                lblInstallments.Text = "Borrower will pay " + agreementItem.LoanTermLength.ToString() + " payments of " + monthlyPayment + " (Php) each at " + paymentMode + " intervals.";
            }
            lblLoanAmount.Text = loanAccount.LoanAmount.ToString("N");
            lblInterestRate.Text = agreement.Application.LoanApplication.InterestRate.ToString() + " %";
            lblPastDueInterestRate.Text = agreement.Application.LoanApplication.PastDueInterestRate.ToString();


            lblpLoanAmount.Text = loanAccount.LoanAmount.ToString("N");


            if (firstPaymentDate.HasValue && lastPaymentDate.HasValue)
            {
                lblLoanPeriod.Text = firstPaymentDate.Value.ToString("MMMM dd, yyyy") + " - " + lastPaymentDate.Value.ToString("MMMM dd, yyyy");
            }

            lblpInterestValPlusProdFeatName.Text = agreementItem.InterestRate + "% " + agreementItem.InterestRateDescription;

        }

        private IQueryable<AmortizationScheduleModel> CreateQueryAmortizationSchedule(int selectedFinancialAccountID)
        {
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == selectedFinancialAccountID);
            //var agreement = ObjectContext.Agreements.SingleOrDefault(entity=>entity.Id == financialAccount.AgreementId);
            var amortizationSchedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == financialAccount.AgreementId);

            if (amortizationSchedule != null)
            {
                var query = from asi in ObjectContext.AmortizationScheduleItems
                            where asi.AmortizationScheduleId == amortizationSchedule.Id
                            select new AmortizationScheduleModel
                            {
                                Id = asi.Id,
                                ScheduledPaymentDate = asi.ScheduledPaymentDate,
                                PrincipalPayment = asi.PrincipalPayment,
                                InterestPayment = asi.InterestPayment,
                                PrincipalBalance = asi.PrincipalBalance,
                                TotalLoanBalance = asi.TotalLoanBalance,
                                IsBilledIndicator = asi.IsBilledIndicator
                            };

                query = query.OrderBy(entity => entity.ScheduledPaymentDate);
                return query;
            }
            else
            {
                return null;
            }
        }
    }
}