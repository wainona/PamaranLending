<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSystemSettings.aspx.cs"
    Inherits="LendingApplication.Applications.FinancialManagement.SystemSettingsUseCases.ListSystemSettings" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
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

        var onBtnSystemSettingsClick = function () {
            window.proxy.requestNewTab('EditSystemSettings', '/Applications/SystemSettingsUseCases/EditSystemSettings.aspx', 'Edit System Settings');
        };

        var onBtnLenderInformationClick = function () {
            window.proxy.requestNewTab('EditLenderInformation', '/Applications/SystemSettingsUseCases/EditLenderInformation.aspx', 'Edit Lender Information');
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
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true"
        Width="250" Height="90" Title="System Settings" BodyStyle="background-color:transparent" LabelWidth="180">
        <Items>
            <ext:Button ID="btnLenderInformation" runat="server" Text="Edit" FieldLabel="Lender Information">
                <Listeners>
                    <Click Handler="onBtnLenderInformationClick();" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="btnSystemSettings" runat="server" Text="Edit" FieldLabel="System Settings">
                <Listeners>
                    <Click Handler="onBtnSystemSettingsClick();" />
                </Listeners>
            </ext:Button>
        </Items>
    </ext:FormPanel>
    </form>
</body>
</html>
