using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;


namespace GISWeb
{
    public partial class assignPostCodes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            bool runOnce = false;

            if (runOnce)
            {
                using (GISEntities context = new GISEntities())
                {
                    var postcodes = context.FSRLists.ToList();

                    foreach (FSRList item in postcodes)
                    {
                        List<GISWeb.Premis> DomesticPremises = new List<GISWeb.Premis>();
                        DomesticPremises = findPremisesByPostCode(item.PostCodeList);
                        DateTime DateAdded = DateTime.Now;

                        if (DomesticPremises.Count > 0)
                        {
                            foreach (Premis premise in DomesticPremises)
                            {
                                RepArea area = new RepArea();
                                area.RepName = "Click Energy Group";
                                area.SalesRepId = 37;
                                area.StartDate = new DateTime(2021, 1, 1);
                                area.EndDate = new DateTime(2022, 12, 31);
                                area.DateAdded = DateAdded;
                                area.Archived = false;
                                area.PostalCodeID = premise.PostalCodeID;
                                context.RepAreas.Add(area);
                                context.SaveChanges();
                            }
                        }
                        if (DateTime.Now <= DateAdded.AddSeconds(1))
                        {
                            System.Threading.Thread.Sleep(1000);  //wait 1 second
                        }
                    }

                }

                lblResults.Text = "Complete";
            }

        }

        private List<GISWeb.Premis> findPremisesByPostCode(string postCodeList)
        {

            List<GISWeb.Premis> UniqueDomesticPremises = new List<GISWeb.Premis>();

            using (PostcodesEntities postcodeContext = new PostcodesEntities())
            {

                List<int> postalCodeIds = new List<int>();

                postalCodeIds = postcodeContext.PostalCodes.Where(s => s.FullPostcode.Contains(postCodeList)).Select(s => s.PostalCodeID).ToList();

                List<GISWeb.Premis> DomesticPremises = new List<GISWeb.Premis>();
                DomesticPremises = postcodeContext.Premises.Where(s => postalCodeIds.Contains(s.PostalCodeID)
                            && (s.DUoSGroup.StartsWith("T01") || s.DUoSGroup.StartsWith("T05"))).ToList();


                List<int> UniquePostCodeIds = new List<int>();


                UniqueDomesticPremises = DomesticPremises.GroupBy(x => x.PostalCodeID).Select(y => y.First()).ToList();

            }

            return UniqueDomesticPremises;

        }
    }
}