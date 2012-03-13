using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintRediscounting : ActivityPageBase
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
            FillHeader();
         
    

        }
        public void FillHeader()
        {
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
                                  join sr in ObjectContext.Encashments on d.PaymentId equals sr.PaymentId
                                  where d.PaymentId == id && d.DisbursementTypeId == DisbursementType.RediscountingType.Id
                                  select new { p, sr }).FirstOrDefault();
            var currencySymbol = Currency.CurrencySymbolByPaymentId(disburseRecord.p.Id);
            var currencyDescription = Currency.CurrencyDescriptionByPaymentId(disburseRecord.p.Id);

            lblDateDisbursed.Text = String.Format("{0:MMMM dd, yyyy}", disburseRecord.p.TransactionDate);
            lblAmountInWords.Text = ": " + ConvertNumbers.EnglishFromNumber((double)disburseRecord.p.TotalAmount) + currencyDescription;
            lblDisbursedTo.Text = ": " + Person.GetPersonFullName((int)disburseRecord.p.ProcessedToPartyRoleId);
            lblAmountDisbursed.Text = "( " + disburseRecord.p.TotalAmount.ToString("N")+" "+currencySymbol + " )";
            lblDisbursedBy.Text = ": " + Person.GetPersonFullName(disburseRecord.p.ProcessedByPartyRoleId);
            lblReceivedby.Text = Person.GetPersonFullName((int)disburseRecord.p.ProcessedToPartyRoleId).ToUpper();
            lblSurchargeFee.Text = ": " + disburseRecord.sr.Surcharge.ToString("N")+" "+currencySymbol;

            var wholeAmount = Math.Floor(disburseRecord.p.TotalAmount);
            decimal decimalAmount = disburseRecord.p.TotalAmount - wholeAmount;
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



            var disbursement = disburseRecord.p.Disbursement;
            if (disbursement.DisbursedToName != null)
                lblReceivedByName.Text = ": "+disbursement.DisbursedToName;
            else lblReceivedByName.Text = ": "+Person.GetPersonFullName((int)disburseRecord.p.ProcessedToPartyRoleId);

            var formDetail = FormDetail.GetByPaymentId(disburseRecord.p.Id);
            if (!string.IsNullOrWhiteSpace(formDetail.Signature))
                imgSignature.ImageUrl = formDetail.Signature;

            var receiptPaymentRecord = (from rpa in ObjectContext.ReceiptPaymentAssocs
                                        join r in ObjectContext.Receipts on rpa.ReceiptId equals r.Id
                                        where rpa.PaymentId == disburseRecord.p.Id
                                        select new { r, rpa }).FirstOrDefault();

            var cheque = (from rpa in ObjectContext.ReceiptPaymentAssocs
                          join p in ObjectContext.Payments on rpa.PaymentId equals p.Id
                          join c in ObjectContext.Cheques on p.Id equals c.PaymentId
                          where p.PaymentTypeId == PaymentType.Receipt.Id && p.SpecificPaymentTypeId == SpecificPaymentType.CheckForRediscountingType.Id
                          && rpa.ReceiptId == receiptPaymentRecord.r.Id
                          select c).FirstOrDefault();
            lblBank.Text = ": " + Organization.GetOrganizationName(cheque.BankPartyRoleId);
            lblCheckNumber.Text = ": " + cheque.Payment.PaymentReferenceNumber.ToString();
            lblCheckDate.Text = ": " + cheque.CheckDate.ToString();
            lblCheckAmount.Text = ": " + cheque.Payment.TotalAmount.ToString("N")+" "+currencySymbol;

            var cashBreakdown = from p in ObjectContext.Payments
                                where p.ParentPaymentId == receiptPaymentRecord.rpa.PaymentId
                                && p.PaymentMethodTypeId == PaymentMethodType.CashType.Id
                                && p.PaymentTypeId == PaymentType.Disbursement.Id
                                select new AmountBreakdown
                                {
                                    PaymentMethod = p.PaymentMethodType.Name,
                                    TotalAmount = p.TotalAmount,
                                    CheckNumber = "N/A",
                                    Payment = p
                                };
            var checkBreakDown = from p in ObjectContext.Payments
                                 join c in ObjectContext.Cheques on p.Id equals c.PaymentId
                                 where p.ParentPaymentId == receiptPaymentRecord.rpa.PaymentId
                                && (p.PaymentMethodTypeId == PaymentMethodType.PayCheckType.Id
                                || p.PaymentMethodTypeId == PaymentMethodType.PersonalCheckType.Id)
                                 && p.PaymentTypeId == PaymentType.Disbursement.Id
                                 select new AmountBreakdown
                                 {
                                     PaymentMethod = p.PaymentMethodType.Name,
                                     TotalAmount = p.TotalAmount,
                                     CheckNumber = p.PaymentReferenceNumber,
                                     Payment = p
                                 };
            checkBreakDown = checkBreakDown.Concat(cashBreakdown);
            strBreakdown.DataSource = checkBreakDown.ToList();
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