<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveLoanApplication.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.ApproveLoanApplication" %>

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
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['printpromisory', 'closedpromisory', 'forapproval', 'spadetailsaved']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'printpromisory' || msg.tag == 'closedpromisory') {
                window.proxy.requestClose();
            }
            else if (msg.tag == 'forapproval') {
                hdnLoanReleaseDate.setValue(msg.data.LoanReleaseDate);
                hdnPaymentStartDate.setValue(msg.data.PaymentStartDate);
            } else if (msg.tag == 'spadetailsaved') {
                hdnLender.setValue(msg.data.Lender.Signature);
                hdnLenderName.setValue(msg.data.Lender.PersonName);

                hdnBorrower.setValue(msg.data.Borrower.Signature);

                hdnWitness1.setValue(msg.data.Witness1.Signature);
                hdnWitness1Name.setValue(msg.data.Witness1.PersonName);

                hdnWitness2.setValue(msg.data.Witness2.Signature);
                hdnWitness2Name.setValue(msg.data.Witness2.PersonName);

                hdnWitness3.setValue(msg.data.Witness3.Signature);
                hdnWitness3Name.setValue(msg.data.Witness3.PersonName);

                hdnWitness4.setValue(msg.data.Witness4.Signature);
                hdnWitness4Name.setValue(msg.data.Witness4.PersonName);

                btnOpenDocument.enable();
                if (msg.data.IsComplete == false) {
                    txtSignSPA.setValue('Signed');
                    //txtSignSPA.setValue('Incomplete');
                    btnApprove.disable();
                }
                else {
                    txtSignSPA.setValue('Signed');
                }
            }
        }

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully approved loan...', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        }

        var changeStatusSuccessful = function () {
            showAlert('Status', 'Successfully updated the status of the loan application record.', function () {
                window.proxy.sendToAll('approvedloanapplication', 'approvedloanapplication');
                promissoryNote();
            });
        }

        var enableDisableButtons = function () {
            if (nfLoanTerm.getValue() == "0") {
                btnApprove.enable();
                //datPaymentStartDate.readOnly(true);
            } else {
                btnGenerate.disable();
            }
        }

        var promissoryNote = function () {
            X.SetControlNumber({
                success: function (result) {
                    if (result) {
                        //OpenPromisoryNote();
                        var data = {};
                        data.id = hdnSelectedLoanID.getValue();
                        window.proxy.sendToParent(data, 'closeapproved');
                        //window.proxy.requestClose();
                    } else {
                        showAlert('Error!', 'Setting of control number failed. Cannot print promisory note.');
                    }
                }
            });
            //            showConfirm('Confirm', 'Do you want to print a promisory note?', function (btn) {
            //                if (btn == 'yes') {
            //                    X.SetControlNumber({
            //                        success: function (result) {
            //                            if (result) {
            //                                //OpenPromisoryNote();
            //                                var data = {};
            //                                data.id = hdnSelectedLoanID.getValue();
            //                                window.proxy.sendToParent(data, 'closeapproved');
            //                                //window.proxy.requestClose();
            //                            } else {
            //                                showAlert('Error!', 'Setting of control number failed. Cannot print promisory note.');
            //                            }
            //                        }
            //                    });
            //                } else if (btn == 'no') {
            //                    window.proxy.requestClose();
            //                }
            //            });
        }

        var OpenPromisoryNote = function () {
            var url = '/Applications/LoanApplicationUseCases/PrintPromisoryNote.aspx';
            var param = url + '?loanApplicationId=' + hdnSelectedLoanID.getValue();
            window.proxy.requestNewTab('PrintPromissoryNote', param, 'Print Promissory Note');
        }

        var onFormValidated = function (valid) {
            valid = valid && txtSignSPA.getValue() == 'Signed'
            if (valid && (nfLoanTerm.getValue() != '0')) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnApprove.enable();
                btnGenerate.enable();
            }
            else if (valid && (nfLoanTerm.getValue() == '0')) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnApprove.enable();
                btnGenerate.disable();
            } else if (txtSignSPA.getValue() != "Signed") {
                PageFormPanelStatusBar.setStatus({ text: 'Documents are unsigned. ', iconCls: 'icon-accept' });
                btnApprove.disable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.', iconCls: 'icon-exclamation' });
                btnGenerate.disable();
            }
        }

        var adjustPaymentStartDate = function () {
            X.setPaymentDate();
        }

        var generationSuccessful = function () {
            btnApprove.enable();
        }

        //-----------------CHECKING-----------------------//
        var Confirmation = function () {
            X.CheckHonorableLoanAmount({
                success: function (result) {
                    if (result) {
                        showAlert('Error', 'Loan Amount is greater than your maximum honorable amount. Cannot proceed with approval.');
                        return fals;
                    } else {
                        showConfirm('Confirm', 'Are you sure you want to approve this loan application?', function (btn) {
                            if (btn == 'yes')
                                Validate();
                        });
                    }
                }
            });
        }

        var Validate = function () {
            //[a]. check if loan amount <= total of selected outstanding loans to pay-off
            var loanAmount = checkLoanAmount();
            if (loanAmount == false) { //if loan amount is true, proceed
                if (checkLoanTerm()) //if loanterm == true 
                    btnSaveActual.fireEvent('click'); //save
            } else {
                return false;
            }
        }

        //[a]
        var checkLoanAmount = function () {
            var loanAmount = document.getElementById('nfLoanAmount').value;
            X.CheckLoanAmount({
                success: function (result) {
                    if (result) {
                        return checkLoanTerm(); //if result == true, may proceed to next condition checking
                    } else {
                        showAlert('Error!', 'Loan amount should be greater than the total of the outstanding loans to pay off.'); //loan term == false - error msg
                        return false;
                    }
                }
            });
        }

        //[b]
        var checkLoanTerm = function () {
            X.CheckLoanTerm({
                success: function (result) {
                    if (result) {
                        return checkCredit(); //if result == true, may proceed to next condition checking
                    } else {
                        showAlert('Error!', 'The loan term you entered cannot be converted to another whole number base on the payment mode. Please change loan term value.');
                        return false;
                    }
                }
            });
        }

        //[c]
        var checkCredit = function () {
            X.CheckCredit({
                success: function (result) {
                    if (result) { //if true, confirmation message asking the user to proceed saving. --check for next condition
                        showConfirm('Confirm', 'Loan amount is greater than the available credit limit of the borrower. Are you sure to approve the loan application with this amount?', function (btn) {
                            if (btn == 'no') { //dont continue saving
                                return false;
                            } else {
                                if (checkDiminishing()) //proceed to next condition
                                    btnApproveActual.fireEvent('click');
                            }
                        });
                    } else {
                        if (checkDiminishing()) //if result == false, no confirmations needed so proceed to next condition
                            btnApproveActual.fireEvent('click');
                    }
                }
            });
        }

        //[d]
        var checkDiminishing = function () {
            X.CheckDiminishing({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Allowed number of diminishing balance loan per customer is already reached. Are you sure to approve the selected loan application?', function (btn) {
                            if (btn == 'no') { //dont continue saving
                                return false;
                            } else {
                                if (checkStraightLine()) //proceed to next condition
                                    return true;
                            }
                        });
                    } else { //if result == false, no confirmations needed so proceed to next condition
                        if (checkStraightLine())
                            return true;
                        //btnApproveActual.fireEvent('click');
                    }
                }
            });
        }

        //[e]
        var checkStraightLine = function () {
            X.CheckStraightLine({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Allowed number of straight line loan per customer is already reached. Are you sure to approve the selected loan application?', function (btn) {
                            if (btn == 'no') {
                                return false;
                            } else {
                                if (checkCreditLimit())
                                    return true;
                            }
                        });
                    } else {
                        if (checkCreditLimit()) //if result == false, no confirmations needed so proceed to next condition
                            return true;
                        //btnApproveActual.fireEvent('click');
                    }
                }
            });
        }

        //[f]
        var checkCreditLimit = function () {
            X.CheckCreditLimit({
                success: function (result) {
                    if (result) {
                        showConfirm('Confirm', 'Loan Amount is greater than Credit Limit and the allowed numbers of straight line/diminishing balance loan per customer is already reached. Are you sure to approve the selected loan application?', function (btn) {
                            if (btn == 'no') {
                                return false;
                            } else {
                                btnApproveActual.fireEvent('click');
                            }
                        });
                    } else {
                        btnApproveActual.fireEvent('click');
                    }
                }
            });
        }
        //-----------------CHECKING-----------------------//

        var onKeyPress = function () {
            btnApprove.disable();
        }

        var onBtnClose = function () {
            showConfirm('Confirm', 'All uploaded images will be deleted. Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    X.DeleteImages({
                        success: function () {
                            showAlert('Success', 'All associated signatures were successfully deleted.');
                            window.proxy.requestClose();
                        }
                    });
                }
            });
        }

        var onBtnOpenDoc = function () {
            //if txtsign == Signed, retreive images using loanAppId
            //var isSigned = true;
            var text = txtSignSPA.getValue();
            //if (text == 'Unsigned') {
            //    isSigned = false;
            var url = '/Applications/LoanApplicationUseCases/PrintSPA.aspx';
            var param = url + "?ResourceGuid=" + hiddenResourceGuid.getValue();
            param += "&customerPartyRoleId=" + hdnCustomerID.getValue();
            param += "&loanApplicationId=" + hdnSelectedLoanID.getValue();
            param += "&mode=approve";
            param += "&status=" + text;
            window.proxy.requestNewTab('PrintSPA', param, 'Print SPA');
        }
    </script>
    <style type="text/css">
        .x-grid-empty {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <%--Hidden Fields--%>
    <ext:Hidden ID="hdnSelectedLoanID" runat="server" />
    <ext:Hidden ID="hdnCustomerID" runat="server" />
    <ext:Hidden ID="hdnStraightLineCount" runat="server" />
    <ext:Hidden ID="hdnDiminishingCount" runat="server" />
    <ext:Hidden ID="hdnLoanReleaseDate" runat="server"></ext:Hidden>
    <ext:Hidden ID="hdnPaymentStartDate" runat="server"></ext:Hidden>
    <ext:Hidden runat="server" ID="hiddenResourceGuid"></ext:Hidden>

    <%--WITNESS 1--%>
    <ext:Hidden runat="server" ID="hdnWitness1"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness1Name"></ext:Hidden>

    <%--WITNESS 2--%>
    <ext:Hidden runat="server" ID="hdnWitness2"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness2Name"></ext:Hidden>

    <%--WITNESS 3--%>
    <ext:Hidden runat="server" ID="hdnWitness3"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness3Name"></ext:Hidden>

    <%--WITNESS 4--%>
    <ext:Hidden runat="server" ID="hdnWitness4"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness4Name"></ext:Hidden>

    <%--LENDER--%>
    <ext:Hidden runat="server" ID="hdnLender"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnLenderName"></ext:Hidden>

    <%--BORROWER--%>
    <ext:Hidden runat="server" ID="hdnBorrower"></ext:Hidden>

    <%--End of Hidden Fields--%>
    <ext:Viewport ID="pageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel runat="server" ID="RootPanel" Layout="FitLayout" Border="false" MonitorPoll="500"
                MonitorValid="true">
                <TopBar>
                    <ext:Toolbar runat="server" ID="RootToolBar">
                        <Items>
                            <ext:Button ID="btnGenerate" runat="server" Text="Generate Schedule" Icon="Calculator"
                                Disabled="true" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" Success="generationSuccessful();">
                                        <EventMask ShowMask="true" Msg="Generating amortization schedule..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator></ext:ToolbarSeparator>
                            <ext:Button runat="server" ID="btnApprove" Icon="ApplicationAdd" Text="Approve" Disabled="false">
                                <Listeners>
                                    <Click Handler="Confirmation();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnApproveActual" runat="server" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnApprove_Click" Success="changeStatusSuccessful();">
                                        <EventMask ShowMask="true" Msg="Updating loan application status..."/>
                                        <ExtraParams>
                                            <ext:Parameter Name="Schedules" Value="Ext.encode(#{grdPnlAmortizationSchedule}.getRowsValues({selectedOnly : false}))" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelGenerateAmortizationSchedule"
                        Padding="5" Layout="FormLayout" LabelWidth="180">
                        <Items>
                            <ext:Panel runat="server" ID="pnlSignSPA" Title="Sign Document"
                                Padding="5" Layout="FormLayout">
                                <Items>
                                    <ext:CompositeField runat="server" ID="cmpSignDoc" FieldLabel="SPA Document">
                                        <Items>
                                            <ext:TextField runat="server" ID="txtSignSPA" Text="Signed" Width="210" ReadOnly="true">
                                            </ext:TextField>
                                            <ext:Button runat="server" ID="btnOpenDocument" Text="Open Document to Sign" Icon="Printer">
                                                <Listeners>
                                                    <Click Handler="onBtnOpenDoc();"/>
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:CompositeField>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" Title="Generate Application Schedule" Padding="5"><Items>
                            <ext:Hidden ID="hiddenProductId" runat="server" />
                            <ext:Hidden ID="hiddenLoanTermTimeUnitId" runat="server" />
                            <ext:TextField ID="nfLoanAmount" runat="server" FieldLabel="Loan Amount" Number="0"
                                Width="400" AllowBlank="false" MinValue="0" EnableKeyEvents="true" MaskRe="[0-9\.\,]" ReadOnly="true">
                                <Listeners>
                                    <KeyPress Fn="onKeyPress" />
                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                </Listeners>
                                </ext:TextField>
                            <%--<ext:NumberField ID="nfLoanTerm" runat="server" FieldLabel="Loan Term" Number="0"
                                Width="400" AllowBlank="false" DecimalPrecision="0" EnableKeyEvents="true" ReadOnly="true">
                                <%--<Listeners>
                                    <KeyPress Fn="onKeyPress" />
                                </Listeners>
                                </ext:NumberField>--%>
                            <ext:TextField runat="server" ID="nfLoanTerm" FieldLabel="Loan Term" Width="400"
                                AllowBlank="false" ReadOnly="true"></ext:TextField>
                            <ext:DateField ID="datLoanReleaseDate" runat="server" FieldLabel="Loan Release Date"
                                Width="400" Editable="false" AllowBlank="false" Vtype="daterange" EndDateField="datPaymentStartDate">
                                    <Listeners>
                                        <Select Handler="adjustPaymentStartDate();#{btnApprove}.disable();" />
                                    </Listeners>
                                </ext:DateField>
                            <ext:DateField ID="datPaymentStartDate" runat="server" FieldLabel="Payment Start Date"
                                Width="400" Editable="false" ReadOnly="true" AllowBlank="false" Vtype="daterange" StartDateField="datLoanReleaseDate">
                                <%--<Listeners>
                                        <Select Handler="#{btnApprove}.disable();" />
                                    </Listeners>--%>
                                </ext:DateField>
                            </Items></ext:Panel>
                            <ext:Panel runat="server" LabelWidth="420" Border="false" AutoScroll="true">
                                <Items>
                                    <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Height="100" MinHeight="315"
                                        Title="Amortization Schedule" AutoScroll="true">
                                        <View>
                                            <ext:GridView EmptyText="No amortization schedule to display" DeferEmptyText="false">
                                            </ext:GridView>
                                        </View>
                                        <LoadMask ShowMask="true" Msg="Loading amortization schedule..."/>
                                        <Store>
                                            <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                                <Listeners>
                                                    <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                                </Listeners>
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
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
                                                    Width="130" Sortable="false" >
                                                    <Editor>
                                                        <ext:DateField runat="server"></ext:DateField>
                                                    </Editor>    
                                                </ext:DateColumn>
                                                <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="130"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total" DataIndex="TotalPayment" Wrap="true" Width="130"
                                                    Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" />
                                                <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="130" Sortable="false" Format=",000.00" Hidden="true"/>
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
                    <AfterRender Handler="enableDisableButtons();" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
