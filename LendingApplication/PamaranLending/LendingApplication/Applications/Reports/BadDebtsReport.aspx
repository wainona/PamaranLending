<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BadDebtsReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.BadDebtsReport" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link rel="stylesheet" type="text/css" href="../../Resources/css/main.css" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });
    </script>
        <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 12px;
            background-color: #fff !important;
        }
        .box-component
        {
            width: 700px;
            height: 400px;
            border-color: Black;
            border-style: solid;
            border-width: 1px;
            padding: 5px 5px 5px 5px;
        }
        .header
        {
            font-size:medium;
        }
        #PrintableContent
        {
            text-align: center;
        }
        .container
        {
            padding: 0px 0px 0px 0px;
            text-align: center;
        }
        .cssClass .x-grid3-row
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-width: 1px;
            border-style: solid;
        }
        .cssClass .x-grid3-hd
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-top-color: Black;
            border-right-color:Black;
            border-width: 1px;
            border-style: solid;
        }
        .cssClass .x-grid3-header
        {
            background-image: none;
            background-color: transparent;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
     <ext:ResourceManager ID="PageResourceManager" runat="server" />
      <div id="PrintableContent">
        <table style="width: 700px; margin: 0 auto;">
            <%--HEADER--%>
            <tr class="heading">
                <td style="text-align: center; font-weight: bold;">
                    <br />
                    <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" CssClass="header" /><br />
                    <asp:Label ID="lblStreetAddress" runat="server" />
                    <asp:Label ID="lblBarangay" runat="server" />
                    <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />,
                    <asp:Label ID="lblProvince" runat="server" />,
                    <asp:Label ID="lblCountry" runat="server" />,
                    <asp:Label ID="lblPostalCode" runat="server" /><br />
                    Tel#:
                    <asp:Label ID="lblPrimTelNumber" runat="server" />,
                    <asp:Label ID="lblSecTelNumber" runat="server" />
                    Fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                    <asp:Label ID="lblEmailAddress" runat="server" />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
            <td class="header">
                <strong style="font-weight: bold">BAD DEBTS REPORT
                <br />
                <br />
                </strong>
            </td>
            </tr>
            <%--BAD DEBTS--%>
            <tr>
                <td style="text-align: justify;">
                    <table>
                        <tr style="vertical-align: top;">
                            <td style="text-align: left;">
                                <ext:GridPanel ID="panelBadDebts" runat="server" AutoHeight="true" Width="700"
                                    ColumnLines="true" BaseCls="cssClass">
                                    <Store>
                                        <ext:Store runat="server" ID="strBadDebts" RemoteSort="false">
                                            <Listeners>
                                                <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                            </Listeners>
                                            <Reader>
                                                <ext:JsonReader IDProperty="LoanAccountId">
                                                    <Fields>
                                                        <ext:RecordField Name="LoanAccountId" />
                                                        <ext:RecordField Name="CustomerName" />
                                                        <ext:RecordField Name="_DatePromiseToPay" />
                                                        <ext:RecordField Name="StationNumber" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionBadDebts" SingleSelect="false" runat="server">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <ColumnModel runat="server" ID="clmBadDebts" Width="100%">
                                        <Columns>
                                           <ext:Column Header="Station Number" MenuDisabled="true" DataIndex="StationNumber" Wrap="true" Locked="true"
                                                Width="120" Sortable="true"   Hideable="false" Align="Center" Css="border-color: Black;"/>
                                            <ext:Column Header="Name of Borrower" MenuDisabled="true" DataIndex="CustomerName" Wrap="true" Locked="true"
                                                Width="250" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;"/>
                                            <ext:Column Header="Loan ID" MenuDisabled="true" DataIndex="LoanAccountId" Locked="true" Wrap="true"
                                                Width="120" Sortable="false"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                            <ext:Column Header="Date Promise To Pay" MenuDisabled="true" DataIndex="_DatePromiseToPay" Locked="true" Wrap="true"
                                                Width="200" Sortable="true"  Hideable="false" Align="Center" Css="border-color: Black;" />
                                        </Columns>
                                    </ColumnModel>
                                </ext:GridPanel>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
