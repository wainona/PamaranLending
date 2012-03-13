<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUserAccount.aspx.cs"
    Inherits="LendingApplication.Applications.UserAccountUseCases.EditUserAccount" %>

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
            showAlert('Status', 'Successfully changed the user account type.', function () {
                window.proxy.sendToAll('updateuseraccount', 'updateuseraccount');
                window.proxy.requestClose();
            });
        }

        var btnEdit_Click = function () {
            btnEdit.hide();
            btnSave.show();
            btnSaveSeparator.show();
            btnCancel.show();

            cmbUserAccountType.setReadOnly(false);
            txtPassword.setReadOnly(false);
            txtConfirmPassword.setReadOnly(false);
            cmbPasswordQuestion.setReadOnly(false);
            txtPasswordAnswer.setReadOnly(false);
        };

        var btnCancel_Click = function () {
            btnEdit.show();
            btnSave.hide();
            btnSaveSeparator.hide();
            btnCancel.hide();

            cmbUserAccountType.setReadOnly(true);
            txtPassword.setReadOnly(true);
            txtConfirmPassword.setReadOnly(true);
            cmbPasswordQuestion.setReadOnly(true);
            txtPasswordAnswer.setReadOnly(true);
        };

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

        var closeAfterActivate = function () {
            window.proxy.sendToAll('activateuseraccountsuccess', 'activateuseraccountsuccess');
            window.proxy.requestClose();
        }

        var closeAfterDeactivate = function () {
            window.proxy.sendToAll('deactivateuseraccountsuccess', 'deactivateuseraccountsuccess');
            window.proxy.requestClose();
        }
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
                BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="160">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                <%--EDIT BUTTON--%>
                                <Listeners>
                                    <Click Handler="btnEdit_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                                <%--SAVE BUTTON--%>
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask Msg="Updating record..." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnSaveSeparator" runat="server"  Hidden="true" />
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cross" Hidden="true">
                                <%--CANCEL BUTTON--%>
                                <Listeners>
                                    <Click Handler="btnCancel_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <%--CLOSE BUTTON--%>
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="hdnUserAccountId" runat="server" />
                    <ext:Hidden ID="hdnPartyRoleId" runat="server" />
                    <ext:TextField ID="txtName" runat="server" FieldLabel="Name" AllowBlank="false" ReadOnly="true" Width="400" />
                    <ext:TextField ID="txtUsername" runat="server" FieldLabel="Username" AllowBlank="false" Width="400" ReadOnly="true"/>
                    <ext:ComboBox ID="cmbUserAccountType" runat="server" FieldLabel="User Account Type"
                        AllowBlank="false" DisplayField="Name" ValueField="Id" Editable="false" Width="400" ReadOnly="true">
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
