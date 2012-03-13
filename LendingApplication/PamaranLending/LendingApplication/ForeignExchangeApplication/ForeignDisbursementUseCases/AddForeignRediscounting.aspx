<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddForeignRediscounting.aspx.cs"
    Inherits="LendingApplication.ForeignExchangeApplication.ForeignDisbursementUseCases.AddForeignRediscounting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Create Encashment</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['bankselected', 'customeradded', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'bankselected') {
                txtBank.setValue(msg.data.OrganizationName);
                txtBankId.setValue(msg.data.PartyRoleId);
            } else if (msg.tag == 'customeradded') {
                txtProcessedToCustomerId.setValue(msg.data.PartyRoleId);
                txtDisbursedTo.setValue(msg.data.Name);
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, msg.data.BankPartyRoleId, {
                    success: function () {
                        totalChequesAmount();
                    }
                });
            }
        };
        var openBank = function () {
            window.proxy.requestNewTab('SelectBank', '/Applications/DisbursementUseCases/BankViewList.aspx', 'Bank List');
        };
        var openCustomer = function () {
            var url = '/Applications/DisbursementUseCases/CustomerCoBorrowerViewList.aspx';
            var id = 'id=' + txtUserID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('SelectCustomerCoBorrower', param, 'Customer and Co-Borrower List');
        };
        var saveSuccessful = function () {
            var data = {};
            data.id = hdnDisbursementId.getValue();
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToParent(data, 'addforeignrediscounting');
            });
        };
        var checkamount = function () {
            X.CheckAmountBlur();
        };

        var validateCheckNumber = function () {
            var checkNumber = txtChkNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtChkNumber.markInvalid('Check Number already exist.');
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
            param += "&type=" + 'Rediscounting';
            window.proxy.requestNewTab('AddForeignRediscountingCheque', param, 'Add Cheque');
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

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
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
        var onFormValidated = function (valid) {
            btnSave.disable();
            var checkToDisburse = txtCheckAmount.getValue();
            checkToDisburse = checkToDisburse.replace(/,/g, '');
            var cashToDisburse = txtCashAmount.getValue();
            cashToDisburse = cashToDisburse.replace(/,/g, '');
            var total = txtAmountToDisburse.getValue();
            total = total.replace(/,/g, '');
            var totalToDisburse = parseFloat(checkToDisburse) + parseFloat(cashToDisburse);
            if (((valid && hdnChequeValid.getValue() == '') || (valid && hdnChequeValid.getValue() == 0)) && (total == totalToDisburse)) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if ((valid && hdnChequeValid.getValue() == '') || (valid && hdnChequeValid.getValue() == 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'Summation of Surcharge Fee, Cash and Check Amount To Disburse must equal to Check Amount. ' });
            } else {
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
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden ID="hdnDisbursementId" runat="server">
    </ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit" AutoHeight="true">
        <Items>
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
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();" Before="return #{PageFormPanel}.getForm().isValid();">
                                        <EventMask ShowMask="true" Msg="Saving.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button ID="Button2" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="txtUserID" runat="server">
                    </ext:Hidden>
                    <ext:Hidden ID="hdnChequeValid" runat="server">
                    </ext:Hidden>
                    <ext:TabPanel ID="TabPanel" runat="server" Padding="0" HideBorders="true" EnableTabScroll="true"
                        DeferredRender="false">
                        <Items>
                            <ext:Panel ID="pnlEncashment" runat="server" Layout="FormLayout" LabelWidth="200"
                                Padding="5" AutoHeight="true" Title="Voucher">
                                <Items>
                                    <ext:Panel ID="Panel2" runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false"
                                        Height="320" AnchorHorizontal="100%">
                                        <Items>
                                            <ext:ComboBox ID="cmbCurrency" runat="server" FieldLabel="Currency" ValueField="Id"
                                                DisplayField="NameDescription" AnchorHorizontal="60%" Editable="false" AllowBlank="false">
                                                <Store>
                                                    <ext:Store ID="strCurrency" runat="server">
                                                        <Reader>
                                                            <ext:JsonReader IDProperty="Id">
                                                                <Fields>
                                                                    <ext:RecordField Name="Id" />
                                                                    <ext:RecordField Name="NameDescription" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                            <ext:CompositeField ID="CompositeField2" AnchorHorizontal="60%" runat="server">
                                                <Items>
                                                    <ext:TextField ID="txtDisbursedTo" FieldLabel="Disbursed To" AllowBlank="false" ReadOnly="true"
                                                        Flex="1" runat="server">
                                                    </ext:TextField>
                                                    <ext:Button runat="server" ID="btnCustomerBrowse" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="openCustomer();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" Editable="false"
                                                AllowBlank="false" runat="server" AnchorHorizontal="60%">
                                            </ext:DateField>
                                            <ext:CompositeField ID="CompositeField1" AnchorHorizontal="60%" runat="server">
                                                <Items>
                                                    <ext:TextField ID="txtBank" FieldLabel="Bank" AllowBlank="false" ReadOnly="true"
                                                        runat="server" Flex="1">
                                                    </ext:TextField>
                                                    <ext:Button ID="btnBankBrowse" runat="server" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="openBank();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:Hidden ID="txtBankId" runat="server">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="hiddenResourceGUID">
                                            </ext:Hidden>
                                            <ext:Hidden runat="server" ID="txtProcessedToCustomerId">
                                            </ext:Hidden>
                                            <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Cheque Type" AnchorHorizontal="60%"
                                                Editable="false" AllowBlank="false">
                                                <Items>
                                                    <ext:ListItem Text="Pay Check" />
                                                    <ext:ListItem Text="Personal Check" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:TextField ID="txtChkNumber" FieldLabel="Check Number" AllowBlank="false" runat="server"
                                                AnchorHorizontal="60%" EnableKeyEvents="true">
                                                <Listeners>
                                                    <KeyUp Handler="validateCheckNumber();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:DateField ID="txtChkDate" Editable="false" FieldLabel="Check Date" AllowBlank="false"
                                                runat="server" AnchorHorizontal="60%">
                                            </ext:DateField>
                                            <ext:TextField ID="txtChkAmount" FieldLabel="Check Amount" AllowBlank="false" runat="server"
                                                AnchorHorizontal="60%">
                                                <Listeners>
                                                    <Change Handler="formatCurrency(txtChkAmount);" />
                                                    <Blur Handler="checkamount();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:CompositeField ID="CompositeField3" runat="server" AnchorHorizontal="60%">
                                                <Items>
                                                    <ext:NumberField ID="txtSurchargeRate" FieldLabel="Surcharge Rate" MaxValue="100"
                                                        AllowBlank="false" runat="server" Flex="1" EnableKeyEvents="true">
                                                        <Listeners>
                                                            <KeyUp Handler="checkamount()" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:Label ID="Label1" Text="%" runat="server">
                                                    </ext:Label>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtReceivedBy" FieldLabel="Received By" runat="server" AnchorHorizontal="60%">
                                            </ext:TextField>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Disbursement Details"
                                HideBorders="true" LabelWidth="200" ColumnWidth=".6" RowHeight=".5" Padding="5">
                                <Items>
                                    <ext:TextField ID="txtSurchargeFee" FieldLabel="Surcharge Fee" ReadOnly="true" runat="server"
                                        AnchorHorizontal="60%">
                                    </ext:TextField>
                                    <ext:TextField ID="txtAmountToDisburse" FieldLabel="Total Amount To Disburse" ReadOnly="true"
                                        runat="server" AnchorHorizontal="60%">
                                    </ext:TextField>
                                    <ext:TextField ID="txtCashAmount" FieldLabel="Cash Amount To Disburse" runat="server"
                                        AnchorHorizontal="60%">
                                        <Listeners>
                                            <Change Handler="formatCurrency(txtCashAmount);" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtCheckAmount" FieldLabel="Check Amount To Disburse" ReadOnly="true"
                                        runat="server" AnchorHorizontal="60%" Hidden="false">
                                    </ext:TextField>
                                    <ext:Panel ID="PanelForPayCheck" AutoHeight="true" AnchorHorizontal="95%" runat="server"
                                        Layout="FormLayout" Border="false" Title="Check Disbursements">
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
                                                            <Click OnEvent="btnDeleteCheques_Click" Success="totalChequesAmount();" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:GridPanel ID="gridPanelChecks" runat="server" Height="230" EnableColumnHide="false"
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
