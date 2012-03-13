<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCustomerClassification.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.CustomerClassificationUseCases.AddCustomerClassification" %>

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
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Customer classification record was successfully created.', function () {
                window.proxy.sendToAll('addcustomerclassification', 'addcustomerclassification');
                window.proxy.requestClose();
            });
        }

        Ext.apply(Ext.layout.FormLayout.prototype, {
            originalRenderItem: Ext.layout.FormLayout.prototype.renderItem,
            renderItem: function (c, position, target) {
                if (c && !c.rendered && c.isFormField && c.fieldLabel && c.allowBlank === false) {
                    c.fieldLabel = " <span class=\"req\" style='color:red;'>&nbsp;*&nbsp;</span>" + c.fieldLabel;
                }
                this.originalRenderItem.apply(this, arguments);
            }
        });

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
        Title="Classification Details"
        Layout="FormLayout" 
        LabelWidth="150">
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" 
                                    Before="return #{PageFormPanel}.getForm().isValid();"
                                    Success="saveSuccessful();" Failure="saveFailed();" >
                                    <EventMask Msg="Saving Customer Classification.." ShowMask="true"/>
                                    </Click>
                            
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TextField ID="District" DataIndex="District" AllowBlank="false" MsgTarget="Side" runat="server" FieldLabel="District" AnchorHorizontal="40%" IsRemoteValidation="true">
                <RemoteValidation OnValidation="checkDistrict"/></ext:TextField>
            <ext:ComboBox runat="server" ID="cmbDistrictType" ValueField="Id" DisplayField="Name"
                FieldLabel="District Type"  AnchorHorizontal="40%" AllowBlank="false" Editable="false" SelectedIndex="0">
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
            <ext:TextField runat="server" ID="StationNumber" DataIndex="StationNumber" AllowBlank="true" FieldLabel="Station Number" AnchorHorizontal="40%"></ext:TextField>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
        </Listeners>
    </ext:FormPanel>
    </Items></ext:Viewport>
    </form>
</body>
</html>
