using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic;
using Ext.Net;

namespace LendingApplication
{
    public class NavigationManager
    {
        private List<string> urls;
        private List<string> nodeIds;
        public TreeNode NavigationRoot { get; private set; }
        public UserAccountType Type { get; set; }
        public TreeNode HelpNavigationRoot { get; set; }

        public NavigationManager(UserAccountType type)
        {
            urls = new List<string>();
            nodeIds = new List<string>();

            this.NavigationRoot = BuildTree(type);
        }

        private TreeNode BuildTree(UserAccountType type)
        {
            TreeNode root = new TreeNode("Root", "Lending Application", Icon.ApplicationHome);
            root.Leaf = false;
            root.Expanded = true;

            TreeNode listTransactions = new TreeNode("MyTransactions", "My Transactions", Icon.ApplicationViewList) { Href = "/Applications/Reports/ListTransactions.aspx" };
            listTransactions.Leaf = true;
            if (UserTypeCanAccessById(type, listTransactions.NodeID))
                root.Nodes.Add(listTransactions);

            TreeNode loanCalculator = new TreeNode("LoanCalculator", "Loan Calculator", Icon.Calculator);
            loanCalculator.Href = "/Applications/AmortizationScheduleUseCases/GenerateAmortizationSchedule.aspx";
            loanCalculator.Leaf = true;
            if (UserTypeCanAccessById(type, loanCalculator.NodeID))
                root.Nodes.Add(loanCalculator);
            loanCalculator.Hidden = true;

            TreeNode loanProducts = new TreeNode("LoanProducts", "Loan Products", Icon.Package);
            loanProducts.Href = "/BestPractice/ListFinancialProducts.aspx";
            loanProducts.Leaf = true;
            if (UserTypeCanAccessById(type, loanProducts.NodeID))
                root.Nodes.Add(loanProducts);

            TreeNode loans = new TreeNode("LoanAccount", "Loan Accounts", Icon.Folder) { Href = "/Applications/LoanUseCases/ListLoans.aspx" };
            loans.Leaf = true;
            if (UserTypeCanAccessById(type, loans.NodeID))
                root.Nodes.Add(loans);
            
            TreeNode loanProcess = new TreeNode("LoanProcess", "Loan Process", Icon.Hourglass);
            loanProcess.Leaf = false;
            loanProcess.Expanded = true;
            if (UserTypeCanAccessById(type, loanProcess.NodeID))
                root.Nodes.Add(loanProcess);

            TreeNode customers = new TreeNode("Customers", "Customers", Icon.Group) { Href = "/Applications/CustomerUseCases/ListCustomers.aspx" };
            customers.Leaf = true;
            if (UserTypeCanAccessById(type, customers.NodeID))
                loanProcess.Nodes.Add(customers);

            TreeNode loanApplications = new TreeNode("LoanApplications", "Loan Applications", Icon.ApplicationForm) { Href = "/Applications/LoanApplicationUseCases/ListLoanApplication.aspx" };
            loanApplications.Leaf = true;
            if (UserTypeCanAccessById(type, loanApplications.NodeID))
                loanProcess.Nodes.Add(loanApplications);

            TreeNode loanDisbursements = new TreeNode("LoanDisbursements", "Disbursements", Icon.Money) { Href = "/Applications/DisbursementUseCases/ViewDisbursementList.aspx" };
            loanDisbursements.Leaf = true;
            if (UserTypeCanAccessById(type, loanDisbursements.NodeID))
                loanProcess.Nodes.Add(loanDisbursements);

            TreeNode additionalLoan = new TreeNode("AdditionalLoan", "Additional Loan", Icon.FolderAdd) { Href = "/Applications/LoanRestructureUseCases/ListAdditionalLoan.aspx" };
            additionalLoan.Leaf = true;
            if (UserTypeCanAccessById(type, additionalLoan.NodeID))
                loanProcess.Nodes.Add(additionalLoan);


            TreeNode loanRestructure = new TreeNode("LoanRestructure", "Loan Restructure", Icon.Wrench) { Href = "/Applications/LoanRestructureUseCases/ListLoanRestructure.aspx" };
            loanRestructure.Leaf = true;
            if (UserTypeCanAccessById(type, loanRestructure.NodeID))
                loanProcess.Nodes.Add(loanRestructure);

            TreeNode loanCollection = new TreeNode("LoanCollection", "Loan Collection", Icon.Mail);
            loanCollection.Leaf = false;
            loanCollection.Expanded = true;
            if (UserTypeCanAccessById(type, loanCollection.NodeID))
                root.Nodes.Add(loanCollection);

            TreeNode customersalary = new TreeNode("CustomerSalary", "Customer Salary", Icon.Vcard) { Href = "/Applications/ReceiptUseCases/AddCustomerSalary.aspx" };
            customersalary.Leaf = true;
            loanCollection.Nodes.Add(customersalary);

            TreeNode applysalary = new TreeNode("ApplyCustomerSalary", "Apply Customer Salary", Icon.Money) { Href = "/Applications/ReceiptUseCases/ApplyCustomerSalary.aspx" };
            applysalary.Leaf = true;
            if (UserTypeCanAccessById(type, applysalary.NodeID))
                loanCollection.Nodes.Add(applysalary);

            TreeNode payment = new TreeNode("Payment", "Payment", Icon.Money) { Href = "/Applications/CollectionUseCases/ListCollection.aspx" };
            payment.Leaf = true;
            if (UserTypeCanAccessById(type, payment.NodeID))
                loanCollection.Nodes.Add(payment);

            TreeNode receipts = new TreeNode("Receipts", "Receipts", Icon.Vcard) { Href = "/Applications/ReceiptUseCases/ListReceipts.aspx" };
            receipts.Leaf = true;
            loanCollection.Nodes.Add(receipts);

            TreeNode cheques = new TreeNode("Cheques", "Cheques", Icon.PageWhiteTextWidth) { Href = "/Applications/ChequeUseCases/ListCheques.aspx" };
            cheques.Leaf = true;
            if (UserTypeCanAccessById(type, cheques.NodeID))
                loanCollection.Nodes.Add(cheques);

            TreeNode chequeEditor = new TreeNode("ChequeEditor", "Cheque Editor", Icon.PageWhiteEdit) { Href = "/Applications/ChequeEditorUseCases/ListChequeEditor.aspx" };
            chequeEditor.Leaf = true;
            if (UserTypeCanAccessById(type, chequeEditor.NodeID))
                loanCollection.Nodes.Add(chequeEditor);


            //TreeNode loanDisbursementVoucher = new TreeNode("LoanDisbursementVoucher", "Loan Disbursement Voucher", Icon.Mail) { Href = "/Applications/LoanDisbursementVoucherUseCases/ViewDisbursementVoucherList.aspx" };
            //loanDisbursementVoucher.Leaf = true;
            //if (UserTypeCanAccessById(type, loanDisbursementVoucher.NodeID))
            //    root.Nodes.Add(loanDisbursementVoucher);

            TreeNode billing = new TreeNode("Billing", "Billing", Icon.Coins) { Href = "/Applications/BackgroundUseCases/GenerateBill.aspx" };
            billing.Leaf = true;
            if (UserTypeCanAccessById(type, billing.NodeID))
                root.Nodes.Add(billing);

            //TreeNode postDatedCheques = new TreeNode("PostDatedCheques", "Post Dated Cheques", Icon.PageWhiteTextWidth) { Href = "/Applications/PostDatedChequeUseCases/ListPostDatedCheques.aspx" };
            //postDatedCheques.Leaf = true;
            //if (UserTypeCanAccessById(type, postDatedCheques.NodeID))
            //    root.Nodes.Add(postDatedCheques);

            TreeNode primarySettings = new TreeNode("PrimarySettings", "Settings", Icon.Cog);
            primarySettings.Leaf = false;
            primarySettings.Expanded = false;
            if (UserTypeCanAccessById(type, primarySettings.NodeID))
                root.Nodes.Add(primarySettings);

            TreeNode employee = new TreeNode("Employee", "Employees", Icon.UserSuit) { Href = "/Applications/EmployeeUseCases/ListEmployee.aspx" };
            employee.Leaf = true;
            if (UserTypeCanAccessById(type, employee.NodeID))
                primarySettings.Nodes.Add(employee);

            TreeNode contact = new TreeNode("Contact", "Contacts", Icon.Telephone) { Href = "/Applications/ContactUseCases/ListContact.aspx" };
            contact.Leaf = true;
            if (UserTypeCanAccessById(type, contact.NodeID))
                primarySettings.Nodes.Add(contact);

            TreeNode banks = new TreeNode("Banks", "Banks", Icon.House) { Href = "/Applications/BankUseCases/ListBank.aspx" };
            banks.Leaf = true;
            if (UserTypeCanAccessById(type, banks.NodeID))
                primarySettings.Nodes.Add(banks);

            TreeNode userAccount = new TreeNode("UserAccount", "User Accounts", Icon.UserGray) { Href = "/Applications/UserAccountsUseCases/ListUserAccounts.aspx" };
            userAccount.Leaf = true;
            if (UserTypeCanAccessById(type, userAccount.NodeID))
                primarySettings.Nodes.Add(userAccount);

            TreeNode holidays = new TreeNode("Holidays", "Holidays", Icon.Calendar) { Href = "/Applications/HolidayUseCases/ListHoliday.aspx" };
            holidays.Leaf = true;
            holidays.Hidden = true;
            if (UserTypeCanAccessById(type, holidays.NodeID))
                primarySettings.Nodes.Add(holidays);

            TreeNode cashOnVault = new TreeNode("CashOnVault", "Cash On Vault", Icon.CoinsAdd) { Href = "/Applications/CashOnVaultUseCases/ListCashOnVault.aspx" };
            cashOnVault.Leaf = true;
            if (UserTypeCanAccessById(type, cashOnVault.NodeID))
                primarySettings.Nodes.Add(cashOnVault);

            TreeNode systemSettings = new TreeNode("SystemSettings", "System Settings", Icon.Cog) { Href = "/Applications/SystemSettingsUseCases/ListSystemSettings.aspx" };
            systemSettings.Leaf = true;
            if (UserTypeCanAccessById(type, systemSettings.NodeID))
                primarySettings.Nodes.Add(systemSettings);

            TreeNode types = new TreeNode("Types", "Types", Icon.Cog);
            types.Leaf = false;
            types.Expanded = false;
            if (UserTypeCanAccessById(type, types.NodeID))
                primarySettings.Nodes.Add(types);

            TreeNode customerClassification = new TreeNode("CustomerClassification", "Customer Classifications", Icon.GroupLink) { Href = "/Applications/CustomerClassificationUseCases/ListCustomerClassification.aspx" };
            customerClassification.Leaf = true;
            if (UserTypeCanAccessById(type, customerClassification.NodeID))
                types.Nodes.Add(customerClassification);

            TreeNode requiredDocumentType = new TreeNode("RequiredDocumentType", "Required Document Types", Icon.PageCopy) { Href = "/Applications/RequiredDocumentTypeUseCases/ListRequiredDocumentTypes.aspx" };
            requiredDocumentType.Leaf = true;
            if (UserTypeCanAccessById(type, requiredDocumentType.NodeID))
                types.Nodes.Add(requiredDocumentType);

            TreeNode customerTypeSetting = new TreeNode("CustomerType", "Customer Types", Icon.UserStar) { Href = "/Applications/CustomerClassificationUseCases/CustomerType.aspx" };
            customerTypeSetting.Leaf = true;
            if (UserTypeCanAccessById(type, customerTypeSetting.NodeID))
                types.Nodes.Add(customerTypeSetting);

           



            TreeNode reports = new TreeNode("Reports", "Reports", Icon.Report);
            reports.Leaf = false;
            reports.Expanded = false;
            if (UserTypeCanAccessById(type, reports.NodeID))
                root.Nodes.Add(reports);

              TreeNode incomeStatementReport = new TreeNode("IncomeStatementReport", "Income Statement", Icon.Money) { Href = "/Applications/Reports/IncomeStatementReport.aspx" };
            incomeStatementReport.Leaf = true;
            if (UserTypeCanAccessById(type, incomeStatementReport.NodeID))
                reports.Nodes.Add(incomeStatementReport);

            TreeNode dailyReceivedAndReleasedReport = new TreeNode("DailyReceivedAndReleasedReport", "Daily Transaction Report", Icon.Script) { Href = "/Applications/Reports/DailyReceivedAndReleasedReport.aspx" };
            dailyReceivedAndReleasedReport.Leaf = true;
            if (UserTypeCanAccessById(type, dailyReceivedAndReleasedReport.NodeID))
                reports.Nodes.Add(dailyReceivedAndReleasedReport);

            TreeNode transactionReport = new TreeNode("TransactionReport", "Transaction Report", Icon.Script) { Href = "/Applications/Reports/TransactionReport.aspx" };
            transactionReport.Leaf = true;
            if (UserTypeCanAccessById(type, transactionReport.NodeID))
                reports.Nodes.Add(transactionReport);

            TreeNode dailyChecksReport = new TreeNode("DailyChecksReport", "Daily Checks Report", Icon.PageWhite) { Href = "/Applications/Reports/DailyChequesReport.aspx" };
            dailyChecksReport.Leaf = true;
            if (UserTypeCanAccessById(type, dailyChecksReport.NodeID))
                reports.Nodes.Add(dailyChecksReport);

            TreeNode postDatedChecksReceivedReport = new TreeNode("PostDatedChecksReport", "Post Dated Checks Report", Icon.PageWhiteCopy) { Href = "/Applications/Reports/PostDatedChecksReport.aspx" };
            postDatedChecksReceivedReport.Leaf = true;
            if (UserTypeCanAccessById(type, postDatedChecksReceivedReport.NodeID))
                reports.Nodes.Add(postDatedChecksReceivedReport);

            TreeNode teachersChecksReceivedReport = new TreeNode("TeachersChecksReceived", "Teacher's Checks Report", Icon.PageWhiteEdit) { Href = "/Applications/Reports/TeachersChequesReceivedReport.aspx" };
            teachersChecksReceivedReport.Leaf = true;
            if (UserTypeCanAccessById(type, teachersChecksReceivedReport.NodeID))
                reports.Nodes.Add(teachersChecksReceivedReport);

        

            TreeNode demandLetter = new TreeNode("DemandLetter", "Demand Letter", Icon.PageWhiteText) { Href = "/Applications/DemandLetterUseCases/DemandLetterList.aspx" };
            demandLetter.Leaf = true;
            if (UserTypeCanAccessById(type, demandLetter.NodeID))
                reports.Nodes.Add(demandLetter);

            TreeNode badDebts = new TreeNode("BadDebts", "Bad Debts", Icon.LayoutError) { Href = "/Applications/Reports/BadDebtsReport.aspx" };
            badDebts.Leaf = true;
            if (UserTypeCanAccessById(type, badDebts.NodeID))
                reports.Nodes.Add(badDebts);

            //TreeNode billStatement = new TreeNode("BillStatement", "Bill Statement", Icon.PageWhiteText) { Href = "/Applications/BackgroundUseCases/CustomersWithNewBill.aspx" };
            //billStatement.Leaf = true;
            //if (UserTypeCanAccessById(type, billStatement.NodeID))
            //    reports.Nodes.Add(billStatement);

            TreeNode outstandingLoan = new TreeNode("OutstandingLoanReport", "Outstanding Loan Report", Icon.PageWhiteTextWidth) { Href = "/Applications/Reports/OutstandingLoansReport.aspx" };
            outstandingLoan.Leaf = true;
            outstandingLoan.Qtip = "Monthly Report on Outstanding Loans, Loans Granted and Payments";
            outstandingLoan.Hidden = true;
            if (UserTypeCanAccessById(type, outstandingLoan.NodeID))
                reports.Nodes.Add(outstandingLoan);

            TreeNode paidOffLoans = new TreeNode("PaidOffLoans", "Summary Of Paid Off Loans", Icon.Table) { Href = "/Applications/Reports/SummaryOfPaidOffLoans.aspx" };
            paidOffLoans.Leaf = true;
            if (UserTypeCanAccessById(type, paidOffLoans.NodeID))
                reports.Nodes.Add(paidOffLoans);

            TreeNode summaryOfLoansGranted = new TreeNode("SummaryOfLoansGranted", "Summary of Loans Granted", Icon.PageWhiteDatabase) { Href = "/Applications/Reports/SummaryOfLoansGranted.aspx" };
            summaryOfLoansGranted.Leaf = true;
            if (UserTypeCanAccessById(type, summaryOfLoansGranted.NodeID))
                reports.Nodes.Add(summaryOfLoansGranted);

            TreeNode scheduleOfOutstandingLoans = new TreeNode("ScheduleOfOutstandingLoans", "Outstanding Loans Schedule", Icon.CalendarViewDay) { Href = "/Applications/Reports/ScheduleOfOutstandingLoans.aspx" };
            scheduleOfOutstandingLoans.Leaf = true;
            if (UserTypeCanAccessById(type, scheduleOfOutstandingLoans.NodeID))
                reports.Nodes.Add(scheduleOfOutstandingLoans);

            TreeNode agingOfAccounts = new TreeNode("AgingOfAccounts", "Aging of Accounts", Icon.TimeGo) { Href = "/Applications/AgingOfAccountsUseCases/ListAgingOfAccounts.aspx" };
            agingOfAccounts.Leaf = true;
            if (UserTypeCanAccessById(type, agingOfAccounts.NodeID))
                reports.Nodes.Add(agingOfAccounts);

            //TreeNode dailyChecksReceivedReport = new TreeNode("DailyChecksReceived", "Daily Checks Received Report", Icon.PageWhite) { Href = "/Applications/Reports/DailyChequesReceivedReport.aspx" };
            //dailyChecksReceivedReport.Leaf = true;
            //if (UserTypeCanAccessById(type, dailyChecksReceivedReport.NodeID))
            //    reports.Nodes.Add(dailyChecksReceivedReport);

           

            TreeNode foreignExhcange = new TreeNode("ForeignExchange", "Foreign Transactions", Icon.MoneyDollar);
            reports.Leaf = false;
            reports.Expanded = false;
            if (UserTypeCanAccessById(type, foreignExhcange.NodeID))
                root.Nodes.Add(foreignExhcange);
            TreeNode currency = new TreeNode("Currency", "Currency", Icon.MoneyEuro) { Href = "/ForeignExchangeApplication/CurrencyUseCases/ListCurrency.aspx" };
            currency.Leaf = true;
            if (UserTypeCanAccessById(type, currency.NodeID))
                foreignExhcange.Nodes.Add(currency);

            TreeNode exchangeRates = new TreeNode("ExchangeRates", "Exchange Rates", Icon.MoneyYen) { Href = "/ForeignExchangeApplication/ExchangeRateUseCases/ListExchangeRates.aspx" };
            exchangeRates.Leaf = true;
            if (UserTypeCanAccessById(type, exchangeRates.NodeID))
                foreignExhcange.Nodes.Add(exchangeRates);

            TreeNode foreignExchangeTransactions = new TreeNode("ForeignExchangeTransactions", "Foreign Exchange", Icon.MoneyPound) { Href = "/ForeignExchangeApplication/ForExTransactionUseCases/ListForExTransactions.aspx" };
            foreignExchangeTransactions.Leaf = true;
            if (UserTypeCanAccessById(type, foreignExchangeTransactions.NodeID))
                foreignExhcange.Nodes.Add(foreignExchangeTransactions);

            TreeNode foreignDisbursements = new TreeNode("ForeignDisbursements", "Foreign Disbursements", Icon.Money) { Href = "/ForeignExchangeApplication/ForeignDisbursementUseCases/ForeignDisbursements.aspx" };
            foreignDisbursements.Leaf = true;
            if (UserTypeCanAccessById(type, foreignDisbursements.NodeID))
                foreignExhcange.Nodes.Add(foreignDisbursements);

   
            TreeNode forExReport = new TreeNode("ForeignExchangeReport", "Foreign Exchange Report", Icon.Report) { Href = "/ForeignExchangeApplication/ForExTransactionUseCases/ForExReport.aspx" };
            forExReport.Leaf = true;
            if (UserTypeCanAccessById(type, forExReport.NodeID))
                foreignExhcange.Nodes.Add(forExReport);

            return root;
        }

