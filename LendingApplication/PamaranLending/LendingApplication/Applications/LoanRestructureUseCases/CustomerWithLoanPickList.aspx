<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerWithLoanPickList.aspx.cs" Inherits="LendingApplication.Applications.LoanRestructureUseCases.CustomerWithLoanPickList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };
   
      

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
                <LoadMask ShowMask="true" Msg="Loading..." />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" LabelWidth="50">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelect_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <%--<ext:ComboBox ID="cmbFilterBy" Width="150px" FieldLabel="Filter By" EmptyText="Filter By..." Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="All" Value="0" />
                                    <ext:ListItem Text="Status" Value="1" />
                                    <ext:ListItem Text="Party Type" Value="2" />
                                </Items>
                                <DirectEvents>
                                    <Select OnEvent="OnChange" />
                                </DirectEvents>
                            </ext:ComboBox>--%>
                            <ext:ComboBox ID="cmbFilterBy2" Width="150px" EmptyText="Search By Status..." Hidden="false" Editable="false" runat="server" StoreID="FilterStore"
                                 ValueField="Id" DisplayField="Name" TypeAhead="true" 
                                 Mode="Local" TriggerAction="All" >
                                 <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue(); #{btnSearch}.fireEvent('click');" />
                                </Listeners>     
                            </ext:ComboBox>
                            <ext:ComboBox ID="cmbSearchBy" Width="100px" EmptyText="Search By..." Hidden="true" Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="ID" Value="0" />
                                    <ext:ListItem Text="Name" Value="1" />
                                </Items>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <ext:TextField ID="txtSearchKey" EmptyText="Search Here..." FieldLabel="Name" runat="server">
                                
                            </ext:TextField>
                            <ext:Button ID="btnSearch" Icon="Find" runat="server" Text="Search">
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
                                    <ext:RecordField Name="PartyTypes" />
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
                        <ext:Column Header="Party Type" DataIndex="PartyTypes" Locked="true" Wrap="true"
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
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
