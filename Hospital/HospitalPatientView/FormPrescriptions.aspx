<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPrescriptions.aspx.cs" Inherits="HospitalPatientView.FormPrescriptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="ButtonRef" runat="server" Text="Обновить" OnClick="ButtonRef_Click" />
            <asp:GridView ID="dataGridView" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Наименование" SortExpression="Title" />
                    <asp:BoundField DataField="Price" HeaderText="Цена" SortExpression="Price" />
                </Columns>
                <SelectedRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
            <br />
            <asp:Button ID="ButtonCancel" runat="server" Text="Вернуться" OnClick="ButtonCancel_Click" />
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetList" TypeName="HospitalImplementations.Implementations.PrescriptionServiceDB"></asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
