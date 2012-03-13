<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenEditTemplate.aspx.cs"
    Inherits="LendingApplication.BestPractice.OpenEditTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var actualState = new Array();

        Ext.onReady(function () {
            //searchPreviousState(RootPanel);
            //alert('RootPanel');
//            searchPreviousState(PanelInterestRate);
//            alert('PanelInterestRate');

            //enableDisablePanelElement(RootPanel, false);
            //enableDisablePanelElement(PanelInterestRate, false);
        });

        var enableDisablePanelElement = function (panel, enable) {
            if (enable == true) {
                panel.cascade(function (item) {
                    var index = actualState.indexOf(item);
                    if (index == -1) {
                        if (item.getXType() == 'button'
                        || item.getXType() == 'checkbox'
                        || item.getXType() == 'radiogroup'
                        || item.getXType() == 'radio'
                        || item.getXType() == "compositefield") {
                            item.enable();
                        }
                        else if (item.getXType() == 'datefield'
                        || item.getXType() == 'combo'
                        )
                            item.setReadOnly(false);
                        else if (item.isFormField && (typeof item.readOnly != 'undefined')) {
                            //item.enable();
                            //setReadOnly(item, false);
                            item.setReadOnly(false);
                        }
                    }
                });
            } else {
                panel.cascade(function (item) {
                    if (item.getXType() == 'button'
                    || item.getXType() == 'checkbox'
                    || item.getXType() == 'radiogroup'
                    || item.getXType() == 'radio'
                    || item.getXType() == "compositefield") {
                        item.disable();
                    }
                    else if (item.getXType() == 'datefield'
                    || item.getXType() == 'combo') {
                        item.setReadOnly(true);
                    }
                    else if (item.isFormField && (typeof item.readOnly != 'undefined')) {
                        //item.disable();
                        //setReadOnly(item, true);
                        item.setReadOnly(true);
                    }
                });
            }
        };

        var setReadOnly = function (element, readOnly) {
            if (element.el.dom.hasAttribute('readOnly') == false)
                return;

            if (readOnly) {
                element.el.dom.setAttribute('readOnly', true);
            } else {
                element.el.dom.removeAttribute('readOnly');
            }
        }

        var searchPreviousState = function (panel) {
            panel.cascade(function (item) {
                if (item.getXType() == 'datefield' || item.getXType() == 'combo' || item.isFormField) {
                    if (item.readOnly) {
                        alert(item.getXType());
                        actualState.push(item);
                    }
                }
            });
        };

        var openOrEdit = function () {
            var enable = btnOpen.getText() == 'Edit';
            //enableDisablePanelElement(RootPanel, enable);

            if (enable) {
                BasicProductInformationPanel.getEl().unmask();
                BasicProductInformationPanel.removeClass('ext-hide-mask');
                PanelInterestRate.getEl().unmask();
                PanelInterestRate.removeClass('ext-hide-mask');
                btnOpen.setText('Open');
            }
            else {
                BasicProductInformationPanel.getEl().mask();
                BasicProductInformationPanel.addClass('ext-hide-mask');
                PanelInterestRate.getEl().mask();
                PanelInterestRate.addClass('ext-hide-mask');
                btnOpen.setText('Edit');
            }
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
          .ext-hide-mask .ext-el-mask
        {
            opacity: 0.04;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="RootPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnOpen" runat="server" Text="Edit" Icon="NoteEdit">
                                <Listeners>
                                    <Click Handler="openOrEdit();" />
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
                                Layout="FormLayout" LabelWidth="200" Padding="5" Enabled="false">
                                <Items>
                                    <ext:TextField ID="txtName" DataIndex="Name" runat="server" FieldLabel="Name" AnchorHorizontal="100%"
                                        AllowBlank="false" MsgTarget="Side" />
                                    <ext:DateField ID="dtIntroductionDate" DataIndex="IntroductionDate" runat="server"
                                        FieldLabel="Introduction Date" AnchorHorizontal="100%" AllowBlank="false" Editable="false"
                                        Vtype="daterange" EndDateField="dtSalesDiscontinuationDate">
                                    </ext:DateField>
                                    <ext:DateField ID="dtSalesDiscontinuationDate" DataIndex="SalesDiscontinuationDate"
                                        runat="server" FieldLabel="Sales Discontinuation Date" AnchorHorizontal="100%"
                                        AllowBlank="true" MsgTarget="Side" Editable="false" Vtype="daterange" StartDateField="dtIntroductionDate">
                                    </ext:DateField>
                                    <ext:TextArea ID="txtComment" DataIndex="Comment" runat="server" FieldLabel="Comment"
                                        Height="50" AnchorHorizontal="100%" AnchorVertical="25%" />
                                    <ext:TextField ID="txtProductStatus" DataIndex="ProductStatus" runat="server" FieldLabel="Product Status"
                                        AnchorHorizontal="100%" ReadOnly="true" Text="Inactive" />
                                    <ext:CompositeField ID="cmfPersonName" runat="server" FieldLabel="Name" AnchorHorizontal="60%" IDMode="Static">
                                        <Items>
                                            <ext:TextField ID="txtPersonName" ReadOnly="true" AllowBlank="false"
                                                runat="server" Flex="1">
                                            </ext:TextField>
                                            <ext:Button ID="btnNameDetail" Hidden="true" Text="..." runat="server" Width="30">
                                            </ext:Button>
                                            <ext:Button ID="btnBrowseCustomer" Text="Browse.." runat="server" Width="60">
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:Panel ID="Panel1" runat="server" Layout="ColumnLayout" Border="false" Header="false"
                                        Height="80">
                                        <Items>
                                            <ext:Panel runat="server" ColumnWidth=".33" ID="fsCollateralRequirement" Title="Collateral Requirement"
                                                Padding="5" Layout="FormLayout" LabelWidth="80">
                                                <Items>
                                                    <ext:Checkbox runat="server" ID="chkBoxSecured" FieldLabel="Secured" Anchor="100%" />
                                                    <ext:Checkbox runat="server" ID="chkBoxUnsecured" FieldLabel="Unsecured" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" ColumnWidth=".33" ID="fsInterestComputationMode" Title="Interest Computation Mode"
                                                Padding="5" Layout="FormLayout" LabelWidth="100">
                                                <Items>
                                                    <ext:RadioGroup runat="server" ID="rg">
                                                        <Items>
                                                            <ext:Radio runat="server" ID="chkBoxDiminish" BoxLabel="Diminishing Balance Method" />
                                                            <ext:Radio runat="server" ID="chkBoxStraight" BoxLabel="Straight Line Method" />
                                                        </Items>
                                                    </ext:RadioGroup>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" ColumnWidth=".33" ID="fsMethodOfCharging" Title="Method of Changing Interest"
                                                Padding="5" Layout="FormLayout" LabelWidth="100">
                                                <Items>
                                                    <ext:Checkbox runat="server" ID="chkBoxAddOn" FieldLabel="Add-On Interest" />
                                                    <ext:Checkbox runat="server" ID="chkBoxDiscounted" FieldLabel="Discounted Interest" />
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="Panel2" runat="server" Layout="ColumnLayout" Border="false" Header="false">
                                        <Items>
                                            <ext:Panel ID="fsLoanLimit" Title="Loan Limit" runat="server" Height="120" ColumnWidth=".5"
                                                Layout="Form" Padding="5">
                                                <Items>
                                                    <ext:NumberField ID="nfMinimumLoanableAmount" FieldLabel="Minimum Loanable Amount"
                                                        runat="server" AllowBlank="false" MsgTarget="Side" MinValue="0" DecimalPrecision="2"
                                                        AnchorHorizontal="100%" EndNumberField="#{nfMaximumLoanableAmount}"
                                                        Number="1" />
                                                    <ext:NumberField ID="nfMaximumLoanableAmount" FieldLabel="Maximum Loanable Amount"
                                                        runat="server" AllowBlank="false" MsgTarget="Side" MinValue="0" DecimalPrecision="2"
                                                        AnchorHorizontal="100%" StartNumberField="#{nfMinimumLoanableAmount}"
                                                        Number="1" />
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel ID="fsLoanTerm" Title="Loan Term" runat="server" Height="120" ColumnWidth=".5"
                                                Layout="Form" Padding="5">
                                                <Items>
                                                    <ext:NumberField ID="nfMinimumLoanTerm" FieldLabel="Minimum Loan Term" runat="server"
                                                        AllowBlank="false" MsgTarget="Side" MinValue="1" DecimalPrecision="0" AnchorHorizontal="100%"
                                                        EndNumberField="#{nfMaximumLoanTerm}" Number="1" />
                                                    <ext:NumberField ID="nfMaximumLoanTerm" FieldLabel="Maximum Loan Term" runat="server"
                                                        AllowBlank="false" MsgTarget="Side" MinValue="1" DecimalPrecision="0" AnchorHorizontal="100%"
                                                        StartNumberField="#{nfMinimumLoanTerm}" Number="1" />
                                                    <ext:ComboBox runat="server" ID="cmbTimeUnit" FieldLabel="Time Unit" AnchorHorizontal="100%"
                                                        DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                                                        ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"/>
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
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button ID="btnInterestRate" runat="server" Text="Add" Icon="Add">
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
                        </Items>
                    </ext:TabPanel>
                </Items>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
