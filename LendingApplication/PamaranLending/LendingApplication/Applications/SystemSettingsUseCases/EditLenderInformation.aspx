<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditLenderInformation.aspx.cs"
    Inherits="LendingApplication.Applications.FinancialManagement.SystemSettingsUseCases.EditLenderInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Customer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully update the Lender information.', function () {
                window.proxy.sendToAll('updatecustomer', 'updatecustomer');
                window.proxy.requestClose();
            });
        }

        var wndAddressDetail_btnCancel_Click = function () {
            this.wndAddressDetail.hide();
        }

        var checkCityorMunicipality = function () {
            if (radioCity.checked) {
                txtProvince.allowBlank = true;
            } else if (radioMunicipality.checked) {
                txtProvince.allowBlank = false;
            }
            markIfRequired(txtProvince);
        };

    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 5px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport runat="server" Layout="Fit">
        <Items>
    <ext:Hidden runat="server" ID="HiddenTempPartyId"></ext:Hidden>
    <ext:FormPanel ID="PageFormPanel" runat="server" Padding="0" ButtonAlign="Right"
        MonitorValid="true" MonitorPoll="500" BodyStyle="background-color:transparent"
        Layout="Fit" LabelWidth="150">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                Success="saveSuccessful();" >
                                <EventMask ShowMask="true" Msg="Saving lender information..." />
                                </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="Button4" runat="server" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TabPanel ID="TabPanel" runat="server" Padding="5" BodyStyle="background-color:transparent" AutoHeight="true">
                <Items>
                    <ext:Panel ID="LenderPanel" runat="server" Layout="FormLayout" Title="Lender Information"
                        LabelWidth="150">
                        <Items>
                            <ext:Hidden runat="server" ID="HiddenPartyId"></ext:Hidden>
                            <ext:TextField ID="txtName" runat="server" FieldLabel="Name" AnchorHorizontal="60%" AllowBlank="false"/>
                            <ext:ComboBox ID="cmbOrganizationType" runat="server" FieldLabel="Organization Type"
                                AnchorHorizontal="60%" Editable="false" ValueField="Id" DisplayField="Name" AllowBlank="false">
                                <Store>
                                    <ext:Store ID="strOrganizationType" runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="Name" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                </ext:ComboBox>
                            <ext:DateField ID="dfDateEstablished" runat="server" FieldLabel="Date Established"
                                Editable="false" AnchorHorizontal="60%" AllowBlank="false">
                            </ext:DateField>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="ContactPanel" runat="server" Title="Contact Information" Layout="FormLayout"
                        LabelWidth="200">
                        <Items>
                            <ext:Hidden ID="HiddenCountry" runat="server" />
                            <ext:Hidden ID="HiddenStreetAddress" runat="server" />
                            <ext:Hidden ID="HiddenBarangay" runat="server" />
                            <ext:Hidden ID="HiddenMunicipality" runat="server" />
                            <ext:Hidden ID="HiddenCity" runat="server" />
                            <ext:Hidden ID="HiddenProvince" runat="server" />
                            <ext:Hidden ID="HiddenState" runat="server" />
                            <ext:Hidden ID="HiddenPostalCode" runat="server" />
                            <ext:CompositeField ID="CompositeField1" DataIndex="" runat="server" AnchorHorizontal="60%">
                                <Items>
                                    <ext:TextArea ID="tareaAddresses" runat="server" Flex="1" ReadOnly="true" Enabled="false" FieldLabel="Business Address" AllowBlank="false">
                                    </ext:TextArea>
                                    <ext:Button ID="btnEditAddress" runat="server" Text="...">
                                        <DirectEvents>
                                            <Click OnEvent="btnEditAddress_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cmpPriTelNumber" DataIndex="" runat="server" FieldLabel="Primary Telephone Number" AnchorHorizontal="60%">
                                <Items>
                                    <ext:TextField ID="txtPrimTelCountryCode" runat="server" Width="50" ReadOnly="true"/>
                                    <ext:DisplayField runat="server" Text="-" />
                                    <ext:TextField ID="txtPrimTelAreaCode" runat="server" Width="50" MaxLength="4" MaskRe="[0-9]"/>
                                    <ext:DisplayField runat="server" Text="-" />
                                    <ext:TextField ID="txtPrimTelPhoneNumber" runat="server" Flex="1" MaxLength="8" MaskRe="[0-9\-]"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cmpSecTelNumber" DataIndex="" runat="server" FieldLabel="Secondary Telephone Number"
                                Flex="1" AnchorHorizontal="60%">
                                <Items>
                                    <ext:TextField ID="txtSecTelCountryCode" runat="server" Width="50" ReadOnly="true"/>
                                    <ext:DisplayField ID="DisplayField1" runat="server" Text="-" />
                                    <ext:TextField ID="txtSecTelAreaCode" runat="server" Width="50" MaxLength="4" MaskRe="[0-9]"/>
                                    <ext:DisplayField ID="DisplayField2" runat="server" Text="-" />
                                    <ext:TextField ID="txtSecTelPhoneNumber" runat="server" Flex="1" MaxLength="8" MaskRe="[0-9\-]"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cmpFaxTelNumber" runat="server" FieldLabel="Fax Number" AnchorHorizontal="60%">
                                <Items>
                                    <ext:TextField ID="txtFaxCountryCode" runat="server" Width="50" ReadOnly="true" />
                                    <ext:DisplayField ID="DisplayField3" runat="server" Text="-" />
                                    <ext:TextField ID="txtFaxAreaCode" runat="server" Width="50" MaxLength="4" MaskRe="[0-9]"/>
                                    <ext:DisplayField ID="DisplayField4" runat="server" Text="-" />
                                    <ext:TextField ID="txtFaxPhoneNumber" runat="server" Flex="1" MaxLength="8" MaskRe="[0-9\-]"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField ID="txtEmailAddress" DataIndex="" runat="server" FieldLabel="Email Address"
                                AnchorHorizontal="60%" Vtype="email"/>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
        </Listeners>
    </ext:FormPanel>
    </Items></ext:Viewport>


    <ext:Window ID="wndAddressDetail" runat="server" Collapsible="false" Height="300"
        Icon="Application" Title="Address" Width="450" Hidden="true" Resizable="false"
        Modal="true" Draggable="false" Closable="false">
        <Items>
            <ext:FormPanel ID="frmPanelAddress" runat="server" Layout="FormLayout" Padding="10" MonitorValid="true">
                <Items>
                    <ext:TextField ID="txtStreetAddress" runat="server" FieldLabel="Street Address" 
                    AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:TextField ID="txtBarangay" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                    <ext:FieldSet Title="City or Municipality" runat="server" Layout="ColumnLayout" Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rgCityOrMunicipality" ColumnWidth=".6">
                                <Items>
                                    <ext:Radio ID="radioCity" runat="server" BoxLabel="City" />
                                    <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality" />
                                </Items>
                                <Listeners>
                                    <Change Handler="checkCityorMunicipality();" />
                                </Listeners>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipality" runat="server" ColumnWidth=".4" Height="30" AllowBlank="false">
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    
                    <ext:TextField ID="txtProvince" runat="server" FieldLabel="Province" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                    <ext:TextField ID="txtState" runat="server" FieldLabel="State" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:ComboBox ID="cmbCountry" runat="server" FieldLabel="Country" TypeAhead="true" AllowBlank="false"
                        Editable="false" ForceSelection="true" AnchorHorizontal="100%" ValueField="Id" DisplayField="Name">
                        <Store>
                        <ext:Store ID="strCountry" runat="server">
                            <Reader>
                                <ext:JsonReader>
                                    <Fields>
                                        <ext:RecordField Name="Id" />
                                        <ext:RecordField Name="Name" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:TextField ID="txtPostalCode" runat="server" FieldLabel="Postal Code" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar runat="server" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDone}.enable();}  else{#{btnDone}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnDone" Text="Done" runat="server" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="wndAddressDetail_btnAdd_Click" Before="return #{frmPanelAddress}.getForm().isValid();"/>
                </DirectEvents>
            </ext:Button>
            <ext:Button Text="Cancel" runat="server" Icon="Cancel">
                <Listeners>
                    <Click Handler="wndAddressDetail_btnCancel_Click();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
