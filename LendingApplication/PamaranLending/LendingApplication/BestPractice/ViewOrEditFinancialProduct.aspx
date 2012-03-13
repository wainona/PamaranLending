<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOrEditFinancialProduct.aspx.cs"
    Inherits="LendingApplication.BestPractice.ViewOrEditFinancialProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../Resources/css/main.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript">

        Ext.onReady(function () {
            window.first = true;
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickrequireddocumenttype', 'onpickrequireddocumenttypes']);
            window.proxy.on('messagereceived', onMessageReceived);

            maskUnmask(BasicProductInformationPanel, false);
            maskUnmask(PanelInterestRate, false);
            //maskUnmask(PastDueInterestRate, false);
            maskUnmask(Fee, false);
            maskUnmask(RequiredDocumentTypeView, false);
        });

        var onMessageReceived = function (msg) {

            if (msg.tag == 'onpickrequireddocumenttype') {
                X.AddRequiredDocument(msg.data.Id, {
                    success: function (result) {
                        if (result == false)
                            showAlert('Information', 'Financial product already contains the selected product required document.');
                    }
                });
            }
            else if (msg.tag == 'onpickrequireddocumenttypes') {
                var requiredDocumentIds = [];
                for (var i = 0; i < msg.data.length; i++) {
                    requiredDocumentIds.push(msg.data[i].Id);
                }

                X.AddRequiredDocuments(requiredDocumentIds);
            }
        };

        var onBtnPickRequiredDocType = function () {
            var url = '/BestPractice/PickListRequiredDocumentType.aspx';
            var param = url + "?mode=" + 'multiple';
            window.proxy.requestNewTab('PickRequriedDocType', param, 'Select Required Document Type');
        }

        var OnItemSelect = function () {
            var selected = cmbChargeableItems.getText();
            if (selected != null && selected == 'Documentary Stamp Tax')
                nfRate.disable();
            else
                nfRate.enable();
        }

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully updated the financial product record.', function () {
                window.proxy.sendToAll('updatefinancialproduct', 'updatefinancialproduct');
                //window.proxy.requestClose();
                openOrEdit();
            });
        }

        var addNewInterestRate = function () {
            cmbNewInterestRate.clearValue();
            newInterestRate.hide();
            nfNewInterestRate.setValue('0');
        }

        var addPastDueInterestRate = function () {
            cmbNewPastDueRate.clearValue();
            newPastDueInterestRate.hide();
            nfNewPastDueRate.setValue('0');
        }

        var resetElementOfFee = function () {
            newFee.hide();
            cmbChargeableItems.clearValue();
            nfChargeAmount.setValue('');
            nfBaseAmount.setValue('');
            nfRate.setValue('');

            nfChargeAmount.enable();
            nfBaseAmount.enable();
            nfRate.enable();
        }

        var updateAllowedFeeElement = function () {
            if (nfChargeAmount.getValue() > 0) {
                nfBaseAmount.allowBlank = true;
                nfRate.disable();
            }
            else if (nfBaseAmount.getValue() > 0) {
                nfChargeAmount.allowBlank = false;
                nfChargeAmount.validate();
                nfRate.disable();
            }
            else if (nfRate.getValue() > 0) {
                nfChargeAmount.disable();
                nfBaseAmount.disable();
            }
            else {
                nfChargeAmount.allowBlank = true;
                nfBaseAmount.allowBlank = true;
                nfChargeAmount.enable();
                nfBaseAmount.enable();
                nfRate.enable();
            }
            markIfRequired(nfBaseAmount);
            markIfRequired(nfChargeAmount);
            markIfRequired(nfRate);
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
        };

        var openOrEdit = function () {
            var enable = btnOpen.getText() == 'Edit';
            maskUnmask(BasicProductInformationPanel, enable);
            maskUnmask(PanelInterestRate, enable);
            //maskUnmask(PastDueInterestRate, enable);
            maskUnmask(Fee, enable);
            maskUnmask(RequiredDocumentTypeView, enable);
            if (enable) {
                btnOpen.setText('Open');
            }
            else {
                btnOpen.setText('Edit');
            }

            RootPanel.validate();
        };

        var onRowSelected = function (btn) {
            var enable = btnOpen.getText() == 'Open';
            if (enable)
                btn.enable();
        };
        var onRowDeselected = function (grid, btn) {
            if (grid.hasSelection() == false) {
                btn.disable();
            }
        };

        var changeStatusSuccessful = function () {
            window.proxy.sendToAll('updatefinancialproduct', 'updatefinancialproduct');
            showAlert('Status', 'Successfully updated the status of the financial product record.');
        }

        var checkInterestRate = function () {
            var records = new Array();
            var test = GridPanelInterestRate.store.each(function (rec) {
                if (rec.json.Description == cmbNewInterestRate.getText() &&
                rec.json.InterestRate == nfNewInterestRate.getValue()) {
                    records.push(rec);
                }
            })

            //            if (records.length > 0)
            //                showAlert('Information', 'Interest rate combination already exist. Please enter another value.');

            return records.length > 0;
        }

        var maskUnmask = function (panel, enable) {
            if (enable == true) {
                panel.getEl().unmask();
                panel.removeClass('ext-hide-mask');
            } else if (enable == false) {
                panel.getEl().mask();
                panel.addClass('ext-hide-mask');

                if (window.first) {
                    for (count = 0; count < 7; count++) {
                        PageTabPanel.setActiveTab(count);

                        if (count == 6) {
                            PageTabPanel.setActiveTab(0);
                        }
                    }

                    window.first = false;
                }
            }

        };

        var checkPastDueRate = function () {
            var records = new Array();
            var test = gridPanelPastDueInterestRate.store.each(function (rec) {
                if (rec.json.Description == cmbNewPastDueRate.getText() &&
                rec.json.InterestRate == nfNewPastDueRate.getValue()) {
                    records.push(rec);
                }
            })

            return records.length > 0;
        }

        var onFormValidated = function (valid) {
            btnSave.disable();
            var enable = btnOpen.getText() == 'Open';
            if (valid && GridPanelInterestRate.store.getCount() > 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                if (enable)
                    btnSave.enable();
            }
            else if (GridPanelInterestRate.store.getCount() == 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Please select an interest rate.', iconCls: 'icon-exclamation' });
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
            }
        }

        var onInterestFormValidated = function (valid) {
            btnSaveNewInterestRate.disable();
            if (checkInterestRate()) {
                StatusInterestRate.setStatus({ text: 'Interest rate already exist.', iconCls: 'icon-exclamation' });
            }
            else if (valid) {
                StatusInterestRate.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSaveNewInterestRate.enable();
            }
            else {
                StatusInterestRate.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
            }
        }

        var onPastDueFormValidated = function (valid) {
            btnSaveNewPastDue.disable();
            if (checkPastDueRate()) {
                StatusPastDue.setStatus({ text: 'Past due rate already exist.', iconCls: 'icon-exclamation' });
            }
            else if (valid) {
                StatusPastDue.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSaveNewPastDue.enable();
            }
            else {
                StatusPastDue.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
            }
        }

        var checkFee = function () {
            var records = new Array();
            var test = gridPanelFee.store.each(function (rec) {
                if (rec.json.Name == cmbChargeableItems.getText() &&
                rec.json.ChargeAmount == nfChargeAmount.getValue() &&
                rec.json.BaseAmount == nfBaseAmount.getValue() &&
                rec.json.Rate == nfRate.getValue()) {
                    records.push(rec);
                }
            });

            return records.length > 0;
        }

        var onFeeFormValidated = function (valid) {
            btnSaveNewFee.disable();
            if (checkFee()) {
                StatusBarFee.setStatus({ text: 'Fee combination already exist.', iconCls: 'icon-exclamation' });
            }
            else if (valid) {
                StatusBarFee.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSaveNewFee.enable();
            }
            else {
                StatusBarFee.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
            }
        }
        var onChangeTermOption = function () {
            var selected = cmbTermOption.getText();
            if (selected == 'No Term') {
                nfMinimumLoanTerm.minValue = 0;
                nfMaximumLoanTerm.minValue = 0;
                nfMinimumLoanTerm.setValue(0);
                nfMaximumLoanTerm.setValue(0);

                fsLoanTerm.disable();
            } else {
                nfMinimumLoanTerm.minValue = 1;
                nfMaximumLoanTerm.minValue = 1;
                nfMinimumLoanTerm.setValue(1);
                nfMaximumLoanTerm.setValue(1);

                fsLoanTerm.enable();
            }
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
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
    <ext:Store runat="server" ID="InterestRateStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="PastDueRateStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="newFeeStore" RemoteSort="false">
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
            <ext:FormPanel ID="RootPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true"
                MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnOpen" runat="server" Text="Edit" Icon="NoteEdit">
                                <Listeners>
                                    <Click Handler="openOrEdit();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnOpenSeparator" />
                            <ext:Button ID="btnActivate" runat="server" Text="Activate" Disabled="true" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnActivate_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to activate this product?" />
                                        <EventMask ShowMask="true" Msg="Updating product status..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnDeactivate" runat="server" Text="Deactivate" Disabled="true" Icon="Decline">
                                <DirectEvents>
                                    <Click OnEvent="btnDeactivate_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to deactivate this product?" />
                                        <EventMask ShowMask="true" Msg="Updating product status..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnRetire" runat="server" Text="Retire" Disabled="true" Icon="Lock">
                                <DirectEvents>
                                    <Click OnEvent="btnRetire_Click" Success="changeStatusSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to retire this product?" />
                                        <EventMask ShowMask="true" Msg="Updating product status..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="false">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{RootPanel}.getForm().isValid();"
                                        Success="saveSuccessful();">
                                        <EventMask ShowMask="true" Msg="Saving Financial Product..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnSaveSeparator" />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TabPanel ID="PageTabPanel" runat="server" EnableTabScroll="true" Padding="0"
                        HideBorders="true">
                        <Items>
                            <ext:Panel runat="server" ID="BasicProductInformationPanel" Title="Basic Product Information"
                                Layout="FormLayout" LabelWidth="200" Padding="5">
                                <Items>
                                    <ext:TextField ID="txtName" DataIndex="Name" runat="server" FieldLabel="Name" AnchorHorizontal="90%"
                                        AllowBlank="false" MsgTarget="Side" />
                                    <ext:DateField ID="dtIntroductionDate" DataIndex="IntroductionDate" runat="server"
                                        FieldLabel="Introduction Date" AnchorHorizontal="90%" AllowBlank="false" Editable="false"
                                        Vtype="daterange" EndDateField="dtSalesDiscontinuationDate">
                                        <Listeners>
                                            <Select Fn="onDateChanged" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:DateField ID="dtSalesDiscontinuationDate" DataIndex="SalesDiscontinuationDate"
                                        runat="server" FieldLabel="Sales Discontinuation Date" AnchorHorizontal="90%"
                                        AllowBlank="true" MsgTarget="Side" Editable="false" Vtype="daterange" StartDateField="dtIntroductionDate">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" HideTrigger="false" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Fn="onDateChanged" />
                                            <TriggerClick Handler="this.reset();" />
                                        </Listeners>
                                    </ext:DateField>
                                    <ext:TextField ID="txtComment" DataIndex="Comment" runat="server" FieldLabel="Comment"
                                        AnchorHorizontal="90%" />
                                    <ext:TextField ID="txtProductStatus" DataIndex="ProductStatus" runat="server" FieldLabel="Product Status"
                                        AnchorHorizontal="90%" ReadOnly="true" Text="Inactive" />
                                    <ext:ComboBox ID="cmbTermOption" runat="server" FieldLabel="Term Option" ValueField="Id"
                                        DisplayField="Name" AnchorHorizontal="90%" Editable="false" AllowBlank="false">
                                        <%--Term Option--%>
                                        <Store>
                                            <ext:Store ID="strTermOption" runat="server">
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
                                        <Listeners>
                                            <Select Handler="onChangeTermOption();"></Select>
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:Panel ID="Panel1" runat="server" Layout="Form" Border="false" Header="false"
                                        Height="80">
                                        <Items>
                                            <ext:CheckboxGroup ID="CheckboxGroup1" runat="server" AllowBlank="false" FieldLabel="Collateral Requirement"
                                                Width="600">
                                                <Items>
                                                    <ext:Checkbox runat="server" ID="chkBoxSecured" BoxLabel="Secured" Checked="true" />
                                                    <ext:Checkbox runat="server" ID="chkBoxUnsecured" BoxLabel="Unsecured" Checked="true" />
                                                </Items>
                                            </ext:CheckboxGroup>
                                            <ext:CheckboxGroup ID="CheckboxGroup2" runat="server" AllowBlank="false" FieldLabel="Interest Computation Mode"
                                                Width="600">
                                                <Items>
                                                    <ext:Checkbox runat="server" ID="chkBoxDiminish" BoxLabel="Diminishing Balance Method"
                                                        Checked="true" />
                                                    <ext:Checkbox runat="server" ID="chkBoxStraight" BoxLabel="Straight Line Method"
                                                        Checked="true" />
                                                </Items>
                                            </ext:CheckboxGroup>
                                            <ext:CheckboxGroup ID="CheckboxGroup3" runat="server" AllowBlank="false" FieldLabel="Method of Changing Interest"
                                                Width="600">
                                                <Items>
                                                    <ext:Checkbox runat="server" ID="chkBoxAddOn" BoxLabel="Add-On Interest" Checked="true" />
                                                    <ext:Checkbox runat="server" ID="chkBoxDiscounted" BoxLabel="Discounted Interest"
                                                        Checked="true" />
                                                </Items>
                                            </ext:CheckboxGroup>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="Panel2" runat="server" Layout="ColumnLayout" Border="false" Header="false"
                                        AnchorHorizontal="90%">
                                        <Items>
                                            <ext:Panel ID="fsLoanLimit" Title="Loan Limit" runat="server" Height="120" ColumnWidth=".5"
                                                Layout="Form" Padding="5">
                                                <Items>
                                                    <ext:NumberField ID="nfMinimumLoanableAmount" FieldLabel="Minimum Loanable Amount"
                                                        runat="server" AllowBlank="false" MsgTarget="Side" MinValue="0" DecimalPrecision="2"
                                                        AnchorHorizontal="95%" Vtype="numberrange" EndNumberField="#{nfMaximumLoanableAmount}"
                                                        Number="1" />
                                                    <ext:NumberField ID="nfMaximumLoanableAmount" FieldLabel="Maximum Loanable Amount"
                                                        runat="server" AllowBlank="false" MsgTarget="Side" MinValue="0" DecimalPrecision="2"
                                                        AnchorHorizontal="95%" Vtype="numberrange" StartNumberField="#{nfMinimumLoanableAmount}"
                                                        Number="1" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="fsLoanTerm" Title="Loan Term" runat="server" Height="120" ColumnWidth=".5"
                                                Layout="Form" Padding="5">
                                                <Items>
                                                    <ext:NumberField ID="nfMinimumLoanTerm" FieldLabel="Minimum Loan Term" runat="server"
                                                        AllowBlank="false" MsgTarget="Side" MinValue="1" DecimalPrecision="0" AnchorHorizontal="95%"
                                                        Vtype="numberrange" EndNumberField="#{nfMaximumLoanTerm}" Number="1" />
                                                    <ext:NumberField ID="nfMaximumLoanTerm" FieldLabel="Maximum Loan Term" runat="server"
                                                        AllowBlank="false" MsgTarget="Side" MinValue="1" DecimalPrecision="0" AnchorHorizontal="95%"
                                                        Vtype="numberrange" StartNumberField="#{nfMinimumLoanTerm}" Number="1" />
                                                    <ext:ComboBox runat="server" ID="cmbTimeUnit" FieldLabel="Time Unit" AnchorHorizontal="95%"
                                                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                                                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                                                        StoreID="TimeUnitStore" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelInterestRate" Title="Interest Rate" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GridPanelInterestRate" runat="server">
                                        <Items>
                                            <ext:Toolbar ID="tbInterestRate" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteInterestRate" runat="server" Text="Delete" Icon="Delete"
                                                        Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteInterestRate_Click" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnInterestRate" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="#{newInterestRate}.show();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store runat="server" ID="storeNewInterestRate" RemoteSort="false">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ProductFeatureApplicabilityId" />
                                                            <ext:RecordField Name="Description" />
                                                            <ext:RecordField Name="InterestRate" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelInterestRate" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteInterestRate);" />
                                                    <RowDeselect Handler="onRowDeselected(GridPanelInterestRate, btnDeleteInterestRate);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Interest Rate Description" Locked="true" DataIndex="Description"
                                                    Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Interest Rate" DataIndex="InterestRate" Locked="true"
                                                    Width="250">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <%--Past Due Interest Rate--%>
                            <ext:Panel runat="server" ID="PastDueInterestRate" Title="Past Due Interest Rate"
                                Layout="FitLayout" Hidden="true">
                                <Items>
                                    <ext:GridPanel ID="gridPanelPastDueInterestRate" runat="server">
                                        <Items>
                                            <ext:Toolbar ID="tbPastDueRate" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeletePastDue" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeletePastDue_Click" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddPastDue" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="#{newPastDueInterestRate}.show();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="storePastDueInterestRate" runat="server">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ProductFeatureApplicabilityId" />
                                                            <ext:RecordField Name="Description" />
                                                            <ext:RecordField Name="InterestRate" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelPastDueInterestRate" SingleSelect="true"
                                                runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeletePastDue);" />
                                                    <RowDeselect Handler="onRowDeselected(gridPanelPastDueInterestRate, btnDeletePastDue);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Description" DataIndex="Description" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Interest Rate" DataIndex="InterestRate" Width="250">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <%--Fee--%>
                            <ext:Panel runat="server" ID="Fee" Title="Fee" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="gridPanelFee" runat="server">
                                        <Items>
                                            <ext:Toolbar ID="tbFee" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteFee" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteFee_Click">
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddFee" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="#{newFee}.show();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="storeFee" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="Name" />
                                                            <ext:RecordField Name="ChargeAmount" />
                                                            <ext:RecordField Name="BaseAmount" />
                                                            <ext:RecordField Name="Rate" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Name" DataIndex="Name" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Charge Amount" DataIndex="ChargeAmount" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Base Amount" DataIndex="BaseAmount" Width="250">
                                                </ext:Column>
                                                <ext:Column runat="server" Header="Rate" DataIndex="Rate" Width="250">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelFee" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteFee);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelFee, btnDeleteFee);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <%--Required Document Type--%>
                            <ext:Panel runat="server" ID="RequiredDocumentTypeView" Title="Required Document Type View"
                                Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="gridRequiredDocumentType" runat="server">
                                        <Items>
                                            <ext:Toolbar ID="tbRequiredDocumentType" runat="server">
                                                <Items>
                                                    <ext:Button ID="btnDeleteDocument" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="btnDeleteDocument_Click">
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator>
                                                    </ext:ToolbarSeparator>
                                                    <ext:Button ID="btnAddDocument" runat="server" Text="Add" Icon="Add">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickRequiredDocType();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="storeRequiredDocumentType" runat="server">
                                                <Reader>
                                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="ProductRequriedDocumentId" />
                                                            <ext:RecordField Name="TypeId" />
                                                            <ext:RecordField Name="Name" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="SelectionModelRequiredDocument" SingleSelect="true" runat="server">
                                                <Listeners>
                                                    <RowSelect Handler="onRowSelected(btnDeleteDocument);" />
                                                    <RowDeselect Handler="onRowDeselected(SelectionModelRequiredDocument, btnDeleteDocument);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Header="Document Name" DataIndex="Name" Width="350">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:TabPanel>
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
    <%--New Interest Rate Window--%>
    <ext:Window ID="newInterestRate" runat="server" Collapsible="true" Height="120" Icon="Application"
        Title="New Interest Rate" Width="350" Hidden="true" Modal="true" Resizable="false">
        <Items>
            <ext:FormPanel ID="frmNewInterstRate" runat="server" Padding="5" LabelWidth="80"
                Layout="FormLayout" MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:ComboBox ID="cmbNewInterestRate" runat="server" FieldLabel="Description" AnchorHorizontal="100%"
                        ValueField="Id" DisplayField="Name" StoreID="InterestRateStore" Editable="false"
                        ForceSelection="true" AllowBlank="false">
                    </ext:ComboBox>
                    <ext:CompositeField runat="server" FieldLabel="Rate" ID="ctl9046">
                        <Items>
                            <ext:NumberField ID="nfNewInterestRate" runat="server" Flex="1" DecimalPrecision="2"
                                MaxValue="100" MinValue="0" Number="0" AllowBlank="false" />
                            <ext:Label runat="server" Text="%" ID="ctl9048">
                            </ext:Label>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <BottomBar>
                    <ext:StatusBar runat="server" ID="StatusInterestRate">
                        <Items>
                            <ext:Button ID="btnSaveNewInterestRate" runat="server" Text="Add" Icon="Add">
                                <DirectEvents>
                                    <Click OnEvent="btnSaveInterestRate_Click" Success="addNewInterestRate();">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelNewIntersteRate" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="newInterestRate.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onInterestFormValidated(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <%--New Past Due Interest Rate Window--%>
    <ext:Window ID="newPastDueInterestRate" runat="server" Collapsible="true" Height="120"
        Icon="Application" Title="Past Due Interest Rate" Width="350" Hidden="true" Modal="true"
        Resizable="false">
        <Items>
            <ext:FormPanel ID="frmNewPastDueInterestRate" runat="server" Padding="5" LabelWidth="80"
                Layout="FormLayout" MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:ComboBox ID="cmbNewPastDueRate" runat="server" FieldLabel="Description" AnchorHorizontal="100%"
                        ValueField="Id" DisplayField="Name" StoreID="PastDueRateStore" Editable="false"
                        ForceSelection="true" AllowBlank="false">
                    </ext:ComboBox>
                    <ext:CompositeField runat="server" FieldLabel="Rate" ID="cfPastDueRate">
                        <Items>
                            <ext:NumberField ID="nfNewPastDueRate" runat="server" Flex="1" DecimalPrecision="2"
                                MaxValue="100" MinValue="0" Number="0" AllowBlank="false" />
                            <ext:Label runat="server" Text="%" ID="Label1">
                            </ext:Label>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusPastDue" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveNewPastDue" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSavePastDueInterestRate_Click" Success="addPastDueInterestRate();">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelNewPastDue" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="newPastDueInterestRate.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onPastDueFormValidated(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    <%--New Fees Window--%>
    <ext:Window ID="newFee" runat="server" Collapsible="true" Height="175" Icon="Application"
        Title="New Fee" Width="370" Hidden="true" Modal="true" Resizable="false">
        <Items>
            <ext:FormPanel ID="fpNewFee" runat="server" Padding="5" LabelWidth="120" Layout="FitLayout"
                MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:ComboBox ID="cmbChargeableItems" runat="server" FieldLabel="Name" Flex="1" ValueField="Id"
                        DisplayField="Name" StoreID="newFeeStore" Editable="false" ForceSelection="true"
                        AllowBlank="false">
                        <Listeners>
                            <Select Fn="OnItemSelect" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:CompositeField ID="cfChargeAmount" runat="server" FieldLabel="Charge Amount">
                        <Items>
                            <ext:NumberField ID="nfChargeAmount" runat="server" Flex="1" DecimalPrecision="2"
                                MinValue="0" FieldLabel="Charge Amount">
                                <Listeners>
                                    <Change Handler="updateAllowedFeeElement();" />
                                </Listeners>
                            </ext:NumberField>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField2" runat="server" FieldLabel="Base Amount">
                        <Items>
                            <ext:NumberField ID="nfBaseAmount" runat="server" Flex="1" DecimalPrecision="2" MinValue="0"
                                FieldLabel="Base Amount">
                                <Listeners>
                                    <Change Handler="updateAllowedFeeElement();" />
                                </Listeners>
                            </ext:NumberField>
                        </Items>
                    </ext:CompositeField>
                    <ext:CompositeField ID="CompositeField3" runat="server" FieldLabel="Rate">
                        <Items>
                            <ext:NumberField ID="nfRate" runat="server" Flex="1" DecimalPrecision="2" MaxValue="100"
                                MinValue="0" Number="0" FieldLabel="Rate">
                                <Listeners>
                                    <Change Handler="updateAllowedFeeElement();" />
                                </Listeners>
                            </ext:NumberField>
                            <ext:Label ID="lblPercent" runat="server" Text="%" Width="20">
                            </ext:Label>
                        </Items>
                    </ext:CompositeField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBarFee" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveNewFee" runat="server" Text="Save" Icon="Disk" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSaveNewFee_Click" Success="resetElementOfFee();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelNewFee" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="resetElementOfFee();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onFeeFormValidated(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
