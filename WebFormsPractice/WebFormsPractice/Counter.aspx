<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Counter.aspx.cs" Inherits="WebFormsPractice.Counter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="counterLabel" runat="server">0</asp:Label>
        </div>
        <p><asp:Button ID="increment" runat="server" OnClick="Increment_Counter" Text="Increment"/>
            <asp:Button ID="decrement" runat="server" OnClick="Decrement_Counter" Text="Decrement"/>
        </p>

    </form>
</body>
</html>
