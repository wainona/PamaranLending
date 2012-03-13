<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LendingApplication.Default" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link rel="stylesheet" type="text/css" href="resources/css/main.css" />
    <script src="resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript" src="resources/js/main.js"></script>
    <script src="resources/js/tabManager.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['logout', 'addfinancialproduct',
                                'addbank', 'updatebank',
                                'addcashonvaultsuccess',
                                'addcheque', 'updatecheque',
                                'addfeepayment', 'addcollection',
                                'addcontactorganization', 'addcontactperson',
                                'editcontactorganization', 'editcontactperson',
                                'addcustomerclassification', 'updatecustomerclassification',
                                'addemployer', 'addcustomer',
                                'updatecustomer', 'updateemployer',
                                'updateuseraccount', 'addchange',
                                'addencashment', 'loanapplicationstatusupdate',
                                'addotherdisbursement', 'addrediscounting',
                                'addemployee', 'updateemployee',
                                'addholiday', 'updateholiday',
                                'addrequireddocument', 'addamortizationschedule',
                                'approvedloanapplication', 'createLoanApplication',
                                'onpickapplicationfee', 'managecollateral',
                                'modifyLoanApplication', 'printpromisory',
                                'closedpromisory', 'onunderlitigation',
                                'addreceipt', 'addrequireddocumenttype', 
                                ]);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var loadHomePage = function () {
            X.RetrieveHomePage({
                success: function (result) {
                    loadExample(result.NodeHref, "-", result.NodeName, "");
                }
            });
            
        };

        var setHomePageSuccess = function () {
            showAlert('Info', 'Home page successfully set.', function () {
                window.location.reload();
            });
        };

        var onMessageReceived = function (msg) {
            if (msg.tag == 'logout') {
                lnkSignout.fireEvent('click');
            } else {
                resetTimeOut();
            }
        };

        var changePasswordSuccess = function () {
            showAlert('Info','Your password has been successfully updated.');
            wndChangePassword.hide();
        };

        var changePassword = function () {
            X.RetrieveAndShowUserAccount();
        };

        var confirmOldPassword = function () {
            X.ConfirmOldPassword();
        };

        var confirmPassword = function () {
            if (txtNewPassword.getValue() == txtConfirmPassword.getValue()) {
                return true;
            }
            txtConfirmPassword.markInvalid('Passwords do not match.');
            return false;
        };

        var onHelpClick = function () {
            var url = '/MNPamaranHowTo/ListOfHowTos.aspx';
            var param = url;
            loadExample(url, "-", "How To - Index", "icon-help");
        }
    </script>
    <script  type="text/javascript">

        var secondsPassed = 0;
        function ShowTimePassed() {
            minutesBeforeLoggedOut = parseInt(minutesBeforeLoggedOut);
            secondsPassed += 1;

            if (minutesBeforeLoggedOut == -1) {
                if (secondsPassed == 30) {
                    var img = new Image(1, 1);
                    img.src = 'Default.aspx?date=' + escape(new Date());
                    secondsPassed = 0;
                }
            }
            else {
                formTimeOut = parseInt(formTimeOut);
                var seconds = formTimeOut * 60;
                if (secondsPassed == minutesToWarning * 60) {
                    var answer;

                    var currentTime = new Date();
                    var expiredTime = new Date();
                    var minutes = expiredTime.getMinutes();
                    minutes += minutesBeforeLoggedOut;
                    expiredTime.setMinutes(minutes);

                    if (minutesBeforeLoggedOut == 1) {
                        answer = showConfirm("Info", "It is now " + currentTime.toLocaleTimeString() + ". You have " + minutesBeforeLoggedOut + " minute left before getting logged out. Do you want to extend the session?", function (btn) {
                            if (btn == 'yes') {
                                var img = new Image(1, 1);
                                img.src = 'Default.aspx?date=' + escape(new Date());
                                secondsPassed = 0;
                                currentTime = new Date();
                                if (currentTime > expiredTime) {
                                    showAlert("Alert", "You've exceeded the time needed to extend the session. You will be logged out now.", function () {
                                        window.location = loginUrl;
                                    });
                                    
                                }
                            }
                        });
                    } else {
                    answer = showConfirm("Info", "It is now " + currentTime.toLocaleTimeString() + ". You have " + minutesBeforeLoggedOut + " minutes left before getting logged out. Do you want to extend the session?", function (btn) {
                        if (btn == 'yes') {
                            var img = new Image(1, 1);
                            img.src = 'Default.aspx?date=' + escape(new Date());
                            secondsPassed = 0;
                            currentTime = new Date();
                            if (currentTime > expiredTime) {
                                showAlert("Alert", "You've exceeded the time needed to extend the session. You will be logged out now.", function () {
                                    window.location = loginUrl;
                                });
                                
                            }
                        }
                    });
                    }
//                   if (answer) {
//                        var img = new Image(1, 1);
//                        img.src = 'Default.aspx?date=' + escape(new Date());
//                        secondsPassed = 0;
//                        currentTime = new Date();
//                        if (currentTime > expiredTime) {
//                            showAlert("Alert", "You've exceeded the time needed to extend the session. You will be logged out now.");
//                            window.location = loginUrl;
//                        }
//                    }

                    var img = new Image(1, 1);
                    img.src = 'Default.aspx?date=' + escape(new Date());
                    currentTime = new Date();
                    if (currentTime > expiredTime) {
                        showAlert("Alert", "Session has timed out. You will be logged out now.", function () {
                            window.location = loginUrl;
                        });
                        
                    }
                } else if (secondsPassed == seconds) {
                    showAlert("Alert", "Session has timed out. You will be logged out now.", function () {
                        window.location = loginUrl; 
                    });
                }
            }
        }

        function resetTimeOut() {
            secondsPassed = 0;
        }

        window.setInterval('ShowTimePassed()', 1000);
    </script>
    <style type="text/css">
        .naviIcon
        {
            background-image: url(resources/icons/world-globe_small.png)
        }
        .fontBlack 
        {
            color: #000000;
            font-weight: bold;
            font-size: 16px;
        }
    </style>
    <title>Home</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <noscript>
            <center>
                <strong><font size="4">Sorry, your browser does not support JavaScript.</font></strong>
                <br />
                <img src="../Resources/images/JavascriptInstructions.png" />
            </center>
        </noscript>
        <ext:ResourceManager ID="MasterResourceManager" runat="server" DirectMethodNamespace="X"
            IDMode="Explicit" />
        <ext:History ID="HistoryManager" runat="server">
            <Listeners>
                <Change Fn="change" />
            </Listeners>
        </ext:History>
        <ext:Viewport runat="server" Layout="border">
            <Items>
                <ext:Panel ID="HeaderPanel" runat="server" Header="false" Region="North" Border="false"
                    Height="49" BodyCssClass="headertest">
                    <Content>
                        <div>
                            <ext:Image runat="server" ImageUrl="/Resources/images/MNPLII_Logo.gif"></ext:Image>
                            <div class="signout">
                                <ext:Label ID="lblNameAndTypeOfUser" runat="server" LabelAlign="Right" Cls="fontBlack"/>
                                &nbsp;&nbsp;|&nbsp;
                                <ext:LinkButton ID="lnkHelp" runat="server" Text="Help" Icon="Help">
                                    <Listeners>
                                        <Click Handler="onHelpClick();" />
                                    </Listeners>
                                </ext:LinkButton>
                                <br />
                                <ext:LinkButton ID="lnkChangePassword" runat="server" Text="change password">
                                    <Listeners>
                                        <Click Handler="changePassword();" />
                                    </Listeners>
                                </ext:LinkButton>
                                &nbsp;|&nbsp;
                                <ext:LinkButton ID="lnkSetHomePage" runat="server" Text="set home page">
                                    <DirectEvents>
                                        <Click OnEvent="setHomePage_Click" >
                                            <EventMask ShowMask="true" Msg="Loading.." />
                                        </Click>
                                    </DirectEvents>
                                </ext:LinkButton>
                                &nbsp;|&nbsp;
                                <ext:LinkButton ID="lnkSignout" runat="server" Text="signout">
                                    <DirectEvents>
                                        <Click OnEvent="signout_Click" >
                                            <EventMask ShowMask="true" Msg="Signing out.." />
                                        </Click>
                                    </DirectEvents>
                                </ext:LinkButton>
                            </div>
                        </div>
                    </Content>
                    <Listeners>
                        <AfterRender Handler="loadHomePage();" />
                    </Listeners>
                </ext:Panel>
                <ext:Panel ID="NavigationPanel" runat="server" Region="West" Layout="Fit" Width="270"
                    Header="false" Collapsible="true" Split="true" CollapseMode="Mini" Margins="0 0 5 5"
                    Border="false">
                    <Items>
                        <ext:TreePanel ID="NavigationTree" runat="server" Width="300" Height="450" AutoScroll="true" Padding="2" Title="Navigation Area" IconCls="naviIcon">
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
                                <Click Handler="if (node.isLeaf()) { e.stopEvent(); loadExample(node.attributes.href, node.id, node.text, node.attributes.iconCls);}; resetTimeOut();" />
                            </Listeners>
                            <DirectEvents>
                                <Click OnEvent="onNode_Click" />
                            </DirectEvents>
                        </ext:TreePanel>
                    </Items>
                </ext:Panel>
                <ext:Panel ID="WorkPanel" runat="server" Region="Center" Header="false" Split="true" Layout="Fit" 
                        Border="false" Margins="0 5 5 0">
                    <Items>
                        <ext:TabPanel ID="WorkspaceTabs" runat="server" EnableTabScroll="true">
                            <Listeners>
                                <TabChange Fn="addToken" />
                            </Listeners>
                            <Plugins>
                                <ext:TabCloseMenu ID="TabCloseMenu" runat="server" />
                            </Plugins>
                        </ext:TabPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="wndChangePassword" runat="server" Width="440" Height="275" Modal="true" Title="Change Password"
            Closable="false" Draggable="false" Hidden="true" Resizable="false">
            <Items>
                <ext:FormPanel ID="formPanelChangePassword" runat="server" Padding="5" LabelWidth="140" MonitorValid="true">
                    <Items>
                        <ext:Label ID="lblEmployeeName" runat="server" FieldLabel="Name" />
                        <ext:Label ID="lblUsername" runat="server" FieldLabel="Username" />
                        <ext:TextField ID="txtOldPassword" runat="server" FieldLabel="Old Password" AllowBlank="false" AnchorHorizontal="95%" InputType="Password" 
                            IsRemoteValidation="true" MsgTarget="Side">
                            <RemoteValidation OnValidation="CheckField" ></RemoteValidation>
                        </ext:TextField>
                        <ext:TextField ID="txtNewPassword" runat="server" FieldLabel="New Password" AllowBlank="false" AnchorHorizontal="95%" InputType="Password"/>
                        <ext:TextField ID="txtConfirmPassword" Vtype="password" runat="server" FieldLabel="Confirm New Password"
                            AllowBlank="false" EnableKeyEvents="true" InputType="Password" AnchorHorizontal="95%" MsgTarget="Side">
                            <CustomConfig>
                                <ext:ConfigItem Name="initialPassField" Value="#{txtNewPassword}" Mode="Value" />
                            </CustomConfig>
                        </ext:TextField>
                        <ext:ComboBox ID="cmbPasswordQuestion" runat="server" FieldLabel="Password Question" ForceSelection="true" AnchorHorizontal="95%" AllowBlank="false"
                            Editable="false">
                            <Items>
                                <ext:ListItem Text="Where did you meet your spouse?" />
                                <ext:ListItem Text="What was the name of your first school?" />
                                <ext:ListItem Text="Who was your childhood hero?" />
                                <ext:ListItem Text="What is the name of your first crush?" />
                                <ext:ListItem Text="What is your favorite pastime?" />
                                <ext:ListItem Text="What is your favorite sports team?" />
                                <ext:ListItem Text="What is your mother's maiden name?" />
                                <ext:ListItem Text="What is the name of your pet?" />
                            </Items>
                        </ext:ComboBox>
                        <ext:TextField ID="txtPasswordAnswer" runat="server" FieldLabel="Password Answer" AnchorHorizontal="95%" AllowBlank="false" />
                    </Items>
                    <BottomBar>
                        <ext:StatusBar ID="StatusBar1" runat="server" />
                    </BottomBar>
                    <Listeners>
                        <ClientValidation Handler="this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && confirmPassword() == true){#{btnSave}.enable();}  else{#{btnSave}.disable();}" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="btnSave_Click" Success="changePasswordSuccess();" Before="return #{formPanelChangePassword}.getForm().isValid();">
                            <EventMask ShowMask="true" Msg="Saving.." RemoveMask="true"/>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="btnCancel" runat="server" Text="Cancel" Icon="Cancel">
                    <Listeners>
                        <Click Handler="#{wndChangePassword}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
        <ext:Window ID="wdnSetHomePage" runat="server" Hidden="true" Width="380" Modal="true" Title="Set Home Page" Draggable="false" Closable="false" Resizable="false">
            <Items>
                <ext:Panel runat="server"  PaddingSummary="5" Border="false">
                    <Items>
                        <ext:ComboBox ID="cmbSetHomePage" runat="server" Editable="false" DisplayField="NodeName" ValueField="NodeId" FieldLabel="Home Page" Width="355">
                            <Store>
                                <ext:Store ID="cmbSetHomePageStore" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="NodeId">
                                            <Fields>
                                                <ext:RecordField Name="NodeId" />
                                                <ext:RecordField Name="NodeName" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                        </ext:ComboBox>
                    </Items>
                </ext:Panel>
            </Items>
            <Buttons>
                <ext:Button runat="server" ID="btnSaveSetHomePage" Text="Save" Icon="Disk">
                    <DirectEvents>
                        <Click OnEvent="btnSaveSetHomePage_Click" Success="setHomePageSuccess();">
                            <EventMask Msg="Saving.." ShowMask="true" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" Text="Close" Icon="Cancel">
                    <Listeners>
                        <Click Handler="#{wdnSetHomePage}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </div>
    </form>
</body>
</html>
