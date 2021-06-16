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
    </asp:LoginView><br /><br />
    <asp:LoginStatus ID="LoginStatus1" runat="server" /><br /><br />
    <asp:LoginName ID="LoginName1" runat="server" /><br /><br />
    <asp:LoginView ID="LoginView1" runat="server">
    </asp:LoginView><br />


    <asp:LoginView id="LoginView2" runat="server">
                    <AnonymousTemplate>
                        Please log in for personalized information.
                    </AnonymousTemplate>
                    <LoggedInTemplate>
                        Thanks for logging in 
                        <asp:LoginName id="LoginName1" runat="Server"></asp:LoginName>.
                    </LoggedInTemplate>
                    <RoleGroups>
                        <asp:RoleGroup Roles="Admin">
                            <ContentTemplate>
                                <asp:LoginName id="LoginName2" runat="Server"></asp:LoginName>, you
                                are logged in as an administrator.
                            </ContentTemplate>
                        </asp:RoleGroup>
                    </RoleGroups>
                </asp:LoginView></p>
</div>
</asp:Content>
