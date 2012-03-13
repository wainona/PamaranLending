<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherDisbursementView.aspx.cs"
    Inherits="LendingApplication.Applications.DisbursementUseCases.OtherDisbursementView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Other Disbursements</title>
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
        var printOtherDisbursement = function () {
            var url = '/Applications/DisbursementUseCases/PrintOtherDisbursement.aspx';
            var id = 'id=' + hiddenDisbursementID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintOtherDisbursement', param, 'Print Other Disbursement');
        };
        var onRowSelect = function () {
            var selectedRow = grdPanelRowSelectionModel.getSelected();
            if (selectedRow.json.PaymentMethod == 'Cash') {
                btnPrintCheque.disable();
            } else {
                btnPrintCheque.enable();
            }
        }
        var onRowSelect = function () {
            var selectedRow = grdPanelRowSelectionModel.getSelected();
            if (selectedRow.json.PaymentMethod == 'Cash') {
                btnPrintCheque.disable();
            } else {
                btnPrintCheque.enable();
            }
        }
        var onBtnPrintCheque = function () {
            var selectedRow = grdPanelRowSelectionModel.getSelected();
            var url = '/Applications/ChequeUseCases/PrintCheque.aspx';
            var id = 'id=' + hiddenDisbursementID.getValue();
            var num = '&cn=' + selectedRow.json.CheckNumber;
            var param = url + "?" + id + num;
            window.proxy.requestNewTab('PrintCheque', param, 'Print Cheque');
        }
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
                    <ext:Button ID="btnPrintRelease" runat="server" Text="Print" Icon="PrinterGo">
                    <Listeners>
                    <Click Handler="printOtherDisbursement();" />
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
            Padding="5" AutoHeight="true" Title="Disbursement Details">
            <Items>
                <ext:Panel ID="Panel2" runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false"
                    Height="150" Width="500">
                    <Items>
                        <ext:Hidden ID="hiddenDisbursementID" runat="server"></ext:Hidden>
                        <ext:DateField ID="txtDateDisbursed" FieldLabel="Date Disbursed" runat="server" ReadOnly="true"
                            AnchorHorizontal="95%"  Format="m/dd/yyyy">
                        </ext:DateField>
                        <ext:TextField ID="txtDisbursedTo" FieldLabel="Disbursed To" runat="server" ReadOnly="true"
                            AnchorHorizontal="95%">
                        </ext:TextField>
                        <ext:TextField ID="txtDisbursedby" FieldLabel="Disbursed By" ReadOnly="true" runat="server"
                            AnchorHorizontal="95%">
                        </ext:TextField>
                        <ext:TextField ID="txtTotalAmountDisbursed" FieldLabel="Amount Disbursed" ReadOnly="true" runat="server"
                         AnchorHorizontal="95%"></ext:TextField>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="pnlDisbursementItems" runat="server" ColumnWidth=".4" Layout="FormLayout"
            Border="false" Title="Disbursement Items" Height="300">
            <Items>
                <ext:GridPanel ID="grdPnlOtherDisbursementItems" runat="server" Height="250">
                    <View>
                    <ext:GridView EmptyText="No disbursements to display"></ext:GridView>
                </View>
                <LoadMask ShowMask="true" />
                    <Store>
                        <ext:Store ID="OtherDisbursementItems" runat="server" OnRefreshData="RefreshItems">
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
                                        <ext:RecordField Name="PerItemAmount" />
                                        <ext:RecordField Name="Id"></ext:RecordField>
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                    </Store>
                    <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                        <Columns>
                            <ext:Column Header="Particular" DataIndex="Particular" Locked="true" Wrap="true" Width="250" Hidden="false" />
                            <ext:NumberColumn Header="Amount" DataIndex="PerItemAmount" Width="140px" Locked="true" Wrap="true" Format=",000.00">
                            </ext:NumberColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                            DisplayMsg="Displaying disbursement items {0} - {1} of {2}" EmptyMsg="No disbursement items to display" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
            <BottomBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:ToolbarFill>
                        </ext:ToolbarFill>
                        <ext:Hidden runat="server" ID="PaymentId"></ext:Hidden>
                        <ext:Label ID="Label1" Text="Total Amount Disbursed:" runat="server"> </ext:Label>
                        <ext:NumberField runat="server" ID="txtTotalAmount" ReadOnly="true">
                        </ext:NumberField>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Panel>

            </Items>
        </ext:Panel>
                 <ext:Panel ID="Panel1" runat="server" Title="Amount Disbursed Breakdown" Width="700">
                    <Items>
                       <ext:GridPanel ID="GridPanelBreakDown" runat="server" AutoHeight="true" Width="700"
                        EnableColumnHide="false" ColumnLines="true">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:ToolbarFill />
                                    <ext:Button ID="btnPrintCheque" runat="server" Text="Print Check" Icon="Printer" Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnPrintCheque();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="strBreakdown" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="PaymentMethod" />
                                            <ext:RecordField Name="BankName" />
                                            <ext:RecordField Name="BankBranch" />
                                            <ext:RecordField Name="CheckNumber" />
                                            <ext:RecordField Name="TotalAmount" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="GridPanelPaymentColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="Payment Method" Locked="true" Hideable="false" DataIndex="PaymentMethod"
                                    Align="Center" Fixed="true" Resizable="false" Width="150" />
                                <ext:Column Header="Bank Name" Locked="true" Hideable="false" DataIndex="BankName"
                                    Align="Center" Width="150" />
                                <ext:Column Header="Branch" Locked="true" Hideable="false" DataIndex="BankBranch"
                                    Align="Center" Width="150" />
                                <ext:Column Header="Check Number" Locked="true" Hideable="false" DataIndex="CheckNumber"
                                    Align="Center" Width="100" />
                                <ext:NumberColumn Header="Amount" Locked="true" Hideable="false" DataIndex="TotalAmount"
                                    Align="Center" Width="145" Format=",000.00" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="grdPanelRowSelectionModel" SingleSelect="true">
                                <Listeners>
                                    <RowSelect Handler="onRowSelect();"/>
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
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
