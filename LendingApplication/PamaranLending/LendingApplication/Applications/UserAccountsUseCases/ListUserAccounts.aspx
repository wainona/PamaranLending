<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUserAccounts.aspx.cs" Inherits="LendingApplication.Applications.UserAccountsUseCases.ListUserAccounts" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Account List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['adduseraccount', 'updateuseraccount', 'activateuseraccountsuccess', 'deactivateuseraccountsuccess']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };
        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {
                var url = '/Applications/UserAccountsUseCases/EditUserAccount.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateUserAccountType', param, 'Update User Account Type');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnChangePasswordClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/UserAccountsUseCases/ChangePasswordUserAccount.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('ChangePasswordUserAccount', param, 'Change Password User Account');
            }
        };

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('CreateUserAccount', '/Applications/UserAccountsUseCases/CreateUserAccount.aspx', 'Create User Account');
        };

        var onRowSelected = function () {
            enableOrDisableButtons();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnOpen.disable();
            }
        };

        var enableOrDisableButtons = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            btnActivate.disable();
            btnDeactivate.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json;
                if (CanDeactivate(json.UserAccountStatus)) {
                    btnActivate.disable();
                    btnDeactivate.enable();
                    btnChangePassword.enable();
                    btnOpen.enable();
                } else {
                    btnActivate.enable();
                    btnDeactivate.disable();
                    btnChangePassword.disable();
                    btnOpen.disable();
                }
            }
        };

        var CanDeactivate = function (status) {
            if (status == 'Active') {
                return true;
            }
            return false;
        };

        var activateDeactivateSuccess = function () {
            PageGridPanel.reload();
            PageGridPanelSelectionModel.clearSelections();
            btnOpen.disable();
            btnActivate.disable();
            btnDeactivate.disable();
            btnChangePassword.disable();
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel Border="false"
                ID="PageGridPanel" runat="server" Layout="Fit" EnableColumnHide="false">
                <View>
                    <ext:GridView EmptyText="No user accounts to display." />
                </View>
                <LoadMask ShowMask="true" Msg="Loading User Accounts.."/>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <%-------UPDATE ACCOUNT TYPE BUTTON------%>
                                    <ext:Button ID="btnOpen" runat="server" Text="Update User Account Type" Icon="NoteEdit" Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnOpenClick();"/>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <ext:Button ID="btnChangePassword" runat="server" Text="Change Password" Icon="KeyGo" Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnChangePasswordClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server"/>
                                    <%------ACTIVATE BUTTON------%>
                                    <ext:Button ID="btnActivate" runat="server" Text="Activate" Icon="Accept">
                                        <DirectEvents>
                                            <Click OnEvent="btnActivate_Click" Success="activateDeactivateSuccess">
                                                <EventMask Msg="Activating.." ShowMask="true" />
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to activate the selected user account?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <%------DEACTIVATE BUTTON------%>
                                    <ext:Button ID="btnDeactivate" runat="server" Text="Deactivate" Icon="Decline">
                                        <DirectEvents>
                                            <Click OnEvent="btnDeactivate_Click" Success="activateDeactivateSuccess">
                                                <EventMask Msg="Deactivating.." ShowMask="true" />
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to deactivate the selected user account?" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSeparator runat="server" />
                                    <%------NEW BUTTON------%>
                                    <ext:Button ID="btnAdd" runat="server" Text="New" Icon="Add">
                                        <Listeners>
                                            <Click Handler="onBtnAddClick();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <%------SEARCH BY COMBO BOX------%>
                                    <ext:ComboBox ID="cmbSearchBy" runat="server" EmptyText="Search By.." Editable="false" Width="150">
                                        <Items>
                                            <ext:ListItem Text="Name of User" Value="Name of User" />
                                            <ext:ListItem Text="Username" Value="Username" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="this.clearValue(); #{txtSearchInput}.reset();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <%------SEARCH INPUT TEXT------%>
                                    <ext:ToolbarSpacer Width="5" />
                                    <ext:TextField ID="txtSearchInput" runat="server" EmptyText="type here.."/>
                                    <ext:ToolbarSpacer Width="5" />
                                    <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                        <DirectEvents>
                                            <Click OnEvent="btnSearch_Click">
                                                <EventMask Msg="Searching.." ShowMask="true" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarFill runat="server"/>
                                    <%------FILTER BY USER TYPE------%>
                                    <ext:ComboBox ID="cmbFilterByUserType" runat="server" Editable="false" EmptyText="Filter by User Type.." DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store runat="server" ID="strFilterByUserType">
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="Id" />
                                                            <ext:RecordField Name="Name" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                         <Triggers>
                                            <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="this.clearValue();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer runat="server" Width="5"/>
                                    <%------FILTER BY STATUS------%>
                                    <ext:ComboBox ID="cmbFilterByStatus" runat="server" ValueField="Id" DisplayField="Name" Editable="false" EmptyText="Filter By Status..">
                                        <Store>
                                            <ext:Store runat="server" ID="strFilterByStatus">
                                                <Reader>
                                                    <ext:JsonReader>
                                                        <Fields>
                                                            <ext:RecordField Name="Id" />
                                                            <ext:RecordField Name="Name" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="this.clearValue();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <%-- BUTTON APPLY FILTERS--%>
                                    <ext:Button ID="btnApplyFilters" runat="server" Text="Apply Filters" Icon="Accept">
                                        <Listeners>
                                            <Click Handler="#{PageGridPanel}.reload();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
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
                            <ext:Parameter Name="limit" Value="22" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="UserAccountId">
                                <Fields>
                                    <ext:RecordField Name="UserAccountId" />
                                    <ext:RecordField Name="NameOfUser" />
                                    <ext:RecordField Name="Username" />
                                    <ext:RecordField Name="UserAccountType" />
                                    <ext:RecordField Name="UserAccountStatus" />
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
                        <ext:Column Header="User Account ID" DataIndex="UserAccountId" Wrap="true" Locked="true" Width="140" />
                        <ext:Column Header="Name of the User" DataIndex="NameOfUser" Locked="true" Wrap="true" Width="230" />
                        <ext:Column Header="Username" DataIndex="Username" Locked="true" Wrap="true" Width="140" />
                        <ext:Column Header="User Account Type" DataIndex="UserAccountType" Locked="true" Wrap="true" Width="140" />
                        <ext:Column Header="User Account Status" DataIndex="UserAccountStatus" Locked="true" Wrap="true"  Width="140" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="22" DisplayInfo="true"
                        DisplayMsg="Displaying user accounts {0} - {1} of {2}" EmptyMsg="No user accounts to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
