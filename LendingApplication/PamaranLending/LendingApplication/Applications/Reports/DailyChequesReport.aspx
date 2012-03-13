<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyChequesReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.DailyChequesReleasedReport" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            window.print();
            //PageToolBar.show();
        }
        var OnGenerateClick = function () {
            X.GenerateReport({
                success: function (result) {
                    btnPrint.enable();
                }
            });
        };

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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <div id="toolBar">
    <ext:Panel ID="MainPanel" runat="server" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Label runat="server" Text="Report Type:"></ext:Label>
                     <ext:ComboBox ID="cmbReportType" runat="server" Width="130" Editable="false">
                        <Items>
                            <ext:ListItem Text="Received" />
                            <ext:ListItem Text="Released" />
                        </Items>
                    </ext:ComboBox>
                    <ext:Label runat="server" Text="Currency: "></ext:Label>
                    <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id"
                                 DisplayField="NameDescription" Width="175" Editable="false" AllowBlank="false">
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
                     <ext:Button ID="btnGenerate" runat="server" Text="Generate" >
                     <Listeners>
                     <Click Handler="OnGenerateClick();" />
                     </Listeners>
                     </ext:Button>
                     <ext:ToolbarFill></ext:ToolbarFill>
                    <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer" Disabled="true">
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
        <center><b><ext:Label ID="lblTitle" runat="server"/>
            <ext:Label ID="lblCurrentDate" runat="server" />
        </b></center>
        <br />
        <br />
        <ext:GridPanel ID="ChequesReportGridPanel" runat="server" AutoHeight="true" Width="750" BaseCls="cssClass" ColumnLines="true">
            <LoadMask Msg="Loading.." ShowMask="true" />
            <Store>
                <ext:Store ID="ChequesReportStore" runat="server">
                    <Reader>
                        <ext:JsonReader>
                            <Fields>
                                <ext:RecordField Name="ProcessedTo" />
                                <ext:RecordField Name="ProcessedBy" />
                                <ext:RecordField Name="CheckNumber" />
                                <ext:RecordField Name="Amount" />
                                <ext:RecordField Name="CurrencySymbol"/>
                                <ext:RecordField Name="TransactionDate"></ext:RecordField>
                            </Fields>
                        </ext:JsonReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column Header="Released To" DataIndex="ProcessedTo" Width="148" Css="border-color: Black;" MenuDisabled="true"/>
                    <ext:Column Header="Released By" DataIndex="ProcessedBy" Width="148" Css="border-color: Black;" MenuDisabled="true"/>
                    <ext:Column Header="Check Number" DataIndex="CheckNumber" Width="148" Css="border-color: Black;" MenuDisabled="true"/>
                    <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="148" Format=",000.00" Css="border-color: Black;" MenuDisabled="true"/>
                    <ext:Column Header="Transaction Date" DataIndex="TransactionDate" Width="148" Css="border-color: Black;" MenuDisabled="true"/>
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <ext:Label ID="lblTotalNumberOfCheques" runat="server" Cls="smallFont"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <ext:Label ID="lblTotalAmount" runat="server" Cls="smallFontRight"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
