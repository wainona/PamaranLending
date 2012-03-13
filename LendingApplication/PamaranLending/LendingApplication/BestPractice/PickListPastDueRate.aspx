<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListPastDueRate.aspx.cs" Inherits="LendingApplication.PickListPastDueRate" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var btnSelectClick = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(data.json, 'onpickpastduerate');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickpastduerates');
            }
            window.proxy.requestClose();
        }

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
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <%--ViewPort--%>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"  
                Layout="Fit">
                <View>
                    <ext:GridView runat="server" EmptyText="No past due rates to display" DeferEmptyText="false"></ext:GridView>
                </View>
                <LoadMask ShowMask="true" />
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
                          </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server" ID="storePastDueRate" RemoteSort="false">
                        <Reader>
                            <ext:JsonReader IDProperty="ProductFeatureApplicabilityId">
                                <Fields>
                                    <ext:RecordField Name="ProductFeatureApplicabilityId" />
                                    <ext:RecordField Name="Description" />
                                    <ext:RecordField Name="InterestRate" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" SingleSelect="false" runat="server">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel>
                    <Columns>
                        <ext:Column runat="server" Header="Description" DataIndex="Description" Width="250">
                        </ext:Column>
                        <ext:Column runat="server" Header="Interest Rate" DataIndex="InterestRate" Width="250">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
