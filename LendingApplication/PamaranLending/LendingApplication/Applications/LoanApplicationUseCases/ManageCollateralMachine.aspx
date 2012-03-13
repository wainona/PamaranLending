<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCollateralMachine.aspx.cs"
    Inherits="LendingApplication.ManageCollateralMachine" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Manage Machine</title>
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
            window.proxy.init(['onpickmortgagee', 'addpropertyowner', 'editpropertyowner']);
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
            if (chkIsMortgaged.getValue() == true) {
                btnPickMortgagee.enable();
                txtMortgageeName.enable();
                txtMortgageeName.allowBlank = false;

            }
            else {
                btnPickMortgagee.disable();
                txtMortgageeName.disable();
                txtMortgageeName.allowBlank = true;
            }
            PageFormPanel.validate();
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
        var onBtnClose = function () {
            showConfirm('Confirm', 'Are you sure you want to close the tab?', function (btn) {
                if (btn == 'yes') {
                    window.proxy.requestClose();
                }
            });
        }
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden runat="server" ID="hiddenMode"></ext:Hidden>
    <ext:Hidden runat="server" ID="hiddenRandomKey"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="PageFormPanel" runat="server" MonitorValid="true" Title="Collateral - Machine"
                BodyStyle="background-color:transparent" LabelWidth="150" Padding="5"  MonitorPoll="500">
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
                    <ext:TextField runat="server" ID="txtCollateralType" ReadOnly="true" Text="Machine"
                        FieldLabel="Collateral Type" Width="400" />
                    <ext:Checkbox runat="server" ID="chkIsMortgaged" BoxLabel="Is the property mortgaged?"
                        Width="400" Hidden="true">
                        <Listeners>
                            <Check Handler="OnChkIsMortgagedChanged();" />
                        </Listeners>
                    </ext:Checkbox>
                    <ext:Hidden runat="server" ID="hiddenMortgageeId"/>
                    <ext:CompositeField ID="cfMortgagee" DataIndex="" runat="server" FieldLabel="Mortgagee"
                        Width="500" Hidden="true">
                        <Items>
                            <ext:TextField ID="txtMortgageeName" runat="server" ReadOnly="true" Width="400" />
                            <ext:Button ID="btnPickMortgagee" runat="server" Text="Browse..." Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnPickMortgagee();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:CompositeField>
                    <ext:TextField runat="server" ID="txtMachineName" FieldLabel="Machine Name"
                         Width="400" MaxLength="50" AllowBlank="false"/>
                    <ext:TextField runat="server" ID="txtBrandName" FieldLabel="Brand Name" Width="400" 
                        MaxLength="50" AllowBlank="false"/>
                    <ext:TextField runat="server" ID="txtModel" FieldLabel="Model" Width="400" 
                        MaxLength="50" AllowBlank="false"/>
                    <ext:TextField runat="server" ID="txtCapacity" FieldLabel="Capacity" Width="400" 
                        MaxLength="50"/>
                    <ext:NumberField runat="server" ID="nfAcquisitionCost" FieldLabel="Acquisition Cost (Php)" MinValue="0" 
                        DecimalPrecision="2" DecimalSeparator="," AllowNegative="false" Width="400" AllowBlank="false"/>
                    <ext:TextArea runat="server" ID="txtCollateralDesc" Width="400" FieldLabel="Collateral Description">
                    </ext:TextArea>
                    <ext:GridPanel ID="GridPanelPropertyOwners" Title="Property Owner/s" runat="server" Height="265"
                     AutoExpandColumn="Address">
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
