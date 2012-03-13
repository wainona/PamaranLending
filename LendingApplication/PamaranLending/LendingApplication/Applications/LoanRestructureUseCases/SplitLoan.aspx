<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SplitLoan.aspx.cs" Inherits="LendingApplication.Applications.LoanRestructureUseCases.SplitLoan" %>

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
        var financialProduct1 = false;
        var financialProduct2 = false;
        var newBalance1 = 0;
        var newBalance2 = 0;
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addamortizationschedule', 'onpickfinancialproduct']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'addamortizationschedule') {
                window.proxy.requestClose();
            } else if (msg.tag == 'onpickfinancialproduct') {
                if (financialProduct1) {
                    X.FillFinancialProductDetails1(msg.data.Id);
                } else if (financialProduct2) {
                    X.FillFinancialProductDetails2(msg.data.Id);
                }
            }
        };

        var onBtnPickFinancialProduct1 = function () {
            financialProduct1 = true;
            if (financialProduct2)
                financialProduct2 = false;

            var url = '/BestPractice/PickListFinancialProduct.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickFinancialProduct', param, 'Select Financial Product');

        };

        var onBtnPickFinancialProduct2 = function () {
            financialProduct2 = true;
            if (financialProduct1)
                financialProduct1 = false;

            var url = '/BestPractice/PickListFinancialProduct.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickFinancialProduct', param, 'Select Financial Product');
        };

        var saveSuccessful = function () {
            showAlert('Status', 'Split loan successfully saved.', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        }

        var onGenerateAmortizationSchedClick = function () {
            X.FillAmortizationDetails({
                success: function (result) {
                    if (result == 1) {
                        var url = '/Applications/LoanRestructureUseCases/AmortizationScheduleSplitLoan.aspx';
                        var guid = '?guid=' + hiddenResourceGUID.getValue();
                        var param = url + guid;
                        window.proxy.requestNewTab('GenerateAmortizationSplitLoanSchedule', param, 'Generate Split Loan Amortization Schedule');
                    }
                }
            });
        };

        var onPercentChange = function () {
            var balance = txtCarryOverBalance.getValue().replace(',', '');
            var percentage1 = txtPercentage.getValue();
            var newValue = 100 - txtPercentage.getValue();
            var percentage2 = newValue;
            var amount1 = (txtCarryOverAmount1.getValue().replace(',', ''));
            var paymentCount = txtNumberOfPayments.getValue();
            if (parseFloat(amount1) > parseFloat(balance)) {
                txtCarryOverAmount1.setValue('');
            } else if (parseFloat(amount1) <= parseFloat(balance)) {
                hiddenPercentage1.setValue(percentage1);
                hiddenPercentage2.setValue(percentage2);
                //newBalance1 = balance * (percentage1 / 100);
                //newBalance2 = balance - newBalance1;
                newBalance1 = amount1;
                newBalance2 = balance - amount1;
                //txtPercentage.setValue(percentage1 + "% (" + Ext.util.Format.number(newBalance1, '0,0.00') + ")");
                //txtPercentage2.setValue(newValue + "% (" + Ext.util.Format.number(newBalance2, '0,0.00') + ")");
                txtCarryOverAmount2.setValue(Ext.util.Format.number(newBalance2, '0,0.00'));
                if (paymentCount != 0) {
                    txtLessPayment1.setValue('');
                    txtLessPayment2.setValue('');
                }
            }
            txtCarryOverBalance1.setValue('');
            txtCarryOverBalance2.setValue('');
            nfInterestRate.setValue('');
            nfInterestRate2.setValue('');
        };

        var onInputLess1 = function () {
            var payment1 = txtLessPayment1.getValue().replace(',', '');
            var newRate = nfInterestRate.getValue() / 100;
            var newLoanBalance = (newBalance1 + (newBalance1 * newRate)) - payment1;
            var receivableAdd = nfReceivableAdd1.getValue();
            newBalance1 = newBalance1 + receivableAdd;
            //txtCarryOverBalance1.setValue(Ext.util.Format.number(newLoanBalance, '0,0.00'));
            X.ComputeBalanceLessPayment(newBalance1, payment1, 1);
            var term1 = txtTerm.getValue();
            var term2 = txtTerm2.getValue();

            if (term1 == 0 && term2 == 0) {
                btnSave.show();
                btnGenerate.hide();
            } else {
                btnSave.hide();
                btnGenerate.show();
            }
        };

        var onInputLess2 = function () {
            var payment2 = txtLessPayment2.getValue().replace(',', '');
            var newRate = nfInterestRate2.getValue() / 100;
            var newLoanBalance = (newBalance2 + (newBalance2 * newRate)) - payment2;
            var receivableAdd = nfReceivableAdd2.getValue();
            newBalance2 = newBalance2 + receivableAdd;
            //txtCarryOverBalance2.setValue(Ext.util.Format.number(newLoanBalance, '0,0.00'));
            X.ComputeBalanceLessPayment(newBalance2, payment2, 2);

            var term1 = txtTerm.getValue();
            var term2 = txtTerm2.getValue();

            if (term1 == 0 && term2 == 0) {
                btnSave.show();
                btnGenerate.hide();
            } else {
                btnSave.hide();
                btnGenerate.show();
            }
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
                //btnGenerate.disable();
                if (btnGenerate.hidden == true) {
                    btnSave.disable();
                } else {
                    btnGenerate.disable();
                }
            }
        };

        var onNumberChange = function () {
            var paymentCount = txtNumberOfPayments.getValue();
            var payment1 = txtNumberofDeduction1.getValue();
            var payment2 = 0;
            if (paymentCount != 0) {
                payment2 = paymentCount - payment1;
            }
            txtNumberofDeduction2.setValue(payment2);

            var icm = cmbInterestComputationMode1.getValue();
            var icm2 = cmbInterestComputationMode2.getValue();
            if (icm != -1) {
                onInputLess1();
            }

            if (icm2 != -1) {
                onInputLess2();
            }
        };

        var checkReceivableAmount = function () {
            var totalReceivable = txtCarryOverReceivables.getValue().replace(',', '');
            var receivableAdd = nfReceivableAdd1.getValue();
            var receivableAdd2 = nfReceivableAdd2.getValue();

            if (receivableAdd == 0) {
                nfReceivableAdd2.maxValue = totalReceivable;
            } else if (receivableAdd > 0) {
                nfReceivableAdd2.maxValue = totalReceivable - receivableAdd;
            }

            if (receivableAdd2 == 0) {
                nfReceivableAdd1.maxValue = totalReceivable;
            } else if (receivableAdd2 > 0) {
                nfReceivableAdd1.maxValue = totalReceivable - receivableAdd2;
            }

            var icm = cmbInterestComputationMode1.getValue();
            var icm2 = cmbInterestComputationMode2.getValue();
            if (icm != -1) {
                onInputLess1();
            }

            if (icm2 != -1) {
                onInputLess2();
            }
        };

        var onLoanTermChange = function () {
            var term1 = txtTerm.getValue();
            var term2 = txtTerm2.getValue();

            if (term1 == 0 && term2 == 0) {
                btnSave.show();
                btnGenerate.hide();
            } else {
                btnSave.hide();
                btnGenerate.show();
            }

            if (term1 == 0) {
                chkAddChecks1.disable();
            } else {
                chkAddChecks1.enable();
            }

            if (term2 == 0) {
                chkAddChecks2.disable();
            } else {
                chkAddChecks2.enable();
            }
        };

        var saveFailure = function () {
            var status = hiddenStatus.getValue();
            var title = '';
            var message = '';
            title = 'Status';
            message = 'Saving failed.';
            showAlert(title, message, function () {
            });
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
            color: Red;
            font-weight: bold;
            font-size: larger;
        }
        
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="hiddenPercentage1" runat="server" />
    <ext:Hidden ID="hiddenPercentage2" runat="server" />
    <%-------------------Unit of Measure Store----------------------%>
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
    <%---------------------Payment Mode Store-----------------------%>
    <ext:Store runat="server" ID="PaymentModeStore1" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--------------Interest Computation Mode Store-----------------%>
    <ext:Store runat="server" ID="InterestComputationModeStore1" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%-------------Method of Charging Interest Store----------------%>
    <ext:Store runat="server" ID="MethodOfChargingInterestStore1" RemoteSort="false">
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
    <ext:Store runat="server" ID="CollateralRequirementStore1" RemoteSort="false">
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
    <ext:Store runat="server" ID="CollateralRequirementStore2" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%---------------------Payment Mode Store-----------------------%>
    <ext:Store runat="server" ID="PaymentModeStore2" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--------------Interest Computation Mode Store-----------------%>
    <ext:Store runat="server" ID="InterestComputationModeStore2" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%-------------Method of Charging Interest Store----------------%>
    <ext:Store runat="server" ID="MethodOfChargingInterestStore2" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%---------------------Interest Rate Store----------------------%>
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
            <%------------------------------Root Panel-------------------------------%>
            <ext:FormPanel ID="RootFormPanel" runat="server" Layout="FitLayout" Border="false" MonitorValid="true" MonitorPoll="500">
                <%-----Top Bar-----%>
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" Hidden="true" Disabled="true" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Failure="saveFailure"
                                        Success="saveSuccessful" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnGenerate" runat="server" Text="Generate Schedule" Icon="Calculator">
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
                    <%---------------------Main Panel-------------------------%>
                    <ext:Panel runat="server" Border="false" Layout="RowLayout"  AutoScroll="true">
                        <Items>
                            <ext:Panel runat="server" LabelWidth="250" Border="false" RowHeight=".25" Padding="15">
                                <Items>
                                    <ext:TextField ID="txtCarryOverBalance" Height="25" runat="server" Width="500px"
                                        FieldLabel="Loan Amount" ReadOnly="true" />
                                    <ext:TextField ID="txtTotalPayments" Height="25" runat="server" Width="500px"
                                        FieldLabel="Total Payments" ReadOnly="true" />
                                    <ext:TextField ID="txtNumberOfPayments" Height="25" runat="server" Width="500px"
                                        FieldLabel="Total Number of Payments" ReadOnly="true" />
                                    <ext:TextField ID="txtCarryOverReceivables" Height="25" runat="server" Width="500px"
                                        FieldLabel="Total of Unpaid Receivables" ReadOnly="true" />
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="PanelBasicInformation" Border="false"  AutoScroll="true" Padding="5" LabelWidth="250"
                                Layout="ColumnLayout" RowHeight=".75" AnchorHorizontal="100%">
                                <Items>
                                    <%--Account 1 Panel--%>
                                    <ext:Panel ID="AccountPanel1" Padding="5" ColumnWidth=".5" AutoScroll="true"  Title="Account 1" runat="server">
                                        <Items>
                                            <ext:Hidden ID="hdnSelectedLoanID" runat="server">
                                            </ext:Hidden>
                                            <ext:TextField ID="txtCarryOverBalance1" Height="25" runat="server" Width="500px"
                                                FieldLabel="Carry Over Balance" ReadOnly="true" LabelStyle="font-weight: bold;" />
                                            <ext:TextField ID="txtPercentage" runat="server" FieldLabel="Percentage (%)" Width="500"
                                                AnchorHorizontal="60%" AllowBlank="true" EnableKeyEvents="true" Hidden="true" >
                                                <Listeners>
                                                    <KeyUp Handler="onPercentChange();" />
                                                    <Focus Handler="this.setValue('');" />
                                                </Listeners>    
                                            </ext:TextField>
                                            <ext:TextField ID="txtCarryOverAmount1" runat="server" FieldLabel="Amount" Width="500"
                                                AnchorHorizontal="60%" AllowBlank="false" MaskRe="[0-9]" >
                                                <Listeners>
                                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00')); onPercentChange();" />
                                                    <Focus Handler="this.setValue('');" />
                                                </Listeners>    
                                            </ext:TextField>
                                            <ext:NumberField ID="nfReceivableAdd1" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                                FieldLabel="Receivable Amount To Carry Over" EnableKeyEvents="true" MinValue="0" DecimalPrecision="2">
                                                <Listeners>
                                                    <KeyUp Handler="checkReceivableAmount();" />
                                                </Listeners>    
                                            </ext:NumberField>
                                            <ext:NumberField ID="txtNumberofDeduction1" Height="25" runat="server" Width="500px"
                                                FieldLabel="Number of Payments to Deduct" EnableKeyEvents="true" MaskRe="[0-9]" >
                                                <Listeners>
                                                    <KeyUp Handler="onNumberChange();" />
                                                </Listeners>    
                                            </ext:NumberField>
                                            <ext:TextField ID="txtLessPayment1" AllowBlank="false" Height="25" runat="server" Width="500px"
                                                FieldLabel="Less Payment" MaskRe="[0-9\.\,]" ReadOnly="true">
                                                <Listeners>
                                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                                    <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                                                </Listeners>
                                            </ext:TextField>
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
                                                ForceSelection="true" Editable="false" AllowBlank="false"><Listeners>
                                                    <Select Handler="onInputLess1();" />
                                                </Listeners></ext:ComboBox>
                                            <ext:NumberField ID="nfInterestRate" AllowBlank="false" Width="500" runat="server" FieldLabel="Interest Rate"
                                                MaxValue="100" MinValue="0" EnableKeyEvents="true" AnchorHorizontal="60%" >
                                                <Listeners>
                                                    <KeyUp Handler="onInputLess1();" />
                                                </Listeners>    
                                            </ext:NumberField>
                                            <ext:DateField ID="datLoanReleaseDate1" Hidden="true" runat="server" FieldLabel="Loan Release Date"
                                                Width="500" Editable="false" AnchorHorizontal="60%" />
                                            <ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                                AnchorHorizontal="60%" Width="500" Editable="false" AllowBlank="false" />
                                            <ext:Hidden ID="hiddenProductId1" runat="server" />
                                            <ext:Hidden ID="hiddenLoanTermTimeUnitId1" runat="server" />
                                            <ext:ComboBox ID="cmbCollateralRequirement1" Width="500" runat="server" FieldLabel="Collateral Requirement"
                                                AnchorHorizontal="60%" StoreID="CollateralRequirementStore1" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbInterestComputationMode1" Width="500" runat="server" FieldLabel="Interest Computation Mode"
                                                AnchorHorizontal="60%" StoreID="InterestComputationModeStore1" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" >
                                                <Listeners>
                                                    <Select Handler="onInputLess1();" />
                                                </Listeners>    
                                            </ext:ComboBox>
                                            <ext:Checkbox ID="chkAddChecks1" FieldLabel="Add Post Dated Checks" LabelWidth="250" LabelSeparator="?" runat="server" />
                                            <ext:NumberField ID="txtTerm" runat="server" EnableKeyEvents="true" AllowBlank="false" Width="500" FieldLabel="Loan Term" AnchorHorizontal="60%">
                                                <Listeners>
                                                    <KeyUp Handler="onLoanTermChange();" />
                                                </Listeners>
                                            </ext:NumberField>
                                            <ext:ComboBox ID="cmbUnit1" runat="server" AllowBlank="false" Width="500" FieldLabel="Loan Term Unit" AnchorHorizontal="60%"
                                                        StoreId="UnitOfMeasureStore" ValueField="Id" DisplayField="Name" 
                                                        ForceSelection="true" Editable="false" />
                                            <ext:ComboBox ID="cmbPaymentMode1" Width="500" runat="server" FieldLabel="Payment Mode"
                                                AnchorHorizontal="60%" StoreID="PaymentModeStore1" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbMethodOfChargingInterest1" Width="500" runat="server" FieldLabel="Method of Charging Interest"
                                                AnchorHorizontal="60%" StoreID="MethodOfChargingInterestStore1" ValueField="Id" DisplayField="Name"
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                        </Items>
                                    </ext:Panel>
                                    <%--Account 2 Panel--%>
                                    <ext:Panel ID="AccountPanel2" Padding="5" ColumnWidth=".5" AutoScroll="true"  Title="Account 2" runat="server">
                                        <Items>
                                            <ext:TextField ID="txtCarryOverBalance2" Height="25" runat="server" Width="500px"
                                                FieldLabel="Carry Over Balance" ReadOnly="true" LabelStyle="font-weight: bold;" />
                                            <ext:TextField ID="txtPercentage2" Width="500" AllowBlank="true" ReadOnly="true" runat="server" FieldLabel="Percentage (%)"
                                                AnchorHorizontal="60%" Hidden="true" />
                                            <ext:TextField ID="txtCarryOverAmount2" runat="server" FieldLabel="Amount" Width="500"
                                                AnchorHorizontal="60%" AllowBlank="false" MaskRe="[0-9]" />
                                            <ext:NumberField ID="nfReceivableAdd2" Height="25" AllowBlank="false" runat="server" Width="500px" 
                                                FieldLabel="Receivable Amount To Carry Over" EnableKeyEvents="true" MinValue="0" DecimalPrecision="2">
                                                <Listeners>
                                                    <KeyUp Handler="checkReceivableAmount();" />
                                                </Listeners>    
                                            </ext:NumberField>
                                            <ext:NumberField ID="txtNumberofDeduction2" Height="25" runat="server" Width="500px"
                                                FieldLabel="Number of Payments to Deduct" MaskRe="[0-9]" ReadOnly="true" />
                                            <ext:TextField ID="txtLessPayment2" AllowBlank="false" Height="25" runat="server" Width="500px"
                                                FieldLabel="Less Payment" MaskRe="[0-9\.\,]" ReadOnly="true">
                                                <Listeners>
                                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                                    <%--<Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />--%>
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Hidden ID="hiddenProductId2" runat="server" />
                                            <ext:Hidden ID="hiddenLoanTermTimeUnitId2" runat="server" />
                                            <ext:CompositeField ID="cfFinancialProduct2" runat="server" AnchorHorizontal="60%" Width="500">
                                                <Items>
                                                    <ext:TextField ID="txtLoanProductName2" FieldLabel="Loan Product Name" Width="178" Flex="1" runat="server" ReadOnly="true" AllowBlank="false"/>
                                                    <ext:Button ID="btnPickFinancialProduct2" runat="server" Width="15" Text="Browse...">
                                                        <Listeners>
                                                            <Click Handler="onBtnPickFinancialProduct2();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:ComboBox ID="cmbInterestRate2" Width="500" runat="server" FieldLabel="Interest Rate Description"
                                                AnchorHorizontal="60%" StoreID="InterestRateDescriptionStore" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" ><Listeners>
                                                    <Select Handler="onInputLess2();" />
                                                </Listeners> </ext:ComboBox>
                                            <ext:NumberField ID="nfInterestRate2" Width="500" runat="server" FieldLabel="Interest Rate"
                                                MaxValue="100" MinValue="0" AnchorHorizontal="60%" EnableKeyEvents="true" AllowBlank="false" >
                                                <Listeners>
                                                    <KeyUp Handler="onInputLess2();" />
                                                </Listeners>    
                                            </ext:NumberField>
                                            <ext:DateField ID="datLoanReleaseDate2" Hidden="true" runat="server" FieldLabel="Loan Release Date"
                                                Width="500" Editable="false" AnchorHorizontal="60%" />
                                            <ext:DateField ID="datPaymentStartDate2" runat="server" FieldLabel="Payment Start Date"
                                                AnchorHorizontal="60%" Width="500" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbCollateralRequirement2" Width="500" runat="server" FieldLabel="Collateral Requirement"
                                                AnchorHorizontal="60%" StoreID="CollateralRequirementStore2" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbInterestComputationMode2" Width="500" runat="server" FieldLabel="Interest Computation Mode"
                                                AnchorHorizontal="60%" StoreID="InterestComputationModeStore2" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" >
                                                <Listeners>
                                                    <Select Handler="onInputLess2();" />
                                                </Listeners>    
                                            </ext:ComboBox>
                                            <ext:Checkbox ID="chkAddChecks2" FieldLabel="Add Post Dated Checks" LabelWidth="250" LabelSeparator="?" runat="server" />
                                            <ext:NumberField ID="txtTerm2" runat="server" EnableKeyEvents="true" AllowBlank="false" Width="500" FieldLabel="Loan Term" AnchorHorizontal="60%" >
                                                <Listeners>
                                                    <KeyUp Handler="onLoanTermChange();" />
                                                </Listeners>
                                            </ext:NumberField>
                                            <ext:ComboBox ID="cmbUnit2" runat="server" AllowBlank="false" Width="500" FieldLabel="Loan Term Unit" AnchorHorizontal="60%"
                                                        StoreId="UnitOfMeasureStore" ValueField="Id" DisplayField="Name" ForceSelection="true" 
                                                        Editable="false" />
                                            <ext:ComboBox ID="cmbPaymentMode2" Width="500" runat="server" FieldLabel="Payment Mode"
                                                AnchorHorizontal="60%" StoreID="PaymentModeStore2" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true" Editable="false" AllowBlank="false" />
                                            <ext:ComboBox ID="cmbMethodOfChargingInterest2" Width="500" runat="server" FieldLabel="Method of Charging Interest"
                                                AnchorHorizontal="60%" StoreID="MethodOfChargingInterestStore2" ValueField="Id" DisplayField="Name" 
                                                ForceSelection="true"  Editable="false" AllowBlank="false" />
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
