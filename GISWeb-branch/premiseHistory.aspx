<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="premiseHistory.aspx.cs" Inherits="GISWeb.premiseHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Premise History</h2>
    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="lblPostCode" runat="server" Text="Enter Premise Id" />
                <asp:TextBox ID="txtPremiseId" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqPremiseId" ControlToValidate="txtPremiseId" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
            </div>
            <div class="col-sm-3" style="padding-top: 20px">
                <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="valGroup1" class="form-control btn btn-primary" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="searchResults" runat="server"></asp:Label>
            </div>
            <div class="col-sm-3">
            </div>
        </div>

        <asp:GridView ID="gvPremiseHistory" runat="server" CellPadding="4"
            HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False"
            EmptyDataText="No Results Found" CssClass="table table-striped" OnRowDataBound="gvPremiseHistory_RowDataBound">
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
                <asp:BoundField DataField="SalesRepId" HeaderText="SalesRepId" />

                <asp:BoundField DataField="ActionDateTime" HeaderText="Action Date" />
                <asp:BoundField DataField="ActionStageEnd" HeaderText="Final Step" />
                <asp:BoundField DataField="ActionStageCancelReason" HeaderText="Final Step Cancel Reason" />
                <asp:TemplateField HeaderText="Sales Rep">
                    <ItemTemplate>
                        <asp:Label ID="lblSalesRepName" Text='<%# Eval("SalesRepId") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
