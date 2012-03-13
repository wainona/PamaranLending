<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUserAccount.aspx.cs"
    Inherits="LendingApplication.Applications.UserAccountUseCases.CreateUserAccount" %>

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
            window.proxy.init(['onpickalloweduser', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added a new user account.', function () {
                window.proxy.sendToAll('adduseraccount', 'adduseraccount');
                window.proxy.requestClose();

            });
        }

        var createSuccess = function () {
            Ext.MessageBox.show({
                title: 'Create Successful',
                msg: 'Receipt record was successfully created.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var modifySuccess = function () {
            Ext.MessageBox.show({
                title: 'Modify Successful',
                msg: 'Receipt record was successfully modified.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var confirmPassword = function () {
            if (txtPassword.getValue() == txtConfirmPassword.getValue()) {
                return true;
            }
            return false;
        }

        /************************************
        *           FOR PICKLISTS           *
        ************************************/
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickalloweduser') {
                hdnPartyId.setValue(msg.data.PartyId);
                X.FillNameOfSelectedPartyId();
            }
        };

        var viewPickListAllowedUsers = function () {
            window.proxy.requestNewTab('AllowedUsersPicker', '/Applications/UserAccountsUseCases/PickListAllowedUsers.aspx?mode=single', 'Allowed Users Pick List');
        };
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
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
                BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="150">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask Msg="Adding record..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="hdnUserAccountId" runat="server" />
                    <ext:Hidden ID="hdnPartyId" runat="server" />
                    <ext:CompositeField ID="cfName" runat="server">
                        <Items>
                            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" AllowBlank="false" Width="400" ReadOnly="true" />
                            <%--Name--%>
                            <ext:Button ID="btnBrowse" runat="server" Text="Browse">
                                <Listeners>
                                    <Click Handler="viewPickListAllowedUsers();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:ComboBox ID="cmbUserAccountType" runat="server" FieldLabel="User Account Type"
                        AllowBlank="false" DisplayField="Name" ValueField="Id" Width="400" Editable="false">
                        <Store>
                            <ext:Store ID="strUserAccountType" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="Id">
                                        <Fields>
                                            <ext:RecordField Name="Id" />
                                            <ext:RecordField Name="Name" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:TextField ID="txtUsername" runat="server" FieldLabel="Username" AllowBlank="false"
                        Width="400" MsgTarget="Side" IsRemoteValidation="true">
                            <RemoteValidation OnValidation="ValidateUsernameOnDatabase" />
                    </ext:TextField>
                    <ext:TextField ID="txtPassword" runat="server" FieldLabel="Password" AllowBlank="false"
                        InputType="Password" Width="400" />
                    <ext:TextField ID="txtConfirmPassword" Vtype="password" runat="server" FieldLabel="Confirm Password"
                        AllowBlank="false" EnableKeyEvents="true" InputType="Password" Width="400" MsgTarget="Side">
                        <CustomConfig>
                            <ext:ConfigItem Name="initialPassField" Value="#{txtPassword}" Mode="Value" />
                        </CustomConfig>
                    </ext:TextField>
                    <ext:ComboBox ID="cmbPasswordQuestion" runat="server" FieldLabel="Password Question"
                        AllowBlank="false" Editable="false" Width="400" ForceSelection="true">
                        <Items>
                            <ext:ListItem Text="Where did you meet your spouse?" />
                            <ext:ListItem Text="What was the name of your first school?" />
                            <ext:ListItem Text="Who was your childhood hero?" />
                            <ext:ListItem Text="What is the name of your first crush?" />
                            <ext:ListItem Text="What is your favorite pastime?" />
                            <ext:ListItem Text="What is your favorite sports team?" />
                            <ext:ListItem Text="What is your mother's maiden name?" />
                            <ext:ListItem Text="What is the answer to the secret question?" />
                            <ext:ListItem Text="What is the name of your pet?" />
                        </Items>
                    </ext:ComboBox>
                    <ext:TextField ID="txtPasswordAnswer" runat="server" FieldLabel="Password Answer"
                        AllowBlank="false" Width="400" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && confirmPassword() == true){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
