<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistrictTypes.aspx.cs" Inherits="LendingApplication.Applications.CustomerClassificationUseCases.DistrictTypes" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>District Type List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcustomerclassification', 'updatecustomerclassification']);
            window.proxy.on('messagereceived', onMessageReceived);

        });
        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/CustomerClassificationUseCases/EditCustomerClassification.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCustomerClassification', param, 'Update Customer Classification');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBeforeBtnDeleteClick = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow)
                X.CanDeleteDistrictType(PageGridPanelSelectionModel.getSelected().id, {
                    success: function (result) {
                        if (result) {
                            Ext.QuickTips.disable();
                            btnDelete.enable();
                        }
                        else {
                            btnDelete.disable();
                            Ext.QuickTips.enable();
                        }
                    }
                });
        }

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddCustomerClassification', '/Applications/CustomerClassificationUseCases/AddCustomerClassification.aspx', 'Add Customer Classification');
        };

        var onRowSelected = function () {
            btnOpen.enable();
            onBeforeBtnDeleteClick();
            Ext.QuickTips.disable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                Ext.QuickTips.disable();
            }
        };
        var success = function () {
            showAlert('Status', 'Customer Classification record was successfully deleted.');
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                Ext.QuickTips.disable();
            }
        };

        var onDistrictTypeValidation = function () {
            X.CheckExistence({
                success: function (result) {
                    if (result) {
                        btnSaveNewDistrictType.disable();
                        StatusBarDistrictType.setStatus({ text: 'District Type already exists.', iconCls: 'icon-exclamation' });
                    } else if (!result && txtDistrictType.getValue() != '') {
                        StatusBarDistrictType.setStatus({ text: '', iconCls: 'icon-exclamation' });
                        btnSaveNewDistrictType.enable();
                    } else {
                        btnSaveNewDistrictType.disable();
                    }
                }
            });
        };

        var clearDistrictType = function () {
            txtDistrictType.setValue('');
            newDistrictType.hide();
            StatusBarDistrictType.setStatus({ text: '', iconCls: 'icon-exclamation' });
        }

        var saveDistrictType = function () {
            showAlert('Status', 'Successfully added district type record.', function () {
                window.proxy.sendToAll('adddistricttype', 'adddistricttype');
                PageGridPanel.reload();
                clearDistrictType();
                newDistrictType.hide();
            });
        }


        var FillDistrictType = function () {
            newDistrictType.show();
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            var id = selectedRow.json.Id;
            var name = selectedRow.json.Name;
            hdnDistrictTypeId.setValue(id);
            txtDistrictType.setValue(name);
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
        .x-grid-empty {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden runat="server" ID="hdnDistrictTypeId" Text="-1"></ext:Hidden>
    <ext:Viewport ID="Viewport1" runat="server" Layout="FitLayout"><Items>
    <ext:FormPanel ID="PageFormPanel" 
        runat="server" 
        ButtonAlign="Right" 
        MonitorValid="true"
        Title="District Types"
        BodyStyle="background-color:transparent"
        Layout="FitLayout" Height="50">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                Layout="Fit">
                <View>
                <ext:GridView EmptyText="No district type to display" DeferEmptyText="false">
                </ext:GridView>
                </View>
                <LoadMask ShowMask="true" Msg="Loading District Types.."/>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true" ToolTip="The selected customer classification record/s cannot be deleted.">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="success();">
                                        <Confirmation BeforeConfirm="onBeforeBtnDeleteClick" ConfirmRequest="true" Message="Are you sure you want to delete the customer classification record/s?" />
                                    <EventMask ShowMask="true" Msg="Deleting" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="FillDistrictType();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnAddDistrictType" runat="server" Text="Add" Icon="Add">
                                <Listeners>
                                    <Click Handler="#{newDistrictType}.show(); #{newDistrictType}.doLayout();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <%--<ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search district type by..">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Items>
                                    <ext:ListItem Value="Name" Text="District"/>
                                </Items>
                                 <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ToolbarSeparator></ext:ToolbarSeparator>--%>
                            <ext:TextField ID="txtSearch" runat="server"></ext:TextField>
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
                            <ext:Parameter Name="limit" Value="10" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
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
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Id" Width="280" DataIndex="Id" Wrap="true" Locked="true">
                        </ext:Column>
                        <ext:Column Header="Name" Width="280" DataIndex="Name" Locked="true" Wrap="true">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                        DisplayMsg="Displaying district types {0} - {1} of {2}" EmptyMsg="No district type to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:FormPanel>
    </Items></ext:Viewport>

    <ext:Window runat="server" ID="newDistrictType" Collapsible="true" Height="115" Icon="Application"
        Title="New District Type" Width="370" Hidden="true" Modal="true" Resizable="false" AutoFocus="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="fpNewDistrictType" runat="server" Padding="5" LabelWidth="100" MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtDistrictType" FieldLabel="District Type" EnableKeyEvents="true" Width="220">
                        <Listeners>
                            <KeyUp Handler="onDistrictTypeValidation();"/>
                        </Listeners>
                    </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBarDistrictType" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveNewDistrictType" runat="server" Text="Save" Icon="Disk" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnSaveNewDistrictType_Click" Success="saveDistrictType();">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancelDistrictType" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="clearDistrictType();"/>
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>