<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListReceipts.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.ListReceipts" %>
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
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addreceipt', 'updatereceipt', 'cancelreceipt', 'addcollection', 'checkstatuschanged', 'addcustomersalary']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {   
                var url = '/Applications/ReceiptUseCases/EditReceipt.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateReceipt', param, 'Update Receipt');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddReceipt', '/Applications/ReceiptUseCases/AddReceipt.aspx', 'Add Receipt');
        };

        var onBtnCancelClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                //                var url = '/Applications/ReceiptUseCases/CancelReceipt.aspx';
                //                var id = 'id=' + selectedRows[0].id;
                //                var param = url + "?" + id;
                //                window.proxy.requestNewTab('CancelReceipt', param, 'Cancel Receipt');
                wdwCancelReceipt.show();
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        /**********ROW SELECTED - ROW DESELECTED**********/
        var onRowSelected = function () {
            enableOrDisableButtons();
            btnOpen.enable();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnCancel.disable();
                btnOpen.disable();
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

        var cancelReceipt = function () {
            showConfirm('Cancel Receipt', 'Are you sure you want to cancel the selected receipt record?', function (btn, text) {
                if (btn.toLocaleLowerCase() == 'yes') {
                    var selectedRows = PageGridPanelSelectionModel.getSelections();
                    if (selectedRows && selectedRows.length > 0) {
//                        var url = '/Applications/ReceiptUseCases/CancelReceipt.aspx';
//                        var id = 'id=' + selectedRows[0].id;
//                        var param = url + "?" + id;
//                        window.proxy.requestNewTab('CancelReceipt', param, 'Cancel Receipt');
                        wdwCancelReceipt.show();
                    }
                } else {
                    
                }
            });
            //return false;
        }

        var enableOrDisableButtons = function (status) {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            btnOpen.disable();
            btnCancel.disable();
            btnClose.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json
                if (canOpenAndCancel(json.Status)) {
                    btnCancel.enable();
                }

                if (json.Status == 'Applied') {
                    btnClose.enable();
                }
            }
        }

        var canOpenAndCancel = function (status) {
            if (status != 'Open') {
                return false;
            } else {
                return true;
            }
        }
        var RefreshItems = function () {
            PageGridPanel.reload();
        }

        var btnCloseSuccess = function () {
            showAlert('Success', 'The selected receipt have been successfully closed.', function () {
                RefreshItems();
            });
        }

        var cancelSuccess = function () {
            wdwCancelReceipt.hide();
            showAlert('Success', 'The selected receipt/s have been successfully cancelled.', function () {
                RefreshItems();
            });
        }
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" Border="false">
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView EmptyText="No receipts to display." />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y">
                                    </ext:Hidden>
                                    <%-- CANCEL BUTTON --%>
                                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Delete" Disabled="false">
                                        <Listeners>
                                            <Click Handler="cancelReceipt();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <%-- OPEN BUTTON --%>
                                    <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="false">
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
                                    <ext:ToolbarSeparator runat="server" Hidden="true"/>
                                    <%-- CLOSE BUTTON --%>
                                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cross" Disabled="true" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnClose_DirectClick" Success="btnCloseSuccess">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to close the seleceted receipt?" />
                                                <EventMask ShowMask="true" Msg="Closing.." />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Label ID="Label3" runat="server" Text="Filter by Date Received From" />
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:DateField ID="dtFrom" runat="server" EndDateField="dtTo" Editable="false"/>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:Label ID="Label4" runat="server" Text="To" />
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:DateField ID="dtTo" runat="server" StartDateField="dtFrom" AllowBlank="true" Editable="false"/>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:Label ID="Label2" runat="server" Text="Search by" />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="5" />
                                    <ext:ComboBox ID="cmbSearchBy" runat="server" Width="120" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="Received From" Value="ReceivedFrom" />
                                            <ext:ListItem Text="Received By" Value="ReceivedBy" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:TextField runat="server" ID="txtSearchInput" EmptyText="type here.."/><%-- txtSearch --%>
                                    
                                    
                                    <ext:ToolbarSpacer runat="server" Width="5" />
                                    <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find">
                                        <DirectEvents>
                                            <Click OnEvent="btnSearch_DirectClick" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                    <ext:ComboBox ID="cmbFilterCategories" runat="server" OnDirectSelect="cmbFilterCategories_TextChanged"
                                        Editabel="false" Width="120" EmptyText="Filter By" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="All" />
                                            <ext:ListItem Text="Status" />
                                            <ext:ListItem Text="Payment Method" />
                                            <ext:ListItem Text="Currency" />
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
                                    <ext:RecordField Name="DateReceived"/>
                                    <ext:RecordField Name="ReceivedFrom" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="PaymentMethod" />
                                    <ext:RecordField Name="Status" />
                                    <ext:RecordField Name="ReceivedBy" />
                                    <ext:RecordField Name="ReceiptBalance" />
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
                        
                        <ext:Column Header="Receipt ID" DataIndex="ReceiptID" Wrap="true" Locked="true" Width="75px" />
                        <ext:Column Header="Date Received" DataIndex="DateReceived" Locked="true" Wrap="true" Width="120px" />
                        <ext:Column Header="Received From" DataIndex="ReceivedFrom" Locked="true" Wrap="true" Width="200px" />
                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Locked="true" Wrap="true" Width="140px" Format=",000.00"/>
                        <ext:NumberColumn Header="Balance" DataIndex="ReceiptBalance" Locked="true" Wrap="true" Width="140px" Format=",000.00" />
                        <ext:Column Header="Currency" DataIndex="Currency" Locked="true" Wrap="true" Width="80" />
                        <ext:Column Header="Payment Method" DataIndex="PaymentMethod" Locked="true" Wrap="true" Width="120px" />
                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="100px" />
                        <ext:Column Header="Receipt Type" DataIndex="ReceiptType" Locked="true" Wrap="true" Width="100px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying receipts {0} - {1} of {2}" EmptyMsg="No receipts to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="wdwCancelReceipt" runat="server" Modal="true" Title="Cancel Receipt" Hidden="true" Width="450" Height="200"
        Draggable="false" Resizable="false" Closable="false">
        <Items>
            <ext:FormPanel ID="CancelFormPanel" runat="server" Padding="5" Layout="FitLayout" Border="false" MonitorValid="true">
                <Items>
                    <ext:TextArea ID="txtRemarks" runat="server" FieldLabel="Remarks" Height="103" AllowBlank="false"/>   
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Hidden="true"/>
                </BottomBar>
                <Listeners>
                    <Clientvalidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave_CancelReceipt}.enable();}  else{#{btnSave_CancelReceipt}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave_CancelReceipt" runat="server" Text="Save" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="btnSave_onDirectEventClick" Success="cancelSuccess" Before="return #{CancelFormPanel}.getForm().isValid()">
                        <EventMask Msg="Updating receipt/s.." ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel_CancelReceipt" runat="server" Text="Cancel" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{wdwCancelReceipt}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
