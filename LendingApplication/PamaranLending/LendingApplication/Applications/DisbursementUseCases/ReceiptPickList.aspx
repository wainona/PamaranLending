<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiptPickList.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.ReceiptPickList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Receipt</title>
     <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
     <script type="text/javascript">
         Ext.onReady(function () {
             window.proxy = new ntfx.MIFMessagingProxy();
             window.proxy.init(['addcustomer']);
             window.proxy.on('messagereceived', onMessageReceived);
         });
         var onMessageReceived = function (msg) {
             grdPanelReceipt.reload();
         };
       var AddSelectedCustomer = function () {
            window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'receiptselected');
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
          <ext:GridPanel ID="grdPanelReceipt" runat="server" Height="560" AutoExpandColumn="Name" Layout="FitLayout">
                        <View>
                <ext:GridView EmptyText="No receipts to display"></ext:GridView>
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
                            <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                        </AutoLoadParams>
                        <Listeners>
                            <LoadException Handler="showAlert('Load failed', response.statusText);" />
                        </Listeners>
                        <Reader>
                            <ext:JsonReader IDProperty="ReceiptID">
                                <Fields>
                                    <ext:RecordField Name="ReceiptID" />
                                    <ext:RecordField Name="PartyRoleId" />
                                    <ext:RecordField Name="Name"/>
                                    <ext:RecordField Name="Balance"/>
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true">
                    </ext:RowSelectionModel>
                </SelectionModel>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                    <Columns>
                    <ext:Column DataIndex="PartyRoleId" Header="Party Role ID" Hidden="true"></ext:Column>
                        <ext:Column Header="Receipt ID" DataIndex="ReceiptID" Locked="true" Wrap="true" Width="140px">
                        </ext:Column>
                        <ext:Column Header="Name" DataIndex="Name" Width="140px" Locked="true"
                            Wrap="true">
                        </ext:Column>
                          <ext:NumberColumn Header="Balance" DataIndex="Balance" Wrap="true" Locked="true" Width="140px" Format="0,000.00">
                        </ext:NumberColumn>
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="10" DisplayInfo="true"
                        DisplayMsg="Displaying receipts {0} - {1} of {2}" EmptyMsg="No receipts to display" />
                </BottomBar>
          </ext:GridPanel>
          </Items>
          </ext:Viewport>
    </form>
</body>
</html>
