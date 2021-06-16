using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;

namespace GISWeb
{
    public partial class OutComes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {
                pnlListOfOutcomes.Visible = false;
                
                using (GISEntities context = new GISEntities())
                {
                    ListItem firstItem = ddlSalesReps.Items[0]; //Retrieve the first item in ddl "Please select an item"
                    ddlSalesReps.Items.Clear();
                    ddlSalesReps.Items.Add(firstItem);

                    List<SalesRep> res = new List<SalesRep>();

                    if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                    {
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).OrderBy(s => s.RepName).Select(s => s).ToList();
                    }
                    else if (User.IsInRole(@"DOMAIN\Reps"))
                    {
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).OrderBy(s => s.RepName).Select(s => s).ToList();
                    }
                    else
                    {
                        res = context.SalesReps.Where(s => s.Archived == false).OrderBy(s => s.RepName).Select(s => s).ToList();
                    }

                    ddlSalesReps.DataTextField = "RepName";
                    ddlSalesReps.DataValueField = "SalesRepId";
                    ddlSalesReps.DataSource = res;
                    ddlSalesReps.DataBind();
                }
            }
        }

        protected void ddlSalesReps_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();

            using (GISEntities context = new GISEntities())
            {
                try
                {
                    int salesRepId = Convert.ToInt32(ddlSalesReps.SelectedValue);

                    dt1 = ConvertToDataTable(context.PremisesBySalesRepIdMostRecentOutcomeCurrentArea(salesRepId).OrderByDescending(s => s.ActionDateTime).ToList());                   

                    if (dt1 != null)
                    {
                        dt1.Columns.Add("Rep Name", typeof(string));

                        DataView dv = new DataView(dt1);

                        DataTable dt = dv.ToTable(false, "Rep Name", "MeterPointAddress", "ActionDateTime", "ActionStageEnd", "ActionStageCancelReason", "SalesRepId");

                        //Rename datatable columns
                        dt.Columns["MeterPointAddress"].ColumnName = "Meter Point Address";
                        dt.Columns["ActionDateTime"].ColumnName = "Action Date Time";
                        dt.Columns["ActionStageEnd"].ColumnName = "Action Stage End";
                        dt.Columns["ActionStageCancelReason"].ColumnName = "Action Stage Cancel Reason";

                        Session["gvListOfOutcomes"] = dt;

                        gvListOfOutcomes.DataSource = dt;
                        gvListOfOutcomes.DataBind();

                        lblSearchResults.Text = dt.Rows.Count.ToString() + " premises found for " + ddlSalesReps.SelectedItem.Text;

                        pnlListOfOutcomes.Visible = true;
                    }

                }
                catch(Exception Ex)
                {
                    throw Ex;
                }
            }
        }

        protected void gvListOfOutcomes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;

                using (GISEntities context = new GISEntities())
                {
                    string salesRepId = e.Row.Cells[5].Text;

                    var res = context.SalesReps.Where(s => s.SalesRepId.ToString() == salesRepId).Select(s => s.RepName).FirstOrDefault();

                    e.Row.Cells[0].Text = res;
                }
            }
        }

        protected void gvListOfOutcomes_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["gvListOfOutcomes"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

                gvListOfOutcomes.DataSource = dt;
                gvListOfOutcomes.DataBind();
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

        protected void gvListOfOutcomes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListOfOutcomes.PageIndex = e.NewPageIndex;
            gvListOfOutcomes.DataSource = Session["gvListOfOutcomes"];
            gvListOfOutcomes.DataBind();
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