<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnderLitigationLoan.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.UnderLitigationLoan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Write Off Laon</title>
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

        var UnderLitigationSuccess = function () {
            showAlert('Under Litigation Success', 'The status of the selected loan is successfully changed to \'Under Litigation\'.', function () {
                window.proxy.sendToAll('onunderlitigation', 'onunderlitigation');
                window.proxy.requestClose();
            });
        };
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 5px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:FormPanel ID="PageFormPanel1" runat="server" Padding="5" Width="600" Title="Under Litigation Loan" MonitorValid="true">
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_OnDirectEvent" Before="return #{PageFormPanel1}.getForm().isValid()" Success="UnderLitigationSuccess">
                                <EventMask Msg="Saving.." ShowMask="true" />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" />
                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:Hidden ID="hdnLoanId" runat="server"></ext:Hidden>
            <ext:TextArea ID="txtComment" runat="server" AnchorHorizontal="100%" FieldLabel="Comment" AllowBlank="false"/>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <Clientvalidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
        </Listeners>
    </ext:FormPanel>
    </form>
</body>
</html>
