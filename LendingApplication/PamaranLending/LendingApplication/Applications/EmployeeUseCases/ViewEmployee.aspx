<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewEmployee.aspx.cs" Inherits="LendingApplication.Applications.EmployeeUseCases.ViewOrEditEmployee" %>

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
            showAlert('Status', 'Employee record was successfully updated.', function () {
                window.proxy.sendToAll('updateemployee', 'updateemployee');
                //window.proxy.requestClose();
                btnCancel_Click();
            });
        }

     

        var btnEdit_Click = function () {
            pnlEmployeeBasicInformation.enable();
            pnlEmployeeContactInformation.enable();
            btnBrowseAddress.enable();
            flupPersonImage.show();
            btnUpload.show();
            btnSave.show();
            btnSaveSeparator.show();
            btnCancel.show();
            btnEdit.hide();
        }

        var btnCancel_Click = function () {
            btnBrowseAddress.disable();
            pnlEmployeeBasicInformation.disable();
            pnlEmployeeContactInformation.disable();
            flupPersonImage.hide();
            btnUpload.hide();
            btnSave.hide();
            btnSaveSeparator.hide();
            btnCancel.hide();
            btnEdit.show();
        }

        var btnBrowseAddress_Click = function () {
            X.BrowseAddress_Click();
        };

        var checkCityorMunicipality = function () {
            if (radioCity.checked) {
                txtProvince.allowBlank = true;
            } else if (radioMunicipality.checked) {
                txtProvince.allowBlank = false;
            }
            markIfRequired(txtProvince);
        };
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
        
        .ext-hide-mask .ext-el-mask
        {
            opacity: 0.25;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
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
            <ext:FormPanel ID="pnlEmployeeInformation" runat="server" Border="false" MonitorValid="true" MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="DatabaseEdit">
                                <Listeners>
                                    <Click Handler="btnEdit_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();" Before="return #{pnlEmployeeInformation}.getForm().isValid();">
                                        <EventMask Msg="Saving.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnSaveSeparator" runat="server" Hidden="true"/>
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Hidden="true" Icon="Cross">
                                <Listeners>
                                    <Click Handler="btnCancel_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TabPanel runat="server" Border="false">
                        <Items>
                            <ext:FormPanel runat="server" ID="pnlBasicInformation" Title="Basic Information" Border="false" Layout="Form">
                                <Items>
                                    <ext:Panel runat="server" ID="pnlEmployeeBasicInformation" Title="Basic Information" Border="false"
                                        Padding="5" Layout="Column" Height="300" AnchorHorizontal="100%" Disabled="true" Cls="ext-hide-mask">
                                        <Items>
                                            <ext:Hidden ID="hdnPickedPartyId" runat="server"></ext:Hidden>
                                            <ext:Hidden ID="hdnPickedPartyRoleId" runat="server"></ext:Hidden>
                                            <ext:Hidden ID="hdnEmployee" runat="server"></ext:Hidden>
                                            <ext:Panel ID="Panel2" runat="server" Height="260" Border="false" Header="false"
                                                ColumnWidth=".2">
                                                <Items>
                                                    <ext:Hidden ID="hdnPersonPicture" runat="server" />
                                                    <ext:Image ID="imgPersonPicture" runat="server" Width="200" Height="200" ImageUrl="../../Resources/images/noimage.jpg" />
                                                    <ext:FileUploadField ID="flupPersonImage" Width="200" Icon="Attach" runat="server" Hidden="true"/>
                                                    <ext:Button ID="btnUpload" Text="Upload" runat="server" Width="60" Hidden="true">
                                                       <DirectEvents>
                                                            <Click OnEvent="onUpload_Click"/>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="pnlInformation" runat="server" LabelWidth="200" Layout="FormLayout" Border="false" ColumnWidth=".8" AutoHeight="true">
                                                <Items>
                                                    <ext:Hidden ID="hdnEmployeeID" runat="server" />
                                                    <ext:TextField runat="server" ID="txtEmployeeName" FieldLabel="Name" ReadOnly="true" Width="341" AllowBlank="false">
                                                        <DirectEvents>
                                                            <Focus OnEvent="txtEmployeeName_Focus"></Focus>
                                                        </DirectEvents>
                                                        <Listeners>
                                                            <Focus Handler="#{wndNameDetailBox}.show();"></Focus>
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:RadioGroup ID="RadioGroup1" runat="server" FieldLabel="Gender" BoxMaxWidth="150" AllowBlank="false">
                                                        <Items>
                                                            <ext:Radio runat="server" ID="radioMale" BoxLabel="Male" />
                                                            <ext:Radio runat="server" ID="radioFemale" BoxLabel="Female" />
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
                                                    <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel="TIN">
                                                        <Items>
                                                            <ext:TextField ID="txtTIN" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label ID="Label1" runat="server" Text=" - " />
                                                            <ext:TextField ID="txtTIN1" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label ID="Label2" runat="server" Text=" - " />
                                                            <ext:TextField ID="txtTIN2" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[0-9]" MaxLength="3"/><ext:Label ID="Label3" runat="server" Text=" - " />
                                                            <ext:TextField ID="txtTIN3" runat="server" Width="74"  FieldLabel="TIN" MaskRe="[-\w]" MaxLength="4"/>
                                                        </Items>
                                                    </ext:CompositeField>
                                                    <ext:TextField ID="txtSSSNumber" runat="server" FieldLabel="SSS Number" MaskRe="[-\w]" Width="341" ></ext:TextField>
                                                    <ext:TextField ID="txtPHICNumber" runat="server" FieldLabel="PHIC Number" MaskRe="[-\w]" Width="341" ></ext:TextField>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" ID="pnlEmployeeContactInformation" Title="Contact Information" Border="false"
                                        Padding="5" Layout="Form" LabelWidth="150" RowHeight=".3" AnchorHorizontal="100%" Disabled="true" Cls="ext-hide-mask">
                                        <Items>
                                            <ext:CompositeField ID="CompositeField2" runat="server">
                                                <Items>
                                                    <ext:TextArea ID="txtPrimaryHomeAddress" runat="server" FieldLabel="Primary Home Address" AllowBlank="false" Width="400" ReadOnly="true">
                                                        <Listeners>
                                                            <Focus Handler="btnBrowseAddress_Click();" />
                                                        </Listeners>
                                                    </ext:TextArea>
                                                    <ext:Button ID="btnBrowseAddress" runat="server" Text="..." Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="btnBrowseAddress_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="cfCellphoneNumber" runat="server" FieldLabel="Cellphone Number">
                                                <Items>
                                                    <ext:TextField ID="txtCellCountryCode" runat="server" Width="70" ReadOnly="true"/>
                                                    <ext:TextField ID="txtCellAreaCode" runat="server" Width="70" MaxLength="4"/>
                                                    <ext:TextField ID="txtCellPhoneNumber" runat="server" Width="250" MaxLength="8"/>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="cfTelephoneNumber" runat="server" FieldLabel="Telephone Number">
                                                <Items>
                                                    <ext:TextField ID="txtTeleCountryCode" runat="server" Width="70" ReadOnly="true"/>
                                                    <ext:TextField ID="txtTeleAreaCode" runat="server" Width="70" MaxLength="4"/>
                                                    <ext:TextField ID="txtTelePhoneNumber" runat="server" Width="250" MaxLength="8"/>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtPrimaryEmailAddress" runat="server" Vtype="email" FieldLabel="Email Address" Width="400" />
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:FormPanel>
                            
                            <ext:GridPanel ID="PageGridPanel" runat="server" AutoScroll="true" Border="false" Title="Transaction History" Height="500">
                                <View>
                                    <ext:GridView EmptyText="No transaction history to display">
                                    </ext:GridView>
                                </View>
                                <LoadMask ShowMask="true" />
                                <Store>
                                    <ext:Store ID="PageGridPanelStore" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="PaymentId">
                                                <Fields>
                                                     <ext:RecordField Name="PaymentId" />
                                                    <ext:RecordField Name="Particular" />
                                                    <ext:RecordField Name="Received" />
                                                    <ext:RecordField Name="Released"/>
                                                    <ext:RecordField Name="CurrencySymbol" />
                                                    <ext:RecordField Name="_Date" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                                    <Columns>
                                       <ext:Column Header="Date" DataIndex="_Date" Width="150px" Locked="true" ></ext:Column>
                                        <ext:Column Header="Particular" DataIndex="Particular" Locked="true" Wrap="true" Width="150px" Hidden="false" />
                                        <ext:NumberColumn Header="Received Amount" DataIndex="Received" Width="150px" Locked="true" Wrap="true" Format=",000.00" />
                                         <ext:NumberColumn Header="Released Amount" DataIndex="Released" Width="150px" Locked="true" Wrap="true" Format=",000.00" />
                                         <ext:Column Header="Currency" DataIndex="CurrencySymbol" Locked="true" Wrap="true" Width="150px" Hidden="false" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                                        DisplayMsg="Displaying transaction items {0} - {1} of {2}" EmptyMsg="No transaction history to display" />
                                </BottomBar>
                            </ext:GridPanel>
                                
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
    <%--Name Detail Window--%>
    <ext:Window runat="server" ID="wndNameDetailBox" Width="350" Height="229" Hidden="true" Modal="true" 
        Draggable="false" Resizable="false" Layout="FormLayout" Title="Name Detail Box" Closable="false">
        <Items>
            <ext:FormPanel ID="NameDetailFormPanel" runat="server" Padding="5" MonitorValid="true" Border="false">
                <Items>
                    <ext:Hidden ID="hdnTitle" runat="server" />
                    <ext:Hidden ID="hdnFirstName" runat="server" />
                    <ext:Hidden ID="hdnMiddleName" runat="server" />
                    <ext:Hidden ID="hdnLastName" runat="server" />
                    <ext:Hidden ID="hdnSuffix" runat="server" />
                    <ext:Hidden ID="hdnNickName" runat="server" />
                    <ext:TextField runat="server" ID="txtTitle" FieldLabel="Title" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField runat="server" ID="txtFirstName" FieldLabel="First Name" AllowBlank="false" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField runat="server" ID="txtMiddleName" FieldLabel="Middle Name" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField runat="server" ID="txtLastName" FieldLabel="Last Name" AllowBlank="false" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField runat="server" ID="txtSuffix" FieldLabel="Suffix" AnchorHorizontal="95%"></ext:TextField>
                    <ext:TextField runat="server" ID="txtNickName" FieldLabel="Nickname" AnchorHorizontal="95%"></ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar runat="server"></ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDone}.enable();}  else{#{btnDone}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <BottomBar>
            <ext:Toolbar runat="server" ID="Toolbar1">
                <Items>
                <ext:ToolbarFill></ext:ToolbarFill>
                    <ext:Button runat="server" ID="btnDone" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnDoneNameDetail_Click" Before="return #{NameDetailFormPanel}.getForm().isValid();"/>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancelNameDetail" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndNameDetailBox}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    <%--Address Detail Window--%>
    <ext:Window ID="wndAddressDetail" runat="server" Collapsible="false" Height="308"
        Icon="Application" Title="Address" Width="450" Hidden="true" Resizable="false"
        Modal="true" Draggable="false" Closable="false">
        <Items>
            <ext:FormPanel ID="Panel1" runat="server" Layout="FormLayout" Padding="10" MonitorValid="true" Border="false">
                <Items>
                    <ext:Hidden ID="hdnStreetAddress" runat="server" />
                    <ext:Hidden ID="hdnBarangay" runat="server" />
                    <ext:Hidden ID="hdnCity" runat="server" />
                    <ext:Hidden ID="hdnMunicipality" runat="server" />
                    <ext:Hidden ID="hdnProvince" runat="server" />
                    <ext:Hidden ID="hdnState" runat="server" />
                    <ext:Hidden ID="hdnPostalCode" runat="server" />
                    <ext:Hidden ID="hdnCountryId" runat="server" />
                    
                    <ext:TextField ID="txtStreetAddress" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%" />
                    <ext:TextField ID="txtBarangay" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%" AllowBlank="false"/>
                    <ext:FieldSet ID="FieldSet1" Title="City or Municipality" runat="server" Layout="ColumnLayout" Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rgMunicipalityOrCity" ColumnWidth=".6" BoxMaxWidth="180" AllowBlank="false">
                                <Items>
                                    <ext:Radio ID="radioCity" runat="server" BoxLabel="City" />
                                    <ext:Radio ID="radioMunicipality" runat="server" BoxLabel="Municipality" />
                                </Items>
                                <Listeners>
                                    <Change Handler="checkCityorMunicipality();" />
                                </Listeners>
                            </ext:RadioGroup>
                            <ext:Panel runat="server" Width="20" Border="false"></ext:Panel>
                            <ext:TextField ID="txtCityOrMunicipality" runat="server" Height="30" AllowBlank="false" Width="200"/>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvince" runat="server" FieldLabel="Province" AnchorHorizontal="100%" />
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
                    <ext:TextField ID="txtPostalCode" runat="server" FieldLabel="Postal Code" AnchorHorizontal="100%" AllowBlank="false" MaskRe="[0-9]" MinLength="4" MaxLength="4"/>
                </Items>
                <BottomBar>
                    <ext:StatusBar runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnOkAddressDetail}.enable();}  else{#{btnOkAddressDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnOkAddressDetail" Text="Ok" runat="server" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="wndAddressDetail_btnAdd_Click" Before="return #{Panel1}.getForm().isValid();"/>
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
