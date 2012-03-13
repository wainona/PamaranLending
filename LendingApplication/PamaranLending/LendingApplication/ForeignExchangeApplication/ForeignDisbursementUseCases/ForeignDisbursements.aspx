<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForeignDisbursements.aspx.cs" Inherits="LendingApplication.ForeignExchangeApplication.ForeignDisbursementUseCases.ForeignDisbursements" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Disbursement List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addforeignencashment','addforeignrediscounting']);
            window.proxy.on('messagereceived', onMessageReceived);

        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'addforeignencashment') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateForeignEncashment');

            } else if (msg.tag == 'addforeignrediscounting') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateForeignRediscounting');
            }
        }

        var createForeignEncashment = function () {
            var url = '/ForeignExchangeApplication/ForeignDisbursementUseCases/AddForeignEncashment.aspx';
            window.proxy.requestNewTab('CreateForeignEncashment', url, 'Create Foreign Encashment');
        };
        var createForeignRediscounting = function () {
            var url = '/ForeignExchangeApplication/ForeignDisbursementUseCases/AddForeignRediscounting.aspx';
            window.proxy.requestNewTab('CreateForeignRediscounting', url, 'Create Foreign Rediscounting');
        };
  
        var onRowSelected = function () {
            btnOpen.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnOpen.disable();
            }
        };
        var FilterSelect = function () {
            PageGridPanel.reload();
        };
        var CheckDisbursementType = function () {
            var result = 0;
            var selectedrow = PageGridPanelSelectionModel.getSelected();
            if (selectedrow) {
                X.checkType(selectedrow.id, {
                    success: function (result) {
                        if (result == 1) {
                            var url = '/Applications/DisbursementUseCases/EncashmentView.aspx';
                            var id = 'id=' + selectedrow.id;
                            var param = url + "?" + id;
                            window.proxy.requestNewTab('ViewForeignEncashment', param, 'View Foreign Encashment');
                        } else if (result == 2) {
                            var url = '/Applications/DisbursementUseCases/RediscountingView.aspx';
                            id = 'id=' + selectedrow.id;
                            param = url + "?" + id;
                            window.proxy.requestNewTab('ViewForeignRediscounting', param, 'View Foreign Rediscounting');
                        }
                    }
                });
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit">
    </ext:ResourceManager>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" AutoExpandColumn="Amount">
              <View>
                <ext:GridView EmptyText="No disbursements to display"></ext:GridView>
                </View>
            <LoadMask ShowMask="true" />
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                          <ext:Button ID="btnOpen" Text="Open" runat="server" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="CheckDisbursementType();" />
                                </Listeners>
                            </ext:Button>
                        <ext:ToolbarSeparator ID="btnOpenSeparator">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnAdd" Text="New" Icon="Add" runat="server">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                            <ext:MenuItem Text="Encashment" Handler="createForeignEncashment">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Rediscounting" Handler="createForeignRediscounting">
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            <ext:ToolbarFill>
                            </ext:ToolbarFill>
                           <ext:Label ID="lbl1" runat="server" Text="Filter by Date" />
                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                            <ext:DateField ID="dtFrom" runat="server" EndDateField="dtTo" />
                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5" />
                            <ext:Label ID="Label2" runat="server" Text="To" />
                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="5" />
                            <ext:DateField ID="dtTo" runat="server" StartDateField="dtFrom" AllowBlank="true" />
                            <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="5" />

                            <ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search by..." Width="120">
                                <Items>
                                    <ext:ListItem Text="Customer Name" Value="0" />
                                    <ext:ListItem Text="Collector Name" Value="1" />
                                </Items>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSearch" runat="server" EmptyText="Search text here.">
                            </ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Label ID="lbl2" runat="server" Text="Filter By"></ext:Label>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                             <ext:ComboBox ID="cmbFilter" runat="server" Width="120" Editable="false" EmptyText="Type...">
                                <Items>
                                    <ext:ListItem Text="Encashment" Value="0" />
                                    <ext:ListItem Text="Rediscounting" Value="1" />
                                    <ext:ListItem Text="All" Value="-1" />
                                </Items>
                                <Listeners>
                                <Select Handler="FilterSelect();" />
                                </Listeners>
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
                            <ext:JsonReader IDProperty="DisbursementId">
                                <Fields>
                                    <ext:RecordField Name="DisbursementId" />
                                    <ext:RecordField Name="Date" />
                                    <ext:RecordField Name="DateStr" />
                                    <ext:RecordField Name="DisbursedTo" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="Type" />
                                    <ext:RecordField Name="DisbursedBy" />
                                    <ext:RecordField Name="DisbursementTypeId" />
                                    <ext:RecordField Name="DisbursementType"></ext:RecordField>
                                    <ext:RecordField Name="strLoanAccountId"></ext:RecordField>
                                    <ext:RecordField Name="Currency"></ext:RecordField>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100">
                    <Columns>
                        <ext:Column Header="Disbursement Type" Wrap="true" DataIndex="DisbursementType" Locked="true" Width="180px" />
                        <ext:Column Header="Loan Account ID" DataIndex="strLoanAccountId" Locked="true" Hidden="true" Wrap="true"
                            Width="120px" />
                        <ext:Column Header="Date" DataIndex="DateStr" Width="100px" Locked="true" Wrap="true">
                        </ext:Column>
                        <ext:Column Header="Disbursement To" DataIndex="DisbursedTo" Wrap="true" Locked="true"
                            Width="160px" />
                        <ext:Column Header="Amount" Width="125px" DataIndex="Amount" Locked="true" Wrap="true">
                        <Renderer Fn="Ext.util.Format.numberRenderer('0,000.00')" />
                        </ext:Column>
                        <ext:Column Header="Currency" Wrap="true" DataIndex="Currency" Locked="true" Width="160px" Hidden="false" />
                        <ext:Column Header="Disbursed By" DataIndex="DisbursedBy" Wrap="true" Locked="true"
                            Width="160px" />
                        
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeselected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying disbursements {0} - {1} of {2}" EmptyMsg="No loan disbursement to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>

