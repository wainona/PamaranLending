<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCollection.aspx.cs"
    Inherits="LendingApplication.Applications.CollectionUseCases.ListCollection" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcollection', 'addfeepayment']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();

            if (msg.tag == 'addcollection') {
                window.proxy.requestClose('AddLoanPayment');
                printLoanPaymentForm(msg.data.id);
            }
        };

        var printPayment = function () {
            if (PageGridPanelSelectionModel.hasSelection() == true) {
                var data = PageGridPanelSelectionModel.getSelected();
                var type = data.get('CollectionType')
                var id = data.get('CollectionID');
                if (type == 'Loan Payment') {
                    printLoanPaymentForm(id);
                } else if (type == 'Fee Payment') {
                    printFeePayment(id);
                }

            }
        }

//        var printPayment = function () {
//            var data = PageGridPanelSelectionModel.getSelected();
//            var id = data.json.CollectionID;
//            printLoanPaymentForm(id);
//        }

        var printLoanPaymentForm = function (id) {
            var url = '/Applications/CollectionUseCases/LoanPaymentForm.aspx';
            var guid = '?loanPaymentId=' + id;
            var param = url + guid;
            window.proxy.requestNewTab('LoanPaymentForm', param, 'Loan Payment Form');
        }
         var printFeePayment = function (id) {
            var url = '/Applications/CollectionUseCases/PrintFeePayment.aspx';
            var guid = '?id=' + id;
            var param = url + guid;
            window.proxy.requestNewTab('FeePaymentRecord', param, 'Fee Payment Form');
        }

        var onRowSelected = function () {
            if (PageGridPanelSelectionModel.hasSelection() == true) {
                btnOpen.enable();
                btnPrint.enable();
            }

        }

        var onRowDeSelected = function () {
            btnOpen.disable();
            btnPrint.disable();
        }

        var openPayment = function () {
            if (PageGridPanelSelectionModel.hasSelection() == true) {
                var data = PageGridPanelSelectionModel.getSelected();
                var type = data.get('CollectionType')
                var id = data.get('CollectionID');
                if (type == 'Loan Payment') {
                    var url = '/Applications/CollectionUseCases/OpenLoanPayment.aspx';
                    var id = 'id=' + id;
                    var param = url + '?' + id;
                    window.proxy.requestNewTab('ViewLoanPaymentForm', param, 'View Payment Breakdown');
                } else if (type == 'Fee Payment') {
                    var url = '/Applications/CollectionUseCases/OpenFeePayment.aspx';
                    var id = 'id=' + id;
                    var param = url + '?' + id;
                    window.proxy.requestNewTab('ViewFeePayment', param, 'View Fee Payment');
                }

            }

        };


        var addLoanPayment = function () {
            window.proxy.requestNewTab('AddLoanPayment', '/Applications/CollectionUseCases/AddLoanPayment.aspx', 'Add Loan Payment');
        };
        var addFeePayment = function () {
            window.proxy.requestNewTab('AddFeePayment', '/Applications/CollectionUseCases/AddFeePayment.aspx', 'Add Fee Payment');
        };
        var FilterCurrency = function () {
            PageGridPanel.reload();
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" AutoExpandColumn="Customer">
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No collections to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                            <ext:MenuItem Text="Loan Payment" Handler="addLoanPayment">
                                            </ext:MenuItem>
                                            <ext:MenuItem Text="Fee Payment" Handler="addFeePayment">
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="openPayment();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer" Disabled="true">
                                <Listeners>
                                    <Click Handler="printPayment();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill />
                            <ext:Label ID="lblFrom" runat="server" Text="Date From: " />
                            <ext:DateField ID="datFromDate" Editable="false" Vtype="daterange" runat="server">
                                <%--<DirectEvents>
                                    <Select OnEvent="dtFromDate_Select"></Select>
                                </DirectEvents>--%>
                            </ext:DateField>
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:Label ID="lblTo" runat="server" Text="To: " />
                            <ext:DateField ID="datToDate" Editable="false" Vtype="daterange" runat="server">
                                <%--<DirectEvents>
                                    <Select OnEvent="dtToDate_Select"></Select>
                                </DirectEvents>--%>
                            </ext:DateField>
                            <ext:ToolbarSpacer />
                            <ext:ComboBox ID="cmbSearchBy" Width="135px" EmptyText="Search By..." Editable="false"
                                runat="server">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                                <Items>
                                    <ext:ListItem Text="Customer Name" Value="1" />
                                    <ext:ListItem Text="Collector Name" Value="2" />
                                </Items>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSearchKey" runat="server" EmptyText="type here..">
                            </ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:ToolbarSpacer />
                            <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id"
                                 DisplayField="Symbol" Width="175" Editable="false" AllowBlank="false" EmptyText="Filter by Currency">
                                   <Store>
                                     <ext:Store ID="strCurrency" runat="server">
                                         <Reader>
                                             <ext:JsonReader IDProperty="Id">
                                                 <Fields>
                                                     <ext:RecordField Name="Id" />
                                                     <ext:RecordField Name="Symbol" />
                                                 </Fields>
                                             </ext:JsonReader>
                                         </Reader>
                                     </ext:Store>
                                 </Store>
                                <Listeners>
                                <Select Handler="FilterCurrency();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:Hidden ID="Hidden1" runat="server" Text="m/d/Y">
                            </ext:Hidden>
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
                            <ext:JsonReader IDProperty="CollectionID">
                                <Fields>
                                    <ext:RecordField Name="CollectionID" />
                                    <ext:RecordField Name="Date"/>
                                    <ext:RecordField Name="DateStr"/>
                                    <ext:RecordField Name="Customer" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="Collector" />
                                    <ext:RecordField Name="CollectionType"></ext:RecordField>
                                    <ext:RecordField Name="CurrencySymbol"></ext:RecordField>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                        <Listeners>
                            <RowSelect Fn="onRowSelected" />
                            <RowDeselect Fn="onRowDeSelected" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Collection ID" DataIndex="CollectionID" Wrap="true" Locked="true"
                            Width="100px" Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Date" DataIndex="DateStr" Locked="true" Wrap="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Customer" DataIndex="Customer" Locked="true" Wrap="true" Width="300px">
                        </ext:Column>
                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Format=",000.00" Locked="true"
                            Wrap="true" Width="150px">
                        </ext:NumberColumn>
                        <ext:Column Header="Currency" DataIndex="CurrencySymbol" Locked="true" Wrap="true" Width="100px">
                        </ext:Column>
                        <ext:Column Header="Collector" DataIndex="Collector" Locked="true" Wrap="true" Width="300px">
                        </ext:Column>
                        <ext:Column Header="CollectionType" DataIndex="CollectionType" Locked="true" Wrap="true" Width="250px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying collections {0} - {1} of {2}" EmptyMsg="No collections to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
