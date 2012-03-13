<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintLoanApplicationForm.aspx.cs"
    Inherits="LendingApplication.Applications.LoanApplicationUseCases.PrintLoanApplicationForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print Loan Application Form</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
        });

        function printContent(printpage) {
            window.print();
        }

    </script>
    <style type="text/css">
        body
        {
            font-family: tahoma,verdana,sans-serif;
            margin: 0;
            padding: 0px;
            font-size: 12px;
            background-color: #fff !important;
        }
        .style3
        {
            width: 358px;
        }
        .style4
        {
            width: 356px;
        }
        .style5
        {
            width: 402px;
        }
        .style6
        {
            width: 403px;
        }
        .style7
        {
            width: 264px;
        }
        .style14
        {
            text-align: center;
            border-bottom: 1px solid black;
        }
        .style15
        {
            text-align: center;
            width: 246px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style16
        {
            text-align: center;
            width: 246px;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style17
        {
            text-align: center;
            height: 18px;
            border-bottom: 1px solid black;
        }
        .style18
        {
            text-align: center;
            width: 246px;
            border-right: 1px solid black;
        }
        .style19
        {
            text-align: center;
        }
        .style20
        {
            text-align: center;
            width: 177px;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style21
        {
            text-align: center;
            width: 177px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style22
        {
            text-align: center;
            height: 18px;
            border-bottom: 1px solid black;
            border-right: 1px solid black;
            width: 154px;
        }
        .style23
        {
            text-align: center;
            width: 154px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style24
        {
            text-align: center;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
            width: 140px;
        }
        .style25
        {
            text-align: center;
            width: 140px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style27
        {
            text-align: center;
            border-bottom: 1px solid black;
            width: 246px;
        }
        .style29
        {
            text-align: center;
            width: 218px;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style30
        {
            text-align: center;
            width: 218px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style31
        {
            width: 218px;
        }
        .style32
        {
            text-align: center;
            width: 270px;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style33
        {
            text-align: center;
            width: 270px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style34
        {
            width: 270px;
        }
        .style35
        {
            text-align: center;
            width: 289px;
            height: 18px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style36
        {
            text-align: center;
            width: 289px;
            border-right: 1px solid black;
            border-bottom: 1px solid black;
        }
        .style37
        {
            width: 289px;
        }
        .box-component
        {
            border-style: solid;
            border-color: Black;
            text-align: center;
            border-width: 1px;
        }
        .checkbox
        {
            border-style: solid;
            border-color: Black;
            border-width: 1px;
        }
        .style39
        {
            width: 360px;
        }
        .checkbox-clear
        {
            border-style: solid;
            border-color: White;
            border-width: 1px;
        }
        .page-break
        {
            display: block;
            page-break-after: always;
        }
        @media screen
        {
            #toolBar
            {
                display: block;
            }
        }
        @media print
        {
            #toolBar
            {
                display: none;
            }
            #PrintableContent
            {
                position:static;
            }
        }
    </style>
</head>
<body>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" />
    <div id="toolBar">
    <ext:Panel ID="RootPanel" runat="server" Border="false">
        <TopBar>
            <ext:Toolbar runat="server" ID="PageToolBar">
                <Items>
                    <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer">
                        <Listeners>
                            <Click Handler="printContent('PrintableContent');" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                        <Listeners>
                            <Click Handler="window.proxy.requestClose();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        </ext:Panel>
        </div>
        <div id="PrintableContent">
            <table style="width: 700px; margin: 0 auto;">
                <%--HEADER--%>
                <tr class="heading">
                    <td style="text-align: center;">
                        <asp:Label ID="lblLenderNameHeader" runat="server" Font-Bold="true" /><br />
                        <asp:Label ID="lblStreetAddress" runat="server" />
                        <asp:Label ID="lblBarangay" runat="server" /><br />
                        <asp:Label ID="lblMunicipality" runat="server" /><asp:Label ID="lblCity" runat="server" />,
                        <asp:Label ID="lblProvince" runat="server" />,
                        <asp:Label ID="lblCountry" runat="server" />,
                        <asp:Label ID="lblPostalCode" runat="server" /><br />
                        tel#:
                        <asp:Label ID="lblPrimTelNumber" runat="server" />,
                        <asp:Label ID="lblSecTelNumber" runat="server" /><br />
                        fax#:<asp:Label ID="lblFaxNumber" runat="server" /><br />
                        <asp:Label ID="lblEmailAddress" runat="server" />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr class="nameOfDocument">
                    <td style="text-align: center; font-weight: bold;">
                        Loan Application Form<br />
                        <br />
                        <br />
                    </td>
                </tr>
                <%--BASIC INFORMATION--%>
                <tr class="BorrowersBasicInformation">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <b>Borrower's Basic Information</b><br />
                                    <br />
                                    Name: _____________________________________________________________<br />
                                    <br />
                                    District: _______________________ &nbsp; &nbsp; Station Number: _____________________<br />
                                    <br />
                                    Gender: _______________________ &nbsp; &nbsp; Birthdate: __________________________<br />
                                    <br />
                                    Birthplace: __________________________________________________________
                                    <br />
                                    <br />
                                    Mother's Maiden Name: ________________________________________________
                                    <br />
                                    <br />
                                    Number of Dependents: _______________________________________________
                                    <br />
                                    <br />
                                </td>
                                <td id="AccountOwnerImage" style="text-align: right;">
                                    <%--<ext:Image runat="server" ID="imgAccountOwner" Width="200" Height="200" ImageUrl="C:\Users\Agent-3\Desktop\balbasur.gif"></ext:Image>--%>
                                    <%--<img src="../../../Uploaded/Images/pnam.jpg" alt="Alternate Text" width="200" height="200"/>--%>
                                    <ext:BoxComponent ID="BoxComponent1" runat="server" Width="200" Height="200" Cls="box-component">
                                        <Content>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <asp:Label ID="Label1" runat="server" Text="2 x 2 Image"></asp:Label>
                                        </Content>
                                    </ext:BoxComponent>
                                    <%--<ext:Image ID="imgPersonPicture" runat="server" ImageUrl="" Width="200" Height="200" />--%>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td style="width: 300px;">
                                    Marital Status:<br />
                                    <br />
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Number
                                    of Dependents: _________________________<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <%--MARITAL STATUS--%>
                                <td style="text-align: justify; padding-left: 45px;">
                                    <ext:CompositeField ID="CompositeField1" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent2" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label2" runat="server" Text="Single">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField2" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent3" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label3" runat="server" Text="Married">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField3" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent4" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label4" runat="server" Text="Divorced">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                                <td>
                                    <ext:CompositeField ID="CompositeField4" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent5" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label5" runat="server" Text="Widowed">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField5" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent6" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label6" runat="server" Text="Annulled">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField6" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent7" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label7" runat="server" Text="Legally Separated">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width: 300px;">
                                    Educational Attainment:<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <%--EDUCATIONAL ATTAINMENT--%>
                                <td style="text-align: justify; padding-left: 45px;">
                                    <ext:CompositeField ID="CompositeField7" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent8" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label8" runat="server" Text="Elementary Graduate">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField8" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent9" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label9" runat="server" Text="High School Graduate">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField9" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent10" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label10" runat="server" Text="College Undergraduate">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                                <td class="style39">
                                    <ext:CompositeField ID="CompositeField10" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent11" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label11" runat="server" Text="College Graduate">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField11" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent12" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label12" runat="server" Text="Post-Graduate">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField12" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent13" runat="server" Width="13" Height="13" Cls="checkbox-clear" />
                                            <ext:Label ID="Label13" runat="server">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width: 300px;">
                                    Home Ownership:<br />
                                    <br />
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Residence
                                    Since: _________________________<br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <%--HOME OWNERSHIP--%>
                                <td style="text-align: justify; padding-left: 45px;">
                                    <ext:CompositeField ID="CompositeField13" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent14" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label14" runat="server" Text="Owned">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField14" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent15" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label15" runat="server" Text="Living with Relatives">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                                <td>
                                    <ext:CompositeField ID="CompositeField15" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent16" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label16" runat="server" Text="Rented">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                    <ext:CompositeField ID="CompositeField16" runat="server">
                                        <Items>
                                            <ext:BoxComponent ID="BoxComponent17" runat="server" Width="13" Height="13" Cls="checkbox" />
                                            <ext:Label ID="Label17" runat="server" Text="Company Owned">
                                            </ext:Label>
                                        </Items>
                                    </ext:CompositeField>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--CONTACT INFORMATION--%>
                <tr class="BorrowersContactInformation">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <b>Borrower's Contact Information</b><br />
                                    <br />
                                    <br />
                                    Primary Home Address: _______________________________________________________________________<br />
                                    <br />
                                    Seconday Home Address: _____________________________________________________________________<br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td class="style5">
                                                Cellphone Number: __________________________________<br />
                                                <br />
                                            </td>
                                            <td class="style3">
                                                Telephone Number: _____________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style5">
                                                Primary E-mail Address: _______________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style5">
                                                Secondary E-mail Address: ____________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr> 
            </table>
            <div style="page-break-before:always;"></div>
            <br /><br />
            <table style="width: 700px; margin: 0 auto;">
                <%--EMPLOYMENT INFORMATION--%>
                <tr class="BorrowersContactInformation">
                    <td>
                        <b>Borrower's Employment Information</b><br />
                        <br />
                    </td>
                </tr>
                <tr class="BorrowersEmploymentInformation">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td class="style6">
                                                Employer:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                Employment Address:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                Telephone Number: ________________________________<br />
                                                <br />
                                            </td>
                                            <td class="style4">
                                                Fax Number: ____________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                E-mail Address: ____________________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                Employee ID NUmber: ______________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                Position: _________________________________________<br />
                                                <br />
                                            </td>
                                            <td class="style4">
                                                Employment Status: ______________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                Salary (Php):&nbsp; _____________________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                SSS Number: _____________________________________<br />
                                            </td>
                                            <td class="style4">
                                                GSIS NUmber: __________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                OWA Number: ____________________________________<br />
                                                <br />
                                            </td>
                                            <td class="style4">
                                                PHIC NUmber: __________________________<br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--SPOUSE INFORMATION--%>
                <tr class="SpouseInformation">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <b>Spouse Information</b><br />
                                    <br />
                                    Name: _______________________________________________________________________<br />
                                    <br />
                                    Birthdate: _____________________________________________________________________<br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--LOAN APPLICATION DETAILS--%>
                <tr class="LoanApplicationDetails">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <b>Loan Application Details</b><br />
                                    <br />
                                    Application Date: ___________________________________<br />
                                    <br />
                                    Loan Product Name: ________________________________<br />
                                    <br />
                                    Loan Amount (Php): ________________________________<br />
                                    <br />
                                    Loan Term: _______________________________________<br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td class="style7">
                                                Collateral Requirement:<br />
                                                <br />
                                            </td>
                                            <td>
                                                Interest Computation Mode:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--COLLATERAL REQUIREMENT--%>
                                            <td style="text-align: justify; padding-left: 45px;" class="style7">
                                                <ext:CompositeField ID="CompositeField17" runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Secured">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Unsecured">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                            </td>
                                            <%--INTEREST COMPUTATION MODE--%>
                                            <td style="text-align: justify; padding-left: 45px;" class="style7">
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Straight Line Method">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Diminishing Balance Method">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style7">
                                                Payment Mode:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--PAYMENT MODE--%>
                                            <td style="text-align: justify; padding-left: 45px;" class="style7">
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Daily">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Semi-Monthly">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Semi-Annually">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                            </td>
                                            <td style="text-align: justify; padding-left: 45px;" class="style7">
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Weekly">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Monthly">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Annually">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style7">
                                                Method of Charging Interest:<br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: justify; padding-left: 45px;" class="style7">
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Add-On Interest">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                                <ext:CompositeField runat="server">
                                                    <Items>
                                                        <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                                        <ext:Label runat="server" Text="Discounted Interest">
                                                        </ext:Label>
                                                    </Items>
                                                </ext:CompositeField>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div style="page-break-before:always;"></div>
            <br /><br />
            <table style="width: 700px; margin: 0 auto;">
                <%--OUTSTANDING LOANS TO PAY OFF--%>
                <%--<tr id="Tr4">
                    <td>
                        <ext:CompositeField runat="server">
                            <Items>
                                <ext:BoxComponent runat="server" Width="13" Height="13" Cls="checkbox" />
                                <ext:Label runat="server" Text="Pay the following outstanding loan/s using the proceeds of my new loan">
                                </ext:Label>
                            </Items>
                        </ext:CompositeField>
                        <br />
                        <br />
                        <b>Outstanding Loan/s to Pay Off<br />
                            <br />
                        </b>
                        <table style="width: 98%; border: 1px solid black;">
                            <tr>
                                <td class="style16">
                                    <b>Product Name</b>
                                </td>
                                <td class="style35">
                                    <b>Interest Computation Mode</b>
                                </td>
                                <td class="style16">
                                    <b>Maturity Date</b>
                                </td>
                                <td class="style29">
                                    <b>No. of Installments</b>
                                </td>
                                <td class="style32">
                                    <b>Payment Mode</b>
                                </td>
                                <td class="style16">
                                    <b>Scheduled Amortization</b>
                                </td>
                                <td class="style17">
                                    <b>Loan Balance</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style36">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style30">
                                    &nbsp;
                                </td>
                                <td class="style33">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style27">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style36">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style30">
                                    &nbsp;
                                </td>
                                <td class="style33">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style27">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style36">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style30">
                                    &nbsp;
                                </td>
                                <td class="style33">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style27">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style36">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style30">
                                    &nbsp;
                                </td>
                                <td class="style33">
                                    &nbsp;
                                </td>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style27">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="text-align: center; border-right: 1px solid black;" class="style37">
                                    &nbsp;
                                </td>
                                <td style="text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="text-align: center; border-right: 1px solid black;" class="style31">
                                    &nbsp;
                                </td>
                                <td style="text-align: center; border-right: 1px solid black;" class="style34">
                                    &nbsp;
                                </td>
                                <td style="text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="text-align: center;">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                <%--CO-BORROWERS--%>
                <tr id="CoBorrowers">
                    <td>
                        <b>
                            <br />
                            Co-Borrower/s<br />
                            <br />
                        </b>
                        <br />
                        <table style="width: 98%; border: 1px solid black;">
                            <tr>
                                <td class="style16">
                                    <b>Name</b>
                                </td>
                                <td class="style17">
                                    <b>Primary Home Address</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style18">
                                    &nbsp;
                                </td>
                                <td class="style19">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--GUARANTORS--%>
                <tr id="Tr1">
                    <td>
                        <b>
                            <br />
                            Guarantor/s<br />
                            <br />
                        </b>
                        <table style="width: 98%; border: 1px solid black;">
                            <tr>
                                <td class="style16">
                                    <b>Name</b>
                                </td>
                                <td class="style17">
                                    <b>Primary Home Address</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style18">
                                    &nbsp;
                                </td>
                                <td class="style19">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--COLLATERALS--%>
                <tr id="Tr2">
                    <td>
                        <b>
                            <br />
                            Collateral/s<br />
                            <br />
                        </b>
                        <table style="width: 98%; border: 1px solid black;">
                            <tr>
                                <td class="style16">
                                    <b>Collateral Type</b>
                                </td>
                                <td class="style17">
                                    <b>Collateral Description</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style15">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style18">
                                    &nbsp;
                                </td>
                                <td class="style19">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--SUBMITTED DOCUMENTS--%>
                <tr id="Tr3">
                    <td>
                        <b>
                            <br />
                            Submitted Document/s<br />
                            <br />
                        </b>
                        <table style="width: 98%; border: 1px solid black;">
                            <tr>
                                <td class="style20">
                                    <b>Document Name</b>
                                </td>
                                <td class="style22">
                                    <b>Date Submitted</b>
                                </td>
                                <td class="style24">
                                    <b>Status</b>
                                </td>
                                <td class="style17">
                                    <b>Description</b>
                                </td>
                            </tr>
                            <tr>
                                <td class="style21">
                                    &nbsp;
                                </td>
                                <td class="style23">
                                    &nbsp;
                                </td>
                                <td class="style25">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style21">
                                    &nbsp;
                                </td>
                                <td class="style23">
                                    &nbsp;
                                </td>
                                <td class="style25">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style21">
                                    &nbsp;
                                </td>
                                <td class="style23">
                                    &nbsp;
                                </td>
                                <td class="style25">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style21">
                                    &nbsp;
                                </td>
                                <td class="style23">
                                    &nbsp;
                                </td>
                                <td class="style25">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 177px; text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="width: 154px; text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="width: 140px; text-align: center; border-right: 1px solid black;">
                                    &nbsp;
                                </td>
                                <td style="width: 140px; text-align: center;">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--REMARKS--%>
                <tr class="Remarks">
                    <td style="text-align: justify;">
                        <table>
                            <tr style="vertical-align: top;">
                                <td style="text-align: left;">
                                    <br />
                                    <br />
                                    <b>Processed by:</b><br />
                                    <br />
                                    <br />
                                    <b>Approved/Rejected by:</b><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
