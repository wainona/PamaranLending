<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintLoanRecord.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.PrintLoanRecord" %>

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
            width: 465px;
        }
        .style2
        {
            width: 227px;
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
        }
        .cssClass .x-grid3-header
        {
            background-image: none;
            background-color: transparent;
        }
        
        .cssClass .x-grid3-hd-row td : hover
        {
            background-image: none;
            background-color: White;
        }
        .style3
        {
            width: 200px;
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
        <ext:Panel runat="server" Border="false">
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
        </ext:Panel>
        </div>
        <div id="PrintableContent">
            <table style="width:800px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;">
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align:center; font-weight:bold;">
                        Loan Record
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr class="AccountOwnersBasicInformationLabel">
                    <td>
                        <b>Account Owner's Basic Information</b><br /><br />
                    </td>
                </tr>
                <tr class="AccountOwnersBasicInformation">
                    <td style="text-align:justify;">
                        <table>
                            <tr style="vertical-align:top;">
                                <td style="text-align:left;">
                                    Account Owner: <br /><br />
                                    District: <br /><br />
                                    Station Number: <br /><br />
                                    Address: <br /><br />
                                    Cellphone Number: <br /><br />
                                    Telephone Number: <br /><br />
                                    Email Address: <br /><br />
                                </td>
                                <td style="text-align:left;" class="style1">
                                    <asp:Label ID="lblAccountOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblDistrictOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblStationNumberOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblAddressOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblCellPhoneNumberOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblPrimTelNumberOwner" runat="server" /><br /><br />
                                    <asp:Label ID="lblEmailAddressOwner" runat="server" /><br /><br />
                                </td>
                                <td id="AccountOwnerImage" style="text-align: right;" class="style3">
                                    <ext:Image ID="imgPersonPicture" runat="server" Width="200" Height="200" ImageUrl="../../Resources/images/noimage.jpg" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="CoOwnerGuarantors">
                    <td>
                        <br /><br />
                        <b>Co-Owner/Guarantors</b><br /><br />
                        <ext:GridPanel runat="server" AutoExpandColumn="PrimaryHomeAddress" AutoHeight="true" BaseCls="cssClass" ColumnLines="true"
                            Width="800" EnableColumnHide="false" EnableColumnMove="false">
                            <Store>
                                <ext:Store ID="grdCoOwnerGuarantors" runat="server">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="FinancialAccountRole" />
                                                <ext:RecordField Name="Name" />
                                                <ext:RecordField Name="CellphoneNumber" />
                                                <ext:RecordField Name="TelephoneNumber" />
                                                <ext:RecordField Name="PrimaryHomeAddress" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column Header="Account Role" DataIndex="FinancialAccountRole" Width="100" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:Column Header="Name" DataIndex="Name" Width="150" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:Column Header="Cellphone Number" DataIndex="CellphoneNumber" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:Column Header="Telephone Number" DataIndex="TelephoneNumber" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:Column Header="Home Address" DataIndex="PrimaryHomeAddress" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                        <br /><br />
                    </td>
                </tr>
                <tr id="LoanAccountDetailsLabel">
                    <td>
                        <b>Loan Account Details</b><br /><br />
                    </td>
                </tr>
                <tr id="LoanAccountDetails">
                    <td>
                        <table>
                            <tr>
                                <td style="text-align:left;" class="style2">
                                    Loan ID: <br /><br />
                                    Loan Release Date: <br /><br />
                                    Loan Term: <br /><br />
                                    Maturity Date: <br /><br />
                                    Loan Status: <br /><br />
                                    Status Comment: <br /><br />
                                    Loan Application ID: <br /><br />
                                    Loan Product Name: <br /><br />
                                    Payment Mode: <br /><br />
                                    Interest Computation Mode: <br /><br />
                                    Interest: <br /><br />
                                    Loan Amount (Php): <br /><br />
                                    Add On Interest: <br /><br />
                                    Total Loan: <br /><br />
                                    Method of Charging Interest: <br /><br />
                                    <asp:Label ID="lblAmortizationLabel" runat="server" /> Amortization: <br /><br />
                                </td>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblLoanID" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanReleaseDate" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanTerm" runat="server" /><br /><br />
                                    <asp:Label ID="lblMaturityDate" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanStatus" runat="server" /><br /><br />
                                    <asp:Label ID="lblStatusComment" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanApplicationID" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanProductName" runat="server" /><br /><br />
                                    <asp:Label ID="lblPaymentMode" runat="server" /><br /><br />
                                    <asp:Label ID="lblInterestComputationMode" runat="server" /><br /><br />
                                    <asp:Label ID="lblInterest" runat="server" /><br /><br />
                                    <asp:Label ID="lblLoanAmount" runat="server" /><br /><br />
                                    <asp:Label ID="lblAddOnInterest" runat="server" /><br /><br />
                                    <asp:Label ID="lblTotalLoan" runat="server" /><br /><br />
                                    <asp:Label ID="lblMethodOfChargingInterest" runat="server" /><br /><br />
                                    <asp:Label ID="lblAmortization" runat="server" /><br /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="AmortizationSchedule">
                    <td>
                        <br /><br />
                        <b>Amortization Schedule</b><br /><br />
                        <ext:GridPanel ID="GridPanel1" runat="server" Width="800" AutoHeight="true" BaseCls="cssClass" ColumnLines="true">
                            <Store>
                                <ext:Store ID="grdAmortizationSchedule" runat="server">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="ScheduledPaymentDate" Type="Date"/>
                                                <ext:RecordField Name="PrincipalPayment" />
                                                <ext:RecordField Name="InterestPayment" />
                                                <ext:RecordField Name="TotalPayment" />
                                                <ext:RecordField Name="PrincipalBalance" />
                                                <ext:RecordField Name="TotalLoanBalance" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:DateColumn Header="Scheduled Payment Date" DataIndex="ScheduledPaymentDate" Width="156" Format="MMMM dd, yyyy" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Principal Payment" DataIndex="PrincipalPayment" Width="156" Format=",000.00" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Interest Payment" DataIndex="InterestPayment" Width="156" Format=",000.00" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Total Payment" DataIndex="TotalPayment" Width="156" Format=",000.00" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Width="156" Format=",000.00" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </td>
                </tr>
                <tr id="PaymentHistory">
                    <td style="text-align:justify;">
                        <br /><br />
                        <b>Loan History</b><br /><br />
                        <ext:GridPanel ID="GridPanel2" runat="server" Width="800" AutoHeight="true" BaseCls="cssClass" ColumnLines="true">
                            <Store>
                                <ext:Store ID="grdPaymentHisotry" runat="server">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                    <ext:RecordField Name="_Date"/>
                                                    <ext:RecordField Name="Date"/>
                                                    <ext:RecordField Name="Disbursement" />
                                                    <ext:RecordField Name="Interest" />
                                                    <ext:RecordField Name="Payment"></ext:RecordField>
                                                    <ext:RecordField Name="WaiveOrRebate"></ext:RecordField>
                                                    <ext:RecordField Name="Remarks"></ext:RecordField>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <ext:Column Header="Date" DataIndex="Date" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Disbursement" DataIndex="Disbursement" Format=",000.00" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Interest" DataIndex="Interest" Format=",000.00" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Payment" DataIndex="Payment" Format=",000.00" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:NumberColumn Header="WaiveOrRebate" DataIndex="WaiveOrRebate" Format=",000.00" Width="120" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                    <ext:Column Header="Remarks" DataIndex="Remarks" Width="180" Css="border-color: Black;" 
                                            Sortable="false" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
