<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeachersChequesReceivedReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.TeachersChequesReceivedReport" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link rel="stylesheet" type="text/css" href="../../Resources/css/main.css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            //PageToolBar.hide();
           // PageGridPanelPagingToolBar.hide();
            window.print();
            //PageToolBar.show();
            //PageGridPanelPagingToolBar.show();
        }
    </script>
    <style type="text/css">
        .bold 
        {
            font-size: medium;
            font-weight: bold;
        }
        .smallFont 
        {
            font-size: small;
            float: left;
            font-weight:bold;
        }
        .smallFontRight
        {
            font-size: small;
            float: right;
            font-weight:bold;
        }
        .cssClass .x-toolbar
        {
            border: 1px solid Black;
            border-top: none;
            background-color: transparent;
            background-image: none;
        }
        @media screen
        {
            #toolBar
            {
                display: block;
            }
        }
        
        @media print
        {
            #toolBar
            {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"  IDMode="Explicit"/>
        <div id="toolBar">
        <ext:Panel ID="MainPanel" runat="server" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer">
                            <Listeners>
                                <Click Handler="printContent('PrintableDiv');" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Panel>
        </div>
        <div id="PrintableDiv" style="font-family: Tahoma; font-size: small;">
            <table style="width: 750px; margin: 0 auto;">
                <tr style="vertical-align:bottom;">
                    <td style="vertical-align:bottom;">
                        <br />
                        <center>
                            <ext:Label ID="lblLenderName" runat="server" Cls="bold"/><br />
                            <ext:Label ID="lblLenderAddress" runat="server" /><br />
                            <ext:Label ID="lblLenderTelephoneNumber" runat="server" />&nbsp;
                            <ext:Label ID="lblLenderFaxNumber" runat="server" /><br />
                        </center>
                        <br />
                        <br />
                        <br />
                        <center>
                            <b>
                                <ext:Label runat="server" Text="Teacher's Check(s) Received for" />
                                <ext:Label ID="lblCurrentDate" runat="server" />
                            </b>
                        </center>
                        <br />
                        <br />
                        <ext:GridPanel ID="ChequesReportGridPanel" runat="server" AutoHeight="true" Width="750" BaseCls="cssClass" ColumnLines="true">
                            <LoadMask Msg="Loading.." ShowMask="true" />
                            <Store>
                                <ext:Store ID="ChequesReportStore" runat="server" OnRefreshData="RefreshData">
                                    <Proxy>
                                        <ext:PageProxy>
                                        </ext:PageProxy>
                                    </Proxy>
                                    <Listeners>
                                        <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                    </Listeners>
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                        <ext:Parameter Name="limit" Value="50" Mode="Raw" />
                                    </AutoLoadParams>
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="ReceivedFrom" />
                                                <ext:RecordField Name="CheckNumber" />
                                                <ext:RecordField Name="Amount" />
                                                <ext:RecordField Name="TransactionDate"></ext:RecordField>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column Header="Received From" DataIndex="ReceivedFrom" Width="215" Css="border-color: Black;" MenuDisabled="true"/>
                                    <ext:Column Header="Check Number" DataIndex="CheckNumber" Width="214" Css="border-color: Black;" MenuDisabled="true"/>
                                    <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="220" Format=",000.00" Css="border-color: Black;" MenuDisabled="true"/>
                                     <ext:Column Header="Transaction Date" DataIndex="TransactionDate" Width="100" Css="border-color: Black;" MenuDisabled="true"/>
                                </Columns>
                            </ColumnModel>  
                            <BottomBar>
                                <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="50" DisplayInfo="true"
                                    DisplayMsg="Displaying cheques {0} - {1} of {2}" EmptyMsg="No cheques to display" Width="740"/>
                            </BottomBar>
                        </ext:GridPanel>
                        <ext:Label ID="lblTotalNumberOfCheques" runat="server" Cls="smallFont"/>
                        <ext:Label ID="lblTotalAmount" runat="server" Cls="smallFontRight"/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
