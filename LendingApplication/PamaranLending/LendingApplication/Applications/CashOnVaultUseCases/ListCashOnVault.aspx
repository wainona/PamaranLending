<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCashOnVault.aspx.cs" Inherits="LendingApplication.Applications.CashOnVaultUseCases.ListCashOnVault" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash On Vault</title>
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

        var onBtnAddCOVTrans = function () {
            url = '/Applications/CashOnVaultUseCases/AddCashOnVault.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('WithdrawDepositCOV', param, 'Add COV Transaction');
        };

        var onBtnViewCOVTrans = function () {
            url = '/Applications/CashOnVaultUseCases/CashOnVaultTransactions.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('COVTrans', param, 'COV Transactions');
        };

        var onBtnViewCOVHistory = function () {
            url = '/Applications/CashOnVaultUseCases/CashOnVaultHistory.aspx';
            dat = '?dat=' + hiddenDate.getValue();
            param = url + dat;
            window.proxy.requestNewTab('COVHistory', param, 'COV History');
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
    <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" Width="365" Height="150" PaddingSummary="5px 5px 5px 5px"
        Title="Cash On Vault" BodyStyle="background-color:transparent" LabelWidth="275">
        <Items>
            <ext:Button Flex="1" ID="btn" runat="server" Text="Generate" FieldLabel="Deposit To/Withdraw From Cash On Vault">
                <Listeners>
                    <Click Handler="onBtnAddCOVTrans();" />
                </Listeners>
            </ext:Button>
            <ext:Button Flex="1" ID="btnInAccountBalances" runat="server" Text="Generate" FieldLabel="View Cash On Vault Transactions">
                <Listeners>
                    <Click Handler="onBtnViewCOVTrans();" />
                </Listeners>
            </ext:Button>
            <ext:Button Flex="1" ID="btnInNumberOfAccounts" runat="server" Text="Generate" FieldLabel="View Cash On Vault History">
                <Listeners>
                    <Click Handler="onBtnViewCOVHistory();" />
                </Listeners>
            </ext:Button>
        </Items>
    </ext:FormPanel>
    </form>
</body>
</html>
