<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeStatementReport.aspx.cs" Inherits="LendingApplication.Applications.Reports.IncomeStatementReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Bill Statement</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printdiv(printpage) {
            var headstr = '<html><head></head><body>';
            var footstr = '</body></html>';
            var newstr = document.getElementById(printpage).innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }
        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
            //PageToolBar.show();
        }

        var enableDisable = function () {
            var date = dtDate.getValue();
        }

    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        .style4
        {
            text-align: center;
        }
        .style3
        {
            width: 222px;
        }
        .style5
        {
            width: 224px;
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
        <div id="toolBar">
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Hidden runat="server" ID="hdnMonth"></ext:Hidden>
                        <ext:DateField runat="server" ID="dtDate" Width="150" Format="F Y" 
                            TodayText="This Month" AllowBlank="false" Editable="false">
                            <Plugins>
                                <ext:MonthPicker ID="MonthPicker1" runat="server"/>
                            </Plugins>
                            <Listeners>
                                <Select Handler="enableDisable();"/>
                            </Listeners>
                        </ext:DateField>
                        <ext:Hidden runat="server" ID="hdnDate"></ext:Hidden>
                        <ext:ToolbarSpacer />
                        <ext:Button ID="btnGenerate" runat="server" Text="Generate Summary" Icon="FolderGo">
                            <DirectEvents>
                                <Click OnEvent="btnGenerate_Click" Before="#{hdnDate}.setValue(Ext.util.Format.date(#{dtDate}.getValue()));">
                                    <EventMask ShowMask="true" Msg="Generating income statement..."/>
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarFill />
                        <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer">
                            <Listeners>
                                <Click Handler="printContent('PrintableContent');" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:FormPanel>
        </div>
        <div id="PrintableContent">
        <br />
            <table style="border: thin none #FFFFFF; width:700px; margin: 0 auto; height:auto">
                <%--HEADER--%>
                <tr class="heading">
                    <td 
                        class="style4">
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <asp:Label ID="lblStreetAddress" runat="server" /> <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br />
                    </td>
                </tr>
    
                <tr>
                <td style="text-align: center; ">

                    <b>INCOME STATEMENT<br />
                   For the  Month of <ext:Label ID="lblMonth" runat="server" 
                        style="text-decoration: underline"></ext:Label></b>
                   <br />
                   <br />
                </td>
                </tr>
                <tr>
                    <td>
                       REVENUES:
                       <br />
                    </td>
                </tr>
                <tr  align="center">
                    <td style="text-align:center; margin: 0 auto;" colspan="8">
                        <ext:GridPanel ID="grdPnlRevenue" runat="server" AutoHeight="true" Width="700"
                            ColumnLines="true" BaseCls="cssClass" Layout="FitLayout">
                            <Store>
                                <ext:Store runat="server" ID="strRevenue" RemoteSort="false">
                                    <Listeners>
                                        <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                    </Listeners>
                                    <Reader>
                                        <ext:JsonReader IDProperty="Station">
                                            <Fields>
                                                <ext:RecordField Name="Station" />
                                                <ext:RecordField Name="InterestPaid" />
                                                <ext:RecordField Name="CapitalPaid" />
                                                <ext:RecordField Name="RemainingLoanBalances" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel2" runat="server" Width="100%">
                                <Columns>
                                   <ext:Column DataIndex="Station" Header="Station"  Css="border-color:Black;" Align="Center" 
                                    Sortable="false" Hideable="false" MenuDisabled="true" Locked="true" Wrap="true" Width="140px">
                                    </ext:Column>
                                    <ext:NumberColumn Header="Interest Paid" MenuDisabled="true" DataIndex="InterestPaid" Locked="true" Wrap="true"
                                              Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                                Css="border-color: Black;" Width="150px" />
                                    <ext:NumberColumn Header="Capital Paid" MenuDisabled="true" DataIndex="CapitalPaid" Locked="true" Wrap="true"
                                Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                        Css="border-color: Black;" Width="150px" />
                                    <ext:NumberColumn Header="Remaining Loan Balances" MenuDisabled="true" DataIndex="RemainingLoanBalances" Locked="true" Wrap="true"
                                   Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                        Css="border-color: Black;" Width="250px" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                    </td>
                </tr>
                <tr>
                <br />
                <br />
                </tr>
                <tr>
                    <td>
                        <table style="width: 700px">
                            <tr>
                                <td class="style5">
                                Total Interest Earned:
                                </td>
                                <td>
                                <ext:Label runat="server" ID="lblTotalInterestEarned" 
                                        style="text-decoration: underline; font-weight: 700"></ext:Label>
                                </td>
                            </tr>
                            <tr>
                               <td class="style5">
                                Less 10% Tithes( <ext:Label runat="server" ID="lblTithes"></ext:Label>):
                                <br />
                                <br />
                                </td>
                                <td>
                               <ext:Label runat="server" ID="lblLessTithes" 
                                        style="font-weight: 700; text-decoration: underline"></ext:Label> 
                                <br />
                                <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    EXPENSES:
                    <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center">
                        <ext:GridPanel ID="grdPnlExpenses" runat="server" AutoHeight="true" Width="700"
                            ColumnLines="true" BaseCls="cssClass">
                            <Store>
                                <ext:Store runat="server" ID="strExpenses" RemoteSort="false">
                                    <Listeners>
                                        <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                    </Listeners>
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="Particular" />
                                                <ext:RecordField Name="Amount"/>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server" Width="100%">
                                <Columns>
                                    <ext:Column DataIndex="Particular" Header="Particular"  Css="border-color:Black;" Align="Center" 
                                    Sortable="false" Hideable="false" Locked="true" Wrap="true" Width="200px" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:NumberColumn Header="Amount" DataIndex="Amount" Locked="true" Wrap="true"
                                   Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                        Css="border-color: Black;" Width="490px" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                        <br/>
                    </td>
                </tr>
                <tr>
                  <td>
                        <table style="width: 693px">
                            <tr>
                                <td class="style3">
                                Additional Capital (Divide by 2):
                                </td>
                                <td>
                                <ext:Label runat="server" ID="lblAdditionalCapital" 
                                        style="text-decoration: underline; font-weight: 700"></ext:Label>
                                </td>
                            </tr>
                            <tr>
                               <td class="style3">
                                Net Income per Children (Divide by 6):
                                </td>
                                <td>
                                <ext:Label runat="server" ID="lblIncomePerChildren" 
                                        style="text-decoration: underline; font-weight: 700"></ext:Label>
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