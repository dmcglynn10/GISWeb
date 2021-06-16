<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="assignPostCodes.aspx.cs" Inherits="GISWeb.assignPostCodes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <asp:DropDownList runat="server" ID="ddlCompany" AutoPostBack="true">
        <asp:ListItem Text="FSR" />
        <asp:ListItem Text="Click Energy" />
    </asp:DropDownList>

    <asp:Button ID="btnAssign" Text="Assign" runat="server" OnClick="btnAssign_Click" />
    <br /><br />
    <asp:Label ID="lblResults" runat="server" />

</asp:Content>
