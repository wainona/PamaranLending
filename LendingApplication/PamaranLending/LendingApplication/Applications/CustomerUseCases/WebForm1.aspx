<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.CustomerUseCases.WebForm1" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ResourceManager ID="ResourceManager1" runat="server">
        </ext:ResourceManager>
        <ext:Store runat="server" ID="FilterStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="CountryStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="CountryTelephoneCode" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Panel runat="server">
        <TopBar>
            <ext:Toolbar>
                <Items>
                    <ext:ComboBox ID="cmbFilterBy" EmptyText="Filter By..." Editable="false" runat="server">
                                <Items>
                                    <ext:ListItem Text="All" />
                                    <ext:ListItem Text="Status" />
                                    <ext:ListItem Text="Party Type" />
                                </Items>
                                <DirectEvents>
                                    <Select OnEvent="OnChange" />
                                </DirectEvents>
                            </ext:ComboBox>
                            <ext:ComboBox ID="cmbFilterBy2" Hidden="true" Editable="false" runat="server" StoreID="FilterStore"
                                 ValueField="Id" DisplayField="Name" ForceSelection="true" TypeAhead="true" 
                                 Mode="Local" TriggerAction="All" />
                            <ext:ComboBox ID="ComboBox1" runat="server" AnchorHorizontal="95%"
                                DisplayField="Name" ValueField="Id" StoreID="CountryStore" ForceSelection="true"
                                Editable="false" TypeAhead="true" Mode="Local" TriggerAction="All" SelectOnFocus="true" />
                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:Panel>
    </div>
    </form>
</body>
</html>
