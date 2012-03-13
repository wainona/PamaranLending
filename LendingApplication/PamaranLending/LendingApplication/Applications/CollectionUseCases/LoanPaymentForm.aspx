<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanPaymentForm.aspx.cs"
    Inherits="LendingApplication.Applications.CollectionUseCases.LoanPaymentForm" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Payment Form</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../../resources/css/LoanPaymentForm.css" rel="stylesheet" type="text/css" />
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
        @media screen
        {
            #toHide
            {
                display: block;
            }
        }
        
        @media print
        {
            #toHide
            {
                display: none;
            }
            .x-form-empty-field
            {
                color: transparent;
                border-bottom-color: Black;
                border-bottom-width: 1px;
            }
        }
        bold
        {
            font-weight: bold;
        }
        center
        {
            text-align: center;
        }
        underline
        {
            text-decoration: underline;
        }
        .smallFont
        {
            font-size: smaller;
            float: left;
            font-weight: bold;
        }
        .smallFontRight
        {
            font-size: smaller;
            float: right;
            font-weight: bold;
        }
        .style14
        {
            width: 40px;
        }
        .center
        {
            text-align: center;
        }
        .centerBold
        {
            text-align: center;
        }
        .style16
        {
            width: 105px;
        }
        .style34
        {
            width: 490px;
        }
        .style35
        {
            width: 560px;
        }
        .style36
        {
            width: 277px;
        }
    </style>
