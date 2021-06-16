using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GISWeb
{
    public partial class PremisesKPBP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                gvPremisesKPBPResults.DataSource = ViewState["dt"];
                gvPremisesKPBPResults.DataBind();
            }
            else
            {

            }
        }

        protected void btnPostCode_Click(object sender, EventArgs e)
        {
            pnlPremisesKPBPResults.Visible = true;

            string pcentKeypad = String.Empty;
            string pcentBillpay = String.Empty;
            Decimal totalPrems = Decimal.Zero;
            Decimal keypadPrems = Decimal.Zero;
            Decimal billpayPrems = Decimal.Zero;

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Premise Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Count (% Count)", typeof(string)));

            using (PostcodesEntities context = new PostcodesEntities())
            {
                try
                {
                    totalPrems = (decimal)context.CountTotalPremisesByPostcode(txtPostCode.Text).FirstOrDefault();
                    keypadPrems = (decimal)context.CountKeypadPremisesByPostcode(txtPostCode.Text).FirstOrDefault();
                    billpayPrems = totalPrems - keypadPrems;

                    //calculate percentages
                    if (totalPrems != 0)
                    {
                        pcentKeypad = ((keypadPrems / totalPrems) * 100).ToString("0.#");
                        pcentBillpay = ((billpayPrems / totalPrems) * 100).ToString("0.#");
                    }
                    else
                    {
                        pcentKeypad = 0.ToString("0.#");
                        pcentBillpay = 0.ToString("0.#");
                    }

                    //% count
                    var premiseCounts = new Dictionary<string, string>
                        {
                            {"Keypad", keypadPrems.ToString() + " (" + pcentKeypad + "%)" },
                            {"Billpay", billpayPrems.ToString() + " (" + pcentBillpay + "%)" },
                            {"Total", totalPrems.ToString() },

                        };

                    //Save results to datatable
                    foreach (var item in premiseCounts)
                    {
                        DataRow dr = dt.NewRow();

                        dr[dt.Columns[0].ColumnName] = item.Key;
                        dr[dt.Columns[1].ColumnName] = item.Value;

                        dt.Rows.Add(dr);
                    }

                    //Bind datatable to gridview;
                    ViewState["dt"] = dt;
                    gvPremisesKPBPResults.DataSource = ViewState["dt"];
                    gvPremisesKPBPResults.DataBind();
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }

        }

        protected void gvPremisesKPBPResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        
    }
}