<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPickList.aspx.cs" Inherits="LendingApplication.Applications.CustomerUseCases.CustomerPickList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcustomer', 'updatecustomer']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };
        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/FinancialManagement/CustomerUseCases/ViewEditCustomer.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCustomer', param, 'Update Customer');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onBtnAddClick = function () {
            var url = '/Applications/FinancialManagement/CustomerUseCases/ManageCustomer.aspx';
            var id = 'id=3';
            var mode = 'mode=Add';
            var param = url + "?" + id + '&' + mode;
            window.proxy.requestNewTab('AddCustomer', param, 'Add Customer');
        };

        var checkIfAllowed = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                var id = data.id;
                X.CheckIfAllowed(id, {
                    success: function (result) {
                        if (!result) {
                            btnSelect.disable();
                            showAlert('Error', 'Selected customer has no credit limit. Please select another customer or update customer information.');
                        } else {
                            btnSelect_Click();
                            //btnSelect.enable();
                        }
                    }
                });
            }
        }

        var onRowSelected = function () {
            btnSelect.enable();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnSelect.disable();
            }
        };

        var btnSelect_Click = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'onpickcustomer');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickcustomers');
            }
            window.proxy.requestClose();
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
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
                AutoExpandColumn="Addresses">
                <LoadMask ShowMask="true" Msg="Loading Customers.."/>
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No customers to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <%--<Click Handler="checkIfAllowed();" />--%>
                                    <Click Handler="btnSelect_Click();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <%--<ext:ComboBox ID="cmbFilterBy" Width="100px" EmptyText="Filter By..." Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="All" Value="0" />
                                    <ext:ListItem Text="Status" Value="1" />
                                    <ext:ListItem Text="Party Type" Value="2" />
                                </Items>
                                <DirectEvents>
                                    <Select OnEvent="OnChange" />
                                </DirectEvents>
                            </ext:ComboBox>--%>
                            <ext:ComboBox ID="cmbFilterBy2" Width="150px" EmptyText="Search By Status..." Editable="false" runat="server" StoreID="FilterStore"
                                 ValueField="Id" DisplayField="Name" TypeAhead="true" 
                                 Mode="Local" TriggerAction="All" FieldLabel="">
                                <%--<Items>
                                    <ext:ListItem Text="All" Value="-1" />
                                 </Items>--%>
                                <%--<DirectEvents>
                                    <Select OnEvent="OnSelectedIndexChange" />
                                </DirectEvents>   --%>
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();#{PageGridPanel}.reload();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <%--<ext:ComboBox ID="cmbSearchBy" Width="100px" EmptyText="Search By..." Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="ID" Value="0" />
                                    <ext:ListItem Text="Name" Value="1" />
                                </Items>
                            </ext:ComboBox>--%>
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
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="CustomerID" DataIndex="CustomerID" Wrap="true" Locked="true" Width="80px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Names" Wrap="true" Locked="true" Width="200px">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Addresses" Locked="true" Wrap="true" Width="200px">
                        </ext:Column>
                        <ext:Column Header="Party Type" DataIndex="PartyType" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true"
                            Width="140px">
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
