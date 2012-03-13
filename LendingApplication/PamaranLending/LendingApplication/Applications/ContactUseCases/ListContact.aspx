<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListContact.aspx.cs" Inherits="LendingApplication.Applications.ContactUseCases.ListContact" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customers List</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <link href="../../Resources/css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init(['addcontactorganization', 'addcontactperson', 'editcontactperson', 'editcontactorganization', 'changeurltoeditorg', 'changeurltoeditperson', 'deletecontact', 'addforextransaction']);
            window.proxy.on('messagereceived', onMessageReceived);
        });

        var onMessageReceived = function (msg) {
            if (msg.tag == 'changeurltoeditperson') {
                window.proxy.requestClose('AddContactPerson');
                openPerson(msg.data.id);
            } else if (msg.tag == 'changeurltoeditorg') {
                window.proxy.requestClose('AddContactOrganization');
                openOrg(msg.data.id);
            } else {
                PageGridPanel.reload();
            }
        };

        var openOrg = function (id) {
            var url = '/Applications/ContactUseCases/EditContactOrganization.aspx';
            var id = 'id=' + id;
            var param = url + "?" + id;
            window.proxy.requestNewTab('UpdateContact', param, 'Update Contact');
        };

         var openPerson = function (id) {
            var url = '/Applications/ContactUseCases/EditContactPerson.aspx';
            var id = 'id=' + id;
            var param = url + "?" + id;
            window.proxy.requestNewTab('UpdateContact', param, 'Update Contact');
        };

        var onBtnOpenClick = function () {
            var selectedRows = PageGridPanelSelectionModel.getSelections();
            if (selectedRows && selectedRows.length > 0) {
                var url;
                if (selectedRows[0].json.PartyType == 'Person') {
                    url = '/Applications/ContactUseCases/EditContactPerson.aspx';
                }
                else {
                    url = '/Applications/ContactUseCases/EditContactOrganization.aspx';
                }

                var values = 'id=' + selectedRows[0].id;
                var param = url + "?" + values;
                window.proxy.requestNewTab('UpdateContact', param, 'Update Contact');
            }
            else {
                showAlert('Status', 'No row/rows selected.');
            }
        };
        var onMenPersonClick = function () {
            window.proxy.requestNewTab('AddContactPerson', '/Applications/ContactUseCases/AddContactPerson.aspx', 'Add Person Contact');
        };
        var onMenOrgClick = function () {
            window.proxy.requestNewTab('AddContactOrganization', '/Applications/ContactUseCases/AddContactOrganization.aspx', 'Add Organization Contact');
        };

        var onRowSelected = function () {
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            X.CanDeleteContact(selectedRow.json.PartyRoleId, {
                success: function (result) {
                    if (result) {
                        btnDelete.enable();
                        btnOpen.enable();
                    } else {
                        btnDelete.disable();
                        btnOpen.enable();
                    }

                }
            });
        };
        var onRowDeselected = function () {
            if (PageGridPanel.hasSelection() == false) {
                btnDelete.disable();
                btnOpen.disable();
            }
        };
        var RefreshItems = function () {
            txtSearch.setValue("");
            PageGridPanel.reload();
        }

        var deleteSuccessful = function () {
            showAlert('Status', 'Contact record was successfully deleted.', function () {
                window.proxy.sendToAll('deletecontact', 'deletecontact');
//                window.proxy.requestClose();
            });
        };

        var onFormValidated = function (valid) {
            //this.getBottomToolbar().setStatus({text : valid ? 'Form is valid. ' : 'Please fill out the form.', iconCls: valid ? 'icon-accept' : 'icon-exclamation'});if (valid && #{PageGridPanel}.store.getCount()){#{btnSave}.enable();}  else{#{btnSave}.disable();}
            btnSave.disable();
            if (valid && PageGridPanel.store.getCount() > 0) {
                PageFormPanelStatusBar.setStatus({ text: 'Form is valid. ' });
                btnSave.enable();
            }
            else {
                PageFormPanelStatusBar.setStatus({ text: 'Please fill out the form.' });
            }
        }

        var onBtnDelete = function () {
            showConfirm('Confirm', 'Are you sure you want to delete the selected record/s?', function (btn) {
                if (btn == 'yes') {
                    canDeleteContact();
                }
            });
        }

        var canDeleteContact = function () {
            var selectedRow = PageGridPanelSelectionModel.getSelected();
            X.CanDeleteContact(selectedRow.json.PartyRoleId, {
                success: function (result) {
                    if (result) {
                        X.DeleteContactInformation({
                            success: function () {
                                onRowDeselected();
                                deleteSuccessful();
                            }
                        });
                    } else {
                        showAlert('Error', 'Cannot delete contact. It is currently being used by another transaction.');
                        PageGridPanel.reload();
                    }
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" IDMode="Explicit" DirectMethodNamespace="X"/>
    <ext:Viewport ID="PageViewPort" runat="server" Layout="Fit">
        <Items>
            <ext:GridPanel ID="PageGridPanel" runat="server" AutoExpandColumn="Address">
             <LoadMask ShowMask="true" />
             <View>
                    <ext:GridView ID="GridView1" ForceFit="true" AutoFill="true" EmptyText="No contacts to display..." runat="server" />
                </View>
                <TopBar>
                    <ext:Toolbar ID="PageGridPanelToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnDelete" runat="server" Text="Delete" Icon = "Delete" Disabled="true">
                                <%--<DirectEvents>
                                    <Click OnEvent="btnDelete_Click" Success="onRowDeselected(); deleteSuccessful();" >
                                        <Confirmation ConfirmRequest="true" Title="Confirm Delete" Message="Are you sure you want to delete the selected customer contacts?" />
                                        <EventMask ShowMask="true" Msg="Deleting.." />
                                    </Click>
                                </DirectEvents>--%>
                                <Listeners>
                                    <Click Handler="onBtnDelete();"/>
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="btnDeleteSeparator" />
                              <ext:Button ID="btnOpen" runat="server" Text="Open" Icon="NoteEdit" Disabled="true">
                                <Listeners>
                                    <Click Handler="onBtnOpenClick();"/>
                                </Listeners>
                            </ext:Button>
                             <ext:ToolbarSeparator ID="btnOpenSeparator"  />
                             <ext:Button ID="btnNew" runat="server" Text="Add" Icon = "Add">
                                <Menu>
                                    <ext:Menu ID="Menu1" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="menPerson" runat="server" Text="Person" Icon =  "User">
                                                <Listeners>
                                                    <Click Handler="onMenPersonClick();" />
                                                </Listeners>
                                            </ext:MenuItem>
                                            <ext:MenuItem ID="menOrg" runat="server" Text="Organization" Icon = "GroupAdd">
                                                <Listeners>
                                                    <Click Handler="onMenOrgClick();" />
                                                </Listeners>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                            <ext:ToolbarFill>
                            </ext:ToolbarFill>
                            <ext:ComboBox runat="server" ID="cmbFilterBy" Width="170"
                                DisplayField="Name" ValueField="Id" Editable="false" TypeAhead="true" Mode="Local"
                                EmptyText="Search by..." ForceSelection="true" TriggerAction="All" SelectOnFocus="true"
                               LabelWidth="50">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" Qtip="Remove selected" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.clearValue();RefreshItems();" />
                                </Listeners>
                                 <Store>
                                    <ext:Store runat="server" ID="storePartyType" RemoteSort="false">
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id">
                                                <Fields>
                                                    <ext:RecordField Name="Id" />
                                                    <ext:RecordField Name="Name" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSearch" runat="server">
                            </ext:TextField>
                            <ext:Button ID="btnSearch" runat="server" Text="Search" OnDirectClick = "btnSearch_Click" Icon = "Find">
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
                            <ext:JsonReader IDProperty="PartyRoleId">
                                <Fields>
                                    <ext:RecordField Name="PartyRoleId" />
                                    <ext:RecordField Name="Name" />
                                    <ext:RecordField Name="Address" />
                                    <ext:RecordField Name="PartyType" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                    </ext:Store>
                </Store>
                <SelectionModel>
                    <ext:RowSelectionModel ID="PageGridPanelSelectionModel" runat="server" SingleSelect="false" >
                        <Listeners>
                            <RowDeselect Handler="onRowDeselected();" />
                            <RowSelect Handler="onRowSelected();" />
                        </Listeners>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <KeyMap>
                <ext:KeyBinding>
                <Keys>
                    <ext:Key Code="DELETE" />
                </Keys>
                <Listeners>
                    <Event Handler="deleteRows(#{PageGridPanel});" />
                </Listeners>
                </ext:KeyBinding>
                </KeyMap>
                <ColumnModel runat="server" ID="PageGridPanelColumnModel">
                    <Columns>
                        <ext:Column Header="Contact ID" DataIndex="PartyRoleId" Wrap="true" Locked="true"
                            Width="200" />
                        <ext:Column Header="Name" DataIndex="Name" Wrap="true" Locked="true" Width="200" />
                        <ext:Column Header="Address" DataIndex="Address" Locked="true" Wrap="true" Width="200" />
                        <ext:Column Header="Party Type" DataIndex="PartyType" Locked="true" Wrap="true" Width="200" />
                    </Columns>
                </ColumnModel>
                <BottomBar>
                    <ext:PagingToolbar ID="PageGridPanelPagingToolBar" runat="server" PageSize="20" DisplayInfo="true"
                        DisplayMsg="Displaying customer contacts {0} - {1} of {2}" EmptyMsg="No customer contacts to display" />
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
