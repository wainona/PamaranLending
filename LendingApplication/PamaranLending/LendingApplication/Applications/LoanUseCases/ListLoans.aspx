<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListLoans.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.ListLoans" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Accounts List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['closeselectedloan', 'onpickcustomer', 'addcollection']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                cmbSearchCustomer.setValue(msg.data.Names);
            } else {
                
            }
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
            btnOpen.enable();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnOpen.disable();
            }
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


        var searchOwner = function () {
            X.searchCustomersWithLoan();
        };

        
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel ID="pnlSearchCustomer" runat="server" Layout="FitLayout" Border="false">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Label runat="server" Text="Search Customer:"></ext:Label>
                            <ext:ToolbarSpacer Width="10" runat="server"></ext:ToolbarSpacer>
                            <ext:ComboBox runat="server" ID="cmbSearchCustomer" HideTrigger="true" Width="250" LoadingText="Searching.." DisplayField="Name" 
                                ValueField="CustomerId" EnableKeyEvents="true">
                                <Store>
                                    <ext:Store ID="strCustomersWithLoan" runat="server" AutoLoad="false">
                                        <Reader>
                                            <ext:JsonReader IDProperty="CustomerId">
                                                <Fields>
                                                    <ext:RecordField Name="CustomerId" />
                                                    <ext:RecordField Name="Name" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <KeyPress OnEvent="SearchCustomerWithLoan" />
                                    <Select OnEvent="DisplayLoans" />
                                </DirectEvents>
                            </ext:ComboBox>
                            <ext:ToolbarSpacer runat="server" Width="3"/>
                            <ext:Button ID="btnBrowse" runat="server" Text="Browse" Icon="Magnifier">
                                <Listeners>
                                    <Click Handler="window.proxy.requestNewTab('SelectCustomer','/Applications/LoanRestructureUseCases/CustomerWithLoanPickList.aspx','Select Customer');" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Border="false">
                        <Items>
                            <ext:GridPanel ID="PageGridPanel" runat="server" Layout="Fit" EnableColumnHide="false" EnableColumnMove="false" Border="false">
                                <View>
                                    <ext:GridView EmptyText="No loan accounts to display." />
                                </View>
                                <LoadMask Msg="Loading..." ShowMask="true" />
                                <TopBar>
                                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                                        <Items>
                                            <ext:Hidden ID="Hidden1" runat="server" Text="M d, Y"></ext:Hidden>
                                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="onBtnOpenClick();"></Click>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server"></ext:ToolbarFill>
                                            <ext:ComboBox ID="cmbLoanProducts" runat="server" ValueField="Id" DisplayField="Name" EmptyText="Filter by Loan Product">
                                                <Store>
                                                    <ext:Store ID="strLoanProducts" runat="server">
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

                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="15"></ext:ToolbarSpacer>
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
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="this.clearValue();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="15"></ext:ToolbarSpacer>

                                            <ext:Label ID="Label3" runat="server" Text="Filter by Release Date From" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer7" runat="server" />
                                            <ext:DateField ID="dtfFrom" runat="server" EndDateField="dtfTo" Vtype="daterange" Editable="false"/>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer8" runat="server" />
                                            <ext:Label ID="Label5" runat="server" Text="To" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer9" runat="server" />
                                            <ext:DateField ID="dtfTo" runat="server" StartDateField="dtfFrom" Vtype="daterange" Editable="false"/>
                                            <%-- Search button for compound search --%>
                                            <ext:ToolbarSpacer runat="server" Width="10"/>
                                            <ext:Button runat="server" ID="btnSearch" Text="Apply Filters" Icon="Accept">
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
                                            <ext:JsonReader IDProperty="LoanId">
                                                <Fields>
                                                    <ext:RecordField Name="LoanId" />
                                                    <ext:RecordField Name="_LoanReleaseDate" />
                                                    <ext:RecordField Name="Name" />
                                                    <ext:RecordField Name="LoanProduct" />
                                                    <ext:RecordField Name="LoanAmount" />
                                                    <ext:RecordField Name="LoanBalance" />
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
                                        <ext:Column Header="Loan ID" DataIndex="LoanId" Wrap="true" Locked="true" Width="60px" />
                                        <ext:Column Header="Loan Release Date" DataIndex="_LoanReleaseDate" Locked="true" Wrap="true" Width="140px" />
                                        <ext:Column Header="Owner" DataIndex="Name" Locked="true" Wrap="true" Width="220px" />
                                        <ext:Column Header="Loan Product" DataIndex="LoanProduct" Locked="true" Wrap="true" Width="140px" />
                                        <ext:NumberColumn Header="Loan Amount" DataIndex="LoanAmount" Locked="true" Wrap="true" Format=",000.00" />
                                        <ext:NumberColumn Header="Loan Balance" DataIndex="LoanBalance" Locked="true" Wrap="true" Format=",000.00" />
                                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true" Width="100px" />
                                        <ext:Column Header="Interest Computation Mode" DataIndex="InterestComputationMode" Locked="true" Width="180" />
                                    </Columns>
                                </ColumnModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                        DisplayMsg="Displaying loan accounts {0} - {1} of {2}" EmptyMsg="No loan accounts to display" />
                                </BottomBar>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
