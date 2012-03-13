using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.ChequeUseCases
{
    public partial class PrintCheque : ActivityPageBase
    {
        private static FinancialEntities Context
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
                //int receiptId = int.Parse(Request.QueryString["id"]);
                string mode = Request.QueryString["mode"];

                if (mode != "foreign")
                {
                    int disbursementId = int.Parse(Request.QueryString["id"]);
                    string checkNumber = Request.QueryString["cn"];
                    Payment payment;
                    if (checkNumber == null) payment = Payment.GetById(disbursementId);
                    else payment = Context.Payments.SingleOrDefault(x => x.PaymentReferenceNumber == checkNumber);

                    Cheque check = Cheque.GetByPaymentId(payment);
                    string amountInWords = ConvertNumbers.EnglishFromNumber((double)payment.TotalAmount);

                    FillCheque(check, payment, amountInWords);
                }
                else
                {
                    int forExChequeId = int.Parse(Request.QueryString["id"]);
                    int forExId = int.Parse(Request.QueryString["forexid"]);
                    ForExCheque cheque = Context.ForExCheques.SingleOrDefault(x => x.Id == forExChequeId);
                    BusinessLogic.ForeignExchange forEx = Context.ForeignExchanges.SingleOrDefault(x => x.Id == forExId);
                    string amountInWords = ConvertNumbers.EnglishFromNumber((double)cheque.ForExDetail.Amount);

                    FillCheque(cheque, forEx, amountInWords);
                }

                FillSignatories();
            }
        }

        protected void FillSignatories()
        {

            var signatures = from e in Context.Employees
                             join pr in Context.PartyRoles on e.PartyRoleId equals pr.Id
                             where pr.EndDate == null & pr.RoleTypeId == RoleType.EmployeeType.Id 
                             && e.Position == EmployeePositionType.OwnerType.Name
                             select pr;

            List<SignatoriesModel> list = new List<SignatoriesModel>();
            list.Add(new SignatoriesModel() { Id = 0, Name = "____________________" });
            foreach (var item in signatures)
            {
                list.Add(new SignatoriesModel(item));
            }

            SignatoryStore.DataSource = list;
            SignatoryStore.DataBind();
        }

        private void FillCheque(Cheque check, Payment payment, string amountInWords)
        {
            lblDate.Text = String.Format("{0: MMMM d, yyyy}", check.CheckDate);
            lblName.Text = "***" + payment.PartyRole1.Party.Name.ToUpper() + "***";
            lblAmount.Text = "***" + payment.TotalAmount.ToString("N") + "***";
            lblAmountInWords.Text = "***" + amountInWords.ToUpper() + " " + "ONLY***";
            var bank = Context.Banks.SingleOrDefault(x => x.PartyRoleId == check.BankPartyRoleId);
            lblBankName.Text = bank.PartyRole.Party.Organization.OrganizationName.ToUpper() + " (" + payment.PaymentReferenceNumber + ")";
            lblCurrency.Text = "PESOS";
        }

        private void FillCheque(ForExCheque check, BusinessLogic.ForeignExchange forEx, string amountInWords)
        {
            lblDate.Text = String.Format("{0: MMMM d, yyyy}", check.CheckDate);
            lblName.Text = "***" + forEx.PartyRole1.Party.Name.ToUpper() + "***";
            lblAmount.Text = "***" + check.ForExDetail.Amount.ToString("N") + "***";
            lblAmountInWords.Text = "***" + amountInWords.ToUpper() + " " + "ONLY***";
            var bank = Context.Banks.SingleOrDefault(x => x.PartyRoleId == check.BankPartyRoleId);
            lblBankName.Text = bank.PartyRole.Party.Organization.OrganizationName.ToUpper() + " (" + check.CheckNumber + ")";
            if (check.ForExDetail.Currency.Symbol == "PHP")
                lblCurrency.Text = "PESOS";
            else lblCurrency.Text = check.ForExDetail.Currency.Symbol;
        }

        protected class SignatoriesModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public SignatoriesModel()
            {

            }

            public SignatoriesModel(PartyRole partyRole)
            {
                this.Id = partyRole.Id;
                this.Name = partyRole.Party.NameV2.ToUpper();
            }
        }
    }
}