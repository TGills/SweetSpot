using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {

        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        //List<Invoice> invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] == null)
            {
                lblReport.Text = "You are not authorized to view reports";
                lblReport.Visible = true;
                lblReport.ForeColor = System.Drawing.Color.Red;
                calStart.Visible = false;
                calEnd.Visible = false;
                btnRunReport.Visible = false;
                txtEndDate.Visible = false;
                txtStartDate.Visible = false;
                pnlDefaultButton.Visible = false;
            }
        }

        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            txtStartDate.Text = calStartDate.SelectedDate.ToShortDateString();
        }

        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            txtEndDate.Text = calEndDate.SelectedDate.ToShortDateString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text == "" || txtEndDate.Text == "")
            {
                lbldate.Visible = true;
                lbldate.Text = "Please Select a Start and End date";
                lbldate.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                DateTime[] reportDates = new DateTime[2];
                reportDates[0] = calStartDate.SelectedDate;
                reportDates[1] = calEndDate.SelectedDate;
                Session["reportDates"] = reportDates;

                //  if(txtStartDate.Text == txtEndDate.Text)
                //  {

                // //invoice = ssm.selectAllInventorySalesBetweenDates(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text).AddHours(23).AddMinutes(59));
                // Session["salesinvoice"] = invoice;

                //}
            }

            Response.Redirect("ReportsCashOut.aspx");
        }

        protected void btnExportInvoices_Click(object sender, EventArgs e)
        {
            r.exportAllSalesToExcel();
            MessageBox.ShowMessage("Report Completed. Check Downloads", this);
        }
    }
}