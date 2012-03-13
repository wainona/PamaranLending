<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOtherDisbursement.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.AddOtherDisbursement" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Create Other Disbursement</title>
 <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['onpickemployee', 'addcheque']);
            window.proxy.on('messagereceived', onMessageReceived);
            formatCurrency(txtAmountDisbursed);
            formatCurrency(txtCashAmount);
            formatCurrency(txtCheckAmount);
            formatCurrency(txtAmount);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickemployee') {
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
            var url = '/Applications/EmployeeUseCases/EmployeePickList.aspx';
            var id = 'id=' + txtUserID.getValue();
            var param = url + "?" + id;
            window.proxy.requestNewTab('SelectEmployee', param, 'Employee List');
        };
        var saveSuccessful = function () {
            var data = {};
            data.id = hdnDisbursmentId.getValue();
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToParent(data, 'addotherdisbursement');
                //window.proxy.sendToAll('addotherdisbursement', 'addotherdisbursement');
                //window.proxy.requestClose();
            });
        };
        var saveOtherItemsSuccessful = function () {
            calculateTotal();
            otherDisbursementItems.hide();
            cmbParticulars.clearValue();
            //txtParticularA.setValue("");
            txtAmount.setValue(0);
        };

        var calculateTotal = function () {
            var cs = PageGridPanel.view.getColumnData();
            var total = 0;
            for (var j = 0, jlen = PageGridPanel.store.getCount(); j < jlen; j++) {
                var r = PageGridPanel.store.getAt(j);
                c = cs[1];
                total += r.get(c.name);
            }

            txtTotalAmountToDisburse.setValue(total);
            formatCurrency(txtTotalAmountToDisburse);
        };

        var canceOtherItems = function () {
            otherDisbursementItems.hide();
            cmbParticulars.clearValue();
            //txtParticularA.setValue("");
            txtAmount.setValue(0);
            clear();
        };

        var OnBtnAddClick = function () {
            hiddenRandomKey.setValue("");
            cmbParticulars.clearValue();
            //txtParticularA.setValue("");
            txtAmount.setValue(0);
            btnParticularSaveA.disable();
            otherDisbursementItems.show();
        }

        var OnBtnEditClick = function () {
            if (PageGridPanel.hasSelection() == true) {
                var data = PageGridPanelSelectionModel.getSelected().json;
                hiddenRandomKey.setValue(data.RandomKey);
                cmbParticulars.setValue(data.Particular);
                txtAmount.setValue(data.Amount);
                otherDisbursementItems.show();
            }
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

        var btnParticularAddSave = function () {
            X.saveOtherDisbursementParticular({
                success: function (result) {
                    cmbParticulars.setValue('');
                    X.LoadDropDown(result);
                }
            });

            btnSaveOtherItems.enable();
            clear();
        };

        var clear = function () {
            txtParticularA.allowBlank = true;
            cmbParticulars.allowBlank = false;
            cmfParticularDropDown.show();
            cmfParticularAdd.hide();
            txtParticularA.setValue('');
            txtParticularA.markValid();
            btnParticularSaveA.disable();
            btnSaveOtherItems.enable();
        }

        var caculateTotal = function () {
            var cash = txtCashAmount.getValue();
             cash = cash.replace(/,/g, '');
             var checkAmount = txtCheckAmount.getValue();
             checkAmount = checkAmount.replace(/,/g, '');
            var totalamountTendered = parseFloat(cash) + parseFloat(checkAmount);
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
            param += "&type=" + 'OtherLoanDisbursement';
            window.proxy.requestNewTab('AddOtherCheque', param, 'Add Cheque');
        }

        var onRowSelectedCheck = function () {
            btnDeleteCheck.enable();
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
        var onRowDeselectedCheck = function () {
            btnDeleteCheck.disable();
        }
        var onFormValidated = function (valid) {
            btnSave.disable();
            var totalamount = txtAmountDisbursed.getValue();
            totalamount = totalamount.replace(/,/g, '');
            var totalpnlamount = txtTotalAmountToDisburse.getValue();
            totalpnlamount = totalpnlamount.replace(/,/g, '');
            totalpnlamount = totalpnlamount.replace(/,/g, '');
            if (valid && PageGridPanel.store.getCount() > 0 && totalamount == totalpnlamount) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else if (valid && PageGridPanel.store.getCount() > 0 && totalamount != totalpnlamount) {
                PageFormPanelStatusBar.setStatus({ text: 'Total amount to be disbursed must equal to total disbursement items amount. ' });
            } else if (valid && totalamount != totalpnlamount) {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill disbursement items. ' });
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
        <ext:Hidden runat="server" ID="hdnDisbursmentId"></ext:Hidden>
        <ext:Hidden runat="server" ID="hdnSignatureImage1"></ext:Hidden>
         <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
    <Items>
      <ext:FormPanel ID="PageFormPanel"  runat="server" Layout="FitLayout"
                Border="false" MonitorValid="true" AutoScroll="true" MonitorPoll="500">
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
            <ext:Panel ID="pnlEncashment" runat="server" Layout="FormLayout" LabelWidth="180"
                Padding="5" AutoHeight="true" Title="Voucher" Border="false">
                <Items>
                    <ext:Panel ID="Panel2" runat="server" ColumnWidth=".75" Layout="FormLayout" Border="false"
                        Height="150" Width="550" LabelWidth="150">
                        <Items>
                        <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
                        <ext:Hidden ID="txtBankID" runat="server"></ext:Hidden>
                             <ext:CompositeField runat="server" AnchorHorizontal="100%">
                            <Items>
                             <ext:TextField ID="txtDisbursedTo" FieldLabel="Disbursed To" AllowBlank="false"
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
                                AnchorHorizontal="100%">
                            </ext:DateField>
                            <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="100%" Height="85">
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
                            <ext:Hidden ID="txtCustId" runat="server"></ext:Hidden>
                        </Items>
                    </ext:Panel>
                     <ext:Panel ID="pnlDisbursementItems" Title="Disbursement Items" AutoHeight="true" AnchorHorizontal="98%"
                      runat="server" Layout="FormLayout" Border="false">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button runat="server" Text="Add" Icon="Add">
                                    <Listeners>
                                    <Click Handler="OnBtnAddClick()" />
                                    </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="btnEdit" runat="server" Text="Edit" Icon="NoteEdit">
                                        <Listeners>
                                            <Click Handler="OnBtnEditClick()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="otherItemsDelete" runat="server" Text="Delete" Icon="Delete">
                                    <DirectEvents>
                                    <Click OnEvent="otherItemsDelete_Click" Success="calculateTotal();">
                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected other disbursement items?" />
                                    </Click>
                                    </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Items>
                            <ext:GridPanel ID="PageGridPanel" runat="server" Height="300">
                                <View>
                                    <ext:GridView EmptyText="No disbursement items to display">
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
                                        <ext:Column Header="Particular" DataIndex="Particular" Locked="true" Wrap="true" Width="300" Hidden="false" />
                                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="500" Locked="true" Wrap="true" Format=",000.00" />
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
                                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                                        DisplayMsg="Displaying disbursement items {0} - {1} of {2}" EmptyMsg="No disbursement items to display" />
                                </BottomBar>
                            </ext:GridPanel>
                        </Items>
                        <BottomBar>
                        <ext:Toolbar runat="server">
                        <Items>
                        <ext:ToolbarFill></ext:ToolbarFill>
                           <ext:TextField ID="txtTotalAmountToDisburse" runat="server"  FieldLabel="Total Amount To Disburse:" MaskRe="/\d/" ReadOnly="true">
                           <Listeners>
                            <Change Handler="formatCurrency(txtTotalAmountToDisburse);" />
                           </Listeners>
                            </ext:TextField>
                        </Items>
                        </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </Items>
            </ext:Panel>
            <ext:Panel ID="Panel1" runat="server" Layout="FormLayout" Title="Disbursement Details"
                        HideBorders="true" LabelWidth="200" ColumnWidth=".6" RowHeight=".5" Padding="5">
                        <Items>
                        <ext:Hidden ID="hiddenResourceGUID" runat="server"></ext:Hidden>
                            <ext:TextField ID="txtCashAmount" FieldLabel="Cash Amount To Disburse" ReadOnly="false"
                                runat="server" AnchorHorizontal="55%">
                                <Listeners>
                                    <Change Handler="formatCurrency(txtCashAmount);caculateTotal();" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtCheckAmount" FieldLabel="Check Amount To Disburse" ReadOnly="true"
                                runat="server" AnchorHorizontal="55%" Hidden="false">
                            </ext:TextField>
                            <ext:TextField ID="txtAmountDisbursed" FieldLabel="Total Amount To Disburse"
                            runat="server" AnchorHorizontal="55%" ReadOnly="true">
                            </ext:TextField>
                            <ext:Panel ID="PanelForPayCheck" Title="Check Disbursements" Height="345" AnchorHorizontal="95%"
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
                                    <ext:GridPanel ID="gridPanelChecks" runat="server" Height="260" EnableColumnHide="false"
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
                                                            <ext:RecordField Name="_CheckDate"/>
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
                                                <ext:Column Header="Check Date" Locked="true" Sortable="false" Width="100" Fixed="true"
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
                                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" PageSize="10" DisplayInfo="true"
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
    <ext:Window ID="otherDisbursementItems" Hidden="true" runat="server" Width="440" Icon="Application"
        Title="Other Disbursement Items" Modal="true" AutoHeight="true" PageY="300">
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
                     <Change Handler="formatCurrency(txtAmount);" />
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
                            <ext:Button ID="Button1" runat="server" Text="Cancel" Icon="Cancel">
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
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
