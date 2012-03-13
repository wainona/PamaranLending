<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDisbursementList.aspx.cs"
    Inherits="LendingApplication.Applications.DisbursementUseCases.ViewDisbursementList" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            window.proxy.init(['addencashment', 'addotherdisbursement', 'addloandisbursement','addchange','addrediscounting']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'addencashment') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateEncashment');
                printEncashment(msg.data.id);
            } else if (msg.tag == 'addrediscounting') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateRediscounting');
                printRediscounting(msg.data.id);
            }
            else if (msg.tag == 'addloandisbursement') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateLoanDis');
                openTransactionSlip(msg.data.id, msg.data.AgreementId);
            } else if (msg.tag == 'addotherdisbursement') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateOther');
                printOtherDisbursement(msg.data.id);
            } else if (msg.tag == 'addchange') {
                PageGridPanel.reload();
                window.proxy.requestClose('CreateChange');
                //printChange(msg.data.id);
            } else {
                PageGridPanel.reload();
            }
        };

        var printEncashment = function (id) {
            var url = '/Applications/DisbursementUseCases/PrintEncashment.aspx';
            var param = url + "?id=" + id;
            window.proxy.requestNewTab('PrintEncashment', param, 'Print Encashment');
        };

        var printOtherDisbursement = function (id) {
            var url = '/Applications/DisbursementUseCases/PrintOtherDisbursement.aspx';
            var param = url + "?id=" + id ;
            window.proxy.requestNewTab('PrintOtherDisbursement', param, 'Print Other Disbursement');
        };

        var printRediscounting = function (id) {
            var url = '/Applications/DisbursementUseCases/PrintRediscounting.aspx';
            var param = url + "?id=" + id;
            window.proxy.requestNewTab('PrintRediscounting', param, 'Print Rediscounting');
        };
        var printChange = function (id) {
            var url = '/Applications/DisbursementUseCases/PrintChange.aspx';
            var param = url + "?id=" + id;
            window.proxy.requestNewTab('PrintChange', param, 'Print Change');
        };

        var FilterSelect = function () {
            PageGridPanel.reload();
        };
        var openTransactionSlip = function (id, AgreementId) {
            var url = '/Applications/DisbursementUseCases/PrintTransactionSlip.aspx';
            var id = 'id=' + id;
            var param = url + '?' + id + "&agreementid=" + AgreementId;
            window.proxy.requestNewTab('PrintTransactionSlip', param, 'Print Transaction Slip');
        };
        var createEncashment = function () {
            var url = '/Applications/DisbursementUseCases/AddEncashment.aspx';
            window.proxy.requestNewTab('CreateEncashment', url, 'Create Encashment');
        };
        var createLoanDisbursement = function () {
            var url = '/Applications/DisbursementUseCases/AddLoanDisbursement.aspx';
            window.proxy.requestNewTab('CreateLoanDis', url, 'Create Loan Disbursement');
        };
        var createOtherDisbursement = function () {
            var url = '/Applications/DisbursementUseCases/AddOtherDisbursement.aspx';
            window.proxy.requestNewTab('CreateOther', url, 'Create Other Disbursement');
        };
        var createChangeDisbursement = function () {
            var url = '/Applications/DisbursementUseCases/AddChange.aspx';
            window.proxy.requestNewTab('CreateChange', url, 'Create Change');
        };
        var createRediscounting = function () {
            var url = '/Applications/DisbursementUseCases/AddRediscounting.aspx';
            window.proxy.requestNewTab('CreateRediscounting', url, 'Create Rediscounting');
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
                            window.proxy.requestNewTab('ViewEncashment', param, 'View Encashment');
                        } else if (result == 2) {
                            url = '/Applications/DisbursementUseCases/LoanDisbursementView.aspx';
                            id = 'id=' + selectedrow.id;
                            param = url + "?" + id;
                            window.proxy.requestNewTab('ViewLoanDisbursement', param, 'View Loan Disbursement');
                        } else if (result == 3) {
                            url = '/Applications/DisbursementUseCases/OtherDisbursementView.aspx';
                            id = 'id=' + selectedrow.id;
                            param = url + "?" + id;
                            window.proxy.requestNewTab('ViewOtherDisbursement', param, 'View Other Disbursement');
                        } else if (result == 4) {
                            url = '/Applications/DisbursementUseCases/ViewChange.aspx';
                            id = 'id=' + selectedrow.id;
                            param = url + "?" + id;
                            window.proxy.requestNewTab('ViewChange', param, 'View Change');
                        } else if (result == 5) {
                            url = '/Applications/DisbursementUseCases/RediscountingView.aspx';
                            id = 'id=' + selectedrow.id;
                            param = url + "?" + id;
                            window.proxy.requestNewTab('ViewRediscounting', param, 'View Rediscounting');
                        }
                    }
                });
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
                                            <ext:MenuItem Text="Encashment" Handler="createEncashment">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Rediscounting" Handler="createRediscounting">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Loan Disbursement" Handler="createLoanDisbursement">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Other Disbursement" Handler="createOtherDisbursement">
                                            </ext:MenuItem>
                                             <ext:MenuItem Text="Change" Handler="createChangeDisbursement">
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            
                          
                            <ext:ToolbarFill>
                            </ext:ToolbarFill>
                           <ext:Label ID="Label1" runat="server" Text="Filter by Date" />
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
                            <ext:ToolbarSeparator runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Label runat="server" Text="Filter By"></ext:Label>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                             <ext:ComboBox ID="cmbFilter" runat="server" Width="160" Editable="false" EmptyText="Type...">
                                <Items>
                                    <ext:ListItem Text="Encashment" Value="0" />
                                    <ext:ListItem Text="Rediscounting" Value="1" />
                                    <ext:ListItem Text="Loan Disbursement" Value="2" />
                                    <ext:ListItem Text="Other Loan Disbursement" Value="3" />
                                    <ext:ListItem Text="Change" Value="4" />
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
                                    <ext:RecordField Name="StrDate" />
                                    <ext:RecordField Name="DisbursedTo" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="Type" />
                                    <ext:RecordField Name="DisbursedBy" />
                                    <ext:RecordField Name="DisbursementTypeId" />
                                    <ext:RecordField Name="DisbursementType"></ext:RecordField>
                                    <ext:RecordField Name="strLoanAccountId"></ext:RecordField>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100">
                    <Columns>
                        <ext:Column Header="Disbursement Type" Wrap="true" DataIndex="DisbursementType" Locked="true" Width="180px" />
                        <ext:Column Header="Loan Account ID" DataIndex="strLoanAccountId" Locked="true" Wrap="true"
                            Width="120px" />
                        <ext:Column Header="Date" DataIndex="StrDate" Width="100px" Locked="true" Wrap="true">
                        </ext:Column>
                        <ext:Column Header="Disbursement To" DataIndex="DisbursedTo" Wrap="true" Locked="true"
                            Width="160px" />
                        <ext:Column Header="Amount" Width="125px" DataIndex="Amount" Locked="true" Wrap="true">
         
                        <Renderer Fn="Ext.util.Format.numberRenderer('0,000.00')" />
                        </ext:Column>
                        <ext:Column Header="Payment Method Type" Wrap="true" DataIndex="Type" Locked="true" Width="160px" Hidden="true" />
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
