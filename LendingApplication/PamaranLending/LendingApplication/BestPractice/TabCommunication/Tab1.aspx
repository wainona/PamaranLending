<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tab1.aspx.cs" Inherits="LendingApplication.BestPractice.TabCommunication.Tab1" %>

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
	        // window.proxy.init means you will listen to events with tag 'pickedcustomer','pickedotherthings'
            window.proxy.init('childsendmessage');
	        window.proxy.on('messagereceived', onMessageReceived);
        });

        // msg has a data property which will contain the payload of the message.
        // example if you send a json of a person name like {firstName:'Ryan John', middleName:'Cueva', lastName:'Velasco'}
        // then you could get it by this statement : msg.data.firstName->> you will have a value of Ryan John

        // msg also has a tag property
        // tag property will have the values you specified in this line of code window.proxy.init('pickedcustomer','pickedotherthings');
	    var onMessageReceived = function (msg) {
            if (msg.tag == 'childsendmessage')
	        {
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
            //window.proxy.sendToParent(name, 'parentsendmessage');
            //given that the selection model is single.

            var name = {};
            name.firstName = txtFirstName.getValue();
            name.middleName = txtMiddleName.getValue();
            name.lastName = txtLastName.getValue();
            //sendToChild (id, data, 'tag'-> event name of the child that it listens to)
            window.proxy.sendToChild('CustomerPicker', name, 'parentsendmessage');
        }
</script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:FieldSet ID="FieldSet1" runat="server" Title="Person name">
        <Items>
            <ext:TextField ID="txtFirstName"  FieldLabel="First Name" runat="server">
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
    <ext:Button ID="btnOpenPickList" runat="server" Text="Open Pick List">
        <Listeners>
            <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/BestPractice/TabCommunication/PickListTab.aspx', 'PickListTab');" />
        </Listeners>
    </ext:Button>
    </form>
</body>
</html>
