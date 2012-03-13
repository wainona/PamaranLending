<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListFinancialProducts.aspx.cs" Inherits="LendingApplication.ListFinancialProducts" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addfinancialproduct', 'updatefinancialproduct']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
        };

        var OnBeforeDelete = function () {
            var result = false;
            var ids = [];
            var selectedRows = PageGridPanelSelectionModel.getSelections();

            for (var i = 0; i < selectedRows.length; i++) {
                ids.push(selectedRows[i].id);
            }

            if (PageGridPanel.hasSelection())
                X.CanDeleteFinancialProduct(ids, {
                    success: function (result) {
                        if (result) {
                            btnDelete.enable();
                            Ext.QuickTips.disable();
                        }
                        else {
                            btnDelete.disable();
                            Ext.QuickTips.enable();
                        }
                    }
                });
            return result;
        }

        var onDelete = function () {
            showAlert('Status', 'Successfully deleted the selected financial product record.', function (btn) {
                if (btn == 'ok') {
                    window.proxy.sendToAll('deletedfinancialproduct', 'deletedfinancialproduct');
                }
            });
            onRowDeselected();
        }

        var updateStatus = function () {
            showAlert('Status', 'Successfully updated the status financial product record.', function (btn) {
                if (btn == 'ok') {
                    window.proxy.sendToAll('updatefinancialproduct', 'updatefinancialproduct');
                }
            });
        }

        var OnBeforeEdit = function () {
            var result = false;
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (PageGridPanel.hasSelection()) {
                if (selectedRows[0].json != 'Retired')
                    btnEdit.enable();
                else
                    btnEdit.disable();
            }
            return result;
        }

        var canChangeStatusTo = function (statusFrom, statusTo) {
            if (statusFrom == 'Active') {
                return statusTo != 'Active';
            }

            if (statusFrom == 'Inactive') {
                return statusTo == 'Active';
            }
            else {
                return false;
            }
        }

        var enableOrDisableButtons = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            btnActivateProduct.disable();
            btnDeactivate.disable();
            btnRetire.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json
                if(canChangeStatusTo(json.Status, 'Active'))
                    btnActivateProduct.enable();
                if(canChangeStatusTo(json.Status, 'Inactive'))
                    btnDeactivate.enable();
                if(canChangeStatusTo(json.Status, 'Retired'))
                    btnRetire.enable();
            }
        }

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/BestPractice/ViewOrEditFinancialProduct.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateFinancialProduct', param, 'Update Financial Product');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onRowSelected = function () {
            OnBeforeDelete();
            OnBeforeEdit();
            enableOrDisableButtons();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnEdit.disable();
                btnActivateProduct.disable();
                btnDeactivate.disable();
                btnRetire.disable();
            }
            else {
                onRowSelected();
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <%--Store--%>
    <ext:Store runat="server" ID="FilterByStore" RemoteSort="false">
            <Reader>
                <ext:JsonReader IDProperty="Id">
                    <Fields>
                        <ext:RecordField Name="Id" />
                        <ext:RecordField Name="Name" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <%--ViewPort--%>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                AutoExpandColumn="Name"
                Layout="Fit">
                <View>
                    <ext:GridView runat="server" EmptyText="No financial products to display" DeferEmptyText="false"></ext:GridView>
                </View>
                <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Disabled="true" 
                                Icon="Delete" ToolTip="Product is not Active or their are records that depend on the selected record.">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onDelete();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected loan products?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeleteSeparator"/>  
                            <ext:Button ID="btnEdit" runat="server" Text="Open" Disabled="true" Icon="NoteEdit" ToolTip="Product can only be edited if Product Status is Active or Inactive.">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnEditSeparator"/>
                            <ext:Button ID="btnNew" runat="server" Text="New" Icon="Add">
                                <Listeners>
                                    <Click Handler="window.proxy.requestNewTab('AddFinancialProduct', '/BestPractice/AddFinancialProduct.aspx', 'Add Financial Product');" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnNewSeparator"/>
                            <ext:Button ID="btnActivateProduct" runat="server" Text="Activate" Disabled="true" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnActivate_Click" Success="updateStatus();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to activate this product?"/>
                                        <EventMask ShowMask="true" Msg="Updating product status..."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnActivateSeparator"/>
                            <ext:Button ID="btnDeactivate" runat="server" Text="Deactivate" Disabled="true" Icon="Decline">
                                <DirectEvents>
                                    <Click OnEvent="btnDeactivate_Click" Success="updateStatus();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to deactivate this product?" />
                                        <EventMask ShowMask="true" Msg="Updating product status..."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeactivateSeparator"/>
                            <ext:Button ID="btnRetire" runat="server" Text="Retire" Disabled="true" Icon="Lock">
                                <DirectEvents>
                                    <Click OnEvent="btnRetire_Click" Success="updateStatus();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to retire this product?" />
                                        <EventMask ShowMask="true" Msg="Updating product status..."/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:ComboBox ID="cmbFilterBy" runat="server" EmptyText="Search by status.." AnchorHorizontal="100%" ValueField="Id" DisplayField="Name"
                                ForceSelection="true" StoreID="FilterByStore" Editable="false">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ToolbarSeparator></ext:ToolbarSeparator>
                            <ext:TextField ID="txtSearch" runat="server" EmptyText="Search here.." FieldLabel="Name" LabelWidth="35"></ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="SearchProductList_Click"></Click>
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
                            <Load Handler="enableOrDisableButtons();" />
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="ID">
                                <Fields>
                                    <ext:RecordField Name="ID" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="IntroductionDate"/>
                                    <ext:RecordField Name="SalesDiscontinuationDate"/>
                                    <ext:RecordField Name="Status" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" SingleSelect="false" runat="server">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                    <Columns>
                        <ext:Column Header="ID" DataIndex="ID" Wrap="true" Locked="true" Width="100"/>
                        <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" Width="200"/>
                        <ext:Column Header="Introduction Date" DataIndex="IntroductionDate" Locked="true" Wrap="true"
                            Width="200" />
                        <ext:Column Header="Sales Discontinuation Date" DataIndex="SalesDiscontinuationDate" 
                            Locked="true" Wrap="true" Width="200" />
                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true"
                            Width="100" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying financial product {0} - {1} of {2}" EmptyMsg="No financial product to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
