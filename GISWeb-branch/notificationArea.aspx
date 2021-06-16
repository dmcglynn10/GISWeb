<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="notificationArea.aspx.cs" Inherits="GISWeb.notificationArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Notification Area</h2>


    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList>

    <br />


    <asp:Panel runat="server" ID="pnlCreateNewSalesRep">
        <br />
        <br />
        <div>
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
                <h3>Create New Notification:</h3>
                <br />
                <div class="row bg-warning">

                    <div class="col-sm-12">
                        <div class="col-sm-4">Rep Name:</div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNewRepName" runat="server" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="row bg-warning">
                    <div class="col-sm-12">
                        <div class="col-sm-4">Company:</div>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlNewCompany" runat="server" class="form-control">
                                <asp:ListItem Text="FSR" Value="FSR" />
                                <asp:ListItem Text="Click Energy" Value="Click Energy" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row bg-warning">
                    <div class="col-sm-12">
                        <div class="col-sm-4">EmailAddress:</div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNewEmailAddress" runat="server" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="row bg-warning">
                    <div class="col-sm-12">
                        <div class="col-sm-4">Archived:</div>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlNewArchived" runat="server" class="form-control">
                                <asp:ListItem Text="False" Value="0" Selected="True" />
                                <asp:ListItem Text="True" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-6">
                        <asp:Button ID="btnCreateNewSalesRepBottom" runat="server" Text="Create New Sales Rep" class="btn btn-info btn-lg" />
                    </div>
                    <div class="col-sm-6">
                        <div style="float: right">
                            <asp:Button ID="btnNewCancel" runat="server" Text="Cancel" class="btn btn-info btn-lg" />
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <div class="col-sm-3">
            </div>
        </div>
    </asp:Panel>



    <br />

    <asp:GridView ID="gvNotificationArea" runat="server" CellPadding="4"
        HorizontalAlign="Center" ForeColor="#333333" GridLines="None" CurrentSortField="NotificationId" CurrentSortDirection="ASC"
        AutoGenerateColumns="false" AllowSorting="true" EmptyDataText="No Results Found" DataKeyNames="NotificationId" CssClass="table table-striped">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        <Columns>
            <asp:BoundField DataField="CompanyToDisplay" SortExpression="CompanyToDisplay" HeaderText="CompanyToDisplay" />
            <asp:BoundField DataField="Author" SortExpression="Author" HeaderText="Author" />
            <asp:BoundField DataField="Company" SortExpression="Company" HeaderText="Company" />
            <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" />
            <asp:BoundField DataField="EndDate" SortExpression="EndDate" HeaderText="EndDate" />
            <asp:BoundField DataField="Archived" SortExpression="Archived" HeaderText="Archived" />
            <%--<asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnSelect" runat="server" Text="Edit" CommandName="edit" CommandArgument='<%# Eval("SalesRepId") %>' class="btn btn-info btn-sm"/>
            </ItemTemplate>
        </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>












    <br />

    <asp:Panel ID="pnlViewDetails" runat="server">
        Notes:
        <asp:TextBox ID="txtNotes" runat="server" /><br />
        Author
        <asp:DropDownList ID="ddlAuthor" runat="server"></asp:DropDownList><br />
        Start Date
        <asp:TextBox ID="txtStartDate" runat="server" /><br />
        End Date
        <asp:TextBox ID="txtEndDate" runat="server" /><br />
        <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
    </asp:Panel>
</asp:Content>
