<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListLoanRestructure.aspx.cs" Inherits="LendingApplication.Applications.LoanRestructureUseCases.ListLoanRestructure" %>
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
                X.FillLoanRestructureGrid(msg.data.CustomerID, {
                    success: function (result) {
                        if (result == 0) {
                            showAlert('Alert', 'No customer loan/s to display.');
                        }
                    }
                });
            } else if (msg.tag == 'addcustomer'
                        || msg.tag == 'updatecustomer'
                        || msg.tag == 'addamortizationschedule'
                        || msg.tag == 'loanapplicationstatusupdate') {
                PageGridPanel.reload();
                btnSplitLoan.disable();
                btnAdditionalLoan.disable();
                btnChangeIcm.disable();
                btnChangeInterest.disable();
                btnConsolidateLoan.disable();
            }
        };

        var onPickCustomer = function () {
            window.proxy.requestNewTab('CustomerPickList', '/Applications/LoanRestructureUseCases/CustomerWithLoanPickList.aspx?mode=single', 'Customer Pick List');
        };

        var onConsolidateLoanClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length == 2) {
                var id1 = 'id1=' + selectedRows[0].json.LoanID;
                var id2 = '&id2=' + selectedRows[1].json.LoanID;
                var cid = '&cid=' + hiddenCustomerID.getValue();
                var b1 = '&b1=' + selectedRows[0].json.RandomKey;
                var b2 = '&b2=' + selectedRows[1].json.RandomKey;
                var guid = '&guid=' + hiddenResourceGuid.getValue();
                var url = '/Applications/LoanRestructureUseCases/ConsolidateLoan.aspx';
                var param = url + "?" + id1 + id2 + cid + b1 + b2 + guid;
                window.proxy.requestNewTab('ConsolidateLoan', param, 'Consolidate Loan');
            } else {

            }
        };

        var onSplitLoanClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows) {
                var id = 'loanId=' + selectedRows[0].json.LoanID;
                var cid = '&cid=' + hiddenCustomerID.getValue();
                var b = '&b=' + selectedRows[0].json.RandomKey;
                var guid = '&guid=' + hiddenResourceGuid.getValue();
                var url = '/Applications/LoanRestructureUseCases/SplitLoan.aspx';
                var param = url + "?" + id + cid + b + guid;
                window.proxy.requestNewTab('SplitLoan', param, 'Split Loan');
            } else {

            }
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
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            var icmTo = "";
            if (selectedRows) {
                var icm = selectedRows[0].json.InterestComputationMode;
                if (icm == 'Diminishing Balance Method') {
                    icmTo = 'Straight Line Method';
                } else {
                    icmTo = 'Diminishing Balance Method';
                }
            }
            showConfirm('Confirm', 'Are you sure you want to change the interest computation mode from '+icm+' to '+icmTo+'?', function (btn, text) {
                if (btn == 'yes') {
                    var selectedRows = PageGridPanelSelectionModel.getSelections();
                    if (selectedRows) {
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

        var onChangeInterestClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows) {
                var id = 'loanId=' + selectedRows[0].json.LoanID;
                var cid = '&cid=' + hiddenCustomerID.getValue();
                var b = '&b=' + selectedRows[0].json.RandomKey;
                var guid = '&guid=' + hiddenResourceGuid.getValue();
                var url = '/Applications/LoanRestructureUseCases/ChangeInterest.aspx';
                var param = url + "?" + id + cid + b + guid;
                window.proxy.requestNewTab('ChangeInterest', param, 'Change Interest');
            } else {
            }

        };
        var changeToZeroInterest = function () {
            showConfirm("Message", "Are you sure you want to change interest to zero?", function (btn) {
                if (btn == 'yes') { //dont continue saving
                    var selectedRows = PageGridPanelSelectionModel.getSelections();
                    X.ChangeToZero(selectedRows[0].json.LoanID, {
                        success: function (result) {
                            if (result == true) {
                                showAlert('Alert', 'Successfully changed interest to zero interest.');
                                PageGridPanel.reload();
                                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                            }
                            else showAlert('Alert', 'Loan already has zero interest type.');
                        }
                    });
                }
            });
        };

        var changeToFixedInterest = function () {
            wndowInterest.show();
        };
        var changeFixedInterest = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            X.ChangeToFixed(selectedRows[0].json.LoanID, {
                success: function (result) {
                    if (result == true) {
                        showAlert('Alert', 'Successfully changed interest to fixed interest.');
                        wndowInterest.hide();
                        PageGridPanel.reload();
                        window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                    }
                    else {
                        wndowInterest.hide();
                        showAlert('Alert', 'Loan already has a fixed interest.');
                    }
                }
            });
        }
        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {   
                var url = '/Applications/LoanRestructureUseCases/EditCustomer.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('UpdateCustomer', param, 'Update Customer');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddCustomer', '/Applications/LoanRestructureUseCases/AddCustomer.aspx', 'Add Customer');
        };

        var onRowSelected = function () {
            if (PageGridPanel.hasSelection() == true) {
                var selectedRows = PageGridPanelSelectionModel.getSelections();
                if (selectedRows && selectedRows.length == 1) {
                    btnSplitLoan.enable();
                    btnAdditionalLoan.enable();
                    btnChangeIcm.enable();
                    btnChangeInterest.enable();
                } else if (selectedRows && selectedRows.length == 2) {
                    btnConsolidateLoan.enable();
                    btnSplitLoan.disable();
                    btnAdditionalLoan.disable();
                    btnChangeIcm.disable();
                    btnChangeInterest.disable();
                } else if (selectedRows && selectedRows.length > 2) {
                    btnConsolidateLoan.disable();
                    btnSplitLoan.disable();
                    btnAdditionalLoan.disable();
                    btnChangeIcm.disable();
                    btnChangeInterest.disable();
                }
            }
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnSplitLoan.disable();
                btnAdditionalLoan.disable();
                btnChangeIcm.disable();
                btnChangeInterest.disable();
                btnConsolidateLoan.disable();
            }
        };
    </script>
    <style>
        .white-bg .x-toolbar-ct .x-toolbar-left
        {
            background-color: White;
        }
        
        .button-bg .x-toolbar-ct .x-toolbar-left .x-toolbar-cell
        {
            background-color: Gray;
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
                ButtonAlign="Center" >
                <View>
                    <ext:GridView ForceFit="true" AutoFill="true" EmptyText="No customer loans to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:Hidden runat="server" ID="hiddenCustomerID" />
                            <ext:TextField runat="server" LabelPad="5" ID="txtCustomerName" ReadOnly="true" LabelWidth="70" FieldLabel="Customer" Width="300" />
                            <ext:Button runat="server" ID="btnBrowseCustomer" Height="30" Cls="boldText" Text="Browse..." Width="60" Icon="Zoom">
                                <Listeners>
                                    <Click Handler="onPickCustomer();" />
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
                                    <ext:RecordField Name="InterestRate" />
                                    <ext:RecordField Name="RemainingPayments" />
                                    <ext:RecordField Name="MaturityDate"/>
                                    <ext:RecordField Name="LoanReleaseDate"/>
                                    <ext:RecordField Name="TotalLoanBalance" />
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
                        <ext:Column Header="Product Term" DataIndex="ProductTerm" Hidden="true" Align="Center" Locked="true" Wrap="true" Width="180px" />
                        <ext:Column Header="Number of Unbilled Payment Schedules" Hidden="true" Align="Center" DataIndex="RemainingPayments" Locked="true" Wrap="true"
                            Width="200px" />
                        <ext:Column Header="Loan Release Date" Align="Center" DataIndex="LoanReleaseDate" Locked="true" Wrap="true"
                            Width="150px" />
                        <ext:NumberColumn Header="Principal Loan Balance" Align="Center" DataIndex="TotalLoanBalance" Locked="true" Wrap="true"
                        Width="150px" Format=",000.00" />
                        <ext:Column Header="Interest Rate" DataIndex="InterestRate" Align="Center" Locked="true" Wrap="true" Width="180px" />
                        <ext:Column Header="Interest Computation Mode" Align="Center" DataIndex="InterestComputationMode" Locked="true" Wrap="true"
                            Width="180px" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying customer loans {0} - {1} of {2}" EmptyMsg="No customer loans to display" />
                </BottomBar>
                <LoadMask ShowMask="true"/>
                <Buttons>
                    <ext:Button runat="server" Height="30" Width="175" ID="btnConsolidateLoan" Disabled="true" Text="Consolidate Loans">
                        <Listeners>
                            <Click Handler="onConsolidateLoanClick();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" Height="30" Width="165"  ID="btnSplitLoan" Disabled="true" Text="Split Loan">
                        <Listeners>
                            <Click Handler="onSplitLoanClick();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" Height="30" Width="175" Hidden="true"  ID="btnAdditionalLoan" Disabled="true" Text="Additional Loan">
                        <Listeners>
                            <Click Handler="onAdditionalLoanClick();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" Height="30" Width="165" ID="btnChangeIcm" Disabled="true" Text="Change ICM">
                        <Listeners>
                            <Click Handler="onChangeIcmClick();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" Height="30" Width="175"  ID="btnChangeInterest" Disabled="true" Text="Change Interest">
                        <Menu>
                        <ext:Menu runat="server">
                        <Items>
                        <ext:MenuItem Text="Interest Rate" Handler="onChangeInterestClick"></ext:MenuItem>
                        <ext:MenuItem Text="Fixed Interest" Handler="changeToFixedInterest"></ext:MenuItem>
                        <ext:MenuItem Text="Zero Interest" Handler="changeToZeroInterest"></ext:MenuItem>
                        </Items>
                        
                        </ext:Menu>
                        </Menu>
                    </ext:Button>
                </Buttons>
                <LoadMask Msg="Loading..." ShowMask="true" />
            </ext:GridPanel>
            <ext:Window ID="wndowInterest" Modal="true" Draggable="false" Resizable="false"
                runat="server" Collapsible="false" Height="155" Hidden="true" Title="Add Fixed Interest"
                Width="350">
                <Items>
                <ext:FormPanel Padding="10" runat="server" ID="frmFxdInterest" LabelWidth="100" MonitorValid="true">
                      <Items>
                          <ext:NumberField ID="txtInterest" runat="server" FieldLabel="Fixed Interest" AllowBlank="false">
                          </ext:NumberField>
                      </Items>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar"  runat="server" Height="25" />
                    </BottomBar>
                    <Buttons>
                        <ext:Button ID="btnSaveFxdInterest" runat="server" Text="Save" Disabled="true">
                        <Listeners>
                        <Click Handler="changeFixedInterest();" />
                        </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnCancel" runat="server" Text="Cancel">
                            <Listeners>
                                <Click Handler="wndowInterest.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ClientValidation Handler="#{StatusBar}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSaveFxdInterest}.enable();}  else{#{btnSaveFxdInterest}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
                 
                
                </Items>
            </ext:Window>
        </Items>
    </ext:Viewport>

    </form>
</body>
</html>
