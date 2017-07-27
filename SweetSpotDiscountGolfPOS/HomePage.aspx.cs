using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class HomePage : System.Web.UI.Page
    {
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        List<Invoice> invoiceList = new List<Invoice>();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("SELECT city FROM tbl_location ", con);

                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                ddlLocation.DataSource = cmd.ExecuteReader();
                ddlLocation.DataTextField = "City";
                ddlLocation.DataBind();
                ddlLocation.SelectedValue = Convert.ToString(Session["Loc"]);
                con.Close();

            }
            if (Session["Admin"] != null)
            {
                lbluser.Text = "You have Admin Access";
                lbluser.Visible = true;
            }
            else/* if (Session["Loc"] != null)*/
            {

                lblLocation.Text = Convert.ToString(Session["Loc"]);
                lblLocation.Visible = true;
                ddlLocation.Visible = false;
            }
            //populate gridview with todays sales
            invoiceList = ssm.getInvoiceBySaleDate(DateTime.Today, lm.locationIDfromCity(Convert.ToString(Session["Loc"])));
            grdSameDaySales.DataSource = invoiceList;
            grdSameDaySales.DataBind();
        }
    }
}