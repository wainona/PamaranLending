<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyLoanApplication.aspx.cs"
    Inherits="LendingApplication.Applications.ModifyLoanApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Modify Loan Application</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
          .ext-hide-mask .ext-el-mask
        {
            opacity: 0.30;
        }
    </style>
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.first = true;
            window.IsSaveBeforeApproval = false;
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickcustomer', 'onpickbank', 'onpickfinancialproduct', 'addcheque', 'onpickinteretrate', 'onpickpastduerate', 'onpickapplicationfee', 'onpickguarantor', 'onpickcoborrower', 'managecollateral', 'addrequireddocument', 'approvedloanapplication', 'closeapproved']);
            window.proxy.on('messagereceived', onMessageReceived);

            window.collateral = [];
            window.collateral['Bank Account'] = {};
            window.collateral['Bank Account'].url = '/Applications/LoanApplicationUseCases/ManageCollateralBankAccount.aspx';
            window.collateral['Bank Account'].tabid = 'ManageBankAccount';
            window.collateral['Bank Account'].tabtitle = 'Manage Bank Account';

            window.collateral['Jewelry/Others'] = {};
            window.collateral['Jewelry/Others'].url = '/Applications/LoanApplicationUseCases/ManageCollateralJewelry.aspx';
            window.collateral['Jewelry/Others'].tabid = 'ManageJewelry';
            window.collateral['Jewelry/Others'].tabtitle = 'Manage Jewelry';

            window.collateral['Land'] = {};
            window.collateral['Land'].url = '/Applications/LoanApplicationUseCases/ManageCollateralLand.aspx';
            window.collateral['Land'].tabid = 'ManageLand';
            window.collateral['Land'].tabtitle = 'Manage Land';

            window.collateral['Machine'] = {};
            window.collateral['Machine'].url = '/Applications/LoanApplicationUseCases/ManageCollateralMachine.aspx';
            window.collateral['Machine'].tabid = 'ManageMachine';
            window.collateral['Machine'].tabtitle = 'Manage Machine';

            window.collateral['Vehicle'] = {};
            window.collateral['Vehicle'].url = '/Applications/LoanApplicationUseCases/ManageCollateralVehicle.aspx';
            window.collateral['Vehicle'].tabid = 'ManageVehicle';
            window.collateral['Vehicle'].tabtitle = 'Manage Vehicle';

            maskUnmask(pnlBorrowersInformation, false);
            maskUnmask(pnlLoanApplicationDetails, false);
            maskUnmask(pnlOutstandingLoans, false);
            maskUnmask(pnlFeeAndCoBorrower, false);
            maskUnmask(pnlGuarantorAndCollaterals, false);
            maskUnmask(PanelCheque, false);

            if (chkPayOutstandingLoan.getValue() == false)
                RowSelectionOutstandingLoans.lock();

            termSettings();
        });

        var onChkPayOutstandingLoan = function () {
            if (chkPayOutstandingLoan.getValue() == false) {
                RowSelectionOutstandingLoans.clearSelections();
                RowSelectionOutstandingLoans.lock();
            }
            else {
                RowSelectionOutstandingLoans.selectAll();
                btnSave.disable();
                RowSelectionOutstandingLoans.unlock();
            }
        };

        var openOrEdit = function () {

            var enable = btnOpen.getText() == 'Edit';

            maskUnmask(pnlBorrowersInformation, enable);
            maskUnmask(pnlLoanApplicationDetails, enable);
            maskUnmask(pnlOutstandingLoans, enable);
            maskUnmask(pnlFeeAndCoBorrower, enable);
            maskUnmask(pnlGuarantorAndCollaterals, enable);
            maskUnmask(PanelCheque, enable);

            if (enable) {
                btnOpen.setText('Open');
            }
            else {
                btnOpen.setText('Edit');
            }
        };

        var maskUnmask = function (panel, enable) {
            if (enable == true) {
                panel.getEl().unmask();
                panel.removeClass('ext-hide-mask');
            } else if (enable == false) {
                panel.getEl().mask();
                panel.addClass('ext-hide-mask');

                if (window.first) {
                    for (count = 0; count < 8; count++) {
                        PageTabPanel.setActiveTab(count);

                        if (count == 7) {
                            PageTabPanel.setActiveTab(0);
                        }
                    }

                    window.first = false;
                }
            }

        };

        var setBtnApprove = function (statusFrom, statusTo) {
            if (txtLoanApplicationStatus.getValue() == 'Pending: Approval') {
                btnApprove.enable()
            }
            else {
                btnApprove.disable();
            }
        }

        var onFormValidated = function (valid) {
            var enable = btnOpen.getText() == 'Open';
            var term = nfLoanTerm.getValue();
            var collateralType = cmbCollateralRequirement.lastSelectionText;

            var loanterm = nfLoanTerm.getValue();
            if (chkPayOutstandingLoan.getValue())
                valid = valid && (RowSelectionOutstandingLoans.getSelections().length > 0);
            if ((loanterm > 0) && (chckCheck.getValue() == false))
                valid = valid && (grdPnlAmortizationSchedule.store.getCount() > 0) && (grdpnlCheque.store.getCount() == 0);
            else if ((loanterm > 0) && (chckCheck.getValue() == true))
                valid = valid && (grdPnlAmortizationSchedule.store.getCount() > 0) && (grdpnlCheque.store.getCount() > 0);
            if (loanterm > 0)
                valid = valid && grdPnlAmortizationSchedule.store.getCount() == loanterm;

            if (collateralType == "Secured") {
                var collaterals = GridPanelCollaterals.store.getCount();
                valid = valid && collaterals != 0;
            }

            var date = hdnOnChangeDates.getValue();
            valid = valid && (date == '0');

            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                if (enable) {
                    btnPrintSchedule.enable();
                    btnSave.enable();
                    checkGenerate();
                    if (hdnTermOption.getValue() == 'No Term') {
                        btnPrintSchedule.disable();
                        btnGenerate.disable();
                    }
                }
                setBtnApprove();
//                if (chckWithoutTerm.getValue() == true) {
//                    btnPrintSchedule.disable();
//                    btnGenerate.disable();
//                }
            }
            else if (chkPayOutstandingLoan.getValue() && RowSelectionOutstandingLoans.getSelections().length == 0) {
                PageFormPanelStatusBar.setStatus({ text: 'No selected outstanding loan.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
                btnPrintSchedule.disable();
            }
            else if ((loanterm > 0) && (grdPnlAmortizationSchedule.store.getCount() == 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'No generated amortization schedule.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
                btnPrintSchedule.disable();
                checkGenerate();
            }
            else if ((loanterm > 0) && (chckCheck.getValue() == true) && (grdPnlAmortizationSchedule.store.getCount() == 0) && (grdpnlCheque.store.getCount() == 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'No checks generated.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
            }
            else if ((loanterm > 0) && (chckCheck.getValue() == false) && (grdpnlCheque.store.getCount() > 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'Schedule and Checks do not match. Generate to refresh data.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
                btnPrintSchedule.disable();
            }
            else if (loanterm != grdPnlAmortizationSchedule.store.getCount()) {
                PageFormPanelStatusBar.setStatus({ text: 'Schedule and Term do not match. Generate schedule to proceed.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
                btnPrintSchedule.disable();
            }
            else if (collateralType == "Secured" && GridPanelCollaterals.store.getCount() == 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Collateral requirement type is secured. Please add collateral/s.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
            else if (date == '1') {
                if (hdnTermOption.getValue() != 'No Term') {
                    PageFormPanelStatusBar.setStatus({ text: 'Loan Release and Payment Start Dates does not match the schedule.', iconCls: 'icon-exclamation' });
                    btnSave.disable();
                } else {
                    PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                    btnSave.enable();
                }
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please completely fill out the forms.', iconCls: 'icon-exclamation' });
                btnSave.disable();
                btnApprove.disable();
                btnPrintSchedule.disable();
                checkGenerate();
            }
        }

        var checkGenerate = function () {
            if ((txtLoanProductName.getValue == '') || (datLoanReleaseDate.getValue() == '') || (nfLoanAmount.getValue() == '')
            || (cmbInterestRate.getValue() == '') || (nfInterestRate.getValue() == ''))
                btnGenerate.disable();
            else
                btnGenerate.enable();
        }

        var ontBtnPrintSchedule = function () {
            var url = '/Applications/LoanApplicationUseCases/PrintAmortizationSchedule.aspx';
            var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
            param += "&customerPartyRoleId=" + hiddenCustomerID.getValue();
            window.proxy.requestNewTab('PrintAmortizationSchedule', param, 'Print Temporary Amortization Schedule');
        }

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                // load information to the CustomerID, Names, Addresses, PartyType, Status
                X.CheckCustomerAgeValidity(msg.data.CustomerID, {
                    success: function (result) {
                        if (result)
                            X.FillCustomerDetails(msg.data.CustomerID);
                        else
                            showAlert('Information', 'The age of the customer below the accepted age limit. Please select another customer.');
                    }
                });
            }
            else if (msg.tag == 'onpickfinancialproduct') {
                X.FillFinancialProductDetails(msg.data.Id, {
                    success: function () {
                        markIfRequired(nfLoanTerm);
                    }
                });
            }
            else if (msg.tag == 'onpickinteretrate') {
                hiddenInterestRateId.setValue(msg.data.ProductFeatureApplicabilityId);
                txtInterestRateDescription.setValue(msg.data.Description);
                nfInterestRate.setValue(msg.data.InterestRate);
            }
            else if (msg.tag == 'onpickpastduerate') {
                hiddenPastDueInterestRate.setValue(msg.data.ProductFeatureApplicabilityId);
                txtPastDueDescription.setValue(msg.data.Description);
                nfPastDueRate.setValue(msg.data.InterestRate);
            }
            else if (msg.tag == 'onpickapplicationfee') {
                X.AddFee(msg.data.ProductFeatureApplicabilityId, msg.data.FeeName, msg.data.Amount);
            }
            else if (msg.tag == 'onpickguarantor') {
                X.AddGuarantor(msg.data.PartyId, msg.data.Name, msg.data.Address);
            }
            else if (msg.tag == 'onpickcoborrower') {
                X.AddCoBorrower(msg.data.PartyId, msg.data.Name, msg.data.Address);
            }
            else if (msg.tag == 'managecollateral') {
                X.UpdateCollaterals();
            }
            else if (msg.tag == 'addrequireddocument') {
                X.UpdateRequiredDocuments();
            }
            else if (msg.tag == 'closeapproved') {
                window.proxy.requestClose('ApproveLoanApplication');
                btnApprove.disable();
                var id = msg.data.id;
                OpenPromissoryNote(id);
            }
            else if (msg.tag == 'addcheque') {
                X.AddCheque(msg.data.BankId, msg.data.Amount, msg.data.ChequeNumber, msg.data.TransactionDate, msg.data.Remarks, msg.data.ChequeDate);
            }
            else if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            }
        };

        var OpenPromissoryNote = function (id) {
            var url = '/Applications/LoanApplicationUseCases/PrintPromissoryNote.aspx';
            var param = url + '?loanApplicationId=' + id;
            window.proxy.requestNewTab('PrintPromisoryNote', param, 'Print Promissory Note');
        }

        var saveSuccessful = function () {
            if (window.IsSaveBeforeApproval == false) {
                showAlert('Status', 'Successfully updated loan application record.', function () {
                    window.proxy.sendToAll('modifyLoanApplication', 'modifyLoanApplication');
                    //window.proxy.requestClose();
                    openOrEdit();
                });
            }

            if (window.IsSaveBeforeApproval) {
                onBtnApproveClick();
            }

            window.IsSaveBeforeApproval = false;
        };

        var changeStatusSuccessful = function () {
            showAlert('Status', 'Successfully updated the status of the loan application record.', function () {
                window.proxy.sendToAll('modifyLoanApplication', 'modifyLoanApplication');
            window.proxy.requestClose();
            });
        }

        var onBtnPickInterestRate = function () {
            if (hiddenProductId.getValue()) {
                var url = '/BestPractice/PickListInterestRate.aspx';
                var param = url + "?mode=" + 'single';
                param += "&financialProductId=" + hiddenProductId.getValue();
                window.proxy.requestNewTab('PickInterestRate', param, 'Select Interest Rate');
            }
            else {
                showAlert('Information', 'Please select a financial product before selecting an interest rate.');
            }
        };

        var onBtnPickPastDueRate = function () {
            if (hiddenProductId.getValue()) {
                var url = '/BestPractice/PickListPastDueRate.aspx';
                var param = url + "?mode=" + 'single';
                param = param + "&financialProductId=" + hiddenProductId.getValue();
                window.proxy.requestNewTab('PickPastDueRate', param, 'Select Past Due Rate');
            }
            else {
                showAlert('Information', 'Please select a financial product before selecting a past due interest rate.');
            }
        };

        var onBtnPickFee = function () {
            if (hiddenProductId.getValue()) {
                var url = '/Applications/LoanApplicationUseCases/ManageApplicationFee.aspx';
                var param = url + "?financialProductId=" + hiddenProductId.getValue();
                param += "&ResourceGuid=" + hiddenResourceGuid.getValue();
                window.proxy.requestNewTab('AddFee', param, 'Add Fee');
            }
            else {
                showAlert('Information', 'Please select a financial product before adding fees.');
            }
        };

        var getIds = function (grid) {
            var partyIds = [];
            for (var j = 0, jlen = grid.store.getCount(); j < jlen; j++) {
                var r = grid.store.getAt(j);
                partyIds.push(r.json.PartyId);
            }
            return partyIds;
        };

        var getToExcludeCustomers = function () {
            var param = "customerId=" + hiddenCustomerID.getValue();

            var coborrowers = getIds(GridPanelCoBorrower);
            var guarantors = getIds(GridPanelGuarantors);
            param += "&coborrowers=" + coborrowers.join(",");
            param += "&guarantors=" + guarantors.join(",");
            return param;
        };

        var onBtnPickCoBorrower = function () {
            if (hiddenCustomerID.getValue()) {
                var url = '/Applications/LoanApplicationUseCases/PickListCoBorrower.aspx';
                var param = url + "?mode=" + 'single';
                param += "&" + getToExcludeCustomers();
                window.proxy.requestNewTab('PickCoBorrower', param, 'Select Co-Borrower');
            } else {
                showAlert('Information', 'Please select a customer before adding co-borrowers.');
            }
        };

        var onBtnPickGuarantor = function () {
            if (hiddenCustomerID.getValue()) {
                var url = '/Applications/LoanApplicationUseCases/PickListGuarantor.aspx';
                var param = url + "?mode=" + 'single';
                param += "&" + getToExcludeCustomers();
                window.proxy.requestNewTab('PickGuarantor', param, 'Select Guarantor');
            } else {
                showAlert('Information', 'Please select a customer before adding guarantors.');
            }
        };

        var onAddCollateral = function (item) {
            var text = item.text;
            var collateral = window.collateral[text];
            onManageCollateral(collateral.url, collateral.tabid, collateral.tabtitle, 'add', '');
        };

        var onManageCollateral = function (url, tabid, tabtitle, mode, randomKey) {
            var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
            param += "&mode=" + mode;
            if (mode == "edit")
                param += "&RandomKey=" + randomKey;
            window.proxy.requestNewTab(tabid, param, tabtitle);
        };

        var onBtnEditCollateral = function (item) {
            var data = SelectionModelCollaterals.getSelected().json;
            var text = data.AssetTypeName;
            if (data.AssetTypeName == 'Jewelry' || data.AssetTypeName == 'Others')
                text = 'Jewelry/Others';
            var collateral = window.collateral[text];
            onManageCollateral(collateral.url, collateral.tabid, collateral.tabtitle, 'edit', data.RandomKey);
        };

        var onBtnPickCustomer = function () {
            var url = '/Applications/CustomerUseCases/CustomerPickList.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickCustomer', param, 'Select Customer');
        };

        var onBtnPickFinancialProduct = function () {
            var url = '/BestPractice/PickListFinancialProduct.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickFinancialProduct', param, 'Select Financial Product');
        };

        var onBtnAddCheque = function () {
            if (nfLoanTerm.getValue() != 0) {
                var url = '/Applications/LoanApplicationUseCases/ManageCheque.aspx';
                var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
                var param = param + "&CustomerId=" + hiddenCustomerID.getValue();
                window.proxy.requestNewTab('ManageCheque', param, 'Manage Cheque');
            }
            else {
                showAlert('Information', 'You do not have a loan term. Please add a loan term to add associated cheques.');
            }
        }

        var onBtnShowLoanCalculator = function () {
            if (hiddenProductId.getValue()) {
                var url = '/Applications/AmortizationScheduleUseCases/GenerateAmortizationSchedule.aspx';
                var param = url + "?financialProductId=" + hiddenProductId.getValue();
                window.proxy.requestNewTab('LoanApplicationCalculator', param, 'Loan Calculator');
            }
            else {
                showAlert('Information', 'Please select a financial product before you can check the calculated loan.');
            }
        }

        var onBtnAddDocument = function () {
            if (hiddenProductId.getValue()) {
                var url = '/Applications/LoanApplicationUseCases/AddSubmitRequiredDocument.aspx';
                var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
                param = param + "&financialProductId=" + hiddenProductId.getValue();
                window.proxy.requestNewTab('AddSubmittedDocument', param, 'Add Documents');
            } else {
                showAlert('Information', 'Please select a financial product before adding required documents.');
            }
        }

        var onBtnOpenDocument = function () {
            var selectedRow = RowSelectionModelDocuments.getSelected();
            if (selectedRow) {
                var url = '/Applications/LoanApplicationUseCases/ViewSubmittedDocument.aspx';
                var param = url + '?RandomKey=' + selectedRow.id + "&ResourceGuid=" + hiddenResourceGuid.getValue();
                window.proxy.requestNewTab('ViewSubmittedDocument', param, 'View Submitted Documents');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        }

        var onRowSelected = function (btn) {
            btn.enable();
        };
        var onRowDeselected = function (grid, btn) {
            if (grid.hasSelection() == false) {
                btn.disable();
            }
        };

        var onBeforeApproveClick = function () {
            if (RootPanel.getForm().isDirty()) {
                window.IsSaveBeforeApproval = true;
                btnSave.fireEvent('click');
            }
            else {
                onBtnApproveClick();
            }
        }

        var onBtnApproveClick = function () {
            var url = '/Applications/LoanApplicationUseCases/ApproveLoanApplication.aspx';
            var id = 'id=' + hiddenApplicationId.getValue();
            var param = url + "?" + id
            window.proxy.requestNewTab('ApproveLoanApplication', param, 'Approve Loan Application Record');
        };

        var onBtnPrint = function () {
            var url = '/Applications/LoanApplicationUseCases/PrintSelectedLoanApplication.aspx';
            var param = url + "?customerPartyRoleId=" + hiddenCustomerID.getValue();
            param += "&loanApplicationId=" + hiddenApplicationId.getValue();
            window.proxy.requestNewTab('PrintSelectedLoanApplication', param, 'Print Loan Application Record');
        };

        var setDate = function () {
            hdnOnChangeDate.setValue('1');
        }

        var adjustPaymentStartDate = function () {
            X.setPaymentDate(null, {
                success: function (result) {
                    if (result == false) {
                        showAlert('Error', 'Please select a financial product before setting the loan release date.');
                        datLoanReleaseDate.setValue('');
                    }
                }
            });
        }

        var checkLoanAmountLimit = function () {
            var minimum = hiddenLoanAmountLimit.minValue;
            var maximum = hiddenLoanAmountLimit.maxValue;

            var msg = "";
            var value = Ext.util.Format.number(nfLoanAmount.getValue().replace(/,/g, ''), '0.00');
            if (value < minimum) {
                msg = "The specified value is lesser than the product's minimum loanable amount.";
            }
            else if (value > maximum) {
                msg = "The specified value is greater than the product's maximum loanable amount.";
            }

            var equal = hiddenLoanAmountLimit.getValue() == value;
            hiddenLoanAmountLimit.setValue(value);
            if (msg == "") {
                if (equal == false)
                    btnRefresh.fireEvent('click');
            }
            else
                nfLoanAmount.markInvalid(msg);
        };

        //-----------------CHECKING-----------------------//
        var Validate = function () {
            var chequeNumberCount = 0;
            for (var j = 0; j < grdpnlCheque.store.getCount(); j++) {
                var data = grdpnlCheque.store.getAt(j);
                chequeNumber = data.get('ChequeNumber');
                if (chequeNumber == '')
                    chequeNumberCount++;
            }

            if (chequeNumberCount > 0) {
                showAlert('Error', 'One or more cheques does not contain all required elements.');
            }
            else {
                var loanAmount = checkLoanAmount(); //if loan amount == 1 - error msg
                if (loanAmount != true) {
                    //loan amount != 1 - proceed
                    if (loanTerm()) //if loanterm == true 
                        btnSaveActual.fireEvent('click'); //save
                } else {
                    showAlert('Error!', 'Loan amount should be greater than the total of the outstanding loans to pay off.'); //loan term == false - error msg
                    return false;
                }
            }
        }

        //[b]
        var checkLoanAmount = function () {
            var selectedValues = [];
            var total = 0;
            result = 0;
            var loanAmount = nfLoanAmount.getValue();
            loanAmount = loanAmount.replace(',', '');
            var selectedRows = RowSelectionOutstandingLoans.getSelections();
            for (var i = 0; i < selectedRows.length; i++) {
                total += selectedRows[i].json.LoanBalance;
            }
            if (loanAmount < total) {
                result = 1;
            }
            return result;
        }

        //[c]
        var loanTerm = function () {
            X.CheckLoanTerm({
                success: function (result) {
                    if (result) {
                        return Confirm(); //if result == true, may proceed to next condition checking
                    } else {
                        showAlert('Error!', 'Loan term cannot be converted to another whole number base on the selected payment mode. Please change the payment mode or the loan term value.');
                        return false;
                    }
                }
            });
        }

        //[d]
        var Confirm = function () {
            X.CheckCredit({
                success: function (result) {
                    if (result) { //if true, confirmation message asking the user to proceed saving. --check for next condition
                        showConfirm('Confirm', 'Loan amount exceeds the available credit limit of the borrower. Do you want to continue creating this loan application record?', function (btn) {
                            if (btn == 'no') { //dont continue saving
                                return false;
                            } else {
                                if (CheckDiminishing()) //proceed to next condition
                                    btnSaveActual.fireEvent('click');
                            }
                        });
                    } else {
                        if (CheckDiminishing())
                            btnSaveActual.fireEvent('click');
                    }
                }
            });
        }

        //[e]
        var CheckDiminishing = function () {
            X.CheckDiminishing({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Allowed number of diminishing balance loan per customer is already reached. Do you want to continue creating this loan application record?', function (btn) {
                            if (btn == 'no') {
                                return false;
                            } else {
                                if (CheckStraightLine())
                                    return true;
                            }
                        });
                    } else {
                        //btnSaveActual.fireEvent('click');
                        if (CheckStraightLine())
                            return true;
                    }
                }
            });
        }

        //[f]
        var CheckStraightLine = function () {
            X.CheckStraightLine({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Allowed number of straight line loan per customer is already reached. Do you want to continue creating this loan application record?', function (btn) {
                            if (btn == 'no') {
                                return false;
                            } else {
                                if (CheckCreditLimit())
                                    return true;
                            }
                        });
                    } else {
                        //btnSaveActual.fireEvent('click');
                        if (CheckCreditLimit())
                            return true;
                    }
                }
            });
        }

        //[g]
        var CheckCreditLimit = function () {
            X.CheckCreditLimit({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Loan Amount is greater than the Credit Limit and the allowed numbers of straight line/diminishing balance loan per customer is already reached. Do you want to continue creating this loan application record?', function (btn) {
                            if (btn == 'no') {
                                return false;
                            } else {
                                btnSaveActual.fireEvent('click');
                            }
                        });
                    } else {
                        btnSaveActual.fireEvent('click');
                    }
                }
            });
        }

        //-----------------End of CHECKING-----------------------//

        var termSettings = function () {
            var termOption = hdnTermOption.getValue();

            if (termOption == 'No Term' && (grdPnlAmortizationSchedule.store.getCount() == 0)) {
                X.ClearForm();
                nfLoanTerm.setValue('0');
                nfLoanTerm.disable();
                cmbPaymentMode.disable();
                cmbPaymentMode.allowBlank = true;
                nfLoanTerm.allowBlank = true;
                markIfRequired(nfLoanTerm);
                markIfRequired(cmbPaymentMode);
            } else {
                hdnLoanTermIndicator.setValue('1')
                nfLoanTerm.enable();
                cmbPaymentMode.enable();
                cmbPaymentMode.allowBlank = false;
                nfLoanTerm.allowBlank = false;
                markIfRequired(nfLoanTerm);
                markIfRequired(cmbPaymentMode);
            }
            nfLoanTerm.validate();
        }

        var OnChkTermChanged = function () {
            if (chckWithTerm.getValue() == true) {
                hdnLoanTermIndicator.setValue('1')
                nfLoanTerm.enable();
                cmbPaymentMode.enable();
                cmbPaymentMode.allowBlank = false;
                nfLoanTerm.allowBlank = false;
                markIfRequired(nfLoanTerm);
                markIfRequired(cmbPaymentMode);
                btnPrintTempSched.enable();
            }
            else if (chckWithTerm.getValue() == false && (grdPnlAmortizationSchedule.store.getCount() == 0)) {
                hdnLoanTermIndicator.setValue('0');

                chckCheck.setValue(true);
                nfLoanTerm.setValue('0');
                nfLoanTerm.disable();
                cmbPaymentMode.disable();
                cmbPaymentMode.allowBlank = true;
                nfLoanTerm.allowBlank = true;
                markIfRequired(nfLoanTerm);
                markIfRequired(cmbPaymentMode);
                btnPrintTempSched.disable();
            }
            else {
                if (grdPnlAmortizationSchedule.store.getCount() > 0) {
                    showConfirm('Warning', 'Schedule contains items. Are you sure you want to switch to a loan with no term?', function (btn) {
                        if (btn == 'no') {
                            chckWithTerm.setValue(true);
                        }
                        else {
                            X.ClearForm();
                            hdnLoanTermIndicator.setValue('0');

                            btnGenerate.disable();
                            btnPrintSchedule.disable();
                            chckCheck.setValue(false);
                            nfLoanTerm.setValue('0');
                            nfLoanTerm.disable();
                            cmbPaymentMode.disable();
                            cmbPaymentMode.allowBlank = true;
                            nfLoanTerm.allowBlank = true;
                            markIfRequired(nfLoanTerm);
                            markIfRequired(cmbPaymentMode);
                        }
                    });
                }
            }
            nfLoanTerm.validate();
        };

        var resetCheque = function () {
            wdwCheque.hide();
            txtAmount.setValue('');
            txtBank.setValue('');
            txtCheckNumber.setValue('');
            dtCheckDate.setValue('');
        }

        var onChequeFormValidated = function () {
            var checkNumber = txtCheckNumber.getValue();
            var proceedToCheckGrid = false;
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        txtCheckNumber.markInvalid('Check Number already exist.');
                        StatusBarCheque.setStatus({ text: 'Check Number already exist.', iconCls: 'icon-exclamation' });
                        btnSaveNewCheque.disable();
                    } else {
                        StatusBarCheque.setStatus({ text: '', iconCls: 'icon-exclamation' });
                        btnSaveNewCheque.enable();
                        proceedToCheckGrid = true;
                    }
                }
            });

            if (proceedToCheckGrid) {
                checkGrid();
            }
        }


        var checkGrid = function () {
            var selected = SelectionModelCheque.getSelected();
            var checkNumber = txtCheckNumber.getValue();
            var chequeNumber = 0;
            var count = 0;
            for (var j = 0; j < grdpnlCheque.store.getCount(); j++) {
                var data = grdpnlCheque.store.getAt(j);
                chequeNumber = data.get('ChequeNumber');
                if ((chequeNumber != checkNumber)) {
                    //btnSaveNewCheque.enable();
                }
                else if ((chequeNumber == checkNumber) && (selected.id != data.id)) {
                    count++;
                }
                else if ((chequeNumber == checkNumber) && (selected.id == data.id)) {
                    //btnSaveNewCheque.enable();
                }
                else {
                    count++;
                }
            }

            if (count > 0) {
                txtCheckNumber.markInvalid();
                StatusBarCheque.setStatus({ text: 'Check Number already exist.', iconCls: 'icon-exclamation' });
                btnSaveNewCheque.disable()
            }
            else if (checkNumber == '') {
                txtCheckNumber.markInvalid();
                StatusBarCheque.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
                btnSaveNewCheque.disable();
            }
            else {
                StatusBarCheque.setStatus({ text: '', iconCls: 'icon-exclamation' });
                btnSaveNewCheque.enable();
            }
        }

        var FillDetails = function () {
            var selected = SelectionModelCheque.getSelected();
            X.FillCheques({
                success: function () {
                    txtCheckNumber.setValue(selected.json.ChequeNumber);
                }
            });
            wdwCheque.show();
        }

        var setSameBank = function () {
            X.SetBank();
        }

        var onBtnPrintProm = function () {
            var id = hiddenApplicationId.getValue();
            openPromissory(id);
        }

        var openPromissory = function (id) {
            var url = '/Applications/LoanApplicationUseCases/PrintPromissoryNote.aspx';
            var param = url + '?loanApplicationId=' + id;
            param += '&mode=modify';
            window.proxy.requestNewTab('PrintPromissoryNote', param, 'Print Promissory Note');
        }

        var onBtnPrintSPA = function () {
            var url = '/Applications/LoanApplicationUseCases/PrintSPA.aspx';
            var param = url + "?customerPartyRoleId=" + hiddenCustomerID.getValue();
            param += "&loanApplicationId=" + hiddenApplicationId.getValue();
            param += "&mode=modify";
            window.proxy.requestNewTab('PrintSPA', param, 'Print SPA');
        }
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden runat="server" ID="hdnLoanTermIndicator"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnStraightLineCount"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnDiminishingCount"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnRandomKey"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnLoanApplicationId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnTermOption"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnTermOptionId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnOnChangeDates" Text="0"></ext:Hidden>
    <ext:Store runat="server" ID="storeCollateralRequirement" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeMethodOfChargingInterest" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storePaymentMethod" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeInterestComputationMode" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeInterestRate" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden ID="hiddenResourceGuid" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenApplicationId" runat="server">
    </ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="RootPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true" MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnOpen" runat="server" Text="Edit" Icon="NoteEdit">
                                <Listeners>
                                    <Click Handler="openOrEdit();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSpacer ID="btnOpenSpacer1" />
                            <ext:ToolbarSeparator ID="btnOpenSeparator"/>
                            <ext:ToolbarSpacer ID="btnOpenSpacer2" />
                            <ext:Button runat="server" ID="btnApprove" Text="Approve" Icon="ApplicationAdd">
                                <Listeners>
                                    <Click Handler="onBeforeApproveClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSpacer ID="btnApproveSpacer1" />
                            <ext:ToolbarSeparator ID="btnApproveSeparator"/>
                            <ext:ToolbarSpacer ID="btnApproveSpacer2" />
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                <DirectEvents>
                                    <Click OnEvent="btnCancel_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to cancel the selected loan applications?"/>
                                        <EventMask ShowMask="true" Msg="Updating loan application status..."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:ToolbarSeparator/>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:Button ID="btnReject" runat="server" Text="Reject" Icon="ApplicationFormDelete">
                                <DirectEvents>
                                    <Click OnEvent="btnReject_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to reject the selected loan applications?"/>
                                        <EventMask ShowMask="true" Msg="Updating loan application status..."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:ToolbarSeparator/>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <%--<ext:Button ID="btnClose" runat="server" Text="Close" Icon="BinClosed">
                                 <DirectEvents>
                                    <Click OnEvent="btnClose_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to close the selected loan applications?"/>
                                        <EventMask ShowMask="true" Msg="Closing the loan application"/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:ToolbarSeparator/>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>--%>
                            <ext:Button runat="server" ID="btnPrintSPA" Text="Print SPA" Icon="Printer">
                                <Listeners>
                                    <Click Handler="onBtnPrintSPA();"/>
                                </Listeners>
                            </ext:Button>                            
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:ToolbarSeparator/>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:Button runat="server" ID="btnPrintPromissory" Icon="Printer" Text="Print Promissory Note">
                                <Listeners>
                                    <Click Handler="onBtnPrintProm();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:ToolbarSeparator/>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:Button ID="btnPrint" runat="server" Text="Print Loan Details" Icon="Printer">
                                <Listeners>
                                    <Click Handler="onBtnPrint();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <ext:Button ID="btnPrintTempSched" runat="server" Text="Print Temporary Schedule" Icon="Printer">
                                <Listeners>
                                    <Click Handler="ontBtnPrintSchedule();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <Listeners>
                                    <Click Handler="Validate();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnSaveActual" runat="server" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();">
                                        <EventMask ShowMask="true" Msg="Saving Loan Application.."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnSaveSeparator"/>
                            <ext:Button ID="btnCloseTab" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TabPanel ID="PageTabPanel" runat="server" EnableTabScroll="true" Padding="0"
                        HideBorders="true" BodyStyle="background-color:transparent">
                        <Items>
                            <ext:Panel ID="pnlBorrowersInformation" runat="server" Title="Borrowers Information" Border="false" Padding="5"
                                AutoScroll="true">
                                <Items>
                                    <ext:Panel runat="server" ID="PanelBorrowerBasicInformation" Title="Basic Information"
                                        Padding="5" Width="800" Layout="Column" RowHeight=".5" Height="260">
                                        <Items>
                                            <ext:Panel runat="server" Width="200" Height="200" Border="false" Header="false"
                                                ColumnWidth=".3">
                                                <Items>
                                                    <ext:Image ID="imgPersonPicture" runat="server" ImageUrl="../../../Resources/images/DSC_0035.jpg"
                                                        Width="200" Height="180" AnchorHorizontal="95%" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" LabelWidth="120" Layout="FormLayout" Border="false" ColumnWidth=".6">
                                                <Items>
                                                    <ext:Hidden ID="hiddenCustomerID" runat="server" />
                                                    <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Name" ReadOnly="true"
                                                        AnchorHorizontal="100%" AllowBlank="false"/>
                                                    <ext:TextField ID="txtDistrict" runat="server" FieldLabel="District" ReadOnly="true"
                                                        AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtStationNumber" runat="server" FieldLabel="Station Number" ReadOnly="true"
                                                        AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtCustomerType" runat="server" FieldLabel="Customer Type" ReadOnly="true"
                                                        AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtCustomerStatus" runat="server" FieldLabel="Customer Status"
                                                        ReadOnly="true" AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtGender" runat="server" FieldLabel="Gender" ReadOnly="true"
                                                        AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtAge" runat="server" FieldLabel="Age" ReadOnly="true" AnchorHorizontal="100%" />
                                                    <ext:TextField ID="txtCreditLimit" runat="server" FieldLabel="Credit Limit" ReadOnly="true"
                                                        AnchorHorizontal="100%" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="ActionPanel" runat="server" Border="false" Header="false" ColumnWidth=".1">
                                                <Items>
                                                    <ext:Button ID="btnPickCustomer" Visible="false" runat="server" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickCustomer();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="PanelBorrowerContactInformation" Title="Contact Information"
                                        Padding="5" Width="800" Layout="Form" LabelWidth="200" RowHeight=".5">
                                        <Items>
                                            <ext:TextArea ID="txtPrimaryHomeAddress" runat="server" FieldLabel="Primary Home Address"
                                                ReadOnly="true" Width="477" />
                                            <ext:CompositeField ID="cmpCellphoneNumber" DataIndex="" runat="server" Width="500">
                                                <Items>
                                                    <ext:TextField ID="txtCellNumberCountryCode" FieldLabel="Cellphone Number" runat="server" Width="150" ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:DisplayField runat="server" Text="-">
                                                    </ext:DisplayField>
                                                    <ext:TextField ID="nfCellNumberAreaCode" runat="server" Width="150" ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:DisplayField runat="server" Text="-">
                                                    </ext:DisplayField>
                                                    <ext:NumberField ID="nfCellNumberPhoneNumber" runat="server" Width="150" ReadOnly="true">
                                                    </ext:NumberField>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="cmpPrimaryTelNumber" DataIndex="" runat="server" FieldLabel="Primary Telephone Number"
                                                Width="500">
                                                <Items>
                                                    <ext:TextField ID="txtPrimaryTelCountryCode" runat="server" Width="150" ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:DisplayField runat="server" Text="-">
                                                    </ext:DisplayField>
                                                    <ext:TextField ID="txtPrimaryTelAreaCode" runat="server" Width="150" ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:DisplayField runat="server" Text="-">
                                                    </ext:DisplayField>
                                                    <ext:TextField ID="txtPrimaryTelPhoneNumber" runat="server" Width="150" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtPrimaryEmailAddress" runat="server" FieldLabel="Primary Email Address"
                                                ReadOnly="true" Width="477" />
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnlLoanApplicationDetails" runat="server" Title="Loan Application Details"
                                Padding="5" Border="false" Layout="Form" LabelWidth="200" AutoScroll="true">
                                <Items>
                                    <ext:Hidden ID="hiddenLoanApplicationID" runat="server" />
                                    <ext:DateField ID="dtApplicationDate" runat="server" FieldLabel="Application Date"
                                        Width="400" ReadOnly="false"/>
                                    <ext:TextField ID="txtLoanApplicationStatus" runat="server" FieldLabel="Loan Application Status"
                                        ReadOnly="true" Width="400" />
                                    <ext:TextField ID="txtStatusComment" runat="server" FieldLabel="Status Comment" Width="400" />
                                    <ext:Hidden ID="hiddenProductId" runat="server" />
                                    <ext:Hidden ID="hiddenLoanTermTimeUnitId" runat="server" />
                                    <ext:CompositeField ID="cfFinancialProduct" DataIndex="" runat="server" Width="500" Height="25">
                                        <Items>
                                            <ext:TextField ID="txtLoanProductName" runat="server" FieldLabel="Loan Product Name" ReadOnly="true" Width="400" Height="25"/>
                                            <ext:Button ID="btnPickFinancialProduct" runat="server" Text="Browse..." Visible="false" Height="25">
                                                <Listeners>
                                                    <Click Handler="onBtnPickFinancialProduct();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField runat="server" ID="txtTermOption" ReadOnly="true" AllowBlank="false" Width="400" FieldLabel="Term Option"></ext:TextField>
                                    <ext:DateField ID="datLoanReleaseDate" runat="server" FieldLabel="Loan Release Date"
                                        ReadOnly="false" Width="400" Editable="false" AllowBlank="false">
                                        <Listeners>
                                            <Select Handler="adjustPaymentStartDate();"/>
                                            <Change Handler="setDate();" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                                                    Width="400" Editable="false" AllowBlank="false" LabelWidth="100" Vtype="daterange" StartDateField="datLoanReleaseDate"/>
                                    <ext:TextField ID="nfLoanAmount" runat="server" FieldLabel="Loan Amount" Number="0"
                                                Width="400" AllowBlank="false" MaskRe="[0-9\.\,]">
                                        <Listeners>
	                                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));checkLoanAmountLimit();" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:NumberField ID="hiddenLoanAmountLimit" runat="server" Number="0" Hidden="true"/>
                                    <%--<ext:RadioGroup ID="rdoLoanTerm" runat="server" FieldLabel="Is the loan with term?"
                                        Width="200" Visible="true" AllowBlank="false">
                                        <Items>
                                            <ext:Radio runat="server" ID="chckWithTerm" BoxLabel="Yes">
                                                <Listeners>
                                                    <Check Handler="OnChkTermChanged();" />
                                                </Listeners>
                                            </ext:Radio>
                                            <ext:Radio runat="server" ID="chckWithoutTerm" BoxLabel="No">
                                                <Listeners>
                                                    <Check Handler="OnChkTermChanged();" />
                                                </Listeners>
                                            </ext:Radio>
                                        </Items>
                                    </ext:RadioGroup>--%>
                                    <ext:NumberField ID="nfLoanTerm" runat="server" FieldLabel="Loan Term" Number="0"
                                        Width="400" ReadOnly="false" AllowBlank="false" DecimalPrecision="0">
                                        </ext:NumberField>
                                    <ext:TextArea ID="txtLoanPurpose" runat="server" FieldLabel="Loan Purpose" Width="400" Hidden="true"/>
                                    <ext:ComboBox runat="server" ID="cmbCollateralRequirement" FieldLabel="Collateral Requirement"
                                        Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeCollateralRequirement"
                                        ValueField="Id" DisplayField="Name" />
                                    <ext:ComboBox runat="server" ID="cmbInterestComputationMode" FieldLabel="Interest Computation Mode"
                                        Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeInterestComputationMode"
                                        ValueField="Id" DisplayField="Name" />
                                    <ext:ComboBox runat="server" ID="cmbPaymentMode" FieldLabel="Payment Mode" Width="400"
                                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                                        SelectOnFocus="true" AllowBlank="false" StoreID="storePaymentMethod" ValueField="Id"
                                        DisplayField="Name" />
                                    <ext:ComboBox runat="server" ID="cmbMethodOfChargingInterest" FieldLabel="Method of Charging Interest"
                                        Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeMethodOfChargingInterest"
                                        ValueField="Id" DisplayField="Name" />
                                    <ext:Panel runat="server" ID="PanelInterestRate" Title="Interest Rate" LabelWidth="180"
                                        Layout="Column" Width="586" Height="80">
                                        <Items>
                                            <ext:Panel ID="Panel1" runat="server" LabelWidth="100" Layout="FormLayout" Border="false"
                                                ColumnWidth=".9" Padding="2">
                                                <Items>
                                                    <ext:ComboBox runat="server" ID="cmbInterestRate" AnchorHorizontal="100%" Editable="false" TypeAhead="true"
                                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                                                        StoreID="storeInterestRate" ValueField="Id" DisplayField="Name" FieldLabel="Description"/>
                                                    <ext:NumberField ID="nfInterestRate" runat="server" FieldLabel="Interest Rate" AnchorHorizontal="100%"
                                                        DecimalPrecision="2" MinValue="0" MaxValue="100" AllowBlank="false" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" ColumnWidth=".1"
                                                Padding="2">
                                                <Items>
                                                    <ext:Button ID="btnPickInterestRateDesc" runat="server" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickInterestRate();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Label ID="Label1" runat="server" Text="%" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="PanelPastDueInterestRate" Title="Past Due Interest Rate"
                                        LabelWidth="180" Layout="Column" Width="586" Height="80" Hidden="true">
                                        <Items>
                                            <ext:Panel ID="Panel3" runat="server" LabelWidth="100" Border="false" ColumnWidth=".9"
                                                Layout="FormLayout" Padding="2">
                                                <Items>
                                                    <ext:Hidden ID="hiddenPastDueInterestRate" runat="server" />
                                                    <ext:TextField ID="txtPastDueDescription" runat="server" FieldLabel="Description"
                                                        AnchorHorizontal="100%" ReadOnly="true" />
                                                    <ext:NumberField ID="nfPastDueRate" runat="server" FieldLabel="Past Due Rate" AnchorHorizontal="100%"
                                                        DecimalPrecision="0" MinValue="0" MaxValue="100" ReadOnly="true" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="Panel4" runat="server" Border="false" Header="false" ColumnWidth=".1"
                                                Padding="2">
                                                <Items>
                                                    <ext:Button ID="btnPickPastDueRate" runat="server" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickPastDueRate();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Label ID="lblPastDuePercentage" runat="server" Text="%" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnlOutstandingLoans" Title="Outstanding Loans"
                                Border="false" LabelWidth="400" Layout="FitLayout">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                                        <Items>
                                           <ext:Checkbox ID="chkPayOutstandingLoan" runat="server" Disabled="true" FieldLabel="Pay the following outstanding loan/s using the proceeds of the new loan">
                                            <Listeners>
                                                <Check Handler="onChkPayOutstandingLoan();" />
                                            </Listeners>
                                        </ext:Checkbox> 
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Items>
                                    <ext:GridPanel ID="panelPayOutstandingLoan" runat="server" Title="Outstanding Loan/s to Pay Off"  ColumnLines="true">
                                        <View>
                                            <ext:GridView EmptyText="No outstanding loans to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                        </View>
                                        <Store>
                                            <ext:Store runat="server" ID="storePayOutstandingLoan" RemoteSort="false">
                                                <Listeners>
                                                    <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                                </Listeners>
                                                <Reader>
                                                    <ext:JsonReader IDProperty="LoanId">
                                                        <Fields>
                                                            <ext:RecordField Name="LoanId" />
                                                            <ext:RecordField Name="LoanProductName" />
                                                            <ext:RecordField Name="InterestComputationMode" />
                                                            <ext:RecordField Name="MaturityDate" Type="Date" />
                                                            <ext:RecordField Name="NoOfInstallments" />
                                                            <ext:RecordField Name="PaymentMode" />
                                                            <ext:RecordField Name="ScheduledAmortization" />
                                                            <ext:RecordField Name="LoanBalance" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" ID="RowSelectionOutstandingLoans" SingleSelect="true"></ext:RowSelectionModel>
                                            <%--<ext:CheckboxSelectionModel ID="RowSelectionOutstandingLoans" SingleSelect="false" runat="server">
                                            </ext:CheckboxSelectionModel>--%>
                                        </SelectionModel>
                                        <ColumnModel runat="server" ID="clmModelPayOutstandingLoan" Width="100%">
                                            <Columns>
                                                <ext:Column Header="Loan ID" DataIndex="LoanId" Wrap="true" Locked="true" Width="80"
                                                    Sortable="false" />
                                                <ext:Column Header="Loan Product Name" DataIndex="LoanProductName" Locked="true"
                                                    Wrap="true" Width="130" />
                                                <ext:Column Header="Interest Computation Mode" DataIndex="InterestComputationMode"
                                                    Locked="true" Wrap="true" Width="180" />
                                                <ext:DateColumn Header="Maturity Date" DataIndex="MaturityDate" Locked="true" Wrap="true"
                                                    Width="120" />
                                                <ext:Column Header="No. Of Installments" DataIndex="NoOfInstallments" Wrap="true"
                                                    Locked="true" Width="140" Sortable="false" />
                                                <ext:Column Header="Payment Mode" DataIndex="PaymentMode" Locked="true" Wrap="true"
                                                    Width="120" />
                                                <ext:NumberColumn Header="Scheduled Amortization" DataIndex="ScheduledAmortization"
                                                    Locked="true" Wrap="true" Width="150" Format=",000.00" />
                                                <ext:NumberColumn Header="Loan Balance" DataIndex="LoanBalance" Locked="true" Wrap="true"
                                                    Width="120" Format=",000.00" />
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnlFeeAndCoBorrower" Title="Fees and Co-Borrowers"
                                Border="false" Layout="RowLayout">
                                <Items>
                                    <ext:GridPanel ID="GridPanelFee" Title="Fees" runat="server" RowHeight=".5" AutoExpandColumn="Name"
                                         ColumnLines="true">
                                        <View>
                                            <ext:GridView EmptyText="No fees to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                        </View>
                                        <Items>
                                            <ext:Toolbar ID="ToolbarFee" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteFee" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteFee_Click" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddFee" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickFee();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnRefresh" runat="server" Text="Re-Calculate" Icon="Reload">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnRefresh_Click" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="StoreFee" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ProductFeatureApplicabilityId" />
                                                            <ext:RecordField Name="Name" />
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="ChargeAmount" />
                                                            <ext:RecordField Name="BaseAmount" />
                                                            <ext:RecordField Name="Rate" />
                                                            <ext:RecordField Name="TotalChargePerFee" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelFee" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteFee);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelFee, btnDeleteFee);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Name" DataIndex="Name" Width="150">
                                                </ext:Column>
                                                <ext:NumberColumn runat="server" Header="Fee Amount" DataIndex="Amount" Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Charge Amount" DataIndex="ChargeAmount" Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Base Amount" DataIndex="BaseAmount" Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Rate" DataIndex="Rate" Width="150" Format="0">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Total Charge Per Fee" DataIndex="TotalChargePerFee"
                                                    Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                    <ext:GridPanel ID="GridPanelCoBorrower" Title="Co-borrowers" runat="server" RowHeight=".5"
                                        AutoExpandColumn="Address"  ColumnLines="true">
                                        <View>
                                            <ext:GridView EmptyText="No co-borrowers to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                        </View>
                                        <Items>
                                            <ext:Toolbar ID="ToolbarCoBorrower" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteCoBorrower" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteCoBorrower_Click" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddCoBorrower" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickCoBorrower();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="StoreCoBorrower" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="PartyId" />
                                                            <ext:RecordField Name="Name" />
                                                            <ext:RecordField Name="Address" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelCoBorrower" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteCoBorrower);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelCoBorrower, btnDeleteCoBorrower);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Party ID" DataIndex="PartyId" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Name" DataIndex="Name" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Primary Home Address" DataIndex="Address" Width="250">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnlGuarantorAndCollaterals" Title="Guarantors, Collaterals and Submitted Documents"
                                Border="false" Layout="RowLayout" RowHeight="1" AutoScroll="true">
                                <Items>
                                    <ext:Panel ID="Panel8" runat="server"  Title="Guarantors">
                                        <Items>
                                            <ext:Toolbar ID="ToolbarGuarantors" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteGuarantor" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteGuarantor_Click">
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddGuarantor" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickGuarantor();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:GridPanel ID="GridPanelGuarantors" runat="server" Height="115"
                                                AutoExpandColumn="Address"  ColumnLines="true" AutoScroll="true" Border="false">
                                                <View>
                                                    <ext:GridView ID="GridView2" EmptyText="No guarantors to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                                </View>
                                                <Store>
                                                    <ext:Store ID="StoreGuarantor" runat="server">
                                                        <Reader>
                                                            <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                                <Fields>
                                                                    <ext:RecordField Name="PartyId" />
                                                                    <ext:RecordField Name="Name" />
                                                                    <ext:RecordField Name="Address" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="SelectionModelGuarantor" SingleSelect="true" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="onRowSelected(btnDeleteGuarantor);" />
                                                            <RowDeselect Handler="onRowDeselected(SelectionModelGuarantor, btnDeleteGuarantor);" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server" Header="Party ID" DataIndex="PartyId" Width="250">
                                                        </ext:Column>
                                                        <ext:Column runat="server" Header="Name" DataIndex="Name" Width="250">
                                                        </ext:Column>
                                                        <ext:Column runat="server" Header="Primary Home Address" DataIndex="Address" Width="250">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="Panel7" runat="server" Title="Collaterals">
                                        <Items>
                                            <ext:Toolbar ID="ToolbarCollateral" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteCollateral" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteCollateral_Click"/>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnEditCollateral" runat="server" Text="Edit" Icon="NoteEdit" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onBtnEditCollateral();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddCollateral" runat="server" Text="Add" Icon="Add">
                                                        <Menu>
                                                            <ext:Menu ID="Menu1" runat="server">
                                                                <Items>
                                                                    <ext:MenuItem ID="miBankAccount" runat="server" Text="Bank Account" Handler="onAddCollateral">
                                                                    </ext:MenuItem>
                                                                    <ext:MenuItem ID="miLand" runat="server" Text="Land" Handler="onAddCollateral">
                                                                    </ext:MenuItem>
                                                                    <ext:MenuItem ID="miMachine" runat="server" Text="Machine" Handler="onAddCollateral">
                                                                    </ext:MenuItem>
                                                                    <ext:MenuItem ID="miVehicle" runat="server" Text="Vehicle" Handler="onAddCollateral">
                                                                    </ext:MenuItem>
                                                                    <ext:MenuItem ID="miJewelry" runat="server" Text="Jewelry/Others" Handler="onAddCollateral">
                                                                    </ext:MenuItem>
                                                                </Items>
                                                            </ext:Menu>
                                                        </Menu>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:GridPanel ID="GridPanelCollaterals" runat="server" Height="118"
                                                AutoExpandColumn="Description"  ColumnLines="true" AutoScroll="true">
                                                <View>
                                                    <ext:GridView ID="GridView3" EmptyText="No collaterals to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                                </View>
                                                <Store>
                                                    <ext:Store ID="StoreCollaterals" runat="server">
                                                        <Reader>
                                                            <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                                <Fields>
                                                                    <ext:RecordField Name="AssetID" />
                                                                    <ext:RecordField Name="AssetTypeName" />
                                                                    <ext:RecordField Name="Description" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="SelectionModelCollaterals" SingleSelect="true" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="onRowSelected(btnDeleteCollateral);onRowSelected(btnEditCollateral);" />
                                                            <RowDeselect Handler="onRowDeselected(SelectionModelCollaterals, btnDeleteGuarantor);onRowDeselected(SelectionModelCollaterals, btnEditCollateral);" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server" Header="Collateral Type" DataIndex="AssetTypeName" Width="375">
                                                        </ext:Column>
                                                        <ext:Column runat="server" Header="Description" DataIndex="Description" Width="375">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="Panel9" runat="server" Title="Submitted Documents">
                                        <Items>
                                            <ext:Toolbar ID="ToolbarSubmittedDocuments" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteDocument" runat="server" Text="Delete" Icon="Delete"
                                                        Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteDocument_Click">
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnOpenDocument" runat="server" Text="Open" Icon="ApplicationViewIcons"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onBtnOpenDocument();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddDocument" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onBtnAddDocument();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:GridPanel ID="GridPanelSubmittedDocuments" runat="server" Height="116" AutoScroll="true"
                                                AutoExpandColumn="DocumentDescription"  ColumnLines="true" Border="false">
                                                <View>
                                                    <ext:GridView ID="GridView4" EmptyText="No documents to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                                </View>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="RowSelectionModelDocuments" SingleSelect="true" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="onRowSelected(btnDeleteDocument);onRowSelected(btnOpenDocument);" />
                                                            <RowDeselect Handler="onRowDeselected(RowSelectionModelDocuments, btnDeleteDocument);onRowDeselected(RowSelectionModelDocuments, btnOpenDocument);" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Store>
                                                    <ext:Store ID="StoreSubmittedDocuments" runat="server">
                                                        <Reader>
                                                            <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                                <Fields>
                                                                    <ext:RecordField Name="SubmittedDocumentID" />
                                                                    <ext:RecordField Name="ProductRequiredDocumentId" />
                                                                    <ext:RecordField Name="ProductRequiredDocumentName" />
                                                                    <ext:RecordField Name="DateSubmitted" />
                                                                    <ext:RecordField Name="Status" />
                                                                    <ext:RecordField Name="DocumentDescription" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server" Header="Document Name" DataIndex="ProductRequiredDocumentName" Width="188">
                                                        </ext:Column>
                                                        <ext:DateColumn runat="server" Header="Date Submitted" DataIndex="DateSubmitted" Width="188" Format="MMMM dd, yyyy">
                                                        </ext:DateColumn>
                                                        <ext:Column runat="server" Header="Status" DataIndex="Status" Width="188">
                                                        </ext:Column>
                                                        <ext:Column runat="server" Header="Description" DataIndex="DocumentDescription" Width="188">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelCheque" Title="Schedule and Cheques"
                                Border="false" Layout="RowLayout" AutoScroll="true">
                                <Items>
                                    <ext:RowLayout ID="RowLayout2" runat="server">
                                        <Rows>
                                            <ext:LayoutRow runat="server" RowHeight="0.5">
                                                <ext:Panel ID="Panel5" runat="server"><Items>
                                                <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Title="Amortization Schedule" AutoScroll="true"
                                                    Height="250" AutoExpandColumn="TotalPayment"  ColumnLines="true">
                                                    <View>
                                                        <ext:GridView ID="GridView1" EmptyText="No amortization schedule to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                                    </View>
                                                    <TopBar>
                                                        <ext:Toolbar ID="Toolbar2" runat="server">
                                                            <Items>
                                                                <ext:ToolbarSpacer />
                                                                <ext:ToolbarSpacer />
                                                                <ext:Checkbox ID="chckCheck" runat="server" LabelWidth="130" FieldLabel="Add Post Dated Checks" Checked="false">
                                                                </ext:Checkbox> 
                                                                <ext:ToolbarFill></ext:ToolbarFill>
                                                                <ext:ToolbarSpacer />
                                                                <ext:ToolbarSpacer />
                                                                <%--<ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                                                    Width="250" Editable="false" AllowBlank="false" LabelWidth="100" Vtype="daterange" StartDateField="datLoanReleaseDate"/>--%>
                                                                <ext:Button ID="btnGenerate" runat="server" Text="Generate Schedule" Icon="Calculator"
                                                                    Disabled="false">
                                                                    <DirectEvents>
                                                                        <Click OnEvent="btnGenerate_Click">
                                                                            <EventMask ShowMask="true" Msg="Generating amortization schedule..." />
                                                                        </Click>
                                                                    </DirectEvents>
                                                                </ext:Button>
                                                                <ext:ToolbarSpacer />
                                                                <%--<ext:ToolbarSeparator />--%>
                                                                <ext:ToolbarSpacer />
                                                                <ext:Button ID="btnPrintSchedule" runat="server" Text="Print Temporary Schedule" Hidden="true" Icon="Printer">
                                                                    <Listeners>
                                                                        <Click Handler="ontBtnPrintSchedule();" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Items>
                                                        </ext:Toolbar>
                                                    </TopBar>
                                                    <Store>
                                                        <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="Counter" />
                                                                        <ext:RecordField Name="ScheduledPaymentDate" Type="Date" />
                                                                        <ext:RecordField Name="PrincipalPayment" />
                                                                        <ext:RecordField Name="InterestPayment" />
                                                                        <ext:RecordField Name="TotalPayment" />
                                                                        <ext:RecordField Name="PrincipalBalance" />
                                                                        <ext:RecordField Name="TotalLoanBalance" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel runat="server" ID="RowSelectionModelAmor">
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                    <ColumnModel runat="server" ID="clmAmortizationSchedule" Width="100%">
                                                        <Columns>
                                                            <ext:Column Header="Unit" DataIndex="Counter" Wrap="true" Width="120" Sortable="false" />
                                                            <ext:DateColumn Header="Payment Due Date" DataIndex="ScheduledPaymentDate" Wrap="true"
                                                                Width="140" Sortable="false" />
                                                            <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true"
                                                                Width="140" Sortable="false" Format=",000.00" />
                                                            <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="140"
                                                                Sortable="false" Format=",000.00" />
                                                            <ext:NumberColumn Header="Total" DataIndex="TotalPayment" Wrap="true" Width="140"
                                                                Sortable="false" Format=",000.00" />
                                                            <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true"
                                                                Width="140" Sortable="false" Format=",000.00" />
                                                            <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                                Width="140" Sortable="false" Format=",000.00" Hidden="true"/>
                                                        </Columns>
                                                    </ColumnModel>
                                                </ext:GridPanel>
                                                </Items></ext:Panel>
                                            </ext:LayoutRow>
                                            <ext:LayoutRow runat="server" RowHeight="0.4">
                                                <ext:Panel ID="Panel6" runat="server" ><Items>
                                                <ext:GridPanel ID="grdpnlCheque" runat="server" AutoExpandColumn="BankName"
                                                    Height="250" AutoScroll="true" Title="Associated Checks" ColumnLines="true">
                                                    <View>
                                                        <ext:GridView EmptyText="No cheques to display" runat="server" DeferEmptyText="false"></ext:GridView>
                                                    </View>
                                                    <%--<Items>
                                                        <ext:Toolbar ID="Toolbar1" runat="server">
                                                            <Items>
                                                                <ext:Button ID="btnEditCheque" runat="server" Text="Edit" Icon="NoteEdit" Disabled="true">
                                                                    <Listeners>
                                                                        <Click Handler="FillDetails();" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Items>
                                                        </ext:Toolbar>
                                                    </Items>--%>
                                                    <Store>
                                                        <ext:Store ID="storeCheques" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                                    <Fields>
                                                                        <ext:RecordField Name="ChequeId" />
                                                                        <ext:RecordField Name="ChequeNumber" />
                                                                        <ext:RecordField Name="Amount" />
                                                                        <ext:RecordField Name="ChequeDate" />
                                                                        <ext:RecordField Name="BankName" />
                                                                        <ext:RecordField Name="BankId" />
                                                                        <ext:RecordField Name="Status" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="SelectionModelCheque" SingleSelect="false" runat="server">
                                                            <%--<Listeners>
                                                                <RowSelect Handler="#{btnEditCheque}.enable();"/>
                                                            </Listeners>--%>
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                    <ColumnModel>
                                                        <Columns>
                                                            <ext:Column runat="server" Header="Check Id" DataIndex="ChequeId" Width="150" Hidden="true">
                                                            </ext:Column>
                                                            <ext:Column runat="server" Header="Check Number" DataIndex="ChequeNumber" Width="150">
                                                            </ext:Column>
                                                            <ext:NumberColumn runat="server" Header="Amount" DataIndex="Amount" Width="150" Format=",000.00">
                                                            </ext:NumberColumn>
                                                            <ext:DateColumn runat="server" Header="Check Date" DataIndex="ChequeDate" Width="150"
                                                                Format="MMM dd yyyy">
                                                            </ext:DateColumn>
                                                            <ext:Column runat="server" Header="Check Date" DataIndex="_ChequeDate" Width="150" Hidden="true">
                                                            </ext:Column>
                                                            <ext:Column runat="server" Header="Bank Name" DataIndex="BankName" Width="150">
                                                            </ext:Column>
                                                            <ext:Column runat="server" Header="Bank Id" DataIndex="BankId" Width="150" Hidden="true">
                                                            </ext:Column>
                                                            <ext:Column runat="server" Header="Status" DataIndex="Status" Width="150">
                                                            </ext:Column>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <Listeners>
                                                        <RowDblClick Handler="FillDetails();" />
                                                    </Listeners>
                                                </ext:GridPanel>
                                                </Items></ext:Panel>
                                            </ext:LayoutRow>
                                        </Rows>
                                    </ext:RowLayout>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onFormValidated(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    <%--Add Cheque--%>
    <ext:Window runat="server" ID="wdwCheque" Collapsible="true" Height="223" Icon="Application"
        Title="Add Cheque" Width="490" Hidden="true" Modal="true" Resizable="false" AutoFocus="true">
        <Items>
            <ext:FormPanel ID="fpNewFee" runat="server" Padding="5" LabelWidth="190" Layout="FitLayout"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <%--Transaction Date--%>
                    <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Hidden="true" Width="400" ReadOnly="true"></ext:DateField>
                    <%--Amount--%>
                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" ReadOnly="true" DataIndex="TotalPayment" Width="400" AllowBlank="false" MaskRe="[0-9\.\,]">
                        <Listeners>
                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                        </Listeners>
                    </ext:TextField>
                    <%--Payment Method--%>
                    <ext:TextField ID="txtPaymentMethod" runat="server" FieldLabel="Check Payment Method" Width="400" ReadOnly="true" AllowBlank="false" Editable="false" >
                    </ext:TextField>
                    <%--BankPickList--%>
                    <ext:CompositeField ID="CompositeField2" runat="server">
                        <Items>
                            <ext:TextField ID="txtBank" runat="server" ReadOnly="true" Width="205" FieldLabel="Bank" AllowBlank="false"/>
                            <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                            <Listeners><Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:Checkbox ID="chckBank" runat="server" FieldLabel="Use this bank for all cheques?" Checked="true">
                        <Listeners>
                            <Check Handler="setSameBank();"/>
                        </Listeners>
                    </ext:Checkbox>
                    <ext:Hidden ID="hdnBankID" runat="server"/>
                    <%--Cheque Number--%>
                    <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="400" AllowBlank="false" EnableKeyEvents="true">
                        <Listeners>
                            <KeyUp Handler="onChequeFormValidated();"/>
                            <%--<Change Handler="onChequeFormValidated();" />--%>
                        </Listeners>
                    </ext:TextField>
                    <%--Cheque Status--%>
                    <ext:TextField runat="server" ID="txtChequeStatus" FieldLabel="Check Status" Width="400" ReadOnly="true" Hidden="true"></ext:TextField>
                    <%--Cheque Remarks--%>
                    <ext:TextArea ID="txtaCheckRemarks" runat="server" FieldLabel="Remarks" Width="400" Hidden="true"/>
                    <%--Cheque Date--%>
                    <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="400" ReadOnly="true" AllowBlank="false"/>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBarCheque" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveNewCheque" runat="server" Text="Save" Icon="Disk" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSaveNewCheque_Click" Success="resetCheque();"/>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelNewFee" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="resetCheque();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onChequeFormValidated();"/>
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
