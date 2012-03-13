<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListChequeEditor.aspx.cs" Inherits="LendingApplication.Applications.ChequeEditorUseCases.ListChequeEditor" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcheque', 'updatecheque', 'onpickcustomer','onpickbank']);
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
            enableOrDisableButtons();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnEdit.disable();
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
            btnEdit.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json
                if (canEdit(json.Status)) {
                    btnEdit.enable();
                }
            }
        };

        var canEdit = function (status) {
            if (status != 'Received') {
                return false;
            } else {
                return true;
            }
        };

        var RefreshItems = function () {
            PageGridPanel.reload();
        }

        ////////////////////////PICK LIST////////////////////////////////
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                // do your stuff here
                hdnCustomerID.setValue(msg.data.CustomerID);
                txtReceivedFrom.setValue(msg.data.Names);
                X.FillDistrictAndStation(msg.data.CustomerID);

            } else if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            }
        };

        var reset = function () {
            ChequeFormPanel.reset();
            
        }

        var saveSuccessfull = function () {
            showAlert('Success','Save successful.');
        };

        var validateCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                    }
                }
            });
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel ID="pnlChequeEditor" runat="server" Layout="RowLayout" Border="false">
                <Items>
                    <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y"></ext:Hidden>
                    <ext:Hidden ID="hdnLoggedInPartyRoleId" runat="server" />
                    <ext:FormPanel ID="ChequeFormPanel" runat="server" Height="360" LabelWidth="180" MonitorValid="true" MonitorPoll="500" Padding="5" Title="Check Editor"
                        AnchorHorizontal="95%" AutoScroll="true" Border="false">
                        <Items>
                            <ext:Hidden ID="hdnReceiptId" runat="server"></ext:Hidden>
                            <ext:CompositeField ID="CompositeField1" runat="server"><%--Received From--%>
                                <Items>
                                    <ext:TextField ID="txtReceivedFrom" runat="server" ReadOnly="true" Width="400" FieldLabel="Received From" AllowBlank="false"/>
                                    <ext:Button ID="btnCustomerBrowse" runat="server" Text="Browse">
                                        <Listeners>
                                            <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/Applications/CustomerUseCases/CustomerPickList.aspx?mode=single', 'Customer List');" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:Hidden ID="hdnCustomerID" runat="server" /><%--Customer ID--%>
                            <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Width="400" Editable="false" AllowBlank="false"/><%--Transaction Date--%>
                            <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="400" AllowBlank="false" MaskRe="[0-9\.\,]">
                                <Listeners>
                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                </Listeners>
                            </ext:TextField><%--Amount--%>
                            <ext:TextField ID="txtReceivedBy" runat="server" FieldLabel="Received By" Width="400" ReadOnly="true" AllowBlank="true"/><%--Received By--%>
                            <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Cheque Payment Method" Width="400" Editable="false" AllowBlank="false"><%--Payment Method--%>
                                <Items>
                                    <ext:ListItem Text="Pay Check" />
                                    <ext:ListItem Text="Personal Check" />
                                </Items>
                                <DirectEvents>
                                    <Select OnEvent="cmbPaymentMethod_OnSelect"></Select>
                                </DirectEvents>          
                            </ext:ComboBox>
                            <ext:CompositeField ID="CompositeField2" runat="server">
                                <Items>
                                    <ext:TextField ID="txtBank" runat="server" ReadOnly="true" Width="400" FieldLabel="Bank" AllowBlank="false"/>
                                    <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                                        <Listeners>
                                            <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:Hidden ID="hdnBankID" runat="server"/>
                            <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="400" AllowBlank="false">
                                <Listeners>
                                    <Change Handler="validateCheckNumber();" />
                                </Listeners>
                            </ext:TextField>
                            <ext:ComboBox ID="cmbCheckStatus" runat="server" FieldLabel="Status" Width="400" Editable="false" ValueField="Id"
                                    DisplayField="Name">
                                <Store>
                                    <ext:Store ID="strCheckStatus" runat="server">
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
                            <ext:TextField ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Width="400" AllowBlank="true"/>
                            <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="400" Editable="false" AllowBlank="false"/>
                        </Items>
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                        <DirectEvents>
                                            <Click OnEvent="btnSave_Click" Success="saveSuccessfull" Before="return #{ChequeFormPanel}.getForm().isValid();">
                                                <EventMask Msg="Saving.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnClear" runat="server" Text="Clear" Icon="Erase">
                                        <Listeners>
                                            <Click Handler="reset();" />
                                        </Listeners>
                                        <%--<DirectEvents>
                                            <Click OnEvent="btnClear_Click">
                                                <EventMask Msg="Clearing.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>--%>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <BottomBar>
                            <ext:StatusBar runat="server"></ext:StatusBar>
                        </BottomBar>
                        <Listeners>
                            <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                        </Listeners>
                    </ext:FormPanel>
                    <ext:GridPanel
                        ID="PageGridPanel"
                        runat="server" Border="false"
                        RowHeight=".39" Height="800" AutoScroll="true">
                        <View>
                            <ext:GridView EmptyText="No cheques to display." />
                        </View>
                        <LoadMask ShowMask="true" Msg="Loading.." />
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button ID="btnEdit" runat="server" Icon="NoteEdit" Text="Edit" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnEdit_Click">
                                                <EventMask Msg="Retrieving record.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
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
                                            <ext:RecordField Name="ChequeNumber" />
                                            <ext:RecordField Name="Bank" />
                                            <ext:RecordField Name="DateReceived" />
                                            <ext:RecordField Name="ReceivedFrom" />
                                            <ext:RecordField Name="Amount" />
                                            <ext:RecordField Name="Status" />
                                            <ext:RecordField Name="ChequeDate" />
                                            <ext:RecordField Name="_ChequeDate" />
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
                                <ext:Column Header="Cheque Number" DataIndex="ChequeNumber" Wrap="true" Locked="true" Width="140px" />
                                <ext:Column Header="Bank" DataIndex="Bank" Wrap="true" Locked="true" Width="140px" />
                                <ext:Column Header="Date Received" DataIndex="DateReceived" Locked="true" Wrap="true" Width="140px">
                                    <Renderer Handler="return Ext.util.Format.date(value, Hidden1.value);" />
                                </ext:Column>
                                <ext:Column Header="Received From" DataIndex="ReceivedFrom" Locked="true" Wrap="true" Width="140px" />
                                <ext:NumberColumn Header="Amount" DataIndex="Amount" Locked="true" Wrap="true" Width="140px" Format=",000.00"/>
                                <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="140px" />
                                <ext:Column Header="Cheque Date" DataIndex="ChequeDate" Hidden="true" Locked="true" Wrap="true" Width="140px">
                                    <Renderer Handler="return Ext.util.Format.date(value, Hidden1.value);" />
                                </ext:Column>
                                <ext:Column Header="Cheque Date" DataIndex="_ChequeDate" Hidden="true" Locked="true" Wrap="true" Width="140px" />
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                DisplayMsg="Displaying cheques {0} - {1} of {2}" EmptyMsg="No cheques to display" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
