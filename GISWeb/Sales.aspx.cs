using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;

namespace GISWeb
{
    public partial class Sales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {

                txtdatepickerWeekStartDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtdatepickerWeekEndDate.Attributes.Add("ReadOnly", "ReadOnly");

                using (GISEntities context = new GISEntities())
                {
                    List<SalesRep> res = new List<SalesRep>();
                    ListItem clickSalesTeam = new ListItem();
                    ListItem fsrSalesTeam = new ListItem();
                    int ddlLastSalesRepId;

                    try
                    {
                        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                        {
                            res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).ToList();

                            SalesRep srClick = new SalesRep(); //Add Click Team item to dropdownlist

                            ddlLastSalesRepId = res.AsEnumerable().OrderByDescending(s => s.SalesRepId).FirstOrDefault().SalesRepId;

                            srClick.SalesRepId = ddlLastSalesRepId += 1;
                            srClick.RepName = "Click Sales Team";

                            res.Add(srClick);
                        }
                        else if (User.IsInRole(@"DOMAIN\Reps"))
                        {
                            res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).ToList();

                            SalesRep srFSR = new SalesRep(); //Add FSR Team item to dropdownlist

                            ddlLastSalesRepId = res.AsEnumerable().OrderByDescending(s => s.SalesRepId).FirstOrDefault().SalesRepId;

                            srFSR.SalesRepId = ddlLastSalesRepId += 1;
                            srFSR.RepName = "FSR Sales Team";

                            res.Add(srFSR);
                        }
                        else
                        {
                            res = context.SalesReps.Where(s => s.Archived == false).Select(s => s).ToList();

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
            using (GISEntities context = new GISEntities())
            {
                try
                {
                    using (PostcodesEntities postcodeContext = new PostcodesEntities())
                    {

                        DataTable dtOutcomes = new DataTable();

                        int salesRepId = Convert.ToInt32(ddlSalesReps.SelectedItem.Value);

                        DateTime startDate = (Convert.ToDateTime(txtdatepickerWeekStartDate.Text)).Date;
                        DateTime endDate = (Convert.ToDateTime(txtdatepickerWeekEndDate.Text)).Date;

                        dtOutcomes = ConvertToDataTable(context.Outcomes.Where(c => c.SalesRepId == salesRepId && DbFunctions.TruncateTime(c.ActionDateTime) >= startDate
                                && DbFunctions.TruncateTime(c.ActionDateTime) <= endDate
                                && c.Sale == true
                                && c.Archived == false                               
                                ).ToList()); //search for premiseIds that exist in Postcodes.Premises table

                        if (dtOutcomes != null)
                        {
                            for(int i = dtOutcomes.Rows.Count - 1; i >= 0; i--) //remove Outcomes with incorrect PremiseIds from 
                            {
                                DataRow row = dtOutcomes.Rows[i];
                                int premiseId = Convert.ToInt32(row["PremiseId"]);
                                int outcomeId = Convert.ToInt32(row["OutcomeId"]);

                                var res = postcodeContext.Premises.Where(s => premiseId.Equals(s.PremiseID)).Select(s => s).FirstOrDefault(); //find record in Premise table with Outcome PremiseId

                                if(res == null)
                                {
                                    row.Delete();

                                    Outcome incorrectPremise = context.Outcomes.Where(c => c.OutcomeId == outcomeId).FirstOrDefault();

                                    if (incorrectPremise != null) //Archive outcome record with incorrect PremiseId
                                    {
                                        incorrectPremise.Archived = true;
                                        incorrectPremise.ArchivedDateTime = DateTime.Now;
                                        context.SaveChanges();
                                    }
                                }
                            }

                            dtOutcomes.AcceptChanges(); //Save changes to datatable

                            dtOutcomes.Columns.Add("RepName", typeof(string));
                            dtOutcomes.Columns.Add("MPRN", typeof(string));
                            dtOutcomes.Columns.Add("Meter Point Address", typeof(string));
                            dtOutcomes.Columns.Add("Sale Type", typeof(string));

                            DataView dv = new DataView(dtOutcomes);

                            DataTable dt = dv.ToTable(false, "RepName", "PremiseId", "MPRN", "Meter Point Address", "ActionDateTime",
                                "SaleBillPay", "SaleKeypad", "Sale Type", "SalesRepId", "OutcomeId");

                            //Rename datatable columns
                            dt.Columns["ActionDateTime"].ColumnName = "Action Date Time";
                            dt.Columns["SaleBillPay"].ColumnName = "BillPay";
                            dt.Columns["SaleKeypad"].ColumnName = "KeyPad";

                            Session["gvSearchResults"] = dt;

                            gvSearchResults.DataSource = Session["gvSearchResults"];
                            gvSearchResults.DataBind();

                            lblSearchResults.Text = gvSearchResults.Rows.Count.ToString() + " sales found for " + ddlSalesReps.SelectedItem.Text; //number of results message

                            pnlSearchResults.Visible = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
        }

        protected void gvSearchResults_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvListOfPremises_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = Session["gvSearchResults"] as DataTable;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //RepName Header row not visible
                e.Row.Cells[5].Visible = false; // Header row not visible
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false; //OutcomeId columns
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //RepName DataRow not visible
                e.Row.Cells[5].Visible = false; // DataRow row not visible
                e.Row.Cells[6].Visible = false; // DataRow row not visible
                e.Row.Cells[8].Visible = false; // DataRow row not visible
                e.Row.Cells[9].Visible = false; //OutcomesId column

                using (PostcodesEntities context = new PostcodesEntities()) //find Meter Point Address
                {
                    int premiseId = Convert.ToInt32(e.Row.Cells[1].Text);

                    var res = context.Premises.Where(s => premiseId.Equals(s.PremiseID)).Select(s => s).FirstOrDefault();

                    if (res != null)
                    { 
                        e.Row.Cells[3].Text = res.MeterPointAddress;
                        e.Row.Cells[2].Text = res.MPRN;
                    }
                }

                using (GISEntities context = new GISEntities()) //find Rep's Name
                {
                    int salesRepId = Convert.ToInt32(e.Row.Cells[8].Text);
                  
                    var res = context.SalesReps.Where(s => s.SalesRepId == salesRepId).Select(s => s.RepName).FirstOrDefault();

                    if (res != null)
                    {
                        e.Row.Cells[0].Text = res;
                    }
                }

                if(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "BillPay")) == "True")
                {
                    e.Row.Cells[7].Text = "BillPay";
                }
                else if(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "KeyPad")) == "True")
                {
                    e.Row.Cells[7].Text = "KeyPad";
                }
                else
                {
                    e.Row.Cells[7].Text = "";
                }
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["gvSearchResults"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

                gvSearchResults.DataSource = dt;
                gvSearchResults.DataBind();
            }
        }

        protected string GetSortDirection(string column)
        {
            string sortDirection = "ASC";

            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvListOfPremises_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchResults.PageIndex = e.NewPageIndex;
            gvSearchResults.DataSource = Session["gvSearchResults"];
            gvSearchResults.DataBind();
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
                catch (Exception Ex)
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
    }
}