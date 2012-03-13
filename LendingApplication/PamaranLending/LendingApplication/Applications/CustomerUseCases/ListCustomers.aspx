<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCustomers.aspx.cs" Inherits="LendingApplication.Applications.CustomerUseCases.ListCustomers" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcustomer', 'updatecustomer', 'changeurltoedit', 'approvedloanapplication', 'onunderlitigation', 'loanapplicationstatusupdate']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag != 'changeurltoedit') {
                PageGridPanel.reload();
            } else {
                window.proxy.requestClose('AddCustomer');
                openCustomer(msg.data.id);
            }
        };

        var openCustomer = function (ids) {
            var url = '/Applications/CustomerUseCases/ViewOrEditCustomer.aspx';
            var idz = 'id=' + ids;
            var param = url + "?" + idz;
            window.proxy.requestNewTab('UpdateCustomer'+ids, param, 'Update Customer');
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/CustomerUseCases/ViewOrEditCustomer.aspx';
                var ids = selectedRows[0].id;
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCustomer'+ids, param, 'Update Customer');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnAddClick = function () {
            var url = '/Applications/CustomerUseCases/ManageCustomer.aspx';
            var id = 'id=3';
            var param = url;
            window.proxy.requestNewTab('AddCustomer', param, 'Add Customer');
        };

        var onRowSelected = function () {
            btnDelete.enable();
            btnOpen.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
            }
        };

    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hdnLogInId" runat="server" />
    <ext:Store runat="server" ID="FilterStore" RemoteSort="false">
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
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                AutoExpandColumn="Addresses" LabelWidth="50">
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No customers to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onRowDeselected();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected customers?" />
                                        <EventMask Msg="Deleting.." ShowMask="true"/>
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator  ID="btnDeleteSeparator"/>
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnOpenSeparator"/>
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                            <Listeners>
                            <Click Handler="onBtnAddClick();" />
                            </Listeners>
                            </ext:Button>
                                 <%--<Menu>
                                    <ext:Menu runat="server">
                                        <Items>
                                        <ext:MenuItem runat="server" Text="Person" Handler="onBtnAddClick"></ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>--%>
                            <ext:ToolbarFill />
                            <%--<ext:ComboBox ID="cmbFilterBy" Width="150" EmptyText="Filter By..." FieldLabel="Filter By" Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="All" Value="0" />
                                    <ext:ListItem Text="Status" Value="1" />
                                    <ext:ListItem Text="Party Type" Value="2" />
                                </Items>
                                <DirectEvents>
                                    <Select OnEvent="OnChange" />
                                </DirectEvents>  
                            </ext:ComboBox>--%>
                            <ext:ComboBox ID="cmbFilterBy2" Width="150" Hidden="false" EmptyText="Search By Status..." Editable="false" runat="server" StoreID="FilterStore"
                                 ValueField="Id" DisplayField="Name" ForceSelection="true" TypeAhead="true" 
                                 Mode="Local" TriggerAction="All" >
                                 <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue(); #{btnSearch}.fireEvent('click');" />
                                </Listeners>   
                            </ext:ComboBox>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <%--<ext:ComboBox ID="cmbSearchBy" Width="150" Editable="false" FieldLabel="Search By" runat="server" SelectedIndex="0">
                                <Items>
                                    <ext:ListItem Text="Name" Value="1" />
                                </Items>
                            </ext:ComboBox>--%>
                            <ext:TextField ID="txtSearchKey" EmptyText="Search Here..." runat="server" FieldLabel="Name" LabelWidth="35">
                            </ext:TextField>
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
                                    <ext:RecordField Name="CustomerType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="CustomerID" DataIndex="CustomerID" Wrap="true" Locked="true" Width="80px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Names" Wrap="true" Locked="true" Width="180px">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Addresses" Locked="true" Wrap="true" Width="200px">
                        </ext:Column>
                        <ext:Column Header="Customer Type" DataIndex="CustomerType" Locked="true" Wrap="true"
                            Width="120px">
                        </ext:Column>
                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true"
                            Width="100px">
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
    </ext:Viewport>
    </form>
</body>
</html>
