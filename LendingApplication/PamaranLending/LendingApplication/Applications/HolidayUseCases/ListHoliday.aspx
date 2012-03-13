<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListHoliday.aspx.cs" Inherits="LendingApplication.Applications.HolidayUseCases.ListHoliday" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Holiday List</title>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addholiday', 'updateholiday']);
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
                var url = '/Applications/HolidayUseCases/EditHoliday.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateHoliday', param, 'Update Holiday');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddHoliday', '/Applications/HolidayUseCases/AddHoliday.aspx', 'Add Holiday');
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
    </script>
      <style type="text/css">
        .x-grid-empty {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                AutoExpandColumn="Name"
                Layout="Fit">
            <View>
            <ext:GridView EmptyText="No holidays to display">
            </ext:GridView>
            </View>
                <LoadMask ShowMask = "true" />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                        <ext:Hidden ID="Hidden1" runat="server" Text="m/d/Y"></ext:Hidden>
                             <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onRowDeselected();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected holiday/s?" />
                                        <EventMask ShowMask="true" Msg="Deleting.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeleteSeparator" />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnOpenSeparator" />
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
                                    <ext:RecordField Name="Id" />
                                    <ext:RecordField Name="Date" />
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
                        <ext:Column Header="ID" DataIndex="Id" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Name" Locked="true" Wrap="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Date" DataIndex="Date" Locked="true" Wrap="true"
                            Width="140px">
                            <Renderer Handler="return Ext.util.Format.date(value, Hidden1.value);" />
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying holidays {0} - {1} of {2}" EmptyMsg="No holidays to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
