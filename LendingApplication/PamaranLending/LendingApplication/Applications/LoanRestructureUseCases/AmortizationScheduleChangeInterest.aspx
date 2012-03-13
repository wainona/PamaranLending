<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmortizationScheduleChangeInterest.aspx.cs"
    Inherits="LendingApplication.Applications.LoanRestructureUseCases.AmortizationScheduleChangeInterest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Add</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Contact record was successfully added', function () {
                window.proxy.sendToAll('addamortizationschedule', 'addamortizationschedule');
                window.proxy.requestClose();
            });
        };

        var saveFailure = function () {
            showAlert('Save Failure', 'Cheques should have a check number.');
        };

        var onDateChanged = function () {
            var today = new Date;
            var status = "Retired";
            if (dtIntroductionDate.getValue() > today)
                status = "Inactive";
            else if (dtIntroductionDate.getValue() <= today
                && (dtSalesDiscontinuationDate.getValue() == ''
                || dtSalesDiscontinuationDate.getValue() > today))
                status = "Active";

            txtProductStatus.setValue(status);
        }

        Ext.apply(Ext.form.VTypes, {
            numberrange: function (val, field) {
                if (!val) {
                    return;
                }

                if (field.startNumberField && (!field.numberRangeMax || (val != field.numberRangeMax))) {
                    var start = Ext.getCmp(field.startNumberField);

                    if (start) {
                        start.setMaxValue(val);
                        field.numberRangeMax = val;
                        start.validate();
                    }
                } else if (field.endNumberField && (!field.numberRangeMin || (val != field.numberRangeMin))) {
                    var end = Ext.getCmp(field.endNumberField);

                    if (end) {
                        end.setMinValue(val);
                        field.numberRangeMin = val;
                        end.validate();
                    }
                }

                return true;
            }
        });
    </script>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <ext:Hidden ID="hiddenResourceGUID" runat="server" />
    <ext:Hidden ID="hiddenCustomerId" runat="server" />
    <ext:Hidden ID="hiddenBalance" runat="server" />
    <ext:Hidden ID="hiddenRandomKey" runat="server" />
    <ext:Hidden ID="hdnSelectedLoanID" runat="server" />
    <ext:Viewport ID="pageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:Panel runat="server" ID="RootPanel" Layout="FitLayout" Border="false">
                <TopBar>
                    <ext:Toolbar runat="server" ID="RootToolBar">
                        <Items>
                            <ext:Button runat="server" Hidden="true" ID="btnGenerate" Text="Generate" Icon="Accept" >
                                <DirectEvents>
                                    <Click OnEvent="btnGenerate_Click" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnSave" Text="Save" Icon="Disk">
                                <DirectEvents>
                                    <Click OnEvent="btnSave_Click" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnCancel" Text="Cancel" Icon="Cancel" />
                            <ext:ToolbarFill runat="server" />
                            <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel" />
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:Panel runat="server" ID="PanelGenerateAmortizationSchedule"
                        Padding="0" Layout="FormLayout" LabelWidth="180">
                        <Items>
                            <ext:Panel runat="server" AutoHeight="true" LabelWidth="420" Border="false">
                                <Items>
                                    <ext:GridPanel runat="server" ID="grdPnlAmortizationSchedule" Height="250" MinHeight="250"
                                        Title="Amortization Schedule" EnableColumnHide = "false" EnableColumnMove = "false" EnableColumnResize = "false">
                                        <Store>
                                            <ext:Store runat="server" ID="storeAmortizationSchedule" RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy>
                                                    </ext:PageProxy>
                                                </Proxy>
                                                <Listeners>
                                                    <LoadException Handler="showAlert('Load failed', response.statusText);" />
                                                </Listeners>
                                                <Reader>
                                                    <ext:JsonReader IDProperty="ID">
                                                        <Fields>
                                                            <ext:RecordField Name="PaymentDueDate" />
                                                            <ext:RecordField Name="PrincipalDue" />
                                                            <ext:RecordField Name="InterestDue" />
                                                            <ext:RecordField Name="Total" />
                                                            <ext:RecordField Name="PrincipalBalance" />
                                                            <ext:RecordField Name="TotalLoanBalance" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <SelectionModel>
                                        </SelectionModel>
                                        <ColumnModel runat="server" ID="clmAmortizationSchedule" Width="100%">
                                            <Columns>
                                                <ext:Column Header="Payment Due Date" DataIndex="PaymentDueDate" Wrap="true"
                                                    Width="180" Sortable="false" />
                                                <ext:Column Header="Principal Due" DataIndex="PrincipalDue" Wrap="true" Width="180"
                                                    Sortable="false" />
                                                <ext:Column Header="Interest Due" DataIndex="InterestDue" Wrap="true" Width="180"
                                                    Sortable="false" />
                                                <ext:Column Header="Total" DataIndex="Total" Wrap="true" Width="180"
                                                    Sortable="false" />
                                                <ext:Column Header="Principal Balance" DataIndex="PrincipalBalance" Wrap="true" Width="180"
                                                    Sortable="false" />
                                                <ext:Column Header="Total Loan Balance" DataIndex="TotalLoanBalance" Wrap="true"
                                                    Width="180" Sortable="false" />
                                            </Columns>
                                        </ColumnModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
     <ext:Window ID="winEditAmortizationScheduleItem" Modal="true" Draggable="false" Resizable="false"
        Icon="ApplicationFormEdit" runat="server" Collapsible="false" Height="105" Hidden="true"
        Title="Edit Amortization Schedule Item" Width="520">
        <Items>
         <ext:FormPanel Padding="10" runat="server" ID="frmAddress" LabelWidth="150">
         <Items>
         <ext:TextField runat="server" ID = "txtPrincipalPayment" FieldLabel = "Principal Payment (Php)" />
         </Items>
         <BottomBar>
         <ext:Toolbar runat="server">
         <Items>
         <ext:ToolbarFill runat="server" />
         <ext:Button runat="server" ID = "btnWinSave" Text = "Save" Icon = "Disk" />
         <ext:Button runat="server" ID = "btnWinCancel" Text = "Cancel" Icon = "Cancel">
         <Listeners>
         <Click Handler = "#{winEditAmortizationScheduleItem}.hide();" />
         </Listeners>
         </ext:Button>
         </Items>
         </ext:Toolbar>
         </BottomBar>
         </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
