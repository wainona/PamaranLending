<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenForExTransaction.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.OpenForExTransaction" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickexchangerate','onpickcustomercontact', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomercontact') {
                hdnCustomerContactId.setValue(msg.data.Id);
                X.FillCustomerField();
            } else if (msg.tag == 'onpickexchangerate') {
                hdnExchangeRateId.setValue(msg.data.Id);
                X.FillExchangeRateField();
            } else if (msg.tag == 'onpickbank') {
                hdnBankId.setValue(msg.data.Id);
                X.FillBankField();
            }
        };

        var confirmCloseTab = function () {
            showConfirm("Message", "Are you sure you want to close the tab?", function (btn) {
                if (btn.toLocaleLowerCase() == 'yes') {
                    window.proxy.requestClose();
                }
                else {

                }
            });
        }

        var addSuccess = function () {
            showAlert('Success!', 'Successfully added a foreign exchange transaction record.', function () {
                window.proxy.sendToAll('addforextransaction', 'addforextransaction');
                window.proxy.requestClose();
            });
        }

        var convert = function () {
            var rate = 1 * hdnRate.getValue();
            var originalAmount = 1 * txtOriginalAmount.getValue();
            var convertedAmount = originalAmount * rate;

            //convertedAmount = convertedAmount.replace(/\$|\,/g, '');
            //txtConvertedAmount.setValue(convertedAmount);

            txtConvertedAmount.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
            txtConvertedBalance.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
        };

        var changeRate = function () {
            var rate = txtRate.getValue();
            hdnRate.setValue(rate);
            //value.replace(/[]/g, '');
        }

        var onBtnPrint = function () {
            var url = '/ForeignExchangeApplication/ForExTransactionUseCases/PrintForExTransaction.aspx';
            var param = url + "?forexid=" + hdnForExId.getValue();
            window.proxy.requestNewTab('PrintForExTransaction', param, 'Print ForEx Transaction');
        }

        var onBtnPrintCheck = function () {
            var mode = "foreign";
            var selectedRow = checkRowSelectionModel.getSelected();
            var url = '/Applications/ChequeUseCases/PrintCheque.aspx';
            var param = url + '?id=' + selectedRow.json.Id;
            param = param + "&forexid=" + hdnForExId.getValue();
            param = param + "&mode=" + mode;
            window.proxy.requestNewTab('PrintCheque', param, 'Print Cheque');
        }

        var onRowSelect = function () {
            btnPrintCheck.enable();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
                <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" MonitorValid="true" LabelWidth="150">
                    <TopBar>
                        <ext:Toolbar ID="PagePanelToolBar" runat="server">
                            <Items>
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="addSuccess();">
                                            <EventMask Msg="Saving.." ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarFill />
                                <ext:Button ID="btnPrint" runat="server" Text="Print ForEx Transaction" Icon="Printer">
                                    <Listeners>
                                        <Click Handler="onBtnPrint();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" Hidden="false"/>
                                <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                    <Listeners>
                                        <Click Handler="confirmCloseTab();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Hidden ID="hdnForExId" runat="server" />
                        <ext:TabPanel runat="server" Padding="5" Border="false">
                            <Items>
                                <ext:Panel Title="Original Amount" runat="server" Border="false">
                                    <Items>
                                        <%-- Customer --%>
                                        <ext:CompositeField ID="CompositeField1" runat="server">
                                            <Items>
                                                <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Customer" Width="350" ReadOnly="true"/>
                                                <ext:Button ID="btnBrowseCustomer" runat="server" Icon="Magnifier" Hidden="true">
                                                    <Listeners>
                                                        <Click Handler="window.proxy.requestNewTab('PickListForExCustomer', '/ForeignExchangeApplication/ForExTransactionUseCases/PickListForExCustomer.aspx', 'Pick List Customer');" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Hidden ID="hdnCustomerContactId" runat="server" />
                                        <%-- Transaction Date --%>
                                        <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Date" Editable="false" Width="505" ReadOnly="true"/>
                                        <%-- Received By --%>
                                        <ext:TextField ID="txtReceivedBy" runat="server" FieldLabel="Received By" Width="350" ReadOnly="true" Hidden="true" />
                                        <ext:Hidden ID="hdnReceivedById" runat="server" />
                                        <%-- Rate --%>
                                        <ext:CompositeField runat="server">
                                            <Items>
                                                <ext:TextField ID="txtExchangeRate" runat="server" FieldLabel="Exchange Rate" Width="350"/>
                                                <ext:Button ID="btnPickExchangeRate" runat="server" Text="" Icon="Magnifier" Hidden="true">
                                                    <Listeners>
                                                        <Click Handler="window.proxy.requestNewTab('PickExchangeRate', '/ForeignExchange/ExchangeRateUseCases/PickListExchangeRate.aspx', 'Pick Exchange Rate');" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Hidden ID="hdnExchangeRateId" runat="server" />
                                        <%--<ext:ComboBox ID="cmbExchangeRate" runat="server" FieldLabel="Exchange Rate" Width="350" AllowBlank="false">
                                            <Store>
                                                <ext:Store ID="storeExchangeRate" runat="server">
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
                                        </ext:ComboBox>--%>
                                        <ext:TextField ID="txtRate" runat="server" FieldLabel="Rate" Width="505" EnableKeyEvents="true" ReadOnly="true">
                                            <Listeners>
                                                <KeyUp Handler="changeRate();" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Hidden ID="hdnRate" runat="server" />
                                        <ext:CompositeField runat="server">
                                            <Items>
                                                <%-- Original Amount --%>
                                                <ext:TextField ID="txtOriginalAmount" runat="server" FieldLabel="Original Amount" Width="275" EnableKeyEvents="true" ReadOnly="true">
                                                    <Listeners>
                                                        <KeyUp Handler="convert();" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <%-- Original Currency --%>
                                                <ext:ComboBox ID="cmbOriginalCurrency" runat="server" Width="70" ValueField="Id" DisplayField="Symbol" Editable="false" ReadOnly="true">
                                                    <Store>
                                                        <ext:Store ID="storeOriginalCurrency" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader IDProperty="Id">
                                                                    <Fields>
                                                                        <ext:RecordField Name="Id" />
                                                                        <ext:RecordField Name="Symbol" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:CompositeField runat="server" Hidden="true">
                                            <Items>
                                                <%-- Converted Amount --%>
                                                <ext:TextField runat="server" FieldLabel="Converted Amount" Width="275" />
                                                <%-- Converted Currency --%>
                                                <ext:ComboBox ID="cmbConvertedCurrency" runat="server" Width="70" ValueField="Id" DisplayField="Symbol" Editable="false">
                                                    <Store>
                                                        <ext:Store ID="storeConvertedCurrency" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader IDProperty="Id">
                                                                    <Fields>
                                                                        <ext:RecordField Name="Id" />
                                                                        <ext:RecordField Name="Symbol" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                </ext:ComboBox>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:GridPanel ID="grdCashDenominationOriginal" runat="server" Height="180" Title="Cash Denomination">
                                            <Store>
                                                <ext:Store ID="storeDenominationOriginal" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="Id">
                                                            <Fields>
                                                                <ext:RecordField Name="Type" />
                                                                <ext:RecordField Name="BillAmount" />
                                                                <ext:RecordField Name="SerialNumber" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar2" runat="server" Hidden="true">
                                                    <Items>
                                                        <%-- Delete Amount From Denomination --%>
                                                        <ext:Button ID="btnDeleteAmountFromDenomination" runat="server" Text="Delete" Icon="Delete">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                        <%-- Edit Amount From Denomination --%>
                                                        <ext:Button ID="btnEditAmountFromDenomination" runat="server" Text="Edit" Icon="DatabaseEdit">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                        <%-- Add Amount From Denomination --%>
                                                        <ext:Button ID="btnAddAmountFromDenomination" runat="server" Text="Add" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="wndAddDenominationOriginal.show();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Type" DataIndex="Type" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="BillAmount" Format=",000.00"/>
                                                    <ext:Column Header="Serial Number" DataIndex="SerialNumber" Width="200"/>
                                                </Columns>
                                            </ColumnModel>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel Title="Converted Amount" runat="server" Border="false">
                                    <Items>
                                        <ext:Panel runat="server" Padding="5" Border="false">
                                            <Items>
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtConvertedAmount" runat="server" FieldLabel="Converted Amount" ReadOnly="true"/>
                                                        <ext:Label ID="lblConvertedCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtAmountCashDetail" runat="server" FieldLabel="Total Cash Amount" ReadOnly="true"/>
                                                        <ext:Label ID="lblAmountCashDetailCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:CompositeField ID="CompositeField2" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtAmountCheckDetail" runat="server" FieldLabel="Total Check Amount" ReadOnly="true"/>
                                                        <ext:Label ID="lblAmountCheckDetailCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:GridPanel ID="grdCashDenomination" runat="server" Height="180" Title="Cash Denomination">
                                            <Store>
                                                <ext:Store ID="storeDenominationConverted" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="RandomKey">
                                                            <Fields>
                                                                <ext:RecordField Name="Type" />
                                                                <ext:RecordField Name="BillAmount" />
                                                                <ext:RecordField Name="SerialNumber" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar1" runat="server" Hidden="true">
                                                    <Items>
                                                        <%-- Delete Amount From Denomination --%>
                                                        <ext:Button ID="btnDeleteAmountToDenomination" runat="server" Text="Delete" Icon="Delete">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                        <%-- Edit Amount From Denomination --%>
                                                        <ext:Button ID="btnEditAmountToDenomination" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator runat="server" Hidden="true" />
                                                        <%-- Add Amount From Denomination --%>
                                                        <ext:Button ID="btnAddAmountToDenomination" runat="server" Text="Add" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="wndAddDenomination.show();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel1" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Type" DataIndex="Type" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="BillAmount" Format=",000.00"/>
                                                    <ext:Column Header="Serial Number" DataIndex="SerialNumber" Width="200"/>
                                                </Columns>
                                            </ColumnModel>
                                        </ext:GridPanel>
                                        <ext:Panel runat="server" Height="10" Border="false">
                                        
                                        </ext:Panel>
                                        <%-- ForExDetails --%>
                                        <ext:GridPanel ID="GridPanelCheck" runat="server" Height="180" Title="Checks">
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar3" runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnDeleteCheck" runat="server" Text="Delete" Icon="Delete"  Hidden="true">
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" Hidden="true"/>
                                                        <ext:Button ID="btnEditCheck" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" Hidden="true" />
                                                        <ext:Button ID="btnAddCheck" runat="server" Text="Add" Icon="Add" Hidden="true">
                                                            <Listeners>
                                                                <Click Handler="wndAddCheck.show();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:Button runat="server" ID="btnPrintCheck" Text="Print Check" Icon="Printer" Disabled="true">
                                                            <Listeners>
                                                                <Click Handler="onBtnPrintCheck();"/>
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <Store>
                                                <ext:Store ID="storeCheck" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="RandomKey">
                                                            <Fields>
                                                                <ext:RecordField Name="Id" />
                                                                <ext:RecordField Name="Amount" />
                                                                <ext:RecordField Name="_CheckDate" />
                                                                <ext:RecordField Name="CheckNumber" />
                                                                <ext:RecordField Name="BankName" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel runat="server">
                                                <Columns>
                                                    <ext:Column Header="Id" DataIndex="Id" Width="60"/>
                                                    <ext:Column Header="Check Date" DataIndex="_CheckDate" />
                                                    <ext:Column Header="Amount" DataIndex="Amount" />
                                                    <ext:Column Header="Check Number" DataIndex="CheckNumber" />
                                                    <ext:Column Header="Bank" DataIndex="BankName" Width="300px" />
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server" ID="checkRowSelectionModel" SingleSelect="true">
                                                    <Listeners>
                                                        <RowSelect Handler="onRowSelect();" />
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:TabPanel>
                    </Items>
                    <BottomBar>
                        <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                    </BottomBar>
                    <Listeners>
                        <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="wndAddDenomination" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="350" Height="200">
            <Items>
                <ext:FormPanel ID="formPanelAddDenomination" runat="server" Border="false" Padding="5">
                    <Items>
                        <ext:ComboBox ID="cmbBillAmountFrom" runat="server" DisplayField="Amount" ValueField="Id" FieldLabel="Bill Amount">
                            <Store>
                                <ext:Store ID="storeBillAmountFrom" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="Id">
                                            <Fields>
                                                <ext:RecordField Name="Id" />
                                                <ext:RecordField Name="Amount" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                        <ext:TextField ID="txtSerialNumberFrom" runat="server" FieldLabel="Serial Number" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnSaveAddDenomination" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSaveAddDenomination_Click" Success="" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="#{wndAddDenomination}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <%-- Add Check --%>
        <ext:Window ID="wndAddCheck" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="350" Height="200">
            <Items>
                <ext:FormPanel ID="formPanelAddCheck" runat="server" Border="false" Padding="5">
                    <Items>
                        <%--<ext:ComboBox ID="ComboBox1" runat="server" DisplayField="Amount" ValueField="Id" FieldLabel="Bill Amount">
                            <Store>
                                <ext:Store ID="store1" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="Id">
                                            <Fields>
                                                <ext:RecordField Name="Id" />
                                                <ext:RecordField Name="Amount" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>--%>
                        <ext:TextField ID="txtAmountAddCheck" runat="server" FieldLabel="Amount" />
                        <ext:CompositeField runat="server">
                            <Items>
                                <ext:TextField ID="txtBank" runat="server" FieldLabel="Bank" />
                                <ext:Button ID="btnPickBank" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Click Handler="window.proxy.requestNewTab('BankList', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:Hidden ID="hdnBankId" runat="server" />
                        <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" />
                        <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnSaveAddCheck" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSaveAddCheck_Click" Success="" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="#{wndAddCheck}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar runat="server" Hidden="true"></ext:StatusBar>
                    </BottomBar>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
