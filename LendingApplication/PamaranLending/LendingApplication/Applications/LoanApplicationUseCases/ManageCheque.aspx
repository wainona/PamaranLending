<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCheque.aspx.cs" Inherits="LendingApplication.Applications.LoanApplicationUseCases.ManageCheque" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Cheque</title>
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
            window.proxy.init(['onpickcustomer', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var saveSuccessful = function () {
            var data = {};
            data.BankId = hdnBankID.getValue();
            amount = txtAmount.getValue();
            data.Amount = amount.replace(',', '');
            data.ChequeNumber = txtCheckNumber.getValue();
            data.TransactionDate = dtTransactionDate.getValue();
            data.Remarks = txtaCheckRemarks.getValue();
            data.ChequeDate = dtCheckDate.getValue();

            window.proxy.sendToAll(data, 'addcheque');
            window.proxy.requestClose();
        }

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                // do your stuff here
                hdnCustomerID.setValue(msg.data.CustomerID);
                txtReceivedFrom.setValue(msg.data.Names);
                X.FillDistrictAndStation(msg.data.CustomerID);

            }
            else if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            }
        };

        var validateCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                    }
                }
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
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden runat="server" ID="hdnCustomerId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnPaymentId"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
        <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500" BodyStyle="background-color:transparent" 
                    Layout="FormLayout" LabelWidth="180">
            <Defaults>
                <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
            <TopBar>
              <ext:ToolBar ID="ToolBar1" runat="server">
                  <Items>
                      <ext:Button ID="btnSave" runat="server" Text="OK" Icon="Disk">
                        <%--<DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="saveSuccessful();">
                                <EventMask Msg="Adding to Database..." ShowMask="true" />
                            </Click>
                          </DirectEvents>--%>
                          <Listeners>
                            <Click Handler="saveSuccessful();" />
                          </Listeners>
                      </ext:Button>
                      <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                          <Listeners>
                              <Click Handler="onBtnClose();" />
                          </Listeners>
                      </ext:Button>
                  </Items>
              </ext:ToolBar>
            </TopBar>
            <Items>
                <%--Transaction Date--%>
                <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Width="400" Editable="false" AllowBlank="false"/><%--Transaction Date--%>
                <%--Amount--%>
                <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="400" AllowBlank="false" MaskRe="[0-9\.\,]">
                    <Listeners>
                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                    </Listeners>
                </ext:TextField>
                <%--Payment Method--%>
                <ext:TextField ID="txtPaymentMethod" runat="server" FieldLabel="Check Payment Method" Width="400" ReadOnly="true" AllowBlank="false" Editable="false" >
                </ext:TextField>
                <%--BankPickList--%>
                <ext:CompositeField ID="CompositeField2" runat="server">
                    <Items>
                        <ext:TextField ID="txtBank" runat="server" ReadOnly="true" Width="400" FieldLabel="Bank" AllowBlank="false"/>
                        <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                        <Listeners>
                                <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:CompositeField>
                <ext:Hidden ID="hdnBankID" runat="server"/>
                <%--Cheque Number--%>
                <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="400" AllowBlank="false">
                    <Listeners>
                        <Change Handler="validateCheckNumber();" />
                    </Listeners>
                </ext:TextField>
                <%--Cheque Status--%>
                <ext:TextField runat="server" ID="txtChequeStatus" FieldLabel="Check Status" Width="400" ReadOnly="true" Hidden="true" AllowBlank="false"></ext:TextField>
                <%--Cheque Remarks--%>
                <ext:TextArea ID="txtaCheckRemarks" runat="server" FieldLabel="Remarks" Width="400" AllowBlank="true" Hidden="true"/>
                <%--Cheque Date--%>
                <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="400" Editable="false" AllowBlank="false"/>
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
    </form>
</body>
</html>
