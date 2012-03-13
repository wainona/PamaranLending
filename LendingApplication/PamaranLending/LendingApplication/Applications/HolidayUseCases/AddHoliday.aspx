<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddHoliday.aspx.cs" Inherits="LendingApplication.Applications.HolidayUseCases.AddHoliday" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Holiday</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Holiday record has been created successfully.', function () {
                window.proxy.sendToAll('addholiday', 'addholiday');
                window.proxy.requestClose();
            });
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
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
    <ext:FormPanel ID="PageFormPanel" 
        runat="server" 
        Padding="5" 
        ButtonAlign="Right" 
        MonitorValid="true"
        Width="600" 
        Height="400"
        Title="Holiday Details"
        BodyStyle="background-color:transparent"
        Layout="FormLayout">
        <Items>
            <ext:DateField ID="dtHoliday" FieldLabel = "Date" runat="server" AnchorHorizontal = "40%" AllowBlank = "false" Format = "MM/dd/yyyy" Editable = "false">
            </ext:DateField>
            <ext:TextField ID="txtName" DataIndex="Name" runat="server" FieldLabel="Name" AnchorHorizontal = "40%" AllowBlank = "false" />
            <ext:TextArea ID="txtDesciption" DataIndex = "Description"  FieldLabel = "Description" AnchorHorizontal = "40%"
             runat="server"/>
             <ext:TextArea ID="txtNotes" DataIndex = "Notes" FieldLabel = "Notes"  AnchorHorizontal = "40%"
             runat="server"/>
        </Items>
        <TopBar>
        <ext:Toolbar runat="server">
        <Items>
         <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="btnSave_Click" 
                            Before="return #{PageFormPanel}.getForm().isValid();" 
                            Success="saveSuccessful();">
                            <EventMask ShowMask="true" Msg="Saving.." />
                            </Click>
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
