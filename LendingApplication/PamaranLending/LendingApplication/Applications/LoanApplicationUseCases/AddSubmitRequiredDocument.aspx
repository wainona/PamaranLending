<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSubmitRequiredDocument.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.AddSubmitRequiredDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Submit Required Document</title>
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
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Document successfully submitted.', function () {
                window.proxy.sendToAll('addrequireddocument', 'addrequireddocument');
                window.proxy.requestClose();
            });
        }

        var successUpload = function () {
            wdwNewDocumentPage.hide();
            fileUpload.clear();
            BasicForm.reset();
        }

        var openUploadWindow = function () {
            fileUpload.clear();
            wdwNewDocumentPage.show();
        }

        //Trim the input text
        function Trim(input) {
            var lre = /^\s*/;
            var rre = /\s*$/;
            input = input.replace(lre, "");
            input = input.replace(rre, "");
            return input;
        }

        var onUpload = function (fb, fileName) {
            var result = false;
            //clear contents
            //Checking for file browsed or not 
            if (Trim(fileName) == '') {
                fileUpload.focus();
                showAlert('Information', 'Please select a file to upload!!!');
                hdnPath.setValue('0');
            }

            //Setting the extension array for diff. type of text files 
            var extArray = new Array(".jpg", ".jpeg", ".bmp",
                                     ".png", ".gif", ".tiff");

            //getting the file name
            while (fileName.indexOf("\\") != -1)
                fileName = fileName.slice(fileName.indexOf("\\") + 1);

            //Getting the file extension                     
            var ext = fileName.slice(fileName.indexOf(".")).toLowerCase();

            //matching extension with our given extensions.
            for (var i = 0; i < extArray.length; i++) {
                if (extArray[i] == ext) {
                    result = true;
                    break;
                }
            }

            if (result == false) {
                showAlert("Error", "Document page cannot be uploaded because the file has an invalid file type." +
                "<br/>Please only upload files that end in types:  (" + (extArray.join("  ")) +
                ").<br/>Please select a new file and upload again.");
                fileUpload.clear();
            }
            fileUpload.focus();
        }

        var onFormValidated = function (valid) {
            //this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && #{DocumentPageGridPanel}.store.getCount()){#{btnSubmit}.enable();}  else{#{btnSubmit}.disable();}
            var count = DocumentPageGridPanel.store.getCount();
            valid = valid && (count > 0);
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSubmit.enable();
            }
            else if (count == 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Please add document.', iconCls: 'icon-exclamation' });
                btnSubmit.disable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please completely fill out the forms.', iconCls: 'icon-exclamation' });
                btnSubmit.disable();
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
        <ext:Hidden ID="hdnPage" runat="server"></ext:Hidden>
    <ext:Store runat="server" ID="storeDocumentPage" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" MonitorValid="true" Title="Submitted Document"
                BodyStyle="background-color:transparent" LabelWidth="150" Padding="5"  MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSubmit" runat="server" Text="Submit" Icon="Tick">
                                <DirectEvents>
                                    <Click OnEvent="btnSubmit_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ctl1014">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Hidden ID="hdnDocumentId" runat="server" />
                    <ext:ComboBox runat="server" ID="cmbDocumentName" FieldLabel="Document Name" Width="400"
                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                        SelectOnFocus="true" AllowBlank="false" ValueField="Id" DisplayField="Name" StoreID="storeDocumentPage" />
                    <ext:DateField ID="dtDateSubmitted" runat="server" FieldLabel="Date Submitted" Width="400"
                        AllowBlank="false" Editable="false" />
                    <ext:TextArea ID="txtDocDescription" runat="server" FieldLabel="Description" Width="400"
                        MaxLength="2556">
                    </ext:TextArea>
                    <ext:GridPanel ID="DocumentPageGridPanel" runat="server" Height="600" MinHeight="200"
                        Title="Document Page" AutoExpandColumn="FileName">
                        <View>
                            <ext:GridView runat="server" EmptyText="No uploaded documents to display" DeferEmptyText="false"></ext:GridView>
                        </View>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Button ID="btnNew" runat="server" Text="New" Icon="Add">
                                        <Listeners>
                                            <Click Handler="openUploadWindow();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnDelete" runat="server" Text="Delete Last" Icon="Delete" Disabled="false">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_Click">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store runat="server" ID="strDocumentPage" RemoteSort="false">
                                <Reader>
                                    <ext:JsonReader IDProperty="RandomKey">
                                        <Fields>
                                            <ext:RecordField Name="PageNumber" />
                                            <ext:RecordField Name="FileName" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModelDocuments" SingleSelect="true" runat="server">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ColumnModel runat="server" ID="clmModelPayOutstandingLoan" Width="100%">
                            <Columns>
                                <ext:Column Header="Page Number" DataIndex="PageNumber" Locked="true" Wrap="true"
                                    Width="120" />
                                <ext:Column Header="File Name" DataIndex="FileName" Locked="true" Wrap="true" Width="120" />
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
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
    <ext:Window ID="wdwNewDocumentPage" runat="server" Hidden="true" Padding="10" Width="500"
        AutoHeight="true" Title="Upload New Document">
        <Items>
            <ext:FormPanel ID="BasicForm" runat="server" Frame="true" MonitorValid="true" PaddingSummary="10px 10px 0 10px"
                LabelWidth="50"  MonitorPoll="500">
                <Defaults>
                    <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                    <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                    <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                </Defaults>
                <Items>
                     <ext:TextField ID="txtDocumentName" runat="server" FieldLabel="Name" />
                    <ext:FileUploadField ID="fileUpload" runat="server" EmptyText="Select an image"
                        FieldLabel="Photo" ButtonText="" Icon="ImageAdd" >
                        <Listeners>
                             <FileSelected Fn="onUpload" />
                        </Listeners>
                        </ext:FileUploadField>
                </Items>
                <Listeners>
                    <ClientValidation Handler="#{SaveButton}.setDisabled(!valid);" />
                </Listeners>
                <Buttons>
                    <ext:Button ID="SaveButton" runat="server" Text="Save">
                        <DirectEvents>
                            <Click OnEvent="UploadClick"
                            Before="Ext.Msg.wait('Uploading your photo...', 'Uploading');"
                            Success="successUpload();"                          
                            Failure="Ext.Msg.show({ 
                                title   : 'Error', 
                                msg     : 'Error during uploading', 
                                minWidth: 200, 
                                modal   : true, 
                                icon    : Ext.Msg.ERROR, 
                                buttons : Ext.Msg.OK 
                            });">
                            <EventMask ShowMask="true" Msg="Uploading file..." />
                        </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="btnReset" runat="server" Text="Reset">
                        <Listeners>
                            <Click Handler="#{BasicForm}.getForm().reset();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
