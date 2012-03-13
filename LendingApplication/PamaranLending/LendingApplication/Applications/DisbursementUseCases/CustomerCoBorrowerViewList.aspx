<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerCoBorrowerViewList.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.CustomerCoBorrowerViewList" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Select Customer/Co-Borrower</title>
  <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
     <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcustomer','createLoanApplication']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            grdPnlvoucherList.reload();
        };
        var AddSelectedCustomer = function () {
            window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'customeradded');
            window.proxy.requestClose();
        };
        </script>
</head>
<body>
    <form id="form1" runat="server">
       <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
          <ext:GridPanel ID="grdPnlvoucherList" runat="server" Height="560" AutoExpandColumn="Address">
                        <View>
                <ext:GridView EmptyText="No customers and coborrowers to display"></ext:GridView>
                </View>
          <LoadMask ShowMask="true" />
           <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                        <ext:Button ID="Button1" runat="server" Text="Select" Icon="Cursor">
                        <Listeners>
                        <Click Handler="AddSelectedCustomer();" />
                        </Listeners>
                        </ext:Button>
                        <ext:ToolbarSeparator></ext:ToolbarSeparator>
                        <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                          <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                        </ext:Button>
                            <ext:ToolbarFill />
                            <ext:ComboBox ID="cmbSearchBy" runat="server" Editable="false" EmptyText="Search by...">
                                <Items>
                                    <ext:ListItem Text="Name" Value="0" />
                                </Items>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSearch" runat="server">
                            </ext:TextField>
                            <ext:Hidden ID="txtUserID" runat="server"></ext:Hidden>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                <DirectEvents>
                                    <Click OnEvent="btnSearch_Click"></Click>
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
                            <ext:Parameter Name="limit" Value="22" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="PartyId">
                                <Fields>
                                    <ext:RecordField Name="PartyRoleId" />
                                    <ext:RecordField Name="PartyId" />
                                    <ext:RecordField Name="Name"/>
                                    <ext:RecordField Name="Address"/>
                                    <ext:RecordField Name="Role" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                    <Columns>
                        <ext:Column Header="Party Id" DataIndex="PartyId" Locked="true" Wrap="true" Width="140px"
                            Hidden="true">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Name" Width="140px" Locked="true"
                            Wrap="true">
                        </ext:Column>
                        <ext:Column Header="Address" DataIndex="Address" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                          <ext:Column Header="Role" DataIndex="Role" Wrap="true" Locked="true" Width="140px">
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="22" DisplayInfo="true"
                        DisplayMsg="Displaying customer and co-borrowers {0} - {1} of {2}" EmptyMsg="No customer and co-borrower to display" />
                </BottomBar>
          </ext:GridPanel>
          </Items>
          </ext:Viewport>
    </form>
</body>
</html>
