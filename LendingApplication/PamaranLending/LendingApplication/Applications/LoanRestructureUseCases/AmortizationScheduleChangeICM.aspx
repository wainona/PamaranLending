﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmortizationScheduleChangeICM.aspx.cs"
    Inherits="LendingApplication.Applications.LoanRestructureUseCases.AmortizationScheduleChangeICM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
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
            showAlert('Status', 'Loan interest computation mode successfully changed.', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
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
            showAlert(title, message);
        };

        var onDateChanged = function () {
            var today = new Date;
            var status = "Retired";
            if (dtIntroductionDate.getValue() > today)
                status = "Inactive";
            else if (dtIntroductionDate.getValue() <= today
                && (dtSalesDiscontinuationDate.getValue() == ''
                || dtSalesDiscontinuationDate.getValue() > today))
                status = "Active";

            txtProductStatus.setValue(status);
        }

        Ext.apply(Ext.form.VTypes, {
            numberrange: function (val, field) {
                if (!val) {
                    return;
                }

                if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
                    var start = Ext.getCmp(field.startNumberField);

                    if (start) {
                        start.setMaxValue(val);
                        field.numberRangeMax = val;
                        start.validate();
                    }
                } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
                    var end = Ext.getCmp(field.endNumberField);

                    if (end) {
                        end.setMinValue(val);
                        field.numberRangeMin = val;
                        end.validate();
                    }
                }

                return true;
            }
        });

        var onFormValidated = function (valid) {
            //var isValid = X.IfChequeHasCheckNumber();
            var loanterm = nfLoanTerm.getValue();
            var storeCount = grdPnlAmortizationSchedule.store.getCount();
            if (valid && storeCount == loanterm) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                if (btnGenerate.hidden == true) {
                    btnSave.enable();
                } else {
                    btnGenerate.enable();
                    btnSave.enable();
                }
            } 
            else if (valid && storeCount != loanterm) {
                PageFormPanelStatusBar.setStatus({ text: 'Schedule and Term do not match. Generate schedule to proceed.', iconCls: 'icon-exclamation' });
                btnGenerate.enable();
                btnSave.disable();
                //showAlert('Status', 'Schedule and Term do not match. Generate schedule to proceed.');
//                if (btnGenerate.hidden == true) {
//                    btnSave.disable();
//                } else {
//                    btnGenerate.disable();
//                }
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                if (btnGenerate.hidden == true) {
                    btnSave.disable();
                } else {
                    btnGenerate.disable();
                    btnSave.disable();
                }
            }
        };

        var generateSucess = function () {
            showAlert('Status', 'Amortization schedule successfully generated.');
            btnSave.enable();
            grdpnlCheque.enable();
        };

        var computeLoanBalance = function () {
            X.computeNewLoanBalance();
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
            var term = nfLoanTerm.getValue();
            if (nfLoanTerm.getValue() != 0) {
                var selectedRows = SelectionModelCheque.getSelections();
                var url = '/Applications/LoanRestructureUseCases/AddCheque.aspx';
                var randomKey = '&key=' + selectedRows[0].json.RandomKey;
                var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
                var type = "&T=changeIcm"
                var param = param + "&CustomerId=" + hiddenCustomerId.getValue() + "&C=" + counters + type + randomKey;
                window.proxy.requestNewTab('ManageCheque', param, 'Manage Cheque');
                counters++;
            }
            else {
                showAlert('Information', 'You do not have a loan term. Please add a loan term to add associated cheques.');
            }
        };
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
    <ext:ResourceManager ID="PageResourceManager" runat="server"  DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="hdnSelectedLoanID" runat="server" />
    <ext:Hidden ID="hiddenNewInterestRate" runat="server" />
    <ext:Hidden ID="hiddenStatus" runat="server" />
    <ext:Viewport ID="pageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel runat="server" ID="RootFormPanel" Layout="FitLayout" Border="false" MonitorValid="true" MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server" ID="RootToolBar">
                        <Items>
                            <ext:Button runat="server" Hidden="false" ID="btnGenerate" Text="Generate Schedule" Icon="Calculator" >
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" Success="generateSucess">
                                        <EventMask Msg="Generating amortization schedule..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="tlbSeparatorForCheckBx"/>
                            <ext:Checkbox ID="chkAddChecks" FieldLabel="Add Post Dated Checks" LabelWidth="140" LabelSeparator="?" runat="server" />
                            <ext:ToolbarFill />
                            <ext:Button runat="server" ID="btnSave" Text="Save" Disabled="true" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful" Failure="saveFailure" >
                                        <EventMask Msg="Saving..." ShowMask="true" />    
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnCancel" Text="Close" Icon="Cancel" >
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelGenerateAmortizationSchedule"
                        Padding="0" Layout="Form" LabelWidth="180">
                        <Items>
                            <ext:Panel ID="Panel1" runat="server" LabelWidth="230" Border="false" Padding="15">
                                <Items>
                                    <ext:TextField ID="txtCarryOverBalance" Height="25" runat="server" Width="500px"
                                        FieldLabel="Balance to Carry Over" ReadOnly="true" />
                                    <ext:DisplayField runat="server" FieldLabel="  Old Interest Rate" ID="dfOldInterestRate" Width="500px" Text="" />
                                    <ext:NumberField ID="nfNewInterestRate" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                        FieldLabel="New Interest Rate (%)" MinValue="0" EnableKeyEvents="true" MaxValue="100" DecimalPrecision="2">
                                        <Listeners>
                                            <KeyUp Handler="computeLoanBalance();" />
                                        </Listeners>    
                                    </ext:NumberField>
                                    <ext:TextField ID="txtCarryOverReceivables" Height="25" runat="server" Width="500px"
                                        FieldLabel="Total of Unpaid Receivables" ReadOnly="true" />
                                    <ext:NumberField ID="nfReceivableAdd" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                        FieldLabel="Receivable Amount To Carry Over" EnableKeyEvents="true" MinValue="0" DecimalPrecision="2">
                                        <Listeners>
                                            <KeyUp Handler="computeLoanBalance();" />
                                        </Listeners>    
                                    </ext:NumberField>
                                    <ext:NumberField ID="nfLoanTerm" Height="25" AllowBlank="true" runat="server" Width="500px" 
                                        FieldLabel="Loan Term" MinValue="0" DecimalPrecision="0" />
                                </Items>
                            </ext:Panel>
                            <%--<ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Hidden="true" Height="500" MinHeight="500"
                                        Title="Amortization Schedule">
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
                                                <ext:DateColumn Header="Payment Due Date" DataIndex="ScheduledPaymentDate" Wrap="true"
                                                    Width="150" Sortable="false" />
                                                <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Due" DataIndex="TotalPayment" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="150" Sortable="false" Format=",000.00" />
                                            </Columns>
                                        </ColumnModel>
                                        <LoadMask Msg="Loading..." ShowMask="true" />
                                    </ext:GridPanel>--%>
                            <ext:TabPanel runat="server" ID="PageTabPanel" AutoScroll="true">
                            <Items>
                            <ext:Panel ID="pnlAmortizationSchedule" runat="server" Title="Amortization Schedule">
                            <Items>
                            <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Height="300" MinHeight="300" AutoScroll="true">
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
                            <ext:Panel ID="pnlCheque" runat="server" Title="Cheques" Height="400" >
                            <Items>
                            <ext:GridPanel ID="grdpnlCheque" runat="server" Disabled="true" AutoScroll="true" AutoExpandColumn="BankName" Height="300" MinHeight="300">
                                        <Items>
                                            <ext:Toolbar ID="Toolbar1" Hidden="true" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteCheque" Hidden="true" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteCheque_Click"></Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator Hidden="true" />
                                                    <ext:Button ID="btnAddCheque" runat="server" Text="Edit" Icon="NoteEdit">
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
                                                    <RowSelect Handler="onRowSelected(btnDeleteCheque); onRowSelected(btnAddCheque);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelCheque, btnDeleteCheque); onRowDeselected(SelectionModelCheque, btnAddCheque);" />
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
     <ext:Window ID="winEditAmortizationScheduleItem" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="105" Hidden="true"
        Title="Edit Amortization Schedule Item" Width="520">
        <Items>
         <ext:FormPanel Padding="10" runat="server" ID="frmAddress" LabelWidth="150">
         <Items>
         <ext:TextField runat="server" ID = "txtPrincipalPayment" FieldLabel = "Principal Payment (Php)" />
         </Items>
         <BottomBar>
         <ext:Toolbar runat="server">
         <Items>
         <ext:ToolbarFill runat="server" />
         <ext:Button runat="server" ID = "btnWinSave" Text = "Save" Icon = "Disk" />
         <ext:Button runat="server" ID = "btnWinCancel" Text = "Cancel" Icon = "Cancel">
         <Listeners>
         <Click Handler = "#{winEditAmortizationScheduleItem}.hide();" />
         </Listeners>
         </ext:Button>
         </Items>
         </ext:Toolbar>
         </BottomBar>
         </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
