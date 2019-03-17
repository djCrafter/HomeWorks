<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Div.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
       <style>
           #lab1{ 
              color:red;
           }
           #lab2{
               color:red;
           }
       </style>
    </title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
              <asp:TextBox runat="server" ID="textBox1" Width="200px"/>
                <asp:Label Text="" runat="server" ID="label1" ForeColor="Red"/>                
            </div>
            <br /><br />
            <div>
                <asp:TextBox runat="server" ID="textBox2" Width="200px"/>
                <asp:Label Text="" runat="server" ID="label2" ForeColor="Red"/>              
            </div>
             <br /><br />
            <div>
                <asp:Button Text="Разделить" runat="server" ID="button"/>
                <asp:Label Text="" runat="server" ID="label3"/>
            </div>
             <br /><br />
            <div>
                <ul>
                    <li id="lab1"></li>
                    <li id="lab2"></li>
                </ul>
            </div>             
        </div>
    </form>
</body>
</html>
