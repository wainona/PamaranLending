<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddForExBuying.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.AddForExBuying" %>
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

        var addSuccess = function () {
            showAlert('Success!', 'Successfully added a foreign exchange transaction record.', function () {
                window.proxy.sendToAll('addforextransaction', 'addforextransaction');
                window.proxy.requestClose();
            });
        }



        var convert = function () {
            var rate = 1 * hdnRate.getValue().replace(',', '');
            var originalAmount = 1 * txtReceivedAmount.getValue().replace(',', '');
            var convertedAmount = originalAmount * rate;

            txtReleasedAmount.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
            txtAmountCashDetail.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
            txtAmountCashDetail.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
        };

        var changeRate = function () {
            var rate = txtRate.getValue();
            hdnRate.setValue(rate);
        }

        var wndAddDenominationReleased_Close = function () {
            wndAddDenominationReleased.hide();
            txtBillAmountTo.setValue();
            txtSerialNumberTo.setValue();
        };

        var wndAddDenominationOriginal_Close = function () {
            wndAddDenominationOriginal.hide();
            txtBillAmountFrom.setValue('');
            txtSerialNumberFrom.setValue('');
        };

        var wndAddCheck_Close = function () {
            wndAddCheck.hide();
            txtAmountAddCheck.setValue('');
            txtBank.setValue('');
            hdnBankId.setValue('');
            dtCheckDate.setValue('');
            txtCheckNumber.setValue('');
            markIfRequired(txtAmountCashDetail);
        };

        var totalCash = function () {
            var bills = txtAmountBillsDetail.getValue().replace(/\$|\,/g, '') * 1;
            var coins = txtAmountCoinsDetail.getValue().replace(/\$|\,/g, '') * 1;
            var total = bills + coins;
            txtAmountCashDetail.setValue(Ext.util.Format.number(total, '0,0.00'));
        }

        var clearExchangeRate = function () {
            txtExchangeRate.clear();
            txtRate.setValue('');
            txtRate.setReadOnly(false);
            txtReleasedAmountCurrency.setValue('');
            txtAmountCashDetailCurrency.setValue('');
            txtAmountCheckDetailCurrency.setValue('');
            txtReceivedAmountCurrency.setValue('');
        };

        /**********************************************/
        var onRowSelectedBillDenominationFrom = function () {
            btnDeleteAmountFromDenomination.enable();
        }

        var onRowDeselectedBillDenominationFrom = function () {

        }

        var onRowSelectedBillDenominationTo = function () {
            btnDeleteAmountToDenomination.enable();
        }

        var onRowDeselectedBillDenominationTo = function () {

        }

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {

        }

        var setCheckAmountDetail = function () {
            X.AllowBlankCheckAmount({
                success: function (result) {
                    markIfRequired(txtAmountCheckDetail);
                }
            });
        };

        var addCashDenominationReleasedSuccess = function () {
            setCheckAmountDetail();
            wndAddDenominationReleased_Close();
        };

        var deleteAmountToDenominationSuccess = function () {
            setCheckAmountDetail();
        };

        var checkIsSpot = function () {
            if (chkIsSpot.checked) {
                txtRate.setReadOnly(false);
            } else {
                txtRate.setReadOnly(true);
            }
        };

        var amountToBuyChange = function () {
            var amountBuy = txtReleasedAmount.getValue();
            txtAmountCashDetail.setValue(Ext.util.Format.number(amountBuy, '0,0.00'));
        };


        var onFormValidated = function (valid) {
            btnSave.disable();
            var total = txtReleasedAmount.getValue();
            total = total.replace(/,/g, '');

            var cashAmount = txtAmountCashDetail.getValue();
            cashAmount = cashAmount.replace(/,/g, '');

            var chequeAmount = txtAmountCheckDetail.getValue();
            chequeAmount = chequeAmount.replace(/,/g, '');

            var totalAmount = parseFloat(cashAmount) + parseFloat(chequeAmount);
            if (valid && totalAmount == total) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Amount To Buy must be equal to Total Cash & Cheque To Release' });
            } else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
       <ext:Hidden ID="hdnReceivedCurrencyId" runat="server"></ext:Hidden>
       <ext:Hidden ID="hdnReleasedCurrencyId" runat="server"></ext:Hidden>
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
                <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" MonitorValid="true" LabelWidth="150" AutoScroll="true">
                    <TopBar>
                        <ext:Toolbar ID="PagePanelToolBar" runat="server">
                            <Items>
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" >
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="addSuccess();">
                                            <EventMask Msg="Saving.." ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
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
                                        <ext:CompositeField ID="CompositeField9" runat="server" AnchorHorizontal="95%">
                                            <Items>
                                                <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Customer" Flex="1"  AllowBlank="false" ReadOnly="true"/>
                                                <ext:Button ID="btnBrowseCustomer" runat="server" Icon="Magnifier">
                                                    <Listeners>
                                                        <Click Handler="window.proxy.requestNewTab('PickListForExCustomer', '/ForeignExchangeApplication/ForExTransactionUseCases/PickListForExCustomer.aspx', 'Pick List Customer');" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Hidden ID="hdnCustomerContactId" runat="server" />
                                        <%-- Transaction Date --%>
                                        <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Date" 
                                            Editable="false" AllowBlank="false" AnchorHorizontal="95%" />
                                    </Items>    
                                </ext:Panel>
                                <ext:Panel ID="InnerDetailsPanel2" runat="server" Border="false" Layout="FormLayout"
                                    ColumnWidth="0.5"  AnchorVertical="100%">
                                    <Items>
                                         <%-- Rate --%>
                                        <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="95%">
                                            <Items>
                                                <ext:TextField ID="txtExchangeRate" runat="server" FieldLabel="Exchange Rate" Flex="1" ReadOnly="true" AllowBlank="false"/>
                                                <ext:Button ID="btnPickExchangeRate" runat="server" Text="" Icon="Magnifier">
                                                    <Listeners>
                                                        <Click Handler="window.proxy.requestNewTab('PickExchangeRate', '/ForeignExchangeApplication/ExchangeRateUseCases/PickListExchangeRate.aspx?ratetype=Buying', 'Pick Exchange Rate');" />
                                                    </Listeners>
                                                </ext:Button>
                                                <ext:Button ID="btnEraseRate" runat="server" Icon="Cancel">
                                                    <Listeners>
                                                        <Click Handler="clearExchangeRate();" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Hidden ID="hdnExchangeRateId" runat="server" />
                                        <ext:CompositeField ID="CompositeField6" runat="server" AnchorHorizontal="95%">
                                            <Items>
                                                <%-- Rate --%>
                                                <ext:TextField ID="txtRate" runat="server" FieldLabel="Rate" ReadOnly="true" 
                                                    AnchorHorizontal="95%" AllowBlank="false" EnableKeyEvents="true">
                                                    <Listeners>
                                                        <KeyUp Handler="changeRate();convert();" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Label ID="lblSpot" runat="server" Text="Is Spot?" />
                                                <%-- Is Spot --%>
                                                <ext:Checkbox ID="chkIsSpot" runat="server">
                                                    <Listeners>
                                                        <Check Handler="checkIsSpot();" />
                                                    </Listeners>
                                                </ext:Checkbox>
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Hidden ID="hdnRate" runat="server" />
                                    </Items>    
                                </ext:Panel>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel1" runat="server" Border="false" Height="10px" />
                        <ext:Panel ID="GridsPanel" runat="server" Border="false" Padding="5"
                            Layout="ColumnLayout" Height="560" AutoScroll="true" >
                            <Items>
                                <ext:Panel ID="AmountReceivedPanel" runat="server" Title="Received" Padding="10"
                                    Layout="FormLayout" ColumnWidth="0.5"  AutoScroll="true"  >
                                    <Items>
                                        <ext:CompositeField ID="CompositeField5" runat="server" AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtReceivedAmount" runat="server" FieldLabel="Amount Received" 
                                                    ReadOnly="false" Flex="1" EnableKeyEvents="true">
                                                    <Listeners>
                                                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                                        <KeyUp Handler="convert();" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:TextField ID="txtReceivedAmountCurrency" runat="server" ReadOnly="true" Width="55" />
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:Panel ID="Panel2" runat="server" Height="5" Border="false">
                                        
                                        </ext:Panel>
                                        <ext:GridPanel ID="grdCashDenominationReceived" runat="server" Height="465" 
                                            Title="Cash Denomination"  AnchorHorizontal="100%">
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
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar2" runat="server">
                                                    <Items>
                                                        <%-- Delete Amount From Denomination --%>
                                                        <ext:Button ID="btnDeleteAmountFromDenomination" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnDeleteAmountFromDenomination_Click" Success="convert();">
                                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected record?" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                        <%-- Edit Amount From Denomination --%>
                                                        <ext:Button ID="btnEditAmountFromDenomination" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" Hidden="true" />
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
                                <ext:Panel ID="AmountReleasedPanel" runat="server" Title="Release" Padding="10"
                                    Layout="FormLayout" ColumnWidth="0.5"  AutoScroll="true"  >
                                    <Items>
                                        <ext:CompositeField ID="CompositeField2" runat="server" AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtReleasedAmount" runat="server" FieldLabel="Converted Amount" ReadOnly="true"
                                                    AllowBlank="false" Flex="1" EnableKeyEvents="true">
                                                </ext:TextField>
                                                <ext:TextField ID="txtReleasedAmountCurrency" runat="server" ReadOnly="true" Width="55" />
                                            </Items>
                                        </ext:CompositeField>
                                        <ext:CompositeField ID="CompositeField3" runat="server"  AnchorHorizontal="98%">
                                            <Items>
                                                <ext:TextField ID="txtAmountCashDetail" runat="server" FieldLabel="Total Cash Amount" MaskRe="[0-9\.\,]" 
                                                    Flex="1" AllowBlank="false" EnableKeyEvents="true">
                                                    <Listeners>
                                                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00')); setCheckAmountDetail();" />
                                                    </Listeners>
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
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar1" runat="server">
                                                    <Items>
                                                        <%-- Delete Amount From Denomination --%>
                                                        <ext:Button ID="btnDeleteAmountToDenomination" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnDeleteAmountToDenomination_Click" Success="deleteAmountToDenominationSuccess">
                                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected record?" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator11" runat="server" />
                                                        <%-- Edit Amount From Denomination --%>
                                                        <ext:Button ID="btnEditAmountToDenomination" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator21" runat="server" Hidden="true"/>
                                                        <%-- Add Amount From Denomination --%>
                                                        <ext:Button ID="btnAddAmountToDenomination" runat="server" Text="Add" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="wndAddDenominationReleased.show();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
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
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar3" runat="server">
                                                    <Items>
                                                        <ext:Button ID="btnDeleteCheck" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnDeleteCheck_Click">
                                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected record?" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                        <ext:Button ID="btnEditCheck" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                                
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" Hidden="true"/>
                                                        <ext:Button ID="btnAddCheck" runat="server" Text="Add" Icon="Add">
                                                            <Listeners>
                                                                <Click Handler="wndAddCheck.show();" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
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
                            </Items>
                        </ext:Panel>
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
        <ext:Window ID="wndAddDenominationReleased" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="309" Height="160" Closable="false" Draggable="false" Resizable="false" Title="Add Bill Denomination">
            <Items>
                <ext:FormPanel ID="formPanelAddDenominationReleased" runat="server" Border="false" Padding="5" MonitorValid="true">
                    <Items>
                        <ext:ComboBox ID="cmbCashTypeTo" runat="server" FieldLabel="Type" AllowBlank="false" Width="175" Editable="false">
                            <Items>
                                <ext:ListItem Text="Bill" Value="Bill" />
                                <ext:ListItem Text="Coins" Value="Coins" />
                            </Items>
                            <DirectEvents>
                                <Select OnEvent="cmbCashTypeTo_Select" />
                            </DirectEvents>
                        </ext:ComboBox>
                        <ext:TextField ID="txtBillAmountTo" runat="server" FieldLabel="Amount" AllowBlank="false" Width="175" MaskRe="[0-9\.\,]">
                            <Listeners>
                                 <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField ID="txtSerialNumberTo" runat="server" FieldLabel="Serial Number" AllowBlank="false" Width="175"/>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnSaveAddDenominationReleased" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSaveAddDenominationReleased_Click" Success="addCashDenominationReleasedSuccess" Before="return #{formPanelAddDenominationReleased}.getForm().isValid();">
                                    <EventMask Msg="Saving.." ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button1" runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="wndAddDenominationReleased_Close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar1" runat="server" Hidden="true"></ext:StatusBar>
                    </BottomBar>
                    <Listeners>
                        <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveAddDenominationReleased}.enable();}  else{#{btnSaveAddDenominationReleased}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <ext:Window ID="wndAddDenominationOriginal" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="309" Height="160" Closable="false" Draggable="false" Resizable="false" Title="Add Bill Denomination">
            <Items>
                <ext:FormPanel ID="formPanelAddDenominationOriginal" runat="server" Border="false" Padding="5" MonitorValid="true">
                    <Items>
                        <ext:ComboBox ID="cmbCashTypeFrom" runat="server" FieldLabel="Type" AllowBlank="false" Width="175" Editable="false">
                            <Items>
                                <ext:ListItem Text="Bill" Value="Bill" />
                                <ext:ListItem Text="Coins" Value="Coins" />
                            </Items>
                            <DirectEvents>
                                <Select OnEvent="cmbCashTypeFrom_Select" />
                            </DirectEvents>
                        </ext:ComboBox>
                        <ext:TextField ID="txtBillAmountFrom" runat="server" FieldLabel="Amount" AllowBlank="false" Width="175" MaskRe="[0-9\.\,]">
                            <Listeners>
                                 <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField ID="txtSerialNumberFrom" runat="server" FieldLabel="Serial Number" AllowBlank="false" Width="175"/>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnSaveAddDenominationOriginal" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSaveAddDenominationReceived_Click" Success="wndAddDenominationOriginal_Close" Before="return #{formPanelAddDenominationOriginal}.getForm().isValid();">
                                    <EventMask Msg="Saving.." ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button2" runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="wndAddDenominationOriginal_Close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar2" runat="server" Hidden="true"></ext:StatusBar>
                    </BottomBar>
                    <Listeners>
                        <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveAddDenominationOriginal}.enable();}  else{#{btnSaveAddDenominationOriginal}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <%-- Add Check --%>
        <ext:Window ID="wndAddCheck" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="430" Height="200" Resizable="false" Draggable="false" Closable="false" Title="Add Check">
            <Items>
                <ext:FormPanel ID="formPanelAddCheck" runat="server" Border="false" Padding="5" MonitorValid="true" LabelWidth="120">
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
                        <ext:TextField ID="txtAmountAddCheck" runat="server" FieldLabel="Amount" AllowBlank="false" Width="250" MaskRe="[0-9\.\,]"/>
                        <ext:CompositeField ID="CompositeField7" runat="server">
                            <Items>
                                <ext:TextField ID="txtBank" runat="server" FieldLabel="Bank" AllowBlank="false" ReadOnly="true" Width="250"/>
                                <ext:Button ID="btnPickBank" runat="server" Icon="Magnifier">
                                    <Listeners>
                                        <Click Handler="window.proxy.requestNewTab('BankList', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:Hidden ID="hdnBankId" runat="server" />
                        <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" AllowBlank="false" Editable="false" Width="250"/>
                        <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" AllowBlank="false" Width="250"/>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnSaveAddCheck" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSaveAddCheck_Click" Success="wndAddCheck_Close" Before="return #{formPanelAddCheck}.getForm().isValid();">
                                    <EventMask Msg="Saving.." ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="Button21" runat="server" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="wndAddCheck_Close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar3" runat="server" Hidden="true"></ext:StatusBar>
                    </BottomBar>
                    <Listeners>
                        <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveAddCheck}.enable();}  else{#{btnSaveAddCheck}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
