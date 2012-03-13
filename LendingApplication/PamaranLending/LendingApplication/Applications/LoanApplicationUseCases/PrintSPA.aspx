<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintSPA.aspx.cs" Inherits="LendingApplication.Applications.LoanApplicationUseCases.PrintSPA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="PageHeader" runat="server">
    <title>Print SPA</title>
    <ext:ResourcePlaceHolder ID="ScriptPlaceHolder" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="StylePlaceHolder" runat="server" Mode="Style" />
    <script src="../../Resources/js/miframe2_1_5/build/miframe.js" type="text/javascript"></script>
    <script src="../../Resources/js/miframe2_1_5/mifmsg.js" type="text/javascript"></script>
    <script src="../../Resources/js/ntfx.MIFMessagingProxy.js" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            window.proxy = new ntfx.MIFMessagingProxy();
            window.proxy.init();
            //editOr();
        });

        function printContent(printpage) {
            window.print();
        }

//        var editOr = function () {
//            var target = Ext.select(".divClass");
//            TextBlockEditor.retarget(target);
//            
//        };
//        var endEdit = function() {
//            TextBlockEditor.completeEdit();
//            Button1.show();
//            btnCompleteEdit.hide();
//        }

        var onBtnSaveSignatures = function () {
            X.SaveSignatures({
                success: function (result) {
                    var isComplete = true;

                    lender = {};
                    lender.Signature = hdnLender.getValue();
                    lender.PersonName = txtLender.getValue();

                    borrower = {};
                    borrower.Signature = hdnBorrower.getValue();

                    witness1 = {};
                    witness1.Signature = hdnWitness1.getValue();
                    witness1.PersonName = txtWitness.getValue();

                    witness2 = {};
                    witness2.Signature = hdnWitness2.getValue();
                    witness2.PersonName = txtWitness2.getValue();

                    witness3 = {};
                    witness3.Signature = hdnWitness3.getValue();
                    witness3.PersonName = txtWitness3.getValue();

                    witness4 = {};
                    witness4.Signature = hdnWitness4.getValue();
                    witness4.PersonName = txtWitness4.getValue();

                    if (lender.PersonName == "" || lender.Signature == "" ||
                        borrower.Signature == "" ||
                        witness1.PersonName == "" || witness1.Signature == "" ||
                        witness2.PersonName == "" || witness2.Signature == "" ||
                        witness3.PersonName == "" || witness3.Signature == "" ||
                        witness4.PersonName == "" || witness4.Signature == "") {
                        isComplete = false;
                    }

                    data = {};
                    data.Lender = lender;
                    data.Borrower = borrower;
                    data.Witness1 = witness1;
                    data.Witness2 = witness2;
                    data.Witness3 = witness3;
                    data.Witness4 = witness4;
                    data.IsComplete = isComplete;

                    window.proxy.sendToAll(data, 'spadetailsaved');
                    window.proxy.requestClose();
                    //btnPrint.enable();
                }
            });
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
        .style12
        {
            text-align: center;
        }
        .style18
        {
            width: 100px;
        }
        .style19
        {
            text-align: left;
        }
                
        td
        {
            text-align: justify;
        }
        underline
        {
            text-decoration: underline;
        }
        .style21
        {
            width: 310px;
        }
        .style13
        {
            text-align: center;
            text-decoration: underline;
            font-weight: bold;
        }
        .style24
        {
            width: 214px;
        }
        .page-break
            {
                display:block;
                page-break-before:always;
            }
        

        @media print
        {
            #toolBar
            {
                display: none;
            }
            
        }
        
        @media screen
        {
            #toolBar
            {
                display: block;
            }
        }
    </style>
    <style media="print" type="text/css">
        @page
        {
            size: 8.5in 14in;
            margin: 0.35in;
        }
    </style>
</head>
<body>
    <center>
    <form id="PageForm" runat="server">
    <ext:ResourceManager ID="PageResourceManager" runat="server" DirectMethodNamespace="X" IDMode="Explicit"/>
    <ext:Hidden runat="server" ID="hdnLoanApplicationId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnCustomerId"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnMode"></ext:Hidden>

    <ext:Hidden runat="server" ID="hdnWitness1"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness2"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness3"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnWitness4"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnLender"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnBorrower"></ext:Hidden>

    <ext:Hidden runat="server" ID="hdnIsSigned"></ext:Hidden>
    <ext:Hidden runat="server" ID="hdnResourceGuid"></ext:Hidden>
    <div id="toolBar">
        <ext:FormPanel ID="FormPanel1" runat="server" Layout="FitLayout" Border="false">
            <TopBar>
                <ext:Toolbar ID="PageToolBar" runat="server">
                    <Items>
                        <ext:Button runat="server" ID="btnSaveSignatures" Text="Save" Icon="Disk">
                            <Listeners>
                                <Click Handler="onBtnSaveSignatures();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSpacer /><ext:ToolbarSeparator /><ext:ToolbarSpacer />
                        <ext:Button runat="server" ID="btnPrint" Text="Print" Icon="Printer" Disabled="true">
                            <Listeners>
                                <Click Handler="printContent(PrintableContent);"/>
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarSpacer /><ext:ToolbarSeparator /><ext:ToolbarSpacer />
                        <ext:Button runat="server" ID="btnClose" Text="Close" Icon="Cancel">
                            <Listeners>
                                <Click Handler="window.proxy.requestClose();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:FormPanel>
    </div>
        <ext:Panel ID="MainPanel" runat="server" AutoHeight="true" Border="false">
            <Content>
                <div id="PrintableContent">
                    <table style="width: 702px;>
                        <%--HEADER--%>
                        <tr class="heading">
                            <td class="style12">
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
                            </td>
                        </tr>
                        <tr class="nameOfDocument">
                            <td style="text-align: center; font-weight: bold; text-decoration: underline;" 
                                class="style1">
                                PROMISSORY NOTE WITH AUTHORIZATION & POWER OF ATTORNEY<br />
                                <br />
                            </td>
                        </tr>
                        <tr class="loanDetails1">
                            <td>
                                <table>
                                    <tr>
                                        <td style="text-align: left; font-weight: bold; "> ₱  </td>
                                        <td style="text-align: left; text-decoration: underline" class="style1;">
                                            <asp:Label runat="server" ID="lblLoanAmount"></asp:Label>
                                        </td>
                                        <td class="style21">
                                        </td>
                                        <td style="text-align: right; font-weight: bold; width: 197px;">
                                            DATE OF LOAN:  &nbsp;
                                        </td>
                                        <td style="text-align: right; text-decoration: underline" class="style1;">
                                            <%--<asp:Label runat="server" ID="lblLoanReleaseDate"></asp:Label>--%>
                                            <ext:Label runat="server" ID="lblReleaseDate"></ext:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="loanDetails2">
                            <td>
                                <table><tr>
                                    <td style="width: 375px;"></td>
                                    <td style="text-align: right; font-weight: bold; width:200px;">
                                        DUE
                                        DATE OF LOAN: &nbsp;
                                    </td>
                                    <td style="text-align: right; text-decoration: underline" class="style1;">
                                            <%--<asp:Label runat="server" ID="lblLoanReleaseDate"></asp:Label>--%>
                                            <ext:Label runat="server" ID="lblMaturityDate"></ext:Label>
                                        </td>
                                </tr></table>
                            </td>
                        </tr>
                        <%--NOTE--%>
                        <tr class="SPANote">
                            <td>
                                <br />
                                <div class="divClass" field="tar">
                                <table>
                                    <tr>
                                        <td style="text-align: justify;">
                                            &nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;
                                            FOR VALUE RECEIVED, I/WE, hereinafter referred to as the "Borrower(s)/Co-Maker(s)" jointly and severally,
                                            promise to pay MN Pamaran Lending Investors, Inc., hereinafter referred to as the "Lender", at its office in Pagadian City,
                                            on or before the 15th/30th day of the month, the principal sum of
                                            <asp:Label runat="server" ID="lblPrincipalLoan" CssClass="style13"></asp:Label> PESOS ( ₱ <asp:Label runat="server" ID="lblPrincipal" CssClass="style13"></asp:Label> ), 
                                            together with interest thereof at
                                            <asp:Label runat="server" ID="lblPercentInWords" CssClass="style13"></asp:Label> PERCENT ( <asp:Label runat="server" ID="lblInterestRate" CssClass="style13"></asp:Label>% ) per month from date hereof until fully paid under the 
                                            following terms and conditions:
                                            <br />
                                            <br />
                                            <p style="text-indent: 30pt; margin: 0em;">1.&nbsp;&nbsp;&nbsp;That I/WE shall first pay the monthly stipulated interest 
                                            computed on the basis of the balance of the principal
                                            sum in any month covered by this More and, my/our option, I/WE may pay partial 
                                            amounts to be applied to the principal
                                            sum any time within the term of this Note; provided, however that on the expiry 
                                            or due date of the loan, I/We shall pay  the entire balance of the principal loan, including unpaid interest thereon.
                                            </p>
                                            <p style="text-indent: 30pt; margin: 0em 0em 0em 0em;">2.&nbsp;&nbsp;&nbsp;That failure topay any monthly interest constitutes default. Thereafter, the balance of the 
                                            principal loan shall become due and payable without need of demand.
                                            </p>
                                            <p style="text-indent: 30pt; margin: 0em;">
                                            3.&nbsp;&nbsp;&nbsp;That acceptance by the "Lender" of partial payments whereof after default shall not be considered as an
                                            extension of the period for payment of any interest, principal sm or waiver or 
                                            novation of my/our obligation under this Note.
                                            </p>
                                            <p style="text-indent: 30pt; margin: 0em;">
                                            4.&nbsp;&nbsp;&nbsp;That if collection or to enforce 
                                            payment of this Note is entrusted to an attortney-at-law, I/We 
                                            shall pay equivalent of TWENTY-FIVE PERCENT (25%) of the amount due on the Notes as 
                                            attorney&#39;s fees and, in case of litigation arising out of or in connection with this Note, the venue may be any proper 
                                            Court of Pagadian City, at the option of the
                                             &quot;Lender&quot;.
                                            </p>
                                            <center><u>AUTHORIZATION</u></center>
                                            <p style="text-indent: 30pt; margin: 0em;">
                                            5.&nbsp;&nbsp;&nbsp;That to facilitate payment of this Note, I/We hereby authorize the School District Supervisor/Principal/School
                                            District&nbsp; Treasurer / Schools Division Office / City or Municipal Treasurer, 
                                            Cashier, Disbursing Officer, Paymaster / Postmaster
                                            or Bank, as the case may be, to release, deliver, and/or pay in favor of the 
                                            &quot;Lender&quot; through its personnel, my/our salaries,
                                            treasury warrants, checks, passbooks or ATM card 
                                            withdrawals, as the case may be or, in payment of this Note, 
                                            I/We hereby issue personal checks, which checks are attached herewith.
                                            </p>
                                            <center><u>POWER OF ATTORNEY</u></center>
                                            <p style="text-indent: 30pt; margin: 0em;">
                                            6.&nbsp;&nbsp;&nbsp;That finally, I/We do 
                                            hereby name and appoint MN Pamaran, Lending 
                                            Investors, Inc., or its authorized
                                            personnel to be my/our true and lawful attorney-in-fact, for my/our name, place 
                                            and stead, to ask, demand, collect or
                                            receive from the Office, Officer or authority concerned the aforementioned 
                                            salaries, checks or bank deposit book/ATM card withdrawals, and thereafter, sign endorse, negotiate and/or encash the same and apply the proceeds 
                                            thereof to the payment of my/our obligation under this Promissory Note.
                                            </p>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>    
                                    <tr>
                                        <td style="text-align: right">IN WITNESS WHEREOF, I/WE have hereunto set my/our hand(s) this <u><asp:Label runat="server" ID="lblDayToday" Text="12th" /></u> day of
                                            <u><asp:Label runat="server" ID="lblMonthToday" Text="December" /></u>, <u><asp:Label runat="server" ID="lblYearToday" Text="2011" /></u>
                                            in the City of Pagadian,
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Philippines.</td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="width: 60px;"><br /></td>
                                                    <td style="text-align: center"><br /><asp:Label runat="server" ID="lblCustomerName" CssClass="style13"></asp:Label><br />Borrower</td>
                                                    <td class="style24"><br /></td>
                                                    <td style="text-align: center"><br /><asp:Label runat="server" ID="lblCoBorrowerName" CssClass="style13"></asp:Label><br />Borrower/Co-maker</td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="width: 60px;"></td>
                                                    <td class="style19">CTC No. <asp:Label runat="server" ID="lblCtcNo" CssClass="style13" Text="_______________"></asp:Label><br />
                                                        Issued on:&nbsp; <asp:Label runat="server" ID="lblCtcIssuedOn" CssClass="style13" Text="_____________"></asp:Label><br />
                                                        At:&nbsp; <asp:Label runat="server" ID="lblCtcIssuedAt" CssClass="style13" Text="__________________"></asp:Label><br />
                                                        School/District:&nbsp; <asp:Label runat="server" ID="lblDistrict" CssClass="style13" Text="____________"></asp:Label><br />
                                                        Home Address: &nbsp; <asp:Label runat="server" ID="lblHomeAddress" CssClass="style13" Text="____________"></asp:Label><br />
                                                        Tel./Mobile #: &nbsp; <asp:Label runat="server" ID="lblContactNo" Text="____________" CssClass="style13" ></asp:Label></td>
                                                    <td class="style18"></td>
                                                    <td style="width: 200px;">CTC No. <asp:Label runat="server" ID="lblCtcNo1" CssClass="style13" Text="_______________"></asp:Label><br />
                                                        Issued on:&nbsp; <asp:Label runat="server" ID="lblCtcIssuedOn1" CssClass="style13" Text="_____________"></asp:Label><br />
                                                        At:&nbsp; <asp:Label runat="server" ID="lblCtcIssuedAt1" CssClass="style13" Text="__________________"></asp:Label><br />
                                                        School/District:&nbsp; <asp:Label runat="server" ID="lblDistrict1" CssClass="style13" Text=" ________________"></asp:Label><br />
                                                        Home Address: &nbsp; <asp:Label runat="server" ID="lblHomeAddress1" CssClass="style13" Text="_______________"></asp:Label><br />
                                                        Tel./Mobile #: &nbsp; <asp:Label runat="server" ID="lblContactNo1" Text="________________" CssClass="style13" ></asp:Label></td>
                                                    <td style="width: 135px;"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr><td></td></tr>
                                                <tr>
                                                    <td style="width: 200px;">
                                                        <br /><br />
                                                        &nbsp;SIGNED IN THE PRESENCE OF: 
                                                    </td>
                                                    <td style="border-bottom: 1px solid Black; width: 200px;">
                                                        <br /><br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <center>
                                                        <ext:Image runat="server" ID="imgWitness" Width="200" Height="60" ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpWitness" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadWitness1"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblWitness"></ext:Label>
                                                        <ext:TextField runat="server" ID="txtWitness" EmptyText="Witness name here.." Width="200" AllowBlank="false"></ext:TextField>
                                                        </center>
                                                    </td>
                                                    <td style="width: 50px;"></td>
                                                    <td style="border-bottom: 1px solid Black; width: 200px;">
                                                        <br /><br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <center>
                                                        <ext:Image runat="server" ID="imgWitness2" Width="200" Height="60" ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpWitness2" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadWitness2"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblWitness2"></ext:Label>
                                                        <ext:TextField runat="server" ID="txtWitness2" EmptyText="Witness name here.." Width="200" AllowBlank="false"></ext:TextField>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr>
                            
                                        <td style="text-align: center; font-weight: bold;"><br /><br />ACKNOWLEDGEMENT</td>
                                    </tr>
                                    <tr>
                                        <td>REPUBLIC OF THE PHILIPPINES)</td>
                                    </tr>
                                    <tr>
                                        <td>CITY OF PAGADIAN ..........)S.S</td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr><td style="text-align: right">BEFORE&nbsp; ME, personally&nbsp; appeared&nbsp; the&nbsp; above-named&nbsp; person(s)&nbsp; with&nbsp; his/their&nbsp; CTC, known&nbsp; to me to be same</td></tr>
                                    <tr><td>person(s) who&nbsp; executed&nbsp; the&nbsp; foregoing instrument and&nbsp; he/they&nbsp; acknowledged&nbsp; to me that the same is their free act and</td></tr>
                                    <tr><td>voluntary deed.</td></tr>
                                    <tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        WITNESS MY HAND AND NOTARIAL SEAL.</td></tr>
                                    <tr><td>
                                        <table>
                                            <tr>
                                                <td>DOC. NO.&nbsp;&nbsp; _________<br />
                                                    PAGE NO.&nbsp;&nbsp; _________<br />
                                                    BOOK NO.&nbsp; _________<br />
                                                    SERIES OF 20 _______</td>
                                                <td style="width: 325px;"></td>
                                                <td>_____________________<br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Notary Public<br />
                                                    Until December 31, 20___<br />
                                                    PTR NO. ______ : ______<br />
                                                    Pagadian City, Philippines</td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td></tr>
                                </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div  class="page-break"></div>
                    <%--<center>- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -</center>--%>
                    <br />
                    <br />
                    <table style="width: 702px; font-size: medium;">
                        <tr id="acknowledgmentHeader">
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            REPUBLIC OF THE PHILIPPINES )
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            IN THE CITY OF PAGADIAN&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)S.S.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            x------------------------------------------x
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="acknowledgmentTitle">
                            <td>
                                <center><b>A C K N O W L E D G M E N T</b></center>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr id="acknowledgmentContent">
                            <td>
                                <table>
                                    <tr>
                                        <td style="text-align: justify;">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>BEFORE ME</b>, a Notary Public for and in the aforesaid place and date, perosnally appeared the above-named person/s with his/her/their corresponding community tax certificate/s, personally known to me to be the same person/s who executed the forgoing instrument and he/she/they acknowledged to me that the same is his/her/their free act and voluntary deed.
                                            <br />
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This document consists of two (2) pages including the page in which  the acknowledgment is being written.
                                            <br />
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>WITNESS MY HAND AND NOTARIAL SEAL.</b>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="acknowledgmentDetails">
                            <td>
                                <br />
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 400px;">
                                            Doc. No.: ___________<br />
                                            Page No.: ___________<br />
                                            Book No.: ___________<br />
                                            Series of ___________
                                        </td>
                                        <td>
                                            _______________________________<br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            Notary Public<br />
                                            Until __________________________<br /> 
                                            PTR No. ________________<br /> 
                                            IBP No. ________________<br />  
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="page-break"></div>
                    <%--<center>- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -</center>
                    --%>
                    <br />
                    <br />
                    <%--MEMORANDUM OF AGREEMENT--%>
                    <table style="width: 702px; font-size: 13pt;">
                        <tr>
                            <td style="text-align: justify;">
                                <center><b>MEMORANDUM OF AGREEMENT</b></center>
                                <br />
                                KNOW ALL MEN BY THESE PRESENTS:
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This Memorandum of Agreement, made and entered into this <u><asp:Label runat="server" ID="lblDayToday1" Text="12th" /></u> day of
                                            <u><asp:Label runat="server" ID="lblMonthToday1" Text="December" /></u>, <u><asp:Label runat="server" ID="lblYearToday1" Text="2012" /></u> at
                                            <u><asp:Label runat="server" ID="lblAt" Text="Pagadian City"></asp:Label></u>, 
                                Philippines, by and between MERARDO NEONITA PAMARAN LENDING INVESTOR, INC. (MNPLI), a corporation duly organized 
                                and existing under the laws of the Republic of the Philippines, represented by its manager, Ms. Deana Pamaran, 
                                with office address at San Pedro District, Pagadian City, hereinafter called the "FIRST PARTY",
                                <br />
                                <center>and</center>
                    
                                ______________________________, with residence/office address at _____________________________, Pagadian City, 
                                hereinafter referred to as the "SECOND PARTY",
                                <br />
                                <center>W I T N E S S E T H: That</center>
                    
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;WHEREAS, the FIRST PARTY is a corporation legitimately engaged in the business of (lending) ____________________;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;WHEREAS, the SECOND PARTY needs to lend money from the FIRST PARTY;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;WHEREAS, the FIRST PARTY is willing to lend money to the SECOND PARTY subject, however, to the terms and conditions 
                                and stipulations below;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;NOW THEREFORE, the FIRST PARTY and the SECOND PARTY have mutually agreed to enter into this agreement subject to the 
                                following terms and conditions and stipulations:
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;1. After the SECOND PARTY has complied with the requisite procedure prescribed by the FIRST PARTY in granting loan to 
                                its clients/borrowers, the SECOND PARTY shall turnover to the FIRST PARTY his/her (SECOND PARTY's) Automated Teller 
                                Machine (ATM) card issued by his/her employer by virtue of his/her employment with the said employer and thru which 
                                his/her salary is to be paid by the said employer and shall give to the FIRST PARTY the said ATM's Personal 
                                Identification Number (PIN); provided, however, that no loan shal be granted by the FIRST PARTY unto the SECOND PARTY 
                                without the turnover of the aforesaid ATM and the giving of its PIN by the latter unto th former;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2. The turnover by the SECOND PARTY of his/her ATM, as well as giving its PIN, to the FIRST PARTY relative to the 
                                provision in the preceding paragraph means the giving of his/her (SECOND PARTY's) authority, power, and consent 
                                voluntarily unto the FIRST PARTY to withdraw money from the said ATM and said withdrawn money shall be considered as 
                                payment by the SECOND PARTY to the FIRST PARTY of the money loaned bby the former from the latter;
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3. The turnover of the ATM and the giving of its PIN by the SECOND PARTY to the FIRST PARTY shall be accompanied by 
                                the execution of a proper document indicating the voluntary turnover of the said ATM and voluntary giving of its PIN 
                                by the SECOND PARTY to the FIRST PARTY in relation to the provision in paragraph 1 above.
                                <br />
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;4. After the SECOND PARTY has fully paid his/her indebtedness with the FIRST PARTY, the latter shall voluntarily 
                                turnover the above-referred ATM to the former.
                                <br />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;5. This agreement becomes valid and binding upon affixing by the FIRST PARTY and SECOND PARTY of their respective 
                                signatures below.
                                <br />
                                <br />
                                <div class="page-break"></div>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Done this <u><asp:Label runat="server" ID="lblDayToday2" Text="12th" /></u> day of
                                            <u><asp:Label runat="server" ID="lblMonthToday2" Text="December" /></u>, <u><asp:Label runat="server" ID="lblYearToday2" Text="2012" /></u>
                                            at <u><asp:Label runat="server" ID="lblAt2" Text="Pagadian City"></asp:Label></u>, Philippines.
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 400px; text-align: center;">
                                            <br /><br />
                                            <b>MNPLI, INC.</b><br />
                                            <b>First Party</b><br />
                                            Represented by<br /><br />
                                            <center>
                                            <table>
                                                <tr>
                                                    <td style="width: 250px; border-bottom: 1px solid Black;">
                                                        <center>
                                                        <ext:Image runat="server" ID="imgLender" Width="250" Height="70" ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpLender" Width="250" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadLender"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblLender"></ext:Label>
                                                        <ext:TextField runat="server" ID="txtLender" Width="250" AllowBlank="false"></ext:TextField>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>
                                            <br />
                                            <br />
                                        </td>
                                        <td style="text-align: center; vertical-align: text-top;">
                                            <br />
                                            <table>
                                                <tr>
                                                    <br />
                                                    <td style="border-bottom: 1px solid Black; width: 400px;">
                                                        <center><ext:Image runat="server" ID="imgBorrower" Width="250" Height="70" ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpBorrower" Width="250" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadBorrower"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblBorrowerName" Text="Borrower Name Here"></ext:Label></center>
                                                    </td>
                                                </tr>
                                            </table>
                                            <b>Second Party</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 400px;">
                                            SIGNED IN THE PRESENCE OF:<br /><br />
                                            <center>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;1&nbsp;.</td>
                                                    <td style="width: 250px; border-bottom: 1px solid Black;">
                                                        <center>
                                                        <ext:Image runat="server" ID="imgWitness3" Width="250" Height="70"  ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpWitness3" Width="250" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadWitness3"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblWitness3"></ext:Label>
                                                        <ext:TextField runat="server" ID="txtWitness3" Width="250" AllowBlank="false"></ext:TextField>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;2&nbsp;.</td>
                                                    <td style="width: 250px; border-bottom: 1px solid Black;">
                                                        <center>
                                                        <ext:Image runat="server" ID="imgWitness4" Width="250" Height="70"  ImageUrl="../../../Resources/images/signhere.png"></ext:Image>
                                                        <ext:FileUploadField runat="server" ID="flUpWitness4" Width="250" AllowBlank="false">
                                                            <DirectEvents>
                                                                <FileSelected OnEvent="onUploadWitness4"></FileSelected>
                                                            </DirectEvents>
                                                        </ext:FileUploadField>
                                                        <ext:Label runat="server" ID="lblWitness4"></ext:Label>
                                                        <ext:TextField runat="server" ID="txtWitness4" Width="250" AllowBlank="false"></ext:TextField>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <table style="width: 702px; margin: 0 auto;">
                                    <tr id="Tr1">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        REPUBLIC OF THE PHILIPPINES )
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        IN THE CITY OF PAGADIAN&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)S.S.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        x------------------------------------------x
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr2">
                                        <td>
                                            <center><b>A C K N O W L E D G M E N T</b></center>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr id="Tr3">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="text-align: justify;">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>BEFORE ME</b>, this _____ day of __________ 2011 at ________________, Philippines, personally appeared
                                                        ____________________________ with CTC No. __________, issued on _____________, at ________________ and _________________________ with CTC No. 
                                                        __________, issued on ___________, at ___________, known to me to be the same person/s who executed the forgoing instrument and he/she/they acknowledged to me that the same is his/her/their free act and voluntary deed.
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="Tr4">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="width: 400px;">
                                                        <br />
                                                        Doc. No.: ___________;<br />
                                                        Page No.: ___________;<br />
                                                        Book No.: ___________;<br />
                                                        Series of 20___.
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<ext:Editor ID="TextBlockEditor" runat="server" AutoSize="Width">
                    <Field>
                        <ext:HtmlEditor runat="server" Height="300" />
                    </Field>
                </ext:Editor>--%>
            </Content>
            <%--<Listeners>
                <Activate Handler="TextBlockEditor.retarget(${#MainPanel [@field=tar].divClass}); Button1.hide(); btnCompleteEdit.show();" Single="true" />
            </Listeners>--%>
        </ext:Panel>
    </form>
    </center>
</body>
</html>
