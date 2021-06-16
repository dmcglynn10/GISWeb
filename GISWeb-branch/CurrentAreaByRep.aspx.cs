using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GISWeb
{
    public partial class CurrentAreaByRep : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page.IsPostBack))
            {
                using (GISEntities context = new GISEntities())
                {
                    List<SalesRep> res = new List<SalesRep>();

                    if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                    {                        
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).OrderBy(s => s.RepName).ToList();
                    }
                    else if (User.IsInRole(@"DOMAIN\Reps"))
                    {
                       // res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).ToList();
                        res = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).OrderBy(s => s.RepName).ToList();
                    }
                    else
                    {
                        //res = context.SalesReps.Where(s => s.Archived == false).Select(s => s).ToList();
                        res = context.SalesReps.Where(s => s.Archived == false).Select(s => s).OrderBy(s => s.RepName).ToList();
                    }

                    ddlSalesReps.DataTextField = "RepName";
                    ddlSalesReps.DataValueField = "SalesRepId";
                    ddlSalesReps.DataSource = res;
                    ddlSalesReps.DataBind();
                }
            }
            else
            {
                BindGridView();
            }
        }

        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    BindGridView();
        //}

        public void BindGridView()
        {
            using (PostcodesEntities context = new PostcodesEntities())
            {
                try
                {
                    DataTable dt = new DataTable();

                    int salesrepid = Convert.ToInt32(ddlSalesReps.SelectedValue.ToString());
                    DateTime reportDate = DateTime.Now.Date;

                    using (GISEntities entities = new GISEntities())
                    {
                        System.Data.Entity.Core.Objects.ObjectResult<RepAreaPremisesAllocatedToSalesRepIdByDate_Result> result = entities.RepAreaPremisesAllocatedToSalesRepIdByDate(salesrepid, reportDate);

                        dt = ConvertToDataTable(result.ToList());
                        if (dt != null)
                        {
                            gvAllocatedAreas.DataSource = dt;
                            gvAllocatedAreas.DataBind();
                        }
                        lblAllocatedPremises.Text = dt.Rows.Count.ToString() + " allocated premises found for " + ddlSalesReps.SelectedItem.Text;
                    }
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