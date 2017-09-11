using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //This class is for getting the employee ID from the userinfo
    public class tbl_userinfomanager
    {
        private string connectionstring;
        //Connection String
        public tbl_userinfomanager()
        {
            connectionstring = "SweetSpotSBConnectionString";
        }

        //This method gets the employee ID from a password
        public int getuserinfologin(int password)
        {
            int empid = 0;
            // List<CurrentUser> empid = new List<CurrentUser>();
            SqlConnection con = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT empID from tbl_userInfo where password = @password";
            cmd.Parameters.AddWithValue("password", password);
            con.Open();
            SqlDataReader read = cmd.ExecuteReader();
            //get userinfo from CurrentUser
            List<tbl_userinfo> cus = new List<tbl_userinfo>();
            while (read.Read())
            {
                tbl_userinfo cu = new tbl_userinfo(Convert.ToInt32(read["empID"]),
                                          (Convert.ToInt32(read["password"])));

                //  int empid = Int32.Parse(Convert.ToInt32(read["empID"]));
                empid = Convert.ToInt32(read["empID"]);
                cus.Add(cu);
            }
            con.Close();
            //Returns employee ID
            return empid;
        }

    }
}
