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
                    int? PremiseID = Int32.MinValue;
                    String MPRN = String.Empty;
                    String MeterPointAddress = String.Empty;
                    String DUoSGroup = String.Empty;
                    String MeterPointStatus = String.Empty;
                    String StartDate = String.Empty;
                    String EndDate = String.Empty;
                    String ActionStageCancelReason = String.Empty;
                    String SaleKeypad = String.Empty;
                    String SaleBillPay = String.Empty;
                    String DoNotContact = String.Empty;
                    DataTable dt = new DataTable();
                    //DataTable outcomes = new DataTable();

                    int salesrepid = Convert.ToInt32(ddlSalesReps.SelectedValue.ToString());
                    DateTime reportDate = DateTime.Now.Date;

                    using (GISEntities entities = new GISEntities())
                    {
                        System.Data.Entity.Core.Objects.ObjectResult<RepAreaPremisesAllocatedToSalesRepIdByDate_Result> result = entities.RepAreaPremisesAllocatedToSalesRepIdByDate(salesrepid, reportDate);
                        //var dt = result.ToList();
                        System.Data.Entity.Core.Objects.ObjectResult<RepAreaPremisesAllocatedToSalesRepIdByDate_Result> resultPremiseIds = entities.RepAreaPremisesAllocatedToSalesRepIdByDate(salesrepid, reportDate);

                        var premiseIds = resultPremiseIds.Select(s => s.PremiseID).ToList(); //list of premiseIds of allocated premises

                        var outcomes = entities.Outcomes.Where(s => premiseIds.Contains(s.PremiseId)).
                            GroupBy(x =>  x.PremiseId, (x, y) => new { Key = x, Value = y.OrderByDescending(z => z.ActionDateTime).FirstOrDefault() })
                            .Select(s => s.Value).ToList(); //most recent outcomes of allocated premises

                        var join = result.GroupJoin(outcomes, a => a.PremiseID, b => b.PremiseId,
                            (x, y) => new { result = x, outcomes = y }).SelectMany(
                            x => x.outcomes.DefaultIfEmpty(),
                            (x, y) => new { result = x.result, outcomes = y }).ToList(); //left join outcomes to repAreas

                        dt.Columns.Add("PremiseID", typeof(String));
                        dt.Columns.Add("MPRN", typeof(String));
                        dt.Columns.Add("MeterPointAddress", typeof(String));
                        dt.Columns.Add("DUoSGroup", typeof(String));
                        dt.Columns.Add("MeterPointStatus", typeof(String));
                        dt.Columns.Add("StartDate", typeof(String));
                        dt.Columns.Add("EndDate", typeof(String));
                        dt.Columns.Add("ActionStageCancelReason", typeof(String));
                        dt.Columns.Add("SaleKeypad", typeof(String));
                        dt.Columns.Add("SaleBillPay", typeof(String));
                        dt.Columns.Add("DoNotContact", typeof(String));

                        foreach (var item in join)
                        {
                            PremiseID = Int32.MinValue;
                            MPRN = String.Empty;
                            MeterPointAddress = String.Empty;
                            DUoSGroup = String.Empty;
                            MeterPointStatus = String.Empty;
                            StartDate = String.Empty;
                            EndDate = String.Empty;
                            ActionStageCancelReason = String.Empty;
                            SaleKeypad = String.Empty;
                            SaleBillPay = String.Empty;
                            DoNotContact = String.Empty;

                            if (item.result != null)
                            {
                                if (!String.IsNullOrEmpty(item.result.PremiseID.ToString())) { PremiseID = item.result.PremiseID; }
                                if (!String.IsNullOrEmpty(item.result.MPRN.ToString())) { MPRN = item.result.MPRN; }
                                if (!String.IsNullOrEmpty(item.result.MeterPointAddress.ToString())) { MeterPointAddress = item.result.MeterPointAddress; }
                                if (!String.IsNullOrEmpty(item.result.DUoSGroup.ToString())) { DUoSGroup = item.result.DUoSGroup; }
                                if (!String.IsNullOrEmpty(item.result.MeterPointStatus.ToString())) { MeterPointStatus = item.result.MeterPointStatus; }
                                if (!String.IsNullOrEmpty(item.result.StartDate.ToString())) { StartDate = Convert.ToDateTime(item.result.StartDate).Date.ToString("dd/MM/yyyy"); }
                                if (!String.IsNullOrEmpty(item.result.EndDate.ToString())) { EndDate = Convert.ToDateTime(item.result.EndDate).Date.ToString("dd/MM/yyyy"); }
                            }
                            if (item.outcomes != null)
                            {
                                if (!String.IsNullOrEmpty(item.outcomes.ActionStageCancelReason)) { ActionStageCancelReason = item.outcomes.ActionStageCancelReason; }
                                if (!String.IsNullOrEmpty(item.outcomes.SaleKeypad.ToString())) { SaleKeypad = item.outcomes.SaleKeypad.ToString(); }
                                if (!String.IsNullOrEmpty(item.outcomes.SaleBillPay.ToString())) { SaleBillPay = item.outcomes.SaleBillPay.ToString(); }
                                if (!String.IsNullOrEmpty(item.outcomes.DoNotContact.ToString())) { DoNotContact = item.outcomes.DoNotContact.ToString(); }
                            }

                            dt.Rows.Add(PremiseID, MPRN, MeterPointAddress, DUoSGroup, MeterPointStatus, 
                                StartDate, EndDate, ActionStageCancelReason, SaleKeypad,
                                SaleBillPay, DoNotContact);
                        }

                        /*dt.PrimaryKey = new DataColumn[]
                        {
                            dt.Columns[0]
                        };

                        dt.Merge(outcomes);*/

                        //Add new column to gridview 'mostRecentOutcome'
                        dt.Columns.Add("MostRecentOutcome", typeof(int));

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

        protected void gvAllocatedAreas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
            else if(e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;

                //32, 33, 34, 35
                if (!String.IsNullOrEmpty(e.Row.Cells[7].Text.Replace("&nbsp;",""))) //ActionStageCancelReason
                {
                    e.Row.Cells[11].Text = e.Row.Cells[7].Text;  
                }
                else if (e.Row.Cells[8].Text.Replace("&nbsp;", "") == "True") //SaleKeypad
                {
                    e.Row.Cells[11].Text = "SaleKeypad";
                }
                else if(e.Row.Cells[9].Text.Replace("&nbsp;", "") == "True") //SaleBillPay
                {
                    e.Row.Cells[11].Text = "SaleBillPay";
                }
                else if(e.Row.Cells[10].Text.Replace("&nbsp;", "") == "True") //DoNotContact
                {
                    e.Row.Cells[11].Text = "DoNotContact";
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