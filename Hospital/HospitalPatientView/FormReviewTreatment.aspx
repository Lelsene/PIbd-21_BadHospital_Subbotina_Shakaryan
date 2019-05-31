<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormReviewTreatment.aspx.cs" Inherits="HospitalPatientView.FormReviewTreatment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Название
        <asp:TextBox ID="textBoxName" runat="server" Width="200px"></asp:TextBox>
            <br />
            <br />
            Цена&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="textBoxPrice" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            <br />
            <br />
            <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="PrescriptionTitle" HeaderText="Наименование" SortExpression="PrescriptionTitle" />
                    <asp:BoundField DataField="Count" HeaderText="Количество" SortExpression="Count" />
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <br />
            <asp:Button ID="ButtonCancel" runat="server" Text="Вернуться" OnClick="ButtonCancel_Click" />
        </div>
    </form>
</body>
</html>
