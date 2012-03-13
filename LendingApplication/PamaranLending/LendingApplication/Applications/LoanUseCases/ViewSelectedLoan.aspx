<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewSelectedLoan.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.ViewSelectedLoan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>View Selected Loan</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onwriteoff', 'onunderlitigation', 'addcollection']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onwriteoff' || msg.tag == 'onunderlitigation') {
                window.proxy.sendToParent('closeselectedloan', 'closeselectedloan');
                window.proxy.requestClose();
            };

            if (msg.tag == 'addcollection') {
                gridPaymentHistory.reload();
            };
        }

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully updated the record.', function () {
                window.proxy.sendToAll('updatecustomer', 'updatecustomer');
                window.proxy.requestClose();
            });
        }

        var onBtnWriteOffClick = function () {
            var url = '/Applications/LoanUseCases/WriteOffLoan.aspx';
            var id = 'id=' + hdnSelectedLoanId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('WriteOffLoan', param, 'Write Off Loan');
        };

        var onBtnUnderLitigationClick = function () {
            var url = '/Applications/LoanUseCases/UnderLitigationLoan.aspx';
            var id = 'id=' + hdnSelectedLoanId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('UnderLitigationLoan', param, 'Under Litigation Loan');
        };

        var onBtnPrintCertificateClick = function () {
            var url = '/Applications/LoanUseCases/CertificateOfFullPayment.aspx';
            var id = 'id=' + hdnSelectedLoanId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('CertificateOfFullPayment', param, 'Certificate Of Full Payment');
        };

        var onBtnPrintLoanRecord = function () {
            var url = '/Applications/LoanUseCases/PrintLoanRecord.aspx';
            var id = 'id=' + hdnSelectedLoanId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintLoanRecord', param, 'Print Loan Record');
        };

        // MESSAGES
        var writeOffLoan = function () {
            Ext.MessageBox.show({
                title: 'Write off Loan',
                msg: 'Are you sure you want to write off the selected loan?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.QUESTION,
                fn: function (btn, text) {
                    if (btn == 'yes') {
                            onBtnWriteOffClick();
                    }
                    else 
                    {
                            
                    }
                }
            });
        }

        var writeOffLoanSuccessful = function () {
            Ext.MessageBox.show({
                title: 'Write off Loan Successful',
                msg: 'The selected loan is successfully written-off.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var setLoanStatus = function () {
            Ext.MessageBox.show({
                title: 'Set Loan Status',
                msg: 'Are you sure to set the status of the selected loan record to \'Under Litigation\'?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.QUESTION,
                fn: function (btn, text) {
                    if (btn == 'yes') {
                        onBtnUnderLitigationClick();
                    }
                    else {

                    }
                }
            });
        }

        var setLoanStatusSuccessful = function () {
            Ext.MessageBox.show({
                title: 'Set Loan Status Successful',
                msg: 'The status of the selected loan is successfully changed to \'Under Litigation\'.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var onRowSelected = function () {
            enableDisableAmortItemsButtons();

        }

        var enableDisableAmortItemsButtons = function () {
            var selectedRows = rsmAmortSchedGridPanel.getSelections();
            var data = selectedRows[0].json;
            if (data.BilledIndicator == 'Yes') {
                btnEditAmortSched.disable();
                btnDeleteAmortSched.disable();
            } else {
                btnEditAmortSched.enable();
                btnDeleteAmortSched.enable();
            }
        }

        var onRowDeselected = function () {
            btnEditAmortSched.disable();
            btnDeleteAmortSched.disable();
        }

        var btnEditAmortization = function () {
            var selectedRows = rsmAmortSchedGridPanel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var selectedId = selectedRows[0].id;
                X.btnOpen(selectedId);
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        function printdiv(printpage) {
            //LoanAgreementToolBar.hide();
            window.print();
            //LoanAgreementToolBar.show();
        }

        var reload = function () {
            grdAmortSchedule.reload();
        }

        function printSelection(node) {

            var content = node.innerHTML
            var pwin = window.open('', 'print_content', 'width=100,height=100');

            pwin.document.open();
            pwin.document.write('<html><body onload="window.print()">' + content + '</body></html>');
            pwin.document.close();

            //setTimeout(function () { pwin.close(); }, 1000);
        }

        var printContent = function (iframe, divToPrint) {
            var oIframe = document.getElementById(iframe);
            var oContent = document.getElementById(divToPrint).innerHTML;
            var oDoc = (oIframe.contentWindow || oIframe.contentDocument);
            if (oDoc.document) oDoc = oDoc.document;

            oDoc.open();
            var header = '';
            if (document.getElementsByTagName != null) {
                var headTags = document.getElementsByTagName("head");
                if (headTags.length > 0)
                    header = headTags[0].innerHTML;
            }

            var headstr = "<html><head>" + header + "</head><body onload='this.focus(); this.print();'>";
            var footstr = "</body></html>";
            oDoc.write(headstr + oContent + footstr);
            oDoc.close();
        }

        var onBtnPrintLoanAgreement = function () {
            var url = '/Applications/LoanUseCases/PrintLoanAgreement.aspx';
            var id = 'id=' + hdnSelectedLoanId.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintLoanAgreement', param, 'Print Loan Agreement');
        }
//        var printContent = function (iframe, divToPrint) {
//            var oIframe = document.getElementById(iframe);
//            var oContent = document.getElementById(divToPrint).innerHTML;
//            var oDoc = (oIframe.contentWindow || oIframe.contentDocument);
//            if (oDoc.document) oDoc = oDoc.document;

//            oDoc.open();
//            oDoc.write(headstr + oContent + footstr);

//            var headstr = "<html><he" + "ad>";
//            if (document.getElementsByTagName != null) {
//                var headTags = document.getElementsByTagName("head");
//                if (headTags.length > 0) headstr += headTags[0].innerHTML;
//            }
//            headstr += "</he" + "ad><body onload='this.focus(); this.print();'>";

//            var footstr = "</body></html>";
//            //document.body.innerHTML = headstr + oContent + footstr;
//            oDoc.close();
//        };
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        
        .PrintableDiv 
        {
            font-size: small;
        }
         @media screen
        {
            .agreementToolbar
            {
                display: block;
            }
        }
        @media print 
        {
            .agreementToolbar
            {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
        <ext:FormPanel ID="PageFormPanel" runat="server" ButtonAlign="Right" Padding="0" MonitorValid="true" AnchorHorizontal="100%"
            AnchorVertical="100%" BodyStyle="background-color:transparent" Layout="FormLayout" Border="false">
            <Defaults>
                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button ID="btnPrintRecord" runat="server" Text="Print Record" Icon="Printer">
                            <Listeners>
                                <Click Handler="onBtnPrintLoanRecord();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnPrintLoanAgreement" runat="server" Text="Print Agreement" Icon="Printer">
                            <Listeners>
                                <Click Handler="onBtnPrintLoanAgreement();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSeparator ID="btnWriteOffSeparator" runat="server" />
                        <ext:Button ID="btnWriteOff" runat="server" Text="Write Off" Icon="NoteEdit">
                            <Listeners>
                                <Click Handler="onBtnWriteOffClick();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSeparator ID="btnUnderLitigationSeparator" runat="server" />
                        <ext:Button ID="btnUnderLitigation" runat="server" Text="Under Litigation" Icon="Exclamation">
                            <Listeners>
                                <Click Handler="onBtnUnderLitigationClick();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSeparator ID="btnPrintCertificateSeparator" runat="server" />
                        <ext:Button ID="btnPrintCertificate" runat="server" Text="Print Certificate" Icon="Printer">
                            <Listeners>
                                <Click Handler="onBtnPrintCertificateClick();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarFill runat="server"/>
                        <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Items>
                <ext:TabPanel runat="server" AutoHeight="true" Border="false">
                    <Items>
                    <%--/******************* Account Owner's Basic Information ********************/--%>
                    <ext:Panel ID="pnlAccountOwnerInformation" runat="server" Title="Account Owner's Basic Information" Padding="5" AutoHeight="true" Border="false" AutoScroll="true">
                        <Items>
                            <ext:Panel runat="server"  Layout="ColumnLayout" Title="Owner Basic Information" Padding="5">
                                <Items>
                                    <ext:Panel ID="ImagePanel" runat="server" Width="250" Height="250" Border="false">
                                        <Items>
                                            <ext:Image ID="imgPersonPicture" ImageUrl="../../Resources/images/noimage.jpg" runat="server" Width="200" Height="200"></ext:Image>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="Panel2" runat="server" Layout="FormLayout" AnchorHorizontal="100%" ColumnWidth=".6" Border="false" LabelWidth="200">
                                        <Items>
                                            <ext:Hidden ID="hdnSelectedLoanId" runat="server"></ext:Hidden>
                                            <ext:TextField ID="txtAccountOwner" runat="server" FieldLabel="Account Owner" ReadOnly="True" Width="450" />
                                            <ext:TextField ID="txtDistrict" runat="server" FieldLabel="District" ReadOnly="True" Width="450" />
                                            <ext:TextField ID="txtStationNumber" runat="server" FieldLabel="Station Number" ReadOnly="True" Width="450" />
                                            <ext:TextArea ID="txtAddress" runat="server" FieldLabel="Address" ReadOnly="True" Width="450" />
                                            <ext:CompositeField ID="CompositeField1" runat="server" FieldLabel="Cellphone Number">
                                                <Items>
                                                    <ext:TextField ID="txtCellNumCountryCode" runat="server" ReadOnly="True" Width="90" />
                                                    <ext:DisplayField ID="DisplayField1" runat="server" Text="-" />
                                                    <ext:TextField ID="txtCellNumAreaCode" runat="server" ReadOnly="True" Width="90" />
                                                    <ext:DisplayField ID="DisplayField2" runat="server" Text="-" />
                                                    <ext:TextField ID="txtCellNumPhoneNumber" runat="server" ReadOnly="True" Width="240" />
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:CompositeField ID="CompositeField2" runat="server" FieldLabel="Primary Telephone Number" AnchorHorizontal="97%">
                                                <Items>
                                                    <ext:TextField ID="txtPrimTelCountyCode" runat="server" ReadOnly="True" Width="90" />
                                                    <ext:DisplayField ID="DisplayField3" runat="server" Text="-" />
                                                    <ext:TextField ID="txtPrimTelAreaCode" runat="server" ReadOnly="True" Width="90" />
                                                    <ext:DisplayField ID="DisplayField4" runat="server" Text="-" />
                                                    <ext:TextField ID="txtPrimTelPhoneNumber" runat="server" ReadOnly="True" Width="240" />
                                                </Items>
                                            </ext:CompositeField>
                                            <ext:TextField ID="txtEmailAddress" runat="server" FieldLabel="Primary Email Address" ReadOnly="True" Width="450"/>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>

                    <%--/************************** Co-owner/Guarantors ***************************/--%>
                    <ext:Panel ID="pnlCoownerOrGuarantor" runat="server" Title="Co-owner/Guarantors" Padding="0" Border="false">
                        <Items>
                            <ext:GridPanel ID="gridGuarantor" runat="server" Layout="FitLayout" AutoHeight="true" Border="false">
                                <View>
                                    <ext:GridView EmptyText="No co-owner/guarantor to display..." />
                                </View>
                                <Store>
                                    <ext:Store ID="strCoOwnerGuarantor" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="Name">
                                                <Fields>
                                                    <ext:RecordField Name="FinancialAccountRole" />
                                                    <ext:RecordField Name="Name" />
                                                    <ext:RecordField Name="CellphoneNumber" />
                                                    <ext:RecordField Name="TelephoneNumber" />
                                                    <ext:RecordField Name="PrimaryHomeAddress" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="Financial Account Role" Width="200"/>
                                        <ext:Column Header="Name" Width="225"/>
                                        <ext:Column Header="Cellphone Number" Width="150"/>
                                        <ext:Column Header="Telephone Number" Width="150"/>
                                        <ext:Column Header="Primary Home Address" Width="390"/>
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>

                    <%--/************************** Loan Account Details **************************/--%>
                    <ext:Panel ID="pnlLoanDetails" runat="server" Title="Loan Account Details" Padding="10" Border="false"
                         Layout="Form" LabelWidth="200" AutoScroll="true" Height="525">
                        <Items>
                            <ext:TextField ID="txtLoanID" runat="server" FieldLabel="Loan ID" ReadOnly="True" Width="400" Hidden="true"/>
                            <ext:TextField ID="txtAgreementID" runat="server" FieldLabel="Agreement ID" ReadOnly="True" Width="400" Hidden="true"/>
                            <ext:TextField ID="txtLoanReleaseDate" runat="server" FieldLabel="Loan Release Date" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtLoanTerm" runat="server" FieldLabel="Loan Term" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtMaturityDate" runat="server" FieldLabel="Maturity Date" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtLoanStatus" runat="server" FieldLabel="Loan Status" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtStatusComment" runat="server" FieldLabel="Status Comment" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtLoanApplicationID" runat="server" FieldLabel="Loan Application ID" ReadOnly="True" Width="400" Hidden="true"/>
                            <ext:TextField ID="txtLoanProductName" runat="server" FieldLabel="Loan Product Name" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtPaymentMode" runat="server" FieldLabel="Payment Mode" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtInterestComputationMode" runat="server" FieldLabel="Interest Computation Mode" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtInterest" runat="server" FieldLabel="Interest" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtPastDueInterest" runat="server" FieldLabel="Past Due Interest" ReadOnly="True" Width="400" Hidden="true"/>
                            <ext:TextField ID="txtLoanAmount" runat="server" FieldLabel="Loan Amount" ReadOnly="True" Width="400">
                                <Listeners>
                                    <Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtLoanBalance" runat="server" FieldLabel="Loan Balance" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtAddOnInterest" runat="server" FieldLabel="Add On Interest" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtTotalLoan" runat="server" FieldLabel="Total Loan" ReadOnly="True" Width="400">
                                <Listeners>
                                    <BeforeShow Handler="this.setValue(Ext.util.Format.number(this.getValue.replace(/[]/g, ''), '0,000.00'));" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtMethodOfChargingInterest" runat="server" FieldLabel="Method of Charging Interest" ReadOnly="True" Width="400" />
                            <ext:TextField ID="txtAmortization" runat="server" FieldLabel="Label" ReadOnly="True" Width="400"/>
                        </Items>
                    </ext:Panel>

                    <%--/***************************** Loan Agreement *****************************/--%>
                    


                    <%--/************************** Amortization Schedule *************************/--%>
                    <ext:Panel runat="server" Title="Amortization Schedule" AutoScroll="true" Height="536" Border="false">
                        <TopBar>
                            <ext:Toolbar ID="amortSchedToolBar" runat="server" Hidden="true">
                                <Items>
                                    <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y"></ext:Hidden>
                                    <ext:Hidden ID="hdnSelectedAmortSchedItemId" runat="server"></ext:Hidden> 
                                    <ext:Button ID="btnAddAmortSched" runat="server" Text="Add" Icon="Add" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnAddAmortizationSchedItem_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                    <ext:Button ID="btnEditAmortSched" runat="server" Text="Edit" Icon="Disk" Disabled="true" Hidden="true">
                                        <Listeners>
                                            <Click Handler="btnEditAmortization();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                    <ext:Button ID="btnDeleteAmortSched" runat="server" Text="Delete" Icon="Delete" Disabled="true" Hidden="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDeleteAmortizationSchedItem_Click" Success="reload">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected amortization item?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Items>
                            <ext:Panel ID="Panel1" runat="server" AutoScroll="true" Border="false">
                                <Items>
                                    <ext:GridPanel ID="grdAmortSchedule" runat="server" AutoHeight="true" Border="false" Layout="FitLayout">
                                        <View>
                                            <ext:GridView EmptyText="No schedule to display..." />
                                        </View>
                                        <Store>
                                            <ext:Store runat="server" ID="strAmortSched" OnRefreshData="RefreshDataAmortSched">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="Id">
                                                        <Fields>
                                                            <ext:RecordField Name="Id" />
                                                            <ext:RecordField Name="ScheduledPaymentDate" />
                                                            <ext:RecordField Name="PrincipalPayment" />
                                                            <ext:RecordField Name="InterestPayment" />
                                                            <ext:RecordField Name="TotalPayment" />
                                                            <ext:RecordField Name="PrincipalBalance" />
                                                            <ext:RecordField Name="TotalLoanBalance" />
                                                            <ext:RecordField Name="BilledIndicator" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column Header="Scheduled Payment Date" DataIndex="ScheduledPaymentDate" Width="170">
                                                    <Renderer Handler="return Ext.util.Format.date(value, Hidden1.value);" />
                                                </ext:Column>
                                                <ext:NumberColumn Header="Principal Payment" DataIndex="PrincipalPayment" Width="160" Format=",000.00"/>
                                                <ext:NumberColumn Header="Interest Payment" DataIndex="InterestPayment" Width="160" Format=",000.00"/>
                                                <ext:NumberColumn Header="Total Payment" DataIndex="TotalPayment" Width="160" Format=",000.00"/>
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Width="160" Format=",000.00"/>
                                                <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Width="160" Format=",000.00" Hidden="true"/>
                                                <ext:Column Header="Billed Indicator" Hidden="true" DataIndex="BilledIndicator" Width="160" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                        <ext:RowSelectionModel ID="rsmAmortSchedGridPanel" runat="server" SingleSelect="true">
                                            <Listeners>
                                                <RowSelect Fn="onRowSelected" />
                                                <RowDeselect Fn="onRowDeselected" />
                                            </Listeners>
                                        </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>

                    <%--/***************************** Payment History ****************************/--%>
                    <ext:Panel runat="server" AutoScroll="true" Layout="Fit" Title="Loan History" Height="536" Border="false">
                        <Items>
                            <ext:GridPanel ID="gridPaymentHistory" runat="server" Layout="Fit" AutoHeight="true" Border="false">
                                <Store>
                                    <ext:Store ID="strPaymentHistory" runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="Date" />
                                                    <ext:RecordField Name="Disbursement" />
                                                    <ext:RecordField Name="Interest" />
                                                    <ext:RecordField Name="Payment"></ext:RecordField>
                                                    <ext:RecordField Name="WaiveOrRebate"></ext:RecordField>
                                                    <ext:RecordField Name="Remarks"></ext:RecordField>
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel runat="server">
                                    <Columns>
                                        <ext:Column Header="Date" Width="150" />
                                        <ext:NumberColumn Header="Disbursement" Width="140" Format=",000.00"/>
                                        <ext:NumberColumn Header="Interest" Width="140" Format=",000.00"/>
                                        <ext:NumberColumn Header="Payment" Width="140"  Format=",000.00"/>
                                        <ext:NumberColumn Header="WaiveOrRebate" Width="140" Format=",000.00" />
                                        <ext:Column Header="Remarks" Width="180"></ext:Column>
                                    </Columns>
                                </ColumnModel>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
            <%--<BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>--%>
            <%--<Listeners>
                <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
            </Listeners>--%>
        </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="wndAmortizationSchedItem" runat="server" Width="400" Height="228" Layout="FormLayout" 
            Title="Amortization Item" Modal="true" Closable="false" Collapsible="false" Resizable="false" Draggable="false" Hidden="true">
            <Items>
                <ext:FormPanel ID="formPnlWndAmortSched" runat="server" Padding="5" LabelWidth="180" MonitorValid="true" MonitorPoll="500">
                    <Items>
                        <ext:DateField ID="dtScheduledPaymentDate" FieldLabel="Scheduled Payment Date" runat="server" AnchorHorizontal="95%"
                                Editable="false" AllowBlank="false"/>
                        <ext:TextField ID="txtPrincipalPayment" FieldLabel="Principal Payment" runat="server" AnchorHorizontal="95%" AllowBlank="false">
                            <Listeners>
                                <Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField ID="txtInterestPayment" FieldLabel="Interest Payment" runat="server" AnchorHorizontal="95%" AllowBlank="false">
                            <Listeners>
                                <Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField ID="txtPrincipalBalance" FieldLabel="Principal Balance" runat="server" AnchorHorizontal="95%" AllowBlank="false">
                            <Listeners>
                                <Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />
                            </Listeners>
                        </ext:TextField>
                        <ext:TextField ID="txtTotalLoanBalance" FieldLabel="Total Loan Balance" runat="server" AnchorHorizontal="95%" AllowBlank="false">
                            <Listeners>
                                <Change Handler="this.setValue(Ext.util.Format.number(newValue.replace(/[]/g, ''), '0,000.00'));" />
                            </Listeners>
                        </ext:TextField>
                    </Items>
                </ext:FormPanel>
            </Items>
            <BottomBar>
                <ext:StatusBar runat="server">
                </ext:StatusBar>
            </BottomBar>
            <Buttons>
                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="btnSaveAmortizationSchedItem_Click" Before="return #{formPnlWndAmortSched}.getForm().isValid();" Success="reload">
                            <EventMask Msg="Saving.." ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                    <DirectEvents>
                        <Click OnEvent="btnCancelAmortizationSchedItem_Click">
                            <EventMask Msg="Loading.." ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:Window>
        <iframe id='ifrmPrint' src='#' style="width: 0px; height: 0px; display:none;"></iframe>
    </form>
</body>
</html>
