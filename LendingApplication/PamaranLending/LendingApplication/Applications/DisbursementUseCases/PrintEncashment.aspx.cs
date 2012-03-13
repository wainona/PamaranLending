using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using Ext.Net;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintEncashment : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
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
            int id = int.Parse(Request.QueryString["id"]);
            FillForm(id);
            
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
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

        public void FillForm(int id)
        {

            var disburseRecord = (from d in ObjectContext.Disbursements
                                  join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                  where d.PaymentId == id && d.DisbursementTypeId == DisbursementType.EncashmentType.Id
                                  select p).FirstOrDefault();
            var currencySymbol = Currency.CurrencySymbolByPaymentId(disburseRecord.Id);
            var currencyDescription = Currency.CurrencyDescriptionByPaymentId(disburseRecord.Id);

            var wholeAmount = Math.Floor(disburseRecord.TotalAmount);
            decimal decimalAmount = disburseRecord.TotalAmount - wholeAmount;
            string decimalAmountInWords = string.Empty;
            string wholeAmountInWords = ConvertNumbers.EnglishFromNumber((double)wholeAmount);
            lblAmountInWords.Text = ": " + wholeAmountInWords + currencyDescription + " Only";

            if (decimalAmount > 0)
            {
                decimalAmount *= 100;
                decimalAmountInWords = ConvertNumbers.EnglishFromNumber((double)decimalAmount);
                lblAmountInWords.Text = ": " + wholeAmountInWords + " and " + decimalAmountInWords +
                    " " + currencyDescription + " Only";
            }

            lblDateDisbursed.Text = String.Format("{0:MMMM dd, yyyy}", disburseRecord.TransactionDate);
            lblDisbursedTo.Text = ": " + Person.GetPersonFullName((int)disburseRecord.ProcessedToPartyRoleId);
            lblAmountDisbursed.Text = "( " + disburseRecord.TotalAmount.ToString("N") + " " + currencySymbol + " )";
            lblDisbursedBy.Text = ": " +Person.GetPersonFullName(disburseRecord.ProcessedByPartyRoleId);
            lblReceivedby.Text = Person.GetPersonFullName((int)disburseRecord.ProcessedToPartyRoleId).ToUpper();

            var receiptPaymentRecord = (from rpa in ObjectContext.ReceiptPaymentAssocs
                                        join r in ObjectContext.Receipts on rpa.ReceiptId equals r.Id
                                        where disburseRecord.Id == rpa.PaymentId
                                        select new { r, rpa }).FirstOrDefault();

            var cheque = (from rpa in ObjectContext.ReceiptPaymentAssocs
                          join p in ObjectContext.Payments on rpa.PaymentId equals p.Id
                          join c in ObjectContext.Cheques on p.Id equals c.PaymentId
                          where p.PaymentTypeId == PaymentType.Receipt.Id && p.SpecificPaymentTypeId == SpecificPaymentType.CheckForEncashmentType.Id
                          && rpa.ReceiptId == receiptPaymentRecord.r.Id
                          select c).FirstOrDefault();
            lblBank.Text = ": " + Organization.GetOrganizationName(cheque.BankPartyRoleId);
            lblCheckNumber.Text = ": " + cheque.Payment.PaymentReferenceNumber.ToString();
            lblCheckDate.Text = ": " + cheque.CheckDate.ToString();
            lblCheckAmount.Text = ": " + cheque.Payment.TotalAmount.ToString("N") + " " + currencySymbol;

            var formDetail = FormDetail.GetByPaymentId(id);

            if(!string.IsNullOrWhiteSpace(formDetail.Signature))
                imgSignature.ImageUrl = formDetail.Signature;

            var breakdown = DisbursementFacade.GetBreakdown(receiptPaymentRecord.rpa.PaymentId);
            strBreakdown.DataSource = breakdown.ToList();
            strBreakdown.DataBind();

          
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