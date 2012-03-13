<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomersWithNewBill.aspx.cs" Inherits="LendingApplication.Applications.BackgroundUseCases.CustomersWithNewBill" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customers List</title>
     <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['generatebillsuccessful']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            window.proxy.requestClose('Billing');
        };

        var onRowSelected = function () {
            btnPrint.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnPrint.disable();
            }
        };
        var openBillStatement = function () {
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow) {
                var id = 'id='+PageGridPanelSelectionModel.getSelected().id;
                var param ='/Applications/BackgroundUseCases/BillStatement.aspx';
                var url = param +'?'+id;
                window.proxy.requestNewTab('PrintBillStatement', url, 'Print Bill Statement');
            }
        };

    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />

    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
          
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                AutoExpandColumn="Addresses" Layout="FitLayout">
                 <View>
                <ext:GridView EmptyText="No customers to display"></ext:GridView>
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                          <ext:Hidden ID="hiddenCustID" runat="server"></ext:Hidden>
                          <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
                            <ext:Button ID="btnPrint" runat="server" Text="Print Bill Statement" Icon="PrinterGo" Disabled="true">
                            <Listeners>
                            <Click Handler="openBillStatement();" />
                            </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                             <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="15"></ext:ToolbarSpacer>
                            <ext:Label ID="Label2" runat="server" Text="Search by Name:"/>
                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server"></ext:ToolbarSpacer>
                            <ext:TextField ID="txtSearch" runat="server" />
                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10"></ext:ToolbarSpacer>
                            <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click" />
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
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="CustomerID">
                                <Fields>
                                    <ext:RecordField Name="CustomerID" />
                                    <ext:RecordField Name="Names" />
                                    <ext:RecordField Name="Addresses" />
                                    <ext:RecordField Name="PartyTypes" />
                                    <ext:RecordField Name="Status" />
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
                <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                    <Columns>
                        <ext:Column Header="Customer ID" DataIndex="CustomerID" Wrap="true" Locked="true">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Names" Wrap="true" Locked="true" Width="150">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Addresses" Locked="true" Wrap="true">
                        </ext:Column>
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
