<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintLoanApplicationRecord.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.PrintLoanApplicationRecord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Loan Record</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            PageToolBar.hide();
            window.print();
            PageToolBar.show();
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
            width: 369px;
        }
        .style2
        {
            width: 113px;
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
        <ext:FormPanel runat="server" Border="false">
        <TopBar>
        <ext:Toolbar runat="server">
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
            <table style="width:700px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;">
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" />, <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align:center; font-weight:bold;">
                        Loan Application Form
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr id="BorrowersBasicInformationLabel">
                    <td>
                        <b>Borrower's Basic Information</b><br /><br />
                    </td>
                </tr>
                <tr class="BorrowersBasicInformation">
                    <td style="text-align:justify;">
                        <table>
                            <tr style="vertical-align:top;">
                                <td style="text-align:left;" class="style2">
                                    Name: <br />
                                    District: <br />
                                    Station Number: <br />
                                    Customer Status: <br />
                                    Gender:<br />
                                    Age: <br />
                                    Credit Limit: <br />
                                    
                                </td>
                                <td style="text-align:left;" class="style1">
                                    <asp:Label ID="lblAccountOwner" runat="server" /><br />
                                    <asp:Label ID="lblDistrictOwner" runat="server" /><br />
                                    <asp:Label ID="lblStationNumberOwner" runat="server" /><br />
                                    <asp:Label ID="lblAddressOwner" runat="server" /><br />
                                    <asp:Label ID="lblCellPhoneNumberOwner" runat="server" /><br />
                                    <asp:Label ID="lblPrimTelNumberOwner" runat="server" /><br />
                                    <asp:Label ID="lblEmailAddressOwner" runat="server" /><br />
                                </td>
                                <td id="AccountOwnerImage" style="text-align: right;">
                                    <%--<ext:Image runat="server" ID="imgAccountOwner" Width="200" Height="200" ImageUrl="C:\Users\Agent-3\Desktop\balbasur.gif"></ext:Image>--%>
                                    <img src="../../../Uploaded/Images/pnam.jpg" alt="Alternate Text" width="200" height="200"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="BorrowersContactInformationLabel">
                    <td>
                        <b>Borrower's Contact Information</b><br /><br />
                    </td>
                </tr>
                <tr id="BorrowersContactInformation">
                    <td>
                        <table>
                            <tr>
                                <td style="text-align:left;">
                                    Primary Home Address: <br />
                                    Cellphone Number: <br />
                                    Telephone Number: <br />
                                    Primary Email Address: <br />
                                </td>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblPrimHomeAddress" runat="server" /><br />
                                    <asp:Label ID="lblCellNumber" runat="server" /><br />
                                    <asp:Label ID="lblTelNumber" runat="server" /><br />
                                    <asp:Label ID="lblPrimEmailAddress" runat="server" /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="LoanApplicationDetailsLabel">
                    <td>
                        <br /><br />
                        <b>Loan Application Details</b>
                        <br /><br />
                    </td>
                </tr>
                <tr id="LoanApplicationDetails">
                    <td>
                        <table>
                            <tr>
                                <td style="text-align:left;">
                                    Loan Application ID: <br />
                                    Application Date: <br />
                                    Loan Application Status: <br />
                                    Status Comment: <br />
                                    Loan Product Name: <br />
                                    Loan Amount (Php): <br />
                                    Loan Term (Php): <br />
                                    Loan Purpose (Php): <br />
                                    Collateral Requirement: <br />
                                    Interest Computation Mode: <br />
                                    Payment Mode: <br />
                                    Method of Charging Interest: <br />
                                </td>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblLoanApplicationID" runat="server" /><br />
                                    <asp:Label ID="lblApplicationDate" runat="server" /><br />
                                    <asp:Label ID="lblLoanApplicationStatus" runat="server" /><br />
                                    <asp:Label ID="lblStatusComment" runat="server" /><br />
                                    <asp:Label ID="lblLoanProductName" runat="server" /><br />
                                    <asp:Label ID="lblLoanAmount" runat="server" /><br />
                                    <asp:Label ID="lblLoanTerm" runat="server" /><br />
                                    <asp:Label ID="lblLoanPurpose" runat="server" /><br />
                                    <asp:Label ID="lblCollateralRequirement" runat="server" /><br />
                                    <asp:Label ID="lblInterestComputationMode" runat="server" /><br />
                                    <asp:Label ID="lblPaymentMode" runat="server" /><br />
                                    <asp:Label ID="lblMethodOfChargingInterest" runat="server" /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="PaymentHistory">
                    <td style="text-align:justify;">
                        <br /><br />
                        <b>Payment History</b><br /><br />
                        <asp:GridView runat="server" ID="grdPaymentHisotry" Font-Size="Small">
                            <Columns>
                                <asp:BoundField DataField="Date" HeaderText="Financial Account Role"/>
                                <asp:BoundField DataField="PaymentID" HeaderText="Name" />
                                <asp:BoundField DataField="Amount" HeaderText="Cellphone Number" />
                                <asp:BoundField DataField="CollectedBy" HeaderText="Telephone Number" />
                            </Columns>
                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
