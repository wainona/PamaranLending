<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddForeignEncashment.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForeignDisbursementUseCases.AddForeignEncashment" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Create Encashment</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['bankselected', 'customeradded', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'bankselected') {
                txtBank.setValue(msg.data.OrganizationName);
                txtBankId.setValue(msg.data.PartyRoleId);
            } else if (msg.tag == 'customeradded') {
                txtProcessedToCustomerId.setValue(msg.data.PartyRoleId);
                txtDisbursedTo.setValue(msg.data.Name);
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, msg.data.BankPartyRoleId, {
                    success: function () {
                        totalChequesAmount();
                        caculateTotal();
                    }
                });
            }
        };
        var openBank = function () {
            window.proxy.requestNewTab('SelectBank', '/Applications/DisbursementUseCases/BankViewList.aspx', 'Bank List');
        };
        var openCustomer = function () {
            var url = '/Applications/DisbursementUseCases/CustomerCoBorrowerViewList.aspx';
            var id = 'id=' + txtUserID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('SelectCustomerCoBorrower', param, 'Customer and Co-Borrower List');
        };
        var saveSuccessful = function () {
            var data = {};
            data.id = hdnDisbursementId.getValue();
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToParent(data, 'addforeignencashment');
            });
        };
        var checkamount = function () {
            var chkamount = txtChkAmount.getValue();
            chkamount = chkamount.replace(/,/g, '');
            txtCashAmount.setValue(chkamount);

            formatCurrency(txtCashAmount);
            caculateTotal();
        };

        var validateCheckNumber = function () {
            var checkNumber = txtChkNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtChkNumber.markInvalid('Check Number already exist.');
                        hdnChequeValid.setValue(1);
                    } else {
                        hdnChequeValid.setValue(0);
                        btnSave.enable();
                    }
                }
            });

        };
        var onAddCheque = function () {
            var url = '/Applications/DisbursementUseCases/AddDisbursementCheque.aspx';
            var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
            var param = url + guid;
            param += "&type=" + 'Encashment';
            window.proxy.requestNewTab('AddForeignEncashmentCheque', param, 'Add Cheque');
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
            if (cash == '') cash = 0;
            if (checkAmount == '') checkAmount = 0;
            var totalamountTendered = parseFloat(cash) + parseFloat(checkAmount);
            txtAmountToDisburse.setValue(totalamountTendered);
            formatCurrency(txtAmountToDisburse);
        };

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
        }

        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
        }
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
        var onFormValidated = function (valid) {
            btnSave.disable();
            var chkamount = txtChkAmount.getValue();
            chkamount = chkamount.replace(/,/g, '');
            var totalToDisburse = txtAmountToDisburse.getValue();
            totalToDisburse = totalToDisburse.replace(/,/g, '');

            if (((valid && hdnChequeValid.getValue() == '') || (valid && hdnChequeValid.getValue() == 0)) && (chkamount == totalToDisburse)) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if ((valid && hdnChequeValid.getValue() == '') || (valid && hdnChequeValid.getValue() == 0)) {
                PageFormPanelStatusBar.setStatus({ text: 'Total Amount to Disburse must be equal to Check Amount' });
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
<form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
        <ext:Hidden ID="hdnDisbursementId" runat="server"></ext:Hidden>
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
     <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" Height="350" AutoScroll="true" MonitorValid="true">
          <Defaults>
             <ext:Parameter Name="MsgTarget" Value="side" />
            </Defaults>
         <TopBar>
             <ext:Toolbar ID="Toolbar1" runat="server">
                 <Items>
                     <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                         <DirectEvents>
                             <Click OnEvent="btnSave_Click" Success="saveSuccessful();" Before="return #{PageFormPanel}.getForm().isValid();">
                               <EventMask ShowMask="true" Msg="Saving.." />
                             </Click>
                         </DirectEvents>
                     </ext:Button>
                     <ext:ToolbarSeparator>
                     </ext:ToolbarSeparator>
                     <ext:Button ID="Button2" runat="server" Text="Close" Icon="Cancel">
                         <Listeners>
                             <Click Handler="onBtnClose();" />
                         </Listeners>
                     </ext:Button>
                 </Items>
             </ext:Toolbar>
         </TopBar>
         <Items>
         <ext:Hidden ID="hiddenResourceGUID" runat="server"></ext:Hidden>
            <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
            <ext:Hidden ID="hdnChequeValid" runat="server"></ext:Hidden>
            <ext:TabPanel ID="TabPanel" runat="server" Padding="0" HideBorders="true"
                        EnableTabScroll="true" DeferredRender="false">
                <Items>
             <ext:Panel ID="pnlEncashment" runat="server" Layout="FormLayout" LabelWidth="150"
                 Padding="5" AutoHeight="true" Title="Voucher" Border="false">
                 <Items>
                     <ext:Panel ID="Panel2" runat="server" ColumnWidth=".6" Layout="FormLayout" Border="false"
                         Height="250" AnchorHorizontal="100%" >
                         <Items>
                          <ext:Hidden ID="txtBankId" runat="server">
                             </ext:Hidden>
                             <ext:Hidden runat="server" ID="txtProcessedToCustomerId"></ext:Hidden>
                             <ext:ComboBox ID="cmbCurrency" runat="server" FieldLabel="Currency" ValueField="Id"
                                 DisplayField="NameDescription" AnchorHorizontal="60%" Editable="false" AllowBlank="false">
                                   <Store>
                                     <ext:Store ID="strCurrency" runat="server">
                                         <Reader>
                                             <ext:JsonReader IDProperty="Id">
                                                 <Fields>
                                                     <ext:RecordField Name="Id" />
                                                     <ext:RecordField Name="NameDescription" />
                                                 </Fields>
                                             </ext:JsonReader>
                                         </Reader>
                                     </ext:Store>
                                 </Store>
                             </ext:ComboBox>
                                <ext:CompositeField ID="CompositeField2" AnchorHorizontal="60%" runat="server">
                                 <Items>
                                     <ext:TextField ID="txtDisbursedTo" FieldLabel="Disbursed To" AllowBlank="false" ReadOnly="true" Flex="1"
                                         runat="server">
                                     </ext:TextField>
                                     <ext:Button runat="server" ID="btnCustomerBrowse" Text="Browse...">
                                         <Listeners>
                                             <Click Handler="openCustomer();" />
                                         </Listeners>
                                     </ext:Button>
                                 </Items>
                             </ext:CompositeField
                             <ext:DateField ID="txtTransactionDate" FieldLabel="Transaction Date" Editable="false"
                                 AllowBlank="false" runat="server" AnchorHorizontal="60%">
                             </ext:DateField>
                             <ext:CompositeField ID="CompositeField1" AnchorHorizontal="60%" runat="server">
                                 <Items>
                                     <ext:TextField ID="txtBank" FieldLabel="Bank" AllowBlank="false" ReadOnly="true" runat="server" Flex="1">
                                     </ext:TextField>
                                     <ext:Button ID="btnBankBrowse" runat="server" Text="Browse...">
                                         <Listeners>
                                             <Click Handler="openBank();" />
                                         </Listeners>
                                     </ext:Button>
                                 </Items>
                             </ext:CompositeField>
                                 <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Cheque Type"
                                 AnchorHorizontal="60%" Editable="false" AllowBlank="false">
                                 <Items>
                                     <ext:ListItem Text="Pay Check" />
                                     <ext:ListItem Text="Personal Check" />
                                 </Items>
                             </ext:ComboBox>
                             <ext:TextField ID="txtChkNumber" FieldLabel="Check Number" AllowBlank="false" runat="server"
                                 AnchorHorizontal="60%" EnableKeyEvents="true">
                                 <Listeners>
                                     <KeyUp Handler="validateCheckNumber();" />
                                 </Listeners>
                             </ext:TextField>
                             <ext:DateField ID="txtChkDate" Editable="false" FieldLabel="Check Date" AllowBlank="false" runat="server"
                                 AnchorHorizontal="60%">
                             </ext:DateField>
                             <ext:TextField ID="txtChkAmount" FieldLabel="Check Amount" AllowBlank="false"
                                 runat="server" AnchorHorizontal="60%">
                                    <Listeners>
                                        <Change Handler="formatCurrency(txtChkAmount);" />
                                        <Blur Handler="checkamount();" />
                                   </Listeners>
                             </ext:TextField>
                          
                                                         <ext:TextField ID="txtDisbursedToName" FieldLabel="Received By" runat="server"
                                 AnchorHorizontal="60%">
                             </ext:TextField>
                         </Items>
                     </ext:Panel>
                         
                 </Items>
             </ext:Panel>
               <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Disbursement Details"
                        HideBorders="true" LabelWidth="200" ColumnWidth=".6" RowHeight=".5" Padding="5">
                        <Items>
                             <ext:TextField ID="txtCashAmount" FieldLabel="Cash Amount To Disburse" ReadOnly="false"
                                 runat="server" AnchorHorizontal="45%">
                                 <Listeners>
                                 <Change Handler="caculateTotal();formatCurrency(txtCashAmount);" />
                                 </Listeners>
                             </ext:TextField>
                             <ext:TextField ID="txtCheckAmount" FieldLabel="Check Amount To Disburse" ReadOnly="true"
                             runat="server" AnchorHorizontal="45%" Hidden="false"></ext:TextField>
                             <ext:TextField ID="txtAmountToDisburse" FieldLabel="Total Amount To Disburse" ReadOnly="true"
                             runat="server" AnchorHorizontal="45%"></ext:TextField>
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

