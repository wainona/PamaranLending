<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleOfOutstandingLoans.aspx.cs" Inherits="LendingApplication.Applications.Reports.ScheduleOfOutstandingLoans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Promissory Note</title>
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

        var FillTotal = function () {
            var loanBalance = 0;
            for (var j = 0; j < panelLoansGranted.store.getCount(); j++) {
                var data = panelLoansGranted.store.getAt(j);
                loanBalance += data.get('LoanBalance');
            }
            txtBalance.setValue(loanBalance);
            formatCurrency(txtBalance);
            lblTotal.setText(txtBalance.getValue());
        }

        var formatCurrency = function (txt) {
            var num = txt.getValue();
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
            var answer = (((sign) ? '' : '-') + num + '.' + cents);
            txt.setValue(String(answer));
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
        .cssClass .x-grid3-row
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-width: 1px;
            border-style: solid;
        }
        .cssClass .x-grid3-hd
        {
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
        .underline
        {
            text-decoration: underline;
            font-weight: bold;
        }
        .style1
        {
            width: 537px;
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
    <ext:Hidden runat="server" ID="hdnTotal"></ext:Hidden>
    <div id="toolBar">
    <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Padding="20" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Label ID="lblDateGen" runat="server" Text="Start Date:"></ext:Label>
                    <ext:ToolbarSpacer />
                    <ext:DateField runat="server" ID="dtMonthStart" Width="120" EndDateField="dtMonthEnd" Editable="false">
                    </ext:DateField>
                    <ext:ToolbarSpacer />
                    <ext:Button ID="Button1" runat="server" Text="Generate Schedule" Icon="FolderGo">
                        <DirectEvents>
                            <Click OnEvent="btnGenerate_Click" Success="FillTotal();"></Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
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
                </td>
            </tr>
            <tr class="nameOfDocument">
                <td>
                    <center><b>
                        <br />
                        <ext:Label runat="server" Text="SCHEDULE OF OUTSTANDING LOANS AS OF: "></ext:Label>
                        <ext:Label runat="server" ID="lblDates"></ext:Label>
                    </b></center>
                </td>
            </tr>
            <%--LOANS GRANTED--%>
            <tr class="LoanApplicationDetails">
                <td style="text-align: justify;">
                    <ext:GridPanel ID="panelLoansGranted" runat="server" AutoHeight="true" Width="800"
                        ColumnLines="true" BaseCls="cssClass">
                        <LoadMask ShowMask="true" Msg="Loading Outstanding Loans..." />
                        <Store>
                            <ext:Store runat="server" ID="strOutstandingLoans" RemoteSort="false">
                                <Reader>
                                    <ext:JsonReader IDProperty="LoanId">
                                        <Fields>
                                            <ext:RecordField Name="LoanId" />
                                            <ext:RecordField Name="Name" />
                                            <ext:RecordField Name="LoanProduct" />
                                            <ext:RecordField Name="LoanAmount" />
                                            <ext:RecordField Name="InterestRate" />
                                            <ext:RecordField Name="LoanReleaseDate" Type="date" />
                                            <ext:RecordField Name="LoanTerm" />
                                            <ext:RecordField Name="MaturityDate" />
                                            <ext:RecordField Name="_LoanReleaseDate" />
                                            <ext:RecordField Name="_MaturityDate" />
                                            <ext:RecordField Name="LoanBalance" />
                                            <ext:RecordField Name="Status" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ColumnModel runat="server" ID="clmModelLoansGranted" Width="100%">
                            <Columns>
                                <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" Width="190" MenuDisabled="true"
                                    Sortable="false" Hideable="false" Align="Center" Css="font-weight:bold; text-align:left; border-color: Black;" />
                                <ext:Column Header="Type" DataIndex="LoanProduct" Locked="true" Wrap="true" Width="107" MenuDisabled="true"
                                    Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                <ext:NumberColumn Header="Principal Amount" DataIndex="LoanAmount" Locked="true" MenuDisabled="true"
                                    Wrap="true" Width="90" Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                    Css="border-color: Black;" />
                                <ext:NumberColumn Header="Interest Rate" DataIndex="InterestRate" Locked="true" Wrap="true" MenuDisabled="true"
                                    Width="80" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;"
                                    Format="0.00%"/>
                                <ext:Column Header="Date Granted" DataIndex="_LoanReleaseDate" Locked="true" Wrap="true" MenuDisabled="true"
                                    Width="80" Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                <ext:Column Header="Loan Term" DataIndex="LoanTerm" Wrap="true" Locked="true" Width="70" MenuDisabled="true"
                                    Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                                <ext:NumberColumn Header="Balance" DataIndex="LoanBalance" Locked="true" Wrap="true" MenuDisabled="true"
                                    Width="70" Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                    Css="border-color: Black;" />
                                <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="80" MenuDisabled="true"
                                    Sortable="false" Hideable="false" Align="Center" Css="border-color: Black;" />
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <br />
                    <table align="center">
                        <tr>
                            <td class="style1"><b><ext:Label runat="server" Text="TOTAL OUTSTANDING LOANS"></ext:Label></b></td>
                            <td style="text-align: left"><b>
                                <ext:Label runat="server" ID="lblTotal" Cls="underline" FieldLabel="₱" LabelWidth="10">
                                </ext:Label>
                                <ext:TextField runat="server" Hidden="true" ID="txtBalance"></ext:TextField>
                            </b></td>
                        </tr>
                    </table>
                    <br /><br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
