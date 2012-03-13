<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListMortgagee.aspx.cs"
    Inherits="LendingApplication.PickListMortgagee" %>

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
            window.proxy.init();
        });

        var btnSelectClick = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(data.json, 'onpickmortgagee');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickmortgagees');
            }
            window.proxy.requestClose();
        };

        var onRowSelected = function () {
            btnSelect.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnSelect.disable();
            }
        };

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <%--ViewPort--%>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" AutoExpandColumn="Address">
            <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelectClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="StoreMortgagee" runat="server" OnRefreshData="RefreshData">
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
                            <ext:JsonReader runat="server" IDProperty="PartyId">
                                <Fields>
                                    <ext:RecordField Name="PartyId" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Address" />
                                    <ext:RecordField Name="PartyType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" SingleSelect="true" runat="server">
                        <Listeners>
                            <RowSelect Handler="onRowSelected();" />
                            <RowDeselect Handler="onRowDeselected();" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel>
                    <Columns>
                        <ext:Column runat="server" Header="Party ID" DataIndex="PartyId" Width="150">
                        </ext:Column>
                        <ext:Column runat="server" Header="Name" DataIndex="Name" Width="150">
                        </ext:Column>
                        <ext:Column runat="server" Header="Address" DataIndex="Address" Width="150">
                        </ext:Column>
                        <ext:Column runat="server" Header="Party Type" DataIndex="PartyType" Width="150">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                    DisplayMsg="Displaying mortgagee {0} - {1} of {2}" EmptyMsg="No mortgagee/s to display" />
            </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
