<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintChange.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.PrintChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Print Change</title>
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
            font-size        : 13px;
	        background-color : #fff !important;
        }
        .style1
        {
            width: 587px;
            text-align: center;
        }
        .style2
        {
            width: 452px;
        }
        .style3
        {
            width: 702px;
        }
        .style4
        {
            width: 197px;
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
        <br />
            <%--Cashier's Copy--%>
            <table style="width:631px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;" class="style7">
                        <br />
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br /><br />
                    </td>
                </tr>
                <tr>
                <td style="text-align: center; font-weight:bold;" class="style7">
                
                    <b>CHANGE</b></td>
                </tr>
                <tr>
                <td style="text-align: right" class="style7">
                 <asp:Label ID="lblDateDisbursed" runat="server" style="text-align: right" />&nbsp;&nbsp;<br />
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
                                      &nbsp; Receipt ID<br />
                                      &nbsp;
                                      Receipt Amount (Php) <br />
                                      &nbsp;
                                      Amount Disbursed (Php) <br />
                                      &nbsp;
                                      Disbursed By 
                                      <br />
                                      <br />
                                      <br />
                                </td>
                                <td style="text-align:left;" class="style2">
                                <asp:Label ID="lblDisbursedTo" runat="server" /><br />
                                <asp:Label ID="lblReceiptId" runat="server" /><br />
                                <asp:Label ID="lblReceiptAmount" runat="server" /><br />
                                <asp:Label ID="lblAmountInWords" runat="server" />&nbsp<strong><asp:Label ID="lblAmountDisbursed" runat="server" /></strong><br />
                                <asp:Label ID="lblDisbursedBy" runat="server" />
                                    <br />
                                    <br />
                                    <br />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td colspan="3" class="style7">
                <table>
                <tr>
                    <td class="style3">
                    </td>
                    <td class="style1">
                        <center>
                            <ext:Image ID="imgSignature" Hidden="false" Height="60" Width="150" Flex="1"
                                                        runat="server" />
                        </center>
                    </td>
                </tr>
                <tr>
                <td class="style3">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td class="style1">
                   <asp:Label ID="lblReceivedby" runat="server" Font-Underline="True" 
                          style="text-align: center"></asp:Label>
                </td>
                </tr>
                <tr>
                <td colspan="1" class="style3">
                </td>
                <td colspan="2" class="style1">
                    <strong>(Printed Name and Signature of Customer)<br />
                    </strong>
                    <br />
                </td>
                </tr>
                </table>
                </td>
                </tr>
                <tr><td>Cashier's Copy</td></tr>
            </table>
            <br /><br />
            <hr />
            <%--Customer's Copy--%>
            <table style="width:631px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;" class="style7">
                        <br />
                        <asp:Label ID="lblLenderNameHeader1" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress1" runat="server" /> <asp:Label ID="lblBarangay1" runat="server" /><br />
                        <asp:Label ID="lblMunicipality1" runat="server" /><asp:Label ID="lblCity1" runat="server" />, <asp:Label ID="lblProvince1" runat="server" />, <asp:Label ID="lblCountry1" runat="server" />, <asp:Label ID="lblPostalCode1" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber1" runat="server" />, <asp:Label ID="lblSecTelNumber1" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber1" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress1" runat="server" />
                        <br /><br />
                    </td>
                </tr>
                <tr>
                <td style="text-align: center; font-weight:bold;" class="style7">
                
                    <b>CHANGE</b></td>
                </tr>
                <tr>
                <td style="text-align: right" class="style7">
                 <asp:Label ID="lblDateDisbursed1" runat="server" style="text-align: right" />&nbsp;&nbsp;<br />
                </td>
                </tr>
                <tr class="AccountOwnersBasicInformationLabel">
                    <td class="style7">
                        <b>&nbsp;Disbursement Details</b><br />
                    </td>
                </tr>
                <tr id="LoanAccountDetails1">
                    <td class="style7">
                        <table>
                            <tr>
                                <td style="text-align:left;" class="style4">
                                      &nbsp;
                                      Disbursed To <br />
                                      &nbsp; Receipt ID<br />
                                      &nbsp;
                                      Receipt Amount (Php) <br />
                                      &nbsp;
                                      Amount Disbursed (Php) <br />
                                      &nbsp;
                                      Disbursed By 
                                      <br />
                                      <br />
                                      <br />
                                </td>
                                <td style="text-align:left;" class="style2">
                                <asp:Label ID="lblDisbursedTo1" runat="server" /><br />
                                <asp:Label ID="lblReceiptId1" runat="server" /><br />
                                <asp:Label ID="lblReceiptAmount1" runat="server" /><br />
                                <asp:Label ID="lblAmountInWords1" runat="server" />&nbsp<strong><asp:Label ID="lblAmountDisbursed1" runat="server" /></strong><br />
                                <asp:Label ID="lblDisbursedBy1" runat="server" />
                                    <br />
                                    <br />
                                    <br />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td colspan="3" class="style7">
                <table>
                <tr>
                <td class="style3">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td class="style1">
                   <asp:Label ID="lblReceivedby1" runat="server" Font-Underline="True" 
                          style="text-align: center"></asp:Label>
                </td>
                </tr>
                <tr>
                <td colspan="1" class="style3">
                </td>
                <td colspan="2" class="style1">
                    <strong>(Printed Name and Signature of Customer)<br />
                    </strong>
                    <br />
                </td>
                </tr>
                </table>
                </td>
                </tr>
                <tr><td>Customer's Copy</td></tr>
            </table>
            <br /><br />
            <hr />
            <%--File Copy--%>
            <table style="width:631px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;" class="style7">
                        <br />
                        <asp:Label ID="lblLenderNameHeader2" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress2" runat="server" /> <asp:Label ID="lblBarangay2" runat="server" /><br />
                        <asp:Label ID="lblMunicipality2" runat="server" /><asp:Label ID="lblCity2" runat="server" />, <asp:Label ID="lblProvince2" runat="server" />, <asp:Label ID="lblCountry2" runat="server" />, <asp:Label ID="lblPostalCode2" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber2" runat="server" />, <asp:Label ID="lblSecTelNumber2" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber2" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress2" runat="server" />
                        <br /><br />
                    </td>
                </tr>
                <tr>
                <td style="text-align: center; font-weight:bold;" class="style7">
                
                    <b>CHANGE</b></td>
                </tr>
                <tr>
                <td style="text-align: right" class="style7">
                 <asp:Label ID="lblDateDisbursed2" runat="server" style="text-align: right" />&nbsp;&nbsp;<br />
                </td>
                </tr>
                <tr class="AccountOwnersBasicInformationLabel">
                    <td class="style7">
                        <b>&nbsp;Disbursement Details</b><br />
                    </td>
                </tr>
                <tr id="LoanAccountDetails2">
                    <td class="style7">
                        <table>
                            <tr>
                                <td style="text-align:left;" class="style4">
                                      &nbsp;
                                      Disbursed To <br />
                                      &nbsp; Receipt ID<br />
                                      &nbsp;
                                      Receipt Amount (Php) <br />
                                      &nbsp;
                                      Amount Disbursed (Php) <br />
                                      &nbsp;
                                      Disbursed By 
                                      <br />
                                      <br />
                                      <br />
                                </td>
                                <td style="text-align:left;" class="style2">
                                <asp:Label ID="lblDisbursedTo2" runat="server" /><br />
                                <asp:Label ID="lblReceiptId2" runat="server" /><br />
                                <asp:Label ID="lblReceiptAmount2" runat="server" /><br />
                                <asp:Label ID="lblAmountInWords2" runat="server" />&nbsp<strong><asp:Label ID="lblAmountDisbursed2" runat="server" /></strong><br />
                                <asp:Label ID="lblDisbursedBy2" runat="server" />
                                    <br />
                                    <br />
                                    <br />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td colspan="3" class="style7">
                <table>
                <tr>
                <td class="style3">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <td class="style1">
                   <asp:Label ID="lblReceivedby2" runat="server" Font-Underline="True" 
                          style="text-align: center"></asp:Label>
                </td>
                </tr>
                <tr>
                <td colspan="1" class="style3">
                </td>
                <td colspan="2" class="style1">
                    <strong>(Printed Name and Signature of Customer)<br />
                    </strong>
                    <br />
                </td>
                </tr>
                </table>
                </td>
                </tr>
                <tr><td>File Copy</td></tr>
            </table>
        </div>
    </form>
</body>
</html>
