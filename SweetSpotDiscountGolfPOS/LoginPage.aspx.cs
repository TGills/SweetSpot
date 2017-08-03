using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }
        //test
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT empID, password FROM tbl_userInfo WHERE password = @password", con);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();           

            //checking for admin and sales staff


            SqlCommand adm = new SqlCommand("SELECT  tbl_userInfo.password,tbl_employee.jobID,tbl_employee.empID FROM tbl_userInfo JOIN tbl_employee ON tbl_employee.empID = tbl_userInfo.empID AND tbl_userInfo.password = @pwd", con);
            adm.Parameters.AddWithValue("@pwd", Convert.ToInt32(txtPassword.Text));
            SqlDataAdapter admA = new SqlDataAdapter(adm);
            DataTable dt1 = new DataTable();
            admA.Fill(dt1);
            con.Open();
            int job = adm.ExecuteNonQuery();
            con.Close();
            if (dt1.Rows.Count > 0)
            {
                DataRow admDr = dt1.Rows[0];

                if (Convert.ToInt32(admDr.ItemArray[1]) == 0)
                {
                    Session["Admin"] = "Admin";
                }

                //else
                //{
                SqlCommand loc = new SqlCommand("SELECT  tbl_location.locationID,tbl_location.city FROM tbl_location JOIN tbl_employee ON tbl_location.locationID = tbl_employee.locationID AND tbl_employee.empID = @emp", con);
                loc.Parameters.AddWithValue("@emp", admDr.ItemArray[2]);
                SqlDataAdapter locA = new SqlDataAdapter(loc);
                DataTable dt2 = new DataTable();
                locA.Fill(dt2);
                con.Open();
                loc.ExecuteNonQuery();
                con.Close();
                int cnt = dt2.Rows.Count;

                DataRow locDR = dt2.Rows[0];
                Session["Loc"] = locDR.ItemArray[1];
                //}
            }
            if (dt.Rows.Count > 0)
            {
                Session["loggedIn"] = true;
                Session["id"] = txtPassword.Text;
                Response.Redirect("HomePage.aspx");
                Session.RemoveAll();


            }
            else
            {                
                lblError.Text = "Your password is incorrect";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}