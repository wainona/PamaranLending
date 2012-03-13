<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCheque.aspx.cs" Inherits="LendingApplication.Applications.ChequeUseCases.EditCheque" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Edit Cheque</title>
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
            window.proxy.init(['onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var modifySuccess = function () {
            showAlert('Status', 'Cheque record was successfully modified.', function () {
                window.proxy.sendToAll('updatecheque', 'updatecheque');
                //window.proxy.requestClose();
                btnCancel_Click();
            });
        };

        var btnEdit_Click = function () {
            btnEdit.hide();
            //btnDeposit.hide();
            btnBankBrowse.enable();
            var status = hdnStatus.getValue();
            if (status == 'Received')
                pnlCheque.enable();
            pnlChkStatus.enable();
            btnEditSeparator.show();
            btnSave.show();
            btnCancel.show();
        };

        var btnCancel_Click = function () {
            btnEdit.show();
            var status = hdnStatus.getValue();
//            if (status == 'Received' || status =='Bounced')
//                btnDeposit.show();
            btnBankBrowse.disable();
            pnlCheque.disable();
            pnlChkStatus.disable();
            btnEditSeparator.hide();
            btnSave.hide();
            btnCancel.hide();
        };

        //FOR PICK LISTS
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            };
        };

        var updateCheckStatusSuccessful = function () {
            showAlert('Status', 'Successfully updated the check status.', function () {
                window.proxy.sendToAll('checkdeposited', 'checkdeposited');
                btnEdit.disable();
                btnDeposit.disable();
            });
        };

    
        var validateCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                        hdnChequeValid.setValue(1);
                    } else {
                        hdnChequeValid.setValue(0);
                        btnSave.enable();
                    }
                }
            });
        };

        var onFormValidated = function (valid) {
            if ((valid && hdnChequeValid.getValue() == 0) || (valid && hdnChequeValid.getValue() == '')) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid.' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnSave.disable();
            }
        };

        var onBtnPrintCheque = function () {
            var url = '/Applications/ChequeUseCases/PrintCheque.aspx';
            var id = 'id=' + hdnSelectedReceiptID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('PrintCheque', param, 'Print Cheque');
        }
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
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        .ext-hide-mask .ext-el-mask
        {
           opacity: 0.04;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
    <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500"
        BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="180" Border="false">
        <Defaults>
            <ext:Parameter Name="MsgTarget" Value="side" />
        </Defaults>
        <TopBar>
            <ext:ToolBar runat="server">
                <Items>
                     <ext:Hidden ID="hdnStatus" runat="server"></ext:Hidden>
                    <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="DatabaseEdit" Hidden="true"><%-- EDIT --%>
                      <Listeners>
                          <Click Handler="btnEdit_Click();" />
                      </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                      <DirectEvents>
                          <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="modifySuccess();">
                            <EventMask Msg="Saving.." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator ID="btnEditSeparator" runat="server" Hidden="true"/>
                    <%--<ext:Button ID="btnDeposit" runat="server" Text="Deposit" Icon="MoneyAdd" Hidden="true">
                        <DirectEvents>
                            <Click OnEvent="btnDeposit_Click" Success="updateCheckStatusSuccessful">
                                <EventMask ShowMask="true" Msg="Changing check status.." />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator ID="btnDepositSeparator" runat="server" Disabled="true"/>
                    <ext:Button ID="btnClear" runat="server" Text="Clear" Icon="Accept" Hidden="true">
                        <DirectEvents>
                            <Click OnEvent="btnClear_Click" Success="updateCheckStatusSuccessful">
                                <EventMask ShowMask="true" Msg="Changing check status.." />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator ID="btnClearSeparator" runat="server" Disabled="true"/>
                    <ext:Button ID="btnBounced" runat="server" Text="Bounced" Icon="Error" Hidden="true">
                        <DirectEvents>
                            <Click OnEvent="btnBounced_Click" Success="updateCheckStatusSuccessful">
                                <EventMask ShowMask="true" Msg="Changing check status.." />
                            </Click>
                        </DirectEvents>
                    </ext:Button>--%>
                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cross" Hidden="true">
                      <Listeners>
                          <Click Handler="btnCancel_Click();" />
                      </Listeners>
                  </ext:Button>
                    <ext:ToolbarFill />
                    <%--PRINT--%>
                    <ext:ToolbarFill />
                    <ext:Button ID="btnPrintCheque" runat="server" Text="Print Cheque" Icon="Printer" Hidden="true">
                        <Listeners>
                            <Click Handler="onBtnPrintCheque();"/>
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" />
                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:ToolBar>
        </TopBar>
        <Items>
            <ext:Panel ID="pnlCheque" runat="server" Cls="ext-hide-mask" Disabled="true" Padding="5" Border="false">
                <Items>
                    <ext:Hidden ID="hdnSelectedReceiptID" runat="server"></ext:Hidden>
                    <ext:Hidden ID="hdnChequeValid" runat="server"></ext:Hidden>
                    <ext:CompositeField runat="server"><%--Received From--%>
                        <Items>
                            <ext:TextField ID="txtReceivedFrom" runat="server" ReadOnly="true" Width="415" FieldLabel="Received From"/>
                            <ext:Button ID="btnCustomerBrowse" runat="server" Text="Browse" Disabled="true">
                                <Listeners>
                                    <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/Applications/CustomerUseCases/CustomerPickList.aspx?mode=single', 'Customer List');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:Hidden ID="hdnCustomerID" runat="server" /><%--Customer ID--%>
                    <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Width="600" Editable="false" AllowBlank="false"/><%--Transaction Date--%>
                    <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Cheque Payment Method" Width="600" Editable="false" AllowBlank="false"><%--Payment Method--%>
                        <Items>
                            <ext:ListItem Text="Pay Check" />
                            <ext:ListItem Text="Personal Check" />
                        </Items>            
                    </ext:ComboBox>
                    <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="600" AllowBlank="false" MaskRe="[0-9\.\,]">
                        <Listeners>
                            <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                        </Listeners>
                    </ext:TextField><%--Amount--%>
                    <ext:TextField ID="txtReceivedBy" runat="server" FieldLabel="Received By" Width="600" ReadOnly="true"/>
                    <ext:CompositeField ID="cfCheckFieldSet" runat="server">
                        <Items>
                            <ext:TextField ID="txtBank" runat="server" Width="415" FieldLabel="Bank" AllowBlank="false" ReadOnly="true"/>
                            <ext:Button ID="btnBankBrowse" runat="server" Text="Browse" Disabled="true">
                                <Listeners>
                                    <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:Hidden ID="hdnBankID" runat="server"/>
                    <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="600" AllowBlank="false" EnableKeyEvents="true">
                        <Listeners>
                            <KeyUp Handler="validateCheckNumber();" />
                        </Listeners>
                    </ext:TextField>
                    <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="600" Editable="false" AllowBlank="false"/>
                  
                </Items>
            </ext:Panel>
            <ext:Panel runat="server" ID="pnlChkStatus" Cls="ext-hide-mask" Disabled="true" Padding="5" Border="false">
            <Items>
              <ext:ComboBox ID="cmbCheckStatus" runat="server" FieldLabel="Status" Width="600" Editable="false" ValueField="Id" DisplayField="Name" 
                            AllowBlank="false" ReadOnly="true">
                        <Store>
                            <ext:Store ID="strCheckStatus" runat="server">
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
                    </ext:ComboBox>
                    <ext:TextArea ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Width="600"></ext:TextArea>
            </Items>
            </ext:Panel>
        </Items>
        <BottomBar>
            <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
        </BottomBar>
        <Listeners>
            <%--<ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />--%>
            <ClientValidation Handler="onFormValidated(valid);" />
        </Listeners>
    </ext:FormPanel>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
