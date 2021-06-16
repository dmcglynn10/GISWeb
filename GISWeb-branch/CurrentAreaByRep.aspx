<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CurrentAreaByRep.aspx.cs" Inherits="GISWeb.CurrentAreaByRep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Current Area by rep</h2>

<%--    <link href="Content/Site.css" rel="stylesheet" />--%>

    <asp:UpdatePanel ID="uplCurrentArea" runat="server">
        <ContentTemplate>

            <div class="container">
                <br />
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="lblSalesRep" runat="server" Text="Sales Rep"/>
                        <asp:DropDownList ID="ddlSalesReps" runat="server" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" onchange="if(this.selectedIndex == 0)return false;">
                            <asp:ListItem  Text="Please select an item" Value="0"/>
                        </asp:DropDownList>
                            <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqSalesRep" ControlToValidate="ddlSalesReps" InitialValue="0" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
                    </div>
                </div>
            </div>
            <%--            <asp:Button ID="btnUpdate" Text="Update" runat="server" OnClick="btnUpdate_Click" />--%>
            
            <asp:Label ID="lblAllocatedPremises" runat="server" />
            <asp:GridView runat="server" ID="gvResults"></asp:GridView>
            <asp:GridView ID="gvAllocatedAreas" runat="server" CellPadding="4"
                HorizontalAlign="Left" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False"
                EmptyDataText="No Results Found" DataKeyNames="PremiseID" CssClass="table table-striped">
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
                        HeaderText="PremiseId" 
/>
                    <asp:BoundField DataField="MPRN" HeaderText="MPRN" />
                    <asp:BoundField DataField="MeterPointAddress" HeaderText="Meter Point Address" />
                    <asp:BoundField DataField="DUoSGroup" HeaderText="DUoS Group" />
                    <asp:BoundField DataField="MeterPointStatus" HeaderText="MeterPointStatus" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" DataFormatString="{0:dd/MM/yyyy}" />

                </Columns>
            </asp:GridView>

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
