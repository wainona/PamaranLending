<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForExReport.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForExTransactionUseCases.ForExReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Foreign Exchange Report</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link rel="stylesheet" type="text/css" href="../../Resources/css/main.css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
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
    </script>
    <style type="text/css">
        .bold 
        {
            font-weight: bold;
            font-size: large;
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
            @page
            {
                size: landscape;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" />
        <div id="toolBar">
        <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Padding="20" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Label ID="lblDateGen" runat="server" Text="From:"></ext:Label>
                        <%-- --%><ext:ToolbarSpacer runat="server" />
                        <ext:DateField runat="server" ID="dtStartDate" Width="120" Editable="false" AllowBlank="false">
                            <DirectEvents>
                                <Select OnEvent="onStartDateSelect" />
                            </DirectEvents>
                        </ext:DateField>
                        <%-- --%><ext:ToolbarSpacer runat="server" />
                        <ext:Label ID="Label1" runat="server" Text="To:"></ext:Label>
                        <%-- --%><ext:ToolbarSpacer runat="server" />
                        <ext:DateField runat="server" ID="dtEndDate" Width="120" Editable="false" AllowBlank="false">
                            <DirectEvents>
                                <Select OnEvent="onEndDateSelect" />
                            </DirectEvents>
                        </ext:DateField>
                        <%-- --%><ext:ToolbarSpacer runat="server" />
                        <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="FolderGo">
                            <DirectEvents>
                                <Click OnEvent="btnGenerate_Click" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer" Disabled="true">
                            <Listeners>
                                <Click Handler="printContent('PrintableContent');" />
                            </Listeners>
                        </ext:Button>
                        <%-- --%><ext:ToolbarSeparator runat="server" />
                        <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Panel>
        </div>
        <div id="PrintableContent" style="font-family: Tahoma; font-size: small;">
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
        <center><b><ext:Label ID="lblTitle" runat="server" Text="Foreign Exchange Transaction Report for "/>
            <ext:Label ID="lblDateRange" runat="server" />
        </b></center>
        <br />
        <br />
        <ext:GridPanel ID="GridPanelForExReport" runat="server" AutoHeight="true" Width="900" BaseCls="cssClass" ColumnLines="true">
                <Store>
                    <ext:Store ID="StoreForExReport" runat="server">
                        <Reader>
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="CustomerName" />
                                    <ext:RecordField Name="_TransactionDate" />
                                    <ext:RecordField Name="OriginalAmount" />
                                    <ext:RecordField Name="OriginalCurrency" />
                                    <ext:RecordField Name="ConvertedAmount" />
                                    <ext:RecordField Name="ConvertedCurrency" />
                                    <ext:RecordField Name="Rate" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel ID="ColumnModelForExReport" runat="server">
                    <Columns>
                        <ext:Column Header="Name" DataIndex="CustomerName" Width="270" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                        <ext:Column Header="Date" DataIndex="_TransactionDate" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
                        <ext:NumberColumn Header="Original Amount" DataIndex="OriginalAmount" Format=",000.00" Width="140" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
                        <ext:Column Header="Currency" DataIndex="OriginalCurrency" Width="70" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
                        <ext:NumberColumn Header="Converted Amount" DataIndex="ConvertedAmount" Format=",000.00" Width="140" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
                        <ext:Column Header="Currency" DataIndex="ConvertedCurrency" Width="70" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
                        <ext:NumberColumn Header="Rate" DataIndex="Rate" Format=",000.00" Css="border-color: Black;" MenuDisabled="true"  Sortable="false"/>
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
