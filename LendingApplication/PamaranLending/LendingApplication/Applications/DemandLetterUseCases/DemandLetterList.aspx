<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemandLetterList.aspx.cs" Inherits="LendingApplication.Applications.DemandLetterUseCases.DemandLetterList" %>
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
                var url = '/Applications/DemandLetterUseCases/EditDemandLetter.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('EditDemandLetter', param, 'Edit Demand Letter');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onRowSelected = function () {
            btnOpen.enable();
            enableGenerateButton();
        };

        var enableGenerateButton = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            var json = selectedRows[0].json;
            if (json.DemandLetterStatus == 'First Demand Letter Sent' || json.DemandLetterStatus == 'Final Demand Letter Sent') {
                btnGenerate.disable();
            } else {
                btnGenerate.enable();
            }
        }

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
            var data = selectedRows[0].json;
            var statusType = data.DemandLetterStatus;
            var dlturl = '';
            var nodeId = '';
            var demandLetterType = '';
            switch (statusType) {
                case 'Require First Demand Letter':
                    dlturl = 'FirstDemandLetter.aspx';
                    nodeId = 'FirstDemandLetter';
                    demandLetterType = 'First Demand Letter';
                    break;

                case 'Require Final Demand Letter':
                    dlturl = 'FinalDemandLetter.aspx';
                    nodeId = 'FinalDemandLetter';
                    demandLetterType = 'Final Demand Letter';
                    break;

                default:
                    break;
            }

            X.SendDemandLetter(selectedRows[0].id);
            PageGridPanel.reload();

            var url = '/Applications/DemandLetterUseCases/' + dlturl;
            var id = 'id=' + selectedRows[0].id;
            var param = url + "?" + id;
            window.proxy.requestNewTab(nodeId, param, demandLetterType);
        };
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" IDMode="Explicit" DirectMethodNamespace="X"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" >
                <Items>
                    <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" EnableColumnHide="false" EnableColumnMove="false" AutoExpandColumn="CustomerName">
                        <View>
                            <ext:GridView EmptyText="No demand letters to display." />
                        </View>
                        <LoadMask Msg="Loading..." ShowMask="true" />
                        <TopBar>
                            <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="ContainerLayout">
                                <Items>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ComboBox ID="cmbDemandLetterType" runat="server" Editable="false" Hidden="true">
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
                                            <ext:ToolbarSeparator runat="server" />
                                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="ApplicationEdit" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="onBtnOpenClick();" />
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
                                            <ext:ComboBox ID="cmbFilterByStatus" runat="server" Editable="false" ForceSelection="true" ValueField="Id" DisplayField="Name" EmptyText="Filter by Status" Width="210">
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
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="this.clearValue();" />
                                                </Listeners>
                                            </ext:ComboBox>

                                            <ext:ToolbarSpacer runat="server" Width="15"></ext:ToolbarSpacer>
 
                                            <ext:Label ID="Label2" runat="server" Text="Search by Owner"/>
                                            <ext:ToolbarSpacer runat="server"></ext:ToolbarSpacer>
                                            <ext:TextField ID="txtSearch" runat="server" />

                                            <ext:ToolbarSpacer runat="server" Width="15"></ext:ToolbarSpacer>

                                            <%-- Search button for compound search --%>
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
                                    <ext:JsonReader IDProperty="Id">
                                        <Fields>
                                            <ext:RecordField Name="LoanAccountId" />
                                            <ext:RecordField Name="CustomerName" />
                                            <ext:RecordField Name="DemandLetterStatus" />
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
                                <ext:Column Header="Loan ID" DataIndex="LoanAccountId" Wrap="true" Locked="true" Width="140px" />
                                <ext:Column Header="Customer Name" DataIndex="CustomerName" Locked="true" Wrap="true" Width="200px" />
                                <ext:Column Header="Status" DataIndex="DemandLetterStatus" Locked="true" Wrap="true" Width="250px" />
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
