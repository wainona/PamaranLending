<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenExchangeRate.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ExchangeRateUseCases.OpenExchangeRate" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            window.proxy.init([]);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
        };

        var confirmCloseTab = function () {
            showConfirm("Message", "Are you sure you want to close the tab?", function (btn) {
                if (btn.toLocaleLowerCase() == 'yes') {
                    window.proxy.requestClose();
                }
                else {

                }
            });
        }

        var addSuccess = function () {
            showAlert('Success!', 'Successfully updated the exchange rate record.', function () {
                window.proxy.sendToAll('editexchangerate', 'editexchangerate');
                cancelClick();
            });
        }

        var editClick = function () {
            btnSave.show();
            btnSaveSeparator.show();
            btnCancel.show();
            btnEdit.hide();
//            cmbCurrencyFrom.setReadOnly(false);
//            cmbCurrencyTo.setReadOnly(false);
//            cmbExchangeRateType.setReadOnly(false);
            txtRate.setReadOnly(false);
        };

        var cancelClick = function () {
            btnSave.hide();
            btnSaveSeparator.hide();
            btnCancel.hide();
            btnEdit.show();
//            cmbCurrencyFrom.setReadOnly(true);
//            cmbCurrencyTo.setReadOnly(true);
//            cmbExchangeRateType.setReadOnly(true);
            txtRate.setReadOnly(true);
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
        .disabled
        {
            opacity: .95;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
                <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Padding="5" MonitorValid="true" LabelWidth="150">
                    <TopBar>
                        <ext:Toolbar ID="PagePanelToolBar" runat="server">
                            <Items>
                                <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="DatabaseEdit">
                                    <Listeners>
                                        <Click Handler="editClick();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                                    <DirectEvents>
                                        <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();">
                                            <EventMask Msg="Saving.." ShowMask="true" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="btnSaveSeparator" runat="server" Hidden="true" />
                                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cross" Hidden="true">
                                    <Listeners>
                                        <Click Handler="cancelClick();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill runat="server" />
                                <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                    <Listeners>
                                        <Click Handler="onBtnClose();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Hidden ID="hdnExchangeRateId" runat="server" />
                         <ext:ComboBox ID="cmbCurrencyFrom" runat="server" FieldLabel="Currency From" Width="350" Editable="false" DisplayField="NameDescription" ValueField="Id" AllowBlank="false" ReadOnly="true">
                            <Store>
                                <ext:Store ID="storeCurrencyFrom" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="Id">
                                            <Fields>
                                                <ext:RecordField Name="Id" />
                                                <ext:RecordField Name="NameDescription" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Expand OnEvent="GetAllCurrencies">
                                    <EventMask ShowMask="true" Msg="Retrieving.." Target="This"/>
                                </Expand>
                            </DirectEvents>
                        </ext:ComboBox>
                        <ext:ComboBox ID="cmbCurrencyTo" runat="server" FieldLabel="Currency To" Width="350" Editable="false" DisplayField="NameDescription" ValueField="Id" AllowBlank="false" ReadOnly="true">
                            <Store>
                                <ext:Store ID="storeCurrencyTo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="Id">
                                            <Fields>
                                                <ext:RecordField Name="Id" />
                                                <ext:RecordField Name="NameDescription" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <DirectEvents>
                                <Expand OnEvent="GetAllCurrencies">
                                    <EventMask Target="This" ShowMask="true" Msg="Retrieving.."/>
                                </Expand>
                            </DirectEvents>
                        </ext:ComboBox>
                         <ext:ComboBox ID="cmbExchangeRateType" runat="server" DisplayField="Name" ValueField="Id" FieldLabel="Exchange Rate Type" AllowBlank="false" Editable="false" ReadOnly="true" Width="350">
                            <Store>
                                <ext:Store ID="storeExchangeRateType" runat="server">
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
                        <ext:TextField ID="txtRate" runat="server" FieldLabel="Rate" AllowBlank="false" Width="350" MaskRe="[0-9\.\,]" ReadOnly="true" />
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
