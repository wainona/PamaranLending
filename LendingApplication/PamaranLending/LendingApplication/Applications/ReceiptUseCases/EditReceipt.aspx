<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditReceipt.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.EditReceipt" %>

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
            window.proxy.init(['onpickbank', 'cancelreceipt']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var modifySuccess = function () {
            showAlert('Modify Successful', 'Receipt record was successfully modified.', function () {
                window.proxy.sendToAll('updatereceipt', 'updatereceipt');
                window.proxy.requestClose();
            });
        }

        var btnEdit_Click = function () {
            btnEdit.hide();
            btnEditSeparator.hide();
            btnCancelRecord.hide();
            btnSave.show();
            btnSaveSeparator.show();
            btnCancel.show();
            pnlReceipt.enable();
        }

        var btnCancel_Click = function () {
            btnSave.hide();
            btnSaveSeparator.hide();
            btnCancel.hide();
            btnEdit.show();
            btnEditSeparator.show();
            btnCancelRecord.show();
            pnlReceipt.disable();
        }

        //Click Cancel Record
        var cancelReceipt = function () {
            showConfirm('Cancel Receipt', 'Are you sure you want to cancel the receipt record?', function (btn, text) {
                if (btn.toLocaleLowerCase() == 'yes') {
                    var selectedId = hdnSelectedReceiptID.getValue();
                    var url = '/Applications/ReceiptUseCases/CancelReceipt.aspx';
                    var id = 'id=' + selectedId;
                    var param = url + "?" + id;
                    window.proxy.requestNewTab('CancelReceipt', param, 'Cancel Receipt');
                } else {

                }
            });
            //return false;
        }

        //FOR PICK LISTS
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            } else if (msg.tag == 'cancelreceipt') {
                window.proxy.requestClose();
            }
        };

        var cmbPaymentMethodChanged = function () {
            if (cmbPaymentMethod.getText() == 'Personal Check' || cmbPaymentMethod.getText() == 'Pay Check') {
                fsCheckField.show();
                txtCheckNumber.allowBlank = false;
                txtBank.allowBlank = false;
                dtCheckDate.allowBlank = false;
                cmbCheckStatus.allowBlank = false;
            } else {
                cmbCheckStatus.allowBlank = true;
                txtCheckNumber.allowBlank = true;
                txtBank.allowBlank = true;
                dtCheckDate.allowBlank = true;
                hdnBankID.clear();
                txtBank.clear();
                txtCheckNumber.clear();
                dtCheckDate.clear();
                txtCheckRemarks.clear();
                fsCheckField.hide();
            }
            markIfRequired(txtCheckNumber);
            markIfRequired(txtBank);
            markIfRequired(dtCheckDate);
            markIfRequired(cmbCheckStatus);
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
    <ext:FormPanel ID="PageFormPanel" runat="server" BodyStyle="background-color:transparent" Layout="Fit" Border="false" MonitorValid="true" MonitorPoll="500" LabelWidth="130">
        <Defaults>
            <ext:Parameter Name="MsgTarget" Value="side" />
        </Defaults>
        <TopBar>
          <ext:ToolBar runat="server">
              <Items>
                  <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="DatabaseEdit" Disabled="true">
                      <Listeners>
                          <Click Handler="btnEdit_Click();" />
                      </Listeners>
                  </ext:Button>
                  <ext:ToolbarSeparator ID="btnEditSeparator" runat="server" />
                  <ext:Button ID="btnCancelRecord" runat="server" Text="Cancel Receipt" Icon="DatabaseDelete" Disabled="true">
                      <Listeners>
                          <Click Handler="cancelReceipt();" />
                      </Listeners>
                  </ext:Button>
                  <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Hidden="true">
                      <DirectEvents>
                          <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="modifySuccess();">
                            <EventMask Msg="Saving.." ShowMask="true" />
                          </Click>
                      </DirectEvents>
                  </ext:Button>
                  <ext:ToolbarSeparator ID="btnSaveSeparator" runat="server" Hidden="true"/>
                  <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cross" Hidden="true">
                      <Listeners>
                          <Click Handler="btnCancel_Click();" />
                      </Listeners>
                  </ext:Button>
                  <ext:ToolbarFill />
                  <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                    <Listeners>
                        <Click Handler="onBtnClose();" />
                    </Listeners>
                  </ext:Button>
                  
              </Items>
          </ext:ToolBar>
        </TopBar>
        <Items>
            <ext:Panel runat="server" ID="pnlReceipt" Height="450" Cls="ext-hide-mask" Padding="5" Border="false" Disabled="true">
                <Items>
                    <ext:Hidden ID="hdnSelectedReceiptID" runat="server" />
                    <ext:Hidden ID="hdnCustomerID" runat="server" />
                    <%----------------RECEIVED RECEIPT FIELD SET-----------------%>
                    <ext:FieldSet ID="fsReceivedReceipt" runat="server" Title="Received Receipt">
                        <Items>
                            <ext:CompositeField runat="server" Width="600">
                                <Items>
                                    <ext:TextField ID="txtReceivedFrom" runat="server" FieldLabel="Received From" ReadOnly="true" Width="465"/>
                                    <ext:Button ID="btnCustomerBrowse" runat="server" Text="Browse" Disabled="true">
                                        <Listeners>
                                            <Click Handler="window.proxy.requestNewTab('CustomerPicker', '/Applications/ReceiptUseCases/CustomerPickList.aspx?mode=single', 'Customer List');" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:CompositeField>
                            <ext:TextField ID="txtDistrictStation" runat="server" FieldLabel="District & Station" Width="600" ReadOnly="true"/>
                            <ext:DateField ID="dtTransactionDate" runat="server" FieldLabel="Transaction Date" Width="600" Editable="false" AllowBlank="false"/>
                            <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Payment Method" ValueField="Id" DisplayField="Name" Width="600" Editable="false" AllowBlank="false" ReadOnly="false">
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
                                    <Select Handler="cmbPaymentMethodChanged();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:TextField ID="txtAmount" runat="server" FieldLabel="Amount (Php)" Width="600" AllowBlank="false" MaskRe="[0-9\.\,]">
                                <Listeners>
                                    <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtReceivedBy" DataIndex="" runat="server" FieldLabel="Received By" Width="600" ReadOnly="true"/>
                        </Items>  
                    </ext:FieldSet>
                    <%---------------------CHECK FIELD SET-----------------------%>
                    <ext:FieldSet ID="fsCheckField" runat="server" Title="Check" Hidden="true">
                        <Items>
                            <ext:CompositeField ID="cfCheckFieldSet" runat="server">
                            <Items>
                                <ext:TextField ID="txtBank" runat="server" Width="465" FieldLabel="Bank" AllowBlank="true"/>
                                <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                                    <Listeners>
                                        <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                            </ext:CompositeField>
                            <ext:Hidden ID="hdnBankID" runat="server"/>
                            <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="600" Disabled="false">
                                <Listeners>
                                    <Change Handler="validateCheckNumber();" />
                                </Listeners>
                            </ext:TextField>
                            <ext:ComboBox ID="cmbCheckStatus" runat="server" FieldLabel="Status" Width="600" ValueField="Id" 
                            DisplayField="Name" Editable="false" AllowBlank="true" ReadOnly="true">
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
                            <ext:TextField ID="txtCheckRemarks" runat="server" FieldLabel="Remarks" Width="600"/>
                            <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Editable="false" Width="600"/>
                        </Items>
                    </ext:FieldSet>
                    <%-----------------RECEIPT STATUS FIELD SET------------------%>
                    <ext:FieldSet ID="fsReceiptStatus" runat="server" Title="Receipt Status">
                        <Items>
                            <ext:TextField ID="txtStatus" runat="server" FieldLabel="Status" Width="600" ReadOnly="true" />
                            <ext:TextField ID="txtRemarks" runat="server" FieldLabel="Remarks" Width="600" ReadOnly="false" />
                        </Items>
                    </ext:FieldSet>
                </Items>
            </ext:Panel>
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
