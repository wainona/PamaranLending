<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCustomerClassification.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases.EditCustomerClassification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Customer</title>
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


        var openOrEdit = function () {
            var enable = btnOpen.getText() == 'Edit';
            if (enable) {
                btnOpen.setText('Open');
                District.setReadOnly(false);
                StationNumber.setReadOnly(false);
                cmbDistrictType.setReadOnly(false);
            }
            else {
                btnOpen.setText('Edit');
                District.setReadOnly(true);
                StationNumber.setReadOnly(true);
                btnSave.disable();
                cmbDistrictType.setReadOnly(true);
            }
        };

        Ext.apply(Ext.layout.FormLayout.prototype, {
            originalRenderItem: Ext.layout.FormLayout.prototype.renderItem,
            renderItem: function (c, position, target) {
                if (c && !c.rendered && c.isFormField && c.fieldLabel && c.allowBlank === false) {
                    c.fieldLabel = " <span class=\"req\" style='color:red;'>&nbsp;*&nbsp;</span>" + c.fieldLabel;
                }
                this.originalRenderItem.apply(this, arguments);
            }
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Customer classification record was successfully modified.', function () {
                window.proxy.sendToAll('updatecustomerclassification', 'updatecustomerclassification');
                //window.proxy.requestClose();
                openOrEdit();
            });
        }

        var onFormValidated = function (valid) {
            btnSave.disable();
            if (valid && btnOpen.getText() == 'Edit') {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
            } else if (valid && btnOpen.getText() != 'Edit') {
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
            }
        }

        var saveFailed = function () {
            showAlert('Status', 'Customer classification record already exists.');
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
	        padding          : 20px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout"><Items>
        <ext:FormPanel ID="PageFormPanel" 
            runat="server" 
            Padding="5" 
            ButtonAlign="Right" 
            MonitorValid="true"
            MonitorPoll = "500"
            Width="400" 
            Height="200"
            Title="Classification Details"
            BodyStyle="background-color:transparent"
            Layout="FormLayout"
            LabelWidth="150">
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button runat="server" ID="btnOpen" Text="Edit" Icon="NoteEdit">
                            <Listeners>
                                <Click Handler="openOrEdit();" />
                            </Listeners>
                        </ext:Button>
                        <%--<ext:Button runat="server" ID="btnEdit" Text="Edit" Icon="NoteEdit">
                            <Listeners>
                                <Click Handler="btnEdit_Click();"/>
                            </Listeners>
                        </ext:Button>--%>
                    <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="btnSave_Click" Success="saveSuccessful();" Failure="saveFailed();"/>
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
                <ext:Panel runat="server" Layout="FormLayout" ID="Panel" Height="200" Border="false">
                    <Items>
                        <ext:Hidden ID="RecordID" DataIndex="ID" runat="server"/>
                        <ext:TextField ID="District" DataIndex="District" AllowBlank="false" MsgTarget="Side" runat="server" ReadOnly="true" FieldLabel="District" AnchorHorizontal="40%" IsRemoteValidation="true">
                            <RemoteValidation OnValidation="checkDistrict"/>
                        </ext:TextField>
                        <ext:ComboBox runat="server" ID="cmbDistrictType" DisplayField="Name" FieldLabel="District Type"
                            ValueField="Id" DataIndex="Id" AnchorHorizontal="40%" ReadOnly="true" Editable="false" AllowBlank="false">
                            <Store>
                            <ext:Store runat="server" ID="strDistrictType" RemoteSort="false">
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
                        <ext:TextField ID="StationNumber" DataIndex="StationNumber" runat="server" AllowBlank="true" FieldLabel="Station Number"
                            AnchorHorizontal="40%" ReadOnly="true"/>
                    </Items>
                </ext:Panel>
            </Items>
            <BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>
            <Listeners>
                <ClientValidation Handler="onFormValidated(valid);" />
            </Listeners>
        </ext:FormPanel>
    </Items></ext:Viewport>
    </form>
</body>
</html>
