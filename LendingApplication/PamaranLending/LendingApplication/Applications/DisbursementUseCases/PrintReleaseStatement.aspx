<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintReleaseStatement.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.PrintReleaseStatement" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Release Statement</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
           // PageToolBar.show();
        }

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
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 12px;
	        background-color : #fff !important;
        }
        .style2
        {
            width: 150px;
        }
        .style3
        {
            height:30px;
        }
        .style5
        {
            width: 175px;
        }
        .gridStyle
        {
            margin-left: 10px;
            margin-top: 10px;
        }
        .gridStyle2
        {
            margin-left: 59px;
        }
        .grid
        {
            margin-left: 250px;
            width: 500px;
        }
        .grid1
        {
            margin-left: 214px;
            width: 450px;
        }
        .style6
        {
            width: 131px;
            height: 84px;
            font-weight: bold;
        }
        .style7
        {
            width: 325px;
            height: 84px;
        }
        .style10
        {
            width: 260px;
            height: 84px;
            font-weight: bold;
        }
        .style11
        {
            width: 600px;
            height: 84px;
        }
        .style12
        {
            width: 225px;
            height: 84px;
        }
        .headerClass
        {
            font-weight: bold;
        }
        .style13
        {
            width: 545px;
        }
        .style14
        {
            width: 346px;
            text-align: center;
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
        .style15
        {
            width: 226px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hiddenPaymentId" runat="server"/>
    <div id="toolBar">
    <ext:FormPanel runat="server" Border="false">
        <TopBar>
        <ext:Toolbar runat="server" ID="PageToolBar">
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
            <ext:Hidden ID="hdnAgreementId" runat="server"></ext:Hidden>
            </Items>
        </ext:Toolbar>
        </TopBar>
    </ext:FormPanel>
    </div>
    <div id="PrintableContent">
        <table style="width:620px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;">
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" />&nbsp;<asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />&nbsp;<asp:Label ID="lblProvince" runat="server" />&nbsp;<asp:Label ID="lblCountry" runat="server" />&nbsp;<asp:Label ID="lblPostalCode" runat="server" /><br />
                        Tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />&nbsp;<asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        Fax#: <asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align:center; font-weight:bold; border-bottom-color: Gray; border-bottom-width:thin;" class="style3">
                        LOAN RELEASE STATEMENT<br />
                    </td>
                </tr>
                <tr>
                <td style="text-align: right">
                <asp:Label ID="lblGranted" runat="server" style="text-align: right"></asp:Label>
                &nbsp;&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 610px">
                            <tr>
                                <td class="style6">
                                    Name: <br /><br />
                                    Station No.: <br /><br />
                                    Payment Start Date: 
                                </td>
                                <td class="style7">
                                    <asp:Label ID="lblName" runat="server" /><br /><br />
                                    <asp:Label ID="lblStationNo" runat="server" /><br /><br />
                                    <asp:Label ID="lblToStartOn" runat="server" /><br />
                                </td>
                                </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                        <table>
                            <tr>
                                <td style="text-align:left; font-weight:bold;" class="style5">
                                    Principal Amount: <br /> <br />
                                </td>
                                <td style="text-align:right; width: 600px;">
                                    <asp:Label ID="lblPrincipalAmount" runat="server" /><br /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="FeesRow">
                   <td style="text-align:justify;">
                        <b>Fees:</b>
                            <asp:GridView CssClass="gridStyle" runat="server" ID="grdFees" Font-Size="Small" 
                            AutoGenerateColumns="False" Width="600" ShowHeader="false" BorderColor="Transparent" 
                            EditRowStyle-BorderColor="Transparent" RowStyle-BorderColor="Transparent" 
                            GridLines="None" >
                            <Columns>
                                <asp:BoundField DataField="Fees" HeaderText=""/>
                                <asp:BoundField DataField="Amount" HeaderText="" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="style10">
                                    <br />Total Fees:&nbsp; <br /><br />
                                    Total Previous Loan(s): <br /><br />
                                    Total Deductions: <br /><br />
                                    NET AMOUNT RECEIVED: <br />
                                </td>
                                <td class="style11">
                                    <asp:Label ID="lblTotalFees" runat="server" Text="0.00" /><br /><br />
                                    <asp:Label ID="Label5" runat="server" /><br /><br />
                                    <asp:Label ID="Label6" runat="server" /><br /><br />
                                    <asp:Label ID="Label7" runat="server" /><br />
                                </td>
                                <td class="style12">
                                    &nbsp;<asp:Label ID="label" runat="server"></asp:Label><br />
                                    <br />
                                    <asp:Label ID="lblTotalPreviousLoan" runat="server" Text="0.00" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lblTotalDeductions" runat="server" Text="0.00" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lblNetAmountReceived" runat="server" Font-Bold="True" /><br />
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        <b>Pending Loan(s):</b>
                            <asp:GridView CssClass="gridStyle2" runat="server" ID="grdPendingLoans" Font-Size="Small" 
                            AutoGenerateColumns="False" Width="483px" BorderColor="Transparent" 
                            EditRowStyle-BorderColor="Transparent" HeaderStyle-Font-Bold="true" RowStyle-BorderColor="Transparent" 
                            GridLines="None" >
                            <Columns>
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name"/>
                                <asp:BoundField DataField="Amount" HeaderText="Balances" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" CssClass="headerClass" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                <td colspan="3">
                  <table>
                            <tr>
                                <td class="style15">
                                    <b>Total Balances:</b>
                                </td>
                                <td>
                                    <b><asp:Label CssClass="grid1" ID="lblTotalAmountPending" runat="server" Text="0.00"></asp:Label></b>
                                </td>
                            </tr>
                        </table>
                </td>
                </tr>
                <tr>
                    <td>
                    <hr />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp; I hereby acknowledge receipt of a copy of this statement prior to the 
                        consumation of this credit transaction and I further confirm that I fully 
                        understand and agree to all the terms and conditions there and I further 
                        acknowledge receipt to the above amount.</td>
                </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="style13">
                            </td>
                            <td class="style14">
                              <center>
                                    <ext:Image ID="imgSignature" Hidden="false" Height="60" Width="150" Flex="1" runat="server" />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td class="style13">
                            </td>
                            <td class="style14">
                                <asp:Label ID="lblBorrowerName" runat="server" Style="text-decoration: underline"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style13">
                            </td>
                            <td class="style14">
                                (Printed Name and Signature of Borrower)
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
