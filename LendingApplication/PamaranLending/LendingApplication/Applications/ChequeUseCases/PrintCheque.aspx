<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintCheque.aspx.cs" Inherits="LendingApplication.Applications.ChequeUseCases.PrintCheque" %>

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
        var origWidth = 8;
        var origHeight = 3;

        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['printselected']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'printselected') {
                var amount = msg.data.Amount;
            }
        };

        function printContent(printpage) {
            window.print();
            window.proxy.sendToAll('printcheque', 'printcheque');
        }

        var close = function () {
            window.proxy.sendToAll('closedcheque', 'closedcheque');
            window.proxy.requestClose();
        }

        var resetCheck = function () {
            var width = nfWidth.getValue();
            var height = nfHeight.getValue();
            var signature1 = "";
            var signature2 = "";
            var widthAdd = 0;
            var heightAdd = 0;

            widthAdd = parseFloat(width) - parseFloat(origWidth);
            widthAdd = widthAdd * 72;


            heightAdd = parseFloat(height) - parseFloat(origHeight);
            heightAdd = heightAdd * 20;


            if (width != '') {
                printableCheck.style.setProperty("width", width + "in");
                origWidth = width;
                var payeeWidth = payeeName.style.width.replace("px", "");
                var amountWidth = amountInWords.style.width.replace("px", "");
                payeeWidth = parseFloat(payeeWidth) + widthAdd;
                payeeWidth = payeeWidth + "px";
                amountWidth = parseFloat(amountWidth) + widthAdd;
                amountWidth = amountWidth + "px";
                payeeName.style.setProperty("width", payeeWidth);
                amountInWords.style.setProperty("width", amountWidth);


                var signatureRight = signature.style.getPropertyValue("margin-right").replace("px", "");
                signatureRight = parseFloat(signatureRight) + widthAdd;
                signatureRight = signatureRight + "px";
                signature.style.setProperty("margin-right", signatureRight);

                var signatureLeft = signatureDiv.style.getPropertyValue("width").replace("px", "");
                signatureLeft = parseFloat(signatureLeft) + (widthAdd * 1.5);
                signatureLeft = signatureLeft + "px";
                signatureDiv.style.setProperty("width", signatureLeft);
                signatureDiv1.style.setProperty("width", signatureLeft);
            }

            if (height != '') {
                printableCheck.style.setProperty("height", height + "in");
                origHeight = height;
                var amountNumWidth = amountInNumbers.style.getPropertyValue("margin-top").replace("px", "");
                amountNumWidth = parseFloat(amountNumWidth) + heightAdd;
                amountNumWidth = amountNumWidth + "px";
                amountInNumbers.style.setProperty("margin-top", amountNumWidth);
                var payeeWidth = payeeName.style.getPropertyValue("margin-top").replace("px", "");
                payeeWidth = parseFloat(payeeWidth) + heightAdd;
                payeeWidth = payeeWidth + "px";
                payeeName.style.setProperty("margin-top", payeeWidth);
                var amountWidth = amountInWords.style.getPropertyValue("margin-top").replace("px", "");
                amountWidth = parseFloat(amountWidth) + heightAdd;
                amountWidth = amountWidth + "px";
                amountInWords.style.setProperty("margin-top", amountWidth);


                var signatureWidth = signature.style.getPropertyValue("margin-bottom").replace("px", "");
                signatureWidth = parseFloat(signatureWidth) + heightAdd;
                signatureWidth = signatureWidth + "px";
                signature.style.setProperty("margin-bottom", signatureWidth);

                var signatureHeight = signature.style.getPropertyValue("margin-top").replace("px", "");
                signatureHeight = parseFloat(signatureHeight) + (heightAdd * 2);
                signatureHeight = signatureHeight + "px";
                signature.style.setProperty("margin-top", signatureHeight);

            }

            if (signature1 != '' && signature2 != '') {

            }
        };

    </script>
    <style type="text/css">
        @media screen
        {
            #toHide { display: block; }
            #cheque { border: 1px solid Black; }
            .toHide { display: block; }
            .noUnderline { border-bottom: 1px solid Black; }
            .whiteFont { color: Black; }
            .tdHide { color: Black; }
            .signatures { text-decoration: none; }
            
        }
        
        @media print
        {
            #toHide { display: none; }
            #cheque { border: 1px solid transparent; }
            .toHide { display: none; }
            .noUnderline { border: 1px solid transparent; }
            .whiteFont { color: Transparent; }
            .tdHide { color: Transparent; }
            .signatures { text-decoration: none; }
        }
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 11px;
            background-color: #fff !important;
        }
        .box-component
        {
            width: 750px;
            height: 300px;
            padding: 5px 5px 5px 5px;
        }
        .underline 
        {
            text-decoration: underline;
            font-weight:bold;
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
        .labelsize
        {
            font-size: 10pt;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <%--Signatories Store--%>
    <ext:Store runat="server" ID="SignatoryStore" RemoteSort="false">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div id="toHide">
        <ext:Panel ID="Panel1" runat="server" Layout="FitLayout" Padding="20" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:NumberField runat="server" ID="nfWidth" LabelWidth="63" FieldLabel="Width (inch)" Width="120" DecimalPrecision="4" Number="8" />
                        <ext:ToolbarSpacer runat="server" />
                        <ext:NumberField runat="server" ID="nfHeight" LabelWidth="63" FieldLabel="Height (inch)" Width="120" DecimalPrecision="4" Number="3" />
                        <ext:ToolbarSeparator runat="server" />
                        <ext:Button runat="server" ID="btnGenerate" Text="Update Check" Icon="FolderGo">
                            <Listeners>
                                <Click Handler="resetCheck();"/>
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarFill runat="server" />
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                            <Listeners>
                                <Click Handler="printContent('PrintableContent');"/>
                            </Listeners>
                        </ext:Button>
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
        <table id="cheque" style="margin: 0 auto; font-size:small;">
            <tr>
                <td>
                    <div id="printableCheck" style="width: 8in; height: 3in">
                        <table class="whiteFont" id="bankName" style="margin-left: 40px; margin-top: 10px; margin-right: 15px; margin-bottom: 0px;">
                            <tr>
                                <td><ext:Label runat="server" ID="lblBankName" Cls="labelsize"></ext:Label></td>
                            </tr>
                        </table>
                        <table style="margin-left: 60px; margin-top: 0px; margin-right: 15px; padding: 0;">
                            <tr>
                                <td style="width: 160px;"></td><td style="width: 340px;"></td>
                                <td class="whiteFont" >Date</td>
                                <td><table class="noUnderline" style=" width: 180px; text-align: right;">
                                    <tr><td></td></tr><tr><td></td></tr>
                                    <tr><td></td></tr><tr><td></td></tr>
                                    <tr><td style="text-align: right;"><ext:Label runat="server" ID="lblDate" Cls="labelsize"></ext:Label></td></tr>
                                </table></td>
                            </tr>
                            <tr>
                                <td class="whiteFont" style="width: 130px; text-align: left;">PAY TO THE ORDER OF</td>
                                <td><table class="noUnderline" id="payeeName" style=" width: 360px; margin-top: 0px;">
                                    <tr><td><center><ext:Label runat="server" ID="lblName"  Cls="labelsize"></ext:Label></center></td></tr>
                                </table></td>
                                <td class="whiteFont">&nbsp;&nbsp; </td>
                                <td><table class="noUnderline" id="amountInNumbers" style=" width: 180px; margin-top: 0px;">
                                    <tr><td style="text-align: right;"><ext:Label runat="server" ID="lblAmount"  Cls="labelsize"></ext:Label></td></tr></table>
                                </td>
                                <td> &nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="whiteFont" style="width: 130px; text-align: left;"><ext:Label runat="server" ID="lblCurrency"></ext:Label></td>
                                <td colspan="4">
                                    <table class="noUnderline" id="amountInWords" style=" width: 580px; margin-top: 0px;">
                                        <tr><td><center><ext:Label runat="server" ID="lblAmountInWords"  Cls="labelsize"></ext:Label></center></td></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table class="" id="signature" style="margin-left: 30px; margin-top: 60px; margin-right: 30px; margin-bottom: 0px;  width: 17cm;">
                            <tr class="signatures" style="text-align: center;  width: 800px;">
                                <td><div id="signatureDiv" style="width: 292px;"></div></td>
                                <td class="noUnderline"><ext:Label runat="server" ID="lblSignature1" Width="174" Text="____________________">
                                        <Editor>
                                            <ext:Editor runat="server" ID="edtSignature1" CancelOnEsc="true" Width="174">
                                                <Field>
                                                    <ext:ComboBox runat="server" Editable="false" StoreID="SignatoryStore" ValueField="Name" 
                                                        DisplayField="Name" TriggerAction="All" ID="ctl2810" 
                                                        HiddenName="ctl2810_Value" Width="174">
                                                            <Template runat="server" Visible="False" ID="ctl2812" 
                                                                StopIDModeInheritance="False" EnableViewState="False"></Template>
                                                    </ext:ComboBox>
                                                </Field>
                                            </ext:Editor>
                                        </Editor>
                                    </ext:Label>
                                </td>
                                <td class="noUnderline"><ext:Label runat="server" ID="lblSignature2" Width="174" Text="____________________">
                                        <Editor>
                                            <ext:Editor runat="server" ID="edtSignature2" CancelOnEsc="true" Width="174">
                                                <Field>
                                                    <ext:ComboBox ID="ComboBox1" runat="server" Editable="false" StoreID="SignatoryStore" ValueField="Name" 
                                                        DisplayField="Name" TriggerAction="All" Width="174">
                                                        <Template runat="server" Visible="False" ID="ctl2816" StopIDModeInheritance="False" EnableViewState="False"></Template>
                                                    </ext:ComboBox>
                                                </Field>
                                            </ext:Editor>
                                        </Editor>
                                    </ext:Label>
                                </td>
                                <td><div id="Div2" style="width: 160px;"></div></td>
                            </tr>
                            <tr style="text-align: center;  width: 800px;">
                                <td><div id="signatureDiv1" style="width: 292px;"></div></td>
                                <td class="tdHide" style="width: 174px;">Authorized Signature</td>
                                <td class="tdHide" style="width: 174px;">Authorized Signature</td>
                                <td><div id="Div1" style="width: 160px;"></div></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
