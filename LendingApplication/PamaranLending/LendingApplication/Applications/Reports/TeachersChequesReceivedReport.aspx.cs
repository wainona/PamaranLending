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
    public partial class TeachersChequesReceivedReport : ActivityPageBase
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

        /**
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                return allowed;
            }
        }
        **/
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DateTime today = DateTime.Now.Date;
            lblCurrentDate.Text = today.ToString("MMMM dd, yyyy");
            var queryList = CreateQuery(today);
            decimal total = 0;
            foreach (var item in queryList)
            {
                total = total + item.Amount;
            }

            lblTotalAmount.Text = "Total Amount: " + total.ToString("N");
            lblTotalNumberOfCheques.Text = "Total Number of Checks Received: " + queryList.Count().ToString();

            this.ChequesReportStore.DataSource = queryList.Take(e.Limit).Skip(e.Start);
            this.ChequesReportStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RetreiveHeaderDetails();
            //DateTime today = DateTime.Now.Date;
            //lblCurrentDate.Text = today.ToString("MMMM dd, yyyy");
            //var queryList = CreateQuery(today);
            //decimal total = 0;
            //foreach (var item in queryList)
            //{
            //    total = total + item.Amount;
            //}

            //lblTotalAmount.Text = "Total Amount: " + total.ToString("N");
            //lblTotalNumberOfCheques.Text = "Total Number of Checks Received: " + queryList.Count().ToString();

            //ChequesReportStore.DataSource = queryList;
            //ChequesReportStore.DataBind();
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

        private List<ChequesReceivedModel> CreateQuery(DateTime now)
        {
            var tomm = now.AddDays(1);
            var query = from chq in ObjectContext.Cheques
                        join pymnt in ObjectContext.Payments.Where(entity => entity.PaymentTypeId == PaymentType.Receipt.Id) 
                            on chq.PaymentId equals pymnt.Id
                        join cust in ObjectContext.Customers
                            on pymnt.ProcessedToPartyRoleId equals cust.PartyRoleId
                        join custcat in ObjectContext.CustomerCategories.Where(entity => entity.EndDate ==  null && entity.CustomerCategoryType == CustomerCategoryType.TeacherType.Id)
                            on cust.PartyRoleId equals custcat.PartyRoleId
                        where pymnt.EntryDate >= now.Date && pymnt.EntryDate <tomm && pymnt.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                        select pymnt;

            List<ChequesReceivedModel> chequesReceivedList = new List<ChequesReceivedModel>();

            foreach (var item in query)
            {
                chequesReceivedList.Add(new ChequesReceivedModel(item));
            }

            return chequesReceivedList;
        }

        private class ChequesReceivedModel
        {
            public string ReceivedFrom { get; set; }
            public string CheckNumber { get; set; }
            public decimal Amount { get; set; }
            public string TransactionDate { get; set; }

            public ChequesReceivedModel(Payment payment)
            {
                var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == payment.ProcessedToPartyRoleId && entity.EndDate == null);
                this.ReceivedFrom = Person.GetPersonFullName(partyRole.Party);

                this.CheckNumber = payment.PaymentReferenceNumber;
                this.Amount = payment.TotalAmount;
                this.TransactionDate = payment.TransactionDate.ToString("MMM dd, yyyy");
            }
        }
    }
}