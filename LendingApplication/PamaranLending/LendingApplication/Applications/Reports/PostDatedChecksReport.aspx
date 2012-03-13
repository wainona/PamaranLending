<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostDatedChecksReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.PostDatedChecksReport" %>
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

        var gridPanelReload = function () {
            var startvalue = dtStartDate.getValue();
            var endvalue = dtEndDate.getValue();
            if (startvalue && endvalue) {
                ChequesReportGridPanel.reload();
                btnPrint.enable();
            } else {
                showAlert('Alert', 'Please fill in the start date and end date fields.');
            }
        };

        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
            //PageToolBar.show();
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
            font-size: smaller;
            float: left;
            font-weight:bold;
        }
        .smallFontRight
        {
            font-size: small;
            float: right;
            font-weight:bold;
        }
        .cssClass .x-grid3-row
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-width: 1px;
            border-style: solid;
            font-size: 13px;
        }
        .cssClass .x-grid3-hd
        {
            font-weight: bold;
            border-left-color: Black;
            border-bottom-color: Black;
            border-top-color: Black;
            border-right-color:Black;
            border-width: 1px;
            border-style: solid;
            white-space:  normal !important;
        }
        
        .cssClass .x-grid3-hd-inner, .x-grid3-cell-inner
        {
            white-space:  normal !important;
            font-size: 12px;
            font-family: helvetica,tahoma,verdana,sans-serif;
        }
        
        .cssClass .x-grid3-header
        {
            background-image: none;
            background-color: transparent;
            white-space:  normal !important;
        }
        
        .cssClass .x-grid3-hd-row td : hover
        {
            background-image: none;
            background-color: White;
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
    <ext:Panel runat="server" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Hidden runat="server" ID="hdnDate"></ext:Hidden>
                    <ext:Label runat="server" Text="Month" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:DateField ID="dtStartDate" runat="server" Editable="false" Format="F Y" Width="150">
                        <Plugins>
                            <ext:MonthPicker runat="server" ></ext:MonthPicker>
                        </Plugins>
                        <DirectEvents>
                            <Select OnEvent="onStartDateSelect" Before="#{hdnDate}.setValue(Ext.util.Format.date(#{dtStartDate}.getValue()));"/>
                        </DirectEvents>
                    </ext:DateField>
                    <ext:Label runat="server" Text="End Date" Hidden="true"/>
                    <ext:DateField ID="dtEndDate" runat="server" Editable="false" Hidden="true">
                        <DirectEvents>
                            <Select OnEvent="onEndDateSelect" />
                        </DirectEvents>
                    </ext:DateField>
                    <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="5" />
                    <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="Accept">
                        <Listeners>
                            <Click Handler="gridPanelReload();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill runat="server" />
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
        <table style="width: 1102px; margin: 0 auto; vertical-align:bottom;">
            <tr>
                <td style="vertical-align: bottom;">
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
        <center><b>
        <ext:Label runat="server" Text="Post Dated Checks Report for " />
            <ext:Label ID="lblStartDate" runat="server"/>
            <ext:Label ID="lblEndDate" runat="server"/>
        </b></center>
        <br />
        <br />
        <ext:GridPanel ID="ChequesReportGridPanel" runat="server" AutoHeight="true" Width="1102" BaseCls="cssClass" ColumnLines="true" EnableColumnHide="false" EnableColumnResize="false"
            EnableColumnMove="false">
            <Store>
                <ext:Store ID="ChequesReportStore" runat="server" OnRefreshData="RefreshData">
                    <Reader>
                        <ext:JsonReader>
                            <Fields>
                                <ext:RecordField Name="_CheckDate" />
                                <ext:RecordField Name="Name" />
                                <ext:RecordField Name="CheckCount" />
                                <ext:RecordField Name="Term" />
                                <ext:RecordField Name="Bank" />
                                <ext:RecordField Name="CheckNumber" />
                                <ext:RecordField Name="RemainingBalance" />
                                <ext:RecordField Name="Interest" />
                                <ext:RecordField Name="CheckAmount" />
                                <ext:RecordField Name="CheckStatus" />
                                <ext:RecordField Name="Remarks" />
                                <ext:RecordField Name="LoanAmount"></ext:RecordField>
                            </Fields>
                        </ext:JsonReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel runat="server">
                <Columns>
                    <ext:Column Header="CheckDate" DataIndex="_CheckDate" Css="border-color: Black;" Width="85" MenuDisabled="true" Wrap="true"/>
                    <ext:Column Header="Name" DataIndex="Name" Css="border-color: Black;" Width="150" MenuDisabled="true"  Wrap="true"/>
                    <ext:Column Header="Count" DataIndex="CheckCount" Css="border-color: Black;" Width="50" MenuDisabled="true"  Wrap="true"/>
                    <ext:Column Header="Term" DataIndex="Term" Css="border-color: Black;" Width="45" MenuDisabled="true"  Wrap="true"/>
                    <ext:Column Header="Bank" DataIndex="Bank" Css="border-color: Black;" MenuDisabled="true"  Wrap="true"/>
                    <ext:Column Header="Check Number" DataIndex="CheckNumber" Css="border-color: Black;" MenuDisabled="true"  Wrap="true"/>
                    <ext:NumberColumn Header="Loan Amount" DataIndex="LoanAmount" Format=",000.00" Css="border-color: Black;" Width="100" MenuDisabled="true" Wrap="true" />
                    <ext:NumberColumn Header="Remaining Bal." DataIndex="RemainingBalance" Format=",000.00" Css="border-color: Black;" Width="100" MenuDisabled="true" Wrap="true"/>
                    <ext:NumberColumn Header="Interest" DataIndex="Interest" Css="border-color: Black;" Width="70"  Format=",000.00" MenuDisabled="true" Wrap="true"/>
                    <ext:NumberColumn Header="Check Amount" DataIndex="CheckAmount" Format=",000.00" Css="border-color: Black;" MenuDisabled="true" Wrap="true"/>
                    <ext:Column Header="Check Status" DataIndex="CheckStatus" Css="border-color: Black;" Width="90" MenuDisabled="true" Wrap="true"/>
                    <ext:Column Header="Remarks" DataIndex="Remarks" Css="border-color: Black;" Width="100" MenuDisabled="true" Wrap="true"/>
                </Columns>
            </ColumnModel>
        </ext:GridPanel><%--
        <ext:Label ID="lblTotalNumberOfCheques" runat="server" Cls="smallFont"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
        <ext:Label ID="lblTotalAmount" runat="server" Cls="smallFontRight"/>
                </td>
            </tr>
        </table> 
    </div>
    </form>
</body>
</html>
