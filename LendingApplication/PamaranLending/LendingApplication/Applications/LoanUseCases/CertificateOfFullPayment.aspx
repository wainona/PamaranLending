<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateOfFullPayment.aspx.cs" Inherits="LendingApplication.Applications.LoanUseCases.CertificateOfFullPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Certificate Of Full Payment</title>
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
            PageToolBar.hide();
            window.print();
            PageToolBar.show();
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
        .style1
        {
            width: 444px;
        }
        .style2
        {
            width: 237px;
            text-align: center;
        }
          .txtfield
        {
           border-style:solid;
           background-image:none;
           font-weight: bold;
           text-align: center;
           border-color: transparent;
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
        <ext:FormPanel runat="server" Border="false">
        <TopBar>
            <ext:Toolbar ID="PageToolBar" runat="server">
                <Items>
                    <ext:Button ID="btnPrint" runat="server" Text="Print" Icon="Printer">
                        <Listeners>
                            <Click Handler="printContent('PrintableContent');" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" />
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
            <table style="width:700px; margin: 0 auto;">
                <tr class="heading">
                    <td style="text-align:center;">
                        <br /><br /><br /><br /><br /><br />
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" /><br />
                        <asp:Label ID="lblStreetAddress" runat="server" />, <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />, <asp:Label ID="lblProvince" runat="server" />, <asp:Label ID="lblCountry" runat="server" />, <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#: <asp:Label ID="lblPrimTelNumber" runat="server" />, <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br /><br /><br />
                    </td>
                </tr>
                <tr class="date">
                    <td style="text-align:right;">
                        <asp:Label runat="server" ID="lblCurrentDate"></asp:Label>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr class="nameOfDOcument">
                    <td style="text-align:center; font-weight:bold;">
                        Certificate of Full Payment
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr class="body">
                    <td style="text-align:justify;">
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        This is to certify that
                        <asp:Label ID="lblAccountOwner" runat="server" Font-Underline="true"/>
                        of
                        <asp:Label ID="lblOwnerAddress" runat="server" Font-Underline="true"/>
                        has completely paid his/her account to
                        <asp:Label ID="lblLenderNameBody" runat="server"/>
                        on <asp:Label ID="lblDatePaidOff" runat="server" Font-Underline="true"/>.
                        <br /><br />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        This certification is issued upon request as to whatever purpose it may serve.
                        <br /><br /><br /><br /><br /><br />
                    </td>
                </tr>
             
                <tr class="managerSignature">
                    <td style="text-align:right;">
                    <table>
                    <tr>
                    <td class="style1">
                    </td>
                    <td class="style2" 
                            style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #000000;">
                        <ext:TextField ID="txtManager" runat="server" Cls="txtfield" Width="237"></ext:TextField>
                    </td>
                    </tr>
                     <tr>
                    <td class="style1">
                    </td>
                    <td class="style2">
                        Manager
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
