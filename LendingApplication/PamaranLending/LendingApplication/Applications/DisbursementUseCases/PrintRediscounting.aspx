<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintRediscounting.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.PrintRediscounting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Loan Record</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../../resources/css/LoanPaymentForm.css" rel="stylesheet" type="text/css" />
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
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 12px;
	        background-color : #fff !important;
        }
        .style2
        {
            width: 478px;
        }
        .style4
        {
            width: 180px;
        }
        .style5
        {
            width: 471px;
            text-align: center;
        }
        .style7
        {
            width: 560px;
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
        <div id="PrintableContent" style="padding: 0px; margin-top: 0px; margin-right: auto;
            margin-bottom: 0px; margin-left: auto;">
            <br />
            <%--Cashier's Copy--%>
            <table style="width:560px; margin: 0 auto; border: 1px solid Black; padding-left: 5px;">
                <%--HEADER--%>
                <tr class="heading">
                    <td style="text-align:center;" class="style7">
                        <br />
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" class="style7">
                        <b>REDISCOUNTING</b></td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style7">
                        <asp:Label ID="lblDateDisbursed" runat="server" style="text-align: right" />
                        &nbsp;&nbsp;<br />
                    </td>
                </tr>
                <tr class="AccountOwnersBasicInformationLabel">
                    <td class="style7">
                        <b>&nbsp;Disbursement Details</b><br />
                    </td>
                </tr>
                <tr id="LoanAccountDetails">
                    <td class="style7">
                        <table>
                            <tr>
                                <td style="text-align:left;" class="style4">
                                      &nbsp;
                                      Disbursed To <br />
                                      &nbsp;
                                      Received By <br />
                                      &nbsp;
                                      Disbursed By <br />
                                      &nbsp;
                                      Bank <br />
                                      &nbsp;
                                      Check Number <br />
                                      &nbsp;
                                      Check Date <br />
                                      &nbsp;
                                      Check Amount (Php) <br />
                                      &nbsp;
                                      Surcharge Fee (Php) <br />
                                      &nbsp;
                                      Amount Disbursed (Php) <br /><br /><br />
                                </td>
                                <td style="text-align:left;" class="style2">
                                    <asp:Label ID="lblDisbursedTo" runat="server" /><br />
                                    <asp:Label ID="lblReceivedByName" runat="server" /><br />
                                    <asp:Label ID="lblDisbursedBy" runat="server" /><br />
                                    <asp:Label ID="lblBank" runat="server" /><br />
                                    <asp:Label ID="lblCheckNumber" runat="server" /><br />
                                    <asp:Label ID="lblCheckDate" runat="server" /><br />
                                    <asp:Label ID="lblCheckAmount" runat="server" /><br />
                                    <asp:Label ID="lblSurchargeFee" runat="server"/><br />
                                    <asp:Label ID="lblAmountInWords" runat="server" />&nbsp<strong><asp:Label ID="lblAmountDisbursed" runat="server" /></strong><br /><br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br />
                    <b>Amount Disbursed Breakdown</b></td>
                </tr>
                <tr>
                    <td>
                        <ext:GridPanel ID="grdBreakdown" runat="server" AutoHeight="true" Width="560"  EnableColumnHide="false" ColumnLines="true" BaseCls="cssClass">
                            <Store>
                            <ext:Store ID="strBreakdown" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="PaymentMethod"/>
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
                                    <ext:Column Header="Payment Method" Locked="true"  Hideable="false"
                                        DataIndex="PaymentMethod" Align="Center" Fixed="true" Resizable="false"
                                        Width="90" MenuDisabled="true" Sortable="false" Css="border-color: Black;" />
                                    <ext:Column Header="Bank Name" Locked="true"  Hideable="false"
                                        DataIndex="BankName" Align="Center" Width="130" MenuDisabled="true"
                                        Sortable="false" Css="border-color: Black;" />
                                    <ext:Column Header="Branch" Locked="true"  Hideable="false" DataIndex="BankBranch"
                                        Align="Center" Width="120" MenuDisabled="true" Sortable="false"
                                        Css="border-color: Black;"  />
                                    <ext:Column Header="Check Number" Locked="true"  Hideable="false"
                                        DataIndex="CheckNumber" Align="Center" Width="100" MenuDisabled="true"
                                        Sortable="false" Css="border-color: Black;" />
                                    <ext:NumberColumn Header="Amount" Locked="true" Hideable="false"
                                        DataIndex="TotalAmount" Align="Center" MenuDisabled="true" Sortable="false"
                                        Css="border-color: Black;"  Width="110" Format=",000.00" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="style7">
                        <table>
                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="style5">
                                    <center>
                                        <ext:Image ID="imgSignature" Hidden="false" Height="60" Width="150" Flex="1"
                                            runat="server" ImageUrl="../../../Resources/images/emptysignature.png"/>
                                    </center>
                                </td>
                            </tr>
                                
                            <tr>
                                <td class="style7">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="style5">
                                    <asp:Label ID="lblReceivedby" runat="server" Font-Underline="True" Style="text-align: center"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="style7">
                                </td>
                                <td colspan="2" class="style5">
                                    <strong>(Printed Name and Signature of Customer)<br />
                                    </strong>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width:560px; margin: 0 auto; padding-left: 5px;">
                <tr>
                    <td>
                        White&nbsp; - Cashier's Copy<br />
                        Blue&nbsp;&nbsp;&nbsp; - Customer&#39;s Copy<br />
                        Yellow - File Copy
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <hr />
        </div>
    </form>
</body>
</html>
