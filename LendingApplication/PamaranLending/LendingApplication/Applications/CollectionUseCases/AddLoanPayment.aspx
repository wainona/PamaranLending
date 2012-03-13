<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLoanPayment.aspx.cs"
    Inherits="LendingApplication.Applications.CollectionUseCases.AddLoanPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickcustomer', 'updatewaive', 'updaterebate', 'manualbill', 'onprintloanpaymentform', 'closeloanpaymentform', 'closemanualbilling', 'onpickcheck', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var visitAllTabs = function () {
            for (count = 0; count < 3; count++) {
                PageTabPanel.setActiveTab(count);
            }
            PageTabPanel.setActiveTab(0);
        };

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                hiddenCustomerID.setValue(msg.data.CustomerID);
                txtCustomerName.setValue(msg.data.Names);
                var loadMask = new Ext.LoadMask(RootFormPanel.body, { msg: 'Retrieving Customer Details...' });
                loadMask.show();
                X.FillCustomer(msg.data.CustomerID, {
                    success: function () {
                        loadMask.hide();
                        calculateTotalChequesCash();
                    }
                });

            } else if (msg.tag == 'onpickcheck') {
                X.AddChequesFromPickList(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data._CheckDate, msg.data.TotalAmount, {
                    success: function () {
                        txtTotalAmount.setValue("0");
                        txtTotalAmount2.setValue("0");
                        calculateTotalChequesCash();
                    }
                });
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, {
                    success: function () {
                        txtTotalAmount.setValue("0");
                        txtTotalAmount2.setValue("0");
                        calculateTotalChequesCash();
                    }
                });

            } else if (msg.tag == 'bankselected') {
                txtBankName.setValue(msg.data.OrganizationName);
                hiddenbankBranch.setValue(msg.data.BranchName);
                hiddenBankID.setValue(msg.data.PartyRoleId);
            } else if (msg.tag == 'updatewaive') {
                txtTotalAmount.setValue("0");
                txtTotalAmount2.setValue("0");
                calculateTotalChequesCash();
                BorrowerSelectionModel.clearSelections();
                CoBorrowerSelectionModel.clearSelections();
                X.UpdateWaive();
            } else if (msg.tag == 'updaterebate') {
                txtTotalAmount.setValue("0");
                txtTotalAmount2.setValue("0");
                calculateTotalChequesCash();
                BorrowerSelectionModel.clearSelections();
                CoBorrowerSelectionModel.clearSelections();
                X.UpdateRebate();
            } else if (msg.tag == 'manualbill') {
                txtTotalAmount.setValue("0");
                txtTotalAmount2.setValue("0");
                calculateTotalChequesCash();
                BorrowerSelectionModel.clearSelections();
                CoBorrowerSelectionModel.clearSelections();
                var x = msg.data.bill;
                MB.setValue(x);
                MB2.setValue(x);
                X.UpdateBill();
                if (hiddenMB.getValue() == 1 && MB.getValue() == 1)
                    btnManualBillingBorrower.disable();
                if (hiddenMB2.getValue() == 1 && MB2.getValue() == 1)
                    btnManualBilling2.disable();
            } else if (msg.tag == 'closeloanpaymentform') {
                RootFormPanel.enable();
            } else if (msg.tag == 'closemanualbilling') {
                btnManualBillingBorrower.enable();
                btnManualBilling2.enable();
            }
        };

        var formatCurrency = function (txt) {

            var num = txt.getValue();
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
            var answer = (((sign) ? '' : '-') + num + '.' + cents);
            txt.setValue(String(answer));
        }


        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added loan payment record.', function () {
                var data = {};
                data.id = hiddenPaymentId.getValue();
                window.proxy.sendToAll(data, 'addcollection');
            });
        };

        var onPickCheque = function () {
            X.FillCustomerId({
                success: function (result) {
                    if (result == 1) {
                        var url = '/Applications/CollectionUseCases/CustomerChecksPickList.aspx';
                        var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
                        var param = url + guid;
                        window.proxy.requestNewTab('CustomerChecksPickList', param, 'Customer Checks Pick List');
                    }
                }
            });
        }

        var onAddCheque = function () {
            var url = '/Applications/CollectionUseCases/AddCheques.aspx';
            var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
            var param = url + guid;
            window.proxy.requestNewTab('AddCheque', param, 'Add Cheque');
        }

        var onPickCustomer = function () {
            window.proxy.requestNewTab('CustomerPickList', '/Applications/CollectionUseCases/CustomerWithLoanPickList.aspx?mode=single', 'Customer Pick List');
        };

        var ManageBillingClick = function (url, randomkey, roletype) {
            var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
            param += "&RandomKey=" + randomkey;
            param += "&RoleType=" + roletype;
            window.proxy.requestNewTab('GenerateInterest', param, 'Generate Interest');
        };

        var onCoBorrowerManualBillingClick = function () {
            hiddenMB2.setValue(parseFloat(1));
            var url = '/Applications/CollectionUseCases/ManualBilling.aspx';
            var roletype = 'Guarantor';
            var data = CoBorrowerSelectionModel.getSelected().json;
            if (data != null)
                ManageBillingClick(url, data.RandomKey, roletype);

        };

        var onBorrowerManualBillingClick = function () {
            hiddenMB.setValue(parseFloat(1));
            var url = '/Applications/CollectionUseCases/ManualBilling.aspx';
            var roletype = 'Borrower';
            var data = BorrowerSelectionModel.getSelected().json;
            if (data != null)
                ManageBillingClick(url, data.RandomKey, roletype);
        };

        var BorrowerWaiveClick = function () {
            var url = '/Applications/CollectionUseCases/Waive.aspx';
            var data = BorrowerSelectionModel.getSelected().json;
            var roletype = 'Borrower';
            if (data != null)
                ManageWaive(url, data.RandomKey, roletype);
        };


        var ManageWaive = function (url, randomkey, roletype) {
            var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
            param += "&RandomKey=" + randomkey;
            param += "&RoleType=" + roletype;
            window.proxy.requestNewTab('BorrowerWaive', param, 'Waive Borrower');

        };
        var CoBorrowerWaiveClick = function () {
            var url = '/Applications/CollectionUseCases/Waive.aspx';
            var roletype = 'Guarantor';
            var data = CoBorrowerSelectionModel.getSelected().json;
            if (data != null)
                ManageWaive(url, data.RandomKey, roletype);
        };

        var ManageRebate = function (url, randomkey, roletype) {
            var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
            param += "&RandomKey=" + randomkey;
            param += "&RoleType=" + roletype;
            window.proxy.requestNewTab('BorrowerRebate', param, 'Rebate Borrower');
        };

        var onRebateClick = function () {
            var url = '/Applications/CollectionUseCases/Rebate.aspx';
            var roletype = 'Borrower';
            var data = BorrowerSelectionModel.getSelected().json;
            if (data != null)
                ManageRebate(url, data.RandomKey, roletype);
        };

        var payinFull = function (selectionModel, btnPay, btnReset) {
            var data = selectionModel.getSelected();
            var totalAmountDue = data.json.TotalAmountDue;
            var paymentAmount = data.json.PaymentAmount;
            var index = selectionModel.getSelectedIndex();
            var data1 = data.store.getAt(index);
            var currentAmount = data1.get('PaymentAmount');
            var currentAmountDue = data1.get('TotalAmountDue');
            var amountTendered = txtamountTendered.getValue();
            var totalAmount = txtTotalAmount.getValue();
            amountTendered = amountTendered.replace(/,/g, '');
            var change = txtChange.getValue();
            change = change.replace(/,/g, '');
            if (parseFloat(change) >= parseFloat(totalAmountDue)) {
                paymentAmount = totalAmountDue;
            } else {
                var temp = parseFloat(currentAmountDue) - parseFloat(currentAmount);
                if (parseFloat(change) >= parseFloat(temp))
                    paymentAmount = parseFloat(temp) + parseFloat(currentAmount);
                else
                    paymentAmount = parseFloat(change) + parseFloat(currentAmount);
            }
            data.set('PaymentAmount', paymentAmount);
            if (txtTotalAmount == 0) {
                btnSave.disable();
            }
            btnReset.enable();
            btnPay.disable();
            total();
        }

        var resetPayment = function (selectionModel, btnPay, btnReset) {
            var data = selectionModel.getSelected();
            var paymentAmount = data.json.PaymentAmount;
            var totalAmount = txtTotalAmount.getValue();
            btnPay.disable();
            btnReset.disable();
            paymentAmount = 0;
            data.set('PaymentAmount', paymentAmount);
            total();
            onRowSelectedBorrower();
            onRowSelectedCoBorrower();
        }

        Ext.grid.RowSelectionModel.override({
            getSelectedIndex: function () {
                return this.grid.store.indexOf(this.selections.itemAt(0));
            }
        });

        var loopGridContents = function () {
            for (var j = 0; j < GridPanelBorrower.store.getCount(); j++) {
                var data = GridPanelBorrower.store.getAt(j);
                payment = data.get('PaymentAmount');
            }
        }

        var totalReceivablesforBorrower = function () {
            totalReceivables(GridPanelBorrower, hiddenTotalBorrowerPrincipal, hiddenTotalBorrowerInterest, hiddenTotalBorrowerPastDue, hiddenTotalBorrowerTotalAmountDue);
        }

        var totalReceivablesforCoBorrower = function () {
            totalReceivables(GridPanelGuarantors, hiddenTotalCoBorrowerPrincipal, hiddenTotalCoBorrowerInterest, hiddenTotalCoBorrowerPastDue, hiddenTotalCoBorrowerTotalAmountDue);
        }

        var totalReceivables = function (grid, hprincipalDue, hinterestDue, hpastDue, htotalAmountDue) {
            var principalDue = 0;
            var interestDue = 0;
            var pastDue = 0;
            var totalAmountDue = 0;
            var paymentAmount = 0;
            var paymentAmountPerRow = 0;
            var semiTotal = 0;
            var partial = 0;
            for (var j = 0; j < grid.store.getCount(); j++) {
                var data = grid.store.getAt(j);
                paymentAmountPerRow = data.get('PaymentAmount');
                if (paymentAmountPerRow != 0) {
                    principalDue += data.get('PrincipalDue');
                    interestDue += data.get('InterestDue');
                    pastDue += data.get('PastDue');
                    totalAmountDue += data.get('TotalAmountDue');
                    paymentAmount += paymentAmountPerRow;

                    if (parseFloat(paymentAmount) > (pastDue)) {
                        semiTotal = parseFloat(paymentAmount) - parseFloat(pastDue);
                        if (semiTotal != 0) {
                            pastDue = pastDue;
                            paymentAmount = semiTotal;
                        } else {
                            pastDue = semiTotal;
                            paymentAmount = semiTotal;
                        }
                    } else if (paymentAmount != 0) {
                        pastDue = parseFloat(pastDue) - parseFloat(paymentAmount);
                        pastDue = paymentAmount;
                        paymentAmount = 0;
                    } else {
                        pastDue = 0;
                    }

                    if (parseFloat(paymentAmount) > parseFloat(interestDue)) {
                        semiTotal = parseFloat(paymentAmount) - parseFloat(interestDue);
                        if (semiTotal != 0) {
                            interestDue = interestDue;
                            paymentAmount = semiTotal;
                        }
                        else {
                            interestDue = semiTotal;
                            paymentAmount = semiTotal;
                        }
                    } else if (paymentAmount != 0) {
                        interestDue = parseFloat(interestDue) - parseFloat(paymentAmount);
                        interestDue = paymentAmount;
                        paymentAmount = 0;
                    } else {
                        interestDue = 0;
                    }

                    if (parseFloat(paymentAmount) > parseFloat(principalDue)) {
                        semiTotal = parseFloat(paymentAmount) - parseFloat(principalDue);
                        if (semiTotal != 0)
                            principalDue = principalDue;
                        else
                            principalDue = semiTotal;
                    } else if (paymentAmount != 0) {
                        principalDue = parseFloat(principalDue) - parseFloat(paymentAmount);
                        principalDue = paymentAmount;
                        paymentAmount = 0;
                    } else {
                        principalDue = 0;
                    }
                }
            }
            hprincipalDue.setValue(principalDue);
            hinterestDue.setValue(interestDue);
            hpastDue.setValue(pastDue);
            htotalAmountDue.setValue(totalAmountDue);
        }

        var totalTotalReceivables = function () {
            totalReceivablesforBorrower();
            totalReceivablesforCoBorrower();
            var secret = txtTotalAmount.getValue();
            var totalAmountDue = 0;
            var BorrowerPrincipal = hiddenTotalBorrowerPrincipal.getValue();
            var BorrowerInterest = hiddenTotalBorrowerInterest.getValue();
            var BorrowerPastDue = hiddenTotalBorrowerPastDue.getValue();
            var borrowerTotals = parseFloat(BorrowerPrincipal) + parseFloat(BorrowerInterest) + parseFloat(BorrowerPastDue);
            var BorrowerTotalAmountDue = hiddenTotalBorrowerTotalAmountDue.getValue();
            var CoBorrowerPrincipal = hiddenTotalCoBorrowerPrincipal.getValue();
            var CoBorrowerInterest = hiddenTotalCoBorrowerInterest.getValue();
            var CoBorrowerPastDue = hiddenTotalCoBorrowerPastDue.getValue();
            var coBorrowerTotals = parseFloat(CoBorrowerPrincipal) + parseFloat(CoBorrowerInterest) + parseFloat(CoBorrowerPastDue);
            var CoBorrowerTotalAmountDue = hiddenTotalCoBorrowerTotalAmountDue.getValue();

            principalDue = parseFloat(BorrowerPrincipal) + parseFloat(CoBorrowerPrincipal);
            interestDue = parseFloat(BorrowerInterest) + parseFloat(CoBorrowerInterest);
            pastDue = parseFloat(BorrowerPastDue) + parseFloat(CoBorrowerPastDue);
            totalAmountDue = parseFloat(BorrowerTotalAmountDue) + parseFloat(CoBorrowerTotalAmountDue);

            hiddenTotalPrincipal.setValue(principalDue);
            hiddenTotalInterest.setValue(interestDue);
            hiddenTotalPastDue.setValue(pastDue);
            hiddenTotalTotalAmountDue.setValue(secret);
            hiddenBorrowerTotals.setValue(borrowerTotals);
            hiddenCoBorrowerTotals.setValue(coBorrowerTotals);
        }


