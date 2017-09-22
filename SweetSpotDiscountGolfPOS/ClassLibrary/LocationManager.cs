using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //Used to gather information about locations
    public class LocationManager
    {
        string connectionString;
        //Connection string
        public LocationManager()
        {
           connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Provinve/State Name based on Province/State ID
        public string provinceName(int provID)
        {
            string provStateName = null;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getProvinceName", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@provID", provID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                provStateName = row["provName"].ToString();
            }
            //Returns province/state name
            return provStateName;
        }
        //Province/State ID based on Province/State name
        public int pronvinceID(string provName)
        {
            int provStateID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getProvinceID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@provName", provName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                provStateID = Convert.ToInt32(row["provStateID"]);
            }
            //Returns province/state ID
            return provStateID;
        }
        //Country name based on country ID
        public string countryName(int countryID)
        {
            string countryName = null;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getCountryName", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@countryID", countryID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                countryName = row["countryDesc"].ToString();
            }
            //Returns country name
            return countryName;
        }
        //Country ID based on country name
        public int countryID(string countryName)
        {
            int countryID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getCountryID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@countryName", countryName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                countryID = Convert.ToInt32(row["countryID"]);
            }
            //Returns country ID
            return countryID;
        }
        //CountryID based on provinceID
        public int countryIDFromProvince(int provID)
        {
            int countryID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getCountryIDFromProvinceID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@provID", provID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                countryID = Convert.ToInt32(row["countryID"]);
            }
            //Returns country ID
            return countryID;
        }
        //Location name based on location ID
        public string locationName(int locationID)
        {
            string locationN = null;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getLocationName", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@locationID", locationID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                locationN = row["locationName"].ToString();
            }
            //Returns location name
            return locationN;
        }
        //Location ID based on location name
        public int locationID(string locationName)
        {
            int locID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getLocationID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@locationName", locationName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                locID = Convert.ToInt32(row["locationID"]);
            }
            //Returns location ID
            return locID;
        }
        //Location ID based on City
        public int locationIDfromCity(string locationName)
        {
            int locID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getLocationIDFromCity", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@cityName", locationName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                locID = Convert.ToInt32(row["locationID"]);
            }
            //REturns location ID
            return locID;
        }
        //Get location for invoice based on City
        public Location returnLocationForInvoice(string cityName)
        {
            Location locationN = null;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getLocationFromCity", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@cityName", cityName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                locationN = new Location(row["locationName"].ToString(), row["address"].ToString(),
                    row["city"].ToString(), Convert.ToInt32(row["provStateID"]),
                    row["postZip"].ToString(), row["PrimaryPhoneINT"].ToString());
            }
            //Returns object location
            return locationN;
        }
        //Gets city based on location id
        public string locationCity(int locID)
        {
            string cityName = "";
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getLocationName", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@locationID", locID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                cityName = row["city"].ToString();
            }
            //Returns city name
            return cityName;
        }
    }
}