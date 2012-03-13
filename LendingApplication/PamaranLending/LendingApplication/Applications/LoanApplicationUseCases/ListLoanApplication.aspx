<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListLoanApplication.aspx.cs" Inherits="LendingApplication.Applications.LoanApplicationUseCases.ListLoanApplication" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Application List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['createLoanApplication', 'modifyLoanApplication', 'approvedloanapplication', 'closeapproved', 'loanapplicationstatusupdate']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag != 'closeapproved')
                PageGridPanel.reload();
            else if (msg.tag == 'createLoanApplication') {
                hdnLoanReleaseDate.setValue(msg.data.LoanReleaseDate);
                hdnPaymentStartDate.setValue(msg.data.PaymentStartDate);
            }
            else {
                window.proxy.requestClose('ApproveLoanApplication');
                openPromissory(msg.data.id);
            }
        };

        var openPromissory = function (id) {
            var url = '/Applications/LoanApplicationUseCases/PrintPromissoryNote.aspx';
            var param = url + '?loanApplicationId=' + id;
            param += '&mode=approve';
            window.proxy.requestNewTab('PrintPromissoryNote', param, 'Print Promissory Note');
        }

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if(selectedRows && selectedRows.length > 0)
            {
                var url = '/Applications/LoanApplicationUseCases/ModifyLoanApplication.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.requestNewTab('ModifyLoanApplication', param, 'Modify Loan Application Record');
            }
            else{
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnApproveClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();


            if (selectedRows && selectedRows.length > 0) {
                var data = {};
                data.LoanReleaseDate = hdnLoanReleaseDate.getValue();
                data.PaymentStartDate = hdnPaymentStartDate.getValue();

                var url = '/Applications/LoanApplicationUseCases/ApproveLoanApplication.aspx';
                var id = 'id=' + selectedRows[0].id;
                var param = url + "?" + id;
                window.proxy.sendToAll(data, 'forapproval');
                window.proxy.requestNewTab('ApproveLoanApplication', param, 'Approve Loan Application Record');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };

        var onBtnPrint = function () {
            window.proxy.requestNewTab('PrintLoanApplicationForm', '/Applications/LoanApplicationUseCases/PrintLoanApplicationForm.aspx', 'Print Loan Application Form');
        }

        var onBtnAddClick = function () {
            window.proxy.requestNewTab('AddLoanApplicationRecord', '/Applications/LoanApplicationUseCases/CreateLoanApplication.aspx', 'Create Loan Application Record');
        };

        var canChangeStatusTo = function (statusFrom, statusTo) {
            //'Pending: Approval' || 'Approved' || 'Pending: In Funding'
            var valid = false;
            if (statusTo == 'Cancelled') {
                valid = statusFrom == 'Pending: Approval' || statusFrom == 'Approved' || statusFrom == 'Pending: In Funding';
            }
            else if (statusTo == 'Rejected' || statusTo == 'Approved') {
                valid = statusFrom == 'Pending: Approval';
            }
            else if (statusTo == 'Closed') {
                valid = statusFrom == 'Pending: In Funding' || statusFrom == 'Pending: Approval';
            }

            return valid;
        }

        var changeStatusSuccessful = function () {
            showAlert('Status', 'Successfully updated the status of the loan application record.', function (btn) {
                if (btn == 'ok') {
                    window.proxy.sendToAll('approvedloanapplication', 'approvedloanapplication');
                }
            });
            PageGridPanel.reload();
            enableOrDisableButtons();
        }

        var deleteSuccessful = function () {
            showAlert('Status', 'Successfully deleted the selected loan application record.', function (btn) {
                if (btn == 'ok') {
                    window.proxy.sendToAll('deletedloanapplication', 'deletedloanapplication');
                }
            });
            onRowDeselected();
        }

        var enableOrDisableButtons = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            btnReject.disable();
            btnCancel.disable();
            btnApprove.disable();
            btnClose.disable();
            for (var i = 0; i < selectedRows.length; i++) {
                var json = selectedRows[i].json
                if (canChangeStatusTo(json.Status, 'Approved'))
                    btnApprove.enable();
                if (canChangeStatusTo(json.Status, 'Cancelled'))
                    btnCancel.enable();
                if (canChangeStatusTo(json.Status, 'Rejected'))
                    btnReject.enable();
                if (canChangeStatusTo(json.Status, 'Closed'))
                    btnClose.enable();
            }
        }

        var enableDisablePanelElement = function (panel, enable) {
            if (enable == true) {
                panel.cascade(function (item) {
                    if (item.isFormField) {
                        item.enable();
                    }
                });
            } else {
                panel.cascade(function (item) {
                    if (item.isFormField) {
                        item.disable();
                    }
                });
            }
        };

        var OnBeforeDelete = function () {
            var result = false;
            var ids = [];
            var selectedRows = PageGridPanelSelectionModel.getSelections();

            for (var i = 0; i < selectedRows.length; i++) {
                ids.push(selectedRows[i].id);
            }

            if (PageGridPanel.hasSelection())
                X.CanDeleteLoanApplication(ids, {
                    success: function (result) {
                        if (result == false) {
                            btnDelete.disable();
                            Ext.QuickTips.enable();
                        }
                        else {
                            btnDelete.enable();
                            Ext.QuickTips.disable();
                        }
                    }
                });
            return result;
        }

        var onRowSelected = function () {
            btnOpen.enable();
            OnBeforeDelete();
            enableOrDisableButtons();
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
                btnCancel.disable();
                btnReject.disable();
                //btnPrint.disable();
            }
        };

        var btnCancel_Click = function () {
            showConfirm('Confirm', 'Are you sure you want to cancel the selected loan applications?', function (btn) {
                if (btn == 'yes') {
                    X.Cancel_Click({
                        success: function (result) {
                            if (result == false) {
                                showAlert('Status Error', 'The status of the application was changed by another user. Please refresh to view the changes.', function (btn) {
                                    return false;
                                });
                            } else {
                                changeStatusSuccessful();
                            }
                        }
                    });
                } else {
                    return false;
                }
            });
        }

        var btnReject_Click = function () {
            showConfirm('Confirm', 'Are you sure you want to reject the selected loan applications?', function (btn) {
                if (btn == 'yes') {
                    X.Reject_Click({
                        success: function (result) {
                            if (result == false) {
                                showAlert('Status Error', 'The status of the application was changed by another user. Please refresh to view the changes.', function (btn) {
                                    return false;
                                });
                            } else {
                                changeStatusSuccessful();
                            }
                        }
                    });
                } else {
                    return false;
                }
            });
        }

        var btnClose_Click = function () {
            showConfirm('Confirm', 'Are you sure you want to close the selected loan applications?', function (btn) {
                if (btn == 'yes') {
                    X.Close_Click({
                        success: function (result) {
                            if (result == false) {
                                showAlert('Status Error', 'The status of the application was changed by another user. Please refresh to view the changes.', function (btn) {
                                    return false;
                                });
                            } else {
                                changeStatusSuccessful();
                            }
                        }
                    });
                } else {
                    return false;
                }
            });
        }
    </script>
    <style type="text/css">
        .x-grid-empty {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden ID="hdnLogInId" runat="server" />
    <ext:Hidden ID="hdnLoanReleaseDate" runat="server"></ext:Hidden>
    <ext:Hidden ID="hdnPaymentStartDate" runat="server"></ext:Hidden>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel 
                ID="PageGridPanel" 
                runat="server"
                Layout="Fit"
                AutoExpandColumn="BorrowersName"
                AutoExpandMin="150">
                <View>
                    <ext:GridView EmptyText="No loan applications to display" runat="server" DeferEmptyText="false"></ext:GridView>
                </View>
                <LoadMask ShowMask="true" Msg="Loading Loan Applications.."/>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server" Layout="Container">
                        <Items>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon="Delete" Disabled="true">
                                        <DirectEvents>
                                            <Click OnEvent="btnDelete_Click" Success="deleteSuccessful();">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the selected loan application record?" />
                                                <EventMask ShowMask="true" Msg="Deleting selected loan application..."/>
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                        <Listeners>
                                            <Click Handler="onBtnOpenClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnNew" runat="server" Text="New" Icon="Add" >
                                        <Listeners>
                                            <Click Handler="onBtnAddClick();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button runat="server" ID="btnApprove" Text="Approve" Icon="ApplicationAdd">
                                        <Listeners>
                                            <Click Handler="onBtnApproveClick();"/>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                                        <%--<DirectEvents>
                                            <Click OnEvent="btnCancel_Click" Success="changeStatusSuccessful();">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to cancel the selected loan applications?"/>
                                                <EventMask ShowMask="true" Msg="Updating loan application status..."/>
                                            </Click>
                                        </DirectEvents>--%>
                                        <Listeners>
                                            <Click Handler="btnCancel_Click();"/>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnReject" runat="server" Text="Reject" Icon="ApplicationFormDelete">
                                        <%--<DirectEvents>
                                            <Click OnEvent="btnReject_Click" Success="changeStatusSuccessful();">
                                                <Confirmation ConfirmRequest="true" Message="Are you sure you want to reject the selected loan applications?"/>
                                                <EventMask ShowMask="true" Msg="Updating loan application status..."/>
                                            </Click>
                                        </DirectEvents>--%>
                                        <Listeners>
                                            <Click Handler="btnReject_Click();"/>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ToolbarSeparator/>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="BinClosed" Hidden="true">
                                        <Listeners>
                                            <Click Handler="btnClose_Click();"/>
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnPrint" runat="server" Text="Print Application Form" Icon="Printer">
                                        <Listeners>
                                            <Click Handler="onBtnPrint();"/>
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Label ID="Label2" runat="server" Text="Search By:"></ext:Label>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ComboBox runat="server" ID="cmbSearchBy" Width="140px">
                                        <Items>
                                            <ext:ListItem Text="Borrower's Name"/>
                                            <ext:ListItem Text="Loan Product"/>
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:TextField ID="txtSearch" runat="server" EmptyText="Search..." LabelWidth="50"></ext:TextField>
                                    <ext:ToolbarSpacer />
                                    <ext:Label runat="server" Text="From: "></ext:Label>
                                    <ext:ToolbarSpacer />
                                    <ext:DateField runat="server" ID="dtRangeFrom" EndDateField="dtRangeTo" Vtype="daterange" Editable="false"></ext:DateField>
                                    <ext:ToolbarSpacer />
                                    <ext:Label ID="Label1" runat="server" Text="To: "></ext:Label>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:DateField runat="server" ID="dtRangeTo" StartDateField="dtRangeFrom" Vtype="daterange" Editable="false"></ext:DateField>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                        <DirectEvents>
                                            <Click OnEvent="SearchLoanApplication_Click"></Click>
                                        </DirectEvents>
                                    </ext:Button>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Label ID="Label3" runat="server" Text="Filter By:"></ext:Label>
                                    <ext:ComboBox runat="server" ID="cmbFilterBy" Width="170px">
                                        <Items>
                                            <ext:ListItem Text="All"/>
                                            <ext:ListItem Text="Collateral Requirement"/>
                                            <ext:ListItem Text="Status"/>
                                        </Items>
                                        <DirectEvents>
                                            <Select  OnEvent="OnSelectedItemChange_Click"/>
                                        </DirectEvents>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer></ext:ToolbarSpacer>
                                    <ext:ComboBox ID="cmbFilter" runat="server"  Width="140px"  AnchorHorizontal="100%" ValueField="Id" DisplayField="Name"
                                        ForceSelection="true">
                                        <Store>
                                            <ext:Store runat="server" ID="strFilter" RemoteSort="false">
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
                                        <DirectEvents>
                                            <Select OnEvent="OnSelectItem" />
                                        </DirectEvents>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                     </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store runat="server" ID="strPageGridPanel" RemoteSort="false" OnRefreshData="RefreshData">
                        <Proxy>
                            <ext:PageProxy>
                            </ext:PageProxy>
                        </Proxy>
                        <AutoLoadParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <Load Handler="enableOrDisableButtons();" />
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="LoanApplicationId">
                                <Fields>
                                    <ext:RecordField Name="LoanApplicationId" />
                                    <ext:RecordField Name="ApplicationDate"/>
                                    <ext:RecordField Name="BorrowersName" />
                                    <ext:RecordField Name="LoanProduct" />
                                    <ext:RecordField Name="CollateralRequirement" />
                                    <ext:RecordField Name="Status" />
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
                        <ext:Column Header="Loan Application Id" DataIndex="LoanApplicationId" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Borrower's Name" DataIndex="BorrowersName" Locked="true" Wrap="true"
                            Width="160px">
                        </ext:Column>
                        <ext:Column Header="Loan Product" DataIndex="LoanProduct" Locked="true" Wrap="true"
                            Width="160px">
                        </ext:Column>
                        <ext:Column Header="Collateral Requirement" DataIndex="CollateralRequirement" Locked="true" Wrap="true"
                            Width="160px">
                        </ext:Column>
                        <ext:Column Header="Application Date" DataIndex="ApplicationDate" Locked="true" Wrap="true"
                            Width="160px">
                        </ext:Column>
                        <ext:Column Header="Status" DataIndex="Status" Locked="true" Wrap="true"
                            Width="160px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying loan application {0} - {1} of {2}" EmptyMsg="No loan applications to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
