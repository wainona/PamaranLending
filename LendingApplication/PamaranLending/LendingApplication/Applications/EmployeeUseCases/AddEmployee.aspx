<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="LendingApplication.Applications.EmployeeUseCases.AddEmployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Employee</title>
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
            window.proxy.init(['onpickallowedemployee']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Employee record was successfully created.', function () {
                window.proxy.sendToAll('addemployee', 'addemployee');
                window.proxy.requestClose();
            });
        };

        var btnPickCustomer_Click = function () {
            window.proxy.requestNewTab('PickListAllowedEmployees', '/Applications/EmployeeUseCases/ListAllowedEmployees.aspx', 'Pick List Allowed Employees');
        };

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickallowedemployee') {
                // do your stuff here
                hdnPickedPartyId.setValue(msg.data.PartyId)
                X.FillPersonInformationFields();
            };
        }

        var checkEmployeeName = function () {
            X.CheckEmployeeName({
                success: function (result) {
                    if (result == 1) {
                        showConfirm('Message', 'Employee record with the same name already exists in the list. Do you want to create another employee record with the same name?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                hdnPickedPartyId.setValue('');
                            }
                            else {
                                var data = {};
                                data.id = hdnEmployeeID.getValue();
                                window.proxy.sendToParent(data, 'changeurltoedit');
                            }
                        });
                    }
                    else if (result == 2) {
                        showConfirm('Message', 'The name already exists in the pick list for allowed employees. Do you want to create a new party record for this employee?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                hdnPickedPartyId.setValue('');
                            }
                            else {
                                X.FillPersonInformationFields();
                            }
                        });
                    }
                }
            });
        }

        var checkCityorMunicipality = function () {
            if (radioCity.checked) {
                txtProvince.allowBlank = true;
            } else if (radioMunicipality.checked) {
                txtProvince.allowBlank = false;
            }
            markIfRequired(txtProvince);
        }

        var confirmCloseTab = function () {
            showConfirm("Message", "Are you sure you want to close the tab?", function (btn) {
                if (btn.toLocaleLowerCase() == 'yes') {
                    window.proxy.requestClose();
                }
                else {

                }
            });
        }
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <%--Employee Positon Store--%>
    <ext:Store runat="server" ID="EmployeePositionStore" RemoteSort="false">
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
            <ext:FormPanel ID="pnlEmployeeInformation" runat="server" Border="false" MonitorValid="true" MonitorPoll="500" Layout="RowLayout">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();" Before="return #{pnlEmployeeInformation}.getForm().isValid()">
                                        <EventMask Msg="Saving.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="confirmCloseTab();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="pnlEmployeeBasicInformation" Title="Basic Information"
                        Padding="5" Layout="Column" RowHeight=".6">
                        <Items>
                            <ext:Hidden ID="hdnPickedPartyId" runat="server"></ext:Hidden>
                            <ext:Hidden ID="hdnEmployee" runat="server"></ext:Hidden>
                            <ext:Panel ID="Panel2" runat="server" ColumnWidth=".2" Height="260" Border="false" Header="false">
                                <Items>
                                    <ext:Hidden ID="hdnPersonPicture" runat="server" />
                                    <ext:Image ID="imgPersonPicture" runat="server" Width="200" ImageUrl="../../Resources/images/noimage.jpg" Height="200"/>
                                    <ext:FileUploadField ID="flupCustomerImage" Width="200" Icon="Attach" runat="server" />
                                    <ext:Button ID="btnUpload" Text="Upload" runat="server" Width="60">
                                       <DirectEvents>
                                            <Click OnEvent="onUpload_Click">
                                                <EventMask Msg="Uploading.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="pnlInformation" runat="server" LabelWidth="200" Layout="FormLayout" ColumnWidth=".8" AutoHeight="true" Border="false">
                                <Items>
                                    <ext:Hidden ID="hdnEmployeeID" runat="server" />
                                    <ext:CompositeField ID="cfEmployeeInformationPanel" runat="server" AnchorHorizontal="100%">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtEmployeeName" Width="341" FieldLabel="Name" ReadOnly="true"  AllowBlank="false">
                                                <Listeners>
                                                    <Focus Handler="#{wndNameDetailBox}.show();"></Focus>
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Button ID="btnPickCustomer" runat="server" Text="Browse">
                                                <Listeners>
                                                    <Click Handler="btnPickCustomer_Click();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:RadioGroup runat="server" FieldLabel="Gender" BoxMaxWidth="150" AllowBlank="false">
                                        <Items>
                                            <ext:Radio runat="server" ID="radioFemale" BoxLabel="Female" />
                                            <ext:Radio runat="server" ID="radioMale" BoxLabel="Male" />
                                        </Items>
                                    </ext:RadioGroup>
                                    <ext:DateField ID="dtBirthDate" runat="server" FieldLabel="Birth Date" Width="341" Editable="false" AllowBlank="false"/>
                                    <ext:TextField ID="txtEmployeeIdNumber" runat="server" MaxLengthText="30" MaskRe="[-\w]" FieldLabel="Employee Id Number" Width="341" />
                                    <ext:TextField ID="txtPosition" runat="server" Hidden="true" FieldLabel="Position" MaxLengthText="50" Width="341" AllowBlank="true"/>
                                    <ext:ComboBox ID="cmbPosition" runat="server" FieldLabel="Position" TypeAhead="true" StoreID="EmployeePositionStore"
                                        Editable="false" ForceSelection="true" Width="341" ValueField="Id" DisplayField="Name" AllowBlank="false" />
                                    <ext:ComboBox ID="cmbEmploymentStatus" runat="server" ForceSelection="true" FieldLabel="Employment Status" Editable="false" Width="341" AllowBlank="false">
                                        <Items>
                                            <ext:ListItem Text="Employed"/>
                                            <ext:ListItem Text="Retired" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:TextField ID="txtSalary" runat="server" FieldLabel="Salary" Width="341" AllowBlank="false" MaskRe="[0-9\.\,]">
                                        <Listeners>
                                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:CompositeField runat="server" FieldLabel="TIN">
                                        <Items>
                                            <ext:TextField ID="txtTIN" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label runat="server" Text=" - " />
                                            <ext:TextField ID="txtTIN1" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label runat="server" Text=" - " />
                                            <ext:TextField ID="txtTIN2" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label runat="server" Text=" - " />
                                            <ext:TextField ID="txtTIN3" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[-\w]" MaxLength="4"/>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtSSSNumber" runat="server" FieldLabel="SSS Number" MaskRe="[-\w]" Width="341" ></ext:TextField>
                                    <ext:TextField ID="txtPHICNumber" runat="server" FieldLabel="PHIC Number" MaskRe="[-\w]" Width="341" ></ext:TextField>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                    <ext:Panel runat="server" ID="pnlEmployeeContactInformation" Title="Contact Information"
                        Padding="5" Layout="Form" LabelWidth="200" RowHeight=".4" AnchorHorizontal="100%">
                        <Items>
                            <ext:CompositeField runat="server">
                                <Items>
                                    <ext:TextArea ID="txtPrimaryHomeAddress" runat="server" FieldLabel="Primary Home Address" AllowBlank="false" Width="400" ReadOnly="true">
                                        <DirectEvents>
                                            <Focus OnEvent="btnBrowseAddress_Click" />
                                        </DirectEvents>
                                    </ext:TextArea>
                                    <ext:Button ID="btnBrowseAddress" runat="server" Text="...">
                                        <DirectEvents>
                                            <Click OnEvent="btnBrowseAddress_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cfCellphoneNumber" runat="server" FieldLabel="Cellphone Number">
                                <Items>
                                    <ext:TextField ID="txtCellCountryCode" runat="server" Width="70" ReadOnly="true"/>
                                    <ext:TextField ID="txtCellAreaCode" runat="server" Width="70" MaskRe="[0-9]" MaxLength="4"/>
                                    <ext:TextField ID="txtCellPhoneNumber" runat="server" Width="250" MaskRe="[0-9]" MaxLength="8"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cfTelephoneNumber" runat="server" FieldLabel="Telephone Number">
                                <Items>
                                    <ext:TextField ID="txtTeleCountryCode" runat="server" Width="70" ReadOnly="true"/>
                                    <ext:TextField ID="txtTeleAreaCode" runat="server" Width="70" MaskRe="[0-9]" MaxLength="4"/>
                                    <ext:TextField ID="txtTelePhoneNumber" runat="server" Width="250" MaskRe="[0-9]" MaxLength="8"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField ID="txtPrimaryEmailAddress" runat="server" Vtype="email" FieldLabel="Email Address" Width="400" />
                        </Items>
                    </ext:Panel>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <Clientvalidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    <%--Name Detail Window--%>
    <ext:Window runat="server" ID="wndNameDetailBox" Width="350" Height="229" Hidden="true" Modal="true" 
        Draggable="false" Resizable="false" Layout="FormLayout" Title="Name Detail Box" Closable="false">
        <Items>
            <ext:Hidden ID="hdnTitleName" runat="server" />
            <ext:Hidden ID="hdnFirstName" runat="server" />
            <ext:Hidden ID="hdnMiddleName" runat="server" />
            <ext:Hidden ID="hdnLastName" runat="server" />
            <ext:Hidden ID="hdnSuffixName" runat="server" />
            <ext:Hidden ID="hdnNickName" runat="server" />
            <ext:FormPanel ID="nameFormPanel" runat="server" MonitorValid="true" Padding="5" Border="false">
                <Items>
                    <ext:TextField runat="server" ID="txtTitle" FieldLabel="Title" AnchorHorizontal="95%" />
                    <ext:TextField runat="server" ID="txtFirstName" FieldLabel="First Name" AllowBlank="false" AnchorHorizontal="95%" />
                    <ext:TextField runat="server" ID="txtMiddleName" FieldLabel="Middle Name" AnchorHorizontal="95%" />
                    <ext:TextField runat="server" ID="txtLastName" FieldLabel="Last Name" AllowBlank="false" AnchorHorizontal="95%" />
                    <ext:TextField runat="server" ID="txtSuffix" FieldLabel="Suffix" AnchorHorizontal="95%" />
                    <ext:TextField runat="server" ID="txtNickName" FieldLabel="Nickname" AnchorHorizontal="95%" />
                </Items>
                <Listeners>
                    <ClientValidation Handler="if (valid){#{btnDone}.enable();}  else{#{btnDone}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <BottomBar>
            <ext:Toolbar runat="server" ID="Toolbar1">
                <Items>
                <ext:ToolbarFill></ext:ToolbarFill>
                    <ext:Button runat="server" ID="btnDone" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnDone_Click" Success="checkEmployeeName();" Before="return #{nameFormPanel}.getForm().isValid();"/>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndNameDetailBox}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    <%--Address Detail Window--%>
    <ext:Window ID="wndAddressDetail" runat="server" Collapsible="false" Height="306"
        Icon="Application" Title="Address" Width="450" Hidden="true" Resizable="false"
        Modal="true" Draggable="false" Closable="false">
        <Items>
            <ext:FormPanel ID="addressFormPanel" runat="server" Layout="FormLayout" Padding="10" MonitorValid="true" Border="false">
                <Items>
                    <ext:TextField ID="txtStreetAddress" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%" />
                    <ext:TextField ID="txtBarangay" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%" AllowBlank="false"/>
                    <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout" Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rgMunicipalityOrCity" ColumnWidth=".6" BoxMaxWidth="180">
                                <Items>
                                    <ext:Radio ID="radioCity" runat="server" BoxLabel="City" />
                                    <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality" />
                                </Items>
                                <Listeners>
                                    <Change Handler="checkCityorMunicipality();" />
                                </Listeners>
                            </ext:RadioGroup>
                            <ext:Panel runat="server" Width="20" Border="false"></ext:Panel>
                            <ext:TextField ID="txtCityOrMunicipality" runat="server" Width="200" Height="30" AllowBlank="false"/>
                            <ext:Hidden ID="hdnCity" runat="server" />
                            <ext:Hidden ID="hdnMunicipality" runat="server" />
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvince" runat="server" FieldLabel="Province" AnchorHorizontal="100%" />
                    <ext:ComboBox ID="cmbCountry" runat="server" FieldLabel="Country" TypeAhead="true"
                        Editable="false" ForceSelection="true" AnchorHorizontal="100%" ValueField="Id" DisplayField="Name" AllowBlank="false">
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
                    <ext:TextField ID="txtPostalCode" runat="server" FieldLabel="Postal Code" AnchorHorizontal="100%" AllowBlank="false" MaxLength="4" MinLength="4" MaskRe="[0-9]"/>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="AddressDetailStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnOkAddressDetail}.enable();}  else{#{btnOkAddressDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnOkAddressDetail" Text="Ok" runat="server" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="wndAddressDetail_btnAdd_Click" Before="return #{addressFormPanel}.getForm().isValid();"/>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancelAddressDetail" Text="Cancel" runat="server" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{wndAddressDetail}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
