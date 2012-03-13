<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddForExTransactionSelling.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.AddForExTransactionSelling" %>
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
                hdnCustomerContactId.setValue(msg.data.PartyRoleId);
                X.FillCustomerField();
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
                if (btn.toLocaleLowerCase() == 'yes') 
                {
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
            var originalAmount = 1 * txtAmountReceived.getValue().replace(',', '');
            var convertedAmount = originalAmount * rate;

            //convertedAmount = convertedAmount.replace(/\$|\,/g, '');
            //txtConvertedAmount.setValue(convertedAmount);

            txtReleasedAmount.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
            txtReleasedBalance.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
            txtAmountCashDetail.setValue(Ext.util.Format.number(convertedAmount, '0,0.00'));
        };

        var changeRate = function () {
            var rate = txtRate.getValue();
            hdnRate.setValue(rate);
            //value.replace(/[]/g, '');
        }

        var wndAddDenominationReleased_Close = function () {
            wndAddDenominationReleased.hide();
            txtBillAmountTo.setValue();
            txtSerialNumberTo.setValue();
        };

//        var SaveAddDenominationConverted_Success = function () {
//            wndAddDenominationConverted_Close();
//        }

        var wndAddDenominationOriginal_Close = function () {
            wndAddDenominationOriginal.hide();
            txtBillAmountFrom.setValue('');
            txtSerialNumberFrom.setValue('');
        };

//        var SaveAddDenominationOriginal_Success = function () {
//            wndAddDenominationOriginal_Close();
//        };

        var wndAddCheck_Close = function () {
            wndAddCheck.hide();
            txtAmountAddCheck.setValue('');
            txtBank.setValue('');
            hdnBankId.setValue('');
            dtCheckDate.setValue('');
            txtCheckNumber.setValue('');
            markIfRequired(txtAmountCashDetail);
        };

