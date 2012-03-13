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
    public partial class PrintLoanRecord : ActivityPageBase
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
                int selectedLoanId = int.Parse(Request.QueryString["id"]);
                hdnLoanId.Text = selectedLoanId.ToString();

                //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderName(party);
                var postalAddress = SetAndGetPostalAddress(party);

                if(!string.IsNullOrWhiteSpace(GetPrimaryPhoneNumber(party)))
                lblPrimTelNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetPrimaryPhoneNumber(party);

                if(!string.IsNullOrWhiteSpace(GetSecondaryPhoneNumber(party)))
                lblSecTelNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetSecondaryPhoneNumber(party);

                lblFaxNumber.Text = postalAddress.Country.CountryTelephoneCode + " " + GetFaxNumber(party);
                lblEmailAddress.Text = GetEmailAddress(party);

                //Loan Account Owner
                //lblAccountOwner.Text = "";
                //lblDistrictOwner.Text = "";
                //lblStationNumberOwner.Text = "";
                //lblAddressOwner.Text = "";
                //lblCellPhoneNumberOwner.Text = "";
                //lblPrimTelNumberOwner.Text = "";
                //lblEmailAddressOwner.Text = "";

                //Co-Owner / Guarantor List
                var coownerGuarantorView = CreateQueryCoownerGuarantor(selectedLoanId);
                grdCoOwnerGuarantors.DataSource = coownerGuarantorView;
                grdCoOwnerGuarantors.DataBind();

                //Loan Account Details
                RetrieveLoanAccountDetails(selectedLoanId);

                //Amortization Schedule
                var amortSchedList = CreateQueryAmortizationSchedule(selectedLoanId);
                if (amortSchedList != null)
                {
                    grdAmortizationSchedule.DataSource = amortSchedList.ToList();
                    grdAmortizationSchedule.DataBind();
                }

                //Payment History

                var LoanHistoryView = LoanAccount.CreateQueryLoanHistory(selectedLoanId);
                grdPaymentHisotry.DataSource = LoanHistoryView.ToList();
                grdPaymentHisotry.DataBind();

                DisplayOwner(selectedLoanId);
                DisplayDatePaidOff(selectedLoanId);
            }
        }
   
        public void FillLenderName(Party party)
        {
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
        }

        public PostalAddress SetAndGetPostalAddress(Party party)
        {
            var postalAddressType = PostalAddressType.BusinessAddressType.Name;
            var addressType = ObjectContext.AddressTypes.First(entity => entity.Name.Equals("Postal Address"));
            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressType);
            var address = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressType.Id &&
                                entity.PostalAddress.PostalAddressType.Name == postalAddressType && entity.PostalAddress.IsPrimary == true);
            var pa = address.PostalAddress;

            if (!string.IsNullOrWhiteSpace(pa.StreetAddress))
            {
                lblStreetAddress.Text = pa.StreetAddress + ",";
            }
            
            lblBarangay.Text = pa.Barangay;
            lblCity.Text = pa.City;
            lblMunicipality.Text = pa.Municipality;
            lblProvince.Text = pa.Province;
            lblCountry.Text = pa.Country.Name;
            lblPostalCode.Text = pa.PostalCode;

            return pa;

        }

        public string GetPrimaryPhoneNumber(Party party)
        {
            string primPhoneNUmber = string.Empty;
            //Primary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
            var telecomNumberTypeBusinessPhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

            InitialDatabaseValueChecker.ThrowIfNull<AddressType>(addressTypTelecom);
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeBusinessPhoneNumber);


            var primaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary);

            // ICHECK KO UGMA
            if (primaryNumber != null)
            {
                var pN = primaryNumber.TelecommunicationsNumber;
                primPhoneNUmber = "(" + pN.AreaCode + ") " + pN.PhoneNumber;

            }
            return primPhoneNUmber;
        }

        public string GetSecondaryPhoneNumber(Party party)
        {
            string secPhoneNUmber = string.Empty;
            //Secondary Business Phone Number
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));
            var telecomNumberTypeBusinessPhoneNumber = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessPhoneNumber));

            var secondaryNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeBusinessPhoneNumber.Id && entity.TelecommunicationsNumber != null
                && entity.TelecommunicationsNumber.IsPrimary == false);

            if (secondaryNumber != null)
            {
                var sN = secondaryNumber.TelecommunicationsNumber;
                secPhoneNUmber = "(" + sN.AreaCode + ") " + sN.PhoneNumber;
            }
            return secPhoneNUmber;
        }

        public string GetFaxNumber(Party party)
        {
            string faxPhoneNUmber = string.Empty;
            var telecomNumberTypeFax = ObjectContext.TelecommunicationsNumberTypes.SingleOrDefault(entity => entity.Name.Equals(TelecommunicationsNumberTypeEnums.BusinessFaxNumber));
            InitialDatabaseValueChecker.ThrowIfNull<TelecommunicationsNumberType>(telecomNumberTypeFax);
            var addressTypTelecom = ObjectContext.AddressTypes.First(entity => entity.Name.Equals(AddressTypeEnums.TelecommunicationNumber));

            var faxNumber = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypTelecom.Id
                && entity.TelecommunicationsNumber.TypeId.Value == telecomNumberTypeFax.Id && entity.TelecommunicationsNumber.IsPrimary);

            if (faxNumber != null)
            {
                var fN = faxNumber.TelecommunicationsNumber;
                faxPhoneNUmber = "(" + fN.AreaCode + ") " + fN.PhoneNumber;

            }
            return faxPhoneNUmber;
        }

        public string GetEmailAddress(Party party)
        {
            string emailAddress = string.Empty;

            var addressTypeId = AddressType.ElectronicAddressType.Id;
            var emailTypeId = ElectronicAddressType.BusinessEmailAddressType.Id;
            var primaryEmail = party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == addressTypeId
                                                 && entity.ElectronicAddress.ElectronicAddressTypeId.Value == emailTypeId
                                                 && entity.ElectronicAddress.IsPrimary);

            if (primaryEmail != null)
            {
                var ea = primaryEmail.ElectronicAddress;
                //Email Address
                emailAddress = ea.ElectronicAddressString;
            }

            return emailAddress;
        }

        public void DisplayOwner(int selectedFinancialAccountID)
        {
            //Account Owners Basic Information
            var financialAccountRole = ObjectContext.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccountID && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id);
            if (financialAccountRole == null) return;
            int financialAccountPartyRoleID = financialAccountRole.PartyRoleId;

            var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.Id == financialAccountPartyRoleID);
            int partyRolePartyID = partyRole.PartyId;

            var party = partyRole.Party;
            var person = party.Person;

            lblAccountOwner.Text = party.Name;
            //PICTURE
            if (person != null) imgPersonPicture.ImageUrl = person.ImageFilename;

            var customer = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.PartyId == party.Id && entity.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id && entity.EndDate == null);
            var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == customer.Id && entity.EndDate == null);
            var classificationType = customerClassification.ClassificationType;

            lblDistrictOwner.Text = classificationType.District;
            lblStationNumberOwner.Text = classificationType.StationNumber;

            //CONTACT INFORMATION
            var homePostalAddressType = PostalAddressType.HomeAddressType.Name;
            var businessPostalAddressType = PostalAddressType.BusinessAddressType.Name;

            PostalAddress postalAddressSpecific = null;
            TelecommunicationsNumber primNumberSpecific = null;
            TelecommunicationsNumber cellTelecomNumberSpecific = null;
            ElectronicAddress primaryEmailAddressSpecific = null;

            //IF OWNER IS ORGANIZATION
            if (party.PartyType == PartyType.OrganizationType)
            {
                //Postal Address
                postalAddressSpecific = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.BusinessAddressType, entity => entity.PostalAddress.IsPrimary == true);
                //Business Mobile Number
                cellTelecomNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessMobileNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                //Business Telephone Number
                primNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessPhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                //Business Email Address
                primaryEmailAddressSpecific = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.BusinessEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true);
            }
            //ELSE IF OWNER IS PERSON
            else
            {
                //Postal Address
                postalAddressSpecific = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, entity => entity.PostalAddress.IsPrimary == true);
                //Personal Mobile Number
                cellTelecomNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.PersonalMobileNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                //Home Telephone Number
                primNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.HomePhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                //Personal Email Address
                primaryEmailAddressSpecific = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true);
            }

            if (postalAddressSpecific != null)
            {
                lblAddressOwner.Text = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress, postalAddressSpecific.Barangay, postalAddressSpecific.City,
                                                            postalAddressSpecific.Municipality, postalAddressSpecific.Province, postalAddressSpecific.Country.Name,
                                                            postalAddressSpecific.PostalCode);
            }
            //fill contact numbers
            

            if (cellTelecomNumberSpecific != null)
            {
                lblCellPhoneNumberOwner.Text = postalAddressSpecific.Country.CountryTelephoneCode + " " + cellTelecomNumberSpecific.AreaCode + " " + cellTelecomNumberSpecific.PhoneNumber;
            }

            if (primNumberSpecific != null)
            {
                lblPrimTelNumberOwner.Text = postalAddressSpecific.Country.CountryTelephoneCode + " " + primNumberSpecific.AreaCode + " " + primNumberSpecific.PhoneNumber;
            }

            if (primaryEmailAddressSpecific != null)
            {
                //fill email address
                if (primaryEmailAddressSpecific.ElectronicAddressString != null) lblEmailAddressOwner.Text = primaryEmailAddressSpecific.ElectronicAddressString;

            }
        }

        public void DisplayDatePaidOff(int selectedFinancialAccountID)
        {
            var loanstatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccountID
                                   && entity.IsActive == true && entity.StatusTypeId == LoanAccountStatusType.PaidOffType.Id);
            
        }

        protected void RetrieveLoanAccountDetails(int selectedFinancialAccoutnID)
        {
            var loanAccount = ObjectContext.LoanAccounts.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID);
            var financialAccountProduct = ObjectContext.FinancialAccountProducts.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID);
            var financialProduct = financialAccountProduct.FinancialProduct;//FOR: Loan Product Name
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Agreement.AgreementType.Name == "Loan Agreement"
                                    && entity.Id == loanAccount.FinancialAccountId);

            var agreement = financialAccount.Agreement;

            var agreementID = agreement.Id;//AgreementId

            var agreementItem = ObjectContext.AgreementItems.Single(entity => entity.AgreementId == agreementID && entity.IsActive == true);//agreementItem
            var loanAccountStatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccoutnID && entity.IsActive == true);
            var statusType = loanAccountStatus.LoanAccountStatusType;
            var amortizationSchedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreementID);
            var amortizationScheduleItems = amortizationSchedule.AmortizationScheduleItems.OrderBy(entity => entity.ScheduledPaymentDate).ToList();
            var count = amortizationScheduleItems.Count();

            decimal addOnInterestSum = 0;
            decimal monthlyPrincipalPayment = 0;
            DateTime? firstPaymentDate = null;
            DateTime? lastPaymentDate = null;
            if (count != 0)
            {
                monthlyPrincipalPayment = amortizationScheduleItems.FirstOrDefault().PrincipalPayment + amortizationScheduleItems.FirstOrDefault().InterestPayment;
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

            //<Payment Mode> Amortization Field
            lblAmortizationLabel.Text = agreementItem.PaymentMode;

            lblLoanID.Text = selectedFinancialAccoutnID.ToString();
            lblLoanReleaseDate.Text = loanAccount.LoanReleaseDate.Value.ToString("MMMM dd, yyyy");
            if (loanAccount.MaturityDate.HasValue)
                lblMaturityDate.Text = loanAccount.MaturityDate.Value.ToString("MMMM dd, yyyy");
            lblLoanTerm.Text = agreementItem.LoanTermLength + " " + agreementItem.LoanTermUom;
            lblLoanStatus.Text = statusType.Name + " on " + loanAccountStatus.TransitionDateTime.ToString("MMMM dd, yyyy");
            lblStatusComment.Text = loanAccountStatus.Remarks;
            lblLoanApplicationID.Text = agreement.ApplicationId.ToString();
            lblLoanProductName.Text = financialProduct.Name;
            lblPaymentMode.Text = agreementItem.PaymentMode;
            lblInterestComputationMode.Text = agreementItem.InterestComputationMode;
            lblInterest.Text = agreementItem.InterestRate + "% " + agreementItem.InterestRateDescription;
            if(!string.IsNullOrWhiteSpace(agreementItem.PastDueInterestRate.ToString()))
            //lblPastDueInterest.Text = agreementItem.PastDueInterestRate + "% " + agreementItem.PastDueInterestRateDescript;
            lblLoanAmount.Text = loanAccount.LoanAmount.ToString("N");
            
            lblAddOnInterest.Text = addOnInterestSum.ToString("N");
            
            /********************************************/
            lblMethodOfChargingInterest.Text = agreementItem.MethodOfChargingInterest;
            lblTotalLoan.Text = (loanAccount.LoanAmount + addOnInterestSum).ToString("N");
            if (agreementItem.InterestComputationMode == "Straight Line Method")
                lblAmortization.Text = monthlyPrincipalPayment.ToString("N");
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
                                TotalLoanBalance = asi.TotalLoanBalance
                            };

                query = query.OrderBy(entity => entity.ScheduledPaymentDate);
                return query;
            }
            else
            {
                return null;
            }
        }

        private List<CoownerGuarantorModel> CreateQueryCoownerGuarantor(int selectedFinancialAccountID)
        {
            var partyRoleQuery = from far in ObjectContext.FinancialAccountRoles
                                 join pr in ObjectContext.PartyRoles.Where(entity => entity.RoleTypeId == RoleType.CoOwnerFinancialType.Id
                                                                             || entity.RoleTypeId == RoleType.GuarantorFinancialType.Id && entity.EndDate == null)
                                     on far.PartyRoleId equals pr.Id
                                 where far.FinancialAccountId == selectedFinancialAccountID && pr.EndDate == null
                                 select pr;

            List<CoownerGuarantorModel> coOwnerGuarantorList = new List<CoownerGuarantorModel>();

            foreach (var item in partyRoleQuery)
            {
                coOwnerGuarantorList.Add(new CoownerGuarantorModel(item));
            }

            //query = query.OrderBy(entity => entity.FinancialAccountRole);
            return coOwnerGuarantorList;
        }

        private IQueryable<PaymentHistoryModel> CreateQueryPaymentHistory(int selectedFinancialAccountID)
        {
            var query = from pay in ObjectContext.PaymentHistoryViewLists
                        where pay.FinancialAccountId == selectedFinancialAccountID
                        select new PaymentHistoryModel
                        {
                            Date = pay.Date,
                            PaymentID = pay.PaymentID,
                            Amount = pay.Amount,
                            CollectedBy = pay.CollectedBy
                        };
            query = query.OrderBy(entity => entity.Date);
            return query;
        }

        public class AmortizationScheduleModel
        {
            public int Id { get; set; }
            public DateTime ScheduledPaymentDate { get; set; }
            public decimal PrincipalPayment { get; set; }
            public decimal InterestPayment { get; set; }
            public decimal TotalPayment
            {
                get
                {
                    return (this.PrincipalPayment + this.InterestPayment);
                }

            }
            public decimal PrincipalBalance { get; set; }
            public decimal TotalLoanBalance { get; set; }

        }

        public class CoownerGuarantorModel
        {
            public string FinancialAccountRole { get; set; }
            public string Name { get; set; }
            public string CellphoneNumber { get; set; }
            public string TelephoneNumber { get; set; }
            public string PrimaryHomeAddress { get; set; }

            public CoownerGuarantorModel(PartyRole partyRole)
            {
                this.FinancialAccountRole = partyRole.PartyRoleType.RoleType.Name;

                this.Name = Person.GetPersonFullName(partyRole.Party);

                var cellNumber = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(partyRole.Party, TelecommunicationsNumberType.PersonalMobileNumberType, true);

                if (cellNumber != null)
                    this.CellphoneNumber = cellNumber.AreaCode + "-" + cellNumber.PhoneNumber;

                var telephoneNumber = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(partyRole.Party, TelecommunicationsNumberType.HomePhoneNumberType, true);
                if (telephoneNumber != null)
                    this.TelephoneNumber = telephoneNumber.AreaCode + "-" + telephoneNumber.PhoneNumber;

                Address postalAddress = partyRole.Party.Addresses.FirstOrDefault(entity => entity.EndDate == null && entity.AddressTypeId == AddressType.PostalAddressType.Id
                                    && entity.PostalAddress.PostalAddressTypeId == PostalAddressType.HomeAddressType.Id && entity.PostalAddress.IsPrimary);

                if (postalAddress != null && postalAddress.PostalAddress != null)
                {
                    PostalAddress postalAddressSpecific = postalAddress.PostalAddress;
                    this.PrimaryHomeAddress = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress,
                                  postalAddressSpecific.Barangay,
                                  postalAddressSpecific.Municipality,
                                  postalAddressSpecific.City,
                                  postalAddressSpecific.Province,
                                  postalAddressSpecific.State,
                                  postalAddressSpecific.Country.Name,
                                  postalAddressSpecific.PostalCode);
                }
            }
        }

        public class PaymentHistoryModel
        {
            public DateTime Date { get; set; }
            public int? PaymentID { get; set; }
            public decimal Amount { get; set; }
            public string CollectedBy { get; set; }

            public PaymentHistoryModel()
            {

            }
        }
    }
}