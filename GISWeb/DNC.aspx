<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DNC.aspx.cs" Inherits="GISWeb.DNC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Do Not Contact</h2>
    <br />
    <div class="container">
        <asp:UpdatePanel ID="uplDNCArchive" runat="server">
            <ContentTemplate>
        <asp:Label ID="lblDNCArchivedMessage" runat="server" />
        <br />
            <asp:GridView runat="server" ID="gvResults"></asp:GridView>
            
            <asp:Label ID="lblDNC" runat="server" />
            <asp:GridView ID="gvDNC" runat="server" CellPadding="4"
                HorizontalAlign="Left" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False" OnRowCommand="gvDNC_RowCommand"
                EmptyDataText="No Results Found" DataKeyNames="OutcomeId" CssClass="table table-striped">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" />
                <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                <Columns>
                    <asp:HyperLinkField 
                        DataNavigateUrlFields="PremiseID" 
                        DataNavigateUrlFormatString="~/premiseHistory.aspx?PremiseId={0}"
                        DataTextField="PremiseID" 
                        HeaderText="PremiseId" />
                    <asp:BoundField DataField="MPRN" HeaderText="MPRN" />
                    <asp:BoundField DataField="MeterPointAddress" HeaderText="Meter Point Address" />
                    <asp:BoundField DataField="DUoSGroup" HeaderText="DUoS Group" />                    
                    <asp:BoundField DataField="ActionDateTime" HeaderText="StartDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ActionStageCancelReason" HeaderText="DoorKnockedCancelReason" />
                    <asp:BoundField DataField="ActionStageEnd" HeaderText="ActionStageEnd" />
                    <asp:BoundField DataField="RepName" HeaderText="RepName" />
                    <asp:BoundField DataField="Company" HeaderText="Company" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnArchive" ClientIDMode="Static" runat="server" Text="Archive" CommandName="Archive" CommandArgument='<%# Eval("OutcomeId") %>' class="btn btn-info btn-sm"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </ContentTemplate>
            <Triggers>
                
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
