<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryOfPaidOffLoans.aspx.cs" Inherits="LendingApplication.Applications.Reports.SummaryOfPaidOffLoans" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
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
                PaidOffLoansGridPanel.reload();
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
            font-size: smaller;
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
        }
    </style>
</head>
<body>
    <form id="formPage" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <div id="toolBar">
    <ext:Panel ID="PageToolBar" runat="server">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Label runat="server" Text="Start Date" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:DateField ID="dtStartDate" runat="server" Editable="false">
                    <DirectEvents>
                            <Select OnEvent="onStartDateSelect" />
                        </DirectEvents>
                    </ext:DateField>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:Label runat="server" Text="End Date" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:DateField ID="dtEndDate" runat="server" Editable="false">
                    <DirectEvents>
                            <Select OnEvent="onEndDateSelect" />
                        </DirectEvents>
                    </ext:DateField>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="Accept">
                        <Listeners>
                            <Click Handler="gridPanelReload();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
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
        <table style="width: 800; margin: 0 auto; vertical-align:bottom;">
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
        <ext:Label ID="Label3" runat="server" Text="SUMMARY OF PAID-OFF LOANS FOR THE PERIOD" />
            <ext:Label ID="lblStartDate" runat="server"/>
            <ext:Label ID="lblEndDate" runat="server"/>
        </b></center>
        <br />
        <br />
        <ext:GridPanel ID="PaidOffLoansGridPanel" runat="server" AutoHeight="true" Width="800"  EnableColumnHide="false" ColumnLines="true" BaseCls="cssClass">
            <Store>
                <ext:Store ID="PaidOffLoansStore" runat="server" OnRefreshData="RefreshData">
                    <Reader>
                        <ext:JsonReader>
                            <Fields>
                                <ext:RecordField Name="Name"/>
                                <ext:RecordField Name="LoanType" />
                                <ext:RecordField Name="LoanAmount" />
                                <ext:RecordField Name="InterestRate" />
                                <ext:RecordField Name="LoanTerm" />
                                <ext:RecordField Name="DatePaidOff" />
                                <ext:RecordField Name="_DatePaidOff" />
                            </Fields>
                        </ext:JsonReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column Header="Name of Borrower" Locked="true"  Hideable="false" Align="Center" Fixed="true" Resizable="false" Width="220" MenuDisabled="true" Css="font-weight:bold; text-align:center; border-color: Black;" />
                    <ext:Column Header="Type of Loan" Locked="true"  Hideable="false" DataIndex="LoanType" Align="Center" MenuDisabled="true" Css="font-weight:bold; text-align:center; border-color: Black;" Width="100"/>
                    <ext:NumberColumn Header="Amount of Loan" Locked="true"  Hideable="false" DataIndex="LoanAmount" Align="Center" Width="100" MenuDisabled="true"  Css="font-weight:bold;border-width:1px; text-align:center; border-color: Black;"  Format=",000.00"/>
                    <ext:Column Header="Interest Rate" Locked="true"  Hideable="false" DataIndex="InterestRate" Align="Center"  MenuDisabled="true" Css="font-weight:bold; text-align:center; border-color: Black;" Width="80"/>
                    <ext:Column Header="Term of Loan" Locked="true"  Hideable="false" DataIndex="LoanTerm" Align="Center" MenuDisabled="true"  Css="font-weight:bold; text-align:center; border-color: Black;" Width="100"/>
                    <ext:Column Header="Date Paid Off" Locked="true" Hideable="false" DataIndex="_DatePaidOff" Align="Center" MenuDisabled="true" Css="font-weight:bold; text-align:center; border-color: Black;" Width="150"/>
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
