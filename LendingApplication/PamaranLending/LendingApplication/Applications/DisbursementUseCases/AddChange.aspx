<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChange.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.AddChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Change</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['receiptselected', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
            formatCurrency(txtCashAmount);
            formatCurrency(txtCheckAmount);
            formatCurrency(txtAmountToDisburse);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'receiptselected') {
                txtCustID.setValue(msg.data.PartyRoleId);
                txtReceiptBalance.setValue(msg.data.Balance);
                txtReceiptID.setValue(msg.data.ReceiptID);
                txtCustomerName.setValue(msg.data.Name);
                txtCashAmount.setValue(msg.data.Balance);
                txtAmountToDisburse.setValue(msg.data.Balance);
                formatCurrency(txtReceiptBalance);
                formatCurrency(txtCashAmount);
                formatCurrency(txtAmountToDisburse);
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, msg.data.BankPartyRoleId, {
                    success: function () {
                        totalChequesAmount();
                        caculateTotal();
                    }
                });
            }
        };
        var browseReceipt = function () {
            window.proxy.requestNewTab('SelectReceipt', '/Applications/DisbursementUseCases/ReceiptPickList.aspx', 'Receipt List');
        };
        var saveSuccessful = function () {
            var data = {};
            data.id = hdnParentPaymentId.getValue();
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll(data, 'addchange');
            });
        };
        var onAddCheque = function () {
            var url = '/Applications/DisbursementUseCases/AddDisbursementCheque.aspx';
            var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
            var param = url + guid;
            param += "&type=" + 'Change';
            window.proxy.requestNewTab('AddChangeCheque', param, 'Add Cheque');
        }

        var totalChequesAmount = function () {
            var amount = 0;
            for (var j = 0; j < gridPanelChecks.store.getCount(); j++) {
                var data = gridPanelChecks.store.getAt(j);
                amount += data.get('Amount');
            }

            txtCheckAmount.setValue(amount);
            formatCurrency(txtCheckAmount);
        }
        var caculateTotal = function () {
            var cash = txtCashAmount.getValue();
            cash = cash.replace(/,/g, '');

            var checkAmount = txtCheckAmount.getValue();
            checkAmount = checkAmount.replace(/,/g, '');

            var totalamountTendered = parseFloat(cash) + parseFloat(checkAmount);
            txtAmountToDisburse.setValue(totalamountTendered);
            formatCurrency(txtAmountToDisburse);
        };
        var formatCurrency = function (txt) {

            var num = txt.getValue();
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
            var answer = (((sign) ? '' : '-') + num + '.' + cents);
            txt.setValue(String(answer));
        }
        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
        }
        var onFormValidated = function (valid) {
            btnSave.disable();
            var checkToDisburse = txtCheckAmount.getValue();
            checkToDisburse = checkToDisburse.replace(/,/g, '');
            var cashToDisburse = txtCashAmount.getValue();
            cashToDisburse = cashToDisburse.replace(/,/g, '');
            var receiptbalance = txtReceiptBalance.getValue();
            receiptbalance = receiptbalance.replace(/,/g, '');

            var totalToDisburse = parseFloat(checkToDisburse) + parseFloat(cashToDisburse);
            if (valid  && ((totalToDisburse == receiptbalance)|| (totalToDisburse<receiptbalance)) && (totalToDisburse > 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid && (totalToDisburse > receiptbalance)) {
                PageFormPanelStatusBar.setStatus({ text: 'Amount to Disburse must be lesser than Receipt Balance. ' });
            } else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
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
</head>
<body>
    <form id="form2" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
        <ext:Hidden runat="server" ID="hdnParentPaymentId"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdnSignatureImage1"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit" AutoHeight="true" >
    <Items>
   <%-- Height="500"--%>
    <ext:FormPanel ID="PageFormPanel"  runat="server" Layout="FitLayout"
                Border="false" MonitorValid="true" AutoScroll="true" MonitorPoll="500">
           <Defaults>
            <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                        <DirectEvents>
                            <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="saveSuccessful();">
                              <EventMask ShowMask="true" Msg="Saving.." />
                            </Click>
                        </DirectEvents>
                    </ext:Button>
                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                    <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TabPanel ID="TabPanel" runat="server" Padding="0" Border="false"
                        EnableTabScroll="true" DeferredRender="false">
                <Items>
                    <ext:Panel ID="pnlChange" runat="server" Layout="FormLayout"
                        Title="Voucher" Border="false"  LabelWidth="200">
                        <Items>
                            <ext:Panel ID="Panel2" LabelWidth="180" runat="server"
                                Padding="5"  Layout="FormLayout" ColumnWidth=".6" RowHeight=".5" AutoScroll="true" Border="false">
                                <Items>
                                    <ext:Hidden runat="server" ID="txtCustID"></ext:Hidden>
                                    <ext:Hidden ID="hiddenResourceGUID" runat="server"></ext:Hidden>
                                    <ext:Hidden runat="server" ID="txtUserID"></ext:Hidden>
                                    <ext:Hidden runat="server" ID="txtReceiptID"></ext:Hidden>
                                    <ext:CompositeField runat="server" AnchorHorizontal="50%">
                                    <Items>
                                        <ext:TextField ID="txtCustomerName" runat="server" ReadOnly="true" FieldLabel="Receipt Owner" Flex="1" ></ext:TextField>
                                        <ext:Button ID="btnBrowseReceipt" Text="Browse" runat="server">
                                        <Listeners>
                                        <Click Handler="browseReceipt();" />
                                        </Listeners>
                                        </ext:Button>
                                    </Items>
                                    </ext:CompositeField>
                                     <ext:TextField ID="txtReceiptBalance" runat="server" ReadOnly="true" FieldLabel="Receipt Balance" AnchorHorizontal="50%">
                                     <Listeners>
                                        <Change Handler="formatCurrency(txtReceiptBalance);" />
                                     </Listeners>
                                     </ext:TextField>
                                    <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" AllowBlank="false"
                                        Editable="false" runat="server" AnchorHorizontal="50%">
                                    </ext:DateField>
                                    <ext:TextField ID="txtReceivedBy" FieldLabel="Received By" runat="server"
                                        AnchorHorizontal="50%">
                                    </ext:TextField>
                                    <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="50%" Height="85">
                                        <Items>
                                            <ext:Image ID="imgSpecimenSignature1" Hidden="false" Height="60" Flex="1"
                                                        runat="server" ImageUrl="../../../Resources/images/signhere.png" />
                                            <ext:FileUploadField ID="flupSpecimenSignature1" FieldLabel="Signature" AllowBlank="false" 
                                                Width="175" Height="60" Icon="Attach" runat="server">
                                                <DirectEvents>
                                                    <FileSelected OnEvent="onUploadImage1_Click">
                                                        <EventMask ShowMask="true" Msg="Uploading..." />
                                                    </FileSelected>
                                                </DirectEvents>
                                            </ext:FileUploadField>
                                        </Items>
                                    </ext:CompositeField>
                                </Items>
                            </ext:Panel>
                           
                        </Items>
                    </ext:Panel>
                      <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Disbursement Details"
                        Border="false" LabelWidth="200" ColumnWidth=".6" RowHeight=".5" Padding="5">
                        <Items>
                            <ext:TextField ID="txtCashAmount" FieldLabel="Cash Amount" ReadOnly="false" runat="server"
                                        AnchorHorizontal="40%">
                                        <Listeners>
                                            <Change Handler="caculateTotal();formatCurrency(txtCashAmount);" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField ID="txtCheckAmount" FieldLabel="Total Check Amount" ReadOnly="true"
                                        runat="server" AnchorHorizontal="40%" Hidden="false">
                                    </ext:TextField>
                                    <ext:TextField ID="txtAmountToDisburse" FieldLabel="Amount To Disburse" ReadOnly="true"
                                        runat="server" AnchorHorizontal="40%">
                                    </ext:TextField>
                             <ext:Panel ID="PanelForPayCheck" Title="Check Disbursements" AutoHeight="true" AnchorHorizontal="95%"
                                                runat="server" Layout="FormLayout" Border="false">
                                             <Items>
                                             <ext:Toolbar ID="Toolbar2" runat="server">
                                             <Items>
                                             <ext:Button ID="btnAddCheck" Text="Add" runat="server" Icon="Add">
                                                <Listeners>
                                                    <Click Handler="onAddCheque();" />
                                                </Listeners>
                                             </ext:Button>
                                             <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                             <ext:Button ID="btnDeleteCheck" Text="Delete" runat="server" Disabled="true" Icon="Delete">
                                             <DirectEvents>
                                                <Click OnEvent="btnDeleteCheques_Click" Success="totalChequesAmount();caculateTotal();" />
                                             </DirectEvents>
                                             </ext:Button>
                                             </Items>
                                             </ext:Toolbar>
                                             <ext:GridPanel ID="gridPanelChecks" runat="server" Height="260" EnableColumnHide="false" ColumnLines="false">
                                             <Store>
                                                <ext:Store ID="StoreCheck" runat="server" OnRefreshData="RefreshCheckData">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="RandomKey">
                                                            <Fields>
                                                                <ext:RecordField Name="BankName" />
                                                                <ext:RecordField Name="Branch" />
                                                                <ext:RecordField Name="CheckType" />
                                                                <ext:RecordField Name="CheckNumber" />
                                                                <ext:RecordField Name="_CheckDate"/>
                                                                <ext:RecordField Name="Amount" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                             </Store>
                                             <ColumnModel ID="columnModelChecks" runat="server">
                                                <Columns>
                                                    <ext:Column Header="Bank" Locked="true" Sortable="false" Width="150" Fixed="true" Resizable="false"></ext:Column>
                                                    <ext:Column Header="Branch" Locked="true" Sortable="false" Width="150" Fixed="true" Resizable="false"></ext:Column>
                                                    <ext:Column Header="Check Type" Locked="true" Sortable="false" Width="130" Fixed="true" Resizable="false"></ext:Column>
                                                    <ext:Column Header="Check No." Locked="true" Sortable="false" Width="150" Fixed="true" Resizable="false"></ext:Column>
                                                    <ext:Column Header="Check Date" Locked="true" Sortable="false" Width="100" Fixed="true" Resizable="false"></ext:Column>
                                                    <ext:NumberColumn Header="Amount" Locked="true" Fixed="true" Sortable="false" Width="150" Resizable="false" Format=",000.00"></ext:NumberColumn>
                                                </Columns>
                                             </ColumnModel>
                                             <SelectionModel>
                                                <ext:RowSelectionModel runat="server" SingleSelect="true" ID="ChequesSelectionModel">
                                                <Listeners>
                                                    <RowSelect Fn="onRowSelectedCheck" />
                                                    <RowDeselect Fn="onRowDeselectedCheck" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                         <BottomBar>
                                        <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                                        DisplayMsg="Displaying cheque {0} - {1} of {2}" EmptyMsg="No cheques to display" />
                                        </BottomBar>
                                             </ext:GridPanel>
                                             </Items>
                                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
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
