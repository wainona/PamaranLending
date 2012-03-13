using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;
using Ext.Net;

namespace LendingApplication.Applications.LoanApplicationUseCases
{
    public partial class PrintAmortizationSchedule : ActivityPageBase
    {
        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
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
                int customerId = int.Parse(Request.QueryString["customerPartyRoleId"]);
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                LoanApplicationForm form = this.Retrieve<LoanApplicationForm>(ParentResourceGuid);
                CustomerDetailsModel model = new CustomerDetailsModel(form.PartyRoleId);
                this.lblCustomerName.Text = model.Name;
                this.lblDateGenerated.Text = String.Format("{0:MMMM dd, yyyy}", DateTime.Today);

                var loanApplication = LoanApplication.GetById(form.LoanApplicationId);
                //HEADER LENDER INFORMATION
                var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
                var party = partyRole.Party;
                FillLenderInformation(party);

                if (form.AvailableSchedule.Count() != 0)
                {
                    storeAmortizationSchedule.DataSource = form.AmortizationSchedules;
                    storeAmortizationSchedule.DataBind();
                }
                else
                {
                    var agr = ObjectContext.Agreements.SingleOrDefault(entity => entity.ApplicationId == loanApplication.ApplicationId);
                    var schedule = ObjectContext.AmortizationSchedules.SingleOrDefault(entity => entity.AgreementId == agr.Id);
                    var items = ObjectContext.AmortizationScheduleItems.Where(entity => entity.AmortizationScheduleId == schedule.Id);
                    var scheduleItems = LoanApplication.CreateScheduleItems(loanApplication, schedule);

                    if (items.Count() == 0)
                    {
                        storeAmortizationSchedule.DataSource = scheduleItems;
                        storeAmortizationSchedule.DataBind();
                    }
                    else
                    {   
                        var type = UnitOfMeasure.GetByID(loanApplication.PaymentModeUomId).Name;
                        int i = 0;
                        foreach (var amModel in items)
                        {
                            AmortizationScheduleModel newModel = new AmortizationScheduleModel();
                            newModel.Counter = GetItemType(type) + " " + (i + 1).ToString();
                            newModel.InterestPayment = amModel.InterestPayment;
                            newModel.IsBilledIndicator = amModel.IsBilledIndicator;
                            newModel.PrincipalBalance = amModel.PrincipalBalance;
                            newModel.PrincipalPayment = amModel.PrincipalPayment;
                            newModel.ScheduledPaymentDate = amModel.ScheduledPaymentDate;
                            newModel.TotalLoanBalance = amModel.TotalLoanBalance;
                            newModel.TotalPayment = amModel.PrincipalPayment + amModel.InterestPayment;
                            i++;

                            form.AddAmortizationModel(newModel);
                        }

                        storeAmortizationSchedule.DataSource = form.AmortizationSchedules;
                        storeAmortizationSchedule.DataBind();
                    }
                }
            }
        }

        public static string GetItemType(string unitOfMeasure)
        {
            string type = "";

            if (unitOfMeasure == UnitOfMeasure.DailyType.Name)
                type = "Day";
            else if (unitOfMeasure == UnitOfMeasure.MonthlyType.Name)
                type = "Month";
            else if (unitOfMeasure == UnitOfMeasure.SemiMonthlyType.Name)
                type = "Semi-Month";
            else if (unitOfMeasure == UnitOfMeasure.WeeklyType.Name)
                type = "Week";
            else if (unitOfMeasure == UnitOfMeasure.AnnuallyType.Name)
                type = "Year";

            return type;
        }

        private void FillLenderInformation(Party party)
        {
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;

            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);

            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);
        }

        private void FillPostalAddress(PostalAddress postalAddress)
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