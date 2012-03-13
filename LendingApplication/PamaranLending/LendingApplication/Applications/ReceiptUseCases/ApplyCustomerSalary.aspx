<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyCustomerSalary.aspx.cs" Inherits="LendingApplication.Applications.ReceiptUseCases.ApplyCustomerSalary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Apply Customer Salary</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['updatereceipt', 'addcustomersalary']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            PageGridPanel.reload();
        };
        var appliedAsPaymentSuccessful = function () {
            showAlert('Message', 'Successfully applied payment.', function () {
                window.proxy.sendToParent('appliedPayment');
                CancelCheckAsPayment();
            });

        }
        var onFormValidation = function (valid) {
            btnPayCPDC.disable();
            if (valid)
                btnPayCPDC.enable();
        }

        var applyAsPayment = function () {
            if (PageGridPanel.hasSelection() == true) {
                var selectedRow = PageGridPanelSelectionModel.getSelected().json;
                X.FillApplyAsPaymentWindow(selectedRow.ReceiptId, selectedRow.PartyRoleId, {
                    success: function (result) {
                        winShow();
                    }
                });
            }
            else showAlert("Alert", "No row selected");
        }

        var winShow = function () {
            formatCurrency(wntxtReceiptAmount);
            formatCurrency(wntxtExistInterest);
            formatCurrency(wntxtAddIntererst);
            wndCPDC.show();
        }

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var selected = selectedRows[0];
                var url = '/Applications/ReceiptUseCases/EditReceipt.aspx';
                var id = 'id=' + selected.json.ReceiptId;
                var param = url + "?" + id;
                param += "&mode=salary";
                window.proxy.requestNewTab('UpdateReceipt', param, 'Update Receipt');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        }

        var CancelCheckAsPayment = function () {
            wndCPDC.hide();
            ClearCheckAsPayment();
        }

        var ClearCheckAsPayment = function () {
            wntxtReceiptAmount.setValue(0);
            wntxtExistInterest.setValue(0);
            wntxtAddIntererst.setValue(0);
            formatCurrency(wntxtLoanPayment);
            formatCurrency(wntxtInterestPayment);
            formatCurrency(wntxtReceiptAmount);
            formatCurrency(wntxtExistInterest);
            formatCurrency(wntxtAddIntererst);
            dtGenerationDate.clear();
        }

        var generateAdditionalInterest = function () {
            X.GenerateAdditionalInterest();
        };

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

        var onRowSelected = function () {
            btnApplyAsPayment.enable();
            btnOpen.enable();
        }
        var onRowDeselected = function () {
            btnApplyAsPayment.disable();
            btnOpen.disable();
        }
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
        <ext:Hidden ID="hdnPartyRoleId" runat="server"></ext:Hidden>
        <ext:Hidden ID="hdnReceiptId" runat="server"></ext:Hidden>
        <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
            <Items>
                <ext:GridPanel ID="PageGridPanel" runat="server" Height="555">
                    <LoadMask ShowMask="true" Msg="Loading customer salaries.."/>
                    <View>
                        <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No customers to display..." runat="server" />
                    </View>
                    <TopBar>
                        <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                            <Items>
                                <ext:Button ID="btnApplyAsPayment" runat="server" Text="Apply As Payment" Disabled="true" Icon="MoneyAdd" >
                                    <Listeners>
                                        <Click Handler="applyAsPayment();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="onBtnOpenClick();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill />
                                <ext:TextField ID="txtSearchKey" EmptyText="Search Text Here..." runat="server" FieldLabel="Name" LabelWidth="30"></ext:TextField>
                                <ext:Button ID="btnSearch" runat="server" Text="Search" Icon="Find">
                                    <DirectEvents>
                                        <Click OnEvent="btnSearch_Click">
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store runat="server" ID="PageGridPanelStore" RemoteSort="false" OnRefreshData="RefreshData">
                            <Proxy>
                                <ext:PageProxy>
                                </ext:PageProxy>
                            </Proxy>
                            <AutoLoadParams>
                                <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                            </AutoLoadParams>
                            <Listeners>
                                <LoadException Handler="showAlert('Load failed', response.statusText);" />
                            </Listeners>
                            <Reader>
                                <ext:JsonReader IDProperty="ReceiptId">
                                    <Fields>
                                        <ext:RecordField Name="ReceiptId" />
                                        <ext:RecordField Name= "PartyRoleId" />
                                        <ext:RecordField Name="Name" />
                                        <ext:RecordField Name="Amount" />
                                        <ext:RecordField Name="PaymentMethodType" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </ext:Store> 
                    </Store>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="true" >
                            <Listeners>
                                <RowSelect Fn="onRowSelected" />
                                <RowDeselect Fn="onRowDeselected" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                        <Columns>
                            <ext:Column Header="Receipt Id" DataIndex="ReceiptId" Wrap="true" Locked="true" Width="50px">
                            </ext:Column>
                            <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" >
                            </ext:Column>
                            <ext:NumberColumn Header="Amount" DataIndex="Amount" Wrap="true" Locked="true" >
                            </ext:NumberColumn>
                            <ext:Column Header="Payment Method Type" DataIndex="PaymentMethodType" Locked="true" Wrap="true">
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <BottomBar>
                        <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                            DisplayMsg="Displaying customer salaries {0} - {1} of {2}" EmptyMsg="No customer salaries to display" />
                    </BottomBar>
                    <LoadMask ShowMask="true"/>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="wndCPDC" runat="server" Title="Apply as Payment" Modal="true" Layout="Fit"
            Width="450" Height="300" Hidden="true" Resizable="false" Closable="false" Draggable="false">
            <Items>
                <ext:FormPanel Padding="10" runat="server" ID="formAppyAsPayment" LabelWidth="200"
                    MonitorValid="true">
                    <Defaults>
                        <ext:Parameter Name="MsgTarget" Value="side" />
                    </Defaults>
                    <Items>
                        <ext:DateField ID="wnDtTransactionDate" runat="server"  FieldLabel="Transaction Date" Editable="false" AnchorHorizontal="95%" AllowBlank="false">
                        </ext:DateField>
                        <ext:TextField ID="wntxtReceiptAmount" runat="server" ReadOnly="true" FieldLabel="Receipt Amount"  AnchorHorizontal="95%"/>
                        <ext:TextField ID="wntxtLoanBalance" runat="server" ReadOnly="true" FieldLabel="Remaining Loan Balance" AnchorHorizontal="95%"/>
                        <ext:TextField ID="wntxtExistInterest" runat="server" ReadOnly="true" FieldLabel="Existing Billed Interest" AnchorHorizontal="95%" />
                        <ext:TextField ID="wntxtExistInterestDate" runat="server" ReadOnly="true" FieldLabel="Existing Interest Date" AnchorHorizontal="95%" />
                        <ext:DateField ID="dtGenerationDate" runat="server" FieldLabel="Generate Additional Interest"
                            Editable="false" AnchorHorizontal="95%">
                            <Listeners>
                                    <Select Handler="generateAdditionalInterest();" />
                            </Listeners>
                        </ext:DateField>
                        <ext:TextField ID="wntxtAddIntererst" runat="server" FieldLabel="Additional Interest Amount" AnchorHorizontal="95%" ReadOnly="true"></ext:TextField>
                    </Items>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar2" runat="server" Height="25" />
                    </BottomBar>
                    <Buttons>
                        <ext:Button ID="btnPayCPDC" runat="server" Text="Pay" Icon="Accept" Disabled="true">
                            <DirectEvents>
                                <Click OnEvent="ApplyCheckAsPayment" Success="appliedAsPaymentSuccessful">
                                    <EventMask Msg="Applying.." ShowMask="true" />
                                </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnCancelCPDC" runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="CancelCheckAsPayment();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                    <Listeners>
                        <ClientValidation Handler="onFormValidation(valid)" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
