   <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintTransactionSlip.aspx.cs"
    Inherits="LendingApplication.Applications.DisbursementUseCases.PrintTransactionSlip" %>

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
        var AddAvailment = function () {
            X.CalculateNewOutstanding();
        };
//        var InterestChange = function () {
//            X.CalculateNetProceed();
//        };
        var AttorneyFeeChange = function () {
            X.CalculateNetProceed();
        };
        var onChkAvailment = function () {
            if (chkAvailment.getValue() == true) {
                chkAvailment2.setValue(true);
                chkAvailment3.setValue(true);
            }else{
                chkAvailment2.setValue(false);
                chkAvailment3.setValue(false);
            }
        }
        var onChkSubAvailment = function () {
            if (chkSubAvailment.getValue() == true) {
                chkSubAvailment2.setValue(true);
                chkSubAvailment3.setValue(true);
            } else {
                chkSubAvailment2.setValue(false);
                chkSubAvailment3.setValue(false);
            }
        }
        var onChkRestructuring = function () {
            if (chkRestructuring.getValue() == true) {
                chkRestructuring2.setValue(true);
                chkRestructuring3.setValue(true);
            } else {
                chkRestructuring2.setValue(false);
                chkRestructuring3.setValue(false);
            }
        }
        var onChkChange = function () {
            if (chkChange.getValue() == true) {
                chkChange2.setValue(true);
                chkChange3.setValue(true);
                }else {
                chkChange2.setValue(false);
                chkChange3.setValue(false);
            }
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
        .style1
        {
            width: 227px;
        }
        .style3
        {
            height: 62px;
        }
        .underline
        {
            
            font-weight:bold;
        }
        .underline1
        {
     
            font-weight:bold;
        }
        .style5
        {
            width: 176px;
        }
        .style9
        {
            width: 259px;
        }
        .style13
        {
            width: 320px;
        }
        .checkbox
        {
            width:125;
        }
        .txtfield
        {
           border-style:solid;
           background-image:none;
           font-weight: bold;
           border-color:transparent;
           
        }
        .style16
        {
            width: 245px;
        }
        .style17
        {
            width: 267px;
        }
        .style18
        {
            width: 500px;
        }
        .style20
        {
            width: 89px;
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
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
        <br />
        <br />
        <br />
        <table style="border: thin solid #000000; width: 500px; margin: 0 auto;" 
            frame="border">
            <%--HEADER--%>
            <tr class="heading">
                <td style="text-align: center;" class="style3" colspan="3">
                    <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" Font-Size="Large" /><br />
                    <asp:Label ID="lblFormType" runat="server" Font-Size="Medium" />
                    <br /><br />
                </td>
            </tr>
            <tr>
            <td colspan="3" 
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;"></td>
            </tr>
      
                <tr>
                    <td class="style9" 
                        
                        
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                        Station No:&nbsp;<ext:Label ID="lblStationNum" runat="server" Cls="underline"></ext:Label></td>
                    <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;" 
                        class="style13">
                        &nbsp;</td>
                    <td class="style5" 
                        
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                        Date:&nbsp;<ext:Label ID="lblTransacDate" runat="server" Cls="underline"></ext:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" 
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                        Received 
                        from:&nbsp;<ext:Label ID="lblReceivedFrom" runat="server" Cls="underline1"></ext:Label>
                    </td>
                </tr>
                    <tr>
                    <td colspan="3" 
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                        the sum of <ext:Label ID="lblSumAmountWords" runat="server" Cls="underline"></ext:Label>
                        </td>
             
                </tr>
                <tr>
                    <td colspan="3" 
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000; text-align:right">
                    (P<ext:Label ID="lblSumAmount" runat="server" Cls="underline"></ext:Label>)
                      </td>
                </tr>

             <%--   <tr>
                    <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;" 
                        class="style9">
                    <br />
                        &nbsp;&nbsp;Cash:&nbsp;
                        <ext:Label ID="lblCash" runat="server" Cls="underline"></ext:Label>
                    </td>
                    <td class="style13">
                    </td>
                    <td class="style5" 
                        
                        style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                    <br />
                    Check 
                        #:<ext:Label ID="lblCheckNum" runat="server" Cls="underline"></ext:Label></td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <table style="width: 583px">
                            <tr>
                                <td class="style17" style="text-align: center;">
                                <br />
                               <ext:Label ID="lblAvailment" runat="server" style="text-decoration: underline"></ext:Label>&nbsp;First Availment
                              </td>
                                <td class="style16" style="text-align: center;">
                                <br />
                                <ext:Label ID="lblSubAvailment" runat="server" style="text-decoration: underline"></ext:Label>&nbsp;Subsequent Availment 
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="2" 
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                    Amount<ext:Label ID="lblAmount" runat="server" Cls="underline"></ext:Label>
                </td>
                 <td colspan="2"  style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                Balance <ext:Label ID="lblLoanBalance" runat="server" Cls="underline"></ext:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" 
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                    Interest Rate:<ext:Label ID="lblInterestRate" runat="server" Cls="underline"></ext:Label>%
                </td>
                <td colspan="2" class="style1" 
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                    Term:<ext:Label ID="lblTerm" runat="server" Cls="underline"></ext:Label> 
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Details of Loan Release:
                </td>
            </tr>
            <tr>
                <td class="style9">
                </td>
                <td 
                    class="style13">
                    Amount:&nbsp;  
                </td>
                <td
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                    P
                    <ext:Label ID="lblAmountDisbursed" runat="server" Cls="underline"></ext:Label> 
                </td>
            </tr>
            <tr>
                <td class="style9">
                <br />
                </td>
                <td class="style13">
                    Deductions:&nbsp;  
                </td>
                <td
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                    P
                    <ext:Label ID="lblDeductions" runat="server" Cls="underline"></ext:Label> 
                </td>
            </tr>
            <tr>
                <td class="style9">
                    <br />
                </td>
                <td class="style13">
                    &nbsp;Net Proceeds:<br />
                </td>
                <td class="style5" 
                    
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                    P<ext:Label ID="txtNetProceeds" runat="server" Width="120" Cls="underline"></ext:Label><br />
                </td>
            </tr>
            <tr>
            <td class="style9" style="text-align: center;">
            <br />
            Endorsed By:
             </td>
            <td class="style13" style="text-align: center;">
            <br />
            Approved By:
            </td>
             <td class="style5" style="text-align: center;">
             <br />
            Released By:
            </td>
            </tr>
            <tr>
            <td colspan="3">
                <br />
                </td>
            </tr>
             <tr>
            <td style="text-align: center;" class="style9" >
            <ext:Label ID="lblEndorsedBy" runat="server" Cls="underline" 
                    Font-Underline="True"></ext:Label>
            </td>
            <td style="text-align: center;" class="style13" >
            <ext:Label ID="lblApprovedBy" runat="server" Cls="underline" 
                    Font-Underline="True"></ext:Label>
            </td>
             <td style="text-align: center;" class="style5" >
             <ext:Label ID="lblReleasedBy" runat="server" Cls="underline" 
                     Font-Underline="True"></ext:Label>
            </td>
            </tr>
            <tr>
            <td style="text-align: center;" class="style9">
             Teller<br /><br />
            </td>
            <td style="text-align: center;" class="style13">
            Officer<br /><br />
            </td>
             <td style="text-align: center;" class="style5">
            Cashier<br /><br />  
            </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table>
                        <tr>
                            <td class="style20">
                                &nbsp;
                            </td>
                            <td>
                                <center>
                                    <ext:Image ID="imgSignature" Hidden="false" Height="60" Width="150" Flex="1" runat="server" />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" 
                                style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000" 
                                class="style20">
                                &nbsp;Received by:
                            </td>
                            <td colspan="2" style="border-bottom: thin ridge #000000; text-align: center;" 
                                class="style18">
                                <asp:Label ID="lblReceivedBy" runat="server" CssClass="underline"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table>
                        <tr>
                            <td colspan="1" class="style20">
                            </td>
                            <td colspan="2" style="text-align: center;" class="style18">
                                (Printed Name and Signature of Borrower)
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            <td class="style9" colspan="2">
            Summary of Outstanding Loan:
            </td>
            </tr>
            <tr>
            <td colspan="2">
                Balance Before Disbursal:</td>
            <td class="style5" 
                    
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                P<ext:Label ID="lblBalance" runat="server" Cls="underline"></ext:Label>
            </td>
            </tr>
            <tr>
            <td colspan="2">
            Add:Availment
            </td>
            <td class="style5" 
                    
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                P<ext:TextField ID="txtAvailment" runat="server" Width="120" Cls="txtfield">
                <Listeners>
                <Change Handler="AddAvailment();" />
                </Listeners>
                </ext:TextField>
            </td>
            </tr>
            <tr>
            <td colspan="2">
            Less: Payment
            </td>
            <td class="style5" 
                    
                    style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                P<ext:Label ID="lblPayment" runat="server" Cls="underline"></ext:Label>
            </td>
            </tr>
                    <tr>
            <td colspan="2" style="border-style: none">
            Balance After Disbursal:</td>
            <td class="style5" 
                            
                            style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                P<ext:Label ID="txtNewBalance" runat="server" Width="120" Cls="underline"></ext:Label>
            </td>
            </tr>
      

        </table>
        <table style="border: thin none #FFFFFF; width: 500px; margin: 0 auto;" 
            frame="border">
             <%-- Footer--%>
            <tr>
            <td colspan="2">
                White&nbsp; - Cashier's Copy<br />
                    Blue&nbsp;&nbsp;&nbsp; - 
                    Customer&#39;s Copy<br />
                    Yellow - File Copy</td>
            <td class="style5">
            No. <asp:Label ID="lblControlNumber" runat="server"></asp:Label>
            </td>
            </tr>
        </table>
        <br />
            <hr />

    </div>
    </form>
</body>
</html>
