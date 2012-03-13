<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintSelectedLoanApplication.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.PrintSelectedLoanApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Customer</title>
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
            window.print();
        }


    </script>
    <style type="text/css">
        @media screen
        {
            #toolBar { display: block; }
        }
        
        @media print
        {
            #toolBar
            {
                display: none;
            }
        }
        
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 12px;
            background-color: #fff !important;
        }
        .style3
        {
            width: 358px;
        }
        .style4
        {
            width: 356px;
        }
        .style6
        {
            width: 403px;
        }
        .style7
        {
            width: 264px;
        }
        .style9
        {
            width: 282px;
        }
        .style10
        {
            width: 411px;
        }
        .style11
        {
            width: 698px;
        }
        .cssClass .x-grid3-row
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-width: 1px;
            border-style: solid;
        }
        .cssClass .x-grid3-hd
        {
            border-right: 1px solid Black;
            border-bottom: 1px solid Black;
        }
        .cssClass .x-grid3-header
        {
            background-image: none;
            background-color: transparent;
        }
        .cssClass .x-grid3-hd-btn 
        {
            width: 0px;
        }
        .cssClass .x-toolbar
        {
            border: 1px solid Black;
            background-image: none;
            background-color: transparent;
        }
        .cssClass .x-grid3-viewport
        {
            border: 1px solid Black;
        }
        td.x-grid3-hd-over .x-grid3-hd-inner,
        td.sort-desc .x-grid3-hd-inner,
        td.sort-asc .x-grid3-hd-inner,
        td.x-grid3-hd-menu-open .x-grid3-hd-inner
        {
            background-image: none;
            background-color: transparent;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden runat="server" ID="hdnLoanApplicationId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnCustomerId"></ext:Hidden>
    <div id="toolBar">
        <ext:FormPanel ID="FormPanel1" runat="server" Layout="FitLayout">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                            <Listeners>
                                <Click Handler="printContent(PrintableContent);" />
                            </Listeners>
                        </ext:Button>
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
    <div id="PrintableContent">
        <table style="width: 700px; margin: 0 auto;">
            <%--HEADER--%>
            <tr class="heading">
                <td style="text-align: center;">
                    <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" /><br />
                    <asp:Label ID="lblStreetAddress" runat="server" />
                    <asp:Label ID="lblBarangay" runat="server" /><br />
                    <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />,
                    <asp:Label ID="lblProvince" runat="server" />,
                    <asp:Label ID="lblCountry" runat="server" />,
                    <asp:Label ID="lblPostalCode" runat="server" /><br />
                    tel#:
                    <asp:Label ID="lblPrimTelNumber" runat="server" />,
                    <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                    fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                    <asp:Label ID="lblEmailAddress" runat="server" />
                    <br />
                </td>
            </tr>
            <tr class="nameOfDocument">
                <td style="text-align: center; font-weight: bold;">
                    <br />Loan Application Form<br />
                    <br /><br />
                </td>
            </tr>
            <%--BASIC INFORMATION--%>
            <tr class="BorrowersBasicInformation">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b>Borrower's Basic Information</b><br /><br /><br />
                                <table>
                                    <tr><td class="style9">Name: <asp:Label ID="lblCustomerName" runat="server" /><br /><br /></td></tr>
                                    <tr>
                                        <td class="style9">District: <asp:Label ID="lblDistricts" runat="server" /><br /><br /></td>
                                        <td class="style9">Station Number: <asp:Label ID="lblStation" runat="server"/><br /><br /></td>
                                    </tr>
                                    <tr>
                                        <td class="style9">Gender: <asp:Label ID="lblGender" runat="server" /><br /><br /></td>
                                        <td class="style9">Age: <asp:Label ID="lblAge" runat="server"/><br /><br /></td>
                                    </tr>
                                    <tr><td>Credit Limit: <asp:Label ID="lblCreditLimit" runat="server"/><br /><br /></td></tr>
                                </table>
                            </td>
                            <td id="AccountOwnerImage" style="text-align: right;">
                                <%--<ext:Image runat="server" ID="imgAccountOwner" Width="200" Height="200" ImageUrl="C:\Users\Agent-3\Desktop\balbasur.gif"></ext:Image>--%>
                                <%--<img src="../../../Uploaded/Images/pnam.jpg" alt="Alternate Text" width="200" height="200"/>--%>
                                <ext:Image ID="imgPersonPicture" runat="server" Width="200" Height="200" ImageUrl="..\..\Resources\images\noimage.jpg"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--CONTACT INFORMATION--%>
            <tr class="BorrowersContactInformation">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;" class="style11">
                                <b>Borrower's Contact Information</b><br /><br /><br />
                                Primary Home Address: <asp:Label ID="lblPrimaryHomeAddress" runat="server"/><br /><br />
                                <table>
                                    <tr>
                                        <td class="style10">Cellphone Number: <asp:Label runat="server" ID="lblCellphoneNumber"/><br /><br /></td>
                                        <td class="style3">Telephone Number: <asp:Label runat="server" ID="lblTelephoneNumber"/><br /><br /></td>
                                    </tr>
                                    <tr><td class="style10">Primary E-mail Address: <asp:Label runat="server" ID="lblPrimaryEmailAddress"/><br /><br /></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--LOAN APPLICATION DETAILS--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br /><br />Loan Application Details</b><br /><br /><br />
                                <table>
                                    <tr><td>Loan Application Id: <asp:Label runat="server" ID="lblApplicationId"/><br /><br /></td></tr>
                                    <tr><td>Application Date: <asp:Label runat="server" ID="lblApplicationDate"/><br /><br /></td></tr>
                                    <tr><td>Loan Application Status: <asp:Label runat="server" ID="lblApplicationStatus"/><br /><br /></td></tr>
                                    <tr><td>Status Comment:<asp:Label runat="server" ID="lblStatusComment"/><br /><br /></td></tr>
                                    <tr><td>Loan Product Name: <asp:Label runat="server" ID="lblProductName"/><br /><br /></td></tr>
                                    <tr><td>Loan Amount (Php): <asp:Label runat="server" ID="lblLoanAmount"/><br /><br /></td></tr>
                                    <tr><td>Loan Term: <asp:Label runat="server" ID="lblLoanTerm"/><br /><br /></td></tr>
                                    <tr><td>Collateral Requirement: <asp:Label runat="server" ID="lblCollateralRequirement"/><br /><br /></td></tr>
                                    <tr><td>Interest Computation Mode: <asp:Label runat="server" ID="lblInterestComputation"/><br /><br /></td></tr>
                                    <tr><td>Payment Mode: <asp:Label runat="server" ID="lblPaymentMode"/><br /><br /></td></tr>
                                    <tr><td>Method of Charging Interest: <asp:Label runat="server" ID="lblMethodOfChargingInterest"/><br /><br /></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--OUTSTANDING LOANS TO PAY OFF--%>
            <%--<tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;"><br /><br /><br /><br /><br />
                                Pay the following outstanding loan/s using the proceeds of the new loan:<br /><br />
                                <b>Outstanding Loan/s to Pay-Off</b><br /><br />
                                <ext:GridPanel ID="panelPayOutstandingLoan" runat="server" AutoHeight="true" Width="700" ColumnLines="true" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store runat="server" ID="storePayOutstandingLoan" RemoteSort="false">
                                            <Listeners>
                                                <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                            </Listeners>
                                            <Reader>
                                                <ext:JsonReader IDProperty="LoanId">
                                                    <Fields>
                                                        <ext:RecordField Name="LoanId" />
                                                        <ext:RecordField Name="LoanProductName" />
                                                        <ext:RecordField Name="InterestComputationMode" />
                                                        <ext:RecordField Name="MaturityDate" Type="Date" />
                                                        <ext:RecordField Name="NoOfInstallments" />
                                                        <ext:RecordField Name="PaymentMode" />
                                                        <ext:RecordField Name="ScheduledAmortization" />
                                                        <ext:RecordField Name="LoanBalance" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionOutstandingLoan" SingleSelect="false" runat="server">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <ColumnModel runat="server" ID="clmModelPayOutstandingLoan" Width="100%">
                                        <Columns>
                                            <ext:Column Header="ID" DataIndex="LoanId" Wrap="true" Locked="true" Width="30" Sortable="false" Hidden="true"
                                                Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;"/>
                                            <ext:Column Header="Product Name" DataIndex="LoanProductName" Locked="true"
                                                Wrap="true" Width="120" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Interest Computation" DataIndex="InterestComputationMode"
                                                Locked="true" Wrap="true" Width="145" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:DateColumn Header="Maturity Date" DataIndex="MaturityDate" Locked="true" Wrap="true"
                                                Width="80" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Installments" DataIndex="NoOfInstallments" Wrap="true"
                                                Locked="true" Width="70" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Payment Mode" DataIndex="PaymentMode" Locked="true" Wrap="true"
                                                Width="80" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:NumberColumn Header="Scheduled Amortization" DataIndex="ScheduledAmortization"
                                                Locked="true" Wrap="true" Width="125" Format="0.00" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:NumberColumn Header="Balance" DataIndex="LoanBalance" Locked="true" Wrap="true"
                                                Width="77" Format="0.00" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <%--INTEREST RATES--%>
            <tr class="InterestRates">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <br /><br /><b>Interest Description</b><br /><br />
                                <table>
                                    <tr><td>&nbsp;&nbsp;&nbsp;Interest Rate Description: <asp:Label runat="server" ID="lblInterestRateDesc" /><br /><br /></td></tr>
                                    <tr><td>&nbsp;&nbsp;&nbsp;Interest Rate: <asp:Label runat="server" ID="lblInterestRate" /><br /><br /></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--PAST DUE INTEREST RATES--%>
            <%--<tr class="PastDueRate">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                    <br /><b>Past Due Interest Rate</b><br /><br />
                                <table>
                                    <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;Past Due Rate Description: <asp:Label runat="server" ID="lblPastDueDesc" /><br /><br /></td></tr>
                                    <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;Past Due Rate: <asp:Label runat="server" ID="lblPastDueRate" /><br /><br /></td></tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <%--FEES--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b>Fee/s</b><br /><br />
                                    <ext:GridPanel ID="GridPanelFee" runat="server" AutoHeight="true" RowHeight=".5" AutoExpandColumn="Name" Width="600" ColumnLines="true" BaseCls="cssClass">
                                        <Store>
                                            <ext:Store ID="StoreFee" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ProductFeatureApplicabilityId" />
                                                            <ext:RecordField Name="Name" />
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="ChargeAmount" />
                                                            <ext:RecordField Name="BaseAmount" />
                                                            <ext:RecordField Name="Rate" />
                                                            <ext:RecordField Name="TotalChargePerFee" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelFee" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteFee);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelFee, btnDeleteFee);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel ID="ctl127">
                                            <Columns>
                                                <ext:Column runat="server" Header="Name" DataIndex="Name" Width="250"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                                </ext:Column>
                                                <ext:NumberColumn runat="server" Header="Fee Amount" DataIndex="Amount" Width="90" Format="0.00"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Charge Amount" DataIndex="ChargeAmount" Width="90" Format="0.00"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Base Amount" DataIndex="BaseAmount" Width="90" Format="0.00"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Rate" DataIndex="Rate" Width="70" Format="0"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                                </ext:NumberColumn>
                                                <ext:NumberColumn runat="server" Header="Total Charge Per Fee" DataIndex="TotalChargePerFee" Width="120" Format="0.00"
                                                    Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                                </ext:NumberColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <BottomBar>
                                            <ext:Toolbar runat="server" Width="600" ID="ctl128">
                                                <Items>
                                                    <ext:ToolbarFill ID="ctl129"/>
                                                    <ext:Label runat="server" ID="lblTotalFees" />
                                                </Items>
                                            </ext:Toolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--CO-BORROWERS--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br />Co-Borrower/s</b><br /><br />
                                <ext:GridPanel ID="GridPanelCoBorrower" AutoHeight="true" runat="server" RowHeight=".5"
                                    Width="600" ColumnLines="True" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store ID="StoreCoBorrower" runat="server">
                                            <Reader>
                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                    <Fields>
                                                        <ext:RecordField Name="PartyId" />
                                                        <ext:RecordField Name="Name" />
                                                        <ext:RecordField Name="Address" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Header="Name" DataIndex="Name" Width="260"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Primary Home Address" DataIndex="Address" Width="340"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--GUARANTORS--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br />Guarantor/s</b><br /><br />
                                <ext:GridPanel ID="GridPanelGuarantors" runat="server" AutoHeight="true" Width="600" ColumnLines="True"
                                    BaseCls="cssClass">
                                    <Store>
                                        <ext:Store ID="StoreGuarantor" runat="server">
                                            <Reader>
                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                    <Fields>
                                                        <ext:RecordField Name="PartyId" />
                                                        <ext:RecordField Name="Name" />
                                                        <ext:RecordField Name="Address" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Header="Name" DataIndex="Name" Width="260"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Primary Home Address" DataIndex="Address" Width="340"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--COLLATERALS--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br />Collateral/s</b><br /><br />
                                <ext:GridPanel ID="GridPanelCollaterals" runat="server" Width="600" AutoHeight="true" BaseCls="cssClass"
                                    ColumnLines="true">
                                    <Store>
                                        <ext:Store ID="StoreCollaterals" runat="server">
                                            <Reader>
                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                    <Fields>
                                                        <ext:RecordField Name="AssetID" />
                                                        <ext:RecordField Name="AssetTypeName" />
                                                        <ext:RecordField Name="Description" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Header="Collateral Type" DataIndex="AssetTypeName" Width="260"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Description" DataIndex="Description" Width="340"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--CHEQUES AS COLLATERAL--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br />Cheque/s as Collateral</b><br /><br />
                                <ext:GridPanel ID="grdpnlCheque" runat="server" Width="600" AutoHeight="true" AutoScroll="true"
                                    ColumnLines="true" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store ID="storeCheques" runat="server">
                                            <Reader>
                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                    <Fields>
                                                        <ext:RecordField Name="ChequeId" />
                                                        <ext:RecordField Name="ChequeNumber" />
                                                        <ext:RecordField Name="Amount" />
                                                        <ext:RecordField Name="ChequeDate" />
                                                        <ext:RecordField Name="BankName" />
                                                        <ext:RecordField Name="Status" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Header="Check Id" DataIndex="ChequeId" Width="150" Hidden="true"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Check Number" DataIndex="ChequeNumber" Width="150"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                            <ext:NumberColumn runat="server" Header="Amount" DataIndex="Amount" Width="95" Format=",000.00"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:NumberColumn>
                                            <ext:DateColumn runat="server" Header="Check Date" DataIndex="ChequeDate" Width="100" Format="MMM dd yyyy"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:DateColumn>
                                            <ext:Column runat="server" Header="Bank Name" DataIndex="BankName" Width="120"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Status" DataIndex="Status" Width="135"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--SUBMITTED DOCUMENTS--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br />Submitted Document/s</b><br /><br />
                                <ext:GridPanel ID="GridPanelSubmittedDocuments" runat="server" Width="600" AutoHeight="true" ColumnLines="true"
                                    BaseCls="cssClass">
                                    <Store>
                                        <ext:Store ID="StoreSubmittedDocuments" runat="server">
                                            <Reader>
                                                <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                    <Fields>
                                                        <ext:RecordField Name="SubmittedDocumentID" />
                                                        <ext:RecordField Name="ProductRequiredDocumentId" />
                                                        <ext:RecordField Name="ProductRequiredDocumentName" />
                                                        <ext:RecordField Name="DateSubmitted" />
                                                        <ext:RecordField Name="Status" />
                                                        <ext:RecordField Name="DocumentDescription" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <ColumnModel>
                                        <Columns>
                                            <ext:Column runat="server" Header="Document Name" DataIndex="ProductRequiredDocumentName" Width="220"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;">
                                            </ext:Column>
                                            <ext:DateColumn runat="server" Header="Date Submitted" DataIndex="DateSubmitted" Width="110"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;"
                                                Format="MMM dd yyyy">
                                            </ext:DateColumn>
                                            <ext:Column runat="server" Header="Status" DataIndex="Status" Width="100"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                            <ext:Column runat="server" Header="Description" DataIndex="DocumentDescription" Width="170"
                                                Wrap="true" Locked="true" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--REMARKS--%>
            <tr class="Remarks">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <b><br /><br /><br />Processed by: </b><asp:Label runat="server" ID="lblProcessedBy"/><br /><br /><br />
                                <b>Approved/Rejected by: </b><asp:Label runat="server" ID="lblApprovedBy"/><br /><br />
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
