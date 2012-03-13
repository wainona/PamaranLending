<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordUserAccount.aspx.cs"
    Inherits="LendingApplication.Applications.UserAccountUseCases.ChangePasswordUserAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Customer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickcustomer', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully changed the password.', function () {
                window.proxy.sendToAll('updateuseraccount', 'updateuseraccount');
                window.proxy.requestClose();
            });
        }

        var confirmPassword = function () {
            if (txtPassword.getValue() == txtConfirmPassword.getValue()) {
                return true;
            }
            return false;
        }

        /************************************
        *          FOR PICKLISTS            *
        ************************************/
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                // do your stuff here
                hdnCustomerID.setValue(msg.data.CustomerID);
                txtReceivedFrom.setValue(msg.data.Names);
                X.FillDistrictAndStation(msg.data.CustomerID);

            }
            else if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.PartyRoleId);
                txtBank.setValue(msg.data.OrganizationName);
            };
        }
    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500"
                BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="160">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <%--SAVE BUTTON--%>
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask Msg="Saving password..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <%--CLOSE BUTTON--%>
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="hdnUserAccountId" runat="server" />
                    <ext:Hidden ID="hdnPartyRoleId" runat="server" />
                    <ext:TextField ID="txtName" runat="server" FieldLabel="Name" ReadOnly="true" Width="400" />
                    <ext:TextField ID="txtUsername" runat="server" FieldLabel="Username" Width="400" ReadOnly="true"/>
                    <ext:TextField ID="txtPassword" runat="server" FieldLabel="New Password" AllowBlank="false"
                        InputType="Password" Width="400"/>
                    <ext:TextField ID="txtConfirmPassword" Vtype="password" runat="server" FieldLabel="Confirm New Password"
                        AllowBlank="false" EnableKeyEvents="true" InputType="Password" Width="400" MsgTarget="Side">
                        <CustomConfig>
                            <ext:ConfigItem Name="initialPassField" Value="#{txtPassword}" Mode="Value" />
                        </CustomConfig>
                    </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
