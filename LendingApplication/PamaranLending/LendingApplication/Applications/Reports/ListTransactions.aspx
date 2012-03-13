<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListTransactions.aspx.cs" Inherits="LendingApplication.Applications.Reports.ListTransactions" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Restructure List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addencashment', 'addrediscounting', 'addloandisbursement', 'addotherdisbursement', 'addchange', 'addcollection', 'addfeepayment']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
            PageGridPanelCash.reload();
        };

        var onPickCustomer = function () {
            window.proxy.requestNewTab('CustomerPickList', '/Applications/LoanRestructureUseCases/CustomerWithLoanPickList.aspx?mode=single', 'Customer Pick List');
        };

        var onRowSelected = function () {
            if (PageGridPanel.hasSelection() == true) {
                var selectedRows = PageGridPanelSelectionModel.getSelections();
            }
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnAdditionalLoan.disable();
            }
        };


        var onRowSelectedCash = function () {
            if (PageGridPanelCash.hasSelection() == true) {
                var selectedRows = PageGridPanelSelectionModelCash.getSelections();
            }
        };
        var onRowDeselectedCash = function () {
            if (PageGridPanelCash.hasSelection() == false) {
            }
        };

        var OnGenerateClick = function () {
            X.Fill();
        };
    </script>
    <style type="text/css">
        .white-bg .x-toolbar-ct .x-toolbar-left
        {
            background-color: White;
        }
        
        .button-bg .x-toolbar-ct .x-toolbar-left .x-toolbar-cell
        {
            background-color: Gray;
        }
        
        .boldFormLabel .x-form-item-label
        {
            font-weight: bold;
        }
        
        .boldText .x-btn-text
        {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGuid" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel runat="server" Border="false" Layout="ColumnLayout">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Label ID="Label2" runat="server" Text="Currency: "></ext:Label>
                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                            <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id" DisplayField="NameDescription" Width="190" Editable="false" AllowBlank="false">
                                <Store>
                                    <ext:Store ID="strCurrency" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id">
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="NameDescription" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" />
                            <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="Accept">
                                 <Listeners>
                                    <Click Handler="OnGenerateClick();" />
                                 </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:GridPanel 
                        ID="PageGridPanelCash" 
                        runat="server"
                        Layout="Fit"
                        Height="500" 
                        ButtonAlign="Center" 
                        Title="Cash Transactions"
                        ColumnWidth="0.5">
                        <View>
                            <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No transactions to display..." runat="server" />
                        </View>
                        <TopBar>
                            <ext:Toolbar ID="PageGridPanelToolbarCash" runat="server">
                                <Items>
                                    <ext:ToolbarSpacer />
                                    <ext:ToolbarSpacer />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store runat="server" ID="PageGridPanelStoreCash" RemoteSort="false" OnRefreshData="RefreshDataCash" >
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
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="TransactionType" />
                                            <ext:RecordField Name="Received" />
                                            <ext:RecordField Name="Released" />
                                            <ext:RecordField Name="EntryDate" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="PageGridPanelSelectionModelCash" runat="server" SingleSelect="false" >
                                <Listeners>
                                    <RowSelect Fn="onRowSelectedCash" />
                                    <RowDeselect Fn="onRowDeselectedCash" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ColumnModel runat="server" ID="PageGridPanelColumnModelCash" Width="100%">
                            <Columns>
                                <ext:Column Header="Transaction Type" DataIndex="TransactionType" Align="Center" Wrap="true" Locked="true" Width="150px" />
                                <ext:Column Header="Received" DataIndex="Received" Align="Center" Locked="true" Wrap="true"
                                    Width="150px"/>
                                <ext:Column Header="Released" DataIndex="Released" Locked="true" Wrap="true"
                                    Width="150px" Align="Center" />
                                <ext:Column Header="Entry Date" DataIndex="EntryDate" Align="Center" Locked="true" Wrap="true" Width="120px" />
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PageGridPanelPagingToolBarCash" runat="server" PageSize="20" DisplayInfo="true"
                                DisplayMsg="Displaying transactions {0} - {1} of {2}" EmptyMsg="No transactions to display" />
                        </BottomBar>
                        <LoadMask ShowMask="true"/>
                        <LoadMask Msg="Loading..." ShowMask="true" />
                    </ext:GridPanel>
                    <ext:GridPanel 
                        ID="PageGridPanel" 
                        runat="server"
                        Layout="Fit"
                        Height="500" 
                        ButtonAlign="Center" 
                        Title="Cheque Transactions" 
                        ColumnWidth="0.5">
                        <View>
                            <ext:GridView ID="GridView2" ForceFit="true" AutoFill="true" EmptyText="No transactions to display..." runat="server" />
                        </View>
                        <TopBar>
                            <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                                <Items>
                                    <ext:ToolbarSpacer />
                                    <ext:ToolbarSpacer />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false" OnRefreshData="RefreshData" >
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
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="TransactionType" />
                                            <ext:RecordField Name="Received" />
                                            <ext:RecordField Name="Released" />
                                            <ext:RecordField Name="CheckNumber" />
                                            <ext:RecordField Name="EntryDate" />
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
                                <ext:Column Header="Transaction Type" DataIndex="TransactionType" Align="Center" Wrap="true" Locked="true" Width="150px" />
                                <ext:Column Header="Received" DataIndex="Received" Align="Center" Locked="true" Wrap="true"
                                    Width="150px" />
                                <ext:Column Header="Released" DataIndex="Released" Locked="true" Wrap="true"
                                    Width="150px" Align="Center" />
                                <ext:Column Header="Check Number" DataIndex="CheckNumber" Locked="true" Wrap="true"
                                    Width="150px" Align="Center" />
                                <ext:Column Header="Entry Date" DataIndex="EntryDate" Align="Center" Locked="true" Wrap="true" Width="100px" />
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                DisplayMsg="Displaying transactions {0} - {1} of {2}" EmptyMsg="No transactions to display" />
                        </BottomBar>
                        <LoadMask ShowMask="true"/>
                        <LoadMask Msg="Loading..." ShowMask="true" />
                    </ext:GridPanel>
                </Items>
                <BottomBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:TextField runat="server" ID="txtCashNetTotal" FieldLabel="Cash Net Total" ReadOnly="true" />
                            <ext:ToolbarFill runat="server" />
                            <ext:TextField runat="server" ID="txtChequeNetTotal" FieldLabel="Cheque Net Total" ReadOnly="true" />
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
