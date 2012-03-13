<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAgingOfAccounts.aspx.cs"
    Inherits="LendingApplication.Applications.AgingOfAccountsUseCases.ListAgingOfAccounts" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aging of Accounts Summary</title>
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

        var onBtnDetailedAgingOfAccountsClick = function () {
            url = '/Applications/AgingOfAccountsUseCases/DetailedAgingSummary.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('DetailedAgingofAccountsSummary', param, 'Detailed Aging of Accounts Summary');
        };

        var onBtnInAccountBalancesClick = function () {
            url = '/Applications/AgingOfAccountsUseCases/AgingSummaryInBalances.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('AgingSummaryInAccountBalances', param, 'Aging of Accounts Summary In Account Balances');
        };

        var onBtnInNumberOfAccountsClick = function () {
            url = '/Applications/AgingOfAccountsUseCases/AgingSummaryInNumberOfAccounts.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('AgingSummaryInNumberOfAccounts', param, 'Aging of Accounts Summary In Number of Accounts');
        };

        var onDateChange = function () {
            X.FillDateSelected();
        };

    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 20px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
    <style media="print" type="text/css">
        @page
        {
            size: 8.5in 14in;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenDate" runat="server" />
<%--    <ext:ViewPort runat="server" Layout="FitLayout"><Items>--%>
    <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" Width="450" Height="150" PaddingSummary="5px 5px 5px 5px"
        Title="Aging of Accounts Report" BodyStyle="background-color:transparent" LabelWidth="350">
        <Items>
            <ext:DateField ID="datSelectedDate" Hidden="true" FieldLabel="Selected Date" runat="server" Editable="false" Width="125">
                <Listeners>
                    <Change Handler="onDateChange();" />
                </Listeners>
            </ext:DateField>            
            <ext:Button Flex="1" ID="btnDetailedAgingOfAccounts" runat="server" Text="Generate" FieldLabel="Detailed Aging of Accounts Summary">
                <Listeners>
                    <Click Handler="onBtnDetailedAgingOfAccountsClick();" />
                </Listeners>
            </ext:Button>
            <ext:Button Flex="1" ID="btnInAccountBalances" runat="server" Text="Generate" FieldLabel="Aging of Accounts Summary In Account Balances">
                <Listeners>
                    <Click Handler="onBtnInAccountBalancesClick();" />
                </Listeners>
            </ext:Button>
            <ext:Button Flex="1" ID="btnInNumberOfAccounts" runat="server" Text="Generate" FieldLabel="Aging of Accounts Summary In Number of Accounts">
                <Listeners>
                    <Click Handler="onBtnInNumberOfAccountsClick();" />
                </Listeners>
            </ext:Button>
        </Items>
    </ext:FormPanel>
<%--    </Items></ext:ViewPort>--%>
    </form>
</body>
</html>
