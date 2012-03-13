<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPromissoryNote.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.PrintPromisoryNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Promisory Note</title>
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
            window.print();
            window.proxy.sendToAll('printpromisory', 'printpromisory');
        }

        var close = function () {
            window.proxy.sendToAll('closedpromisory', 'closedpromisory');
            window.proxy.requestClose();
        }

        var onBtnSave = function () {
            var id = hdnLoanApplicationId.getValue();
            X.SaveSignatures(id, {
                success: function () {
                    showAlert('Success', 'Signatures successfully saved');
                    //flUpBorrower.hide();
                    btnPrint.enable();
                }
            });
        }

        var onUploadSuccess = function () {
            btnPrint.disable();
        }
    </script>
    <style type="text/css">
        @media screen
        {
            #toHide { display: block; }
        }
        
        @media print
        {
            #toHide { display: none; }
            #toHide2 { display: none;}
        }
        
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            background-color: #fff !important;
        }
        .box-component
        {
            width: 700px;
            height: 300px;
            padding: 5px 5px 5px 5px;
        }
        .underline 
        {
            text-decoration: underline;
            font-weight:bold;
        }
        .style1
        {
            width: 175px;
            padding-left:10px;
            text-align: center;
        }
        .header
        {
            font-size:medium;
        }
        #PrintableContent
        {
            text-align: center;
        }
        .addressAlign
        {
            text-align: left;
        }
        .container
        {
            padding: 0px 0px 0px 0px;
            text-align: center;
        }
        .style5
        {
            width: 189px;
        }
        .style8
        {
            width: 11px;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden runat="server" ID="hdnLoanApplicationId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnCustomerId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnMode"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnBorrower"></ext:Hidden>
    <div id="toHide">
        <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Padding="20" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Button runat="server" ID="btnSaveSignatures" Text="Save" Icon="Disk">
                            <Listeners>
                                <Click Handler="onBtnSave();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSpacer /><ext:ToolbarSeparator /><ext:ToolbarSpacer />
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer" Disabled="true">
                            <Listeners>
                                <Click Handler="printContent('PrintableContent');"/>
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSpacer /><ext:ToolbarSeparator /><ext:ToolbarSpacer />
                        <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="close();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Panel>
    </div>
    <div id="PrintableContent">
        <table style="width: 610px; margin: 0 auto; font-size:small;">
            <%--CASHIER'S COPY--%>
            <tr>
                <td>
                    <table style="border-color: Black; border-style: solid; border-width: 2px; font-size: 9pt;
                        padding-top: 20px;">
                        <tr class="heading">
                            <td style="text-align: center;">
                                <%--HEADER--%>
                                <b>
                                    <ext:Label ID="lblHeader" runat="server" Text="MN PAMARAN LENDING INVESTORS, Inc."
                                        Cls="header">
                                    </ext:Label>
                                </b>
                                <br />
                                <br />
                                <ext:Label ID="lblFormName" runat="server" Text="PROMISSORY NOTE">
                                </ext:Label>
                                <br />
                                <ext:Label ID="lblSubName" runat="server" Text="(For New Loans / Balance Forward)">
                                </ext:Label>
                                <br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr class="Note">
                            <td style="text-align: justify; padding-left: 5px;">
                                <table>
                                    <tr>
                                        <td>
                                            For value received amounting to &nbsp;&nbsp;
                                        </td>
                                        <td style="width: 200px; text-align: center; text-decoration: underline;">
                                            <ext:Label runat="server" ID="lblLoanAmountWords" Cls="underline">
                                            </ext:Label>
                                        </td>
                                        <td style="width: 120px; text-align: center;">
                                            &nbsp;&nbsp;<ext:Label runat="server" ID="lblLoanAmount" Cls="underline">
                                            </ext:Label>
                                        </td>
                                        <td>
                                            , I promise
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            to pay to MNPLI, Inc. upon the receipt of my monthly checks starting from the month of
                                        </td>
                                        <td style="width: 95px; text-align: center; text-decoration: underline;">
                                            <ext:Label runat="server" ID="lblmonthTerm" Cls="underline">
                                            </ext:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                             with an interest rate of
                                        </td>
                                        <td style="width: 70px; text-align: center; text-decoration: underline;">
                                            <ext:Label runat="server" ID="lblRate" Cls="underline">
                                            </ext:Label>
                                        </td>
                                        <td>
                                            per month until my obligation is fully paid. I am authorizing MNPLI, Inc.
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                               to collect/sign and encash the said checks.<br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="style1">
                                            <table>
                                                <tr>
                                                    <td class="style5">
                                                        <b>
                                                            <ext:Label runat="server" ID="lblCoborrower">
                                                            </ext:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5">
                                                        <b>
                                                            <ext:Label runat="server" ID="lblGuarantor">
                                                            </ext:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                            </table>
                                            _________________________
                                            <br />
                                            Guarantor/Co-debtor
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                        <td style="padding-left: 20px;">
                                            <table>
                                                <tr>
                                                    <td><br />Print Name/Signature:&nbsp;</td>
                                                    <td style="border-bottom: 1px solid Black;">
                                                        <ext:Image runat="server" ID="imgBorrower" Width="150" Height="40"  ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <div id="toHide2"><ext:FileUploadField runat="server" ID="flUpBorrower" Width="150">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadBorrower" Success="onUploadSuccess"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        </div>
                                                        <center><ext:Label runat="server" ID="lblName">
                                                        </ext:Label></center>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        District:&nbsp;
                                                    </td>
                                                    <td style="border-bottom: 1px solid Black;">
                                                        <ext:Label runat="server" ID="lblDistrict">
                                                        </ext:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Address: &nbsp;
                                                    </td>
                                                    <td class="addressAlign">
                                                        <ext:Label runat="server" ID="lblAddress" Cls="underline">
                                                        </ext:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />Contact#: &nbsp;<br />
                                                    </td>
                                                    <td>
                                                        <ext:Label runat="server" ID="lblContact" Cls="underline">
                                                        </ext:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding: 10px;">
                                            &nbsp;<ext:Label runat="server" ID="lblDate" Cls="underline">
                                            </ext:Label>
                                            <br />
                                            <center>Date</center>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <ext:Label ID="Label1" runat="server">
                                            </ext:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                       <tr>
                            <td style="text-align: left; padding-left: 12px;">White: &nbsp;&nbsp; Cashier's Copy</td>
                            <td style="text-align: right; width: 390px;">No: &nbsp;&nbsp;</td>
                            <td><asp:Label runat="server" ID="lblControlNum"></asp:Label></td>
                       </tr>
                       <tr>
                            <td style="text-align: left; padding-left: 12px;">Yellow: &nbsp;&nbsp;Customer's Copy</td>
                       </tr>
                       <tr>
                            <td style="text-align: left; padding-left: 12px;">Blue: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; File Copy</td>
                       </tr>
                    </table>
                    <%--<table>
                        <tr class="style6">
                            <td>
                                <ext:Label runat="server" ID="lblCopy" Text="Cashier's Copy" FieldLabel="White" LabelWidth="50"></ext:Label>
                            </td>
                            <td class="style4">
                                <ext:Label runat="server" ID="lblControlNumber" FieldLabel="No"></ext:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style6">
                                <ext:Label runat="server" ID="lblCopy1" Text="Customer's Copy" FieldLabel="Yellow"></ext:Label>
                            </td>
                        </tr>
                        <tr>
                            <td  class="style6">
                                <ext:Label runat="server" ID="lblCopy2" Text="File Copy" FieldLabel="Blue"></ext:Label><br />                                
                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>