<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadForm.aspx.vb" Inherits="UploadForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Nomina Upload</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label2" runat="server" Font-Size="X-Large" Text="Nomina Registry"></asp:Label><br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="default.aspx">Home</asp:HyperLink><br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Provider Registry Schema URL: "></asp:Label>
        <asp:TextBox ID="regitryFileUrl" runat="server" Width="325px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Upload" />
        <br />
        <br />
        <asp:Label ID="resultLabel" runat="server"></asp:Label></div>
    </form>
</body>
</html>
