<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesRepActions.aspx.cs" Inherits="GISWeb.SalesRepActions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
          
    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>    

    <script src="Scripts/moment.min.js"></script>
    
 
    <script>
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $("#datepickerWeekStartDate").datepicker({
                    format: 'yyyy-mm-dd',
                    todayHighlight: true,
                    clearBtn: true,
                    orientation: 'bottom'
                });

                $('#datepickerWeekStartDate').on('changeDate', function (e) {
                    value = document.getElementById('<%= txtdatepickerWeekStartDate.ClientID %>').value;

                    firstDate = moment(value, "yyyy-MM-D").format("yyyy-MM-D");
                    lastDate = moment(value, "yyyy-MM-D").add(7, 'days').format("yyyy-MM-D");
                    document.getElementById('<%= txtdatepickerWeekStartDate.ClientID %>').value = firstDate;
                    document.getElementById('<%= txtdatepickerWeekEndDate.ClientID %>').value = lastDate;
                });
            }

            $("#datepickerWeekStartDate").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                clearBtn: true,
                orientation: 'bottom'
            });

            $('#datepickerWeekStartDate').on('changeDate', function (e) {
                value = document.getElementById('<%= txtdatepickerWeekStartDate.ClientID %>').value;

                firstDate = moment(value, "yyyy-MM-D").format("yyyy-MM-D");
                lastDate = moment(value, "yyyy-MM-D").add(7,'days').format("yyyy-MM-D");
                document.getElementById('<%= txtdatepickerWeekStartDate.ClientID %>').value = firstDate;
                document.getElementById('<%= txtdatepickerWeekEndDate.ClientID %>').value = lastDate;
            });
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $("#datepickerDayStartDate").datepicker({
                    format: 'yyyy-mm-dd',
                    todayHighlight: true,
                    clearBtn: true,
                    orientation: 'bottom'
                });
            }

            $("#datepickerDayStartDate").datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
                clearBtn: true,
                orientation: 'bottom'
            });
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $("#datepickerMonthDate").datepicker({
                    format: 'yyyy-mm',
                    minViewMode: 'months',
                    clearBtn: true,
                    todayHighlight: true,
                    showButtonPanel: true,
                    orientation: 'bottom'
                });
            }

            $("#datepickerMonthDate").datepicker({
                format: 'yyyy-mm',
                minViewMode: 'months',
                todayHighlight: true,
                clearBtn: true,
                showButtonPanel:true,
                orientation: 'bottom'
            });
        });
    </script>

    <style>
        #datepickerMonthStartDate-calendar {
            /*display:none;*/
        }

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

        .dll.form-control{
            float:left;
        }
        .req{
            float:right;
        }
    </style>

    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-3">

                <asp:Label ID="lblSalesRep" runat="server" Text="Sales Rep"/>
                <asp:DropDownList runat="server" ID="ddlSalesReps" Cssclass="ddl form-control" AppendDataBoundItems="true">
                    <asp:ListItem  Text="Please select an item" Value="0"/>
                </asp:DropDownList>


                    <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqSalesRep" ControlToValidate="ddlSalesReps" InitialValue="0" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />

            </div>           
        
            

                <div class="form-group">
                    <asp:UpdatePanel ID="uplTimeSpan" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                        <ContentTemplate>
                                <div class="col-sm-3">
                                <asp:Label ID="lblTimeSpan" runat="server" Text="Time" />
                                <asp:DropDownList ID="ddlTimeSpan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTimeSpan_SelectedIndexChanged" CssClass="form-control">
                                    <asp:ListItem Text="Day" Value="Day" />
                                    <asp:ListItem Text="Week" Value="Week" />
                                    <asp:ListItem Text="Month" Value="Month" />
                                </asp:DropDownList>
                                </div>
                            <div class="col-sm-3">
                            <asp:Panel ID="pnlWeek" runat="server">


                                    <div class="form-group">
                                        <asp:Label ID="lbldatepickerWeekStartDate" runat="server" Text="Start Date" />
                                        <div class="input-group date" id="datepickerWeekStartDate">
                                        <asp:TextBox id="txtdatepickerWeekStartDate" runat="server" class="form-control"/>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>                                       
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqDatePickerWeekStartDate" ControlToValidate="txtdatepickerWeekStartDate" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
                                    </div>
                                    

 

                                    <div class="form-group">
                                        <asp:Label ID="lbldatepickerWeekEndDate" runat="server" Text="End Date" />
                                        <div class="input-group date" id="datepickerWeekEndDate">
                                        <asp:TextBox id="txtdatepickerWeekEndDate" runat="server" class="form-control"/>
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                        </div>
                                    </div>


                            </asp:Panel>
                            <asp:Panel ID="pnlDay" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="lblDayStartDate" runat="server" Text="Start Date" />
                                    <div class="input-group date" id="datepickerDayStartDate">
                                        <asp:TextBox id="txtDayStartDate" runat="server" class="form-control" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>                                     
                                    </div> 
                                    <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqDayStartDate" ControlToValidate="txtDayStartDate" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
                                </div>                               
                            </asp:Panel>
                            <asp:Panel ID="pnlMonth" runat="server">
                                <div class="form-group">
                                    <asp:Label ID="lblMonthDate" runat="server" Text="Start Date" />
                                    <div class="input-group date" id="datepickerMonthDate">
                                    <asp:TextBox id="txtdatepickerMonthDate" runat="server" class="form-control" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>                                  
                                    </div>
                                    <asp:RequiredFieldValidator ValidationGroup="valGroup1"  ForeColor="Red" runat="server" ID="reqdatepickerMonthDate" ControlToValidate="txtdatepickerMonthDate" ErrorMessage="*" Font-Size="Medium" Display="Static" CssClass="req" />
                                </div>
                            </asp:Panel>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTimeSpan" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="col-sm-3" style="padding-top:20px">
                    
                        <asp:Button ID="btnSearch" ValidationGroup="valGroup1" runat="server" Text="Search" class="form-control btn-primary" OnClick="btnSearch_Click"/>
                    
                </div>   
                </div>
            </div>
            <div class="row">
                <asp:Label ID="lblSearchResults" runat="server" />
                <asp:UpdatePanel ID="uplSearchResults" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlSearchResults" runat="server">
                            <asp:GridView ID="gvSearchResults" runat="server" CellPadding="4"
                                HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
                                OnPageIndexChanging="gvListOfPremises_PageIndexChanging" OnRowDataBound="gvListOfPremises_RowDataBound" OnSorting="gvSearchResults_Sorting"
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
                                    <%--<asp:BoundField DataField="ContactId" HeaderText="ContactId" />
                                    <asp:BoundField DataField="Action" HeaderText="Action" />
                                    <asp:BoundField DataField="ActionDateTime" HeaderText="ActionDateTime" />
                                    <asp:BoundField DataField="ActionBy" HeaderText="ActionBy" />
                                    <asp:BoundField DataField="RescheduleDate" HeaderText="RescheduleDate" />
                                    <asp:BoundField DataField="DoNotContactAgainDate" HeaderText="DoNotContactAgainDate" />
                                    <asp:BoundField DataField="PremiseID" HeaderText="PremiseID" />
                                    <asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/>--%>                
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        
</asp:Content>
