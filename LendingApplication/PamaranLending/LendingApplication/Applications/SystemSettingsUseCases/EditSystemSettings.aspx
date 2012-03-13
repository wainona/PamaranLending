<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSystemSettings.aspx.cs" Inherits="LendingApplication.Applications.FinancialManagement.SystemSettingsUseCases.EditSystemSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Update Customer</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/RequiredIndicatorExtension.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        var saveSuccessful = function () {
            showAlert('Status', 'Successfully updated the system settings.', function () {
                window.proxy.sendToAll('updatecustomer', 'updatecustomer');
                window.proxy.requestClose();
            });
        }

    </script>
    <style type="text/css">
        body {
            font-family      : tahoma,verdana,sans-serif;
	        margin           : 0;
	        padding          : 5px;
            font-size        : 13px;
	        background-color : #fff !important;
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
        <ext:ResourceManager ID="PageResourceManager" runat="server" />
        <ext:Viewport runat="server" Layout="FitLayout"><Items>
        <ext:FormPanel ID="PageFormPanel" 
            runat="server" 
            Padding="5" 
            ButtonAlign="Right" 
            MonitorValid="true"
            MonitorPoll="500"
            Width="600" 
            Height="444"
            Title="Update System Settings"
            BodyStyle="background-color:transparent"
            Layout="FormLayout" 
            LabelWidth="250">
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                            <DirectEvents>
                                <Click OnEvent="btnSave_Click" Before="return #{PageFormPanel}.getForm().isValid();"
                                    Success="saveSuccessful();" >
                                    <EventMask ShowMask="true" Msg="Saving system settings..." />
                                    </Click>
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>

            <Items>
                <ext:Hidden ID="RecordID" DataIndex="ID" runat="server" />

                <%--[SYSTEM SETTING FIELD] Grace Period--%>
                <ext:CompositeField runat="server"  AnchorHorizontal="57%">
                    <Items>
                        <ext:NumberField ID="txtGracePeriod" FieldLabel="Grace Period" runat="server" Flex="1" MinValue="0" DecimalPrecision="0" InvalidText="Invalid" AllowBlank="false"/>
                        <ext:Label runat="server" Text="Day/s" AnchorHorizontal="60%"/>
                    </Items>
                </ext:CompositeField>
                
                <ext:CompositeField Hidden="true" runat="server" AnchorHorizontal="57%">
                    <Items>
                        <ext:NumberField ID="txtInvoice" FieldLabel="Invoice Generation Timing" runat="server" AnchorHorizontal="60%" Flex="1" MinValue="0" DecimalPrecision="0" AllowBlank="false"/>
                        <ext:Label ID="Label1" runat="server" Text="Day/s" AnchorHorizontal="60%" />
                    </Items>
                </ext:CompositeField>

                <ext:CompositeField runat="server" AnchorHorizontal="57%" Hidden="true"><%-- HIDE THIS --%>
                    <Items>
                        <ext:NumberField ID="txtDemand" FieldLabel="Demand Collection After" runat="server" AnchorHorizontal="60%" Flex="1" MinValue="1" DecimalPrecision="0" AllowBlank="true"/>
                        <ext:Label ID="Label2" runat="server" Text="Day/s" AnchorHorizontal="60%" />
                    </Items>
                </ext:CompositeField>

                <ext:CompositeField ID="CompositeField1" runat="server" AnchorHorizontal="56.75%">
                    <Items>
                        <ext:NumberField ID="txtAgeLimit" runat="server" FieldLabel="Age Limit of Borrower" AnchorHorizontal="60%" Flex="1" MinValue="18" AllowBlank="false"/>
                        <ext:Label runat="server" Text="Years" AnchorHorizontal="60%" />
                    </Items>
                </ext:CompositeField>
                

                <ext:RadioGroup runat="server" FieldLabel="Apply Loan Pretermination Penalty?" ID="rdgApply" OnDirectChange="rdgApply_DirectChange"
                    ColumnsWidths="100" Hidden="true"><%-- HIDE THIS --%>
                    <Items>
                        <ext:Radio ID="rdApplyYes" runat="server" BoxLabel="Yes">
                        </ext:Radio>
                        <ext:Radio ID="rdApplyNo" runat="server" BoxLabel="No">
                        </ext:Radio>
                    </Items>
                </ext:RadioGroup>

                <%--[SYSTEM SETTING FIELD] Percantage of Loan Amount Paid--%>
                <ext:CompositeField runat="server" FieldLabel="Percentage of Loan Amount Paid" AnchorHorizontal="57%" Hidden="true">
                    <Items>
                        <ext:TextField ID="txtPercentage" runat="server"  Flex="1" MinValue="0" MaxValue="99"/>
                        <ext:Label runat="server" Text="%" AnchorHorizontal="60%" />
                    </Items>
                </ext:CompositeField>

                <%--[SYSTEM SETTING FIELD] Period in Calculating Penalty--%>
                <ext:ComboBox ID="cbCalculate" runat="server" FieldLabel="Calculate Penalty" AllowBlank="true" Editable="false" Hidden="true">
                    <Items>
                        <ext:ListItem Text="After Maturity Date" />
                        <ext:ListItem Text="After Payment Due Date" />
                    </Items>
                </ext:ComboBox>

                <%--Allow Delete on Row Records? COMBOX--%>
                <ext:RadioGroup runat="server" ColumnsWidths="100" FieldLabel="Allow Delete on Row Records?" ID="rdgAllow" OnDirectChange="rgdAllow_DirectChange">
                    <Items>
                        <ext:Radio ID="rdAllowYes" runat="server" BoxLabel="Yes">
                        </ext:Radio>
                        <ext:Radio ID="rdAllowNo" runat="server" BoxLabel="No">
                        </ext:Radio>
                    </Items>
                </ext:RadioGroup>

                <%--[SYSTEM SETTING FIELD] Allow Delete on Loans with Age--%>
                <ext:CompositeField runat="server" AnchorHorizontal="57%" FieldLabel="Allow Delete on Loans with Age">
                    <Items>
                        <ext:TextField ID="txtAllowDelWithAge" runat="server" AnchorHorizontal="60%" Flex="1" MinValue="1"/>
                        <ext:Label runat="server" Text="Years" AnchorHorizontal="60%" />
                    </Items>
                </ext:CompositeField>

                <%--[SYSTEM SETTING FIELD] Options to Set Date Payment--%>
                <ext:RadioGroup runat="server" ColumnsWidths="150" FieldLabel="Set Date Payment Option" ID="RadioGroup1" Hidden="true">
                    <Items>
                        <ext:Radio ID="radioBefore" runat="server" BoxLabel="Before Date Payment">
                        </ext:Radio>
                        <ext:Radio ID="radioAfter" runat="server" BoxLabel="After Date Payment">
                        </ext:Radio>
                    </Items>
                </ext:RadioGroup>

                <ext:TextField ID="txtMaxAmountAppovableByClerk" runat="server" FieldLabel="Clerk's Maximum Honorable Amount"
                    MaskRe="[0-9/./,]" Width="350">
                    <Listeners>
                        <Change Handler="var value = this.getValue().replace(/,/g,'');value = value.replace(/[]/g, '');this.setValue(Ext.util.Format.number(value, '0,0.00'));" />
                    </Listeners>
                </ext:TextField>

                <ext:ComboBox ID="cmbAdvanceChangeNoInterestStartDay" runat="server" FieldLabel="Advance Change No Interest Start Day" Width="350" Editable="false">
                    <Items>
                        <ext:ListItem Text="1" Value="1" />
                        <ext:ListItem Text="2" Value="2" />
                        <ext:ListItem Text="3" Value="3" />
                        <ext:ListItem Text="4" Value="4" />
                        <ext:ListItem Text="5" Value="5" />
                        <ext:ListItem Text="6" Value="6" />
                        <ext:ListItem Text="7" Value="7" />
                        <ext:ListItem Text="8" Value="8" />
                        <ext:ListItem Text="9" Value="9" />
                        <ext:ListItem Text="10" Value="10" />
                        <ext:ListItem Text="11" Value="11" />
                        <ext:ListItem Text="12" Value="12" />
                        <ext:ListItem Text="13" Value="13" />
                        <ext:ListItem Text="14" Value="14" />
                        <ext:ListItem Text="15" Value="15" />
                        <ext:ListItem Text="16" Value="16" />
                        <ext:ListItem Text="17" Value="17" />
                        <ext:ListItem Text="18" Value="18" />
                        <ext:ListItem Text="19" Value="19" />
                        <ext:ListItem Text="20" Value="20" />
                        <ext:ListItem Text="21" Value="21" />
                        <ext:ListItem Text="22" Value="22" />
                        <ext:ListItem Text="23" Value="23" />
                        <ext:ListItem Text="24" Value="24" />
                        <ext:ListItem Text="25" Value="25" />
                        <ext:ListItem Text="26" Value="26" />
                        <ext:ListItem Text="27" Value="27" />
                        <ext:ListItem Text="28" Value="28" />
                        <ext:ListItem Text="29" Value="29" />
                        <ext:ListItem Text="30" Value="30" />
                        <ext:ListItem Text="31" Value="31" />
                    </Items>
                </ext:ComboBox>

                <ext:FieldSet ID="FieldSet1" runat="server" Title="Allowable Number of Loans per Customer" Collapsible="false" Width="625" Padding="10">
                    <Items>
                        <ext:TextField ID="txtStraightLine" runat="server" FieldLabel="Stragiht Line Loan" AnchorHorizontal="57%" MinValue="1"/>
                        <ext:TextField ID="txtDiminishing" runat="server" FieldLabel="Diminishing Balance Loan" AnchorHorizontal="57%" MinValue="1"/>
                    </Items>
                </ext:FieldSet>
            </Items>
            <BottomBar>
                <ext:StatusBar ID="PageFormPanelStatusBar" runat="server" Height="25" />
            </BottomBar>
            <Listeners>
                <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
            </Listeners>
        </ext:FormPanel>
        </Items></ext:Viewport>
    </form>
</body>
</html>
