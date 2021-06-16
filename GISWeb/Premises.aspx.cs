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
    public partial class Premises : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                
            }
            else
            {
                pnlListOfPremises.Visible = false;
                pnlAllocateSalesRep.Visible = false;
                pnlPostCodeAlreadyAllocated.Visible = false;
                pnlPostcodesAllocatedToOtherGroup.Visible = false;               
            }

        }

        protected void btnPostCode_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);

            pnlAllocationSummary.Visible = false;
            pnlAllocateSalesRep.Visible = false;
            pnlPostCodeAlreadyAllocated.Visible = false;
            pnlPostcodesAllocatedToOtherGroup.Visible = false;

            lblAllocatedMessage.Text = String.Empty;

            List<SalesRep> res = new List<SalesRep>();

            try
            {
                using (GISEntities context = new GISEntities())
                {
                    using (PostcodesEntities contextPostcodes = new PostcodesEntities())
                    {
                        string postCode = txtPostCode.Text;
                        DataTable dt = new DataTable();

                        //All postcodeIds
                        var postalCodeId = contextPostcodes.PostalCodes.Where(s => s.FullPostcode.Contains(postCode)).Select(s => s.PostalCodeID).ToList();

                        //var excludeCommercials = context.Premises.Where(c => !c.DUoSGroup.StartsWith("T03")).Select(c => c.PostalCodeID).ToList();

                        //Click Energy sales reps
                        var salesRepsClickEnergy = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "Click Energy")).Select(s => s).ToList();

                        //FSR sales reps
                        var salesRepsFSR = context.SalesReps.Where(s => (s.Archived == false) && (s.Company == "FSR")).Select(s => s).ToList();

                        List<int> salesRepIdsFSR = salesRepsFSR.Select(r => r.SalesRepId).ToList(); //FSR SalesRepIds

                        List<int> salesRepIds = salesRepsClickEnergy.Select(r => r.SalesRepId).ToList(); //Click Energy SalesRepIds

                        //Areas allocated to click energy salesreps
                        var postalcodeIds = context.RepAreas.Where(s => (s.Archived == false) && salesRepIds.Contains(s.SalesRepId)).Select(s => s.PostalCodeID).ToList();

                        //Areas allocated to click energy salesreps
                        var postalcodeIdsFSR = context.RepAreas.Where(s => (s.Archived == false) && salesRepIdsFSR.Contains(s.SalesRepId)).Select(s => s.PostalCodeID).ToList();

                        var a = postalcodeIds.Intersect(postalCodeId);
                        var b = postalcodeIdsFSR.Intersect(postalCodeId);

                        if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                        {
                            res = salesRepsClickEnergy;

                            //Premises allocated to click energy 
                            dt = ConvertToDataTable(contextPostcodes.Premises.Where(
                            s => postalCodeId.Contains(s.PostalCodeID) && (!s.DUoSGroup.StartsWith("T03")) && (!s.DUoSGroup.StartsWith("T7")) &&
                            (!s.DUoSGroup.StartsWith("T1")) && (!s.DUoSGroup.StartsWith("T2")) && (!b.Contains(s.PostalCodeID))
                            ).Select(s => new
                            {
                                s.PremiseID,
                                s.MPRN,
                                MeterPointAddress = s.MeterPointAddress.TrimStart(),
                                s.DUoSGroup,
                                s.MeterConfigurationCode,
                                s.MeterPointStatus,
                                s.PostalCodeID,
                                s.Live,
                                s.Pending
                            }).OrderBy(s => s.MeterPointAddress).ThenBy(s => s.MeterPointStatus).ToList());
                        }
                        else if (User.IsInRole(@"DOMAIN\Reps"))
                        {
                            res = salesRepsFSR;

                            dt = ConvertToDataTable(contextPostcodes.Premises.Where(
                                s => postalCodeId.Contains(s.PostalCodeID) && (!s.DUoSGroup.StartsWith("T03")) && (!s.DUoSGroup.StartsWith("T7")) &&
                                (!s.DUoSGroup.StartsWith("T1")) && (!s.DUoSGroup.StartsWith("T2")) &&  (!a.Contains(s.PostalCodeID))
                                ).Select(s => new
                                {
                                    s.PremiseID,
                                    s.MPRN,
                                    MeterPointAddress = s.MeterPointAddress.TrimStart(),
                                    s.DUoSGroup,
                                    s.MeterConfigurationCode,
                                    s.MeterPointStatus,
                                    s.PostalCodeID,
                                    s.Live,
                                    s.Pending
                                }).OrderBy(s => s.MeterPointAddress).ThenBy(s => s.MeterPointStatus).ToList());
                        }
                        else
                        {                        
                            res = context.SalesReps.Where(s => (s.Archived == false)).Select(s => s).ToList();

                            dt = ConvertToDataTable(contextPostcodes.Premises.Where(
                                s => postalCodeId.Contains(s.PostalCodeID) && (!s.DUoSGroup.StartsWith("T03")) && (!s.DUoSGroup.StartsWith("T7")) &&
                                (!s.DUoSGroup.StartsWith("T1")) && (!s.DUoSGroup.StartsWith("T2"))
                                ).Select(s => new
                                {
                                    s.PremiseID,
                                    s.MPRN,
                                    MeterPointAddress = s.MeterPointAddress.TrimStart(),
                                    s.DUoSGroup,
                                    s.MeterConfigurationCode,
                                    s.MeterPointStatus,
                                    s.PostalCodeID,
                                    s.Live,
                                    s.Pending
                                }).OrderBy(s => s.MeterPointAddress).ThenBy(s => s.MeterPointStatus).ToList());
                        }
                                                                            

                        if (dt != null)
                        {
                            Session["gvListOfPremises"] = dt;
                            gvListOfPremises.DataSource = Session["gvListOfPremises"];
                            gvListOfPremises.DataBind();

                            var uniquePostalCodeIds = dt.AsEnumerable().Select(s => s.Field<int>("PostalCodeID")).Distinct().ToList();

                            searchResults.Text = dt.Rows.Count.ToString() + " premises found with " + uniquePostalCodeIds.Count.ToString() + " unique PostCodes.";                          

                            if (dt.Rows.Count != 0)
                            {
                                pnlListOfPremises.Visible = true;
                                pnlAllocationSummary.Visible = true;

                                DataTable allocatedPostcodes = new DataTable();

                                if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                                {
                                    //check if any PostCodes in search results are  already allocated
                                    allocatedPostcodes = ConvertToDataTable(context.RepAreas.Where(s => postalcodeIds.Contains(s.PostalCodeID) &&
                                        postalCodeId.Contains(s.PostalCodeID) &&
                                        s.Archived == false && s.EndDate > DateTime.Now).ToList());
                                }
                                else if (User.IsInRole(@"DOMAIN\Reps"))
                                {
                                    allocatedPostcodes = ConvertToDataTable(context.RepAreas.Where(s => postalcodeIdsFSR.Contains(s.PostalCodeID) &&
                                        postalCodeId.Contains(s.PostalCodeID) &&
                                        s.Archived == false && s.EndDate > DateTime.Now).ToList());
                                }
                                else
                                {
                                    allocatedPostcodes = ConvertToDataTable(context.RepAreas.Where(s => postalCodeId.Contains(s.PostalCodeID) &&
                                        s.Archived == false && s.EndDate > DateTime.Now).ToList());
                                }

                                allocatedPostcodes.Columns.Add("Postcode", typeof(string)); //add Postcode column
                                allocatedPostcodes.Columns.Add("Company", typeof(string)); //add Postcode column

                                //reorder datatable columns
                                DataView allocatedPostcodesDV = new DataView(allocatedPostcodes);
                                DataTable allocatedPostcodesDT = allocatedPostcodesDV.ToTable(false, "PostalCodeID", "SalesRepId", "RepName",
                                    "DateAdded", "StartDate", "EndDate", "Postcode", "Company");

                                //Rename datatable columns
                                allocatedPostcodesDT.Columns["SalesRepID"].ColumnName = "SalesRep ID";
                                allocatedPostcodesDT.Columns["RepName"].ColumnName = "Name";
                                allocatedPostcodesDT.Columns["DateAdded"].ColumnName = "Date Added";
                                allocatedPostcodesDT.Columns["StartDate"].ColumnName = "Start Date";
                                allocatedPostcodesDT.Columns["EndDate"].ColumnName = "End Date";

                                var uniqueAllocatedPostCodes = allocatedPostcodes.AsEnumerable().Select(s => s.Field<int>("PostalCodeID")).Distinct().ToList();


                                Session["allocatedPostcodesDT"] = allocatedPostcodesDT;
                                gvAllocationSummary.DataSource = Session["allocatedPostcodesDT"];
                                gvAllocationSummary.DataBind();

                                if (uniqueAllocatedPostCodes.Count != 0)
                                {
                                    if (uniqueAllocatedPostCodes.Count == 1)
                                    {
                                        lblAllocationSummary.Text = uniqueAllocatedPostCodes.Count.ToString() + " selected postcode is already allocated.";
                                    }
                                    else
                                    {
                                        lblAllocationSummary.Text = uniqueAllocatedPostCodes.Count.ToString() + " selected postcodes are already allocated.";
                                    }

                                    pnlPostCodeAlreadyAllocated.Visible = true;

                                    lblPostCodeAlreadyAllocated.Text = "Do you want to allocate all selected premises to a SalesRep?";

                                }
                                else
                                {
                                    pnlAllocateSalesRep.Visible = true;
                                }                               
                            }
                        }
                    }                  
                }
            }
            catch (Exception Ex)
            {
                 throw Ex;
            }

            try
            {
                /*DataTable dt = new DataTable();

                dt = ConvertToDataTable(context.PremisesByPostCode(txtPostCode.Text).ToList());

                if (dt != null)
                {
                    gvListOfPremises.DataSource = dt;
                    gvListOfPremises.DataBind();

                    Session["gvListOfPremises"] = dt;

                    searchResults.Text = dt.Rows.Count.ToString() + " premises found.";

                    pnlListOfPremises.Visible = true;
                    pnlAllocateSalesRep.Visible = true;
                }*/
                ListItem firstItem = ddlSalesReps.Items[0]; //Retrieve the first item in ddl "Please select an item"
                ddlSalesReps.Items.Clear();
                ddlSalesReps.Items.Add(firstItem);

                    

                ddlSalesReps.DataTextField = "RepName";
                ddlSalesReps.DataValueField = "SalesRepId";
                ddlSalesReps.DataSource = res;
                ddlSalesReps.DataBind();
            }
            catch(Exception Ex)
            {
                throw Ex;
            }

            System.Text.StringBuilder sbClose = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#myModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').remove();");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sbClose.ToString(), false);
        }

        protected void gvAllocationSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable allocatedPostcodesDT = (DataTable)Session["allocatedPostcodesDT"];

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;

                using (PostcodesEntities context = new PostcodesEntities()) //find FullPostCode
                {
                    string postalCodeId = e.Row.Cells[0].Text;

                    var res = context.PostalCodes.Where(s => s.PostalCodeID.ToString() == postalCodeId).Select(s => s).FirstOrDefault();

                    e.Row.Cells[6].Text = res.FullPostcode;
                    allocatedPostcodesDT.Rows[e.Row.RowIndex][6] = e.Row.Cells[6].Text;
                }

                using (GISEntities context = new GISEntities()) //find SalesRepId
                {
                    string salesRepId = e.Row.Cells[1].Text;

                    var res = context.SalesReps.Where(s => s.SalesRepId.ToString() == salesRepId).Select(s => s).FirstOrDefault();

                    e.Row.Cells[7].Text = res.Company;
                    allocatedPostcodesDT.Rows[e.Row.RowIndex][7] = e.Row.Cells[7].Text;
                }
            }

            Session["allocatedPostcodesDT"] = allocatedPostcodesDT;
        }

        protected void btnPostCodeAlreadyAllocatedYes_Click(object sender, EventArgs e)
        {

            string postCode = txtPostCode.Text;
            pnlPostCodeAlreadyAllocated.Visible = false;             
            pnlAllocateSalesRep.Visible = true;
            pnlPostcodesAllocatedToOtherGroup.Visible = false;

        }

        protected void btnPostCodeAlreadyAllocatedNo_Click(object sender, EventArgs e)
        {
            pnlPostCodeAlreadyAllocated.Visible = false;
            txtPostCode.Focus();
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

        protected void btnAllocateSalesRep_Click(object sender, EventArgs e)
        {
            pnlPostcodesAllocatedToOtherGroup.Visible = false;

            //Take SalesRepId and PostalCodeId and write new record to RepArea table.
            using (GISEntities context = new GISEntities())
            {
                try
                {                   
                    DataTable dt = (DataTable)Session["gvListOfPremises"];

                    var postalCodeId = dt.AsEnumerable().Select(r => r.Field<int>("PostalCodeID")).Distinct().ToList(); //List of PostalCodeIDs in search results

                    //find who allocated postcodes are allocated to
                    //find company of each salesRep
                    DataTable allocatedPostcodesDT = (DataTable)Session["allocatedPostcodesDT"];

                    int salesRepId = Convert.ToInt32(ddlSalesReps.SelectedItem.Value);
                    
                    foreach (var item in allocatedPostcodesDT.AsEnumerable()) //find if any selected postcodes are allocated to a different group
                    {
                        int Id = Convert.ToInt32(item.Field<int>("SalesRep ID"));

                        var salesRepAllocated = context.SalesReps.Where(s => s.SalesRepId == Id).FirstOrDefault(); 

                        var salesRep = context.SalesReps.Where(s => s.SalesRepId == salesRepId).FirstOrDefault();

                        if (User.IsInRole(@"DOMAIN\RepsClickGroup") || User.IsInRole(@"DOMAIN\Reps")) //if admin is in 'Click Energy group' or 'FSR group' do not allow allocation from one group to the other
                        {
                            if (!item.Field<string>("Company").Contains(salesRep.Company))
                            {
                                lblPostcodesAllocatedToOtherGroup.Text = "Postcode in selection allocated to " + salesRepAllocated.Company.ToString() + " group"; //display warning message
                                pnlPostcodesAllocatedToOtherGroup.Visible = true;
                                break;
                            }
                        }
                    }

                    if (pnlPostcodesAllocatedToOtherGroup.Visible == false) //if there's no warning message proceed with allocating postcode(s)
                    {
                        //Archive RepAreas that are allocated to area with postalCodeId
                        context.RepAreas.Where(s => postalCodeId.Contains(s.PostalCodeID) && s.Archived == false).ToList().ForEach(s => { s.Archived = true; });

                        foreach (var item in postalCodeId)
                        {
                            RepArea repArea = new RepArea() //Add new RepArea for allocated SalesRep
                            {
                                SalesRepId = Convert.ToInt32(ddlSalesReps.SelectedItem.Value),
                                RepName = ddlSalesReps.SelectedItem.Text,
                                PostalCodeID = Convert.ToInt32(item),
                                StartDate = Convert.ToDateTime(txtStartDate.Text),
                                EndDate = Convert.ToDateTime(txtEndDate.Text),
                                DateAdded = DateTime.Now,
                                Archived = false
                            };

                            context.RepAreas.Add(repArea);
                        }

                        context.SaveChanges();

                        lblAllocatedMessage.Text = "Postcodes allocated to " + ddlSalesReps.SelectedItem.Text;
                        //Response.Redirect("Premises.aspx");
                    }

                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }


        }

        protected void gvListOfPremises_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListOfPremises.PageIndex = e.NewPageIndex;

            gvListOfPremises.DataSource = Session["gvListOfPremises"];
            gvListOfPremises.DataBind();

        }

        protected void gvAllocationSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllocationSummary.PageIndex = e.NewPageIndex;

            gvAllocationSummary.DataSource = Session["allocatedPostcodesDT"];
            gvAllocationSummary.DataBind();

        }

        protected void gvListOfPremises_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }

    }
}