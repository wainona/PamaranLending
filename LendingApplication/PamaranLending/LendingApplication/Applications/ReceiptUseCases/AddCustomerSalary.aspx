<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCustomerSalary.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.AddCustomerSalary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Customer Salary</title>
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
            defaultValues();
        });

        var defaultValues = function() {
            fsCheckDetails.show();
            txtCheckNumber.allowBlank = false;
            txtBank.allowBlank = false;
            dtCheckDate.allowBlank = false;
            txtRefNum.hide();
            markIfRequired(txtCheckNumber);
            markIfRequired(txtBank);
            markIfRequired(dtCheckDate);
        }

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
            if (msg.tag == 'onpickbank') {
                hdnBankID.setValue(msg.data.Id);
                txtBank.setValue(msg.data.Name);
            }
        };

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('addcustomersalary', 'addcustomersalary');
                onBtnClear();
            });
        }

        var onFormValidation = function (valid) {
            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            } else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
                btnSave.disable();
            }
        }

        var onBtnClear = function () {
            txtName.setValue('');
            txtAmount.setValue('');
            hdnBankID.setValue('');
            txtBank.setValue('');
            txtCheckNumber.setValue('');
            dtCheckDate.setValue('');
            txtRefNum.setValue('');
        }

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
        }

        var generateAdditionalInterest = function () {
        }

        var cmbPaymentMethodChanged = function () {
            if (cmbPaymentMethod.getText() == 'Personal Check' || cmbPaymentMethod.getText() == 'Pay Check') {
                fsCheckDetails.show();
                txtCheckNumber.allowBlank = false;
                txtBank.allowBlank = false;
                dtCheckDate.allowBlank = false;
                txtRefNum.hide();
            } else {
                txtCheckNumber.allowBlank = true;
                txtBank.allowBlank = true;
                dtCheckDate.allowBlank = true;
                hdnBankID.clear();
                txtBank.clear();
                txtCheckNumber.clear();
                dtCheckDate.clear();
                fsCheckDetails.hide();
                txtRefNum.show();
            }
            markIfRequired(txtCheckNumber);
            markIfRequired(txtBank);
            markIfRequired(dtCheckDate);
        };

        var onRowSelected = function () {
            var result = false;
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            var selected = selectedRows[0];

            hdnSelectedPartyRoleId.setValue(selected.id);
            txtName.setValue(selected.json.Names);

        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden runat="server" ID="hdnSelectedPartyRoleId"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" Padding="5" MonitorValid="true" MonitorPoll="500" Title="New Receipt"
            BodyStyle="background-color:transparent" LabelWidth="130" Border="false">
                <Items>
                    <ext:Panel ID="DetailsPanel" runat="server" Border="false" Layout="ColumnLayout" 
                            Padding="5" >
                        <Items>
                        <ext:Panel runat="server" Layout="FormLayout"
                                    ColumnWidth="0.5" Height="600" Border="true">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk" Disabled="true">
                                            <DirectEvents>
                                                <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();" Success="saveSuccessful();">
                                                    <EventMask Msg="Adding to Database..." ShowMask="true" />
                                                </Click>
                                              </DirectEvents>
                                          </ext:Button>
                                          <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                          <ext:Button ID="btnCancel" runat="server" Text="Clear" Icon="Cancel">
                                              <Listeners>
                                                  <Click Handler="onBtnClear();" />
                                              </Listeners>
                                          </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Items>
                                <ext:Panel runat="server" ID="Details" Title="Receipt Details" LabelWidth="150" AutoHeight="true" Padding="5">
                                    <Items>
                                        <ext:TextField ID="txtName" runat="server" ReadOnly="true" Width="450" FieldLabel="Name" AllowBlank="false"/>
                                        <ext:TextField ID="txtAmount" runat="server" Width="450" FieldLabel="Amount" AllowBlank="false" MaskRe="[0-9\.\,]">
                                            <Listeners>
                                                <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                                <Show Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                                            </Listeners>
                                        </ext:TextField>
                                         <ext:ComboBox ID="cmbPaymentMethod" runat="server" FieldLabel="Payment Method" ValueField="Id" DisplayField="Name" 
                                            Width="450" Editable="false" AllowBlank="false"><%--Payment Method--%>
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
                                        <ext:TextField ID="txtRefNum" runat="server" Width="450" FieldLabel="Reference Number" />
                                        <ext:FieldSet runat="server" ID="fsCheckDetails" Hidden="true" Border="false" LabelWidth="140">
                                            <Items>
                                                <ext:CompositeField ID="CompositeField1" runat="server" Width="450">
                                                    <Items>
                                                        <ext:TextField ID="txtBank" runat="server" ReadOnly="true" Width="240" FieldLabel="Bank" />
                                                        <ext:Button ID="btnBankBrowse" runat="server" Text="Browse">
                                                        <Listeners>
                                                                <Click Handler="window.proxy.requestNewTab('BankPicker', '/Applications/BankUseCases/PickListBank.aspx?mode=single', 'Bank List');" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Items>
                                                </ext:CompositeField>
                                                <ext:Hidden ID="hdnBankID" runat="server"/>
                                                <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check Number" Width="440">
                                                    <Listeners>
                                                        <Change Handler="validateCheckNumber();" />
                                                    </Listeners>
                                                </ext:TextField>
                                                <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" Width="440" Editable="false" />
                                            </Items>
                                        </ext:FieldSet>
                                    </Items>
                                    
                                    <BottomBar>
                                        <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
                                    </BottomBar>
                                </ext:Panel>
                                <%--<ext:Panel runat="server" ID="PickList" Title="Loan/s Details" LabelWidth="175" Padding="5">
                                    <Items>
                                        <ext:TextField ID="wntxtLoanBalance" runat="server" ReadOnly="true" FieldLabel="Remaining Loan Balance" Width="450"/>
                                        <ext:TextField ID="wntxtExistInterest" runat="server" ReadOnly="true" FieldLabel="Existing Billed Interest" Width="450" />
                                        <ext:TextField ID="wntxtExistInterestDate" runat="server" ReadOnly="true" FieldLabel="Existing Interest Date" Width="450" />
                                        <ext:DateField ID="dtGenerationDate" runat="server" FieldLabel="Generate Additional Interest"
                                            Editable="false" Width="450">
                                            <Listeners>
                                                 <Select Handler="generateAdditionalInterest();" />
                                            </Listeners>
                                        </ext:DateField>
                                        <ext:TextField ID="wntxtAddIntererst" runat="server" FieldLabel="Additional Interest Amount" Width="450" ReadOnly="true"></ext:TextField>
                                    </Items>
                                </ext:Panel>--%>
                            </Items>
                        </ext:Panel>
                        <ext:Panel ID="Panel1" runat="server" Border="false" Layout="FormLayout"
                                        ColumnWidth="0.5" Height="600">
                        <Items>
                        <ext:GridPanel 
                            ID="PageGridPanel" 
                            runat="server"
                            AutoExpandColumn="Addresses" Height="555">
                            <LoadMask ShowMask="true" Msg="Loading Customers.."/>
                            <View>
                                <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No customers to display..." runat="server" />
                            </View>
                            <TopBar>
                                <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                                    <Items>
                                        <ext:ToolbarFill />
                                        <ext:TextField ID="txtSearchKey" EmptyText="Search Text Here..." runat="server" FieldLabel="Name" LabelWidth="30"></ext:TextField>
                                        <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                            <DirectEvents>
                                                <Click OnEvent="btnSearch_Click">
                                                </Click>
                                            </DirectEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false" OnRefreshData="RefreshData">
                                    <Proxy>
                                        <ext:PageProxy>
                                        </ext:PageProxy>
                                    </Proxy>
                                    <AutoLoadParams>
                                        <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                        <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                                    </AutoLoadParams>
                                    <Listeners>
                                        <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                    </Listeners>
                                    <Reader>
                                        <ext:JsonReader IDProperty="CustomerID">
                                            <Fields>
                                                <ext:RecordField Name="CustomerID" />
                                                <ext:RecordField Name="Names" />
                                                <ext:RecordField Name="Addresses" />
                                                <ext:RecordField Name="PartyType" />
                                                <ext:RecordField Name="Status" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                                    <Listeners>
                                        <RowSelect Fn="onRowSelected" />
                                        <%--<RowDeselect Fn="onRowDeselected" />--%>
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                                <Columns>
                                    <ext:Column Header="CustomerID" DataIndex="CustomerID" Wrap="true" Locked="true" Width="100px">
                                    </ext:Column>
                                    <ext:Column Header="Name" DataIndex="Names" Wrap="true" Locked="true" >
                                    </ext:Column>
                                    <ext:Column Header="Address" DataIndex="Addresses" Locked="true" Wrap="true" Hidden="true">
                                    </ext:Column>
                                    <ext:Column Header="Party Type" DataIndex="PartyType" Locked="true" Wrap="true" Hidden="true">
                                    </ext:Column>
                                    <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Hidden="true">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                    DisplayMsg="Displaying customers {0} - {1} of {2}" EmptyMsg="No customers to display" />
                            </BottomBar>
                            <LoadMask ShowMask="true"/>
                        </ext:GridPanel>
                        </Items>
                        </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
                <Listeners>
                    <ClientValidation Handler="onFormValidation(valid)"/>
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
