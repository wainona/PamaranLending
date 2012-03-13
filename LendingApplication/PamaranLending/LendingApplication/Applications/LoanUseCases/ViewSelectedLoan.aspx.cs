using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.LoanUseCases
{
    public partial class ViewSelectedLoan : ActivityPageBase
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        protected void RefreshDataAmortSched(object sender, StoreRefreshDataEventArgs e)
        {
            int selectedLoanId = int.Parse(hdnSelectedLoanId.Text);
            var amortizationscheduleList = CreateQueryAmortizationSchedule(selectedLoanId);
            strAmortSched.DataSource = amortizationscheduleList.ToList();
            strAmortSched.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int selectedLoanId = int.Parse(Request.QueryString["id"]);
                hdnSelectedLoanId.Text = selectedLoanId.ToString();
                var loanStatus = ObjectContext.LoanAccountStatus.SingleOrDefault(entity=>entity.FinancialAccountId == selectedLoanId && entity.IsActive == true);
                var loanStatusType = loanStatus.LoanAccountStatusType.Name;

                dtScheduledPaymentDate.DisabledDays = ApplicationSettings.DisabledDays;

                switch (loanStatusType)
                {
                    case "Current":
                        btnUnderLitigation.Hidden = true;
                        btnUnderLitigationSeparator.Hidden = true;
                        btnWriteOff.Hidden = true;
                        btnWriteOffSeparator.Hidden = true;
                        btnPrintCertificate.Hidden = true;
                        btnPrintCertificateSeparator.Hidden = true;
                        break;

                    case "Delinquent":
                        btnPrintCertificate.Hidden = true;
                        btnPrintCertificateSeparator.Hidden = true;
                        break;

                    case "Under Litigation":
                        btnUnderLitigation.Hidden = true;
                        btnUnderLitigationSeparator.Hidden = true;
                        btnPrintCertificate.Hidden = true;
                        btnPrintCertificateSeparator.Hidden = true;
                        break;

                    case "Paid-Off":
                        btnUnderLitigation.Hidden = true;
                        btnUnderLitigationSeparator.Hidden = true;
                        btnWriteOff.Hidden = true;
                        btnWriteOffSeparator.Hidden = true;
                        amortSchedToolBar.Disabled = true;
                        break;

                    case "Written-Off":
                        btnUnderLitigation.Hidden = true;
                        btnUnderLitigationSeparator.Hidden = true;
                        btnWriteOff.Hidden = true;
                        btnWriteOffSeparator.Hidden = true;
                        btnPrintCertificate.Hidden = true;
                        btnPrintCertificateSeparator.Hidden = true;
                        amortSchedToolBar.Disabled = true;
                        break;

                    case "Restructured":
                        btnUnderLitigation.Hidden = true;
                        btnUnderLitigationSeparator.Hidden = true;
                        btnWriteOff.Hidden = true;
                        btnWriteOffSeparator.Hidden = true;
                        btnPrintCertificate.Hidden = true;
                        btnPrintCertificateSeparator.Hidden = true;
                        break;

                    default:
                        break;
                }

                RetrieveOwnerInformation(selectedLoanId);
                var coownerGuarantorList = CreateQueryCoownerGuarantor(selectedLoanId);
                RetrieveLoanAccountDetails(selectedLoanId);
                
                //loan agreement
                var amortizationscheduleList = CreateQueryAmortizationSchedule(selectedLoanId);
                var loanhistoryList = LoanAccount.CreateQueryLoanHistory(selectedLoanId);

                strCoOwnerGuarantor.DataSource = coownerGuarantorList.ToList();
                strAmortSched.DataSource = amortizationscheduleList.ToList();
                //grdAmortScheduleLoanAgreement.DataSource = amortizationscheduleList.ToList();
                strPaymentHistory.DataSource = loanhistoryList.ToList();

                strCoOwnerGuarantor.DataBind();
                strAmortSched.DataBind();
                //grdAmortScheduleLoanAgreement.DataBind();
                strPaymentHistory.DataBind();

            }
        }

        protected void RetrieveOwnerInformation(int selectedFinancialAccountID)
        {
            //Account Owners Basic Information
            var financialAccountRole = ObjectContext.FinancialAccountRoles.SingleOrDefault(entity => entity.FinancialAccountId == selectedFinancialAccountID && entity.PartyRole.RoleTypeId == RoleType.OwnerFinancialType.Id);
            if (financialAccountRole == null) return;
            int financialAccountPartyRoleID = financialAccountRole.PartyRoleId;

            var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.OwnerFinancialType.Id && entity.Id == financialAccountPartyRoleID);
            int partyRolePartyID = partyRole.PartyId;

            var party = partyRole.Party;
            var person = party.Person;

            //var driversLicensePersonIdentification = PersonIdentification.GetPersonIdentification(party, IdentificationType.DriversLicenseType);
            //var sssPersonIdentification = PersonIdentification.GetPersonIdentification(party, IdentificationType.SSSIdType);

            //if (driversLicensePersonIdentification != null)
            //    lblBorrowerDriversLicenseNum.Text = driversLicensePersonIdentification.IdNumber;

            //if (sssPersonIdentification != null)
            //    lblBorrowerSSSNum.Text = sssPersonIdentification.IdNumber;

            //FILL TXT FIELDS
            if (!string.IsNullOrWhiteSpace(person.ImageFilename)) imgPersonPicture.ImageUrl = person.ImageFilename;
            //fill name
            var name = party.Name;
            txtAccountOwner.Text = name;//Owner of loan
            //lblpBorrowerName.Text = name;

            //fill station number and district
            var customer = ObjectContext.PartyRoles.FirstOrDefault(entity => entity.PartyId == party.Id && entity.PartyRoleType.RoleTypeId == RoleType.CustomerType.Id && entity.EndDate == null);
            var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == customer.Id && entity.EndDate == null);
            var classificationType = customerClassification.ClassificationType;

            txtDistrict.Text = classificationType.District;
            txtStationNumber.Text = classificationType.StationNumber;

            //fill address
            //txtAddress.Text = loanview.Address;
     
            //FOR POSTAL ADDRESS
            var homePostalAddressType = PostalAddressType.HomeAddressType.Name;
            var businessPostalAddressType = PostalAddressType.BusinessAddressType.Name;

            PostalAddress postalAddressSpecific = null;
            TelecommunicationsNumber primNumberSpecific = null;
            TelecommunicationsNumber cellTelecomNumberSpecific = null;
            ElectronicAddress primaryEmailAddressSpecific = null;

            ////IF OWNER IS ORGANIZATION
            //if (party.PartyType == PartyType.OrganizationType)
            //{
            //    //Postal Address
            //    postalAddressSpecific = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.BusinessAddressType, entity => entity.PostalAddress.IsPrimary == true);
            //    //Business Mobile Number
            //    cellTelecomNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessMobileNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
            //    //Business Telephone Number
            //    primNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.BusinessPhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
            //    //Business Email Address
            //    primaryEmailAddressSpecific = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.BusinessEmailAddressType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
            //}
            ////ELSE IF OWNER IS PERSON
            //else
            //{
                postalAddressSpecific = PostalAddress.GetCurrentPostalAddress(party, PostalAddressType.HomeAddressType, entity => entity.PostalAddress.IsPrimary == true);
                cellTelecomNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.PersonalMobileNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                primNumberSpecific = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(party, TelecommunicationsNumberType.HomePhoneNumberType, entity => entity.TelecommunicationsNumber.IsPrimary == true);
                primaryEmailAddressSpecific = ElectronicAddress.GetCurrentElectronicAddress(party, ElectronicAddressType.PersonalEmailAddressType, entity => entity.ElectronicAddress.IsPrimary == true);
            //}

            /******************************************************************************/
            //fill contact numbers

            if (postalAddressSpecific != null)
            {
                txtAddress.Text = StringConcatUtility.Build(", ", postalAddressSpecific.StreetAddress, postalAddressSpecific.Barangay, postalAddressSpecific.City,
                                                            postalAddressSpecific.Municipality, postalAddressSpecific.Province, postalAddressSpecific.Country.Name,
                                                            postalAddressSpecific.PostalCode);

                txtCellNumCountryCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
                txtPrimTelCountyCode.Text = postalAddressSpecific.Country.CountryTelephoneCode;
            }

            if (cellTelecomNumberSpecific != null)
            {
                if (cellTelecomNumberSpecific.AreaCode != null) txtCellNumAreaCode.Text = cellTelecomNumberSpecific.AreaCode;
                if (cellTelecomNumberSpecific.PhoneNumber != null) txtCellNumPhoneNumber.Text = cellTelecomNumberSpecific.PhoneNumber;
            }

            if (primNumberSpecific != null)
            {
                if (primNumberSpecific.AreaCode != null) txtPrimTelAreaCode.Text = primNumberSpecific.AreaCode;
                if (primNumberSpecific.PhoneNumber != null) txtPrimTelPhoneNumber.Text = primNumberSpecific.PhoneNumber;
               // lblBorrowerAreaCodeTelNum.Text = primNumberSpecific.AreaCode + " " + primNumberSpecific.PhoneNumber;
            }

            if (primaryEmailAddressSpecific != null)
            {
                //fill email address
                if (primaryEmailAddressSpecific.ElectronicAddressString != null) txtEmailAddress.Text = primaryEmailAddressSpecific.ElectronicAddressString;
                
            }

            /******************************************************************************/
            
            //Loan Agreement
            //lblBorrowerDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            //lblBorrowerName.Text = name;
            //lblBorrowerDateOfBirth.Text = person.Birthdate.Value.ToString("MMMM dd, yyyy");
            //if (postalAddressSpecific != null)
            //{
            //    if (postalAddressSpecific.Province != null || postalAddressSpecific.Province != string.Empty)
            //        lblBorrowerStreetAddress.Text = postalAddressSpecific.StreetAddress;

            //    if (string.IsNullOrWhiteSpace(postalAddressSpecific.City)){
            //        lblBorrowerCity.Text = postalAddressSpecific.Municipality;
            //    } 
            //    else if(string.IsNullOrWhiteSpace(postalAddressSpecific.Municipality))
            //    {
            //        lblBorrowerCity.Text = postalAddressSpecific.City;
            //    }

            //    if (postalAddressSpecific.Province != null || postalAddressSpecific.Province != string.Empty)
            //        lblBorrowerProvince.Text = postalAddressSpecific.Province;
            //    lblBorrowerZipCode.Text = postalAddressSpecific.PostalCode;
            //}
            
        }

        //LOAN DETAILS
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

            if(rec.Count() != 0) {
                foreach (var item in rec)
	            {
                    addOnInterestSum = addOnInterestSum + item.sum;
	            }
            }

            //<Payment Mode> Amortization Field
            if (agreementItem.PaymentMode == "Annually") txtAmortization.FieldLabel = "Annual Principal Amortization";
            else txtAmortization.FieldLabel = agreementItem.PaymentMode  + " Amortization";

            txtLoanID.Text = selectedFinancialAccoutnID.ToString();
            txtAgreementID.Text = agreementID.ToString();
            txtLoanReleaseDate.Text = loanAccount.LoanReleaseDate.Value.ToString("MMMM dd, yyyy");
            if(loanAccount.MaturityDate.HasValue)
                txtMaturityDate.Text = loanAccount.MaturityDate.Value.ToString("MMMM dd, yyyy");

            if (agreementItem.LoanTermLength != 0)
            {
                txtLoanTerm.Text = agreementItem.LoanTermLength + " " + agreementItem.LoanTermUom;
            }

            txtLoanStatus.Text = statusType.Name + " on " + loanAccountStatus.TransitionDateTime.ToString("MMMM dd, yyyy");
            txtStatusComment.Text = loanAccountStatus.Remarks;
            txtLoanApplicationID.Text = agreement.ApplicationId.ToString();
            txtLoanProductName.Text = financialProduct.Name;
            txtPaymentMode.Text =  agreementItem.PaymentMode;
            txtInterestComputationMode.Text =  agreementItem.InterestComputationMode;
            if (loanAccount.InterestTypeId == InterestType.PercentageInterestTYpe.Id || loanAccount.InterestTypeId == null)
            {
                txtInterest.Text = agreementItem.InterestRate + "% " + agreementItem.InterestRateDescription;
            }
            else if (loanAccount.InterestTypeId == InterestType.FixedInterestTYpe.Id)
            {
                var interestItem = loanAccount.InterestItems.FirstOrDefault(entity => entity.IsActive == true);
                txtInterest.Text = "Php " + interestItem.Amount.ToString();
            }
            else if (loanAccount.InterestTypeId == InterestType.ZeroInterestTYpe.Id)
            {
                var interestItem = loanAccount.InterestItems.FirstOrDefault(entity => entity.IsActive == true);
                txtInterest.Text = interestItem.Amount.ToString();
            }
            if(!string.IsNullOrWhiteSpace(agreementItem.PastDueInterestRate.ToString()))
            txtPastDueInterest.Text =  agreementItem.PastDueInterestRate.ToString() + "% " + agreementItem.PastDueInterestRateDescript;
            txtLoanAmount.Text = loanAccount.LoanAmount.ToString("N");
            txtLoanBalance.Text = loanAccount.LoanBalance.ToString("N");
            txtAddOnInterest.Text = addOnInterestSum.ToString("N");

            txtMethodOfChargingInterest.Text = agreementItem.MethodOfChargingInterest;
            txtTotalLoan.Text = (loanAccount.LoanAmount + addOnInterestSum).ToString("N");

            string monthlyPayment = "";
            if (agreementItem.InterestComputationMode == "Straight Line Method")
            {
                monthlyPayment = monthlyPrincipalPayment.ToString("N");
                txtAmortization.Text = monthlyPrincipalPayment.ToString("N");
            }
            else
            {
                monthlyPayment = "_____________";
            }

            string paymentMode = agreementItem.PaymentMode;
            //Loan Agreement
            //if (agreementItem.LoanTermLength != 0)
            //{
            //    if(paymentMode == "Annually") paymentMode = "Annual";
            //    lblAmortizationSchedLoanAgreement.Text = "Amortization Schedule";
            //    lblInstallments.Text = "Borrower will pay " + agreementItem.LoanTermLength.ToString() + " payments of " + monthlyPayment + " (Php) each at " + paymentMode + " intervals.";
            //}
            //lblLoanAmount.Text = loanAccount.LoanAmount.ToString("N");
            //lblInterestRate.Text = agreement.Application.LoanApplication.InterestRate.ToString() + " %";
            //lblPastDueInterestRate.Text = agreement.Application.LoanApplication.PastDueInterestRate.ToString();
            

            //lblpLoanAmount.Text = loanAccount.LoanAmount.ToString("N");


            //if (firstPaymentDate.HasValue && lastPaymentDate.HasValue)
            //{
            //    lblLoanPeriod.Text = firstPaymentDate.Value.ToString("MMMM dd, yyyy") + " - " + lastPaymentDate.Value.ToString("MMMM dd, yyyy");
            //}

            //lblpInterestValPlusProdFeatName.Text = agreementItem.InterestRate + "% " + agreementItem.InterestRateDescription;

        }

        //AMORTIZATION SCHEDULE BUTTONS
        protected void btnPrintCertificate_DirectClick(object sender, DirectEventArgs e)
        {
            
        }

        protected void btnAddAmortizationSchedItem_Click(object sender, DirectEventArgs e)
        {
            hdnSelectedAmortSchedItemId.Text = "";
            wndAmortizationSchedItem.Show();
        }

        [DirectMethod]
        public void btnOpen(int amortItemId) 
        {
            hdnSelectedAmortSchedItemId.Text = amortItemId.ToString();
            FillAmortWindow(amortItemId);
            wndAmortizationSchedItem.Show();
        }

        protected void btnDeleteAmortizationSchedItem_Click(object sender, DirectEventArgs e)
        {
            RowSelectionModel rowSelMod = this.rsmAmortSchedGridPanel;
            string selectedAmortItemId = rowSelMod.SelectedRow.RecordID;
            int selectedAmortSchedItemId = int.Parse(selectedAmortItemId);

            var amortSchedItem = ObjectContext.AmortizationScheduleItems.SingleOrDefault(entity => entity.Id == selectedAmortSchedItemId);
            ObjectContext.AmortizationScheduleItems.DeleteObject(amortSchedItem);
            ObjectContext.SaveChanges();
            strAmortSched.DataBind();
        }

        protected void btnSaveAmortizationSchedItem_Click(object sender, DirectEventArgs e) 
        {
            int selectedLoanId = int.Parse(hdnSelectedLoanId.Text);
            string selectedAmortSchedItemId = hdnSelectedAmortSchedItemId.Text;//CHANGE

            if (selectedAmortSchedItemId != "")
            {
                int asId = int.Parse(selectedAmortSchedItemId);
                //EDIT TOMORROW
                var amortSchedItem = ObjectContext.AmortizationScheduleItems.SingleOrDefault(entity => entity.Id == asId);
                amortSchedItem.ScheduledPaymentDate = dtScheduledPaymentDate.SelectedDate;
                amortSchedItem.PrincipalPayment = decimal.Parse(txtPrincipalPayment.Text);
                amortSchedItem.InterestPayment = decimal.Parse(txtInterestPayment.Text);
                amortSchedItem.PrincipalBalance = decimal.Parse(txtPrincipalBalance.Text);
                amortSchedItem.TotalLoanBalance = decimal.Parse(txtTotalLoanBalance.Text);

                ObjectContext.SaveChanges();
                strAmortSched.DataBind();
                wndAmortizationSchedItem.Hide();
                ClearAmortSchedItemWindow();
                return;
            }

            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity => entity.Id == selectedLoanId);
            var agreement = ObjectContext.Agreements.SingleOrDefault(entity => entity.Id == financialAccount.AgreementId);
            var amortizationSchedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agreement.Id);

            AmortizationScheduleItem newAmortSchedItem = new AmortizationScheduleItem();
            newAmortSchedItem.AmortizationSchedule = amortizationSchedule;
            newAmortSchedItem.ScheduledPaymentDate = dtScheduledPaymentDate.SelectedDate;
            newAmortSchedItem.PrincipalPayment = decimal.Parse(txtPrincipalPayment.Text);
            newAmortSchedItem.InterestPayment = decimal.Parse(txtInterestPayment.Text);
             newAmortSchedItem.PrincipalBalance = decimal.Parse(txtPrincipalBalance.Text);
            newAmortSchedItem.TotalLoanBalance = decimal.Parse(txtTotalLoanBalance.Text);

            ObjectContext.AmortizationScheduleItems.AddObject(newAmortSchedItem);
            ObjectContext.SaveChanges();
            wndAmortizationSchedItem.Hide();
            ClearAmortSchedItemWindow();
            strAmortSched.DataBind();
        }

        protected void btnCancelAmortizationSchedItem_Click(object sender, DirectEventArgs e)
        {
            wndAmortizationSchedItem.Hide();
            ClearAmortSchedItemWindow();
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

        private IQueryable<AmortizationScheduleModel> CreateQueryAmortizationSchedule(int selectedFinancialAccountID)
        {
            var financialAccount = ObjectContext.FinancialAccounts.SingleOrDefault(entity =>entity.Id == selectedFinancialAccountID);
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

        protected void ClearAmortSchedItemWindow()
        {
            dtScheduledPaymentDate.Clear();
            txtPrincipalPayment.Clear();
            txtInterestPayment.Clear();
            txtPrincipalBalance.Clear();
            txtTotalLoanBalance.Clear();
        }

        protected void FillAmortWindow(int selectedAmortItemId)
        {
            var amortSchedItem = ObjectContext.AmortizationScheduleItems.SingleOrDefault(entity => entity.Id == selectedAmortItemId);

            dtScheduledPaymentDate.SelectedDate = amortSchedItem.ScheduledPaymentDate;
            txtPrincipalPayment.Text = amortSchedItem.PrincipalPayment.ToString();
            txtInterestPayment.Text = amortSchedItem.InterestPayment.ToString();
            txtPrincipalBalance.Text = amortSchedItem.PrincipalBalance.ToString();
            txtTotalLoanBalance.Text = amortSchedItem.TotalLoanBalance.ToString();
        }

        [DirectMethod]
        public bool checkIfBilled(int amortSchedItemId)
        {
            var amortSchedItem = ObjectContext.AmortizationScheduleItems.SingleOrDefault(entity => entity.Id == amortSchedItemId);
            return amortSchedItem.IsBilledIndicator;
        }
    }

    public class AmortizationScheduleModel
    {
        private const string Yes = "Yes";
        private const string No = "No";
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
        public string BilledIndicator { 
            get { 
                if(this.IsBilledIndicator == true)
                    return Yes;
                else
                    return No;
            }
        }
        public bool IsBilledIndicator { get; set;}
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

            var cellNumber = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(partyRole.Party, TelecommunicationsNumberType.PersonalMobileNumberType, true);
            if (cellNumber != null)
                this.CellphoneNumber = postalAddress.PostalAddress.Country.CountryTelephoneCode + " " + cellNumber.AreaCode + cellNumber.PhoneNumber;

            var telephoneNumber = TelecommunicationsNumber.GetCurrentTelecommNumberAddress(partyRole.Party, TelecommunicationsNumberType.HomePhoneNumberType, true);
            if (telephoneNumber != null)
                this.TelephoneNumber = postalAddress.PostalAddress.Country.CountryTelephoneCode +" ("+ telephoneNumber.AreaCode +") "+ telephoneNumber.PhoneNumber;

        }
    }

}