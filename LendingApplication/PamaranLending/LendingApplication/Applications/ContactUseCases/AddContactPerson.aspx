<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddContactPerson.aspx.cs"
    Inherits="LendingApplication.Applications.ContactUseCases.AddContactPerson" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Contact Person</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();

        });

        var saveSuccessful = function () {
            showAlert('Status', 'Contact record was successfully added', function () {
                window.proxy.sendToAll('addcontactperson', 'addcontactperson');
                window.proxy.requestClose();
            });
        }

        var provinceChecker = function () {
            if (radioCity.checked == true) {
                txtProvince.allowBlank = true;
            }
            else if (radioMunicipality.checked == true) {
                txtProvince.allowBlank = false;
            }
            markIfRequired(txtProvince);
        }

        var checkTelephone = function () {
            var areaCode = txtTelephoneAreaCode.getValue();
            var number = txtTelephoneNumber.getValue();
            if (parseFloat(areaCode) >= 0) {
                txtTelephoneNumber.allowBlank = false;
            } else {
                txtTelephoneNumber.allowBlank = true;
            }

            txtTelephoneAreaCode.validate();
            txtTelephoneNumber.validate();
        }

        var checkCellphone = function () {
            var areaCode = txtCellphoneAreaCode.getValue();
            var number = txtCellphoneNumber.getValue();
            if (parseFloat(areaCode) >= 0) {
                txtCellphoneNumber.allowBlank = false;
            } else {
                txtCellphoneNumber.allowBlank = true;
            }

            txtCellphoneAreaCode.validate();
            txtCellphoneNumber.validate();
        }

        var onDateChanged = function () {
            var today = new Date;
            var status = "Retired";
            if (dtIntroductionDate.getValue() > today)
                status = "Inactive";
            else if (dtIntroductionDate.getValue() <= today
                && (dtSalesDiscontinuationDate.getValue() == ''
                || dtSalesDiscontinuationDate.getValue() > today))
                status = "Active";

            txtProductStatus.setValue(status);
        }

        Ext.apply(Ext.form.VTypes, {
            numberrange: function (val, field) {
                if (!val) {
                    return;
                }

                if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
                    var start = Ext.getCmp(field.startNumberField);

                    if (start) {
                        start.setMaxValue(val);
                        field.numberRangeMax = val;
                        start.validate();
                    }
                } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
                    var end = Ext.getCmp(field.endNumberField);

                    if (end) {
                        end.setMinValue(val);
                        field.numberRangeMin = val;
                        end.validate();
                    }
                }

                return true;
            }
        });

        var onFormValidated = function (valid) {
            //this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && #{PageGridPanel}.store.getCount()){#{btnSave}.enable();}  else{#{btnSave}.disable();}
            btnSave.disable();
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
            }
        }

        var checkPersonName = function () {
            X.btnDonePersonName_Click();
            X.checkPersonName(hiddenName.getValue(), {
                success: function (result) {
                    if (result == 1) {
                        showConfirm("Message", "Do you want to create  another contact record with the same name?", function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                RecordId.setValue('');
                            }
                            else {
                                var data = {};
                                data.id = hiddenID.getValue();
                                window.proxy.sendToParent(data, 'changeurltoeditperson');
                            }
                        });
                    }
                }
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
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
          .ext-hide-mask .ext-el-mask
        {
            opacity: 0.04;
        }
        .bold .x-btn-text
        {
            font-weight:bold;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Store runat="server" ID="TimeUnitStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="500" MonitorValid="true" MonitorPoll = "500"
                Layout="FitLayout">
                <Defaults>
                <ext:Parameter Name="AllowBlank" Value="true" Mode="Raw" />
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
        <TopBar>
            <ext:Toolbar runat="server" ID="tlbView">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Success="saveSuccessful();">
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
            <ext:TabPanel ID="PageTabPanel" runat="server" EnableTabScroll="true" Padding="0"
                HideBorders="true">
                <Items>
                   <ext:Panel runat="server" ID="PanelBasicInfo" Title="Basic Information" LabelWidth="80"
                                Padding="5" Height="380" Layout="ColumnLayout">
                        <Items>
                            <ext:Panel runat="server" Border="false" Layout="FormLayout" StyleSpec = "padding-top:5px; padding-right:0px; padding-left:0px;" Width="445">
                                <Items>
                                    <ext:Hidden ID="RecordId" DataIndex="ID" runat="server" />
                                    <ext:TextField ID="txtName" DataIndex="Name"  ValidateDelay="100" runat="server" FieldLabel="Name" Width="350" AllowBlank="false" ReadOnly="true">
                                        <Listeners>
                                            <Focus Handler="#{winPersonName}.show();" />
                                            <Change Delay = "100" Handler = "checkPersonName();" /> 
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:RadioGroup ID="radGender" Width="120" FieldLabel="Gender" runat="server" AllowBlank="false">
                                        <Items>
                                            <ext:Radio ID="radMale" BoxLabel="Male" runat="server">
                                            </ext:Radio>
                                            <ext:Radio ID="radFemale" BoxLabel="Female" runat="server">
                                            </ext:Radio>
                                        </Items>
                                    </ext:RadioGroup>
                                    <ext:DateField ID="dtBirthdate" DataIndex="Birthdate" runat="server" FieldLabel="Birth Date"
                                        Width="350" Editable="false" Vtype="daterange">
                                    </ext:DateField>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ColumnWidth=".2" Border="false" Layout="FormLayout" StyleSpec = "padding-top:5px; padding-right:0px; padding-left:1px;">
                                <Items>
                                    <ext:Button ID="btnBrowseName" Cls="bold" Text="..." runat="server">  
                                        <Listeners>
                                            <Click Handler="#{winPersonName}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                   <ext:Panel runat="server" ID="PanelContactInformation" Title="Contact Information"
                                LabelWidth="150" Padding="5" Height="380" Layout="ColumnLayout">
                        <Items>
                            <ext:Panel runat="server" Layout="FormLayout" ColumnWidth=".8" StyleSpec = "padding-top:5px; padding-right:0px; padding-left:0px;" Border="false">
                                <Items>
                                 <ext:CompositeField ID="CompositeField1" runat="server">
                                    <Items>
                                        <ext:TextArea ID="txtHomeAddress" DataIndex="HomeAddress" runat="server" FieldLabel="Home Address"
                                            AllowBlank="false" ReadOnly="true" Width="400">
                                            <Listeners>
                                                <Focus Handler="#{winAddressDetail}.show();" />
                                            </Listeners>
                                        </ext:TextArea>
                                        <ext:Button runat="server" ID="btnBrowseAddress" Cls="bold" Text="...">
                                            <Listeners>
                                                <Click Handler="#{winAddressDetail}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField ID="Cellphone" runat="server" FieldLabel="Cellphone Number">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtCellphoneCode" ReadOnly="true" MaxLengthText="3" Width="70">
                                            <Listeners>
                                            <Change Handler = "checkCellphone();" />
                                            </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server" ID="txtCellphoneAreaCode" MaxLength="3" MaskRe = "/\d/" Width="70">
                                            <Listeners>
                                            <Change Handler = "checkCellphone();" />
                                            </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server" ID="txtCellphoneNumber" MaxLength="8" MaskRe = "/\d/" Width="250">
                                            </ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField ID="Telephone" runat="server" FieldLabel="Telephone Number">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtTelephoneCode" ReadOnly="true" MaxLength="3"
                                                MaxLengthText="3" Width="70"></ext:TextField>
                                            <ext:TextField runat="server" ID="txtTelephoneAreaCode" MaxLength="3" MaskRe="/\d/" Width="70">
                                                <Listeners>
                                                    <Change Handler = "checkTelephone();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:TextField runat="server" ID="txtTelephoneNumber" MaxLength="8" MaskRe="/[0-9\-]/" Width="250">
                                            <Listeners>
                                            <Change Handler = "checkTelephone();" />
                                            </Listeners>
                                            </ext:TextField>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField runat="server" ID="txtEmailAddress" Vtype="email" FieldLabel="Email Address" Width="400">
                                    </ext:TextField>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
        </Items>
        <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler = "onFormValidated(valid);" />
                </Listeners>
    </ext:FormPanel>
    <%--Pop up window--%>
    <ext:Window ID="winPersonName" Modal="true" Draggable="false" Resizable="false" Icon="ApplicationFormEdit"
        runat="server" Collapsible="false" Height="300" Hidden="true" Title="Person Name"
        Width="520">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmPersonName" LabelWidth="150" MonitorValid = "true">
                <Items>
                    <ext:ComboBox runat="server" ID="cmbTitle" FieldLabel="Title" AnchorHorizontal="90%"
                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                        SelectOnFocus="true">
                        <Items>
                            <ext:ListItem Text="Mr." />
                            <ext:ListItem Text="Ms." />
                            <ext:ListItem Text="Mrs." />
                        </Items>
                    </ext:ComboBox>
                    <ext:Hidden runat="server" ID="hiddenTitle" />
                    <ext:Hidden runat="server" ID = "hiddenID" />
                    <ext:TextField runat="server" ID="txtFirstName" FieldLabel="First Name" AnchorHorizontal="90%" AllowBlank="false"/>
                    <ext:Hidden runat="server" ID="hiddenFirstName" />
                    <ext:TextField runat="server" ID="txtLastName" FieldLabel="Last Name" AnchorHorizontal="90%" AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hiddenLastName" />
                    <ext:TextField runat="server" ID="txtMiddleName" FieldLabel="Middle Name" AnchorHorizontal="90%" />
                    <ext:Hidden runat="server" ID="hiddenMiddleName" />
                    <ext:TextField runat="server" ID="txtNickName" FieldLabel="Nick Name" AnchorHorizontal="90%" />
                    <ext:Hidden runat="server" ID="hiddenNickName" />
                    <ext:TextField runat="server" ID="txtNameSuffix" FieldLabel="Name Suffix" AnchorHorizontal="90%" />
                    <ext:Hidden runat="server" ID="hiddenNameSuffix" />
                    <ext:TextField runat="server" ID="txtMothersMaidenName" FieldLabel="Mother's Maiden Name"
                        AnchorHorizontal="90%"/>
                    <ext:Hidden runat="server" ID="hiddenMaidenName" />
                </Items>
               <BottomBar>
                <ext:StatusBar runat="server" ID = "StatusBar1">
                </ext:StatusBar>
                </BottomBar>
                <Buttons>
               <ext:Button runat="server" ID="btnDonePersonName" Text="Done" Icon="Accept">
                   <Listeners>
                            <Click Delay="100" Handler="checkPersonName();" />
                   </Listeners>
                </ext:Button>
                <ext:Button runat="server" ID="btnCancelPersonName" Text="Cancel" Icon="Cancel"
                    OnDirectClick="btnCancelPersonName_DirectClick" />
               </Buttons>
               <Listeners>
                    <ClientValidation Handler="#{StatusBar1}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDonePersonName}.enable();}  else{#{btnDonePersonName}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddressDetail" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="310" Hidden="true"
        Title="Address Details" Width="520">
        <Items>
             <ext:FormPanel Padding="10" runat="server" ID="frmAddress" LabelWidth="120" MonitorValid="true">
                <Items>
                    <ext:TextField ID="txtStreetAddress" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:TextField ID="txtBarangay" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%"  AllowBlank="false">
                    </ext:TextField>
                    <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                        Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="zzz" ColumnWidth=".5">
                            <Listeners>
                            <Change Handler ="provinceChecker();" />
                            </Listeners>
                                <Items>
                                    <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality">
                                    </ext:Radio>
                                    <ext:Radio ID="radioCity" runat="server" Checked="true" BoxLabel="City">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipality" runat="server" ColumnWidth=".5" Height="30" AllowBlank = "false">
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvince" runat="server" FieldLabel="Province" AnchorHorizontal="100%" AllowBlank="true">
                    </ext:TextField>
                    <ext:TextField ID="txtState" Hidden = "true" runat="server" FieldLabel="State" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:ComboBox runat="server" ID="cmbCountry" FieldLabel="Country" AnchorHorizontal="100%"
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
                    <ext:TextField ID="txtPostalCode" runat="server" FieldLabel="Postal Code" AnchorHorizontal="100%" AllowBlank="false" MaxLength="4" MaskRe="/\d/">
                    </ext:TextField>
                </Items>
                <BottomBar>
                <ext:StatusBar runat="server" ID = "statusWinAddressDetail">
                </ext:StatusBar>
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
                    <ClientValidation Handler="#{statusWinAddressDetail}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetail}.enable();}  else{#{btnDoneAddressDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Hidden runat="server" ID = "hiddenName" />
    <ext:Hidden runat="server" ID = "hiddenCountry" />
    <ext:Hidden runat="server" ID = "hiddenStreet" />
    <ext:Hidden runat="server" ID = "hiddenCity" />
    <ext:Hidden runat="server" ID = "hiddenMunicipality" />
    <ext:Hidden runat="server" ID  = "hiddenProvince" />
    <ext:Hidden runat="server" ID = "hiddenState" />
    <ext:Hidden runat="server" ID = "hiddenPostalCode" />
    <ext:Hidden runat="server" ID = "hiddenBarangay" />
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
