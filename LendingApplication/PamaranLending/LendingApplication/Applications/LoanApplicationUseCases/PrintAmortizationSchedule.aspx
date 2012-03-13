<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintAmortizationSchedule.aspx.cs" Inherits="LendingApplication.Applications.LoanApplicationUseCases.PrintAmortizationSchedule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Loan Application Form</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/main.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            window.print();
        }

    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 13px;
            background-color: #fff !important;
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
        @media screen
        {
            #toolBar
            {
                display: block;
            }
        }
        @media print
        {
            #toolBar
            {
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hiddenResourceGuid" runat="server">
    </ext:Hidden>
    <div id="toolBar">
    <ext:Panel ID="RootPanel1" runat="server" Border="false">
        <TopBar>
            <ext:Toolbar runat="server" ID="PageToolBar">
                <Items>
                    <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                        <Listeners>
                            <Click Handler="printContent('PrintableContent');" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        </ext:Panel>
    </div>
    <ext:Panel ID="RootPanel" runat="server" Border="false">
        <Items>
            <ext:Container runat="server">
                <Content>
                <div id="PrintableContent">
                    <table style="width: 700px; margin: 0 auto;">
                        <%--HEADER--%>
                        <tr class="heading">
                            <td style="text-align: center;">
                                <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" /><br />
                                <asp:Label ID="lblStreetAddress" runat="server" />
                                <asp:Label ID="lblBarangay" runat="server" /><br />
                                <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />,
                                <asp:Label ID="lblProvince" runat="server" />,
                                <asp:Label ID="lblCountry" runat="server" />,
                                <asp:Label ID="lblPostalCode" runat="server" /><br />
                                tel#:
                                <asp:Label ID="lblPrimTelNumber" runat="server" />,
                                <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                                fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                                <asp:Label ID="lblEmailAddress" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr class="nameOfDocument">
                            <td style="text-align: center; font-weight: bold;">
                                TEMPORARY AMORTIZATION SCHEDULE<br />
                                <br />
                            </td>
                        </tr>
                        <%--BASIC INFORMATION--%>
                        <tr class="BorrowersBasicInformation">
                            <td style="text-align: justify;">
                                <table>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                        <ext:CompositeField runat="server">
                                            <Content>
                                                <ext:Label runat="server" Text="Customer Name:"></ext:Label>
                                                <b><ext:Label ID="lblCustomerName" runat="server"></ext:Label></b>
                                            </Content>
                                        </ext:CompositeField>
                                        <br />
                                        <ext:CompositeField ID="CompositeField1" runat="server">
                                            <Content>
                                                <ext:Label runat="server" Text="Date Generated:"></ext:Label>
                                                <b><ext:Label ID="lblDateGenerated" runat="server"></ext:Label></b>
                                            </Content>
                                        </ext:CompositeField>
                                        <br />
                                        <ext:Label ID="lblSchedule" runat="server" FieldLabel="Schedule"></ext:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--TEMPORARY AMORTIZATION SCHEDULE--%>
                        <tr class="LoanApplicationDetails">
                            <td style="text-align: justify;">
                                <center>
                                <table>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" AutoHeight="true"
                                                BaseCls="cssClass" ColumnLines="true">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                                        <Reader>
                                                            <ext:JsonReader>
                                                                <Fields>
                                                                    <ext:RecordField Name="Counter" />
                                                                    <ext:RecordField Name="ScheduledPaymentDate" Type="Date" />
                                                                    <ext:RecordField Name="PrincipalPayment" />
                                                                    <ext:RecordField Name="InterestPayment" />
                                                                    <ext:RecordField Name="TotalPayment" />
                                                                    <ext:RecordField Name="PrincipalBalance" />
                                                                    <ext:RecordField Name="TotalLoanBalance" />
                                                                </Fields>
                                                            </ext:JsonReader>
                                                        </Reader>
                                                    </ext:Store>
                                                </Store>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server" ID="RowSelectionModelAmor">
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <ColumnModel runat="server" ID="clmAmortizationSchedule" Width="100%">
                                                    <Columns>
                                                        <ext:Column Header="Unit" DataIndex="Counter" Wrap="true" Width="70" Sortable="false"
                                                            Css="border-color: Black;" Hideable="false" Align="Center" MenuDisabled="true"/>
                                                        <ext:DateColumn Header="Payment Due Date" DataIndex="ScheduledPaymentDate" Wrap="true"
                                                            Width="110" Sortable="false" Format="MMMM dd, yyyy" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center"/>
                                                        <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalPayment" Wrap="true"
                                                            Width="90" Sortable="false" Format=",000.00" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center"/>
                                                        <ext:NumberColumn Header="Interest Due" DataIndex="InterestPayment" Wrap="true" Width="90"
                                                            Sortable="false" Format=",000.00" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center"/>
                                                        <ext:NumberColumn Header="Total" DataIndex="TotalPayment" Wrap="true" Width="110"
                                                            Sortable="false" Format=",000.00" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center"/>
                                                        <ext:NumberColumn Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true"
                                                            Width="110" Sortable="false" Format=",000.00" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center"/>
                                                        <ext:NumberColumn Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                            Width="110" Sortable="false" Format=",000.00" MenuDisabled="true"
                                                            Css="border-color: Black;" Hideable="false" Align="Center" Hidden="true"/>
                                                    </Columns>
                                                </ColumnModel>
                                            </ext:GridPanel>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: right;">
                                            <ext:Label ID="Label1" runat="server" Text="*temporary amortization schedule only."></ext:Label><br /><br />
                                        </td>
                                    </tr>
                                </table>
                                </center>
                                <br /><br />
                            </td>
                        </tr>
                        <br /><br />
                    </table>
                </div>
                </Content>
            </ext:Container>
        </Items>
    </ext:Panel>
    </form>
</body>
</html>
