<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickListGuarantor.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.PickListGuarantor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add Co-Borrower</title>
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

        var onRowSelected = function () {
            btnSelect.enable();
        };

        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnSelect.disable();
            }
        };

        var validateSelectedGuarantor = function () {
            var result = 0;
            if (PageGridPanel.hasSelection()) {
                var row = PageGridPanelSelectionModel.getSelected();
                X.validateSelect(row.id, {
                    success: function (result) {
                        if (result == 1)
                            showAlert('Message', 'The selected person is already a guarantor for the loan application.',
                            function () {
                                window.proxy.requestClose();
                            });
                        else if (result == 2)
                            showAlert('Message', 'Borrower and guarantor should not be the same. Please select another person as guarantor.',
                            function () {
                                window.proxy.requestClose();
                            });
                        else if (result == 3)
                            showAlert('Message', 'Guarantor and Co-borrower should not be the same. Please select another person as guarantor.',
                            function () {
                                window.proxy.requestClose();
                            });
                        else if (result == 4) {
                            btnSelectClick();
                        }
                    }
                });
            }
            return result;
        };

        var btnSelectClick = function () {
            if (PageGridPanelSelectionModel.singleSelect) {
                var data = PageGridPanelSelectionModel.getSelected();
                window.proxy.sendToParent(data.json, 'onpickguarantor');
            }
            else {
                var selectedValues = [];
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedValues.push(selectedRows[i].json);
                }
                window.proxy.sendToParent(selectedValues, 'onpickguarantors');
            }
            window.proxy.requestClose();
        }
          
    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit">
    </ext:ResourceManager>
    <ext:Hidden ID="hiddenCustomerId" runat="server"></ext:Hidden>
    <ext:Hidden ID="hiddenCoborrowers" runat="server"></ext:Hidden>
    <ext:Hidden ID="hiddenGuarantors" runat="server"></ext:Hidden>
    <ext:Hidden ID="hiddenUserId" runat="server"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" AutoExpandColumn="Address" Layout="Fit">
            <View>
                <ext:GridView runat="server" EmptyText="No allowed guarantors to display" DeferEmptyText="false"></ext:GridView>
            </View>
            <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor" Disabled="true">
                                <Listeners>
                                    <Click Handler="btnSelectClick();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnClose" Text="Cancel" runat="server" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill>
                            </ext:ToolbarFill>
                            <ext:ComboBox ID="cmbSearch" Editable="false" runat="server" EmptyText="Search by...">
                                <Items>
                                    <ext:ListItem Text="Name" Value="0" />
                                </Items>
                            </ext:ComboBox>
                            <ext:TextField runat="server" ID="txtSearch">
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
                            <ext:JsonReader IDProperty="PartyId">
                                <Fields>
                                    <ext:RecordField Name="PartyId" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Address" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Party Id" DataIndex="PartyId" Locked="true" Wrap="true" Width="140px"
                            Hidden="false" />
                        <ext:Column Header="Name" DataIndex="Name" Width="140px" Locked="true" Wrap="true" />
                        <ext:Column Header="Address" DataIndex="Address" Wrap="true" Locked="true" Width="140px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying allowed {0} - {1} of {2}" EmptyMsg="No allowed partners to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
