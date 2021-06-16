using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Data.Entity;

namespace GISWeb
{
    public partial class SalesRepActions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {
                pnlDay.Visible = true;
                pnlWeek.Visible = false;
                pnlMonth.Visible = false;

                txtdatepickerWeekStartDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtdatepickerWeekEndDate.Attributes.Add("ReadOnly", "ReadOnly");

                using (GISEntities context = new GISEntities())
                {
                    List<SalesRep> res = new List<SalesRep>();
                   
                    if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                    {
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).ToList();
                    }
                    else if (User.IsInRole(@"DOMAIN\Reps"))
                    {
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).ToList();
                    }
                    else
                    {
                        res = context.SalesReps.Where(s => s.Archived == false).Select(s => s).ToList();
                    }

                    ddlSalesReps.DataTextField = "RepName";
                    ddlSalesReps.DataValueField = "SalesRepId";
                    ddlSalesReps.DataSource = res;
                    ddlSalesReps.DataBind();
                }
            }
        }

        protected void gvListOfPremises_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false; //PremiseID Header row not visible
                e.Row.Cells[6].Visible = false; //SalesRepId Header row not visible
            }
            else if(e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false; //PremiseID DataRow not visible
                e.Row.Cells[6].Visible = false; //SalesRepId DataRow row not visible

                using (PostcodesEntities context = new PostcodesEntities()) //find Meter Point Address
                {
                    string premiseId = e.Row.Cells[1].Text;

                    var res = context.Premises.Where(s => s.PremiseID.ToString() == premiseId).Select(s => s.MeterPointAddress).FirstOrDefault();

                    e.Row.Cells[2].Text = res;                   
                }

                using (GISEntities context = new GISEntities()) //find Rep's Name
                {
                    string salesRepId = e.Row.Cells[6].Text;

                    var res = context.SalesReps.Where(s => s.SalesRepId.ToString() == salesRepId).Select(s => s.RepName).FirstOrDefault();

                    e.Row.Cells[0].Text = res;
                }
            }
        }

        protected void ddlTimeSpan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTimeSpan.SelectedItem.Text == "Day")
            {
                pnlDay.Visible = true;
                pnlWeek.Visible = false;
                pnlMonth.Visible = false;
            }
            else if(ddlTimeSpan.SelectedItem.Text == "Week")
            {
                pnlDay.Visible = false;
                pnlWeek.Visible = true;
                pnlMonth.Visible = false;
            }
            else if(ddlTimeSpan.SelectedItem.Text == "Month")
            {
                pnlDay.Visible = false;
                pnlWeek.Visible = false;
                pnlMonth.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (GISEntities context = new GISEntities())
            {
                try
                {
                    DataTable dt1 = new DataTable();

                    int salesRepId = Convert.ToInt32(ddlSalesReps.SelectedItem.Value);

                    if (ddlTimeSpan.SelectedValue == "Day")
                    {
                        DateTime date = (Convert.ToDateTime(txtDayStartDate.Text)).Date;

                        dt1 = ConvertToDataTable(context.Outcomes.Where(c => c.SalesRepId == salesRepId && DbFunctions.TruncateTime(c.ActionDateTime) == date).ToList());
                    }
                    else if (ddlTimeSpan.SelectedValue == "Week")
                    {

                        DateTime startDate = (Convert.ToDateTime(txtdatepickerWeekStartDate.Text)).Date;
                        DateTime endDate = (Convert.ToDateTime(txtdatepickerWeekEndDate.Text)).Date;

                        dt1 = ConvertToDataTable(context.Outcomes.Where(c => c.SalesRepId == salesRepId && DbFunctions.TruncateTime(c.ActionDateTime) >= startDate
                                && DbFunctions.TruncateTime(c.ActionDateTime) <= endDate).ToList());
                    }
                    else if (ddlTimeSpan.SelectedValue == "Month")
                    {
                        int year = (Convert.ToDateTime(txtdatepickerMonthDate.Text)).Year;
                        int month = (Convert.ToDateTime(txtdatepickerMonthDate.Text)).Month;

                        dt1 = ConvertToDataTable(context.Outcomes.Where(c => c.SalesRepId == salesRepId && ((DateTime)c.ActionDateTime).Year == year
                            && ((DateTime)c.ActionDateTime).Month == month).ToList());
                    }

                    if (dt1 != null)
                    {
                        dt1.Columns.Add("Rep Name", typeof(string));
                        dt1.Columns.Add("Meter Point Address", typeof(string));

                        DataView dv = new DataView(dt1);

                        DataTable dt = dv.ToTable(false, "Rep Name", "PremiseId", "Meter Point Address", "ActionDateTime", "ActionStageEnd", "ActionStageCancelReason", "SalesRepId");

                        //Rename datatable columns
                        dt.Columns["ActionDateTime"].ColumnName = "Action Date Time";
                        dt.Columns["ActionStageEnd"].ColumnName = "Action Stage End";
                        dt.Columns["ActionStageCancelReason"].ColumnName = "Action Stage Cancel Reason";

                        Session["gvSearchResults"] = dt;

                        gvSearchResults.DataSource = Session["gvSearchResults"];
                        gvSearchResults.DataBind();

                        lblSearchResults.Text = dt.Rows.Count.ToString() + " actions found."; //number of results message

                        pnlSearchResults.Visible = true;
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
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