﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewPage.aspx.cs" Inherits="SessionManagement.NewPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Button ID="btnLogIn" runat="server" Text="Log In" OnClick="btnLogIn_Click" />
            <asp:Label ID="lblUser" runat="server" Font-Size="Large" />
        </div>
    </form>
</body>
</html>
