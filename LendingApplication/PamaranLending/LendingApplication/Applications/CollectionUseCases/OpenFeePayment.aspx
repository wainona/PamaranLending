<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenFeePayment.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.OpenFeePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Encashment View</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        var printFee = function () {
            var url = '/Applications/CollectionUseCases/PrintFeePayment.aspx';
            var id = 'id=' + hiddenPaymentId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintFee', param, 'Print Fee Payment');
        };
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
    <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="550" MonitorValid="true">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator>
                    </ext:ToolbarSeparator>
                    <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="PrinterGo">
                    <Listeners>
                    <Click Handler="printFee();" />
                    </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
          <ext:TabPanel ID="TabPanel" runat="server" Padding="0" HideBorders="true"
                        EnableTabScroll="true" DeferredRender="false">
          <Items>
        <ext:Panel ID="pnlEncashmentView" runat="server" Layout="FormLayout" LabelWidth="180"
            Padding="5" AutoHeight="true" Title="Fee Payment Details">
            <Items>
                <ext:Panel ID="Panel2" runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false"
                    Height="100" Width="500">
                    <Items>
                        <ext:Hidden ID="hiddenPaymentId" runat="server"></ext:Hidden>
                        <ext:TextField ID="txtReceivedFrom" FieldLabel="Received From" runat="server" ReadOnly="true"
                            AnchorHorizontal="95%">
                        </ext:TextField>
                        <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" runat="server" ReadOnly="true"
                            AnchorHorizontal="95%"  Format="m/dd/yyyy">
                        </ext:DateField>
                        <ext:TextField ID="txtReceivedBy" FieldLabel="Received By" runat="server" ReadOnly="true"
                        AnchorHorizontal="95%"></ext:TextField>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="pnlFeeItems" runat="server" ColumnWidth=".4" Layout="FormLayout"
            Border="false" Title="Fee Items" Height="300">
            <Items>
                <ext:GridPanel ID="grdFeeItems" runat="server" Height="250">
                    <View>
                    <ext:GridView EmptyText="No fee items to display"></ext:GridView>
                </View>
                <LoadMask ShowMask="true" />
                    <Store>
                        <ext:Store ID="FeeItems" runat="server">
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
                                <ext:JsonReader IDProperty="Id">
                                    <Fields>
                                        
                                        <ext:RecordField Name="Particular" />
                                        <ext:RecordField Name="FeeAmount" />
                                        <ext:RecordField Name="Id"></ext:RecordField>
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                        <Columns>
                            <ext:Column Header="Particular" DataIndex="Particular" Locked="true" Wrap="true" Width="140px" Hidden="false" />
                            <ext:NumberColumn Header="Amount" DataIndex="FeeAmount" Width="140px" Locked="true" Wrap="true" Format=",000.00">
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                            DisplayMsg="Displaying fee items {0} - {1} of {2}" EmptyMsg="No fee items to display" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
            <BottomBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:ToolbarFill>
                        </ext:ToolbarFill>
                        <ext:Label ID="Label1" Text="Total Paid Fees:" runat="server"> </ext:Label>
                        <ext:NumberField runat="server" ID="txtTotalFeeAmount" ReadOnly="true">
                        </ext:NumberField>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>

            </Items>
        </ext:Panel>
                 <ext:Panel ID="Panel1" runat="server" Title="Payment Breakdown" Width="700">
                    <Items>
                       <ext:GridPanel ID="GridPanelBreakDown" runat="server" AutoHeight="true" Width="700"
                        EnableColumnHide="false" ColumnLines="true">
                        <Store>
                            <ext:Store ID="strBreakdown" runat="server" OnRefreshData="RefreshItems">
                                <Reader>
                                    <ext:JsonReader IDProperty="PaymentId">
                                        <Fields>
                                            <ext:RecordField Name="PaymentMethod" />
                                            <ext:RecordField Name="TotalAmount" />
                                            <ext:RecordField Name="BankName" />
                                            <ext:RecordField Name="CheckNumber" />
                                            <ext:RecordField Name="PaymentId"/>
                                            <ext:RecordField Name="CurrencySymbol"></ext:RecordField>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="GridPanelPaymentColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="Payment Method" Locked="true" Hideable="false" DataIndex="PaymentMethod"
                                    Align="Center" Fixed="true" Resizable="false" Width="120" />
                                     <ext:NumberColumn Header="Amount" Locked="true" Hideable="false" DataIndex="TotalAmount"
                                    Align="Center" Width="120" Format=",000.00" />
                                      <ext:Column Header="Currency Symbol" Locked="true" Hideable="false" DataIndex="CurrencySymbol"
                                    Align="Center" Width="120" />
                                <ext:Column Header="Bank Name" Locked="true" Hideable="false" DataIndex="BankName"
                                    Align="Center" Width="120" />
                                <ext:Column Header="Check Number" Locked="true" Hideable="false" DataIndex="CheckNumber"
                                    Align="Center" Width="120" />
                              
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    </Items>
                    </ext:Panel>
          </Items>
          </ext:TabPanel>
        </Items>
    </ext:FormPanel>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>

