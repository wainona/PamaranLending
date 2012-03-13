<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFeePayment.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.AddFeePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Create Fee Payment</title>
 <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['customeradded', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'customeradded') {
                txtDisbursedTo.setValue(msg.data.Name);
                txtCustId.setValue(msg.data.PartyRoleId);
            } else if (msg.tag == 'addcheque') {
                X.AddChequesManually(msg.data.BankName, msg.data.Branch, msg.data.CheckNumber, msg.data.CheckType, msg.data.CheckDate, msg.data.TotalAmount, msg.data.BankPartyRoleId, {
                    success: function () {
                        totalChequesAmount();
                        caculateTotal();
                    }
                });
            }

        };

        var openCustomer = function () {
            var url = '/Applications/DisbursementUseCases/CustomerCoBorrowerViewList.aspx';
            var id = 'id=' + txtUserID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('SelectPerson', param, 'Person List');
        };

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('addfeepayment', 'addfeepayment');
                window.proxy.requestClose();
            });
        };

        var saveOtherItemsSuccessful = function () {
            calculateTotal();
            var amount = txtAmount.getValue();
            otherDisbursementItems.hide();
            cmbParticulars.clearValue();
            txtAmount.setValue("");
        };

        var calculateTotal = function () {
            var cs = PageGridPanel.view.getColumnData();
            var total = 0;
            for (var j = 0, jlen = PageGridPanel.store.getCount(); j < jlen; j++) {
                var r = PageGridPanel.store.getAt(j);
                c = cs[1];
                total += r.get(c.name);
            }
            //           var amountreceived = parseFloat(txtAmountReceived.getValue().replace(/\$|\,/g, ''));
            //            var change = amountreceived - total;
            //            txtChange.setValue(Ext.util.Format.number(change, '0,0.00'));
            //            X.ChangeToMoneyFormat(total);
            txtTotalAmountPaid.setValue(total);
            formatCurrency(txtTotalAmountPaid);

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

        var canceOtherItems = function () {
            otherDisbursementItems.hide();
            cmbParticulars.clearValue();
            //txtParticular.setValue("");
            txtAmount.setValue("");
        };

        var OnBtnAddClick = function () {
            hiddenRandomKey.setValue("");
            cmbParticulars.clearValue();
            //txtParticular.setValue("");
            txtAmount.setValue(0);
            otherDisbursementItems.show();
        }
        var btnParticularAdd = function () {
            cmfParticularDropDown.hide();
            cmfParticularAdd.show();
            btnSaveOtherItems.disable();
            txtParticularA.allowBlank = false;
            cmbParticulars.allowBlank = true;
        };

        var btnParticularCancel = function () {
            cmfParticularDropDown.show();
            cmfParticularAdd.hide();
            txtParticularA.allowBlank = true;
            cmbParticulars.allowBlank = false;
        };

        var OnBtnEditClick = function () {
            if (PageGridPanel.hasSelection() == true) {
                var data = PageGridPanelSelectionModel.getSelected().json;
                hiddenRandomKey.setValue(data.RandomKey);
                cmbParticulars.setValue(data.Particular);
                txtAmount.setValue(data.Amount);
                otherDisbursementItems.show();
            }
        }

    
        var newParticular = function () {
            cmfExistParticular.hide();
            cmfNewParticular.show();
            btnSaveOtherItems.disable();
        }

        var btnParticularAddSave = function () {
            X.AddNewParticular({
                success: function (result) {
                    cmbParticulars.setValue('');
                    X.LoadCombo(result);
                }
            });
            btnSaveOtherItems.enable();
            clear();
        }

        var clear = function () {
            txtParticularA.allowBlank = true;
            cmbParticulars.allowBlank = false;
            cmfParticularDropDown.show();
            cmfParticularAdd.hide();
            txtParticularA.setValue('');
          //  txtParticularA.markValid();
            btnParticularSaveA.disable();
            btnSaveOtherItems.enable();
        }
        var caculateTotal = function () {
            var cash = txtCashAmount.getValue();
            cash = cash.replace(/,/g, '');
            var checkAmount = txtCheckAmount.getValue();
            checkAmount = checkAmount.replace(/,/g, '');
            var atmAmount = txtATMAmount.getValue();
            atmAmount = atmAmount.replace(/,/g, '');

            if (cash == '') cash = 0;
            if (checkAmount == '') checkAmount = 0;

            var totalamountTendered = parseFloat(cash) + parseFloat(checkAmount)+parseFloat(atmAmount);
            txtAmountDisbursed.setValue(totalamountTendered);
            formatCurrency(txtAmountDisbursed);
        };
        var totalChequesAmount = function () {
            var amount = 0;
            for (var j = 0; j < gridPanelChecks.store.getCount(); j++) {
                var data = gridPanelChecks.store.getAt(j);
                amount += data.get('Amount');
            }

            txtCheckAmount.setValue(amount);
            formatCurrency(txtCheckAmount);
        }
        var onAddCheque = function () {
        var url = '/Applications/DisbursementUseCases/AddDisbursementCheque.aspx';
        var guid = '?ResourceGuid=' + hiddenResourceGUID.getValue();
        var param = url + guid;
        param += "&type=" + 'FeePayment';
        window.proxy.requestNewTab('AddChequeFee', param, 'Add Cheque');
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
              var atmAmount = txtATMAmount.getValue();
            atmAmount = atmAmount.replace(/,/g, '');

            var totalToDisburse = parseFloat(checkToDisburse) + parseFloat(cashToDisburse) + parseFloat(atmAmount);

            var totalFees = txtTotalAmountPaid.getValue();
            totalFees = totalFees.replace(/,/g, '');

            if (valid && (PageGridPanel.store.getCount() > 0) && ((totalToDisburse == totalFees)|| (totalToDisburse > totalFees))) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid && (PageGridPanel.store.getCount() > 0) && (totalToDisburse < totalFees)) {
                PageFormPanelStatusBar.setStatus({ text: 'Total payment must be >= total fee amount. ' });
            }else if (valid && (PageGridPanel.store.getCount() <= 0)){
                  PageFormPanelStatusBar.setStatus({ text: 'Please fill Fee Items. ' });
            } else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
            }
        }
         var checkValid = function () {
            X.CheckIfValid({
                success: function (result) {
                    if (result) {
                        StatusBar2.setStatus({ text: '' });
                        btnParticularSaveA.enable();
                    } else {
                        StatusBar2.setStatus({ text: 'Particular already exists.' });
                        btnParticularSaveA.disable();
                    }
                }
            });
        }

        var onFormValidated2 = function (valid) {
            var amount = txtAmount.getValue();
            amount = amount.replace(/,/g, '');
            if (valid && btnParticularSaveA.disabled == false && amount > 0) {
                StatusBar2.setStatus({ text: 'Form is valid. ' });
                if (txtParticularA.allowBlank == true)
                    btnSaveOtherItems.enable();
                else
                    btnSaveOtherItems.disable();
            } else if (valid && amount > 0) {
                StatusBar2.setStatus({ text: 'Form is valid. ' });
                btnSaveOtherItems.enable();
            } else if (valid) {
                StatusBar2.setStatus({ text: 'Amount must be greater than 0.00. ' });
                btnSaveOtherItems.disable();
            } else if (!valid) {
                if (cmfParticularDropDown.hidden == true) {
                } else {
                    StatusBar2.setStatus({ text: 'Please fill out the form.' });
                    btnSaveOtherItems.disable();
                }

            }
        }

        //For ATM Records javascript
        var onFormValidated3 = function (valid) {
            var amount = txtATMAmount1.getValue();
            amount = amount.replace(/,/g, '');
            if (valid && amount > 0) {
                StatusBar3.setStatus({ text: 'Form is valid. ' });
                btnSaveATM.enable();
            } else if (valid) {
                StatusBar3.setStatus({ text: 'Amount must be >0.00. ' });
                btnSaveATM.disable();
            } else if (!valid) {
                StatusBar3.setStatus({ text: 'Please fill out the form.' });
                btnSaveATM.disable();
            }
        }

        var OnBtnAddATM = function () {
            HiddenRandomKey2.setValue("");
            txtATMReferenceNum.setValue("");
            txtATMAmount1.setValue(0);
            btnSaveATM.disable();
            wnAddATM.show();
        }

        var OnBtnEditATM = function () {
            if (grdPnlATM.hasSelection() == true) {
                var data = grdPnlATMSelectionModel.getSelected().json;
                HiddenRandomKey2.setValue(data.RandomKey);
                txtATMAmount1.setValue(data.Amount);
                txtATMReferenceNum.setValue(data.ATMReferenceNumber)
                wnAddATM.show();
            }
        }
        var saveATMSuccessful = function () {
            calculateATMTotal();
            wnAddATM.hide();
            txtATMReferenceNum.setValue("");
            txtATMAmount1.setValue(0);
        };

        var calculateATMTotal = function () {
            var amount = 0;
            for (var j = 0; j < grdPnlATM.store.getCount(); j++) {
                var data = grdPnlATM.store.getAt(j);
                amount += data.get('Amount');
            }

            txtATMAmount.setValue(amount);
            formatCurrency(txtATMAmount);
            caculateTotal();
        };
        var cancelATM = function () {
            wnAddATM.hide();
            txtATMReferenceNum.setValue("");
            txtAmount.setValue("");
        };
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
         <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
    <ext:FormPanel ID="PageFormPanel" runat="server" Border="false" MonitorValid="true" AutoScroll="true">
        <TopBar>
            <ext:Toolbar runat="server">
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
                    <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="onBtnClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:TabPanel ID="TabPanel" runat="server" Padding="0" HideBorders="true"
                    EnableTabScroll="true" DeferredRender="false">
            <Items>
            <ext:Panel ID="pnlEncashment" runat="server" Layout="FormLayout" LabelWidth="180"
                Padding="5" AutoHeight="true" Title="Voucher" Border="false">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" Layout="FormLayout" Border="false" AutoHeight="true" Width="600">
                        <Items>
                        <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
                        <ext:Hidden ID="txtBankID" runat="server"></ext:Hidden>
                            <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="95%">
                            <Items>
                             <ext:TextField ID="txtDisbursedTo" FieldLabel="Received From" AllowBlank="false"
                             runat="server" Flex="1" ReadOnly="true">
                            </ext:TextField>
                            <ext:Button runat="server" ID="btnCustomerBrowse" Text="Browse">
                                         <Listeners>
                                             <Click Handler="openCustomer();" />
                                         </Listeners>
                            </ext:Button>
                            </Items>
                            </ext:CompositeField>
                            <ext:DateField ID="txtTransactionDate" Editable="false" AllowBlank="false" FieldLabel="Transaction Date" runat="server"
                                AnchorHorizontal="95%">
                            </ext:DateField>
                            <ext:Hidden ID="txtCustId" runat="server"></ext:Hidden>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="pnlDisbursementItems" runat="server" Layout="FormLayout"
                        Border="true" Title="Fee Items" Height="350">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button ID="Button1" runat="server" Text="Add" Icon="Add">
                                    <Listeners>
                                    <Click Handler="OnBtnAddClick()" />
                                    </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                        <Listeners>
                                            <Click Handler="OnBtnEditClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="otherItemsDelete" runat="server" Text="Delete" Icon="Delete">
                                    <DirectEvents>
                                    <Click OnEvent="otherItemsDelete_Click" Success="calculateTotal();">
                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected fee items?" />
                                    </Click>
                                    </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Items>
                            <ext:GridPanel ID="PageGridPanel" runat="server" Height="350" Border="false">
                                <View>
                                    <ext:GridView EmptyText="No fee items to display">
                                    </ext:GridView>
                                </View>
                            <LoadMask ShowMask="true" />
                                <Store>
                                    <ext:Store ID="PageGridPanelStore" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="RandomKey">
                                                <Fields>
                                                    <ext:RecordField Name="Particular" />
                                                    <ext:RecordField Name="Amount" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                                    <Columns>
                                        <ext:Column Header="Particular" DataIndex="Particular" Locked="true" Wrap="true" Width="140px" Hidden="false" />
                                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="140px" Locked="true" Wrap="true" Align="Right" Format=",000.00" />
                                    </Columns>
                                </ColumnModel>
                                 <Listeners>
                                    <AfterRender Handler="calculateTotal();" Delay="100" />
                                </Listeners>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="12" DisplayInfo="true"
                                        DisplayMsg="Displaying fee items {0} - {1} of {2}" EmptyMsg="No fee items to display" />
                                </BottomBar>
                            </ext:GridPanel>
                        </Items>
                        <BottomBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:TextField ID="txtTotalAmountPaid" runat="server"  FieldLabel="Total Fee Amount" MaskRe="/\d/" ReadOnly="true">
                                    </ext:TextField>
                            </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </Items>
            </ext:Panel>
                        <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Payment Details"
                        HideBorders="true" LabelWidth="200" ColumnWidth=".6" Height="600" Padding="5">
                        <Items>
                        <ext:Hidden ID="hiddenResourceGUID" runat="server"></ext:Hidden>
                            <ext:TextField ID="txtCashAmount" FieldLabel="Cash Payment" ReadOnly="false"
                                runat="server" AnchorHorizontal="55%">
                                <Listeners>
                                    <Change Handler="caculateTotal();formatCurrency(txtCashAmount);" />
                                    
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtATMAmount" FieldLabel="ATM Payment" ReadOnly="true"
                            runat="server" AnchorHorizontal="55%"></ext:TextField>
                            <ext:TextField ID="txtCheckAmount" FieldLabel="Check Payment" ReadOnly="true"
                                runat="server" AnchorHorizontal="55%" Hidden="false">
                            </ext:TextField>
                            <ext:TextField ID="txtAmountDisbursed" FieldLabel="Total Payment"
                            runat="server" AnchorHorizontal="55%" ReadOnly="true">
                            </ext:TextField>
                            <ext:Panel ID="PanelForPayCheck" Title="Check Payment" Height="200" AnchorHorizontal="95%"
                                runat="server" Layout="FormLayout" Border="false">
                                <Items>
                                    <ext:Toolbar ID="Toolbar3" runat="server">
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
                                    <ext:GridPanel ID="gridPanelChecks" runat="server" Height="140" EnableColumnHide="false"
                                        ColumnLines="false">
                                        <Store>
                                            <ext:Store ID="StoreCheck" runat="server" OnRefreshData="RefreshCheckData">
                                                <Reader>
                                                    <ext:JsonReader IDProperty="RandomKey">
                                                        <Fields>
                                                            <ext:RecordField Name="BankName" />
                                                            <ext:RecordField Name="Branch" />
                                                            <ext:RecordField Name="CheckType" />
                                                            <ext:RecordField Name="CheckNumber" />
                                                            <ext:RecordField Name="_CheckDate" />
                                                            <ext:RecordField Name="Amount" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="columnModelChecks" runat="server">
                                            <Columns>
                                                <ext:Column Header="Bank" Locked="true" Sortable="false" Width="150" Fixed="true"
                                                    Resizable="false">
                                                </ext:Column>
                                                <ext:Column Header="Branch" Locked="true" Sortable="false" Width="150" Fixed="true"
                                                    Resizable="false">
                                                </ext:Column>
                                                <ext:Column Header="Check Type" Locked="true" Sortable="false" Width="130" Fixed="true"
                                                    Resizable="false">
                                                </ext:Column>
                                                <ext:Column Header="Check No." Locked="true" Sortable="false" Width="150" Fixed="true"
                                                    Resizable="false">
                                                </ext:Column>
                                                <ext:Column Header="Check Date" DataIndex="_CheckDate" Locked="true" Sortable="false" Width="100" Fixed="true"
                                                    Resizable="false">
                                                </ext:Column>
                                                <ext:NumberColumn Header="Amount" Locked="true" Fixed="true" Sortable="false" Width="150"
                                                    Resizable="false" Format=",000.00">
                                                </ext:NumberColumn>
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
                                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="5" DisplayInfo="true"
                                                DisplayMsg="Displaying cheque {0} - {1} of {2}" EmptyMsg="No cheques to display" />
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                       <ext:Panel ID="Panel3" Title="ATM Payment" Height="200" AnchorHorizontal="95%"
                                runat="server" Layout="FormLayout" Border="false">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar4" runat="server">
                                <Items>
                                    <ext:Button ID="btnATMAdd" runat="server" Text="Add" Icon="Add">
                                    <Listeners>
                                    <Click Handler="OnBtnAddATM()" />
                                    </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="btnATMEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                        <Listeners>
                                            <Click Handler="OnBtnEditATM()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="btnATMDelete" runat="server" Text="Delete" Icon="Delete">
                                    <DirectEvents>
                                    <Click OnEvent="onBtnATMDelete_Click" Success="calculateATMTotal();">
                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to remove selected payment?" />
                                    </Click>
                                    </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Items>
                            <ext:GridPanel ID="grdPnlATM" runat="server" Height="150">
                                <View>
                                    <ext:GridView EmptyText="No atm items to display">
                                    </ext:GridView>
                                </View>
                            <LoadMask ShowMask="true" />
                                <Store>
                                    <ext:Store ID="grdPnlATMStore" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="RandomKey">
                                                <Fields>
                                                    <ext:RecordField Name="Amount" />
                                                    <ext:RecordField Name="ATMReferenceNumber" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel runat="server" ID="ColumnModel1" Width="100%">
                                    <Columns>
                                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="140px" Locked="true" Wrap="true" Align="Right" Format=",000.00" />
                                        <ext:Column Header="Reference Number" DataIndex="ATMReferenceNumber" Locked="true" Wrap="true" Width="140px" Hidden="false" />
                                    </Columns>
                                </ColumnModel>
                                 <Listeners>
                                    <AfterRender Handler="calculateATMTotal();"/>
                                </Listeners>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="grdPnlATMSelectionModel" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="5" DisplayInfo="true"
                                        DisplayMsg="Displaying disbursement items {0} - {1} of {2}" EmptyMsg="No atm payments to display" />
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

        <ext:Window ID="otherDisbursementItems" Hidden="true" runat="server" Width="440" Icon="Application"
        Title="Fee Items" Modal="true" AutoHeight="true" PageY="300">
        <Items>
            <ext:FormPanel ID="FormPanel1" runat="server" LabelWidth="140" MonitorValid="true">
                <Items>
                      <ext:CompositeField runat="server" ID="cmfParticularDropDown" FieldLabel="Particular" AnchorHorizontal="95%">
                        <Items>
                            <ext:ComboBox runat="server" ID="cmbParticulars" FieldLabel="Particular" AllowBlank="false"
                                Editable="false" ForceSelection="true" ValueField="Id" DisplayField="Name" Flex="1">
                                <Store>
                                    <ext:Store runat="server" ID="storeParticulars" RemoteSort="false">
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
                            <ext:Button runat="server" ID="btnAddParticularD" Icon="Add" Width="30">
                                <Listeners>
                                    <Click Handler="btnParticularAdd();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                      </ext:CompositeField>
                      <ext:CompositeField runat="server" ID="cmfParticularAdd" FieldLabel="Particular" Hidden="true" AnchorHorizontal="95%">
                        <Items>
                            <ext:TextField ID="txtParticularA" EmptyText="Type particular here..." runat="server" AllowBlank="true" 
                                MaskRe="^[a-zA-Z0-9 ]+$" Flex="1">
                                <Listeners>
                                    <Change Handler="checkValid();" />
                                </Listeners>
                            </ext:TextField>
                            <ext:Button runat="server" ID="btnParticularSaveA" Icon="Disk" Disabled="true" Width="30">
                                <Listeners>
                                    <Click Handler="btnParticularAddSave();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnAddParticularCancel" Icon="Cancel" Width="30">
                                <Listeners>
                                    <Click Handler="clear();" />
                                </Listeners>
                            </ext:Button> 
                        </Items>
                      </ext:CompositeField>
                      <ext:TextField ID="txtAmount" FieldLabel="Amount" AllowBlank="false" runat="server" AnchorHorizontal="95%">
                      <Listeners>
                     <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                      </Listeners>
                     </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar2" runat="server">
                        <Items>
                            <ext:Hidden ID ="hiddenRandomKey" runat="server"></ext:Hidden>
                            <ext:Button ID="btnSaveOtherItems" runat="server" Text="Save" Icon="Disk" Disabled="true">
                            <DirectEvents>
                                    <Click OnEvent="btnSaveOtherItems_Click" Success="saveOtherItemsSuccessful();"/>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="Button2" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="canceOtherItems();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onFormValidated2(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>

        <ext:Window ID="wnAddATM" Hidden="true" runat="server" Width="450" Icon="Application"
        Title="ATM Payment" Modal="true" AutoHeight="true" PageY="300">
        <Items>
            <ext:FormPanel ID="FormPanel2" runat="server" LabelWidth="160" MonitorValid="true">
                <Items>
                      <ext:TextField ID="txtATMAmount1" FieldLabel="Amount" AllowBlank="false" runat="server" AnchorHorizontal="95%">
                      <Listeners>
                     <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                      </Listeners>
                     </ext:TextField>
                     <ext:TextField ID="txtATMReferenceNum" FieldLabel="Reference Number" AllowBlank="false" runat="server" AnchorHorizontal="95%"></ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar3" runat="server">
                        <Items>
                               <ext:Hidden ID ="HiddenRandomKey2" runat="server"></ext:Hidden>
                            <ext:Button ID="btnSaveATM" runat="server" Text="Save" Icon="Disk" Disabled="true">
                            <DirectEvents>
                                    <Click OnEvent="btnSaveATM_Click" Success="saveATMSuccessful();"/>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCanceATM" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="cancelATM();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="onFormValidated3(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>

