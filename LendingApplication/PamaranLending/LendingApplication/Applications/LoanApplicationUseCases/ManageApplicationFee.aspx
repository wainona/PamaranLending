<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageApplicationFee.aspx.cs"
    Inherits="LendingApplication.ManageApplicationFee" %>

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
            window.proxy.init('onpickfee');
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickfee') {
                hiddenId.setValue(msg.data.ProductFeatureApplicabilityId);
                nfFeeAmount.setValue('0');
                setReadOnly(nfFeeAmount, true);
                setReadOnly(txtFeeName, true);

                X.ValidateName(msg.data.Name, {
                    success: function (result) {
                        markAs(txtFeeName, result);
                        txtFeeName.setValue(msg.data.Name);
                    }
                });
            }
        }

        var markAs = function(field, valid) {
            field.rvConfig.validating = false;
            field.rvConfig.remoteValidated = true;
            field.rvConfig.remoteValid = valid;
        }

        var onBtnPickFee = function () {
            var url = '/BestPractice/PickListFees.aspx';
            var param = url + "?mode=" + 'single';
            param = param + "&financialProductId=" + hiddenProductId.getValue();
            window.proxy.requestNewTab('PickFee', param, 'Select Fee');
        };

        var clear = function () {
            txtFeeName.setValue('');
            hiddenId.setValue('');
            setReadOnly(nfAmount, false);
            setReadOnly(txtFeeName, false);
        }

        var setReadOnly = function (formField, readOnly) {
            if (readOnly) {
                formField.el.dom.setAttribute('readOnly', true);
            } else {
                formField.el.dom.removeAttribute('readOnly');
            }
        }

        var triggerHandler = function (el, trigger, index) {
            switch (index) {
                case 0:
                    clear();
                    break;
                case 1:
                    onBtnPickFee();
                    break;
            }
        }

        var saveSuccessful = function () {
            var data = {};
            data.ProductFeatureApplicabilityId = hiddenId.getValue();
            amount = nfFeeAmount.getValue();
            data.Amount = amount.replace(',', '');
            //data.Amount = nfFeeAmount.getValue();
            data.FeeName = txtFeeName.getValue();

            window.proxy.sendToAll(data, 'onpickapplicationfee');
            window.proxy.requestClose();
        }

        var onFormValidated = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSave.enable();
            }
            else {
                if (txtFeeName.getValue().trim() != '')
                    PageFormPanelStatusBar.setStatus({ text: txtFeeName.invalidText, iconCls: 'icon-exclamation' });
                else
                    PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
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
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden ID="hiddenProductId" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" MonitorValid="true" Title="Application Fee"
                BodyStyle="background-color:transparent" LabelWidth="150" Padding="5">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="OK" Icon="Accept">
                                <Listeners>
                                    <Click Handler="saveSuccessful();" />
                                </Listeners>
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
                    <ext:Hidden runat="server" ID="hiddenId">
                    </ext:Hidden>
                    <ext:CompositeField ID="cfFee" DataIndex="" runat="server" FieldLabel="Fee Name"
                        Width="500">
                        <Items>
                            <ext:TriggerField ID="txtFeeName" runat="server" Width="400" AllowBlank="false"
                             IsRemoteValidation="true">
                                <RemoteValidation OnValidation="CheckFeeNameChanged"></RemoteValidation>
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Clear" />
                                    <ext:FieldTrigger Icon="Search" Qtip="Browse"/>
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Fn="triggerHandler" CausesValidation="true"/>
                                </Listeners>
                            </ext:TriggerField>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="cfAmount" runat="server" Width="500">
                        <Items>
                            <ext:TextField runat="server" FieldLabel="Amount" ID="nfFeeAmount" AllowBlank="false"
                                Width="400">
                                <Listeners>
                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                </Listeners>
                            </ext:TextField>
                            <ext:Label runat="server" Text="₱" Width="20"/>
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
