<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication6.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Styles.css" />
</head>
<body>
    <form id="rsvpform" runat="server">
        <div>
            <h1>Новый год у Татьяны!</h1>
            <p>Мы устроим классную вечеринку и вы приглашены!</p>
        </div>
        <asp:ValidationSummary ID="VS" runat="server" ShowModelStateErrors="true" />
        <div>
            <label>Ваше имя:</label><input type="text" id="name" runat="server"/>
        </div>
        <div>
            <label>Ваш E-Mail:</label><input type="text" id="email" runat="server"/>
        </div>
        <div>
            <label>Ваш телефон:</label><input type="text" id="phone" runat="server"/>
        </div>
        <div>
        <label>Вы придете?</label>
        <select id="willattend" runat="server">
            <option value="">Выберите один из вариантов</option>
            <option value="true">Да</option>
            <option value="false">Нет</option>
        </select>
        </div>
        <div>
            <button type="submit">Отправить приглашение RSVP</button>
        </div>
    </form>
</body>
</html>
