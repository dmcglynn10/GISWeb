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
    public partial class premiseHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                int premiseId = 0;
                bool result = int.TryParse(Request.QueryString["PremiseId"], out premiseId); //i now = 108  

                if (result)
                {
                    txtPremiseId.Text = premiseId.ToString();
                    FindOutcomesForPremise(premiseId);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int premiseId = 0;
            bool result = int.TryParse(txtPremiseId.Text, out premiseId); //i now = 108  

            if (result)
            {
                FindOutcomesForPremise(premiseId);                
            }
        }

        private void FindOutcomesForPremise(int PremiseId)
        {

            using (GISEntities context = new GISEntities())
            {
                try
                {
                    var res = context.Outcomes.Where(o => (o.Archived == false) && (o.PremiseId == PremiseId)).OrderByDescending(s => s.ActionDateTime).Select(s => s).ToList();
                    DataTable dt1 = new DataTable();

                    dt1 = ConvertToDataTable(res);
                    gvPremiseHistory.DataSource = dt1;
                    gvPremiseHistory.DataBind();

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

        protected void gvPremiseHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; //SalesRepId Header row not visible
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false; //SalesRepId DataRow row not visible

                using (GISEntities context = new GISEntities()) //find Rep's Name
                {
                    string salesRepId = e.Row.Cells[0].Text;

                    var res = context.SalesReps.Where(s => s.SalesRepId.ToString() == salesRepId).Select(s => s.RepName).FirstOrDefault();

                    e.Row.Cells[4].Text = res;
                }
            }
        }
    }
}