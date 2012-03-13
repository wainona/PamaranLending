<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RediscountingView.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.RediscountingView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title> View Rediscounting</title>
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
        var printEncashment = function () {
            var url = '/Applications/DisbursementUseCases/PrintRediscounting.aspx';
            var id = 'id=' + hiddenDisbursementID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintRediscounting', param, 'Print Rediscounting');
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
    <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="450" MonitorValid="true" AutoScroll="true">
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
                    <Click Handler="printEncashment();" />
                    </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
          <Items>
              <ext:Panel ID="pnlEncashmentView" runat="server" Layout="FormLayout" LabelWidth="180" Padding="5"  AutoHeight="true" Title="Disbursement Details">
                        <Items>
             <ext:Panel ID="Panel2" runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false" Height="250" Width="500">
                <Items>
                <ext:Hidden ID="hiddenDisbursementID" runat="server"></ext:Hidden>
                    <ext:TextField ID="txtBank"  FieldLabel="Bank" ReadOnly="true" runat="server" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField ID="txtCheckNumber" FieldLabel="Check Number" Hidden="true" runat="server" AnchorHorizontal="95%"></ext:TextField>
                    <ext:DateField  ID="txtChkDate" FieldLabel="Check Date" runat="server"  Format="m/dd/yyyy"   ReadOnly="true"  AnchorHorizontal="95%">
                    </ext:DateField>
                    <ext:CompositeField ID="CompositeField1" AnchorHorizontal="95%" runat="server">
                        <Items>
                            <ext:TextField ID="txtChkAmount" FieldLabel="Check Amount" runat="server" ReadOnly="true"
                                Flex="3">
                            </ext:TextField>
                            <ext:TextField ID="txtCurrency1" runat="server" ReadOnly="true" Flex="1">
                            </ext:TextField>
                        </Items>
                    </ext:CompositeField>
                    <ext:DateField ID="txtDateDisbursed" FieldLabel="Date Disbursed"  Format="m/dd/yyyy"  runat="server" ReadOnly="true"  AnchorHorizontal="95%"></ext:DateField>
                    <ext:TextField ID="txtDisbursedTo" FieldLabel="Disbursed To" runat="server" ReadOnly="true"  AnchorHorizontal="95%"></ext:TextField>
                     <ext:CompositeField ID="CompositeField3" AnchorHorizontal="95%" runat="server">
                                        <Items>
                                         <ext:TextField ID="txtSuchargeFee" FieldLabel="Surcharge Fee" runat="server" ReadOnly="true" Flex="3"></ext:TextField>
                                             <ext:TextField ID="txtCurrency3" runat="server" ReadOnly="true" Flex="1"></ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                     <ext:CompositeField ID="CompositeField2" AnchorHorizontal="95%" runat="server">
                                        <Items>
                                         <ext:TextField ID="txtAmountDisbursed" FieldLabel="Amount Disbursed" runat="server" ReadOnly="true" Flex="3"></ext:TextField>
                                             <ext:TextField ID="txtCurrency2" runat="server" ReadOnly="true" Flex="1"></ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                    <ext:TextField ID="txtDisbursedBy" FieldLabel="Disbursed By"  ReadOnly="true" runat="server" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField ID="txtReceivedBy" FieldLabel="Received By"  ReadOnly="true" runat="server" AnchorHorizontal="95%"></ext:TextField>
                </Items>
            </ext:Panel>
            <ext:Panel ID="Panel1" runat="server" Title="Amount Disbursed Breakdown" Width="700">
                    <Items>
                       <ext:GridPanel ID="GridPanelBreakDown" runat="server" AutoHeight="true" Width="700"
                        EnableColumnHide="false" ColumnLines="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
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
            </ext:Panel>
        </Items>
    </ext:FormPanel>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>

