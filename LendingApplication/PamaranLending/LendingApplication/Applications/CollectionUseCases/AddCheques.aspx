<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCheques.aspx.cs" Inherits="LendingApplication.Applications.CollectionUseCases.AddCheques" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Cheques</title>
     <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['bankselected']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'bankselected') {
                txtBankName.setValue(msg.data.OrganizationName);
                txtBankBranch.setValue(msg.data.BranchName);
                hiddenBankID.setValue(msg.data.PartyRoleId);
            }
        }

        var validateCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                        hdnChequeValid.setValue(1);
                    } else {
                        hdnChequeValid.setValue(0);
                        btnAdd.enable();
                    }
                }
            });

        };

        var validateNewCheckNumber = function () {
            var checkNumber = txtCheckNumber.getValue();
            X.ValidateNewCheckNumber(checkNumber, {
                success: function (result) {
                    if (result == 1) {
                        showAlert('Alert!', 'The check number is already used by another check.');
                        txtCheckNumber.markInvalid('Check Number already exist.');
                        hdnChequeValid.setValue(1);
                    } else {
                        hdnChequeValid.setValue(0);
                        btnAdd.enable();
                    }
                }
            });

        };

        var onFormValidated = function (valid) {
            if ((valid && hdnChequeValid.getValue() == 0) || (valid && hdnChequeValid.getValue() == '')) {
                StatusBarCheque.setStatus({ text: 'Form is valid. ' });
                btnAdd.enable();
            }
            else {
                StatusBarCheque.setStatus({ text: 'Please fill out the form.' });
                btnAdd.disable();
            }
        }

        var AddCheque = function () {
            var data = {};
            data.BankName = txtBankName.getValue();
            data.Branch = txtBankBranch.getValue();
            data.CheckNumber = txtCheckNumber.getValue();
            data.CheckType = cmbCheckType.getValue();
            data.CheckDate = dtCheckDate.getValue();
            var totalAmount = txtAmount.getValue();
            totalAmount = totalAmount.replace(/,/g, '');
            data.TotalAmount = parseFloat(totalAmount);

            window.proxy.sendToParent(data, 'addcheque');
            window.proxy.requestClose();
        }


        var onPickBank = function () {
            window.proxy.requestNewTab('BankPickList', '/Applications/DisbursementUseCases/BankViewList.aspx', 'Bank Pick List');
        };

        var checkAmount = function () {
            if (parseInt(txtAmount.getValue()) > 0)
                txtAtik.allowBlank = true;
            else
                txtAtik.allowBlank = false;
        }
        var formatCurrency = function (txt) {
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
    </script>
    <style id="Style1" type="text/css" runat="server">
    .bold .x-btn-text
    {
        font-weight:bold;
    }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewPort" runat="server" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="PageFormPanel" 
                           runat="server" 
                           Padding="5" 
                           ButtonAlign="Right" 
                           MonitorValid="true"
                           Width="600" 
                           Height="400"
                           Title="Add Cheque"
                           Layout="FormLayout">
                <TopBar>
                    <ext:Toolbar runat="server">
                        <Items>
                            <ext:Button ID="btnAdd" runat="server" Text="Add" Icon="Add">
                                <Listeners>
                                    <Click Handler="AddCheque();" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator runat="server" />
                            <ext:Button ID="Close" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel ID="PanelAddCheques" runat="server" Layout="ColumnLayout" Height="400" Border="false">
                        <Items>
                            <ext:TextField ID="txtAtik" runat="server" AllowBlank="false" Hidden="true" />
                            <ext:Hidden ID="hdnChequeValid" runat="server" ></ext:Hidden>
                            <ext:Hidden ID="hiddenRandomKey" runat="server" />
                            <ext:Panel ID="PanelDetails" runat="server" ColumnWidth=".5" Height="400" Layout="FormLayout" Border="false">
                                <Items>
                                    <ext:Hidden ID="hiddenCheckDate" runat="server"/>
                                    <ext:TextField ID="txtBankName" runat="server" FieldLabel="Bank" AnchorHorizontal="98%" AllowBlank="false" ReadOnly="true" />
                                    <ext:TextField ID="txtBankBranch" runat="server" FieldLabel="Branch" AnchorHorizontal="98%" ReadOnly="true" />
                                    <ext:TextField ID="txtCheckNumber" runat="server" FieldLabel="Check No." AnchorHorizontal="98%" AllowBlank="false" EnableKeyEvents="true">
                                        <Listeners>
                                            <KeyUp Handler="validateCheckNumber();validateNewCheckNumber();" />
                                        </Listeners>
                                    </ext:TextField> 
                                    <ext:ComboBox ID="cmbCheckType" runat="server" AllowBlank="false" FieldLabel="Check Type" AnchorHorizontal="98%" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="Pay Check" />
                                            <ext:ListItem Text="Personal Check" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:DateField ID="dtCheckDate" runat="server" FieldLabel="Check Date" AnchorHorizontal="98%" AllowBlank="false" />
                                    <ext:TextField ID="txtAmount" runat="server" Text="0.00" FieldLabel="Amount" MaskRe="/[0-9\.\,]/" AnchorHorizontal="98%" AllowBlank="false">
                                        <Listeners>
                                            <Blur Handler="formatCurrency(txtAmount);checkAmount();" />
                                        </Listeners>
                                    </ext:TextField>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="PanelBrowseBank" runat="server" ColumnWidth=".5" Border="false" Layout="FormLayout" Height="400">
                                <Items>
                                    <ext:Button ID="btnBrowseBank" runat="server" Cls="bold" Text="Browse..." Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="onPickBank();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
                <BottomBar>
                <ext:StatusBar ID="StatusBarCheque" runat="server">
                </ext:StatusBar>
                </BottomBar>
                 <Listeners>
                   <%-- <ClientValidation Handler="#{StatusBarCheque}.setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnAdd}.enable();}  else{#{btnAdd}.disable();}" />--%>
                   <ClientValidation Handler="onFormValidated(valid);" />
                </Listeners>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