        public static TreeNode BuildHelpTree()
        {
            TreeNode root = new TreeNode("Root", "Help Index", Icon.Help);
            root.Leaf = false;
            root.Expanded = true;

            TreeNode loanProducts = new TreeNode("ProductsHelp", "Loan Products", Icon.BulletBlack);
            loanProducts.Leaf = false;
            loanProducts.Expanded = false;
            root.Nodes.Add(loanProducts);

            TreeNode useLoanProductsNode = new TreeNode("HowToUseLoanProductsNode", "How To Use Loan Products Node", Icon.BulletWhite);
            useLoanProductsNode.Href = "/MNPamaranHowTo/HowToUseLoanProductsNode.pdf";
            useLoanProductsNode.Leaf = true;
            loanProducts.Nodes.Add(useLoanProductsNode);

            TreeNode customer = new TreeNode("CustomerHelp", "Customer", Icon.BulletBlack);
            customer.Leaf = false;
            customer.Expanded = false;
            root.Nodes.Add(customer);

            TreeNode customerCreate = new TreeNode("HowToCreateCustomer", "How To Create Customer", Icon.BulletWhite);
            customerCreate.Href = "/MNPamaranHowTo/Customer/HowToCreateCustomer.pdf";
            customerCreate.Leaf = true;
            customer.Nodes.Add(customerCreate);

            TreeNode customerEditDelete = new TreeNode("HowToEditDeleteCustomer", "How To Edit & Delete Customer", Icon.BulletWhite);
            customerEditDelete.Href = "/MNPamaranHowTo/Customer/HowToEditDeleteCustomer.pdf";
            customerEditDelete.Leaf = true;
            customer.Nodes.Add(customerEditDelete);


            TreeNode loanApplications = new TreeNode("LoanApplicationsHelp", "Loan Application", Icon.BulletBlack);
            loanApplications.Leaf = false;
            loanApplications.Expanded = false;
            root.Nodes.Add(loanApplications);

            TreeNode loanApplicationCreateNoTerm = new TreeNode("HowToCreateLoanApplication(NoTerm)", "How To Create Loan Application (No Term)", Icon.BulletWhite);
            loanApplicationCreateNoTerm.Href = "/MNPamaranHowTo/LoanApplication/HowToCreateLoanApplication(NoTerm).pdf";
            loanApplicationCreateNoTerm.Leaf = true;
            loanApplications.Nodes.Add(loanApplicationCreateNoTerm);

            TreeNode loanApplicationCreateWithTerm = new TreeNode("HowToCreateLoanApplication(withTerm)", "How To Create Loan Application (With Term)", Icon.BulletWhite);
            loanApplicationCreateWithTerm.Href = "/MNPamaranHowTo/LoanApplication/HowToCreateLoanApplication(withTerm).pdf";
            loanApplicationCreateWithTerm.Leaf = true;
            loanApplications.Nodes.Add(loanApplicationCreateWithTerm);

            TreeNode loanApplicationEdit = new TreeNode("HowToEditLoanApplication", "How To Edit Loan Application", Icon.BulletWhite);
            loanApplicationEdit.Href = "/MNPamaranHowTo/LoanApplication/HowToEditLoanApplication.pdf";
            loanApplicationEdit.Leaf = true;
            loanApplications.Nodes.Add(loanApplicationEdit);

            TreeNode loanApplicationList = new TreeNode("HowToUseLoanApplication", "How To Use Loan Application List", Icon.BulletWhite);
            loanApplicationList.Href = "/MNPamaranHowTo/LoanApplication/HowToUseLoanApplication.pdf";
            loanApplicationList.Leaf = true;
            loanApplications.Nodes.Add(loanApplicationList);

            TreeNode disbursements = new TreeNode("DisbursementsHelp", "Disbursement", Icon.BulletBlack);
            disbursements.Leaf = false;
            disbursements.Expanded = false;
            root.Nodes.Add(disbursements);

            TreeNode loanDisbursementAdd = new TreeNode("HowToAddLoanDisbursement", "How To Add Loan Disbursement", Icon.BulletWhite);
            loanDisbursementAdd.Href = "/MNPamaranHowTo/Disbursements/HowToAddLoanDisbursement.pdf";
            loanDisbursementAdd.Leaf = true;
            disbursements.Nodes.Add(loanDisbursementAdd);

            TreeNode additionalLoans = new TreeNode("AdditionalLoanHelp", "Additional Loan", Icon.BulletBlack);
            additionalLoans.Leaf = false;
            additionalLoans.Expanded = false;
            root.Nodes.Add(additionalLoans);

            TreeNode additionalLoanWithoutTerm = new TreeNode("AdditionalLoanWithoutTerm", "How To Create Additional Loan (Without Term)", Icon.BulletWhite);
            additionalLoanWithoutTerm.Href = "/MNPamaranHowTo/AdditionalLoan/AdditionalLoanWithoutTerm.pdf";
            additionalLoanWithoutTerm.Leaf = true;
            additionalLoans.Nodes.Add(additionalLoanWithoutTerm);

            TreeNode additionalLoanWithTerm = new TreeNode("AdditionalLoanWithTerm", "How To Create Additional Loan (With Term)", Icon.BulletWhite);
            additionalLoanWithTerm.Href = "/MNPamaranHowTo/AdditionalLoan/AdditionalLoanWithTerm.pdf";
            additionalLoanWithTerm.Leaf = true;
            additionalLoans.Nodes.Add(additionalLoanWithTerm);

            TreeNode advanceChange = new TreeNode("AdvanceChangeHelp", "Advance Change", Icon.BulletBlack);
            advanceChange.Leaf = false;
            advanceChange.Expanded = false;
            root.Nodes.Add(advanceChange);

            TreeNode advanceChangesCreate = new TreeNode("CreateAdvanceChange", "How To Create Advance Change", Icon.BulletWhite);
            advanceChangesCreate.Href = "/MNPamaranHowTo/AdvanceChange/AdvanceChange.pdf";
            advanceChangesCreate.Leaf = true;
            advanceChange.Nodes.Add(advanceChangesCreate);


            TreeNode loanRestructure = new TreeNode("LoanRestructureHelp", "Loan Restructure", Icon.BulletBlack);
            loanRestructure.Leaf = false;
            loanRestructure.Expanded = false;
            root.Nodes.Add(loanRestructure);

            TreeNode changeIcm = new TreeNode("ChangeIcmHelp", "How To Restructure A Loan (Change ICM)", Icon.BulletWhite);
            changeIcm.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureChangeICM.pdf";
            changeIcm.Leaf = true;
            loanRestructure.Nodes.Add(changeIcm);

            TreeNode changeInterest = new TreeNode("ChangeInterestsHelp", "How To Restructure A Loan (Change Interest - Percentage)", Icon.BulletWhite);
            changeInterest.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureChangeInterest.pdf";
            changeInterest.Leaf = true;
            loanRestructure.Nodes.Add(changeInterest);

            TreeNode changeInterestFixed = new TreeNode("ChangeInterestFixed", "How To Restructure A Loan (Change Interest - Fixed)", Icon.BulletWhite);
            changeInterestFixed.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureChangeInterestFixed.pdf";
            changeInterestFixed.Leaf = true;
            loanRestructure.Nodes.Add(changeInterestFixed);

            TreeNode changeInterestZero = new TreeNode("ChangeInterestZero", "How To Restructure A Loan (Change Interest - Zero)", Icon.BulletWhite);
            changeInterestZero.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureChangeInterestZero.pdf";
            changeInterestZero.Leaf = true;
            loanRestructure.Nodes.Add(changeInterestZero);

            TreeNode splitLoan = new TreeNode("LoanSplitHelp", "How To Split Loans", Icon.BulletWhite);
            splitLoan.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureSplitLoan.pdf";
            splitLoan.Leaf = true;
            loanRestructure.Nodes.Add(splitLoan);

            TreeNode consolidateLoan = new TreeNode("LoanConsolidateHelp", "How To Consolidate Loans", Icon.BulletWhite);
            consolidateLoan.Href = "/MNPamaranHowTo/LoanRestructure/LoanRestructureConsolidateLoans.pdf";
            consolidateLoan.Leaf = true;
            loanRestructure.Nodes.Add(consolidateLoan);

            TreeNode payments = new TreeNode("PaymentsHelp", "Payment", Icon.BulletBlack);
            payments.Leaf = false;
            payments.Expanded = false;
            root.Nodes.Add(payments);

            TreeNode addLoanPaymentCash = new TreeNode("HowToAddLoanPaymentUsingCash", "How To Add Loan Payment Using Cash", Icon.BulletWhite);
            addLoanPaymentCash.Href = "/MNPamaranHowTo/Payments/HowToAddLoanPaymentUsingCash.pdf";
            addLoanPaymentCash.Leaf = true;
            payments.Nodes.Add(addLoanPaymentCash);

            TreeNode addLoanPaymentCheck = new TreeNode("HowToAddLoanPaymentUsingCheck", "How To Add Loan Payment Using Check", Icon.BulletWhite);
            addLoanPaymentCheck.Href = "/MNPamaranHowTo/Payments/HowToAddLoanPaymentUsingCheck.pdf";
            addLoanPaymentCheck.Leaf = true;
            payments.Nodes.Add(addLoanPaymentCheck);

            TreeNode addLoanPaymentATM = new TreeNode("HowToAddLoanPaymentUsingATM", "How To Add Loan Payment Using ATM", Icon.BulletWhite);
            addLoanPaymentATM.Href = "/MNPamaranHowTo/Payments/HowToAddLoanPaymentUsingATM.pdf";
            addLoanPaymentATM.Leaf = true;
            payments.Nodes.Add(addLoanPaymentATM);

            TreeNode addLoanPaymentCashCheckATM = new TreeNode("HowToAddLoanPaymentCashCheckATM", "How To Add Loan Payment Using Cash, Check and ATM", Icon.BulletWhite);
            addLoanPaymentCashCheckATM.Href = "/MNPamaranHowTo/Payments/HowToAddLoanPaymentUsingCashCheckATM.pdf";
            addLoanPaymentCashCheckATM.Leaf = true;
            payments.Nodes.Add(addLoanPaymentCashCheckATM);

            TreeNode generateInterest = new TreeNode("HowToGenerateInterest", "How To Generate Interest", Icon.BulletWhite);
            generateInterest.Href = "/MNPamaranHowTo/Payments/HowToGenerateInterest.pdf";
            generateInterest.Leaf = true;
            payments.Nodes.Add(generateInterest);

            TreeNode applyLoanRebate = new TreeNode("HowToApplyLoanRebate", "How To Apply Loan Rebate", Icon.BulletWhite);
            applyLoanRebate.Href = "/MNPamaranHowTo/Payments/HowToApplyLoanRebate.pdf";
            applyLoanRebate.Leaf = true;
            payments.Nodes.Add(applyLoanRebate);

            TreeNode waiveLoan = new TreeNode("HowToWaiveLoan", "How To Waive Loan", Icon.BulletWhite);
            waiveLoan.Href = "/MNPamaranHowTo/Payments/HowToWaiveLoan.pdf";
            waiveLoan.Leaf = true;
            payments.Nodes.Add(waiveLoan);

            TreeNode settings = new TreeNode("SettingsHelp", "Settings", Icon.BulletBlack);
            settings.Leaf = false;
            settings.Expanded = false;
            root.Nodes.Add(settings);

            TreeNode customerClassification = new TreeNode("HelpCustomersClassification", "How To Use Customer Classification Node", Icon.BulletWhite);
            customerClassification.Href = "/MNPamaranHowTo/Settings/HowToUseCustomerClassificationNode.pdf";
            customerClassification.Leaf = true;
            settings.Nodes.Add(customerClassification);
            customerClassification.Hidden = true;

            TreeNode requiredDocumentType = new TreeNode("RequiredDocumentsHelp", "How To Use Required Document Type Node", Icon.BulletWhite);
            requiredDocumentType.Href = "/MNPamaranHowTo/Settings/HowToUseRequiredDocumentTypeNode.pdf";
            requiredDocumentType.Leaf = true;
            settings.Nodes.Add(requiredDocumentType);

            TreeNode systemSettings = new TreeNode("HelpSystemSettings", "How To Use System Settings Node", Icon.BulletWhite);
            systemSettings.Href = "/MNPamaranHowTo/Settings/HowToUseSystemSettingsNode.pdf";
            systemSettings.Leaf = true;
            settings.Nodes.Add(systemSettings);

            TreeNode bankNode = new TreeNode("HowToUseBankNode", "How to use Bank Node", Icon.BulletWhite);
            bankNode.Href = "/MNPamaranHowTo/Settings/HowToUseBankNode.pdf";
            bankNode.Leaf = true;
            settings.Nodes.Add(bankNode);
            bankNode.Hidden = true;

            TreeNode useHolidaysNode = new TreeNode("UseHolidaysNode", "How To Use Holidays Node", Icon.BulletWhite);
            useHolidaysNode.Href = "/MNPamaranHowTo/Settings/HowToUseHolidaysNode.pdf";
            useHolidaysNode.Leaf = true;
            settings.Nodes.Add(useHolidaysNode);

            TreeNode useContactsNode = new TreeNode("UseContactsNode", "How To Use Contacts Node", Icon.BulletWhite);
            useContactsNode.Href = "/MNPamaranHowTo/Settings/HowToUseContactsNode.pdf";
            useContactsNode.Leaf = true;
            settings.Nodes.Add(useContactsNode);

            TreeNode useUserAccountNode = new TreeNode("UseUserAccountNode", "How To Use User Account Node", Icon.BulletWhite);
            useUserAccountNode.Href = "/MNPamaranHowTo/Settings/HowToUseUserAccountNode.pdf";
            useUserAccountNode.Leaf = true;
            settings.Nodes.Add(useUserAccountNode);

            TreeNode useEmployeesNode = new TreeNode("UseEmployeesNode", "How To Use Employees Node", Icon.BulletWhite);
            useEmployeesNode.Href = "/MNPamaranHowTo/Settings/HowToUseEmployeesNode.pdf";
            useEmployeesNode.Leaf = true;
            settings.Nodes.Add(useEmployeesNode);

            TreeNode miscellaneous = new TreeNode("MiscellaneousHelp", "Miscellaneous", Icon.BulletBlack);
            miscellaneous.Leaf = false;
            miscellaneous.Expanded = false;
            root.Nodes.Add(miscellaneous);

            TreeNode fillUpName = new TreeNode("FillUpName", "How To Fill Up A Name", Icon.BulletWhite);
            fillUpName.Href = "/MNPamaranHowTo/Miscellaneous/HowToFillUpAName.pdf";
            fillUpName.Leaf = true;
            miscellaneous.Nodes.Add(fillUpName);

            TreeNode setHomePage = new TreeNode("SetHomePage", "How To Set Your Home Page", Icon.BulletWhite);
            setHomePage.Href = "/MNPamaranHowTo/Miscellaneous/HowToSetYourHomePage.pdf";
            setHomePage.Leaf = true;
            miscellaneous.Nodes.Add(setHomePage);

            TreeNode uploadImage = new TreeNode("UploadImage", "How To Upload Image", Icon.BulletWhite);
            uploadImage.Href = "/MNPamaranHowTo/Miscellaneous/HowToUploadImage.pdf";
            uploadImage.Leaf = true;
            miscellaneous.Nodes.Add(uploadImage);

            TreeNode useSearchFilter = new TreeNode("UseSearchFilter", "How To Use Search & Filter", Icon.BulletWhite);
            useSearchFilter.Href = "/MNPamaranHowTo/Miscellaneous/HowToUseSearchFilter.pdf";
            useSearchFilter.Leaf = true;
            miscellaneous.Nodes.Add(useSearchFilter);

            return root;
        }

