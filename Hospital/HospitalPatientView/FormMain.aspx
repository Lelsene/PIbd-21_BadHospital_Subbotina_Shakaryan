<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormMain.aspx.cs" Inherits="HospitalPatientView.FormMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        #form1 {
            height: 666px;
            width: 1067px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Menu ID="Menu" runat="server" BackColor="White" ForeColor="Black" Height="150px">
            <Items>
                <asp:MenuItem Text="Каталог рецептов" Value="Каталог рецептов" NavigateUrl="~/FormPrescriptions.aspx"></asp:MenuItem>
                <asp:MenuItem Text="Отчеты" Value="Отчеты" NavigateUrl="~/FormPatientTreatments.aspx"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Button ID="ButtonCreateTreatment" runat="server" Text="Выбрать лечение" OnClick="ButtonCreateTreatment_Click" />
        <asp:Button ID="ButtonReviewTreatment" runat="server" Text="Просмотреть лечение" OnClick="ButtonReviewTreatment_Click" />
        <asp:Button ID="ButtonTreatmentReservation" runat="server" Text="Зарезервировать" OnClick="ButtonTreatmentReservation_Click" />
        <asp:Button ID="ButtonRef" runat="server" Text="Обновить список" OnClick="ButtonRef_Click" />
        <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                <asp:CommandField ShowSelectButton="true" SelectText=">>" />
                <asp:BoundField DataField="Title" HeaderText="Наименование" SortExpression="Title" />
                <asp:BoundField DataField="Date" HeaderText="Дата" SortExpression="Date" />
                <asp:BoundField DataField="TotalCost" HeaderText="Стоимость" SortExpression="TotalCost" />
                <asp:BoundField DataField="isReserved" HeaderText="Бронь" SortExpression="isReserved" />
            </Columns>
            <SelectedRowStyle BackColor="#CCCCCC" />
        </asp:GridView>
    </form>
</body>
</html>
