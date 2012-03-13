<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmortizationScheduleSplitLoan.aspx.cs"
    Inherits="LendingApplication.Applications.LoanRestructureUseCases.AmortizationScheduleSplitLoan" %>

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
            showAlert('Status', 'Loans successfully split.', function () {
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
            showAlert(title, message, function () {
            });
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
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnGenerate.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnGenerate.disable();
            }
        }

        var checkGrid = function () {
            if (storeAmortizationSchedule.Count() == 0 && store1.Count() == 0) {
                btnSave.disable();
            } else if (storeAmortizationSchedule.Count() != 0 && store1.Count() != 0) {
                btnSave.enable();
            }
        };

        var generateSuccessful = function () {
            btnSave.enable();
        };

        var onBtnAddCheque = function (selectionModel) {
                var selectedRows = selectionModel.getSelections();
                var url = '/Applications/LoanRestructureUseCases/AddCheque.aspx';
                var randomKey = '&key=' + selectedRows[0].json.RandomKey;
                var param = url + "?ResourceGuid=" + hiddenResourceGUID.getValue();
                var type = "&T=splitLoan"
                var param = param + "&CustomerId=" + hiddenCustomerId.getValue() + "&C=" + counters + type + randomKey;
                window.proxy.requestNewTab('ManageCheque', param, 'Manage Cheque');
                counters++;

        };

        var onRowSelected = function (btn) {
            btn.enable();
        };
        var onRowDeselected = function (grid, btn) {
            if (grid.hasSelection() == false) {
                btn.disable();
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenStatus" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Viewport ID="pageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel runat="server" ID="RootFormPanel" Layout="FitLayout" Border="false">
                <TopBar>
                    <ext:Toolbar runat="server" ID="RootToolBar">
                        <Items>
                            <ext:Button runat="server" Hidden="true" ID="btnGenerate" Text="Generate" Icon="Accept" />
                            <ext:Button runat="server" ID="btnSave" Text="Save" Icon="Disk" >
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful" Failure="saveFailure" >
                                        <EventMask Msg="Saving amortization schedule..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelGenerateAmortizationSchedule" Padding="0" Layout="FormLayout"
                        LabelWidth="180">
                        <Items>
                            <ext:TabPanel runat="server" ID="PageTabPanel">
                            <Items>
                            <ext:Panel runat="server" ID="pnlAccount1" Title="Account 1" AutoHeight="true" LabelWidth="420" Border="false">
                                <Items>
                                    <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule1" Height="250" MinHeight="250"
                                        Title="Amortization Schedule" EnableColumnHide="false" EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <LoadMask Msg="Loading..." ShowMask="true" />
                                        <Store>
                                            <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                                <Reader>
                                                    <ext:JsonReader>
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
                                                <ext:NumberColumn Header="Total Due" DataIndex="TotalPayment" Wrap="true" Width="150" Sortable="false"
                                                    Format=",000.00" />
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Loan Balance" Hidden="true" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="150" Sortable="false" Format=",000.00" />
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                    <ext:GridPanel ID="grdpnlCheque1" Title="Cheque" runat="server" Disabled="true" AutoScroll="true" AutoExpandColumn="BankName" Height="250" MinHeight="250">
                                        <Items>
                                            <ext:Toolbar ID="Toolbar1" Hidden="true" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnAddCheque" Disabled="true" runat="server" Text="Edit" Icon="NoteEdit">
                                                        <Listeners>
                                                            <Click Handler="onBtnAddCheque(SelectionModelCheque);"/>
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
                                            <RowDblClick Handler="onBtnAddCheque(SelectionModelCheque);" />
                                        </Listeners>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnlAccount2" runat="server" Title="Account 2" AutoHeight="true" LabelWidth="420" Border="false">
                                <Items>
                                    <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule2" Height="250" MinHeight="250"
                                        Title="Amortization Schedule" EnableColumnHide="false" EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <Store>
                                            <ext:Store runat="server" ID="store1" RemoteSort="false">
                                                <Reader>
                                                    <ext:JsonReader>
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
                                        <ColumnModel runat="server" ID="ColumnModel1" Width="100%">
                                            <Columns>
                                                <ext:Column Header="Unit" DataIndex="Counter" Wrap="true"
                                                    Width="100" Sortable="false"/>
                                                <ext:Column Header="Payment Due Date" DataIndex="_ScheduledPaymentDate" Wrap="true"
                                                    Width="150" Sortable="false" />
                                                <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total" DataIndex="TotalPayment" Wrap="true" Width="150" 
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true" Width="150"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Loan Balance" Hidden="true" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="150" Sortable="false" />
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                    <ext:GridPanel ID="grdpnlCheque2" runat="server" Title="Cheque" Disabled="true" AutoScroll="true" AutoExpandColumn="BankName" Height="250" MinHeight="250">
                                        <Items>
                                            <ext:Toolbar ID="Toolbar2" Hidden="true" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnAddCheque2" Disabled="true" runat="server" Text="Edit" Icon="NoteEdit">
                                                        <Listeners>
                                                            <Click Handler="onBtnAddCheque(SelectionModelCheque2);"/>
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="storeCheques2" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ChequeId" />
                                                            <ext:RecordField Name="ChequeNumber" />
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="ChequeDate"/>
                                                            <ext:RecordField Name="BankName" />
                                                            <ext:RecordField Name="Status" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelCheque2" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnAddCheque2);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelCheque2, btnAddCheque2);" />
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
                                                <ext:DateColumn runat="server" Header="Check Date" DataIndex="ChequeDate" Width="150" Format="MMM dd yyyy">
                                                </ext:DateColumn>
                                                <ext:Column runat="server" Header="Bank Name" DataIndex="BankName" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Status" DataIndex="Status" Width="150">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <Listeners>
                                            <RowDblClick Handler="onBtnAddCheque(SelectionModelCheque2);" />
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
                    <ext:TextField runat="server" ID="txtPrincipalPayment" FieldLabel="Principal Payment (Php)" />
                </Items>
                <BottomBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:ToolbarFill runat="server" />
                            <ext:Button runat="server" ID="btnWinSave" Text="Save" Icon="Disk" />
                            <ext:Button runat="server" ID="btnWinCancel" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="#{winEditAmortizationScheduleItem}.hide();" />
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
