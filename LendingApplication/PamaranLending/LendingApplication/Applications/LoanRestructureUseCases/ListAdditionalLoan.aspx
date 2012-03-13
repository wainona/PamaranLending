<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAdditionalLoan.aspx.cs" Inherits="LendingApplication.Applications.LoanRestructureUseCases.ListAdditionalLoan" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Restructure List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcustomer', 'updatecustomer', 'onpickcustomer', 'addamortizationschedule']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'onpickcustomer') {
                hiddenCustomerID.setValue(msg.data.CustomerID);
                txtCustomerName.setValue(msg.data.Names);
                X.FillLoanRestructureGrid(msg.data.CustomerID);
            } else if (msg.tag == 'addcustomer' || msg.tag == 'updatecustomer' || msg.tag == 'addamortizationschedule' || msg.tag == 'loanapplicationstatusupdate') {
                PageGridPanel.reload();
                btnAdditionalLoan.disable();
            }
        };

        var onPickCustomer = function () {
            window.proxy.requestNewTab('CustomerPickList', '/Applications/LoanRestructureUseCases/CustomerWithLoanPickList.aspx?mode=single', 'Customer Pick List');
        };

        var onAdditionalLoanClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows) {
                var id = 'loanId=' + selectedRows[0].json.LoanID;
                var cid = '&cid=' + hiddenCustomerID.getValue();
                var b = '&b=' + selectedRows[0].json.RandomKey;
                var guid = '&guid=' + hiddenResourceGuid.getValue();
                var url = '/Applications/LoanRestructureUseCases/AdditionalLoan.aspx';
                var param = url + "?" + id + cid + b + guid;
                window.proxy.requestNewTab('AdditionalLoan', param, 'Additional Loan');
            } else {
            }
        };

        var onChangeIcmClick = function () {
            showConfirm('Confirm', 'Are you sure you want to change the interest computation mode?', function (btn, text) {
                if (btn == 'yes') {
                    var selectedRows = PageGridPanelSelectionModel.getSelections();
                    if (selectedRows && selectedRows.length == 1) {
                        var id = 'loanId=' + selectedRows[0].json.LoanID;
                        var cid = '&cid=' + hiddenCustomerID.getValue();
                        var b = '&b=' + selectedRows[0].json.RandomKey;
                        var guid = '&guid=' + hiddenResourceGuid.getValue();
                        var url = '/Applications/LoanRestructureUseCases/AmortizationScheduleChangeICM.aspx';
                        var param = url + "?" + id + cid + b + guid;
                        window.proxy.requestNewTab('ChangeIcm', param, 'Change ICM');
                    } else {
                    }
                } else {
                }
            });

        };

        var showResult = function (btn) {
            if (btn == 'Yes') {
                alert("Hello");
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                if (selectedRows && selectedRows.length == 1) {
                    var id = 'loanId=' + selectedRows[0].json.LoanID;
                    var cid = '&cid=' + hiddenCustomerID.getValue();
                    var b = '&b=' + selectedRows[0].json.RandomKey;
                    var guid = '&guid=' + hiddenResourceGuid.getValue();
                    var url = '/Applications/LoanRestructureUseCases/AmortizationScheduleChangeICM.aspx';
                    var param = url + "?" + id + cid + b + guid;

                    window.proxy.requestNewTab('ChangeIcm', param, 'Change ICM');
                } else {
                }
            } else {
                alert("Bye!");
            }
        };

        var onRowSelected = function () {
            if (PageGridPanel.hasSelection() == true) {
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                if (selectedRows && selectedRows.length == 1) {
                    btnAdditionalLoan.enable();
                } else if (selectedRows && selectedRows.length == 2) {
                }
            }
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnAdditionalLoan.disable();
            }
        };
    </script>
    <style type="text/css">
        .white-bg .x-toolbar-ct .x-toolbar-left
        {
            background-color: White;
        }
        
        .button-bg .x-toolbar-ct .x-toolbar-left .x-toolbar-cell
        {
            background-color: Gray;
        }
        
        .boldFormLabel .x-form-item-label
        {
            font-weight: bold;
        }
        
        .boldText .x-btn-text
        {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenResourceGuid" runat="server" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                Layout="Fit"
                Height="500" 
                ButtonAlign="Center">
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No customer loans to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:Hidden runat="server" ID="hiddenCustomerID" />
                            <ext:TextField runat="server" LabelPad="5" ID="txtCustomerName" ReadOnly="true"  LabelWidth="70" FieldLabel="Customer" Width="300" />
                            <ext:Button runat="server" ID="btnBrowseCustomer" Text="Browse..." Height="30" Width="60" Cls="boldText" Icon="Zoom">
                                <Listeners>
                                    <Click Handler="onPickCustomer();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button runat="server" Height="30" Width="175" Cls="boldText"  ID="btnAdditionalLoan" Disabled="true" Icon="Add" Text="Additional Loan">
                                <Listeners>
                                    <Click Handler="onAdditionalLoanClick();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false" OnRefreshData="RefreshData" >
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="RandomKey">
                                <Fields>
                                    <ext:RecordField Name="LoanID" />
                                    <ext:RecordField Name="LoanAmount" />
                                    <ext:RecordField Name="ProductTerm" />
                                    <ext:RecordField Name="RemainingPayments" />
                                    <ext:RecordField Name="LoanReleaseDate"/>
                                    <ext:RecordField Name="TotalLoanBalance" />
                                    <ext:RecordField Name="InterestRate" />
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
                        <ext:Column Header="Loan ID" DataIndex="LoanID" Align="Center" Wrap="true" Locked="true" Width="70px" />
                        <ext:NumberColumn Header="Loan Amount" DataIndex="LoanAmount" Align="Center" Locked="true" Wrap="true"
                            Width="150px" Format=",000.00" />
                        <ext:Column Header="Product Term" DataIndex="ProductTerm" Hidden="true" Locked="true" Align="Center" Wrap="true" Width="180px" />
                        <ext:Column Header="Number of Unbilled Payment Schedules" Hidden="true" DataIndex="RemainingPayments" Align="Center" Locked="true" Wrap="true"
                            Width="200px" />
                        <ext:Column Header="Loan Release Date" DataIndex="LoanReleaseDate" Locked="true" Wrap="true"
                            Width="150px" Align="Center" />
                        <ext:NumberColumn Header="Principal Loan Balance" DataIndex="TotalLoanBalance" Locked="true" Wrap="true"
                        Width="150px" Format=",000.00" Align="Center" />
                        <ext:Column Header="Interest Rate" DataIndex="InterestRate" Align="Center" Locked="true" Wrap="true" Width="180px" />
                        <ext:Column Header="Interest Computation Mode" DataIndex="InterestComputationMode" Locked="true" Wrap="true"
                            Width="180px" Align="Center" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying customer loans {0} - {1} of {2}" EmptyMsg="No customer loans to display" />
                </BottomBar>
                <LoadMask ShowMask="true"/>
                <LoadMask Msg="Loading..." ShowMask="true" />
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
