<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Todo.aspx.cs" Inherits="WebFormsPractice.Todo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TodoText" runat="server" placeholder="Write Todo.."></asp:TextBox><br/>
            <asp:Button ID="TodoBtn" runat="server" onclick="AddTodo" Text="AddTodo"/>
        </div>

        <div class="Todo-container" runat="server" id="TodoContainer">

        </div>
    </form>
</body>
</html>
