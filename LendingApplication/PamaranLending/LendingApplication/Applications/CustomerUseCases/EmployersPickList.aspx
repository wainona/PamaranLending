<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployersPickList.aspx.cs" Inherits="LendingApplication.Applications.CustomerUseCases.EmployersPickList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addemployer', 'updateemployer', 'changeurltoedit']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
            if (msg.tag != 'changeurltoedit') {
                PageGridPanel.reload();
            } else {
                window.proxy.requestClose('AddEmployer');
                openEmployer(msg.data.id);
            }
        };

        var openEmployer = function (ids) {
            var url = '/Applications/CustomerUseCases/ViewOrEditEmployer.aspx';
            var id = 'id=' + ids;
            var param = url + "?" + id;
            window.proxy.requestNewTab('UpdateEmployer', param, 'Update Employer');
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url = '/Applications/CustomerUseCases/ViewOrEditEmployer.aspx';
                var id = 'id=' + selectedRows[0].id;
                var mode = 'mode=Edit';
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateEmployer', param, 'Update Employer');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnAddClick = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow)
                X.CheckBeforeAdd(PageGridPanelSelectionModel.getSelected().id, {
                    success: function (result) {
                        if (result) {
                            Ext.ConfirmationBox.alert();
                            showAlert('Status', 'The selected employer record is successfully deleted.');
                            var url = '/Applications/CustomerUseCases/ViewOrEditEmployer.aspx';
                            var id = 'id=' + selectedRows[0].id;
                            var mode = 'pt=Organization' ;
                            var param = url + "?" + id + '&' + mode;
                            window.proxy.requestNewTab('AddEmployer', param, 'Add Employer');
                        }
                        else {
                            showAlert('Status', 'The selected employer record is currently used by one or more customer records. Please select another employer record to delete.');
                        }
                    }
                });
        };

        var onBtnPersonAddClick = function () {
            var url = '/Applications/CustomerUseCases/AddEmployer.aspx';
            var pt = 'pt=Person';
            var param = url + "?" + pt;
            window.proxy.requestNewTab('AddEmployerPerson', param, 'Add Employer - Person');
        };

        var onBtnOrganizationAddClick = function () {
            var url = '/Applications/CustomerUseCases/AddEmployer.aspx';
            var pt = 'pt=Organization';
            var param = url + "?" + pt;
            window.proxy.requestNewTab('AddEmployerOrganization', param, 'Add Employer - Organization');
        };

        var onRowSelected = function () {
            btnDelete.enable();
            btnOpen.enable();
            btnSelect.enable();
        };

        var onBeforeDelete = function () {
            var result = false;
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            if (selectedRow)
                X.CheckIfDeletable(PageGridPanelSelectionModel.getSelected().id, {
                    success: function (result) {
                        if (result) {
                            btnDelete.enable();
                            showAlert('Status', 'The selected employer record is successfully deleted.');
                        }
                        else {
                            showAlert('Status', 'The selected employer record is currently used by one or more customer records. Please select another employer record to delete.');
                        }
                    }
                });

        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                btnSelect.disable();
            }
        };

        var btnSelect_Click = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToAll(PageGridPanelSelectionModel.getSelected().json, 'onpickemployer');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickemployers');
            }
            window.proxy.requestClose();
        };

    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
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
                runat="server" >
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No employers to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" LabelWidth="100">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelect_Click();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onRowDeselected();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure to delete the selected employer record?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                        <ext:MenuItem ID="MenuItem1" runat="server" Text="Person" Handler="onBtnPersonAddClick"></ext:MenuItem>
                                        <ext:MenuItem ID="MenuItem2" runat="server" Text="Organization" Handler="onBtnOrganizationAddClick"></ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:TextField ID="txtSearchKey" FieldLabel="Search By Name" runat="server" EmptyText="Search Here..." />
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find" >
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click" />
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
                            <ext:JsonReader IDProperty="EmployerID">
                                <Fields>
                                    <ext:RecordField Name="EmployerID" />
                                    <ext:RecordField Name="Names" />
                                    <ext:RecordField Name="Addresses" />
                                    <ext:RecordField Name="PartyType" />
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
                        <ext:Column Header="EmployerID" DataIndex="EmployerID" Wrap="true" Locked="true" Width="100px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Names" Wrap="true" Locked="true" Width="200px">
                        </ext:Column>
                        <ext:Column Header="Business Address" DataIndex="Addresses" Locked="true" Wrap="true"
                            Width="250px">
                        </ext:Column>
                        <ext:Column Header="PartyType" DataIndex="PartyType" Locked="true" Wrap="true" Width="140px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying employers {0} - {1} of {2}" EmptyMsg="No employers to display" />
                </BottomBar>
                <LoadMask ShowMask="true"/>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
