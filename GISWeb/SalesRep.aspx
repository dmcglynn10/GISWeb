<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesRep.aspx.cs" Inherits="GISWeb.SalesRepPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List Sales Reps</h2>
    <style>
    


</style>

    <div class="container">
        <asp:UpdatePanel ID="uplGridView" runat="server">
            <ContentTemplate >           
                <asp:Panel runat="server" ID="pnlGridView">
                    <br />
                    <div class="row">
                        <div class="btnCreateTop col-sm-3">
                            <asp:Button ID="btnCreateTop" runat="server" Text="Add SalesRep" OnClick="CreateNewSalesRepBtn_Click" class="btn btn-info btn-lg"/>
                        </div>
                        <div class="btnArchivedRepsTop col-sm-3">
                            <asp:Button ID="btnArchivedRepsTop" runat="server" Text="Show Archived Reps" class="btn btn-info btn-lg" OnClick="btnArchivedReps_Click"/>
                        </div>
                    </div>
                    <br />
                    <asp:GridView ID="gvListOfSalesReps" runat="server" CellPadding="4"
                        HorizontalAlign="Center" ForeColor="#333333" GridLines="None" CurrentSortField="SalesRepId" CurrentSortDirection="ASC"
                        AutoGenerateColumns="false" OnRowEditing="gvListOfSalesReps_RowEditing" OnRowDataBound="gvListOfSalesReps_RowDataBound"
                        AllowSorting="true" OnSorting="gvListOfSalesReps_Sorting" EmptyDataText="No Results Found" DataKeyNames="SalesRepId" CssClass="table table-striped">
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
                            <asp:BoundField DataField="SalesRepId" SortExpression="SalesRepId" HeaderText="SalesRepId" />
                            <asp:BoundField DataField="RepName" SortExpression="RepName" HeaderText="Rep Name" />
                            <asp:BoundField DataField="Company" SortExpression="Company" HeaderText="Company" />
                            <asp:BoundField DataField="EmailAddress" SortExpression="EmailAddress" HeaderText="EmailAddress" />
                            <asp:BoundField DataField="Archived" SortExpression="Archived" HeaderText="Archived" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="Edit" CommandName="edit" CommandArgument='<%# Eval("SalesRepId") %>' class="btn btn-info btn-sm"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <div class="row">
                        <div class="btnCreateBottom col-sm-3">
                            <asp:Button ID="btnCreateBottom" runat="server" Text="Add SalesRep" OnClick="CreateNewSalesRepBtn_Click" class="btn btn-info btn-lg"/>
                        </div>
                        <div class="btnArchivedRepsBottom col-sm-3">
                            <asp:Button ID="btnArchivedRepsBottom" runat="server" Text="Show Archived Reps" class="btn btn-info btn-lg" OnClick="btnArchivedReps_Click"/>
                        </div>
                    </div>
                    <br />
                </asp:Panel>           

        <asp:Panel runat="server" ID="pnlEditDetails">
        <br />
        <div>
        <!--<asp:Button ID="btnSaveChangesTop" runat="server" Text="Save changes" OnClick="SaveChangesBtn_Click" class="btn btn-info btn-lg"/>-->
        </div>
        <br />
        <div>           
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
                <h3>Edit SalesRep Details:</h3><br />
            <div class="row bg-warning">
                <div class="col-sm-12">
                    <div class="col-sm-4">SalesRep ID:</div>
                    <div class="col-sm-8">
                        <asp:Label ID="lblSalesRepId" runat="server" class="form-control-static"/>
                    </div>
                </div>
            </div>
            <div class="row bg-warning">
                <div class="col-sm-12">
                    <div class="col-sm-4">Rep Name:</div>
                    <div class="col-sm-8">

                            <asp:TextBox ID="txtRepName" runat="server" class="form-control"/>

                    </div>
                </div>
            </div>
            <div class="row bg-warning">
                <div class="col-sm-12">
                    <div class="col-sm-4">Company:</div>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlCompany" runat="server" class="form-control">
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
                        <asp:TextBox ID="txtEmailAddress" runat="server" class="form-control"/>
                    </div>
                </div>
            </div>
            <div class="row bg-warning">
                <div class="col-sm-12">
                    <div class="col-sm-4">Archived:</div>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlArchived" runat="server" class="form-control">
                            <asp:ListItem Text="False" Value="0" />
                            <asp:ListItem Text="True" Value="1" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-6">
                    <asp:Button ID="btnSaveChangesBottom" runat="server" Text="Save changes" OnClick="SaveChangesBtn_Click" class="btn btn-info btn-lg"/>
                </div>
                <div class="col-sm-6">
                    <div style="float:right">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-info btn-lg" />
                    </div>
                </div>
            </div>
            <br />
            </div>
            <div class="col-sm-3">
            </div>
        </div>
        
        </asp:Panel>

        <asp:Panel runat="server" ID="pnlCreateNewSalesRep">
        <br />
        <div>
        <!--<asp:Button ID="btnCreateNewSalesRepTop" runat="server" Text="Create New Sales Rep" OnClick="AddNewSalesRepBtn_Click" class="btn btn-info btn-lg"/>-->
        </div>
        <br />
        <div>
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
                <h3>Create New SalesRep:</h3><br />
                <div class="row bg-warning">
                <!--<div class="col-sm-12">
                    <div class="col-sm-4">SalesRep ID:</div>
                    <div class="col-sm-8">
                        <asp:Label ID="lblNewSalesRepID" runat="server" class="form-control-static"/>
                    </div>
                </div>-->
                <div class="col-sm-12">
                    <div class="col-sm-4">Rep Name:</div>
                    <div class="col-sm-8">
                            <asp:TextBox ID="txtNewRepName" runat="server" class="form-control"/>
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
                        <asp:TextBox ID="txtNewEmailAddress" runat="server" class="form-control"/>
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
                    <asp:Button ID="btnCreateNewSalesRepBottom" runat="server" Text="Create New Sales Rep" OnClick="AddNewSalesRepBtn_Click" class="btn btn-info btn-lg"/>
                </div>
                <div class="col-sm-6">
                    <div style="float:right">
                        <asp:Button ID="btnNewCancel" runat="server" Text="Cancel" OnClick="btnNewCancel_Click" class="btn btn-info btn-lg"/>
                    </div>
                </div>
            </div>
            <br />
        </div>
        <div class="col-sm-3">
        </div>
        </div>      
        </asp:Panel>
         </ContentTemplate>
            <Triggers >
                <asp:AsyncPostBackTrigger ControlID="btnCreateNewSalesRepTop" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnCreateNewSalesRepBottom" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnArchivedRepsTop" EventName="Click"/>
                <asp:AsyncPostBackTrigger ControlID="btnArchivedRepsBottom" EventName="Click"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

