<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CurrentArea.aspx.cs" Inherits="GISWeb.CurrentArea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link type="text/css" href="Content/bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="Content/bootstrap-datepicker.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap-datepicker.min.js"></script>

    <script src="Scripts/moment.min.js"></script>

    <style>
        .centerHeaderText th {
            text-align: center;
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

    <script>

        function openModal() {
            alert("opened");
            $('#myModal').modal('show');
        }

        function closeModal() {
            $('#myModal').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        } 

        $(document).ready(function () {
            
            
                       
            var hdnfldPageIndex = $('#hdnfldNumRows').val();
            //alert(hdnfldPageIndex);

            for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {               
                $("#btnPostcodes".concat(i)).popover()
            };

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EnPopover);

            function EnPopover(sender, args) {               
                var hdnfldPageIndex = $('#hdnfldNumRows').val();
                //alert(hdnfldPageIndex);
                

                for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {
                    $("#btnPostcodes".concat(i)).popover()
                };
            };

            for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {               

                    var name = "EndRequestHandler".concat(i);
                    

                    $(".input-group.date.endDate#datepicker".concat(i)).datepicker({
                        format: 'dd/mm/yyyy',
                        todayHighlight: true,
                        clearBtn: false,
                        orientation: 'bottom'
                    });
               
                //document.getElementById("txtBox".concat(i)).value = gv.rows[i].cells[5] ;
                date = moment($("#txtBox".concat(i)).val(), "D/MM/yyyy").format("D/MM/yyyy");
                $(".input-group.date.endDate#datepicker".concat(i)).datepicker('setDate', date);

                
            };            
            
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(En);

            function En(sender, args) {
                var hdnfldPageIndex = $('#hdnfldNumRows').val();
                //alert(hdnfldPageIndex);

                for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {

                        $(".input-group.date.endDate#datepicker".concat(i)).datepicker({
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            clearBtn: false,
                            orientation: 'bottom'
                        });

   
                        //document.getElementById("txtBox".concat(i)).value = gv.rows[i].cells[6];
                        //document.getElementById("txtBox".concat(i)).value = 0;
                        date = moment($("#txtBox".concat(i)).val(), "D/MM/yyyy").format("D/MM/yyyy");
                        $(".input-group.date.endDate#datepicker".concat(i)).datepicker('setDate', date);                   
                   
                };
            };

            var hdnfldPageIndex = $('#hdnfldNumRows').val();
            //alert(hdnfldPageIndex);

            for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {

                var name = "EndRequestHandler".concat(i);

                $('.input-group.date.startDate#datepickerStartDate'.concat(i)).datepicker({
                    format: 'dd/mm/yyyy',
                    todayHighlight: true,
                    clearBtn: false,
                    orientation: 'bottom'
                });

                startDate = moment($("#txtBoxStartDate".concat(i)).val(), "D/MM/yyyy").format("D/MM/yyyy");
                $('.input-group.date.startDate#datepickerStartDate'.concat(i)).datepicker('setDate', startDate);
            };

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EnStartDate);

            function EnStartDate(sender, args) {
                var hdnfldPageIndex = $('#hdnfldNumRows').val();
                //alert(hdnfldPageIndex);

                for (var i = 0; i < parseInt(hdnfldPageIndex); i++) {
                                   

                    $('.input-group.date.startDate#datepickerStartDate'.concat(i)).datepicker({
                        format: 'dd/mm/yyyy',
                        todayHighlight: true,
                        clearBtn: false,
                        orientation: 'bottom'
                    });

                    startDate = moment($("#txtBoxStartDate".concat(i)).val(), "D/MM/yyyy").format("D/MM/yyyy");
                    $('.input-group.date.startDate#datepickerStartDate'.concat(i)).datepicker('setDate', startDate);
                };
            };

        });

    </script>


    <h2>List Allocated Areas</h2>
    <div class="container">
        <asp:UpdatePanel ID="uplCurrentArea" runat="server">
            <ContentTemplate>
                <br />
                <br />
                <asp:HiddenField ID="hdnfldNumRows" runat="server" ClientIDMode="Static"/> 
                 <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="lblPostCode" runat="server" Text="Enter PostCode" />
                        <asp:TextBox ID="txtPostCode" runat="server" Text="" class="form-control"/>               
                    </div>
                    <div class="col-sm-3" style="padding-top:20px">
                        <asp:Button ID="btnPostCode" ValidationGroup="valGroup1" runat="server" Text="Search" class="form-control btn btn-primary" OnCommand="btnPostCode_Click" />
                    </div>
                    <div class="col-sm-3" style="padding-top:20px">
                        <asp:Button ID="btnReset" ValidationGroup="" runat="server" Text="Reset" class="form-control btn btn-primary" OnCommand="btnReset_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">   
                        <asp:RequiredFieldValidator ValidationGroup="valGroup1" runat="server" ID="reqPostCode" ControlToValidate="txtPostCode" ErrorMessage="*" Display="Static" ForeColor="Red" Font-Size="Medium" />
                        <asp:RegularExpressionValidator ValidationGroup="valGroup1" ID="revPostCode" runat="server" ControlToValidate="txtPostCode" 
                            ValidationExpression="^([bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{2}|[bB][tT][0-9]?[0-9]?\s[0-9][a-zA-Z]{1}|[bB][tT][0-9]?[0-9]?\s[0-9]|[bB][tT][0-9]|[bB][tT][0-9][0-9])$" 
                            ErrorMessage="PostCode format must be one of the following: 'BT45 5RQ' , 'BT4 5RQ' ,
                            'BT45 5R' , 'BT4 5R', 'BT45 5' , 'BT4 5', 'BT45', 'BT4'" Display="Static" ForeColor="Red" />
                    </div>           
                </div>                

                <asp:Panel runat="server" ID="pnlListOfCurrentAreas" ClientIDMode="Static">
                    <br />
                    <br />
                    <asp:Label ID="lblSavedEndDate" runat="server" />
                    <asp:GridView ID="gvListOfCurrentAreas" runat="server" CellPadding="4" ClientIDMode="Static"
                        HorizontalAlign="Center" ForeColor="#333333" GridLines="None"
                        AutoGenerateColumns="true" OnRowDataBound="gvListOfCurrentAreas_RowDataBound" OnRowCommand="lnkbtn_OnRowCommand" OnRowDeleting="OnRowDeleting"
                        EmptyDataText="No Results Found" DataKeyNames="SalesRepId" CssClass="table table-striped" AllowPaging="True" PageSize="20" OnPageIndexChanging="gvListOfCurrentAreas_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" CssClass="centerHeaderText"  Font-Underline="False" BorderStyle="NotSet" />
                        <RowStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#F7F6F3" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        <Columns>
                            <%--<asp:BoundField DataField="RepAreaId" HeaderText="RepAreaId" />
                    <asp:BoundField DataField="RepName" HeaderText="Rep Name" />
                    <asp:BoundField DataField="SalesRepId" HeaderText="SalesRepId" />
                    <asp:BoundField DataField="PostalCodeID" HeaderText="PostalCodeID" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" />
                    <asp:BoundField DataField="DateAdded" HeaderText="DateAdded" />
                    <asp:BoundField DataField="Archived" HeaderText="Archived" /> --%>
                            <%--<asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/> --%>
                        </Columns>
                    </asp:GridView>

                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAllocatedAreas">
                    <br />
                    <br />
                    <asp:Label ID="lblAllocatedPremises" runat="server" />
                    <asp:GridView ID="gvAllocatedAreas" runat="server" CellPadding="4"
                        HorizontalAlign="Left" ForeColor="#333333" GridLines="None"
                        AutoGenerateColumns="False"
                        EmptyDataText="No Results Found" DataKeyNames="PremiseID" CssClass="table table-striped">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" CssClass="centerHeaderText" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle ForeColor="#333333" HorizontalAlign="Left" BackColor="#F7F6F3" />
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
                                HeaderText="PremiseId" Target="_blank" />
                            <asp:BoundField DataField="MPRN" HeaderText="MPRN" />
                            <asp:BoundField DataField="MeterPointAddress" HeaderText="Meter Point Address" />
                            <asp:BoundField DataField="DUoSGroup" HeaderText="DUoS Group" />
                            <%--                    <asp:BoundField DataField="MeterConfigurationCode" HeaderText="MeterConfigurationCode" />--%>
                            <asp:BoundField DataField="MeterPointStatus" HeaderText="MeterPointStatus" />
                            <%--                    <asp:BoundField DataField="PostalCodeID" HeaderText="PostalCodeID" />--%>
                            <asp:BoundField DataField="Live" HeaderText="Live" />
                            <asp:BoundField DataField="Pending" HeaderText="Pending" />

                            <%--<asp:BoundField DataField="ApplicationDate" HeaderText="Sign Up Date" DataFormatString="{0:dd/MM/yyyy}"/> --%>
                        </Columns>
                    </asp:GridView>

                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPostCode" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="pnlModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="myModal" tabindex ="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-left" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h3 class="modal-title" id="exampleModalLabel">Add Item</h3>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <p><b> Are you sure you want to add an item? </b></p>         
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                                        <asp:button id="btnSubmitModal" type="button" runat="server" OnClick="btnClose3_Click" class="btn btn-primary unclaim" Text="Add" />
                                    </div>
                                </div>
                            </div>
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="pnlAddModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="removeModal" tabindex ="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-left" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h3 class="modal-title" id="addModalLabel">Remove Item</h3>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <p><b> Are you sure you want to remove an item? </b></p>         
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                                        <asp:button id="Button1" type="button" runat="server" OnClick="btnRemove_Click" class="btn btn-primary unclaim" Text="Remove" />
                                    </div>
                                </div>
                            </div>
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdateProgress ID="upCurrentArea" runat="server" AssociatedUpdatePanelID="uplCurrentArea">
        <ProgressTemplate>
            <div class="upModal">
                <div class="center">
                    <img alt="" src="./Content/Site/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdateProgress ID="upUnclaimModal" runat="server" AssociatedUpdatePanelID="pnlModal">
        <ProgressTemplate>
            <div class="upModal">
                <div class="center">
                    <img alt="" src="./Content/Site/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
     <asp:UpdateProgress ID="upCurrentArea1" runat="server" AssociatedUpdatePanelID="pnlAddModal">
        <ProgressTemplate>
            <div class="upModal">
                <div class="center">
                    <img alt="" src="./Content/Site/loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
