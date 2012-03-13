using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication.Applications.Reports
{
    public partial class PostDatedChecksReport : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Accountant");
                allowed.Add("Admin");
                allowed.Add("Teller");

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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DateTime startDate = DateTime.Parse(hdnDate.Value.ToString());
            var endDate = dtEndDate.SelectedDate;

            lblStartDate.Text = startDate.ToString("MMMM yyyy");
            lblEndDate.Text = string.Empty;

            var queryList = CreateQuery(startDate, endDate).OrderBy(entity => entity.CheckDate);
            decimal total = queryList.Sum(entity => entity.CheckAmount);

            lblTotalAmount.Text = "Total Amount: " + total.ToString("N");
            //lblTotalNumberOfCheques.Text = "Total Number of Checks Received: " + queryList.Count().ToString();

            ChequesReportStore.DataSource = queryList;
            ChequesReportStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                RetreiveHeaderDetails();
                int numberOfDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                dtStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                hdnDate.Value = dtStartDate.SelectedDate.ToString();
                dtEndDate.SelectedDate = new DateTime(dtStartDate.SelectedDate.Year, dtStartDate.SelectedDate.Month, numberOfDays);
            }
        }

        protected void onStartDateSelect(object sender, EventArgs e)
        {
            DateTime StartDate = DateTime.Parse(hdnDate.Value.ToString());
            int numberOfDays = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);
            dtEndDate.SelectedDate = new DateTime(StartDate.Year, StartDate.Month, numberOfDays);
        }

        protected void onEndDateSelect(object sender, EventArgs e)
        {
            dtStartDate.MaxDate = dtEndDate.SelectedDate.Date;
        }

        private void RetreiveHeaderDetails()
        {
            var lenderPartyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.RoleTypeId == RoleType.LendingInstitutionType.Id && entity.EndDate == null);
            var lenderName = ObjectContext.Organizations.SingleOrDefault(entity => entity.PartyId == lenderPartyRole.Party.Id).OrganizationName;
            lblLenderName.Text = lenderName;

            var lenderAddress = PostalAddress.GetCurrentPostalAddress(lenderPartyRole.Party, PostalAddressType.BusinessAddressType, true);
            lblLenderAddress.Text = StringConcatUtility.Build(", ", lenderAddress.StreetAddress, lenderAddress.Barangay,
                                                                    lenderAddress.City, lenderAddress.Municipality);

            lblLenderTelephoneNumber.Text = "Tel. No. " + PrintFacade.GetPrimaryPhoneNumber(lenderPartyRole.Party, lenderAddress);
            lblLenderFaxNumber.Text = "Fax " + PrintFacade.GetFaxNumber(lenderPartyRole.Party, lenderAddress);
        }

        private List<ChequesReceivedModel> CreateQuery(DateTime startDate, DateTime endDate)
        {
            var query = from chq in ObjectContext.Cheques
                        join chqassoc in ObjectContext.ChequeLoanAssocs on chq.Id equals chqassoc.ChequeId
                        join la in ObjectContext.LoanAccounts on chqassoc.FinancialAccountId equals la.FinancialAccountId
                        join pymnt in ObjectContext.Payments on chq.PaymentId equals pymnt.Id
                        join chqstat in ObjectContext.ChequeStatus on chq.Id equals chqstat.CheckId
                        where chq.CheckDate >= startDate && chq.CheckDate <= endDate && chqstat.IsActive == true
                        select new ChequesReceivedModel()
                        {
                            Payment = pymnt,
                            Cheque = chq,
                            LoanAccount = la,
                            _CheckStatus = chqstat
                        };

            return query.ToList();
        }

        private class ChequesReceivedModel
        {
            public DateTime CheckDate
            {
                get
                {
                    return this.Cheque.CheckDate;
                }
            }
            public string _CheckDate
            {
                get
                {
                    return this.CheckDate.ToString("MMM dd, yyyy");
                }
            }
            public string Name
            {
                get
                {
                    var party = this.Payment.PartyRole1.Party;
                    return party.Name;
                }
            }
            public int CheckCount
            {
                get
                {
                    int count = 0;
                    var temp = ObjectContext.ChequeLoanAssocs.Where(entity => entity.FinancialAccountId == this.LoanAccount.FinancialAccountId).OrderBy(entity => entity.Id);
                    foreach (var item in temp)
                    {
                        if (item.ChequeId == this.Cheque.Id)
                        {
                            count += 1;
                            break;
                        }
                        count++;
                    }
                    return count;
                }
            }
            public string Term
            {
                get
                {
                    var agreementItem = this.LoanAccount.FinancialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive);
                    if (agreementItem != null)
                        return agreementItem.LoanTermLength.ToString();
                    else
                        return string.Empty;
                }
            }
            public string Bank
            {
                get
                {
                    return this.Cheque.PartyRole.Party.Name;
                }
            }
            public string CheckNumber
            {
                get
                {
                    return this.Payment.PaymentReferenceNumber;
                }
            }
            public decimal RemainingBalance
            {
                get
                {
                    return this.LoanAccount.LoanBalance;
                }
            }
            public decimal LoanAmount
            {
                get
                {
                    return this.LoanAccount.LoanAmount;
                }
            }
            public decimal Interest
            {
                get
                {
                    var agreementItem = this.LoanAccount.FinancialAccount.Agreement.AgreementItems.SingleOrDefault(entity => entity.IsActive == true);

                    if (agreementItem != null)
                    {
                        if (agreementItem.InterestComputationMode == "StraightLine Computation Mode")
                            return this.LoanAmount * (agreementItem.InterestRate/100);
                        else
                            return this.RemainingBalance * (agreementItem.InterestRate/100);
                    }
                    else
                        return 0;
                }
            }
            public decimal CheckAmount
            {
                get
                {
                    return this.Payment.TotalAmount;
                }
            }
            public string CheckStatus
            {
                get
                {
                    return _CheckStatus.ChequeStatusType.Name;
                }
            }
            public string Remarks
            {
                get
                {
                    return this._CheckStatus.Remarks;
                }
            }

            public Payment Payment { get; set; }
            public LoanAccount LoanAccount { get; set; }
            public Cheque Cheque { get; set; }
            public ChequeStatu _CheckStatus { get; set; }

            public ChequesReceivedModel()
            {

            }

            public ChequesReceivedModel(Payment payment)
            {
                //var customerClassification = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == payment.ProcessedToPartyRoleId && entity.EndDate == null);
                //if (customerClassification != null)
                //    this.District = customerClassification.ClassificationType.District;

                //var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == payment.ProcessedToPartyRoleId && entity.EndDate == null);
                //this.ReceivedFrom = Person.GetPersonFullName(partyRole.Party);
                //this.Date = payment.TransactionDate.Date;
                //this.CheckNumber = payment.PaymentReferenceNumber;
                //this.Amount = payment.TotalAmount;
            }
        }
    }
}