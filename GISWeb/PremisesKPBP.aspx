<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PremisesKPBP.aspx.cs" Inherits="GISWeb.PremisesKPBP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <style>
        .centerHeaderText th{
            text-align:center;
        }

        .centerHeaderText td{
            text-align:center;
        }

        #premisesKPBPResults{
            text-align:center;
        }

        #searchRow{
            display:flex;
            justify-content:center;
            width:100%;
        }
        #reqRow{
            display:flex;
            justify-content:center;
            width:100%;
        }
        </style>

        <h2>Premise Keypad / Billpay</h2>
        <br />
        <asp:UpdatePanel ID="uplSearchResults" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                        <div class="row col-sm-6" id="searchRow">
                            <div class="col-sm-3">
                                <asp:Label ID="lblPostCode" runat="server" Text="Enter PostCode" />
                                <asp:TextBox ID="txtPostCode" runat="server" Text="" class="form-control"/>               
                            </div>
                            <div class="col-sm-3" style="padding-top:20px">
                                <asp:Button ID="btnPostCode" ValidationGroup="valGroup1" runat="server" Text="Search" class="form-control btn btn-primary" OnCommand="btnPostCode_Click" />
                            </div>
                        </div>
                        <div class="row" id="reqRow">
                            <div class="col-sm-6">   
                                <asp:RequiredFieldValidator ValidationGroup="valGroup1" runat="server" ID="reqPostCode" ControlToValidate="txtPostCode" ErrorMessage="*" Display="Static" ForeColor="Red" Font-Size="Medium" />
                                <asp:RegularExpressionValidator ValidationGroup="valGroup1" ID="revPostCode" runat="server" ControlToValidate="txtPostCode" 
                                    ValidationExpression="^([bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{2}|[bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{1}|[bB][tT][0-9]?[0-9]?\s[0-9]|[bB][tT][0-9]?[0-9]?\s|[bB][tT][0-9][0-9]?)$" 
                                    ErrorMessage="PostCode format must be one of the following: 'BT45 5RQ' , 'BT4 5RQ' ,
                                    'BT45 5R' , 'BT4 5R' ,
                                    'BT45 5' , 'BT4 5', BT45, BT4" Display="Static" ForeColor="Red" />
                            </div>
            
                        </div>

                <br />

            </ContentTemplate>
        </asp:UpdatePanel>        
        <div class="row">
        <div class="col-sm-12" id="premisesKPBPResults">           
            <asp:UpdatePanel ID="uplPremisesKPBPResults" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlPremisesKPBPResults" runat="server">
                        <asp:Label runat="server" ID="lblDate" />
                        <asp:GridView ID="gvPremisesKPBPResults" runat="server" CellPadding="4"
                            HorizontalAlign="Center" ForeColor="#333333" GridLines="None" Width="35%"
                            AutoGenerateColumns="true" OnRowDataBound="gvPremisesKPBPResults_RowDataBound"
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
                                                
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPostCode" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>  
            <asp:UpdateProgress runat="server" ID="udpPremisesKPBPResults">
                <ProgressTemplate>
                    Counting Keypad and Billpay Premises ...
                </ProgressTemplate>
            </asp:UpdateProgress>
            </div>
        </div>
    </div>
</asp:Content>
