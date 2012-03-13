<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListAllowedUsers.aspx.cs" Inherits="LendingApplication.Applications.UserAccountsUseCases.PickListAllowedUsers" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addemployee', 'updateemployee']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };

        var onRowSelected = function () {
            btnSelect.enable();
        };

        var onRowDeselected = function () {
            
        };

        //SELECTCLICK
        var btnSelect_Click = function () {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(data.json, 'onpickalloweduser');
                window.proxy.requestClose();
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                AutoExpandColumn="Address"
                Layout="Fit">
                <View>
                    <ext:GridView EmptyText="No allowed users to display." />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelect_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel" Disabled="false">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Label Text="Search by Name" runat="server" />
                            <ext:ToolbarSpacer />
                            <ext:TextField runat="server" ID="txtSearch" EmptyText="Search here"></ext:TextField>
                            <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find"></ext:Button>
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
                            <ext:Parameter Name="limit" Value="10" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="PartyId">
                                <Fields>
                                    <ext:RecordField Name="PartyId" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Address" />
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
                        <ext:Column Header="Party Id" DataIndex="PartyId" Wrap="true" Locked="true" Width="140px" />
                        <ext:Column Header="Name" DataIndex="Name" Locked="true" Wrap="true" Width="140px" />
                        <ext:Column Header="Address" DataIndex="Address" Locked="true" Wrap="true" Width="140px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                        DisplayMsg="Displaying customers {0} - {1} of {2}" EmptyMsg="No customers to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
