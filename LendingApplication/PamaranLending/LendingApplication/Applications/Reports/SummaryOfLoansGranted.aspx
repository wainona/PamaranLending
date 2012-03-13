<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryOfLoansGranted.aspx.cs" Inherits="LendingApplication.Applications.Reports.SummaryOfLoansGranted" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Promisory Note</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
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
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 12px;
            background-color: #fff !important;
        }
        .box-component
        {
            width: 700px;
            height: 400px;
            border-color: Black;
            border-style: solid;
            border-width: 1px;
            padding: 5px 5px 5px 5px;
        }
        .header
        {
            font-size:medium;
        }
        #PrintableContent
        {
            text-align: center;
        }
        .container
        {
            padding: 0px 0px 0px 0px;
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
     
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden runat="server" ID="hdnLoanApplicationId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnCustomerId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnDate"></ext:Hidden>
    <div id="toolBar">
    <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Padding="20" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Label runat="server" Text="Report for: "></ext:Label>
                    <ext:ToolbarSpacer />
                    <%--<ext:ComboBox runat="server" ID="cmbMonths">
                        <Store>
                            <ext:Store runat="server" ID="strMonths">
                                <Reader>
                                    
                                </Reader>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>--%>
                    <ext:DateField runat="server" ID="dtMonthStart" Width="120" Format="F Y" 
                        TodayText="This Month" AllowBlank="false" Editable="false">
                        <Plugins>
                            <ext:MonthPicker runat="server"/>
                        </Plugins>
                    </ext:DateField>
                    <ext:ToolbarSpacer />
                    <%--<ext:DateField runat="server" ID="dtMonthEnd" Width="120" StartDateField="dtMonthStart" Editable="false">
                        <Plugins>
                            <ext:MonthPicker ID="MonthPicker2" runat="server"></ext:MonthPicker>
                        </Plugins>
                    </ext:DateField>
                    <ext:ToolbarSpacer />--%>
                    <ext:Button runat="server" Text="Generate Summary" Icon="FolderGo">
                        <DirectEvents>
                            <Click OnEvent="btnGenerate_Click" Before="#{hdnDate}.setValue(Ext.util.Format.date(#{dtMonthStart}.getValue()))"></Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                    <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                        <Listeners>
                            <Click Handler="printContent('PrintableContent');" />
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
    </ext:Panel>
    </div>
    <div id="PrintableContent">
        <table style="width: 800px; margin: 0 auto;">
            <%--HEADER--%>
            <tr class="heading">
                <td style="text-align: center; font-weight: bold;">
                    <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" CssClass="header" /><br />
                    <asp:Label ID="lblStreetAddress" runat="server" />
                    <asp:Label ID="lblBarangay" runat="server" />
                    <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />,
                    <asp:Label ID="lblProvince" runat="server" />,
                    <asp:Label ID="lblCountry" runat="server" />,
                    <asp:Label ID="lblPostalCode" runat="server" /><br />
                    Tel#:
                    <asp:Label ID="lblPrimTelNumber" runat="server" />,
                    <asp:Label ID="lblSecTelNumber" runat="server" />
                    Fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                    <asp:Label ID="lblEmailAddress" runat="server" />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr class="nameOfDocument">
                <td style="text-align: justify;">
                    TO: THE BOARD OF DIRECTORS<br />
                    <br />
                    FROM: THE MANAGER<br />
                    <br />
                    SUBJECT: &nbsp; <b>
                        <ext:Label ID="lblForm" runat="server" Text="LOANS GRANTED FOR THE PERIOD"></ext:Label>
                        <ext:Label ID="lblFormName" runat="server">
                        </ext:Label>
                    </b>
                    <br />
                    <br />
                    <br />
                    <center>
                        HEREINUNDER IS THE SCHEDUE OF LOANS GRANTED FOR YOUR CONFIRMATION:</center>
                </td>
            </tr>
            <%--LOANS GRANTED--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <ext:GridPanel ID="panelLoansGranted" runat="server" AutoHeight="true" Width="800"
                                    ColumnLines="true" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store runat="server" ID="strLoansGranted" RemoteSort="false">
                                            <Listeners>
                                                <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                            </Listeners>
                                            <Reader>
                                                <ext:JsonReader IDProperty="LoanApplicationId">
                                                    <Fields>
                                                        <ext:RecordField Name="LoanApplicationId" />
                                                        <ext:RecordField Name="BorrowersName" />
                                                        <ext:RecordField Name="LoanProduct" />
                                                        <ext:RecordField Name="LoanAmount" />
                                                        <ext:RecordField Name="InterestRate" />
                                                        <ext:RecordField Name="LoanTerm" />
                                                        <ext:RecordField Name="PaymentMode" />
                                                        <ext:RecordField Name="CollateralRequirement" />
                                                        <ext:RecordField Name="LoanBalance" />
                                                        <ext:RecordField Name="LoanAccountStatus"></ext:RecordField>
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionLoansGranted" SingleSelect="false" runat="server">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <ColumnModel runat="server" ID="clmModelLoansGranted" Width="100%">
                                        <Columns>
                                            <ext:Column Header="Name of Borrower" DataIndex="BorrowersName" Wrap="true" Locked="true" MenuDisabled="true" 
                                                Width="170" Sortable="false"  Hideable="false" Align="Left" Css="font-weight:bold; text-align:left; border-color: Black;"/>
                                            <ext:Column Header="Type of Loan" DataIndex="LoanProduct" Locked="true" Wrap="true" Hidden="true" MenuDisabled="true" 
                                                Width="107" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:NumberColumn Header="Loan Amount" DataIndex="LoanAmount" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="120" Format=",000.00" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;"/>
                                            <ext:Column Header="Interest Rate" DataIndex="InterestRate" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="80" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Loan Term" DataIndex="LoanTerm" Wrap="true" Locked="true" Width="70" MenuDisabled="true" 
                                                Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Payment Mode" DataIndex="PaymentMode" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="90" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Security" DataIndex="CollateralRequirement" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="90" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:NumberColumn Header="Balance" DataIndex="LoanBalance" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="70" Format=",000.00" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Current Loan Status" DataIndex="LoanAccountStatus" Locked="true" Wrap="true" MenuDisabled="true" 
                                                Width="107" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                                <br />
                                <br />
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