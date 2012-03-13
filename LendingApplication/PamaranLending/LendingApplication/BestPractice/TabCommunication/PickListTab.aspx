<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListTab.aspx.cs" Inherits="LendingApplication.BestPractice.TabCommunication.PickListTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            // pick list also list to add events because most of them have a new button for adding new items.
            window.proxy.init('parentsendmessage');
            window.proxy.on('messagereceived', onMessageReceived);
        });

        // msg also has a tag property
        // tag property will have the values you specified in this line of code window.proxy.init('pickedcustomer','pickedotherthings');
        var onMessageReceived = function (msg) {
            if (msg.tag == 'parentsendmessage') {
                // do your stuff here
                txtFirstName.setValue(msg.data.firstName);
                txtMiddleName.setValue(msg.data.middleName);
                txtLastName.setValue(msg.data.lastName);
            }
        };

        //this will be called by the trigger event that says the user wanted this item.
        var SendMessage = function () {
            //for testing...
            //var name = {firstName:'Ryan John', middleName:'Cueva', lastName:'Velasco'};
            //window.proxy.sendToParent(name, 'childsendmessage');
            //given that the selection model is single.

            var name = {};
            name.firstName = txtFirstName.getValue();
            name.middleName = txtMiddleName.getValue();
            name.lastName = txtLastName.getValue();
            window.proxy.sendToParent(name, 'childsendmessage');
        }

</script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:FieldSet ID="FieldSet1" runat="server" Title="Person name">
        <Items>
            <ext:TextField ID="txtFirstName" FieldLabel="First Name" runat="server">
            </ext:TextField>
            <ext:TextField ID="txtMiddleName" FieldLabel="Middle Name" runat="server">
            </ext:TextField>
            <ext:TextField ID="txtLastName" FieldLabel="Last Name" runat="server">
            </ext:TextField>
        </Items>
    </ext:FieldSet>
    <ext:Button ID="btnSendMessage" runat="server" Text="Send Name to Child">
        <Listeners>
            <Click Handler="SendMessage();" />
        </Listeners>
    </ext:Button>
    </form>
</body>
</html>
