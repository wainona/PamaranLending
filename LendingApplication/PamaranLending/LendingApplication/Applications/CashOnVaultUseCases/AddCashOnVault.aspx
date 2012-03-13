<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCashOnVault.aspx.cs" Inherits="LendingApplication.Applications.CashOnVaultUseCases.AddCashOnVault" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Bill</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">
      Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        var addCashOnVaultSuccessful = function () {
            showAlert('Status', 'Successfully updated the cash on vault.', function () {
                window.proxy.sendToAll('addcashonvaultsuccess', 'addcashonvaultsuccess');
                window.proxy.requestClose();
            });
        };
    
        var onFormValidated = function (valid) {
            if (valid) {
                StatusBarCOV.setStatus({ text: 'Form is valid.' });
                btnAddCashToVault.enable();
            } else {
                StatusBarCOV.setStatus({ text: 'Please fill out the form.' });
                btnAddCashToVault.disable();
               
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
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Form">
        <Items>
          <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="580" MonitorValid="true">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button runat="server" ID="btnAddCashToVault" Text="Save" Icon="Money" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnAdd_Click" Success="addCashOnVaultSuccessful();">
                                        <EventMask ShowMask="true" Msg="Saving COV Trans..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                               <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
               <ext:Panel ID="Panel1" runat="server" Border="false" Padding="5" Layout="FormLayout" Width="400"  LabelWidth="150">
                <Items>
                    <ext:Hidden ID="hiddenUserId" runat="server"></ext:Hidden>
                     <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id" DisplayField="NameDescription" AnchorHorizontal="95%" 
                     Editable="false" AllowBlank="false" AutoScroll="true" FieldLabel="Currency">
                                <Store>
                                    <ext:Store ID="strCurrency" runat="server">
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
                            </ext:ComboBox>
                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount" MaskRe="[0-9/./,]" AnchorHorizontal="95%" AllowBlank="false">
                        <Listeners>
                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                        </Listeners>
                    </ext:TextField>
                    <ext:ComboBox ID="cmbCOVTransType" runat="server" ValueField="Id" DisplayField="Name"
                         Editable="false" AllowBlank="false" FieldLabel="Transaction Type" AnchorHorizontal="95%">
                        <Store>
                            <ext:Store ID="strCOVTransType" runat="server">
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
                    <ext:TextArea ID="txtRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="95%" AllowBlank="true"></ext:TextArea>
                    <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" AllowBlank="false"
                                Editable="false" runat="server" AnchorHorizontal="95%">
                            </ext:DateField>
                </Items>
            </ext:Panel>
                </Items>
                <BottomBar>
                <ext:StatusBar ID="StatusBarCOV" runat="server">
                </ext:StatusBar>
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
