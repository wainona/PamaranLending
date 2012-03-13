﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdditionalLoan.aspx.cs"
    Inherits="LendingApplication.Applications.LoanRestructureUseCases.AdditionalLoan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var counters = 0;
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'addcheque') {
                X.AddCheque(msg.data.BankId, msg.data.Amount, msg.data.ChequeNumber, msg.data.TransactionDate, msg.data.Remarks, msg.data.ChequeDate, msg.data.RandomKey);
            }
        };

        var saveSuccessful = function () {
            showAlert('Status', 'Additional loan amount successfully added.', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        }

        var checkAmount = function () {
            X.CheckLoanAmount();
        };

        var saveFailure = function () {
            var status = hiddenStatus.getValue();
            var title = '';
            var message = '';
//            if (status == 0) {
//                title = 'Status';
//                message = 'Saving failed.';
//            } else if (status == 1) {
//                title = 'Cannot Continue Saving';
//                message = 'New loan amount exceeds the maximum loanable amount. Please specify new additional amount.';
//            } else if (status == 2) {
//                title = 'Cannot Continue Saving';
//                message = 'Cheques should have a check number.';
//            }
            title = 'Cannot Continue Saving';
            message = 'Cheques should have a check number.';
            showAlert(title, message, function () {
            });
        }

        var generateSuccessful = function () {
            var count = grdPnlAmortizationSchedule.store.getCount();
            //showAlert('Status', 'Amortization schedule successfully generated.');
            if (count > 0) {
                btnSave.enable();
                btnSave2.enable();
            }
        };

        var onFormValidated = function (valid) {
            var storeCount = grdPnlAmortizationSchedule.store.getCount();
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                if (btnGenerate.hidden == true) {
                    btnSave2.enable();
                } else {
                    btnGenerate.enable();
                    //btnSave.enable();
                }
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                //btnSave.disable();
                if (btnGenerate.hidden == true) {
                    btnSave2.disable();
                } else {
                    btnGenerate.disable();
                    //btnSave.disable();
                }
            }
        };

        var onRowSelected = function (btn) {
            btn.enable();
        };
        var onRowDeselected = function (grid, btn) {
            if (grid.hasSelection() == false) {
                btn.disable();
            }
        };

        var onBtnAddCheque = function () {
                var selectedRows = SelectionModelCheque.getSelections();
                var url = '/Applications/LoanRestructureUseCases/AddCheque.aspx';
                var randomKey = '&key=' + selectedRows[0].json.RandomKey;
                var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
                var type = "&T=additionalLoan"
                var param = param + "&CustomerId=" + hiddenCustomerId.getValue() + "&C=" + counters + type + randomKey;
                window.proxy.requestNewTab('ManageCheque', param, 'Manage Cheque');
                counters++;
        };
            var onBtnClose = function () {
                showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                    if (btn == 'yes') {
                        window.proxy.requestClose();
                    }
                });
            }
    </script>
    <style>
        .transparent
        {
            background-color: transparent;
        }
        
        .req
        {
            color:Red;
            font-weight:bold;
            font-size:larger;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenStatus" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="hdnSelectedLoanID" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="RootFormPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true" MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnGenerate" Hidden="false" runat="server" Text="Generate Schedule" Icon="Calculator">
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" Before="return #{RootFormPanel}.getForm().isValid();" Success="generateSuccessful">
                                        <EventMask Msg="Generating amortization schedule..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="tlbSeparatorForCheckBx"/>
                            <ext:Checkbox ID="chkAddChecks" FieldLabel="Add Post Dated Checks" LabelWidth="140" LabelSeparator="?" runat="server" />
                            <ext:Button ID="btnSave2" runat="server" Disabled="true" Hidden="true" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click"
                                        Success="saveSuccessful" Failure="saveFailure">
                                        <EventMask Msg="Saving..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="tlbSeparator" Hidden="true" />
                            <ext:Button ID="btnClose2" runat="server" Hidden="true"  Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill Hidden="true" />
                            <ext:Button ID="btnSave" runat="server" Disabled="true" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click"
                                        Success="saveSuccessful" Failure="saveFailure">
                                        <EventMask Msg="Saving amortization schedule..." ShowMask="true" />
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
                    <ext:Panel runat="server" ID="PanelAdditionalLoan" Border="false" Layout="Form">
                        <Items>
                            <ext:Panel runat="server" LabelWidth="200" Border="false" Padding="15">
                                <Items>
                                    <ext:DisplayField ID="txtCurrentLoanBalance" AllowBlank="true" ReadOnly="true" Height="25" runat="server" Width="500px"
                                        FieldLabel="Current Loan Balance" MaskRe="[0-9\.\,]">
                                    </ext:DisplayField>
                                    <ext:TextField runat="server" ID="txtInterestRate" ReadOnly="true" Width="500px" FieldLabel="Interest Rate" />
                                    <ext:TextField runat="server" ID="txtICM" ReadOnly="true" Width="500px" FieldLabel="Interest Computation Mode" />
                                    <ext:TextField ID="txtAdditionalAmount" AllowBlank="false" Height="25" runat="server" Width="500px"
                                        FieldLabel="Additional Amount" MaskRe="[0-9\.\,]">
                                        <Listeners>
                                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00')); checkAmount();" />
                                            <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:DateField ID="datLoanReleaseDate" runat="server" FieldLabel="Loan Release Date"
                                        Width="500" Editable="false" AllowBlank="false">
                                        <DirectEvents>
                                            <Select OnEvent="btnGenerate_Click">
                                                <EventMask Msg="Generating amortization schedule..." ShowMask="true" />
                                            </Select>
                                        </DirectEvents>
                                    </ext:DateField>
                                </Items>
                            </ext:Panel>
                            <ext:TabPanel runat="server" ID="PageTabPanel" AutoScroll="true">
                            <Items>
                            <ext:Panel ID="pnlAmortizationSchedule" runat="server" Title="Amortization Schedule">
                            <Items>
                            <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Height="500" MinHeight="500" AutoScroll="true">
                                <Store>
                                    <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                        <Listeners>
                                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                        </Listeners>
                                        <Reader>
                                            <ext:JsonReader IDProperty="RandomKey">
                                                <Fields>
                                                    <ext:RecordField Name="Counter" />
                                                    <ext:RecordField Name="ScheduledPaymentDate" Type="Date" />
                                                    <ext:RecordField Name="_ScheduledPaymentDate"/>
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
                                </SelectionModel>
                                <ColumnModel runat="server" ID="clmAmortizationSchedule" Width="100%">
                                    <Columns>
                                        <ext:Column Header="Unit" DataIndex="Counter" Wrap="true"
                                                    Width="100" Sortable="false"/>
                                        <ext:Column Header="Payment Due Date" DataIndex="_ScheduledPaymentDate" Wrap="true"
                                            Width="150" Sortable="false" />
                                        <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true" Width="150"
                                            Sortable="false" Format=",000.00" />
                                        <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="150"
                                            Sortable="false" Format=",000.00" />
                                        <ext:NumberColumn Header="Total Due" DataIndex="TotalPayment" Wrap="true" Width="150" 
                                            Sortable="false" Format=",000.00" />
                                        <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true" Width="150"
                                            Sortable="false" Format=",000.00" />
                                        <ext:NumberColumn Header="Total Loan Balance" Hidden="true" DataIndex="TotalLoanBalance" Wrap="true"
                                            Width="150" Sortable="false" Format=",000.00" />
                                    </Columns>
                                </ColumnModel>
                                <LoadMask Msg="Loading..." ShowMask="true" />
                            </ext:GridPanel>
                            </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnlCheque" runat="server" Title="Cheques" Height="500" >
                            <Items>
                            <ext:GridPanel ID="grdpnlCheque" runat="server" Disabled="true" AutoScroll="true" AutoExpandColumn="BankName" Height="500" MinHeight="500">
                                        <Items>
                                            <ext:Toolbar ID="Toolbar1" Hidden="true" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnAddCheque" runat="server" Disabled="true" Text="Edit" Icon="NoteEdit">
                                                        <Listeners>
                                                            <Click Handler="onBtnAddCheque();"/>
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="storeCheques" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ChequeId" />
                                                            <ext:RecordField Name="ChequeNumber" />
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="ChequeDate"/>
                                                            <ext:RecordField Name="_ChequeDate"/>
                                                            <ext:RecordField Name="BankName" />
                                                            <ext:RecordField Name="Status" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelCheque" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnAddCheque);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelCheque, btnAddCheque);" />
                                                </Listeners>
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
                                                <%--<ext:DateColumn runat="server" Header="Check Date" DataIndex="ChequeDate" Width="150" Format="MMM dd yyyy">
                                                </ext:DateColumn>--%>
                                                <ext:Column runat="server" Header="Check Date" DataIndex="_ChequeDate" Width="150" />
                                                <ext:Column runat="server" Header="Bank Name" DataIndex="BankName" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Status" DataIndex="Status" Width="150">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <Listeners>
                                            <RowDblClick Handler="onBtnAddCheque();" />
                                        </Listeners>
                                    </ext:GridPanel>
                            </Items>
                            </ext:Panel>
                            </Items>
                            </ext:TabPanel>
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
    </form>
</body>
</html>
