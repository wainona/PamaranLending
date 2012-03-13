<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerChecksPickList.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.CustomerChecksPickList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Checks Pick List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
        var onMessageReceived = function (msg) {
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
                window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'onpickcheck');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickcheck');
            }
            window.proxy.requestClose();
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="FitLayout">
        <Items>
            <ext:Hidden ID="hiddenResourceGUID" runat="server" />
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                 Layout="FitLayout"
                 Height="600" AutoExpandColumn="TotalAmount">
                <LoadMask ShowMask="true" Msg="Loading..." />
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
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
                            <ext:Parameter Name="limit" Value="24" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="RandomKey">
                                <Fields>
                                    <ext:RecordField Name="BankName" />
                                    <ext:RecordField Name="Branch" />
                                    <ext:RecordField Name="CheckNumber" />
                                    <ext:RecordField Name="CheckType" />
                                    <ext:RecordField Name="_CheckDate" />
                                    <ext:RecordField Name="TotalAmount" />
                                  <%--  <ext:RecordField Name="Status" />--%>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Bank" DataIndex="BankName" Sortable="false" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Branch" DataIndex="Branch" Sortable="false" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                          <ext:Column Header="Check Type" DataIndex="CheckType" Sortable="false" Wrap="true" Locked="true" Width="120px">
                        </ext:Column>
                        <ext:Column Header="Check No." DataIndex="CheckNumber" Sortable="false" Locked="true" Wrap="true" Width="160px">
                        </ext:Column>
                        <ext:Column Header="Check Date" DataIndex="_CheckDate" Sortable="false" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                        <ext:NumberColumn Header="Total Amount" DataIndex="TotalAmount" Sortable="false" Locked="true" Wrap="true"
                            Width="160px" Format=",000.00">
                        </ext:NumberColumn>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="24" DisplayInfo="true"
                        DisplayMsg="Displaying checks {0} - {1} of {2}" EmptyMsg="No checks to display" />
                </BottomBar>
            </ext:GridPanel>
            <ext:Hidden ID="hiddenRandomKey" runat="server" />
            <ext:Hidden ID="hiddenType" runat="server" />
            <ext:Hidden ID="hiddenPatyRoleId" runat="server" />
            <ext:Hidden ID="hiddenBank" runat="server" />
            <ext:Hidden ID="hiddenBranch" runat="server" />
            <ext:Hidden ID="hiddenCheckNumber" runat="server" />
            <ext:Hidden ID="hiddenCheckDate" runat="server" />
            <ext:Hidden ID="hiddenAmount" runat="server" />
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>