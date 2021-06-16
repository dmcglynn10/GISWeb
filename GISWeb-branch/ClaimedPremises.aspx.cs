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
    public partial class ClaimedPremises : System.Web.UI.Page
    {
        static List<SalesRep> salesReps = new List<SalesRep>();
        static ArrayList tooltipArray = new ArrayList();
        static DateTime nDaysAgo = DateTime.Now.AddDays(-60); //include RepAreas with DateAdded more than n days old
        static Button unclaimBtn = new Button();
        static string unclaimString = String.Empty;
        static GridView unclaimGv = new GridView();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

                gvListOfCurrentAreas.DataSource = ViewState["dt"];
                gvListOfCurrentAreas.DataBind();

                RegisterPostBackControl();

            }
            else
            {
                pnlListOfCurrentAreas.Visible = false;
                pnlAllocatedAreas.Visible = false;
                pnlSearchResults.Visible = false;

                using (GISEntities context = new GISEntities())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                        {
                            salesReps = context.SalesReps.Where(s => s.Company == "Click Energy" && s.Archived == false).Select(s => s).ToList();

                            GetCurrentAreas(ref dt, salesReps);

                        }
                        else if (User.IsInRole(@"DOMAIN\Reps"))
                        {
                            salesReps = context.SalesReps.Where(s => s.Company == "FSR" && s.Archived == false).Select(s => s).ToList();

                            GetCurrentAreas(ref dt, salesReps);
                        }
                        else
                        {
                            salesReps = context.SalesReps.Where(s => s.Archived == false).Select(s => s).ToList();

                            GetCurrentAreas(ref dt, salesReps);
                        }

                        if (dt != null)
                        {
                            gvListOfCurrentAreas.DataSource = dt;
                            gvListOfCurrentAreas.DataBind();
                        }

                        ViewState["dt"] = dt;

                        pnlListOfCurrentAreas.Visible = true;

                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine("{0} Exception caught:", Ex);
                    }
                }
            }
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
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;

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
                            Select(s => s.PostalCodeID).ToList();
                    }
                    else
                    {
                        repAreas = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            s.Archived == false && dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s).ToList();

                        result = context.RepAreas.Where(s => s.SalesRepId == salesRepId && s.Archived == false &&
                            s.Archived == false && dateAdded == DbFunctions.AddMilliseconds(s.DateAdded, -(s.DateAdded.Millisecond))).
                            Select(s => s.PostalCodeID).ToList();
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



                    PlaceHolder phPostcodes = new PlaceHolder();
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

                    phPostcodes.Controls.Add(btnPostcodes);


                    PlaceHolder phUnclaim = new PlaceHolder();
                    e.Row.Cells[11].Controls.Add(phUnclaim);

                    //Unclaim Button
                    Button btnUnclaim = new Button();
                    btnUnclaim.Text = "Unclaim";
                    btnUnclaim.ID = "btnUnclaim" + e.Row.RowIndex;
                    btnUnclaim.ClientIDMode = ClientIDMode.Static;
                    btnUnclaim.Attributes["class"] = "btn btn-sm btn-primary unclaim";
                    btnUnclaim.ValidationGroup = "ValGroup1";
                    btnUnclaim.CommandName = "Unclaim";
                    btnUnclaim.CommandArgument = res + " , " + e.Row.Cells[0].Text.ToString(); //append SalesRepId  to commandArgument
                    btnUnclaim.Visible = true;
                    
                    var div1 = new HtmlGenericControl("div");

                    phUnclaim.Controls.Add(btnUnclaim);
                                        
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

                    lnkbtn.ID = "lnkbtn" + e.Row.RowIndex;
                    lnkbtn.ClientIDMode = ClientIDMode.Static;
                    //lnkbtn.Click += new EventHandler(lnkbtn_Click);
                    lnkbtn.CommandName = "select";
                    lnkbtn.CommandArgument = res + " , " + e.Row.Cells[1].Text.ToString(); //append RepName onto commandArgument
                    ph.Controls.Add(lnkbtn);

                    PlaceHolder phEndDate = new PlaceHolder();

                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkbtn);
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBox);
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnUnclaim);
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnPostcodes);
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnClose3);
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
            string salesRep = ((DropDownList)sender).SelectedItem.Text; //SalesRep area is reallocated to


            LinkButton lnkbtn = row.FindControlRecursive("lnkbtn" + row.RowIndex) as LinkButton;
            lnkbtn.Text = salesRep;  //Change linkbutton text          

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

                    var res = context.RepAreas.Where(s => (postalCodeIds.Contains(s.PostalCodeID)) && (s.Archived == false)
                        && s.SalesRepId == salesRepId).ToList();

                    if (res != null) //Update RepName and SalesRepId in RepAreas table
                    {
                        foreach (var item in res)
                        {
                            item.RepName = salesRep;
                            item.SalesRepId = Convert.ToInt32(ddlReallocate.SelectedValue);
                            item.DateAdded = DateTime.Now;

                            row.Cells[1].Text = ddlReallocate.SelectedItem.Text; //change gridview row RepName
                            row.Cells[2].Text = ddlReallocate.SelectedValue; //change gridview row SalesRepId
                            row.Cells[6].Text = item.DateAdded.ToString();//change gridview DateAdded

                            lblSavedEndDate.Text = "SalesRep changed from " + dt.Rows[row.RowIndex][1].ToString() + " to " + salesRep;

                        }

                        context.SaveChanges();

                        dt.Rows[row.RowIndex][1] = ddlReallocate.SelectedItem.Text; //update datatable RepName
                        dt.Rows[row.RowIndex][2] = ddlReallocate.SelectedValue;//update datatable SalesRepId
                        dt.Rows[row.RowIndex][6] = row.Cells[6].Text; //update datatable DateAdded
                        ViewState["dt"] = dt;

                        gvListOfCurrentAreas.DataSource = ViewState["dt"];
                        gvListOfCurrentAreas.DataBind(); //Bind datatable to gridview

                        ddlReallocate.SelectedValue = "0"; //Reset ddlReallocate to 'Select item'
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
                        Button btn = (Button)row.FindControlRecursive("btn" + row.RowIndex.ToString());

                        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                        List<int> postalCodeIds = new List<int>();

                        for (int i = 0; i <= commandArgs.Length - 1; i++)
                        {
                            postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                        }

                        string txtBoxId = "txtBox" + row.RowIndex.ToString();
                        TextBox txt = (TextBox)pnlListOfCurrentAreas.FindControlRecursive(txtBoxId); //New EndDate
                        int salesRepId = Convert.ToInt32(row.Cells[2].Text); //SalesRepId currently allocated

                        var res = context.RepAreas.Where(s => (postalCodeIds.Contains(s.PostalCodeID)) && (s.Archived == false)
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

                                    dt.Rows[row.RowIndex][5] = txt.Text;
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

                                    dt.Rows[row.RowIndex][5] = txt.Text;
                                    dt.Rows[row.RowIndex].Delete();
                                    dt.AcceptChanges();

                                }
                            }

                            ViewState["dt"] = dt;
                            gvListOfCurrentAreas.DataSource = ViewState["dt"];
                            gvListOfCurrentAreas.DataBind();
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
                    LinkButton lnkbtn = row.FindControlRecursive("lnkbtn" + row.RowIndex) as LinkButton;
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkbtn);

                    //TextBox txtBox = row.FindControlRecursive("txtBox" + row.RowIndex) as TextBox;
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(txtBox);

                    //Button btn = row.FindControlRecursive("btnUnclaim" + row.RowIndex) as Button;
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);

                    Button btnPostcodes = row.FindControlRecursive("btnPostcodes" + row.RowIndex) as Button;
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnPostcodes);

                    //Button btnSubmitModal = row.FindControlRecursive("btnSubmitModal" + row.RowIndex) as Button;
                    //ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmitModal);
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

                var dt1 = context.RepAreas.Where(s =>
                    (salesRepsIds.Contains(s.SalesRepId)) &&
                    (s.Archived == false))
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


                using (PostcodesEntities postcodeContext = new PostcodesEntities())
                {
                    dtCount.Columns.Add("Postcode Count", typeof(int));
                    dtCount.Columns.Add("Premise Count", typeof(int));

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

                        row["Premise Count"] = count;

                        var postcodes = postcodeContext.PostalCodes.Where(p => postalcodeIds.Contains(p.PostalCodeID)).Select(p => p.FullPostcode);
                        var postcodeCount = postcodes.Count();

                        tooltipContent = String.Empty;

                        foreach (var item in postcodes.AsEnumerable())
                        {
                            tooltipContent += "<div>" + item.ToString() + "</div>";
                        }

                        tooltipArray.Add(tooltipContent);

                        row["Postcode Count"] = postcodeCount;
                    }
                }

                dtCount.Columns.Add("Primary Key", typeof(int));

                for (int i = 0; i < dtCount.Rows.Count; i++)
                {
                    dtCount.Rows[i]["Primary Key"] = i;
                }

                dtCount.PrimaryKey = new DataColumn[]
                {
                    dtCount.Columns[4]
                };

                dt = ConvertToDataTable(dt1.GroupBy(s => new { s.SalesRepId, s.dateAdded }).OrderBy(g => g.Key.SalesRepId)
                    .ThenBy(g => g.Key.dateAdded).Select(s => s.FirstOrDefault()).ToList());

                dt.Columns.Add("Primary Key", typeof(int));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Primary Key"] = i;
                }

                dt.PrimaryKey = new DataColumn[]
                {
                    dt.Columns[8]
                };

                dt.Merge(dtCount);

                //Rename datatable columns
                dt.Columns["RepName"].ColumnName = "Rep Name";
                dt.Columns["StartDate"].ColumnName = "Start Date";
                dt.Columns["EndDate"].ColumnName = "End Date";
                dt.Columns["dateAdded"].ColumnName = "Date Added";
                dt.Columns["Premise Count"].ColumnName = "No. Premises";

                dt.Columns.Add("Unclaim", typeof(int));
            }
        }

        protected void lnkbtn_OnRowCommand(object sender, GridViewCommandEventArgs e) //List allocated premises
        {
            pnlAllocatedAreas.Visible = true;

            if (e.CommandName == "select")
            {
                using (PostcodesEntities context = new PostcodesEntities())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                        List<int> postalCodeIds = new List<int>();

                        for (int i = 0; i <= commandArgs.Length - 2; i++)
                        {
                            postalCodeIds.Add(Convert.ToInt32(commandArgs[i]));
                        }

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
            else if(e.CommandName == "Unclaim")
            {
                unclaimBtn = (Button)e.CommandSource;
                unclaimGv = (GridView)sender;
                unclaimString = e.CommandArgument.ToString();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#myModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

            }
        }

        protected void btnPostCode_Click(object sender, EventArgs e)
        {
            ArrayList tooltipArrayCopy = new ArrayList();
            string postcode = txtPostCode.Text.ToLower();

            if (ViewState["dt"] == null)
                return;

            DataTable dt = (DataTable)ViewState["dt"] ;

            DataTable dtNew = dt.Clone();

           

            foreach (DataRow row in dt.Rows)
            {               
                string[] commandArgs = tooltipArray[dt.Rows.IndexOf(row)].ToString().Split(new string[] { "<div>","</div>" }, StringSplitOptions.None);

                List<string> tooltipPostcodes = new List<string>();

                for (int i = 0; i <= commandArgs.Length - 1; i++)
                {
                    if (commandArgs[i] != "")
                    {
                        tooltipPostcodes.Add(commandArgs[i].ToLower());                       
                    }
                }

                if( tooltipPostcodes.Contains(postcode))
                {
                    dtNew.Rows.Add(row.ItemArray);
                    tooltipArrayCopy.Add(tooltipArray[dt.Rows.IndexOf(row)]);
                }
            }
            
            tooltipArray = tooltipArrayCopy;

            ViewState["dt"] = dtNew;
            gvListOfCurrentAreas.DataSource = ViewState["dt"];
            gvListOfCurrentAreas.DataBind();

            pnlSearchResults.Visible = true;

            lblSavedEndDate.Text = dtNew.Rows.Count.ToString() + " results found.";

        }

        protected void gvSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClaimedPremises.aspx");
        }

        protected void gvListOfCurrentAreas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnClose3_Click(object sender, EventArgs e)
        {           
            using (GISEntities context = new GISEntities())
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

            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

        }
    }
}