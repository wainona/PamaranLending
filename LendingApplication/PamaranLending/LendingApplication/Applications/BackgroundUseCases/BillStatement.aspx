<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillStatement.aspx.cs" Inherits="LendingApplication.Applications.BackgroundUseCases.BillStatement" %>

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
    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
        #namewidth
        {
            width:470px;
        }
        #date
        {
            width:230px;
        }
        .secondrow
        {
            width:230px;
        }
        .style1
        {
            text-align: center;
        }
        .style2
        {
            width: 282px;
        }
        .style3
        {
            width: 652px;
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
        <ext:Hidden ID="hdnLoanId" runat="server" />
        </Items>
        </ext:Toolbar>
        </TopBar>
        </ext:FormPanel>
        </div>
        <div id="PrintableContent">
        <br />
            <table style="border: thin ridge #000000; width:705px; margin: 0 auto; height:auto">
                <%--HEADER--%>
                <tr class="heading">
                    <td 
                        class="style1">
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
                <td>
                <table>
                <tr>
                <td style="text-align: right" class="style3">
                <ext:Label ID="lblDateBilled" runat="server"></ext:Label>
                </td>
                </tr>
                </table>
                </td>
                </tr>
                <tr>
                <td style="text-align: center; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">

                    <b>BILL STATEMENT</b>

                </td>
                </tr>
                <tr>
                    <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                        <table style="width: 691px">
                            <tr>
                            <td id="namewidth">
                            Name:
                            <ext:Label ID="lblBorrowerName" runat="server"></ext:Label>
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                            <tr>
                                <td class="style2">
                                    Last Payment Made:
                                    <ext:Label ID="lblLastPayment" runat="server"/>
                                </td>
                                <td class="secondrow">
                                    Principal Paid:
                                    <ext:Label ID="lblPrincipalPaid" runat="server"/>
                                </td>
                                <td class="secondrow">
                                Interest Paid:
                                <ext:Label ID="lblInterestPaid" runat="server"></ext:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <table style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000">
                            <tr>
                                <td class="secondrow">
                                    Current Principal Due:
                                    <ext:Label ID="lblCurrentPrincipal" runat="server"/>
                                </td>
                                <td class="secondrow">
                                    Current Interest Due:
                                    <ext:Label ID="lblCurrentInterest" runat="server"/>
                                </td>
                                <td class="secondrow">
                                Total Amount Due:
                                <ext:Label ID="lblCurrentDue" runat="server"></ext:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                <td>
                    <br />
                Details:
                    <br />
                    <br />
                </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <ext:GridPanel ID="pnlBillStatement" runat="server" AutoHeight="true" Width="700"
                            ColumnLines="true" BaseCls="cssClass">
                            <Store>
                                <ext:Store runat="server" ID="strBillStatement" RemoteSort="false">
                                    <Listeners>
                                        <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                    </Listeners>
                                    <Reader>
                                        <ext:JsonReader IDProperty="LoanID">
                                            <Fields>
                                                <ext:RecordField Name="LoanID" />
                                                <ext:RecordField Name="DateReleased" Type="Date"/>
                                                <ext:RecordField Name="PrincipalDue" />
                                                <ext:RecordField Name="InterestDue" />
                                                <ext:RecordField Name="TotalAmountDue" />
                                                <ext:RecordField Name="RoleType"></ext:RecordField>
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel runat="server">
                                <Columns>
                                    <ext:Column DataIndex="LoanID" Header="Loan ID"  Css="border-color:Black;" Align="Center" 
                                    Sortable="false" Hideable="false" Locked="true" Wrap="true" Width="80px" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:DateColumn DataIndex="DateReleased" Header="Date Released" Align="Center" Locked="true" 
                                    Wrap="true" Css="border-color: Black;" Sortable="false" Hideable="false" Width="140px" MenuDisabled="true"></ext:DateColumn>
                                    <ext:NumberColumn Header="Principal Due" DataIndex="PrincipalDue" Locked="true" Wrap="true"
                                              Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                                Css="border-color: Black;" Width="130px" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Interest Due" DataIndex="InterestDue" Locked="true" Wrap="true"
                                Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                        Css="border-color: Black;" Width="130px" MenuDisabled="true" />
                                    <ext:NumberColumn Header="Total Amount Due" DataIndex="TotalAmountDue" Locked="true" Wrap="true"
                                   Format=",000.00" Sortable="false" Hideable="false" Align="Center"
                                        Css="border-color: Black;" Width="130px" MenuDisabled="true" />
                                    <ext:Column DataIndex="RoleType" Header="Role Type"  Css="border-color:Black;" Align="Center" 
                                    Sortable="false" Hideable="false" Locked="true" Wrap="true" Width="90px" MenuDisabled="true">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                        </ext:GridPanel>
                        <br/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
