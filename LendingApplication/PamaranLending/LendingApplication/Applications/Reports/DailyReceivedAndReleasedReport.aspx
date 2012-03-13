<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyReceivedAndReleasedReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.DailyReceivedAndReleasedReport" %>
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

        var sumTotalCash = function () {
            var total1000 = 1000 * nf1000Bills.getValue();
            txtTotal1000Bills.setValue(total1000);
            var total500 = 500 * nf500Bills.getValue();
            txtTotal500Bills.setValue(total500);
            var total200 = 200 * nf200Bills.getValue();
            txtTotal200Bills.setValue(total200);
            var total100 = 100 * nf100Bills.getValue();
            txtTotal100Bills.setValue(total100);
            var total50 = 50 * nf50Bills.getValue();
            txtTotal50Bills.setValue(total50);
            var total20 = 20 * nf20Bills.getValue();
            txtTotal20Bills.setValue(total20);
            var totalCoins = 1 * nfCoins.getValue();
            txtTotalCoins.setValue(totalCoins);
            var totalDamaged = 1 * nfDamaged.getValue();
            txtTotalDamaged.setValue(totalDamaged);

            var cov = hdnCashOnVault.getValue();

            var totalAll = (total1000 + total500 + total200 + total100 + total50 + total20 + totalCoins) + totalDamaged;
            txtTotalCashSummary.hide();
            txtTotalCashSummary.setValue(totalAll);
            txtTotalCashSummary.show();

            btnSaveToCashOnVault.disable();
            var cashTotal = txtTotalCash.getValue().replace(/\$|\,/g, '');
            if (parseFloat(cashTotal) == parseFloat(totalAll)) {
                X.CanCloseVault({
                    success: function (result) {
                        if (result == true) {
                            btnSaveToCashOnVault.enable();
                        } else {
                            btnSaveToCashOnVault.disable();
                        }
                    }
                });
            }
        };

        var saveToCashOnVaultSuccess = function () {
            showAlert('Success', 'Successfully saved the amount to the vault.');
        };

        var OnGenerateClick = function () {
            X.GenerateReport({
                success: function (result) {
                    btnPrint.enable();
                    var cashsummary_table = document.getElementById('tableCashSummary');
                    if (cmbCurrency.getValue() != '1') {
                        cashsummary_table.style.visibility = 'hidden';
                        //btnSaveToCashOnVault.enable();
                    } else {
                        cashsummary_table.style.visibility = 'visible';
                    }
                }
            });
        };

        var hideIfForeign = function () {
            if (cmbCurrency.getValue() != '1') {
                tableCashSummary.hide();
            } else {
                tableCashSummary.show();
            }
        }
    </script>
    <style type="text/css">
        .bold 
        {
            font-size: medium;
            font-weight: bold;
        }
        
        .style1
        {
            width: 170px;
        }
        .style2
        {
            width: 170px;
        }
        
        .style3
        {
            width: 23px;
        }
        .style4
        {
            width: 126px;
        }
        
        .rightFloat 
        {
            float: right;
        }
        .style5
        {
            width: 141px;
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
            @page
            {
                size: landscape;
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
                    <ext:Hidden ID="hdnCashOnVault" runat="server" />
                    <ext:Label ID="Label2" runat="server" Text="Currency: "></ext:Label>
                    <ext:ToolbarSpacer runat="server" Width="5" />
                    <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id" DisplayField="NameDescription" Width="190" Editable="false" AllowBlank="false">
                        <Store>
                            <ext:Store ID="strCurrency" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="Id">
                                        <Fields>
                                            <ext:RecordField Name="Id" />
                                            <ext:RecordField Name="NameDescription" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:ToolbarSpacer runat="server" />
                    <ext:Button ID="btnGenerate" runat="server" Text="Generate" Icon="Accept">
                         <Listeners>
                            <Click Handler="OnGenerateClick();" />
                         </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" />
                    <ext:Button ID="btnSaveToCashOnVault" Hidden="false" runat="server" Text="Close Vault" Icon="CoinsAdd" Disabled="true">
                        <DirectEvents>
                            <Click OnEvent="btnSaveToCashOnVault_DirectClick" Success="saveToCashOnVaultSuccess">
                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to close this vault for this day?" />
                                <EventMask ShowMask="true" Msg="Saving.." />
                            </Click>
                        </DirectEvents>
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
        <table style="width: 1000px; margin: 0 auto;">
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
                    <center><b><ext:Label ID="lblDailyTrasanctionLabel" runat="server"/>
                    </b></center>
                    <br />
                    <br />
                    <ext:Label ID="txtCashOnVault" runat="server" Cls="rightFloat"/>
                    <br />
                    <br />
                    <ext:GridPanel ID="TransactionReportGridPanel" runat="server" AutoHeight="true" EnableColumnHide="false" EnableColumnMove="false" Width="1050" 
                            BaseCls="cssClass" ColumnLines="true" EnableColumnResize="false">
                        <Store>
                            <ext:Store ID="TransactionReportStore" runat="server">
                                <Reader>
                                    <ext:JsonReader>
                                        <Fields>
                                            <ext:RecordField Name="StationNumber" />
                                            <ext:RecordField Name="Name" />
                                            <ext:RecordField Name="PaymentsCash" />
                                            <ext:RecordField Name="PaymentsCheck" />
                                            <ext:RecordField Name="ReleasedCash" />
                                            <ext:RecordField Name="ReleasedCheck" />
                                            <ext:RecordField Name="Bank" />
                                            <ext:RecordField Name="CheckNumber" />
                                            <ext:RecordField Name="Remarks" />
                                            <ext:RecordField Name="CashOnVault" />
                                            <ext:RecordField Name="_EntryDate" />
                                            <ext:RecordField Name="_TransactionDate" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="colModelTransReport" runat="server">
                            <Columns>
                                <ext:Column Header="Station No." DataIndex="StationNumber" Css="border-color: Black;" Width="60" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Name" DataIndex="Name" Css="border-color: Black;" Width="195" MenuDisabled="true" Sortable="false"/>
                                <ext:NumberColumn Header="COV" DataIndex="CashOnVault" Css="border-color: Black;" Width="90" MenuDisabled="true" Sortable="false" Format=",000.00"/>
                                <ext:Column Header="Received (Cash)" DataIndex="PaymentsCash" Css="border-color: Black;" Width="80" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Released (Cash)" DataIndex="ReleasedCash" Css="border-color: Black;" Width="80" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Received (Check)" DataIndex="PaymentsCheck" Css="border-color: Black;" Width="80" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Released (Check)" DataIndex="ReleasedCheck" Css="border-color: Black;" Width="80" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Bank" DataIndex="Bank" Css="border-color: Black;" Width="100" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Check Number" DataIndex="CheckNumber" Css="border-color: Black;" Width="85" MenuDisabled="true" Sortable="false" />
                                <ext:Column Header="Entry Date" DataIndex="_EntryDate" Css="border-color: Black;" Width="110" MenuDisabled="true" Sortable="false"/>
                                <ext:Column Header="Transaction Date" DataIndex="_TransactionDate" Css="border-color: Black;" Width="88" MenuDisabled="true" Sortable="false" />
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="1" style="height: 36px; width: 450px">
                        <tr>
                            <td class="style1">
                                <ext:Label runat="server" Text="CASH" />
                            </td>
                            <td class="style2">
                                <ext:Label ID="Label3" runat="server" Text="CHECK" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <ext:TextField ID="txtDRCash" runat="server" ReadOnly="true" FieldLabel="DR" LabelWidth="40"/>
                            </td>
                            <td class="style2">
                                <ext:TextField ID="txtDRCheck" runat="server" ReadOnly="true" FieldLabel="DR" LabelWidth="40"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <ext:TextField ID="txtCRCash" runat="server" ReadOnly="true" FieldLabel="CR" LabelWidth="40"/>
                            </td>
                            <td class="style2">
                                <ext:TextField ID="txtCRCheck" runat="server" ReadOnly="true" FieldLabel="CR" LabelWidth="40"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <ext:TextField ID="txtTotalCash" runat="server" ReadOnly="true" FieldLabel="Total" LabelWidth="40"/>
                            </td>
                            <td class="style2">
                                <ext:TextField ID="txtTotalCheck" runat="server" ReadOnly="true" FieldLabel="Total" LabelWidth="40"/>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="1" id="tableCashSummary">
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="CASH SUMMARY" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="1000 x " />
                            </td>
                            <td class="style4">
                                <ext:NumberField ID="nf1000Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal1000Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="500 x " />
                            </td>
                            <td class="style4">
                                <ext:NumberField ID="nf500Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal500Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="200 x " />
                            </td>
                            <td class="style4">
                                <ext:NumberField ID="nf200Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal200Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="100 x " />
                            </td>
                            <td class="style4">
                                <ext:NumberField ID="nf100Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal100Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="50 x " />
                            </td>
                            <td class="style4">
                                <ext:NumberField ID="nf50Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal50Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="20 x " />
                            </td>
                            <td class="style4">    
                                <ext:NumberField ID="nf20Bills" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotal20Bills" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="Coins: " />
                            </td>
                            <td class="style4">    
                                <ext:NumberField ID="nfCoins" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotalCoins" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ext:Label runat="server" Text="Damaged: " />
                            </td>
                            <td class="style4">    
                                <ext:NumberField ID="nfDamaged" runat="server" Width="100" EnableKeyEvents="true">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:NumberField>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotalDamaged" runat="server" EmptyText="0" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                            </td>
                            <td class="style4">
                                <b><ext:Label ID="Label1" runat="server" Text="TOTAL "/></b>
                            </td>
                            <td class="style3">
                                =</td>
                            <td class="style5">
                                <ext:TextField ID="txtTotalCashSummary" runat="server" Visible="true" DecimalPrecision="2" EmptyText="0" ReadOnly="true">
                                    <Listeners>
                                        <Show Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                    </Listeners>
                                </ext:TextField>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
    </div>
    </form>
</body>
</html>
