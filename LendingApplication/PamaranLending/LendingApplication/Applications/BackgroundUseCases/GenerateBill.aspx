<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateBill.aspx.cs" Inherits="LendingApplication.Applications.BackgroundUseCases.GenerateBill" %>

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
     <script type="text/javascript">
      Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        var generateSuccessful = function () {
            showAlert('Status', 'Successfully generated the bill.', function () {
                window.proxy.sendToAll('generatebillsuccessful', 'generatebillsuccessful');
                window.proxy.requestClose();
               // openCustomerList();
            });
        };
        var openCustomerList = function () {
            var param = '/Applications/BackgroundUseCases/CustomersWithNewBill.aspx';
            window.proxy.requestNewTab('CustomerBillStatement', param, 'Customer With Bill');
        };
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Form">
        <Items>
            <ext:Panel runat="server" Border="false" Padding="10">
                <Items>
                    <ext:CompositeField runat="server">
                        <Items>
                            <ext:DateField ID="dtGenerationDate" Width="400" runat="server" FieldLabel="Generation Date"
                                AllowBlank="true" MsgTarget="Side" Editable="false">
                            </ext:DateField>
                            <ext:Button runat="server" ID="btnGenerate" Text="Generate Bill" Icon="ArrowRight">
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" Success="generateSuccessful();">
                                        <EventMask ShowMask="true" Msg="Generating Bill..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
