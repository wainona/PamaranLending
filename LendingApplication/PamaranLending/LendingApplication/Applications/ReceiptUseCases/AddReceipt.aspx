<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddReceipt.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.AddReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add Customer</title>
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
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('addreceipt', 'addreceipt');
                window.proxy.requestClose();
            });
        }

        var createSuccess = function () {
            Ext.MessageBox.show({
                title: 'Create Successful',
                msg: 'Receipt record was successfully created.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }

        var modifySuccess = function () {
            Ext.MessageBox.show({
                title: 'Modify Successful',
                msg: 'Receipt record was successfully modified.',
                buttons: Ext.MessageBox.OK,
                icon: Ext.MessageBox.INFO
            });
        }
        
        var cancel = function () {
            Ext.MessageBox.show({
                title: 'Cancel',
                msg: 'Are you sure you want to cancel the selected reciept record?',
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.QUESTION
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
            };
        }

        var cmbPaymentMethodChanged = function () {

//            if (cmbPaymentMethod.getText() == 'Personal Check' || cmbPaymentMethod.getText() == 'Pay Check') {
//                fsCheckField.show();
//                /* Cash Fields ------------------------------*/
//                txt1000Bills.clear();
//                txt500Bills.clear();
//                txt200Bills.clear();
//                txt100Bills.clear();
//                txt50Bills.clear();
//                txt20Bills.clear();
//                txtCoins.clear();
//                txtAmount.clear();
//                /*-------------------------------------------*/
//                fsCashDenomination.hide();
//                txtCheckNumber.allowBlank = false;
//                txtBank.allowBlank = false;
//                dtCheckDate.allowBlank = false;
//                X.cmbPaymentMethod_Changed();
//            } else {
//                txtCheckNumber.allowBlank = true;
//                txtBank.allowBlank = true;
//                dtCheckDate.allowBlank = true;
//                hdnBankID.clear();
//                txtBank.clear();
//                txtCheckNumber.clear();
//                dtCheckDate.clear();
//                txtCheckRemarks.clear();
//                fsCheckField.hide();
//                fsCashDenomination.show();
//            }
//            markIfRequired(txtCheckNumber);
//            markIfRequired(txtBank);
//            markIfRequired(dtCheckDate);
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

        var sumTotalCash = function () {
            var totalAll = 0;

            var total1000 = 1000 * txt1000Bills.getValue();
//            txtAmount.setValue(total1000);
            var total500 = 500 * txt500Bills.getValue();
//            txtAmount.setValue(total500);
            var total200 = 200 * txt200Bills.getValue();
//            txtAmount.setValue(total200);
            var total100 = 100 * txt100Bills.getValue();
//            txtAmount.setValue(total100);
            var total50 = 50 * txt50Bills.getValue();
//            txtAmount.setValue(total50);
            var total20 = 20 * txt20Bills.getValue();
//            txtAmount.setValue(total20);
            var totalCoins = 1 * txtCoins.getValue();
//            txtAmount.setValue(totalCoins);

            var totalAll = (total1000 + total500 + total200 + total100 + total50 + total20 + totalCoins);
            txtAmount.hide();
            txtAmount.setValue(totalAll);
            txtAmount.show();
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
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
        <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500" Title="New Receipt"
            BodyStyle="background-color:transparent" Layout="FormLayout" LabelWidth="130" Border="false">
            <Defaults>
                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
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
                <%-----------------------------------------RECEIVED RECEIPT FIELD SET------------------------------------------%>
                <ext:FieldSet ID="FieldSet1" runat="server" Title="Received Receipt">
                    <Items>
                        <ext:CompositeField runat="server"><%--Received From--%>
                            <Items>
                                <ext:TextField ID="txtReceivedFrom" runat="server" ReadOnly="true" Width="465" FieldLabel="Received From" AllowBlank="false"/>
                                <ext:Button ID="btnCustomerBrowse" runat="server" Text="Browse" Icon="Magnifier">
                                    <Listeners>
                                        <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/Applications/CustomerUseCases/CustomerPickList.aspx?mode=single', 'Customer List');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:Hidden ID="hdnCustomerID" runat="server" /><%--Customer ID--%>
                        <ext:TextField ID="txtDistrictStation" DataIndex="" runat="server" FieldLabel="District & Station" Width="600" ReadOnly="true" AllowBlank="false"/><%--District & Station--%>
                        <ext:DateField ID="dtTransactionDate" DataIndex="" runat="server" FieldLabel="Transaction Date" Width="600" Editable="false" AllowBlank="false"/><%--Transaction Date--%>
                        <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Payment Method" ValueField="Id" DisplayField="Name" 
                            Width="600" Editable="false" AllowBlank="false"><%--Payment Method--%>
                            <Store>
                                <ext:Store ID="strPaymentMethod" runat="server">
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
                            <Listeners>
                                <Select Handler="cmbPaymentMethodChanged();"></Select>
                            </Listeners>
                        </ext:ComboBox>
                        <ext:FieldSet runat="server" ID="fsCashDenomination" Title="Cash Denomination" Width="600">
                            <Items>
                                <ext:TextField ID="txt1000Bills" runat="server" FieldLabel="1000 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txt500Bills" runat="server" FieldLabel="500 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txt200Bills" runat="server" FieldLabel="200 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txt100Bills" runat="server" FieldLabel="100 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txt50Bills" runat="server" FieldLabel="50 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txt20Bills" runat="server" FieldLabel="20 Bills" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                                <ext:TextField ID="txtCoins" runat="server" FieldLabel="Coins" EnableKeyEvents="true" Width="575">
                                    <Listeners>
                                        <KeyUp Handler="sumTotalCash();" />
                                    </Listeners>
                                </ext:TextField>
                            </Items>
                        </ext:FieldSet>
                        <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="600" AllowBlank="false" MaskRe="[0-9\.\,]">
                            <Listeners>
                                <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                <Show Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                            </Listeners>
                        </ext:TextField><%--Amount--%>
                        <ext:Hidden ID="hdnLoggedInPartyRoleId" runat="server" />
                        <ext:TextField ID="txtReceivedBy" DataIndex="" runat="server" FieldLabel="Received By" Width="600" ReadOnly="true" /><%--Received By--%>
                    </Items>
                </ext:FieldSet>
                <%----------------------------------------------CHECK FIELD SET------------------------------------------------%>
                <ext:FieldSet ID="fsCheckField" runat="server" Title="Check" Hidden="true">
                    <Items>
                        <ext:CompositeField runat="server" Width="600">
                            <Items>
                                <ext:TextField ID="txtBank" runat="server" ReadOnly="true" Width="415" FieldLabel="Bank" />
                                <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                                <Listeners>
                                        <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:CompositeField>
                        <ext:Hidden ID="hdnBankID" runat="server"/>
                        <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="600">
                            <Listeners>
                                <Change Handler="validateCheckNumber();" />
                            </Listeners>
                        </ext:TextField>
                        <ext:ComboBox ID="cmbCheckStatus" runat="server" FieldLabel="Status" Width="600" Editable="false" ValueField="Id"
                                DisplayField="Name" ReadOnly="true">
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
                        <ext:TextField ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Width="600" />
                        <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="600" Editable="false" />
                    </Items>
                </ext:FieldSet>
                <%------------------------------------------RECEIPT STATUS FIELD SET-------------------------------------------%>
                <ext:FieldSet runat="server" Title="Receipt Status">
                    <Items>
                        <ext:TextField ID="txtStatus" runat="server" FieldLabel="Status" Width="600" ReadOnly="true" />
                        <ext:TextField ID="txtRemarks" runat="server" FieldLabel="Remarks" Width="600" />
                    </Items>
                </ext:FieldSet>
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
