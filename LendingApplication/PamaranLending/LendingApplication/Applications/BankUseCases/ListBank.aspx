<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListBank.aspx.cs" Inherits="LendingApplication.Applications.BankUseCases.ListBank" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Banks List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addbank', 'updatebank', 'changeurltoedit']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag != 'changeurltoedit')
                PageGridPanel.reload();
            else {
                window.proxy.requestClose('AddBank');
                openBank(msg.data.id);
            }
        };
        var openBank = function (id) {
           var url = '/Applications/BankUseCases/EditBank.aspx';
            var id = 'id=' + id;
            var param = url + "?" + id;
            window.proxy.requestNewTab('UpdateBank', param, 'Update Bank');
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/BankUseCases/EditBank.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateBank', param, 'Update Bank');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddBank', '/Applications/BankUseCases/AddBank.aspx', 'Add Bank');
        };

        var onRowSelected = function () {
            btnOpen.enable();
            checkIfCanDelete();
            checkIfCanDeactivate();
            checkIfCanActivate();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                btnActivate.disable();
                btnDeactivate.disable();

            }
        };
        var deleteSuccessful = function () {
            showAlert('Message', 'The selected bank record successfully deleted');
        };
        var deactivateSuccessful = function () {
            showAlert('Message', 'The selected bank record successfully deactivated');
            btnDeactivate.disable();
            btnActivate.enable();
        };
        var activateSuccessful = function () {
            showAlert('Message', 'The selected bank record successfully activated');
            btnActivate.disable();
            btnDeactivate.enable();
        };
        var checkIfCanDelete = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow) {
                X.CanDeleteBank(PageGridPanelSelectionModel.getSelected().id, {
                    success: function (result) {
                        if (result) {
                            btnDelete.enable();
                            Ext.QuickTips.disable();
                        } else {
                            btnDelete.disable();
                            Ext.QuickTips.enable();
                        }
                    }
                });
            }
            return result;
        };

        var checkIfCanDeactivate = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow) {
                X.CanDeactivateBank(selectedRow.id, {
                    success: function (result) {
                        if (result == true)
                            btnDeactivate.enable();
                    }
                });
            }
            return result
        };
        var checkIfCanActivate = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow) {
                X.CanActivateBank(selectedRow.id, {
                    success: function (result) {
                        if (result == true)
                            btnActivate.enable();
                    }
                });
            }
            return result
        };
    </script>

</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" AutoExpandColumn="Address">
            <View>
            <ext:GridView EmptyText="No banks to display"></ext:GridView>
            </View>
            <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                           <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y"></ext:Hidden>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" ToolTip="Selected bank record cannot be deleted." Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="deleteSuccessful()">
                                        <Confirmation BeforeConfirm="checkIfCanDelete();" ConfirmRequest="true" Message="Are you sure you want to delete the selected bank record?"/>
                                        <EventMask ShowMask="true" Msg="Deleting.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeleteSeparator" />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnOpenSeparator" />
                               <ext:Button ID="btnDeactivate" runat="server" Text="Deactivate" Icon="Decline" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDeactivate_Click"  Success="deactivateSuccessful()">
                                        <Confirmation BeforeConfirm="checkIfCanDeactivate" ConfirmRequest="true" Message="Are you sure you want to deactivate this bank record?" />
                                          <EventMask ShowMask="true" Msg="Deactivating.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeactivateSeparator" />
                            <ext:Button ID="btnActivate" runat="server" Text="Activate" Icon="Accept" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnActivate_Click" Success="activateSuccessful()">
                                        <Confirmation BeforeConfirm="checkIfCanActivate" ConfirmRequest="true" Message="Are you sure you want to activate this bank record?" />
                                          <EventMask ShowMask="true" Msg="Activating.." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                                  <ext:ToolbarSeparator ID="btnActivateSeparator" />
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                <Listeners>
                                    <Click Handler="onBtnAddClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <%--<ext:Label ID="Label1" runat="server" Text="Search By:"></ext:Label>--%>
                            <ext:ComboBox ID="cmbFilter" runat="server" Editable="false" EmptyText="Filter By Status..." Width="130">
                                <Items>
                                    <ext:ListItem Text="Active" Value="0" />
                                    <ext:ListItem Text="Inactive" Value="1" />
                                    <ext:ListItem Text="All" Value="-1" />
                                </Items>
                                <%--<Listeners>
                                <Select Handler="FilterSelect();" />
                                </Listeners>--%>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer />
                            <ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search by..." Width="130">
                                <Items>
                                    <ext:ListItem Text="Name" Value="0" />
                                    <ext:ListItem Text="Branch" Value="1" />
                                    <ext:ListItem Text="Address" Value="2" />
                                </Items>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSearch" runat="server">
                            </ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click"></Click>
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
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="Id" />
                                    <ext:RecordField Name="BranchName" />
                                    <ext:RecordField Name="OrganizationName"/>
                                    <ext:RecordField Name="Acronym" />
                                    <ext:RecordField Name="Status"/>
                                    <ext:RecordField Name="Address"/>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100">
                    <Columns>
                        <ext:Column Header="Role Id" DataIndex="Id" Locked="true" Wrap="true" Width="140px"
                            Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="OrganizationName" Width="200px" Locked="true"
                            Wrap="true">
                        </ext:Column>
                        <ext:Column Header="Acronym" DataIndex="Acronym" Wrap="true" Locked="true" Width="80px">
                        </ext:Column>
                        <ext:Column Header="Branch" DataIndex="BranchName" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Address" Wrap="true" Locked="true"
                            Width="140px">
                        </ext:Column>
                        <ext:Column Header="Status" DataIndex="Status" Wrap="true" Locked="true" Width="80px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying banks {0} - {1} of {2}" EmptyMsg="No banks to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
