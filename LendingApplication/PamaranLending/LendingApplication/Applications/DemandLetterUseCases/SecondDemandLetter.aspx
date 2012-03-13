<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondDemandLetter.aspx.cs" Inherits="LendingApplication.Applications.DemandLetterUseCases.SecondDemandLetter" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            //PageToolBar.hide();
            window.print();
            //PageToolBar.show();
        }
    </script>
    <style type="text/css">
       .bold
        {
            font-weight: bold;
            font-size: medium;
        }
        .rightAlign
        {
            float: right;
        }
        .justified 
        {
            text-align: justify;
        }
        .gridStyle
        {
           margin-left: 50px;
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <div id="toolBar">
    <ext:Panel ID="MainPanel" runat="server" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer">
                        <Listeners>
                            <Click Handler="printContent('PrintableDiv');" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:Panel>
    </div>
    <div id="PrintableDiv" style="font-family: Tahoma; font-size: small;">
        <table style="width: 700px; margin: 0 auto;">
            <tr>
                <td class="justified" style="vertical-align:bottom;">
        <br />
        <center>
            <ext:Label ID="lblLenderName" runat="server" Cls="bold"/><br />
            <ext:Label ID="lblLenderAddress" runat="server" /><br />
            <ext:Label ID="lblLenderTelephoneNumber" runat="server" />&nbsp;
            <ext:Label ID="lblLenderFaxNumber" runat="server" /><br />
        </center>
        <br /><br />
        <ext:Label ID="lblDateToday" runat="server" Cls="rightAlign"/>
        <br /><br />
        <ext:Label ID="lblOwnerName" runat="server" Text="LastName, FirstName M." />
        <br />
        <ext:Label ID="lblOwnerAddress" runat="server" Text="123 New Street, San Pedro District, Pagadian City, Zamboanga Del Sur, Philippines, 6066" />
        <br /><br /><br />
        Dear Mr./Ms. <ext:Label ID="lblOwnerNameAddressed" runat="server" Text="Mr. Last Name" />,
        <br /><br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        This has reference to your Promissory Note for <u><ext:Label ID="lblLoanAmount" runat="server" /></u> dated <u><ext:Label ID="lblAgreementDate" runat="server" /></u> executed in favor of <u><ext:Label ID="lblOwnerName2" runat="server" /></u>. 
        In the interest of updating your account, the status is presented below:
        <br /><br />
        Outstanding Balance <ext:Label ID="lblOutstandingBalance" runat="server" Cls="rightAlign"/>
        <br /><br />
        Installment(s) Due (excluding past due interest and penalties)
        <br /><br />
        <ext:GridPanel ID="GridPanelInstallmentsDue" runat="server" EnableColumnMove="false" AutoHeight="true" Border="false">
            <Store>
                <ext:Store ID="InstallmentsDueStore" runat="server">
                    <Reader>
                        <ext:JsonReader>
                            <Fields>
                                <ext:RecordField Name="DueDate" Type="Date"/>
                                <ext:RecordField Name="AmountDue" />
                            </Fields>
                        </ext:JsonReader>
                    </Reader>
                </ext:Store>
            </Store>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:DateColumn Header="Due Dates" DataIndex="DueDate" Format="MMMM dd, yyyy" Width="364" />
                    <ext:NumberColumn Header="Amount Due" DataIndex="AmountDue" Width="364" Format=",000.00"/>
                </Columns>
            </ColumnModel>
        </ext:GridPanel>
        <br /><br />
        TOTAL INSTALLMENTS DUE <ext:Label ID="lblTotalIntallmentsDue" runat="server" Cls="rightAlign"/>
        <br />
        (excluding past due interest and penalties)
        <br /><br />
        <p class="justified">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Despite our previous letter to you, the past due installment(s) have remained unpaid.</p>
        <br />
	    <p class="justified">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            We now have the option to demand the entire outstanding balance plus accruing past due interest rate and penalties due and demandable.</p>
	    <br />
        <p class="justified">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Although we dislike to press you, we are sure you agree that we have been patient and reasonable in waiting for you to update your account. Now that we need your assistance, could you help us by updating your account with us?</p>
	    <br />
        <p class="justified">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            It is our desire to do you no injustice by taking summary action in a legal way which might reflect unnecessarily upon your credit standing so we offer you this opportunity to take care of your account. Failure to settle your account within THREE (3) DAYS from receipt of this letter will constrain us to institute legal action which shall entail legal costs and attorney’s fees.</p>
	    <br />
        <p class="justified">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            We trust you understand our situation and give this matter your preferential attention.</p>
        <br /><br />
            </td>
        </tr>
            <tr>
                <td class="rightAlign">
                    Very truly yours,<br /><br />
                    <ext:Label ID="lblLenderName2" runat="server" /><br /><br />
                    By:<br /><br />
                    <center>______________________________</center>
                    <center>Manager</center>
                </td>
            </tr>
            <tr>
                <td>
                    <br /><br /><br />
                    Cc:
                    <asp:GridView CssClass="gridStyle" runat="server" ID="grdCoOwner" Font-Size="Small" 
                        AutoGenerateColumns="False" Width="350" ShowHeader="false" BorderColor="Transparent" 
                        EditRowStyle-BorderColor="Transparent" RowStyle-Font-Underline="true" RowStyle-BorderColor="Transparent" 
                        GridLines="None" >
                        <Columns>
                            <asp:BoundField DataField="NameAddress" HeaderText=""/>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        
        
    </div>
    </form>
</body>
</html>
