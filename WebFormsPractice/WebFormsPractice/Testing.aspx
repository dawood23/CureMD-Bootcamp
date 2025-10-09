<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Testing.aspx.cs" Inherits="WebFormsPractice.Testing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

            <asp:Label ID="Namelbl" runat="server">Name:</asp:Label>
            <asp:TextBox ID="Nametxt" runat="server" placeholder="Enter Your Name.." type="text"></asp:TextBox>
            <br/>
            <asp:Label ID="Emaillbl" runat="server">Email:</asp:Label>
            <asp:TextBox ID="Emailtxt" runat="server" placeholder="Enter Your Email.." type="email"></asp:TextBox>
            
            <p><asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click"/>
                </p>

         <asp:Label ID="NameOutput" runat="server"></asp:Label><br/>
         <asp:Label ID="EmailOutput" runat="server"></asp:Label>
    </form>
</body>
</html>
