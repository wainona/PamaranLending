<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRequiredDocumentType.aspx.cs" Inherits="LendingApplication.Applications.RequiredDocumentTypeUseCases.EditRequiredDocumentType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Customer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var actualState = new Array();

        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
            searchPreviousState(PageFormPanel);
            enableDisablePanelElement(PageFormPanel, false);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Required document type was successfully modified.', function () {
                window.proxy.sendToAll('updaterequireddocumenttype', 'updaterequireddocumenttype');
                window.proxy.requestClose();
            });
        }

        var saveFailure = function () {
            showAlert('Status', 'Required document type already exists.');
        }

        var onFormValidated = function (valid) {
            btnSave.disable();
            if (valid && btnOpen.getText() != 'Edit') {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid && btnOpen.getText() == 'Edit') {
                PageFormPanelStatusBar.setStatus({ text: 'Currently in View Mode' });
                btnSave.disable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnSave.disable();
            }
        }

        var openOrEdit = function () {
            var enable = btnOpen.getText() == 'Edit';
            enableDisablePanelElement(PageFormPanel, enable);

            if (enable) {
                btnOpen.setText('Open');
            }
            else {
                btnOpen.setText('Edit');
            }
        };

        var enableDisablePanelElement = function (panel, enable) {
            if (enable == true) {
                panel.cascade(function (item) {
                    var index = actualState.indexOf(item);
                    if (index == -1) {
                        if (item.getXType() == 'button'
                        || item.getXType() == 'checkbox'
                        || item.getXType() == 'radiogroup'
                        || item.getXType() == 'radio') {
                            item.enable();
                        }
                        else if (item.getXType() == 'datefield'
                        || item.getXType() == 'combo'
                        )
                            item.setReadOnly(false);
                        else if (item.isFormField && (typeof item.readOnly != 'undefined')) {
                            //item.enable();
                            //setReadOnly(item, false);
                            item.setReadOnly(false);
                        }
                    }
                });
            } else {
                panel.cascade(function (item) {
                    if (item.getXType() == 'button'
                    || item.getXType() == 'checkbox'
                    || item.getXType() == 'radiogroup'
                    || item.getXType() == 'radio') {
                        item.disable();
                    }
                    else if (item.getXType() == 'datefield'
                    || item.getXType() == 'combo') {
                        item.setReadOnly(true);
                    }
                    else if (item.isFormField && (typeof item.readOnly != 'undefined')) {
                        //item.disable();
                        //setReadOnly(item, true);
                        item.setReadOnly(true);
                    }
                });
            }
        };

        var setReadOnly = function (element, readOnly) {
            if (element.el.dom.hasAttribute('readOnly') == false)
                return;

            if (readOnly) {
                element.el.dom.setAttribute('readOnly', true);
            } else {
                element.el.dom.removeAttribute('readOnly');
            }
        }

        var searchPreviousState = function (panel) {
            panel.cascade(function (item) {
                if (item.getXType() == 'datefield' || item.getXType() == 'combo' || item.isFormField) {
                    if (item.readOnly) {
                        //alert(item.getXType());
                        actualState.push(item);
                    }
                }
            });
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
        
        .ext-hide-mask .ext-el-mask
        {
            opacity: 0.30;
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
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnOpen" runat="server" Text="Edit" Icon="NoteEdit">
                        <Listeners>
                            <Click Handler="openOrEdit();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill />
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" 
                                    Before="return #{PageFormPanel}.getForm().isValid();" 
                                    Success="saveSuccessful();"
                                    Failure="saveFailure();">
                                <EventMask Msg="Updating..." ShowMask="true"/>        
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
            <Items>
                <ext:Hidden ID="RecordID" DataIndex="ID" runat="server"/>
                <ext:TextField ID="Name" Cls="nameTrans" DataIndex="Name" runat="server" IsRemoteValidation="true" FieldLabel="Name" AnchorHorizontal="60%">
                    <RemoteValidation Failure="saveFailure" OnValidation="CheckField" />
                </ext:TextField>
            </Items>
            <LoadMask Msg="Retrieving required document type..." ShowMask="true" />
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
