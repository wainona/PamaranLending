<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgingSummaryInNumberOfAccounts.aspx.cs" Inherits="LendingApplication.Applications.AgingOfAccountsUseCases.AgingSummaryInNumberOfAccounts" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aging of Accounts Summary In Number Of Accounts</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
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

        var onSelectDate = function () {
            X.FillTable();
        };

    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 16px;
	        background-color : #fff !important;
        }
        .style3
        {
            height: 59px;
        }
        .gridStyle
        {
            margin-left: 10px;
            margin-top: 10px;
        }
        .gridStyle2
        {
            margin-left: 250px;
        }
        .grid
        {
            margin-left: 250px;
            width: 500px;
        }
        .grid1
        {
            margin-left: 214px;
            width: 450px;
        }
        .headerClass
        {
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
            @page
            {
                size: landscape;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenPaymentId" runat="server"/>
    <ext:Hidden ID="hiddenDate" runat="server" />
    <div id="toolBar">
    <ext:FormPanel runat="server" Border="false">
        <TopBar>
        <ext:Toolbar runat="server" ID="PageToolBar">
            <Items>
            <ext:DateField ID="datSelectedDate" FieldLabel="Selected Date" LabelWidth="100" runat="server" Width="200" Editable="false" >
            </ext:DateField>
            <ext:Button ID="btnGenerate" runat="server" Text="Generate">
                <Listeners>
                    <Click Handler="onSelectDate();" />
                </Listeners>
            </ext:Button>
            <ext:Hidden ID="hdnLoanId" runat="server" />
            <ext:Hidden ID="hdnAgreementId" runat="server"></ext:Hidden>
            <ext:ToolbarFill ID="ctl778" />
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
            </Items>
        </ext:Toolbar>
        </TopBar>
    </ext:FormPanel>
    </div>
    <div id="PrintableContent">
        <table style="width:800px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;" colspan="7">
                        <ext:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <ext:Label ID="lblStreetAddress" runat="server" />&nbsp;<ext:Label ID="lblBarangay" runat="server" /><br />
                        <ext:Label ID="lblMunicipality" runat="server" /><ext:Label ID="lblCity" runat="server" />&nbsp;<ext:Label ID="lblProvince" runat="server" />&nbsp;<ext:Label ID="lblCountry" runat="server" />&nbsp;<ext:Label ID="lblPostalCode" runat="server" /><br />
                        Tel#: <ext:Label ID="lblPrimTelNumber" runat="server" />&nbsp;<ext:Label ID="lblSecTelNumber" runat="server" /><br />
                        Fax#: <ext:Label ID="lblFaxNumber" runat="server" /><br />
                        <ext:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align:center; font-weight:bold; vertical-align:bottom; border-bottom-color: Gray; border-bottom-width:thin; text-transform:uppercase;" class="style3" colspan="7">
                        <br />
                        AGING 
                        of Accounts SUMMARY AS OF
                        <ext:Label ID="lblSelectedDate" runat="server" />
                        <p style="text-transform:capitalize;">In Number Of Accounts</p>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table border="1" width="950px" cellpadding="10" cellspacing="0" >
                            <tr id="FeesRow" style="height: 30px">
                               <td></td>
                               <td align="center" style="font-weight: bold" width="150">Total</td>
                               <td align="center" style="font-weight: bold">Current</td>
                               <td align="center" style="font-weight: bold">1-30 Days</td>
                               <td align="center" style="font-weight: bold">31-60 Days</td>
                               <td align="center" style="font-weight: bold">61-90 Days</td>
                               <td align="center" style="font-weight: bold">Over 90 Days</td>
                            </tr>
                            <tr style="height: 35px">
                               <td align="center" width="120">Number of Accounts</td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNoAccountsTotal" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNumberCurrent" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNumber130" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNumber3160" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNumber6190" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblNumberOver90" />
                               </td>
                            </tr>
                            <tr style="height: 35px">
                                <td align="center">Ratio to Total</td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatioTotal" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatioCurrent" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatio130" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatio3160" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatio6190" />
                               </td>
                               <td align="center" >
                                    <ext:Label runat="server" ID="lblRatioOver90" />
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