        public bool UserTypeCanAccessById(UserAccountType type, string nodeId)
        {
            bool canAccess = true;
            if (type.Id == UserAccountType.Accountant.Id)
            {
                if (
                        //nodeId == "LoanDisbursements"
                        //|| nodeId == "Payment"
                        nodeId == "Employee"
                        || nodeId == "UserAccount"
                        || nodeId == "CustomerClassification"
                        || nodeId == "RequiredDocumentType"
                        || nodeId == "SystemSettings"
                        || nodeId == "CashOnVault"
                        || nodeId == "Types"
                        || nodeId == "IncomeStatementReport"
                        || nodeId == "Currency"
                        || nodeId == "ExchangeRates"
                        || nodeId == "AgingOfAccounts"
                        //|| nodeId == "TransactionReport"
                        //|| nodeId == "DailyReceivedAndReleasedReport"
                        //|| nodeId == "DailyChecksReceived"
                        //|| nodeId == "ChecksReceivedReport"
                        //|| nodeId == "CashOnVault"
                ) canAccess = false;
            }
            else if (type.Id == UserAccountType.Teller.Id)
            {
                if (
                        //nodeId == "LoanProducts"
                        //|| nodeId == "LoanCalculator"
                        //|| nodeId == "LoanApplications"
                        //|| nodeId == "LoanRestructure"
                        //|| nodeId == "AdditionalLoan"
                        nodeId == "Employee"
                        || nodeId == "CustomerClassification"
                        || nodeId == "RequiredDocumentType"
                        || nodeId == "UserAccount"
                        || nodeId == "SystemSettings"
                        || nodeId == "CashOnVault"
                        || nodeId == "Types"
                        || nodeId == "IncomeStatementReport"
                        || nodeId == "Currency"
                        || nodeId == "ExchangeRates"
                        || nodeId == "AgingOfAccounts"
                        //|| nodeId == "DemandLetter"
                        //|| nodeId == "CashOnVault"
                        //|| nodeId == "Cheques"
                        //|| nodeId == "ChequeEditor"
                        //|| nodeId == "Receipts"
                    ) canAccess = false;
            }
            else if (type.Id == UserAccountType.Admin.Id)
            {
                if (
                    //nodeId == "LoanProducts"
                    //|| nodeId == "LoanCalculator"
                    //|| nodeId == "LoanApplications"
                    //|| nodeId == "LoanRestructure"
                    //|| nodeId == "AdditionalLoan"
                      nodeId == "IncomeStatementReport"
                    //|| nodeId == "DemandLetter"
                    //|| nodeId == "Payment"
                    //|| nodeId == "CashOnVault"
                ) canAccess = false;
            }

            return canAccess;
        }

        public bool UserTypeCanAccessByUrl(string url)
        {
            return urls.Contains(url);
        }

    }
}