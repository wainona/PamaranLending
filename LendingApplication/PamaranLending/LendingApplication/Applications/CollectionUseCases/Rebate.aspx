<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rebate.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.Rebate" %>

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

        var jpScriptCheckValue = function () {
            formatCurrency(txtInterestDue2);
            formatCurrency(txtPrincipalDue2);
            var interest1 = txtInterestDue.getValue();
            var interest2 = txtInterestDue2.getValue();
            interest2 = interest2.replace(/,/g, '');
            interest1 = interest1.replace(/,/g, '');
            var principalDue1 = txtPrincipalDue.getValue();
            principalDue1 = principalDue1.replace(/,/g, '');
            var principalDue2 = txtPrincipalDue2.getValue();
            principalDue2 = principalDue2.replace(/,/g, '');
            var totalLoanBalance = txtTotalLoanBalance.getValue();
            totalLoanBalance = totalLoanBalance.replace(/,/g, '');
            if (parseFloat(interest1) < parseFloat(interest2) && parseFloat(interest1) > 0) {
                txtTotalLoanBalance2.setValue(null);
                txtInterestDue2.markInvalid('Value must be lesser than the original interest due');
            } else if (parseFloat(interest2) < 0) {
                txtTotalLoanBalance2.setValue(null);
                txtInterestDue2.markInvalid('Invalid value!');
            } else if (parseFloat(principalDue1) < parseFloat(principalDue2) && parseFloat(principalDue1) > 0) {
                txtTotalLoanBalance2.setValue(null);
                txtPrincipalDue2.markInvalid('Value must be lesser than the original principal due');
            } else if (parseFloat(principalDue2) < 0) {
                txtTotalLoanBalance2.setValue(null);
                txtPrincipalDue2.markInvalid('Invalid value!');
            } else {
                var sum = 0;
                var result = 0;
                sum = parseFloat(interest2) + parseFloat(principalDue2);
                result = parseFloat(totalLoanBalance) - sum
                txtTotalLoanBalance2.setValue(String(result));
                formatCurrency(txtTotalLoanBalance2);
            }
        }

         var formatCurrency = function(txt) {
            var num = txt.getValue();
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
            var answer = (((sign) ? '' : '-') + num + '.' + cents);
            txt.setValue(String(answer));
        }

        var saveSuccessful = function () {
            showAlert('Status', 'Adjustments was successfully created!', function () {
                window.proxy.sendToAll('updaterebate', 'updaterebate');
                window.proxy.requestClose();
            });
        }
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
    <ext:ResourceManager ID="PageResourceManager" runat="server" /> 
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:FormPanel ID="RootFormPanel" runat="server" Layout="FitLayout" Border="false"
                MonitorValid="true">
                <TopBar>
                    <ext:Toolbar runat="server" ID="tlbView">
                        <Items>
                            <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" Success="saveSuccessful();" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelBasicInformation" Border="false"
                        Padding="5" LabelWidth="180" Layout="ColumnLayout" AnchorHorizontal="100%">
                        <Items>
                            <ext:FieldSet ID="fsReceivableItems" ColumnWidth=".5" Title="Receivable Items" runat="server">
                                <Items>
                                    <ext:Hidden ID="hiddenRandomKey" runat="server"></ext:Hidden>
                                    <ext:Hidden ID="hdnSelectedLoanID" runat="server"></ext:Hidden>
                                    <ext:TextField ID="txtPrincipalDue" runat="server" FieldLabel="Principal Due  (Php)"
                                        AnchorHorizontal="100%" Width="500"
                                         MsgTarget="Side" ReadOnly = "true"/>
                                    <ext:TextField ID="txtInterestDue" runat="server" FieldLabel="Interest Due  (Php)"
                                        AnchorHorizontal="100%" Width="500"
                                         MsgTarget="Side" />
                                    <ext:TextField ID="txtTotalLoanBalance" runat="server" FieldLabel="Total Loan Balance  (Php)" ReadOnly="true"
                                        AnchorHorizontal="100%" Width="500"
                                         MsgTarget="Side" />
                                </Items>
                            </ext:FieldSet>
                            <ext:FieldSet ID="fsRebateAmount" ColumnWidth=".5" Title="Rebate Amount" runat="server">
                                <Items>
                                    <ext:TextField ID="txtPrincipalDue2" runat="server" Text="0.00" FieldLabel="Principal Due (Php)" ValidateOnBlur="true"
                                        AnchorHorizontal="100%" Width="500">
                                        <Listeners>
                                        <Blur Handler="jpScriptCheckValue();" />
                                        </Listeners>
                                        </ext:TextField>
                                    <ext:TextField ID="txtInterestDue2" runat="server" Text="0.00" FieldLabel="Interest Due  (Php)" ValidateOnBlur = "true"
                                        AnchorHorizontal="100%" Width="500">
                                       <%--  <DirectEvents>
                                         <Blur OnEvent="txtInterestDue2_Blur"></Blur>
                                         </DirectEvents>--%>
                                         <Listeners> 
                                         <Blur Handler = "jpScriptCheckValue();"/>
                                         </Listeners>
                                         </ext:TextField>     
                                    <ext:TextField ID="txtTotalLoanBalance2" runat="server" Text="0.00" FieldLabel="Total Loan Balance  (Php)" ReadOnly="true"
                                        AnchorHorizontal="100%" Width="500" AllowBlank = "false" />
                                </Items>
                            </ext:FieldSet>
                        </Items>
                    </ext:Panel>
                </Items>
                <BottomBar>
                    <ext:StatusBar ID="StatusBar5" Text="Hellow" runat="server" Height="25" />
                </BottomBar>
                <Listeners>
                    <ClientValidation Handler="#{StatusBar5}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
