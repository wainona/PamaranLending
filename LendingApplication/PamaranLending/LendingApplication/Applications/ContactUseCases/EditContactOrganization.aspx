<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditContactOrganization.aspx.cs"
    Inherits="LendingApplication.Applications.ContactUseCases.EditContactOrganization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Contact record was successfully modified', function () {
                window.proxy.sendToAll('editcontactorganization', 'editcontactorganization');
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

            txtTelephoneNumber.validate();
        }

        var checkFax = function () {
            var areaCode = txtFaxAreacode.getValue();
            var number = txtFaxNumber.getValue();
            if (parseFloat(areaCode) >= 0) {
                txtFaxAreacode.allowBlank = false;
            } else {
                txtFaxAreacode.allowBlank = true;
            }

            txtFaxAreacode.validate();
            txtFaxNumber.validate();
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
            if (btnEdit.hidden == true) {
                StatusBar1.show();
                if (valid) {
                    StatusBar1.setStatus({ text: 'Form is valid. ' });
                    btnSave.enable();
                }
                else {
                    StatusBar1.setStatus({ text: 'Please fill out the form.' });
                    btnSave.disable();
                }
            } else {
                btnSave.disable();
                StatusBar1.hide();
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
            opacity: 0.30;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
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
            <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="500"  MonitorPoll = "500" MonitorValid="true"
                Layout="FitLayout">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Hidden="true" Icon="NoteEdit">
                                <DirectEvents>
                                    <Click OnEvent="btnOpen_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                <DirectEvents>
                                    <Click OnEvent="btnEdit_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Disabled = "true">
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
                            <ext:Panel runat="server" ID="PanelBasicInfo" Title="Basic Information" Padding = "5" LabelWidth="150"
                                 Height="380" Layout="ColumnLayout">
                                <Items>
                                    <ext:Panel runat="server" ID = "Panel1" ColumnWidth=".6" Border="false" LabelWidth = "150" Padding = "10" Layout="FormLayout" Disabled="true" Cls="ext-hide-mask">
                                        <Items>
                                            <ext:Hidden ID="RecordId" DataIndex="ID" runat="server" />
                                            <ext:TextField ID="txtName" DataIndex="Name" runat="server" FieldLabel="Name" AnchorHorizontal="60%"
                                                AllowBlank="false"/>
                                            <ext:ComboBox runat="server" ID="cmbOrganizationType" FieldLabel="Organization Type"
                                                AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" Editable="false"
                                                TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeOrganizationType" RemoteSort="false">
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
                                            <ext:DateField ID="dtDateEstablished" DataIndex="DateEstablished" runat="server"
                                                FieldLabel="Date Established" AnchorHorizontal="25%" Editable="false"
                                                Vtype="daterange">
                                            </ext:DateField>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelContactInformation" Title="Contact Information"
                                LabelWidth="150" Height="380" Layout="ColumnLayout">
                                <Items>
                                    <ext:Panel runat="server" ID = "Panel2" Layout="FormLayout" ColumnWidth=".6" StyleSpec = "padding-top:10px; padding-right:0px; padding-left:10px;" Border="false" Disabled="true" Cls="ext-hide-mask">
                                        <Items>
                                            <ext:CompositeField runat="server">
                                                <Items>
                                                    <ext:TextField ID="txtBusinessAddress" DataIndex="BusinessAddress" runat="server"
                                                    FieldLabel="Business Address" Width="400" AllowBlank="false"
                                                    ReadOnly="true">
                                                        <Listeners>
                                                            <Focus Handler="#{winAddressDetail}.show();" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:Button runat="server" ID="btnBrowseAddress" Cls="bold" Text="..." Disabled = "true">
                                                        <Listeners>
                                                            <Click Handler="#{winAddressDetail}.show();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="Telephone" runat="server" FieldLabel="Telephone Number">
                                                <Items>
                                                    <ext:TextField runat="server" ID="txtTelephoneCode" ReadOnly="true" MaxLength="3" Width="70"/>
                                                    <ext:TextField runat="server" ID="txtTelephoneAreaCode" MaxLength="3" MaskRe = "/\d/" Width="70">
                                                    <Listeners>
                                                     <Blur Handler = "checkTelephone();" />
                                                    </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField runat="server" ID="txtTelephoneNumber" MaxLength = "8" MaskRe = "/[0-9\-]/" Width="250">
                                                    <Listeners>
                                                    <Blur Handler = "checkTelephone();" />
                                                    </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="Fax" runat="server" FieldLabel="Fax Number">
                                                <Items>
                                                    <ext:TextField runat="server" ID="txtFaxCode" ReadOnly="true" MaxLength="3" Width="70"/>
                                                    <ext:TextField runat="server" ID="txtFaxAreacode" MaxLength="3" MaskRe = "/\d/" Width="70">
                                                    <Listeners>
                                                    <Blur Handler = "checkFax();" />
                                                    </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField runat="server" ID="txtFaxNumber" MaxLength = "8" MaskRe = "/[0-9\-]/" Width="250">
                                                    <Listeners>
                                                    <Blur Handler = "checkFax();" />
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
            <ext:StatusBar ID="StatusBar1" runat="server" Height="25" Hidden="true"/>
                </BottomBar>
                <Listeners>
            <ClientValidation Handler="onFormValidated(valid);" />
        </Listeners>
            </ext:FormPanel>
            <%--Pop up window--%>
            <ext:Window ID="winAddressDetail" Modal="true" Draggable="false" Resizable="false"
                Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="310" Hidden="true"
                Title="Address Details" Width="520">
                <Items>
                    <ext:FormPanel Padding="10" runat="server" ID="frmAddress" LabelWidth="120" MonitorValid="true">
                        <Items>
                            <ext:TextField ID="txtStreetAddress" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%">
                            </ext:TextField>
                            <ext:TextField ID="txtBarangay" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%">
                            </ext:TextField>
                            <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                                Height="50" Padding="1">
                                <Items>
                                    <ext:RadioGroup runat="server" ID="zzz" ColumnWidth=".5">
                                    <Listeners>
                                    <Change Handler = "provinceChecker();" />
                                    </Listeners>
                                        <Items>
                                            <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality">
                                            </ext:Radio>
                                            <ext:Radio ID="radioCity" runat="server" BoxLabel="City">
                                            </ext:Radio>
                                        </Items>
                                    </ext:RadioGroup>
                                    <ext:TextField ID="txtCityOrMunicipality" runat="server" ColumnWidth=".5" Height="30" AllowBlank = "false">
                                    </ext:TextField>
                                </Items>
                            </ext:FieldSet>
                            <ext:TextField ID="txtProvince" runat="server" FieldLabel="Province" AnchorHorizontal="100%">
                            </ext:TextField>
                            <ext:TextField ID="txtState" Hidden ="true" runat="server" FieldLabel="State" AnchorHorizontal="100%">
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
                            <ext:TextField ID="txtPostalCode" runat="server" FieldLabel="Postal Code" AnchorHorizontal="100%" MaxLength="4" MaskRe="/\d/">
                            </ext:TextField>
                        </Items>
                        <BottomBar>
                            <ext:StatusBar runat="server" ID="statusWinAddressDetail">
                            </ext:StatusBar>
                        </BottomBar>
                        <Buttons>
                            <ext:Button runat="server" ID="btnDoneAddressDetail" Text="Done" Icon="Accept" Disabled = "true">
                                <DirectEvents>
                                    <Click OnEvent="btnDoneAddressDetail_DirectClick" Before="return #{frmAddress}.getForm().isValid();" />
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
            <ext:Hidden runat="server" ID="hiddenPartyRoleID" />
            <ext:Hidden runat="server" ID="hiddenCountry" />
            <ext:Hidden runat="server" ID="hiddenStreet" />
            <ext:Hidden runat="server" ID="hiddenCity" />
            <ext:Hidden runat="server" ID="hiddenMunicipality" />
            <ext:Hidden runat="server" ID="hiddenProvince" />
            <ext:Hidden runat="server" ID="hiddenState" />
            <ext:Hidden runat="server" ID="hiddenPostalCode" />
            <ext:Hidden runat="server" ID="hiddenBarangay" />
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
