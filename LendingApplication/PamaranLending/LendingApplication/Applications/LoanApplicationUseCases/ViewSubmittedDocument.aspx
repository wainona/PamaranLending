<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewSubmittedDocument.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.ViewSubmittedDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Submit Required Document</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
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
            showAlert('Status', 'Successfully added the record.', function () {
                window.proxy.sendToAll('addcustomer', 'addcustomer');
                window.proxy.requestClose();
            });
        }

        var deleteSuccessful = function () {
            showAlert('Delete Successful', 'Last page successfully deleted.');
        }
    </script>
    <style type="text/css">
        .images-view .x-panel-body{
	        background: white;
	        font: 11px Arial, Helvetica, sans-serif;
        }
        .images-view .thumb{
	        background: #dddddd;
	        padding: 3px;
        }
        .images-view .thumb img{
	        height: 60px;
	        width: 80px;
        }
        .images-view .thumb-wrap{
	        float: left;
	        margin: 4px;
	        margin-right: 0;
	        padding: 5px;
	        text-align:center;
        }
        .images-view .thumb-wrap span{
	        display: block;
	        overflow: hidden;
	        text-align: center;
        }

        .images-view .x-view-over{
            border:1px solid #dddddd;
            background: #efefef url(../../Shared/images/row-over.gif) repeat-x left top;
	        padding: 4px;
        }

        .images-view .x-view-selected{
	        background: #eff5fb url(../../Shared/images/selected.gif) no-repeat right bottom;
	        border:1px solid #99bbe8;
	        padding: 4px;
        }
        .images-view .x-view-selected .thumb{
	        background:transparent;
        }

        .images-view .loading-indicator {
	        font-size:11px;
	        background-image:url(../../Shared/images/loading.gif);
	        background-repeat: no-repeat;
	        background-position: left;
	        padding-left:20px;
	        margin:10px;
        }
    </style>  
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Hidden runat="server" ID="hiddenRandomKey">
    </ext:Hidden>
    <ext:Hidden runat="server" ID="hdnTotalPages">
    </ext:Hidden>
    <ext:Viewport ID="Viewport1" runat="server" Layout="Border">
        <Items>
            <ext:Panel ID="HeaderPanel" runat="server" Header="false" Region="North" Border="false">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="btnDeleteDocument" runat="server" Text="Delete" Icon="Delete" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnDeleteLastPage_Click" Success="deleteSuccessful();">
                                        <Confirmation ConfirmRequest="true" Message="Are you sure you want to delete the last page?" />
                                        <EventMask ShowMask="true" Msg="Deleting last page..." />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeleteSeparator" Hidden="true"/>
                            <ext:Button ID="btnApprove" runat="server" Text="Approve" Icon="Accept" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnApprove_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnApproveSeparator" Hidden="true"/>
                            <ext:Button ID="btnReject" runat="server" Text="Reject" Icon="Decline" Hidden="true">
                                <DirectEvents>
                                    <Click OnEvent="btnReject_Click">
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnRejectSeparator" Hidden="true"/>
                            <ext:ToolbarFill>
                            </ext:ToolbarFill>
                            <ext:Button ID="btnClose" runat="server" Text="Close" Icon="Cancel">
                                <Listeners>
                                    <Click Handler="window.proxy.requestClose();"/>
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
            </ext:Panel>
            <ext:Panel ID="panelDocument" runat="server" Width="300" Layout="Form" Region="West"
                Padding="10">
                <Items>
                    <ext:Label runat="server" ID="lblDocumentName" Text="Document Name" Hidden="true">
                    </ext:Label>
                    <ext:Label runat="server" ID="lblDateSubmitted" FieldLabel="Date Submitted" Text="Date Submitted">
                    </ext:Label>
                    <ext:Label runat="server" ID="lblDescription" FieldLabel="Description" Text="Description">
                    </ext:Label>
                </Items>
            </ext:Panel>
            <ext:Panel ID="Panel3" runat="server" Border="false" Region="Center" Layout="Center">
                <Items>
                    <ext:Panel ID="Panel1" runat="server" Border="false"
                        Layout="FitLayout" Height="570">
                        <CustomConfig>
                            <ext:ConfigItem Name="width" Value="95%" Mode="Value" />
                        </CustomConfig>
                        <Items>
                            <ext:DataView ID="DataView1" runat="server"
                                OverClass="x-view-over" ItemSelector="div.thumb-wrap" EmptyText="No images to display" Height="570">
                                <Store>
                                    <ext:Store runat="server" ID="strPageGridPanel" RemoteSort="false">
                                        <Reader>
                                            <ext:JsonReader IDProperty="RandomKey">
                                                <Fields>
                                                    <ext:RecordField Name="DocumentId" />
                                                    <ext:RecordField Name="FileName"/>
                                                    <ext:RecordField Name="FilePath" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <Template ID="Template1" runat="server">
                                    <Html>
                                        <tpl for=".">
								           <div class="thumb"><img id="img" src="{FilePath}" title="{FileName}" width="100%" height="575"></div>
							            </tpl>
                                        <div class="x-clear"></div>
                                    </Html>
                                </Template>
                            </ext:DataView>
                        </Items>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolbar1" runat="server" StoreID="strPageGridPanel" PageSize="1" HideRefresh="true" />
                        </BottomBar>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
