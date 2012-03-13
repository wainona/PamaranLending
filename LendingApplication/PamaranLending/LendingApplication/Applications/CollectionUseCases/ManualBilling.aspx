<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualBilling.aspx.cs"
    Inherits="LendingApplication.Applications.FinancialManagement.CollectionUseCases.ManualBilling" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var close = function () {
            window.proxy.sendToParent('closemanualbilling', 'closemanualbilling')
            window.proxy.requestClose();
        }

        var saveSuccessful = function () {
            showAlert('Status', 'Receivable records was successfully created.', function () {
                var data = {};
                data.bill = 1;
                window.proxy.sendToAll(data, 'manualbill');
                window.proxy.requestClose();
            });
        }
        var generateBill = function () {
            X.GenerateBill();
        };
    </script>
    <style>
        .transparent
        {
            background-color: transparent;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel ID="RootPanel" runat="server" Layout="FitLayout" Border="false">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();">
                                    <EventMask ShowMask="true" Msg="Saving..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="close();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                <%--HIDDEN FIELDS--%>
                <ext:Hidden ID="hiddenParentResource" runat="server"></ext:Hidden>
                   <ext:Hidden ID="hiddenRandomKey" runat="server"></ext:Hidden>
                  <ext:Hidden ID="hiddenType" runat="server"></ext:Hidden>
                  <ext:Hidden ID="hiddenLoanID" runat="server"></ext:Hidden>
                  <ext:Hidden ID="hiddenAgreementID" runat="server"/>
                <%--HIDDEN FIELDS--%>
                    <ext:Panel runat="server" ID="PanelPaymentSchedule" Title="Payment Schedule" Border="false"
                        Padding="5" LabelWidth="150" LabelAlign="Right">
                        <Items>
                            <ext:Panel runat="server" LabelWidth="150" Layout="FormLayout" Border="false">
                                <Items>
                                <ext:DateField FieldLabel="Date" ID="dtBill" runat="server" AnchorHorizontal="60%">
                                <Listeners>
                                <Select Handler="generateBill();" />
                                </Listeners>
                                </ext:DateField>
                                <ext:TextField ID="txtTotalLoanBalance" ReadOnly="true" runat="server" FieldLabel="Remaining Loan Balance  (Php)"
                                        AnchorHorizontal="60%"/>
                                    <ext:TextField ID="txtInterestPayment" ReadOnly="true" runat="server" FieldLabel="Computed Interest Due  (Php)"
                                        AnchorHorizontal="60%" />
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
