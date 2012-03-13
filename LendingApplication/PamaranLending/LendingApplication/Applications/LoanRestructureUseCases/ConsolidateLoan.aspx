<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsolidateLoan.aspx.cs"
    Inherits="LendingApplication.Applications.LoanRestructureUseCases.ConsolidateLoan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addamortizationschedule', 'onpickfinancialproduct']);
            window.proxy.on('messagereceived', onMessageReceived);
        });


        var onMessageReceived = function (msg) {
            if (msg.tag == 'addamortizationschedule') {
                window.proxy.requestClose();
            } else if (msg.tag == 'onpickfinancialproduct') {
                X.FillFinancialProductDetails1(msg.data.Id);
            }
        };

        var onBtnPickFinancialProduct1 = function () {
            var url = '/BestPractice/PickListFinancialProduct.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickFinancialProduct', param, 'Select Financial Product');

        };

        var saveSuccessful = function () {
            showAlert('Status', 'Loans successfully consolidated.', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        }

        var onGenerateAmortizationSchedClick = function () {
            X.GenerateConsolidatedLoanSchedule({
                success: function (result) {
                    if (result == 1) {
                        var url = '/Applications/LoanRestructureUseCases/GenerateAmortizationSchedule.aspx';
                        var guid = '?guid=' + hiddenResourceGUID.getValue();
                        var param = url + guid;
                        window.proxy.requestNewTab('GenerateAmortizationConsolidatedLoanSchedule', param, 'Generate Consolidated Loan Amortization Schedule'); 
                    }
                }
            });
        };

        var onFormValidated = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                if (btnGenerate.hidden == true) {
                    btnSave.enable();
                } else {
                    btnGenerate.enable();
                }
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                if (btnGenerate.hidden == true) {
                    btnSave.disable();
                } else {
                    btnGenerate.disable();
                }
            }
        }

        var checkGrid = function () {
            if (storeAmortizationSchedule.Count() == 0) {
                btnSave.disable();
            } else {
                btnSave.enable();
            }
        };

        var onInterestInputChange = function () {
            X.Fill();
        };

        var onLoantermChange = function () {
            var term = txtTerm.getValue();
            if (term == 0) {
                btnGenerate.hide();
                btnSave.show();
                chkAddChecks1.disable();
            } else {
                btnGenerate.show();
                btnSave.hide();
                chkAddChecks1.enable();
            }
        };
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
        }
    </script>
    <style>
        .transparent
        {
            background-color: transparent;
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
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenRandomKey1" runat="server" />
    <ext:Hidden ID="hiddenRandomKey2" runat="server" />
    <%--------------------------Unit of Measure Store-----------------------------%>
    <ext:Store runat="server" ID="UnitOfMeasureStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%-----------------------------Payment Mode Store-----------------------------%>
    <ext:Store runat="server" ID="PaymentModeStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%-----------------------Interest Computation Mode Store----------------------%>
    <ext:Store runat="server" ID="InterestComputationModeStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%----------------------Method of Charging Interest Store---------------------%>
    <ext:Store runat="server" ID="MethodOfChargingInterestStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--------------Collateral Requirement Store-----------------%>
    <ext:Store runat="server" ID="CollateralRequirementStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%----------------------------Interest Rate Store-----------------------------%>
    <ext:Store runat="server" ID="InterestRateDescriptionStore" RemoteSort="false">
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
            <ext:FormPanel ID="RootFormPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true" MonitorPoll="500" LabelWidth="250">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" Hidden="true" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click"
                                        Success="saveSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnGenerate" Hidden="false" runat="server" Text="Generate Schedule" Icon="Calculator">
                                <Listeners>
                                    <Click Handler="onGenerateAmortizationSchedClick();" />
                                </Listeners>
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
                    <ext:Panel runat="server" ID="PanelPaymentSchedule" Border="false"
                        Padding="5" LabelWidth="275" LabelAlign="Left">
                        <Items>
                            <ext:Panel runat="server" LabelWidth="275" LabelAlign="Left" Layout="FormLayout" Border="false">
                                <Items>
                                    <ext:Hidden ID="hdnSelectedLoanId1" runat="server"></ext:Hidden>
                                    <ext:Hidden ID="hdnSelectedLoanId2" runat="server"></ext:Hidden>
                                    <ext:Panel ID="Panel1" runat="server" LabelWidth="300" Border="false" Padding="15">
                                        <Items>
                                            <ext:TextField ID="txtCarryOverBalance" Height="25" AllowBlank="false" 
                                                ReadOnly="true" runat="server" AnchorHorizontal="46.5%" Width="500" 
                                                FieldLabel="Balance to Carry Over" />
                                            <ext:TextField ID="txtCarryOverReceivables1" Height="25" runat="server" Width="500px"
                                                FieldLabel="Total of Unpaid Receivables (Account 1)" ReadOnly="true" />
                                            <ext:NumberField ID="nfReceivableAdd1" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                                FieldLabel="Receivable Amount To Carry Over (Account 1)" MinValue="0" DecimalPrecision="2">    
                                            </ext:NumberField>
                                            <ext:TextField ID="txtCarryOverReceivables2" Height="25" runat="server" Width="500px"
                                                FieldLabel="Total of Unpaid Receivables (Account 2)" ReadOnly="true" />
                                            <ext:NumberField ID="nfReceivableAdd2" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                                FieldLabel="Receivable Amount To Carry Over (Account 2)" MinValue="0" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="pnlConsolidateLoanDetails" Title="Consolidated Loan Details" LabelWidth="250" Padding="15" runat="server" AnchorHorizontal="100%">
                                        <Items>
                                            <ext:DateField ID="datLoanReleaseDate" Hidden="true" runat="server" FieldLabel="Loan Release Date"
                                                Width="500" Editable="false"  />
                                            <ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                                AnchorHorizontal="60%" Width="500" AllowBlank="false" Editable="false" />
                                            <ext:Hidden ID="hiddenProductId1" runat="server" />
                                            <ext:Hidden ID="hiddenLoanTermTimeUnitId1" runat="server" />
                                            <ext:CompositeField ID="cfFinancialProduct1" runat="server" AnchorHorizontal="60%" Width="500">
                                                <Items>
                                                    <ext:TextField ID="txtLoanProductName1" FieldLabel="Loan Product Name" Width="178" Flex="1" runat="server" ReadOnly="true" AllowBlank="false"/>
                                                    <ext:Button ID="btnPickFinancialProduct1" runat="server" Width="15" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickFinancialProduct1();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:ComboBox ID="cmbInterestRate1" Width="500" runat="server" FieldLabel="Interest Rate Description"
                                                AnchorHorizontal="60%" StoreID="InterestRateDescriptionStore" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false">
                                                <Listeners>
                                                    <Select Handler="onInterestInputChange();" />
                                                </Listeners>      
                                            </ext:ComboBox>
                                            <ext:NumberField ID="nfInterestRate" Width="500" runat="server" FieldLabel="Interest Rate"
                                                MaxValue="100" MinValue="0" AnchorHorizontal="60%" EnableKeyEvents="true" AllowBlank="false">
                                                <Listeners>
                                                    <KeyUp Handler="onInterestInputChange();" />
                                                    <Focus Handler="this.setValue('');" />
                                                </Listeners>      
                                            </ext:NumberField>
                                            <ext:ComboBox ID="cmbCollateralRequirement1" Width="500" runat="server" FieldLabel="Collateral Requirement"
                                                AnchorHorizontal="60%" StoreID="CollateralRequirementStore" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbInterestComputationMode1" Width="500" runat="server" FieldLabel="Interest Computation Mode"
                                                AnchorHorizontal="60%" StoreID="InterestComputationModeStore" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" EnableKeyEvents="true" AllowBlank="false">
                                                <Listeners>
                                                    <KeyUp Handler="onInterestInputChange();" />
                                                </Listeners>      
                                            </ext:ComboBox>
                                            <ext:Checkbox ID="chkAddChecks1" FieldLabel="Add Post Dated Checks" LabelWidth="250" LabelSeparator="?" runat="server" />
                                            <ext:NumberField ID="txtTerm" runat="server" EnableKeyEvents="true" AllowBlank="false" Width="500" FieldLabel="Loan Term" AnchorHorizontal="60%">
                                                <Listeners>
                                                    <KeyUp Handler="onLoantermChange();" />
                                                </Listeners>
                                            </ext:NumberField>
                                            <ext:ComboBox ID="cmbUnit1" runat="server" Width="500" FieldLabel="Loan Term Unit" AnchorHorizontal="60%"
                                                        StoreId="UnitOfMeasureStore" ValueField="Id" DisplayField="Name" 
                                                        ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbPaymentMode1" Width="500" runat="server" FieldLabel="Payment Mode"
                                                AnchorHorizontal="60%" StoreID="PaymentModeStore" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbMethodOfChargingInterest1" Width="500" runat="server" FieldLabel="Method of Charging Interest"
                                                AnchorHorizontal="60%" StoreID="MethodOfChargingInterestStore" ValueField="Id" DisplayField="Name"
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                        </Items>
                                </ext:Panel>
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
