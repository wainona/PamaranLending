<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBank.aspx.cs" Inherits="LendingApplication.Applications.BankUseCases.AddBank" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Bank</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('addbank', 'addbank');
                window.proxy.requestClose();
            });
        };

        var nameChanged = function () {
            showAlert('NameChange', 'Successfully added the record.');
        };
        var checkTelField = function () {
            if (txtAreaCode.getValue() != null) {
                txtPhoneNum.allowBlank = false;
                txtPhoneNum.validate();
            }
            if (txtPhoneNum.getValue() != null) {
                txtAreaCode.allowBlank = false;
                txtAreaCode.validate();
            }
        }
        var checkFaxField = function () {
            if (txtFaxAreaCode.getValue() != null) {
                txtFaxNum.allowBlank = false;
                txtFaxNum.validate();
            }
            if (txtFaxNum.getValue() != null) {
                checkFaxField.allowBlank = false;
                checkFaxField.validate();
            }
        }
        var provinceChecker = function () {
            if (radioCity.checked == true) {
                txtProvince1.allowBlank = true;
            }
            else if (radioMunicipality.checked == true) {
                txtProvince1.allowBlank = false;
            }
            markIfRequired(txtProvince1);
        }
        var checkBranch = function () {
            X.checkBranch({
                success: function (result) {
                    if (result == 1) {
                        showConfirm("Message", "Do you want to create  another bank record with the same name?", function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                hiddenPartyId.setValue('');
                            }
                            else {
                                var data = {};
                                data.id = hiddenID.getValue();
                                window.proxy.sendToParent(data, 'changeurltoedit');
                            }
                        });
                    }
                    else if (result == 2) {
                        showConfirm("Message", "Do you want to reuse existing record?", function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                hiddenPartyId.setValue(hiddenID.getValue());
                            }
                            else {
                                hiddenPartyId.setValue('');
                            }
                        });
                    }
                }
            });

        }
        var onBtnClose = function () {
            //delete all uploaded documents
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
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
       
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden ID="hiddenID" runat="server"></ext:Hidden>
     <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
      <Items>
    <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="300" MonitorValid="true">
    <Defaults>
   <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                Success="saveSuccessful();">
                                <EventMask ShowMask="true" Msg="Saving.." />
                                </Click> 
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TabPanel ID="TabPanel" runat="server" EnableTabScroll="true" Padding="0" HideBorders="true"
                AutoHeight="true">
                <Items>
                    <ext:Panel runat="server" ID="BankInformationPanel" Title="Bank Information" LabelWidth="80"
                        Padding="5" AutoHeight="true" Layout="FormLayout">
                        <Defaults>
                            <ext:Parameter Name="AllowBlank" Value="true" Mode="Raw" />
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:Hidden ID="hiddenPartyId" runat="server" />
                            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" Width="300"
                                AllowBlank="false" ValidateDelay="200">
                                <Listeners>
                                    <Change Delay="100" Handler="checkBranch();" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtAcronym" DataIndex="Acronym" runat="server" FieldLabel="Acronym"
                                 Width="300" AllowBlank="true">
                            </ext:TextField>
                            <ext:TextField ID="txtBranch" DataIndex="Branch" runat="server" FieldLabel="Branch"
                                 Width="300" AllowBlank="true">
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
                    <ext:Panel runat="server" ID="BankContactInformation" Title="Contact Information"
                        Layout="ColumnLayout" LabelWidth="200" Padding="5" AutoHeight="true">
                        <Defaults>
                            <ext:Parameter Name="AllowBlank" Value="true" Mode="Raw" />
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:Panel runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false"
                                Height="200">
                                <Items>
                                    <ext:Hidden ID="Hidden1" DataIndex="PartyRoleId" runat="server" />
                                    <ext:TextField ID="txtAddress" FieldLabel="Address" runat="server" AnchorHorizontal="99%"
                                        AllowBlank="false" ReadOnly="true">
                                        <Listeners>
                                        <Focus Handler="#{winAddressDetail}.show();" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:CompositeField ID="txtTelNum" runat="server" FieldLabel="Business Telephone Number"
                                        AnchorHorizontal="99%">
                                        <Items>
                                            <ext:TextField ID="txtCountryCode" runat="server" Flex="1" ReadOnly="true" />
                                            <ext:TextField ID="txtAreaCode" runat="server" Flex="1" AllowBlank="true" MaskRe="/\d/" MaxLength="3">
                                            </ext:TextField>
                                            <ext:TextField ID="txtPhoneNum" runat="server" Flex="1" AllowBlank="true" MaskRe="/\d/" MaxLength="7">
                                            </ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField runat="server" FieldLabel="Business Fax Number" AnchorHorizontal="99%">
                                        <Items>
                                            <ext:TextField ID="txtFaxCode" runat="server" Flex="1" ReadOnly="true" />
                                             <ext:TextField ID="txtFaxAreaCode" runat="server" Flex="1"  MaskRe="/\d/" MaxLength="3">
                                             </ext:TextField>
                                             <ext:TextField ID="txtFaxNum" runat="server" Flex="1" MaskRe="/\d/" MaxLength="7">
                                             </ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtEmailAdd" FieldLabel="Business Email Address" runat="server"
                                        Vtype="email" AnchorHorizontal="99%" AllowBlank="true" />
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ColumnWidth=".4" Layout="FormLayout" Border="false">
                                <Items>
                                    <ext:Button ID="btnBrowseAddress" runat="server" Text="Browse...">
                                        <Listeners>
                                            <Click Handler="#{winAddressDetail}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="StatusBar1" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
        </Listeners>
    </ext:FormPanel>
    <ext:Window ID="winAddressDetail" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="330" Hidden="true"
        Title="Address Details" Width="520">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmAddress" LabelWidth="120" MonitorValid="true">
            <Defaults>
             <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
                <Items>
                    <ext:Hidden runat="server" ID="hiddenStreetName" />
                    <ext:TextField runat="server" ID="txtStreet" FieldLabel="Street" AnchorHorizontal="90%" AllowBlank="true" />
                    <ext:TextField runat="server" ID="txtBarangay1" FieldLabel="Barangay" AnchorHorizontal="90%" AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hiddenBarangay" />
                    <ext:Hidden runat="server" ID="hiddenCity"></ext:Hidden>
                    <ext:Hidden runat="server" ID="hiddenMunicipality"></ext:Hidden>
                    <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                        Height="50" Padding="1" AnchorHorizontal="90%">
                        <Items>
                            <ext:RadioGroup runat="server" ID="zzz" ColumnWidth=".5">
                                <Listeners>
                                    <Change Handler="provinceChecker();" />
                                </Listeners>
                                <Items>
                                    <ext:Radio ID="radioCity" runat="server" BoxLabel="City" Checked="true">
                                    </ext:Radio>
                                    <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipality" AllowBlank="false" runat="server" ColumnWidth=".5" Height="30">
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField runat="server" ID="txtProvince1" FieldLabel="Province" AnchorHorizontal="90%"/>
                    <ext:Hidden runat="server" ID="hiddenProvince" />
                    <ext:ComboBox runat="server" ID="cmbCountry" FieldLabel="Country" AnchorHorizontal="90%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false">
                        <Store>
                            <ext:Store runat="server" ID="storeCountry" RemoteSort="false">
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
                    <ext:Hidden runat="server" ID="hiddenCountry" />
                    <ext:TextField runat="server" ID="txtPostalCode" FieldLabel="Postal Code" AnchorHorizontal="90%" AllowBlank="false"  MaskRe="/\d/" MaxLength="4" />
                    <ext:Hidden runat="server" ID="hiddenPostalCode" />
                      
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar2" Text="Hellow" runat="server" Height="25" />
               </BottomBar>
               <Buttons>
               <ext:Button runat="server" ID="btnDoneAddressDetail" Text="Done" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="btnDoneAddressDetail_DirectClick" Before="return #{frmAddress}.getForm().isValid();"/>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="btnCancelAddressDetail" Text="Cancel" Icon="Cancel"
                    OnDirectClick="btnCancelAddressDetail_DirectClick" />
               </Buttons>
               <Listeners>
                    <ClientValidation Handler="#{StatusBar2}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetail}.enable();}  else{#{btnDoneAddressDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
