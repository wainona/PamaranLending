<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListOfHowTos.aspx.cs"
    Inherits="LendingApplication.MNPamaranHowTo.ListOfHowTos" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aging of Accounts Summary</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/js/main.js" type="text/javascript"></script>
    <script src="../resources/js/tabManager.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var onHelpClick = function (name, title, folder) {
            if(folder == "")
                url = '/MNPamaranHowTo/' + name + '.pdf';
            else
                url = '/MNPamaranHowTo/' + folder + '/' + name + '.pdf';
            param = url;
            window.proxy.requestNewTab(name, param, title);
        };

        var onTreeHelpClick = function (url, title, name) {
            param = url;
            window.proxy.requestNewTab(name, param, title);
        };
    

        var onDateChange = function () {
            X.FillDateSelected();
        };

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
    </style>
    <style media="print" type="text/css">
        @page
        {
            size: 8.5in 14in;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Hidden ID="hiddenDate" runat="server" />
    <ext:ViewPort runat="server" Layout="FitLayout"><Items>
    <ext:Panel ID="PageFormPanel1" runat="server" PaddingSummary="10px 0px 0px 0px"
        BodyStyle="background-color:transparent" Border="false" LabelWidth="350" Layout="FitLayout">
        <%--<Content>                 
                <ext:LinkButton ID="HowToUseBankNode" runat="server" Hidden="true" Text="How to use Bank Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseCashOnVaultNode" runat="server" Hidden="true" Text="How to use Cash on Vault Node"  Icon="BulletBlack">
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseChequeEditor" runat="server" Text="How to use Cheque Editor Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseChequesNode" runat="server" Hidden="true" Text="How to use Cheques Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>             
                <ext:LinkButton ID="HowToUseContactsNode" runat="server" Text="How to use Contacts Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToAddLoanDisbursement" Hidden="false" runat="server" Text="How to Create Loan Disbursement" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), 'Disbursements');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseEmployeesNode" runat="server" Text="How to use Employees Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseLoanProductsNode" runat="server" Text="How to use Loan Products Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseReceiptsNode" runat="server" Text="How to use Receipts Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
                <br />  
                <br />              
                <ext:LinkButton ID="HowToUseUserAccountNode" runat="server" Text="How to use UserAccount Node" Icon="BulletBlack" >
                    <Listeners>
                        <Click Handler="onHelpClick(this.getId(), this.getText(), '');" />
                    </Listeners>
                </ext:LinkButton>
        </Content>--%>
        <Items>
            <ext:TreePanel ID="NavigationTree" runat="server" Footer="false" AutoScroll="true" Padding="2" Border="false" Frame="false">
                <Root>
                            
                </Root>
                <Loader>
                    <ext:PageTreeLoader RequestMethod="GET" PreloadChildren="true">
                        <EventMask ShowMask="true" Target="Parent" Msg="Loading..." />
                        <BaseAttributes>
                            <ext:Parameter Name="singleClickExpand" Value="true" Mode="Raw" />
                            <ext:Parameter Name="loaded" Value="true" Mode="Raw" />
                        </BaseAttributes>
                    </ext:PageTreeLoader>
                </Loader>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar" runat="server" AutoClear="1500" />
                </BottomBar>
                <Listeners>
                    <Click Handler="if (node.isLeaf()) { e.stopEvent(); onTreeHelpClick(node.attributes.href, node.text, node.id);};" />
                </Listeners>
            </ext:TreePanel>
        </Items>
    </ext:Panel>
    </Items></ext:ViewPort>
    </form>
</body>
</html>
