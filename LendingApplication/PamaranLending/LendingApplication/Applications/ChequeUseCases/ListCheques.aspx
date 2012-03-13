<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCheques.aspx.cs" Inherits="LendingApplication.Applications.ChequeUseCases.ListCheques" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
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
            window.proxy.init(['addcheque', 'updatecheque', 'checkdeposited', 'cancelreceipt']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {   
                var url = '/Applications/ChequeUseCases/EditCheque.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCheque', param, 'Update Cheque');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddCheque', '/Applications/ChequeUseCases/AddCheque.aspx', 'Add Cheque');
        };

        var onBtnCancelClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/ChequeUseCases/CancelCheque.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('CancelCheque', param, 'Cancel Cheque');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        /**********ROW SELECTED - ROW DESELECTED**********/
        var onRowSelected = function () {
        btnOpen.enable
            enableOrDisableButtons();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnOpen.disable();
                enableOrDisableButtons();
            }
            else {
                enableOrDisableButtons();
            }
        };
        /*************************************************/

        //MESSAGES TO SHOW
        var modifyFailed = function () {
            Ext.MessageBox.show({
                title: 'Modify Failed',
                msg: 'The selected receipt record is no longer opened. It cannot be modified.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var cancelFailed = function () {
            Ext.MessageBox.show({
                title: 'Cancel Failed',
                msg: 'The selected receipt record is already cancelled.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var cancelFailed2 = function () {
            Ext.MessageBox.show({
                title: 'Cancel Failed',
                msg: 'The selected receipt record is applied or closed. It cannot be cancelled.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var enableOrDisableButtons = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            btnOpen.disable();
            btnDeposit.disable();
            btnClear.disable();
            btnBounced.disable();
            btnCancel.disable();
            btnOnHold.disable();
            btnApplyAsPayment.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json
                btnOpen.enable();



                if (canDeposit(json.Status)) {
                    btnDeposit.enable();
                }

                if (canClearOrBounce(json.Status)) {
                    btnClear.enable();
                    btnBounced.enable();
                }

                if (canCancel(json.Status)) {
                    btnCancel.enable();
                }

                if (canHold(json.Status)) {
                    btnOnHold.enable();
                }



                if (canApplyAsPayment(json.Status, json.ReceiptType)) {
                    X.ReceiptHasBalance(json.ReceiptID, {
                        success: function (result) {
                            if (result == 1) {
                                btnApplyAsPayment.enable();
                            } else {
                                btnApplyAsPayment.disable();
                            }
                        }
                    });

                };

            }
        }

        var canDeposit = function (status) {
            if (status == 'Received' || status == 'Bounced' || status == 'On Hold') {
                return true;
            } else {
                return false;
            }
        }

        var canClearOrBounce = function (status) {
            if (status == 'Deposited' || status == 'On Hold') {
                return true;
            } else {
                return false;
            }
        }

    

        var canCancel = function (status) {
            if (status != 'Cancelled') {
                return true;
            } else {
                return false;
            }
        }

        var canHold = function (status) {
            if (status == 'Cancelled' || status == 'On Hold') {
                return false;
            } else {
                return true;
            }
        }


        var canApplyAsPayment = function (status, receiptType) {
            if (receiptType =="Collateral" && status == 'Cleared') {
                return true;
            } else {
                return false;
            }
        }

        var RefreshItems = function () {
            PageGridPanel.reload();
        }

        var changeCheckStatusSuccessful = function () {
            btnDeposit.disable();
            btnBounced.disable();
            btnClear.disable();
            showAlert('Status', 'Check status successfully changed.');
            PageGridPanel.reload();
            PageGridPanelSelectionModel.clearSelections();
            wndCPDC.hide();
            wndCheckRemarks.hide();
            txtCheckRemarks.clear();
            window.proxy.sendToAll('checkstatuschanged', 'checkstatuschanged');
        }

//        var btnCancelClick = function () {
//            var selectedRows = PageGridPanelSelectionModel.getSelections();
//            if (selectedRows != null) {
//                showConfirm("Confirm", "Are you sure you want to cancel the selected cheque/s?", function () {
//                    if (btn.toLocaleLowerCase() == 'yes') {
//                        wndCheckRemarks.show();
//                    } else {

//                    }
//                });
//            } else {
//                showAlert
//            }
//        };


//Postdated cheques javascripts
        var isPostDatedCheck = function () {
            X.CheckIfPostedCheck({
                success: function (result) {
                    if (result == 1) {
                        return true;
                    } else {
                        return false;
                    }
                }
            });
        };
        var onFormValidated2 = function (valid) {
            btnPayCPDC.disable();
            var loanpayment = wntxtLoanPayment.getValue();
            loanpayment = loanpayment.replace(/,/g, '');

            var interestPayment = wntxtInterestPayment.getValue();
            interestPayment = interestPayment.replace(/,/g, '');

             var checkAmount = wntxtChkAmount.getValue();
             checkAmount = checkAmount.replace(/,/g, '');

             var existingInterest = wntxtExistInterest.getValue();
             existingInterest = existingInterest.replace(/,/g, '');

             var additionalInterest = wntxtAddIntererst.getValue();
             additionalInterest = additionalInterest.replace(/,/g, '');

             var totalInterest = parseFloat(existingInterest) + parseFloat(additionalInterest);
             var totalpayment = parseFloat(interestPayment)+parseFloat(loanpayment);

            if (valid && (interestPayment<=totalInterest )&&(totalpayment <= checkAmount)) {
                StatusBar2.setStatus({ text: 'Form is valid. ' });
                btnPayCPDC.enable();
            } else if (valid && (interestPayment > totalInterest) && (totalpayment <= checkAmount)) {
                StatusBar2.setStatus({ text: 'Interest Payment must be < Additional Interest + Existing Interest.' });
            } else if (valid && (interestPayment <= totalInterest) && (totalpayment > checkAmount)) {
                StatusBar2.setStatus({ text: 'Interest + Loan Payment must be < Check Balance' });
            } else if (valid) {
                StatusBar2.setStatus({ text: 'Form is invalid' });
            }else {
                StatusBar2.setStatus({ text: 'Please fill out the form.' });
            }
        }
        var appliedAsPaymentSuccessful = function () {
            showAlert('Message', 'Successfully applied posdated check as payment.', function () {
                window.proxy.sendToParent('appliedCheckAsPayment');
                CancelCheckAsPayment();
            });
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

        var applyAsPayment = function () {
            X.FillApplyAsPaymentWindow({
                success: function (result) {
                    winShow();
                }
            });
        }

        var winShow = function () {
            formatCurrency(wntxtLoanPayment);
            formatCurrency(wntxtInterestPayment);
            formatCurrency(wntxtChkAmount);
            formatCurrency(wntxtExistInterest);
            formatCurrency(wntxtAddIntererst);
            wndCPDC.show();
          
        }
         var CancelCheckAsPayment = function () {
             wndCPDC.hide();
             ClearCheckAsPayment();
             
         }
            var ClearCheckAsPayment = function () {
                wntxtLoanPayment.setValue(0);
                wntxtInterestPayment.setValue(0);
                wntxtChkAmount.setValue(0);
                wntxtExistInterest.setValue(0);
                wntxtAddIntererst.setValue(0);
                formatCurrency(wntxtLoanPayment);
                formatCurrency(wntxtInterestPayment);
                formatCurrency(wntxtChkAmount);
                formatCurrency(wntxtExistInterest);
                formatCurrency(wntxtAddIntererst);
                dtGenerationDate.clear();
            }
            var generateAdditionalInterest = function () {
                X.GenerateAdditionalInterest();
            };
            
    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 5px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" Border="false" EnableColumnHide="false" EnableColumnMove="false" Draggable="false">
                <View>
                    <ext:GridView EmptyText="No cheques to display." />
                </View>
                <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y" />
                                    <%-- OPEN BUTTON --%>
                                    <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnOpenClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <%-- NEW BUTTON --%>
                                    <ext:Button ID="btnAdd" runat="server" Text="New" Icon="Add">
                                        <Listeners>
                                            <Click Handler="onBtnAddClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnDeposit" runat="server" Text="Deposit" Icon="MoneyAdd" Disabled="true">
                                        <%-- DEPOSIT --%>
                                        <DirectEvents>
                                            <Click OnEvent="btnDeposit_Click" Success="changeCheckStatusSuccessful">
                                                <EventMask Msg="Changing status.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnClear" runat="server" Text="Clear" Icon="Accept" Disabled="true">
                                        <%-- CLEAR --%>
                                        <%--<Listeners>
                                            <Click Handler="checkIfPostDatedCheck();" />
                                        </Listeners>--%>
                                        <DirectEvents>
                                            <Click OnEvent="btnClear_Click" Success="changeCheckStatusSuccessful">
                                                <EventMask Msg="Changing status.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnBounced" runat="server" Text="Bounced" Icon="Error" Disabled="true">
                                        <%-- BOUNCED --%>
                                        <DirectEvents>
                                            <Click OnEvent="btnBounced_Click" Success="changeCheckStatusSuccessful">
                                                <EventMask Msg="Changing status.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnOnHold" runat="server" Text="On Hold" Disabled="true" Icon="PauseBlue">
                                        <%-- ON HOLD --%>
                                        <DirectEvents>
                                            <Click OnEvent="btnOnHold_Click" Success="changeCheckStatusSuccessful">
                                                <EventMask Msg="Changing status.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Disabled="true" Icon="Cancel">
                                        <%-- CANCEL --%>
                                        <DirectEvents>
                                            <Click OnEvent="btnCancel_Click">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to cancel the selected cheque/s?" Title="Cancel Check" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnApplyAsPayment" runat="server" Text="Apply As Payment" Disabled="true" Icon="MoneyAdd" ToolTip="Must be a Cleared Collateral Cheque.">
                                        <%-- APPLY AS PAYMENT --%>
                                        <Listeners>
                                            <Click Handler="applyAsPayment();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server">
                                <Items>
                                <ext:Hidden ID="hdnLoanId" runat="server"></ext:Hidden>
                                    <ext:Label ID="Label1" runat="server" Text="Filter by Check Date" />
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:DateField ID="dtFrom" runat="server" EndDateField="dtTo" Editable="false"/>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:Label ID="Label2" runat="server" Text="To" />
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:DateField ID="dtTo" runat="server" StartDateField="dtFrom" AllowBlank="true" Editable="false"/>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:ComboBox ID="cmbSearchBy" runat="server" Width="120" EmptyText="Search By.." Editable="false">
                                        <Items>
                                            <ext:ListItem Text="Received From" Value="ReceivedFrom" />
                                            <ext:ListItem Text="Check Number" Value="CheckNumber" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:TextField runat="server" ID="txtSearchInput" EmptyText="type here.."/>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find">
                                        <DirectEvents>
                                            <Click OnEvent="btnSearch_DirectClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                    <ext:ComboBox ID="cmbFilterCategories" runat="server" EmptyText="Filter By.." OnDirectSelect="cmbFilterCategories_TextChanged"
                                        Editabel="false" Width="120">
                                        <Items>
                                            <ext:ListItem Text="All" Value="All" />
                                            <ext:ListItem Text="Status" Value="Status" />
                                            <ext:ListItem Text="Bank" Value="Bank" />
                                            <ext:ListItem Text="Currency" Value="Currency" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="3" />
                                    <ext:ComboBox ID="cmbFilterItems" runat="server" ValueField="Id" DisplayField="Name"
                                        Editable="false" Width="120">
                                        <Listeners>
                                            <Select Handler="RefreshItems();" />
                                        </Listeners>
                                        <Store>
                                            <ext:Store ID="strFilterItems" runat="server">
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
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false" OnRefreshData="RefreshData">
                        <Proxy>
                            <ext:PageProxy>
                            </ext:PageProxy>
                        </Proxy>
                        <AutoLoadParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="ReceiptID">
                                <Fields>
                                    <ext:RecordField Name="ReceiptID" />
                                    <ext:RecordField Name="ChequeNumber" />
                                    <ext:RecordField Name="Bank" />
                                    <ext:RecordField Name="DateReceived" />
                                    <ext:RecordField Name="ReceivedFrom" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="Status" />
                                    <ext:RecordField Name="ChequeDate" />
                                    <ext:RecordField Name="PaymentMethodType" />
                                    <ext:RecordField Name="Remarks" />
                                    <ext:RecordField Name="IsPostDated" />
                                    <ext:RecordField Name="Currency" />
                                    <ext:RecordField Name="ReceiptType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Id" DataIndex="ReceiptID" Wrap="true" Locked="true" Width="40px"></ext:Column>
                          <ext:Column Header="Date Received" DataIndex="DateReceived" Locked="true" Wrap="true" Width="100px" />
                        <ext:Column Header="Received From" DataIndex="ReceivedFrom" Locked="true" Wrap="true" Width="120px" />
                        <ext:Column Header="Amount" DataIndex="Amount" Locked="true" Wrap="true" Width="125px"/>
                        <ext:Column Header="Cheque Number" DataIndex="ChequeNumber" Wrap="true" Locked="true" Width="125px" />
                        <ext:Column Header="Cheque Date" DataIndex="ChequeDate" Locked="true" Wrap="true" Width="100px" />
                        <ext:Column Header="Cheque Type" DataIndex="PaymentMethodType" Wrap="true" Locked="true" Width="120px" />
                         <ext:Column Header="Bank" DataIndex="Bank" Wrap="true" Locked="true" Width="140px" />
                         <ext:Column Header="Receipt Type " DataIndex="ReceiptType" Wrap="true" Locked="true" Width="90px" />
                          <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="80px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying cheques {0} - {1} of {2}" EmptyMsg="No cheques to display" />
                </BottomBar>
            </ext:GridPanel>
            <ext:Window ID="wndCheckRemarks" runat="server" Hidden="true" Modal="true" Width="450" Height="200" Title="Cancel Check" Resizable="false" Closable="false" Draggable="false">
                <Items>
                    <ext:FormPanel ID="CancelFormPanel" runat="server" Padding="5" Border="false" Layout="FitLayout" MonitorValid="true">
                        <Items>
                             <ext:TextArea ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Height="103" AllowBlank="false"/>
                        </Items>
                        <BottomBar>
                            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Hidden="true"/>
                        </BottomBar>
                        <Listeners>
                            <Clientvalidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Success="changeCheckStatusSuccessful" Before="return #{CancelFormPanel}.getForm().isValid()">
                                <EventMask Msg="Saving.." ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnCloseCheckRemarks" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndCheckRemarks}.hide(); txtCheckRemarks.clear();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>
            <ext:Window ID="wndCPDC" runat="server" Title="Apply as Payment" Modal="true" Layout="Fit"
                Width="450" Height="350" Hidden="true" Resizable="false" Closable="false" Draggable="false">
                <Items>
                    <ext:FormPanel Padding="10" runat="server" ID="formAppyAsPayment" LabelWidth="200"
                        MonitorValid="true">
                        <Defaults>
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:DateField ID="wnDtTransactionDate" runat="server"  FieldLabel="Transaction Date" Editable="false" AnchorHorizontal="95%" AllowBlank="false"></ext:DateField>
                            <ext:TextField ID="wntxtChkAmount" runat="server" ReadOnly="true" FieldLabel="Check Balance"  AnchorHorizontal="95%"/>
                            <ext:TextField ID="wntxtLoanBalance" runat="server" ReadOnly="true" FieldLabel="Remaining Loan Balance" AnchorHorizontal="95%"/>
                            <ext:TextField ID="wntxtExistInterest" runat="server" ReadOnly="true" FieldLabel="Existing Billed Interest" AnchorHorizontal="95%" />
                            <ext:TextField ID="wntxtExistInterestDate" runat="server" ReadOnly="true" FieldLabel="Existing Interest Date" AnchorHorizontal="95%" />
                            <ext:DateField ID="dtGenerationDate" runat="server" FieldLabel="Generate Additional Interest"
                                Editable="false" AnchorHorizontal="95%">
                                <Listeners>
                                     <Select Handler="generateAdditionalInterest();" />
                                </Listeners>
                                </ext:DateField>
                            <ext:TextField ID="wntxtAddIntererst" runat="server" FieldLabel="Additional Interest Amount" AnchorHorizontal="95%" ReadOnly="true"></ext:TextField>
                            <ext:TextField ID="wntxtLoanPayment" runat="server" FieldLabel="Loan Payment" AllowBlank="false" AnchorHorizontal="95%">
                             <Listeners>
                            <Change Handler="formatCurrency(wntxtLoanPayment);" />
                            </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="wntxtInterestPayment" runat="server" FieldLabel="Interest Payment" AllowBlank="false" AnchorHorizontal="95%">
                            <Listeners>
                            <Change Handler="formatCurrency(wntxtInterestPayment);" />
                            </Listeners>
                            </ext:TextField>
                        </Items>
                        <BottomBar>
                            <ext:StatusBar ID="StatusBar2" runat="server" Height="25" />
                        </BottomBar>
                        <Buttons>
                            <ext:Button ID="btnPayCPDC" runat="server" Text="Pay" Icon="Accept" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="ApplyCheckAsPayment" Success="appliedAsPaymentSuccessful">
                                        <EventMask Msg="Applying.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelCPDC" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="CancelCheckAsPayment();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                        <Listeners>
                            <ClientValidation Handler="onFormValidated2(valid)" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
