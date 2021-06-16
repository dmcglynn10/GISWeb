using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GISWeb
{
    public partial class notificationArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //PopulateGV();


            if (!Page.IsPostBack)
            {
                List<NotificationArea> notificationsList = new List<NotificationArea>();
                DataTable dt = new DataTable();
                using (GISEntities context = new GISEntities())
                {
                    if (User.IsInRole(@"DOMAIN\RepsClickGroup"))
                    {
                        notificationsList = context.NotificationAreas.Where(s => s.CompanyToDisplay == "Click Energy" && s.Archived == false).Select(s => s).ToList();
                        ListItem itemClick = new ListItem("Click Energy", "Click Energy");
                        ddlCompany.Items.Add(itemClick);
                    }
                    else if (User.IsInRole(@"DOMAIN\Reps"))
                    {
                        notificationsList = context.NotificationAreas.Where(s => s.CompanyToDisplay == "FSR" && s.Archived == false).Select(s => s).ToList();
                        ListItem itemFSR = new ListItem("FSR", "FSR");
                        ddlCompany.Items.Add(itemFSR);
                    }
                    else
                    {
                        notificationsList = context.NotificationAreas.Where(s => s.Archived == false).Select(s => s).ToList();
                        ListItem itemFSR = new ListItem("FSR", "FSR");
                        ListItem itemClick = new ListItem("Click Energy", "Click Energy");
                        ddlCompany.Items.Add(itemFSR);
                        ddlCompany.Items.Add(itemClick);
                    }
                }

            }
        }

        //private void PopulateGV()
        //{
        //    //gvNotificationArea

        //    using (GISEntities context = new GISEntities())
        //    {
        //        NotificationArea results[] = context.NotificationAreas.ToList();

        //        txtNotes.Text = results.Notes;
        //        ddlAuthor.SelectedValue = results.Author;
        //        txtStartDate.Text = results.StartDate.ToString();
        //        txtEndDate.Text = results.EndDate.ToString();

        //    }
        //}

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue.Trim();
            using (GISEntities context = new GISEntities())
            {
                NotificationArea results = context.NotificationAreaByCompany(company).FirstOrDefault();

                txtNotes.Text = results.Notes;
                ddlAuthor.SelectedValue = results.Author;
                txtStartDate.Text = results.StartDate.ToString();
                txtEndDate.Text = results.EndDate.ToString();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}