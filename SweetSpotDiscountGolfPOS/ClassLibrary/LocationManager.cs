using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class LocationManager
    {
        string connectionString;
        public LocationManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Provinve/State Name based on Province/State ID
        public string provinceName(int provID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select provName from tbl_provState where provStateID = " + provID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string provStateName = null;
            while (reader.Read())
            {
                string name = reader["provName"].ToString();
                provStateName = name;
            }
            conn.Close();
            return provStateName;
        }
        //Province/State ID based on Province/State name
        public int pronvinceID(string provName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select provStateID from tbl_provState where provName = '" + provName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int provStateID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["provStateID"]);
                provStateID = n;
            }
            conn.Close();
            return provStateID;
        }
        //Country name based on country ID
        public string countryName(int countryID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select countryDesc from tbl_country where countryID = " + countryID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string countryName = null;
            while (reader.Read())
            {
                string name = reader["countryDesc"].ToString();
                countryName = name;
            }
            conn.Close();
            return countryName;
        }
        //Country ID based on country name
        public int countryID(string countryName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select countryID from tbl_country where countryDesc = '" + countryName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int countryID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["countryID"]);
                countryID = n;
            }
            conn.Close();
            return countryID;
        }
        //Location name based on location ID
        public string locationName(int locationID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationName from tbl_location where locationID = " + locationID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string locationN = null;
            while (reader.Read())
            {
                string name = reader["locationName"].ToString();
                locationN = name;
            }
            conn.Close();
            return locationN;
        }
        //Location ID based on location name
        public int locationID(string locationName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationID from tbl_location where locationName = '" + locationName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int locID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["locationID"]);
                locID = n;
            }
            conn.Close();
            return locID;
        }
        public int locationIDfromCity(string locationName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationID from tbl_location where city = '" + locationName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int locID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["locationID"]);
                locID = n;
            }
            conn.Close();
            return locID;
        }
        public Location returnLocationForInvoice(string cityName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select locationName, address, city, provStateID, postZip, PrimaryPhoneINT from tbl_location where city = '" + cityName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Location locationN = null;
            while (reader.Read())
            {
                locationN = new Location(reader["locationName"].ToString(), reader["address"].ToString(),
                    reader["city"].ToString(), Convert.ToInt32(reader["provStateID"]),
                    reader["postZip"].ToString(), reader["PrimaryPhoneINT"].ToString());
            }
            conn.Close();
            return locationN;
        }

    }
}