<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactionReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.TransactionReport" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link rel="stylesheet" type="text/css" href="../../Resources/css/main.css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">

        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var gridPanelReload = function () {
            var value = dtStartDate.getValue();
            if (value) {
                TransactionReportGridPanel.reload();
                btnPrint.enable();
            } else {
                showAlert('Alert', 'Please fill in the start date and end date fields.');
            }
        };

        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
            //PageToolBar.show();
        }
    </script>
    <style type="text/css">
        .bold 
        {
            font-size: medium;
            font-weight: bold;
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
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <div id="toolBar">
    <ext:FormPanel ID="MainPanel" runat="server" Border="false" MonitorValid="true" MonitorPoll="500">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Label runat="server" Text="Transaction Classification:" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:ComboBox ID="cmbTransactionClassification" runat="server" Width="150" Editable="false" AllowBlank="false">
                        <Items>
                            <ext:ListItem Text="All" Value="All" />
                            <ext:ListItem Text="Loan Related" Value="LoanRelated" />
                            <ext:ListItem Text="Non-loan Related" Value="NonLoanRelated" />
                        </Items>
                    </ext:ComboBox>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:Label runat="server" Text="Start Date" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:DateField ID="dtStartDate" runat="server" Editable="false" AllowBlank="false">
                        <DirectEvents>
                            <Select OnEvent="onStartDateSelect" />
                        </DirectEvents>
                    </ext:DateField>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:Label runat="server" Text="End Date" />
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:DateField ID="dtEndDate" runat="server" Editable="false" AllowBlank="false">
                        <DirectEvents>
                            <Select OnEvent="onEndDateSelect" />
                        </DirectEvents>
                    </ext:DateField>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="Accept">
                        <Listeners>
                            <Click Handler="gridPanelReload();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill runat="server" />
                    <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer" Disabled="true">
                        <Listeners>
                            <Click Handler="printContent('PrintableDiv');" />
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
    <div id="PrintableDiv" style="font-family: Tahoma; font-size: small;">
        <table style="width: 750px; margin: 0 auto;">
            <tr>
                <td style="vertical-align:bottom;">
                    <br />
                    <center>
                        <ext:Label ID="lblLenderName" runat="server" Cls="bold"/><br />
                        <ext:Label ID="lblLenderAddress" runat="server" /><br />
                        <ext:Label ID="lblLenderCountryAndPostal" runat="server" /><br />
                        <ext:Label ID="lblLenderTelephoneNumber" runat="server" />&nbsp;
                        <ext:Label ID="lblLenderFaxNumber" runat="server" /><br />
                    </center>
                    <br />
                    <br />
                    <br />
                    <center><b><ext:Label ID="lblTrasanctionLabel" runat="server" Text="Transaction Report for"/>
                        <ext:Label ID="lblStartDate" runat="server" />
                        <ext:Label ID="lblEndDate" runat="server" />
                    </b></center>
                    <br />
                    <br />
                    <ext:GridPanel ID="TransactionReportGridPanel" runat="server" AutoHeight="true" EnableColumnHide="false" EnableColumnMove="false" Width="790" 
                            BaseCls="cssClass" ColumnLines="true" EnableColumnResize="false">
                        <Store>
                            <ext:Store ID="TransactionReportStore" runat="server" OnRefreshData="RefreshData">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="Name" />
                                            <ext:RecordField Name="LoanId" />
                                            <ext:RecordField Name="_Date" />
                                            <ext:RecordField Name="TransactionType" />
                                            <ext:RecordField Name="TransactionSubType" />
                                            <ext:RecordField Name="Amount" />
                                            <ext:RecordField Name="CurrencySymbol"/>
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="colModelTransReport" runat="server">
                            <Columns>
                                <ext:Column Header="Name" DataIndex="Name" Width="188" Css="border-color: Black;" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Loan Id" DataIndex="LoanId" Width="60" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                                <ext:Column Header="Date" DataIndex="_Date" Width="100" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                                <ext:Column Header="Transaction Type" DataIndex="TransactionType" Width="125" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                                <ext:Column Header="Trans. SubType" DataIndex="TransactionSubType" Width="140" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                                <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="105" Format=",000.00" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                                <ext:Column Header="Currency" DataIndex="CurrencySymbol" Width="70" Css="border-color: Black;" MenuDisabled="true" Sortable="false"/>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
