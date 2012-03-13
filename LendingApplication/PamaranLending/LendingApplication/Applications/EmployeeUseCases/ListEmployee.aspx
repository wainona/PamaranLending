<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListEmployee.aspx.cs" Inherits="LendingApplication.Applications.EmployeeUseCases.ListEmployee" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addemployee', 'updateemployee', 'changeurltoedit']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag != 'changeurltoedit')
                PageGridPanel.reload();
            else {
                window.proxy.requestClose('AddEmployee');
                openEmployee(msg.data.id);
            }
        };

        var openEmployee = function (id) {
            var url = '/Applications/EmployeeUseCases/ViewEmployee.aspx';
            var newid = 'id=' + id;
            var param = url + "?" + newid;
            window.proxy.requestNewTab('UpdateEmloyee', param, 'Update Employee');
        }

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {   
                var url = '/Applications/EmployeeUseCases/ViewEmployee.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateEmloyee', param, 'Update Employee');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddEmployee', '/Applications/EmployeeUseCases/AddEmployee.aspx', 'Add Employee');
        };

        var OnBeforeDelete = function () {
            var result = false;
            var ids = [];
            var selectedRows = PageGridPanelSelectionModel.getSelections();

            for (var i = 0; i < selectedRows.length; i++) {
                ids.push(selectedRows[i].id);
            }

            if (PageGridPanel.hasSelection())
                X.CanDeleteEmployee(ids, {
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

        var onFailDelete = function () {
            showAlert('Status', 'The selected employee record is currently used by other records. Please select another employee record to delete.');
        }

        var onRowSelected = function () {
            OnBeforeDelete();
            btnOpen.enable();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
            }
            else {
                onRowSelected();
            }
        };

        var successfullyDeleted = function () {
            showAlert('Status', 'The selected employee record is successfully deleted.');
            onRowDeselected();
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                AutoExpandColumn="Address"
                Layout="Fit"
                Border="false">
                <View>
                    <ext:GridView EmptyText="No employees to display." />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="successfullyDeleted" Failure="onFailDelete();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected employee record?" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add" Disabled="true" ToolTip="Lender Information is not yet set.">
                                <Listeners>
                                    <Click Handler="onBtnAddClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill runat="server" />
                            <ext:ComboBox ID="cmbSearchBy" runat="server"  Width="160px" EmptyText="Search by.." AnchorHorizontal="100%"
                                ForceSelection="true" >
                                <Items>
                                    <ext:ListItem Text="Employee ID Number"/>
                                    <ext:ListItem Text="Name"/>
                                </Items>
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer></ext:ToolbarSpacer>
                            <ext:TextField runat="server" ID="txtSearch" EmptyText="type here.."></ext:TextField>
                            <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                            <ext:ComboBox ID="cmbFilterBy" runat="server" EmptyText="Filter by Employment Status.." Width="200px" AnchorHorizontal="100%"
                                ForceSelection="true">
                                <Items>
                                    <ext:ListItem Text="All" />
                                    <ext:ListItem Text="Employed"/>
                                    <ext:ListItem Text="Retired"/>
                                </Items>
                                <%--<Listeners>
                                    <Select Handler="#{PageGridPanelStore}.reload();" />
                                </Listeners>--%>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                            <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find">
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
                            <ext:Parameter Name="limit" Value="22" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="EmployeeId">
                                <Fields>
                                    <ext:RecordField Name="EmployeeId" />
                                    <ext:RecordField Name="EmployeeIdNumber" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Address" />
                                    <ext:RecordField Name="EmploymentStatus" />
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
                        <ext:Column Header="Employee Id" DataIndex="EmployeeId" Wrap="true" Locked="true" Width="140px" Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Employee Id Number" DataIndex="EmployeeIdNumber" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Name" Locked="true" Wrap="true" Width="235px">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Address" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                        <ext:Column Header="Employment Status" DataIndex="EmploymentStatus" Locked="true" Wrap="true"
                            Width="140px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="22" DisplayInfo="true"
                        DisplayMsg="Displaying employees {0} - {1} of {2}" EmptyMsg="No employees to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
