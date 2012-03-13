<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCustomerClassification.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases.ListCustomerClassification" %>
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
            window.proxy.init(['addcustomerclassification', 'updatecustomerclassification']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };
        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0)
            {   
                var url = '/Applications/CustomerClassificationUseCases/EditCustomerClassification.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCustomerClassification', param, 'Update Customer Classification');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBeforeBtnDeleteClick = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow)
                X.CanDeleteCustomerClassifications(PageGridPanelSelectionModel.getSelected().id, {
                    success: function (result) {
                        if (result) {
                            Ext.QuickTips.disable();
                            btnDelete.enable();
                        }
                        else {
                            btnDelete.disable();
                            Ext.QuickTips.enable();
                        }
                    }
                });
        }

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddCustomerClassification', '/Applications/CustomerClassificationUseCases/AddCustomerClassification.aspx', 'Add Customer Classification');
        };

        var onRowSelected = function () {
            btnOpen.enable();
            onBeforeBtnDeleteClick();
            Ext.QuickTips.disable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                Ext.QuickTips.disable();
            }
        };
        var success = function () {
            showAlert('Status', 'Customer Classification record was successfully deleted.');
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                Ext.QuickTips.disable();
            }
        };


        var onBtnOpenDistrictTypeClick = function () {
            var url = '/Applications/CustomerClassificationUseCases/DistrictTypes.aspx';
            window.proxy.requestNewTab('DistrictTypes', url, 'District Types');
        }

    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        .x-grid-empty {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport runat="server" Layout="FitLayout"><Items>
    <ext:FormPanel ID="PageFormPanel" 
        runat="server" 
        ButtonAlign="Right" 
        MonitorValid="true"
        Title="Customer Classification Types"
        BodyStyle="background-color:transparent"
        Layout="FitLayout">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                Layout="Fit">
                <View>
                <ext:GridView EmptyText="No customer classifications to display" DeferEmptyText="false">
                </ext:GridView>
                </View>
                <LoadMask ShowMask="true" Msg="Loading Customer Classification.."/>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true" ToolTip="The selected customer classification record/s cannot be deleted.">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="success();">
                                        <Confirmation BeforeConfirm="onBeforeBtnDeleteClick" ConfirmRequest="true" Message="Are you sure you want to delete the customer classification record/s?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                <Listeners>
                                    <Click Handler="onBtnAddClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnOpenDistrictTypes" runat="server" Text="District Type" Icon="Building">
                                <Listeners>
                                    <Click Handler="onBtnOpenDistrictTypeClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search classification by..">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Items>
                                    <ext:ListItem Value="District" Text="District"/>
                                    <ext:ListItem Value="StationNumber" Text="Station Number" />
                                    <ext:ListItem Value="Type" Text="District Type"/>
                                </Items>
                                 <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ToolbarSeparator></ext:ToolbarSeparator>
                            <ext:TextField ID="txtSearch" runat="server"></ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click">
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
                            <ext:Parameter Name="limit" Value="22" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="District" />
                                    <ext:RecordField Name="StationNumber" />
                                    <ext:RecordField Name="Type"></ext:RecordField>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="District" Width="280" DataIndex="District" Wrap="true" Locked="true">
                        </ext:Column>
                        <ext:Column Header="Station Number" Width="280" DataIndex="StationNumber" Locked="true" Wrap="true">
                        </ext:Column>
                        <ext:Column Header="District Type" Width="280" DataIndex="Type" Locked="true" Wrap="true"></ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="22" DisplayInfo="true"
                        DisplayMsg="Displaying customer classification types {0} - {1} of {2}" EmptyMsg="No customer classification type to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:FormPanel>
    </Items></ext:Viewport>
    </form>
</body>
</html>
