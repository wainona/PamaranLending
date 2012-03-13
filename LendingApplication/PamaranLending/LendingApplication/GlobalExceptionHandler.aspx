<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalExceptionHandler.aspx.cs" Inherits="LendingApplication.GlobalExceptionHandler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    
        .style
        {
            font-size: 15px;
            font-family: "Courier New", Courier, monospace;
            color: black;
            font-style: normal;
            background-color: Menu;
        }
        
        .style2
        {
            font-size: 16px;
            font-family: Arial;
            color: Red;
            text-align: justify;
        }
        .style3
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: large;
            text-align: justify;
        }
        .style5
        {
            color: #CC0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="style2">
        <span class="style3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
&nbsp;&nbsp;&nbsp; <span class="style5">An error has occured in the application. 
        This event has been logged and will be addressed by the site administrator. 
        In the meantime you can attempt to link to a different section of the site or try again later. <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
&nbsp;&nbsp;&nbsp; We appreciate your patience.</span>
        </span>
        <br/><br/>
    </div>
    <div class="style">
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblErrorMessage" 
            style="text-align: justify" />
        <br />
        <br />
    </div>
    </form>
</body>
</html>
