<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCustomer.aspx.cs" Inherits="LendingApplication.Applications.CustomerUseCases.ManageCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Customer</title>
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
            window.proxy.init(['onpickcustomer', 'onpickemployer', 'addcustomerclassification']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var addSourceOfIncome = function () {
            hiddenRandomKey.setValue('');
            manageSourcesOfIncome.show();
            //txtAmount.setValue('0');
        };

        var saveSoiSuccessful = function () {
            manageSourcesOfIncome.hide();
            txtAmount.setValue('0');
        };

        var municipalityChange = function () {
            frmAddressDetail1.validate();
        }

        var editSourceOfIncome = function () {
            if (GridPanel1.hasSelection() == true) {
                var data = PageGridPanelSelectionModel.getSelected().json;
                hiddenRandomKey.setValue(data.RandomKey);
                txtAmount.setValue(data.Amount);
                cmbSourceOfIncome.setValue(data.SourceOfIncomeId);
                manageSourcesOfIncome.show();
            }
        };

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickemployer') {
                hiddenNewEmployerID.setValue(msg.data.EmployerID);
                txtEmployerName.setValue(msg.data.Names);
                txtEmploymentAddress.setValue(msg.data.Addresses);

                var loadMask = new Ext.LoadMask(RootPanel.body, { msg: 'Retrieving Employment Details...' });
                loadMask.show();
                X.FillEmploymentDetails(hiddenEmployerID.getValue(), msg.data.EmployerID, {
                    success: function () {
                        loadMask.hide();
                    }
                });
                txtEmploymentStatus.allowBlank = false;
                txtEmpPosition.allowBlank = false;
                txtSalary.allowBlank = false;
                markIfRequired(txtEmploymentStatus);
                markIfRequired(txtEmpPosition);
                markIfRequired(txtSalary);
            } else if (msg.tag == 'onpickcustomer') {
                txtPersonName.setValue(msg.data.Name);
                txtPrimaryHomeAddress.setValue(msg.data.Address);
                CustomerID.setValue(msg.data.PartyID);
                var loadMasks = new Ext.LoadMask(RootPanel.body, { msg: 'Retrieving Customer Details...' });
                loadMasks.show();
                X.FillCustomerDetails(msg.data.PartyRoleID, {
                    success: function () {
                        loadMasks.hide();
                    }
                });
            } else if (msg.tag == 'addcustomerclassification') {
                X.RefreshClassification();
            }
        };

        var onFormAddressValidated = function (valid) {
            if (valid) {
                StatusBar4.setStatus({ text: 'Form is valid. ' });
                btnDoneAddressDetail1.enable();
            }else if (valid && (txtMunicipalityD1.getValue() != '' || txtCityD1.getValue() != '')) {
                StatusBar4.setStatus({ text: 'Form is valid. ' });
            } else if (valid && (txtMunicipalityD1.getValue() != '' && txtCityD1.getValue() == '')) {
                StatusBar4.setStatus({ text: 'Form is valid. ' });
            } else if (valid && (txtMunicipalityD1.getValue() == '' && txtCityD1.getValue() != '')) {
                StatusBar4.setStatus({ text: 'Form is valid. ' });
            } else if ((txtMunicipalityD1.getValue() == '' || txtCityD1.getValue() == '')) {
                StatusBar4.setStatus({ text: 'Please fill out the form.' });
            } else {
                StatusBar4.setStatus({ text: 'Please fill out the form.' });
                btnDoneAddressDetail1.disable();
            }
        }

        var saveSuccessful = function () {
            var res = false;
            showAlert('Status', 'Customer record was successfully created.', function (btn) {
                    window.proxy.sendToAll('addcustomer', 'addcustomer');
                    window.proxy.requestClose();
            });
        };

        var openEmployersList = function () {
            var param = '/Applications/CustomerUseCases/EmployersPickList.aspx';
            window.proxy.requestNewTab('ListEmployers', param, 'Employers Pick List');
        };

        var openAllowedCustomersList = function () {
            var param = '/Applications/CustomerUseCases/AllowedCustomersPickList.aspx';
            window.proxy.requestNewTab('ListAllowedCustomers', param, 'Allowed Customers Pick List');
        };

        var checkSuccessful = function () {
            showConfirm('Confirm', 'A customer record with the same name already exists. Do you want to create another customer record with the same name?',
                function (response) {
                    if (response == 'yes') {

                    } else {
                        window.proxy.requestClose();
                    }
                }
             );
        };

        var checkFailure = function () {
            showConfirm('Confirm', 'A record with the same name already exists in the pick list. Do you want to create another record with the same name?',
                function (response) {
                    if (response == 'yes') {

                    } else {
                        window.proxy.requestClose();
                    }
                }
             );
        };

        var onRowSelected = function () {
            btnDelete.enable();
            btnOpenIncome.enable();
        };

        var onRowDeselected = function () {
            if (GridPanel1.hasSelection() == false) {
                btnDelete.disable();
                btnOpenIncome.disable();
            }
        };

        var onRowSelected2 = function () {
            btnSelectDistrict.enable();
        };

        var onRowDeselected2 = function () {
            if (grdDistrictPanel.hasSelection() == false) {
                btnSelectDistrict.disable();
            }
        };

        var checkSelected = function () {
            var selectedRow = grdDistrictPanelModel.getSelected();
            showAlert('Status', selectedRow.id);
            txtDistrict.setValue(selectedRow.value);
            if (selectedRow) {
                Customer.btnSelectDistricts(selectedRow.id);
            }
        };

        var provinceCheckerA1 = function () {
            if (radioCityA1.checked == true) {
                txtProvinceA1.allowBlank = true;
            }
            else if (radioMunicipalityA1.checked == true) {
                txtProvinceA1.allowBlank = false;
            }
            markIfRequired(txtProvinceA1);
        };

        var provinceCheckerA2 = function () {
            if (radioCityA2.checked == true) {
                txtProvinceA2.allowBlank = true;
            }
            else if (radioMunicipalityA2.checked == true) {
                txtProvinceA2.allowBlank = false;
            }
            markIfRequired(txtProvinceA2);
        };

        var provinceCheckerB1 = function () {
            if (radioCityB1.checked == true) {
                txtProvinceB1.allowBlank = true;
            }
            else if (radioMunicipalityB1.checked == true) {
                txtProvinceB1.allowBlank = false;
            }
            markIfRequired(txtProvinceB1);
        };

        var onId1Change = function () {
            X.onIdChange1();
        };

        var onId2Change = function () {
            X.onIdChange2();
        };

        var checkCustomerName = function () {
            X.btnSavePersonNameDetails();
            X.checkCustomerName(hiddenName.getValue(), {
                success: function (result) {
                    if (result == 1) {
                        showConfirm('Message', 'A customer record with the same name already exists. Do you want to create another customer record with the same name?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {
                                CustomerID.setValue('-1');
                            }
                            else {
                                var data = {};
                                data.id = hiddenID.getValue();
                                window.proxy.sendToParent(data, 'changeurltoedit');
                            }
                        });
                    } else if (result == 2) {
                        showConfirm('Message', 'A record with the same name already exists in the pick list. Do you want to create another record with the same name?', function (btn) {
                            if (btn.toLocaleLowerCase() == 'yes') {

                            }
                            else {
                                X.FillCustomerDetails(hiddenID.getValue());
                                hiddenUsesExistRecord.setValue('yes');
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
        
        .dot-label
        {
            font-weight: bold;
            font-size: 20px;
            vertical-align: text-top;
        }
       
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenID" runat="server" />
    <ext:Hidden ID="hiddenName" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="txtpartyId" runat="server" />
    <ext:Hidden ID="txtStreetAdd1" runat="server" />
    <ext:Hidden ID="txtBarangay1" runat="server" />
    <ext:Hidden ID="txtCity1" runat="server" />
    <ext:Hidden ID="txtState1" runat="server" />
    <ext:Hidden ID="txtPostal1" runat="server" />
    <ext:Hidden ID="txtCountry1" runat="server" />
    <ext:Hidden ID="txtMunicipality1" runat="server" />
    <ext:Hidden ID="txtProvince1" runat="server" />
    <ext:Hidden ID="txtStreetAdd2" runat="server" />
    <ext:Hidden ID="txtBarangay2" runat="server" />
    <ext:Hidden ID="txtCity2" runat="server" />
    <ext:Hidden ID="txtState2" runat="server" />
    <ext:Hidden ID="txtPostal2" runat="server" />
    <ext:Hidden ID="txtCountry2" runat="server" />
    <ext:Hidden ID="txtMunicipality2" runat="server" />
    <ext:Hidden ID="txtProvince2" runat="server" />
    <ext:Hidden ID="txtStreetAdd3" runat="server" />
    <ext:Hidden ID="txtBarangay3" runat="server" />
    <ext:Hidden ID="txtCity3" runat="server" />
    <ext:Hidden ID="txtState3" runat="server" />
    <ext:Hidden ID="txtPostal3" runat="server" />
    <ext:Hidden ID="txtCountry3" runat="server" />
    <ext:Hidden ID="txtMunicipality3" runat="server" />
    <ext:Hidden ID="txtProvince3" runat="server" />
    <ext:Hidden ID="txtPersonTitle" runat="server" />
    <ext:Hidden ID="txtPersonFirstName" runat="server" />
    <ext:Hidden ID="txtPersonLastName" runat="server" />
    <ext:Hidden ID="txtPersonMiddleName" runat="server" />
    <ext:Hidden ID="txtPersonNickName" runat="server" />
    <ext:Hidden ID="txtPersonNameSuffix" runat="server" />
    <ext:Hidden ID="txtPersonMothersMaidenName" runat="server" />
    <ext:Hidden ID="txtSpouseTitle" runat="server" />
    <ext:Hidden ID="txtSpouseFirstName" runat="server" />
    <ext:Hidden ID="txtSpouseLastName" runat="server" />
    <ext:Hidden ID="txtSpouseMiddleName" runat="server" />
    <ext:Hidden ID="txtSpouseNickName" runat="server" />
    <ext:Hidden ID="txtSpouseNameSuffix" runat="server" />
    <ext:Hidden ID="txtSpouseMothersMaidenName" runat="server" />
    <ext:Hidden ID="hiddenUsesExistRecord" runat="server" />
    <ext:Hidden ID="hiddenPostalAddressId" runat="server" />
    <ext:Hidden ID="hiddenSecondPostalAddress" runat="server" />
    <ext:Hidden ID="CustomerID" runat="server" />
    <ext:Hidden ID="hiddenClassificationTypeID" runat="server" />
    <ext:Hidden ID="hiddenEmployerID" runat="server" />
    <ext:Hidden ID="hiddenNewEmployerID" runat="server" />
    <ext:Hidden ID="hiddenImageUrl" runat="server" />
    <ext:Hidden ID="SpouseID" runat="server" />
    <%--Countries Store--%>
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
    <%--Customer Type Store--%>
    <ext:Store runat="server" ID="CustomerTypeStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Customer Classification Store--%>
    <ext:Store runat="server" ID="CustomerClassStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="StationNumber" />
                    <ext:RecordField Name="District" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Sources Of Income Store--%>
    <ext:Store runat="server" ID="SourceOfIncomeStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Marital Status Store--%>
    <ext:Store runat="server" ID="MaritalStatusStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Educational Attainment Store--%>
    <ext:Store runat="server" ID="EducAttainmentStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Home Ownership Store--%>
    <ext:Store runat="server" ID="HomeOwnershipStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Nationality Store--%>
    <ext:Store runat="server" ID="NationalityStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Identification Type Store--%>
    <ext:Store runat="server" ID="IDTypeStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--Identification Type2 Store--%>
    <ext:Store runat="server" ID="IDTypeStore2" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit" AutoHeight="true" >
        <Items>
            <%--AutoHeight="true" --%>
            <ext:FormPanel ID="RootPanel" runat="server" Layout="FitLayout"
                Border="false" MonitorValid="true" AutoScroll="true" MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click"  Success="saveSuccessful();" >
                                        <EventMask Msg="Saving.." ShowMask="true"/>    
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
                    <%--AutoHeight="true"--%>
                    <ext:TabPanel ID="PageTabPanel"  runat="server" Padding="0" HideBorders="true"
                        EnableTabScroll="true" DeferredRender="false">
                        <Items>
                            <%-------------------Basic Information Panel-----------------------%>
                            <%--Height="500"--%>
                            <ext:Panel ID="BasicInformationPanel1" runat="server" Title="Basic Information" Layout="FitLayout" >
                            <Items>
                            <%--Height="540"--%>
                            <ext:Panel ID="BasicInformationPanel" LabelWidth="200" runat="server"
                                Padding="5"  Layout="Column" RowHeight=".5" AutoScroll="true">
                                <Items>
                                    <%--Height="525"--%>
                                    <ext:Panel runat="server" Border="false" Header="false"
                                                ColumnWidth=".25" AutoScroll="true" >
                                        <Items>
                                            <ext:Image ID="PersonImageFile" Hidden="false" Width="230" Height="200" AnchorHorizontal="100%" runat="server" 
                                                 />
                                            <ext:FileUploadField ID="flupCustomerImage" Width="230" Height="35" Icon="Attach" runat="server">
                                            </ext:FileUploadField>
                                            <ext:Button ID="btnUpload" Text="Upload" runat="server" Icon="ImageAdd" Width="60">
                                                <DirectEvents>
                                                    <Click OnEvent="onUpload_Click">
                                                        <EventMask Msg="Uploading..." ShowMask="true" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel runat="server" LabelWidth="100" Layout="FormLayout" Border="false" ColumnWidth=".6" AutoScroll="true">
                                    <Items>
                                    <ext:CompositeField ID="cmfPersonName" runat="server" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtPersonName" MsgTarget="Side" FieldLabel="Name" ReadOnly="true" AllowBlank="false"
                                                runat="server" Flex="1" ValidateDelay="100">
                                                <Listeners>
                                                    <Focus Handler="#{wndPersonNameDetail}.show();" />
                                                    <Change Delay="100" Handler="checkCustomerName();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Button ID="btnNameDetail" Hidden="true" Text="..." runat="server" Width="20">
                                                <Listeners>
                                                    <Click Handler="#{wndPersonNameDetail}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button ID="btnBrowseCustomer" Text="Browse.." runat="server" Width="60">
                                                <Listeners>
                                                    <Click Handler="openAllowedCustomersList();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField ID="cmfDistrict" runat="server" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtDistrict" AllowBlank="false" FieldLabel="District" ReadOnly="true" runat="server" Flex="1">
                                                <Listeners>
                                                    <Focus Handler="#{wndDistrictList}.show();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Button ID="btnBrowseDistrict" Text="Browse.." runat="server" Width="60">
                                                <Listeners>
                                                    <Click Handler="#{wndDistrictList}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtStationNumber" FieldLabel="Station Number" ReadOnly="true"
                                        runat="server" AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtCustomerStatus" FieldLabel="Customer Status" ReadOnly="true"
                                        runat="server" Text="New" AnchorHorizontal="60%" />
                                    <ext:ComboBox ID="cmbCustomerType" EmptyText="" FieldLabel="Customer Type" runat="server"
                                        AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="CustomerTypeStore"
                                        ForceSelection="false" Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All"
                                        SelectOnFocus="true" AllowBlank="false" />
                                    <ext:RadioGroup ID="rdGender" runat="server" Width="300" FieldLabel="Gender" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:Radio ID="rdFemale" runat="server" Width="20" BoxLabel="Female" Checked="true" />
                                            <ext:Radio ID="rdMale" runat="server" Width="20" BoxLabel="Male" />
                                        </Items>
                                    </ext:RadioGroup>
                                    <ext:DateField ID="datBirthdate" AllowBlank="false" runat="server"
                                        FieldLabel="Birthdate" AnchorHorizontal="60%" Editable="false" >
                                            <DirectEvents>
                                                <Change OnEvent="checkAge" />
                                            </DirectEvents>
                                    </ext:DateField>
                                    <ext:CompositeField ID="cmfBirthplace" runat="server" FieldLabel="BirthPlace" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtBirthPlace" AllowBlank="true" FieldLabel="Birthplace" runat="server"
                                                Flex="1" ReadOnly="true">
                                                <Listeners>
                                                    <Focus Handler="#{winAddressDetailB1}.show();" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Button ID="Button2" Text="..." runat="server" Width="30">
                                                <Listeners>
                                                    <Click Handler="#{winAddressDetailB1}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtMothersMaidenName" Text="" AllowBlank="true" FieldLabel="Mother's Maiden Name"
                                        runat="server" AnchorHorizontal="60%" />
                                    <ext:ComboBox ID="cmbMaritalStatus" AllowBlank="false" FieldLabel="Marital Status"
                                        runat="server" AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="MaritalStatusStore"
                                        ForceSelection="true" Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All"
                                        SelectOnFocus="true">
                                        <DirectEvents>
                                            <Select OnEvent="onMaritalStatusChange" />
                                        </DirectEvents>    
                                    </ext:ComboBox>
                                    <ext:NumberField ID="nfNumberOfDependents" AllowBlank="false" FieldLabel="Number Of Dependents"
                                        runat="server" AnchorHorizontal="60%" MinValue="0" AllowDecimals="false" />
                                    <ext:ComboBox ID="cmbEducationalAttainment" EmptyText="" AllowBlank="false" FieldLabel="Educational Attainment"
                                        runat="server" AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="EducAttainmentStore"
                                        Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All" />
                                    <ext:ComboBox ID="cmbHomeOwnership" FieldLabel="Home Ownership" runat="server" AnchorHorizontal="60%"
                                        DisplayField="Name" AllowBlank="false" ValueField="Id" StoreID="HomeOwnershipStore" ForceSelection="true"
                                        Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All" SelectOnFocus="true" />
                                    <ext:DateField ID="datResidentSince" AllowBlank="false" FieldLabel="Resident Since"
                                        runat="server" AnchorHorizontal="60%" Editable="false" />
                                    <ext:ComboBox ID="cmbNationality" EmptyText="" AllowBlank="true" FieldLabel="Nationality"
                                        runat="server" AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="NationalityStore"
                                        ForceSelection="false" Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All"
                                        SelectOnFocus="true" />
                                    <ext:CompositeField ID="cmfTin" runat="server" FieldLabel="TIN" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:NumberField ID="txtTin1" MaxLength="3" TabIndex="1" Text="" runat="server" Flex="1" />
                                            <ext:DisplayField ID="DisplayField1" runat="server" Text="-" Cls="dot-label" />
                                            <ext:NumberField ID="txtTin2" MaxLength="3" TabIndex="2" Text="" runat="server" Flex="1" />
                                            <ext:DisplayField ID="DisplayField2" runat="server" Text="-" Cls="dot-label" />
                                            <ext:NumberField ID="txtTin3" MaxLength="3" TabIndex="3" Text="" runat="server" Flex="1" />
                                            <ext:DisplayField ID="DisplayField3" runat="server" Text="-" Cls="dot-label" />
                                            <ext:NumberField ID="txtTin4" MaxLength="4"  TabIndex="4" Text="" runat="server" Flex="1" />
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtCtc" AllowBlank="true" FieldLabel="CTC Number" runat="server"
                                        AnchorHorizontal="60%" Text="" />
                                    <ext:DateField ID="datDateIssued" Text="" AllowBlank="true" FieldLabel="Date Issued"
                                        runat="server" AnchorHorizontal="60%" Editable="false" />
                                    <ext:TextField ID="txtPlaceIssued" AllowBlank="true" Text="" FieldLabel="Place Issued"
                                        MaxLengthText="256" runat="server" AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtCreditLimit" Text="" AllowBlank="true" FieldLabel="Credit Limit"
                                        runat="server" AnchorHorizontal="60%" MaskRe="[0-9\.\,]">
                                        <Listeners>
                                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                            <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                                        </Listeners>
                                    </ext:TextField>
                                    </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            </Items>
                            </ext:Panel>
                            <%---------------------------IDs Panel-----------------------------%>
                            <%--AutoHeight="true"--%>
                            <ext:Panel ID="IDsPanel"  LabelWidth="200" runat="server"
                                Title="ID's" Layout="FormLayout" Padding="5">
                                <Items>
                                    <ext:ComboBox ID="cmbIdType1" EmptyText="" FieldLabel="ID Type 1" runat="server"
                                        AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="IdTypeStore"
                                        ForceSelection="false" Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All"
                                        SelectOnFocus="true" >
                                        <Listeners>
                                            <Change Handler="onId1Change();" />
                                        </Listeners>    
                                    </ext:ComboBox>
                                    <ext:TextField ID="txtIDNumber1" Text="" AllowBlank="true" FieldLabel="ID Number 1"
                                        runat="server" AnchorHorizontal="60%" />
                                    <ext:ComboBox ID="cmbIdType2" EmptyText="" FieldLabel="ID Type 2" runat="server"
                                        AnchorHorizontal="60%" DisplayField="Name" ValueField="Id" StoreID="IdTypeStore2"
                                        ForceSelection="false" Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All"
                                        SelectOnFocus="true" >
                                        <Listeners>
                                            <Change Handler="onId2Change();" />
                                        </Listeners>  
                                    </ext:ComboBox>
                                    <ext:TextField ID="txtIDNumber2" Text="" AllowBlank="true" FieldLabel="ID Number 2"
                                        runat="server" AnchorHorizontal="60%" />
                                </Items>
                            </ext:Panel>
                            <%--------------------Contact Information Panel--------------------%>
                            <ext:Panel ID="ContactInformationPanel" LabelWidth="200" runat="server"
                                Title="Contact Information" Layout="FormLayout" Padding="5">
                                <Items>
                                    <ext:CompositeField ID="cmfPrimaryHomeAddress" runat="server" 
                                        LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtPrimaryHomeAddress" FieldLabel="Primary Home Address" AllowBlank="false" DataIndex="PrimaryHomeAddress"
                                                runat="server" Flex="1" ReadOnly="true">
                                                <Listeners>
                                                    <Focus Handler="#{winAddressDetailA1}.show();" />
                                                </Listeners>    
                                            </ext:TextField>
                                            <ext:Button ID="btnBrowsePrimAddress" runat="server" Width="70" Text="...">
                                                <Listeners>
                                                    <Click Handler="#{winAddressDetailA1}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField ID="cmfSecondaryHomeAddress" runat="server"
                                        LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtSecondaryHomeAddress" DataIndex="SecondaryHomeAddress" runat="server"
                                                FieldLabel="Secondary Home Address" AllowBlank="true" Flex="1" ReadOnly="true">
                                                <Listeners>
                                                    <Focus Handler="#{winAddressDetailA2}.show();" />
                                                </Listeners>    
                                            </ext:TextField>
                                            <ext:Button ID="btnBrowseSecAddress" runat="server" Width="70" Text="...">
                                                <Listeners>
                                                    <Click Handler="#{winAddressDetailA2}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField LabelPad="10" ID="CellphoneNumber" runat="server" FieldLabel="Cellphone Number"
                                        LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtCellTelCodes" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField4" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtCellAreaCode" MaxLength="5" MinLength="1" MaskRe="/\d/" AllowBlank="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField5" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtCellPhoneNumber" AllowBlank="true" MinLength="5" MaxLength="7" MaskRe="/\d/"  runat="server" Flex="1" />
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField LabelPad="10" ID="TelephoneNumber" runat="server" FieldLabel="Telephone Number"
                                        LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtTelCodes" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField6" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtTelAreaCode" MaxLength="4" MinLength="1" MaskRe="/\d/" AllowBlank="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField7" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtTelPhoneNumber" AllowBlank="true" MinLength="5" MaxLength="7" MaskRe="/\d/"  runat="server" Flex="1" />
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField LabelPad="10" ID="txtPrimaryEmailAddress" AllowBlank="true" DataIndex="PrimaryEmailAddress"
                                        runat="server" LabelWidth="500" FieldLabel="Primary Email Address" AnchorHorizontal="60%"
                                        Vtype="email" />
                                    <ext:TextField LabelPad="10" ID="txtSecondaryEmailAddress" AllowBlank="true" DataIndex="SecondaryEmailAddress"
                                        runat="server" LabelWidth="500" FieldLabel="Secondary Email Address" AnchorHorizontal="60%"
                                        Vtype="email" />
                                </Items>
                            </ext:Panel>
                            <%----------------Employment Information Panel---------------------%>
                            <ext:Panel LabelWidth="200" ID="EmploymentInformationPanel"
                                runat="server" Title="Employment Information" Layout="FormLayout" Padding="5">
                                <Items>
                                    <ext:CompositeField ID="cmfEmployer" runat="server" FieldLabel="Employer" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtEmployerName" ReadOnly="true" runat="server" Flex="1" />
                                            <ext:Button ID="Button15" Text="Browse.." runat="server" Width="60">
                                                <Listeners>
                                                    <Click Handler="openEmployersList();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField ID="txtEmploymentAddress" ReadOnly="true" FieldLabel="Employment Address"
                                        runat="server" AnchorHorizontal="60%" />
                                    <ext:CompositeField LabelPad="10" ID="EmpTelephoneNumber" ReadOnly="true" runat="server"
                                        FieldLabel="Telephone Number" LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtEmpTelCode" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField8" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtEmpAreaCode" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField9" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtEmpPhoneNumber" ReadOnly="true" runat="server" Flex="1" />
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:CompositeField LabelPad="10" ID="EmpFaxNumber" ReadOnly="true" runat="server"
                                        FieldLabel="Fax Number" LabelWidth="500" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtEmpFaxTelCode" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField10" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtEmpFaxAreaCode" ReadOnly="true" runat="server" Width="75" />
                                            <ext:DisplayField ID="DisplayField11" runat="server" Text="-" Cls="dot-label" />
                                            <ext:TextField ID="txtEmpFaxNumber" ReadOnly="true" runat="server" Flex="1" />
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:TextField LabelPad="10" ID="txtEmpEmailAddress" ReadOnly="true" runat="server"
                                        LabelWidth="500" FieldLabel="Email Address" AnchorHorizontal="60%" Vtype="email" />
                                    <ext:TextField LabelPad="10" ID="txtEmpIDNumber" AllowBlank="true" runat="server" LabelWidth="500"
                                        FieldLabel="Employee ID Number" AnchorHorizontal="60%" />
                                    <ext:TextField LabelPad="10" ID="txtEmpPosition" AllowBlank="true" runat="server" LabelWidth="500"
                                        FieldLabel="Position" AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtEmploymentStatus" AllowBlank="true" FieldLabel="Employment Status"
                                        runat="server" AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtSalary" AllowBlank="true" FieldLabel="Salary" runat="server"
                                        AnchorHorizontal="60%" MaskRe="[0-9\.\,]" >
                                        <Listeners>
                                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                            <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                                        </Listeners>    
                                    </ext:TextField>
                                    <ext:TextField ID="txtSssNumber" AllowBlank="true" runat="server" FieldLabel="SSS Number"
                                        AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtGsisNumber" AllowBlank="true" runat="server" FieldLabel="GSIS Number"
                                        AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtOWANumber" AllowBlank="true" runat="server" FieldLabel="OWA Number"
                                        AnchorHorizontal="60%" />
                                    <ext:TextField ID="txtPhicNumber" AllowBlank="true" runat="server" FieldLabel="PHIC Number"
                                        AnchorHorizontal="60%" />
                                </Items>
                            </ext:Panel>
                            <%----------------Other Source of Income Panel---------------------%>
                            <ext:Panel ID="OtherSourceOfIncomePanel" AutoHeight="true" runat="server" Title="Other Sources of Income"
                                Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GridPanel1" runat="server" Height="550">
                                        <TopBar>
                                            <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteSourceIncome_Click">
                                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected sources of income?" />
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnOpenIncome" runat="server" Text="Edit" Icon="NoteEdit" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="editSourceOfIncome();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="addSourceOfIncome();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="CustomerSourceOfIncomeId" />
                                                            <ext:RecordField Name="SourceOfIncomeId" />
                                                            <ext:RecordField Name="Amount" />
                                                            <ext:RecordField Name="CustomerSourceOfIncome" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false">
                                                <Listeners>
                                                    <RowSelect Fn="onRowSelected" />
                                                    <RowDeselect Fn="onRowDeselected" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                                            <Columns>
                                                <ext:Column Header="Source Of Income" DataIndex="CustomerSourceOfIncome" Wrap="true" Locked="true"
                                                    Width="140px">
                                                </ext:Column>
                                                <ext:NumberColumn Header="Amount" DataIndex="Amount" Locked="true" Wrap="true" Width="140px"
                                                    Format=",000.00">
                                                </ext:NumberColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <LoadMask ShowMask="true"/>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <%------------------Spouse Information Panel-----------------------%>
                            <ext:Panel ID="SpouseInformationPanel" LabelWidth="200" runat="server"
                                Title="Spouse Information" Layout="FormLayout" Padding="5" Disabled="true">
                                <Items>
                                    <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="60%">
                                        <Items>
                                            <ext:TextField ID="txtSpouseName" AllowBlank="true" FieldLabel="Name" runat="server"
                                                Flex="1" ReadOnly="true" >
                                                <Listeners>
                                                    <Focus Handler="#{wndSpouseNameDetail}.show();" />
                                                </Listeners>    
                                            </ext:TextField>
                                            <ext:Button ID="Button1" Text="..." runat="server" Width="60">
                                                <Listeners>
                                                    <Click Handler="#{wndSpouseNameDetail}.show();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:DateField ID="datSpouseBirthdate" FieldLabel="Birthdate" runat="server"
                                        AllowBlank="true" AnchorHorizontal="60%" Editable="false" >
                                        <DirectEvents>
                                                <Change OnEvent="checkAge2" />
                                        </DirectEvents>    
                                    </ext:DateField>
                                </Items>
                            </ext:Panel>
                            <%------------------------Remarks Panel----------------------------%>
                            <ext:Panel ID="RemarksPanel" LabelWidth="200" runat="server"
                                Title="Remarks" Layout="FormLayout" Padding="5">
                                <Items>
                                    <ext:TextArea ID="txtRemarks" runat="server" FieldLabel="Remarks" AnchorHorizontal="60%"
                                        AnchorVertical="-250" BoxMinHeight="200" AllowBlank="true" />
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
                    <ext:Hidden ID="RecordID" DataIndex="ID" runat="server" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    <%------------------------------Customer Sources of Income-------------------------------------%>
    <ext:Window ID="manageSourcesOfIncome" Hidden="true" runat="server" Width="450" Icon="Application"
        Title="Other Sources of Income" Modal="true" AutoHeight="true" PageY="300">
        <Items>
            <ext:FormPanel runat="server" LabelWidth="200" MonitorPoll="500" MonitorValid="true">
                <Items>
                    <ext:ComboBox ID="cmbSourceOfIncome" runat="server" FieldLabel="Source of Income" AnchorHorizontal="95%"
                        DisplayField="Name" ValueField="Id" StoreID="SourceOfIncomeStore" ForceSelection="true" Editable="false" AllowBlank="false"/>
                    <ext:TextField ID="txtAmount" MaskRe="/[0-9\-\,\.]/" DataIndex="Amount" runat="server" FieldLabel="Amount"
                        AnchorHorizontal="95%" AllowBlank="false">
                        <Listeners>
                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                            <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                        </Listeners>
                    </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="sourceOfIncomeStatusBar" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveSourceIncome" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSaveSourceIncome_Click" Success="saveSoiSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="#{manageSourcesOfIncome}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="#{sourceOfIncomeStatusBar}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveSourceIncome}.enable();}  else{#{btnSaveSourceIncome}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="wndAddressDetail1" Modal="true" runat="server" Collapsible="true" Height="310"
        Icon="Application" Title="Primary Address" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmAddressDetail1" LabelWidth="120"
                MonitorValid="true">
                <Items>
                    <ext:TextField runat="server" ID="txtStreetNumberD1" FieldLabel="Street Address"
                        AnchorHorizontal="90%" AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden7" />
                    <ext:TextField runat="server" ID="txtBarangayD1" FieldLabel="Barangay" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden8" />
                    <ext:TextField runat="server" ID="txtMunicipalityD1" FieldLabel="Municipality" AnchorHorizontal="90%"
                        AllowBlank="true">
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange" />
                        </DirectEvents>    
                    </ext:TextField>
                    <ext:TextField runat="server" ID="txtCityD1" FieldLabel="City" AnchorHorizontal="90%"
                        AllowBlank="true" >
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange"  />
                        </DirectEvents>      
                    </ext:TextField>
                    <ext:Hidden runat="server" ID="hidden9" />
                    <ext:TextField runat="server" ID="txtProvinceD1" FieldLabel="Province" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden10" />
                    <ext:ComboBox runat="server" ID="cmbCountryD1" FieldLabel="Country" AnchorHorizontal="90%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:Hidden runat="server" ID="hidden11" />
                    <ext:TextField runat="server" ID="txtPostalCodeD1" FieldLabel="Postal Code" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden12" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar4" Text="" runat="server" Height="25" />
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
        <DirectEvents>
            <Show OnEvent="onMunicipalityCityChange" />
        </DirectEvents>
    </ext:Window>
    <ext:Window ID="wndAddressDetail2" Modal="true" runat="server" Collapsible="true" Height="310"
        Icon="Application" Title="Secondary Address" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmAddressDetail2" LabelWidth="120"
                MonitorValid="true">
                <Items>
                    <ext:TextField runat="server" ID="txtStreetNumberD2" FieldLabel="Street Address"
                        AnchorHorizontal="90%" AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden1" />
                    <ext:TextField runat="server" ID="txtBarangayD2" FieldLabel="Barangay" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden2" />
                    <ext:TextField runat="server" ID="txtMunicipalityD2" FieldLabel="Municipality" AnchorHorizontal="90%"
                        AllowBlank="true">
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange2" />
                        </DirectEvents>     
                    </ext:TextField>
                    <ext:TextField runat="server" ID="txtCityD2" FieldLabel="City" AnchorHorizontal="90%"
                        AllowBlank="true">
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChange2" />
                        </DirectEvents> 
                    </ext:TextField>
                    <ext:Hidden runat="server" ID="hidden3" />
                    <ext:TextField runat="server" ID="txtProvinceD2" FieldLabel="Province" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden4" />
                    <ext:ComboBox runat="server" ID="cmbCountryD2" FieldLabel="Country" AnchorHorizontal="90%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:Hidden runat="server" ID="hidden5" />
                    <ext:TextField runat="server" ID="txtPostalCodeD2" FieldLabel="Postal Code" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden6" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar3" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Buttons>
                    <ext:Button runat="server" ID="btnDoneAddressDetail2" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnSaveAddress2_Click" Before="return #{frmAddressDetail2}.getForm().isValid();" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancelAddressDetail" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndAddressDetail2}.hide()" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar3}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && (#{txtMunicipalityD2}.getValue() != '' || #{txtCityD2}.getValue() != '')){#{btnDoneAddressDetail2}.enable();}  else{#{btnDoneAddressDetail2}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
        <DirectEvents>
            <Show OnEvent="onMunicipalityCityChange2" />
        </DirectEvents>
    </ext:Window>
    <ext:Window ID="wndBirthPlaceAddDetail" Modal="true" runat="server" Collapsible="true" Height="310"
        Icon="Application" Title="Birthplace Address" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmBirthPlaceAddDetail" LabelWidth="120"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtStreetNumberB" FieldLabel="Street Address" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenStreetName" />
                    <ext:TextField runat="server" ID="txtBarangayB" FieldLabel="Barangay" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hiddenBarangay" />
                    <ext:TextField runat="server" ID="txtMunicipalityB" FieldLabel="Municipality" AnchorHorizontal="90%"
                        AllowBlank="true">
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChangeB" />
                        </DirectEvents>     
                    </ext:TextField>
                    <ext:TextField runat="server" ID="txtCityB" FieldLabel="City" AnchorHorizontal="90%"
                        AllowBlank="true">
                        <DirectEvents>
                            <Change OnEvent="onMunicipalityCityChangeB" />
                        </DirectEvents>     
                    </ext:TextField>
                    <ext:Hidden runat="server" ID="hiddenMunicipality" />
                    <ext:TextField runat="server" ID="txtProvinceB" FieldLabel="Province" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddenProvince" />
                    <ext:ComboBox runat="server" ID="cmbCountry" FieldLabel="Country" AnchorHorizontal="90%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:Hidden runat="server" ID="hiddenCountry" />
                    <ext:TextField runat="server" ID="txtPostalCodeB" FieldLabel="Postal Code" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hiddenPostalCode" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar1" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Buttons>
                    <ext:Button runat="server" ID="btnDoneBirthPlaceAddDetail" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnSaveBirthAdd_Click" Before="return #{frmBirthPlaceAddDetail}.getForm().isValid();" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnCancelBrithAdd" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndBirthPlaceAddDetail}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar1}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && (#{txtMunicipalityB}.getValue() != '' || #{txtCityB}.getValue() != '')){#{btnDoneBirthPlaceAddDetail}.enable();}  else{#{btnDoneBirthPlaceAddDetail}.disable();}" />
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
                    <ext:TextField ID="txtPostalCodeA1" runat="server" FieldLabel="Postal Code" MaskRe="/\d/" MinLength="4" MaxLength="4" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                </Items>
                <BottomBar>
                <ext:StatusBar runat="server" ID="statusWinAddressDetailA1">
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
    <ext:Window ID="winAddressDetailA2" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="310" Hidden="true"
        Title="Address Details" Width="520">
        <Items>
             <ext:FormPanel Padding="10" runat="server" ID="frmAddressA2" LabelWidth="120" MonitorValid="true">
                <Items>
                    <ext:TextField ID="txtStreetAddressA2" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:TextField ID="txtBarangayA2" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%"  AllowBlank="false">
                    </ext:TextField>
                    <ext:FieldSet ID="FieldSet2" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                        Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rdgCityMunicipality" ColumnWidth=".5">
                                <Listeners>
                                    <Change Handler ="provinceCheckerA2();" />
                                </Listeners>
                                <Items>
                                    <ext:Radio ID="radioMunicipalityA2" runat="server" BoxLabel="Municipality">
                                    </ext:Radio>
                                    <ext:Radio ID="radioCityA2" Checked="true" runat="server" BoxLabel="City">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipalityA2" runat="server" ColumnWidth=".5" Height="30" AllowBlank = "false">
                                <Listeners>
                                    <Focus Handler ="provinceCheckerA2();" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvinceA2" runat="server" LabelSeparator=":" FieldLabel="Province" AnchorHorizontal="100%" AllowBlank="true">
                    </ext:TextField>
                    <ext:ComboBox runat="server" ID="cmbCountryA2" FieldLabel="Country" AnchorHorizontal="100%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:TextField ID="txtPostalCodeA2" runat="server" FieldLabel="Postal Code" MaskRe="/\d/" MinLength="4" MaxLength="4" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                </Items>
                <BottomBar>
                <ext:StatusBar runat="server" ID = "statusWinAddressDetailA2">
                </ext:StatusBar>
                </BottomBar>
                <Buttons>
               <ext:Button runat="server" ID="btnDoneAddressDetailA2" Text="Done" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="btnDoneAddressDetailA2_DirectClick" Before="return #{frmAddressA2}.getForm().isValid();"/>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="btnCancelA2" Text="Cancel" Icon="Cancel">
                    <Listeners>
                            <Click Handler="#{winAddressDetailA2}.hide();" />
                    </Listeners>
                </ext:Button>
               </Buttons>
               <Listeners>
                    <ClientValidation Handler="#{statusWinAddressDetailA2}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetailA2}.enable();}  else{#{btnDoneAddressDetailA2}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="winAddressDetailB1" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="310" Hidden="true"
        Title="Address Details" Width="520">
        <Items>
             <ext:FormPanel Padding="10" runat="server" ID="frmAddressB1" LabelWidth="120" MonitorValid="true">
                <Items>
                    <ext:TextField ID="txtStreetAddressB1" runat="server" FieldLabel="Street Address" AnchorHorizontal="100%">
                    </ext:TextField>
                    <ext:TextField ID="txtBarangayB1" runat="server" FieldLabel="Barangay" AnchorHorizontal="100%"  AllowBlank="false">
                    </ext:TextField>
                    <ext:FieldSet ID="FieldSet3" Title="City or Municipality" runat="server" Layout="ColumnLayout"
                        Height="50" Padding="1">
                        <Items>
                            <ext:RadioGroup runat="server" ID="rdgCityMunicipalityB1" ColumnWidth=".5">
                                <Listeners>
                                    <Change Handler ="provinceCheckerB1();" />
                                </Listeners>
                                <Items>
                                    <ext:Radio ID="radioMunicipalityB1" runat="server" BoxLabel="Municipality">
                                    </ext:Radio>
                                    <ext:Radio ID="radioCityB1" Checked="true" runat="server" BoxLabel="City">
                                    </ext:Radio>
                                </Items>
                            </ext:RadioGroup>
                            <ext:TextField ID="txtCityOrMunicipalityB1" runat="server" ColumnWidth=".5" Height="30" AllowBlank = "false">
                                <Listeners>
                                    <Focus Handler ="provinceCheckerB1();" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                    </ext:FieldSet>
                    <ext:TextField ID="txtProvinceB1" runat="server" LabelSeparator=":" FieldLabel="Province" AnchorHorizontal="100%" AllowBlank="true">
                    </ext:TextField>
                    <ext:ComboBox runat="server" ID="cmbCountryB1" FieldLabel="Country" AnchorHorizontal="100%"
                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                        StoreID="CountryStore" />
                    <ext:TextField ID="txtPostalCodeB1" runat="server" FieldLabel="Postal Code" MaskRe="/\d/" MaxLength="4" MinLength="4" AnchorHorizontal="100%" AllowBlank="false">
                    </ext:TextField>
                </Items>
                <BottomBar>
                <ext:StatusBar runat="server" ID="statusWinAddressDetailB1">
                </ext:StatusBar>
                </BottomBar>
                <Buttons>
               <ext:Button runat="server" ID="btnDoneAddressDetailB1" Text="Done" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="btnDoneAddressDetailB1_DirectClick" Before="return #{frmAddressB1}.getForm().isValid();"/>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" ID="btnCancelB1" Text="Cancel" Icon="Cancel">
                    <Listeners>
                            <Click Handler="#{winAddressDetailB1}.hide();" />
                    </Listeners>
                </ext:Button>
               </Buttons>
               <Listeners>
                    <ClientValidation Handler="#{statusWinAddressDetailB1}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnDoneAddressDetailB1}.enable();}  else{#{btnDoneAddressDetailB1}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>

    <ext:Window ID="wndPersonNameDetail" Modal="true" runat="server" Collapsible="true" Height="280"
        Icon="Application" Title="Customer Name" Width="350" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmPersonNameDetail" LabelWidth="120"
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
                            <Click Delay="100" Handler="checkCustomerName();" />
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
    <ext:Window ID="wndSpouseNameDetail" Modal="true" runat="server" Collapsible="true" Height="300"
        Icon="Application" Title="Spouse Name" Width="380" Hidden="true">
        <Items>
            <ext:FormPanel Padding="10" runat="server" ID="frmSpouseNameDetail" LabelWidth="150"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtTitleS" FieldLabel="Title" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hiddentxtTitleP" />
                    <ext:TextField runat="server" ID="txtFirstNameS" FieldLabel="First Name" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:Hidden runat="server" ID="hidden14" />
                    <ext:TextField runat="server" ID="txtMiddleNameS" FieldLabel="Middle Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden15" />
                    <ext:TextField runat="server" ID="txtLastNameS" FieldLabel="Last Name" AnchorHorizontal="90%"
                        AllowBlank="false" />
                    <ext:TextField runat="server" ID="txtMothersMaidenNameS" FieldLabel="Mother's Maiden Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden16" />
                    <ext:TextField runat="server" ID="txtNickNameS" FieldLabel="Nick Name" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden17" />
                    <ext:TextField runat="server" ID="txtNameSuffixS" FieldLabel="Name Suffix" AnchorHorizontal="90%"
                        AllowBlank="true" />
                    <ext:Hidden runat="server" ID="hidden18" />
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar5" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Buttons>
                    <ext:Button runat="server" ID="btnSaveSpouseNameDetail" Text="Done" Icon="Accept">
                        <DirectEvents>
                            <Click OnEvent="btnSaveSpouseNameDetail_Click" Before="return #{frmPersonNameDetail}.getForm().isValid();" />
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button runat="server" ID="Button3" Text="Cancel" Icon="Cancel">
                        <Listeners>
                            <Click Handler="#{wndSpouseNameDetail}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar5}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveSpouseNameDetail}.enable();}  else{#{btnSaveSpouseNameDetail}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <ext:Window ID="wndDistrictList" runat="server" Modal="true" Collapsible="true" AutoHeight="true"
        Icon="Application" Title="District List" Width="600" Hidden="true">
        <Items>
            <ext:GridPanel ID="grdDistrictPanel" StoreID="CustomerClassStore" runat="server"
                AutoHeight="true">
                <TopBar>
                    <ext:Toolbar ID="Toolbar2" runat="server">
                        <Items>
                            <ext:Button ID="btnSelectDistrict" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSelectDistrict_DirectClick" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="Button6" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="#{wndDistrictList}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <SelectionModel>
                    <ext:RowSelectionModel ID="grdDistrictPanelModel" runat="server" SingleSelect="true">
                        <Listeners>
                            <RowSelect Fn="onRowSelected2" />
                            <RowDeselect Fn="onRowDeselected2" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="ColumnModel1" Width="100%">
                    <Columns>
                        <ext:Column Header="ID" DataIndex="Id" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Station Number" DataIndex="StationNumber" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                        <ext:Column Header="District" DataIndex="District" Locked="true" Wrap="true" Width="140px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" DisplayInfo="true"
                        DisplayMsg="Displaying sources of income {0} - {1} of {2}" EmptyMsg="No sources of income to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
