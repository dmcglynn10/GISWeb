<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutComes.aspx.cs" Inherits="GISWeb.OutComes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <h2>Outcomes by rep</h2>


    <style>
        .centerHeaderText th{
            text-align:center;
        }

        .centerHeaderText td{
            padding-left:5px;
            padding-right:5px;
        }

        .centerHeaderText span{
            text-decoration:underline;
            text-decoration-color:white;
            font-size:larger;
            
        }
    </style>

    <div class="container">
        <asp:UpdatePanel runat="server" id="uplOutcomes" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
        <br />
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="lblSalesRep" runat="server" Text="Sales Rep"/>
                <asp:DropDownList runat="server" ID="ddlSalesReps" Cssclass="ddl form-control" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSalesReps_SelectedIndexChanged" onchange="if(this.selectedIndex == 0)return false;">
                    <asp:ListItem  Text="Please select an item" Value="0" />
                </asp:DropDownList>
                    <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqSalesRep" ControlToValidate="ddlSalesReps" InitialValue="0" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
            </div>   
            <div class="col-sm-3" style="padding-top:20px">

            </div>
        </div>
        <div class="row">
            <panel id="pnlListOfOutcomes" runat="server" Text="">
                <asp:Label runat="server" ID="lblSearchResults" />
                <asp:GridView ID="gvListOfOutcomes" runat="server" CellPadding="4"
                    HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
                    OnPageIndexChanging="gvListOfOutcomes_PageIndexChanging" OnSorting="gvListOfOutcomes_Sorting" OnRowDataBound="gvListOfOutcomes_RowDataBound"  
                    EmptyDataText="No Results Found" CssClass="table table-striped" AllowPaging="True" PageSize="20" AllowSorting="True">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText"/>
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" CssClass="centerHeaderText" />
                    <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    <Columns>
                        <%--                       
                        <asp:BoundField DataField="ContactId" HeaderText="ContactId" />
                        <asp:BoundField DataField="Action" HeaderText="Action" />
                        <asp:BoundField DataField="ActionDateTime" HeaderText="ActionDateTime" />
                        <asp:BoundField DataField="ActionBy" HeaderText="ActionBy" />
                        <asp:BoundField DataField="RescheduleDate" HeaderText="RescheduleDate" />
                        <asp:BoundField DataField="DoNotContactAgainDate" HeaderText="DoNotContactAgainDate" />
                        <asp:BoundField DataField="PremiseID" HeaderText="PremiseID" />
                        <asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/>--%>                
                    </Columns>
                </asp:GridView>
             </panel>           
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlSalesReps" EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
