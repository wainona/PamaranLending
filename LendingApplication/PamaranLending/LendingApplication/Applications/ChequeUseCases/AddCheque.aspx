<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCheque.aspx.cs" Inherits="LendingApplication.Applications.ChequeUseCases.AddCheque" %>

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
            showAlert('Status', 'Cheque record was successfully created.', function () {
                window.proxy.sendToAll('addcheque', 'addcheque');
                window.proxy.requestClose();
            });
        }

        /************************************
        *          FOR PICKLISTS            *
        ************************************/
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
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnSave.disable();
            }
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
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
        <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500" BodyStyle="background-color:transparent" 
                    Layout="FormLayout" LabelWidth="180" Border="false">
            <Defaults>
                <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
            <TopBar>
              <ext:ToolBar runat="server">
                  <Items>
                      <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="saveSuccessful();">
                                <EventMask Msg="Adding to Database..." ShowMask="true" />
                            </Click>
                          </DirectEvents>
                      </ext:Button>
                      <ext:ToolbarSeparator runat="server" />
                      <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                          <Listeners>
                              <Click Handler="onBtnClose();" />
                          </Listeners>
                      </ext:Button>
                  </Items>
              </ext:ToolBar>
            </TopBar>
            <Items>
                <ext:Hidden ID="hdnChequeValid" runat="server" />
                <ext:CompositeField runat="server"><%--Received From--%>
                    <Items>
                        <ext:TextField ID="txtReceivedFrom" runat="server" ReadOnly="true" Width="400" FieldLabel="Received From" AllowBlank="false"/>
                        <ext:Button ID="btnCustomerBrowse" runat="server" Text="Browse">
                            <Listeners>
                                <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/Applications/CustomerUseCases/CustomerPickList.aspx?mode=single', 'Customer List');" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:CompositeField>
                <ext:Hidden ID="hdnCustomerID" runat="server" /><%--Customer ID--%>
                <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Width="400" Editable="false" AllowBlank="false"/><%--Transaction Date--%>
                <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="400" AllowBlank="false" MaskRe="[0-9\.\,]">
                    <Listeners>
                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                    </Listeners>
                </ext:TextField><%--Amount--%>
                <ext:TextField ID="txtReceivedBy" runat="server" FieldLabel="Received By" Width="400" ReadOnly="true" AllowBlank="true"/><%--Received By--%>
                <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Cheque Payment Method" Width="400" Editable="false" AllowBlank="false"><%--Payment Method--%>
                    <Items>
                        <ext:ListItem Text="Pay Check" />
                        <ext:ListItem Text="Personal Check" />
                    </Items>
                    <DirectEvents>
                        <Select OnEvent="cmbPaymentMethod_OnSelect" />
                    </DirectEvents>          
                </ext:ComboBox>
                <ext:CompositeField runat="server">
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
                <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="400" AllowBlank="false" EnableKeyEvents="true">
                    <Listeners>
                        <KeyUp Handler="validateCheckNumber();" />
                        <%--<Change Handler="validateCheckNumber();" />--%>
                    </Listeners>
                </ext:TextField>
                <ext:ComboBox ID="cmbCheckStatus" runat="server" FieldLabel="Status" Width="400" ValueField="Id" ReadOnly="false" DisplayField="Name">
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
                <ext:TextField ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Width="400" AllowBlank="true"/>
                <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="400" Editable="false" AllowBlank="false"/>
            </Items>
            <BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>
            <Listeners>
                <%--ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" /--%>
                <ClientValidation Handler="onFormValidated(valid);" />
            </Listeners>
        </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
