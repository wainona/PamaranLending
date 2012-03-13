<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoucherViewList.aspx.cs"
    Inherits="LendingApplication.Applications.DisbursementUseCases.VoucherViewList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voucher List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['approvevoucher']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var AddVoucher = function () {
            window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'voucherselected');
            window.proxy.requestClose();
        };

        var onMessageReceived = function (msg) {
            grdPnlvoucherList.reload();
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
                    <ext:GridPanel ID="grdPnlvoucherList" Height="550" runat="server">
                       <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                        <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
                            <ext:Button ID="btnSelect" runat="server" Text="Select" Icon="Cursor">
                             <Listeners>
                            <Click Handler="AddVoucher();" />
                            </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator>
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill/>
                            <ext:Label ID="Label1" Text="Filter By:" runat="server"></ext:Label> 
                            <ext:ComboBox ID="cmbFilter" runat="server" DisplayField="Name"
                                ValueField="Id" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true"
                                TriggerAction="All" SelectOnFocus="true">
                                  <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();" />
                                </Listeners>
                                <Store>
                                    <ext:Store runat="server" ID="storeFilter" RemoteSort="false">
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
                            <ext:Label ID="lblSearch" Text="Search by Name:"></ext:Label>
                            <ext:TextField ID="txtSearch" runat="server">
                            </ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                            <DirectEvents>
                            <Click OnEvent="btnSearch_Click"></Click>
                            </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                    <LoadMask ShowMask="true" />
                        <Store>
                            <ext:Store ID="storeVoucherList" runat="server" OnRefreshData="RefreshData">
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
                                    <ext:JsonReader IDProperty="VoucherID">
                                        <Fields>
                                            <ext:RecordField Name="VoucherID" />
                                            <ext:RecordField Name="AgreementID" />
                                            <ext:RecordField Name="CustomerName" />
                                            <ext:RecordField Name="CustomerID"/>
                                            <ext:RecordField Name="LoanProduct" />
                                            <ext:RecordField Name="LoanAmount"/>
                                            <ext:RecordField Name="LoanProductID"></ext:RecordField>
                                            
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                            <Columns>
                                <ext:Column Header="Voucher ID" DataIndex="VoucherID" Locked="true" Wrap="true"
                                    Width="140px" Hidden="false" />
                                <ext:Column Header="Loan Agreement ID" DataIndex="AgreementID" Width="140px" Locked="true"
                                    Wrap="true" />
                                <ext:Column Header="Customer Name" DataIndex="CustomerName" Locked="true" Wrap="true"
                                    Width="140px" Hidden="false" />
                                <ext:Column Header="Loan Product" DataIndex="LoanProduct" Locked="true" Wrap="true"
                                    Width="140px" Hidden="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                                DisplayMsg="Displaying vouchers {0} - {1} of {2}" EmptyMsg="No voucher to display" />
                        </BottomBar>
                    </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
