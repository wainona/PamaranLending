<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateAmortizationSchedule.aspx.cs"
    Inherits="LendingApplication.Applications.AmortizationScheduleUseCases.GenerateAmortizationSchedule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickfinancialproduct', 'onpickinteretrate']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickfinancialproduct') {
                X.FillFinancialProductDetails(msg.data.Id, {
                    success: function () {
                        markIfRequired(nfLoanTerm);
                    } 
                });
            }
            else if (msg.tag == 'onpickinteretrate') {
                cmbInterestRate.setValue(msg.data.FeatureID);
                nfInterestRate.setValue(msg.data.InterestRate);
            }
        };

        var saveSuccessful = function () {
            showAlert('Status', 'Contact record was successfully added', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        }

        var onBtnPickFinancialProduct = function () {
            var url = '/BestPractice/PickListFinancialProduct.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickFinancialProduct', param, 'Select Financial Product');
        };

        var onBtnPickInterestRate = function () {
            if (hiddenProductId.getValue()) {
                var url = '/BestPractice/PickListInterestRate.aspx';
                var param = url + "?mode=" + 'single';
                param += "&financialProductId=" + hiddenProductId.getValue();
                window.proxy.requestNewTab('PickInterestRate', param, 'Select Interest Rate');
            }
            else {
                showAlert('Information', 'Please select a financial product before selecting an interest rate.');
            }
        };

        var onFormValidated = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnGenerate.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
                btnGenerate.disable();
            }
        }

        var adjustPaymentStartDate = function () {
            X.setPaymentDate();
        }

        var creatingLoanApplicationSuccessful = function () {
            var url = '/Applications/LoanApplicationUseCases/CreateLoanApplication.aspx';
            var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
            window.proxy.requestNewTab('GAS_CreateLoanApplication', param, 'Create Loan Application');
        }

        var successfullyAppliedChanges = function () {
            window.proxy.sendToParent('applyloanamortizationchanges', 'applyloanamortizationchanges');
            window.proxy.requestClose();
        }

        var generationSuccessful = function () {
            btnCreateLoanApplication.enable();
            btnApply.enable();
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden ID="hdnLoanTermIndicator" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="hdnSelectedLoanID" runat="server" />
    <ext:Hidden ID="hiddenResourceGuid" runat="server">
    </ext:Hidden>
    <ext:Store runat="server" ID="storeCollateralRequirement" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeMethodOfChargingInterest" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storePaymentMethod" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeInterestComputationMode" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="storeInterestRate" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="pageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel runat="server" ID="RootPanel" Layout="FitLayout" Border="false" MonitorPoll="500"
                MonitorValid="true">
                <TopBar>
                    <ext:Toolbar runat="server" ID="RootToolBar">
                        <Items>
                            <ext:Button ID="btnGenerate" runat="server" Text="Generate Schedule" Icon="Calculator"
                                Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" Success="generationSuccessful();">
                                        <EventMask ShowMask="true" Msg="Generating amortization schedule..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnApply" runat="server" Text="Apply Changes"
                                Icon="ApplicationForm" Disabled="true" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnCreateLoanApplication_Click" Success="successfullyAppliedChanges();">
                                        <EventMask ShowMask="true" Msg="Applying changes..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCreateLoanApplication" runat="server" Text="Create Loan Application Base On Input"
                                Icon="ApplicationForm" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnCreateLoanApplication_Click" Success="creatingLoanApplicationSuccessful();">
                                        <EventMask ShowMask="true" Msg="Creating Loan Application..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnErrorTrue" runat="server" Hidden="true" Text="Error True" Icon="Cancel">
                                <DirectEvents>
                                    <Click OnEvent="btnErrorTrue_Click" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Button ID="btnClose" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelGenerateAmortizationSchedule" Title="Generate Application Schedule"
                        Padding="5" Layout="FormLayout" LabelWidth="180" AutoScroll="true">
                        <Items>
                            <ext:Hidden ID="hiddenProductId" runat="server" />
                            <ext:Hidden ID="hiddenLoanTermTimeUnitId" runat="server" />
                            <ext:CompositeField ID="cfFinancialProduct" DataIndex="" runat="server"
                                Width="500">
                                <Items>
                                    <ext:TextField ID="txtLoanProductName" runat="server" ReadOnly="true" Width="400" FieldLabel="Loan Product Name" AllowBlank="false"/>
                                    <ext:Button ID="btnPickFinancialProduct" runat="server" Text="Browse...">
                                        <Listeners>
                                            <Click Handler="onBtnPickFinancialProduct();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField ID="nfLoanAmount" runat="server" FieldLabel="Loan Amount" Width="400"
                                AllowBlank="false" MaskRe="[0-9\.\,]">
                                <Listeners>
                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                </Listeners>
                            </ext:TextField>
                            <ext:NumberField ID="nfLoanTerm" runat="server" FieldLabel="Loan Term"
                                Width="400" ReadOnly="true" AllowBlank="false" DecimalPrecision="0"/>
                            <ext:ComboBox runat="server" ID="cmbCollateralRequirement" FieldLabel="Collateral Requirement"
                                Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeCollateralRequirement"
                                ValueField="Id" DisplayField="Name" />
                            <ext:ComboBox runat="server" ID="cmbInterestComputationMode" FieldLabel="Interest Computation Mode"
                                Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeInterestComputationMode"
                                ValueField="Id" DisplayField="Name" />
                            <ext:ComboBox runat="server" ID="cmbPaymentMode" FieldLabel="Payment Mode" Width="400"
                                Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                                SelectOnFocus="true" AllowBlank="false" StoreID="storePaymentMethod" ValueField="Id"
                                DisplayField="Name" />
                            <ext:ComboBox runat="server" ID="cmbMethodOfChargingInterest" FieldLabel="Method of Charging Interest"
                                Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                TriggerAction="All" SelectOnFocus="true" AllowBlank="false" StoreID="storeMethodOfChargingInterest"
                                ValueField="Id" DisplayField="Name" />
                            <ext:CompositeField ID="cfDescription" runat="server" Width="500">
                                <Items>
                                    <ext:ComboBox runat="server" ID="cmbInterestRate" FieldLabel="Description" Width="400" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" AllowBlank="false"
                                        StoreID="storeInterestRate" ValueField="Id" DisplayField="Name" />
                                    <ext:Button ID="btnPickInterestRateDesc" runat="server" Text="Browse...">
                                        <Listeners>
                                            <Click Handler="onBtnPickInterestRate();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:CompositeField ID="cfInterestRate" runat="server"
                                Width="500">
                                <Items>
                                    <ext:NumberField ID="nfInterestRate" FieldLabel="Interest Rate" runat="server" AnchorHorizontal="100%" DecimalPrecision="2"
                                        MinValue="0" MaxValue="100" AllowBlank="false" Width="400" />
                                    <ext:Label ID="Label1" runat="server" Text="%"/>
                                </Items>
                            </ext:CompositeField>
                            <ext:DateField ID="datLoanReleaseDate" runat="server" FieldLabel="Loan Release Date"
                                Width="400" Editable="false" AllowBlank="false" Vtype="daterange" EndDateField="datPaymentStartDate">
                                    <Listeners>
                                        <Select Handler="adjustPaymentStartDate();" />
                                    </Listeners>
                                </ext:DateField>
                            <ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                Width="400" Editable="false" AllowBlank="false" Vtype="daterange" StartDateField="datLoanReleaseDate"/>
                            <ext:Panel runat="server" AutoHeight="true" LabelWidth="420" Border="false">
                                <Items>
                                    <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Height="400" MinHeight="400"
                                        Title="Amortization Schedule">
                                        <Store>
                                            <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                               
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="Counter" />
                                                            <ext:RecordField Name="ScheduledPaymentDate" Type="Date" />
                                                            <ext:RecordField Name="PrincipalPayment" />
                                                            <ext:RecordField Name="InterestPayment" />
                                                            <ext:RecordField Name="TotalPayment" />
                                                            <ext:RecordField Name="PrincipalBalance" />
                                                            <ext:RecordField Name="TotalLoanBalance" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                        </SelectionModel>
                                        <ColumnModel runat="server" ID="clmAmortizationSchedule" Width="100%">
                                            <Columns>
                                                <ext:Column Header="Unit" DataIndex="Counter" Wrap="true"
                                                    Width="100" Sortable="false"/>
                                                <ext:DateColumn Header="Payment Due Date" DataIndex="ScheduledPaymentDate" Wrap="true"
                                                    Width="130" Sortable="false" />
                                                <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="130"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total" DataIndex="TotalPayment" Wrap="true" Width="130"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" />
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
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
    </form>
</body>
</html>
