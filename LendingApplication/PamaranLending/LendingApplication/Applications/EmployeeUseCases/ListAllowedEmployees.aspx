<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAllowedEmployees.aspx.cs" Inherits="LendingApplication.Applications.EmployeeUseCases.ListAllowedEmployees" %>
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
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };


        var onbtnSelect = function () {
            window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'onpickallowedemployee');
            window.proxy.requestClose();
        }

        var onRowSelected = function () {
            btnDelete.enable();
            btnOpen.enable();
        };
        var onRowDeselected = function () {
//            if (PageGridPanel.hasSelection() == false) {
//                btnDelete.disable();
//                btnOpen.disable();
//            }
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                Layout="Fit">
                <View>
                    <ext:GridView EmptyText="No allowed employees to display." />
                </View>
                <LoadMask Msg="Loading.." ShowMask="true"/>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor">
                                <Listeners>
                                    <Click Handler="onbtnSelect();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
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
                            <ext:JsonReader IDProperty="PartyId">
                                <Fields>
                                    <ext:RecordField Name="PartyId" />
                                    <ext:RecordField Name="Name" />
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
                        <ext:Column Header="Party Id" DataIndex="PartyId" Wrap="true" Locked="true" Width="100px" />
                        <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" Width="250px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying customers {0} - {1} of {2}" EmptyMsg="No customers to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