//        var totalTotalAmountDue = function (grid) {
//            var payment = 0;
//            for (var j = 0; j < grid.store.getCount(); j++) {
//                var data = grid.store.getAt(j);
//                payment += data.get('TotalAmountDue');
//            }
//        }

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
        }

        var onRowSelectedBorrower = function () {
            if (BorrowerSelectionModel.hasSelection() == true) {
                var data = BorrowerSelectionModel.getSelected();
                var paymentAmount = data.json.PaymentAmount;
                var amountTendered = txtamountTendered.getValue();
                var index = BorrowerSelectionModel.getSelectedIndex();
                var data1 = data.store.getAt(index);
                payment = data1.get('PaymentAmount');
                amountDue = data1.get('TotalAmountDue');
                principalDue = data1.get('PrincipalDue');
            }

            if (amountTendered == "") {
                btnPayinFullBorrower.disable();
            } else if (txtChange.getValue() == 0 && payment == 0) {
                btnPayinFullBorrower.disable();
                btnResetPaymentBorrower.disable();
            } else if (payment == 0) {
                btnPayinFullBorrower.enable();
                btnResetPaymentBorrower.disable();
            } else if (amountDue > payment) {
                btnPayinFullBorrower.enable();
            } else {
                btnPayinFullBorrower.disable();
                btnResetPaymentBorrower.enable();
            }

            btnManualBillingBorrower.enable();
            btnWaiveBorrower.enable();
            btnRebate.enable();
            btnViewInterest.enable();
        };

        var onRowDeselectedBorrower = function () {
            btnManualBillingBorrower.disable();
            btnWaiveBorrower.disable();
            btnViewInterest.disable();

        };
        var onRowDeselectedCoBorrower = function () {
            btnWaive2.disable();
            btnManualBilling2.disable();
            btnCoborInterestBreakdown.disable();
        };
        var onRowSelectedCoBorrower = function () {
            if (CoBorrowerSelectionModel.hasSelection() == true) {
                var data = CoBorrowerSelectionModel.getSelected();
                var paymentAmount = data.json.PaymentAmount;
                var amountTendered = txtamountTendered.getValue();
                var index = CoBorrowerSelectionModel.getSelectedIndex();
                var data1 = data.store.getAt(index);
                payment = data1.get('PaymentAmount');
                amountDue = data1.get('TotalAmountDue');
                principalDue = data1.get('PrincipalDue');
            }

            if (amountTendered == "") {
                btnPayinFullCoBorrower.disable();
            } else if (txtChange.getValue() == 0 && payment == 0) {
                btnPayinFullCoBorrower.disable();
                btnResetPaymentCoBorrower.disable();
            } else if (payment == 0) {
                btnPayinFullCoBorrower.enable();
                btnResetPaymentCoBorrower.disable();
            } else {
                btnPayinFullCoBorrower.disable();
                btnResetPaymentCoBorrower.enable();
            }
            btnManualBilling2.enable();
            //            if (parseFloat(principalDue) == 0)
            //                btnManualBilling2.enable();
            //            else
            //                btnManualBilling2.disable();
            btnWaive2.enable();
            btnCoborInterestBreakdown.enable();
        };
        var beforeEditBorrower = function (e) {
            var changeStr = txtChange.getValue();
            changeStr = changeStr.replace(/,/g, '');
            var change = Ext.util.Format.number(changeStr, '');
            var totalamountdue = e.record.data.TotalAmountDue;

            if (change <= parseFloat(totalamountdue))
                numPaymentAmount.maxValue = change;
            else
                numPaymentAmount.maxValue = totalamountdue;
        };
        var beforeEditCoBorrower = function (e) {
            //numPaymentAmount2.maxValue = e.record.data.TotalAmountDue;
            var changeStr = txtChange.getValue();
            changeStr = changeStr.replace(/,/g, '');
            var change = Ext.util.Format.number(changeStr, '');
            var totalamountdue = e.record.data.TotalAmountDue;

            if (change <= parseFloat(totalamountdue))
                numPaymentAmount2.maxValue = change;
            else
                numPaymentAmount2.maxValue = totalamountdue;
        };

        var resetPaymentTotals = function (grid) {
            var totalAmount = txtTotalAmount.getValue();
            var amountTendered = txtamountTendered.getValue();

            if (amountTendered > totalAmount)
                return;

            for (var j = 0; j < grid.store.getCount(); j++) {
                var data = grid.store.getAt(j);
                data.set('PaymentAmount', 0);
            }

            total();
        }

        var calculateSemiTotal = function (grid, totalAmount) {
            var bg = grid.view.getColumnData();
            var total = 0;

            for (var j = 0, jlen = grid.store.getCount(); j < jlen; j++) {
                var r = grid.store.getAt(j);
                b = bg[6];
                total += parseFloat(r.get(b.name));
            }
            totalAmount.setValue(total)
        };


        var total = function () {
            calculateSemiTotal(GridPanelBorrower, hiddenTotalAmount);
            calculateSemiTotal(GridPanelGuarantors, hiddenTotalAmount1);
            var amountTendered = txtamountTendered.getValue();
            amountTendered = amountTendered.replace(/,/g, '');
            var borrower = hiddenTotalAmount.getValue();
            borrower = borrower.replace(/,/g, '');
            var cobborower = hiddenTotalAmount1.getValue();
            cobborower = cobborower.replace(/,/g, '');
            var total = parseFloat(borrower) + parseFloat(cobborower);
            var change = parseFloat(amountTendered) - total;
            txtTotalAmount.setValue(String(total));
            txtTotalAmount2.setValue(String(total));
            txtChange.setValue(String(change));
            txtChange2.setValue(String(change));
            formatCurrency(txtChange);
            formatCurrency(txtChange2);
            formatCurrency(txtamountTendered);
            formatCurrency(txtTotalAmount);
            formatCurrency(txtTotalAmount2);
            if (txtTotalAmount.getValue() == 0) {
                txtAtik.allowBlank = false;
            } else if (txtAtik.getValue() == "1") {
                txtAtik.allowBlank = true;
            } else {
                txtAtik.allowBlank = true;
            }
        };


        var paymentCheckingBorrower = function () {
            if (BorrowerSelectionModel.hasSelection() == true) {
                var data = BorrowerSelectionModel.getSelected();
                var paymentAmount = data.json.PaymentAmount;
                var amountTendered = txtamountTendered.getValue();
                var index = BorrowerSelectionModel.getSelectedIndex();
                var data1 = data.store.getAt(index);
                var change = txtChange.getValue();
                var payment = data1.get('PaymentAmount');
            }

            if (amountTendered == "" || amountTendered == 0) {
                btnPayinFullBorrower.disable();
                btnResetPaymentBorrower.disable();
            } else if (payment == 0 && change != 0) {
                btnPayinFullBorrower.enable();
                btnResetPaymentBorrower.disable();
            } else {
                btnPayinFullBorrower.disable();
                btnResetPaymentBorrower.enable();
            }
        }

        var paymentCheckingCoBorrower = function () {
            if (CoBorrowerSelectionModel.hasSelection() == true) {
                var data = CoBorrowerSelectionModel.getSelected();
                var paymentAmount = data.json.PaymentAmount;
                var amountTendered = txtamountTendered.getValue();
                var index = CoBorrowerSelectionModel.getSelectedIndex();
                var data1 = data.store.getAt(index);
                var change = txtChange2.getValue();
                var payment = data1.get('PaymentAmount');
            }

            if (amountTendered == "" || amountTendered == 0) {
                btnPayinFullCoBorrower.disable();
                btnResetPaymentCoBorrower.disable();
            } else if (payment == 0 && change != 0) {
                btnPayinFullCoBorrower.enable();
                btnResetPaymentCoBorrower.disable();
            } else {
                btnPayinFullCoBorrower.disable();
                btnResetPaymentCoBorrower.enable();
            }
        }

        var changeChecker = function (btnFull, selectionModel) {
            if (selectionModel.hasSelection() == true) {
                var data = selectionModel.getSelected();
                var paymentAmount = data.json.PaymentAmount;
                var totalAmountDue = data.json.TotalAmountDue;
                var index = selectionModel.getSelectedIndex();
                var change = txtChange.getValue();
                change = change.replace(/,/g, '');
                data1 = data.store.getAt(index);
                var payment = data1.get('PaymentAmount');
                var amountDue = data.get('TotalAmountDue');
            }
            if (txtChange.getValue() == 0) {
                btnFull.disable();
            } else if (amountDue > payment) {
                btnFull.enable();
            }
        }

        var PaymentMethodSelect = function () {
            if (cmbPaymentMethod.getText() == 'Cash' || cmbPaymentMethod.getText() == 'ATM') {
                PanelForPayCheck.hide();
                hiddenBankID.setValue('-1');
                txtCheckNumber.allowBlank = true;
                txtBankName.allowBlank = true;
                datCheckDate.allowBlank = true;
                btnPickBank.hide();
            } else {
                PanelForPayCheck.show();
                btnPickBank.show();
                txtCheckDate.hide();
                txtCheckNumber.allowBlank = false;
                txtBankName.allowBlank = false;
                datCheckDate.allowBlank = false;
                datCheckDate.show();
                txtCheckNumber.validate();
                txtBankName.validate();
                datCheckDate.validate();
            }
            markIfRequired(txtCheckNumber);
            markIfRequired(txtBankName);
            markIfRequired(datCheckDate);

        }

        var DeleteCheck = function () {
            var sm = gridPanelChecks.getSelectionModel();
            var sel = sm.getSelections();
            for (i = 0; i < sel.length; i++) {
                gridPanelChecks.store.remove(sel[i]);
            }
        }

        var totalChequesAmount = function () {
            var amount = 0;
            for (var j = 0; j < gridPanelChecks.store.getCount(); j++) {
                var data = gridPanelChecks.store.getAt(j);
                amount += data.get('Amount');
            }

            hiddenTotalChequesAmount.setValue(amount);
        }

        var convertToNumber = function (control) {
            var cashAmount = control.getValue();
            cashAmount = cashAmount.replace(/,/g, '');
            if (cashAmount == "")
                cashAmount = 0;

            return parseFloat(cashAmount);
        }

        var calculateTotalChequesCash = function () {
            totalChequesAmount();
            var cashAmount = convertToNumber(txtCashPayment);
            var cashBalanceFromReceipts = convertToNumber(txtBalanceCashReceipts);
            var checkAmount = hiddenTotalChequesAmount.getValue();
            var atmAmount = txtATMAmount.getValue();
            var totalamountTendered = cashAmount + parseFloat(checkAmount) + cashBalanceFromReceipts +parseFloat(atmAmount);
            txtamountTendered.setValue(totalamountTendered);
            formatCurrency(txtamountTendered);
            total();
        }

        var onFocusCash = function () {
            if (txtCashPayment.readOnly == false) {
                cashDenomination.show();
            }
        }

        var sumTotalCash = function () {
            var totalAll = 0;

            var total1000 = 1000 * txt1000Bills.getValue();
            //            txtAmount.setValue(total1000);
            var total500 = 500 * txt500Bills.getValue();
            //            txtAmount.setValue(total500);
            var total200 = 200 * txt200Bills.getValue();
            //            txtAmount.setValue(total200);
            var total100 = 100 * txt100Bills.getValue();
            //            txtAmount.setValue(total100);
            var total50 = 50 * txt50Bills.getValue();
            //            txtAmount.setValue(total50);
            var total20 = 20 * txt20Bills.getValue();
            //            txtAmount.setValue(total20);
            var totalCoins = 1 * txtCoins.getValue();
            //            txtAmount.setValue(totalCoins);

            var totalAll = (total1000 + total500 + total200 + total100 + total50 + total20 + totalCoins);
            txtAmount.hide();
            txtAmount.setValue(totalAll);
            txtAmount.show();
        };

        /****ATM PAYMENTS******/
        var calculateATMTotal = function () {
            var amount = 0;
            for (var j = 0; j < grdPnlATM.store.getCount(); j++) {
                var data = grdPnlATM.store.getAt(j);
                amount += data.get('Amount');
            }
            txtATMAmount.setValue(amount);
        };

        var saveATMSuccessful = function () {
            calculateATMTotal();
            wnAddATM.hide();
            txtATMReferenceNum.setValue("");
            txtATMAmount1.setValue(0);
            calculateTotalChequesCash();
        };
        var cancelATM = function () {
            wnAddATM.hide();
            txtATMReferenceNum.setValue("");
            txtAmount.setValue("");
        };
        var OnBtnAddATM = function () {
            HiddenRandomKey2.setValue("");
            txtATMReferenceNum.setValue("");
            txtATMAmount1.setValue(0);
            btnSaveATM.disable();
            wnAddATM.show();
        }

        var OnBtnEditATM = function () {
            if (grdPnlATM.hasSelection() == true) {
                var data = grdPnlATMSelectionModel.getSelected().json;
                HiddenRandomKey2.setValue(data.RandomKey);
                txtATMAmount1.setValue(data.Amount);
                txtATMReferenceNum.setValue(data.ATMReferenceNumber)
                wnAddATM.show();
            }
        }
        var onFormValidated3 = function (valid) {
            var amount = txtATMAmount1.getValue();
            amount = amount.replace(/,/g, '');
            if (valid && amount > 0) {
                StatusBar3.setStatus({ text: 'Form is valid. ' });
                btnSaveATM.enable();
            } else if (valid) {
                StatusBar3.setStatus({ text: 'Amount must be >0.00. ' });
                btnSaveATM.disable();
            } else if (!valid) {
                StatusBar3.setStatus({ text: 'Please fill out the form.' });
                btnSaveATM.disable();
            }
        }
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
        }

        var closeInterestBreakdown = function () {
            wndInterestBreakdown.hide();
        }


        var onBorrowerInterestBreakdown = function () {
            if (BorrowerSelectionModel.hasSelection() == true) {
                var data = BorrowerSelectionModel.getSelected().json;
                X.FillInterestBreakdown(data.RandomKey, {
                    success: function () {
                        wndInterestBreakdown.show();
                    }
                });
            }
        }

        var onCoBorrowerInterestBreakdown = function () {
            if (CoBorrowerSelectionModel.hasSelection() == true) {
                var data = CoBorrowerSelectionModel.getSelected().json;
                X.FillInterestBreakdown(data.RandomKey, {
                    success: function () {
                        wndInterestBreakdown.show();
                    }
                });
            }
        }

    
    </script>
    <style id="Style1" type="text/css" runat="server">
        .bold .x-btn-text
        {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="RootFormPanel" runat="server" Layout="FitLayout" Border="false"
                MonitorValid="true">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Disabled="true" Text="Save" Icon="Disk">
                                <Listeners>
                                    <Click Handler="totalTotalReceivables();" />
                                </Listeners>
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();">
                                        <EventMask ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Borrowers" Value="Ext.encode(#{GridPanelBorrower}.getRowsValues({selectedOnly : false}))"
                                                Mode="Raw" />
                                            <ext:Parameter Name="Guarantors" Value="Ext.encode(#{GridPanelGuarantors}.getRowsValues({selectedOnly : false}))"
                                                Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                 <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TabPanel ID="PageTabPanel" runat="server" EnableTabScroll="true" Padding="0"
                        HideBorders="true" BodyStyle="background-color:transparent" DeferredRender="false">
                        <Items>
                            <ext:Panel runat="server" ID="PanelVoucher" Layout="FormLayout" Title="Voucher" Border="false"
                                Padding="5" AutoScroll="true">
                                <Items>
                                    <ext:Panel ID="Panel1" runat="server" LabelWidth="180" Layout="FormLayout" Border="false"
                                        ColumnWidth=".8" AutoHeight="true">
                                        <Items>
                                            <ext:Hidden ID="hiddenMB" runat="server" />
                                            <ext:Hidden ID="MB" runat="server" />
                                            <ext:Hidden ID="hiddenMB2" runat="server" />
                                            <ext:Hidden ID="MB2" runat="server" />
                                            <ext:Hidden ID="hiddenPaymentId" runat="server" />
                                            <ext:TextField ID="txtAtik" Hidden="true" runat="server" />
                                            <ext:Hidden ID="hiddenTotalChequesAmount" runat="server" />
                                            <ext:Hidden ID="hiddenPaymentMethod" runat="server" />
                                            <ext:Hidden ID="hiddenbankBranch" runat="server" />
                                            <ext:Hidden ID="hiddenTotalBorrowerPrincipal" runat="server" />
                                            <ext:Hidden ID="hiddenTotalBorrowerInterest" runat="server" />
                                            <ext:Hidden ID="hiddenTotalBorrowerPastDue" runat="server" />
                                            <ext:Hidden ID="hiddenTotalBorrowerTotalAmountDue" runat="server" />
                                            <ext:Hidden ID="hiddenTotalCoBorrowerPrincipal" runat="server" />
                                            <ext:Hidden ID="hiddenTotalCoBorrowerInterest" runat="server" />
                                            <ext:Hidden ID="hiddenTotalCoBorrowerPastDue" runat="server" />
                                            <ext:Hidden ID="hiddenTotalCoBorrowerTotalAmountDue" runat="server" />
                                            <ext:Hidden ID="hiddenBorrowerTotals" runat="server" />
                                            <ext:Hidden ID="hiddenCoBorrowerTotals" runat="server" />
                                            <ext:Hidden ID="hiddenTotalPrincipal" runat="server" />
                                            <ext:Hidden ID="hiddenTotalInterest" runat="server" />
                                            <ext:Hidden ID="hiddenTotalPastDue" runat="server" />
                                            <ext:Hidden ID="hiddenTotalTotalAmountDue" runat="server" />
                                            <ext:Hidden ID="hiddenPA" runat="server" />
                                            <ext:Hidden ID="hiddenResourceGUID" runat="server" />
                                            <ext:Hidden ID="hiddenCustomerID" runat="server" />
                                            <ext:Hidden ID="hiddenAmountTendered" runat="server" />
                                            <ext:Hidden ID="hiddenTotalAmount" runat="server" />
                                            <ext:Hidden ID="txtCustomerId" runat="server" />
                                            <ext:Hidden ID="txtATMAmount" runat="server" />
                                            <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="50%">
                                                <Items>
                                                    <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Name" AllowBlank="false"
                                                        ReadOnly="true" Flex="1">
                                                    </ext:TextField>
                                                    <ext:Button ID="btnPickCustomer" runat="server" Text="Browse..." Icon="Zoom">
                                                        <Listeners>
                                                            <Click Handler="onPickCustomer();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtDistrict" runat="server" FieldLabel="District" ReadOnly="true"
                                                AnchorHorizontal="50%" />
                                            <ext:TextField ID="txtStationNumber" runat="server" FieldLabel="Station Number" ReadOnly="true"
                                                AnchorHorizontal="50%" />
                                            <ext:DateField ID="datTransactionDate" runat="server" FieldLabel="Transaction Date"
                                                AnchorHorizontal="50%" AllowBlank="false" Editable="false" />
                                            <ext:Panel ID="PanelForCash" Title="Cash Payment" Height="105" AnchorHorizontal="50%"
                                                runat="server" Layout="FormLayout" Padding="5" Border="true">
                                                <Items>
                                                    <ext:TextField ID="txtBalanceCashReceipts" runat="server" Text="0.00" AnchorHorizontal="100%"
                                                        FieldLabel="Balance of Cash Receipts" EnableKeyEvents="true" ReadOnly="true">
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtCashPayment" runat="server" Text="0.00" AnchorHorizontal="100%"
                                                        FieldLabel="Amount (Php)" EnableKeyEvents="true">
                                                        <Listeners>
                                                            <Blur Handler="formatCurrency(txtCashPayment);" />
                                                            <%--<KeyUp Handler="calculateTotalChequesCash();" />--%>
                                                            <Focus Handler="onFocusCash();" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="PanelForPayCheck" Title="Check Payment" AnchorHorizontal="95%" runat="server"
                                                Layout="FormLayout" Border="false" Height="260">
                                                <Items>
                                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                                        <Items>
                                                            <ext:Button ID="btnAddCheck" Text="Add" Disabled="true" runat="server" Icon="Add">
                                                                <Listeners>
                                                                    <Click Handler="onAddCheque();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Button ID="btnBrowseCheck" Text="Browse..." Disabled="true" runat="server" Icon="Zoom">
                                                                <Listeners>
                                                                    <Click Handler="onPickCheque();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                            <ext:Button ID="btnDeleteCheck" Text="Delete" runat="server" Disabled="true" Icon="Delete">
                                                                <DirectEvents>
                                                                    <Click OnEvent="btnDeleteCheques_Click" Success="calculateTotalChequesCash();" />
                                                                </DirectEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:GridPanel ID="gridPanelChecks" runat="server" Height="200" EnableColumnHide="false"
                                                        ColumnLines="false" AutoScroll="true" >
                                                        <Store>
                                                            <ext:Store ID="StoreCheck" runat="server" OnRefreshData="RefreshCheckData">
                                                                <Reader>
                                                                    <ext:JsonReader IDProperty="RandomKey">
                                                                        <Fields>
                                                                            <ext:RecordField Name="BankName" />
                                                                            <ext:RecordField Name="Branch" />
                                                                            <ext:RecordField Name="CheckType" />
                                                                            <ext:RecordField Name="CheckNumber" />
                                                                            <ext:RecordField Name="_CheckDate" />
                                                                            <ext:RecordField Name="Amount" />
                                                                        </Fields>
                                                                    </ext:JsonReader>
                                                                </Reader>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel ID="columnModelChecks" runat="server">
                                                            <Columns>
                                                                <ext:Column Header="Bank" Locked="true" Sortable="false" Width="150" Fixed="true"
                                                                    Resizable="false">
                                                                </ext:Column>
                                                                <ext:Column Header="Branch" Locked="true" Sortable="false" Width="150" Fixed="true"
                                                                    Resizable="false">
                                                                </ext:Column>
                                                                <ext:Column Header="Check Type" Locked="true" Sortable="false" Width="130" Fixed="true"
                                                                    Resizable="false">
                                                                </ext:Column>
                                                                <ext:Column Header="Check No." Locked="true" Sortable="false" Width="150" Fixed="true"
                                                                    Resizable="false">
                                                                </ext:Column>
                                                                <ext:Column Header="Check Date" Locked="true" Sortable="false" Width="100" Fixed="true"
                                                                    Resizable="false">
                                                                </ext:Column>
                                                                <ext:NumberColumn Header="Amount" Locked="true" Fixed="true" Sortable="false" Width="150"
                                                                    Resizable="false" Format=",000.00">
                                                                </ext:NumberColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server" SingleSelect="true" ID="ChequesSelectionModel">
                                                                <Listeners>
                                                                    <RowSelect Fn="onRowSelectedCheck" />
                                                                    <RowDeselect Fn="onRowDeselectedCheck" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                                                                DisplayMsg="Displaying cheque {0} - {1} of {2}" EmptyMsg="No cheques to display" />
                                                        </BottomBar>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>
                                           <ext:Panel ID="Panel3" Title="ATM Payment" Height="200" AnchorHorizontal="95%"
                                                runat="server" Layout="FormLayout" Border="false">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar4" runat="server">
                                                        <Items>
                                                            <ext:Button ID="btnATMAdd" runat="server" Text="Add" Icon="Add">
                                                                <Listeners>
                                                                    <Click Handler="OnBtnAddATM()" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button ID="btnATMEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                                                <Listeners>
                                                                    <Click Handler="OnBtnEditATM()" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button ID="btnATMDelete" runat="server" Text="Delete" Icon="Delete">
                                                                <DirectEvents>
                                                                    <Click OnEvent="onBtnATMDelete_Click" Success="calculateATMTotal();">
                                                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to remove selected payment?" />
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             <Items>
                                    <ext:GridPanel ID="grdPnlATM" runat="server" Height="150">
                                        <View>
                                            <ext:GridView EmptyText="No atm items to display">
                                            </ext:GridView>
                                        </View>
                                        <LoadMask ShowMask="true" />
                                        <Store>
                                            <ext:Store ID="grdPnlATMStore" runat="server">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="ATMReferenceNumber" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server" ID="ColumnModel1" Width="100%">
                                            <Columns>
                                                <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="140px" Locked="true" Wrap="true" Align="Right" Format=",000.00" />
                                                <ext:Column Header="Reference Number" DataIndex="ATMReferenceNumber" Locked="true" Wrap="true" Width="140px" Hidden="false" />
                                            </Columns>
                                        </ColumnModel>
                                        <Listeners>
                                            <AfterRender Handler="calculateATMTotal();"/>
                                        </Listeners>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="grdPnlATMSelectionModel" runat="server" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <BottomBar>
                                            <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="5" DisplayInfo="true"
                                                DisplayMsg="Displaying disbursement items {0} - {1} of {2}" EmptyMsg="No atm payments to display" />
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                                           </ext:Panel>                                                                                                                                                                                             <ext:CompositeField ID="CompositeField3" runat="server" FieldLabel="Amount Tendered"
                                                AnchorHorizontal="50%">
                                                <Items>
                                                    <ext:Label ID="lblAmountTendered" runat="server" Text="₱">
                                                    </ext:Label>
                                                    <ext:TextField ID="txtamountTendered" runat="server" ReadOnly="true" AllowBlank="false"
                                                        Flex="1" MinValue="0" FieldLabel="₱" AnchorHorizontal="30%" MaskRe="/[0-9\.\,]/">
                                                        <Listeners>
                                                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));total();resetPaymentTotals(GridPanelBorrower);resetPaymentTotals(GridPanelGuarantors);paymentCheckingBorrower();paymentCheckingCoBorrower();" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                           <ext:Hidden ID="hiddenReceivedBy" runat="server" />
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelBorrower" Title="Borrower" Border="false" 
                                Layout="FormLayout" AutoScroll="true">
                                <Items>
                                    <ext:GridPanel ID="GridPanelBorrower" runat="server" Height="400" AutoScroll="true">
                                        <LoadMask ShowMask="true" />
                                        <Items>
                                            <ext:Toolbar ID="ToolbarBorrower" runat="server">
                                                <Items>
                                                    <%--   testing--%>
                                                    <ext:Button ID="loop" Text="Loop" Hidden="true" runat="server">
                                                        <Listeners>
                                                            <Click Handler="totalReceivables(GridPanelBorrower, hiddenTotalBorrowerPrincipal, hiddenTotalBorrowerInterest, hiddenTotalBorrowerPastDue, hiddenTotalBorrowerTotalAmountDue);totalReceivables(GridPanelGuarantors, hiddenTotalCoBorrowerPrincipal, hiddenTotalCoBorrowerInterest, hiddenTotalCoBorrowerPastDue, hiddenTotalCoBorrowerTotalAmountDue);totalTotalReceivables();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button ID="btnManualBillingBorrower" runat="server" Text="Generate Interest"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onBorrowerManualBillingClick();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnWaiveBorrower" runat="server" Text="Waive" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="BorrowerWaiveClick();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnRebate" runat="server" Text="Rebate" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onRebateClick();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnViewInterest" runat="server" Text="View Interests" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onBorrowerInterestBreakdown();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                    <ext:Button ID="btnPayinFullBorrower" runat="server" Icon="MoneyAdd" Text="Pay" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="payinFull(BorrowerSelectionModel, btnPayinFullBorrower, btnResetPaymentBorrower);changeChecker(btnPayinFullBorrower, BorrowerSelectionModel);changeChecker(btnPayinFullCoBorrower, CoBorrowerSelectionModel);" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnResetPaymentBorrower" runat="server" Text="Widthdraw Payment"
                                                        Icon="MoneyDelete" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="resetPayment(BorrowerSelectionModel, btnPayinFullBorrower, btnResetPaymentBorrower);changeChecker(btnPayinFullBorrower, BorrowerSelectionModel);changeChecker(btnPayinFullCoBorrower, CoBorrowerSelectionModel);" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="StoreBorrower" runat="server">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="LoanID" />
                                                            <ext:RecordField Name="LoanProductName" />
                                                            <ext:RecordField Name="PrincipalDue" />
                                                            <ext:RecordField Name="InterestDue" />
                                                            <ext:RecordField Name="PastDue" />
                                                            <ext:RecordField Name="TotalAmountDue" />
                                                            <ext:RecordField Name="PaymentAmount" />
                                                            <ext:RecordField Name="Type" />
                                                            <ext:RecordField Name="ComputedInterest">
                                                            </ext:RecordField>
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <BeforeEdit Fn="beforeEditBorrower" />
                                        </Listeners>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Loan ID" DataIndex="LoanID" Width="80">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Loan Product Name" DataIndex="LoanProductName" Width="150">
                                                </ext:Column>
                                                <ext:NumberColumn runat="server" Header="Principal Due" DataIndex="PrincipalDue"
                                                    Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Interest Due" DataIndex="InterestDue" Width="150"
                                                    Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Past Due" DataIndex="PastDue" Width="150"
                                                    Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Total Amount Due" DataIndex="TotalAmountDue"
                                                    Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Payment Amount" DataIndex="PaymentAmount"
                                                    Width="150" Format=",000.00">
                                                    <Editor>
                                                        <ext:NumberField runat="server" ID="numPaymentAmount">
                                                            <Listeners>
                                                                <Change Handler="total();" Delay="100" />
                                                            </Listeners>
                                                        </ext:NumberField>
                                                    </Editor>
                                                </ext:NumberColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" SingleSelect="true" ID="BorrowerSelectionModel">
                                                <Listeners>
                                                    <RowSelect Fn="onRowSelectedBorrower" />
                                                    <RowDeselect Fn="onRowDeselectedBorrower" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <LoadMask ShowMask="true" />
                                    </ext:GridPanel>
                                    <ext:Panel ID="Panel2" runat="server" AnchorHorizontal="100%" Layout="FormLayout">
                                        <Items>
                                            <ext:Hidden ID="hiddenTotalAmount2" runat="server" />
                                            <ext:CompositeField ID="CompositeField4" runat="server" FieldLabel="Total Amount"
                                                AnchorHorizontal="30%">
                                                <Items>
                                                    <ext:Label ID="lblPeso2" runat="server" Text="₱">
                                                    </ext:Label>
                                                    <ext:TextField ID="txtTotalAmount2" runat="server" Flex="1" Text="0.00" AllowBlank="false"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="CompositeField5" runat="server" FieldLabel="Change" AnchorHorizontal="30%">
                                                <Items>
                                                    <ext:Label ID="lblPeso3" runat="server" Text="₱">
                                                    </ext:Label>
                                                    <ext:TextField ID="txtChange2" Text="0.00" runat="server" Flex="1" AllowBlank="false"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelCoBorrowerOrGuarantor" Title="Co-Borrowers/Guarantors"
                                Border="false" Layout="FormLayout" AutoScroll="true">
                                <Items>
                                    <ext:GridPanel ID="GridPanelGuarantors" runat="server" Height="400" AutoScroll="true">
                                        <Items>
                                            <ext:Toolbar ID="ToolbarGuarantors" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnManualBilling2" Disabled="true" runat="server" Text="Generate Interest">
                                                        <Listeners>
                                                            <Click Handler="onCoBorrowerManualBillingClick();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnWaive2" Disabled="true" runat="server" Text="Waive">
                                                        <Listeners>
                                                            <Click Handler="CoBorrowerWaiveClick();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnCoborInterestBreakdown" runat="server" Text="View Interests" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="onCoBorrowerInterestBreakdown();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                                    <ext:Button ID="btnPayinFullCoBorrower" runat="server" Text="Pay" Icon="MoneyAdd"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="payinFull(CoBorrowerSelectionModel, btnPayinFullCoBorrower, btnResetPaymentCoBorrower);changeChecker(btnPayinFullBorrower, BorrowerSelectionModel);changeChecker(btnPayinFullCoBorrower, CoBorrowerSelectionModel);" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnResetPaymentCoBorrower" runat="server" Text="Widthdraw Payment"
                                                        Icon="MoneyDelete" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="resetPayment(CoBorrowerSelectionModel, btnPayinFullCoBorrower, btnResetPaymentCoBorrower);changeChecker(btnPayinFullBorrower, BorrowerSelectionModel);changeChecker(btnPayinFullCoBorrower, CoBorrowerSelectionModel);" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="StoreCoBorrowersGuarantor" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="LoanID" />
                                                            <ext:RecordField Name="LoanProductName" />
                                                            <ext:RecordField Name="PrincipalDue" />
                                                            <ext:RecordField Name="InterestDue" />
                                                            <ext:RecordField Name="PastDue" />
                                                            <ext:RecordField Name="TotalAmountDue" />
                                                            <ext:RecordField Name="PaymentAmount" />
                                                            <ext:RecordField Name="Type" />
                                                            <ext:RecordField Name="ComputedInterest">
                                                            </ext:RecordField>
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <BeforeEdit Fn="beforeEditCoBorrower" />
                                        </Listeners>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Loan ID" DataIndex="LoanID" Width="80">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Loan Product Name" DataIndex="LoanProductName" Width="150">
                                                </ext:Column>
                                                <ext:NumberColumn runat="server" Header="Principal Due" DataIndex="PrincipalDue"
                                                    Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Interest Due" DataIndex="InterestDue" Width="150"
                                                    Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Past Due" DataIndex="PastDue" Width="150"
                                                    Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Total Amount Due" DataIndex="TotalAmountDue"
                                                    Width="150" Format=",000.00">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Payment Amount" DataIndex="PaymentAmount"
                                                    Width="150" Format=",000.00">
                                                    <Editor>
                                                        <ext:NumberField ID="numPaymentAmount2" runat="server">
                                                            <Listeners>
                                                                <Change Handler="total();" Delay="100" />
                                                            </Listeners>
                                                        </ext:NumberField>
                                                    </Editor>
                                                </ext:NumberColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" SingleSelect="true" ID="CoBorrowerSelectionModel">
                                                <Listeners>
                                                    <RowDeselect Fn="onRowDeselectedCoBorrower" />
                                                    <RowSelect Fn="onRowSelectedCoBorrower" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </ext:GridPanel>
                                    <ext:Panel ID="PanelAdditional" runat="server" AnchorHorizontal="100%" AutoScroll="true"
                                        Layout="FormLayout">
                                        <Items>
                                            <ext:Hidden ID="hiddenTotalAmount1" runat="server" />
                                            <ext:CompositeField ID="CompositeField2" runat="server" FieldLabel="Total Amount"
                                                AnchorHorizontal="30%">
                                                <Items>
                                                    <ext:Label ID="lblPeso" runat="server" Text="₱" />
                                                    <ext:TextField ID="txtTotalAmount" runat="server" Flex="1" AllowBlank="false" ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="cfChange" runat="server" FieldLabel="Change" AnchorHorizontal="30%">
                                                <Items>
                                                    <ext:Label ID="lblPesoChange" runat="server" Text="₱" />
                                                    <ext:TextField ID="txtChange" Text="0.00" runat="server" Flex="1" AllowBlank="false"
                                                        ReadOnly="true">
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar5" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar5}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="cashDenomination" runat="server" Collapsible="true" Height="300"
        Icon="Application" Title="Cash Denomination" Width="450" Hidden="true" Modal="true"
        Resizable="false">
        <Items>
            <ext:FormPanel ID="frmNewInterstRate" runat="server" Padding="5" LabelWidth="110"
                Layout="FormLayout" MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField ID="txt1000Bills" runat="server" FieldLabel="1000 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txt500Bills" runat="server" FieldLabel="500 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txt200Bills" runat="server" FieldLabel="200 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txt100Bills" runat="server" FieldLabel="100 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txt50Bills" runat="server" FieldLabel="50 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txt20Bills" runat="server" FieldLabel="20 Bills" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txtCoins" runat="server" FieldLabel="Coins" EnableKeyEvents="true"
                        Width="300">
                        <Listeners>
                            <KeyUp Handler="sumTotalCash();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="300"
                        AllowBlank="false" MaskRe="[0-9\.\,]" ReadOnly="true">
                        <Listeners>
                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                            <Show Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                        </Listeners>
                    </ext:TextField>
                    <%--Amount--%>
                </Items>
                <BottomBar>
                    <ext:StatusBar runat="server" ID="StatusInterestRate">
                        <Items>
                            <ext:Button ID="btnApplyToCashField" runat="server" Text="Add" Icon="Add">
                                <Listeners>
                                    <Click Handler="var total = txtAmount.getValue(); txtCashPayment.setValue(total); cashDenomination.hide(); calculateTotalChequesCash();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnCancelCashField" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="cashDenomination.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
            </ext:FormPanel>
        </Items>
    </ext:Window>

    <!-- ATM Window-->
    <ext:Window ID="wnAddATM" Hidden="true" runat="server" Width="450" Icon="Application"
        Title="ATM Payment" Modal="true" AutoHeight="true" PageY="300">
        <Items>
            <ext:FormPanel ID="FormPanel2" runat="server" LabelWidth="160" MonitorValid="true">
                <Items>
                      <ext:TextField ID="txtATMAmount1" FieldLabel="Amount" AllowBlank="false" runat="server" AnchorHorizontal="95%">
                      <Listeners>
                     <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                      </Listeners>
                     </ext:TextField>
                     <ext:TextField ID="txtATMReferenceNum" FieldLabel="Reference Number" AllowBlank="false" runat="server" AnchorHorizontal="95%"></ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar3" runat="server">
                        <Items>
                               <ext:Hidden ID ="HiddenRandomKey2" runat="server"></ext:Hidden>
                            <ext:Button ID="btnSaveATM" runat="server" Text="Save" Icon="Disk" Disabled="true">
                            <DirectEvents>
                                    <Click OnEvent="btnSaveATM_Click" Success="saveATMSuccessful();"/>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCanceATM" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="cancelATM();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onFormValidated3(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <%--Interest Breakdown --%>
    <ext:Window ID="wndInterestBreakdown" runat="server" Width="420" Height="300" Hidden="true" Modal="true" Layout="FitLayout" Title="Interest Breakdown">
            <Items>
            <ext:GridPanel ID="grdPnlInterestBreak" runat="server" Border="false" AutoExpandColumn="Amount" AutoScroll="true">
                    <Store>
                        <ext:Store ID="strInterestBreakdown" runat="server">
                            <Reader>
                                <ext:JsonReader IDProperty="_DueDate">
                                    <Fields>
                                        <ext:RecordField Name="Amount" />
                                        <ext:RecordField Name="DueDate" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column Header="Date" DataIndex="DueDate" />
                            <ext:Column Header="Amount" DataIndex="Amount" />
                        </Columns>
                    </ColumnModel>
                    <Buttons>
                        <ext:Button ID="btnCloseInterest" runat="server" Text="Close">
                            <Listeners>
                                <Click Handler="closeInterestBreakdown();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:GridPanel>
                </Items>
        </ext:Window>
        <%--Interest Breakdown --%>
    </form>
</body>
</html>
