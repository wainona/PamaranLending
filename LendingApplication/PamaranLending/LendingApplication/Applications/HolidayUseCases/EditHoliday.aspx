<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditHoliday.aspx.cs" Inherits="LendingApplication.Applications.HolidayUseCases.EditHoliday" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Holiday</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully update the record.', function () {
                window.proxy.sendToAll('updateholiday', 'updateholiday');
                window.proxy.requestClose();
            });
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
            padding: 20px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
          .ext-hide-mask .ext-el-mask
        {
            opacity: 0.30;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" ButtonAlign="Right"
                MonitorValid="true" MonitorPoll = "500" Width="600" Height="600" Title="Holiday Details" BodyStyle="background-color:transparent"
                Layout="FormLayout">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Hidden="true" Icon="NoteEdit">
                                <DirectEvents>
                                    <Click OnEvent="btnOpen_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                <DirectEvents>
                                    <Click OnEvent="btnEdit_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Disabled = "true">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask ShowMask="true" Msg="Saving.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel ID="pnlHoliday" runat="server" Disabled="true" Padding = "5" LabelWidth="80"
                        Height="1000" Layout="FormLayout" Border = "false" Cls="ext-hide-mask">
                        <Items>
                            <ext:Hidden ID="hiddenId" runat="server" />
                            <ext:DateField ID="dtHoliday" FieldLabel="Date" runat="server" AnchorHorizontal="40%"
                                Editable="false" AllowBlank="false">
                            </ext:DateField>
                            <ext:TextField ID="txtName" DataIndex="Name" runat="server" FieldLabel="Name" AnchorHorizontal="40%"
                                AllowBlank="false" />
                            <ext:TextArea ID="txtDesciption" DataIndex="Description" FieldLabel="Description"
                                AnchorHorizontal="40%" runat="server" />
                            <ext:TextArea ID="txtNotes" DataIndex="Notes" FieldLabel="Notes" AnchorHorizontal="40%"
                                runat="server" />
                        </Items>
                    </ext:Panel>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="#{PageFormPanelStatusBar}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetail}.enable();}  else{#{btnDoneAddressDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
