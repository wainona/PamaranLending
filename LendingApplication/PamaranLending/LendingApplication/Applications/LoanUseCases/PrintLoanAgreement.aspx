<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintLoanAgreement.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.PrintLoanAgreement" %>

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
        <ext:Panel ID="Panel1" runat="server" Border="false">
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
        <div id="PrintableDiv">
            <table style="width: 600px; margin: 0 auto; font-family: Tahoma;">
                <tr>
                    <td>
                        <center>
                            <asp:Label ID="lblHeaderLenderName" runat="server" Font-Bold="true"/><br />
                            <asp:Label ID="lblHeaderLenderStreetAddress" runat="server" />, <asp:Label ID="lblHeaderBarangay" runat="server" /><br />
                            <asp:Label ID="lblHeaderMunicipality" runat="server" /><asp:Label ID="lblHeaderCity" runat="server" />, <asp:Label ID="lblHeaderProvince" runat="server" />, <asp:Label ID="lblHeaderCountry" runat="server" />, <asp:Label ID="lblHeaderPostalCode" runat="server" /><br />
                            Tel# <asp:Label ID="lblHeaderLenderPrimPhone" runat="server" />, <asp:Label ID="lblHeaderLenderSecPhone" runat="server" /><br />
                            Fax# <asp:Label ID="lblHeaderLenderFaxNumber" runat="server" /><br />
                            <asp:Label ID="lblHeaderLenderEmail" runat="server" />
                        </center>
                    <br />
                    <br />
            <center><b>Loan Agreement</b></center>
            <br />
            <br />
            <asp:Table ID="tblBorrowersInformation" runat="server" BorderColor="Black" GridLines="Both" Width="675">
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="TableCell1" runat="server" Font-Bold="true" HorizontalAlign="Center">
                    Borrower's Information
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow2" runat="server">
                <asp:TableCell ID="TableCell2" runat="server">
                    <asp:Table ID="Table1" runat="server" GridLines="Both" Width="675">
                        <asp:TableRow ID="TableRow3" runat="server">
                            <asp:TableCell ID="TableCell3" runat="server" Width="332" VerticalAlign="Top">
                                Name:<br />
                                <asp:Label runat="server" ID="lblBorrowerName" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell4" runat="server" Width="333" VerticalAlign="Top">
                                Date:<br />
                                <asp:Label runat="server" ID="lblBorrowerDate" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow4" runat="server">
                            <asp:TableCell ID="TableCell5" runat="server" Width="332" VerticalAlign="Top">
                                Street Address:<br />
                                <asp:Label runat="server" ID="lblBorrowerStreetAddress" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell6" runat="server" Width="333" VerticalAlign="Top">
                                Date of Birth:<br />
                                <asp:Label runat="server" ID="lblBorrowerDateOfBirth" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow5" runat="server">
                            <asp:TableCell ID="TableCell7" runat="server" Width="332" VerticalAlign="Top">
                                City:<br />
                                <asp:Label runat="server" ID="lblBorrowerCity" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell8" runat="server" Width="333" VerticalAlign="Top">
                                Area Code/Telephone Number:<br />
                                <asp:Label runat="server" ID="lblBorrowerAreaCodeTelNum" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow6" runat="server">
                            <asp:TableCell ID="TableCell9" runat="server" Width="332" VerticalAlign="Top">
                                Province:<br />
                                <asp:Label runat="server" ID="lblBorrowerProvince" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell10" runat="server" Width="333" VerticalAlign="Top">
                                Driver's License Number:<br />
                                <asp:Label runat="server" ID="lblBorrowerDriversLicenseNum" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow7" runat="server">
                            <asp:TableCell ID="TableCell11" runat="server" Width="332" VerticalAlign="Top">
                                Zip Code:<br />
                                <asp:Label runat="server" ID="lblBorrowerZipCode" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell12" runat="server" Width="333" VerticalAlign="Top">
                                Social Security Number:<br />
                                <asp:Label runat="server" ID="lblBorrowerSSSNum" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <br />
            <br />
        <asp:Table ID="tblLenderInformation" runat="server" BorderColor="Black" GridLines="Both" Width="675" CssClass="noPadding">
            <asp:TableRow ID="TableRow8" runat="server">
                <asp:TableCell ID="TableCell13" runat="server" Font-Bold="true" HorizontalAlign="Center">
                    Lender Information
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow9" runat="server">
                <asp:TableCell ID="TableCell14" runat="server" CssClass="noPadding">
                    <asp:Table ID="Table2" runat="server" GridLines="Both" Width="675">
                        <asp:TableRow ID="TableRow11" runat="server">
                            <asp:TableCell Width="333" VerticalAlign="Top">
                                Name:<br />
                                <asp:Label runat="server" ID="lblLenderName" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell17" runat="server" Width="333" VerticalAlign="Top">
                                Area code/Telephone Number:<br />
                                <asp:Label runat="server" ID="lblLenderAreaCodeTelephoneNumber" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow12" runat="server">
                            <asp:TableCell Width="332" VerticalAlign="Top">
                                Street Address:<br />
                                <asp:Label runat="server" ID="lblLenderStreetAddress" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell18" runat="server" Width="333" VerticalAlign="Top">
                                If paying by check, make check payable to:<br />
                                <asp:Label runat="server" ID="lblLenderNameForCheck" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow13" runat="server">
                            <asp:TableCell Width="332" VerticalAlign="Top">
                                City:<br />
                                <asp:Label runat="server" ID="lblLenderCity" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow14" runat="server">
                            <asp:TableCell Width="333" VerticalAlign="Top">
                                Province:<br />
                                <asp:Label runat="server" ID="lblLenderState" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow15" runat="server">
                            <asp:TableCell Width="332" VerticalAlign="Top">
                                Zip:<br />
                                <asp:Label runat="server" ID="lblLenderZip" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <br />
            <br />
        <asp:Table ID="tblLoanInformation" runat="server" BorderColor="Black" GridLines="Both" Width="675">
            <asp:TableRow ID="TableRow18" runat="server">
                <asp:TableCell ID="TableCell19" runat="server" Font-Bold="true" HorizontalAlign="Center">
                    Loan Information
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow19" runat="server">
                <asp:TableCell ID="TableCell20" runat="server">
                    <asp:Table ID="Table5" runat="server" GridLines="Both" Width="675">
                        <asp:TableRow ID="TableRow20" runat="server">
                            <asp:TableCell ID="TableCell21" runat="server" Width="332" VerticalAlign="Top">
                                Loan Amount:<br />
                                <ext:Label runat="server" ID="lblLoanAmount" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell22" runat="server" Width="333" VerticalAlign="Top">
                                Loan Period:<br />
                                <asp:Label runat="server" ID="lblLoanPeriod" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow21" runat="server">
                            <asp:TableCell ID="TableCell23" runat="server" Width="332" VerticalAlign="Top">
                                Interest Rate:<br />
                                <asp:Label runat="server" ID="lblInterestRate" />
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell24" runat="server" Width="333" VerticalAlign="Top">
                                Past Due Interest Rate:<br />
                                <asp:Label runat="server" ID="lblPastDueInterestRate" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <br />
            <br />
            <center><b><asp:Label ID="lblAmortizationSchedLoanAgreement" runat="server" /></b></center>
            <asp:GridView ID="grdAmortScheduleLoanAgreement" runat="server" Font-Size="Small" AutoGenerateColumns="False" 
                        BorderColor="#999999" BackColor="White" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                        GridLines="Both" Width="675">
            <Columns>
                <asp:BoundField DataField="ScheduledPaymentDate" HeaderText="Scheduled Payment Date" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="PrincipalPayment" HeaderText="Principal Payment" DataFormatString="{0:N}"/>
                <asp:BoundField DataField="InterestPayment" HeaderText="Interest Payment" DataFormatString="{0:N}"/>
                <asp:BoundField DataField="TotalPayment" HeaderText="Total Payment" DataFormatString="{0:N}"/>
                <asp:BoundField DataField="PrincipalBalance" HeaderText="Principal Balance" DataFormatString="{0:N}"/>
                <%--<asp:BoundField DataField="TotalLoanBalance" HeaderText="Total Loan Balance" DataFormatString="{0:N}" />--%>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="White" Font-Bold="true" ForeColor="Black" HorizontalAlign="Center" />
            <EmptyDataRowStyle />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
            <br />
            <br />
            <div style="text-align: justify;">
                <br /><br />1. <b>Promise to Pay.</b> For value received, <asp:Label ID="lblpBorrowerName" runat="server" Font-Underline="true"/> (Borrower) promises to pay <asp:Label ID="lblpLenderName" runat="server" Font-Underline="true"/> (Lender) <asp:Label ID="lblpLoanAmount" runat="server" Font-Underline="true"/> (Php) and interest at the rate of <asp:Label ID="lblpInterestValPlusProdFeatName" runat="server"/>. 
                <br /><br />2. <b>Installments.</b> <br /><br />
                    <asp:Label ID="lblInstallments" runat="server"/> 
                <br /><br />3. <b>Application of Payments.</b> Payments will be applied first to interest and then to principal.
                <br /><br />4. <b>Prepayment.</b> Borrower may prepay all or any part of the principal without penalty.
                <br /><br />5. <b>Security.</b> <br /><br />
                                <asp:Checkbox ID="chkUnsecuredNote" runat="server" /> This is an unsecured note. <br />
                                <asp:Checkbox ID="chkSecuredByCollateral" runat="server" /> Borrower agrees that until the principal and interest owed under this promissory note are paid in full, this note will be secured by the collateral enumerated in the loan application.

                <br /><br />6. <b>Collection Costs.</b> If Lender prevails in a lawsuit to collect on this note, Borrower will pay Lender's costs and lawyer's fees in an amount the court finds to be reasonable.
                <br /><br />The undersigned and all other parties to this note, whether as endorsers, guarantors or sureties, agree to remain fully bound until this note shall be fully paid and waive demand, presentment and protest and all notices hereto and further agree to remain bound notwithstanding any extension, modification, waiver, or other indulgence or discharge or release of any obligor hereunder or exchange, substitution, or release of any collateral granted as security for this note. No modification or indulgence by any holder hereof shall be binding unless in writing; and any indulgence on any one occasion shall not be an indulgence for any other or future occasion. Any modification or change in terms, hereunder granted by any holder hereof, shall be valid and binding upon each of the undersigned, notwithstanding the acknowledgement of any of the undersigned, and each of the undersigned does hereby irrevocably grant to each of the others a power of attorney to enter into any such modification on their behalf. The rights of any holder hereof shall be cumulative and not necessarily successive. This note shall take effect as a sealed instrument and shall be construed, governed and enforced in accordance with the laws of the Republic of the Philippines.
                <br /><br />
                <center>
                Witnessed: ________________________  Date: ______________<br /><br />
                    
                Witnessed: ________________________  Date: ______________<br /><br />
                    
                Borrower: &nbsp;________________________  Date: ______________<br /><br />
                    
                Borrower: &nbsp;________________________  Date: ______________
                <//center>
                <br /><br />
            </div>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
    </form>
</body>
</html>
