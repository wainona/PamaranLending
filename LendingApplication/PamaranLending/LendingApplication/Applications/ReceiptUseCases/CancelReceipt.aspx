<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelReceipt.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.CancelReceipt" %>

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

        var cancelSuccess = function () {
            showAlert('Cancel Successful', 'The receipt record was successfully cancelled.', function () {
                window.proxy.sendToAll('cancelreceipt', 'cancelreceipt');
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
    <ext:FormPanel ID="PageFormPanel1" runat="server" Padding="5" Width="600" Title="Cancel Receipt">
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_onDirectEventClick" Success="cancelSuccess">
                                <EventMask Msg="Saving.." ShowMask="true"/>
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
            <ext:Hidden ID="hdnReceiptID" runat="server" />
            <ext:TextArea ID="txtComment" runat="server" AnchorHorizontal="100%" FieldLabel="Comment" />
        </Items>
    </ext:FormPanel>
    </form>
</body>
</html>
