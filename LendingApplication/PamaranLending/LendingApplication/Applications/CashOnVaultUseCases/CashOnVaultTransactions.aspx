<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CashOnVaultTransactions.aspx.cs" Inherits="LendingApplication.Applications.CashOnVaultUseCases.CashOnVaultTransactions" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash On Vault Transactions</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
         
        });

     
        var OnGenerateClick = function () {
            X.FillCOVHistory();
        };

    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
        <ext:GridPanel ID="GridPanel1" runat="server">
            <LoadMask Msg="Loading.." ShowMask="true" />
             <View>
            <ext:GridView EmptyText="No records to display"></ext:GridView>
            </View>
                <TopBar>
             <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label ID="Label2" runat="server" Text="Currency: ">
                            </ext:Label>
                            <ext:ComboBox ID="cmbCurrency" runat="server" ValueField="Id" DisplayField="NameDescription"
                                Width="180" Editable="false" AllowBlank="false" AutoScroll="true">
                                <Store>
                                    <ext:Store ID="strCurrency" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id">
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="NameDescription" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:Button ID="btnGenerate" runat="server" Text="Display Transactions" Icon="Accept">
                                <Listeners>
                                    <Click Handler="OnGenerateClick();" />
                                </Listeners>
                                </ext:Button>
                            <ext:Button ID="Button2" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            <Store>
                <ext:Store ID="Store" runat="server">
                    <Reader>
                        <ext:JsonReader IDProperty="COVTransId">
                                <Fields>
                                    <ext:RecordField Name="COVTransId" />
                                    <ext:RecordField Name="Amount" />
                                    <ext:RecordField Name="TransType"/>
                                    <ext:RecordField Name="ProcessedBy"/>
                                    <ext:RecordField Name="Remarks" />
                                    <ext:RecordField Name="_TransactDate" />
                                    <ext:RecordField Name="_EntryDate" />
                                </Fields>
                            </ext:JsonReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel2" runat="server">
                <Columns>
                      
                        <ext:NumberColumn Header="Amount" DataIndex="Amount" Width="140px" Locked="true" Format=",000.00"/>
                        <ext:Column Header="Transaction Type" DataIndex="TransType" Width="140px" Locked="true" Wrap="true" />
                        <ext:Column Header="Processed By" DataIndex="ProcessedBy" Width="140px" Locked="true" Wrap="true" />
                        <ext:Column Header="Remarks" DataIndex="Remarks" Width="140px" Locked="true" Wrap="true" />
                         <ext:Column Header="Transaction Date" DataIndex="_TransactDate" Width="180px" Locked="true" />
                         <ext:Column Header="Entry Date" DataIndex="_EntryDate" Width="180px" Locked="true" />
                </Columns>
            </ColumnModel>
              <BottomBar>
                    <ext:PagingToolbar ID="PagingToolbar2" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying records {0} - {1} of {2}" EmptyMsg="No records to display" />
                </BottomBar>
        </ext:GridPanel>
          </Items>
    </ext:Viewport>
    </form>
</body>
</html>

