<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenLoanPayment.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.OpenLoanPayment" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>View Loan Payment</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="pageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
        <ext:Viewport ID="PageViewPort" runat="server" Layout="ColumnLayout">
            <Items>
                <ext:Hidden ID="hiddenPaymentID" runat="server" />
                <ext:GridPanel 
                ID="PageGridPanel"
                Layout="FitLayout"
                runat="server">
                 <LoadMask ShowMask="true" />
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
                            <ext:JsonReader IDProperty="PaymentMethod">
                                <Fields>
                                    <ext:RecordField Name="_TransactionDate" />
                                    <ext:RecordField Name="PaymentMethod" />
                                    <ext:RecordField Name="BankName" />
                                    <ext:RecordField Name="BankBranch" />
                                    <ext:RecordField Name="CheckNumber" />
                                    <ext:RecordField Name="AmountApplied" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                    </Store>
                     <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                    </ext:RowSelectionModel>
                    </SelectionModel>
                    <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                        <Columns>
                        <ext:Column Header="Transaction Date" DataIndex="_TransactionDate" Locked="true" Wrap="true"
                            Width="150px">
                        </ext:Column>
                        <ext:Column Header="Payment Method" DataIndex="PaymentMethod" Wrap="true" Locked="true" Width="150px">
                        </ext:Column>
                        <ext:Column Header="Bank" DataIndex="BankName" Locked="true" Wrap="true"
                            Width="150px">
                        </ext:Column>
                        <ext:Column Header="Branch" DataIndex="BankBranch" Locked="true" Wrap="true" Width="150px">
                        </ext:Column>
                        <ext:Column Header="Cheque Number/Ref #" DataIndex="CheckNumber" Locked="true" Wrap="true"
                            Width="150px">
                        </ext:Column>
                        <ext:NumberColumn Header="Amount Applied" Locked="true" Hideable="false" DataIndex="AmountApplied"
                         Align="Center" Width="150px" Format=",000.00" />
                    </Columns>
                    </ColumnModel>
                    <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying breakdown {0} - {1} of {2}" EmptyMsg="No breakdown of payments to display" />
                    </BottomBar>
                 </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>
