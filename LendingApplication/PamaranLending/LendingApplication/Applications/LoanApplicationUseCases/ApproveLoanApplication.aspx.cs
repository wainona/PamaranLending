using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using LendingApplication;
using FirstPacific.UIFramework;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using System.IO;
using System.Threading;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class ApproveLoanApplication : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Accountant");
                return allowed;
            }
        }

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

        public int StraightLineCount
        {
            get
            {
                return int.Parse(hdnStraightLineCount.Value.ToString());
            }
        }

        public int DiminishingCount
        {
            get
            {
                return int.Parse(hdnDiminishingCount.Value.ToString());
            }
        }

        public void SetAllInterestComputationModes(LoanApprovalForm form)
        {
            PartyRole customerPartyRole = PartyRole.GetById(form.CustomerPartyRoleId);
            RoleType borrowerRoleType = RoleType.BorrowerApplicationType;
            var applications = LoanApplication.GetAllLoanApplicationOf(customerPartyRole.Party, borrowerRoleType);
            var interestComputationModes = from application in applications
                                           join agreement in ObjectContext.Agreements on application.ApplicationId equals agreement.ApplicationId
                                           join agreementItem in ObjectContext.AgreementItems on agreement.Id equals agreementItem.AgreementId
                                           where agreement.EndDate == null
                                           group agreementItem by agreementItem.InterestComputationMode into icm
                                           select new { InterestComputationMode = icm.Key, Count = icm.Count() };

            var diminishingCount = from icm in interestComputationModes
                                   where icm.InterestComputationMode == ProductFeature.DiminishingBalanceMethodType.Name
                                   select icm.Count;

            var straightLineCount = from icm in interestComputationModes
                                    where icm.InterestComputationMode == ProductFeature.StraightLineMethodType.Name
                                    select icm.Count;

            hdnStraightLineCount.Value = straightLineCount.FirstOrDefault();
            hdnDiminishingCount.Value = diminishingCount.FirstOrDefault();
        }

        [DirectMethod(ShowMask = true, Msg = "Checking outstanding loans of customer...")] //[a]
        public bool CheckLoanAmount()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            if (loanAmount <= form.TotalOfSelectedOutstandingLoans)
                return false;
            return true;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking loan term...")]//[b]
        public bool CheckLoanTerm()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            int loanTerm = Convert.ToInt32(nfLoanTerm.Text);
            int paymentMode = form.PaymentModeUomId;

            var financialProduct = FinancialProduct.GetById(form.FinancialProductId);
            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, financialProduct);
            int termUnit = UnitOfMeasure.All(UnitOfMeasureType.TimeUnitType).SingleOrDefault(entity =>
                entity.Name == minimumTerm.LoanTerm.UnitOfMeasure.Name).Id;

            var value = TimeUnitConversion.Convert(termUnit, paymentMode, loanTerm);
            decimal abs = (int)value;
            return abs == value;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking credit...")]//[c]
        public bool CheckCredit()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();
            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;
            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);

            decimal? creditLimit = form.CustomerCreditLimit;
            if (creditLimit == null)
                creditLimit = decimal.MaxValue;
            else
                creditLimit = Convert.ToDecimal(form.CustomerCreditLimit);

            if (creditLimit != null && ((this.StraightLineCount < straightLineLimit)
                || (this.DiminishingCount < diminishingLineLimit)) && loanAmount > form.CustomerCreditLimit)
                return true; //confirmation message
            else
                return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking diminishing loans of customer...")]//[d]
        public bool CheckDiminishing()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();
            var interestComputationMode = form.InterestComputationMode;
            var loanAmount = Convert.ToDecimal(nfLoanAmount.Text);

            decimal? creditLimit = form.CustomerCreditLimit;
            if (creditLimit == null)
                creditLimit = decimal.MaxValue;
            else
                creditLimit = Convert.ToDecimal(form.CustomerCreditLimit);

            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;
            if (interestComputationMode == "Diminishing Balance Method" && DiminishingCount == diminishingLineLimit)
            {
                if (creditLimit != 0 && loanAmount <= creditLimit)
                    return true;
            }
            return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking straight line loans of customer...")]//[e]
        public bool CheckStraightLine()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();
            var loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            var interestComputationMode = form.InterestComputationMode;

            decimal? creditLimit = form.CustomerCreditLimit;
            if (creditLimit == null)
                creditLimit = decimal.MaxValue;
            else
                creditLimit = Convert.ToDecimal(form.CustomerCreditLimit);

            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            if (interestComputationMode == "Straight Line Method" && (StraightLineCount == straightLineLimit))
            {
                if (creditLimit != 0 && loanAmount <= creditLimit)
                    return true;
            }
            return false;
        }

        [DirectMethod(ShowMask = true, Msg = "Checking credit limit...")]//[f]
        public bool CheckCreditLimit()
        {
            var form = this.CreateOrRetrieve<LoanApprovalForm>();

            int straightLineLimit = SystemSetting.AllowableStraightLineLoans;
            int diminishingLineLimit = SystemSetting.AllowableDiminishingLoans;

            decimal loanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            decimal? creditLimit = ObjectContext.Customers.SingleOrDefault(entity =>
                entity.PartyRoleId == form.CustomerPartyRoleId).CreditLimit;

            //if (loanAmount > creditLimit && (StraightLineCount >= straightLineLimit)
            //    || (DiminishingCount >= diminishingLineLimit))
            //    return true;
            //return false;

            bool result = true;
            if (string.IsNullOrWhiteSpace(creditLimit.ToString()) == false)
            {
                result = loanAmount > creditLimit;
            }
            else
                result = false;
            var interestComputationMode = form.InterestComputationMode;
            if (interestComputationMode == "Straight Line Method" && (StraightLineCount == straightLineLimit))
                result &= StraightLineCount >= straightLineLimit;
            else
                result &= DiminishingCount >= diminishingLineLimit;

            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                this.ResourceGuid = Request.QueryString["ResourceGuid"];
                if (string.IsNullOrWhiteSpace(this.ResourceGuid))
                {
                    this.ResourceGuid = null;
                    this.CreateOrRetrieve<LoanApprovalForm>();
                }
                this.hiddenResourceGuid.Value = this.ResourceGuid;
                
                int id = int.Parse(Request.QueryString["id"]);
                hdnSelectedLoanID.Value = id;
                hdnStraightLineCount.Value = -1;
                hdnDiminishingCount.Value = -1;
                
                
                datLoanReleaseDate.DisabledDays = ApplicationSettings.DisabledDays;

                var loanApplication = LoanApplication.GetById(id);
                var chequeAssoc = ObjectContext.ChequeApplicationAssocs.Where(entity => entity.ApplicationId == loanApplication.ApplicationId);
                if (chequeAssoc.Count() > 0)
                {
                    datLoanReleaseDate.ReadOnly = true;
                    datPaymentStartDate.ReadOnly = true;
                }
                else
                {
                    datLoanReleaseDate.MinDate = DateTime.Now;
                }

                Retrieve(loanApplication.ApplicationId);

                datLoanReleaseDate.DisabledDays = ApplicationSettings.DisabledDays;
                datPaymentStartDate.DisabledDays = ApplicationSettings.DisabledDays;
            }
        }

        public void Retrieve(int id)
        {            
            var loanApplication = LoanApplication.GetById(id);

            LoanApprovalForm form = this.CreateOrRetrieve<LoanApprovalForm>();
            form.Retrieve(loanApplication.ApplicationId);

            hdnCustomerID.Value = form.CustomerPartyRoleId;
            var loanTerm = UnitOfMeasure.GetByID(form.LoanTermUomId);
            this.nfLoanTerm.FieldLabel = string.Format("Loan Term ({0})", loanTerm.Name);
            var dateManager = new DateTimeOperationManager(form.PaymentModeName, datLoanReleaseDate.SelectedDate);
            datPaymentStartDate.SelectedDate = dateManager.Increment();

            var product = FinancialProduct.GetById(form.FinancialProductId);

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, product);
            var minimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, product);
            var maximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            nfLoanAmount.Text = form.LoanAmount.ToString("N");
            nfLoanTerm.Text = loanApplication.LoanTermLength.ToString();

            SetAllInterestComputationModes(form);

            var agr = ObjectContext.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
            var sched = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agr.Id);
            datLoanReleaseDate.SelectedDate = sched.LoanReleaseDate;
            datPaymentStartDate.SelectedDate = sched.PaymentStartDate;

            form.LoanAmount = decimal.Parse(nfLoanAmount.Text);
            form.LoanTerm = Convert.ToInt32(nfLoanTerm.Text);
            form.PaymentStartDate = sched.PaymentStartDate;
            form.LoanReleaseDate = sched.LoanReleaseDate;
            form.ClearAndGenerateSchedule();

            storeAmortizationSchedule.DataSource = form.AmortizationSchedules;
            storeAmortizationSchedule.DataBind();

            PartyRole borrower = PartyRole.GetById(form.CustomerPartyRoleId);
            var formDetail = FormDetail.GetByLoanAppIdAndType(int.Parse(hdnSelectedLoanID.Value.ToString()), FormType.SPAType);
            EnableDisableControls(borrower.Party);

        }

        public void EnableDisableControls(Party customerParty)
        {
            var borrowerHasLoans = LoanApprovalForm.BorrowerLoans(customerParty);
            if (borrowerHasLoans.Count() > 0)
            {
                pnlSignSPA.Hide();
                txtSignSPA.AllowBlank = true;
                txtSignSPA.Text = "Signed";
            }
        }

        [DirectMethod]
        public bool CheckHonorableLoanAmount()
        {
            if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
            {
                var loanAmount = Decimal.Parse(nfLoanAmount.Text);
                if (loanAmount > SystemSetting.ClerksMaximumHonorableAmount)
                    return true;
                else
                    return false;
            }
            return false;
        }

        protected void btnApprove_Click(object sender, DirectEventArgs e)
        {
            string schedules = e.ExtraParams["Schedules"];

            LoanApprovalForm form = this.CreateOrRetrieve<LoanApprovalForm>();
            form.LoanAmount = Convert.ToDecimal(nfLoanAmount.Text);
            form.LoanTerm = Convert.ToInt32(nfLoanTerm.Text);
            form.PaymentStartDate = datPaymentStartDate.SelectedDate;
            form.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            form.ProcessedByPartyId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRole.PartyId;
            form.HasSignatures = false;

            if (txtSignSPA.Text == "Signed")
            {
                form.HasSignatures = true;
                form.SPADocumentDetails.Lender.FilePath = hdnLender.Text;
                form.SPADocumentDetails.Lender.PersonName = hdnLenderName.Text;

                form.SPADocumentDetails.Borrower.FilePath = hdnBorrower.Text;

                form.SPADocumentDetails.Witness1.FilePath = hdnWitness1.Text;
                form.SPADocumentDetails.Witness1.PersonName = hdnWitness1Name.Text;

                form.SPADocumentDetails.Witness2.FilePath = hdnWitness2.Text;
                form.SPADocumentDetails.Witness2.PersonName = hdnWitness2Name.Text;

                form.SPADocumentDetails.Witness3.FilePath = hdnWitness3.Text;
                form.SPADocumentDetails.Witness3.PersonName = hdnWitness3Name.Text;

                form.SPADocumentDetails.Witness4.FilePath = hdnWitness4.Text;
                form.SPADocumentDetails.Witness4.PersonName = hdnWitness4Name.Text;
            }

            SubmitHandler schedulerHandler = new SubmitHandler(schedules);
            foreach (AmortizationScheduleModel model in schedulerHandler.Object<AmortizationScheduleModel>())
            {
                AmortizationScheduleModel toUpdate = form.RetrieveSchedule(model.Counter);
                toUpdate.ScheduledPaymentDate = model.ScheduledPaymentDate;
            }

            using (UnitOfWorkScope scope = new UnitOfWorkScope(true))
            {
                form.PrepareForSave();
            }
        }

        [DirectMethod]
        public bool setPaymentDate()
        {
            int id = Convert.ToInt32(hdnSelectedLoanID.Value);
            var loanApplication = LoanApplication.GetById(id);
            var paymentMode = UnitOfMeasure.GetByID(loanApplication.PaymentModeUomId);
            var dateManager = new DateTimeOperationManager(paymentMode.Name, datLoanReleaseDate.SelectedDate.AddMonths(-1));
            datPaymentStartDate.SelectedDate = dateManager.Increment();

            return true;
        }

        protected void btnGenerate_Click(object sender, DirectEventArgs e)
        {
            LoanApprovalForm form = this.CreateOrRetrieve<LoanApprovalForm>();
            form.LoanAmount = decimal.Parse(nfLoanAmount.Text);
            form.LoanTerm = Convert.ToInt32(nfLoanTerm.Text);
            form.PaymentStartDate = datPaymentStartDate.SelectedDate;
            form.LoanReleaseDate = datLoanReleaseDate.SelectedDate;
            form.ClearAndGenerateSchedule();

            storeAmortizationSchedule.DataSource = form.AmortizationSchedules;
            storeAmortizationSchedule.DataBind();
        }

        [DirectMethod]
        public bool SetControlNumber()
        {
            LoanApplication loanApplication = LoanApplication.GetById(int.Parse(hdnSelectedLoanID.Text));
            ControlNumberFacade.Create(loanApplication);
            ObjectContext.SaveChanges();
            return true;
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        [DirectMethod(ShowMask = true, Msg = "Deleting all input and uploaded signatures..")]
        public void DeleteImages()
        {
            var formType = FormType.SPAType;
            string uploadDirectory = Path.Combine(Request.PhysicalApplicationPath, "Uploaded\\Agreement");
            uploadDirectory = Path.Combine(uploadDirectory, hdnSelectedLoanID.Value.ToString());
            uploadDirectory = Path.Combine(uploadDirectory, formType.Name);
            if (Directory.Exists(uploadDirectory))
            {
                string[] directories = Directory.GetDirectories(uploadDirectory);
                foreach (var d in directories)
                {
                    Directory.Delete(d, true);
                }

                Directory.Delete(uploadDirectory);
            }
        }
    }
}