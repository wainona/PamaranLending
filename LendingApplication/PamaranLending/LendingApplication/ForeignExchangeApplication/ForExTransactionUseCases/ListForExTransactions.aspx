<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListForExTransactions.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.ListForExTransactions" %>
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
            window.proxy.init(['addforextransaction', 'editforextransaction']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };

        var onRowSelected = function () {
            btnEditForExTransaction.enable();
        };

        var onRowDeselected = function () {
        };

        var btnEditForExTransaction_Click = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/ForeignExchangeApplication/ForExTransactionUseCases/OpenForExTransaction.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('OpenForeignExchangeTransaction', param, 'Open ForEx Transaction');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var addBuying = function () {
            window.proxy.requestNewTab('AddForExTransactionBuying', '/ForeignExchangeApplication/ForExTransactionUseCases/AddForExBuying.aspx', 'Add ForEx (Buying)');
        };

        var addSelling = function () {
            window.proxy.requestNewTab('AddForExTransactionSelling', '/ForeignExchangeApplication/ForExTransactionUseCases/AddForExSelling.aspx', 'Add ForEx (Selling)');
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" Border="false" EnableColumnHide="false" EnableColumnMove="false" >
                <LoadMask Msg="Loading.." ShowMask="true" />
                <View>
                    <ext:GridView EmptyText="No foreign exchange transactions to display" />
                </View>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Hidden ID="hdnRecordId" runat="server" />
                            <ext:Button ID="btnDeleteForExTransaction" runat="server" Text="Delete" Icon="CoinsDelete" Disabled="true" Hidden="true">
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" Hidden="true"/>
                            <ext:Button ID="btnEditForExTransaction" runat="server" Text="Open" Icon="Coins" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnEditForExTransaction_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnAddForExTransaction" runat="server" Text="Add" Icon="CoinsAdd">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                            <ext:MenuItem Text="Buying" Handler="addBuying">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Selling" Handler="addSelling">
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                                <%--<Listeners>
                                    <Click Handler="window.proxy.requestNewTab('AddForeignExchangeTransaction', '/ForeignExchangeApplication/ForExTransactionUseCases/AddForExTransaction.aspx', 'Add Foreign Exchange Transaction');" />
                                </Listeners>--%>
                            </ext:Button>
                            <ext:ToolbarFill runat="server" />
                            <ext:Label runat="server" Text="Search Customer:" />
                            <ext:ToolbarSpacer runat="server" />
                            <ext:TextField ID="txtSearchString" runat="server" EmptyText="Type here.." />
                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" />
                            <ext:Button ID="btnSearch" Text="Search" runat="server" Icon="Magnifier">
                                <Listeners>
                                    <Click Handler="PageGridPanel.reload();" />
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
                                    <ext:RecordField Name="ProcessedBy" />
                                    <ext:RecordField Name="CustomerName" />
                                    <ext:RecordField Name="_Date" />
                                    <ext:RecordField Name="AmountReceived" />
                                    <ext:RecordField Name="ReceivedCurrency" />
                                    <ext:RecordField Name="AmountReleased" />
                                    <ext:RecordField Name="ReleasedCurrency" />
                                    <ext:RecordField Name="Currency" />
                                    <ext:RecordField Name="Rate" />
                                    <ext:RecordField Name="ForExType"></ext:RecordField>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column Header="Date" DataIndex="_Date" Width="100" />
                        <ext:Column Header="Customer" DataIndex="CustomerName" Width="250" />
                        <ext:NumberColumn Header="Amount Received" DataIndex="AmountReceived" Width="120" Format=",000.00"/>
                        <ext:Column Header="" DataIndex="ReceivedCurrency" Width="70"/>
                        <ext:NumberColumn Header="Amount Released" DataIndex="AmountReleased" Width="120" Format=",000.00"/>
                        <ext:Column Header="" DataIndex="ReleasedCurrency" Width="70"/>
                        <ext:NumberColumn Header="Rate" DataIndex="Rate" Width="70" Format=",000.00" />
                        <ext:Column Header="Processed By" DataIndex="ProcessedBy" Width="170"/>
                        <ext:Column Header="ForEx Type" DataIndex="ForExType" Width="100" />
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
