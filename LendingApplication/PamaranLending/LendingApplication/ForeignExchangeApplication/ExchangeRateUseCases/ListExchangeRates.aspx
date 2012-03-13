<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListExchangeRates.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ExchangeRateUseCases.ListExchangeRates" %>
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
            btnEdit.enable();
            btnDelete.enable();
        };

        var onRowDeselected = function () {
        };

        var btnEditForExTransaction_Click = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/ForeignExchangeApplication/ExchangeRateUseCases/OpenExchangeRate.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('OpenExchangeRate', param, 'Open Exchange Rate');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var deleteSuccess = function () {
            showAlert('Delete Success', 'You have successfully deleted the exchange rate.', function () {
                PageGridPanel.reload();
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
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
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="CoinsDelete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="deleteSuccess">
                                        <Confirmation Title="Confirm Delete" ConfirmRequest="true" Message="Are you sure you want to delete the selected exchange rate?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" Hidden="true"/>
                            <ext:Button ID="btnEdit" runat="server" Text="Open" Icon="Coins" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnEditForExTransaction_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="CoinsAdd">
                                <Listeners>
                                    <Click Handler="window.proxy.requestNewTab('AddExchangeRate', '/ForeignExchangeApplication/ExchangeRateUseCases/AddExchangeRate.aspx', 'Add Exchange Rate');" />
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
                                    <ext:RecordField Name="_Date" />
                                    <ext:RecordField Name="ExchangeRateType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column Header="Currency From" DataIndex="CurrencyFrom" Width="200" />
                        <ext:Column Header="Currency To" DataIndex="CurrencyTo" Width="200" />
                        <ext:NumberColumn Header="Rate" DataIndex="Rate" Width="150" Format=",000.0000" />
                        <ext:Column Header="Date" DataIndex="_Date" Width="150" />
                        <ext:Column Header="Type" DataIndex="ExchangeRateType" Width="200" />
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
