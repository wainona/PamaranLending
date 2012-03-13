<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="LendingApplication.Security.LoginPage" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <title>Login</title>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();

            window.proxy.sendToAll('logout', 'logout');
        });

        var clearContent = function () {
            txtUsername.setValue("");
            txtPassword.setValue("");
            PageFormPanelStatusBar.setStatus({
                text: '',
                clear: true
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <noscript>
            <center>
                <strong><font size="4">Sorry, your browser does not support JavaScript.</font></strong>
                <br />
                <img src="../Resources/images/JavascriptInstructions.png" />
            </center>
        </noscript>
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Label ID="lblMessage" runat="server" />
        <ext:Window ID="PageWindow" runat="server" Closable="false" Resizable="false" Height="152"
            Icon="Lock" Title="User Login" Draggable="true" Width="350" Modal="true" Padding="5"
            Layout="Form">
            <KeyMap runat="server">
                <ext:KeyBinding>
                    <Keys>
                        <ext:Key Code="ENTER" />
                    </Keys>
                    <Listeners>
                        <Event Handler="#{btnLogin}.fireEvent('click');" />
                    </Listeners>
                </ext:KeyBinding>
            </KeyMap>
            <Items>
                <ext:TextField ID="txtUsername" runat="server" FieldLabel="Username" AllowBlank="false"
                    BlankText="Your username is required." AnchorHorizontal="100%" />
                <ext:TextField ID="txtPassword" runat="server" InputType="Password" FieldLabel="Password"
                    AllowBlank="false" BlankText="Your password is required." AnchorHorizontal="100%" />
            </Items>
            <Buttons>
                <ext:Button ID="btnLogin" runat="server" Text="Login" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="btnLogin_DirectClick">
                            <EventMask ShowMask="true" Msg="Verifying..." />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Decline">
                    <Listeners>
                        <Click Handler="clearContent();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>
        </ext:Window>
    </div>
    </form>
</body>
</html>
