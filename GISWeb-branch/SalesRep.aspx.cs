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
    public partial class SalesRepPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {
                pnlGridView.Visible = true;
                pnlEditDetails.Visible = false;
                pnlCreateNewSalesRep.Visible = false;

                ViewState["ViewArchivedReps"] = false;


                DataTable dt = new DataTable();
                dt = ViewReps(false);

                if (dt != null)
                {
                    gvListOfSalesReps.DataSource = dt;
                    gvListOfSalesReps.DataBind();
                }
                Session["gvListOfSalesReps"] = dt;

                //using (GISEntities context = new GISEntities())
                //{
                //    try
                //    {
                //        DataTable dt = new DataTable();

                //        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                //        {
                //            dt = ConvertToDataTable(context.SalesReps.Where(s => s.Company == "Click Energy").Select(s => s).OrderBy(s => s.RepName).ToList());
                //            //res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).OrderBy(s => s.RepName).ToList();

                //        }
                //        else if (User.IsInRole(@"DOMAIN\Reps"))
                //        {
                //            dt = ConvertToDataTable(context.SalesReps.Where(s => s.Company == "FSR").Select(s => s).OrderBy(s => s.RepName).ToList());
                //        }
                //        else
                //        {
                //            dt = ConvertToDataTable(context.SalesReps.Select(s => s).OrderBy(s => s.RepName).ToList());
                //        }


                //        if (dt != null)
                //        {
                //            gvListOfSalesReps.DataSource = dt;
                //            gvListOfSalesReps.DataBind();

                //            Session["gvListOfSalesReps"] = dt;                           
                //        }                                          
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("{0} Exception caught.", ex);
                //    }
                //}
            }
        }

        private DataTable ViewReps(bool viewArchived)
        {
            DataTable dt = new DataTable();

            using (GISEntities context = new GISEntities())
            {
                try
                {
                    if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                    {
                        dt = ConvertToDataTable(context.SalesReps.Where(s => (s.Archived == viewArchived) && (s.Company == "Click Energy")).Select(s => s).OrderBy(s => s.RepName).ToList());
                    }
                    else if (User.IsInRole(@"DOMAIN\Reps"))
                    {
                        dt = ConvertToDataTable(context.SalesReps.Where(s => (s.Archived == viewArchived) && s.Company == "FSR").Select(s => s).OrderBy(s => s.RepName).ToList());
                    }
                    else
                    {
                        dt = ConvertToDataTable(context.SalesReps.Where(s => (s.Archived == viewArchived)).Select(s => s).OrderBy(s => s.RepName).ToList());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} Exception caught.", ex);
                }
            }

            return dt;

        }

        protected void gvListOfSalesReps_RowEditing(object sender, GridViewEditEventArgs e)
        {
            pnlGridView.Visible = false;
            pnlEditDetails.Visible = true;

            int SalesRepId = Convert.ToInt32(gvListOfSalesReps.Rows[e.NewEditIndex].Cells[0].Text);


            using (GISEntities context = new GISEntities())
            {
                try
                {
                    SalesRep r = context.SalesReps.Where(s => s.SalesRepId == SalesRepId).FirstOrDefault();

                    lblSalesRepId.Text = r.SalesRepId.ToString();
                    if (r.RepName != null) { txtRepName.Text = r.RepName.ToString(); }
                    if (r.Company != null) { ddlCompany.SelectedValue = r.Company.ToString(); }
                    if (r.EmailAddress != null) { txtEmailAddress.Text = r.EmailAddress.ToString(); }
                    if (r.Archived != null) { ddlArchived.SelectedValue = (Convert.ToByte(r.Archived)).ToString(); }

                    if (User.IsInRole(@"DOMAIN\RepsClickGroup") || User.IsInRole(@"DOMAIN\Reps"))
                    {
                        ddlCompany.Enabled = false;
                        ddlCompany.CssClass = "form-control";
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("{0} Exception caught.", Ex);
                }
            }
        }

        protected void gvListOfSalesReps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells[4].Text == "True")
            {
                //e.Row.Cells[4].Text = "Yes";
                e.Row.Visible = Convert.ToBoolean(ViewState["ViewArchivedReps"]);
            }
        }

        protected void gvListOfSalesReps_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["gvListOfSalesReps"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

                gvListOfSalesReps.DataSource = dt;
                gvListOfSalesReps.DataBind();
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

        protected void btnArchivedReps_Click(object sender, EventArgs e)
        {
            if (btnArchivedRepsTop.Text == "Hide Archived Reps")
            {
                btnArchivedRepsTop.Text = "Show Archived Reps";
                btnArchivedRepsBottom.Text = "Show Archived Reps";
                ViewState["ViewArchivedReps"] = false;


                DataTable dt = new DataTable();
                dt = ViewReps(false);

                if (dt != null)
                {
                    gvListOfSalesReps.DataSource = dt;
                    gvListOfSalesReps.DataBind();
                }
                Session["gvListOfSalesReps"] = dt;


            }
            else
            {
                btnArchivedRepsTop.Text = "Hide Archived Reps";
                btnArchivedRepsBottom.Text = "Hide Archived Reps";
                ViewState["ViewArchivedReps"] = true;

                DataTable dt = new DataTable();
                dt = ViewReps(true);

                if (dt != null)
                {
                    gvListOfSalesReps.DataSource = dt;
                    gvListOfSalesReps.DataBind();
                }
                Session["gvListOfSalesReps"] = dt;


            }

            //gvListOfSalesReps.DataSource = Session["gvListOfSalesReps"];
            //gvListOfSalesReps.DataBind();

        }

        protected void CreateNewSalesRepBtn_Click(object sender, EventArgs e)
        {
            pnlCreateNewSalesRep.Visible = true;
            pnlEditDetails.Visible = false;
            pnlGridView.Visible = false;

            if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
            {
                ddlNewCompany.SelectedValue = "Click Energy";
                ddlNewCompany.Enabled = false;
                ddlNewCompany.CssClass = "form-control";
            }
            else if (User.IsInRole(@"DOMAIN\Reps"))
            {
                ddlNewCompany.SelectedValue = "FSR";
                ddlNewCompany.Enabled = false;
                ddlNewCompany.CssClass = "form-control";
            }

        }

        protected void SaveChangesBtn_Click(object sender, EventArgs e)
        {
            using (GISEntities context = new GISEntities())
            {
                try
                {
                    int salesRepId = Convert.ToInt32(lblSalesRepId.Text); //Convert SalesRepId to int

                    SalesRep rep = context.SalesReps.Where(s => s.SalesRepId == salesRepId).FirstOrDefault();

                    rep.RepName = txtRepName.Text;
                    rep.EmailAddress = txtEmailAddress.Text;
                    rep.Company = ddlCompany.SelectedValue;
                    rep.Archived = Convert.ToBoolean(Convert.ToInt32(ddlArchived.SelectedValue));

                    context.SaveChanges();

                }
                catch (Exception Ex)
                {
                    Console.WriteLine("{0} Exception caught.", Ex);
                }
            }

            Response.Redirect("SalesRep.aspx");
        }

        protected void AddNewSalesRepBtn_Click(object sender, EventArgs e)
        {
            using (GISEntities context = new GISEntities())
            {
                try
                {
                    SalesRep rep = new SalesRep()
                    {
                        RepName = txtNewRepName.Text,
                        EmailAddress = txtNewEmailAddress.Text,
                        Company = ddlNewCompany.SelectedValue,
                        Archived = Convert.ToBoolean(Convert.ToInt32(ddlNewArchived.SelectedValue))
                    };

                    context.SalesReps.Add(rep);
                    context.SaveChanges();
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("{0} Exception caught.", Ex);
                }
            }

            Response.Redirect("SalesRep.aspx");
        }

        protected void btnNewCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesRep.aspx");
            /*pnlGridView.Visible = true;
            pnlEditDetails.Visible = false;
            pnlCreateNewSalesRep.Visible = false;*/
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesRep.aspx");
            /*pnlGridView.Visible = true;
            pnlEditDetails.Visible = false;
            pnlCreateNewSalesRep.Visible = false;*/
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