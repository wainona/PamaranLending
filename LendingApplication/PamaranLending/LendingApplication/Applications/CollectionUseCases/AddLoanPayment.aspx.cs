using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class AddLoanPayment : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                datTransactionDate.DisabledDays = ApplicationSettings.DisabledDays;
                AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
                this.hiddenResourceGUID.Value = this.ResourceGuid;
                decimal amount = 0;
                txtTotalAmount.Text = amount.ToString("N");
                txtATMAmount.Text = amount.ToString(); ;
                datTransactionDate.SelectedDate = DateTime.Now;
            }
        }

        public void btnSaveATM_Click(object sender, DirectEventArgs e)
        {
            string referencenumber = txtATMReferenceNum.Text;
            decimal amount = Decimal.Parse(txtATMAmount1.Text);
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            if (string.IsNullOrWhiteSpace(HiddenRandomKey2.Text))
            {
                ATMPaymentModel model = new ATMPaymentModel();
                model.Amount = amount;
                model.ATMReferenceNumber = referencenumber;
                form.AddATM(model);

            }
            else
            {
                ATMPaymentModel model = form.GetATMs(HiddenRandomKey2.Text);
                model.ATMReferenceNumber = referencenumber;
                model.Amount = amount;
                model.MarkEdited();
            }

            grdPnlATMStore.DataSource = form.AvailableATMs;
            grdPnlATMStore.DataBind();
        }

        public void onBtnATMDelete_Click(object sender, DirectEventArgs e)
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            SelectedRowCollection rows = this.grdPnlATMSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveATMs(row.RecordID);
            }
            grdPnlATMStore.DataSource = form.AvailableATMs;
            grdPnlATMStore.DataBind();
        }
        [DirectMethod]
        public void UpdateWaive()
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            StoreBorrower.DataSource = form.AvailableBorrower;
            StoreBorrower.DataBind();

            StoreCoBorrowersGuarantor.DataSource = form.AvailableAddGuaBorror;
            StoreCoBorrowersGuarantor.DataBind();
        }
        [DirectMethod]
        public bool CanManualBill(int id)
        {
            bool flag = false;
            var loanAccount = from l in ObjectContext.LoanAccounts
                              join f in ObjectContext.FinancialAccounts on l.FinancialAccountId equals f.Id
                              join a in ObjectContext.Agreements on f.AgreementId equals a.Id
                              join ai in ObjectContext.AgreementItems on f.AgreementId equals ai.AgreementId
                              where l.FinancialAccountId == id && a.EndDate == null && ai.IsActive == true
                              select new
                              {
                                  loanaccount = l,
                                  agreementitem = ai
                              };
            if (loanAccount.Count() != 0)
            {
                var ammortSchedItems = from l in loanAccount
                                       join ash in ObjectContext.AmortizationSchedules on l.agreementitem.AgreementId equals ash.AgreementId
                                       join ashi in ObjectContext.AmortizationScheduleItems on ash.Id equals ashi.AmortizationScheduleId
                                       orderby ashi.ScheduledPaymentDate ascending
                                       where ash.EndDate == null && ashi.IsBilledIndicator == false
                                       select ashi;

                if (ammortSchedItems.Count() > 0)
                {
                    flag = true;
                }
            }
            return flag;
        }
        [DirectMethod]
        public void UpdateRebate()
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            StoreBorrower.DataSource = form.AvailableBorrower;
            StoreBorrower.DataBind();
        }

        [DirectMethod]
        public void UpdateBill()
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            StoreBorrower.DataSource = form.AvailableBorrower;
            StoreBorrower.DataBind();

            StoreCoBorrowersGuarantor.DataSource = form.AvailableAddGuaBorror;
            StoreCoBorrowersGuarantor.DataBind();
        }

        [DirectMethod]
        public void FillInterestBreakdown(string randomKey)
        {

            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            LoanPaymentModel borrower = (LoanPaymentModel)form.RetrieveBorrower(randomKey);
            if(borrower == null)
                borrower = (LoanPaymentModel)form.RetrieveGuaCoBorower(randomKey);
            this.Register(borrower);

            var interestReceivables = from r in borrower.InterestReceivables
                                      orderby r.DueDate ascending
                                      group r by r._DueDate into rp
                                      select new InterestBreakdown()
                                      {
                                          _Amount = rp.Sum(entity => entity.Amount),
                                          DueDate = rp.Key
                                      };
            strInterestBreakdown.DataSource = interestReceivables.ToList();
            strInterestBreakdown.DataBind();

        }

        private class InterestBreakdown
        {
            public decimal _Amount { get; set; }
            public string DueDate { get; set; }
            public string Amount
            {
                get
                {
                    return this._Amount.ToString("N");
                }
            }
        }
        [DirectMethod]
        public int FillCustomerId()
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            form.CustomerId = int.Parse(hiddenCustomerID.Text);

            return 1;
        }

        private void RetrieveBasicInformation(PartyRole customer)
        {
            var district = ObjectContext.CustomerClassifications.SingleOrDefault(entity => entity.PartyRoleId == customer.Id);
            if (district != null)
            {
                txtDistrict.Text = district.ClassificationType.District;
                txtStationNumber.Text = district.ClassificationType.StationNumber;
            }
            else
            {
                txtDistrict.Hidden = true;
                txtStationNumber.Hidden = true;
            }
        }
       

        private void RetrievePaymentsFromCash(PartyRole customer, AddLoanPaymentForm form)
        {
            form.ReceiptsOfCashPaymentMadeByCustomer = null;
            var receiptsWithBalance = ReceiptPaymentFacade.RetrieveReceiptsOfCashPaymentMadeByCustomer(customer);
            var receiptsId = from rpc in receiptsWithBalance select rpc.Id;
            form.ReceiptsOfCashPaymentMadeByCustomer = receiptsId.ToList();

            txtBalanceCashReceipts.Text = "0.00";
            //Check if naa gibayad na cash para naa maretrieve sa textbox or wala
            if (receiptsWithBalance.Count() > 0)
            {
                decimal total = receiptsWithBalance.Sum(x => x.ReceiptBalance.Value);
                form.BalanceOfCashReceipts = total;
                txtBalanceCashReceipts.Text = total.ToString();
                txtamountTendered.Text = total.ToString();
            }
        }

        private LoanPaymentModel CreatePaymentModel(FinancialAccount financialAccount)
        {
            LoanPaymentModel model = new LoanPaymentModel();

            //InterestDue
            var Interest = from r in financialAccount.LoanAccount.Receivables.Where(entity =>
                entity.ReceivableTypeId == ReceivableType.InterestType.Id)
                           select new ReceivableInfo
                           {
                               Amount = r.Balance,
                               ReceivableId = r.Id,
                               DueDate = r.ValidityPeriod
                           };
            //PastDue
            var PastDueInterest = from r in financialAccount.LoanAccount.Receivables.Where(entity =>
                entity.ReceivableTypeId == ReceivableType.PastDueType.Id)
                                  select new ReceivableInfo
                                  {
                                      Amount = r.Balance,
                                      ReceivableId = r.Id,
                                      DueDate = r.ValidityPeriod
                                  };
            model.InterestReceivables.Clear();
            model.PastDueReceivables.Clear();
            model.InterestReceivables.AddRange(Interest);
            model.PastDueReceivables.AddRange(PastDueInterest);

            //FinancialProductId
            var financialProduct = financialAccount.FinancialAccountProducts.SingleOrDefault(entity => entity.EndDate == null);
            model.LoanProductName = financialProduct.FinancialProduct.Name;
            model.LoanID = financialAccount.Id;
            model.LoanProductID = financialProduct.FinancialProductId;
            model.ComputeTotals();

            return model;
        }

        private DateTime? RetrieveLoanAccountsOfCustomer(PartyRole customer, AddLoanPaymentForm form)
        {
            var financialAccountRolesOfCustomer = from farc in ObjectContext.FinancialAccountRoles
                                                  join pr in ObjectContext.PartyRoles on farc.PartyRoleId equals pr.Id
                                                  where pr.PartyId == customer.PartyId
                                                  && pr.EndDate == null
                                                  && pr.RoleTypeId == RoleType.OwnerFinancialType.Id
                                                  select farc;

            var financialAccounts = from farc in financialAccountRolesOfCustomer
                                    join la in ObjectContext.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                    join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                    where las.IsActive && farc.FinancialAccount.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id
                                        && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.PendingEndorsementType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                    select farc.FinancialAccount;



            var oldestReleaseDate = from f in financialAccounts
                                    join la in ObjectContext.LoanAccounts on f.Id equals la.FinancialAccountId
                                    orderby la.LoanReleaseDate
                                    select la.LoanReleaseDate;

            foreach (var financialAccount in financialAccounts)
            {
                LoanPaymentModel borrower = CreatePaymentModel(financialAccount);
                form.AddBorrower(borrower);
            }

            if (oldestReleaseDate.Count() != 0)
                return (DateTime)oldestReleaseDate.FirstOrDefault();
            else return null;

        }

        private void RetrieveLoanAccountsOfCustomerAsCoBorrower(PartyRole customer, AddLoanPaymentForm form)
        {
            var customerAsBorrowerOrGuarantorRoles = from farc in ObjectContext.FinancialAccountRoles
                                                     join pr in ObjectContext.PartyRoles on farc.PartyRoleId equals pr.Id
                                                     where pr.PartyId == customer.PartyId
                                                     && pr.EndDate == null
                                                     && (pr.RoleTypeId == RoleType.CoOwnerFinancialType.Id
                                                     || pr.RoleTypeId == RoleType.GuarantorFinancialType.Id)
                                                     select farc;

            var financialAccounts = from farc in customerAsBorrowerOrGuarantorRoles
                                    join la in ObjectContext.LoanAccounts on farc.FinancialAccountId equals la.FinancialAccountId
                                    join las in ObjectContext.LoanAccountStatus on la.FinancialAccountId equals las.FinancialAccountId
                                    where las.IsActive && farc.FinancialAccount.FinancialAccountTypeId == FinancialAccountType.LoanAccountType.Id
                                        && (las.StatusTypeId == LoanAccountStatusType.CurrentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.DelinquentType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.PendingEndorsementType.Id ||
                                        las.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id)
                                    select farc.FinancialAccount;


            foreach (var financialAccount in financialAccounts)
            {
                LoanPaymentModel coborrower = CreatePaymentModel(financialAccount);
                form.AddGuaCoBorror(coborrower);
            }
        }

        [DirectMethod]
        public void FillCustomer(int id)
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            var today = DateTime.Now;

            btnAddCheck.Disabled = false;
            btnBrowseCheck.Disabled = false;

            form.ClearCheques();
            form.ClearBorrowerList();
            form.ClearCoBorrowerList();

            var customerOtherRole = PartyRole.GetById(id);
            //var partyrole = ObjectConte.PartyRoles.FirstOrDefault(entity => entity.PartyId == partyId && entity.RoleTypeId == role.Id && entity.EndDate == null);
            var customer = PartyRole.GetByPartyIdAndRole(customerOtherRole.PartyId, RoleType.CustomerType);
            if (customer == null) customer = PartyRole.GetByPartyIdAndRole(customerOtherRole.PartyId, RoleType.EmployeeType);
            if (customer == null) customer = PartyRole.GetByPartyIdAndRole(customerOtherRole.PartyId, RoleType.ContactType);
            hiddenCustomerID.Text = customer.Id.ToString();

            RetrieveBasicInformation(customer);
            RetrievePaymentsFromCash(customer, form);
           
            var mindate = RetrieveLoanAccountsOfCustomer(customer, form);
            if (mindate == null)
                datTransactionDate.MinDate = DateTime.Today;
            else datTransactionDate.MinDate = (DateTime)mindate;
            RetrieveLoanAccountsOfCustomerAsCoBorrower(customer, form);

            StoreCheck.DataSource = form.AvailableCheques;
            StoreBorrower.DataSource = form.AvailableBorrower;
            StoreCoBorrowersGuarantor.DataSource = form.AvailableAddGuaBorror;
            StoreCheck.DataBind();
            StoreBorrower.DataBind();
            StoreCoBorrowersGuarantor.DataBind();
        }

        [DirectMethod]
        public void AddChequesFromPickList(string bankName, string bankBranch, string checkNumber, string checkType, string checkDate, decimal checkAmount)
        {
            AddLoanPaymentForm form = CreateOrRetrieve<AddLoanPaymentForm>();
            ChequesModel model = new ChequesModel();
            model.BankName = bankName;
            model.Branch = bankBranch;
            model.CheckType = checkType;
            model.CheckNumber = checkNumber;
            model.CheckDate = DateTime.Parse(checkDate);
            model.Amount = checkAmount;

            form.AddCheques(model);
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }

        [DirectMethod]
        public void AddChequesManually(string bankName, string bankBranch, string checkNumber, string checkType, DateTime checkDate, decimal checkAmount)
        {
            AddLoanPaymentForm form = CreateOrRetrieve<AddLoanPaymentForm>();
            ChequesModel model = new ChequesModel();
            model.BankName = bankName;
            model.Branch = bankBranch;
            model.CheckType = checkType;
            model.CheckNumber = checkNumber;
            model.CheckDate = checkDate;
            model.Amount = checkAmount;

            form.AddCheques(model);
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }

        protected void btnDeleteCheques_Click(object sender, DirectEventArgs e)
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            SelectedRowCollection rows = this.ChequesSelectionModel.SelectedRows;
            foreach (SelectedRow row in rows)
            {
                form.RemoveCheques(row.RecordID);
            }

            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
            btnDeleteCheck.Disabled = true;
        }

        //[DirectMethod(ShowMask = true, Msg = "Opening Loan Payment Form...")]
        //public int FillPaymentFormInfo()
        //{
        //    AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
        //    form.Name = txtCustomerName.Text;
        //    form.TransactionDate = datTransactionDate.SelectedDate;
        //    form.StationNumber = txtStationNumber.Text;
        //    form.AmountTendered = decimal.Parse(txtamountTendered.Text);
        //    form.totalPrincipalDue = decimal.Parse(hiddenTotalPrincipal.Text);
        //    form.totalInterestDue = decimal.Parse(hiddenTotalInterest.Text);
        //    form.totalPastDue = decimal.Parse(hiddenTotalPastDue.Text);
        //    form.totalTotalAmountDue = decimal.Parse(hiddenTotalTotalAmountDue.Text);
        //    form.borrowerTotals = decimal.Parse(hiddenBorrowerTotals.Text);
        //    form.coBorrowerTotals = decimal.Parse(hiddenCoBorrowerTotals.Text);
        //    form.CashPayment = decimal.Parse(txtCashPayment.Text);
        //    form.CustomerId = int.Parse(hiddenCustomerID.Text);

        //    return 1;
        //}

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            AddLoanPaymentForm form = this.CreateOrRetrieve<AddLoanPaymentForm>();
            form.ProcessedByPartyRoleId = UserAccount.GetAssociatedEmployee(this.LoginInfo.UserId).PartyRoleId;
            form.CustomerId = int.Parse(hiddenCustomerID.Text);

            string borrowersJson = e.ExtraParams["Borrowers"];
            string guarantorsJson = e.ExtraParams["Guarantors"];
            SubmitHandler borrowerHandler = new SubmitHandler(borrowersJson);
            SubmitHandler guarantorHandler = new SubmitHandler(guarantorsJson);

            decimal totalAmount = 0;
            foreach (LoanPaymentModel model in borrowerHandler.Object<LoanPaymentModel>())
            {
                LoanPaymentModel toUpdate = form.RetrieveBorrower(model.LoanID);
                toUpdate.PaymentAmount = model.PaymentAmount;
                totalAmount += model.PaymentAmount;
            }

            foreach (LoanPaymentModel model in guarantorHandler.Object<LoanPaymentModel>())
            {
                LoanPaymentModel toUpdate = form.RetrieveGuaCoBorower(model.LoanID);
                toUpdate.PaymentAmount = model.PaymentAmount;
                totalAmount += model.PaymentAmount;
            }

            form.TotalAmountApplied = totalAmount;
            form.BalanceOfCashReceipts = decimal.Parse(txtBalanceCashReceipts.Text);
            form.AmountTendered = decimal.Parse(txtamountTendered.Text);
            form.CashPayment = decimal.Parse(txtCashPayment.Text);
            form.TransactionDate = datTransactionDate.SelectedDate;

            using (var unitOfWork = new UnitOfWorkScope(true))
            {
                form.PrepareForSave();
            }

            hiddenPaymentId.Value = form.ParentPayment.Id;
        }

        public void checkAmount(object sender, RemoteValidationEventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtTotalAmount.Text) == false)
                && (string.IsNullOrWhiteSpace(txtamountTendered.Text) == false))
            {
                decimal varTotalAmount = decimal.Parse(txtTotalAmount.Text);
                decimal varAmountTendered = decimal.Parse(txtamountTendered.Text);
                if (varTotalAmount > varAmountTendered)
                {
                    e.Success = false;
                    e.ErrorMessage = "Must be lesser than Amount Tendered";
                }
                else e.Success = true;
            }
        }

        protected void OnRefreshDataCollaterals(object sender, StoreRefreshDataEventArgs e)
        {
            DataSourceCollateral dataSource = new DataSourceCollateral();
            e.Total = dataSource.Count;
            //this.StoreCollaterals.DataSource = dataSource.SelectAll(e.Start, e.Limit, e.Sort);
            //this.StoreCollaterals.DataBind();
        }

        protected void RefreshCheckData(object sender, StoreRefreshDataEventArgs e)
        {
            AddLoanPaymentForm form = CreateOrRetrieve<AddLoanPaymentForm>();
            StoreCheck.DataSource = form.AvailableCheques;
            StoreCheck.DataBind();
        }
        private class CollateralsListModel
        {
            public int AssetID { get; set; }
            public string Description { get; set; }
            public string CollateralType { get; set; }
        }
        private class PaymentReceiptModel
        {
            public Payment payment { get; set; }
            public Receipt receipt { get; set; }
        }
        private class DataSourceCollateral : IPageAbleDataSource<CollateralsListModel>
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public DataSourceCollateral()
            {
                this.Name = string.Empty;
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

            private IQueryable<CollateralsListModel> CreateQuery(FinancialEntities context)
            {
                int id = this.ID;

                var query = from a in context.Assets
                            join at in context.AssetTypes on a.AssetTypeId equals at.Id
                            select new CollateralsListModel()
                            {
                                AssetID = a.Id,
                                Description = a.Description,
                                CollateralType = at.Name
                            };

                return query;
            }

            public override List<CollateralsListModel> SelectAll(int start, int limit, Func<CollateralsListModel, string> orderBy)
            {
                List<CollateralsListModel> collection;
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