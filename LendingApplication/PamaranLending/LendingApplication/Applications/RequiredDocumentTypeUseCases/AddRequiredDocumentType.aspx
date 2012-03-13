<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRequiredDocumentType.aspx.cs" Inherits="LendingApplication.Applications.RequiredDocumentTypeUseCases.AddRequiredDocumentType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Required Document Typer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Required document type was successfully created.', function () {
                window.proxy.sendToAll('addrequireddocumenttype', 'addrequireddocumenttype');
                window.proxy.requestClose();
            });
        }

        var onFormValidated = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnSave.disable();
            }
        }

        var saveFailure = function () {
            showAlert('Status', 'Required document type already exists.');
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
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        
        .nameTrans 
        {
            text-transform: capitalize;
        }
        
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="FitLayout" AutoHeight="true">
        <Items>
    <ext:FormPanel ID="PageFormPanel" 
        runat="server" 
        Padding="5" 
        ButtonAlign="Right" 
        AutoHeight="true"
        Title="Required Document Type Details"
        BodyStyle="background-color:transparent"
        Layout="FormLayout" MonitorValid="true" MonitorPoll="500">
        <Defaults>
            <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
            <ext:Parameter Name="MsgTarget" Value="side" />
        </Defaults>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" 
                                    Before="return #{PageFormPanel}.getForm().isValid();" 
                                    Success="saveSuccessful();"
                                    Failure="saveFailure();">
                             <EventMask Msg="Saving.." ShowMask="true"/>       
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
            <ext:TextField ID="Name" Cls="nameTrans" DataIndex="Name" runat="server" IsRemoteValidation="true" FieldLabel="Name" AnchorHorizontal="60%">
                <RemoteValidation Failure="saveFailure" OnValidation="CheckField"></RemoteValidation>
            </ext:TextField>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <ClientValidation Handler="onFormValidated(valid);" />
        </Listeners>
    </ext:FormPanel>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