//        var SaveAddCheck_Success = function () {
//            wndAddCheck_Close();
//        }

        var totalCash = function () {
            var bills = txtAmountBillsDetail.getValue().replace(/\$|\,/g, '') * 1;
            var coins = txtAmountCoinsDetail.getValue().replace(/\$|\,/g, '') * 1;
            var total = bills + coins;
            txtAmountCashDetail.setValue(Ext.util.Format.number(total, '0,0.00'));
        }

        var clearExchangeRate = function () {
            txtExchangeRate.clear();
            //txtExchangeRate.setReadOnly(true);
            txtRate.setValue('');
            txtRate.setReadOnly(false);
            cmbReceivedCurrency.setReadOnly(false);
            cmbReleasedCurrency.setReadOnly(false);
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
                txtRate.readOnly = false;
            } else {
                txtRate.readOnly = true;
            }
        };

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
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" >
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="addSuccess();">
                                            <EventMask Msg="Saving.." ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                    <Listeners>
                                        <Click Handler="confirmCloseTab();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:TabPanel runat="server" Padding="5" Border="false">
                            <Items>
                                <ext:Panel ID="Panel2" Title="Amount Released" runat="server" Border="false" AutoScroll="true">
                                    <Items>
                                        <ext:Panel ID="Panel3" runat="server" Border="false">
                                            <Items>
                                                <%-- Customer --%>
                                                <ext:CompositeField ID="CompositeField9" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtCustomerName" runat="server" FieldLabel="Customer" Width="350" AllowBlank="false" ReadOnly="true"/>
                                                        <ext:Button ID="btnBrowseCustomer" runat="server" Icon="Magnifier">
                                                            <Listeners>
                                                                <Click Handler="window.proxy.requestNewTab('PickListForExCustomer', '/ForeignExchangeApplication/ForExTransactionUseCases/PickListForExCustomer.aspx', 'Pick List Customer');" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:Hidden ID="hdnCustomerContactId" runat="server" />
                                                <%-- Transaction Date --%>
                                                <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Date" Editable="false" AllowBlank="false" Width="505" />
                                                <%-- Received By --%>
                                                <ext:TextField ID="txtReceivedBy" runat="server" FieldLabel="Received By" Width="350" ReadOnly="true" Hidden="true"/>
                                                <ext:Hidden ID="hdnReceivedById" runat="server" />
                                                <%-- Rate --%>
                                                <ext:CompositeField ID="CompositeField1" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtExchangeRate" runat="server" FieldLabel="Exchange Rate" Width="350" ReadOnly="true" AllowBlank="false"/>
                                                        <ext:Button ID="btnPickExchangeRate" runat="server" Text="" Icon="Magnifier">
                                                            <Listeners>
                                                                <Click Handler="window.proxy.requestNewTab('PickExchangeRate', '/ForeignExchangeApplication/ExchangeRateUseCases/PickListExchangeRate.aspx', 'Pick Exchange Rate');" />
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
                                                <%-- Is Spot --%>
                                                <ext:Checkbox ID="chkIsSpot" runat="server" FieldLabel="Is Spot?">
                                                    <Listeners>
                                                        <Change Handler="checkIsSpot();" />
                                                    </Listeners>
                                                </ext:Checkbox>
                                                <ext:TextField ID="txtRate" runat="server" FieldLabel="Rate" Width="505" AllowBlank="false" EnableKeyEvents="true">
                                                    <Listeners>
                                                        <KeyUp Handler="changeRate();convert();" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:Hidden ID="hdnRate" runat="server" />
                                                <ext:CompositeField ID="CompositeField2" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtReleasedBalance" runat="server" FieldLabel="Amount to Release" ReadOnly="true" Width="350" />
                                                        <ext:Label ID="lblReleasedBalanceCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:CompositeField ID="CompositeField3" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtAmountCashDetail" runat="server" FieldLabel="Total Cash Amount" MaskRe="[0-9\.\,]" Width="350" AllowBlank="false" EnableKeyEvents="true">
                                                            <Listeners>
                                                                <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00')); setCheckAmountDetail();" />
                                                            </Listeners>
                                                        </ext:TextField>
                                                        <ext:Label ID="lblAmountCashDetailCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:CompositeField ID="CompositeField4" runat="server">
                                                    <Items>
                                                        <ext:TextField ID="txtAmountCheckDetail" runat="server" FieldLabel="Total Check Amount" MaskRe="[0-9\.\,]" ReadOnly="true" Width="350" AllowBlank="false"/>
                                                        <ext:Label ID="lblAmountCheckDetailCurrency" runat="server" />
                                                    </Items>
                                                </ext:CompositeField>
                                            </Items>
                                        </ext:Panel>
                                        <ext:GridPanel ID="grdCashDenomination" runat="server" Height="206" Title="Cash Denomination">
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
                                        <ext:Panel ID="Panel4" runat="server" Height="5" Border="false">
                                        
                                        </ext:Panel>
                                        <%-- ForExDetails --%>
                                        <ext:GridPanel ID="GridPanelCheck" runat="server" Height="206" Title="Checks">
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
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                        <ext:Button ID="btnEditCheck" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                                
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" Hidden="true"/>
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
                                                    <ext:Column Header="Bank" DataIndex="BankName" Width="390"/>
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
                                <ext:Panel Title="Amount Received" runat="server">
                                    <Items>
                                        <ext:CompositeField runat="server">
                                            <Items>
                                                <%-- Original Amount --%>
                                                <ext:TextField ID="txtAmountReceived" runat="server" FieldLabel="Amount to Pay" Width="275" AllowBlank="false" EnableKeyEvents="true" MaskRe="[0-9\.\,]">
                                                    <Listeners>
                                                        <KeyUp Handler="convert();" />
                                                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <%-- Original Currency --%>
                                                <ext:ComboBox ID="cmbReceivedCurrency" runat="server" Width="70" AllowBlank="false" ValueField="Id" DisplayField="Symbol" Editable="false" ReadOnly="true">
                                                    <Store>
                                                        <ext:Store ID="storeReceivedCurrency" runat="server">
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
                                        <ext:GridPanel ID="grdCashDenominationReceived" runat="server" Height="320" Title="Cash Denomination">
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
                                                <ext:Toolbar runat="server">
                                                    <Items>
                                                        <%-- Delete Amount From Denomination --%>
                                                        <ext:Button ID="btnDeleteAmountFromDenomination" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                            <DirectEvents>
                                                                <Click OnEvent="btnDeleteAmountFromDenomination_Click">
                                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected record?" />
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator runat="server" />
                                                        <%-- Edit Amount From Denomination --%>
                                                        <ext:Button ID="btnEditAmountFromDenomination" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true">
                                                            <Listeners>
                                                                <Click Handler="" />
                                                            </Listeners>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator runat="server" Hidden="true" />
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
                                                    <ext:Column Header="Type" DataIndex="Type" Width="150" />
                                                    <ext:NumberColumn Header="Amount" DataIndex="BillAmount" Width="150" Format=",000.00"/>
                                                    <ext:Column Header="Serial Number" DataIndex="SerialNumber" Width="200"/>
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
                                        <ext:Panel ID="Panel1" runat="server" Height="5" Border="false">
                                        
                                        </ext:Panel>
                                        <ext:CompositeField runat="server" Hidden="true">
                                            <Items>
                                                <%-- Released Amount --%>
                                                <ext:TextField ID="txtReleasedAmount" runat="server" FieldLabel="Released Amount" Width="275" AllowBlank="true" ReadOnly="true"/>
                                                <%-- Released Currency --%>
                                                <ext:ComboBox ID="cmbReleasedCurrency" runat="server" Width="70" AllowBlank="false" ValueField="Id" DisplayField="Symbol" Editable="false" ReadOnly="true">
                                                    <Store>
                                                        <ext:Store ID="storeReleasedCurrency" runat="server">
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
        <ext:Window ID="wndAddDenominationReleased" runat="server" Layout="FitLayout" Hidden="true" Modal="true" Width="309" Height="160" Closable="false" Draggable="false" Resizable="false" Title="Add Bill Denomination">
            <Items>
                <ext:FormPanel ID="formPanelAddDenominationReleased" runat="server" Border="false" Padding="5" MonitorValid="true">
                    <Items>
                        <ext:ComboBox ID="cmbCashTypeTo" runat="server" FieldLabel="Type" AllowBlank="false" Width="175">
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
                        <ext:Button runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="wndAddDenominationReleased_Close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar runat="server" Hidden="true"></ext:StatusBar>
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
                        <ext:ComboBox ID="cmbCashTypeFrom" runat="server" FieldLabel="Type" AllowBlank="false" Width="175">
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
                        <ext:StatusBar runat="server" Hidden="true"></ext:StatusBar>
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
                        <ext:CompositeField runat="server">
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
                        <ext:Button runat="server" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="wndAddCheck_Close();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <BottomBar>
                        <ext:StatusBar runat="server" Hidden="true"></ext:StatusBar>
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
