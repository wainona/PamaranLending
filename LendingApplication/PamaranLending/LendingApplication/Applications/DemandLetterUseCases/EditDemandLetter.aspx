<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditDemandLetter.aspx.cs"
    Inherits="LendingApplication.Applications.DemandLetterUseCases.EditDemandLetter" %>

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
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickcustomer', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully updated demand letter record.', function () {
                window.proxy.sendToAll('updateuseraccount', 'updateuseraccount');
                window.proxy.requestClose();
            });
        }

        var btnEdit_Click = function () {
            btnEdit.hide();
            btnSave.show();
            btnSaveSeparator.show();
            btnCancel.show();

            dfDatePromiseToPay.setReadOnly(false);
            txtRemarks.setReadOnly(false);
        };

        var btnCancel_Click = function () {
            btnEdit.show();
            btnSave.hide();
            btnSaveSeparator.hide();
            btnCancel.hide();

            dfDatePromiseToPay.setReadOnly(true);
            txtRemarks.setReadOnly(true);
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
                BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="130">
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
                            <ext:ToolbarSeparator ID="btnSaveSeparator" runat="server" Hidden="true"/>
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel" Hidden="true">
                                <%--CANCEL BUTTON--%>
                                <Listeners>
                                    <Click Handler="btnCancel_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cross">
                                <%--CLOSE BUTTON--%>
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="hdnDemandLetterId" runat="server" />
                    <ext:Hidden ID="hdnPartyRoleId" runat="server" />
                    <ext:TextField ID="txtName" runat="server" FieldLabel="Customer Name" ReadOnly="true" Width="400" />
                    <ext:TextField ID="txtLoanId" runat="server" FieldLabel="Loan Id" Width="400" ReadOnly="true"/>
                    <ext:DateField ID="dfDateSent" runat="server" FieldLabel="Date Sent" AllowBlank="false" Width="400" ReadOnly="true"/>
                    <ext:DateField ID="dfDatePromiseToPay" runat="server" FieldLabel="Date Promised to Pay" AllowBlank="false" Width="400" ReadOnly="true"/>
                    <ext:TextArea ID="txtRemarks" runat="server" FieldLabel="Remarks" AllowBlank="true" Width="400" Height="200" ReadOnly="true"/>
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
