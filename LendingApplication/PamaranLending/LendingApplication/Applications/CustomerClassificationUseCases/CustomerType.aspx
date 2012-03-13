<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerType.aspx.cs" Inherits="LendingApplication.Applications.CustomerClassificationUseCases.CustomerType" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Type List</title>
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
            hdnCustomerTypeId.setValue(-1);
            onRowDeselected();
        });

        var onBtnOpenClick = function () {
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            var id = selectedRow.json.Id;
            X.OpenCustomerType(id,{
                success: function () {
                    wdwCustomerType.show();
                }
            });
          
        }
        var onBtnAddClick = function () {
            wdwCustomerType.show();
        }

        var deleteSuccessful = function () {
            showAlert('Status', 'Successfully deleted customer type record.', function (btn) {
                    PageGridPanel.reload();
            });
        }
        var onClickSave = function () {
            X.CheckExistence({
                success: function (result) {
                    if (result == false) {
                        X.SaveNewType({
                            success: function () {
                                showAlert('Status', 'Successfully saved customer type record.', function (btn) {
                                    if (btn == 'ok') {
                                        clearCustomerType();
                                        PageGridPanel.reload();
                                    }
                                });
                            }
                        });
                    } else {
                        showAlert('Status', 'Customer type already exists.');
                        clearCustomerType();
                    }
                }
            });
        }


        var clearCustomerType = function () {
            txtCustomerType.setValue('');
            hdnCustomerTypeId.setValue(-1);
            wdwCustomerType.hide();
        }
        var onRowSelected = function () {
            btnDelete.enable();
            btnOpen.enable();

            var selectedRow = PageGridPanelSelectionModel.getSelected();
            var id = selectedRow.json.Id;

            X.CanDeleteCustomerType(id, {
                success: function (result) {
                    if (result) {
                        btnDelete.enable();
                    } else {
                        btnDelete.disable();
                    }
                }
            });
        }
        var onRowDeselected = function () {
            btnDelete.disable();
            btnOpen.disable();
         
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
        <ext:Hidden ID="hdnCustomerTypeId" runat="server"></ext:Hidden>
        <ext:Viewport runat="server" Layout="FitLayout">
            <Items>
                <ext:FormPanel ID="PageFormPanel" runat="server" ButtonAlign="Right" MonitorValid="true"
                    Title="Customer Types" BodyStyle="background-color:transparent"
                    Layout="FitLayout">
                    <Items>
                        <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit">
                            <View>
                                <ext:GridView EmptyText="No customer types to display" DeferEmptyText="false">
                                </ext:GridView>
                            </View>
                            <LoadMask ShowMask="true" Msg="Loading Customer Types.."/>
                            <TopBar>
                                <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                                    <Items>
                                        <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true" ToolTip="The selected customer type cannot be deleted.">
                                            <DirectEvents>
                                                <Click OnEvent="btnDelete_Click" Success="deleteSuccessful();">
                                                    <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the customer type?" />
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
                                            <Listeners>
                                                <Click Handler="onBtnAddClick();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator />
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                        <ext:TextField ID="txtSearch" runat="server"></ext:TextField>
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
                                        <ext:JsonReader IDProperty="Id">
                                            <Fields>
                                                <ext:RecordField Name="Name" />
                                                <ext:RecordField Name="Id" />
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
                                    <ext:Column Header="Id" Width="280" DataIndex="Id" Wrap="true" Locked="true">
                                    </ext:Column>
                                    <ext:Column Header="Name" Width="280" DataIndex="Name" Locked="true" Wrap="true">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                    DisplayMsg="Displaying customer types {0} - {1} of {2}" EmptyMsg="No customer type to display" />
                            </BottomBar>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window runat="server" ID="wdwCustomerType" Collapsible="true" Height="115" Icon="Application"
        Title="New Customer Type" Width="370" Hidden="true" Modal="true" Resizable="false" AutoFocus="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="fpNewCustomerType" runat="server" Padding="5" LabelWidth="100" MonitorValid="true" MonitorPoll="500">
                <Items>
                    <ext:TextField runat="server" ID="txtCustomerType" FieldLabel="Customer Type" AllowBlank="false" Width="220">
                    </ext:TextField>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBarCustomerType" runat="server">
                        <Items>
                            <ext:Button ID="btnSaveNewCustomerType" runat="server" Text="Save" Icon="Disk" Disabled="true">
                            <Listeners>
                            <Click Handler="onClickSave();" />
                            </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnCancelCustomerType" runat="server" Text="Cancel" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="clearCustomerType();"/>
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:StatusBar>
                </BottomBar>
                <Listeners>
                 <ClientValidation Handler="#{StatusBarCustomerType}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveNewCustomerType}.enable();}  else{#{btnSaveNewCustomerType}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
