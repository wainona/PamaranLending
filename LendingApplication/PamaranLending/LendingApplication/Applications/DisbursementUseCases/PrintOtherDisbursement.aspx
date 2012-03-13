<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintOtherDisbursement.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.PrintOtherDisbursement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Other Disbursement</title>
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
            font-family: Tahoma;
            margin : 0;
            padding : 0px;
            font-size: 12px;
            background-color : #fff !important;
        }
             .rightLabel
        {
            margin-left: 500px;
            width: 300px;
        }
        .style1
        {
            width: 162px;
        }
        .header
        {
            text-align:center;
        }
        .style4
        {
            height: 27px;
            width: 546px;
        }
          .style7
        {
            width: 487px;
            text-align: center;
        }
        .style8
        {
            width: 560px;
        }
        @media screen
        {
            #hiddenDiv
            {
                display: none;
            }
            
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
            
            #hiddenDiv
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
                <tr class="heading">
                    <td style="text-align:center;" class="style8">
                        <br />
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; font-weight: 700" class="style8">
                        OTHER DISBURSEMENT</td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style8">
                        <asp:Label ID="lblDateDisbursed" runat="server" /><br />
                    </td>
                </tr>
                <tr class="AccountOwnersBasicInformationLabel">
                    <td class="style8">
                        <b>Disbursement Details</b><br />
                    </td>
                </tr>
                <tr id="DisbursementDetails">
                    <td class="style8">
                        <table>
                            <tr>
                                <td>
                                    Disbursed To <br />
                                          Amount <br />
                                     Disbursed By<br />
                            </td>
                                <td>
                                    <asp:Label ID="lblDisbursedTo" runat="server" /><br />
                                    <asp:Label ID="lblAmountReceived" runat="server"></asp:Label><br />
                                    <asp:Label ID="lblDisbursedBy" runat="server" /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br />
                    <b>Amount Disbursed Breakdown:</b></td>
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
                                            DataIndex="PaymentMethod" Align="Center" Fixed="true"
                                            Resizable="false" Width="90" MenuDisabled="true"  
                                            Css="font-weight:bold; text-align:center; border-color: Black;" />
                                    <ext:Column Header="Bank Name" Locked="true"  Hideable="false" DataIndex="BankName" 
                                            Align="Center" Css="font-weight:bold; text-align:center; border-color: Black;" 
                                            Width="130" MenuDisabled="true" />
                                    <ext:Column Header="Branch" Locked="true"  Hideable="false" DataIndex="BankBranch" 
                                            Align="Center" Css="font-weight:bold;border-width:1px; text-align:center; border-color: Black;" 
                                            Width="120" MenuDisabled="true" />
                                    <ext:Column Header="Check Number" Locked="true"  Hideable="false" DataIndex="CheckNumber" 
                                            Align="Center"  Css="font-weight:bold; text-align:center; border-color: Black;" 
                                            Width="100" MenuDisabled="true"/>
                                    <ext:NumberColumn Header="Amount" Locked="true" Hideable="false" DataIndex="TotalAmount" 
                                            Align="Center" Css="font-weight:bold; text-align:center; border-color: Black;" 
                                            Width="115" Format=",000.00" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </td>
                </tr>
                <tr><td><br /></td></tr>
                <tr id="Items">
                    <td class="style8">
                        <b>Disbursement Items:</b><br />
                        <asp:GridView runat="server" ID="grdOtherDisbursement" 
                            AutoGenerateColumns="False" Width="560px" style="text-align: center">
                            <Columns>
                                <asp:BoundField DataField="Particular" HeaderStyle-CssClass="header" HeaderText="Particular" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"/>
                                <asp:BoundField DataField="PerItemAmount" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header" HeaderText="Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" DataFormatString="{0:N}"/>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: right" class="style4">
                        &nbsp;<asp:Label ID="Label" Text="Total Amount:" runat="server"></asp:Label>
                            &nbsp;&nbsp;<b><asp:Label ID="lblTotalAmount" runat="server" 
                            Font-Underline="True"></asp:Label>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="style8">
                        <table style="width: 577px">
                            <tr>
                                <td class="style8">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="style7">
                                    <center>
                                        <ext:Image ID="imgSignature" Height="60" Width="150" Flex="1"
                                            runat="server" />
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td class="style8">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="style7">
                                    <asp:Label ID="lblReceivedby" runat="server" Font-Underline="True" Style="text-align: center"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1" class="style6"></td>
                                <td colspan="2" class="style7">
                                    <strong>(Printed Name and Signature of Employee)<br /></strong>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width:560px; margin: 0 auto; padding-left: 5px;">
                <tr><td>White: Cashier's Copy</td></tr>
                <tr><td>Yellow: Customer's Copy</td></tr>
                <tr><td>Blue: File Copy</td></tr>
            </table>
            <br /><br />
        </div>
    </form>
</body>
</html>

