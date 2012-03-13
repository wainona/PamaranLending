using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using Ext.Net;
using BusinessLogic;

namespace LendingApplication.Applications.ReceiptUseCases
{
    public partial class AddCustomerSalary : ActivityPageBase
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

        public string LogInId
        {
            get
            {
                if (ViewState["LogInId"] != null)
                    return ViewState["LogInId"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["LogInId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                int partyroleid = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
                this.LogInId = PartyRole.GetById(partyroleid).PartyId.ToString();

                var paymenttypes = Context.PaymentMethodTypes.Where(entity => (entity.Id == PaymentMethodType.ATMType.Id) || (entity.Id == PaymentMethodType.PayCheckType.Id));
                paymenttypes.ToList();
                strPaymentMethod.DataSource = paymenttypes;
                strPaymentMethod.DataBind();
                cmbPaymentMethod.SelectedIndex = 0;
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            var TransactionDate = DateTime.Today;
            var employee = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId);
            DateTime Now = DateTime.Now;
            Receipt newReceipt = Receipt.CreateReceipt(null, decimal.Parse(txtAmount.Text));
            ReceiptStatu newReceiptStatu = ReceiptStatu.Create(newReceipt, Now, ReceiptStatusType.OpenReceiptStatusType, true);
            Payment newPayment = Payment.CreatePayment(Now,
                                                        TransactionDate,
                                                        int.Parse(hdnSelectedPartyRoleId.Text),
                                                        employee.PartyRoleId,
                                                        decimal.Parse(txtAmount.Text),
                                                        PaymentType.Receipt.Id,
                                                        int.Parse(cmbPaymentMethod.SelectedItem.Value),
                                                        SpecificPaymentType.LoanPaymentType.Id,
                                                        null);
            ReceiptPaymentAssoc newRecPayAssoc = Receipt.CreateReceiptPaymentAssoc(newReceipt, newPayment);

            if (cmbPaymentMethod.SelectedItem.Value == PaymentMethodType.PayCheckType.Id.ToString())
            {
                Cheque newChe = Cheque.CreateCheque(newPayment, int.Parse(hdnBankID.Text), dtCheckDate.SelectedDate);
                ChequeStatu newChqStatu = ChequeStatu.CreateChequeStatus(newChe, ChequeStatusType.ClearedType.Id, Now, txtCheckNumber.Text, true);
                newPayment.PaymentReferenceNumber = txtCheckNumber.Text;

                //Add objects to ObjectContext for Checks
                Context.Cheques.AddObject(newChe);
                Context.ChequeStatus.AddObject(newChqStatu);
            }
            else
                newPayment.PaymentReferenceNumber = txtRefNum.Text;

            SalaryReceipt salaryReceipt = new SalaryReceipt();
            salaryReceipt.Receipt = newReceipt;
            salaryReceipt.IsApplied = false;


            //Add objects to ObjectContext
            Context.Receipts.AddObject(newReceipt);
            Context.ReceiptStatus.AddObject(newReceiptStatu);
            Context.Payments.AddObject(newPayment);
            Context.ReceiptPaymentAssocs.AddObject(newRecPayAssoc);
            Context.SaveChanges();
        }

        protected void btnSearch_Click(object sender, DirectEventArgs e)
        {
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            dataSource.SearchString = txtSearchKey.Text;
            dataSource.UserId = int.Parse(LogInId);


            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public int ValidateCheckNumber(string inputCheckNumber)
        {
            return Cheque.ValidateCheckNumber(inputCheckNumber);
        }

        private class CustomersListModel
        {
            public int CustomerID { get; set; }
            public string Names { get; set; }
        }

        public enum SearchBy
        {
            ID = 0,
            Name = 1,
            None = -1
        }

        private class DataSource : IPageAbleDataSource<PersonWithLoan>
        {
            public string Name { get; set; }
            public string filterString2 { get; set; }
            public string SearchString { get; set; }
            public SearchBy SearchBy { get; set; }
            public int UserId { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
                this.SearchString = string.Empty;
                this.SearchBy = SearchBy.None;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        var query = CreateQuery(context);
                        count = query.Count();
                    }

                    return count;
                }
            }

            private IEnumerable<PersonWithLoan> CreateQuery(FinancialEntities context)
            {
                var lendingRoleId = context.PartyRoles.SingleOrDefault(entity => entity.PartyRoleType.RoleType.Name == RoleTypeEnums.LendingInstitution && entity.EndDate == null);
                var query = Party.GetAllCustomersWithLoan(this.UserId);

                switch (SearchBy)
                {
                    case SearchBy.Name:
                        query = query.Where(entity => entity.Names.ToLower().Contains(SearchString));
                        break;

                    case SearchBy.None:
                        break;
                    default:
                        break;
                }

                return query;
            }

            public override List<PersonWithLoan> SelectAll(int start, int limit, Func<PersonWithLoan, string> orderBy)
            {
                List<PersonWithLoan> collection;
                using (var context = new FinancialEntities())
                {
                    var query = CreateQuery(context);
                    collection = query.OrderBy(orderBy).Skip(start).Take(limit).ToList();
                }

                return collection;
            }
        }
    }
}