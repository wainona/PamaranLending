using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class PrintOtherDisbursement : ActivityPageBase
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
            /*DISBURSEMENT DETAILS*/
            int id = int.Parse(Request.QueryString["id"]);
            FillForm(id);
            
            /*HEADER DETAILS*/
            var partyRole = ObjectContext.PartyRoles.FirstOrDefault(x => x.RoleTypeId == RoleType.LendingInstitutionType.Id);
            var party = partyRole.Party;
            Organization organization = party.Organization;
            lblLenderNameHeader.Text = organization.OrganizationName;
            //lblLenderNameHeader1.Text = organization.OrganizationName;
            //lblLenderNameHeader2.Text = organization.OrganizationName;

            var postalAddress = PrintFacade.SetAndGetPostalAddress(party);
            FillPostalAddress(postalAddress);

            lblPrimTelNumber.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            lblSecTelNumber.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            lblFaxNumber.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            lblEmailAddress.Text = PrintFacade.GetEmailAddress(party);

            //lblPrimTelNumber1.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            //lblSecTelNumber1.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            //lblFaxNumber1.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            //lblEmailAddress1.Text = PrintFacade.GetEmailAddress(party);

            //lblPrimTelNumber2.Text = PrintFacade.GetPrimaryPhoneNumber(party, postalAddress).ToString();
            //lblSecTelNumber2.Text = PrintFacade.GetSecondaryPhoneNumber(party, postalAddress).ToString();
            //lblFaxNumber2.Text = PrintFacade.GetFaxNumber(party, postalAddress).ToString();
            //lblEmailAddress2.Text = PrintFacade.GetEmailAddress(party);

            /*ITEMS*/
            var items = ObjectContext.DisbursementItems.Where(entity => entity.PaymentId == id).ToList();
            grdOtherDisbursement.DataSource = items;
            grdOtherDisbursement.DataBind();

            //grdOtherDisbursement1.DataSource = items;
            //grdOtherDisbursement1.DataBind();

            //grdOtherDisbursement2.DataSource = items;
            //grdOtherDisbursement2.DataBind();
        }

        private void FillForm(int id)
        {
            var details = ObjectContext.OtherDisbursementDetails.FirstOrDefault(entity => entity.PaymentId == id);
            var totalAmountDisbursed = ObjectContext.DisbursementViewLists.FirstOrDefault(entity => entity.DisbursementId == id && entity.DisbursementTypeId == DisbursementType.OtherLoanDisbursementType.Id);
            
            lblTotalAmount.Text = totalAmountDisbursed.Amount.ToString("N");
            lblDateDisbursed.Text = String.Format("{0:MMMM dd, yyyy}", details.Date_Disbursed);
            lblDisbursedTo.Text = ": " + details.DisbursedTo;
        
            //lblTotalAmount1.Text = totalAmountDisbursed.Amount.ToString("N");
            //lblDateDisbursed1.Text = String.Format("{0:MMMM dd, yyyy}", details.Date_Disbursed);
            //lblDisbursedTo1.Text = ": " + details.DisbursedTo;

            //lblTotalAmount2.Text = totalAmountDisbursed.Amount.ToString("N");
            //lblDateDisbursed2.Text = String.Format("{0:MMMM dd, yyyy}", details.Date_Disbursed);
            //lblDisbursedTo2.Text = ": " + details.DisbursedTo;


            lblDisbursedBy.Text = ": " + details.DisbursedBy;
            lblReceivedby.Text = details.DisbursedTo.ToUpper();
            lblAmountReceived.Text = ": " + ConvertNumbers.EnglishFromNumber((double)totalAmountDisbursed.Amount) + " Pesos Only";

            //lblDisbursedBy1.Text = ": " + details.DisbursedBy;
            //lblReceivedby1.Text = details.DisbursedTo.ToUpper();
            //lblAmountReceived1.Text = ": " + ConvertNumbers.EnglishFromNumber((double)totalAmountDisbursed.Amount) + " Pesos Only";

            //lblDisbursedBy2.Text = ": " + details.DisbursedBy;
            //lblReceivedby2.Text = details.DisbursedTo.ToUpper();
            //lblAmountReceived2.Text = ": " + ConvertNumbers.EnglishFromNumber((double)totalAmountDisbursed.Amount) + " Pesos Only";

            var cashBreakdown = from p in ObjectContext.Payments
                                where p.ParentPaymentId == id
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
                                 where p.ParentPaymentId == id
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

            var formDetail = FormDetail.GetByPaymentId(id);

            if (formDetail != null)
            imgSignature.ImageUrl = formDetail.Signature;
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

            //lblStreetAddress1.Text = postalAddress.StreetAddress;
            //lblBarangay1.Text = postalAddress.Barangay;
            //lblCity1.Text = postalAddress.City;
            //lblMunicipality1.Text = postalAddress.Municipality;
            //lblProvince1.Text = postalAddress.Province;
            //lblCountry1.Text = postalAddress.Country.Name;
            //lblPostalCode1.Text = postalAddress.PostalCode;

            //lblStreetAddress2.Text = postalAddress.StreetAddress;
            //lblBarangay2.Text = postalAddress.Barangay;
            //lblCity2.Text = postalAddress.City;
            //lblMunicipality2.Text = postalAddress.Municipality;
            //lblProvince2.Text = postalAddress.Province;
            //lblCountry2.Text = postalAddress.Country.Name;
            //lblPostalCode2.Text = postalAddress.PostalCode;
        }
    }
}