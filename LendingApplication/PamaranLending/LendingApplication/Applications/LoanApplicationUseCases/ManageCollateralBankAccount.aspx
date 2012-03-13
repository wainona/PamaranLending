<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCollateralBankAccount.aspx.cs"
    Inherits="LendingApplication.ManageCollateralBankAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Edit Asset Type</title>
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
            window.proxy.init(['onpickmortgagee', 'addpropertyowner', 'editpropertyowner', 'onpickbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickmortgagee') {
                hiddenMortgageeId.setValue(msg.data.PartyId);
                txtMortgageeName.setValue(msg.data.Name);
            }
            else if (msg.tag == 'addpropertyowner') {
                X.AddPropertyOwner(msg.data.PartyId, msg.data.Name, msg.data.Address, msg.data.PercentOwned);
            }
            else if (msg.tag == 'editpropertyowner') {
                X.EditPropertyOwner(msg.data.RandomKey, msg.data.PartyId, msg.data.Name, msg.data.Address, msg.data.PercentOwned);
            }
            else if (msg.tag == 'onpickbank') {
                hiddenBankPartyRoleId.setValue(msg.data.Id);
                txtBankName.setValue(msg.data.Name);
            }
        }

        var saveSuccessful = function () {
            window.proxy.sendToAll('managecollateral', 'managecollateral');
            window.proxy.requestClose();
        }

        var onBtnPickMortgagee = function () {
            var url = '/Applications/LoanApplicationUseCases/PickListMortgagee.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickMortgagee', param, 'Select Mortgagee');
        }

        var onBtnPickBank = function () {
            var url = '/Applications/BankUseCases/PickListBank.aspx';
            var param = url + "?mode=" + 'single';
            window.proxy.requestNewTab('PickBank', param, 'Select Bank');
        }

        var onBtnAddPropertyOwner = function () {
            var url = '/Applications/LoanApplicationUseCases/ManagePropertyOwner.aspx';
            var param = url + "?mode=" + 'add';
            window.proxy.requestNewTab('PickPropertyOwner', param, 'Select Property Owner');
        }

        var onBtnEditPropertyOwner = function () {
            if (GridPanelPropertyOwners.hasSelection() == true) {
                var url = '/Applications/LoanApplicationUseCases/ManagePropertyOwner.aspx';
                var param = url + "?mode=" + 'edit';
                var data = SelectionModelPropertyOwners.getSelected().json;
                param += "&RandomKey=" + data.RandomKey;
                param += "&PartyId=" + data.PartyId;
                param += "&Name=" + data.Name;
                param += "&Address=" + data.Address;
                param += "&PercentOwned=" + data.PercentOwned;
                window.proxy.requestNewTab('PickPropertyOwner', param, 'Select Property Owner');
            }
        }

        var OnChkIsMortgagedChanged = function () {
            if (chkMortgaged.getValue() == true) {
                btnPickMortgagee.enable();
                txtMortgageeName.enable();
                txtMortgageeName.allowBlank = false;

            }
            else {
                btnPickMortgagee.disable();
                txtMortgageeName.disable();
                txtMortgageeName.allowBlank = true;
            }
            txtMortgageeName.validate();
        };

        var onRowSelected = function () {
            btnEditPropertyOwner.enable();
            btnDeletePropertyOwner.enable();
        };
        var onRowDeselected = function () {
            if (GridPanelPropertyOwners.hasSelection() == false) {
                btnEditPropertyOwner.disable();
                btnDeletePropertyOwner.disable();
            }
        };

        var onFormValidated = function (valid) {
            //this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && #{GridPanelPropertyOwners}.store.getCount()){#{btnSave}.enable();}  else{#{btnSave}.disable();}
            var count = GridPanelPropertyOwners.store.getCount();
            var total = 0;
            for (var j = 0; j < GridPanelPropertyOwners.store.getCount(); j++) {
                var data = GridPanelPropertyOwners.store.getAt(j);
                total = total + data.get('PercentOwned');
            }
            valid = valid && (count > 0) && (total <= 100);

            if (valid) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ', iconCls: 'icon-accept' });
                btnSave.enable();
            }
            else if (count == 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Please add property owner.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
            else if (total > 100) {
                PageFormPanelStatusBar.setStatus({ text: 'Total percentage of owned asset exceeds 100.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please completely fill out the forms.', iconCls: 'icon-exclamation' });
                btnSave.disable();
            }
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
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 20px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden runat="server" ID="hiddenMode">
    </ext:Hidden>
    <ext:Hidden runat="server" ID="hiddenRandomKey">
    </ext:Hidden>
    <ext:Store runat="server" ID="storeBankAccountType" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" MonitorValid="true" Title="Collateral - Bank Account"
                BodyStyle="background-color:transparent" LabelWidth="200" Padding="5"  MonitorPoll="500">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="OK" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                        Success="saveSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancel" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="onBtnClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TextField runat="server" ID="txtCollateralType" ReadOnly="true" Text="Bank Account"
                        FieldLabel="Collateral Type" Width="400" />
                    <ext:RadioGroup ID="rgMortgage" runat="server" FieldLabel="Is the property mortgaged?"
                        Width="200" Visible="false">
                        <Items>
                            <ext:Radio runat="server" ID="chkMortgaged" BoxLabel="Yes">
                                <Listeners>
                                    <Check Handler="OnChkIsMortgagedChanged();" />
                                </Listeners>
                            </ext:Radio>
                            <ext:Radio runat="server" ID="chkNotMortgaged" BoxLabel="No" Checked="true">
                                <Listeners>
                                    <Check Handler="OnChkIsMortgagedChanged();" />
                                </Listeners>
                            </ext:Radio>
                        </Items>
                    </ext:RadioGroup>
                    <ext:Hidden runat="server" ID="hiddenMortgageeId" />
                    <ext:CompositeField ID="cfMortgagee" DataIndex="" runat="server" FieldLabel="Mortgagee"
                        Width="500" Visible="false">
                        <Items>
                            <ext:TextField ID="txtMortgageeName" runat="server" ReadOnly="true" Width="400" />
                            <ext:Button ID="btnPickMortgagee" runat="server" Text="Browse..." Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnPickMortgagee();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:ComboBox runat="server" ID="cmbBankAccountType" FieldLabel="Bank Account Type"
                        Width="400" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                        TriggerAction="All" SelectOnFocus="true" AllowBlank="false" ValueField="Id" DisplayField="Name"
                        StoreID="storeBankAccountType" />
                    <ext:Hidden ID="hiddenBankPartyRoleId" runat="server" />
                    <ext:CompositeField ID="cfBank" runat="server" Width="500">
                        <Items>
                            <ext:TextField ID="txtBankName" runat="server" FieldLabel="Bank" ReadOnly="true" Width="400" AllowBlank="false" />
                            <ext:Button ID="btnPickBank" runat="server" Text="Browse...">
                                <Listeners>
                                    <Click Handler="onBtnPickBank();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:TextField runat="server" ID="txtAccountNumber" MaxLength="25" AllowBlank="false"
                        Width="400" FieldLabel="Bank Account Number" MaskRe="[a-zA-Z0-9\- ]" MsgTarget="Side">
                    </ext:TextField>
                    <ext:TextField runat="server" ID="txtAccountName" AllowBlank="false" Width="400"
                        FieldLabel="Bank Account Name" MsgTarget="Side">
                    </ext:TextField>
                    <ext:TextArea runat="server" ID="txtCollateralDesc" Width="400" FieldLabel="Collateral Description">
                    </ext:TextArea>
                    <ext:GridPanel ID="GridPanelPropertyOwners" Title="Property Owner/s" runat="server"
                        Height="290" AutoExpandColumn="Address">
                        <View>
                            <ext:GridView runat="server" EmptyText="No property owners to display" DeferEmptyText="false"></ext:GridView>
                        </View>
                        <Items>
                            <ext:Toolbar ID="ToolbarPropertyOwners" runat="server">
                                <Items>
                                    <ext:Button ID="btnDeletePropertyOwner" runat="server" Text="Delete" Icon="Delete"
                                        Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDeletePropertyOwner_Click" />
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button ID="btnEditPropertyOwner" runat="server" Text="Edit" Icon="NoteEdit"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnEditPropertyOwner();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator>
                                    </ext:ToolbarSeparator>
                                    <ext:Button ID="btnAddPropertyOwner" runat="server" Text="Add" Icon="Add">
                                        <Listeners>
                                            <Click Handler="onBtnAddPropertyOwner();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                        <Store>
                            <ext:Store ID="StorePropertyOwner" runat="server">
                                <Reader>
                                    <ext:JsonReader runat="server" IDProperty="RandomKey">
                                        <Fields>
                                            <ext:RecordField Name="PartyId" />
                                            <ext:RecordField Name="Name" />
                                            <ext:RecordField Name="Address" />
                                            <ext:RecordField Name="PercentOwned" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="SelectionModelPropertyOwners" SingleSelect="true" runat="server">
                                <Listeners>
                                    <RowSelect Handler="onRowSelected();" />
                                    <RowDeselect Handler="onRowDeselected();" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server" Header="Party ID" DataIndex="PartyId" Width="150">
                                </ext:Column>
                                <ext:Column runat="server" Header="Name" DataIndex="Name" Width="150">
                                </ext:Column>
                                <ext:Column runat="server" Header="Address" DataIndex="Address" Width="150">
                                </ext:Column>
                                <ext:Column runat="server" Header="Percent Owned" DataIndex="PercentOwned" Width="150">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                    </ext:GridPanel>
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
