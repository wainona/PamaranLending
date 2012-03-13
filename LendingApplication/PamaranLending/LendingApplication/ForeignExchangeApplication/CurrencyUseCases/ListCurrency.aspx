<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCurrency.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.CurrencyUseCases.ListCurrency" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcurrencyconversion', 'editcurrencyconversion']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };

        var onRowSelected = function () {
            btnEditCurrency.enable();
            btnDeleteCurrency.enable();
            //canBeDeleted();
        };

        var onRowDeselected = function () {
        };

        var deleteSuccess = function () {
            //showAlert('Delete Success', 'The selected currency conversion record/s have been successfully deleted.', function () {
                PageGridPanel.reload();
            //});
        };

        var saveSuccess = function () {
            if (hdnRecordId.getValue() == 'Add') {
                showAlert('Success', 'Successfully added the currency.');
            } else {
                showAlert('Success', 'Successfully updated the currency.');
            }

            wndAddEditCurrency.hide();
            PageGridPanel.reload();
        };

        var canBeDeleted = function () {
            X.CheckCurrencyIfActive(currecnyId, {
                success: function (result) {
                    if (result == 0)
                        btnDeleteCurrency.enable();

                }
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" Border="false">
                <LoadMask Msg="Loading.." ShowMask="true" />
                <View>
                    <ext:GridView EmptyText="No currencies to display" />
                </View>
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Hidden ID="hdnRecordId" runat="server" />
                            <ext:Button ID="btnDeleteCurrency" runat="server" Text="Delete" Icon="CoinsDelete" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="deleteSuccess();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected currency record/s?" />
                                            <EventMask Msg="Deleting.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            
                            <ext:Button ID="btnEditCurrency" runat="server" Text="Edit" Icon="Coins" Disabled="true">
                                <DirectEvents>
                                    <Click OnEvent="EditClick">
                                        <EventMask Msg="Loading.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Button ID="btnAddCurrency" runat="server" Text="Add" Icon="CoinsAdd">
                                <DirectEvents>
                                    <Click OnEvent="AddClick">
                                        <EventMask Msg="Loading.." ShowMask="true" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="PageGridPanelStore" runat="server" OnRefreshData="RefreshData" RemoteSort="false">
                        <Proxy>
                            <ext:PageProxy>
                            </ext:PageProxy>
                        </Proxy>
                        <AutoLoadParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Reader>
                            <ext:JsonReader IDProperty="Id">
                                <Fields>
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Description" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column Header="Name" DataIndex="Name" Width="150"/>
                        <ext:Column Header="Description" DataIndex="Description" Width="250"/>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false" >
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying currency conversions {0} - {1} of {2}" EmptyMsg="No currency conversions to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="wndAddEditCurrency" runat="server" Modal="true" Hidden="true" Width="400" Height="127" Resizable="false" Draggable="false" Closable="false" Title="Currency">
        <Items>
            <ext:FormPanel ID="FormPanelAddEditCurrency" runat="server"  Padding="5" Border="false">
                <Items>
                    <ext:TextField ID="txtSymbol" runat="server" FieldLabel="Name" AnchorHorizontal="99%" AllowBlank="false" />
                    <ext:TextField ID="txtDescription" runat="server" FieldLabel="Description" AnchorHorizontal="99%" AllowBlank="false"/>
                </Items>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                <DirectEvents>
                    <Click OnEvent="btnSave_Click">
                        <EventMask Msg="Saving.." ShowMask="true" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{wndAddEditCurrency}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
        <BottomBar>
        
        </BottomBar>
    </ext:Window>
    </form>
</body>
</html>
