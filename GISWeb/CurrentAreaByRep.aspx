<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CurrentAreaByRep.aspx.cs" Inherits="GISWeb.CurrentAreaByRep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .upModal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 225px;
            height: 225px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
    </style>
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
            <asp:GridView ID="gvAllocatedAreas" runat="server" CellPadding="4" OnRowDataBound="gvAllocatedAreas_RowDataBound"
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
                    <asp:BoundField DataField="ActionStageCancelReason" HeaderText="ActionStageCancelReason" />
                    <asp:BoundField DataField="SaleKeypad" HeaderText="SaleKeypad" />
                    <asp:BoundField DataField="SaleBillPay" HeaderText="SaleBillPay" />
                    <asp:BoundField DataField="DoNotContact" HeaderText="DoNotContact" />
                    <asp:BoundField DataField="MostRecentOutcome" HeaderText="Most Recent Outcome" /> 
                </Columns>
            </asp:GridView>

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

     <asp:UpdateProgress ID="upCurrentArea" runat="server" AssociatedUpdatePanelID="uplCurrentArea">
        <ProgressTemplate>
            <div class="upModal">
                <div class="center">
                    <img alt="" src="./Content/Site/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
