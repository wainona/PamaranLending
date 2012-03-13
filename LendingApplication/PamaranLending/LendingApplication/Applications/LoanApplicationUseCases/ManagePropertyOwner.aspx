<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePropertyOwner.aspx.cs"
    Inherits="LendingApplication.ManagePropertyOwner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Edit Asset Type</title>
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
            window.proxy.init('onpickmortgagee');
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickmortgagee') {
                hiddenPartyId.setValue(msg.data.PartyId);
                txtPropertyOwner.setValue(msg.data.Name);
                txtAddress.setValue(msg.data.Address);
            }
        }

        var onBtnPickPropertyOwner = function () {
            var url = '/Applications/LoanApplicationUseCases/PickListMortgagee.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickMortgagee', param, 'Select Property Owner');
        }

        var saveSuccessful = function () {
            var propertyOwner = {};
            propertyOwner.PartyId = hiddenPartyId.getValue();
            propertyOwner.Name = txtPropertyOwner.getValue();
            propertyOwner.Address = txtAddress.getValue();
            propertyOwner.PercentOwned = nfPercentOwned.getValue();

            var tag = hiddenMode.getValue() + 'propertyowner';
            if (hiddenMode.getValue() == "edit")
                propertyOwner.RandomKey = hiddenRandomKey.getValue();

            window.proxy.sendToParent(propertyOwner, tag);
            window.proxy.requestClose();
        }
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
        }
        var onFormValidated = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please completely fill out the forms.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
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
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden runat="server" ID="hiddenMode"></ext:Hidden>
    <ext:Hidden runat="server" ID="hiddenRandomKey"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" MonitorValid="true" Title="Collateral - Property Owner"
                BodyStyle="background-color:transparent" LabelWidth="150" Padding="5"  MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="OK" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();" />
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
                    <ext:Hidden runat="server" ID="hiddenPartyId">
                    </ext:Hidden>
                    <ext:CompositeField ID="cfPropertyOwner" DataIndex="" runat="server" 
                        Width="500">
                        <Items>
                            <ext:TextField ID="txtPropertyOwner" FieldLabel="Property Owner" runat="server" ReadOnly="true" Width="400" AllowBlank="false"/>
                            <ext:Button ID="btnPickPropertyOwner" runat="server" Text="Browse...">
                                <Listeners>
                                    <Click Handler="onBtnPickPropertyOwner();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:TextArea runat="server" ID="txtAddress" ReadOnly="true" Width="400" FieldLabel="Address">
                    </ext:TextArea>
                    <ext:CompositeField ID="cfPercentOwned" DataIndex="" runat="server" 
                        Width="500">
                        <Items>
                            <ext:NumberField runat="server" ID="nfPercentOwned" MinValue="0" MaxValue="100" AllowBlank="false"
                                Width="400" FieldLabel="Percent Owned">
                            </ext:NumberField>
                            <ext:Label runat="server" FieldLabel="%" Text="%" Width="20"/>
                        </Items>
                    </ext:CompositeField>
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
