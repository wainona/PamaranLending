<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleAddEditTemplate.aspx.cs" Inherits="LendingApplication.SimpleAddEditTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Edit Asset Type</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Asset type is successfully modified.', function () {
                window.proxy.sendToAll('assettypeedit', 'assettypeedit');
                window.proxy.requestClose();
            });
        }
        var saveFailed = function () {
            showAlert('Status', 'Asset type name already exists.', function () {
                window.proxy.sendToAll('assettypeedit', 'assettypeedit');
                window.proxy.requestClose();
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
        <ext:FormPanel ID="PageFormPanel" 
            runat="server" 
            Padding="5" 
            ButtonAlign="Right" 
            MonitorValid="true"
            Width="300" 
            Height="200"
            Title="Asset Types"
            BodyStyle="background-color:transparent"
            Layout="FormLayout">
            <Defaults>
                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
            <Items>
                <ext:Hidden ID="RecordID" DataIndex="ID" runat="server"/>
                <ext:TextField ID="txtName" DataIndex="Name" MsgTarget="Side" AllowBlank ="false"  runat="server" FieldLabel="Asset Type" AnchorHorizontal="95%" IsRemoteValidation = "true">
            <RemoteValidation OnValidation = "checkEdit"></RemoteValidation></ext:TextField>
                <ext:Label ID = "lblAppraisable" Text = "Appraisable?" runat="server"></ext:Label>
                <ext:RadioGroup ID="radGroup" runat="server">
                <Items>
                <ext:Radio ID="radTrue" BoxLabel = "True" runat="server">
                </ext:Radio>
                <ext:Radio ID="radFalse" BoxLabel = "False" runat="server">
                </ext:Radio>
                </Items>
                 </ext:RadioGroup>
            </Items>
            <Buttons>
                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="btnSave_Click" 
                                Before="return #{PageFormPanel}.getForm().isValid();" 
                                Success="saveSuccessful();" Failure = "saveFailed();"/>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                    <Listeners>
                        <Click Handler="window.proxy.requestClose();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>
            <Listeners>
                <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
            </Listeners>
        </ext:FormPanel>
    </form>
</body>
</html>