</head>
<body>
    <form id="pageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <div id="toHide">
        <ext:FormPanel runat="server" LabelWidth="100" Closable="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Hidden ID="hiddenPartyRoleId" runat="server" />
                        <ext:Hidden ID="hiddenParentPaymentId" runat="server" />
                        <ext:Hidden ID="hiddenRandomKey" runat="server" />
                        <ext:Hidden ID="hiddenGUID" runat="server" />
                        <ext:Hidden ID="hiddenType" runat="server" />
                        <ext:Hidden ID="hiddenPrincipalBorrower" runat="server" />
                        <ext:Hidden ID="hiddenPrincipalCoBorrower" runat="server" />
                        <ext:Hidden ID="hiddenPrincipal" runat="server" />
                        <ext:Hidden ID="hiddenInterestBorrower" runat="server" />
                        <ext:Hidden ID="hiddenInterestCoBorrower" runat="server" />
                        <ext:Hidden ID="hiddenInterest" runat="server" />
                        <ext:Hidden ID="hiddenPastDueBorrower" runat="server" />
                        <ext:Hidden ID="hiddenPastDueCoBorrower" runat="server" />
                        <ext:Hidden ID="hiddenPastDue" runat="server" />
                        <ext:Hidden ID="hiddenTotalAmount" runat="server" />
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                            <Listeners>
                                <Click Handler="printContent('PrintableContent');" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSeparator runat="server" />
                        <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:FormPanel>
    </div>
    <div id="PrintableContent" style="padding: 0px; margin-top: 0px; margin-right: auto;
        margin-bottom: 0px; margin-left: auto;">
    <%--START of NEW FORM--%>
        <table style="width:560px; margin: 0 auto; border: 1px solid Black; padding-left: 5px;">
            <tr class="heading">
                <td style="text-align:center;" class="style35">
                    <br />
                    <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" Font-Size="Medium"/><br />
                    <%--<asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                    <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                    tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                    fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                    <asp:Label ID="lblEmailAddress" runat="server" />--%>
                </td>
            </tr>
            <tr>
            <td style="text-align: center; font-weight: bold" class="style35">
                LOAN PAYMENT FORM<br /><br /></td>
            </tr>
            <tr>
                <td class="style35">
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table style="width: 560px;">
                        <tr>
                            <td style="width: 95px;">Station Number: </td>
                            <td style="width: 330px; text-align: left; border-bottom: 1px solid Black;"><ext:Label ID="lblLabelStationNumber" runat="server" /></td>
                            <td style="text-align: right; width: 40px;">Date:</td>
                            <td style="text-align: right; width: 115px; border-bottom: 1px solid Black;"><center><ext:Label ID="lblDate" runat="server" /></center></td>
                        </tr>
                        <tr>
                            <td class="style16">Received from:</td>
                            <td colspan="3" style="border-bottom: 1px solid Black;"><ext:Label ID="lblReceivedFrom" runat="server" /></td>
                        </tr>
                        <tr><td></td></tr>
                    </table>
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td class="style16">the sum of: </td>
                            <td  style="width: 325px; border-bottom: 1px solid Black;"><ext:Label runat="server" ID="lblLoanAmountWords" /></td>
                            <td>₱&nbsp;(</td>
                            <td  style=" width: 90px; border-bottom: 1px solid Black;"><center><b><ext:Label runat="server" ID="lblLoanAmount" /></b></center></td>
                            <td>)</td>
                        </tr>
                    </table>
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr><td>In Payment of the following:</td></tr>
                    </table>
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table style="width: 560px;">
                        <tr>
                            <td class="style14"></td>
                            <td>Principal</td>
                            <td>_____________________________________________________</td>
                            <td>&nbsp;₱&nbsp;</td>
                            <td style="text-align: right; border-bottom: 1px solid Black;"><ext:Label ID="lblPrincipal" Cls="indent1" Text="Principal" LabelWidth="50" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="style14"></td>
                            <td>Interest</td>
                            <td>____________________________________________________</td>
                            <td>&nbsp;₱&nbsp;</td>
                            <td style="text-align: right;  border-bottom: 1px solid Black;"><ext:Label ID="lblInterest" Cls="indent1" Text="Interest" LabelWidth="50" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="style14"></td>
                            <td>Others</td>
                            <td>_____________________________________________________</td>
                            <td>&nbsp;₱&nbsp;</td>
                            <td style="text-align: right;  border-bottom: 1px solid Black;"><ext:Label ID="lblOthers" Cls="indent1" LabelWidth="50" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="style14"></td>
                            <td><b>Total</b></td>
                            <td>_____________________________________________________</td>
                            <td>&nbsp;₱&nbsp;</td>
                            <td style="text-align: right;  border-bottom: 1px solid Black;"><b><ext:Label ID="lblTotal" Cls="indent1" Text="Others" LabelWidth="50" runat="server" /></b></td>
                        </tr>
                    </table>
                    <br />
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td><b>Details/Form of Payment</b></td>
                        </tr>
                        <tr>
                            <td style="padding-left: 10px; padding-right: 10px;">
                                <ext:GridPanel ID="GridPanelPayments" runat="server" AutoHeight="true" Width="560"
                                    EnableColumnHide="false" ColumnLines="true" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store ID="GridPanelPaymentsStore" runat="server" RemoteSort="false">
                                            <Reader>
                                                <ext:JsonReader>
                                                    <Fields>
                                                        <ext:RecordField Name="PaymentMethod" />
                                                        <ext:RecordField Name="BankName" />
                                                        <ext:RecordField Name="BankBranch" />
                                                        <ext:RecordField Name="CheckNumber" />
                                                        <ext:RecordField Name="_TransactionDate" />
                                                        <ext:RecordField Name="AmountApplied" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel ID="GridPanelPaymentColumnModel" runat="server">
                                        <Columns>
                                            <ext:Column Header="Payment Method" Locked="true" Hideable="false" DataIndex="PaymentMethod"
                                                Align="Center" Fixed="true" Resizable="false" Width="95" Css="font-weight:bold; text-align:center; border-color: Black; " 
                                                MenuDisabled="true" Sortable="false" />
                                            <ext:Column Header="Bank Name" Locked="true" Hideable="false" DataIndex="BankName"
                                                Align="Center" Css="font-weight:bold; text-align:center; border-color: Black; white-space:  normal !important;"
                                                Width="135" MenuDisabled="true" Sortable="false" />
                                            <ext:Column Header="Branch" Locked="true" Hideable="false" DataIndex="BankBranch"
                                                Align="Center" Css="font-weight:bold;border-width:1px; text-align:center; border-color: Black;"
                                                Width="140" MenuDisabled="true" Hidden="true" Sortable="false"/>
                                            <ext:Column Header="Check Number" Locked="true" Hideable="false" DataIndex="CheckNumber"
                                                Align="Center" Css="font-weight:bold; text-align:center; border-color: Black;"
                                                Width="95" MenuDisabled="true" Sortable="false" />
                                            <ext:Column Header="Transaction Date" Locked="true" Hideable="false" DataIndex="_TransactionDate"
                                                Align="Center" Css="font-weight:bold; text-align:center; border-color: Black;"
                                                Width="95" MenuDisabled="true" Sortable="false" />
                                            <ext:NumberColumn Header="Amount Applied" Locked="true" Hideable="false" DataIndex="AmountApplied"
                                                Align="Center" Css="font-weight:bold; text-align:center; border-color: Black;"
                                                Width="135" Format=",000.00" MenuDisabled="true" Sortable="false" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr><td class="style35"><br /></td></tr>
            <tr>
                <td class="style35">
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td style="width: 47px;"><b>Change</b></td>
                            <td style="width: 345px;">___________________________________________________________</td>
                            <td>&nbsp;₱&nbsp;</td>
                            <td style="text-align: right; width: 75px; border-bottom: 1px solid Black;"><center><b><asp:Label ID="lblChange" runat="server" /></b></center></td>
                        </tr>
                     </table>
                </td>
            </tr>
            <tr>
                <td class="style35">
                    <br /><br />
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td></td>
                            <td style="width: 355px; text-align: right;">Received From:</td>
                            <td style="width: 205px; border-bottom: 1px solid Black;">
                                <center><ext:Label ID="lblCustomerSignature" runat="server" StyleSpec="font-weight: bold;"/></center>
                            </td>
                        </tr>
                     </table>
                     <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td style="width: 320px;"></td>
                            <td>(Printed Name and Signature of Borrower)</td>                
                        </tr>
                     </table>
                </td>
            </tr>
            <tr><td class="style35"><hr /></td></tr>
            <tr>
                <td class="style35">
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td style="text-align: left; width: 186px;">Endorsed By:<ext:Label ID="lblPreparedBy" FieldLabel="Endorsed By" runat="server" Hidden="true"/></td>
                            <td style="text-align: left; width: 186px;">Checked By:<ext:Label ID="lblCheckedBy" FieldLabel="Checked By" runat="server" Hidden="true"/></td>
                            <td  style="width: 187px; text-align: left;">Released By:<ext:Label ID="lblReleasedBy" FieldLabel="Released By" runat="server" Hidden="true"/></td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 186;"><br /><ext:Label ID="lblTeller" runat="server" StyleSpec="text-decoration:underline;" /></td>
                            <td style="text-align: center; width: 186;"><br /><ext:TextField ID="txtOfficer" runat="server" Width="170" EmptyText="Type Officer name here..." StyleSpec="text-decoration:underline; text-align: center;" Cls="txtfield"/></td>
                            <td style="text-align: center; width: 187;"><br /><ext:TextField ID="txtCashier" runat="server" Width="170" EmptyText="Type Cashier name here..." StyleSpec="text-decoration:underline; text-align: center;" Cls="txtfield"/></td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 186;">Teller</td>
                            <td style="text-align: center; width: 186;">Officer</td>
                            <td style="text-align: center; width: 187;">Cashier</td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr><td class="style35"><hr /></td></tr>
            <tr>
                <td class="style35">
                    <%--<table  style=" padding-left: 20px; padding-right: 20px;">--%>
                    <table>
                        <tr>
                            <td>Loan Balance(Owner)</td><td>:</td>
                            <td style="text-align: right;">&nbsp;&nbsp;₱&nbsp;</td>
                            <td><asp:Label ID="lblOwnerBalance" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Loan Balance(Co-Owner)</td><td>:</td>
                            <td style="text-align: right;">&nbsp;&nbsp;₱&nbsp;</td>
                            <td><asp:Label ID="lblCoOwnerBalance" runat="server" /></td>
                        </tr>
                        <tr>
                            <td><b>Total Loan Balance</b></td><td>:</td>
                            <td style="text-align: right;"><b>&nbsp;&nbsp;₱&nbsp;</b></td>
                            <td><b><asp:Label ID="lblLoanBalance" runat="server" /></b></td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
         </table>
         <table style="width:560px; margin: 0 auto;">
            <tr>
                <td style="width: 280px; text-align: left; padding-left: 5px;">
                    Cashier's Copy<br />
                    Customer's Copy<br />
                    File Copy
                </td>
                <td style="width: 280px; text-align: right;">
                   No: <ext:Label ID="lblControlNumber" StyleSpec="font-size:15px;" LabelWidth="20"
                    runat="server"></ext:Label> 
                </td>
            </tr>
         </table>
        </div>
    <%--END of NEW FORM--%>
    </form>
</body>
</html>
