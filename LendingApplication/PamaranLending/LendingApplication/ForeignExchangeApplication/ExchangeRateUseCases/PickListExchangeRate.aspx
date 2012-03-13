<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListExchangeRate.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ExchangeRateUseCases.PickListExchangeRate" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            window.proxy.init(['addexchangerate', 'editexchangerate']);
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

        var btnSelect_Click = function () {
            var data = PageGridPanelSelectionModel.getSelected();
            window.proxy.sendToParent(data.json, 'onpickexchangerate');
            window.proxy.requestClose();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hdnExRateType" runat="server" Text="None" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" Border="false">
                <LoadMask Msg="Loading.." ShowMask="true" />
                <View>
                    <ext:GridView EmptyText="No foreign exchange transactions to display" />
                </View>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Hidden ID="hdnRecordId" runat="server" />
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Accept" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelect_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="PageGridPanelStore" runat="server" OnRefreshData="RefreshData" RemoteSort="false">
                        <Proxy>
                            <ext:PageProxy>
                            </ext:PageProxy>
                        </Proxy>
                        <AutoLoadParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Reader>
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="Id" />
                                    <ext:RecordField Name="CurrencyFrom" />
                                    <ext:RecordField Name="CurrencyTo" />
                                    <ext:RecordField Name="Rate" />
                                    <ext:RecordField Name="Date" />
                                    <ext:RecordField Name="Type" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column Header="Currency From" DataIndex="CurrencyFrom" Width="200" />
                        <ext:Column Header="Currency To" DataIndex="CurrencyTo" Width="200" />
                        <ext:NumberColumn Header="Rate" DataIndex="Rate" Width="150" Format=",000.00" />
                        <ext:DateColumn Header="Date" DataIndex="Date" Width="150" Format="MMM dd, Y" />
                        <ext:Column Header="Type" DataIndex="Type" Width="200" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying foreign exchange transactions {0} - {1} of {2}" EmptyMsg="No foreign exchange transactions to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
