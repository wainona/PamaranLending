<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailedAgingSummary.aspx.cs" Inherits="LendingApplication.Applications.AgingOfAccountsUseCases.DetailedAgingSummary" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detailed Aging of Accounts Summary</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
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

        var onSelectDate = function () {
            X.FillTable();
        };

    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 0px;
            font-size        : 15px;
	        background-color : #fff !important;
        }
        .style3
        {
            height: 59px;
        }
        .gridStyle
        {
            margin-left: 0px;
            margin-top: 0px;
        }
        .gridStyle2
        {
            margin-left: 250px;
        }
        .grid
        {
            margin-left: 250px;
            width: 500px;
        }
        .grid1
        {
            margin-left: 214px;
            width: 450px;
        }
        .headerClass
        {
            font-weight: bold;
            text-align: center;
        }
        .style13
        {
            width: 20px;
            height: 11px;
        }
        .fontbold
        {
            font-weight: bold;
        }
        
        .cssClass .x-grid3-row
        {
            border-left-color: Black;
            border-bottom-color: Black;
            border-width: 1px;
            border-style: solid;
            font-size: 13px;
        }
        .cssClass .x-grid3-hd
        {
            font-weight: bold;
            border-left-color: Black;
            border-bottom-color: Black;
            border-top-color: Black;
            border-right-color:Black;
            border-width: 1px;
            border-style: solid;
            white-space:  normal !important;
        }
        
        .cssClass .x-grid3-hd-inner, .x-grid3-cell-inner
        {
            white-space:  normal !important;
            font-size: 12px;
            font-family: helvetica,tahoma,verdana,sans-serif;
            vertical-align: middle;
        }
        
        .cssClass .x-grid3-header
        {
            background-image: none;
            background-color: transparent;
            white-space:  normal !important;
        }
        
        .cssClass .x-grid3-hd-row td : hover
        {
            background-image: none;
            background-color: White;
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
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenPaymentId" runat="server"/>
    <ext:Hidden ID="hiddenDate" runat="server" />
    <div id="toolBar">
    <ext:FormPanel runat="server" LabelWidth="100" Border="false">
        <TopBar>
        <ext:Toolbar runat="server" ID="PageToolBar">
            <Items>
                <ext:DateField ID="datSelectedDate" FieldLabel="Selected Date" LabelWidth="100" runat="server" Width="200" Editable="false" >
                </ext:DateField>
                <ext:Button ID="btnGenerate" runat="server" Text="Generate">
                    <Listeners>
                        <Click Handler="onSelectDate();" />
                    </Listeners>
                </ext:Button>
                <ext:Hidden ID="hdnLoanId" runat="server" />
                <ext:Hidden ID="hdnAgreementId" runat="server"></ext:Hidden>
                <ext:ToolbarFill ID="ctl778" />
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
        <table style="width:823px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;" colspan="9">
                        <ext:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true"/><br />
                        <ext:Label ID="lblStreetAddress" runat="server" />&nbsp;<ext:Label ID="lblBarangay" runat="server" /><br />
                        <ext:Label ID="lblMunicipality" runat="server" /><ext:Label ID="lblCity" runat="server" />&nbsp;<ext:Label ID="lblProvince" runat="server" />&nbsp;<ext:Label ID="lblCountry" runat="server" />&nbsp;<ext:Label ID="lblPostalCode" runat="server" /><br />
                        Tel#: <ext:Label ID="lblPrimTelNumber" runat="server" />&nbsp;<ext:Label ID="lblSecTelNumber" runat="server" /><br />
                        Fax#: <ext:Label ID="lblFaxNumber" runat="server" /><br />
                        <ext:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align:center; vertical-align:bottom; font-weight:bold; border-bottom-color: Gray; border-bottom-width:thin; text-transform:uppercase;" class="style3" colspan="9">
                        <br />
                        DETAILED AGING OF ACCOUNTS AS OF 
                        <ext:Label ID="lblSelectedDate" LabelAlign="Top" runat="server"/>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr id="Tr1" align="center">
                   <td style="text-align:center; margin: 0 auto;" colspan="9">
                       <ext:GridPanel ID="GridPanel1" Border="true" Width="823px" ColumnLines="true" BaseCls="cssClass" runat="server" Layout="FitLayout" AutoHeight="true">
                            <Store>
                                <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="AccountName" />
                                                <ext:RecordField Name="LoanReleaseDate1" Type="Date" />
                                                <ext:RecordField Name="LoanReleaseDate"/>
                                                <ext:RecordField Name="DueDate1" Type="Date" />
                                                <ext:RecordField Name="LastPaymentDate" />
                                                <ext:RecordField Name="Balance" />
                                                <ext:RecordField Name="_Current" />
                                                <ext:RecordField Name="_OneToThirty" />
                                                <ext:RecordField Name="_ThirtyOneToSixty" />
                                                <ext:RecordField Name="_SixtyOneToNinety" />
                                                <ext:RecordField Name="_OverNinety" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true"  >
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <ColumnModel runat="server" ID="PageGridPanelColumnModel" Width="100%">
                                <Columns>
                                    <ext:Column Header="Account Name" Hideable="false" Sortable="false" DataIndex="AccountName" Align="Left" MenuDisabled="true" Css="font-weight:bold; text-align:left; border-color: Black; white-space: normal !important;" Wrap="true" Locked="true" Width="148px" />
                                    <ext:Column Header="Loan Release Date" Hideable="false" Sortable="false" DataIndex="LoanReleaseDate" MenuDisabled="true" Wrap="true" Align="Center" Css="border-color: Black; white-space: normal !important;" Locked="true" Width="85px"/>
                                    <%--<ext:DateColumn Header="Last Payment Date" Hideable="false" Sortable="false" DataIndex="DueDate1" Wrap="true" Align="Center" Css="border-color: Black;" Locked="true" Width="120px" Format="MMMM dd, yyyy" />--%>
                                    <ext:Column Header="Last Payment Date" Hideable="false" Sortable="false" DataIndex="LastPaymentDate" MenuDisabled="true" Wrap="true" Align="Center" Css="border-color: Black;" Locked="true" Width="90px" />
                                    <ext:NumberColumn Header="Balance" Hideable="false" Sortable="false" DataIndex="Balance" Format=",000.00" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true" 
                                        Width="83px" />
                                    <ext:Column Header="Current" Hideable="false" Sortable="false" DataIndex="_Current" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="83px" />
                                    <ext:Column Header="1-30 Days" Hideable="false" Sortable="false" DataIndex="_OneToThirty" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="83px" />
                                    <ext:Column Header="31-60 Days" Hideable="false" Sortable="false" DataIndex="_ThirtyOneToSixty" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="83px" />
                                    <ext:Column Header="61-90 Days" Hideable="false" Sortable="false" DataIndex="_SixtyOneToNinety" MenuDisabled="true" Align="Center" Css="border-color: Black;" Locked="true" Wrap="true"
                                        Width="83px" />
                                    <ext:Column Header="Over 90 Days" Hideable="false" Sortable="false" DataIndex="_OverNinety" Align="Center" Css="border-color: Black;" MenuDisabled="true" Locked="true" Wrap="true"
                                        Width="83px" />
                                </Columns>
                            </ColumnModel>
                       </ext:GridPanel>
                    </td>
                </tr>
<%--                <tr style="height:15px;">
                    <td style="width: 160px;" valign="bottom" ><asp:Label ID="lblAcountTotalLabel" runat="server" 
                            Font-Bold="True" Width="160px" Text="" /></td>
                    <td style=" width:40px;" align="center" valign="bottom"><asp:Label ID="lbl" runat="server" Width="40px" /></td>
                    <td style=" width:44px;" align="center" valign="bottom"><asp:Label ID="Label1" runat="server" Width="44px" /></td>
                    <td style=" width:97px;" align="center" valign="bottom">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblBalanceTotal" runat="server" 
                            Width="97px" /></td>
                    <td style=" width:96px;" align="center" valign="bottom">&nbsp;&nbsp;<asp:Label ID="lblCurrentTotal" runat="server" 
                            Width="96px" /></td>
                    <td style=" width:102px;" align="center" valign="bottom">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblOneThirtyTotal" runat="server" 
                            Width="102px" /></td>
                    <td style=" width:118px;" align="center" valign="bottom">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblThirtyOneSixtyTotal" runat="server" 
                            Width="118px" /></td>
                    <td style=" width:120px;" align="center" valign="bottom">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblSixtyOneNinetyTotal" runat="server" 
                            Width="120px" /></td>
                    <td style=" width:132px;" align="center" valign="bottom">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblOverNinetyTotal" runat="server" 
                            Width="132px" /></td>
                </tr>--%>
            </table>
    </div>
    </form>
</body>
</html>
