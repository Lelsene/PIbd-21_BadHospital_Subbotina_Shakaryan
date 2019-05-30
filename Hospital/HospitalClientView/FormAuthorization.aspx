<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormAuthorization.aspx.cs" Inherits="HospitalClientView.AuthorizationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
        <div style="height: 241px">

            <br />
            ФИО&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="textBoxFIO" runat="server" Height="16px" Width="280px">Nastya</asp:TextBox>
            <br />
            <br />
            Почта&nbsp;&nbsp;&nbsp; <asp:TextBox ID="textBoxEmail" runat="server" Height="16px" Width="280px">Nastya.Shakaryan@yandex.ru</asp:TextBox>
            <br />
            <br />
            Пароль&nbsp;
        <asp:TextBox ID="textBoxPassword" runat="server" Height="16px" Width="280px">123</asp:TextBox>
            <br />
            <br />
            <asp:Button ID="RegistrationButton" runat="server" OnClick="RegistrationButton_Click" Text="Зарегестрироваться" />
            <asp:Button ID="SignInButton" runat="server" OnClick="SignInButton_Click" Text="Войти" />

        </div>
    </form>
</body>
</html>
