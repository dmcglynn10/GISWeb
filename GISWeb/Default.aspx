<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GISWeb._default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
    <h1>Sales Rep Management</h1>
    <p class="lead">Add/edit reps.  Allocate to areas.&nbsp; Review KPIs.</p>
    <a href="SalesRep.aspx" class="btn btn-primary btn-lg">Add/Edit Reps</a>
    <a href="CurrentAreaByRep.aspx" class="btn btn-primary btn-lg">Current Rep Areas</a>
        <a href="Premises.aspx" class="btn btn-primary btn-lg">Allocate Areas</a>
        <a href="KPI.aspx" class="btn btn-primary btn-lg">KPI</a>
</div>
<div>
    <asp:LoginView ID="lgvLandingPage" runat="server">
     <RoleGroups>
          <asp:RoleGroup Roles="DOMAIN\it">
               <ContentTemplate>
                    Group DOMAIN\it
               </ContentTemplate>
          </asp:RoleGroup>
          <asp:RoleGroup Roles="DOMAIN\RepsClickGroup">
               <ContentTemplate>
                    Group DOMAIN\RepsClickGroup
               </ContentTemplate>
          </asp:RoleGroup>
         <asp:RoleGroup Roles="DOMAIN\Reps">
               <ContentTemplate>
                    Group DOMAIN\Reps
               </ContentTemplate>
          </asp:RoleGroup>
     </RoleGroups>
    </asp:LoginView>
</div>
</asp:Content>
