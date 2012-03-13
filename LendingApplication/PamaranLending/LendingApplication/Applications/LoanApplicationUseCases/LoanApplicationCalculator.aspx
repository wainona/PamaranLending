<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanApplicationCalculator.aspx.cs" Inherits="LendingApplication.Applications.LoanApplicationUseCases.LoanApplicationCalculator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Customer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickinteretrate']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added loan application record.', function () {
                window.proxy.sendToAll('createLoanAgreement', 'createLoanAgreement');
                window.proxy.requestClose();
            });
        };

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickinteretrate') {
                hiddenInterestRateId.setValue(msg.data.ProductFeatureApplicabilityId);
                txtInterestRateDescription.setValue(msg.data.Description);
                nfInterestRate.setValue(msg.data.InterestRate);
                }
            }

        var onEdit = function () {
            txtCustomerName.setAttribute('readOnly', false);
        };

        var onReject = function () {
            window.proxy.requestNewTab('RejectLoanAgreement', '/Applications/FinancialManagement/LoanAgreementUseCases/RejectLoanAgreement.aspx', 'Reject Loan Agreement');
        };

        var onAccept = function () {
            window.proxy.requestNewTab('AcceptLoanAgreement', '/Applications/FinancialManagement/LoanAgreementUseCases/AcceptLoanAgreement.aspx', 'Accept Loan Agreement');
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
    </script>
    <style>
        .transparent
        {
            background-color: transparent;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hiddenProductId" runat="server" Text="3"/>
    <ext:Store runat="server" ID="StoreLoanTerm" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="StoreInterest" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="StorePaymentMode" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="StoreMethodOfCharging" RemoteSort="false">
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
            <ext:Panel ID="RootPanel" runat="server" Layout="FitLayout" Border="false">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnCalculate" runat="server" Text="Calculate" Icon="Calculator">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{BasicProductInformationPanel}.getForm().isValid();"
                                        Success="saveSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel ID="PagePanel" runat="server" EnableTabScroll="true" Padding="5"
                        HideBorders="true" BodyStyle="background-color:transparent">
                        <Items>
                            <ext:Panel runat="server" ID="PanelLoanAgreementDetails" Title="Loan Agreement Details"
                                Padding="5" Border="false" Layout="Form" LabelWidth="300" Height="325">
                                <Items>
                                    <ext:NumberField ID="nfLoanAmount" runat="server" FieldLabel="Loan Amount (Php)" 
                                        Width="500" AllowBlank="false"  DecimalPrecision="2" MinValue="1" />
                                    <ext:NumberField ID="nfLoanTerm" runat="server" FieldLabel="Loan Term" 
                                        Width="500" AllowBlank="false" DecimalPrecision="0" MinValue="1" />
                                    <ext:ComboBox runat="server" ID="cmbLoanTermUOM" FieldLabel="Loan Term UOM" 
                                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                                        SelectOnFocus="true" AllowBlank="false" Width="500" StoreID="StoreLoanTerm"
                                        ValueField="Id" DisplayField="Name" />
                                    <ext:ComboBox runat="server" ID="cmbPaymentMode" FieldLabel="Payment Mode" 
                                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All"
                                        SelectOnFocus="true" AllowBlank="false" Width="500" StoreID="StorePaymentMode"
                                        ValueField="Id" DisplayField="Name"/>
                                        <ext:Hidden ID="hiddenInterestRateId" runat="server" />
                                    <ext:CompositeField ID="CompositeField1" runat="server" Padding="5" Width="800" Layout="Form"
                                        LabelWidth="200" RowHeight=".5">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtInterestRateDescription" FieldLabel="Interest Rate Description"
                                                Editable="false" Mode="Local" SelectOnFocus="true" AllowBlank="false" Width="500">
                                            </ext:TextField>
                                            <ext:Button runat="server" ID="btnBrowse" Text="Browse..">
                                                <Listeners>
                                                            <Click Handler="onBtnPickInterestRate();" />
                                                        </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                    <ext:NumberField ID="nfInterestRate" runat="server" FieldLabel="Interest Rate" Width="500"
                                        DecimalPrecision="0" MinValue="0" AllowBlank="false" MaxValue="100"  />
                                    <ext:ComboBox runat="server" ID="cmbInterestComputationMode" FieldLabel="Interest Computation Mode"
                                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" Width="500"
                                        StoreID="StoreInterest" ValueField="Id" DisplayField="Name"/>
                                    <ext:ComboBox runat="server" ID="cmdMethodOfChargingInterest" FieldLabel="Method of Charging Interest"
                                        Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" Width="500"
                                        StoreID="StoreMethodOfCharging" ValueField="Id" DisplayField="Name"/>
                                    <ext:TextField ID="txtTotalInterestPayment" runat="server" ReadOnly="true" FieldLabel="Total Interest Payment  (Php)"
                                        AnchorHorizontal="60%" Regex="(^[0-9]*[1-9]+[0-9]*\.[0-9]*$)|(^[0-9]*\.[0-9]*[1-9]+[0-9]*$)|(^[0-9]*[1-9]+[0-9]*$)"
                                        MsgTarget="Side" />
                                    <ext:DateField ID="datLoanReleaseDate" runat="server" AllowBlank="false" FieldLabel="Loan Release Date"
                                     Format="m/dd/yyyy" AnchorHorizontal="60%" />
                                    <ext:DateField ID="datPaymentStartDate" runat="server" AllowBlank="false" FieldLabel="Payment Start Date"
                                     Format="m/dd/yyyy" AnchorHorizontal="60%" />
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="PanelAmortizationSchedule" Title="Amortization Schedule" runat="server" Border="false"
                                Padding="5" Layout="FormLayout" LabelWidth="150">
                                <Items>
                                     <ext:GridPanel ID="GridPanelAmortizationSchedule" runat="server" RowHeight=".5"
                                        AnchorHorizontal="100%">
                                        <Items>
                                        </Items>
                                        <Store>
                                            <ext:Store ID="StoreAmortizationSchedule" runat="server">
                                                <Proxy>
                                                    <ext:PageProxy>
                                                    </ext:PageProxy>
                                                </Proxy>
                                                <Reader>
                                                    <ext:JsonReader runat="server">
                                                        <Fields>
                                                            <ext:RecordField Name="PaymentDate" />
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
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server" Sortable="false" Header="Payment Date" DataIndex="PaymentDate" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Sortable="false" Header="Principal Payment" DataIndex="PrincipalPayment" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Sortable="false" Header="Interest Payment" DataIndex="InterestPayment" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Sortable="false" Header="Total Payment" DataIndex="TotalPayment" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Sortable="false" Header="Principal Balance" DataIndex="PrincipalBalance" Width="150">
                                                </ext:Column>
                                                <ext:Column runat="server" Sortable="false" Header="Total Loan Balance" DataIndex="TotalLoanBalance" Width="150">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
