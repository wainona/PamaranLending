<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutstandingLoansReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.OutstandingLoansReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Loan Record</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        function printdiv(printpage) {
            var headstr = '<html><head></head><body>';
            var footstr = '</body></html>';
            var newstr = document.getElementById(printpage).innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }

        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
            //PageToolBar.show();
        }
    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
        .document
        {
            text-decoration: underline;
        }
        .gridviewheader
        {
            padding:20px;
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
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
        <div id="toolBar">
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false">
        <TopBar>
        <ext:Toolbar ID="PageToolBar" runat="server">
        <Items>
        <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer">
            <Listeners>
                <Click Handler="printContent('PrintableContent');" />
            </Listeners>
        </ext:Button>
        <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
            <Listeners>
                <Click Handler="window.proxy.requestClose();" />
            </Listeners>
        </ext:Button>
        <ext:Hidden ID="hdnLoanId" runat="server" />
        </Items>
        </ext:Toolbar>
        </TopBar>
        </ext:FormPanel>
        </div>
        <div id="PrintableContent">
            <table style="width:845px; margin: 0 auto;">
                <%--HEADER--%>
                <tr class="heading">
                    <td style="text-align:center;" colspan="8">
                        <ext:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <ext:Label ID="lblStreetAddress" runat="server" /> <ext:Label ID="lblBarangay" runat="server" /><br />
                        <ext:Label ID="lblMunicipality" runat="server" /><ext:Label ID="lblCity" runat="server" />, <ext:Label ID="lblProvince" runat="server" />, <ext:Label ID="lblCountry" runat="server" />, <ext:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <ext:Label ID="lblPrimTelNumber" runat="server" />, <ext:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<ext:Label ID="lblFaxNumber" runat="server" /><br />
                        <ext:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br /><br />
                    </td>
                </tr>
                  <tr class="nameOfDocument">
                    <td style="text-align:center; vertical-align:bottom; font-weight:bold; border-bottom-color: Gray; border-bottom-width:thin; text-transform:uppercase;" class="style3" colspan="9">
                        REPORT ON OUTSTANDING LOANS, LOANS GRANTED AND PAYMENTS
                        <br />
                        FOR THE MONTH OF
                        <ext:Label ID="lblSelectedMonth" Cls="document" LabelAlign="Top" runat="server"/>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr id="Tr1" align="center">
                   <td style="text-align:center; margin: 0 auto;" colspan="8">
                       <ext:GridPanel ID="GridPanel1" Border="true" Width="842px" ColumnLines="true" BaseCls="cssClass" runat="server" Layout="FitLayout" AutoHeight="true">
                            <Store>
                                <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="FinancialProductName" />
                                                <ext:RecordField Name="OutstandingLoanBalanceAsOfLastMonthEnd"/>
                                                <ext:RecordField Name="LoansGranted"/>
                                                <ext:RecordField Name="PrincipalPayment" />
                                                <ext:RecordField Name="OutstandingLoanBalanceAsOfCurrentMonthEnd" />
                                                <ext:RecordField Name="LoanAdjustments"></ext:RecordField>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                                <Columns>
                                    <ext:Column Header="Product" Hideable="false" Sortable="false" DataIndex="FinancialProductName" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="140px"  />
                                    <ext:NumberColumn Header="Outstanding Loan Balance as of (last month-end)" Hideable="false" Sortable="false" DataIndex="OutstandingLoanBalanceAsOfLastMonthEnd" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true" 
                                        Width="140px" />
                                    <ext:NumberColumn Header="Loans Granted" Hideable="false" Sortable="false" DataIndex="LoansGranted" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="140px" />
                                    <ext:NumberColumn Header="Principal Payments" Hideable="false" Sortable="false" DataIndex="PrincipalPayment" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="140px" />
                                        <ext:NumberColumn Header="Loan Adjustments" Hideable="false" Sortable="false" DataIndex="LoanAdjustments" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="140px" />
                                    <ext:NumberColumn Header="Outstanding Loans as of(this month-end)" Hideable="false" Sortable="false" DataIndex="OutstandingLoanBalanceAsOfCurrentMonthEnd" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="140px" />
                                </Columns>
                            </ColumnModel>
                       </ext:GridPanel>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
