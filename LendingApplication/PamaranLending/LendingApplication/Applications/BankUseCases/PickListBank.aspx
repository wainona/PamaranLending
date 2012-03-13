<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListBank.aspx.cs"
    Inherits="LendingApplication.PickListBank" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addbank', 'updatebank', 'changeurltoedit']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
        };

        var btnSelectClick = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(data.json, 'onpickbank');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickbanks');
            }
            window.proxy.requestClose();
        };

        var onRowSelected = function () {
            btnSelect.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnSelect.disable();
            }
        };

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <%--ViewPort--%>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" AutoExpandColumn="Address">
            <LoadMask ShowMask="true" />
             <View>
            <ext:GridView EmptyText="No banks to display"></ext:GridView>
            </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelectClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search by...">
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
                            <ext:ComboBox ID="cmbFilter" runat="server" Editable="false" EmptyText="Filter by...">
                                <Items>
                                    <ext:ListItem Text="Active" Value="0" />
                                    <ext:ListItem Text="Inactive" Value="1" />
                                    <ext:ListItem Text="All" Value="-1" />
                                </Items>
                                <DirectEvents>
                                <Select OnEvent="cmbFilter_Select"></Select>
                                </DirectEvents>
                            </ext:ComboBox>
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
                                    <ext:RecordField Name="Acronym" />
                                    <ext:RecordField Name="Name"/>
                                    <ext:RecordField Name="Status"/>
                                    <ext:RecordField Name="Address"/>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" SingleSelect="true" runat="server">
                        <Listeners>
                            <RowSelect Handler="onRowSelected();" />
                            <RowDeselect Handler="onRowDeselected();" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100">
                    <Columns>
                        <ext:Column Header="Role Id" DataIndex="Id" Locked="true" Wrap="true" Width="140px"
                            Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Name" Width="140px" Locked="true"
                            Wrap="true" Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Acronym" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Branch" DataIndex="BranchName" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Status" DataIndex="Status" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Address" Wrap="true" Locked="true"
                            Width="140px">
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
