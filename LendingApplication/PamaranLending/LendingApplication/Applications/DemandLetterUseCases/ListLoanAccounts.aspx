<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListLoanAccounts.aspx.cs" Inherits="LendingApplication.Applications.DemandLetterUseCases.ListLoanAccounts" %>
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
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function(msg)
        {
            PageGridPanel.reload();
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {
                var url = '/Applications/LoanUseCases/ViewSelectedLoan.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('ViewSelectedLoan', param, 'View Selected Loan');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onRowSelected = function () {
            btnGenerate.enable();
        };

        var onRowDeselected = function () {

        };

        Ext.apply(Ext.form.VTypes, {
            numberrange: function (val, field) {
                if (!val) {
                    return;
                }

                if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
                    var start = Ext.getCmp(field.startNumberField);

                    if (start) {
                        start.setMaxValue(val);
                        field.numberRangeMax = val;
                        start.validate();
                    }
                } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
                    var end = Ext.getCmp(field.endNumberField);

                    if (end) {
                        end.setMinValue(val);
                        field.numberRangeMin = val;
                        end.validate();
                    }
                }
                return true;
            }
        });

        var generateDemandLetter = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            var demandLetterType = cmbDemandLetterType.getValue();
            var dlturl = '';
            var nodeId = '';
            switch (demandLetterType) {
                case 'First Demand Letter':
                    dlturl = 'FirstDemandLetter.aspx';
                    nodeId = 'FirstDemandLetter';
                    break;

                case 'Final Demand Letter':
                    dlturl = 'FinalDemandLetter.aspx';
                    nodeId = 'FinalDemandLetter';
                    break;

                default:
                    break;
            }

            var url = '/Applications/DemandLetterUseCases/' + dlturl;
            var id = 'id=' + selectedRows[0].id;
            var param = url + "?" + id;
            window.proxy.requestNewTab(nodeId, param, demandLetterType);
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" >
                <Items>
                    <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" EnableColumnHide="false" EnableColumnMove="false">
                        <LoadMask Msg="Loading..." ShowMask="true" />
                        <TopBar>
                            <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="ContainerLayout">
                                <Items>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ComboBox ID="cmbDemandLetterType" runat="server" Editable="false">
                                                <Items>
                                                    <ext:ListItem Text="First Demand Letter" />
                                                    <ext:ListItem Text="Final Demand Letter" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:Button ID="btnGenerate" runat="server" Text="Generate" Disabled="true" Icon="Accept"><%-- Generate Button --%>
                                                <Listeners>
                                                    <Click Handler="generateDemandLetter();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarFill runat="server" />
                                            <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                                                <Listeners>
                                                    <Click Handler="window.proxy.requestClose();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y"></ext:Hidden>
                                           
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                                            <ext:ComboBox ID="cmbFilterByStatus" runat="server" Editable="false" ForceSelection="true" ValueField="Id" DisplayField="Name" EmptyText="Filter by Status">
                                                <Store>
                                                    <ext:Store runat="server" ID="strFilterByStatus">
                                                        <Reader>
                                                            <ext:JsonReader IDProperty="Id">
                                                                <Fields>
                                                                    <ext:RecordField Name="Id" />
                                                                    <ext:RecordField Name="Name" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>

                                            <ext:ToolbarSpacer runat="server" Width="15"></ext:ToolbarSpacer>
 
                                            <ext:Label ID="Label2" runat="server" Text="Search by Owner"/>
                                            <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                            <ext:TextField ID="txtSearch" runat="server" />

                                            <ext:ToolbarSpacer runat="server" Width="15"></ext:ToolbarSpacer>

                                            <ext:Label ID="Label3" runat="server" Text="Filter by Loan Release Date From" />
                                            <ext:ToolbarSpacer runat="server" />
                                            <ext:DateField ID="dtfFrom" runat="server" EndDateField="dtfTo" Vtype="daterange" />
                                            <ext:ToolbarSpacer runat="server" />
                                            <ext:Label ID="Label5" runat="server" Text="To" />
                                            <ext:ToolbarSpacer runat="server" />
                                            <ext:DateField ID="dtfTo" runat="server" StartDateField="dtfFrom" Vtype="daterange" />
                                            <ext:Button runat="server" ID="btnSearch" Text="Search" Icon="Find">
                                                <DirectEvents>
                                                    <Click OnEvent="btnSearch_Click" />
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
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
                                    <ext:JsonReader IDProperty="LoanId">
                                        <Fields>
                                            <ext:RecordField Name="LoanId" />
                                            <ext:RecordField Name="LoanReleaseDate" />
                                            <ext:RecordField Name="Name" />
                                            <ext:RecordField Name="LoanProduct" />
                                            <ext:RecordField Name="Status" />
                                            <ext:RecordField Name="InterestComputationMode" />
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
                                <ext:Column Header="Loan ID" DataIndex="LoanId" Wrap="true" Locked="true" Width="140px" />
                                <%--<ext:Column Header="Loan Release Date" DataIndex="LoanReleaseDate" Locked="true" Wrap="true" Width="140px">
                                    <Renderer Handler="return Ext.util.Format.date(value, Hidden1.value);" />
                                </ext:Column>--%>
                                <ext:Column Header="Customer Name" DataIndex="Name" Locked="true" Wrap="true" Width="200px" />
                                <%--<ext:Column Header="Loan Product" DataIndex="LoanProduct" Locked="true" Wrap="true" Width="140px" />--%>
                                <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="140px" />
                                <%--<ext:Column Header="Interest Computation Mode" DataIndex="InterestComputationMode" Locked="true" Width="180" />--%>
                            </Columns>
                        </ColumnModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                DisplayMsg="Displaying customers {0} - {1} of {2}" EmptyMsg="No customers to display" />
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
