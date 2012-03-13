<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankViewList.aspx.cs" Inherits="LendingApplication.Applications.DisbursementUseCases.BankViewList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
<title>Create Encashment</title>
 <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addbank']);
            window.proxy.on('messagereceived', onMessageReceived);
        });
        var onMessageReceived = function (msg) {
            if (msg.tag == 'addbank')
                gridPanel1.reload();
        };
        var AddSelectedBank = function () {
            window.proxy.sendToParent(PageGridPanelSelectionModel.getSelected().json, 'bankselected');
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
    <ext:GridPanel ID="gridPanel1" runat="server" Height="580" AutoExpandColumn="Address">
                  <View>
                <ext:GridView EmptyText="No banks to display"></ext:GridView>
                </View>
    <LoadMask ShowMask="true" />
        <TopBar>
            <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                <Items>
                    <ext:Button runat="server" Text="Select" Icon="Accept">
                        <Listeners>
                            <Click Handler="AddSelectedBank();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator>
                    </ext:ToolbarSeparator>
                    <ext:Button runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" Text="">
                    </ext:Button>
                    <ext:ToolbarFill />
                    <ext:ComboBox ID="cmbSearch" runat="server" Editable="false" EmptyText="Search by...">
                        <Items>
                            <ext:ListItem Text="Name" Value="0" />
                            <ext:ListItem Text="Branch" Value="1" />
                            <ext:ListItem Text="Address" Value="2" />
                        </Items>
                    </ext:ComboBox>
                    <ext:TextField ID="txtSearch" runat="server">
                    </ext:TextField>
                    <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                        <DirectEvents>
                            <Click OnEvent="btnSearch_Click">
                            </Click>
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
                    <ext:JsonReader IDProperty="PartyRoleId">
                        <Fields>
                            <ext:RecordField Name="PartyRoleId" />
                            <ext:RecordField Name="BranchName" />
                            <ext:RecordField Name="OrganizationName" />
                            <ext:RecordField Name="Status" />
                            <ext:RecordField Name="Address" />
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
                <ext:Column Header="Role Id" DataIndex="PartyRoleId" Locked="true" Wrap="true" Width="140px"
                    Hidden="true">
                </ext:Column>
                <ext:Column Header="Name" DataIndex="OrganizationName" Width="140px" Locked="true"
                    Wrap="true">
                </ext:Column>
                <ext:Column Header="Branch" DataIndex="BranchName" Wrap="true" Locked="true" Width="140px">
                </ext:Column>
                <ext:Column Header="Status" DataIndex="Status" Wrap="true" Locked="true" Width="140px">
                </ext:Column>
                <ext:Column Header="Address" DataIndex="Address" Wrap="true" Locked="true" Width="140px">
                </ext:Column>
            </Columns>
        </ColumnModel>
        <BottomBar>
            <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                DisplayMsg="Displaying banks {0} - {1} of {2}" EmptyMsg="No banks to display" />
        </BottomBar>
    </ext:GridPanel>
    </Items>
    </ext:Viewport>
    </form>
</body>
</html>
