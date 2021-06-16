using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;

namespace GISWeb
{
    public partial class CurrentArea : System.Web.UI.Page
    {
        static List<SalesRep> salesReps = new List<SalesRep>();
        static ArrayList tooltipArray = new ArrayList();
        static DateTime nDaysAgo = DateTime.Now.AddDays(-90); //include RepAreas with DateAdded more than n days old
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                gvListOfCurrentAreas.DataSource = (DataTable)ViewState["dt"];
                gvListOfCurrentAreas.DataBind();

                RegisterPostBackControl();
            }
            else
            {
                pnlListOfCurrentAreas.Visible = true;
                pnlAllocatedAreas.Visible = false;

                using (GISEntities context = new GISEntities())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                        {
                            salesReps = context.SalesReps.Where(s => s.Company == "Click Energy" && s.Archived == false).Select(s => s).ToList();

                            //GetCurrentAreas(ref dt, salesReps);

                        }
                        else if (User.IsInRole(@"DOMAIN\Reps"))
                        {
                            salesReps = context.SalesReps.Where(s => s.Company == "FSR" && s.Archived == false).Select(s => s).ToList();

                            //GetCurrentAreas(ref dt, salesReps);
                        }
                        else
                        {
                            salesReps = context.SalesReps.Where(s => s.Archived == false).Select(s => s).ToList();

                            //GetCurrentAreas(ref dt, salesReps);
                        }

                        RegisterPostBackControl();
                        /*if (dt != null)
                        {
                            gvListOfCurrentAreas.DataSource = dt;
                            gvListOfCurrentAreas.DataBind();
                        }

                        ViewState["dt"] = dt;*/

                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine("{0} Exception caught:", Ex);
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("CurrentArea.aspx");
        }

        protected void btnPostCode_Click(object sender, EventArgs e)
        {
            pnlAllocatedAreas.Visible = false;

            try
            {
                using (GISEntities context = new GISEntities())
                {
                    DataTable dt = new DataTable();

                    GetCurrentAreas(ref dt, salesReps);

                    if (dt != null)
                    {
                        gvListOfCurrentAreas.DataSource = dt;
                        gvListOfCurrentAreas.DataBind();

                        hdnfldNumRows.Value = dt.Rows.Count.ToString();
                       
                    }

                    ViewState["dt"] = dt;

                    /*ArrayList tooltipArrayCopy = new ArrayList();
                    string postcode = txtPostCode.Text.ToLower();

                    if (ViewState["dt"] == null)
                        return;

                    dt = (DataTable)ViewState["dt"];

                    DataTable dtNew = dt.Clone();



                    foreach (DataRow row in dt.Rows)
                    {
                        string[] commandArgs = tooltipArray[dt.Rows.IndexOf(row)].ToString().Split(new string[] { "<div>", "</div>" }, StringSplitOptions.None);

                        List<string> tooltipPostcodes = new List<string>();

                        for (int i = 0; i <= commandArgs.Length - 1; i++)
                        {
                            if (commandArgs[i] != "")
                            {
                                tooltipPostcodes.Add(commandArgs[i].ToLower());
                            }
                        }

                        foreach (string item in tooltipPostcodes)
                        {
                            if (item.StartsWith(postcode))
                            {
                                dtNew.Rows.Add(row.ItemArray);
                                tooltipArrayCopy.Add(tooltipArray[dt.Rows.IndexOf(row)]);
                                break;
                            }
                        }
                    }

                    tooltipArray = tooltipArrayCopy;

                    ViewState["dt"] = dtNew;
                    gvListOfCurrentAreas.DataSource = (DataTable)ViewState["dt"];
                    gvListOfCurrentAreas.DataBind();

                    pnlListOfCurrentAreas.Visible = true;

                    lblSavedEndDate.Text = dtNew.Rows.Count.ToString() + " results found.";*/
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }

        }

        private DataTable ConvertToDataTable<T>(IList<T> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {

                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                }

                table.Rows.Add(row);
            }
            return table;
        }

        protected void gvListOfCurrentAreas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<RepArea> repAreas = new List<RepArea>();
            List<int> result = new List<int>();

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[6].Visible = false; //DateAdded
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[14].Visible = true;
                e.Row.Cells[15].Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[6].Visible = false; //DateAdded
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[14].Visible = true;
                e.Row.Cells[15].Visible = false;

                using (GISEntities context = new GISEntities())
                {
                    //use e.Row.Cells[2].Text (SalesRepId) to find SaleRep's Company in SalesReps and reassign RepName to 'Click Energy Group' or 'FSR Group' for DateAdded > 3 months ago
                    int salesRepId = Convert.ToInt32(e.Row.Cells[2].Text);
                    SalesRep salesRep = context.SalesReps.Where(s => s.SalesRepId == salesRepId).FirstOrDefault();

                    e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text).ToShortDateString(); //display startdate date only

                    string repName = e.Row.Cells[1].Text;
                    DateTime dateAdded = Convert.ToDateTime(e.Row.Cells[6].Text).AddMilliseconds(-(Convert.ToDateTime(e.Row.Cells[6].Text).Millisecond)); //set milliseconds to 0

                    //If salesRep is 'Click Energy Group' or 'FSR Group' find RepAreas Archived or not Archived

                    if (dateAdded < nDaysAgo) //dateAdded > 3 months ago
                    {
                        repAreas = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s).ToList();

