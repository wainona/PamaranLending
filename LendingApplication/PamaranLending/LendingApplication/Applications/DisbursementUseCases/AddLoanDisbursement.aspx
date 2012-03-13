<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLoanDisbursement.aspx.cs"
    Inherits="LendingApplication.Applications.DisbursementUseCases.AddLoanDisbursement" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Loan Disbursement</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['voucherselected', 'bankselected', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
            cmbLoanDisType.hide();
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'voucherselected') {
                txtLoanDisbursementVoucherId.setValue(msg.data.VoucherID);
                txtCustID.setValue(msg.data.CustomerID);
                txtCustomerName.setValue(msg.data.CustomerName);
                txtLoanAgreementId.setValue(msg.data.AgreementID);
                txtLoanProduct.setValue(msg.data.LoanProduct);
                txtLoanAmount.setValue(msg.data.LoanAmount);
                txtLoanProductID.setValue(msg.data.LoanProductID);
                txtAmountDisbursed.setValue(msg.data.LoanAmount);
                txtCashAmount.setValue(msg.data.LoanAmount);
                isAdditional(msg.data.VoucherID);
                change();
                X.SetDate(msg.data.VoucherID, msg.data.AgreementID);
            } else if (msg.tag == 'bankselected') {
                txtBank.setValue(msg.data.OrganizationName);
                txtBankID.setValue(msg.data.PartyRoleId);
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, msg.data.BankPartyRoleId, {
                    success: function () {
                        totalChequesAmount();
                        caculateTotal();
                    }
                });
            }

        };

        var isAdditional = function (id) {
            X.CheckIfAdditional(id, {
                success: function (result) {
                    if (result == 1) {
                        cmbLoanDisType.show();
                    } else {
                        cmbLoanDisType.hide();
                    }

                }
            });
        };
        var change = function () {
            X.AmountDisbursedChanged({
                success: function (result) {
                    txtDeductions.setValue(result);
                    formatCurrency(txtDeductions);
                    caculateTotal();
                }
            });

        };

        var openVoucherList = function () {
            var url = '/Applications/DisbursementUseCases/VoucherViewList.aspx';
            var id = 'id=' + txtUserID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('SelectVoucher', param, 'Voucher List');
        };
        var saveSuccessful = function () {
            var data = {};
            data.AgreementId = txtLoanAgreementId.getValue();
            data.id = hdnDisbursemntId.getValue();
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('loanapplicationstatusupdate', 'loanapplicationstatusupdate');
                window.proxy.sendToParent(data, 'addloandisbursement');
            });
        };

        var validateCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                        hdnChequeValid.setValue(1);
                    } else {
                        hdnChequeValid.setValue(0);
                        btnSave.enable();
                    }
                }
            });

        };
        var onAddCheque = function () {
            var url = '/Applications/DisbursementUseCases/AddDisbursementCheque.aspx';
            var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
            var param = url + guid;
            param += "&type=" + 'LoanDisbursement';
            window.proxy.requestNewTab('AddDisheque', param, 'Add Cheque');
        }
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
        var totalChequesAmount = function () {
            var amount = 0;
            for (var j = 0; j < gridPanelChecks.store.getCount(); j++) {
                var data = gridPanelChecks.store.getAt(j);
                amount += data.get('Amount');
            }

            txtCheckAmount.setValue(amount);
            formatCurrency(txtCheckAmount);
        }
        var caculateTotal = function () {
            formatCurrency(txtCashAmount);
            formatCurrency(txtAmountDisbursed);
            formatCurrency(txtCheckAmount);
            formatCurrency(txtLoanAmount);
            formatCurrency(txtDeductions);
            formatCurrency(txtNetAmountReceived);

            var cash = txtCashAmount.getValue();
            cash = cash.replace(/,/g, '');
            var checkAmount = txtCheckAmount.getValue();
            checkAmount = checkAmount.replace(/,/g, '');
            var deductions = txtDeductions.getValue();
            deductions = deductions.replace(/,/g, '');

            var totalamountTendered = parseFloat(cash) + parseFloat(checkAmount);
            var netAmount = totalamountTendered - parseFloat(deductions);

            txtAmountDisbursed.setValue(totalamountTendered);
            txtNetAmountReceived.setValue(netAmount);
            formatCurrency(txtNetAmountReceived);
            formatCurrency(txtAmountDisbursed);

        };

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
        }
        //            var chkamount = txtChkAmount.getValue();
        //            chkamount = chkamount.replace(/,/g, '');
        var onFormValidated = function (valid) {
            btnSave.disable();

            var checkToDisburse = txtCheckAmount.getValue();
            checkToDisburse = checkToDisburse.replace(/,/g, '');
            var cashToDisburse = txtCashAmount.getValue();
            cashToDisburse = cashToDisburse.replace(/,/g, '');

            var amount = txtLoanAmount.getValue();
            amount = amount.replace(/,/g, '');

            var deductions = txtDeductions.getValue();
            deductions = deductions.replace(/,/g, '');

            var totalToDisburse = parseFloat(checkToDisburse) + parseFloat(cashToDisburse);
            var amountDisbursable = parseFloat(amount);

            if (valid && totalToDisburse == amountDisbursable && totalToDisburse >= deductions) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid && totalToDisburse == amountDisbursable && totalToDisburse < deductions) {
                PageFormPanelStatusBar.setStatus({ text: 'Amount to be Disbursed must be greater than Deductions. ' });
            } else if (valid && totalToDisburse != amountDisbursable) {
                PageFormPanelStatusBar.setStatus({ text: 'Amount to be Disbursed must be equal to Loan Amount. ' });
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
            }
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
    <form id="form2" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden ID="hdnDisbursemntId" runat="server" />
    <ext:Hidden ID="hiddenImageUrl" runat="server" />
    <ext:Hidden ID="hdnChequeValid" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenInterest" runat="server">
    </ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit" AutoHeight="true">
        <Items>
            <%-- Height="500"--%>
            <ext:FormPanel ID="PageFormPanel" runat="server" Layout="FitLayout" Border="false"
                MonitorValid="true" AutoScroll="true" MonitorPoll="500">
                <Defaults>
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask ShowMask="true" Msg="Saving.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TabPanel ID="TabPanel" runat="server" Padding="0" Border="false" EnableTabScroll="true"
                        DeferredRender="false">
                        <Items>
                            <ext:Panel ID="pnlEncashment" runat="server" Layout="ColumnLayout" Title="Voucher"
                                Border="false" LabelWidth="250">
                                <Items>
                                    <ext:Panel ID="Panel2" LabelWidth="230" runat="server" Padding="5" Layout="FormLayout"
                                        ColumnWidth=".6" RowHeight=".5" AutoScroll="true" Border="false">
                                        <Items>
                                            <ext:TextField ID="txtLoanDisbursementVoucherId" AllowBlank="false" FieldLabel="Loan Disbursement Voucher"
                                                runat="server" AnchorHorizontal="95%" Hidden="true">
                                            </ext:TextField>
                                            <ext:Hidden runat="server" ID="txtCustID">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="txtLoanAgreementId">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="txtUserID">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="txtBankID">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="txtLoanProductID">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="hiddenResourceGUID">
                                            </ext:Hidden>
                                            <ext:CompositeField ID="CompositeField1" AnchorHorizontal="60%" runat="server">
                                                <Items>
                                                    <ext:TextField ID="txtCustomerName" FieldLabel="Disbursed To" AllowBlank="false"
                                                        ReadOnly="true" Flex="1" runat="server">
                                                    </ext:TextField>
                                                    <ext:Button ID="btnBrowseVoucher" runat="server" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="openVoucherList();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtLoanProduct" FieldLabel="Loan Product" ReadOnly="true" runat="server"
                                                AnchorHorizontal="60%">
                                            </ext:TextField>
                                            <ext:TextField ID="txtLoanAmount" FieldLabel="Loan Amount" ReadOnly="true" runat="server"
                                                AnchorHorizontal="60%">
                                            </ext:TextField>
                                            <ext:ComboBox ID="cmbLoanDisType" runat="server" FieldLabel="Disbursement Type" ValueField="Id"
                                                DisplayField="Name" AnchorHorizontal="60%" Editable="false" AllowBlank="false">
                                                <Store>
                                                    <ext:Store ID="strLoanDisType" runat="server">
                                                        <Reader>
                                                            <ext:JsonReader IDProperty="Id">
                                                                <Fields>
                                                                    <ext:RecordField Name="Id" />
                                                                    <ext:RecordField Name="Name" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                            <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" AllowBlank="false"
                                                Editable="false" runat="server" AnchorHorizontal="60%">
                                            </ext:DateField>
                                            <ext:TextField runat="server" AnchorHorizontal="60%" FieldLabel="Received By" ID="txtDisbursedToName">
                                            </ext:TextField>
                                            <ext:CompositeField ID="CompositeField4" runat="server" AnchorHorizontal="60%" Height="85">
                                                <Items>
                                                    <ext:Image ID="PersonImageFile" Hidden="false" Height="60" Flex="1" runat="server"
                                                        ImageUrl="../../../Resources/images/signhere.png" />
                                                    <ext:FileUploadField ID="fileUpDisburseTo" FieldLabel="Signature" AllowBlank="false"
                                                        Width="250" Height="60" Icon="Attach" runat="server">
                                                        <DirectEvents>
                                                            <FileSelected OnEvent="onUpload_Click">
                                                                <EventMask ShowMask="true" Msg="Uploading..." />
                                                            </FileSelected>
                                                        </DirectEvents>
                                                    </ext:FileUploadField>
                                                </Items>
                                            </ext:CompositeField>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Disbursement Details"
                                Border="false" LabelWidth="200" ColumnWidth=".6" RowHeight=".5" Padding="5">
                                <Items>
                                    <ext:TextField ID="txtCashAmount" FieldLabel="Cash Amount To Disburse" ReadOnly="false"
                                        runat="server" AnchorHorizontal="55%">
                                        <Listeners>
                                            <Change Handler="caculateTotal();formatCurrency(txtCashAmount);" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtCheckAmount" FieldLabel="Check Amount To Disburse" ReadOnly="true"
                                        runat="server" AnchorHorizontal="55%" Hidden="false">
                                    </ext:TextField>
                                    <ext:TextField ID="txtAmountDisbursed" FieldLabel="Total Amount To Disburse" runat="server"
                                        AnchorHorizontal="55%" ReadOnly="true">
                                    </ext:TextField>
                                    <ext:TextField ID="txtDeductions" FieldLabel="Deductions" Hidden="false" ReadOnly="true"
                                        runat="server" AnchorHorizontal="55%">
                                    </ext:TextField>
                                    <ext:TextField ID="txtNetAmountReceived" FieldLabel="Net Amount Received" ReadOnly="true"
                                        runat="server" AnchorHorizontal="55%">
                                    </ext:TextField>
                                    <ext:Panel ID="PanelForPayCheck" Title="Check Disbursements" Height="345" AnchorHorizontal="95%"
                                        runat="server" Layout="FormLayout" Border="false">
                                        <Items>
                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnAddCheck" Text="Add" runat="server" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onAddCheque();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                    <ext:Button ID="btnDeleteCheck" Text="Delete" runat="server" Disabled="true" Icon="Delete">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteCheques_Click" Success="totalChequesAmount();caculateTotal();" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:GridPanel ID="gridPanelChecks" runat="server" Height="260" EnableColumnHide="false"
                                                ColumnLines="false">
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
    </form>
</body>
</html>
