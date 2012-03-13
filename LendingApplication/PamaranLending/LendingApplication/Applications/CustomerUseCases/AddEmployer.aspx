﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEmployer.aspx.cs" Inherits="LendingApplication.Applications.CustomerUseCases.AddEmployer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Customer</title>
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
            showAlert('Status', 'Employer record was successfully created.', function () {
                    window.proxy.sendToAll('addemployer', 'addemployer');
                    window.proxy.requestClose();
            });
        }

        var provinceCheckerA1 = function () {
            if (radioCityA1.checked == true) {
                txtProvinceA1.allowBlank = true;
            }
            else if (radioMunicipalityA1.checked == true) {
                txtProvinceA1.allowBlank = false;
            }
            markIfRequired(txtProvinceA1);
        };

        var checkEmployer = function (result) {
            X.btnSavePersonNameDetails();
            X.checkEmployerName({
                success: function (result) {
                    if (result == 1) {
                        showConfirm('Message', 'An employer record with the same name already exists in the list. Do you want to create another employer record with the same name?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                EmployerID.setValue(-1);
                                hiddenExistingPartyRole.setValue(-1);
                            }
                            else {
                                var data = {};
                                data.id = hiddenID.getValue();
                                window.proxy.sendToParent(data, 'changeurltoedit');
                            }
                        });
                    } else if (result == 2) {
                        shwoConfirm('Message', 'A party record with the same name already exists. Do you want to use this record to create the new employer record?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                hiddenUsesExistRecord.setValue('yes');
                                X.FillAddress(hiddenExistingParty.getValue());
                            }
                            else {
                            }
                        });
                    }
                }
            });
        };

        var closeTab = function () {
            showConfirm('Confirm', 'Are you sure you want to close tab?',
                function (response) {
                    if (response == 'yes') {
                        window.proxy.requestClose();
                    } else {

                    }
                }
             );
        };

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
        
        .req
        {
            color:Red;
            font-weight:bold;
            font-size:larger;
        }
        
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="EmployerID" DataIndex="ID" runat="server" />
    <ext:Hidden ID="hiddenID" runat="server" />
    <ext:Hidden ID="hiddenExistingParty" DataIndex="ID" runat="server" />
    <ext:Hidden ID="hiddenExistingPartyRole" DataIndex="ID" runat="server" />
    <ext:Hidden ID="hiddenName" DataIndex="ID" runat="server" />
    <ext:Hidden ID="txtpartyId" runat="server" />
    <ext:Hidden ID="txtStreetAdd" runat="server" />
    <ext:Hidden ID="txtBarangay" runat="server" />
    <ext:Hidden ID="txtCity" runat="server" />
    <ext:Hidden ID="txtState" runat="server" />
    <ext:Hidden ID="txtPostal" runat="server" />
    <ext:Hidden ID="txtCountry" runat="server" />
    <ext:Hidden ID="txtMunicipality" runat="server" />
    <ext:Hidden ID="txtProvince" runat="server"/>
    <ext:Hidden ID="hiddenUsesExistRecord" runat="server"/>
    <ext:Store runat="server" ID="CountryStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="CountryTelephoneCode" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit" AutoHeight="true">
        <Items>
    <ext:FormPanel ID="RootPanel" runat="server" Layout="FitLayout" AutoHeight="true" Border="false"
        MonitorValid="true" MonitorPoll="500" >
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{RootPanel}.getForm().isValid();"
                                Success="saveSuccessful();">
                                <EventMask Msg="Saving..." ShowMask="true" />    
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="closeTab();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                    <ext:ToolbarSpacer />
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TabPanel ID="TabPanel1" runat="server" AutoHeight="true" Padding="0" HideBorders="true">
                <Items>
                    <ext:Panel ID="BasicInformationPanel" LabelWidth="160" Padding="5" 
                        runat="server" AutoHeight="true" Layout="FormLayout" Title="Basic Information">
                        <Items>
                            <ext:CompositeField ID="cmfPersonName" runat="server" Hidden="true"  AnchorHorizontal="60%">
                                <Items>
                                    <ext:TextField ID="txtPersonName" ReadOnly="true" FieldLabel="Name" AllowBlank="false"
                                        runat="server" Flex="1">
                                        <Listeners>
                                            <Focus Handler="#{wndPersonNameDetail}.show();" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:Button ID="btnNameDetail" Text="..." runat="server" Width="30">
                                        <Listeners>
                                            <Click Handler="#{wndPersonNameDetail}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField ID="txtOrganizationName" Hidden="true" runat="server"
                                FieldLabel="Name" AnchorHorizontal="60%" AllowBlank="false" ValidateDelay="200" >
                                <Listeners>
                                    <Change Delay="100" Handler="checkEmployer();" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="ContactInformationPanel" LabelWidth="160" Padding="5" 
                        runat="server" AutoHeight="true" Layout="FormLayout"  Title="Contact Information">
                        <Items>
                                <ext:CompositeField ID="cmfBusinessAddress" runat="server"
                                    LabelWidth="500" AnchorHorizontal="60%">
                                    <Items>
                                        <ext:TextField ID="txtBusinessAddress" DataIndex="BusinessAddress" runat="server" FieldLabel="Business Address"
                                            Flex="1" AllowBlank="false" >
                                            <Listeners>
                                                <Focus Handler="#{winAddressDetailA1}.show();" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:Button ID="btnBrowseAddress" runat="server" Width="70" Text="...">
                                        <Listeners>
                                            <Click Handler="#{winAddressDetailA1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    </Items>
                                </ext:CompositeField>
                                <ext:CompositeField LabelPad="10" ID="BusinessPhoneNumber" runat="server" FieldLabel="Business Phone Number"
                                    LabelWidth="500" AnchorHorizontal="60%">
                                    <items>
                                        <ext:TextField ID="txtTelCode" ReadOnly="true" runat="server" Width="75"/>
                                        <ext:DisplayField ID="DisplayField1" runat="server" Text="-" />
                                        <ext:TextField ID="nfAreaCode" MaxLength="4" MinLength="1" MaskRe="/\d/" runat="server" Width="75"/>
                                        <ext:DisplayField ID="DisplayField2" runat="server" Text="-" />
                                        <ext:TextField ID="nfPhoneNumber" MaskRe="/\d/" MinLength="5" MaxLength="7"  runat="server" Flex="1"/>    
                                    </items>
                                </ext:CompositeField>
                                <ext:CompositeField LabelPad="10" ID="BusinessFaxNumber" runat="server" FieldLabel="Business Fax Number"
                                    LabelWidth="500" AnchorHorizontal="60%">
                                    <items>
                                        <ext:TextField ID="txtFaxTelCode" ReadOnly="true" runat="server" Width="75"/>
                                        <ext:DisplayField ID="DisplayField3" runat="server" Text="-" />
                                        <ext:TextField ID="nfFaxAreaCode" MaxLength="4" MinLength="1" MaskRe="/\d/" runat="server" Width="75"/>
                                        <ext:DisplayField ID="DisplayField4" runat="server" Text="-" />
                                        <ext:TextField ID="nfFaxPhoneNumber" MaskRe="/\d/" MinLength="5" MaxLength="7" runat="server" Flex="1"/>    
                                    </items>
                                </ext:CompositeField>
                                <ext:TextField LabelPad="10" ID="txtBusinessEmailAddress" DataIndex="BusinessEmailAddress"
                                    runat="server" LabelWidth="500" FieldLabel="Business Email Address" AnchorHorizontal="60%"
                                    Vtype="email" />
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
    </Items>
    </ext:Viewport>
    <ext:Hidden ID="txtPersonTitle" runat="server" />
    <ext:Hidden ID="txtPersonFirstName" runat="server" />
    <ext:Hidden ID="txtPersonLastName" runat="server" />
    <ext:Hidden ID="txtPersonMiddleName" runat="server" />
    <ext:Hidden ID="txtPersonNickName" runat="server" />
    <ext:Hidden ID="txtPersonNameSuffix" runat="server" />
    <ext:Hidden ID="txtPersonMothersMaidenName" runat="server" />
    <ext:Hidden ID="txtStreetAdd1" runat="server" />
    <ext:Hidden ID="txtBarangay1" runat="server" />
    <ext:Hidden ID="txtCity1" runat="server" />
    <ext:Hidden ID="txtState1" runat="server" />
    <ext:Hidden ID="txtPostal1" runat="server" />
    <ext:Hidden ID="txtCountry1" runat="server" />
    <ext:Hidden ID="txtMunicipality1" runat="server" />
    <ext:Hidden ID="txtProvince1" runat="server" />
    <ext:Hidden ID="txtEmployerPartyType" runat="server" />
    <ext:Window ID="wndAddressDetail" Modal="true" runat="server" Collapsible="true" Height="310"
        Icon="Application" Title="Primary Address" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmAddressDetail1" LabelWidth="120"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtStreetNumberD1" FieldLabel="Street Address"
                        AnchorHorizontal="90%" AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden7" />
                    <ext:TextField runat="server" ID="txtBarangayD1" FieldLabel="Barangay" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden8" />
                    <ext:TextField runat="server" ID="txtMunicipalityD1" FieldLabel="Municipality" AnchorHorizontal="90%"
                        AllowBlank="true" >
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange" />
                        </DirectEvents>    
                    </ext:TextField>
                    <ext:TextField runat="server" ID="txtCityD1" FieldLabel="City" AnchorHorizontal="90%"
                        AllowBlank="true" >
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange" />
                        </DirectEvents>    
                    </ext:TextField>
                    <ext:Hidden runat="server" ID="hidden9" />
                    <ext:TextField runat="server" ID="txtProvinceD1" FieldLabel="Province" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden10" />
                    <ext:ComboBox runat="server" ID="cmbCountryD1" FieldLabel="Country" AnchorHorizontal="90%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:Hidden runat="server" ID="hidden11" />
                    <ext:TextField runat="server" ID="txtPostalCodeD1" MaxLength="4" MinLength="4" MaskRe="/\d/" FieldLabel="Postal Code" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden12" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar4" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Buttons>
                    <ext:Button runat="server" ID="btnDoneAddressDetail1" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnSaveAddress_Click" Before="return #{frmAddressDetail1}.getForm().isValid();" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancelSaveAdd1" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndAddressDetail1}.hide()" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar4}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && (#{txtMunicipalityD1}.getValue() != '' || #{txtCityD1}.getValue() != '')){#{btnDoneAddressDetail1}.enable();}  else{#{btnDoneAddressDetail1}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="wndPersonNameDetail" Modal="true" runat="server" Collapsible="true" Height="300"
        Icon="Application" Title="Employer Name" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmPersonNameDetail" LabelWidth="170"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtTitleP" FieldLabel="Title" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenTitleP" />
                    <ext:TextField runat="server" ID="txtFirstNameP" FieldLabel="First Name" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hiddenFirstNameP" />
                    <ext:TextField runat="server" ID="txtMiddleNameP" FieldLabel="Middle Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenMiddleNameP" />
                    <ext:TextField runat="server" ID="txtLastNameP" FieldLabel="Last Name" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:TextField runat="server" ID="txtMothersMaidenNameP" FieldLabel="Mother's Maiden Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenLastNameP" />
                    <ext:TextField runat="server" ID="txtNickNameP" FieldLabel="Nick Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenNickNameP" />
                    <ext:TextField runat="server" ID="txtNameSuffixP" FieldLabel="Name Suffix" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenNameSuffixP" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar2" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Buttons>
                    <ext:Button runat="server" ID="btnSavePersonNameDetail" Text="Done" Icon="Accept">
                        <Listeners>
                            <Click Delay="100" Handler="checkEmployer();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancelPersonNameDetail" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndPersonNameDetail}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar2}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSavePersonNameDetail}.enable();}  else{#{btnSavePersonNameDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddressDetailA1" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="310" Hidden="true"
        Title="Address Details" Width="520">
        <Items>
             <ext:FormPanel Padding="10" runat="server" ID="frmAddressA1" LabelWidth="120" MonitorValid="true">
                <Items>
                    <ext:TextField ID="txtStreetAddressA1" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:TextField ID="txtBarangayA1" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%"  AllowBlank="false">
                    </ext:TextField>
                    <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                        Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rdgProvince" ColumnWidth=".5">
                                <Listeners>
                                    <Change Handler ="provinceCheckerA1();" />
                                </Listeners>
                                <Items>
                                    <ext:Radio ID="radioMunicipalityA1" runat="server" BoxLabel="Municipality">
                                    </ext:Radio>
                                    <ext:Radio ID="radioCityA1" Checked="true" runat="server" BoxLabel="City">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipalityA1" runat="server" ColumnWidth=".5" Height="30" AllowBlank = "false">
                                <Listeners>
                                    <Focus Handler ="provinceCheckerA1();" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvinceA1" LabelSeparator=":" runat="server" FieldLabel="Province" AnchorHorizontal="100%" AllowBlank="true">
                    </ext:TextField>
                    <ext:ComboBox runat="server" ID="cmbCountryA1" FieldLabel="Country" AnchorHorizontal="100%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:TextField ID="txtPostalCodeA1" runat="server" FieldLabel="Postal Code" MaskRe="/\d/" MaxLength="4" MinLength="4" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                </Items>
                <BottomBar>
                <ext:StatusBar runat="server" ID = "statusWinAddressDetailA1">
                </ext:StatusBar>
                </BottomBar>
                <Buttons>
               <ext:Button runat="server" ID="btnDoneAddressDetailA1" Text="Done" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="btnDoneAddressDetailA1_DirectClick" Before="return #{frmAddressA1}.getForm().isValid();"/>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="btnCancelA1" Text="Cancel" Icon="Cancel">
                    <Listeners>
                            <Click Handler="#{winAddressDetailA1}.hide();" />
                    </Listeners>
                </ext:Button>
               </Buttons>
               <Listeners>
                    <ClientValidation Handler="#{statusWinAddressDetailA1}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetailA1}.enable();}  else{#{btnDoneAddressDetailA1}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