                        result = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s.RepAreaId).ToList();
                    }
                    else
                    {
                        repAreas = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            s.Archived == false && dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s).ToList();

                        result = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            s.Archived == false && dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s.RepAreaId).ToList();
                    }

                    string res = String.Empty;

                    for (int i = 0; i <= result.Count - 1; i++) //place PostalCodeIDs in a comma delimited string which will be linkbutton and button commandArgument
                    {
                        if (i.Equals(result.Count - 1))
                        {
                            res += result[i].ToString();
                        }
                        else
                        {
                            res += result[i].ToString() + ",";
                        }
                    }



                    /*PlaceHolder phPostcodes = new PlaceHolder();
                    e.Row.Cells[9].Controls.Add(phPostcodes);

                    Button btnPostcodes = new Button();
                    btnPostcodes.ID = "btnPostcodes" + e.Row.RowIndex;
                    btnPostcodes.ClientIDMode = ClientIDMode.Static;

                    btnPostcodes.Attributes["data-toggle"] = "popover";
                    btnPostcodes.Attributes["data-placement"] = "right";
                    btnPostcodes.Attributes["data-html"] = "true";
                    btnPostcodes.Attributes["data-content"] = tooltipArray[e.Row.RowIndex].ToString();
                    btnPostcodes.OnClientClick = "return false";
                    btnPostcodes.Text = e.Row.Cells[9].Text;
                    btnPostcodes.Attributes["class"] = "btn btn-primary";

                    phPostcodes.Controls.Add(btnPostcodes);*/


                    PlaceHolder phReallocate = new PlaceHolder();
                    e.Row.Cells[16].Controls.Add(phReallocate);

                    //drop down list that will reallocate a salesrep
                    DropDownList ddlReallocate = new DropDownList();
                    ddlReallocate.ID = "ddlReallocate" + e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize);
                    ddlReallocate.CssClass = "form-control";
                    ddlReallocate.AutoPostBack = true;
                    ddlReallocate.AppendDataBoundItems = true;
                    ddlReallocate.SelectedIndexChanged += new EventHandler(ddlReallocate_SelectIndexChanged);
                    ddlReallocate.Items.Add(new ListItem("Select salesrep", "0"));
                    ddlReallocate.DataTextField = "RepName";
                    ddlReallocate.DataValueField = "SalesRepId";
                    ddlReallocate.DataSource = salesReps;
                    ddlReallocate.Style["width"] = "150px";

                    if ((e.Row.Cells[1].Text == "Click Energy Group") || (e.Row.Cells[1].Text == "FSR Group"))
                    {
                        if ((Convert.ToDateTime(e.Row.Cells[5].Text) < DateTime.Now))
                        {
                            ddlReallocate.Enabled = false;
                        }
                        else
                        {
                            ddlReallocate.Enabled = true;
                        }
                    }
                    else
                    {
                        ddlReallocate.Enabled = true;
                    }

                    ddlReallocate.DataBind();
                    phReallocate.Controls.Add(ddlReallocate);

                    //Add SalesRep Button
                    Button btnAddSalesRep = new Button();
                    btnAddSalesRep.Text = "+";
                    btnAddSalesRep.ID = "btnAddSalesRep" + e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize);
                    btnAddSalesRep.ClientIDMode = ClientIDMode.Static;
                    btnAddSalesRep.Attributes["class"] = "btn btn-sm btn-primary";
                    btnAddSalesRep.ValidationGroup = "ValGroup1";
                    btnAddSalesRep.CommandName = "add";
                    btnAddSalesRep.CommandArgument = res; //append textBox ID to commandArgument
                    btnAddSalesRep.Visible = true;
                    btnAddSalesRep.Click += new EventHandler(AddNewRowToGridView_Click);

                    //Add SalesRep Button
                    Button btnRemoveSalesRep = new Button();
                    btnRemoveSalesRep.Text = "-";
                    btnRemoveSalesRep.ID = "btnRemoveSalesRep" + e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize);
                    btnRemoveSalesRep.ClientIDMode = ClientIDMode.Static;
                    btnRemoveSalesRep.Attributes["class"] = "btn btn-sm btn-primary";
                    btnRemoveSalesRep.ValidationGroup = "ValGroup1";
                    btnRemoveSalesRep.CommandName = "add";
                    btnRemoveSalesRep.CommandArgument = res; //append textBox ID to commandArgument

                    if (e.Row.Cells[15].Text == "1")
                    {
                        btnRemoveSalesRep.Visible = true;
                        btnAddSalesRep.Visible = true;
                    }
                    else
                    {
                        btnRemoveSalesRep.Visible = true;
                        btnAddSalesRep.Visible = true;
                    }

                    btnRemoveSalesRep.Click += new EventHandler(RemoveButton_Click);

                    phReallocate.Controls.Add(btnRemoveSalesRep);
                    phReallocate.Controls.Add(btnAddSalesRep);

                    //Make StartDate Editable
                    PlaceHolder phStartDate = new PlaceHolder();

                    e.Row.Cells[4].Controls.Add(phStartDate);
                    TextBox txtBoxStartDate = new TextBox();
                    txtBoxStartDate.ID = "txtBoxStartDate" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    txtBoxStartDate.ClientIDMode = ClientIDMode.Static;
                    txtBoxStartDate.TextChanged += new EventHandler(TextChangedEventHandlerStartDate);
                    txtBoxStartDate.AutoPostBack = true;
                    txtBoxStartDate.Text = e.Row.Cells[4].Text;
                    txtBoxStartDate.Attributes["class"] = "form-control";
                    txtBoxStartDate.Attributes["ReadOnly"] = "true";
                    txtBoxStartDate.Style["Width"] = "100px";

                    //Save Button
                    Button btnStartDate = new Button();
                    btnStartDate.Text = "Save";
                    btnStartDate.ID = "btnStartDate" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    btnStartDate.ClientIDMode = ClientIDMode.Static;
                    btnStartDate.Attributes["class"] = "btn btn-sm btn-primary";
                    btnStartDate.ValidationGroup = "ValGroup1";
                    btnStartDate.CommandName = "save";
                    btnStartDate.CommandArgument = res; //append textBox ID to commandArgument
                    btnStartDate.Visible = false;

                    var divStartDateRow = new HtmlGenericControl("div");
                    divStartDateRow.Attributes["class"] = "row";

                    //EndDate div
                    var divStartDate = new HtmlGenericControl("div");
                    divStartDate.Attributes["class"] = "form-group";
                    divStartDate.Style["display"] = "flex";
                    divStartDate.Style["justify-content"] = "space-evenly";
                    //divEndDate.Style["border"] = "dashed";

                    //textbox div
                    var divTxtStartDate = new HtmlGenericControl("div");
                    divTxtStartDate.Attributes["class"] = "input-group date startDate";
                    divTxtStartDate.Attributes["id"] = "datepickerStartDate" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString();
                    divTxtStartDate.Style["padding-top"] = "0px";
                    divTxtStartDate.Style["width"] = "50px";

                    //datepicker calendar
                    var span1StartDate = new HtmlGenericControl("span");
                    span1StartDate.Attributes["class"] = "input-group-addon";

                    //Save button div
                    var divBtnStartDate = new HtmlGenericControl("div");
                    divBtnStartDate.Attributes["class"] = "";
                    divBtnStartDate.Visible = false;

                    var span2StartDate = new HtmlGenericControl("span");
                    span2StartDate.Attributes["class"] = "glyphicon glyphicon-calendar";
                    span1StartDate.Controls.Add(span2StartDate);

                    phStartDate.Controls.Add(divStartDateRow);
                    divStartDateRow.Controls.Add(divStartDate);
                    divTxtStartDate.Controls.Add(txtBoxStartDate);
                    divTxtStartDate.Controls.Add(span1StartDate);

                    divStartDate.Controls.Add(divTxtStartDate);
                    divStartDate.Controls.Add(divBtnStartDate);
                    divBtnStartDate.Controls.Add(btnStartDate);

                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBoxStartDate);
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnStartDate);

                    PlaceHolder ph = new PlaceHolder();
                    e.Row.Cells[1].Controls.Add(ph);

                    LinkButton lnkbtn = new LinkButton();

                    if (Convert.ToDateTime(e.Row.Cells[6].Text) < nDaysAgo) //dateAdded > 3 months ago
                    {
                        if (salesRep.Company == "Click Energy") //if RepArea was allocated to 'Click Energy' SalesRep change RepName to 'Click Energy Group'
                        {
                            foreach (var item in repAreas)
                            {
                                item.RepName = "Click Energy Group";
                                //item.Archived = false; //unarchive RepArea
                                item.SalesRepId = context.SalesReps.Where(s => s.RepName == "Click Energy Group").Select(s => s.SalesRepId).FirstOrDefault();//change SalesRepId
                            }
                            context.SaveChanges();

                            lnkbtn.Text = "Click Energy Group";
                            lnkbtn.ForeColor = System.Drawing.Color.Red;

                        }
                        else if (salesRep.Company == "FSR")
                        {
                            foreach (var item in repAreas)
                            {
                                item.RepName = "FSR Group";
                                //item.Archived = false;
                                item.SalesRepId = context.SalesReps.Where(s => s.RepName == "FSR Group").Select(s => s.SalesRepId).FirstOrDefault();//change SalesRepId
                            }
                            context.SaveChanges();

                            lnkbtn.Text = "FSR Group";
                            lnkbtn.ForeColor = System.Drawing.Color.Red;
                        }




                    }
                    else
                    {
                        lnkbtn.Text = e.Row.Cells[1].Text;
                    }

                    //EndDate

                    lnkbtn.ID = "lnkbtn" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString();
                    lnkbtn.ClientIDMode = ClientIDMode.Static;
                    //lnkbtn.Click += new EventHandler(lnkbtn_Click);
                    lnkbtn.CommandName = "select";
                    lnkbtn.CommandArgument = res + " , " + e.Row.Cells[1].Text.ToString(); //append RepName onto commandArgument
                    ph.Controls.Add(lnkbtn);

                    PlaceHolder phEndDate = new PlaceHolder();

                    e.Row.Cells[5].Controls.Add(phEndDate);
                    TextBox txtBox = new TextBox();
                    txtBox.ID = "txtBox" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    txtBox.ClientIDMode = ClientIDMode.Static;
                    txtBox.TextChanged += new EventHandler(TextChangedEventHandler);
                    txtBox.AutoPostBack = true;
                    txtBox.Text = e.Row.Cells[5].Text;
                    txtBox.Attributes["class"] = "form-control";
                    txtBox.Attributes["ReadOnly"] = "true";
                    txtBox.Style["Width"] = "100px";

                    RequiredFieldValidator reqEndDate = new RequiredFieldValidator();
                    reqEndDate.ControlToValidate = "txtBox" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    reqEndDate.ErrorMessage = "*";
                    reqEndDate.ForeColor = System.Drawing.Color.Red;
                    reqEndDate.CssClass = "req";
                    reqEndDate.ClientIDMode = ClientIDMode.Static;
                    reqEndDate.ValidationGroup = "ValGroup1";
                    //phEndDate.Controls.Add(reqEndDate);

                    RegularExpressionValidator revEndDate = new RegularExpressionValidator();
                    revEndDate.ControlToValidate = "txtBox" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    revEndDate.ErrorMessage = "Format must be 'yyyy-mm-dd'";
                    revEndDate.ForeColor = System.Drawing.Color.Red;
                    revEndDate.CssClass = "req";
                    revEndDate.ClientIDMode = ClientIDMode.Static;
                    revEndDate.ValidationGroup = "ValGroup1";
                    revEndDate.ValidationExpression = @"^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$";

                    //Save Button
                    Button btn = new Button();
                    btn.Text = "Save";
                    btn.ID = "btn" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));
                    btn.ClientIDMode = ClientIDMode.Static;
                    btn.Attributes["class"] = "btn btn-sm btn-primary";
                    btn.ValidationGroup = "ValGroup1";
                    btn.CommandName = "save";
                    btn.CommandArgument = res; //append textBox ID to commandArgument
                    btn.Visible = false;

                    var divRow = new HtmlGenericControl("div");
                    divRow.Attributes["class"] = "row";

                    //EndDate div
                    var divEndDate = new HtmlGenericControl("div");
                    divEndDate.Attributes["class"] = "form-group";
                    divEndDate.Style["display"] = "flex";
                    divEndDate.Style["justify-content"] = "space-evenly";
                    divEndDate.Style["padding-left"] = "5px";
                    //divEndDate.Style["border"] = "dashed";

                    //textbox div
                    var div = new HtmlGenericControl("div");
                    div.Attributes["class"] = "input-group date endDate";
                    div.Attributes["id"] = "datepicker" + (e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString();
                    div.Style["padding-top"] = "0px";
                    div.Style["width"] = "50px";

                    //datepicker calendar
                    var span1 = new HtmlGenericControl("span");
                    span1.Attributes["class"] = "input-group-addon";

                    var span2 = new HtmlGenericControl("span");
                    span2.Attributes["class"] = "glyphicon glyphicon-calendar";
                    span1.Controls.Add(span2);

                    //Save button div
                    var divBtn = new HtmlGenericControl("div");
                    divBtn.Attributes["class"] = "";
                    divBtn.Visible = false;

                    //Required field div
                    var divReq = new HtmlGenericControl("div");
                    divReq.Attributes["class"] = "";
                    divReq.Style["float"] = "left";

                    phEndDate.Controls.Add(divRow);
                    divRow.Controls.Add(divEndDate);
                    div.Controls.Add(txtBox);
                    div.Controls.Add(span1);
                    divBtn.Controls.Add(btn);
                    divReq.Controls.Add(reqEndDate);

                    divEndDate.Controls.Add(div);
                    divEndDate.Controls.Add(divBtn);

                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkbtn);
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBox);
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnPostcodes);

                    string[] commandArgs = tooltipArray[(e.Row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize))].ToString().Split(new string[] { "<div>", "</div>" }, StringSplitOptions.None);

                    List<string> tooltipPostcodes = new List<string>();

                    for (int i = 0; i <= commandArgs.Length - 1; i++)
                    {
                        if (commandArgs[i] != "")
                        {
                            tooltipPostcodes.Add(commandArgs[i].ToLower());
                        }
                    }

                    //if (e.Row.Cells[15].Text == "0")
                    //{
                    //Find the shortest common PostCode of all postcodes allocated to each salesRep.
                    // Determine size of the array
                    int n = tooltipPostcodes.Count;

                    // Take first word from array as reference
                    String postcode1 = tooltipPostcodes[0];
                    int len = postcode1.Length;

                    String commonPostcode = "";

                    for (int i = 0; i < len; i++)
                    {
                        for (int j = i + 1; j <= len; j++)
                        {
                            // generating all substrings
                            // of our reference string arr[0] starting at first character.
                            String stem = postcode1.Substring(0, j);
                            int k = 1;
                            for (k = 1; k < n; k++)
                            {
                                // Check if the generated stem is
                                // common to all words
                                if (!tooltipPostcodes[k].Contains(stem))
                                {
                                    break;
                                }
                            }
                            // If current substring is present in
                            // all strings and its length is greater
                            // than current result
                            if (k == n && commonPostcode.Length < stem.Length)
                            {
                                commonPostcode = stem;
                            }
                        }
                    }

                    e.Row.Cells[10].Text = commonPostcode.ToUpper(); //assign resulting postcode to gridview column 'Postcodes'
                    //}

                }
            }
        }

        protected void AddNewRowToGridView_Click(object sender, EventArgs e)
        {
            Button ddlReallocate = (Button)sender;
            GridViewRow row = (GridViewRow)ddlReallocate.NamingContainer;

            rowIndexAddNewRow = row.RowIndex;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);



            //Add new rows to RepArea table
            //
        }

        protected void AddNewRowToGridView(int rowIndex)
        {

            //Add new rows to RepArea table.
            using (GISEntities context = new GISEntities())
            {
                //Extract postalCodeIds from toolTipArray element
                string[] commandArgs = tooltipArray[rowIndex].ToString().Split(new string[] { "<div>", "</div>" }, StringSplitOptions.None);

                List<string> tooltipPostcodes = new List<string>();

                for (int i = 0; i <= commandArgs.Length - 1; i++)
                {
                    if (commandArgs[i] != "")
                    {
                        tooltipPostcodes.Add(commandArgs[i].ToLower());
                    }
                }

                DateTime dateAddedNew = DateTime.Now; //asign timestamp

                foreach (var item in tooltipPostcodes)
                {
                    //
                    try
                    {
                        using (PostcodesEntities postcodeContext = new PostcodesEntities())
                        {
                            DataTable dt = (DataTable)ViewState["dt"];

                            DateTime dateAdded = Convert.ToDateTime(dt.Rows[rowIndex]["Date Added"]);

                            dateAdded = dateAdded.AddTicks(-(dateAdded.Ticks % TimeSpan.TicksPerSecond)); //set milliseconds of dateAdded to 0

                            //DateTime dateAdded = Convert.ToDateTime(gvListOfCurrentAreas.Rows[rowIndex].Cells[6].Text);

                            var postalcodes = postcodeContext.PostalCodes.Where(r => r.FullPostcode.ToLower() == item.ToLower()).Select(r => r).FirstOrDefault(); //find RepAreas belonging to gridview row.

                            var repArea = context.RepAreas.Where(r => r.PostalCodeID == postalcodes.PostalCodeID &&
                                DbFunctions.AddMilliseconds(r.DateAdded, -(r.DateAdded.Millisecond)) == dateAdded).Select(r => r).FirstOrDefault();

                            if (repArea != null) // create new record in RepArea table
                            {
                                RepArea repAreaNew = new RepArea()
                                {
                                    RepName = repArea.RepName,
                                    SalesRepId = repArea.SalesRepId,
                                    PostalCodeID = repArea.PostalCodeID,
                                    StartDate = repArea.StartDate,
                                    EndDate = repArea.EndDate,
                                    DateAdded = dateAddedNew,
                                    Archived = false
                                };

                                context.RepAreas.Add(repAreaNew);
                                context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                }
                //Get corresponding postalcodeIds

                /*foreach (string item in tooltipPostcodes)
                {
                    if (item.StartsWith(postcode))
                    {
                        dtNew.Rows.Add(row.ItemArray);
                        tooltipArrayCopy.Add(tooltipArray[dt.Rows.IndexOf(row)]);
                        break;
                    }
                }

                var repAreas = context.RepAreas.Where(r => r.RepAreaId.Contains(repAreaId)); //find RepAreas belonging to gridview row.

                try
                {
                    RepArea repArea = new RepArea()
                    {
                        RepAreaId = ,
                        Archived = ,
                        RepName = ,

                        
                    }
                }
                catch(Exception Ex)
                {
                    throw Ex;
                }*/

                if (ViewState["dt"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["dt"];
                    DataRow drCurrentRow = null;

                    if (dtCurrentTable.Rows.Count > 0)
                    {


                        ViewState["dt"] = dtCurrentTable;

                        for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                        {

                            //extract the TextBox values   
                            string box1 = dtCurrentTable.Rows[i]["Rep Name"].ToString();
                            string box2 = dtCurrentTable.Rows[i]["SalesRepId"].ToString();
                            //string box3 = dtCurrentTable.Rows[i][""].ToString();
                            DateTime box4 = Convert.ToDateTime(dtCurrentTable.Rows[i]["Start Date"]); //startDate
                            DateTime box5 = Convert.ToDateTime(dtCurrentTable.Rows[i]["End Date"]); //endDate
                            DateTime box6 = Convert.ToDateTime(dtCurrentTable.Rows[i]["Date Added"]); //dateAdded
                            //string box7 = dtCurrentTable.Rows[i][7].ToString();
                            //string box8 = dtCurrentTable.Rows[i][8].Text;
                            //string box9 = dtCurrentTable.Rows[i][9].Text;
                            string box10 = dtCurrentTable.Rows[i]["Postcodes"].ToString(); //postcodes
                            string box11 = dtCurrentTable.Rows[i]["Premises"].ToString(); //premises
                            string box12 = dtCurrentTable.Rows[i]["Keypad"].ToString(); //keypad
                            string box13 = dtCurrentTable.Rows[i]["Billpay"].ToString(); //billpay
                            string box14 = dtCurrentTable.Rows[i]["Click Customers"].ToString(); //click customers
                            //DropDownList box15 = (DropDownList)dtCurrentTable.Rows[i][15].FindControl("ddlReallocate" + i.ToString());

                            dtCurrentTable.Rows[i]["Rep Name"] = box1;
                            dtCurrentTable.Rows[i]["SalesRepId"] = box2;
                            dtCurrentTable.Rows[i]["Start Date"] = box4;
                            dtCurrentTable.Rows[i]["End Date"] = box5;
                            dtCurrentTable.Rows[i]["Postcodes"] = box10;
                            dtCurrentTable.Rows[i]["Date Added"] = box6;
                            dtCurrentTable.Rows[i]["Keypad"] = box12;
                            dtCurrentTable.Rows[i]["Billpay"] = box13;
                            dtCurrentTable.Rows[i]["Premises"] = box11;
                            dtCurrentTable.Rows[i]["Keypad"] = box12;
                            dtCurrentTable.Rows[i]["Billpay"] = box13;
                            dtCurrentTable.Rows[i]["Click Customers"] = box14;
                            //dtCurrentTable.Rows[i]["Reallocate"] = box15.SelectedItem;
                            //dtCurrentTable.Rows[i]["Duplicate"] = "0";

                            //extract the DropDownList Selected Items   

                            //DropDownList ddl1 = (DropDownList)gvListOfCurrentAreas.Rows[i].Cells[3].FindControl("DropDownList1");
                            //DropDownList ddl2 = (DropDownList)gvListOfCurrentAreas.Rows[i].Cells[4].FindControl("DropDownList2");

                            if (i == rowIndex)
                            {
                                tooltipArray.Insert(rowIndex + 1, tooltipArray[i]);
                            }
                        }

                        //if duplicating last row
                        if (rowIndex == dtCurrentTable.Rows.Count - 1)
                        {
                            tooltipArray.Add(tooltipArray[rowIndex]);
                        }

                        int j = rowIndex;

                        //extract the TextBox values   
                        string box1New = dtCurrentTable.Rows[j]["Rep Name"].ToString();
                        string box2New = dtCurrentTable.Rows[j]["SalesRepId"].ToString();
                        //string box3New = dtCurrentTable.Rows[j][3];
                        DateTime box4New = Convert.ToDateTime(dtCurrentTable.Rows[j]["Start Date"].ToString());
                        DateTime box5New = Convert.ToDateTime(dtCurrentTable.Rows[j]["End Date"].ToString());
                        DateTime box6New = Convert.ToDateTime(dtCurrentTable.Rows[j]["Date Added"].ToString());
                        //string box7New = dtCurrentTable.Rows[j][7].Text;
                        //string box8New = dtCurrentTable.Rows[j][8].Text;
                        //string box9New = dtCurrentTable.Rows[j][9].Text;
                        string box10New = dtCurrentTable.Rows[j]["Postcodes"].ToString();
                        string box11New = dtCurrentTable.Rows[j]["Premises"].ToString();
                        string box12New = dtCurrentTable.Rows[j]["Keypad"].ToString();
                        string box13New = dtCurrentTable.Rows[j]["Billpay"].ToString();
                        string box14New = dtCurrentTable.Rows[j]["Click Customers"].ToString();
                        //DropDownList box15New = (DropDownList)gvListOfCurrentAreas.Rows[j].Cells[14].FindControl("ddlReallocate" + j.ToString());
                        string box16New = "1";

                        drCurrentRow = dtCurrentTable.NewRow();
                        //drCurrentRow["Primary Key"] = Convert.ToInt32(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Primary Key"]) + 1;
                        //drCurrentRow["Row Number"] = Convert.ToInt32(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Row Number"]) + 1;

                        List<int> primaryKeyList = dtCurrentTable.AsEnumerable()
                           .Select(r => r.Field<int>("Primary Key"))
                           .ToList();

                        int maxPrimaryKey = primaryKeyList.Max();

                        /*j = rowIndex + 1;

                        dtCurrentTable.Rows[j]["Rep Name"] = box1New;
                        dtCurrentTable.Rows[j]["SalesRepId"] = box2New;
                        dtCurrentTable.Rows[j]["Start Date"] = box4New;
                        dtCurrentTable.Rows[j]["End Date"] = box5New;
                        dtCurrentTable.Rows[j]["Postcodes"] = box10New;
                        dtCurrentTable.Rows[j]["Date Added"] = box6New;
                        dtCurrentTable.Rows[j]["Premises"] = box11New;
                        dtCurrentTable.Rows[j]["Keypad"] = box12New;
                        dtCurrentTable.Rows[j]["Billpay"] = box13New;
                        dtCurrentTable.Rows[j]["Click Customers"] = box14New;
                        dtCurrentTable.Rows[j]["Reallocate"] = box15New.SelectedItem;
                        dtCurrentTable.Rows[j]["Duplicate"] = box16New;*/

                        drCurrentRow["Primary Key"] = maxPrimaryKey + 1;
                        drCurrentRow["Row Number"] = drCurrentRow["Primary Key"];
                        drCurrentRow["Rep Name"] = box1New;
                        drCurrentRow["SalesRepId"] = box2New;
                        drCurrentRow["Start Date"] = box4New;
                        drCurrentRow["End Date"] = box5New;
                        drCurrentRow["Postcodes"] = box10New;
                        drCurrentRow["Date Added"] = dateAddedNew;
                        drCurrentRow["Premises"] = box11New;
                        drCurrentRow["Keypad"] = box12New;
                        drCurrentRow["Billpay"] = box13New;
                        drCurrentRow["Click Customers"] = box14New;
                        //drCurrentRow["Reallocate"] = box15New.SelectedItem;
                        drCurrentRow["Duplicate"] = box16New;

                        dtCurrentTable.Rows.InsertAt(drCurrentRow, rowIndex + 1);

                        ViewState["dt"] = dtCurrentTable;
                        gvListOfCurrentAreas.DataSource = (DataTable)ViewState["dt"];
                        gvListOfCurrentAreas.DataBind();

                        hdnfldNumRows.Value = dtCurrentTable.Rows.Count.ToString();
                    }
                }
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["dt"] = dt;
            gvListOfCurrentAreas.DataSource = ViewState["dt"];
            gvListOfCurrentAreas.DataBind();

            hdnfldNumRows.Value = dt.Rows.Count.ToString();
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#removeModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

            Button lb = (Button)sender;
            GridViewRow gvRow = (GridViewRow)lb.NamingContainer;
            int rowID = gvRow.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize);

            rowIndexAddNewRow = rowID;

            /*DataTable dt = (DataTable)ViewState["dt"];

            if (ViewState["dt"] != null)
            {
                //Archive records in RepArea table
                //Set Previous Data on Postbacks  
                //SetPreviousData();
                using (GISEntities context = new GISEntities())
                {
                    //Extract postalCodeIds from toolTipArray element
                    string[] commandArgs = tooltipArray[rowID].ToString().Split(new string[] { "<div>", "</div>" }, StringSplitOptions.None);

                    List<string> tooltipPostcodes = new List<string>();

                    for (int i = 0; i <= commandArgs.Length - 1; i++)
                    {
                        if (commandArgs[i] != "")
                        {
                            tooltipPostcodes.Add(commandArgs[i].ToLower());
                        }
                    }

                    
                    try
                    {
                        using (PostcodesEntities postcodeContext = new PostcodesEntities())
                        {
                            DateTime dateAdded = Convert.ToDateTime(dt.Rows[rowID]["Date Added"]);

                            //Get postcodes with postalcodeIds in tooltipPostcodes
                            //Put postcodes into a list
                            //Put postalcodes into a list

                            var postalcodes = postcodeContext.PostalCodes.Where(r => tooltipPostcodes.Contains(r.FullPostcode.ToLower())).Select(r => r.PostalCodeID).ToList(); //find RepAreas belonging to gridview row.

                            var repAreas = context.RepAreas.Where(r => postalcodes.Contains(r.PostalCodeID) &&
                                DbFunctions.AddMilliseconds(r.DateAdded, -r.DateAdded.Millisecond) == DbFunctions.AddMilliseconds(dateAdded, -dateAdded.Millisecond)).Select(r => r).ToList();

                            repAreas.ForEach(r => r.Archived = true);

                            context.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                    tooltipArray.RemoveAt(rowID);
                }

                if (dt.Rows.Count > 1)
                {
                    if (gvRow.RowIndex < dt.Rows.Count)
                    {
                        //Remove the Selected Row data and reset row number  
                        dt.Rows.Remove(dt.Rows[rowID]);
                        ResetRowID(dt);
                    }
                }

                //Store the current data in ViewState for future reference  
                ViewState["dt"] = dt;

                //Re bind the GridView for the updated data  
                gvListOfCurrentAreas.DataSource = ViewState["dt"];
                gvListOfCurrentAreas.DataBind();
                           
            }*/

        }

        protected void RemoveSalesRep(int rowID)
        {
            DataTable dt = (DataTable)ViewState["dt"];

            if (ViewState["dt"] != null)
            {
                //Archive records in RepArea table
                //Set Previous Data on Postbacks  
                //SetPreviousData();
                using (GISEntities context = new GISEntities())
                {
                    //Extract postalCodeIds from toolTipArray element
                    string[] commandArgs = tooltipArray[rowID].ToString().Split(new string[] { "<div>", "</div>" }, StringSplitOptions.None);

                    List<string> tooltipPostcodes = new List<string>();

                    for (int i = 0; i <= commandArgs.Length - 1; i++)
                    {
                        if (commandArgs[i] != "")
                        {
                            tooltipPostcodes.Add(commandArgs[i].ToLower());
                        }
                    }


                    try
                    {
                        using (PostcodesEntities postcodeContext = new PostcodesEntities())
                        {
                            DateTime dateAdded = Convert.ToDateTime(dt.Rows[rowID]["Date Added"]);

                            dateAdded = dateAdded.AddTicks(-(dateAdded.Ticks % TimeSpan.TicksPerSecond)); //set milliseconds of dateAdded to 0

                            //Get postcodes with postalcodeIds in tooltipPostcodes
                            //Put postcodes into a list
                            //Put postalcodes into a list

                            var postalcodes = postcodeContext.PostalCodes.Where(r => tooltipPostcodes.Contains(r.FullPostcode.ToLower())).Select(r => r.PostalCodeID).ToList(); //find RepAreas belonging to gridview row.                           

                            var repAreas = context.RepAreas.Where(r => postalcodes.Contains(r.PostalCodeID) &&
                                DbFunctions.AddMilliseconds(r.DateAdded, -r.DateAdded.Millisecond) == DbFunctions.AddMilliseconds(dateAdded, -dateAdded.Millisecond)).Select(r => r).ToList();

                            repAreas.ForEach(r => r.Archived = true);

                            context.SaveChanges();
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                    tooltipArray.RemoveAt(rowID);
                }

                if (dt.Rows.Count > 1)
                {
                    if (rowID < dt.Rows.Count)
                    {
                        //Remove the Selected Row data and reset row number  
                        dt.Rows.Remove(dt.Rows[rowID]);
                        ResetRowID(dt);
                    }
                }

                //Store the current data in ViewState for future reference  
                ViewState["dt"] = dt;

                //Re bind the GridView for the updated data  
                gvListOfCurrentAreas.DataSource = ViewState["dt"];
                gvListOfCurrentAreas.DataBind();

                hdnfldNumRows.Value = dt.Rows.Count.ToString();
            }
        }

        private void ResetRowID(DataTable dt)
        {
            int rowNumber = 1;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["Row Number"] = rowNumber;
                    rowNumber++;
                }
            }
        }

        protected void btnSave_Click(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ddlReallocate_SelectIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlReallocate = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlReallocate.NamingContainer;
            string salesRep = ((DropDownList)sender).SelectedItem.Text; //RepName of SalesRep attempting to reallocated to
            int salesRepIdReallocate = Convert.ToInt32(((DropDownList)sender).SelectedItem.Value); //SalesRepId of SalesRep attempting to reallocate to

            LinkButton lnkbtn = row.FindControlRecursive("lnkbtn" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as LinkButton;

            DataTable dt = (DataTable)ViewState["dt"];

            //Change RepName in RepAreas table
            using (GISEntities context = new GISEntities())
            {
                try
                {

                    string[] commandArgs = lnkbtn.CommandArgument.ToString().Split(new char[] { ',' });
                    List<int> postalCodeIds = new List<int>();

                    for (int i = 0; i <= commandArgs.Length - 2; i++)
                    {
                        postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                    }

                    string lblSavedLabelsId = commandArgs[commandArgs.Length - 1];
                    Label lbl = (Label)pnlListOfCurrentAreas.FindControlRecursive(lblSavedLabelsId);
                    int salesRepId = Convert.ToInt32(row.Cells[2].Text);

                    var res = context.RepAreas.Where(s => (postalCodeIds.Contains(s.RepAreaId)) && (s.Archived == false)
                        && s.SalesRepId == salesRepId).ToList();

                    //companyReallocate is thecompany of SalesRep premises is currenly allocated to
                    //company is company of SalesRep premises is being allocated to
                    //if they match proceed with allocation
                    //if not diplay a warning message
                    string companyReallocate = context.SalesReps.Where(s => (salesRepIdReallocate.Equals(s.SalesRepId))).Select(s => s.Company).FirstOrDefault();

                    string company = context.SalesReps.Where(s => (salesRepId.Equals(s.SalesRepId))).Select(s => s.Company).FirstOrDefault();

                    if (companyReallocate == company)
                    {
                        if (res != null) //Update RepName and SalesRepId in RepAreas table
                        {
                            lnkbtn.Text = salesRep;  //Change linkbutton text 

                            foreach (var item in res)
                            {
                                item.RepName = salesRep;
                                item.SalesRepId = Convert.ToInt32(ddlReallocate.SelectedValue);
                                //item.DateAdded = DateTime.Now;

                                row.Cells[1].Text = ddlReallocate.SelectedItem.Text; //change gridview row RepName
                                row.Cells[2].Text = ddlReallocate.SelectedValue; //change gridview row SalesRepId
                                //row.Cells[6].Text = item.DateAdded.ToString();//change gridview DateAdded

                                lblSavedEndDate.Text = "SalesRep changed from " + dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][1].ToString() + " to " + salesRep;

                            }

                            context.SaveChanges();

                            dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][1] = ddlReallocate.SelectedItem.Text; //update datatable RepName
                            dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][2] = ddlReallocate.SelectedValue;//update datatable SalesRepId
                            dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][6] = row.Cells[6].Text; //update datatable DateAdded
                            ViewState["dt"] = dt;

                            gvListOfCurrentAreas.DataSource = ViewState["dt"];
                            gvListOfCurrentAreas.DataBind(); //Bind datatable to gridview

                            hdnfldNumRows.Value = dt.Rows.Count.ToString();

                            ddlReallocate.SelectedValue = "0"; //Reset ddlReallocate to 'Select item'
                        }
                    }
                    else
                    {
                        lblSavedEndDate.Text = "SalesRep " + salesRep + " does not belong to " + company + " group."; //display warning message
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }

        }

        protected void TextChangedEventHandler(object sender, EventArgs e) //change EndDate
        {
            TextBox txtBox = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtBox.NamingContainer;

            string cellEndDate = Convert.ToDateTime(row.Cells[5].Text).ToShortDateString();
            string txtBoxEndDate = txtBox.Text;

            DataTable dt = (DataTable)ViewState["dt"];

            if (cellEndDate != txtBoxEndDate) //if EndDate has changed
            {

                using (GISEntities context = new GISEntities())
                {
                    try
                    {
                        Button btn = (Button)row.FindControlRecursive("btn" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString());

                        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                        List<int> postalCodeIds = new List<int>();

                        for (int i = 0; i <= commandArgs.Length - 1; i++)
                        {
                            postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                        }

                        string txtBoxId = "txtBox" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString();
                        TextBox txt = (TextBox)pnlListOfCurrentAreas.FindControlRecursive(txtBoxId); //New EndDate
                        int salesRepId = Convert.ToInt32(row.Cells[2].Text); //SalesRepId currently allocated

                        var res = context.RepAreas.Where(s => (postalCodeIds.Contains(s.RepAreaId)) && (s.Archived == false)
                            && s.SalesRepId == salesRepId).ToList();

                        SalesRep salesRep = context.SalesReps.Where(s => s.SalesRepId == salesRepId).FirstOrDefault();

                        if (res != null)
                        {
                            if (Convert.ToDateTime(txt.Text) >= DateTime.Now.Date) //change EndDate
                            {
                                foreach (var item in res)
                                {
                                    item.EndDate = Convert.ToDateTime(txt.Text);

                                    context.SaveChanges();

                                    dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][5] = txt.Text;
                                    lblSavedEndDate.Text = "EndDate changed to " + txt.Text + " for salesRep " + item.RepName.ToString();

                                }
                            }
                            else if (Convert.ToDateTime(txt.Text) < DateTime.Now.Date) //Archive record and remove row from gridview
                            {
                                foreach (var item in res)
                                {
                                    item.EndDate = Convert.ToDateTime(txt.Text);

                                    if (salesRep.Company == "Click Energy") //if RepArea was allocated to 'Click Energy' SalesRep change RepName to 'Click Energy Group'
                                    {

                                        item.RepName = "Click Energy Group";
                                        //item.Archived = false; //unarchive RepArea
                                        item.SalesRepId = context.SalesReps.Where(s => s.RepName == "Click Energy Group").Select(s => s.SalesRepId).FirstOrDefault();//change SalesRepId


                                    }
                                    else if (salesRep.Company == "FSR")
                                    {

                                        item.RepName = "FSR Group";
                                        //item.Archived = false;
                                        item.SalesRepId = context.SalesReps.Where(s => s.RepName == "FSR Group").Select(s => s.SalesRepId).FirstOrDefault();//change SalesRepId

                                    }

                                    context.SaveChanges();

                                    dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][5] = txt.Text;
                                    dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)].Delete();
                                    dt.AcceptChanges();

                                }
                            }

                            ViewState["dt"] = dt;
                            //gvListOfCurrentAreas.DataSource = ViewState["dt"];
                            //gvListOfCurrentAreas.DataBind();

                            hdnfldNumRows.Value = dt.Rows.Count.ToString();

                        }

                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            }
        }

        protected void TextChangedEventHandlerStartDate(object sender, EventArgs e) //change StartDate
        {
            TextBox txtBox = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtBox.NamingContainer;

            string cellStartDate = Convert.ToDateTime(row.Cells[4].Text).ToShortDateString();
            string txtBoxStartDate = txtBox.Text;

            DataTable dt = (DataTable)ViewState["dt"];

            if (cellStartDate != txtBoxStartDate) //if EndDate has changed
            {

                using (GISEntities context = new GISEntities())
                {
                    try
                    {
                        Button btn = (Button)row.FindControlRecursive("btnStartDate" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString());

                        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                        List<int> postalCodeIds = new List<int>();

                        for (int i = 0; i <= commandArgs.Length - 1; i++)
                        {
                            postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                        }

                        string txtBoxId = "txtBoxStartDate" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString();
                        TextBox txt = (TextBox)pnlListOfCurrentAreas.FindControlRecursive(txtBoxId); //New EndDate
                        int salesRepId = Convert.ToInt32(row.Cells[2].Text); //SalesRepId currently allocated

                        var res = context.RepAreas.Where(s => (postalCodeIds.Contains(s.RepAreaId)) && (s.Archived == false)
                            && s.SalesRepId == salesRepId).ToList();

                        SalesRep salesRep = context.SalesReps.Where(s => s.SalesRepId == salesRepId).FirstOrDefault();

                        if (res != null)
                        {
                            if (Convert.ToDateTime(txt.Text) >= DateTime.Now.Date) //change StartDate
                            {
                                foreach (var item in res)
                                {
                                    item.StartDate = Convert.ToDateTime(txt.Text);

                                    context.SaveChanges();

                                    dt.Rows[row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)][4] = txt.Text;
                                    lblSavedEndDate.Text = "StartDate changed to " + txt.Text + " for salesRep " + item.RepName.ToString();

                                }
                            }
                            else //StartDate is before present date, display warning
                            {

                                lblSavedEndDate.Text = "StartDate must be greater than or equal to " + DateTime.Now.Date.ToString();
                            }

                            ViewState["dt"] = dt;
                            gvListOfCurrentAreas.DataSource = ViewState["dt"];
                            gvListOfCurrentAreas.DataBind();

                            hdnfldNumRows.Value = dt.Rows.Count.ToString();
                        }

                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            }
        }

        private void RegisterPostBackControl()
        {           
            foreach (GridViewRow row in gvListOfCurrentAreas.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lnkbtn = row.FindControlRecursive("lnkbtn"+ (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as LinkButton;
                    if (lnkbtn != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkbtn);
                    }

                    TextBox txtBox = row.FindControlRecursive("txtBox" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as TextBox;
                    if (txtBox != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBox);
                    }

                    Button btn = row.FindControlRecursive("btn" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as Button;
                    if (btn != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                    }

                    Button btnPostcodes = row.FindControlRecursive("btnPostcodes" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as Button;
                    if (btnPostcodes != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnPostcodes);
                    }

                    TextBox txtBoxStartDate = row.FindControlRecursive("txtBoxStartDate" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as TextBox;
                    if (txtBoxStartDate != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBoxStartDate);
                    }

                    Button btnStartDate = row.FindControlRecursive("btnStartDate" + (row.RowIndex + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize)).ToString()) as Button;
                    if (btnStartDate != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                    }

                    Button btnPostCode = row.FindControlRecursive("btnPostCode") as Button;
                    if (btnPostCode != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnPostCode);
                    }

                }
            }
        }

        private void GetCurrentAreas(ref DataTable dt, List<SalesRep> salesReps)
        {

            using (GISEntities context = new GISEntities())
            {
                tooltipArray.Clear();
                String tooltipContent = String.Empty;
                List<int> salesRepsIds = salesReps.Select(s => s.SalesRepId).ToList();
                DateTime today = DateTime.Now;
                List<int> postalcodeIds = new List<int>();

                using (PostcodesEntities postcodeContext = new PostcodesEntities())
                {
                    string partialPostcode = txtPostCode.Text;

                    var postcodeIds = postcodeContext.PostalCodes.Where(p => p.FullPostcode.StartsWith(partialPostcode)).Select(p => p.PostalCodeID).ToList();


                    var dt1 = context.RepAreas.Where(s => ((s.EndDate > DateTime.Now) &&
                        (salesRepsIds.Contains(s.SalesRepId)) && postcodeIds.Contains(s.PostalCodeID) &&
                        (s.Archived == false))
                        ||
                        ((s.DateAdded < nDaysAgo) &&
                        (salesRepsIds.Contains(s.SalesRepId)) &&
                        (!s.RepName.Equals("Click Energy Group")) && 
                        (!s.RepName.Equals("FSR Group")) &&
                        (s.Archived == false)))
                                    .OrderBy(s => s.StartDate)
                                    .Select(s => new
                                    {
                                        s.RepAreaId,
                                        s.RepName,
                                        s.SalesRepId,
                                        s.PostalCodeID,
                                        startDate = DbFunctions.TruncateTime(s.StartDate),
                                        endDate = DbFunctions.TruncateTime(s.EndDate),
                                        dateAdded = DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond)),
                                        s.Archived
                                    }); //currently allocated RepAreas

                    DataTable dtCount = ConvertToDataTable(dt1.GroupBy(s => new { s.SalesRepId, s.dateAdded }).OrderBy(g => g.Key.SalesRepId)
                        .ThenBy(g => g.Key.dateAdded).Select(s => new { s.Key.SalesRepId, s.Key.dateAdded }).ToList());





                    dtCount.Columns.Add("Postcodes", typeof(string));
                    //dtCount.Columns.Add("Postcode", typeof(string));
                    dtCount.Columns.Add("Premise Count", typeof(int));
                    dtCount.Columns.Add("Keypad", typeof(string));
                    dtCount.Columns.Add("Billpay", typeof(string));
                    dtCount.Columns.Add("Click Customers", typeof(string));
                    dtCount.Columns.Add("Duplicate", typeof(string));                    

                    foreach (DataRow row in dtCount.Rows)
                    {
                        int salesRepId = Convert.ToInt32(row["SalesRepId"]);
                        DateTime dateAdded = Convert.ToDateTime(row["dateAdded"]);

                        postalcodeIds = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false
                            && DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond)) == dateAdded).
                            Select(s => s.PostalCodeID).ToList();

                        int count = postcodeContext.Premises.Where(s => postalcodeIds.Contains(s.PostalCodeID)
                            && (s.DUoSGroup.StartsWith("T01") || s.DUoSGroup.StartsWith("T05"))
                            ).ToList().Count();

                        int countKeypads = postcodeContext.Premises.Where(s => postalcodeIds.Contains(s.PostalCodeID)
                            && s.DUoSGroup.StartsWith("T05")
                            ).ToList().Count();

                        float percentageKeypad = ((float)countKeypads / count) * 100;

                        int countBillpays = postcodeContext.Premises.Where(s => postalcodeIds.Contains(s.PostalCodeID)
                            && s.DUoSGroup.StartsWith("T01")
                            ).ToList().Count();

                        float percentageBillpay = ((float)countBillpays / count) * 100;

                        int countClickCustomers = postcodeContext.Premises.Where(s => postalcodeIds.Contains(s.PostalCodeID)
                            && ((s.DUoSGroup.StartsWith("T01") || s.DUoSGroup.StartsWith("T05"))
                            && s.Live == true
                            )).ToList().Count();

                        float percentageClickCustomers = ((float)countClickCustomers / count) * 100;

                        row["Premise Count"] = count;
                        row["Keypad"] = countKeypads.ToString() + " " + " (" + percentageKeypad.ToString("0.#") + "%)" ;
                        row["Billpay"] = countBillpays.ToString() + " " + " (" + percentageBillpay.ToString("0.#") + "%)";
                        row["Click Customers"] = countClickCustomers.ToString() + " " + " (" + percentageClickCustomers.ToString("0.#") + "%)";

                        var postcodes = postcodeContext.PostalCodes.Where(p => postalcodeIds.Contains(p.PostalCodeID)).Select(p => p.FullPostcode);
                        var postcodeCount = postcodes.Count();

                        tooltipContent = String.Empty;

                        foreach (var item in postcodes.AsEnumerable())
                        {
                            tooltipContent += "<div>" + item.ToString() + "</div>";
                        }

                        tooltipArray.Add(tooltipContent);

                        row["Postcodes"] = postcodeCount;
                        //row["Postcode"] = String.Empty;
                    }


                    dtCount.Columns.Add("Primary Key", typeof(int));

                    for (int i = 0; i < dtCount.Rows.Count; i++)
                    {
                        dtCount.Rows[i]["Primary Key"] = i;                       
                        dtCount.Rows[i]["Duplicate"] = "0";
                    }

                    dtCount.PrimaryKey = new DataColumn[]
                    {
                    dtCount.Columns[8]
                    };

                    dt = ConvertToDataTable(dt1.GroupBy(s => new { s.SalesRepId, s.dateAdded }).OrderBy(g => g.Key.SalesRepId)
                        .ThenBy(g => g.Key.dateAdded).Select(s => s.FirstOrDefault()).ToList());

                    dt.Columns.Add("Row Number", typeof(int));
                    dt.Columns.Add("Primary Key", typeof(int));                   

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Primary Key"] = i;
                        dt.Rows[i]["Row Number"] = i;
                    }

                    dt.PrimaryKey = new DataColumn[]
                    {
                        dt.Columns[9]
                    };

                    dt.Merge(dtCount);

                    //Rename datatable columns
                    dt.Columns["RepName"].ColumnName = "Rep Name";
                    dt.Columns["StartDate"].ColumnName = "Start Date";
                    dt.Columns["EndDate"].ColumnName = "End Date";
                    dt.Columns["dateAdded"].ColumnName = "Date Added";
                    dt.Columns["Premise Count"].ColumnName = "Premises";
                    //dt.Columns.Remove("Primary Key");


                    dt.Columns.Add("Reallocate", typeof(string));
                }
            }
        }

        protected void lnkbtn_OnRowCommand(object sender, GridViewCommandEventArgs e) //List allocated premises
        {
            pnlAllocatedAreas.Visible = true;

            if (e.CommandName == "select")
            {
                using (GISEntities GISContext = new GISEntities())
                {
                    using (PostcodesEntities context = new PostcodesEntities())
                    {
                        try
                        {
                            DataTable dt = new DataTable();

                            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                            List<int> repAreaIds = new List<int>();

                            for (int i = 0; i <= commandArgs.Length - 2; i++)
                            {
                                repAreaIds.Add(Convert.ToInt32(commandArgs[i]));
                            }

                            var postalCodeIds = GISContext.RepAreas.Where(s => repAreaIds.Contains(s.RepAreaId)).Select(s => s.PostalCodeID).ToList();

                            dt = ConvertToDataTable(context.Premises.Where(s => postalCodeIds.Contains(s.PostalCodeID)
                                    && (s.DUoSGroup.StartsWith("T01") || s.DUoSGroup.StartsWith("T05"))).ToList());

                            if (dt != null)
                            {
                                gvAllocatedAreas.DataSource = dt;
                                gvAllocatedAreas.DataBind();
                            }

                            lblAllocatedPremises.Text = dt.Rows.Count.ToString() + " allocated premises found for " + commandArgs[commandArgs.Length - 1].ToString();
                        }
                        catch (Exception Ex)
                        {
                            throw Ex;
                        }
                    }
                }
            }           
        }

        protected void gvListOfCurrentAreas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListOfCurrentAreas.PageIndex = e.NewPageIndex;

            gvListOfCurrentAreas.DataSource = (DataTable)ViewState["dt"];
            gvListOfCurrentAreas.DataBind();

            RegisterPostBackControl();

            DataTable dt = ViewState["dt"] as DataTable;

            hdnfldNumRows.Value = dt.Rows.Count.ToString();

        }

        protected void btnClose3_Click(object sender, EventArgs e)
        {
            /*using (GISEntities context = new GISEntities())
            {
                ArrayList tooltipArrayCopy = new ArrayList();
                Button btn = unclaimBtn;
                GridViewRow row = (GridViewRow)btn.NamingContainer;

                GridView gv = unclaimGv;

                GridView gvNew = new GridView();

                string[] commandArgs = unclaimString.Split(new char[] { ',' });// e.CommandArgument.ToString().Split(new char[] { ',' });
                List<int> postalCodeIds = new List<int>();

                for (int i = 0; i <= commandArgs.Length - 2; i++)
                {
                    postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                }

                int repAreaId = Convert.ToInt32(commandArgs[commandArgs.Length - 1]);

                foreach (GridViewRow gvRow in gv.Rows)
                {
                    if (gvRow != row)
                    {
                        tooltipArrayCopy.Add(tooltipArray[gvRow.RowIndex]);
                    }
                }

                tooltipArray = tooltipArrayCopy;

                List<RepArea> repAreas = context.RepAreas.Where(s => postalCodeIds.Contains(s.PostalCodeID) &&
                    s.RepAreaId == repAreaId).ToList();

                foreach (RepArea repArea in repAreas)
                {
                    repArea.Archived = true;
                }

                context.SaveChanges();

                ((DataTable)ViewState["dt"]).Rows[row.RowIndex].Delete();
                ((DataTable)ViewState["dt"]).AcceptChanges();
                gvListOfCurrentAreas.DataSource = (DataTable)ViewState["dt"];
                gvListOfCurrentAreas.DataBind();

                lblSavedEndDate.Text = (commandArgs.Length - 1).ToString() + " postcode(s) unclaimed.";

            }*/

            AddNewRowToGridView(rowIndexAddNewRow + (gvListOfCurrentAreas.PageIndex * gvListOfCurrentAreas.PageSize));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveSalesRep(rowIndexAddNewRow);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);
        }

        } 
}