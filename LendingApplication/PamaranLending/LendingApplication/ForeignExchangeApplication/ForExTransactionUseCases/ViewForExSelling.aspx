<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewForExSelling.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.AddForExSelling" %>
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
            window.proxy.init(['onpickexchangerate', 'onpickcustomercontact', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomercontact') {
                hdnCustomerContactId.setValue(msg.data.PartyRoleId);
                txtCustomerName.setValue(msg.data.Name);
            } else if (msg.tag == 'onpickexchangerate') {
                hdnExchangeRateId.setValue(msg.data.Id);
                X.FillExchangeRateField({
                    success: function (result) {
                        changeRate();
                        convert();
                    }
                });
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
            });
        }



        var setCheckAmountDetail = function () {
            X.AllowBlankCheckAmount({
                success: function (result) {
                    markIfRequired(txtAmountCheckDetail);
                }
            });
        };


        var checkIsSpot = function () {
            if (chkIsSpot.checked) {
                txtRate.setReadOnly(false);
            } else {
                txtRate.setReadOnly(true);
            }
        };

        var amountToBuyChange = function () {
            var amountBuy = txtAmountToBuy.getValue();
            txtAmountCashDetail.setValue(Ext.util.Format.number(amountBuy, '0,0.00'));
        };

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
                <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" MonitorValid="true" LabelWidth="150" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="PagePanelToolBar" runat="server">
                            <Items>
                                <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                    <Listeners>
                                        <Click Handler="confirmCloseTab();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Panel ID="DetailsPanel" runat="server" Border="false" Layout="ColumnLayout" 
                            Padding="5" Height="65">
                            <Items>
                                <ext:Panel ID="InnerDetailsPanel" runat="server" Border="false" Layout="FormLayout"
                                    ColumnWidth="0.5" AnchorVertical="100%">
                                    <Items>
                                        <%-- Customer --%>
                                        <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Customer" Flex="1"  AllowBlank="false" ReadOnly="true"/>
                                        <%-- Transaction Date --%>
                                        <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Date" 
                                            Editable="false" AllowBlank="false" AnchorHorizontal="95%" ReadOnly="true" />
                                    </Items>    
                                </ext:Panel>
                                <ext:Panel ID="InnerDetailsPanel2" runat="server" Border="false" Layout="FormLayout"
                                    ColumnWidth="0.5"  AnchorVertical="100%">
                                    <Items>
                                         <%-- Exchange Rate --%>
                                        <ext:TextField ID="txtExchangeRate" runat="server" FieldLabel="Exchange Rate" Flex="1" ReadOnly="true" AllowBlank="false"/>
                                        <%-- Rate --%>
                                        <ext:TextField ID="txtRate" runat="server" FieldLabel="Rate" ReadOnly="true" 
                                            AnchorHorizontal="95%" AllowBlank="false" EnableKeyEvents="true">
                                        </ext:TextField>
                                        <ext:Hidden ID="hdnRate" runat="server" />
                                    </Items>    
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel1" runat="server" Border="false" Height="10px" />
                        <ext:Panel ID="GridsPanel" runat="server" Border="false" Padding="5"
                            Layout="ColumnLayout" Height="560" AutoScroll="true" >
                            <Items>
                                <ext:Panel ID="AmountReleasedPanel" runat="server" Title="Release" Padding="10"
                                    Layout="FormLayout" ColumnWidth="0.5"  AutoScroll="true"  >
                                    <Items>
                                        <ext:CompositeField ID="CompositeField2" runat="server" AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtAmountToBuy" runat="server" FieldLabel="Amount Bought" ReadOnly="false"
                                                    AllowBlank="false" Flex="1" EnableKeyEvents="true">
                                                </ext:TextField>
                                                <ext:TextField ID="txtAmountToBuyCurrency" runat="server" ReadOnly="true" Width="55" />
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:CompositeField ID="CompositeField3" runat="server"  AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtAmountCashDetail" runat="server" FieldLabel="Total Cash Amount" MaskRe="[0-9\.\,]" 
                                                    Flex="1" AllowBlank="false" EnableKeyEvents="true" ReadOnly="true">
                                                </ext:TextField>
                                                <ext:TextField ID="txtAmountCashDetailCurrency" runat="server" ReadOnly="true" Width="55" />
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:CompositeField ID="CompositeField4" runat="server"  AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtAmountCheckDetail" runat="server" FieldLabel="Total Check Amount" MaskRe="[0-9\.\,]" 
                                                    ReadOnly="true" Flex="1" AllowBlank="false"/>
                                                <ext:TextField ID="txtAmountCheckDetailCurrency" runat="server" ReadOnly="true"  Width="55"  />
                                            </Items>
                                        </ext:CompositeField>
                                        <%--Cash Grid Panel--%>
                                        <ext:GridPanel ID="grdCashDenomination" runat="server" Height="206" 
                                            Title="Cash Denomination" AnchorHorizontal="98%">
                                            <Store>
                                                <ext:Store ID="storeDenominationReleased" runat="server">
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
                                            <ColumnModel ID="ColumnModel12" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Type" DataIndex="Type" Width="100" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="BillAmount" Width="140" Format=",000.00"/>
                                                    <ext:Column Header="Serial Number" DataIndex="SerialNumber" Width="200" />
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="rsmBillDenominationTo" runat="server">
                                                    <Listeners>
                                                        <RowSelect Fn="onRowSelectedBillDenominationTo" />
                                                        <RowDeselect Fn="onRowDeselectedBillDenominationTo" />
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                        <%--Check Grid Panel--%>
                                        <ext:Panel ID="Panel4" runat="server" Height="5" Border="false">
                                        
                                        </ext:Panel>
                                        <ext:GridPanel ID="GridPanelCheck" runat="server" Height="206" 
                                            Title="Checks"  AnchorHorizontal="98%" AutoExpandColumn="BankName">
                                            <Store>
                                                <ext:Store ID="storeCheck" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="RandomKey">
                                                            <Fields>
                                                                <ext:RecordField Name="Amount" />
                                                                <ext:RecordField Name="_CheckDate" />
                                                                <ext:RecordField Name="CheckNumber" />
                                                                <ext:RecordField Name="BankName" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel ID="ColumnModel1" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Check Date" DataIndex="_CheckDate" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="Amount" Format=",000.00"/>
                                                    <ext:Column Header="Check Number" DataIndex="CheckNumber" />
                                                    <ext:Column Header="Bank" DataIndex="BankName" Width="150"/>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="rsmCheck" runat="server">
                                                    <Listeners>
                                                        <RowSelect Fn="onRowSelectedCheck" />
                                                        <RowDeselect Fn="onRowDeselectedCheck" />
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="AmountReceivedPanel" runat="server" Title="Received" Padding="10"
                                    Layout="FormLayout" ColumnWidth="0.5"  AutoScroll="true"  >
                                    <Items>
                                        <ext:CompositeField ID="CompositeField5" runat="server" AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtReceivedAmount" runat="server" FieldLabel="Converted Amount" ReadOnly="true" Flex="1" />
                                                <ext:TextField ID="txtReceivedAmountCurrency" runat="server" ReadOnly="true" Width="55" />
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Panel ID="Panel2" runat="server" Height="5" Border="false">
                                        
                                        </ext:Panel>
                                        <ext:GridPanel ID="grdCashDenominationReceived" runat="server" Height="465" 
                                            Title="Cash Denomination"  AnchorHorizontal="98%">
                                            <Store>
                                                <ext:Store ID="storeDenominationReceived" runat="server">
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
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Type" DataIndex="Type" Width="80" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="BillAmount" Width="100" Format=",000.00"/>
                                                    <ext:Column Header="Serial Number" DataIndex="SerialNumber" Width="120"/>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="rsmBillDenominationFrom" runat="server">
                                                    <Listeners>
                                                        <RowSelect Fn="onRowSelectedBillDenominationFrom" />
                                                        <RowDeselect Fn="onRowDeselectedBillDenominationFrom" />
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
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
    </form>
</body>
</html>
