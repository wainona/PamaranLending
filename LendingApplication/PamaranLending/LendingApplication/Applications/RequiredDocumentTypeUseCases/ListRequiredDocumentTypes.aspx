<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListRequiredDocumentTypes.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.RequiredDocumentTypeUseCases.ListRequiredDocumentTypes" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Required Document Types List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addrequireddocumenttype', 'updaterequireddocumenttype']);
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
                var url = '/Applications/RequiredDocumentTypeUseCases/EditRequiredDocumentType.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateRequiredDocumentType', param, 'Update Required Document Type');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddRequiredDocumentType', '/Applications/RequiredDocumentTypeUseCases/AddRequiredDocumentType.aspx', 'Add Required Document Type');
        };

        var onRowSelected = function () {
            btnDelete.enable();
            btnOpen.enable();

        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
            }
        };

        var onDeleteFailure = function () {
            showAlert('Status', 'The selected required document type is currently used and cannot be deleted.');
        };

        var onDeleteSuccessful = function () {
            showAlert('Status', 'The selected required document type/s was successfully deleted.');
        };
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="PageFormPanel" 
        runat="server" 
        Padding="0" 
        ButtonAlign="Right" 
        MonitorValid="true"
        Border="false"
        Title="Required Document Types List"
        BodyStyle="background-color:transparent"
        Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                AutoExpandColumn="Name"
                Layout="Fit">
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No required document types to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onRowDeselected(); onDeleteSuccessful(); " Failure="onDeleteFailure();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected required document type/s?" />
                                        <EventMask Msg="Deleting..." ShowMask="true" />
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
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="Name" />
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
                        <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" Width="280">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying required document types {0} - {1} of {2}" EmptyMsg="No required document types to display" />
                </BottomBar>
                <LoadMask ShowMask="true"/>
            </ext:GridPanel>
        </Items>
    </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
