using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;

namespace GISWeb
{
    public partial class KPI : System.Web.UI.Page
    {
        static string billpayCount;
        static string keypadCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                pnlKPIResults.Visible = false;

                gvKPIResults.DataSource = ViewState["dtCount"];
                gvKPIResults.ShowHeader = false;
                gvKPIResults.DataBind();

                RegisterPostBackControl();
            }
            else
            {
                pnlKPIResults.Visible = false;

                pnlDay.Visible = true;
                pnlWeek.Visible = false;

                txtdatepickerWeekStartDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtdatepickerWeekEndDate.Attributes.Add("ReadOnly", "ReadOnly");

                using (GISEntities context = new GISEntities())
                {
                    try
                    {
                        //Find SalesRepsIds
                        List<SalesRep> res = new List<SalesRep>();

                        //
                        ListItem clickSalesTeam = new ListItem();
                        ListItem fsrSalesTeam = new ListItem();
                        int ddlLastSalesRepId;

                        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                        {
                            res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).OrderBy(s => s.RepName).ToList();

                            SalesRep srClick = new SalesRep(); //Add Click Team item to dropdownlist

                            ddlLastSalesRepId = res.AsEnumerable().OrderByDescending(s => s.SalesRepId).FirstOrDefault().SalesRepId;

                            srClick.SalesRepId = ddlLastSalesRepId += 1;
                            srClick.RepName = "Click Sales Team";

                            res.Add(srClick);
                        }
                        else if (User.IsInRole(@"DOMAIN\Reps"))
                        {

                            res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).OrderBy(s => s.RepName).ToList();
                           
                            SalesRep srFSR = new SalesRep(); //Add FSR Team item to dropdownlist

                            ddlLastSalesRepId = res.AsEnumerable().OrderByDescending(s => s.SalesRepId).FirstOrDefault().SalesRepId;

                            srFSR.SalesRepId = ddlLastSalesRepId += 1;
                            srFSR.RepName = "FSR Sales Team";
     
                            res.Add(srFSR);
                        }
                        else
                        {
                            res = context.SalesReps.Where(s => s.Archived == false).Select(s => s).OrderBy(s => s.RepName).ToList();

                            SalesRep srClick = new SalesRep(); //Add Click Energy and FSR Team items to dropdownlist
                            SalesRep srFSR = new SalesRep();

                            ddlLastSalesRepId = res.AsEnumerable().OrderByDescending(s => s.SalesRepId).FirstOrDefault().SalesRepId;

                            srClick.SalesRepId = ddlLastSalesRepId += 1;
                            srClick.RepName = "Click Sales Team";

                            srFSR.SalesRepId = ddlLastSalesRepId += 1;
                            srFSR.RepName = "FSR Sales Team";

                            res.Add(srClick);
                            res.Add(srFSR);
                        }

                        ddlSalesReps.DataTextField = "RepName";
                        ddlSalesReps.DataValueField = "SalesRepId";
                        ddlSalesReps.DataSource = res;
                        ddlSalesReps.DataBind();                                               

                    }
                    catch(Exception Ex)
                    {
                        throw Ex;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int salesRepId = int.MinValue; 
            string timeSpan = String.Empty;
            string company = String.Empty;            
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;

            pnlKPIResults.Visible = true;
            DataTable dt = new DataTable();                       

            using (GISEntities context = new GISEntities())
            {
                try
                {
                    timeSpan = ddlTimeSpan.SelectedValue; ;

                    if (timeSpan == "Day")   //KPI table title
                    {
                        startDate = (Convert.ToDateTime(txtDayStartDate.Text)).Date;
                        lblDate.Text = "KPI " + startDate.ToString("dddd, MMMM d, yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    else if (timeSpan == "Week")
                    {
                        startDate = (Convert.ToDateTime(txtdatepickerWeekStartDate.Text)).Date;
                        endDate = (Convert.ToDateTime(txtdatepickerWeekEndDate.Text)).Date;

                        lblDate.Text = "KPI " + startDate.ToString("dddd, MMMM d, yyyy", CultureInfo.CreateSpecificCulture("en-US")) + " - " +
                             endDate.ToString("dddd, MMMM d, yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                    }

                    //Find actions
                    if (ddlSalesReps.SelectedItem.Text == "Click Sales Team" || ddlSalesReps.SelectedItem.Text == "FSR Sales Team")
                    {
                        if(ddlSalesReps.SelectedItem.Text == "Click Sales Team")
                        {
                            company = "Click Energy";
                        }
                        else if(ddlSalesReps.SelectedItem.Text == "FSR Sales Team")
                        {
                            company = "FSR";
                        }

                        List<int> salesRepIdsNoNulls = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == company)).Select(s => s.SalesRepId).ToList();
                        List<int?> salesRepIds = salesRepIdsNoNulls.Select(s => (int?)s).ToList();
                        dt = SalesTeamKPI(startDate, endDate, timeSpan, salesRepIds);
                    }
                    else
                    {
                        salesRepId = Convert.ToInt32(ddlSalesReps.SelectedItem.Value);
                        dt = SalesRepKPI(startDate, endDate, timeSpan, salesRepId);
                    }                                                    

                    //Rename datatable columns
                    dt.Columns["DoorKnocked"].ColumnName = "Doors Knocked";
                    dt.Columns["ContactMade"].ColumnName = "Contact Made";
                    dt.Columns["DecisionMakerMet"].ColumnName = "Decision Maker Met";
                    dt.Columns["DecisionMakerPresentedTo"].ColumnName = "Decision Maker Presented To";
                    dt.Columns["SaleBillPay"].ColumnName = "BillPay";
                    dt.Columns["SaleKeypad"].ColumnName = "KeyPad";
                    dt.Columns["Sale"].ColumnName = "Sales";

                    //Count each action

                    //Pivot datatable dt
                    DataTable dtCount = new DataTable();

                    for (int i = 0; i < 2; i++)
                    {
                        dtCount.Columns.Add();
                    }

                    foreach (var item in dt.Columns)
                    {
                        DataRow dr = dtCount.NewRow();
                        dr[0] = item.ToString();
                        dr[1] = dt.Select("[" + item.ToString() + "]" + " = True").Count();
                        dtCount.Rows.Add(dr);
                    }

                    billpayCount = dtCount.Rows[4][1].ToString();
                    keypadCount = dtCount.Rows[5][1].ToString();

                    ViewState["dtCount"] = dtCount;

                    gvKPIResults.DataSource = dtCount;
                    gvKPIResults.ShowHeader = false;
                    gvKPIResults.DataBind();

                    gvKPIResults.Rows[4].Visible = false;
                    gvKPIResults.Rows[5].Visible = false;
                    
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
        }

        protected void gvKPIResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Header)
            {

            }
            else if(e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 6) //Put billpaycount and keypadcount into tooltip
                {
                    /*if(e.Row.Cells[6] == true)
                    {

                    }
                    else if()
                    {

                    }
                    else
                    {

                    }*/
                    PlaceHolder ph = new PlaceHolder();
                    e.Row.Cells[1].Controls.Add(ph);
                    Button btn = new Button();
                    btn.ID = "btn" + e.Row.RowIndex;
                    btn.ClientIDMode = ClientIDMode.Static;
                    btn.Attributes["class"] = "btn btn-primary";
                    btn.Attributes["data-toggle"] = "popover";
                    btn.Attributes["data-placement"] = "right";
                    btn.Attributes["data-html"] = "true";
                    btn.Attributes["data-content"] = "<div>Billpay "+billpayCount+"</div><div> Keypad "+keypadCount+"</div>";
                    btn.OnClientClick = "return false";
                    btn.Text = e.Row.Cells[1].Text;
                    ph.Controls.Add(btn);

                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                }
                else
                {
                    e.Row.Cells[1].Attributes.Add("style", "padding-left:20px;");
                }
            }
        }

        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvKPIResults.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowIndex == 6)
                    {
                        Button btn = row.FindControlRecursive("btn" + row.RowIndex) as Button;
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                    }
                }
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

        private DataTable SalesTeamKPI(DateTime startDate, DateTime endDate, string timeSpan, List<int?> salesRepIds)
        {           

            using (GISEntities context = new GISEntities())
            {
                DataTable dt = new DataTable();

                try
                {
                                     
                    if (timeSpan == "Day")
                    {
                        dt = ConvertToDataTable(context.Outcomes.Where(s => salesRepIds.Contains(s.SalesRepId) 
                                && DbFunctions.TruncateTime(s.ActionDateTime) == startDate
                                && s.Archived == false).Select(
                                                                s => new
                                                                {
                                                                    s.DoorKnocked,
                                                                    s.ContactMade,
                                                                    s.DecisionMakerMet,
                                                                    s.DecisionMakerPresentedTo,
                                                                    s.SaleBillPay,
                                                                    s.SaleKeypad,
                                                                    s.Sale
                                                                }).ToList());
                    }
                    else //Timespan is week
                    {

                        dt = ConvertToDataTable(context.Outcomes.Where(s => salesRepIds.Contains(s.SalesRepId) && DbFunctions.TruncateTime(s.ActionDateTime) >= startDate
                                && DbFunctions.TruncateTime(s.ActionDateTime) <= endDate 
                                && s.Archived == false).Select(
                                                                s => new
                                                                {
                                                                    s.DoorKnocked,
                                                                    s.ContactMade,
                                                                    s.DecisionMakerMet,
                                                                    s.DecisionMakerPresentedTo,
                                                                    s.SaleBillPay,
                                                                    s.SaleKeypad,
                                                                    s.Sale
                                                                }).ToList());
}

                    return dt;
                }
                catch(Exception Ex)
                {
                    throw Ex;
                }
            }
        }

        private DataTable SalesRepKPI(DateTime startDate, DateTime endDate, string timeSpan, int? salesRepId)
        {

            using (GISEntities context = new GISEntities())
            {
                DataTable dt = new DataTable();

                try
                {
                    
                    if (timeSpan == "Day")
                    {
                        dt = ConvertToDataTable(context.Outcomes.Where(s => (s.SalesRepId == salesRepId) && DbFunctions.TruncateTime(s.ActionDateTime) == startDate
                                && s.Archived == false).Select(
                                                                s => new
                                                                {
                                                                    s.DoorKnocked,
                                                                    s.ContactMade,
                                                                    s.DecisionMakerMet,
                                                                    s.DecisionMakerPresentedTo,
                                                                    s.SaleBillPay,
                                                                    s.SaleKeypad,
                                                                    s.Sale
                                                                }).ToList());
                    }
                    else //Timespan is Week
                    {                        

                        dt = ConvertToDataTable(context.Outcomes.Where(s => (s.SalesRepId == salesRepId) && DbFunctions.TruncateTime(s.ActionDateTime) >= startDate
                                && DbFunctions.TruncateTime(s.ActionDateTime) <= endDate
                                && s.Archived == false).Select(
                                                                s => new
                                                                {
                                                                    s.DoorKnocked,
                                                                    s.ContactMade,
                                                                    s.DecisionMakerMet,
                                                                    s.DecisionMakerPresentedTo,
                                                                    s.SaleBillPay,
                                                                    s.SaleKeypad,
                                                                    s.Sale
                                                                }).ToList());
                    }

                    return dt;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
        }

        protected void ddlTimeSpan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTimeSpan.SelectedItem.Text == "Day")
            {
                pnlDay.Visible = true;
                pnlWeek.Visible = false;
            }
            else if (ddlTimeSpan.SelectedItem.Text == "Week")
            {
                pnlDay.Visible = false;
                pnlWeek.Visible = true;
            }

        }
    }
}