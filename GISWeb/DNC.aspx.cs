using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ComponentModel;
using System.Data;

namespace GISWeb
{
    public partial class DNC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page.IsPostBack))
            {
                using (GISEntities context = new GISEntities())
                {
                    DataTable dt = new DataTable();
                    System.Data.Entity.Core.Objects.ObjectResult<OutcomeDNC_Result> result = context.OutcomeDNC();
                    dt = ConvertToDataTable(result.ToList());
                    if (dt != null)
                    {
                        gvDNC.DataSource = dt;
                        gvDNC.DataBind();
                    }

                    lblDNC.Text = dt.Rows.Count.ToString() + " results found.";

                    RegisterPostBackControl();
                    //lblAllocatedPremises.Text = dt.Rows.Count.ToString() + " allocated premises found for " + ddlSalesReps.SelectedItem.Text;
                }
            }
            else
            {               
                //  BindGridView();
            }

        }

        protected void gvDNC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            using (GISEntities context = new GISEntities())
            {
                DataTable dt = new DataTable();

                try
                {
                    int outcomeId = Convert.ToInt32(e.CommandArgument);//Convert OutcomeId to int

                    Outcome outcome = context.Outcomes.Where(s => s.OutcomeId == outcomeId).FirstOrDefault();

                    outcome.Archived = true;
                    outcome.ArchivedDateTime = DateTime.Now;                   

                    context.SaveChanges();

                    System.Data.Entity.Core.Objects.ObjectResult<OutcomeDNC_Result> result = context.OutcomeDNC(); //Refresh gridview
                    dt = ConvertToDataTable(result.ToList());
                    if (dt != null)
                    {
                        gvDNC.DataSource = dt;
                        gvDNC.DataBind();
                    }

                    RegisterPostBackControl();

                    lblDNCArchivedMessage.Text = "Outcome with ID " + outcomeId.ToString() + " archived";

                }
                catch (Exception Ex)
                {
                    Console.WriteLine("{0} Exception caught.", Ex);
                }
            }
        }

        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in gvDNC.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Button btn = row.FindControlRecursive("btnArchive") as Button;
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
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

        protected void gvDNC_RowEditing1(object sender, GridViewEditEventArgs e)
        {

        }
    }
}