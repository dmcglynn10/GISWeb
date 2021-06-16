<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Premises.aspx.cs" Inherits="GISWeb.Premises" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>    

    <script src="Scripts/moment.min.js"></script>

    <script type="text/javascript">
        
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            $("#startDatePicker").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                clearBtn: true,
                orientation: 'bottom'
            });
        }

        $("#startDatePicker").datepicker({
            format: 'yyyy-mm-dd',
            todayHighlight: true,
            clearBtn: true,
            orientation: 'bottom'
        });
    });

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            $("#endDatePicker").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                clearBtn: true,
                orientation: 'bottom'
            });
        }

        $("#endDatePicker").datepicker({
            format: 'yyyy-mm-dd',
            todayHighlight: true,
            clearBtn: true,
            orientation: 'bottom'
        });
    });
    </script>

    <style>
        .centerHeaderText th{
            text-align:center;
        }

        .centerHeaderText td{
            padding-left:5px;
            padding-right:5px;
        }

        .centerHeaderText span {
            text-decoration: underline;
            text-decoration-color: white;
            font-size: larger;
        }

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
        }
    </style>

    <h2>Allocate Rep to area</h2>
    <div class="container">
        <br />
        <asp:UpdatePanel ID="uplSearchResults" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="lblPostCode" runat="server" Text="Enter PostCode" />
                <asp:TextBox ID="txtPostCode" runat="server" Text="" class="form-control"/>               
            </div>
            <div class="col-sm-3" style="padding-top:20px">
                <asp:Button ID="btnPostCode" ValidationGroup="valGroup1" runat="server" Text="Search" class="form-control btn btn-primary" OnCommand="btnPostCode_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">   
                <asp:RequiredFieldValidator ValidationGroup="valGroup1" runat="server" ID="reqPostCode" ControlToValidate="txtPostCode" ErrorMessage="*" Display="Static" ForeColor="Red" Font-Size="Medium" />
                <asp:RegularExpressionValidator ValidationGroup="valGroup1" ID="revPostCode" runat="server" ControlToValidate="txtPostCode" 
                    ValidationExpression="^([bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{2}|[bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{1}|[bB][tT][0-9]?[0-9]?\s[0-9])$" 
                    ErrorMessage="PostCode format must be one of the following: 'BT45 5RQ' , 'BT4 5RQ' ,
                    'BT45 5R' , 'BT4 5R' ,
                    'BT45 5' , 'BT4 5'" Display="Static" ForeColor="Red" />
            </div>
            
        </div>
        <br />     

        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="searchResults" runat="server"></asp:Label>
            </div>
            
        </div>

        
                <asp:Panel runat="server" ID="pnlListOfPremises">
                    <asp:GridView ID="gvListOfPremises" runat="server" CellPadding="4"
                        HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
                        AutoGenerateColumns="false" OnPageIndexChanging="gvListOfPremises_PageIndexChanging" OnRowDataBound="gvListOfPremises_RowDataBound"
                        EmptyDataText="No Results Found" DataKeyNames="PremiseId" CssClass="table table-striped" AllowPaging="True" PageSize="20" AllowCustomPaging="False">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText"/>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" CssClass="centerHeaderText"  Font-Underline="False" BorderStyle="NotSet" />
                        <FooterStyle />
                        <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        <Columns>
                            <asp:BoundField DataField="PremiseId" HeaderText="PremiseId" />
                            <asp:BoundField DataField="MPRN" HeaderText="MPRN" />
                            <asp:BoundField DataField="MeterPointAddress" HeaderText="Meter Point Address" />
                            <asp:BoundField DataField="DUoSGroup" HeaderText="DUoS Group" />
                            <asp:BoundField DataField="MeterConfigurationCode" HeaderText="MeterConfigurationCode" />
                            <asp:BoundField DataField="MeterPointStatus" HeaderText="MeterPointStatus" />
                            <asp:BoundField DataField="PostalCodeID" HeaderText="PostalCodeID" />
                            <asp:BoundField DataField="Live" HeaderText="Live" />
                            <asp:BoundField DataField="Pending" HeaderText="Pending" />
                            <%--<asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/> --%>                
                        </Columns> 
                    </asp:GridView>

                </asp:Panel>



                <br />
                
                <asp:Panel runat="server" ID="pnlAllocationSummary">
                    <div class="row">
                    <div class="col-sm-12">
                        <asp:Label ID="lblAllocationSummary" runat="server"></asp:Label>
                    </div>
                   
                    </div>
                    <asp:GridView ID="gvAllocationSummary" runat="server" CellPadding="4"
                        HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
                        OnRowDataBound="gvAllocationSummary_RowDataBound" OnPageIndexChanging="gvAllocationSummary_PageIndexChanging"
                        EmptyDataText="No Results Found" DataKeyNames="PostalCodeID" CssClass="table table-striped" AllowPaging="True" PageSize="20" AllowCustomPaging="False">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText"/>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" CssClass="centerHeaderText"  Font-Underline="False" BorderStyle="NotSet" />
                        <FooterStyle />
                        <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        <Columns>

                            <%--<asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/> --%>                
                        </Columns> 
                    </asp:GridView>

                </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlPostcodesAllocatedToOtherGroup">
            <div class="row">
                <div class="col-sm-8">
                    <asp:Label ID="lblPostcodesAllocatedToOtherGroup" runat="server" class="lblPostcodesAllocatedToOtherGroup">
                    </asp:Label>
                </div>               
            </div>
        </asp:Panel>
       
        <asp:Panel runat="server" ID="pnlPostCodeAlreadyAllocated">
            <div class="row">
                <div class="col-sm-8">
                    <asp:Label ID="lblPostCodeAlreadyAllocated" runat="server" class="lblPostCodeAlreadyAllocated">
                    </asp:Label>
                </div>

                <div class="col-sm-2">
                     <div class="col-sm-6">
                        
                    </div>
                    <div class="col-sm-6">
                        <asp:Button ID="btnPostCodeAlreadyAllocatedYes" runat="server" Text="Yes" OnClick="btnPostCodeAlreadyAllocatedYes_Click" class="form-control btn-primary"/>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="col-sm-6">
                        
                    </div>
                    <div class="col-sm-6">
                        <asp:Button ID="btnPostCodeAlreadyAllocatedNo" runat="server" Text="No" OnClick="btnPostCodeAlreadyAllocatedNo_Click" class="form-control btn-primary"/>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlAllocateSalesRep">
            <div class="row">
                <div class="col-sm-12">
                    <asp:Label ID="lblPreviousDateAdded" runat="server" >
                    </asp:Label>                   
                </div>
            </div>
            <br />
            <asp:Label ID="lblAllocatedMessage" runat="server" />
            <br />
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="lblSalesRep" runat="server" Text="Sales Rep" />
                    <asp:DropDownList runat="server" ID="ddlSalesReps" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem  Text="Please select an item" Value="0"/>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date" />
                        <div class="input-group date" id="startDatePicker">
                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            
                            
                        </div>
                    </div>
                </div>               
                <div class="col-sm-3">
                    <div class="form-group">
                        <asp:Label ID="lblEndDate" runat="server" Text="End Date" />
                        <div class="input-group date" id="endDatePicker">
                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                            
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group" style="padding-top:20px">
                        <asp:Label ID="lblAllocateBtn" runat="server" Text="" />
                        <asp:Button ID="btnAllocateSalesRep" ValidationGroup="valGroup2" runat="server" Text="Allocate SalesRep" class="form-control btn-primary" OnClick="btnAllocateSalesRep_Click"/>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group" style="color:red">
                        <asp:RequiredFieldValidator ValidationGroup="valGroup2" runat="server" ID="reqSalesRep" ControlToValidate="ddlSalesReps" InitialValue="0" ErrorMessage="Please select an item!" Display="Static" /> 
                    </div>
                </div> 
                <div class="col-sm-3">
                    <div class="form-group" style="color:red">
                        <asp:RequiredFieldValidator ValidationGroup="valGroup2" runat="server" id="reqStartDate" controltovalidate="txtStartDate" errormessage="Please enter a date!" Display="Static"/>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group" style="color:red">
                        <asp:RequiredFieldValidator ValidationGroup="valGroup2" runat="server" id="reqEndDate" controltovalidate="txtEndDate" errormessage="Please enter a date!" Display="Static"/>                      
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">

                </div>
                <div class="col-sm-3">

                </div>
                <div class="col-sm-3" style="color:red">
                    <asp:CompareValidator ValidationGroup="valGroup2" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate" Display="Dynamic" id="cvEndDate"
                                    Text="End Date must be greater than Start Date!" Operator="GreaterThan" Type="Date" Runat="Server" />
                </div>
            </div>
        </asp:Panel>
        
                </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPostCode" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPostCodeAlreadyAllocatedYes" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPostCodeAlreadyAllocatedNo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAllocateSalesRep" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        
    </div>
    <asp:UpdateProgress ID="upCurrentArea" runat="server" AssociatedUpdatePanelID="uplSearchResults">
        <ProgressTemplate>
            <div class="upModal">
                <div class="center">
                    <img alt="" src="./Content/Site/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
